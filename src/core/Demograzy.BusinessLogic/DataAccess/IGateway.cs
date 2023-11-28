using System.Collections.Generic;
using System.Threading.Tasks;


namespace Demograzy.BusinessLogic.DataAccess
{
    public interface IGateway
    {
        Task<bool> LockAsync();
    }
}