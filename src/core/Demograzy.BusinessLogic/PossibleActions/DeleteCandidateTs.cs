using System.Threading.Tasks;
using Demograzy.BusinessLogic.DataAccess;
using Demograzy.BusinessLogic.PossibleActions;

namespace Demograzy.BusinessLogic
{
    internal class DeleteCandidateTs : TransactionScript<bool>
    {
        private readonly int _candidateId;


        public DeleteCandidateTs(int candidateId, ITransactionMeans transactionMeans) : base(transactionMeans)
        {
            _candidateId = candidateId;
        }


        protected override async Task<Result> OnRunAsync()
        {
            if (await MayDeleteCandidate())
            {
                return Result.DependsOn(await CandidateGateway.DeleteCandidateAsync(_candidateId));
            }
            else
            {
                return Result.Fail(false);
            }

            //----------
            async Task<bool> MayDeleteCandidate()
            {
                var candidateInfo = await CandidateGateway.GetCandidateInfo(_candidateId);
                if (!candidateInfo.HasValue) return false;

                var roomInfo = await RoomGateway.GetRoomInfoAsync(candidateInfo.Value.roomId);
                if (!roomInfo.HasValue) return false;
                if (roomInfo.Value.votingStarted) return false;

                return true;
            }
        }
        
    }
}