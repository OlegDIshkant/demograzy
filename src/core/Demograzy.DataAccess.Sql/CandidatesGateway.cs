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
        private readonly static string CANDIDATE_TABLE = "demograzy.candidate";
        private readonly static string ID_COLUMN = "id";
        private readonly static string NAME_COLUMN = "name";
        private readonly static string ROOM_COLUMN = "room";
        
        protected override string TableName => CANDIDATE_TABLE;


        public CandidatesGateway(
            Func<IQueryBuilder> PeekQueryBuilder, 
            Func<INonQueryBuilder> PeekNonQueryBuilder,
            Func<ILockCommandsBuilder> PeekLockCommandsBuilder) :
            base(PeekQueryBuilder, PeekNonQueryBuilder, PeekLockCommandsBuilder)
        {
        }


        public async Task<int?> AddCandidateAsync(int roomId, string name)
        {
            await NonQueryBuilder.Create(
                new InsertOptions()
                {
                    Into = CANDIDATE_TABLE,
                    Values = new List<(ColumnName, Parameter)>()
                    {
                        (new ColumnName(NAME_COLUMN), new Parameter(name)),
                        (new ColumnName(ROOM_COLUMN), new Parameter(roomId))
                    }
                }
            )
            .ExecuteAsync();

            return await GetLastInsertedIdAsync();
        }


        public async Task<CandidateInfo?> GetCandidateInfo(int candidateId)
        {
            var queryResult = await QueryBuilder.Create(
                new SelectOptions()
                {
                    Select = new SelectClause(new ColumnName(NAME_COLUMN), new ColumnName(ROOM_COLUMN)),
                    From = CANDIDATE_TABLE,
                    Where = new Comparison(new ColumnName(ID_COLUMN), CompareType.EQUALS, new Parameter(candidateId))
                }
                ,
                r => new CandidateInfo()
                {
                    name = r.GetString(0),
                    roomId = r.GetInt(1)
                }
            )
            .ExecuteAsync();

            return SingleOrNull(queryResult);
        }



        public async Task<ICollection<int>> GetCandidates(int roomId)
        {
            return await QueryBuilder.Create(
                new SelectOptions()
                {
                    Select = new SelectClause(new ColumnName(ID_COLUMN)),
                    From = CANDIDATE_TABLE,
                    Where = new Comparison(new ColumnName(ROOM_COLUMN), CompareType.EQUALS, new Parameter(roomId))
                }
                ,
                r => r.GetInt(0)
            )
            .ExecuteAsync();
        }


        public async Task<int> GetCandidatesAmount(int roomId)
        {
            var queryResult = await QueryBuilder.Create(
                new SelectOptions()
                {
                    Select = new SelectClause(new Count()),
                    From = CANDIDATE_TABLE,
                    Where = new Comparison(new ColumnName(ROOM_COLUMN), CompareType.EQUALS, new Parameter(roomId))
                }
                ,
                r => r.GetInt(0)
            )
            .ExecuteAsync();

            return queryResult.Single();
        }


        public async Task<bool> DeleteCandidateAsync(int candidateId)
        {
            var deletedRows = await NonQueryBuilder.Create(
                new DeleteOptions()
                {
                    From = CANDIDATE_TABLE,
                    Where = new Comparison(new ColumnName(ID_COLUMN), CompareType.EQUALS, new Parameter(candidateId))
                }
            ).ExecuteAsync();

            return CheckIfSingleRowChanged(deletedRows);
        }


    }
}