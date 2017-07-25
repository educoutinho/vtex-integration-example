using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enginesoft.VtexIntegrationSample.Models
{
    public class GetItemsPriceRequest
    {
        public Models.ClientIntegration ClientIntegration { get; private set; }

        public string Zipcode { get; private set; }

        public List<Models.ItemGetItemsPriceRequest> ItemsList { get; private set; }

        public string SupplierPaymentConditionCode { get; private set; }
        
        public GetItemsPriceRequest(Models.ClientIntegration clientIntegration, string zipcode, List<Models.ItemGetItemsPriceRequest> itemsList, string supplierPaymentConditionCode)
        {
            this.ClientIntegration = clientIntegration;
            this.Zipcode = zipcode;
            this.ItemsList = itemsList;
            this.SupplierPaymentConditionCode = supplierPaymentConditionCode;
        }
    }
}
