using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enginesoft.VtexIntegrationSample.Models
{
    public class GetItemRequest
    {
        public string SupplierItemCode { get; set; }

        public GetItemRequest(string supplierItemCode)
        {
            this.SupplierItemCode = supplierItemCode;
        }
    }
}
