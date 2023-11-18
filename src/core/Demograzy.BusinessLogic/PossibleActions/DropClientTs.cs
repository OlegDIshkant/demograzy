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
                return await ClientGateway.DropClientAsync(_clientId);
            }
            else
            {
                return false;
            }

        }


        private async Task<bool> MayDropClient()
        {
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