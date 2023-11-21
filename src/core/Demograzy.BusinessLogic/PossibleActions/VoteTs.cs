using System;
using System.Threading.Tasks;
using Demograzy.BusinessLogic.DataAccess;
using Demograzy.BusinessLogic.PossibleActions;

namespace Demograzy.BusinessLogic
{
    internal class VoteTs : TransactionScript<bool>
    {
        private static int _versusId; 
        private static int _clientId; 
        private static bool _votedForFirst;


        public VoteTs(int versusId, int clientId, bool votedForFirst, ITransactionMeans transactionMeans) : 
            base(transactionMeans)
        {
            _versusId = versusId;
            _clientId = clientId;
            _votedForFirst = votedForFirst;
        }


        protected override async Task<Result> OnRunAsync()
        {
            if (await MayVote())
            {
                return Result.DependsOn(await Vote());
            }
            else
            {
                return Result.Fail(false);
            }
        }


        private async Task<bool> MayVote()
        {
            var versusInfo = (await VersesGateway.GetVersusInfoAsync(_versusId)).Value;
            var memberIds = await MembershipGateway.GetRoomMembersAsync(versusInfo.roomId);
            var voterIsNotMember = !memberIds.Contains(_clientId);
            
            if (voterIsNotMember) return false; 

            return true;
        }


        private async Task<bool> Vote()
        {
            throw new NotImplementedException();
        }


        
    }
}