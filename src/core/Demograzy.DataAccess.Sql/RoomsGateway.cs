using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Sql;
using Demograzy.BusinessLogic.DataAccess;


namespace Demograzy.DataAccess.Sql
{
    internal class RoomsGateway : Gateway, BusinessLogic.DataAccess.IRoomsGateway
    {
        private static readonly string ROOM_TABLE = "demograzy.room";
        private static readonly string ID_COLUMN = "id";
        private static readonly string OWNER_COLUMN = "owner";
        private static readonly string TITLE_COLUMN = "title";
        private static readonly string PASSPHRASE_COLUMN = "passphrase";
        private static readonly string VOTING_STARTED_COLUMN = "voting_started";
        
        protected override string TableName => ROOM_TABLE;


        public RoomsGateway(
            Func<IQueryBuilder> PeekQueryBuilder,
            Func<INonQueryBuilder> PeekNonQueryBuilder,
            Func<ILockCommandsBuilder> PeekLockCommandsBuilder) :
            base(PeekQueryBuilder, PeekNonQueryBuilder,PeekLockCommandsBuilder)
        {
        }



        public async Task<int?> AddRoomAsync(int ownerId, string title, string passphrase)
        {
            await NonQueryBuilder.Create(
                new InsertOptions()
                {
                    Into = ROOM_TABLE,
                    Values = new List<(ColumnName, Parameter)>()
                    {
                        (new ColumnName(OWNER_COLUMN), new Parameter(ownerId)),
                        (new ColumnName(TITLE_COLUMN), new Parameter(title)),
                        (new ColumnName(PASSPHRASE_COLUMN), new Parameter(passphrase)),
                        (new ColumnName(VOTING_STARTED_COLUMN), new Parameter(false))
                    }
                }
            )
            .ExecuteAsync();

            return await GetLastInsertedIdAsync();            
        }

        public async Task<RoomInfo?> GetRoomInfoAsync(int roomId)
        {
            var queryResult = await QueryBuilder.Create(
                new SelectOptions()
                {
                    Select = new SelectClause(
                        new ColumnName(OWNER_COLUMN), 
                        new ColumnName(TITLE_COLUMN), 
                        new ColumnName(PASSPHRASE_COLUMN), 
                        new ColumnName(VOTING_STARTED_COLUMN)),
                    From = ROOM_TABLE,
                    Where = new Comparison(new ColumnName(ID_COLUMN), CompareType.EQUALS, new Parameter(roomId))
                }
                ,
                r => new RoomInfo()
                {
                    ownerClientId = r.GetInt(0),
                    title = r.GetString(1),
                    passphrase = r.GetString(2),
                    votingStarted = r.GetBool(3),
                }
            )
            .ExecuteAsync();

            return SingleOrNull(queryResult);
        }



        public async Task<bool> StartVotingAsync(int roomId)
        {
            var updatedRowsNum = await NonQueryBuilder.Create(
                new UpdateOptions()
                {
                    Update = ROOM_TABLE,
                    Set = new List<(ColumnName, Parameter)>() 
                    { 
                        (new ColumnName(VOTING_STARTED_COLUMN), new Parameter(true)) 
                    },
                    Where = new Comparison(new ColumnName(ID_COLUMN), CompareType.EQUALS, new Parameter(roomId))
                }
            ).ExecuteAsync();

            return CheckIfSingleRowChanged(updatedRowsNum);
        }




        public async Task<ICollection<int>> GetOwnedRoomsAsync(int ownerId)
        {
            return await QueryBuilder.Create(
                new SelectOptions()
                {
                    Select = new SelectClause(new ColumnName(ID_COLUMN)),
                    From = ROOM_TABLE,
                    Where = new Comparison(new ColumnName(OWNER_COLUMN), CompareType.EQUALS, new Parameter(ownerId))
                }
                ,
                r => r.GetInt(0)
            )
            .ExecuteAsync();
        }



        public async Task<bool> CheckRoomExistsAsync(int roomId)
        {
            //TODO: check properly (via SQL)
            return (await GetRoomInfoAsync(roomId)).HasValue;
        }



        public async Task<bool> DeleteRoomAsync(int roomId)
        {
            var modifiedRows = await NonQueryBuilder.Create(
                new DeleteOptions()
                {
                    From = ROOM_TABLE,
                    Where = new Comparison(new ColumnName(ID_COLUMN), CompareType.EQUALS, new Parameter(roomId))
                }
            ).ExecuteAsync();

            return CheckIfSingleRowChanged(modifiedRows);
        }
    }
}