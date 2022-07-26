using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TRMDataManagerLibrary.DataAccess;
using TRMDataManagerLibrary.Models;

namespace TRMApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SaleController : ControllerBase
    {

        private readonly IConfiguration _config;

        public SaleController(IConfiguration config)
        {
            this._config = config;
        }
        [Authorize(Roles = "Cashier")]
        // This method is post from the UI ==> Post to API ==> post to the database
        public void Post(SaleModel sale)
        {
            SaleData data = new SaleData(_config);
            //string userId = RequestContext.Principal.Identity.GetUserId();
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            data.SaveSale(sale, userId);
        }


        // lesson 22a
        [Authorize(Roles = "Admin,Manager")]
        [Route("GetSalesReport")]
        public List<SaleReportModel> GetSalesReport()
        {
            SaleData data = new SaleData(_config);
            var result = data.GetSaleReport();
            return result;
        }
    }
}
