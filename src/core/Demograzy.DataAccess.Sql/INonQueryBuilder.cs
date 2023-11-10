using System.Collections.Generic;

namespace Demograzy.DataAccess.Sql
{
    public interface INonQueryBuilder
    {
        ISqlCommand<bool> Create(InsertOptions insertOptions);
    }

}