using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enginesoft.VtexIntegrationSample.Models
{
    public class GetOrderStatusRequest
    {
        public Models.ClientIntegration ClientIntegration { get; set; }

        public string SupplierOrderNumber { get; set; }

        public GetOrderStatusRequest()
        {
            
        }
    }
}
