using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TRMDataManagerLibrary.DataAccess;
using TRMDataManagerLibrary.Models;

namespace TRMDataManager.Controllers
{
    [Authorize]
    public class InventoryController : ApiController
    {
        [Authorize(Roles = "Admin,Manager")]
        public List<InventoryModel> Get()
        {
            InventoryData data = new InventoryData();
            var result = data.GetInventory();
            return result;
        }

        [Authorize(Roles = "Admin")]
        public void Post(InventoryModel item)
        {
            //if (RequestContext.Principal.IsInRole("Admin"))
            //{
            //    // Do admin stuff
            //}else if (RequestContext.Principal.IsInRole("Manager"))
            //{
            //    // Do Manager stuff
            //}
            InventoryData data = new InventoryData();
            data.SaveInventoryRecord(item);
        }
    }
}
