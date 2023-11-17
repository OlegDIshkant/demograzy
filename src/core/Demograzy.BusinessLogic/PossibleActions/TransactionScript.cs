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
        protected ICandidatesGateway CandidateGateway => _transactionMeans.CandidatesGateway; 

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
            
            R result = default;
            try
            {
                result = await OnRunAsync();
                await _transactionMeans.FinishAsync(toCommit: true);
                return result;
            }
            catch (Exception e)
            {
                await _transactionMeans.FinishAsync(toCommit: false);
                throw e;
            }
        } 


        protected abstract Task<R> OnRunAsync();


        protected override void OnDispose()
        {
            base.OnDispose();

            _transactionMeans.Dispose();
            _transactionMeans = null;
        }
    }
}