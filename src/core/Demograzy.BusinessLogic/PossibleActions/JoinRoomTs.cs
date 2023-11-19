using System.Collections.Generic;
using System.Threading.Tasks;
using Demograzy.BusinessLogic.DataAccess;
using Demograzy.BusinessLogic.PossibleActions;

namespace Demograzy.BusinessLogic
{
    internal class JoinRoomTs : TransactionScript<bool>
    {
        private readonly int _roomId;
        private readonly int _clientId;


        public JoinRoomTs(int roomId, int clientId, ITransactionMeans transactionMeans) : base(transactionMeans)
        {
            _roomId = roomId;
            _clientId = clientId;
        }


        protected override async Task<Result> OnRunAsync()
        {
            if (await MayJoinRoom())
            {
                return Result.DependsOn(await JoinRoom());
            }
            else
            {
                return Result.Fail(false);
            }
        }


        private async Task<bool> MayJoinRoom()
        {
            var limitReached = (await MembershipGateway.GetRoomMembersAsync(_roomId)).Count >= Limits.MAX_MEMBERS_PER_ROOMS;

            return !limitReached;
        }


        private Task<bool> JoinRoom()
        {
            return MembershipGateway.AddRoomMemberAsync(_roomId, _clientId);
        }
        
    }
}