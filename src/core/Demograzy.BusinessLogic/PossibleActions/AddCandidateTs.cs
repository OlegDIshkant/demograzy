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


        protected override async Task<int?> OnRunAsync()
        {
            var candidatesAmount = await CandidateGateway.GetCandidatesAmount(_roomId);
            if (candidatesAmount < Limits.MAX_CANDIDATES_PER_ROOM)
            {
                return await CandidateGateway.AddCandidateAsync(_roomId, _candidateName);
            }
            else
            {
                return null;
            }
        }
        
    }
}