using System;
using System.Text;
using Microsoft.Data.Sqlite;

namespace DataAccess.Sql.SQLite
{
    internal class NonQueryBuilder : INonQueryBuilder
    {
        private readonly Func<SqliteConnection> _PeekConnection;

        
        public NonQueryBuilder(Func<SqliteConnection> PeekConnection)
        {
            _PeekConnection = PeekConnection;
        }


        public ISqlCommand<bool> Create(InsertOptions insertOptions)
        {
            return new GenericCommand<bool> (
                async () =>
                {
                    using (var command = _PeekConnection().CreateCommand())
                    {
                        command.CommandText = BuildInsertStatement(insertOptions);
                        InsertParameters(command, insertOptions);
                        var changedAmount = await command.ExecuteNonQueryAsync();
                        return  changedAmount > 0;
                    }
                }
            );
        }


        private string BuildInsertStatement(InsertOptions insertOptions)
        {
            var b = new StringBuilder();

            b.Append($"INSERT INTO {insertOptions.Into} (");
            foreach (var (itemName, _) in insertOptions.Values)
            {
                b.Append($"{itemName}, ");
            }
            b.Remove(b.Length - 2, 2);

            b.Append(") VALUES (");
            foreach (var (itemName, _) in insertOptions.Values)
            {
                b.Append($"${itemName}, ");
            }
            b.Remove(b.Length - 2, 2);
            b.Append(")");

            return b.ToString();
        }


        private void InsertParameters(SqliteCommand command, InsertOptions insertOptions)
        {
            foreach (var (itemName, itemValue) in insertOptions.Values)
            {
                command.Parameters.AddWithValue($"${itemName}", itemValue);
            }
        }
    }
}