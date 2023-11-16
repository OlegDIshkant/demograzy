using Demograzy.BusinessLogic.DataAccess;
using static Demograzy.Core.Test.GeneralConstants;

namespace Demograzy.Core.Test.Room.Create.Success
{
    [TestFixture]
    public class CommonCreation
    {
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
        public async Task WhenRoomJustAddedThenMembersListContainsOnlyOwner()
        {
            var service = CommonRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("client_for_room");

            var roomId = (await service.AddRoomAsync(ownerId, "some_room", "")).Value;

            var members = await service.GetMembers(roomId);             
            Assert.That(members, Has.Count.EqualTo(1));
            Assert.That(members, Has.Member(roomId));
        }



        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenRoomJustAddedThenJoinedRoomsListOfOwnerContainsTheRoom()
        {
            var service = CommonRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("client_for_room");

            var roomId = (await service.AddRoomAsync(ownerId, "some_room", "")).Value;

            Assert.That(await service.GetJoinedRooms(ownerId), Has.Member(roomId));
        }


    }
}