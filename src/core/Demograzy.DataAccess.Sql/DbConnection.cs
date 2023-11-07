using Npgsql;
using System;

namespace Demograzy.DataAccess.Sql
{
    internal static class DbConnection
    {
        private static NpgsqlDataSource _dataSource;


        public static NpgsqlConnection GetNewConnection(IConnectionStringProvider connStringProvider)
        {
            if (_dataSource == null)
            {
                using (var connString = connStringProvider.ConnectionString)
                {
                    _dataSource = NpgsqlDataSource.Create(connString.ToString());
                }
            }

            return _dataSource.CreateConnection();
        }

    }   

}

