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


        public async Task<ClientInfo?> GetClientInfo(int clientId) =>
            await new GetClientInfoTs(clientId, await _transactionMeansFactory.CreateAsync()).RunAsync();


        public async Task<int> GetClientAmount() =>
            await new GetClientsAmountTs(await _transactionMeansFactory.CreateAsync()).RunAsync();


        public async Task<int?> AddRoomAsync(int ownerClientId, string title, string passphrase) => 
            await new AddRoomTs(ownerClientId, title, passphrase, await _transactionMeansFactory.CreateAsync()).RunAsync();

        public async Task<ICollection<int>> GetOwnedRoomsAsync(int ownerClientId) => 
            await new GetOwnedRoomsTs(ownerClientId, await _transactionMeansFactory.CreateAsync()).RunAsync();

        public async Task<RoomInfo?> GetRoomInfoAsync(int roomId) => 
            await new GetRoomInfoTs(roomId, await _transactionMeansFactory.CreateAsync()).RunAsync();

        public async Task<bool> DeleteRoomAsync(int roomId) => 
            await new DeleteRoomTs(roomId, await _transactionMeansFactory.CreateAsync()).RunAsync();

        public async Task<bool> AddMember(int roomId, int clientId, string passphrase) => 
            await new JoinRoomTs(roomId, clientId, passphrase, await _transactionMeansFactory.CreateAsync()).RunAsync();

        public async Task<ICollection<int>> GetMembers(int roomId) => 
            await new GetRoomMembersTs(roomId, await _transactionMeansFactory.CreateAsync()).RunAsync();

        public async Task<bool> DeleteMemberAsync(int roomId, int clientId) => 
            await new LeftRoomTs(roomId, clientId, await _transactionMeansFactory.CreateAsync()).RunAsync();

        public async Task<ICollection<int>> GetJoinedRooms(int clientId) => 
            await new GetJoinedRoomsTs(clientId, await _transactionMeansFactory.CreateAsync()).RunAsync();

        public async Task<int?> AddCandidateAsync(int roomId, string name) => 
            await new AddCandidateTs(roomId, name, await _transactionMeansFactory.CreateAsync()).RunAsync();

        public async Task<bool> DeleteCandidateAsync(int candidateId) => 
            await new DeleteCandidateTs(candidateId, await _transactionMeansFactory.CreateAsync()).RunAsync();

        public async Task<ICollection<int>> GetCandidatesAsync(int roomId) => 
            await new GetCandidatesTs(roomId, await _transactionMeansFactory.CreateAsync()).RunAsync();

        public async Task<CandidateInfo?> GetCandidateInfo(int candidateId) => 
            await new GetCandidateInfoTs(candidateId, await _transactionMeansFactory.CreateAsync()).RunAsync();

        public async Task<bool> StartVotingAsync(int roomId) =>
            await new StartVotingTs(roomId, await _transactionMeansFactory.CreateAsync()).RunAsync();

        public async Task<int?> GetWinnerAsync(int roomId) => 
            await new GetWinnerTs(roomId, await _transactionMeansFactory.CreateAsync()).RunAsync();

        public async Task<ICollection<int>> GetActiveVersesAsync(int roomId, int clientId) => 
            await new GetActiveVersesTs(roomId, clientId, await _transactionMeansFactory.CreateAsync()).RunAsync();

        public async Task<VersusInfo?> GetVersusInfoAsync(int versusId) => 
            await new GetVersusInfoTs(versusId, await _transactionMeansFactory.CreateAsync()).RunAsync();

        public async Task<bool> VoteAsync(int versusId, int clientId, bool votedForFirst) => 
            await new VoteTs(versusId, clientId, votedForFirst, await _transactionMeansFactory.CreateAsync()).RunAsync();

    } 
}