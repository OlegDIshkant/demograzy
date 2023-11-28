
namespace DataAccess.Sql.PostgreSql
{
    public sealed class SqlCommandBuilderFactory : ISqlCommandBuilderFactory
    {

        private readonly IConnectionStringProvider _connectionStringProvider;


        public SqlCommandBuilderFactory(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }


        public async Task<ISqlCommandBuilder> CreateAsync()
        {
            return await SqlCommandBuilder.CreateAsync(_connectionStringProvider);
        }
    }
}