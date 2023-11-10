using System.Collections.Generic;

namespace Demograzy.DataAccess.Sql
{
    public sealed class InsertOptions
    {
        public List<(string, object)> Values { get; set; }
        public string Into { get; set; }
    }

}