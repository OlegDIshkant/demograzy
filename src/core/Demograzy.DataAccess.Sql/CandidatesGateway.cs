using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.XPath;
using DataAccess.Sql;
using Demograzy.BusinessLogic.DataAccess;


namespace Demograzy.DataAccess.Sql
{
    internal class CandidatesGateway : Gateway, BusinessLogic.DataAccess.ICandidatesGateway
    {
        private readonly static string CANDIDATE_TABLE = "candidate";
        private readonly static string ID_COLUMN = "id";
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



        public async Task<CandidateInfo?> GetCandidateInfo(int candidateId)
        {
            var query = QueryBuilder.Create(
                new SelectOptions()
                {
                    Select = new SelectClause(new ColumnName(NAME_COLUMN), new ColumnName(ROOM_COLUMN)),
                    From = CANDIDATE_TABLE,
                    Where = new Comparison(new ColumnName(ID_COLUMN), CompareType.EQUALS, new Parameter(candidateId))
                }
            );

            using (var result = await query.ExecuteAsync())
            {
                var e = result.GetEnumerator();
                e.MoveNext();
                return new CandidateInfo()
                {
                    name = e.Current.GetString(0),
                    roomId = e.Current.GetInt(1)
                };
            }
        }

    }
}