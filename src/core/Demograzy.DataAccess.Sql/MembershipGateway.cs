using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Sql;


namespace Demograzy.DataAccess.Sql
{
    internal class MembershipGateway : Gateway, BusinessLogic.DataAccess.IMembershipGateway
    {
        private static readonly string MEMBERSHIP_TABLE = "demograzy.room_membership";
        private static readonly string CLIENT_COLUMN = "client";
        private static readonly string ROOM_COLUMN = "room";

        protected override string TableName => MEMBERSHIP_TABLE;


        public MembershipGateway(
            Func<IQueryBuilder> PeekQueryBuilder, 
            Func<INonQueryBuilder> PeekNonQueryBuilder,
            Func<ILockCommandsBuilder> PeekLockCommandsBuilder) :
            base(PeekQueryBuilder, PeekNonQueryBuilder, PeekLockCommandsBuilder)
        {
        }



        public async Task<bool> AddRoomMemberAsync(int roomId, int clientId)
        {
            var insertedAmount = await NonQueryBuilder.Create(
                new InsertOptions()
                {
                    Into = MEMBERSHIP_TABLE,
                    Values = new List<(ColumnName, Parameter)>()
                    {
                        (new ColumnName(ROOM_COLUMN), new Parameter(roomId)),
                        (new ColumnName(CLIENT_COLUMN), new Parameter(clientId))
                    }
                }
            )
            .ExecuteAsync();

            return insertedAmount > 0;
        }



        public async Task<ICollection<int>> GetRoomMembersAsync(int roomId)
        {
            return await QueryBuilder.Create(
                new SelectOptions()
                {
                    Select = new SelectClause(new ColumnName(CLIENT_COLUMN)),
                    From = MEMBERSHIP_TABLE,
                    Where = new Comparison(new ColumnName(ROOM_COLUMN), CompareType.EQUALS, new Parameter(roomId))
                }
                ,
                r => r.GetInt(0)
            )
            .ExecuteAsync();
            
        }



        public async Task<ICollection<int>> GetJoinedRoomsAsync(int clientId)
        {
            return await QueryBuilder.Create(
                new SelectOptions()
                {
                    Select = new SelectClause(new ColumnName(ROOM_COLUMN)),
                    From = MEMBERSHIP_TABLE,
                    Where = new Comparison(new ColumnName(CLIENT_COLUMN), CompareType.EQUALS, new Parameter(clientId))
                }
                ,
                r => r.GetInt(0)
            )
            .ExecuteAsync();
            
        }



        public async Task<bool> ForgetAllMembersAsync(int roomId)
        {
            var command = await NonQueryBuilder.Create(
                new DeleteOptions()
                {
                    From = MEMBERSHIP_TABLE,
                    Where = new Comparison(new ColumnName(ROOM_COLUMN), CompareType.EQUALS, new Parameter(roomId))
                }
            ).ExecuteAsync();

            return true;
        }



        public async Task<bool> ForgetMemberAsync(int roomId, int clientId)
        {
            var changedRowsAmount = await NonQueryBuilder.Create(
                new DeleteOptions()
                {
                    From = MEMBERSHIP_TABLE,
                    Where = MultiComparison.And(
                        new Comparison(new ColumnName(ROOM_COLUMN), CompareType.EQUALS, new Parameter(roomId)),
                        new Comparison(new ColumnName(CLIENT_COLUMN), CompareType.EQUALS, new Parameter(clientId))
                    ) 
                }
            ).ExecuteAsync();

            return CheckIfSingleRowChanged(changedRowsAmount);
        }


    }
}