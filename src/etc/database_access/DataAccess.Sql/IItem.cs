namespace DataAccess.Sql
{
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
}