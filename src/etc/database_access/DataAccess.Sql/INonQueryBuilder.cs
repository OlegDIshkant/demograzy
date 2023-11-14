using System.Collections.Generic;

namespace DataAccess.Sql
{
    public interface INonQueryBuilder
    {
        ISqlCommand<bool> Create(InsertOptions insertOptions);
        ISqlCommand<bool> Create(DeleteOptions deleteOptions);
    }

}