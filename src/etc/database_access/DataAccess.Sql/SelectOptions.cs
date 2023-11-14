using System.Collections.Generic;
using System.Linq;


namespace DataAccess.Sql
{
    public sealed class SelectOptions
    {
        public SelectClause Select { get; set; }
        public string From { get; set; }
        public IWhereClause Where { get; set; }
    }


    public sealed class SelectClause
    {
        public List<IItem> Items { get; private set; }
        public SelectClause(params IItem[] items)
        {
            Items = items.ToList();
        }
    }


    


}