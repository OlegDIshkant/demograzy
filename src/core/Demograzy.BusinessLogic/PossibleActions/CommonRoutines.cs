using System;
using System.IO;
using System.Threading.Tasks;
using Demograzy.BusinessLogic.DataAccess;

namespace Demograzy.BusinessLogic
{
    internal class CommonRoutines
    {
        private IClientsGateway ClientGateway { get; set; } 
        private IRoomsGateway RoomGateway { get; set; } 
        private IMembershipGateway MembershipGateway { get; set; } 
        private ICandidatesGateway CandidateGateway { get; set; } 
        private IWinnersGateway WinnersGateway { get; set; } 
        private IVersesGateway VersesGateway { get; set; }
        private IVotesGateway VotesGateway { get; set; }


        public CommonRoutines(ITransactionMeans transactionMeans)
        {
            ClientGateway = transactionMeans.ClientsGateway; 
            RoomGateway = transactionMeans.RoomsGateway; 
            MembershipGateway = transactionMeans.MembershipGateway; 
            CandidateGateway = transactionMeans.CandidatesGateway; 
            WinnersGateway = transactionMeans.WinnersGateway; 
            VersesGateway = transactionMeans.VersesGateway;
            VotesGateway = transactionMeans.VotesGateway;
        }


        public async Task<bool> DeleteRoomAndForgetItsMembers(int roomId)
        {
            return 
                await RoomGateway.DeleteRoomAsync(roomId) &&
                await MembershipGateway.ForgetAllMembersAsync(roomId);
        }



        public async Task<bool> RoomExistsAndVotingStarted(int roomId, Action<RoomInfo> OnRoomInfoFetched = null)
        {
            var roomInfo = await RoomGateway.GetRoomInfoAsync(roomId);
            if (!roomInfo.HasValue)
            {
                return false;
            }

            OnRoomInfoFetched?.Invoke(roomInfo.Value);
            return roomInfo.Value.votingStarted;
        }


    }
}