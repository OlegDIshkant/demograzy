
using Microsoft.Data.Sqlite;

namespace Demograzy.DataAccess.Sql.SQLite
{
    public class SqlCommandBuilderFactory : ISqlCommandBuilderFactory
    {
        private readonly string _connectionString;


        public static SqlCommandBuilderFactory Create(string pathToTestDb)
        {
            return new SqlCommandBuilderFactory(
                new SqliteConnectionStringBuilder()
                {
                    DataSource = pathToTestDb
                }.ToString()
            );
        }



        private SqlCommandBuilderFactory(string connectionString)
        {
            _connectionString = connectionString;
        }


        public async Task<ISqlCommandBuilder> CreateAsync()
        {
            return await SqlCommandBuilder.CreateAsync(_connectionString);
        }
    }
}