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


        public async Task<int?> AddRoomAsync(int ownerClientId, string title, string passphrase) => 
            await new AddRoomTs(ownerClientId, title, passphrase, await _transactionMeansFactory.CreateAsync()).RunAsync();

        public Task<List<int>> GetOwnedRoomsAsync(int ownerClientId) => throw new NotImplementedException();

        public Task<RoomInfo?> GetRoomInfoAsync(int roomId) => throw new NotImplementedException();

        public Task<bool> DeleteRoomAsync(int roomId) => throw new NotImplementedException();

        public async Task<bool> AddMember(int roomId, int clientId) => 
            await new JoinRoomTs(roomId, clientId, await _transactionMeansFactory.CreateAsync()).RunAsync();

        public async Task<List<int>> GetMembers(int roomId) => 
            await new GetRoomMembersTs(roomId, await _transactionMeansFactory.CreateAsync()).RunAsync();

        public Task<bool> DeleteMemberAsync(int roomId, int clientId) => throw new NotImplementedException();

        public async Task<List<int>> GetJoinedRooms(int clientId) => 
            await new GetJoinedRoomsTs(clientId, await _transactionMeansFactory.CreateAsync()).RunAsync();

        public async Task<int?> AddCandidateAsync(int roomId, string name) => 
            await new AddCandidateTs(roomId, name, await _transactionMeansFactory.CreateAsync()).RunAsync();

        public async Task<bool> DeleteCandidateAsync(int candidateId) => 
            await new DeleteCandidateTs(candidateId, await _transactionMeansFactory.CreateAsync()).RunAsync();

        public async Task<List<int>> GetCandidatesAsync(int roomId) => 
            await new GetCandidatesTs(roomId, await _transactionMeansFactory.CreateAsync()).RunAsync();

        public async Task<CandidateInfo?> GetCandidateInfo(int candidateId) => 
            await new GetCandidateInfoTs(candidateId, await _transactionMeansFactory.CreateAsync()).RunAsync();

        public async Task<bool> StartVotingAsync(int roomId) =>
            await new StartVotingTs(roomId, await _transactionMeansFactory.CreateAsync()).RunAsync();

        public Task<int?> GetWinnerAsync(int roomId) => throw new NotImplementedException();

        public Task<List<int>> GetActiveVersesAsync(int roomId, int clientId) => throw new NotImplementedException();

        public Task<VersusInfo> GetVersusInfoAsync(int versusId) => throw new NotImplementedException(); 

        public Task<bool> VoteAsync(int versusId, int clientId, bool votedForFirst) => throw new NotImplementedException();

    } 
}