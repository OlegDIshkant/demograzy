
using System.Threading.Tasks;
using Demograzy.BusinessLogic.DataAccess;


namespace Demograzy.DataAccess.Sql
{
    public sealed class TransactionMeansFactory : ITransactionMeansFactory
    {
        private readonly ISqlCommandBuilderFactory _factory;


        public TransactionMeansFactory(ISqlCommandBuilderFactory factory)
        {
            _factory = factory;
        }



        public async Task<ITransactionMeans> CreateAsync()
        {
            return await TransactionMeans.NewAsync(await _factory.CreateAsync());
        }
    }
}