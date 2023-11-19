using System.Collections.Generic;
using System.Threading.Tasks;
using Demograzy.BusinessLogic.DataAccess;


namespace Demograzy.BusinessLogic.PossibleActions
{
    internal class GetRoomInfoTs : TransactionScript<RoomInfo?>
    {
        private readonly int _roomId;

        public GetRoomInfoTs(int roomId, ITransactionMeans transactionMeans) : base(transactionMeans)
        {
            _roomId = roomId;
        }


        protected override async Task<Result> OnRunAsync()
        {
            var roomInfo = await RoomGateway.GetRoomInfoAsync(_roomId);
            var roomExists = roomInfo.HasValue;

            if (roomExists)
            {
                return Result.Success(roomInfo.Value);
            }
            else 
            {
                return Result.Fail(null);
            }


        }
        
    }
}