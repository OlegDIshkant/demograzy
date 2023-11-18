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


        protected override Task<ClientInfo?> OnRunAsync()
        {
            return ClientGateway.GetClientInfoAsync(_clientId);
        }
        
    }
}