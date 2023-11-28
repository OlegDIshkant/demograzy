using System;

namespace DataAccess.Sql
{
    public interface ISqlCommandBuilder : IDisposable
    {
        ITransactionBuilder Transactions { get; }
        ILockCommandsBuilder LockCommands { get; }
        IQueryBuilder Queries { get; }
        INonQueryBuilder NonQueries { get; }
    }
}