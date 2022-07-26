using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRMDataManagerLibrary.Internal.DataAccess;
using TRMDataManagerLibrary.Models;

namespace TRMDataManagerLibrary.DataAccess
{
    public class UserData
    {
        private readonly IConfiguration _config;

        public UserData(IConfiguration config)
        {
            this._config = config;
        }
        // Lesson 11B: Making Back end on method using the stored procedure by SqlDataAccess
        public List<UserModel> GetUserById (string Id)
        {
            // This is not the best way to code directly but in the future it will be changed 
            SqlDataAccess sql = new SqlDataAccess(_config);
            var p = new { Id };    // Anonymous Object 
            var output = sql.LoadData<UserModel, dynamic>("dbo.spUserLookup", p, "TRMData");         
            return output;
        }
    }
}
