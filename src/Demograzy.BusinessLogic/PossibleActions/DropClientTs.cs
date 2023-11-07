using System.Threading.Tasks;
using Demograzy.BusinessLogic.DataAccess;


namespace Demograzy.BusinessLogic.PossibleActions
{
    internal class DropClientTs : TransactionScript<bool>
    {
        private readonly int _clientId;


        public DropClientTs(int clientId, ITransactionMeans transactionMeans) : base(transactionMeans)
        {
            _clientId = clientId;
        }


        protected override Task<bool> OnRunAsync()
        {
            return ClientGateway.DropClientAsync(_clientId);
        }
        
    }
}