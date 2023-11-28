using DataAccess.Sql.Common;
using Npgsql;

namespace DataAccess.Sql.PostgreSql
{
    internal class LockCommandsBuilder : ILockCommandsBuilder
    {
        private readonly Func<NpgsqlConnection> _PeekConnection;


        public LockCommandsBuilder(Func<NpgsqlConnection> PeekConnection)
        {
            _PeekConnection = PeekConnection;
        }


        public ISqlCommand<bool> Create(string tableToLock)
        {
            return new GenericCommand<bool>(() => ExecuteCommand(tableToLock));
        }


        private async Task<bool> ExecuteCommand(string tableToLock)
        {
            using (var command = _PeekConnection().CreateCommand())
            {
                command.CommandText = $"LOCK {tableToLock}";
                var changed = await command.ExecuteNonQueryAsync();
                return true;
            }
        }

    }
}