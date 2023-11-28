using System.Threading.Tasks;


namespace Demograzy.BusinessLogic.DataAccess
{
    public interface IClientsGateway : IGateway
    {
        Task<int> AddClientAsync(string name);
        Task<ClientInfo?> GetClientInfoAsync(int id);
        Task<bool> CheckClientExistsAsync(int id);
        Task<int> GetClientsAmountAsync();
        Task<bool> DropClientAsync(int id);
    }
}