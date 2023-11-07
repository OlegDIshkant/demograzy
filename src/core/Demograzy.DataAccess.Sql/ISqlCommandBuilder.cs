using System;

namespace Demograzy.DataAccess.Sql
{
    public interface ISqlCommandBuilder : IDisposable
    {
        ITransactionBuilder Transactions { get; }
    }
}