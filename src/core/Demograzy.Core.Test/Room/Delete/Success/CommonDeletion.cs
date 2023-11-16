using Demograzy.BusinessLogic.DataAccess;
using static Demograzy.Core.Test.GeneralConstants;

namespace Demograzy.Core.Test.Room.Delete.Success
{
    [TestFixture]
    public class CommonDeletion
    {

        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenDeleteExistingRoomThenReturnsTrue()
        {
            var service = CommonRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("client_for_room"); 
            var roomId = await service.AddRoomAsync(ownerId, "some_room", "");

            var deleteSuccessful = await service.DeleteRoomAsync(roomId.Value);

            Assert.That(deleteSuccessful);
        }



        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenDeleteRoomThenItsInfoQueryReturnsNull()
        {
            var service = CommonRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("client_for_room"); 
            var roomId = (await service.AddRoomAsync(ownerId, "some_room", "")).Value;

            Assert.That(await service.DeleteRoomAsync(roomId));

            Assert.That(await service.GetRoomInfoAsync(ownerId), Is.Null);
        }





        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenDeleteRoomThenOwnedRoomsListHasNotItsId()
        {
            var service = CommonRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("client_for_room"); 
            var roomId = (await service.AddRoomAsync(ownerId, "some_room", "")).Value;

            Assert.That(await service.DeleteRoomAsync(roomId));

            Assert.That(await service.GetOwnedRoomsAsync(ownerId), Has.No.Member(roomId));
        }



        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenDeleteRoomThenMembersListsIsNull()
        {
            var service = CommonRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("client_for_room");
            var roomId = (await service.AddRoomAsync(ownerId, "some_room", "")).Value;
            for (int i = 0; i < MAX_ROOM_MEMBERS - 1; i++)
            {
                var otherClientId = await service.AddClientAsync($"client_{i}");
                Assert.That(await service.AddMember(roomId, otherClientId));
            }

            Assert.That(await service.DeleteRoomAsync(roomId));

            Assert.That(await service.GetMembers(roomId), Is.Null);
        }


        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenDeleteRoomThenJoinedRoomsListsOfExMembersDoNotContainedTheRoom()
        {
            var service = CommonRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("client_for_room");
            var roomId = (await service.AddRoomAsync(ownerId, "some_room", "")).Value;
            var memberIds = new List<int>();
            for (int i = 0; i < MAX_ROOM_MEMBERS - 1; i++)
            {
                var otherClientId = await service.AddClientAsync($"client_{i}");
                Assert.That(await service.AddMember(roomId, otherClientId));
                memberIds.Add(otherClientId);
            }

            Assert.That(await service.DeleteRoomAsync(roomId));

            foreach (var id in memberIds)
            {
                Assert.That(await service.GetJoinedRooms(id), Has.No.Member(roomId));
            }
        }



    }
}