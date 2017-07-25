using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enginesoft.VtexIntegrationSample.Extensions;

namespace Enginesoft.VtexIntegrationSample.Models
{
    public class Item
    {
        public string Barcode { get; set; }

        public string SupplierItemCode { get; set; }

        public string Name { get; set; }

        public string Brand { get; set; }

        public string UrlImage { get; set; }

        public string ItemGroup { get; set; }

        public string ItemCategory { get; set; }

        public string ItemSubCategory { get; set; }

        public string Description { get; set; }

        public string AdditionalInfo { get; set; }

        //QdeMnmVndMer- quantidade mínima de embalagens que o produto é vendido
        public int MinimumSaleQuantity { get; set; }

        //VLRCNVUNDVNDMER- quantas unidades a embalagem tem
        public int ItemsInPackage { get; set; }

        //QDEUNDVNDCXAFRN- quantas embalagens a caixa fechada tem
        public int PackagesInBox { get; set; }

        //produto só pode ser vendido em quantidades múltiplas desse valor
        public int MultipleQuantity { get; set; }
        
        public bool Active { get; set; }

        public string SellerCode { get; set; }

        public override string ToString()
        {
            return string.Format("IntegrationItem -- Barcode: {0}, SupplierItemCode: {1}, Name: {2}, Brand: {3}, UrlImage: {4}, ItemGroup: {5}, ItemCategory: {6}, ItemSubCategory: {7}, AdditionalInfo: {8}, MinimumSaleQuantity: {9}, MultipleQuantity: {10}",
                this.Barcode, this.SupplierItemCode, this.Name, this.Brand, this.UrlImage, this.ItemGroup, this.ItemCategory, this.ItemSubCategory, this.AdditionalInfo, this.MinimumSaleQuantity, this.MultipleQuantity);
        }

        public Item(string supplierItemCode)
        {
            this.SupplierItemCode = supplierItemCode;
        }

        public Item(string supplierItemCode, string barcode, string name, string brand, string urlImage, string itemGroup, string itemCategory,
            string itemSubCategory, string additionalInfo, int minimumSaleQuantity, int itemsInPackage, int packagesInBox,
            int multipleQuantity, string description, bool active, string sellerCode)
        {
            this.SupplierItemCode = supplierItemCode;
            this.Barcode = barcode;
            this.Name = name;
            this.Brand = brand;
            this.UrlImage = urlImage;
            this.ItemGroup = itemGroup;
            this.ItemCategory = itemCategory;
            this.ItemSubCategory = itemSubCategory;
            this.AdditionalInfo = additionalInfo;
            this.MinimumSaleQuantity = minimumSaleQuantity;
            this.ItemsInPackage = itemsInPackage;
            this.PackagesInBox = packagesInBox;
            this.MultipleQuantity = multipleQuantity;
            this.Description = description;
            this.Active = active;
            this.SellerCode = sellerCode;
        }

        public void Sanitize()
        {
            if (!string.IsNullOrEmpty(this.Barcode))
                this.Barcode = this.Barcode.ToLower().RemoveSpecialCharacters(true, true, true, true);

            if (!string.IsNullOrEmpty(this.SupplierItemCode))
                this.SupplierItemCode = this.SupplierItemCode.ToLower().RemoveSpecialCharacters(true, true, true, true);

            if (!string.IsNullOrEmpty(this.Name))
                this.Name = this.Name.Trim().RemovePageBreak();

            if (!string.IsNullOrEmpty(this.Brand))
                this.Brand = this.Brand.Trim().RemovePageBreak();

            if (!string.IsNullOrEmpty(this.UrlImage))
                this.UrlImage = this.UrlImage.Trim().RemovePageBreak().RemoveSpaces();

            if (!string.IsNullOrEmpty(this.ItemGroup))
                this.ItemGroup = this.ItemGroup.Trim().RemovePageBreak();

            if (!string.IsNullOrEmpty(this.ItemCategory))
                this.ItemCategory = this.ItemCategory.Trim().RemovePageBreak();

            if (!string.IsNullOrEmpty(this.ItemSubCategory))
                this.ItemSubCategory = this.ItemSubCategory.Trim().RemovePageBreak();
            
            if (!string.IsNullOrEmpty(this.Description))
                this.Description = this.Description.Trim().RemoveHtmlTags();

            if (!string.IsNullOrEmpty(this.AdditionalInfo))
                this.AdditionalInfo = this.AdditionalInfo.Trim().RemovePageBreak().RemoveHtmlTags();

            if (!string.IsNullOrEmpty(this.SellerCode))
                this.SellerCode = this.SellerCode.Trim().RemovePageBreak();
        }
    }    
}

