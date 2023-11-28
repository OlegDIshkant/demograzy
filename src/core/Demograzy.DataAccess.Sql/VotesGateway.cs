using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Sql;
using Demograzy.BusinessLogic.DataAccess;


namespace Demograzy.DataAccess.Sql
{
    internal class VotesGateway : Gateway, BusinessLogic.DataAccess.IVotesGateway
    {
        private static readonly string VOTE_TABLE = "demograzy.vote";
        private static readonly string ID_COLUMN = "id";
        private static readonly string CLIENT_COLUMN = "client";
        private static readonly string VERSUS_COLUMN = "versus";
        private static readonly string VOTED_FOR_FIRST_COLUMN = "voted_for_first";
        
        protected override string TableName => VOTE_TABLE;


        public VotesGateway(
            Func<IQueryBuilder> PeekQueryBuilder, 
            Func<INonQueryBuilder> PeekNonQueryBuilder,
            Func<ILockCommandsBuilder> PeekLockCommandsBuilder) :
            base(PeekQueryBuilder, PeekNonQueryBuilder, PeekLockCommandsBuilder)
        {
        }


        public async Task<bool> AddVoteAsync(int versusId, int clientId, bool votedForFirst)
        {
            var insertedAmount = await NonQueryBuilder.Create(
                new InsertOptions()
                {
                    Into = VOTE_TABLE,
                    Values = new List<(ColumnName, Parameter)>()
                    {
                        (new ColumnName(VERSUS_COLUMN), new Parameter(versusId)),
                        (new ColumnName(CLIENT_COLUMN), new Parameter(clientId)),
                        (new ColumnName(VOTED_FOR_FIRST_COLUMN), new Parameter(votedForFirst))
                    }
                }
            ).ExecuteAsync();

            return insertedAmount == 1;
        }


        public async Task<bool> CheckIfClientVotedAsync(int versusId, int clientId)
        {
            var queryResult = await QueryBuilder.Create(
                new SelectOptions()
                {
                    Select = new SelectClause(new ColumnName(ID_COLUMN)),
                    From = VOTE_TABLE,
                    Where = MultiComparison.And(
                        new Comparison(new ColumnName(CLIENT_COLUMN), CompareType.EQUALS, new Parameter(clientId)),
                        new Comparison(new ColumnName(VERSUS_COLUMN), CompareType.EQUALS, new Parameter(versusId))
                    )
                }
                ,
                r => r.GetInt(0)
            )
            .ExecuteAsync();

            return queryResult.Count == 1;
        }



        public async Task<int> GetVotesAmountAsync(int versusId)
        {
            var queryResult = await QueryBuilder.Create(
                new SelectOptions()
                {
                    Select = new SelectClause(new Count()),
                    From = VOTE_TABLE,
                    Where = new Comparison(new ColumnName(VERSUS_COLUMN), CompareType.EQUALS, new Parameter(versusId))
                }
                ,
                r => r.GetInt(0)
            )
            .ExecuteAsync();

            return queryResult.Single();
        }



        public async Task<int> GetVotesAmountForFirstCandidateAsync(int versusId)
        {
            var queryResult = await QueryBuilder.Create(
                new SelectOptions()
                {
                    Select = new SelectClause(new Count()),
                    From = VOTE_TABLE,
                    Where = MultiComparison.And(
                        new Comparison(new ColumnName(VERSUS_COLUMN), CompareType.EQUALS, new Parameter(versusId)),
                        new Comparison(new ColumnName(VOTED_FOR_FIRST_COLUMN), CompareType.EQUALS, new Parameter(true))) 
                }
                ,
                r => r.GetInt(0)
            )
            .ExecuteAsync();

            return queryResult.Single();
        }
    }
}