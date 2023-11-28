using System;

namespace DataAccess.Sql
{
    public interface ILockCommandsBuilder
    {
        ISqlCommand<bool> Create(string tableToLock);
    }
}