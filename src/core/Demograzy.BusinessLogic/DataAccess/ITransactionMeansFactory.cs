
using System.Threading.Tasks;

namespace Demograzy.BusinessLogic.DataAccess
{
    public interface ITransactionMeansFactory
    {
        Task<ITransactionMeans> CreateAsync();
    }
}