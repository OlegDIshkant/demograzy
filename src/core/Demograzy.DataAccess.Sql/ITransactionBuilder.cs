namespace Demograzy.DataAccess.Sql
{
    public interface ITransactionBuilder
    {
        ISqlCommand<object> Begin();
        ISqlCommand<object> Commit();
        ISqlCommand<object> Rollback(); 
    }
}