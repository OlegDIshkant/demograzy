using Demograzy.BusinessLogic.DataAccess;
using Demograzy.Core.Test.CommonRoutines;
using static Demograzy.Core.Test.GeneralConstants;

namespace Demograzy.Core.Test.Versus.Vote.Success
{
    
    [TestFixture]
    public class CompletingSomeVersus
    {

        [TestCase(true)]
        [TestCase(false)]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenCompleteFirstVersusThenActiveVersesListsHasCertainLength (bool candidateAmountIsEven)
        {     
            var service = StartUpRoutines.PrepareMainService();
            // Add room
            var (room, owner) = await service.NewRoomAndOwner();
            // Add extra room members
            var extraMember1 = await service.NewExtraMember(room);
            var extraMember2 = await service.NewExtraMember(room);
            var extraMember3 = await service.NewExtraMember(room);
            // Add candidates
            var candidatesAmount = candidateAmountIsEven ? 6 : 11;
            var candidates = Enumerable.Range(0, candidatesAmount)
            .Select(async i => await service.AddCandidateAsync(room, $"candidate_{i}"))
            .ToList();
            // Start voting
            Assert.That(await service.StartVotingAsync(room));
            // Get first stage verses
            var firstStageVerses = await service.GetActiveVersesAsync(room, owner);


            // Complete fist versus
            var versus1 = firstStageVerses[0];
            Assert.That(await service.VoteAsync(versus1, extraMember1, votedForFirst: true));
            Assert.That(await service.VoteAsync(versus1, extraMember2, votedForFirst: true));
            Assert.That(await service.VoteAsync(versus1, owner, votedForFirst: true));


            var expectedLength = (int)(candidatesAmount / 2) - (candidateAmountIsEven ? 1 : 0);
            Assert.That(await service.GetActiveVersesAsync(room, extraMember1), Has.Count.EqualTo(expectedLength));
            Assert.That(await service.GetActiveVersesAsync(room, extraMember2), Has.Count.EqualTo(expectedLength));
            Assert.That(await service.GetActiveVersesAsync(room, extraMember3), Has.Count.EqualTo(expectedLength));
        }



        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenHasSixCandidatesAndCompleteTwoVersesThenTheirWinnersGoToNewVersus()
        {     
            var service = StartUpRoutines.PrepareMainService();
            // Add room
            var (room, owner) = await service.NewRoomAndOwner();
            // Add extra room members
            var extraMember1 = await service.NewExtraMember(room);
            var extraMember2 = await service.NewExtraMember(room);
            var extraMember3 = await service.NewExtraMember(room);
            // Add candidates
            const int candidatesAmount = 6;
            var candidates = Enumerable.Range(0, candidatesAmount)
            .Select(async i => await service.AddCandidateAsync(room, $"candidate_{i}"))
            .ToList();
            // Start voting
            Assert.That(await service.StartVotingAsync(room));
            // Get first stage verses
            var firstStageVerses = await service.GetActiveVersesAsync(room, owner);


            // Complete fist versus
            var versus1 = firstStageVerses[0];
            var firstWinner = (await service.GetVersusInfoAsync(versus1)).firstCandidateId;
            Assert.That(await service.VoteAsync(versus1, extraMember1, votedForFirst: true));
            Assert.That(await service.VoteAsync(versus1, extraMember2, votedForFirst: true));
            Assert.That(await service.VoteAsync(versus1, owner, votedForFirst: true));
            // Complete second versus
            var versus2 = firstStageVerses[1];
            var secondWinner = (await service.GetVersusInfoAsync(versus1)).secondCandidateId;
            Assert.That(await service.VoteAsync(versus2, extraMember1, votedForFirst: false));
            Assert.That(await service.VoteAsync(versus2, extraMember2, votedForFirst: false));
            Assert.That(await service.VoteAsync(versus2, extraMember3, votedForFirst: false));


            var newVersus = (await service.GetActiveVersesAsync(room, owner)).ToHashSet().Except(firstStageVerses.ToHashSet()).Single();
            var newVersusInfo = await service.GetVersusInfoAsync(newVersus);
            Assert.That(newVersusInfo.firstCandidateId, Is.EqualTo(firstWinner));
            Assert.That(newVersusInfo.secondCandidateId, Is.EqualTo(secondWinner));
        }



        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenOneOfManyMembersVotesThenVersusNoMoreActiveForHim()
        {     
            var service = StartUpRoutines.PrepareMainService();
            // Add room
            var (room, owner) = await service.NewRoomAndOwner();
            // Add extra room members
            var extraMember1 = await service.NewExtraMember(room);
            var extraMember2 = await service.NewExtraMember(room);
            var extraMember3 = await service.NewExtraMember(room);
            // Add candidates
            const int candidatesAmount = 6;
            var candidates = Enumerable.Range(0, candidatesAmount)
            .Select(async i => await service.AddCandidateAsync(room, $"candidate_{i}"))
            .ToList();
            // Start voting
            Assert.That(await service.StartVotingAsync(room));
            // Get first stage verses
            var versus = (await service.GetActiveVersesAsync(room, extraMember1)).First();


            // Member votes
            Assert.That(await service.VoteAsync(versus, extraMember1, votedForFirst: true));


            Assert.That(await service.GetActiveVersesAsync(room, extraMember1), Has.No.Member(versus));
        }



        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenOneOfManyMembersVotesThenVersusStillActiveForOthers()
        {     
            var service = StartUpRoutines.PrepareMainService();
            // Add room
            var (room, owner) = await service.NewRoomAndOwner();
            // Add extra room members
            var extraMember1 = await service.NewExtraMember(room);
            var extraMember2 = await service.NewExtraMember(room);
            var extraMember3 = await service.NewExtraMember(room);
            // Add candidates
            const int candidatesAmount = 6;
            var candidates = Enumerable.Range(0, candidatesAmount)
            .Select(async i => await service.AddCandidateAsync(room, $"candidate_{i}"))
            .ToList();
            // Start voting
            Assert.That(await service.StartVotingAsync(room));
            // Get first stage verses
            var versus = (await service.GetActiveVersesAsync(room, extraMember1)).First();


            // Member votes
            Assert.That(await service.VoteAsync(versus, extraMember1, votedForFirst: true));


            Assert.That(await service.GetActiveVersesAsync(room, extraMember2), Has.Member(versus));
            Assert.That(await service.GetActiveVersesAsync(room, extraMember3), Has.Member(versus));
            Assert.That(await service.GetActiveVersesAsync(room, owner), Has.Member(versus));
        }



        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenMostMembersVotesTheSameWayThenVersusNoLongerActive()
        {     
            var service = StartUpRoutines.PrepareMainService();
            // Add room
            var (room, owner) = await service.NewRoomAndOwner();
            // Add extra room members
            var extraMember1 = await service.NewExtraMember(room);
            var extraMember2 = await service.NewExtraMember(room);
            var extraMember3 = await service.NewExtraMember(room);
            var extraMember4 = await service.NewExtraMember(room);
            // Add candidates
            const int candidatesAmount = 6;
            var candidates = Enumerable.Range(0, candidatesAmount)
            .Select(async i => await service.AddCandidateAsync(room, $"candidate_{i}"))
            .ToList();
            // Start voting
            Assert.That(await service.StartVotingAsync(room));
            // Get first stage verses
            var versus = (await service.GetActiveVersesAsync(room, extraMember1)).First();


            // Members vote
            Assert.That(await service.VoteAsync(versus, extraMember1, votedForFirst: false));
            Assert.That(await service.VoteAsync(versus, extraMember2, votedForFirst: false));
            Assert.That(await service.VoteAsync(versus, extraMember3, votedForFirst: false));


            Assert.That(await service.GetActiveVersesAsync(room, extraMember1), Has.No.Member(versus));
            Assert.That(await service.GetActiveVersesAsync(room, extraMember2), Has.No.Member(versus));
            Assert.That(await service.GetActiveVersesAsync(room, extraMember3), Has.No.Member(versus));
            Assert.That(await service.GetActiveVersesAsync(room, extraMember4), Has.No.Member(versus));
            Assert.That(await service.GetActiveVersesAsync(room, owner), Has.Member(versus));
        }



        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenMostMembersVotesForSecondCandidateThenItBecomesVersusWinner()
        {     
            var service = StartUpRoutines.PrepareMainService();
            // Add room
            var (room, owner) = await service.NewRoomAndOwner();
            // Add extra room members
            var extraMember1 = await service.NewExtraMember(room);
            var extraMember2 = await service.NewExtraMember(room);
            var extraMember3 = await service.NewExtraMember(room);
            var extraMember4 = await service.NewExtraMember(room);
            // Add candidates
            const int candidatesAmount = 6;
            var candidates = Enumerable.Range(0, candidatesAmount)
            .Select(async i => await service.AddCandidateAsync(room, $"candidate_{i}"))
            .ToList();
            // Start voting
            Assert.That(await service.StartVotingAsync(room));
            // Get first stage verses
            var versus = (await service.GetActiveVersesAsync(room, extraMember1)).First();


            // Members vote
            Assert.That(await service.VoteAsync(versus, extraMember1, votedForFirst: false));
            Assert.That(await service.VoteAsync(versus, extraMember2, votedForFirst: false));
            Assert.That(await service.VoteAsync(versus, extraMember3, votedForFirst: false));


            Assert.That((await service.GetVersusInfoAsync(versus)).status, Is.EqualTo(VersusInfo.Statuses.SECOND_WON));
        }

    }
}