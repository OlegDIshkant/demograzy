#nullable disable

using Common;
using Npgsql;

namespace Demograzy.DataAccess.Sql.PostgreSql
{
    internal sealed class SqlCommandBuilder : DisposableObject, ISqlCommandBuilder
    {
        private NpgsqlConnection _connection;

        public ITransactionBuilder Transactions { get; private set; }

        public IQueryBuilder Queries => throw new NotImplementedException();
        public INonQueryBuilder NonQueries => throw new NotImplementedException();


        public SqlCommandBuilder(IConnectionStringProvider connectionStringProvider)
        {
            _connection = DbConnection.GetNewConnection(connectionStringProvider);

            Transactions = new TransactionBuilder(() => _connection);
        }


        protected override void OnDispose()
        {
            base.OnDispose();

            _connection.Dispose();
            _connection = null;
        }

    }
}