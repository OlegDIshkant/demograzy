using System.Collections.Generic;
using System.Threading.Tasks;


namespace Demograzy.BusinessLogic.DataAccess
{
    public interface IVersesGateway
    {
        Task<int?> AddVersusAsync(int roomId, int firstCandidate, int secondCandidate);
        Task<int?> AddFictiveVersusAsync(int roomId, int singleCandidate);
        Task<VersusInfo?> GetVersusInfoAsync(int versusId);
        Task<List<int>> GetUnfinishedVersesAsync(int roomId);
        Task<List<int>> GetCompletedVersesWithoutFollowUpAsync(int roomId);
        Task<bool> SetVersusWinnerAsync(int versusId, bool firstIsWinner);
        Task<int?> GetVersusWinnerAsync(int versusId);
        Task<bool> SetVersusFollowUpAsync(int versusId, int referencedVersusId);
    }
}