
using System;

namespace Demograzy.BusinessLogic.DataAccess
{
    public interface ITransactionMeans : IDisposable
    {
        public IClientsGateway ClientsGateway { get; }
    }
}