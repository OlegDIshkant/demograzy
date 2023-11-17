using System.Collections.Generic;
using System.Threading.Tasks;
using Demograzy.BusinessLogic.DataAccess;


namespace Demograzy.BusinessLogic.PossibleActions
{
    internal class GetCandidatesTs : TransactionScript<List<int>>
    {
        private readonly int _roomId;


        public GetCandidatesTs(int roomId, ITransactionMeans transactionMeans) : base(transactionMeans)
        {
            _roomId = roomId;
        }


        protected override Task<List<int>> OnRunAsync()
        {
            return CandidateGateway.GetCandidates(_roomId);
        }
        
    }
}