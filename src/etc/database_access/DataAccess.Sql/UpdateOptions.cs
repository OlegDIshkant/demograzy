using System.Collections.Generic;
using System.Linq;


namespace DataAccess.Sql
{
    public sealed class UpdateOptions
    {
        public string Update { get; set; }
        public List<(ColumnName column, Parameter value)> Set { get; set; }
        public IWhereClause Where { get; set; }
    }


    


}