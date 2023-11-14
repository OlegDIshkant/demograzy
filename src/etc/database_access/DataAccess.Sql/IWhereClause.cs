namespace DataAccess.Sql
{

    public interface IWhereClause { }
    public class Comparison : IWhereClause
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