using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enginesoft.VtexIntegrationSample.Models
{
    public class ItemGetItemsPriceRequest
    {
        public string SupplierItemCode { get; private set; }

        public string ItemBarcode { get; private set; }

        public int Quantity { get; private set; }
        
        public string SellerCode { get; private set; }

        public ItemGetItemsPriceRequest(string supplierItemCode, string itemBarcode, int quantity, string sellerCode)
        {
            this.SupplierItemCode = supplierItemCode;
            this.ItemBarcode = itemBarcode;
            this.Quantity = quantity;
            this.SellerCode = sellerCode;
        }
    }
}
