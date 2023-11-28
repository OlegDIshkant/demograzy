

using Npgsql;

namespace DataAccess.Sql.PostgreSql
{
    internal class TransactionBuilder : ITransactionBuilder
    {
        private Func<NpgsqlConnection> _GetConnection;

        private NpgsqlTransaction _currentTransaction;


        public TransactionBuilder(Func<NpgsqlConnection> GetConnection)
        {
            _GetConnection = GetConnection;
        }


        public ISqlCommand<object> Begin()
        {
            return new GenericCommand<object>(
                async () =>
                {
                    if (_currentTransaction != null)
                    {
                        throw new InvalidOperationException("Some transaction has already been started.");
                    }
                    _currentTransaction = await _GetConnection().BeginTransactionAsync(System.Data.IsolationLevel.Snapshot);
                    return Task<object>.CompletedTask;
                } 
            );
        }


        public ISqlCommand<object> Commit()
        {
            return new GenericCommand<object>(
                () => EndCurrentTransaction(t => t.CommitAsync())
            );
        }


        public ISqlCommand<object> Rollback()
        {
            return new GenericCommand<object>(
                () => EndCurrentTransaction(t => t.RollbackAsync())
            );
        }


        private async Task<object> EndCurrentTransaction(Func<NpgsqlTransaction, Task> completeAction)
        {
            if (_currentTransaction == null)
            {
                throw new InvalidOperationException("Cannot end transaction since none been started.");
            }

            await completeAction(_currentTransaction);

            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;

            return Task<object>.CompletedTask;
        }

    }
}