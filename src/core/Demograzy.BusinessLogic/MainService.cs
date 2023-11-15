using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Demograzy.BusinessLogic.DataAccess;
using Demograzy.BusinessLogic.PossibleActions;


namespace Demograzy.BusinessLogic
{
    public sealed class MainService
    {
        private readonly ITransactionMeansFactory _transactionMeansFactory;
        

        public MainService(ITransactionMeansFactory transactionMeansFactory)
        {
            _transactionMeansFactory = transactionMeansFactory;
        }


        public async Task<int> AddClientAsync(string name) =>
            await new AddClientTs(name, await _transactionMeansFactory.CreateAsync()).RunAsync();


        public async Task<bool> DropClientAsync(int clientId) =>
            await new DropClientTs(clientId, await _transactionMeansFactory.CreateAsync()).RunAsync();


        public async Task<ClientInfo> GetClientInfo(int clientId) =>
            await new GetClientInfoTs(clientId, await _transactionMeansFactory.CreateAsync()).RunAsync();


        public async Task<int> GetClientAmount() =>
            await new GetClientsAmountTs(await _transactionMeansFactory.CreateAsync()).RunAsync();


        public Task<int?> AddRoomAsync(int ownerClientId, string title, string passphrase) => throw new NotImplementedException(); 

        public Task<List<int>> GetOwnedRoomsAsync(int ownerClientId) => throw new NotImplementedException();

        public Task<RoomInfo?> GetRoomInfoAsync(int roomId) => throw new NotImplementedException();

        public Task<bool> DeleteRoomAsync(int roomId) => throw new NotImplementedException();

    }
}