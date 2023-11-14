using System.Text;

namespace DataAccess.Sql.SQLite
{
    internal static class SelectStatementBuilder
    {
        public static string Build(SelectOptions selectOptions, out Dictionary<string, object> parameters)
        {
            parameters = new Dictionary<string, object>();
            var b = new StringBuilder();

            b.AppendSelectClause(selectOptions);
            b.AppendFromClause(selectOptions);
            b.AppendWhereClause(selectOptions.Where, parameters);

            return b.ToString();
        }



        private static void AppendSelectClause(this StringBuilder b, SelectOptions selectOptions)
        {
            if ((selectOptions.Select?.Items?.Count ?? 0) <= 0)
            {
                throw new ArgumentException("No columns presented in SELECT statement.");
            }

            b.Append("SELECT ");
            foreach(var item in selectOptions.Select.Items)
            {
                b.AppendItem(item, null);
                b.Append(", ");
            }
            b.DropLastChars(2);
            b.AppendWhitespace();
        }



        private static void AppendFromClause(this StringBuilder b, SelectOptions selectOptions)
        {
            if (selectOptions.From != null)
            {
                b.Append("FROM ");
                b.Append(selectOptions.From);
                b.AppendWhitespace();
            }

        }

    }
}