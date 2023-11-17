using System.Threading.Tasks;
using Demograzy.BusinessLogic.DataAccess;


namespace Demograzy.BusinessLogic.PossibleActions
{
    internal class GetCandidateInfoTs : TransactionScript<CandidateInfo?>
    {
        private readonly int _candidateId;


        public GetCandidateInfoTs(int candidateId, ITransactionMeans transactionMeans) : base(transactionMeans)
        {
            _candidateId = candidateId;
        }


        protected override Task<CandidateInfo?> OnRunAsync()
        {
            return CandidateGateway.GetCandidateInfo(_candidateId);
        }
        
    }
}