using System.Collections.Generic;
using System.Threading.Tasks;


namespace Demograzy.BusinessLogic.DataAccess
{
    public interface IWinnersGateway
    {
        Task<int?> GetWinnerAsync(int roomId);
    }
}