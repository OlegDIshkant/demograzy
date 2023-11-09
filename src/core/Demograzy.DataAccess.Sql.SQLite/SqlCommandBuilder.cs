using Common;
using Microsoft.Data.Sqlite;

namespace Demograzy.DataAccess.Sql.SQLite
{
    internal class SqlCommandBuilder : DisposableObject, ISqlCommandBuilder
    {

        private SqliteConnection _connection;

        public ITransactionBuilder Transactions { get; private set; }


        public static async Task<SqlCommandBuilder> CreateAsync(string connectionString)
        {
            var connection = new SqliteConnection(connectionString);
            await connection.OpenAsync();
            return new SqlCommandBuilder(connection);
        }


        private SqlCommandBuilder(SqliteConnection connection)
        {
            _connection = connection;
            Transactions = new TransactionBuilder(() => _connection);
        }


        protected override void OnDispose()
        {
            base.OnDispose();
            _connection.DisposeAsync();
        }


    }
}