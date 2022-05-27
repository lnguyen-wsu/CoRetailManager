using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using TRMDataManagerLibrary.DataAccess;
using TRMDataManagerLibrary.Models;

namespace TRMDataManager.Controllers
{
    [System.Web.Http.Authorize]
    public class UserController : ApiController
    {

        // Lesson 11B: Only testing the UserDataAccess we just make in the previous 
           
        public UserModel GetById()
        {        
            string userId = RequestContext.Principal.Identity.GetUserId(); // the current logging user 
            UserData data = new UserData();
            var result = data.GetUserById(userId)?.First();
            if (result != null) { result.Id = userId; }
            return result;
        }

       
    }
}
