using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enginesoft.VtexIntegrationSample.Models
{
    public class ItemPrice
    {
        public int RequestItemIndex { get; private set; }

        public int ItemIndex { get; private set; }
        
        
        public string SupplierItemCode { get; private set; }

        public string SellerCode { get; private set; }

        public string Barcode { get; private set; }

        public decimal Value { get; private set; }

        public decimal PackageValue { get; private set; }

        public int ProfileID { get; private set; }

        public int Quantity { get; private set; }
        
        public int StockBalance { get; private set; }

        public bool ItemInStock { get; private set; }

        
        public bool HasError { get; private set; }

        public string ErrorMessage { get; private set; }


        public List<ShippingInformation> ShippingInformations { get; set; }
        

        public void SetStockInfo(bool itemInStock, int stockBalance)
        {
            this.ItemInStock = itemInStock;
            this.StockBalance = stockBalance;
        }

        public void SetError(string errrorMessage)
        {
            this.HasError = true;
            this.ErrorMessage = errrorMessage;
        }

        public ItemPrice(int requestItemIndex, int itemIndex, string supplierItemCode, string sellerCode, string barcode, decimal value, decimal packageValue, int profileID, int quantity)
        {
            this.RequestItemIndex = requestItemIndex;
            this.ItemIndex = itemIndex;
            this.SupplierItemCode = supplierItemCode;
            this.SellerCode = sellerCode;
            this.Barcode = barcode;
            this.Value = value;
            this.PackageValue = packageValue;
            this.ProfileID = profileID;
            this.Quantity = quantity;

            this.ShippingInformations = new List<ShippingInformation>();
        }

        public override string ToString()
        {
            return string.Format("IntegrationItemPrice -- Barcode: {0}, SupplierItemCode: {1}, ProfileID: {2}, Price: {3}",
                this.Barcode, this.SupplierItemCode, this.ProfileID, this.Value.ToString("#,##0.00"));
        }
    }
}
