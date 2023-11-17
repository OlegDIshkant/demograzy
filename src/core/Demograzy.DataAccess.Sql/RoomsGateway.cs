using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.Sql;


namespace Demograzy.DataAccess.Sql
{
    internal class RoomsGateway : Gateway, BusinessLogic.DataAccess.IRoomsGateway
    {
        private static readonly string ROOM_TABLE = "room";
        private static readonly string OWNER_COLUMN = "owner";
        private static readonly string TITLE_COLUMN = "title";
        private static readonly string PASSPHRASE_COLUMN = "passphrase";


        public RoomsGateway(Func<IQueryBuilder> PeekQueryBuilder, Func<INonQueryBuilder> PeekNonQueryBuilder) :
            base(PeekQueryBuilder, PeekNonQueryBuilder)
        {
        }


        public async Task<int?> AddRoomAsync(int ownerId, string title, string passphrase)
        {
            await NonQueryBuilder.Create(
                new InsertOptions()
                {
                    Into = ROOM_TABLE,
                    Values = new List<(string, object)>()
                    {
                        (OWNER_COLUMN, ownerId),
                        (TITLE_COLUMN, title),
                        (PASSPHRASE_COLUMN, passphrase)
                    }
                }
            )
            .ExecuteAsync();

            return await GetLastInsertedIdAsync();
            
        }


    }
}