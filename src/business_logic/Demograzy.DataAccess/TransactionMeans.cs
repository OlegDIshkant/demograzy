using Demograzy.BusinessLogic.DataAccess;
using Npgsql;


namespace Demograzy.DataAccess
{
    public class TransactionMeans : ITransacionMeans
    {
        private NpgsqlConnection _connection;
        
        public IClientsGateway ClientsGateway { get; private set; }


        public TransactionMeans(IConnectionStringProvider connStringProvider)
        {
            _connection = DbConnection.GetNewConnection(connStringProvider) ?? throw new System.Exception("Failed to create connection with db.");   

            //ClientsGateway = null;         
        }


    }
}