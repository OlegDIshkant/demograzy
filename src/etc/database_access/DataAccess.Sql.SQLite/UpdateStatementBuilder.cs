using System.Text;

namespace DataAccess.Sql.SQLite
{
    internal static class UpdateStatementBuilder
    {
        public static string Build(UpdateOptions updateOptions, out Dictionary<string, object> parameters)
        {
            parameters = new Dictionary<string, object>();
            var b = new StringBuilder();

            b.AppendUpdateClause(updateOptions);
            b.AppendSetClause(updateOptions, parameters);
            b.AppendWhereClause(updateOptions.Where, parameters);

            return b.ToString();
        }



        private static void AppendUpdateClause(this StringBuilder b, UpdateOptions updateOptions)
        {
            b.Append("UPDATE");
            b.AppendWhitespace();
            b.Append(updateOptions.Update);
            b.AppendWhitespace();
        }



        private static void AppendSetClause(this StringBuilder b, UpdateOptions updateOptions, Dictionary<string, object> parameters)
        {
            b.Append("SET");
            b.AppendWhitespace();
            
            foreach(var setter in updateOptions.Set)
            {
                string parameterName = $"${setter.column}";
                b.Append(setter.column);
                b.Append(" = ");
                b.Append(parameterName);
                parameters.Add(parameterName, setter.value);
                b.Append(", ");
            }

            b.DropLastChars(2);
            b.AppendWhitespace();
        }




    }
}