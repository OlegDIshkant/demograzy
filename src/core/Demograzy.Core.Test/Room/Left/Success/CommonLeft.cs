using Demograzy.Core.Test.CommonRoutines;
using static Demograzy.Core.Test.GeneralConstants;

namespace Demograzy.Core.Test.Room.Left.Success
{
    [TestFixture]
    public class CommonLeft
    {



        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenMemberLeftRoomThenReturnsTrue()
        {
            var service = StartUpRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("client_for_room");
            var roomId = (await service.AddRoomAsync(ownerId, "some_room", "")).Value;
            var memberId = await service.AddClientAsync("client_for_room_1");
            Assert.That(await service.AddMember(roomId, memberId));

            var deleteSuccess = await service.DeleteMemberAsync(roomId, memberId);

            Assert.That(deleteSuccess);
        }


        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenMemberLeftRoomThenMembersListDoesNotContainHim()
        {
            var service = StartUpRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("client_for_room");
            var roomId = (await service.AddRoomAsync(ownerId, "some_room", "")).Value;
            var memberId = await service.AddClientAsync("client_for_room_1");
            Assert.That(await service.AddMember(roomId, memberId));

            await service.DeleteMemberAsync(roomId, memberId);

            Assert.That(await service.GetMembers(roomId), Has.No.Member(memberId));
        }


        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenMemberLeftRoomThenHisJoinedRoomsListDoesNotContainTheRoom()
        {
            var service = StartUpRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("client_for_room");
            var roomId = (await service.AddRoomAsync(ownerId, "some_room", "")).Value;
            var memberId = await service.AddClientAsync("client_for_room_1");
            Assert.That(await service.AddMember(roomId, memberId));

            await service.DeleteMemberAsync(roomId, memberId);

            Assert.That(await service.GetJoinedRooms(memberId), Has.No.Member(roomId));
        }




    }
}