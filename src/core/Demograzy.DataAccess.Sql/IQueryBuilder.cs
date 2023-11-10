using System;
using System.Collections.Generic;


namespace Demograzy.DataAccess.Sql
{
    public interface IQueryBuilder
    {
        ISqlCommand<IQueryResult> Create(SelectOptions selectOptions); 
    }


    public interface IQueryResult : IDisposable, IEnumerable<IRow>
    {

    }


    public interface IRow
    {
        int GetInt(int columnIndex);
    }

    

}