using System.Collections.Generic;
using System.Threading.Tasks;


namespace Demograzy.BusinessLogic.DataAccess
{
    public interface IVersesGateway
    {
        Task<List<int>> GetUnfinishedVersesAsync(int roomId);
    }
}