using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enginesoft.VtexIntegrationSample.Models
{
    public class SendOrderRequestItem
    {
        public string ItemBarcode { get; private set; }

        public string SupplierItemCode { get; private set; }

        public string SellerCode { get; private set; }

        public int Quantity { get; private set; }

        public decimal Value { get; private set; }

        public decimal Total { get; private set; }

        public Models.ShippingInformation ShippingInformation { get; private set; }

        public SendOrderRequestItem(string itemBarcode, string supplierItemCode, string sellerCode, int quantity, decimal value, decimal total, Models.ShippingInformation shippingInformation)
        {
            this.ItemBarcode = itemBarcode;
            this.SupplierItemCode = supplierItemCode;
            this.SellerCode = sellerCode;
            this.Quantity = quantity;
            this.Value = value;
            this.Total = total;
            this.ShippingInformation = shippingInformation;
        }
    }
}
