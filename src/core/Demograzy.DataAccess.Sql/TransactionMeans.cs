#nullable disable

using System;
using System.Threading.Tasks;
using Common;
using DataAccess.Sql;
using Demograzy.BusinessLogic.DataAccess;


namespace Demograzy.DataAccess.Sql
{
    public class TransactionMeans : DisposableObject, ITransactionMeans
    {
        private ISqlCommandBuilder _commandBuilder;

        public IClientsGateway ClientsGateway { get; private set; }

        public IRoomsGateway RoomsGateway { get; private set; }

        internal static async Task<TransactionMeans> NewAsync(ISqlCommandBuilder commandBuilder)
        {
            var instance = new TransactionMeans(commandBuilder);
            await instance.StartAsync();
            return instance;
        }


        private TransactionMeans(ISqlCommandBuilder commandBuilder)
        {
            _commandBuilder = commandBuilder;

            ClientsGateway = new ClientsGateway(
                () => _commandBuilder.Queries,
                () => _commandBuilder.NonQueries);   
            RoomsGateway = new RoomsGateway(
                () => _commandBuilder.Queries,
                () => _commandBuilder.NonQueries);
        }


        private Task StartAsync()
        {
            return _commandBuilder.Transactions.Begin().ExecuteAsync();
        }


        public async Task FinishAsync(bool toCommit)
        {
            ExceptionIfDisposed();
            await _commandBuilder.Transactions.Commit().ExecuteAsync();
            this.Dispose();
        }
        

        protected override void OnDispose()
        {
            base.OnDispose();

            _commandBuilder.Dispose();

            if (_commandBuilder != null)
            {
                _commandBuilder.Transactions.Rollback().ExecuteAsync();
                _commandBuilder = null;
            }
        }
    }
}