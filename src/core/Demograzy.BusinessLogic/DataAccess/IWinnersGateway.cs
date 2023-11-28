using System.Collections.Generic;
using System.Threading.Tasks;


namespace Demograzy.BusinessLogic.DataAccess
{
    public interface IWinnersGateway : IGateway
    {
        Task<bool> AddWinnerAsync(int roomId, int winnerId);
        Task<int?> GetWinnerAsync(int roomId);
    }
}