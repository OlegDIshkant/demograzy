
using System;
using System.Threading.Tasks;

namespace Demograzy.BusinessLogic.DataAccess
{
    public interface ITransactionMeans : IDisposable
    {
        public IClientsGateway ClientsGateway { get; }
        public IRoomsGateway RoomsGateway { get; }
        public IMembershipGateway MembershipGateway { get; }
        public ICandidatesGateway CandidatesGateway { get; }
        public IWinnersGateway WinnersGateway { get; }

        public Task FinishAsync(bool toCommitInsteadOfRollback);
    }
}