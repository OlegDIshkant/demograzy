using System;


namespace Demograzy.DataAccess
{
    public interface IConnectionStringProvider 
    {
        string ConnectionString { get; }
    }
}