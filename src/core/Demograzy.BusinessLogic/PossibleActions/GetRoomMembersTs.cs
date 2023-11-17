using System.Collections.Generic;
using System.Threading.Tasks;
using Demograzy.BusinessLogic.DataAccess;
using Demograzy.BusinessLogic.PossibleActions;

namespace Demograzy.BusinessLogic
{
    internal class GetRoomMembersTs : TransactionScript<List<int>>
    {
        private readonly int _roomId;


        public GetRoomMembersTs(int roomId, ITransactionMeans transactionMeans) : base(transactionMeans)
        {
            _roomId = roomId;
        }


        protected override Task<List<int>> OnRunAsync()
        {
            return RoomGateway.GetRoomMembersAsync(_roomId);
        }
        
    }
}