#nullable disable

using System;
using Common;
using Demograzy.BusinessLogic.DataAccess;
using Npgsql;


namespace Demograzy.DataAccess
{
    public class TransactionMeans : DisposableObject, ITransactionMeans
    {
        private NpgsqlConnection _connection;
        private NpgsqlTransaction _currentTransaction;

        public IClientsGateway ClientsGateway { get; private set; }



        public TransactionMeans(IConnectionStringProvider connStringProvider)
        {
            _connection = DbConnection.GetNewConnection(connStringProvider) ?? throw new System.Exception("Failed to create connection with db.");   

            ClientsGateway = new ClientsGateway(() => _connection);   

            _currentTransaction = _connection.BeginTransaction();
        }


        public void CompleteTransaction()
        {
            if (_currentTransaction == null)
            {
                throw new InvalidOperationException($"Can't complete transaction since it already finished.");
            }
            _currentTransaction.Commit();
            _currentTransaction = null;
        }


        protected override void OnDispose()
        {
            base.OnDispose();

            if (_currentTransaction != null)
            {
                _currentTransaction.Rollback();
                _currentTransaction = null;
            }
        }

    }
}