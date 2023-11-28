using Demograzy.Core.Test.CommonRoutines;
using static Demograzy.Core.Test.GeneralConstants;

namespace Demograzy.Core.Test.Versus.Vote.Success
{
    
    [TestFixture]
    public class CompletingWholeVoting
    {

        [TestCase(true)]
        [TestCase(false)]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenCompleteCertainAmountOfVersesThenWinnerIsWinnerOfLastVersus (bool voteForFirstInLastVersus)
        {     
            var service = StartUpRoutines.PrepareMainService();
            // Add room
            var (room, owner) = await service.NewRoomAndOwner();
            // Add extra room members
            var extraMember1 = await service.NewExtraMember(room);
            var extraMember2 = await service.NewExtraMember(room);
            var extraMember3 = await service.NewExtraMember(room);
            // Add candidates
            var candidate1 = (await service.AddCandidateAsync(room, "candidate")).Value;
            var candidate2 = (await service.AddCandidateAsync(room, "candidate")).Value;
            var candidate3 = (await service.AddCandidateAsync(room, "candidate")).Value;
            var candidate4 = (await service.AddCandidateAsync(room, "candidate")).Value;
            var candidate5 = (await service.AddCandidateAsync(room, "candidate")).Value;
            var candidate6 = (await service.AddCandidateAsync(room, "candidate")).Value;
            var candidate7 = (await service.AddCandidateAsync(room, "candidate")).Value;
            var candidate8 = (await service.AddCandidateAsync(room, "candidate")).Value;
            // Start voting
            Assert.That(await service.StartVotingAsync(room));
            // Get first stage verses
            var firstStageVerses = await service.GetActiveVersesAsync(room, owner);
            // Complete fist versus
            var versus1 = firstStageVerses.ElementAt(0);
            Assert.That(await service.VoteAsync(versus1, extraMember1, votedForFirst: true));
            Assert.That(await service.VoteAsync(versus1, extraMember2, votedForFirst: true));
            Assert.That(await service.VoteAsync(versus1, owner, votedForFirst: true));
            // Complete second versus
            var versus2 = firstStageVerses.ElementAt(1);
            Assert.That(await service.VoteAsync(versus2, extraMember1, votedForFirst: false));
            Assert.That(await service.VoteAsync(versus2, extraMember2, votedForFirst: false));
            Assert.That(await service.VoteAsync(versus2, owner, votedForFirst: false));
            // Complete third versus 
            var versus3 = firstStageVerses.ElementAt(2);
            Assert.That(await service.VoteAsync(versus3, extraMember1, votedForFirst: false));
            Assert.That(await service.VoteAsync(versus3, extraMember3, votedForFirst: false));
            Assert.That(await service.VoteAsync(versus3, owner, votedForFirst: false));
            // Complete forth versus 
            var versus4 = firstStageVerses.ElementAt(3);
            Assert.That(await service.VoteAsync(versus4, extraMember1, votedForFirst: true));
            Assert.That(await service.VoteAsync(versus4, extraMember3, votedForFirst: true));
            Assert.That(await service.VoteAsync(versus4, extraMember2, votedForFirst: true));
            // Get second stage verses
            var secondStageVerses = await service.GetActiveVersesAsync(room, owner);
            // Complete fifth versus
            var versus5 = secondStageVerses.ElementAt(0);
            Assert.That(await service.VoteAsync(versus5, extraMember3, votedForFirst: false));
            Assert.That(await service.VoteAsync(versus5, extraMember2, votedForFirst: false));
            Assert.That(await service.VoteAsync(versus5, owner, votedForFirst: false));
            // Complete sixth versus
            var versus6 = secondStageVerses.ElementAt(1);
            Assert.That(await service.VoteAsync(versus6, extraMember1, votedForFirst: false));
            Assert.That(await service.VoteAsync(versus6, extraMember2, votedForFirst: false));
            Assert.That(await service.VoteAsync(versus6, extraMember3, votedForFirst: false));
            // Get last versus
            var lastVersus = (await service.GetActiveVersesAsync(room, owner)).ElementAt(0);


            // Complete last versus
            Assert.That(await service.VoteAsync(lastVersus, extraMember3, voteForFirstInLastVersus));
            Assert.That(await service.VoteAsync(lastVersus, extraMember2, voteForFirstInLastVersus));
            Assert.That(await service.VoteAsync(lastVersus, owner, voteForFirstInLastVersus));


            var expectedWinner = voteForFirstInLastVersus ?
                (await service.GetVersusInfoAsync(lastVersus)).Value.firstCandidateId :
                (await service.GetVersusInfoAsync(lastVersus)).Value.secondCandidateId;
            var actualWinner = (await service.GetWinnerAsync(room)).Value;
            Assert.That(actualWinner, Is.EqualTo(expectedWinner));
        }

    }
}