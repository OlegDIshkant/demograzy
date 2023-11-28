using System.Collections.Generic;

namespace DataAccess.Sql
{
    public sealed class InsertOptions
    {
        public List<(ColumnName, Parameter)> Values { get; set; }
        public string Into { get; set; }
    }

}