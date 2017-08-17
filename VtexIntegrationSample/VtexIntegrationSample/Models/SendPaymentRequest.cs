using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enginesoft.VtexIntegrationSample.Models
{
    public class SendPaymentRequest
    {
        public Models.ClientIntegration ClientIntegration { get; private set; }

        public string SupplierOrderNumber { get; private set; }
        
        public string PaymentTransactionCode { get; private set; }

        public PaymentConditionInformation PaymentConditionInformation { get; private set; }

        public SendPaymentRequest(Models.ClientIntegration clientIntegration, string supplierOrderNumber, string paymentTransactionCode, PaymentConditionInformation paymentConditionInformation)
        {
            this.ClientIntegration = clientIntegration;
            this.SupplierOrderNumber = supplierOrderNumber;
            this.PaymentTransactionCode = paymentTransactionCode;
            this.PaymentConditionInformation = paymentConditionInformation;
        }
    }
}
