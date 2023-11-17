using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Sql;


namespace Demograzy.DataAccess.Sql
{
    internal class CandidatesGateway : Gateway, BusinessLogic.DataAccess.ICandidatesGateway
    {
        private readonly static string CANDIDATE_TABLE = "candidate";
        private readonly static string NAME_COLUMN = "name";
        private readonly static string ROOM_COLUMN = "room";
        


        public CandidatesGateway(Func<IQueryBuilder> PeekQueryBuilder, Func<INonQueryBuilder> PeekNonQueryBuilder) :
            base(PeekQueryBuilder, PeekNonQueryBuilder)
        {
        }


        public async Task<int?> AddCandidateAsync(int roomId, string name)
        {
            await NonQueryBuilder.Create(
                new InsertOptions()
                {
                    Into = CANDIDATE_TABLE,
                    Values = new List<(string, object)>()
                    {
                        (NAME_COLUMN, name),
                        (ROOM_COLUMN, roomId)
                    }
                }
            )
            .ExecuteAsync();

            return await GetLastInsertedIdAsync();
        }




    }
}