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

        private bool _finished = false;

        private ISqlCommandBuilder _commandBuilder;

        public IClientsGateway ClientsGateway { get; private set; }
        public IRoomsGateway RoomsGateway { get; private set; }
        public ICandidatesGateway CandidatesGateway { get; private set; }
        public IMembershipGateway MembershipGateway { get; private set; }
        public IWinnersGateway WinnersGateway { get; private set; }
        public IVersesGateway VersesGateway { get; private set; }
        public IVotesGateway VotesGateway { get; private set; }


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
                () => _commandBuilder.NonQueries,
                () => _commandBuilder.LockCommands);   
            RoomsGateway = new RoomsGateway(
                () => _commandBuilder.Queries,
                () => _commandBuilder.NonQueries,
                () => _commandBuilder.LockCommands);
            MembershipGateway = new MembershipGateway(
                () => _commandBuilder.Queries,
                () => _commandBuilder.NonQueries,
                () => _commandBuilder.LockCommands);
            CandidatesGateway = new CandidatesGateway(
                () => _commandBuilder.Queries,
                () => _commandBuilder.NonQueries,
                () => _commandBuilder.LockCommands);
            WinnersGateway = new WinnersGateway(
                () => _commandBuilder.Queries,
                () => _commandBuilder.NonQueries,
                () => _commandBuilder.LockCommands);
            VersesGateway = new VersesGateway(
                () => _commandBuilder.Queries,
                () => _commandBuilder.NonQueries,
                () => _commandBuilder.LockCommands);
            VotesGateway = new VotesGateway(
                () => _commandBuilder.Queries,
                () => _commandBuilder.NonQueries,
                () => _commandBuilder.LockCommands);
        }


        private Task StartAsync()
        {
            return _commandBuilder.Transactions.Begin().ExecuteAsync();
        }


        public async Task FinishAsync(bool toCommit)
        {
            ExceptionIfDisposed();
            await _commandBuilder.Transactions.Commit().ExecuteAsync();
            _finished = true;
            this.Dispose();
        }
        

        protected override void OnDispose()
        {
            base.OnDispose();

            if (!_finished)
            {
                _commandBuilder.Transactions.Rollback().ExecuteAsync();
            }
            _commandBuilder.Dispose();
            _commandBuilder = null;
        }
    }
}