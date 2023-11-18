using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Demograzy.BusinessLogic.DataAccess;


namespace Demograzy.BusinessLogic.PossibleActions
{
    internal class DropClientTs : TransactionScript<bool>
    {
        private readonly int _clientId;


        public DropClientTs(int clientId, ITransactionMeans transactionMeans) : base(transactionMeans)
        {
            _clientId = clientId;
        }


        protected override async Task<bool> OnRunAsync()
        {
            if (await MayDropClient())
            {
                var ownedRoomIds = await RoomGateway.GetOwnedRoomsAsync(_clientId);
                
                if (!await ClientGateway.DropClientAsync(_clientId))
                {
                    throw new Exception($"Failed to delete the client '{_clientId}'.");
                }

                foreach (var roomId in ownedRoomIds)
                {
                    await DeleteRoomAndForgetItsMembers(roomId);
                }

                return true;
            }
            else
            {
                return false;
            }

        }


        private async Task DeleteRoomAndForgetItsMembers(int roomId)
        {
            if (!await RoomGateway.DeleteRoomAsync(roomId))
            {
                throw new Exception($"Failed to delete owned room '{roomId}'.");
            }

            if (!await MembershipGateway.ForgetAllMembersAsync(roomId))
            {
                throw new Exception($"Failed to forget members of the room '{roomId}'.");
            }
        }


        private async Task<bool> MayDropClient()
        {
            var clientDoesNotExist = !await ClientGateway.CheckClientExistsAsync(_clientId);
            if (clientDoesNotExist)
            {
                return false;
            }

            var joinedRoomIds = await MembershipGateway.GetJoinedRoomsAsync(_clientId);
            if (joinedRoomIds is null)
            {
                return false;
            } 

            return !await RoomWithStartedVotingExists(joinedRoomIds);
        }


        private async Task<bool> RoomWithStartedVotingExists(List<int> joinedRoomIds)
        {
            // TODO: find a solution with fewer database accesses.
            foreach (var roomId in joinedRoomIds)
            {
                var votingStarted = (await RoomGateway.GetRoomInfoAsync(roomId)).Value.votingStarted;
                if (votingStarted)
                {
                    return true;
                }
            }
            return false;
        }
        
    }
}