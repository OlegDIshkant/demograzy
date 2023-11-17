
using System;
using System.Threading.Tasks;

namespace Demograzy.BusinessLogic.DataAccess
{
    public interface ITransactionMeans : IDisposable
    {
        public IClientsGateway ClientsGateway { get; }
        public IRoomsGateway RoomsGateway { get; }

        public Task FinishAsync(bool toCommit);
    }
}