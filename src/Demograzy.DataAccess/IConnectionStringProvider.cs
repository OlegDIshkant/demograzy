using System;
using System.Security;


namespace Demograzy.DataAccess
{
    public interface IConnectionStringProvider 
    {
        SecureString ConnectionString { get; }
    }
}