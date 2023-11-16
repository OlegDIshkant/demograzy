using Demograzy.Core.Test.CommonRoutines;
using static Demograzy.Core.Test.GeneralConstants;

namespace Demograzy.Core.Test.Room.StartVoting.Fail
{
    [TestFixture]
    public class RepeatedInvoke
    {

        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenStartVotingAgainThenReturnsFalse()
        {
            var service = StartUpRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("room_owner"); 
            var roomId = (await service.AddRoomAsync(ownerId, "some_room", "")).Value;
            Assert.That(await service.StartVotingAsync(roomId));

            var secondStartingFailed = !await service.StartVotingAsync(roomId);

            Assert.That(secondStartingFailed);
        }

    }
}