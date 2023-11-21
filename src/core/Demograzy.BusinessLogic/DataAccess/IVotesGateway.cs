using System.Collections.Generic;
using System.Threading.Tasks;


namespace Demograzy.BusinessLogic.DataAccess
{
    public interface IVotesGateway
    {
        Task<bool> AddVoteAsync(int versusId, int clientId, bool votedForFirst);
        Task<int> GetVotesAmountAsync(int versusId);
        Task<int> GetVotesAmountForFirstCandidateAsync(int versusId);
        Task<bool> CheckIfClientVotedAsync(int versusId, int clientId);
    }
}