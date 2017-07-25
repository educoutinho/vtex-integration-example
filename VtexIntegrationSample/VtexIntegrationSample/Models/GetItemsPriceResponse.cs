using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enginesoft.VtexIntegrationSample.Models
{
    public class GetItemsPriceResponse
    {
        public GetItemsPriceStatusEnum Status { get; private set; }

        public string Message { get; private set; }

        public string ServiceCode { get; private set; }
        
        public List<ItemPrice> ItemsList { get; private set; }
        
        public List<ShippingInformation> ConsolidatedShippingInformations { get; private set; }

        public List<PaymentCondition> PaymentConditionsList { get; private set; }

        public GetItemsPriceResponse(GetItemsPriceStatusEnum status, string message, string serviceCode, 
            List<ItemPrice> itemsList, List<ShippingInformation> consolidatedShippingInformationsList, List<PaymentCondition> paymentConditionsList)
        {
            this.Status = status;
            this.Message = message;
            this.ServiceCode = serviceCode;
            this.ItemsList = itemsList;
            this.ConsolidatedShippingInformations = consolidatedShippingInformationsList;
            this.PaymentConditionsList = paymentConditionsList;
        }
    }
}
