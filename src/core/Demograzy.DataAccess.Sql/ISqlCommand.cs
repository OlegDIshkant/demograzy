using System.Threading.Tasks;

namespace Demograzy.DataAccess.Sql
{
    public interface ISqlCommand<R>
    {
        Task<R> ExecuteAsync();
    }
}