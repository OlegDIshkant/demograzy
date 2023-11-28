using System.Collections.Generic;
using System.Threading.Tasks;


namespace Demograzy.BusinessLogic.DataAccess
{
    public interface IMembershipGateway : IGateway
    {
        Task<bool> AddRoomMemberAsync(int roomId, int clientId);
        Task<ICollection<int>> GetRoomMembersAsync(int roomId);
        Task<ICollection<int>> GetJoinedRoomsAsync(int clientId);
        Task<bool> ForgetAllMembersAsync(int roomId);
        Task<bool> ForgetMemberAsync(int roomId, int clientId);
    }
}