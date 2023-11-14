using System.Text;

namespace DataAccess.Sql.SQLite
{
    internal static class DeleStatementBuilder
    {
        public static string Build(DeleteOptions deleteOptions, out Dictionary<string, object> parameters)
        {
            parameters = new Dictionary<string, object>();
            var b = new StringBuilder();

            b.Append($"DELETE FROM {deleteOptions.From}");
            b.AppendWhitespace();
            b.AppendWhereClause(deleteOptions.Where, parameters);

            return b.ToString();
        }
    }
}