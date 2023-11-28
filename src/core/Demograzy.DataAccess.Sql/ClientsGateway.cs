using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Sql;
using Demograzy.BusinessLogic.DataAccess;


namespace Demograzy.DataAccess.Sql
{
    internal class ClientsGateway : Gateway, BusinessLogic.DataAccess.IClientsGateway
    {
        private readonly static string CLIENT_TABLE = "demograzy.client";
        private readonly static string ID = "id";
        private readonly static string NAME = "name";

        protected override string TableName => CLIENT_TABLE;

        public ClientsGateway(
            Func<IQueryBuilder> PeekQueryBuilder,
             Func<INonQueryBuilder> PeekNonQueryBuilder,
            Func<ILockCommandsBuilder> PeekLockCommandsBuilder) :
            base(PeekQueryBuilder, PeekNonQueryBuilder, PeekLockCommandsBuilder) 
        {
        }



        public async Task<int> AddClientAsync(string name)
        {
            await InsertClientAsync(name);
            return await GetLastInsertedIdAsync();
        }


        private async Task InsertClientAsync(string name)
        {
            var command =
                NonQueryBuilder.Create(
                    new InsertOptions()
                    {
                        Into = CLIENT_TABLE,
                        Values = new List<(ColumnName, Parameter)>()
                        {
                            (new ColumnName(NAME), new Parameter(name))
                        }
                    }
                );

            var success = await command.ExecuteAsync() > 0;
            if (!success)
            {
                throw new Exception("Operation failed.");
            }
        }



        public async Task<ClientInfo?> GetClientInfoAsync(int id)
        {
            var queryResult = await QueryBuilder.Create(
                new SelectOptions()
                {
                    Select = new SelectClause(new ColumnName(NAME)),
                    From = CLIENT_TABLE,
                    Where = new Comparison(new Parameter(id), CompareType.EQUALS, new ColumnName(ID))
                }
                ,
                r => new ClientInfo() { name = r.GetString(0) }
            )
            .ExecuteAsync();
            
            if (queryResult.Any())
            {
                return queryResult.Single();
            }
            else
            {
                return null;
            }
        }



        public async Task<bool> DropClientAsync(int id)
        {
            var deleteCommand = NonQueryBuilder.Create(
                new DeleteOptions()
                {
                    From = CLIENT_TABLE,
                    Where = new Comparison(new ColumnName(ID), CompareType.EQUALS, new Parameter(id))
                }
            );      

            try
            {
                return await deleteCommand.ExecuteAsync() > 0;
            }
            catch(Exception e)
            {
                Debug.WriteLine($"Failed to drop a client via '{deleteCommand}' due to exception: {e}.");
                return false;
            }
        }


        public async Task<int> GetClientsAmountAsync()
        {
            var queryResult = 
                await QueryBuilder.Create(
                    new SelectOptions()
                    {
                        Select = new SelectClause(new Count()),
                        From = CLIENT_TABLE
                    }
                    ,
                    r => r.GetInt(0)
                )
                .ExecuteAsync();
            
            return queryResult.Single();
        }

        public async Task<bool> CheckClientExistsAsync(int id)
        {
            //TODO: check properly (via SQL)
            return (await GetClientInfoAsync(id)).HasValue;
        }
    }
}