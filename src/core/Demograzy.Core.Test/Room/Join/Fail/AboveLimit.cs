using Demograzy.Core.Test.CommonRoutines;
using static Demograzy.Core.Test.GeneralConstants;

namespace Demograzy.Core.Test.Room.Join.Fail
{

    [TestFixture]
    public class AboveLimit
    {

        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenClientTriesJoinRoomAboveLimitThenReturnsFalse()
        {
            var service = StartUpRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("client_for_room");
            var roomId = (await service.AddRoomAsync(ownerId, "some_room", "")).Value;
            for(int i = 0; i < MAX_ROOM_MEMBERS - 1; i++)
            {
                var otherClientId = await service.AddClientAsync($"client_{i}");
                await service.AddMember(roomId, otherClientId);
            }

            var clientToFailId = await service.AddClientAsync("client_to_fail");
            var jointFailed = !await service.AddMember(roomId, clientToFailId);

            Assert.That(jointFailed);
        }




        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenClientTriesJoinRoomAboveLimitThenMembersListDoesNotChange()
        {
            var service = StartUpRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("client_for_room");
            var roomId = (await service.AddRoomAsync(ownerId, "some_room", "")).Value;
            for(int i = 0; i < MAX_ROOM_MEMBERS - 1; i++)
            {
                var otherClientId = await service.AddClientAsync($"client_{i}");
                await service.AddMember(roomId, otherClientId);
            }
            var expectedMembers = await service.GetMembers(roomId);

            var clientToFailId = await service.AddClientAsync("client_to_fail");
            await service.AddMember(roomId, clientToFailId);

            Assert.That(expectedMembers, Is.EqualTo(await service.GetMembers(roomId)));
        }


    }

}