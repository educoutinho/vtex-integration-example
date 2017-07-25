using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enginesoft.VtexIntegrationSample.ModelsVtex
{
    internal class SkuPriceResponse
    {
        public List<Item> items { get; set; }
        public RatesAndBenefitsData ratesAndBenefitsData { get; set; }
        public PaymentData paymentData { get; set; }
        public List<object> selectableGifts { get; set; }
        public object marketingData { get; set; }
        public object postalCode { get; set; }
        public object country { get; set; }
        public List<LogisticsInfo> logisticsInfo { get; set; }
        public List<object> messages { get; set; }

        internal class Item
        {
            public string id { get; set; }
            public int requestIndex { get; set; }
            public int quantity { get; set; }
            public string seller { get; set; }
            public int tax { get; set; }
            public string priceValidUntil { get; set; }
            public decimal? price { get; set; }
            public int listPrice { get; set; }
            public int rewardValue { get; set; }
            public int sellingPrice { get; set; }
            public List<object> offerings { get; set; }
            public List<object> priceTags { get; set; }
            public string measurementUnit { get; set; }
            public decimal unitMultiplier { get; set; }
        }

        internal class RatesAndBenefitsData
        {
            public string attachmentId { get; set; }
            public List<object> rateAndBenefitsIdentifiers { get; set; }
            public List<object> teaser { get; set; }
        }

        internal class SellerMerchantInstallment
        {
            public string id { get; set; }
            public int count { get; set; }
            public bool hasInterestRate { get; set; }
            public int interestRate { get; set; }
            public int value { get; set; }
            public int total { get; set; }
        }

        internal class Installment
        {
            public int count { get; set; }
            public bool hasInterestRate { get; set; }
            public int interestRate { get; set; }
            public int value { get; set; }
            public int total { get; set; }
            public List<SellerMerchantInstallment> sellerMerchantInstallments { get; set; }
        }

        internal class InstallmentOption
        {
            public string paymentSystem { get; set; }
            public object bin { get; set; }
            public string paymentName { get; set; }
            public string paymentGroupName { get; set; }
            public int value { get; set; }
            public List<Installment> installments { get; set; }
        }

        internal class PaymentSystem
        {
            public int id { get; set; }
            public string name { get; set; }
            public string groupName { get; set; }
            public object validator { get; set; }
            public string stringId { get; set; }
            public string template { get; set; }
            public bool requiresDocument { get; set; }
            public bool isCustom { get; set; }
            public string description { get; set; }
            public bool requiresAuthentication { get; set; }
            public string dueDate { get; set; }
        }

        internal class PaymentData
        {
            public List<InstallmentOption> installmentOptions { get; set; }
            public List<PaymentSystem> paymentSystems { get; set; }
            public List<object> payments { get; set; }
            public List<object> giftCards { get; set; }
            public List<object> giftCardMessages { get; set; }
            public List<object> availableAccounts { get; set; }
            public List<object> availableTokens { get; set; }
        }

        internal class DeliveryId
        {
            public string courierId { get; set; }
            public string warehouseId { get; set; }
            public string dockId { get; set; }
            public string courierName { get; set; }
            public int quantity { get; set; }
        }

        internal class Sla
        {
            public string id { get; set; }
            public string name { get; set; }
            public List<DeliveryId> deliveryIds { get; set; }
            public string shippingEstimate { get; set; }
            public object shippingEstimateDate { get; set; }
            public object lockTTL { get; set; }
            public List<object> availableDeliveryWindows { get; set; }
            public object deliveryWindow { get; set; }
            public decimal? price { get; set; }
            public int listPrice { get; set; }
            public int tax { get; set; }
            public object pickupStoreInfo { get; set; }
        }

        internal class LogisticsInfo
        {
            public int itemIndex { get; set; }
            public int stockBalance { get; set; }
            public int quantity { get; set; }
            public List<string> shipsTo { get; set; }
            public List<Sla> slas { get; set; }
        }
    }
}
