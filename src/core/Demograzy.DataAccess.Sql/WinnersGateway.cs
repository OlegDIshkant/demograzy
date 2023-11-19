using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Sql;
using Demograzy.BusinessLogic.DataAccess;


namespace Demograzy.DataAccess.Sql
{
    internal class WinnersGateway : Gateway, BusinessLogic.DataAccess.IWinnersGateway
    {
        private static readonly string WINNER_TABLE = "winner";
        private static readonly string ID_COLUMN = "id";
        private static readonly string ROOM_COLUMN = "room";
        


        public WinnersGateway(Func<IQueryBuilder> PeekQueryBuilder, Func<INonQueryBuilder> PeekNonQueryBuilder) :
            base(PeekQueryBuilder, PeekNonQueryBuilder)
        {
        }
        

        public async Task<int?> GetWinnerAsync(int roomId)
        {
            var query = QueryBuilder.Create(
                new SelectOptions()
                {
                    Select = new SelectClause(new ColumnName(ID_COLUMN)),
                    From = WINNER_TABLE,
                    Where = new Comparison(new ColumnName(ROOM_COLUMN), CompareType.EQUALS, new Parameter(roomId))
                }
            );

            var queryResult = await InvokeQuery(query, r => r.GetInt(0));
            if (queryResult.Count == 1)
            {
                return queryResult.Single();
            }
            else
            {
                return null;
            }
        }


    }
}