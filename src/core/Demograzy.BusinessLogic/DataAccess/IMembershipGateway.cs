using System.Collections.Generic;
using System.Threading.Tasks;


namespace Demograzy.BusinessLogic.DataAccess
{
    public interface IMembershipGateway
    {
        Task<bool> AddRoomMemberAsync(int roomId, int clientId);
        Task<List<int>> GetRoomMembersAsync(int roomId);
        Task<List<int>> GetJoinedRoomsAsync(int clientId);
    }
}