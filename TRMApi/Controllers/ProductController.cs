﻿using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Cashier")]
    public class ProductController : ControllerBase
    {
        private readonly IConfiguration _config;

        public ProductController(IConfiguration config)
        {
            this._config = config;
        }
        [HttpGet]
        public List<ProductModel> Get()
        {
            ProductData data = new ProductData(_config);
            var result = data.GetProducts();
            return result;
        }
    }
}
