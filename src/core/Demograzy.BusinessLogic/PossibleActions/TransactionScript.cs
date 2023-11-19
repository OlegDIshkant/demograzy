#nullable disable

using System;
using System.Threading.Tasks;
using Demograzy.BusinessLogic.DataAccess;

namespace Demograzy.BusinessLogic.PossibleActions
{
    public abstract class TransactionScript<R> : Common.DisposableObject
    {
        private ITransactionMeans _transactionMeans;

        protected IClientsGateway ClientGateway => _transactionMeans.ClientsGateway; 
        protected IRoomsGateway RoomGateway => _transactionMeans.RoomsGateway; 
        protected IMembershipGateway MembershipGateway => _transactionMeans.MembershipGateway; 
        protected ICandidatesGateway CandidateGateway => _transactionMeans.CandidatesGateway; 
        protected IWinnersGateway WinnersGateway => _transactionMeans.WinnersGateway; 
        public IVersesGateway VersesGateway => _transactionMeans.VersesGateway;
        public IVotesGateway VotesGateway => _transactionMeans.VotesGateway;


        public bool Started { get; private set; } = false;


        public TransactionScript(ITransactionMeans transactionMeans)
        {
            _transactionMeans = transactionMeans;
        }


        public async Task<R> RunAsync()
        {
            ExceptionIfDisposed();

            if (Started)
            {
                throw new InvalidOperationException($"Can't run script {this} since it has already been started.");
            }
            Started = true;
            
            Result result = default;

            //try
            //{
                result = await OnRunAsync();
                var commitAllowed = result.Status == Result.Statuses.SUCCESS;
                await _transactionMeans.FinishAsync(toCommitInsteadOfRollback: commitAllowed);
                return result.Value;
            //}
            //catch (Exception e)
            //{
            //    await _transactionMeans.FinishAsync(toCommitInsteadOfRollback: false);
            //    throw e.InnerException;
            //}
        } 


        protected abstract Task<Result> OnRunAsync();


        protected override void OnDispose()
        {
            base.OnDispose();

            _transactionMeans.Dispose();
            _transactionMeans = null;
        }


        public struct Result
        {
            public enum Statuses { SUCCESS, FAIL } 
            public R Value { get; set; } 
            public Statuses Status { get; set; }

            public static Result Success(R value)
            {
                return new Result()
                {
                    Value = value,
                    Status = Statuses.SUCCESS
                };
            }

            public static Result Fail(R value)
            {
                return new Result()
                {
                    Value = value,
                    Status = Statuses.FAIL
                };
            }

            public static Result DependsOn(bool value)
            {
                return new Result()
                {
                    Value = (R)(object)value,
                    Status = value ? Statuses.SUCCESS : Statuses.FAIL
                };
            }

            public static Result DependsIfNull(R value)
            {
                return new Result()
                {
                    Value = value,
                    Status = value == null ? Statuses.FAIL : Statuses.SUCCESS
                };
            }


        }
    }
}