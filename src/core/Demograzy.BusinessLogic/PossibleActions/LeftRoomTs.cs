using System.Collections.Generic;
using System.Threading.Tasks;
using Demograzy.BusinessLogic.DataAccess;
using Demograzy.BusinessLogic.PossibleActions;

namespace Demograzy.BusinessLogic
{
    internal class LeftRoomTs : TransactionScript<bool>
    {
        private readonly int _roomId;
        private readonly int _clientId;


        public LeftRoomTs(int roomId, int clientId, ITransactionMeans transactionMeans) : base(transactionMeans)
        {
            _roomId = roomId;
            _clientId = clientId;
        }


        protected override async Task<Result> OnRunAsync()
        {
            if (await MayLeftRoom())
            {
                return Result.DependsOn(await LeftRoom());
            }
            else
            {
                return Result.Fail(false);
            }
        }


        private async Task<bool> MayLeftRoom()
        {
            var roomInfo = await RoomGateway.GetRoomInfoAsync(_roomId);
            if (RoomDoesNotExist(roomInfo)) return false;
            if (VotingStarted(roomInfo)) return false;

            return true;

            //-----------
            bool RoomDoesNotExist(RoomInfo? roomInfo) => !roomInfo.HasValue;
            bool VotingStarted(RoomInfo? roomInfo) => roomInfo.Value.votingStarted;
        }


        private Task<bool> LeftRoom()
        {
            return MembershipGateway.ForgetMemberAsync(_roomId, _clientId);
        }
        
    }
}