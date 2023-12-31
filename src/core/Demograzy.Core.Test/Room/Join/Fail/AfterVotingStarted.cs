using Demograzy.Core.Test.CommonRoutines;
using static Demograzy.Core.Test.GeneralConstants;

namespace Demograzy.Core.Test.Room.Join.Fail
{

    [TestFixture]
    public class AfterVotingStarted
    {
        

        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenClientTriesJoinRoomAfterVotingStartedThenReturnsFalse()
        {
            var service = StartUpRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("client_for_room");
            var roomId = (await service.AddRoomAsync(ownerId, "some_room", PASSPHRASE)).Value;
            for(int i = 0; i < MAX_ROOM_MEMBERS - 2; i++)
            {
                var otherClientId = await service.AddClientAsync($"client_{i}");
                await service.AddMember(roomId, otherClientId, PASSPHRASE);
            }
            var candidate1 = await service.AddCandidateAsync(roomId, "c1");
            var candidate2 = await service.AddCandidateAsync(roomId, "c2");
            Assert.That(await service.StartVotingAsync(roomId));

            var clientToFailId = await service.AddClientAsync("client_to_fail");
            var jointFailed = !await service.AddMember(roomId, clientToFailId, PASSPHRASE);

            Assert.That(jointFailed);
        }


        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenClientTriesJoinRoomAfterVotingStartedThenMembersListDoesNotChange()
        {
            var service = StartUpRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("client_for_room");
            var roomId = (await service.AddRoomAsync(ownerId, "some_room", PASSPHRASE)).Value;
            for(int i = 0; i < MAX_ROOM_MEMBERS - 2; i++)
            {
                var otherClientId = await service.AddClientAsync($"client_{i}");
                await service.AddMember(roomId, otherClientId, PASSPHRASE);
            }
            var candidate1 = await service.AddCandidateAsync(roomId, "c1");
            var candidate2 = await service.AddCandidateAsync(roomId, "c2");
            Assert.That(await service.StartVotingAsync(roomId));
            var expectedMembers = await service.GetMembers(roomId);

            var clientToFailId = await service.AddClientAsync("client_to_fail");
            await service.AddMember(roomId, clientToFailId, PASSPHRASE);

            Assert.That(expectedMembers, Is.EqualTo(await service.GetMembers(roomId)));
        }




    }

}