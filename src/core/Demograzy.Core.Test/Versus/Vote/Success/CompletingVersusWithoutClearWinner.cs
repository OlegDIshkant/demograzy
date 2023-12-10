using Demograzy.Core.Test.CommonRoutines;
using static Demograzy.Core.Test.GeneralConstants;
using static Demograzy.BusinessLogic.DataAccess.VersusInfo;

namespace Demograzy.Core.Test.Versus.Vote.Success
{
    
    [TestFixture]
    public class CompletingVersusWithoutClearWinner
    {

        [TestCase(true)]
        [TestCase(false)]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenVotersVoteEvenlyThenWinnerEntryInVersusInfoDefinedByTheLastVote(bool lastVoteForFirst)
        {     
            var service = StartUpRoutines.PrepareMainService();
            // Add room
            var (room, owner) = await service.NewRoomAndOwner();
            // Add extra room members
            var extraMember1 = await service.NewExtraMember(room);
            var extraMember2 = await service.NewExtraMember(room);
            var extraMember3 = await service.NewExtraMember(room);
            // Add candidates
            var candidate1 = (await service.AddCandidateAsync(room, "candidate1")).Value;
            var candidate2 = (await service.AddCandidateAsync(room, "candidate2")).Value;
            // Start voting
            Assert.That(await service.StartVotingAsync(room));
            // Get versus
            var versus = (await service.GetActiveVersesAsync(room, owner)).Single();

            // Complete versus!
            // Votes should be distributed equally between candidates. 
            if (lastVoteForFirst)
            {
                Assert.That(await service.VoteAsync(versus, extraMember1, votedForFirst: false));
                Assert.That(await service.VoteAsync(versus, extraMember2, votedForFirst: false));
                Assert.That(await service.VoteAsync(versus, owner, votedForFirst: true));
                Assert.That(await service.VoteAsync(versus, extraMember3, votedForFirst: true));
            }
            else
            {
                Assert.That(await service.VoteAsync(versus, extraMember1, votedForFirst: true));
                Assert.That(await service.VoteAsync(versus, extraMember2, votedForFirst: true));
                Assert.That(await service.VoteAsync(versus, owner, votedForFirst: false));
                Assert.That(await service.VoteAsync(versus, extraMember3, votedForFirst: false));
            }


            // Check
            var status = (await service.GetVersusInfoAsync(versus)).Value.status;

            if (lastVoteForFirst) 
                Assert.That(status == Statuses.FIRST_WON);
            else 
                Assert.That(status == Statuses.SECOND_WON);

        }




        [TestCase(true)]
        [TestCase(false)]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenVotersVoteEvenlyThenCandidateChosenByLastVoteGoesToNextVersus(bool lastVoteForFirst)
        {     


            var service = StartUpRoutines.PrepareMainService();
            // Add room
            var (room, owner) = await service.NewRoomAndOwner();
            // Add extra room members
            var extraMember1 = await service.NewExtraMember(room);
            var extraMember2 = await service.NewExtraMember(room);
            var extraMember3 = await service.NewExtraMember(room);
            // Add candidates
            var candidate1 = (await service.AddCandidateAsync(room, "candidate1")).Value;
            var candidate2 = (await service.AddCandidateAsync(room, "candidate2")).Value;
            var candidate3 = (await service.AddCandidateAsync(room, "candidate3")).Value;
            // Start voting
            Assert.That(await service.StartVotingAsync(room));
            // Get versus
            var versus = (await service.GetActiveVersesAsync(room, owner)).Single();
            var versusInfo = (await service.GetVersusInfoAsync(versus)).Value;
            var first = versusInfo.firstCandidateId;
            var second = versusInfo.secondCandidateId;

            // Complete versus!
            // Votes should be distributed equally between candidates. 
            if (lastVoteForFirst)
            {
                Assert.That(await service.VoteAsync(versus, extraMember1, votedForFirst: false));
                Assert.That(await service.VoteAsync(versus, extraMember2, votedForFirst: false));
                Assert.That(await service.VoteAsync(versus, owner, votedForFirst: true));
                Assert.That(await service.VoteAsync(versus, extraMember3, votedForFirst: true));
            }
            else
            {
                Assert.That(await service.VoteAsync(versus, extraMember1, votedForFirst: true));
                Assert.That(await service.VoteAsync(versus, extraMember2, votedForFirst: true));
                Assert.That(await service.VoteAsync(versus, owner, votedForFirst: false));
                Assert.That(await service.VoteAsync(versus, extraMember3, votedForFirst: false));
            }


            // Check
            var nextVersus = (await service.GetActiveVersesAsync(room, owner)).Single();
            var nextVersusInfo = (await service.GetVersusInfoAsync(nextVersus)).Value;
            var nextVersusCandidates = new List<int>() 
            { 
                nextVersusInfo.firstCandidateId,
                nextVersusInfo.secondCandidateId
            };

            if (lastVoteForFirst)
            {
                Assert.That(nextVersusCandidates, Has.Member(first));
                Assert.That(nextVersusCandidates, Has.No.Member(second));
            }
            else
            {
                Assert.That(nextVersusCandidates, Has.Member(second));
                Assert.That(nextVersusCandidates, Has.No.Member(first));
            }

        }

    }
}