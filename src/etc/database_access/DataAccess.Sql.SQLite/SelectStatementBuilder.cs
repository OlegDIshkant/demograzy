using System.Text;

namespace DataAccess.Sql.SQLite
{
    internal static class SelectStatementBuilder
    {
        public static string Build(SelectOptions selectOptions, out Dictionary<string, object> parameters)
        {
            var p = new Dictionary<string, object>();
            var b = new StringBuilder();

            b.AppendSelectClause(selectOptions);
            b.AppendFromClause(selectOptions);
            b.AppendWhereClause(selectOptions, (k, v) => p.Add(k, v));

            parameters = p;
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
                b.AppendItem(item, (_, _) => throw new NotSupportedException());
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



        private static void AppendWhereClause(this StringBuilder b, SelectOptions selectOptions, Action<string, object> OnNewParameter)
        {
            if (selectOptions.Where != null)
            {
                b.Append("WHERE ");

                if (selectOptions.Where is Comparison comparison)
                {
                    b.AppendItem(comparison.Left, OnNewParameter);
                    b.AppendWhitespace();
                    b.AppendComparisonSign(comparison.CompareType);
                    b.AppendWhitespace();
                    b.AppendItem(comparison.Right, OnNewParameter);
                }
                else 
                {
                    throw new NotSupportedException($"Where clause with type '{selectOptions.Where.GetType()}' is not supported yet.");
                }

                b.AppendWhitespace();
            }

        }


        private static void AppendItem(this StringBuilder b, IItem item, Action<string, object> OnNewParameter)
        {
            if (item is ColumnName columnName)
            {
                b.Append($"{columnName.Value}");
            }
            else if (item is Parameter parameter)
            {
                var @ref = $"${parameter.RefIndex}";
                b.Append(@ref);
                OnNewParameter(@ref, parameter.Value);
            }
            else if (item is Count)
            {
                b.Append("count(*)");
            }
            else if (item is LastInsertedRowId)
            {
                b.Append("last_insert_rowid()");
            }
            else 
            {
                throw new NotSupportedException($"Item with type '{item.GetType()}' is not supported yet.");
            }
        }

        private static void AppendWhitespace(this StringBuilder b)
        {
            b.Append(' ');
        }

        private static void DropLastChars(this StringBuilder b, int amount)
        {
            b.Remove(b.Length - amount, amount);
        }


        private static void AppendComparisonSign(this StringBuilder b, CompareType compareType)
        {
            if (compareType is CompareType.EQUALS)
            {
                b.Append("==");
            }
            else if (compareType is CompareType.MORE_THAN)
            {
                b.Append(">");
            }
            else if (compareType is CompareType.LESS_THAN)
            {
                b.Append("<");
            }
            else 
            {
                throw new NotSupportedException($"Compare type '{compareType}' is not supported yet.");
            }
        }

    }
}