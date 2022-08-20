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
    public class ProductData : IProductData
    {
        private readonly ISqlDataAccess _sql;

        public ProductData( ISqlDataAccess sql)
        {
            this._sql = sql;
        }
        public List<ProductModel> GetProducts()
        {          
            var p = new { };    // Anonymous Object 
            var output = _sql.LoadData<ProductModel, dynamic>("dbo.spProduct_GetAll", p, "TRMData");
            return output;
        }

        public ProductModel GetProductById(int productId)
        {
            
            var output = _sql.LoadData<ProductModel, dynamic>("dbo.spProduct_GetById", new { Id = productId }, "TRMData").FirstOrDefault();
            return output;
        }
    }
}
