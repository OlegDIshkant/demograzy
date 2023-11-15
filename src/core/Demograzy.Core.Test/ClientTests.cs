using Demograzy.BusinessLogic.DataAccess;

namespace Demograzy.Core.Test
{
    [TestFixture()]
    public class ClientTests
    {
        public const int STANDARD_TIMEOUT = 3000;


        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenAddClientThenAmountOfClientsGrows()
        {
            var service = CommonRoutines.PrepareMainService();
            var clientsExpected = await service.GetClientAmount() + 1;

            await service.AddClientAsync("test_client");

            var clientsActually = await service.GetClientAmount();
            Assert.That(clientsActually, Is.EqualTo(clientsExpected));
        }



        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenAddClientThenCanQueryHisInfo()
        {
            var service = CommonRoutines.PrepareMainService();
            var originalInfo = new ClientInfo() { name = "test_client" };

            var clientId = await service.AddClientAsync(originalInfo.name);

            var fetchedInfo = await service.GetClientInfo(clientId);
            Assert.That(fetchedInfo, Is.EqualTo(originalInfo));
        }



        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenAddClientsWithSameNameThenDistinctClientsAdded()
        {
            const int clientsAmount = 4;
            Assert.That(clientsAmount, Is.AtLeast(2));
            var service = CommonRoutines.PrepareMainService();
            var clientIds = new List<int>();

            for (int i = 0; i < clientsAmount; i++)
            {
                clientIds.Add(await service.AddClientAsync("same_name"));
            }
                        
            Assert.That(clientIds.Count, Is.EqualTo(clientsAmount));
            Assert.That(clientIds, Is.Unique);
        }



        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenDeleteClientThenAmountOfClientsDecrease()
        {
            var service = CommonRoutines.PrepareMainService();
            var clientId = await service.AddClientAsync("test_client");
            var clientsExpected = await service.GetClientAmount() - 1;

            await service.DropClientAsync(clientId);

            var clientsActually = await service.GetClientAmount();
            Assert.That(clientsActually, Is.EqualTo(clientsExpected));
        }



        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenDeleteClientAgainThenAmountOfClientsStaysTheSame()
        {
            var service = CommonRoutines.PrepareMainService();
            var clientId = await service.AddClientAsync("test_client");
            Assert.True(await service.DropClientAsync(clientId));
            var clientsExpected = await service.GetClientAmount();

            await service.DropClientAsync(clientId);

            var clientsActually = await service.GetClientAmount();
            Assert.That(clientsActually, Is.EqualTo(clientsExpected));
        }



        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenDeleteClientThenDeletionReturnsTrue()
        {
            var service = CommonRoutines.PrepareMainService();
            var clientId = await service.AddClientAsync("test_client");

            var success = await service.DropClientAsync(clientId);

            Assert.True(success);
        }



        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenDeleteClientAgainThenDeletionReturnsFalse()
        {
            var service = CommonRoutines.PrepareMainService();
            var clientId = await service.AddClientAsync("test_client");
            Assert.True(await service.DropClientAsync(clientId));

            var success = await service.DropClientAsync(clientId);

            Assert.False(success);
        }






        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenDeleteRecentlyAddedClientThenFurtherNewClientsDoNotGainHisId()
        {            
            const int clientsToCheck = 4;
            Assert.That(clientsToCheck, Is.AtLeast(1));
            var service = CommonRoutines.PrepareMainService();
            var originalId = await service.AddClientAsync("original_client");
            Assert.True(await service.DropClientAsync(originalId));
            var furtherIds = new List<int>();

            for (int i = 0; i < clientsToCheck; i++)
            {
                furtherIds.Add(await service.AddClientAsync("further_client_" + i));
            }

            Assert.That(furtherIds, Has.None.EqualTo(originalId));
        }






        [Test]
        [Timeout(STANDARD_TIMEOUT)]
        public async Task WhenDeleteClientThenNullReturnsAsItsInfo()
        {            
            var service = CommonRoutines.PrepareMainService();
            var originalId = await service.AddClientAsync("some_client");

            var wasDeleted = await service.DeleteRoomAsync(originalId);

            Assert.That(wasDeleted);
            Assert.That(await service.GetRoomInfoAsync(originalId), Is.Null);
        }



    }
}