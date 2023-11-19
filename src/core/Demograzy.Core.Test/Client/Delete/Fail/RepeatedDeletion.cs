using Demograzy.Core.Test.CommonRoutines;
using static Demograzy.Core.Test.GeneralConstants;

namespace Demograzy.Core.Test.Client.Delete.Fail
{
    [TestFixture]
    public class CommonDeletion
    {
        
        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenDeleteClientAgainThenAmountOfClientsStaysTheSame()
        {
            var service = StartUpRoutines.PrepareMainService();
            var clientId = await service.AddClientAsync("test_client");
            Assert.True(await service.DropClientAsync(clientId));
            var clientsExpected = await service.GetClientAmount();

            await service.DropClientAsync(clientId);

            var clientsActually = await service.GetClientAmount();
            Assert.That(clientsActually, Is.EqualTo(clientsExpected));
        }






        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenDeleteClientAgainThenDeletionReturnsFalse()
        {
            var service = StartUpRoutines.PrepareMainService();
            var clientId = await service.AddClientAsync("test_client");
            Assert.True(await service.DropClientAsync(clientId));

            var deletionFailed = !await service.DropClientAsync(clientId);

            Assert.That(deletionFailed);
        }


    }
}