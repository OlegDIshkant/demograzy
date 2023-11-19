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


        protected override async Task<Result> OnRunAsync()
        {
            var ownerExists = await ClientGateway.CheckClientExistsAsync(_ownerId);
            var limitReached = (await RoomGateway.GetOwnedRoomsAsync(_ownerId)).Count >= Limits.MAX_OWNED_ROOMS; 

            if (!ownerExists || limitReached)
            {
                return Result.Fail(null);
            }

            var roomId = await RoomGateway.AddRoomAsync(_ownerId, _roomTitle, _passphrase);
            
            if (roomId.HasValue &&
                await MembershipGateway.AddRoomMemberAsync(roomId.Value, _ownerId))
            {
                return Result.Success(roomId);
            }
            return Result.Fail(null);
        }
        
    }
}