using System.Collections.Generic;
using System.Threading.Tasks;
using Demograzy.BusinessLogic.DataAccess;


namespace Demograzy.BusinessLogic.PossibleActions
{
    internal class GetOwnedRoomsTs : TransactionScript<List<int>>
    {
        private readonly int _clientId;

        public GetOwnedRoomsTs(int clientId, ITransactionMeans transactionMeans) : base(transactionMeans)
        {
            _clientId = clientId;
        }


        protected override async Task<List<int>> OnRunAsync()
        {
            var clientExists = (await ClientGateway.GetClientInfoAsync(_clientId)).HasValue;

            if (clientExists)
            {
                return await RoomGateway.GetOwnedRoomsAsync(_clientId);
            }
            else
            {
                return null;
            }
        }
        
    }
}