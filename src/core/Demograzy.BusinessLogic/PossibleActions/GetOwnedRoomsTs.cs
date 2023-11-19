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


        protected override async Task<Result> OnRunAsync()
        {
            if (await ClientGateway.CheckClientExistsAsync(_clientId))
            {
                return Result.Success(await RoomGateway.GetOwnedRoomsAsync(_clientId));
            }
            else
            {
                return Result.Fail(null);
            }
        }
        
    }
}