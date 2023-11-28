using System.Collections.Generic;
using System.Text;

namespace DataAccess.Sql.Common
{
    public static class InsertStatementBuilder
    {
        public static string Build(InsertOptions insertOptions, out Dictionary<string, object> parameters, IStatementBuildSettings settings)
        {
            parameters = new Dictionary<string, object>();
            var b = new StringBuilder();

            b.AppendInsertClause(insertOptions);
            b.AppendValuesClause(insertOptions, parameters, settings);

            return b.ToString();
        }



        private static void AppendInsertClause(
            this StringBuilder b, 
            InsertOptions insertOptions)
        {

            b.Append($"INSERT INTO {insertOptions.Into}");
            b.AppendWhitespace();
            b.Append('(');
            foreach (var (columnName, _) in insertOptions.Values)
            {
                b.Append(columnName.Value);
                b.Append(", ");
            }
            b.Remove(b.Length - 2, 2);
            b.Append(')');
            b.AppendWhitespace();
        }



        private static void AppendValuesClause(
            this StringBuilder b, 
            InsertOptions insertOptions, 
            Dictionary<string, object> parameters,
            IStatementBuildSettings settings)
        {
            b.Append("VALUES");
            b.AppendWhitespace();
            b.Append('(');
            foreach (var (_, parameter) in insertOptions.Values)
            {
                b.AppendItem(parameter, parameters, settings);
                b.Append(", ");
            }
            b.Remove(b.Length - 2, 2);
            b.Append(")");

        }


    }
}