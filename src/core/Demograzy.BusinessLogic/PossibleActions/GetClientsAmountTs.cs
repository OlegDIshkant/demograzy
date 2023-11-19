using System.Threading.Tasks;
using Demograzy.BusinessLogic.DataAccess;


namespace Demograzy.BusinessLogic.PossibleActions
{
    internal class GetClientsAmountTs : TransactionScript<int>
    {

        public GetClientsAmountTs(ITransactionMeans transactionMeans) : base(transactionMeans)
        {
            
        }


        protected override async Task<Result> OnRunAsync()
        {
            return Result.Success(await ClientGateway.GetClientsAmountAsync());
        }
        
    }
}