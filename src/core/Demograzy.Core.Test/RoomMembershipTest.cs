using Demograzy.BusinessLogic.DataAccess;

namespace Demograzy.Core.Test
{
    public class RoomMembershipTests
    {
        public static readonly int MAX_ROOM_MEMBERS = 10;
        public const int STANDARD_TIMEOUT = 3000;


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
        public async Task WhenClientTriesJoinRoomAboveLimitThenReturnsFalse()
        {
            var service = CommonRoutines.PrepareMainService();
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
        public async Task WhenClientTriesJoinRoomAboveLimitThenMembersListDoesNotChange()
        {
            var service = CommonRoutines.PrepareMainService();
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


        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenMemberLeftRoomThenReturnsTrue()
        {
            var service = CommonRoutines.PrepareMainService();
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
            var service = CommonRoutines.PrepareMainService();
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
            var service = CommonRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("client_for_room");
            var roomId = (await service.AddRoomAsync(ownerId, "some_room", "")).Value;
            var memberId = await service.AddClientAsync("client_for_room_1");
            Assert.That(await service.AddMember(roomId, memberId));

            await service.DeleteMemberAsync(roomId, memberId);

            Assert.That(await service.GetJoinedRooms(memberId), Has.No.Member(roomId));
        }


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


        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenClientJustAddedThenHisJoinedRoomsListIsEmpty()
        {
            var service = CommonRoutines.PrepareMainService();

            var clientId = await service.AddClientAsync("client");

            Assert.That(await service.GetJoinedRooms(clientId), Is.Empty);
        }


        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenClientJustAddedThenHisJoinedRoomsListIsNull()
        {
            var service = CommonRoutines.PrepareMainService();
            var clientId = await service.AddClientAsync("client");

            Assert.That(await service.DropClientAsync(clientId));

            Assert.That(await service.GetJoinedRooms(clientId), Is.Null);
        }


        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenDeleteClientThenMembersListOfRoomHeJoinedDoesNotContainHim()
        {
            var service = CommonRoutines.PrepareMainService();
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
            var service = CommonRoutines.PrepareMainService();
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

            Assert.That(await service.DropClientAsync(ownerId));

            foreach (var id in memberIds)
            {
                Assert.That(await service.GetJoinedRooms(id), Has.No.Member(roomId));
            }
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