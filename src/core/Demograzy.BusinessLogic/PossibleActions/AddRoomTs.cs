using System.Threading.Tasks;
using Demograzy.BusinessLogic.DataAccess;
using Demograzy.BusinessLogic.PossibleActions;

namespace Demograzy.BusinessLogic
{
    internal class AddRoomTs : TransactionScript<int?>
    {
        private readonly int _ownerId;
        private readonly string _roomTitle;
        private readonly string _passphrase;


        public AddRoomTs(int ownerId, string roomTitle, string passphrase, ITransactionMeans transactionMeans) : base(transactionMeans)
        {
            _ownerId = ownerId;
            _roomTitle = roomTitle;
            _passphrase = passphrase;
        }


        protected override async Task<int?> OnRunAsync()
        {
            var roomId = await RoomGateway.AddRoomAsync(_ownerId, _roomTitle, _passphrase);
            if (roomId != null)
            {
                var success = await MembershipGateway.AddRoomMemberAsync(roomId.Value, _ownerId);
            }
            return roomId;
        }
        
    }
}