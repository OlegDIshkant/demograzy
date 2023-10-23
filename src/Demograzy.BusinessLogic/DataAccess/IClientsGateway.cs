using System.Threading.Tasks;


namespace Demograzy.BusinessLogic.DataAccess
{
    public interface IClientsGateway
    {
        Task<bool> TryAddClientAsync(int id, string name, string sessionId);
        Task<(string name, string sessionId)?> TryFindClientByIdAsync(int id);
        Task<bool> TryDropClientAsync(int id);
    }
}