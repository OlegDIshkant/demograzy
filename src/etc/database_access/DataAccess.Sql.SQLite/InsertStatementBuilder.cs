using System.Text;

namespace DataAccess.Sql.SQLite
{
    internal static class InsertStatementBuilder
    {
        public static string Build(InsertOptions insertOptions, out Dictionary<string, object> parameters)
        {
            InsertParameters(insertOptions, out parameters);

            var b = new StringBuilder();

            b.Append($"INSERT INTO {insertOptions.Into} (");
            foreach (var (itemName, _) in insertOptions.Values)
            {
                b.Append($"{itemName}, ");
            }
            b.Remove(b.Length - 2, 2);

            b.Append(") VALUES (");
            foreach (var (itemName, _) in insertOptions.Values)
            {
                b.Append($"${itemName}, ");
            }
            b.Remove(b.Length - 2, 2);
            b.Append(")");

            return b.ToString();
        }



        private static void InsertParameters(InsertOptions insertOptions, out Dictionary<string, object> parameters)
        {
            parameters = new Dictionary<string, object>();

            foreach (var (itemName, itemValue) in insertOptions.Values)
            {
                parameters.Add($"${itemName}", itemValue);
            }
        }

    }
}