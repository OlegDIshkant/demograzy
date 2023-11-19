using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Sql;
using Demograzy.BusinessLogic.DataAccess;


namespace Demograzy.DataAccess.Sql
{
    internal class VersesGateway : Gateway, BusinessLogic.DataAccess.IVersesGateway
    {
        private static readonly string VERSUS_TABLE = "versus";
        private static readonly string ID_COLUMN = "id";
        private static readonly string ROOM_COLUMN = "room";
        private static readonly string FIRST_CANDIDATE_COLUMN = "first_candidate";
        private static readonly string SECOND_CANDIDATE_COLUMN = "second_candidate";
        private static readonly string FIRST_CANDIDATE_WON = "first_candidate_won";
        


        public VersesGateway(Func<IQueryBuilder> PeekQueryBuilder, Func<INonQueryBuilder> PeekNonQueryBuilder) :
            base(PeekQueryBuilder, PeekNonQueryBuilder)
        {
        }


        public async Task<List<int>> GetUnfinishedVersesAsync(int roomId)
        {
            var query = QueryBuilder.Create(
                new SelectOptions()
                {
                    Select = new SelectClause(new ColumnName(ID_COLUMN)),
                    From = VERSUS_TABLE,
                    Where = MultiComparison.And(
                        new Comparison(new ColumnName(ROOM_COLUMN), CompareType.EQUALS, new Parameter(roomId)),
                        new Comparison(new ColumnName(FIRST_CANDIDATE_WON), CompareType.EQUALS, new Parameter(null))
                    ) 
                }
            );

            var queryResult = await InvokeQuery(query, r => r.GetInt(0));
            return queryResult.ToList();
        }


    }
}