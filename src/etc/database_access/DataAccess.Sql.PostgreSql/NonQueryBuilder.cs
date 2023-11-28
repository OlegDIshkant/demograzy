using DataAccess.Sql.Common;
using Npgsql;

namespace DataAccess.Sql.PostgreSql
{
    internal class NonQueryBuilder : INonQueryBuilder
    {

        private Func<NpgsqlConnection> _PeekConnection;


        public NonQueryBuilder(Func<NpgsqlConnection> PeekConnection)
        {
            _PeekConnection = PeekConnection;
        }


        public ISqlCommand<int> Create(InsertOptions insertOptions)
        {
            return GenerateCommand(
                () => (InsertStatementBuilder.Build(insertOptions, out var parameters, new StatementBuildSettings()), parameters)
            );
        }


        public ISqlCommand<int> Create(DeleteOptions deleteOptions)
        {
            return GenerateCommand(
                () => (DeleStatementBuilder.Build(deleteOptions, out var parameters, new StatementBuildSettings()), parameters)
            );
        }

        public ISqlCommand<int> Create(UpdateOptions updateOptions)
        {
            return GenerateCommand(
                () => (UpdateStatementBuilder.Build(updateOptions, out var parameters, new StatementBuildSettings()), parameters)
            );
        }


        private ISqlCommand<int> GenerateCommand(Func<(string text, Dictionary<string, object> parameters)> PrepareCommand)
        {
            return new GenericCommand<int> (
                async () =>
                {
                    using (var command = _PeekConnection().CreateCommand())
                    {
                        var preparedCommand = PrepareCommand();
                        command.CommandText = preparedCommand.text;
                        foreach (var item in preparedCommand.parameters)
                        {
                            command.Parameters.AddWithValue(item.Key, item.Value ?? DBNull.Value);
                        }
                        var changedAmount = await command.ExecuteNonQueryAsync();
                        return  changedAmount;
                    }
                }
            );
        }


    }
}