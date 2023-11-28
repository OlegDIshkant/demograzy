using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Demograzy.BusinessLogic.DataAccess;


namespace Demograzy.BusinessLogic.PossibleActions
{
    internal class GetJoinedRoomsTs : TransactionScript<ICollection<int>>
    {
        private readonly int _clientId;

        public GetJoinedRoomsTs(int clientId, ITransactionMeans transactionMeans) : base(transactionMeans)
        {
            _clientId = clientId;
        }


        protected override async Task<Result> OnRunAsync()
        {
            if (await ClientGateway.CheckClientExistsAsync(_clientId))
            {
                return Result.Success(await MembershipGateway.GetJoinedRoomsAsync(_clientId));
            }
            {
                return Result.Fail(null);
            }
        }
        
    }
}