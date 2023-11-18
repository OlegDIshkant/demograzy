using System.Collections.Generic;
using System.Threading.Tasks;


namespace Demograzy.BusinessLogic.DataAccess
{
    public interface IRoomsGateway
    {
        Task<int?> AddRoomAsync(int ownerId, string title, string passphrase);
        Task<List<int>> GetOwnedRoomsAsync(int ownerId);
        Task<RoomInfo?> GetRoomInfoAsync(int roomId);
        Task<bool> StartVotingAsync(int roomId);
    }
}