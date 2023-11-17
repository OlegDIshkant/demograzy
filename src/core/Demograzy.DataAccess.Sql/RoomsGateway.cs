using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Sql;


namespace Demograzy.DataAccess.Sql
{
    internal class RoomsGateway : Gateway, BusinessLogic.DataAccess.IRoomsGateway
    {
        private static readonly string ROOM_TABLE = "room";
        private static readonly string ROOM_OWNER_COLUMN = "owner";
        private static readonly string ROOM_TITLE_COLUMN = "title";
        private static readonly string ROOM_PASSPHRASE_COLUMN = "passphrase";
        private static readonly string MEMBERSHIP_TABLE = "room_membership";
        private static readonly string MEMBERSHIP_CLIENT_COLUMN = "client";
        private static readonly string MEMBERSHIP_ROOM_COLUMN = "room";
        


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
                        (ROOM_OWNER_COLUMN, ownerId),
                        (ROOM_TITLE_COLUMN, title),
                        (ROOM_PASSPHRASE_COLUMN, passphrase)
                    }
                }
            )
            .ExecuteAsync();

            return await GetLastInsertedIdAsync();
            
        }



        public Task<bool> AddRoomMemberAsync(int roomId, int clientId)
        {
            return NonQueryBuilder.Create(
                new InsertOptions()
                {
                    Into = MEMBERSHIP_TABLE,
                    Values = new List<(string, object)>()
                    {
                        (MEMBERSHIP_ROOM_COLUMN, roomId),
                        (MEMBERSHIP_CLIENT_COLUMN, clientId)
                    }
                }
            )
            .ExecuteAsync();
        }



        public async Task<List<int>> GetRoomMembersAsync(int roomId)
        {
            var query = QueryBuilder.Create(
                new SelectOptions()
                {
                    Select = new SelectClause(new ColumnName(MEMBERSHIP_CLIENT_COLUMN)),
                    From = MEMBERSHIP_TABLE,
                    Where = new Comparison(new ColumnName(MEMBERSHIP_ROOM_COLUMN), CompareType.EQUALS, new Parameter(roomId))
                }
            );

            using (var queryResult = await query.ExecuteAsync())
            {
                return queryResult.Select(r => r.GetInt(0)).ToList();
            }
        }


    }
}