using System;
using System.Threading.Tasks;
using Demograzy.BusinessLogic.DataAccess;
using Demograzy.BusinessLogic.PossibleActions;

namespace Demograzy.BusinessLogic
{
    internal class VoteTs : TransactionScript<bool>
    {
        private enum Winner { FIRST, SECOND }

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
            var winnerAlreadyDefined = versusInfo.status != VersusInfo.Statuses.UNCOMPLETED;

            if (winnerAlreadyDefined) return false;

            var memberIds = await MembershipGateway.GetRoomMembersAsync(versusInfo.roomId);
            var voterIsNotMember = !memberIds.Contains(_clientId);
            
            if (voterIsNotMember) return false; 

            return true;
        }


        private async Task<bool> Vote()
        {
            var votingFailed = !await VotesGateway.AddVoteAsync(_versusId, _clientId, _votedForFirst);
            if (votingFailed) return false;

            var winner = await TryFigureOutWinner();
            if (winner.HasValue)
            {
                return await CompleteVersus(winner.Value);
            }

            return true;
        }


        private async Task<Winner?> TryFigureOutWinner()
        {
            var versusInfo = (await VersesGateway.GetVersusInfoAsync(_versusId)).Value;
            var maxVotes = (await MembershipGateway.GetRoomMembersAsync(versusInfo.roomId)).Count;
            var votesMajority = (int)(maxVotes / 2) + 1;
            var votesSoFar = await VotesGateway.GetVotesAmountAsync(_versusId);
            var fewVoted = votesSoFar < votesMajority;

            if (fewVoted) return null;

            var votesForFirst = await VotesGateway.GetVotesAmountForFirstCandidateAsync(_versusId);
            var firstWon = votesForFirst >= votesMajority;

            if (firstWon) return Winner.FIRST; 

            var votesForSecond = votesSoFar - votesForFirst;
            var secondWon = votesForSecond >= votesMajority;

            if (secondWon) return Winner.SECOND;

            return null;
        }


        private async Task<bool> CompleteVersus(Winner winner)
        {
            var setWinnerFailed = !await VersesGateway.SetVersusWinnerAsync(_versusId, winner == Winner.FIRST);
            if (setWinnerFailed) return false;

            return true;
        }


        
    }
}