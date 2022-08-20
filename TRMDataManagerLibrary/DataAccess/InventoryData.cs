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
    public class InventoryData : IInventoryData
    {
        private readonly IConfiguration _config;
        private readonly ISqlDataAccess _sqlDataAccess;

        public InventoryData(IConfiguration config, ISqlDataAccess sqlDataAccess)
        {
            this._config = config;
            this._sqlDataAccess = sqlDataAccess;
        }
        public List<InventoryModel> GetInventory()
        {
            
            return _sqlDataAccess.LoadData<InventoryModel, dynamic>("dbo.spInventory_GetAll", new { }, "TRMData");
        }

        public void SaveInventoryRecord (InventoryModel item)
        {
           
            _sqlDataAccess.SaveData("dbo.spInventory_Insert", item, "TRMData");
        }
    }
}
