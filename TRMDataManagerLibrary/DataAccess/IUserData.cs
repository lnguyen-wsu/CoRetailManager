using System.Collections.Generic;
using TRMDataManagerLibrary.Models;

namespace TRMDataManagerLibrary.DataAccess
{
    public interface IUserData
    {
        List<UserModel> GetUserById(string Id);
    }
}