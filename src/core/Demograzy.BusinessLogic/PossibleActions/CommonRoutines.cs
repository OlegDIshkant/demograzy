using System.IO;
using System.Threading.Tasks;
using Demograzy.BusinessLogic.DataAccess;

namespace Demograzy.BusinessLogic
{
    internal static class CommonRoutines
    {
        public static async Task<bool> IsVotingStarted(this IRoomsGateway roomsGateway, int roomId)
        {
            return (await roomsGateway.GetRoomInfoAsync(roomId)).Value.votingStarted;
        }


        public static async Task<int> GetRoom(this ICandidatesGateway CandidateGateway, int candidateId)
        {
            return (await CandidateGateway.GetCandidateInfo(candidateId)).Value.roomId;
        }
    }
}