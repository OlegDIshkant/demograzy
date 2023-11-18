using System.Collections.Generic;
using System.Threading.Tasks;
using Demograzy.BusinessLogic.DataAccess;
using Demograzy.BusinessLogic.PossibleActions;

namespace Demograzy.BusinessLogic
{
    internal class JoinRoomTs : TransactionScript<bool>
    {
        private readonly int _roomId;
        private readonly int _clientId;


        public JoinRoomTs(int roomId, int clientId, ITransactionMeans transactionMeans) : base(transactionMeans)
        {
            _roomId = roomId;
            _clientId = clientId;
        }


        protected override Task<bool> OnRunAsync()
        {
            return MembershipGateway.AddRoomMemberAsync(_roomId, _clientId);
        }
        
    }
}