using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enginesoft.VtexIntegrationSample.Models
{
    public class CompleteOrderRequest
    {
        public Models.ClientIntegration ClientIntegration { get; private set; }

        public string SupplierOrderNumber { get; private set; }
        
        public CompleteOrderRequest(Models.ClientIntegration clientIntegration, string supplierOrderNumber)
        {
            this.ClientIntegration = clientIntegration;
            this.SupplierOrderNumber = supplierOrderNumber;
        }
    }
}
