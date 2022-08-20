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
    public class SaleData : ISaleData
    {
       
        private readonly ISqlDataAccess _sql;
        public IProductData _product { get; }
        public SaleData( IProductData product , ISqlDataAccess sql)
        {        
            _product = product;
            this._sql = sql;
        }

        public void SaveSale(SaleModel saleInfo, string cashierId)
        {
            //TODO: Make this SOLID/DRY/Better
            // Start filling in the models we will save to the database ==>  // Fill in the available info
            List<SaleDetailDBModel> details = new List<SaleDetailDBModel>();
            
            var taxRate = (decimal)ConfigHelper.GetTaxRate() / 100;
            foreach (var item in saleInfo.SaleDetails)
            {
                // Handle to get the SaleDetailDBmodel all its properties before save to the database
                var detail = new SaleDetailDBModel
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };

                // Get the info about this product from the stored procedure
                var productInfo = _product.GetProductById(detail.ProductId);
                if (productInfo == null)
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
            // Lesson 21A: C# Transaction SQL


            try
            {
                _sql.StartTransaction("TRMData");
                // Save the sale model 
                _sql.SaveDataInTransaction("dbo.spSale_Insert", sale);
                // Get the ID from the sale model
                sale.Id = _sql.LoadDataInTransaction<int, dynamic>("dbo.spSale_Lookup", new { sale.CashierId, sale.SaleDate }).FirstOrDefault();
                // Finish filling in the sale detail models.
                foreach (var item in details)
                {
                    item.SaleId = sale.Id;
                    // Save the sale detail models
                    _sql.SaveDataInTransaction("dbo.spSaleDetail_Insert", item);
                }
                _sql.CommitTransaction();
            }
            catch
            {
                _sql.RollbackTransaction();
                throw;
            }

           
        }

        // Lesson 22: Get Sale Report Model
        public List<SaleReportModel> GetSaleReport()
        {
           
            var output = _sql.LoadData<SaleReportModel, dynamic>("dbo.spSale_SaleReport", new { }, "TRMData");
            return output;
        }

    }
}
