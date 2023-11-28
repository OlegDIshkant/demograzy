using System;
using System.Collections.Generic;


namespace DataAccess.Sql
{
    public interface IQueryBuilder
    {
        ISqlCommand<ICollection<R>> Create<R>(SelectOptions selectOptions, Func<IRow, R> ReadRow); 
    }


    public interface IRow
    {
        int GetInt(int columnIndex);
        string GetString(int columnIndex);
        bool GetBool(int columnIndex);
        bool IsNull(int columnIndex);
    }

    

}