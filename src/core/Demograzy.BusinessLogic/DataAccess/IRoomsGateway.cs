using System.Collections.Generic;
using System.Threading.Tasks;


namespace Demograzy.BusinessLogic.DataAccess
{
    public interface IRoomsGateway
    {
        Task<int?> AddRoomAsync(int ownerId, string title, string passphrase);
        Task<RoomInfo?> GetRoomInfoAsync(int roomId);
        Task<bool> StartVotingAsync(int roomId);
    }
}