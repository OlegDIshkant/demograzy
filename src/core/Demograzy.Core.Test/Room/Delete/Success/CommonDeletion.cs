using Demograzy.BusinessLogic.DataAccess;
using Demograzy.Core.Test.CommonRoutines;
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
            var service = StartUpRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("client_for_room"); 
            var roomId = await service.AddRoomAsync(ownerId, "some_room", "");

            var deleteSuccessful = await service.DeleteRoomAsync(roomId.Value);

            Assert.That(deleteSuccessful);
        }



        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenDeleteRoomThenItsInfoQueryReturnsNull()
        {
            var service = StartUpRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("client_for_room"); 
            var roomId = (await service.AddRoomAsync(ownerId, "some_room", "")).Value;

            Assert.That(await service.DeleteRoomAsync(roomId));

            Assert.That(await service.GetRoomInfoAsync(ownerId), Is.Null);
        }





        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenDeleteRoomThenOwnedRoomsListHasNotItsId()
        {
            var service = StartUpRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("client_for_room"); 
            var roomId = (await service.AddRoomAsync(ownerId, "some_room", "")).Value;

            Assert.That(await service.DeleteRoomAsync(roomId));

            Assert.That(await service.GetOwnedRoomsAsync(ownerId), Has.No.Member(roomId));
        }



        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenDeleteRoomThenMembersListsIsNull()
        {
            var service = StartUpRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("client_for_room");
            var roomId = (await service.AddRoomAsync(ownerId, "some_room", PASSPHRASE)).Value;
            for (int i = 0; i < MAX_ROOM_MEMBERS - 1; i++)
            {
                var otherClientId = await service.AddClientAsync($"client_{i}");
                Assert.That(await service.AddMember(roomId, otherClientId, PASSPHRASE));
            }

            Assert.That(await service.DeleteRoomAsync(roomId));

            Assert.That(await service.GetMembers(roomId), Is.Null);
        }


        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenDeleteRoomThenJoinedRoomsListsOfExMembersDoNotContainedTheRoom()
        {
            var service = StartUpRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("client_for_room");
            var roomId = (await service.AddRoomAsync(ownerId, "some_room", PASSPHRASE)).Value;
            var memberIds = new List<int>();
            for (int i = 0; i < MAX_ROOM_MEMBERS - 1; i++)
            {
                var otherClientId = await service.AddClientAsync($"client_{i}");
                Assert.That(await service.AddMember(roomId, otherClientId, PASSPHRASE));
                memberIds.Add(otherClientId);
            }

            Assert.That(await service.DeleteRoomAsync(roomId));

            foreach (var id in memberIds)
            {
                Assert.That(await service.GetJoinedRooms(id), Has.No.Member(roomId));
            }
        }


        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenVotingStartedAndDeleteRoomThenActiveVersesListsAreNull()
        {
            var service = StartUpRoutines.PrepareMainService();
            var (room, owner) = await service.NewRoomAndOwner();
            // Add extra members
            var extraMember1 = await service.NewExtraMember(room);
            var extraMember2 = await service.NewExtraMember(room);
            var extraMember3 = await service.NewExtraMember(room);
            // And candidates
            var candidate1 = await service.AddCandidateAsync(room, "1");
            var candidate2 = await service.AddCandidateAsync(room, "2");
            var candidate3 = await service.AddCandidateAsync(room, "3");
            // Start voting
            Assert.That(await service.StartVotingAsync(room));
            // Some votes
            var verses = await service.GetActiveVersesAsync(room, owner);
            foreach (var versus in verses)
            {
                Assert.That(await service.VoteAsync(versus, owner, votedForFirst: false));
            }
            

            Assert.That(await service.DeleteRoomAsync(room));


            Assert.That(await service.GetActiveVersesAsync(room, owner), Is.Null);
            Assert.That(await service.GetActiveVersesAsync(room, extraMember1), Is.Null);
            Assert.That(await service.GetActiveVersesAsync(room, extraMember2), Is.Null);
            Assert.That(await service.GetActiveVersesAsync(room, extraMember3), Is.Null);
        }



    }
}