using Microsoft.Data.Sqlite;

namespace DataAccess.Sql.SQLite
{
    internal class QueryBuilder : IQueryBuilder
    {
        private readonly Func<SqliteConnection> _PeekConnection;


        public QueryBuilder(Func<SqliteConnection> PeekConnection)
        {
            _PeekConnection = PeekConnection;
        }


        public ISqlCommand<IQueryResult> Create(SelectOptions selectOptions)
        {
            return new GenericCommand<IQueryResult>(
                async () =>
                {
                    var command = _PeekConnection().CreateCommand();
                    command.CommandText = SelectStatementBuilder.Build(selectOptions, out var parameters);
                    foreach (var p in parameters)
                    {
                        command.Parameters.AddWithValue(p.Key, p.Value ?? DBNull.Value);
                    }                         
                    return new QueryResult(command, await command.ExecuteReaderAsync());
                }
            );
        }
    }
}