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


        protected override Task<bool> OnRunAsync()
        {
            return CandidateGateway.DeleteCandidateAsync(_candidateId);
        }
        
    }
}