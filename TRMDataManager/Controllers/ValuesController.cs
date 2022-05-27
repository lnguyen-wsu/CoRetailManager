using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TRMDataManager.Controllers
{
    [Authorize]
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            // Lesson 1a : get the register User ID and send it back to response
            string userId = RequestContext.Principal.Identity.GetUserId();
            string email = RequestContext.Principal.Identity.GetUserName();          
            return new string[] { "value1", "value2", userId };
        }

        // Lesson 1b : we can use IHttp IHttpActionResult
        //public IHttpActionResult GetIHttpActionResult()
        //{
        //    // Lesson 1a : get the register User ID and send it back to response
        //    string userId = RequestContext.Principal.Identity.GetUserId();
        //    string email = RequestContext.Principal.Identity.GetUserName();
        //    return Ok(new string[] { "value1", "value2", userId });
        //}

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
