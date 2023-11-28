using System.Collections.Generic;
using System.Text;

namespace DataAccess.Sql.Common
{
    public static class DeleStatementBuilder
    {
        public static string Build(DeleteOptions deleteOptions, out Dictionary<string, object> parameters, IStatementBuildSettings settings)
        {
            parameters = new Dictionary<string, object>();
            var b = new StringBuilder();

            b.Append($"DELETE FROM {deleteOptions.From}");
            b.AppendWhitespace();
            b.AppendWhereClause(deleteOptions.Where, parameters, settings);

            return b.ToString();
        }
    }
}