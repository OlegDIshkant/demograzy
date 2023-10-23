

namespace Demograzy.BusinessLogic.DataAccess
{
    public interface IClientsGateway
    {
        bool TryAddNewClient(out int clientId, out string sessionId);
        bool TryRemoveClient(int clientId);
    }
}