using System.Collections.Generic;
using System.Threading.Tasks;
using Demograzy.BusinessLogic.DataAccess;


namespace Demograzy.BusinessLogic.PossibleActions
{
    internal class GetCandidatesTs : TransactionScript<ICollection<int>>
    {
        private readonly int _roomId;


        public GetCandidatesTs(int roomId, ITransactionMeans transactionMeans) : base(transactionMeans)
        {
            _roomId = roomId;
        }


        protected override async Task<Result> OnRunAsync()
        {
            return Result.Success(await CandidateGateway.GetCandidates(_roomId));
        }
        
    }
}