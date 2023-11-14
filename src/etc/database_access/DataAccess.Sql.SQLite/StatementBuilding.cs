using System.Text;

namespace DataAccess.Sql.SQLite
{
    internal static class StatementBuilding
    {
        public static void AppendWhereClause(this StringBuilder b, IWhereClause whereClause, Dictionary<string, object> parameters)
        {
            if (whereClause != null)
            {
                b.Append("WHERE ");

                if (whereClause is Comparison comparison)
                {
                    b.AppendItem(comparison.Left, parameters);
                    b.AppendWhitespace();
                    b.AppendComparisonSign(comparison.CompareType);
                    b.AppendWhitespace();
                    b.AppendItem(comparison.Right, parameters);
                }
                else 
                {
                    throw new NotSupportedException($"Where clause with type '{whereClause.GetType()}' is not supported yet.");
                }

                b.AppendWhitespace();
            }

        }
      


        public static void AppendItem(this StringBuilder b, IItem item, Dictionary<string, object> parameters)
        {
            if (item is ColumnName columnName)
            {
                b.Append($"{columnName.Value}");
            }
            else if (item is Parameter parameter)
            {
                var @ref = $"${parameter.RefIndex}";
                b.Append(@ref);
                parameters.Add(@ref, parameter.Value);
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

        public static void AppendWhitespace(this StringBuilder b)
        {
            b.Append(' ');
        }

        public static void DropLastChars(this StringBuilder b, int amount)
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