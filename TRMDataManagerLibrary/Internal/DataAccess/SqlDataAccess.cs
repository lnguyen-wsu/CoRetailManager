using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRMDataManagerLibrary.Internal.DataAccess
{
    public class SqlDataAccess : IDisposable, ISqlDataAccess
    {
        private readonly IConfiguration _config;
        private readonly ILogger<SqlDataAccess> _logger;

        public SqlDataAccess(IConfiguration config, ILogger<SqlDataAccess> logger  )
        {
            this._config = config;
            this._logger = logger;
        }

        // ConfigurationManager.ConnectionStrings[name].ConnectionString;
        public string GetConnectionString(string name) => _config.GetConnectionString(name);

        // After Installed Dapper ==> Lesson 11A : Set up and Load Data
        public List<T> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName)
        {
            using (IDbConnection connection = new SqlConnection(GetConnectionString(connectionStringName)))
            {
                // using dapper to load data 
                // It means Get the connection to database
                List<T> rows = connection.Query<T>(storedProcedure, parameters,
                    commandType: CommandType.StoredProcedure).ToList();
                return rows;
            }
        }

        public void SaveData<T>(string storedProcedure, T parameters, string connectionStringName)
        {
            using (IDbConnection connection = new SqlConnection(GetConnectionString(connectionStringName)))
            {
                // using dapper to load data 
                // It means Get the connection to database
                connection.Query<T>(storedProcedure, parameters,
                    commandType: CommandType.StoredProcedure);
            }
        }
        // lesson 21A: Open SQL transaction in the C#
        private IDbConnection _connection;
        private IDbTransaction _transaction;
        public void StartTransaction(string connectionStringName)
        {
            _connection = new SqlConnection(GetConnectionString(connectionStringName));
            _connection.Open();
            _transaction = _connection.BeginTransaction();
            isClosed = false;
        }

        public void SaveDataInTransaction<T>(string storedProcedure, T parameters)
        {
            _connection.Query<T>(storedProcedure, parameters,
                commandType: CommandType.StoredProcedure, transaction: _transaction);

        }
        public List<T> LoadDataInTransaction<T, U>(string storedProcedure, U parameters)
        {

            // using dapper to load data 
            // It means Get the connection to database
            List<T> rows = _connection.Query<T>(storedProcedure, parameters,
                commandType: CommandType.StoredProcedure, transaction: _transaction).ToList();
            return rows;

        }

        private bool isClosed = false;


        public void CommitTransaction()
        {
            _transaction?.Commit();
            _connection?.Close();
            isClosed = true;
        }
        public void RollbackTransaction()
        {
            _transaction?.Rollback();
            _connection?.Close();
            isClosed = true;
        }

        public void Dispose()
        {
            if (isClosed)
            {
                try
                {
                    CommitTransaction();
                }
                catch(Exception ex)
                {
                    //TODO: Log the issue
                    _logger.LogError(ex, "Commit transaction failed in the dispose method");
                }
            }
            _transaction = null;
            _connection = null;
        }
        // Open connect/Start transaction method
        // Load using the transaction 
        // Saave using the transaction 
        // CLoe connection/stop transaction method
        // Dispose

    }
}
