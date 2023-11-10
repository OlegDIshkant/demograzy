
using System.Text;
using Microsoft.Data.Sqlite;

namespace Demograzy.DataAccess.Sql.SQLite
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
                    command.CommandText = BuildSelectStatement(selectOptions);                         
                    return new QueryResult(command, await command.ExecuteReaderAsync());
                }
            );
        }


        private string BuildSelectStatement(SelectOptions selectOptions)
        {
            var b = new StringBuilder();

            b.Append("SELECT ");

            if ((selectOptions.Select?.Count ?? 0) <= 0)
            {
                throw new ArgumentException("No columns presented in SELECT statement.");
            }

            foreach(var columnOption in selectOptions.Select)
            {
                if (columnOption is Count)
                {
                    b.Append("count(*), ");
                }
                else if (columnOption is LastInsertedRowId)
                {
                    b.Append("last_insert_rowid(), ");
                }
                else 
                {
                    throw new NotSupportedException($"Column option with type '{columnOption.GetType()}' is not supported yet.");
                }
            }
            b.Remove(b.Length - 2, 2);

            if (selectOptions.From != null)
            {
                b.Append(" FROM ");
                b.Append(selectOptions.From);
                b.Append(' ');
            }

            return b.ToString();
        }
    }
}