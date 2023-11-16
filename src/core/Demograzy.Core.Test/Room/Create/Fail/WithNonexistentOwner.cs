using Demograzy.BusinessLogic.DataAccess;
using static Demograzy.Core.Test.GeneralConstants;

namespace Demograzy.Core.Test.Room.Create.Fail
{
    [TestFixture]
    public class WithNonexistentOwner
    {
        


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



    }
}