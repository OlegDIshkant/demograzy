using System.Collections.Generic;
using System.Threading.Tasks;


namespace Demograzy.BusinessLogic.DataAccess
{
    public interface IVersesGateway
    {
        Task<int?> AddVersusAsync(int roomId, int firstCandidate, int secondCandidate);
        Task<List<int>> GetUnfinishedVersesAsync(int roomId);
    }
}