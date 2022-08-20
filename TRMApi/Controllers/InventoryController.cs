using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
       
        private readonly IInventoryData _inventoryData;

        public InventoryController( IInventoryData inventoryData)
        {           
            this._inventoryData = inventoryData;
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpGet]
        public List<InventoryModel> Get()
        {        
            var result = _inventoryData.GetInventory();
            return result;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public void Post(InventoryModel item)
        {
            //if (RequestContext.Principal.IsInRole("Admin"))
            //{
            //    // Do admin stuff
            //}else if (RequestContext.Principal.IsInRole("Manager"))
            //{
            //    // Do Manager stuff
            //}
           
            _inventoryData.SaveInventoryRecord(item);
        }
    }
}
