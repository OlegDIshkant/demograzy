using System;
using System.Security;


namespace Demograzy.DataAccess.Sql
{
    public interface IConnectionStringProvider 
    {
        SecureString ConnectionString { get; }
    }
}