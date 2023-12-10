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
        private readonly string _passphrase; 


        public JoinRoomTs(int roomId, int clientId, string passphrase, ITransactionMeans transactionMeans) : base(transactionMeans)
        {
            _roomId = roomId;
            _clientId = clientId;
            _passphrase = passphrase;
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
            var roomInfo = await RoomGateway.GetRoomInfoAsync(_roomId);
            if (RoomDoesNotExist(roomInfo)) return false;
            if (VotingStarted(roomInfo)) return false;
            var roomMembers = await MembershipGateway.GetRoomMembersAsync(_roomId);
            if (LimitReached(roomMembers)) return false;
            if (ClientAlreadyJoinedRoom(roomMembers)) return false;

            return true;

            //-----------
            bool RoomDoesNotExist(RoomInfo? roomInfo) => !roomInfo.HasValue;
            bool VotingStarted(RoomInfo? roomInfo) => roomInfo.Value.votingStarted;
            bool LimitReached(ICollection<int> roomMemberIds) => roomMemberIds.Count >= Limits.MAX_MEMBERS_PER_ROOMS;
            bool ClientAlreadyJoinedRoom(ICollection<int> roomMemberIds) => roomMemberIds.Contains(_clientId);
        }


        private Task<bool> JoinRoom()
        {
            return MembershipGateway.AddRoomMemberAsync(_roomId, _clientId);
        }
        
    }
}