using DataAccess.Sql.Common;
using Npgsql;

namespace DataAccess.Sql.PostgreSql
{
    internal class QueryBuilder : IQueryBuilder
    {
        private readonly Func<NpgsqlConnection> _PeekConnection;


        public QueryBuilder(Func<NpgsqlConnection> PeekConnection)
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


        private void SetUpCommand(NpgsqlCommand command, SelectOptions selectOptions)
        {
            command.CommandText = SelectStatementBuilder.Build(selectOptions, out var parameters, new StatementBuildSettings()); 
            foreach (var parameter in parameters)
            {
                command.Parameters.Add(new NpgsqlParameter(parameter.Key, parameter.Value ?? DBNull.Value));
            }
        }


        private async Task<ICollection<R>> ExecuteAndReadResult<R> (NpgsqlCommand command, Func<IRow, R> ReadRow)
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
            public NpgsqlDataReader reader;
            public bool GetBool(int columnIndex) => reader.GetBoolean(columnIndex);
            public int GetInt(int columnIndex) => reader.GetInt32(columnIndex);
            public string GetString(int columnIndex) => reader.GetString(columnIndex);
            public bool IsNull(int columnIndex) => reader.IsDBNull(columnIndex);
        }


    }
}