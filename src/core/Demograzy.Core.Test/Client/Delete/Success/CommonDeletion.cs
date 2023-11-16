using Demograzy.Core.Test.CommonRoutines;
using static Demograzy.Core.Test.GeneralConstants;


namespace Demograzy.Core.Test.Client.Delete.Success
{
    [TestFixture]
    public class CommonDeletion
    {

        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenDeleteClientThenDeletionReturnsTrue()
        {
            var service = StartUpRoutines.PrepareMainService();
            var clientId = await service.AddClientAsync("test_client");

            var success = await service.DropClientAsync(clientId);

            Assert.True(success);
        }



        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenDeleteClientThenAmountOfClientsDecrease()
        {
            var service = StartUpRoutines.PrepareMainService();
            var clientId = await service.AddClientAsync("test_client");
            var clientsExpected = await service.GetClientAmount() - 1;

            await service.DropClientAsync(clientId);

            var clientsActually = await service.GetClientAmount();
            Assert.That(clientsActually, Is.EqualTo(clientsExpected));
        }



        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenDeleteRecentlyAddedClientThenFurtherNewClientsDoNotGainHisId()
        {            
            const int clientsToCheck = 4;
            Assert.That(clientsToCheck, Is.AtLeast(1));
            var service = StartUpRoutines.PrepareMainService();
            var originalId = await service.AddClientAsync("original_client");
            Assert.True(await service.DropClientAsync(originalId));
            var furtherIds = new List<int>();

            for (int i = 0; i < clientsToCheck; i++)
            {
                furtherIds.Add(await service.AddClientAsync("further_client_" + i));
            }

            Assert.That(furtherIds, Has.None.EqualTo(originalId));
        }






        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenDeleteClientThenNullReturnsAsItsInfo()
        {            
            var service = StartUpRoutines.PrepareMainService();
            var originalId = await service.AddClientAsync("some_client");

            var wasDeleted = await service.DeleteRoomAsync(originalId);

            Assert.That(wasDeleted);
            Assert.That(await service.GetRoomInfoAsync(originalId), Is.Null);
        }



        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenDeleteClientThenHisOwnedRoomsListIsNull()
        {
            var service = StartUpRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("client_for_room"); 
            var roomId = (await service.AddRoomAsync(ownerId, "some_room", "")).Value;

            Assert.That(await service.DropClientAsync(ownerId));

            Assert.That(await service.GetOwnedRoomsAsync(ownerId), Is.Null);
        }




        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenDeleteClientThenHisRoomsInfoQueriesReturnNull()
        {
            var service = StartUpRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("client_for_room"); 
            var roomIds = new List<int>();
            for (int i = 0; i < MAX_OWNED_ROOMS; i++)
            {
                var roomId = await service.AddRoomAsync(ownerId, $"some_room_{i}", "");
                roomIds.Add(roomId.Value);
            }

            Assert.That(await service.DropClientAsync(ownerId));

            foreach (var id in roomIds)
            {
                Assert.That(await service.GetRoomInfoAsync(id), Is.Null);
            }
        }



        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenClientJustDeletedThenHisJoinedRoomsListIsNull()
        {
            var service = StartUpRoutines.PrepareMainService();
            var clientId = await service.AddClientAsync("client");

            Assert.That(await service.DropClientAsync(clientId));

            Assert.That(await service.GetJoinedRooms(clientId), Is.Null);
        }





        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenDeleteClientThenMembersListOfRoomHeJoinedDoesNotContainHim()
        {
            var service = StartUpRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("client_for_room");
            var roomId = (await service.AddRoomAsync(ownerId, "some_room", "")).Value;
            var memberId = await service.AddClientAsync("client");

            Assert.That(await service.DropClientAsync(memberId));

            Assert.That(await service.GetMembers(roomId), Has.No.Member(memberId));
        }


        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenDeleteRoomOwnerThenMembersListIsNull()
        {
            var service = StartUpRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("client_for_room");
            var roomId = (await service.AddRoomAsync(ownerId, "some_room", "")).Value;
            var memberId = await service.AddClientAsync("client");

            Assert.That(await service.DropClientAsync(ownerId));

            Assert.That(await service.GetMembers(roomId), Is.Null);
        }




        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenDeleteRoomOwnerThenJoinedRoomsListsOfExMembersDoNotContainTheRoom()
        {
            var service = StartUpRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("client_for_room");
            var roomId = (await service.AddRoomAsync(ownerId, "some_room", "")).Value;
            var memberIds = new List<int>();
            for (int i = 0; i < MAX_ROOM_MEMBERS - 1; i++)
            {
                var otherClientId = await service.AddClientAsync($"client_{i}");
                Assert.That(await service.AddMember(roomId, otherClientId));
                memberIds.Add(otherClientId);
            }

            Assert.That(await service.DropClientAsync(ownerId));

            foreach (var id in memberIds)
            {
                Assert.That(await service.GetJoinedRooms(id), Has.No.Member(roomId));
            }
        }




    }
}