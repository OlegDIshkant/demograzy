
using Demograzy.BusinessLogic.DataAccess;


namespace Demograzy.DataAccess.Sql
{
    public sealed class TransactionMeansFactory : ITransactionMeansFactory
    {
        private readonly IConnectionStringProvider _connectionStringProvider;


        public TransactionMeansFactory(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }



        public ITransactionMeans Create()
        {
            return new TransactionMeans(_connectionStringProvider);
        }
    }
}