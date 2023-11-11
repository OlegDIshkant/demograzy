using System.Collections.Generic;
using System.Data.Common;
using System.Linq;


namespace DataAccess.Sql
{
    public sealed class SelectOptions
    {
        public SelectClause Select { get; set; }
        public string From { get; set; }
        public IWhereClause Where { get; set; }
    }


    public sealed class SelectClause
    {
        public List<IItem> Items { get; private set; }
        public SelectClause(params IItem[] items)
        {
            Items = items.ToList();
        }
    }


    public interface IItem { }
    public struct ColumnName : IItem
    {  
        public string Value { get; set; }  
        public ColumnName(string value) { Value = value; }
    }
    public class Parameter : IItem
    {
        private static int autoIncr = 0;

        public int RefIndex { get; private set; }
        public object Value { get; set; }  
        public Parameter(object value)
        { 
            Value = value; 
            RefIndex = autoIncr;
        }
    }
    public interface Function : IItem { }
    public struct Count : Function { }
    public struct LastInsertedRowId : Function { }
    


    public interface IWhereClause { }
    public struct Comparison : IWhereClause
    {
        public IItem Left { get; set; }  
        public IItem Right { get; set; } 
        public CompareType CompareType { get; set; }
        public Comparison(IItem left, CompareType compareType, IItem right)
        {
            Left = left;
            Right = right;
            CompareType = compareType;
        } 
    }


    public enum CompareType
    {
        EQUALS,
        MORE_THAN,
        LESS_THAN
    };

}