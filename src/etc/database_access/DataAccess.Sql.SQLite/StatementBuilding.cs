using System.Text;

namespace DataAccess.Sql.SQLite
{
    internal static class StatementBuilding
    {
        private static readonly string AND_COPULA = " AND ";

        public static void AppendWhereClause(this StringBuilder b, IWhereClause whereClause, Dictionary<string, object> parameters)
        {
            if (whereClause != null)
            {
                b.Append("WHERE ");

                if (whereClause is IComparison comparison)
                {
                    b.AppendComparison(comparison, parameters);
                }
                else 
                {
                    throw new NotSupportedException($"Where clause with type '{whereClause.GetType()}' is not supported yet.");
                }

                b.AppendWhitespace();
            }

        }


        private static void AppendComparison(this StringBuilder b, IComparison comparison, Dictionary<string, object> parameters)
        {
                if (comparison is Comparison simpleComparison)
                {
                    b.AppendItem(simpleComparison.Left, parameters);
                    b.AppendWhitespace();
                    b.AppendComparisonSign(simpleComparison.CompareType);
                    b.AppendWhitespace();
                    b.AppendItem(simpleComparison.Right, parameters);
                }
                else if (comparison is MultiComparison multiComparison)
                {
                    b.Append("(");

                    var copula = FigureOutCopula(multiComparison.Type);

                    foreach (var comp in multiComparison.Comparisons)
                    {
                        b.AppendComparison(comp, parameters);
                        b.Append(copula);        
                    }

                    b.DropLastChars(copula.Length);
                    b.Append(")");
                }
                else if (comparison is NullComparison nullComparison)
                {
                    b.AppendItem(nullComparison.Item, parameters);
                    b.AppendWhitespace();
                    b.AppendNullComparison(nullComparison.CompareType);
                }
                else 
                {
                    throw new NotSupportedException($"Comparison with type '{comparison.GetType()}' is not supported yet.");
                }
        }



        private static string FigureOutCopula(MultiComparison.Types copulaType)
        {
            if (copulaType == MultiComparison.Types.AND)
            {
                return AND_COPULA;
            }  
            throw new NotImplementedException();
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


        private static void AppendNullComparison(this StringBuilder b, NullCompareType compareType)
        {
            if (compareType is NullCompareType.IS_NULL)
            {
                b.Append("IS NULL");
            }
            else if (compareType is NullCompareType.IS_NOT_NULL)
            {
                b.Append("IS NOT NULL");
            }
            else 
            {
                throw new NotSupportedException($"Compare type '{compareType}' is not supported yet.");
            }
        }
    }
}