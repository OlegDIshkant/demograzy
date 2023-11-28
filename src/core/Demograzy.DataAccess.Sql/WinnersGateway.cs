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
        private static readonly string WINNER_TABLE = "demograzy.winner";
        private static readonly string ID_COLUMN = "id";
        private static readonly string ROOM_COLUMN = "room";
        
        protected override string TableName => WINNER_TABLE;


        public WinnersGateway(
            Func<IQueryBuilder> PeekQueryBuilder,
            Func<INonQueryBuilder> PeekNonQueryBuilder,
            Func<ILockCommandsBuilder> PeekLockCommandsBuilder) :
            base(PeekQueryBuilder, PeekNonQueryBuilder, PeekLockCommandsBuilder)
        {
        }


        public async Task<bool> AddWinnerAsync(int roomId, int winnerId)
        {
            var changedAmount = await NonQueryBuilder.Create(
                new InsertOptions()
                {
                    Into = WINNER_TABLE,
                    Values = new List<(ColumnName, Parameter)>()
                    {
                        (new ColumnName(ID_COLUMN), new Parameter(winnerId)),
                        (new ColumnName(ROOM_COLUMN), new Parameter(roomId))
                    }
                }
            ).ExecuteAsync();

            return changedAmount == 1;
        }



        public async Task<int?> GetWinnerAsync(int roomId)
        {
            var queryResult = await QueryBuilder.Create(
                new SelectOptions()
                {
                    Select = new SelectClause(new ColumnName(ID_COLUMN)),
                    From = WINNER_TABLE,
                    Where = new Comparison(new ColumnName(ROOM_COLUMN), CompareType.EQUALS, new Parameter(roomId))
                }
                ,
                r => r.GetInt(0)
            )
            .ExecuteAsync();

            if (queryResult.Any())
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