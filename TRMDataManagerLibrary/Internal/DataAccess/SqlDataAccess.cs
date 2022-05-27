using Dapper;
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
    internal class SqlDataAccess
    {
        public string GetConnectionString(string name) => ConfigurationManager.ConnectionStrings[name].ConnectionString;

        // After Installed Dapper ==> Lesson 11A : Set up and Load Data
        public List<T> LoadData<T,U> (string storedProcedure, U parameters, string connectionStringName)
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

    }
}
