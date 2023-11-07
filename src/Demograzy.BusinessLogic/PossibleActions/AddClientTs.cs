using System.Threading.Tasks;
using Demograzy.BusinessLogic.DataAccess;
using Demograzy.BusinessLogic.PossibleActions;

namespace Demograzy.BusinessLogic
{
    internal class AddClientTs : TransactionScript<int>
    {
        private readonly string _clientName;


        public AddClientTs(string clientName, ITransactionMeans transactionMeans) : base(transactionMeans)
        {
            _clientName = clientName;
        }


        protected override Task<int> OnRunAsync()
        {
            return base.ClientGateway.AddClientAsync(_clientName);
        }
        
    }
}