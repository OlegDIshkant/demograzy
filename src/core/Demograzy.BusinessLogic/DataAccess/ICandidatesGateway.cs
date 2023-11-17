using System.Threading.Tasks;


namespace Demograzy.BusinessLogic.DataAccess
{
    public interface ICandidatesGateway
    {
        Task<int?> AddCandidateAsync(int roomId, string name);
        Task<CandidateInfo?> GetCandidateInfo(int candidateId);
    }
}