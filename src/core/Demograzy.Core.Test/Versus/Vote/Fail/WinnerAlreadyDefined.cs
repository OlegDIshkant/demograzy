using Demograzy.BusinessLogic.DataAccess;
using Demograzy.Core.Test.CommonRoutines;
using static Demograzy.Core.Test.GeneralConstants;

namespace Demograzy.Core.Test.Versus.Vote.Fail
{
    
    [TestFixture]
    public class WinnerAlreadyDefined
    {

        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenMostMembersVotedTheSameWayAndOtherMemberTriesVoteThenReturnsFalse()
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

            // Other member vote
            var votingFailed = !await service.VoteAsync(versus, extraMember4, votedForFirst: false);

            Assert.That(votingFailed);
        }



    }
}