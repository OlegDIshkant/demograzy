using static Demograzy.Core.Test.GeneralConstants;

namespace Demograzy.Core.Test.Room.Left.Fail
{

    [TestFixture]
    public class ByNonMember
    {
        


        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenNonMemberTryLeftRoomThenReturnsFalse()
        {
            var service = CommonRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("client_for_room");
            var roomId = (await service.AddRoomAsync(ownerId, "some_room", "")).Value;
            var nonMemberId = await service.AddClientAsync("client");

            var deleteFailed = !await service.DeleteMemberAsync(roomId, nonMemberId);

            Assert.That(deleteFailed);
        }


        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenNonMemberTryLeftRoomThenMemberListDoesNotChange()
        {
            var service = CommonRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("client_for_room");
            var roomId = (await service.AddRoomAsync(ownerId, "some_room", "")).Value;
            var nonMemberId = await service.AddClientAsync("client");
            var expectedMembers = await service.GetMembers(roomId);

            await service.DeleteMemberAsync(roomId, nonMemberId);

            Assert.That(await service.GetMembers(roomId), Is.EqualTo(expectedMembers));
        }


        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenNonMemberTryLeftRoomThenHisJoinedRoomsListDoesNotChange()
        {
            var service = CommonRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("client_for_room");
            var roomId = (await service.AddRoomAsync(ownerId, "some_room", "")).Value;
            var nonMemberId = await service.AddClientAsync("client");
            var expectedRooms = await service.GetJoinedRooms(nonMemberId);

            await service.DeleteMemberAsync(roomId, nonMemberId);

            Assert.That(await service.GetJoinedRooms(nonMemberId), Is.EqualTo(expectedRooms));
        }






    }

}