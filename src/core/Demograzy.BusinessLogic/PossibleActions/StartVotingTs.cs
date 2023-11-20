using System.Collections.Generic;
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


        private Task<bool> StartVoting()
        {
            return RoomGateway.StartVotingAsync(_roomId);
        }
        
    }
}