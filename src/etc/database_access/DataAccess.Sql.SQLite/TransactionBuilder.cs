using System;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace DataAccess.Sql.SQLite
{
    internal class TransactionBuilder : ITransactionBuilder
    {
        private readonly Func<SqliteConnection> _PeekConnection;

        private DbTransaction _currentTransaction;


        public TransactionBuilder(Func<SqliteConnection> PeekConnection)
        {
            _PeekConnection = PeekConnection;
        }



        public ISqlCommand<object> Begin()
        {
            return new GenericCommand<object>(
                async () =>
                {
                    if (_currentTransaction != null)
                    {
                        throw new InvalidOperationException("Some transaction has already started.");
                    }
                    _currentTransaction = await _PeekConnection().BeginTransactionAsync();
                    return Task<object>.CompletedTask;
                } 
            );
        }


        public ISqlCommand<object> Commit()
        {
            return EndTransactionCommand((t) => _currentTransaction.CommitAsync());
        }


        public ISqlCommand<object> Rollback()
        {
            return EndTransactionCommand((t) => _currentTransaction.RollbackAsync());
        }


        private ISqlCommand<object> EndTransactionCommand(Func<DbTransaction, Task> EndAction)
        {
            return new GenericCommand<object>(
                async () =>
                {
                    if (_currentTransaction == null)
                    {
                        throw new InvalidOperationException("There is no started transaction.");
                    }
                    await EndAction(_currentTransaction);
                    await _currentTransaction.DisposeAsync();
                    _currentTransaction = null;
                    return Task<object>.CompletedTask;
                } 
            );
        }
    }
}