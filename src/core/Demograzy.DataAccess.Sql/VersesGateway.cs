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
        private static readonly string VERSUS_TABLE = "demograzy.versus";
        private static readonly string ID_COLUMN = "id";
        private static readonly string ROOM_COLUMN = "room";
        private static readonly string FIRST_CANDIDATE_COLUMN = "first_candidate";
        private static readonly string SECOND_CANDIDATE_COLUMN = "second_candidate";
        private static readonly string FIRST_CANDIDATE_WON_COLUMN = "first_candidate_won";
        private static readonly string FOLLOW_UP_COLUMN = "follow_up";
        
        protected override string TableName => VERSUS_TABLE;


        public VersesGateway(
            Func<IQueryBuilder> PeekQueryBuilder,
            Func<INonQueryBuilder> PeekNonQueryBuilder,
            Func<ILockCommandsBuilder> PeekLockCommandsBuilder) :
            base(PeekQueryBuilder, PeekNonQueryBuilder, PeekLockCommandsBuilder)
        {
        }


        public async Task<int?> AddFictiveVersusAsync(int roomId, int singleCandidateId)
        {
            var insertedAmount = await NonQueryBuilder.Create(
                new InsertOptions()
                {
                    Into = VERSUS_TABLE,
                    Values = new List<(ColumnName, Parameter)>()
                    {
                        (new ColumnName(ROOM_COLUMN), new Parameter(roomId)),
                        (new ColumnName(FIRST_CANDIDATE_COLUMN), new Parameter(singleCandidateId)),
                        (new ColumnName(SECOND_CANDIDATE_COLUMN), new Parameter(null)),
                        (new ColumnName(FIRST_CANDIDATE_WON_COLUMN), new Parameter(true))
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
                    Values = new List<(ColumnName, Parameter)>()
                    {
                        (new ColumnName(ROOM_COLUMN), new Parameter(roomId)),
                        (new ColumnName(FIRST_CANDIDATE_COLUMN), new Parameter(firstCandidateId)),
                        (new ColumnName(SECOND_CANDIDATE_COLUMN), new Parameter(secondCandidateId)),
                        (new ColumnName(FIRST_CANDIDATE_WON_COLUMN), new Parameter(null))
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



        public async Task<ICollection<int>> GetCompletedVersesWithoutFollowUpAsync(int roomId)
        {
            return await QueryBuilder.Create(
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
                ,
                r => r.GetInt(0)
            )
            .ExecuteAsync();
        }



        public async Task<ICollection<int>> GetUnfinishedVersesAsync(int roomId)
        {
            return await QueryBuilder.Create(
                new SelectOptions()
                {
                    Select = new SelectClause(new ColumnName(ID_COLUMN)),
                    From = VERSUS_TABLE,
                    Where = MultiComparison.And(
                        new Comparison(new ColumnName(ROOM_COLUMN), CompareType.EQUALS, new Parameter(roomId)),
                        new NullComparison(new ColumnName(FIRST_CANDIDATE_WON_COLUMN), NullCompareType.IS_NULL)
                    ) 
                }
                ,
                r => r.GetInt(0)
            )
            .ExecuteAsync();
        }


        public async Task<VersusInfo?> GetVersusInfoAsync(int versusId)
        {
            var queryResult = await QueryBuilder.Create(
                new SelectOptions()
                {
                    Select = new SelectClause(
                        new ColumnName(ROOM_COLUMN),
                        new ColumnName(FIRST_CANDIDATE_COLUMN),
                        new ColumnName(SECOND_CANDIDATE_COLUMN),
                        new ColumnName(FIRST_CANDIDATE_WON_COLUMN)),
                    From = VERSUS_TABLE,
                    Where = new Comparison(new ColumnName(ID_COLUMN), CompareType.EQUALS, new Parameter(versusId))
                }
                ,
                r => new VersusInfo()
                {
                    roomId = r.GetInt(0),
                    firstCandidateId = r.GetInt(1),
                    secondCandidateId = r.GetInt(2),
                    status = 
                        r.IsNull(3) ? VersusInfo.Statuses.UNCOMPLETED :
                        r.GetBool(3) ? VersusInfo.Statuses.FIRST_WON :
                        VersusInfo.Statuses.SECOND_WON
                }
            )
            .ExecuteAsync();

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
            var query = QueryBuilder.Create<int?>(
                new SelectOptions()
                {
                    Select = new SelectClause(
                        new ColumnName(FIRST_CANDIDATE_WON_COLUMN),
                        new ColumnName(FIRST_CANDIDATE_COLUMN),
                        new ColumnName(SECOND_CANDIDATE_COLUMN)),
                    From = VERSUS_TABLE,
                    Where = new Comparison(new ColumnName(ID_COLUMN), CompareType.EQUALS, new Parameter(versusId))
                }
                , 
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
                }
            );

            var queryResult = await query.ExecuteAsync();

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
                    Set = new List<(ColumnName, Parameter)>() 
                    { 
                        (new ColumnName(FOLLOW_UP_COLUMN), new Parameter(referencedVersusId)) 
                    },
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
                    Set = new List<(ColumnName, Parameter)>()
                    {
                        (new ColumnName(FIRST_CANDIDATE_WON_COLUMN), new Parameter(firstIsWinner))
                    },
                    Where = new Comparison(new ColumnName(ID_COLUMN), CompareType.EQUALS, new Parameter(versusId))
                }
            ).ExecuteAsync();

            return changedAmount == 1;
        }

    }
}