using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Demograzy.BusinessLogic.DataAccess;


namespace Demograzy.BusinessLogic.PossibleActions
{
    internal class GetActiveVersesTs : TransactionScript<List<int>>
    {
        private readonly int _roomId;
        private readonly int _clientId;


        public GetActiveVersesTs(int roomId, int clientId, ITransactionMeans transactionMeans) : base(transactionMeans)
        {
            _roomId = roomId;
            _clientId = clientId;
        }


        protected override async Task<Result> OnRunAsync()
        {
            if (await MayGetActiveVerses())
            {
                return Result.DependsIfNull(await GetActiveVerses());
            }
            else
            {
                return Result.Fail(null);
            }
        }


        private async Task<bool> MayGetActiveVerses()
        {
            return (await RoomGateway.GetRoomInfoAsync(_roomId)).Value.votingStarted;
        }


        private async Task<List<int>> GetActiveVerses()
        {
            var result = new List<int>();
            foreach (var versusId in await VersesGateway.GetUnfinishedVersesAsync(_roomId))
            {
                if (!await VotesGateway.CheckIfClientVotedAsync(versusId, _clientId))
                {
                    result.Add(versusId);
                }
            }
            return result;
        }

        
    }
}