using Demograzy.Core.Test.CommonRoutines;
using static Demograzy.Core.Test.GeneralConstants;

namespace Demograzy.Core.Test.Client.Delete.Fail
{
    [TestFixture]
    public class AfterVotingStarted
    {
        


        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenDeleteClientAfterVotingStartedThenReturnsFalse()
        {
            var service = StartUpRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("client_for_room");
            var roomId = (await service.AddRoomAsync(ownerId, "some_room", "")).Value;
            for(int i = 0; i < MAX_ROOM_MEMBERS - 3; i++)
            {
                var otherClientId = await service.AddClientAsync($"client_{i}");
                await service.AddMember(roomId, otherClientId);
            }
            var lastMemberId = await service.AddClientAsync("client_to_fail");
            Assert.That(await service.AddMember(roomId, lastMemberId));
            Assert.That(await service.StartVotingAsync(roomId));
            var expectedMembers = await service.GetMembers(roomId);

            var deleteFailed = !await service.DropClientAsync(lastMemberId);

            Assert.That(deleteFailed);
        }




    }
}