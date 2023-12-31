using DataAccess.Sql.Common;
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


        public ISqlCommand<ICollection<R>> Create<R>(SelectOptions selectOptions, Func<IRow, R> ReadRow)
        {
            return new GenericCommand<ICollection<R>>(() => ExecuteQuery(selectOptions, ReadRow));
        }



        private Task<ICollection<R>> ExecuteQuery<R>(SelectOptions selectOptions, Func<IRow, R> ReadRow)
        {
            using (var command = _PeekConnection().CreateCommand())
            {
                SetUpCommand(command, selectOptions);
                return ExecuteAndReadResult(command, ReadRow);
            }
        }


        private void SetUpCommand(SqliteCommand command, SelectOptions selectOptions)
        {
            var buildSettings = new StatementBuildSettings();
            command.CommandText = SelectStatementBuilder.Build(selectOptions, out var parameters, buildSettings); 
            foreach (var p in parameters)
            {
                command.Parameters.AddWithValue(p.Key, p.Value ?? DBNull.Value);
            }          
        }


        private async Task<ICollection<R>> ExecuteAndReadResult<R> (SqliteCommand command, Func<IRow, R> ReadRow)
        {
            using (var reader = await command.ExecuteReaderAsync())
            {
                var result = new List<R>();
                while(await reader.ReadAsync())
                {
                    result.Add(ReadRow(new Row() { reader = reader }));
                }
                return result;                    
            }  
        }


        private struct Row : IRow
        {
            public SqliteDataReader reader;
            public bool GetBool(int columnIndex) => reader.GetBoolean(columnIndex);
            public int GetInt(int columnIndex) => reader.GetInt32(columnIndex);
            public string GetString(int columnIndex) => reader.GetString(columnIndex);
            public bool IsNull(int columnIndex) => reader.IsDBNull(columnIndex);
        }
    }
}