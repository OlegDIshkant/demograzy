using Demograzy.Core.Test.CommonRoutines;
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
            var service = StartUpRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("client_for_room");
            var roomId = (await service.AddRoomAsync(ownerId, "some_room", PASSPHRASE)).Value;
            var joinResults = new List<bool>();

            for(int i = 0; i < MAX_ROOM_MEMBERS - 1; i++)
            {
                var otherClientId = await service.AddClientAsync($"client_{i}");
                joinResults.Add(await service.AddMember(roomId, otherClientId, PASSPHRASE));
            }

            Assert.That(joinResults, Has.No.Member(false));
        }



        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenOtherClientsJoinRoomThenMembersListHasOnlyThemAndOwner()
        {
            var service = StartUpRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("client_for_room");
            var roomId = (await service.AddRoomAsync(ownerId, "some_room", PASSPHRASE)).Value;
            var expectedMembers = new List<int>() { ownerId };

            for(int i = 0; i < MAX_ROOM_MEMBERS - 1; i++)
            {
                var otherClientId = await service.AddClientAsync($"client_{i}");
                Assert.That(await service.AddMember(roomId, otherClientId, PASSPHRASE));
                expectedMembers.Add(otherClientId);
            }

            Assert.That(await service.GetMembers(roomId), Is.EquivalentTo(expectedMembers));
        }



        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenOtherClientsJoinRoomThenTheirJoinedRoomsListsContainTheRoom()
        {
            var service = StartUpRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("client_for_room");
            var roomId = (await service.AddRoomAsync(ownerId, "some_room", PASSPHRASE)).Value;
            var clientIds = new List<int>();

            for(int i = 0; i < MAX_ROOM_MEMBERS - 1; i++)
            {
                var otherClientId = await service.AddClientAsync($"client_{i}");
                await service.AddMember(roomId, otherClientId, PASSPHRASE);
                clientIds.Add(otherClientId);
            }

            foreach(var id in clientIds)
            {
                Assert.That(await service.GetJoinedRooms(id), Has.Member(roomId));
            }
        }



        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenClientJoinsRoomThenActiveVersesListsAreNull()
        {
            var service = StartUpRoutines.PrepareMainService();
            var owner = await service.AddClientAsync("client_for_room");
            var room = (await service.AddRoomAsync(owner, "some_room", PASSPHRASE)).Value;
            var extraMember = await service.AddClientAsync("extra_member");
            
            await service.AddMember(room, extraMember, PASSPHRASE);

            Assert.That(await service.GetActiveVersesAsync(room, owner), Is.Null);
            Assert.That(await service.GetActiveVersesAsync(room, extraMember), Is.Null);
        }


    }

}