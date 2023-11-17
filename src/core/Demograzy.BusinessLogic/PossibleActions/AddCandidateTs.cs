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


        protected override Task<int?> OnRunAsync()
        {
            return base.CandidateGateway.AddCandidateAsync(_roomId, _candidateName);
        }
        
    }
}