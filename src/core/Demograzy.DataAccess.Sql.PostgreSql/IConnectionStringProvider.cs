using System;
using System.Security;


namespace Demograzy.DataAccess.Sql.PostgreSql
{
    public interface IConnectionStringProvider 
    {
        SecureString ConnectionString { get; }
    }
}