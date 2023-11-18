using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Sql;
using Demograzy.BusinessLogic.DataAccess;


namespace Demograzy.DataAccess.Sql
{
    internal class RoomsGateway : Gateway, BusinessLogic.DataAccess.IRoomsGateway
    {
        private static readonly string ROOM_TABLE = "room";
        private static readonly string ID_COLUMN = "id";
        private static readonly string OWNER_COLUMN = "owner";
        private static readonly string TITLE_COLUMN = "title";
        private static readonly string PASSPHRASE_COLUMN = "passphrase";
        private static readonly string VOTING_STARTED_COLUMN = "voting_started";
        


        public RoomsGateway(Func<IQueryBuilder> PeekQueryBuilder, Func<INonQueryBuilder> PeekNonQueryBuilder) :
            base(PeekQueryBuilder, PeekNonQueryBuilder)
        {
        }



        public async Task<int?> AddRoomAsync(int ownerId, string title, string passphrase)
        {
            await NonQueryBuilder.Create(
                new InsertOptions()
                {
                    Into = ROOM_TABLE,
                    Values = new List<(string, object)>()
                    {
                        (OWNER_COLUMN, ownerId),
                        (TITLE_COLUMN, title),
                        (PASSPHRASE_COLUMN, passphrase),
                        (VOTING_STARTED_COLUMN, false)
                    }
                }
            )
            .ExecuteAsync();

            return await GetLastInsertedIdAsync();
            
        }

        public async Task<RoomInfo?> GetRoomInfoAsync(int roomId)
        {
            var query = QueryBuilder.Create(
                new SelectOptions()
                {
                    Select = new SelectClause(
                        new ColumnName(OWNER_COLUMN), 
                        new ColumnName(TITLE_COLUMN), 
                        new ColumnName(PASSPHRASE_COLUMN), 
                        new ColumnName(VOTING_STARTED_COLUMN)),
                    From = ROOM_TABLE,
                    Where = new Comparison(new ColumnName(ID_COLUMN), CompareType.EQUALS, new Parameter(roomId))
                }
            );

            using (var queryResult = await query.ExecuteAsync())
            {
                var e = queryResult.GetEnumerator();
                e.MoveNext();
                return new RoomInfo()
                {
                    ownerClientId = e.Current.GetInt(0),
                    title = e.Current.GetString(1),
                    passphrase = e.Current.GetString(2),
                    votingStarted = e.Current.GetBool(3),
                };
            }
        }



        public async Task<bool> StartVotingAsync(int roomId)
        {
            var updatedRowsNum = await NonQueryBuilder.Create(
                new UpdateOptions()
                {
                    Update = ROOM_TABLE,
                    Set = new List<(string, object)>() { (VOTING_STARTED_COLUMN, true) },
                    Where = new Comparison(new ColumnName(ID_COLUMN), CompareType.EQUALS, new Parameter(roomId))
                }
            ).ExecuteAsync();

            return CheckIfSingleRowChanged(updatedRowsNum);
        }




        public async Task<List<int>> GetOwnedRoomsAsync(int ownerId)
        {
            var query = QueryBuilder.Create(
                new SelectOptions()
                {
                    Select = new SelectClause(new ColumnName(ID_COLUMN)),
                    From = ROOM_TABLE,
                    Where = new Comparison(new ColumnName(OWNER_COLUMN), CompareType.EQUALS, new Parameter(ownerId))
                }
            );

            return await InvokeQuery(query, r => r.GetInt(0));
        }


    }
}