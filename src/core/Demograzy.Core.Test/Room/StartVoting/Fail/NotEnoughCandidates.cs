using Demograzy.Core.Test.CommonRoutines;
using static Demograzy.Core.Test.GeneralConstants;

namespace Demograzy.Core.Test.Room.StartVoting.Fail
{
    [TestFixture]
    public class NotEnoughCandidates
    {

        [TestCase(0)]
        [TestCase(1)]
        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenStartVotingWithFewCandidatesThenReturnsFalse(int candidatesAmount)
        {
            Assert.That(candidatesAmount < MIN_CANDIDATES);
            Assert.That(candidatesAmount >= 0);
            var service = StartUpRoutines.PrepareMainService();
            var owner = await service.AddClientAsync("room_owner"); 
            var room = (await service.AddRoomAsync(owner, "some_room", "")).Value;
            // Add candidates
            var candidates = Enumerable.Range(0, candidatesAmount)
            .Select(async i => await service.AddCandidateAsync(room, $"candidate_{i}"));

            var startingFailed = !await service.StartVotingAsync(room);

            Assert.That(startingFailed);
        }

    }
}