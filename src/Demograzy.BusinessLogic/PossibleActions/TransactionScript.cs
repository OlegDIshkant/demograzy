#nullable disable

using System;
using System.Threading.Tasks;
using Demograzy.BusinessLogic.DataAccess;

namespace Demograzy.BusinessLogic.PossibleActions
{
    public abstract class TransactionScript : Common.DisposableObject
    {
        private ITransactionMeans _transactionMeans;

        public bool Started { get; private set; } = false;


        public TransactionScript(ITransactionMeans transactionMeans)
        {
            _transactionMeans = transactionMeans;
        }


        public Task RunAsync()
        {
            ExceptionIfDisposed();

            if (Started)
            {
                throw new InvalidOperationException($"Can't run script {this} since it has already been started.");
            }
            Started = true;

            return OnRunAsync();
        } 


        protected abstract Task OnRunAsync();


        protected override void OnDispose()
        {
            base.OnDispose();

            _transactionMeans.Dispose();
            _transactionMeans = null;
        }
    }
}