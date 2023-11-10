using System.Collections.Generic;


namespace Demograzy.DataAccess.Sql
{
    public sealed class SelectOptions
    {
        public List<IColumnOption> Select { get; set; }
        public string From { get; set; }
    }


    public interface IColumnOption { }
    public struct StandardColumn : IColumnOption {  public string Name { get; set; }  }
    public interface Function : IColumnOption { }
    public struct Count : Function { }
    public struct LastInsertedRowId : Function { }
    

}