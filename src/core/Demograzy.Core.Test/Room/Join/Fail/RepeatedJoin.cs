using static Demograzy.Core.Test.GeneralConstants;

namespace Demograzy.Core.Test.Room.Join.Fail
{

    [TestFixture]
    public class RepeatedJoin
    {


        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenSameClientTryJoinRoomAgainThenReturnsFalse()
        {
            var service = CommonRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("client_for_room");
            var roomId = (await service.AddRoomAsync(ownerId, "some_room", "")).Value;
            var memberId = await service.AddClientAsync("client_for_room_1");
            Assert.That(await service.AddMember(roomId, memberId));

            var extraJoinFailed = !await service.AddMember(roomId, memberId);

            Assert.That(extraJoinFailed);
        }



        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenSameClientTryJoinRoomAgainThenMembersListDoesNotChange()
        {
            var service = CommonRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("client_for_room");
            var roomId = (await service.AddRoomAsync(ownerId, "some_room", "")).Value;
            var memberId = await service.AddClientAsync("client_for_room_1");
            Assert.That(await service.AddMember(roomId, memberId));
            var expectedMembers = await service.GetMembers(roomId);

            await service.AddMember(roomId, memberId);

            Assert.That(await service.GetMembers(roomId), Is.EqualTo(expectedMembers));
        }



        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenSameClientTryJoinRoomAgainThenHisJoinedRoomsListDoesNotChange()
        {
            var service = CommonRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("client_for_room");
            var roomId = (await service.AddRoomAsync(ownerId, "some_room", "")).Value;
            var memberId = await service.AddClientAsync("client_for_room_1");
            Assert.That(await service.AddMember(roomId, memberId));
            var expectedRooms = await service.GetJoinedRooms(memberId);

            await service.AddMember(roomId, memberId);

            Assert.That(await service.GetJoinedRooms(memberId), Is.EqualTo(expectedRooms));
        }



    }

}