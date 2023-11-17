using System.Collections.Generic;
using System.Threading.Tasks;


namespace Demograzy.BusinessLogic.DataAccess
{
    public interface ICandidatesGateway
    {
        Task<int?> AddCandidateAsync(int roomId, string name);
        Task<CandidateInfo?> GetCandidateInfo(int candidateId);
        Task<List<int>> GetCandidates(int roomId);
        Task<int> GetCandidatesAmount(int roomId);
    }
}