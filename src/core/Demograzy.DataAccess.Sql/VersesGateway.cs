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



        public async Task<int?> AddVersusAsync(int roomId, int firstCandidateId, int secondCandidateId)
        {
            var insertedAmount = await NonQueryBuilder.Create(
                new InsertOptions()
                {
                    Into = VERSUS_TABLE,
                    Values = new List<(string, object)>()
                    {
                        (ROOM_COLUMN, roomId),
                        (FIRST_CANDIDATE_COLUMN, firstCandidateId),
                        (SECOND_CANDIDATE_COLUMN, secondCandidateId),
                        (FIRST_CANDIDATE_WON, null)
                    }
                }
            ).ExecuteAsync();

            if (insertedAmount == 1)
            {
                return await GetLastInsertedIdAsync();
            }
            else
            {
                return null;
            }

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
                        new NullComparison(new ColumnName(FIRST_CANDIDATE_WON), NullCompareType.IS_NULL)
                    ) 
                }
            );

            var queryResult = await InvokeQuery(query, r => r.GetInt(0));
            return queryResult.ToList();
        }


    }
}