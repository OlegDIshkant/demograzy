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
        private static readonly string FIRST_CANDIDATE_WON_COLUMN = "first_candidate_won";
        private static readonly string FOLLOW_UP_COLUMN = "follow_up";
        


        public VersesGateway(Func<IQueryBuilder> PeekQueryBuilder, Func<INonQueryBuilder> PeekNonQueryBuilder) :
            base(PeekQueryBuilder, PeekNonQueryBuilder)
        {
        }


        public async Task<int?> AddFictiveVersusAsync(int roomId, int singleCandidateId)
        {
            var insertedAmount = await NonQueryBuilder.Create(
                new InsertOptions()
                {
                    Into = VERSUS_TABLE,
                    Values = new List<(string, object)>()
                    {
                        (ROOM_COLUMN, roomId),
                        (FIRST_CANDIDATE_COLUMN, singleCandidateId),
                        (SECOND_CANDIDATE_COLUMN, null),
                        (FIRST_CANDIDATE_WON_COLUMN, true)
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
                        (FIRST_CANDIDATE_WON_COLUMN, null)
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



        public async Task<List<int>> GetCompletedVersesWithoutFollowUpAsync(int roomId)
        {
            var query = QueryBuilder.Create(
                new SelectOptions()
                {
                    Select = new SelectClause(new ColumnName(ID_COLUMN)),
                    From = VERSUS_TABLE,
                    Where = MultiComparison.And (
                        new Comparison(new ColumnName(ROOM_COLUMN), CompareType.EQUALS, new Parameter(roomId)),
                        new NullComparison(new ColumnName(FIRST_CANDIDATE_WON_COLUMN), NullCompareType.IS_NOT_NULL),
                        new NullComparison(new ColumnName(FOLLOW_UP_COLUMN), NullCompareType.IS_NULL)
                    )
                }
            );

            return await InvokeQuery(query, r => r.GetInt(0));
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
                        new NullComparison(new ColumnName(FIRST_CANDIDATE_WON_COLUMN), NullCompareType.IS_NULL)
                    ) 
                }
            );

            var queryResult = await InvokeQuery(query, r => r.GetInt(0));
            return queryResult.ToList();
        }


        public async Task<VersusInfo?> GetVersusInfoAsync(int versusId)
        {
            var query = QueryBuilder.Create(
                new SelectOptions()
                {
                    Select = new SelectClause(
                        new ColumnName(ROOM_COLUMN),
                        new ColumnName(FIRST_CANDIDATE_COLUMN),
                        new ColumnName(SECOND_CANDIDATE_COLUMN),
                        new ColumnName(FIRST_CANDIDATE_WON_COLUMN)),
                    From = VERSUS_TABLE,
                    Where = new Comparison(new ColumnName(ID_COLUMN), CompareType.EQUALS, new Parameter(versusId))
                });

            var queryResult = await InvokeQuery(
                query,
                r => new VersusInfo()
                {
                    roomId = r.GetInt(0),
                    firstCandidateId = r.GetInt(1),
                    secondCandidateId = r.GetInt(2),
                    status = 
                        r.IsNull(3) ? VersusInfo.Statuses.UNCOMPLETED :
                        r.GetBool(3) ? VersusInfo.Statuses.FIRST_WON :
                        VersusInfo.Statuses.SECOND_WON
                });

            if (queryResult.Count == 1)
            {
                return queryResult.Single();
            }
            else
            {
                return null;
            }
        }



        public async Task<int?> GetVersusWinnerAsync(int versusId)
        {
            var query = QueryBuilder.Create(
                new SelectOptions()
                {
                    Select = new SelectClause(
                        new ColumnName(FIRST_CANDIDATE_WON_COLUMN),
                        new ColumnName(FIRST_CANDIDATE_COLUMN),
                        new ColumnName(SECOND_CANDIDATE_COLUMN)),
                    From = VERSUS_TABLE,
                    Where = new Comparison(new ColumnName(ID_COLUMN), CompareType.EQUALS, new Parameter(versusId))
                }
            );

            var queryResult = await InvokeQuery<int?>(
                query, 
                r => 
                {
                    var winnerExists = !r.IsNull(0);
                    if (winnerExists)
                    {
                        var firstIsWinner = r.GetBool(0);
                        return firstIsWinner ?
                            r.GetInt(1) :
                            r.GetInt(2);
                    }
                    return null;
                });

            if (queryResult.Count == 1)
            {
                return queryResult.Single();
            }
            return null;
        }



        public async Task<bool> SetVersusFollowUpAsync(int versusId, int referencedVersusId)
        {
            var changedAmount = await NonQueryBuilder.Create(
                new UpdateOptions()
                {
                    Update = VERSUS_TABLE,
                    Set = new List<(string, object)>() { (FOLLOW_UP_COLUMN, referencedVersusId) },
                    Where = new Comparison(new ColumnName(ID_COLUMN), CompareType.EQUALS, new Parameter(versusId))
                }
            ).ExecuteAsync();

            return changedAmount == 1;
        }



        public async Task<bool> SetVersusWinnerAsync(int versusId, bool firstIsWinner)
        {
            var changedAmount = await NonQueryBuilder.Create(
                new UpdateOptions()
                {
                    Update = VERSUS_TABLE,
                    Set = new List<(string column, object value)>()
                    {
                        (FIRST_CANDIDATE_WON_COLUMN, firstIsWinner)
                    },
                    Where = new Comparison(new ColumnName(ID_COLUMN), CompareType.EQUALS, new Parameter(versusId))
                }
            ).ExecuteAsync();

            return changedAmount == 1;
        }

    }
}