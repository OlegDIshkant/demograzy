using Demograzy.BusinessLogic.DataAccess;
using Demograzy.BusinessLogic.PossibleActions;


namespace Demograzy.BusinessLogic
{
    public sealed class TsFactory
    {
        private readonly ITransactionMeansFactory _transactionMeansFactory;
        

        public TsFactory(ITransactionMeansFactory transactionMeansFactory)
        {
            _transactionMeansFactory = transactionMeansFactory;
        }


        public TransactionScript<int> AddClient(string name) =>
            new AddClientTs(name, _transactionMeansFactory.Create());


        public TransactionScript<bool> DropClient(int clientId) =>
            new DropClientTs(clientId, _transactionMeansFactory.Create());


        public TransactionScript<ClientInfo> GetClientInfo(int clientId) =>
            new GetClientInfoTs(clientId, _transactionMeansFactory.Create());


        public TransactionScript<int> GetClientAmount() =>
            new GetClientsAmountTs(_transactionMeansFactory.Create());

    }
}