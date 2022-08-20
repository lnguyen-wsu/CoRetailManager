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
    public class UserData : IUserData
    {
        private readonly IUserData _userData;
        private readonly ISqlDataAccess _sql;

        public UserData( ISqlDataAccess sql)
        {         
            this._sql = sql;
        }
        // Lesson 11B: Making Back end on method using the stored procedure by SqlDataAccess
        public List<UserModel> GetUserById(string Id)
        {
            // This is not the best way to code directly but in the future it will be changed                  
            var output = _sql.LoadData<UserModel, dynamic>("dbo.spUserLookup", new { Id }, "TRMData");
            return output;
        }
    }
}
