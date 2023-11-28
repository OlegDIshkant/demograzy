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
            if (await LockData() && await MayStartVoting())
            {
                return Result.DependsOn(await StartVoting());
            }
            else
            {
                return Result.Fail(false);
            }
        }

        private async Task<bool> LockData()
        {
            return
                await RoomGateway.LockAsync() &&
                await CandidateGateway.LockAsync() &&
                await VersesGateway.LockAsync();
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

            var failedToStartVersesForPairs = !await StartVersesForCandidatePairs(candidates);
            if (failedToStartVersesForPairs) return false;

            if (ShouldAddFictiveVersusForLastCandidate(candidates))
            {
                var failedToAddFictiveVersus = !await AddFictiveVersesForCandidate(candidates.Last());
                if (failedToAddFictiveVersus) return false;
            }

            return true;
        }
        
    


        private async Task<bool> StartVersesForCandidatePairs(ICollection<int> candidates)
        {
            var pairsAmount = (int)(candidates.Count / 2);
            for (int i = 0; i < pairsAmount; i += 1)
            {
                var versus = await VersesGateway.AddVersusAsync(
                    _roomId,
                    candidates.ElementAt(i * 2),
                    candidates.ElementAt(i * 2 + 1));
                    
                if (!versus.HasValue)
                {
                    return false;
                }
            }
            return true;
        }
        
    


        private bool ShouldAddFictiveVersusForLastCandidate(ICollection<int> candidates)
        {
            var candidatesAmountIsOdd = (candidates.Count % 2) != 0;
            return candidatesAmountIsOdd; 
        }
        
    


        private async Task<bool> AddFictiveVersesForCandidate(int candidateId)
        {
            return (await VersesGateway.AddFictiveVersusAsync(_roomId, candidateId)).HasValue;

        }
        
    }
}