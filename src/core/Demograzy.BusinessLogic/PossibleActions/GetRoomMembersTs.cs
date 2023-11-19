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


        protected override async Task<Result> OnRunAsync()
        {
            if (await RoomGateway.CheckRoomExistsAsync(_roomId))
            {
                return Result.Success(await MembershipGateway.GetRoomMembersAsync(_roomId));
            }
            else
            {
                return Result.Fail(null);
            }
        }
        
    }
}