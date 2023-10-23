using Npgsql;
using System;

namespace Demograzy.DataAccess
{
    internal static class DbConnection
    {
        private static NpgsqlDataSource _dataSource;


        public static NpgsqlConnection GetNewConnection(IConnectionStringProvider connStringProvider)
        {
            if (_dataSource == null)
            {
                _dataSource = NpgsqlDataSource.Create(connStringProvider.ConnectionString);
            }

            return _dataSource.CreateConnection();
        }

    }   

}

