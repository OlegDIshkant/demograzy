using Demograzy.Core.Test.CommonRoutines;
using static Demograzy.Core.Test.GeneralConstants;

namespace Demograzy.Core.Test.Room.Delete.Fail
{
    [TestFixture]
    public class NonexistentRoom
    {
        


        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenDeleteNonexistingRoomThenReturnsFalse()
        {
            var service = StartUpRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("client_for_room"); 
            var roomId = await service.AddRoomAsync(ownerId, "some_room", "");
            await service.DeleteRoomAsync(roomId.Value);

            var deleteFailed = !await service.DeleteRoomAsync(roomId.Value);

            Assert.That(deleteFailed);
        }




    }
}