

using Common;
using Npgsql;

namespace DataAccess.Sql.PostgreSql
{
    internal sealed class SqlCommandBuilder : DisposableObject, ISqlCommandBuilder
    {
        private NpgsqlConnection _connection;

        public ITransactionBuilder Transactions { get; private set; }
        public IQueryBuilder Queries { get; private set; }
        public INonQueryBuilder NonQueries { get; private set; }
        public ILockCommandsBuilder LockCommands { get; private set; }


        public static async Task<SqlCommandBuilder> CreateAsync(IConnectionStringProvider connectionStringProvider)
        {
            var connection =  DbConnection.GetNewConnection(connectionStringProvider);
            await connection.OpenAsync();
            return new SqlCommandBuilder(connection);

        }


        private SqlCommandBuilder(NpgsqlConnection connection)
        {
            _connection = connection;
            Transactions = new TransactionBuilder(() => _connection);
            Queries = new QueryBuilder(() => _connection);
            NonQueries = new NonQueryBuilder(() => _connection);
            LockCommands = new LockCommandsBuilder(() => _connection);
        }


        protected override void OnDispose()
        {
            base.OnDispose();

            _connection.Dispose();
            _connection = null;
        }

    }
}