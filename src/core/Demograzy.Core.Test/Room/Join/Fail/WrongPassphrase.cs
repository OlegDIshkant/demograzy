using Demograzy.Core.Test.CommonRoutines;
using static Demograzy.Core.Test.GeneralConstants;

namespace Demograzy.Core.Test.Room.Join.Fail
{

    [TestFixture]
    public class WrongPassphrase
    {

        [TestCase("Q123")]
        [TestCase("й123")]
        [TestCase("q1234")]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenClientTriesJoinRoomWithWrongPassphraseThenReturnsFalse(string WrongPassphrase)
        {            
            var service = StartUpRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("client_for_room");
            var roomId = (await service.AddRoomAsync(ownerId, "some_room", PASSPHRASE)).Value;
            var clientToFailId = await service.AddClientAsync("client_to_fail");

            var jointFailed = !await service.AddMember(roomId, clientToFailId, WrongPassphrase);

            Assert.That(jointFailed);
        }




        [TestCase("Q123")]
        [TestCase("й123")]
        [TestCase("q1234")]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenClientTriesJoinRoomWithWrongPassphraseThenMembersListDoesNotChange(string wrongPassphrase)
        {
            var service = StartUpRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("client_for_room");
            var roomId = (await service.AddRoomAsync(ownerId, "some_room", PASSPHRASE)).Value;
            var clientToFailId = await service.AddClientAsync("client_to_fail");
            var expectedMembers = await service.GetMembers(roomId);

            await service.AddMember(roomId, clientToFailId, wrongPassphrase);

            Assert.That(expectedMembers, Is.EqualTo(await service.GetMembers(roomId)));
        }


    }

}