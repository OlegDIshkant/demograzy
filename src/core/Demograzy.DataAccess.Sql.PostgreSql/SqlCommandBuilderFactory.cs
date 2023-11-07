
namespace Demograzy.DataAccess.Sql.PostgreSql
{
    public sealed class SqlCommandBuilderFactory : ISqlCommandBuilderFactory
    {

        private readonly IConnectionStringProvider _connectionStringProvider;


        public SqlCommandBuilderFactory(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }


        public Task<ISqlCommandBuilder> CreateAsync()
        {
            return Task.FromResult((ISqlCommandBuilder)new SqlCommandBuilder(_connectionStringProvider));
        }
    }
}