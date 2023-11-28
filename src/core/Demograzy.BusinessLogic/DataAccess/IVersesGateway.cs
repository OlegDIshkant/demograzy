using System.Collections.Generic;
using System.Threading.Tasks;


namespace Demograzy.BusinessLogic.DataAccess
{
    public interface IVersesGateway : IGateway
    {
        Task<int?> AddVersusAsync(int roomId, int firstCandidate, int secondCandidate);
        Task<int?> AddFictiveVersusAsync(int roomId, int singleCandidate);
        Task<VersusInfo?> GetVersusInfoAsync(int versusId);
        Task<ICollection<int>> GetUnfinishedVersesAsync(int roomId);
        Task<ICollection<int>> GetCompletedVersesWithoutFollowUpAsync(int roomId);
        Task<bool> SetVersusWinnerAsync(int versusId, bool firstIsWinner);
        Task<int?> GetVersusWinnerAsync(int versusId);
        Task<bool> SetVersusFollowUpAsync(int versusId, int referencedVersusId);
    }
}