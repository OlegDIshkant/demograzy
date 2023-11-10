using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Demograzy.BusinessLogic.DataAccess;


namespace Demograzy.DataAccess.Sql
{
    internal class ClientsGateway : BusinessLogic.DataAccess.IClientsGateway
    {
        private readonly static string CLIENT_TABLE = "client";
        private readonly static string NAME = "name";


        private Func<ISqlCommandBuilder> _PeekCommandBuilder;


        public ClientsGateway(Func<ISqlCommandBuilder> PeekCommandBuilder)
        {
            _PeekCommandBuilder = PeekCommandBuilder;
        }



        public async Task<int> AddClientAsync(string name)
        {
            await InsertClientAsync(name);
            return await GetInsertedClientIdAsync();
        }


        private async Task InsertClientAsync(string name)
        {
            var command =
                _PeekCommandBuilder().NonQueries.Create(
                    new InsertOptions()
                    {
                        Into = CLIENT_TABLE,
                        Values = new List<(string, object)>()
                        {
                            (NAME, name)
                        }
                    }
                );

            var success = await command.ExecuteAsync();
            if (!success)
            {
                throw new Exception("Operation failed.");
            }
        }


        private async Task<int> GetInsertedClientIdAsync()
        {
            var query =
                _PeekCommandBuilder().Queries.Create(
                    new SelectOptions()
                    {
                        Select = new List<IColumnOption>() { new LastInsertedRowId() }
                    }
                );

            using (var result = (await query.ExecuteAsync()).GetEnumerator())
            {
                result.MoveNext();
                return result.Current.GetInt(0);
            }
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


        public async Task<int> GetClientsAmountAsync()
        {
            var query = 
                _PeekCommandBuilder().Queries.Create(
                    new SelectOptions()
                    {
                        Select = new List<IColumnOption>() { new Count() },
                        From = CLIENT_TABLE
                    }
                );
            
            using (var result = (await query.ExecuteAsync()).GetEnumerator())
            {
                result.MoveNext();
                return result.Current.GetInt(0);
            }
        }
        

    }
}