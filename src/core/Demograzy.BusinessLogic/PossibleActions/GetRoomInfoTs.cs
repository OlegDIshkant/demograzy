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


        protected override async Task<RoomInfo?> OnRunAsync()
        {
            var roomInfo = (await RoomGateway.GetRoomInfoAsync(_roomId)).Value;
            var ownerExists = await ClientGateway.CheckClientExistsAsync(roomInfo.ownerClientId); 
 
            if (ownerExists)
            {
                return roomInfo;
            }
            else 
            {
                return null;
            }


        }
        
    }
}