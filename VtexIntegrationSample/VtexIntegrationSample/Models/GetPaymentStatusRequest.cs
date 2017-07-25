using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enginesoft.VtexIntegrationSample.Models
{
    public class GetPaymentStatusRequest
    {
        public Models.ClientIntegration ClientIntegration { get; private set; }
        
        public string SupplierTransactionCode { get; private set; }

        public GetPaymentStatusRequest(Models.ClientIntegration clientIntegration, string supplierTransactionCode)
        {
            this.ClientIntegration = clientIntegration;
            this.SupplierTransactionCode = supplierTransactionCode;
        }
    }
}
