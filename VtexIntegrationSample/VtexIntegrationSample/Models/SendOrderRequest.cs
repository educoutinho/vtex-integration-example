using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enginesoft.VtexIntegrationSample.Models
{
    public class SendOrderRequest
    {
        public Models.ClientIntegration ClientIntegration { get; private set; }

        public decimal Total { get; private set; }

        public List<SendOrderRequestItem> ItemsList { get; private set; }

        public Address DeliveryAddress { get; private set; }

        public PaymentConditionInformation PaymentConditionInformation { get; private set; }

        public Dictionary<string, string> Coookies { get; private set; }

        public SendOrderRequest(Models.ClientIntegration clientIntegration, decimal total,
            List<SendOrderRequestItem> itemsList, Address deliveryAddress, PaymentConditionInformation paymentConditionInformation)
        {
            this.ClientIntegration = clientIntegration;
            this.Total = total;
            this.ItemsList = itemsList;
            this.DeliveryAddress = deliveryAddress;
            this.PaymentConditionInformation = paymentConditionInformation;
        }
    }
}
