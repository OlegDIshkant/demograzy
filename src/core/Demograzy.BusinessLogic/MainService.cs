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


        public async Task<bool> DropClient(int clientId) =>
            await new DropClientTs(clientId, await _transactionMeansFactory.CreateAsync()).RunAsync();


        public async Task<ClientInfo> GetClientInfo(int clientId) =>
            await new GetClientInfoTs(clientId, await _transactionMeansFactory.CreateAsync()).RunAsync();


        public async Task<int> GetClientAmount() =>
            await new GetClientsAmountTs(await _transactionMeansFactory.CreateAsync()).RunAsync();

    }
}