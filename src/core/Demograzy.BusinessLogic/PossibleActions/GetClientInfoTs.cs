using System.Threading.Tasks;
using Demograzy.BusinessLogic.DataAccess;


namespace Demograzy.BusinessLogic.PossibleActions
{
    internal class GetClientInfoTs : TransactionScript<ClientInfo?>
    {
        private readonly int _clientId;


        public GetClientInfoTs(int clientId, ITransactionMeans transactionMeans) : base(transactionMeans)
        {
            _clientId = clientId;
        }


        protected override async Task<Result> OnRunAsync()
        {
            return Result.Success(await ClientGateway.GetClientInfoAsync(_clientId));
        }
        
    }
}