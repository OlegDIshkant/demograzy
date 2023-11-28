using Npgsql;
using System;
using System.Runtime.InteropServices;

namespace DataAccess.Sql.PostgreSql
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
                    _dataSource = NpgsqlDataSource.Create(
                        Marshal.PtrToStringBSTR(Marshal.SecureStringToBSTR(connString)));
                }
            }

            return _dataSource.CreateConnection();
        }

    }   

}

