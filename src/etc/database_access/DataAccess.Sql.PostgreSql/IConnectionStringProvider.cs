using System;
using System.Security;


namespace DataAccess.Sql.PostgreSql
{
    public interface IConnectionStringProvider 
    {
        SecureString ConnectionString { get; }
    }
}