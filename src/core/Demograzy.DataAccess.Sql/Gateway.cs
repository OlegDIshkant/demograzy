using System;
using System.Collections.Generic;
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




        protected bool CheckIfSingleRowChanged(int changedRowsAmount)
        {
            if (changedRowsAmount == 0)
            {
                return false;
            } 
            else if (changedRowsAmount == 1)
            {
                return true;
            }
            else
            {
                throw new Exception($"Unexpected amount of updated rows: '{changedRowsAmount}' (should be 1 or 0).");
            }
        }


        protected async Task<List<R>> InvokeQuery<R>(ISqlCommand<IQueryResult> query, Func<IRow, R> ExtractRowInfo)
        {
            using (var queryResult = await query.ExecuteAsync())
            {
                var result = new List<R>();
                var e = queryResult.GetEnumerator();
                while(e.MoveNext())
                {
                    result.Add(ExtractRowInfo(e.Current));
                }
                return result;
            }
        }

    }
}