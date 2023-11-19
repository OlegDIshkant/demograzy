using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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


        protected override async Task<Result> OnRunAsync()
        {
            var success =
                await MayDropClient() &&
                await DeleteOwnedRoomsAndForgetItsMembers() &&
                await ClientGateway.DropClientAsync(_clientId);

            return success ? 
                Result.Success(true) :
                Result.Fail(false);
        }


        private async Task<bool> DeleteOwnedRoomsAndForgetItsMembers()
        {
            var ownedRoomIds = await RoomGateway.GetOwnedRoomsAsync(_clientId);
            foreach (var roomId in ownedRoomIds)
            {
                if (!await CommonRoutines.DeleteRoomAndForgetItsMembers(roomId))
                {
                    return false;
                }
            }
            return true;
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