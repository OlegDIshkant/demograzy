using System.Collections.Generic;
using System.Text;

namespace DataAccess.Sql.Common
{
    public static class UpdateStatementBuilder
    {
        public static string Build(UpdateOptions updateOptions, out Dictionary<string, object> parameters, IStatementBuildSettings settings)
        {
            parameters = new Dictionary<string, object>();
            var b = new StringBuilder();

            b.AppendUpdateClause(updateOptions);
            b.AppendSetClause(updateOptions, parameters, settings);
            b.AppendWhereClause(updateOptions.Where, parameters, settings);

            return b.ToString();
        }



        private static void AppendUpdateClause(this StringBuilder b, UpdateOptions updateOptions)
        {
            b.Append("UPDATE");
            b.AppendWhitespace();
            b.Append(updateOptions.Update);
            b.AppendWhitespace();
        }



        private static void AppendSetClause(
            this StringBuilder b, 
            UpdateOptions updateOptions, 
            Dictionary<string, object> parameters,
            IStatementBuildSettings settings)
        {
            b.Append("SET");
            b.AppendWhitespace();
            
            foreach(var setter in updateOptions.Set)
            {
                b.Append(setter.column.Value);
                b.Append(" = ");
                b.AppendItem(setter.value, parameters, settings);
                b.Append(", ");
            }

            b.DropLastChars(2);
            b.AppendWhitespace();
        }




    }
}