using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRMDataManagerLibrary.Internal.DataAccess;
using TRMDataManagerLibrary.Models;

namespace TRMDataManagerLibrary.DataAccess
{
    public class SaleData
    {

        public void SaveSale (SaleModel saleInfo , string cashierId)
        {
            //TODO: Make this SOLID/DRY/Better
            // Start filling in the models we will save to the database ==>  // Fill in the available info
            List<SaleDetailDBModel> details = new List<SaleDetailDBModel>();
            ProductData products = new ProductData();
            var taxRate =(decimal) ConfigHelper.GetTaxRate() / 100;
            foreach (var item in saleInfo.SaleDetails)
            {
                // Handle to get the SaleDetailDBmodel all its properties before save to the database
                var detail = new SaleDetailDBModel
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity              
                };

                // Get the info about this product from the stored procedure
                var productInfo = products.GetProductById(detail.ProductId);
                if(productInfo == null)
                {
                    throw new Exception($"The product Id of {detail.ProductId} could not be found the the database");
                }
                detail.PurchasePrice = productInfo.RetailPrice * detail.Quantity;

                if (productInfo.IsTaxable)
                {
                    detail.Tax = (detail.PurchasePrice * taxRate);
                }
                details.Add(detail);
            }


            // Create the Sale model
            SaleDBModel sale = new SaleDBModel
            {
                Tax = details.Sum(x => x.Tax),
                SubTotal = details.Sum(x => x.PurchasePrice),
                CashierId = cashierId
            };
            sale.Total = sale.SubTotal + sale.Tax;
            // ==> Done the prep step, now call stored procedure and save the sale Model
            // Save the sale model 
            SqlDataAccess sql = new SqlDataAccess();
            sql.SaveData("dbo.spSale_Insert", sale, "TRMData");

            // Get the ID from the sale model
            sale.Id =  sql.LoadData<int, dynamic>("dbo.spSale_Lookup", new { sale.CashierId, sale.SaleDate }, "TRMData").FirstOrDefault();
            // Finish filling in the sale detail models.
            foreach (var item in details)
            {
                item.SaleId = sale.Id;
                // Save the sale detail models
                sql.SaveData("dbo.spSaleDetail_Insert", item, "TRMData");
            }
           
        }


        //public List<ProductModel> GetProducts()
        //{
        //    SqlDataAccess sql = new SqlDataAccess();
        //    var p = new { };    // Anonymous Object 
        //    var output = sql.LoadData<ProductModel, dynamic>("dbo.spProduct_GetAll", p, "TRMData");
        //    return output;
        //}
    }
}
