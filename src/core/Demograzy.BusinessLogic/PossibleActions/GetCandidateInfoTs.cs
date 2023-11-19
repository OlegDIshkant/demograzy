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


        protected override async Task<Result> OnRunAsync()
        {
            return Result.Success(await CandidateGateway.GetCandidateInfo(_candidateId));
        }
        
    }
}