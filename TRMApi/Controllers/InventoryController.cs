using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TRMDataManagerLibrary.DataAccess;
using TRMDataManagerLibrary.Models;

namespace TRMApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InventoryController : ControllerBase
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
