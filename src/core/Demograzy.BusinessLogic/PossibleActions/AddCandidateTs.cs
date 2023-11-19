using System.Threading.Tasks;
using Demograzy.BusinessLogic.DataAccess;
using Demograzy.BusinessLogic.PossibleActions;

namespace Demograzy.BusinessLogic
{
    internal class AddCandidateTs : TransactionScript<int?>
    {
        private readonly int _roomId;
        private readonly string _candidateName;


        public AddCandidateTs(int roomId, string candidateName, ITransactionMeans transactionMeans) : base(transactionMeans)
        {
            _roomId = roomId;
            _candidateName = candidateName;
        }


        protected override async Task<Result> OnRunAsync()
        {
            var votingNotStarted = !(await RoomGateway.GetRoomInfoAsync(_roomId)).Value.votingStarted;
            var limitNotReached = await CandidateGateway.GetCandidatesAmount(_roomId) < Limits.MAX_CANDIDATES_PER_ROOM;

            if (votingNotStarted && limitNotReached)
            {
                return Result.DependsIfNull(await CandidateGateway.AddCandidateAsync(_roomId, _candidateName));
            }
            else
            {
                return Result.Fail(null);
            }
        }
        
    }
}