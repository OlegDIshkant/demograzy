using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using DataAccess.Sql;
using Demograzy.BusinessLogic.DataAccess;


namespace Demograzy.DataAccess.Sql
{
    internal class ClientsGateway : BusinessLogic.DataAccess.IClientsGateway
    {
        private readonly static string CLIENT_TABLE = "client";
        private readonly static string ID = "id";
        private readonly static string NAME = "name";


        private Func<IQueryBuilder> _PeekQueryBuilder;
        private Func<INonQueryBuilder> _PeekNonQueryBuilder;


        public ClientsGateway(Func<IQueryBuilder> PeekQueryBuilder, Func<INonQueryBuilder> PeekNonQueryBuilder)
        {
            _PeekQueryBuilder = PeekQueryBuilder;
            _PeekNonQueryBuilder = PeekNonQueryBuilder;
        }



        public async Task<int> AddClientAsync(string name)
        {
            await InsertClientAsync(name);
            return await GetInsertedClientIdAsync();
        }


        private async Task InsertClientAsync(string name)
        {
            var command =
                _PeekNonQueryBuilder().Create(
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
                _PeekQueryBuilder().Create(
                    new SelectOptions()
                    {
                        Select = new SelectClause(new LastInsertedRowId())
                    }
                );

            using (var result = (await query.ExecuteAsync()).GetEnumerator())
            {
                result.MoveNext();
                return result.Current.GetInt(0);
            }
        }



        public async Task<ClientInfo> GetClientInfoAsync(int id)
        {
            var query = _PeekQueryBuilder().Create(
                new SelectOptions()
                {
                    Select = new SelectClause(new ColumnName(NAME)),
                    From = CLIENT_TABLE,
                    Where = new Comparison(new Parameter(id), CompareType.EQUALS, new ColumnName(ID))
                }
            );
            
            using (var result = (await query.ExecuteAsync()).GetEnumerator())
            {
                result.MoveNext();
                return new ClientInfo() { name = result.Current.GetString(0) };
            }
        }



        public async Task<bool> DropClientAsync(int id)
        {
            var deleteCommand = _PeekNonQueryBuilder().Create(
                new DeleteOptions()
                {
                    From = CLIENT_TABLE,
                    Where = new Comparison(new ColumnName(ID), CompareType.EQUALS, new Parameter(id))
                }
            );      

            try
            {
                return await deleteCommand.ExecuteAsync();
            }
            catch(Exception e)
            {
                Debug.WriteLine($"Failed to drop a client via '{deleteCommand}' due to exception: {e}.");
                return false;
            }
        }


        public async Task<int> GetClientsAmountAsync()
        {
            var query = 
                _PeekQueryBuilder().Create(
                    new SelectOptions()
                    {
                        Select = new SelectClause(new Count()),
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