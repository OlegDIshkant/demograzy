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


        protected override async Task<List<int>> OnRunAsync()
        {
            if (await ClientGateway.CheckClientExistsAsync(_clientId))
            {
                return await MembershipGateway.GetJoinedRoomsAsync(_clientId);
            }
            {
                return null;
            }
        }
        
    }
}