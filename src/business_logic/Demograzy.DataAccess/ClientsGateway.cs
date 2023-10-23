using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Npgsql;


namespace Demograzy.DataAccess
{
    internal class ClientsGateway : BusinessLogic.DataAccess.IClientsGateway
    {
        private readonly static string CLIENT_TABLE = "client";
        private readonly static string ID = "id";
        private readonly static string NAME = "name";
        private readonly static string SESSION_ID = "session_id";


        private Func<NpgsqlConnection> _PeekConnection;


        public ClientsGateway(Func<NpgsqlConnection> PeekConnection)
        {
            _PeekConnection = PeekConnection;
        }



        public async Task<bool> TryAddClientAsync(int id, string name, string sessionId)
        {
            var cmdText = $"INSERT INTO {CLIENT_TABLE} ({ID}, {NAME}, {SESSION_ID}) VALUES ($1), (%2), (%3)";
            var cmd = new NpgsqlCommand(cmdText, _PeekConnection())
            {
                Parameters =
                {
                    new NpgsqlParameter() { Value = id },
                    new NpgsqlParameter() { Value = name },
                    new NpgsqlParameter() { Value = sessionId }
                }
            };            

            try
            {
                await cmd.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Failed to add a client via '{cmdText}' due to exception: {e}.");
                return false;
            }
        }



        public async Task<(string name, string sessionId)?> TryFindClientByIdAsync(int id)
        {

            var cmdText = $"SELECT {ID}, {NAME}, {SESSION_ID} FROM {CLIENT_TABLE} WHERE {ID} = ($1)";
            var cmd = new NpgsqlCommand(cmdText, _PeekConnection())
            {
                Parameters =
                {
                    new NpgsqlParameter() { Value = id }
                }
            };     

            try
            {
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        reader.NextResult();
                        var result = (reader.GetString(1), reader.GetString(2));
                        
                        if (reader.NextResult())
                        {
                            Debug.WriteLine($"Failed to find a client by ID via '{cmdText}' since thare more than one such user.");
                        }

                        return result;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Failed to find a client by ID via '{cmdText}' due to exception: {e}.");
            }

            return null;
        }



        public async Task<bool> TryDropClientAsync(int id)
        {
            var cmdText = $"DELETE FROM {CLIENT_TABLE} WHERE {ID} = ($1)";
            var cmd = new NpgsqlCommand(cmdText, _PeekConnection())
            {
                Parameters =
                {
                    new NpgsqlParameter() { Value = id }
                }
            };            

            try
            {
                await cmd.ExecuteNonQueryAsync();
            }
            catch(Exception e)
            {
                Debug.WriteLine($"Failed to drop a client via '{cmdText}' due to exception: {e}.");
                return false;
            }

            return true;
        }
    }
}