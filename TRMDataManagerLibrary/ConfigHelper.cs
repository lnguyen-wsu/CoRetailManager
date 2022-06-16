using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRMDataManagerLibrary
{
    public class ConfigHelper
    {       
        //TODO: Move this from config to the API
        public static double GetTaxRate()
        {
            var taxRate = ConfigurationManager.AppSettings["taxRate"];
            bool isValid = double.TryParse(taxRate, out double output);
            if (isValid == false)
            {
                throw new ConfigurationErrorsException("Tax rate is invalid");
            }
            return output;
        }
        
    }
}
