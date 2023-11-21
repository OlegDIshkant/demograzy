using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Sql
{

    public interface IWhereClause { }



    public interface IComparison : IWhereClause
    {

    }

    public class Comparison : IComparison
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

    public class MultiComparison : IComparison
    {
        public enum Types { AND, OR }

        private List<IComparison> _comparisons = new List<IComparison>();

        public IReadOnlyList<IComparison> Comparisons => _comparisons;
        public Types Type { get; private set; }

        public static MultiComparison And(params IComparison[] comparisons)
        {            
            var instance = new MultiComparison();
            instance._comparisons = comparisons.ToList();
            instance.Type = Types.AND;
            return instance;
        }

        private MultiComparison()
        {            
        }
    }


    public enum CompareType
    {
        EQUALS,
        MORE_THAN,
        LESS_THAN
    };


    public class NullComparison : IComparison
    {
        public IItem Item { get; set; }  
        public NullCompareType CompareType { get; set; }
        public NullComparison(IItem item, NullCompareType compareType)
        {
            Item = item;
            CompareType = compareType;
        } 
    }

    public enum NullCompareType
    {
        IS_NULL,
        IS_NOT_NULL
    };
}