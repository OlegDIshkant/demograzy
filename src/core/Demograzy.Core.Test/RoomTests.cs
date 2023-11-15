using Demograzy.BusinessLogic.DataAccess;

namespace Demograzy.Core.Test
{
    [TestFixture(Category = "Rooms")]
    public class RoomTests
    {
        public static readonly int MAX_OWNED_ROOMS = 1;
        public const int STANDARD_TIMEOUT = 3000;


        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenAddRoomForActualClientThenReturnsNotNullId()
        {
            var service = CommonRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("client_for_room");

            var roomId = await service.AddRoomAsync(ownerId, "some_room", "");

            Assert.That(roomId, Is.Not.Null);
        }


        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenAddRoomThenCanQueryItsActualInfo()
        {
            var service = CommonRoutines.PrepareMainService();
            var expectedInfo = new RoomInfo()
            {
                ownerClientId = await service.AddClientAsync("client_for_room"),
                passphrase = "hello_hello",
                title = "test_room"
            };

            var roomId = await service.AddRoomAsync(expectedInfo.ownerClientId, expectedInfo.title, expectedInfo.passphrase);

            Assert.That(await service.GetRoomInfoAsync(roomId.Value), Is.EqualTo(expectedInfo));
        }




        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenAddRoomForNonexistingClientThenReturnsNullId()
        {
            var service = CommonRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("client_for_room");
            Assert.That(await service.DropClientAsync(ownerId));

            var roomId = await service.AddRoomAsync(ownerId, "some_room", "");

            Assert.That(roomId, Is.Null);
        }




        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenAddRoomsThenOwnedRoomsListContainsReturnedIds()
        {
            var service = CommonRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("client_for_room");
            var expectedRoomIds = new List<int>(); 
            
            for (int i = 0; i < MAX_OWNED_ROOMS; i++)
            {
                var roomId = await service.AddRoomAsync(ownerId, $"some_room_{i}", "");
                expectedRoomIds.Add(roomId.Value);
            }

            Assert.That(await service.GetOwnedRoomsAsync(ownerId), Is.EquivalentTo(expectedRoomIds));
        }




        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenAddRoomsThenDistinctIdsReturned()
        {
            var service = CommonRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("client_for_room");
            var roomIds = new List<int>(); 
            
            for (int i = 0; i < MAX_OWNED_ROOMS; i++)
            {
                var roomId = await service.AddRoomAsync(ownerId, $"some_room_{i}", "");
                roomIds.Add(roomId.Value);
            }

            Assert.That(roomIds, Is.Unique);
        }




        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenTryAddRoomsAboveLimitThenOwnedRoomsListDoesNotChange()
        {
            var service = CommonRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("client_for_room");       
            for (int i = 0; i < MAX_OWNED_ROOMS; i++)
            {
                var roomId = await service.AddRoomAsync(ownerId, $"some_room_{i}", "");
            }
            var expectedRoomIds = await service.GetOwnedRoomsAsync(ownerId);

            var failedRoomId = await service.AddRoomAsync(ownerId, $"room_to_fail", "");

            Assert.That(await service.GetOwnedRoomsAsync(ownerId), Is.EqualTo(expectedRoomIds));
        }




        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenTryAddRoomsAboveLimitThenReturnNullId()
        {
            var service = CommonRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("client_for_room");       
            for (int i = 0; i < MAX_OWNED_ROOMS; i++)
            {
                var roomId = await service.AddRoomAsync(ownerId, $"some_room_{i}", "");
            }

            var failedRoomId = await service.AddRoomAsync(ownerId, $"room_to_fail", "");

            Assert.That(failedRoomId, Is.Null);
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
        public async Task WhenDeleteClientThenHisOwnedRoomsListIsNull()
        {
            var service = CommonRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("client_for_room"); 
            var roomId = (await service.AddRoomAsync(ownerId, "some_room", "")).Value;

            Assert.That(await service.DropClientAsync(ownerId));

            Assert.That(await service.GetOwnedRoomsAsync(ownerId), Is.Null);
        }




        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenDeleteClientThenHisRoomsInfoQueriesReturnNull()
        {
            var service = CommonRoutines.PrepareMainService();
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
        public async Task WhenDeleteNonexistingRoomThenReturnsFalse()
        {
            var service = CommonRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("client_for_room"); 
            var roomId = await service.AddRoomAsync(ownerId, "some_room", "");
            await service.DeleteRoomAsync(roomId.Value);

            var deleteFailed = !await service.DeleteRoomAsync(roomId.Value);

            Assert.That(deleteFailed);
        }

    }
}