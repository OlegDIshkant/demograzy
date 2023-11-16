using Demograzy.BusinessLogic.DataAccess;
using static Demograzy.Core.Test.GeneralConstants;

namespace Demograzy.Core.Test.Client.Create.Success
{
    [TestFixture]
    public class CommonCreation
    {
        
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
        public async Task WhenClientJustAddedThenHisJoinedRoomsListIsEmpty()
        {
            var service = CommonRoutines.PrepareMainService();

            var clientId = await service.AddClientAsync("client");

            Assert.That(await service.GetJoinedRooms(clientId), Is.Empty);
        }


    }
}