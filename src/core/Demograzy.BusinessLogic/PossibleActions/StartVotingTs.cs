using System.Collections.Generic;
using System.Threading.Tasks;
using Demograzy.BusinessLogic.DataAccess;
using Demograzy.BusinessLogic.PossibleActions;

namespace Demograzy.BusinessLogic
{
    internal class StartVotingTs : TransactionScript<bool>
    {
        private readonly int _roomId;


        public StartVotingTs(int roomId, ITransactionMeans transactionMeans) : base(transactionMeans)
        {
            _roomId = roomId;
        }


        protected override async Task<Result> OnRunAsync()
        {
            return Result.DependsOn(await RoomGateway.StartVotingAsync(_roomId));
        }
        
    }
}