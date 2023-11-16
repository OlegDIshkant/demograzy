using static Demograzy.Core.Test.GeneralConstants;

namespace Demograzy.Core.Test.Room.Create.Fail
{
    [TestFixture]
    public class AboveLimit
    {
        


        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenTryAddRoomsAboveLimitThenOwnedRoomsListDoesNotChange()
        {
            var service = CommonRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("client_for_room");       
            for (int i = 0; i < MAX_OWNED_ROOMS; i++)
            {
                var roomId = await service.AddRoomAsync(ownerId, $"some_room_{i}", "");
            }
            var expectedRoomIds = await service.GetOwnedRoomsAsync(ownerId);

            var failedRoomId = await service.AddRoomAsync(ownerId, $"room_to_fail", "");

            Assert.That(await service.GetOwnedRoomsAsync(ownerId), Is.EqualTo(expectedRoomIds));
        }




        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenTryAddRoomsAboveLimitThenReturnNullId()
        {
            var service = CommonRoutines.PrepareMainService();
            var ownerId = await service.AddClientAsync("client_for_room");       
            for (int i = 0; i < MAX_OWNED_ROOMS; i++)
            {
                var roomId = await service.AddRoomAsync(ownerId, $"some_room_{i}", "");
            }

            var failedRoomId = await service.AddRoomAsync(ownerId, $"room_to_fail", "");

            Assert.That(failedRoomId, Is.Null);
        }




    }
}