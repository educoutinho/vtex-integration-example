using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enginesoft.VtexIntegrationSample.ModelsVtex
{
    internal class SkuInformationResponse
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string NameComplete { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string SkuName { get; set; }
        public bool IsActive { get; set; }
        public bool IsTransported { get; set; }
        public bool IsInventoried { get; set; }
        public bool IsGiftCardRecharge { get; set; }
        public string ImageUrl { get; set; }
        public string DetailUrl { get; set; }
        public object CSCIdentification { get; set; }
        public string BrandId { get; set; }
        public string BrandName { get; set; }
        public SkuDimension Dimension { get; set; }
        public SkuDimension RealDimension { get; set; }
        public string ManufacturerCode { get; set; }
        public bool IsKit { get; set; }
        public IList<object> KitItems { get; set; }
        public IList<object> Services { get; set; }
        public IList<object> Categories { get; set; }
        public IList<object> Attachments { get; set; }
        public IList<object> Collections { get; set; }
        public IList<SkuSeller> SkuSellers { get; set; }
        public IList<object> SkuPriceSheet { get; set; }
        public List<SkuImage> Images { get; set; }
        public IList<object> SkuSpecifications { get; set; }
        public IList<object> ProductSpecifications { get; set; }
        public string ProductClustersIds { get; set; }
        public string ProductCategoryIds { get; set; }
        
        public Dictionary<string, string> ProductCategories { get; set; }
        
        public int CommercialConditionId { get; set; }
        public decimal RewardValue { get; set; }
        public SkuAlternateIds AlternateIds { get; set; }
        public IList<string> AlternateIdValues { get; set; }
        public object EstimatedDateArrival { get; set; }
        public string MeasurementUnit { get; set; }
        public decimal UnitMultiplier { get; set; }
        public string InformationSource { get; set; }
        public object ModalType { get; set; }

        internal class SkuAlternateIds
        {
            public string Ean { get; set; }
            public string RefId { get; set; }
        }

        internal class SkuCommercialOffer
        {
            public IList<SkuDeliverySlaSample> DeliverySlaSamples { get; set; }
            public int Price { get; set; }
            public int ListPrice { get; set; }
            public int AvailableQuantity { get; set; }
        }

        internal class SkuDeliverySlaPerType
        {
            public string TypeName { get; set; }
            public decimal Price { get; set; }
            public string Time { get; set; }
        }

        internal class SkuDeliverySlaSample
        {
            public SkuRegion Region { get; set; }
            public IList<SkuDeliverySlaPerType> DeliverySlaPerTypes { get; set; }
        }

        internal class SkuDimension
        {
            public decimal cubicweight { get; set; }
            public decimal height { get; set; }
            public decimal length { get; set; }
            public decimal weight { get; set; }
            public decimal width { get; set; }
        }

        internal class SkuRegion
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string CountryCode { get; set; }
            public string Zipcode { get; set; }
        }

        internal class SkuSeller
        {
            public SkuCommercialOffer SkuCommercialOffer { get; set; }
            public string SellerId { get; set; }
            public int StockKeepingUnitId { get; set; }
            public string SellerStockKeepingUnitId { get; set; }
            public bool IsActive { get; set; }
            public decimal FreightCommissionPercentage { get; set; }
            public decimal ProductCommissionPercentage { get; set; }
        }

        internal class SkuImage
        {
            public string ImageUrl { get; set; }

            public string ImageName { get; set; }

            public int FileId { get; set; }
        }
    }
}
