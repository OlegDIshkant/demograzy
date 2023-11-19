using System.Threading.Tasks;
using Demograzy.BusinessLogic.DataAccess;
using Demograzy.BusinessLogic.PossibleActions;

namespace Demograzy.BusinessLogic
{
    internal class DeleteRoomTs : TransactionScript<bool>
    {
        private readonly int _roomId;


        public DeleteRoomTs(int roomId, ITransactionMeans transactionMeans) : base(transactionMeans)
        {
            _roomId = roomId;
        }


        protected override async Task<Result> OnRunAsync()
        {
            return Result.DependsOn(await CommonRoutines.DeleteRoomAndForgetItsMembers(_roomId));
        }
        
    }
}