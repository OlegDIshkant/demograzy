using System.Collections.Generic;
using System.Threading.Tasks;


namespace Demograzy.BusinessLogic.DataAccess
{
    public interface IVotesGateway
    {
        Task<bool> CheckIfClientVotedAsync(int versusId, int clientId);
    }
}