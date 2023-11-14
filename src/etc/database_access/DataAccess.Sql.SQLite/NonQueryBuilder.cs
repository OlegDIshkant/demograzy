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
            return GenerateCommand(
                () => (InsertStatementBuilder.Build(insertOptions, out var parameters), parameters)
            );
        }


        public ISqlCommand<bool> Create(DeleteOptions deleteOptions)
        {
            return GenerateCommand(
                () => (DeleStatementBuilder.Build(deleteOptions, out var parameters), parameters)
            );
        }


        private ISqlCommand<bool> GenerateCommand(Func<(string text, Dictionary<string, object> parameters)> PrepareCommand)
        {
            return new GenericCommand<bool> (
                async () =>
                {
                    using (var command = _PeekConnection().CreateCommand())
                    {
                        var preparedCommand = PrepareCommand();
                        command.CommandText = preparedCommand.text;
                        foreach (var item in preparedCommand.parameters)
                        {
                            command.Parameters.AddWithValue(item.Key, item.Value);
                        }
                        var changedAmount = await command.ExecuteNonQueryAsync();
                        return  changedAmount > 0;
                    }
                }
            );
        }

    }
}