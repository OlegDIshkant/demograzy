using System;
using System.Threading.Tasks;
using Demograzy.BusinessLogic.DataAccess;


namespace Demograzy.DataAccess.Sql
{
    internal class ClientsGateway : BusinessLogic.DataAccess.IClientsGateway
    {
        private readonly static string CLIENT_TABLE = "client";
        private readonly static string ID = "id";
        private readonly static string NAME = "name";
        private readonly static string SESSION_ID = "session_id";


        private Func<ISqlCommandBuilder> _PeekCommandBuilder;


        public ClientsGateway(Func<ISqlCommandBuilder> PeekCommandBuilder)
        {
            _PeekCommandBuilder = PeekCommandBuilder;
        }



        public Task<int> AddClientAsync(string name)
        {
            throw new NotImplementedException();
            /*var cmdText = $"INSERT INTO {CLIENT_TABLE} ({ID}, {NAME}, {SESSION_ID}) VALUES ($1), (%2), (%3)";
            var cmd = new NpgsqlCommand(cmdText, _PeekCommandBuilder())
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
            }*/
        }



        public Task<ClientInfo> GetClientInfoAsync(int id)
        {
            throw new NotImplementedException();
            /*
            var cmdText = $"SELECT {ID}, {NAME}, {SESSION_ID} FROM {CLIENT_TABLE} WHERE {ID} = ($1)";
            var cmd = new NpgsqlCommand(cmdText, _PeekCommandBuilder())
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

            return null;*/
        }



        public Task<bool> DropClientAsync(int id)
        {
            throw new NotImplementedException();
            /*
            var cmdText = $"DELETE FROM {CLIENT_TABLE} WHERE {ID} = ($1)";
            var cmd = new NpgsqlCommand(cmdText, _PeekCommandBuilder())
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

            return true;*/
        }


        public Task<int> GetClientsAmountAsync()
        {
            throw new NotImplementedException();
        }
    }
}