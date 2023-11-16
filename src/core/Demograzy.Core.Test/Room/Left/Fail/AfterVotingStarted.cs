using static Demograzy.Core.Test.GeneralConstants;

namespace Demograzy.Core.Test.Room.Left.Fail
{

    [TestFixture]
    public class AfterVotingStarted
    {
        


        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenClientTriesLeftRoomAfterVotingStartedThenReturnsFalse()
        {
            var service = CommonRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("client_for_room");
            var roomId = (await service.AddRoomAsync(ownerId, "some_room", "")).Value;
            for(int i = 0; i < MAX_ROOM_MEMBERS - 3; i++)
            {
                var otherClientId = await service.AddClientAsync($"client_{i}");
                await service.AddMember(roomId, otherClientId);
            }
            var lastMemberId = await service.AddClientAsync("client_to_fail");
            Assert.That(await service.AddMember(roomId, lastMemberId));
            Assert.That(await service.StartVotingAsync(roomId));
            var expectedMembers = await service.GetMembers(roomId);

            var deleteFailed = !await service.DeleteMemberAsync(roomId, lastMemberId);

            Assert.That(deleteFailed);
        }




        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenClientTriesLeftRoomAfterVotingStartedThenMembersListDoesNotChange()
        {
            var service = CommonRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("client_for_room");
            var roomId = (await service.AddRoomAsync(ownerId, "some_room", "")).Value;
            for(int i = 0; i < MAX_ROOM_MEMBERS - 3; i++)
            {
                var otherClientId = await service.AddClientAsync($"client_{i}");
                await service.AddMember(roomId, otherClientId);
            }
            var lastMemberId = await service.AddClientAsync("client_to_fail");
            Assert.That(await service.AddMember(roomId, lastMemberId));
            Assert.That(await service.StartVotingAsync(roomId));
            var expectedMembers = await service.GetMembers(roomId);

            Assert.That(!await service.DeleteMemberAsync(roomId, lastMemberId));

            Assert.That(expectedMembers, Is.EqualTo(await service.GetMembers(roomId)));
        }





    }

}