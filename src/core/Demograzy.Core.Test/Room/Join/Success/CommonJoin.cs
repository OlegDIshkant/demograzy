using static Demograzy.Core.Test.GeneralConstants;

namespace Demograzy.Core.Test.Room.Join.Success
{

    [TestFixture]
    public class CommonJoin
    {



        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenOtherClientsJoinsRoomThenReturnsTrueEachTime()
        {
            var service = CommonRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("client_for_room");
            var roomId = (await service.AddRoomAsync(ownerId, "some_room", "")).Value;
            var joinResults = new List<bool>();

            for(int i = 0; i < MAX_ROOM_MEMBERS - 1; i++)
            {
                var otherClientId = await service.AddClientAsync($"client_{i}");
                joinResults.Add(await service.AddMember(roomId, otherClientId));
            }

            Assert.That(joinResults, Has.No.Member(false));
        }



        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenOtherClientsJoinRoomThenMembersListHasOnlyThemAndOwner()
        {
            var service = CommonRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("client_for_room");
            var roomId = (await service.AddRoomAsync(ownerId, "some_room", "")).Value;
            var expectedMembers = new List<int>() { ownerId };

            for(int i = 0; i < MAX_ROOM_MEMBERS - 1; i++)
            {
                var otherClientId = await service.AddClientAsync($"client_{i}");
                Assert.That(await service.AddMember(roomId, otherClientId));
                expectedMembers.Add(otherClientId);
            }

            Assert.That(await service.GetMembers(roomId), Is.EquivalentTo(expectedMembers));
        }



        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenOtherClientsJoinRoomThenTheirJoinedRoomsListsContainTheRoom()
        {
            var service = CommonRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("client_for_room");
            var roomId = (await service.AddRoomAsync(ownerId, "some_room", "")).Value;
            var clientIds = new List<int>();

            for(int i = 0; i < MAX_ROOM_MEMBERS - 1; i++)
            {
                var otherClientId = await service.AddClientAsync($"client_{i}");
                await service.AddMember(roomId, otherClientId);
                clientIds.Add(otherClientId);
            }

            foreach(var id in clientIds)
            {
                Assert.That(await service.GetJoinedRooms(id), Has.Member(roomId));
            }
        }


    }

}