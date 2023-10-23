using Demograzy.BusinessLogic.DataAccess;


namespace Demograzy.BusinessLogic
{
    public sealed class TsFactory
    {
        private readonly ITransactionMeansFactory _transactionMeansFactory;
        

        //TODO: add Transaction Script generating properties...
        // public TransactionScript NewTS => new TransactionScript();

        public TsFactory(ITransactionMeansFactory transactionMeansFactory)
        {
            _transactionMeansFactory = transactionMeansFactory;
        }


    }
}