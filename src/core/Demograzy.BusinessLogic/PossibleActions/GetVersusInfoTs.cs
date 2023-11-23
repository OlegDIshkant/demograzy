using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Threading.Tasks;
using Demograzy.BusinessLogic.DataAccess;
using Demograzy.BusinessLogic.PossibleActions;

namespace Demograzy.BusinessLogic
{
    internal class GetVersusInfoTs : TransactionScript<VersusInfo?>
    {
        private readonly int _versusId;


        public GetVersusInfoTs(int versusId, ITransactionMeans transactionMeans) : base(transactionMeans)
        {
            _versusId = versusId;
        }


        protected override async Task<Result> OnRunAsync()
        {
            return Result.DependsIfNull(await VersesGateway.GetVersusInfoAsync(_versusId));
        }
        
    }
}