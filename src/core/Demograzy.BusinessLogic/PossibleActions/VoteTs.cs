using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demograzy.BusinessLogic.DataAccess;
using Demograzy.BusinessLogic.PossibleActions;

namespace Demograzy.BusinessLogic
{
    internal class VoteTs : TransactionScript<bool>
    {
        private enum Winner { FIRST, SECOND }

        private readonly int _versusId; 
        private readonly int _clientId; 
        private readonly bool _votedForFirst;




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

            return await OnVoted();
        }


        private async Task<bool> OnVoted()
        {
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

            return await OnVersusCompleted();
        }


        private async Task<bool> OnVersusCompleted()
        {
            StartVersusArgs args = default;
            if (await ShouldStartNewVersus(a => args = a))
            {
                return await StartNewVersus(args);
            }
            
            return true;
        }


        private async Task<bool> ShouldStartNewVersus(
            Action<StartVersusArgs> OnStartVersusArgsKnown)
        {
            var otherVersusId = await FindOtherUnpairedCompletedVersus();
            if (otherVersusId.HasValue)
            {
                OnStartVersusArgsKnown(
                    new StartVersusArgs()
                    {
                        firstCompletedVersusId = _versusId,
                        firstVersusWinnerId = (await VersesGateway.GetVersusWinnerAsync(_versusId)).Value,
                        secondCompletedVersusId = otherVersusId.Value,
                        secondVersusWinnerId = (await VersesGateway.GetVersusWinnerAsync(otherVersusId.Value)).Value
                    });
                return true;
            }
            return false;
        }


        private async Task<int?> FindOtherUnpairedCompletedVersus()
        {
            var roomId = (await VersesGateway.GetVersusInfoAsync(_versusId)).Value.roomId;
            var unpairedVerses = await VersesGateway.GetCompletedVersesWithoutFollowUpAsync(roomId);
            var otherVersusIndex = unpairedVerses.FindIndex(vId => vId != _versusId);
            if (otherVersusIndex == -1)
            {
                return null;
            }
            return unpairedVerses[otherVersusIndex];
        }


        private async Task<bool> StartNewVersus(StartVersusArgs args)
        {
            var roomId = (await VersesGateway.GetVersusInfoAsync(_versusId)).Value.roomId;
            var newVersusId = await VersesGateway.AddVersusAsync(roomId, args.firstVersusWinnerId, args.secondVersusWinnerId);
            var versusCreationFailed = !newVersusId.HasValue;

            if (versusCreationFailed) return false;

            return await MakeCompletedVersesReferenceNewOne(
                completedVersusIds: new List<int>() { args.firstCompletedVersusId, args.secondCompletedVersusId },
                newVersusId: newVersusId.Value
            );
        }


        private async Task<bool> MakeCompletedVersesReferenceNewOne(List<int> completedVersusIds, int newVersusId)
        {
            foreach(var versusId in completedVersusIds)
            {
                var referencingFailed = !await VersesGateway.SetVersusFollowUpAsync(versusId, newVersusId);
                if (referencingFailed) return false;
            }
            return true;
        }


        private struct StartVersusArgs
        {
            public int firstCompletedVersusId;
            public int firstVersusWinnerId;
            public int secondCompletedVersusId;
            public int secondVersusWinnerId;
        }
    }
}