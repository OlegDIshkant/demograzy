using System.Collections.Generic;
using System.Threading.Tasks;
using Demograzy.BusinessLogic.DataAccess;


namespace Demograzy.BusinessLogic.PossibleActions
{
    internal class GetJoinedRoomsTs : TransactionScript<List<int>>
    {
        private readonly int _clientId;

        public GetJoinedRoomsTs(int clientId, ITransactionMeans transactionMeans) : base(transactionMeans)
        {
            _clientId = clientId;
        }


        protected override Task<List<int>> OnRunAsync()
        {
            return MembershipGateway.GetJoinedRoomsAsync(_clientId);
        }
        
    }
}