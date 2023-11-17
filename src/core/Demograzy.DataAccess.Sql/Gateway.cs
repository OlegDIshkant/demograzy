using System;
using System.Threading.Tasks;
using DataAccess.Sql;


namespace Demograzy.DataAccess.Sql
{
    internal abstract class Gateway 
    {
        private Func<IQueryBuilder> _PeekQueryBuilder;
        private Func<INonQueryBuilder> _PeekNonQueryBuilder;


        protected IQueryBuilder QueryBuilder => _PeekQueryBuilder();
        protected INonQueryBuilder NonQueryBuilder => _PeekNonQueryBuilder();


        public Gateway(Func<IQueryBuilder> PeekQueryBuilder, Func<INonQueryBuilder> PeekNonQueryBuilder)
        {
            _PeekQueryBuilder = PeekQueryBuilder;
            _PeekNonQueryBuilder = PeekNonQueryBuilder;
        }




        protected async Task<int> GetLastInsertedIdAsync()
        {
            var query =
                _PeekQueryBuilder().Create(
                    new SelectOptions()
                    {
                        Select = new SelectClause(new LastInsertedRowId())
                    }
                );

            using (var result = (await query.ExecuteAsync()).GetEnumerator())
            {
                result.MoveNext();
                return result.Current.GetInt(0);
            }
        }


    }
}