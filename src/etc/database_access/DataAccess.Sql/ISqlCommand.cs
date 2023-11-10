using System.Threading.Tasks;

namespace DataAccess.Sql
{
    public interface ISqlCommand<R>
    {
        Task<R> ExecuteAsync();
    }
}