using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Sql;
using Demograzy.BusinessLogic.DataAccess;


namespace Demograzy.DataAccess.Sql
{
    internal abstract class Gateway : IGateway
    {
        private Func<IQueryBuilder> _PeekQueryBuilder;
        private Func<INonQueryBuilder> _PeekNonQueryBuilder;
        private Func<ILockCommandsBuilder> _PeekLockCommandsBuilder;


        protected IQueryBuilder QueryBuilder => _PeekQueryBuilder();
        protected INonQueryBuilder NonQueryBuilder => _PeekNonQueryBuilder();

        protected abstract string TableName { get; }

        public Gateway(
            Func<IQueryBuilder> PeekQueryBuilder,
            Func<INonQueryBuilder> PeekNonQueryBuilder,
            Func<ILockCommandsBuilder> PeekLockCommandsBuilder)
        {
            _PeekQueryBuilder = PeekQueryBuilder;
            _PeekNonQueryBuilder = PeekNonQueryBuilder;
            _PeekLockCommandsBuilder = PeekLockCommandsBuilder;
        }




        protected async Task<int> GetLastInsertedIdAsync()
        {
            var queryResult =
                await _PeekQueryBuilder().Create(
                    new SelectOptions()
                    {
                        Select = new SelectClause(new LastInsertedRowId())
                    }
                    ,
                    r => r.GetInt(0)
                ).ExecuteAsync();

            return queryResult.Single();
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




        protected R? SingleOrNull<R>(ICollection<R> collection)
            where R : struct
        {
            if (collection.Any())
            {
                return collection.Single();
            } 
            return null;
        }

        public Task<bool> LockAsync()
        {
            return _PeekLockCommandsBuilder().Create(TableName).ExecuteAsync();
        }

    }
}