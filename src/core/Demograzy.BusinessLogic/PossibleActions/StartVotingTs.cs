using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Demograzy.BusinessLogic.DataAccess;
using Demograzy.BusinessLogic.PossibleActions;

namespace Demograzy.BusinessLogic
{
    internal class StartVotingTs : TransactionScript<bool>
    {
        private readonly int _roomId;


        public StartVotingTs(int roomId, ITransactionMeans transactionMeans) : base(transactionMeans)
        {
            _roomId = roomId;
        }


        protected override async Task<Result> OnRunAsync()
        {
            if (await MayStartVoting())
            {
                return Result.DependsOn(await StartVoting());
            }
            else
            {
                return Result.Fail(false);
            }
        }


        private async Task<bool> MayStartVoting()
        {
            var roomInfo = await RoomGateway.GetRoomInfoAsync(_roomId);
            if (VotingAlreadyStarted(roomInfo.Value)) return false;
            var candidatesAmount = await CandidateGateway.GetCandidatesAmount(_roomId);
            return EnoughCandidates(candidatesAmount);

            //----------
            bool EnoughCandidates(int candidateAmount) => candidateAmount >= Limits.MIN_CANDIDATES_TO_START_VOTING;
            bool VotingAlreadyStarted(RoomInfo roomInfo) => roomInfo.votingStarted;
        }


        private async Task<bool> StartVoting()
        {
            return
                await RoomGateway .StartVotingAsync(_roomId) &&
                await StartFirstVerses();
        }


        private async Task<bool> StartFirstVerses()
        {
            var candidates = await CandidateGateway.GetCandidates(_roomId);

            var versesAmount = (int)(candidates.Count / 2);
            for (int i = 0; i < versesAmount; i += 1)
            {
                var versus = await VersesGateway.AddVersusAsync(
                    _roomId,
                    candidates[i * 2],
                    candidates[i * 2 + 1]);
                if (!versus.HasValue)
                {
                    return false;
                }
            }

            return true;
        }
        
    }
}