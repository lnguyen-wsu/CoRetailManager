﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRMDataManagerLibrary.Internal.DataAccess;
using TRMDataManagerLibrary.Models;

namespace TRMDataManagerLibrary.DataAccess
{
    public class ProductData
    {
        public List<ProductModel> GetProducts()
        {
            SqlDataAccess sql = new SqlDataAccess();
            var p = new { };    // Anonymous Object 
            var output = sql.LoadData<ProductModel, dynamic>("dbo.spProduct_GetAll", p, "TRMData");
            return output;
        }
    }
}
