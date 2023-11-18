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


        protected override async Task<bool> OnRunAsync()
        {
            var candidateInfo = await CandidateGateway.GetCandidateInfo(_candidateId);
            var candidateExists = candidateInfo.HasValue;

            if (!candidateExists)
            {
                return false;
            } 

            var roomInfo = await RoomGateway.GetRoomInfoAsync(candidateInfo.Value.roomId);
            var votingNotStarted = !roomInfo.Value.votingStarted;

            if (votingNotStarted)
            {
                return await CandidateGateway.DeleteCandidateAsync(_candidateId);
            }
            else
            {
                return false;
            }
        }
        
    }
}