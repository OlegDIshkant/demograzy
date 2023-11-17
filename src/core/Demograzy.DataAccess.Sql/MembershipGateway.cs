using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Sql;


namespace Demograzy.DataAccess.Sql
{
    internal class MembershipGateway : Gateway, BusinessLogic.DataAccess.IMembershipGateway
    {
        private static readonly string MEMBERSHIP_TABLE = "room_membership";
        private static readonly string CLIENT_COLUMN = "client";
        private static readonly string ROOM_COLUMN = "room";
        


        public MembershipGateway(Func<IQueryBuilder> PeekQueryBuilder, Func<INonQueryBuilder> PeekNonQueryBuilder) :
            base(PeekQueryBuilder, PeekNonQueryBuilder)
        {
        }



        public async Task<bool> AddRoomMemberAsync(int roomId, int clientId)
        {
            var insertedAmount = await NonQueryBuilder.Create(
                new InsertOptions()
                {
                    Into = MEMBERSHIP_TABLE,
                    Values = new List<(string, object)>()
                    {
                        (ROOM_COLUMN, roomId),
                        (CLIENT_COLUMN, clientId)
                    }
                }
            )
            .ExecuteAsync();

            return insertedAmount > 0;
        }



        public async Task<List<int>> GetRoomMembersAsync(int roomId)
        {
            var query = QueryBuilder.Create(
                new SelectOptions()
                {
                    Select = new SelectClause(new ColumnName(CLIENT_COLUMN)),
                    From = MEMBERSHIP_TABLE,
                    Where = new Comparison(new ColumnName(ROOM_COLUMN), CompareType.EQUALS, new Parameter(roomId))
                }
            );

            using (var queryResult = await query.ExecuteAsync())
            {
                return queryResult.Select(r => r.GetInt(0)).ToList();
            }
        }


    }
}