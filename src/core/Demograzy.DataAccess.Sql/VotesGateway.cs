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
        private static readonly string VOTE_TABLE = "vote";
        private static readonly string ID_COLUMN = "id";
        private static readonly string CLIENT_COLUMN = "client";
        private static readonly string VERSUS_COLUMN = "versus";
        private static readonly string VOTED_FOR_FIRST_COLUMN = "voted_for_first";
        


        public VotesGateway(Func<IQueryBuilder> PeekQueryBuilder, Func<INonQueryBuilder> PeekNonQueryBuilder) :
            base(PeekQueryBuilder, PeekNonQueryBuilder)
        {
        }


        public async Task<bool> CheckIfClientVotedAsync(int versusId, int clientId)
        {
            var query = QueryBuilder.Create(
                new SelectOptions()
                {
                    Select = new SelectClause(new ColumnName(ID_COLUMN)),
                    From = VOTE_TABLE,
                    Where = MultiComparison.And(
                        new Comparison(new ColumnName(CLIENT_COLUMN), CompareType.EQUALS, new Parameter(clientId)),
                        new Comparison(new ColumnName(VERSUS_COLUMN), CompareType.EQUALS, new Parameter(versusId))
                    )
                }
            );

            var queryResult = await InvokeQuery(query, r => r.GetInt(0));
            return queryResult.Count == 1;
        }
    }
}