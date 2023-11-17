using System.Collections.Generic;

namespace DataAccess.Sql
{
    public interface INonQueryBuilder
    {
        ISqlCommand<int> Create(InsertOptions insertOptions);
        ISqlCommand<int> Create(DeleteOptions deleteOptions);
        ISqlCommand<int> Create(UpdateOptions updateOptions);
    }

}