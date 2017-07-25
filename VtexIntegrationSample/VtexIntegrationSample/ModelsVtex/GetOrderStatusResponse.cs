using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enginesoft.VtexIntegrationSample.ModelsVtex
{
    internal class GetOrderStatusResponse
    {
        public string orderId { get; set; }
        public string sequence { get; set; }
        public string marketplaceOrderId { get; set; }
        public object marketplaceServicesEndpoint { get; set; }
        public string sellerOrderId { get; set; }
        public string origin { get; set; }
        public string affiliateId { get; set; }
        public string salesChannel { get; set; }
        public object merchantName { get; set; }
        public string status { get; set; }
        public string statusDescription { get; set; }
        public int value { get; set; }
        public string creationDate { get; set; }
        public string lastChange { get; set; }
        public string orderGroup { get; set; }
        public List<Total> totals { get; set; }
        public List<Item> items { get; set; }
        public List<object> marketplaceItems { get; set; }
        public ClientProfileData clientProfileData { get; set; }
        public object giftRegistryData { get; set; }
        public object marketingData { get; set; }
        public RatesAndBenefitsData ratesAndBenefitsData { get; set; }
        public ShippingData shippingData { get; set; }
        public PaymentData paymentData { get; set; }
        public PackageAttachment packageAttachment { get; set; }
        public List<Seller> sellers { get; set; }
        public object callCenterOperatorData { get; set; }
        public string followUpEmail { get; set; }
        public object lastMessage { get; set; }
        public string hostname { get; set; }
        public object changesAttachment { get; set; }
        public object openTextField { get; set; }
        public int roundingError { get; set; }
        public string orderFormId { get; set; }
        public object commercialConditionData { get; set; }
        public bool isCompleted { get; set; }
        public object customData { get; set; }

        internal class Total
        {
            public string id { get; set; }
            public string name { get; set; }
            public int value { get; set; }
        }

        internal class Content
        {
        }

        internal class ItemAttachment
        {
            public Content content { get; set; }
            public object name { get; set; }
        }

        internal class Dimension
        {
            public double cubicweight { get; set; }
            public decimal height { get; set; }
            public decimal length { get; set; }
            public decimal weight { get; set; }
            public decimal width { get; set; }
        }

        internal class AdditionalInfo
        {
            public string brandName { get; set; }
            public string brandId { get; set; }
            public string categoriesIds { get; set; }
            public string productClusterId { get; set; }
            public string commercialConditionId { get; set; }
            public Dimension dimension { get; set; }
            public object offeringInfo { get; set; }
            public object offeringType { get; set; }
            public object offeringTypeId { get; set; }
        }

        internal class Item
        {
            public string uniqueId { get; set; }
            public string id { get; set; }
            public string productId { get; set; }
            public string ean { get; set; }
            public object lockId { get; set; }
            public ItemAttachment itemAttachment { get; set; }
            public List<object> attachments { get; set; }
            public int quantity { get; set; }
            public string seller { get; set; }
            public string name { get; set; }
            public string refId { get; set; }
            public int price { get; set; }
            public int listPrice { get; set; }
            public object manualPrice { get; set; }
            public List<object> priceTags { get; set; }
            public string imageUrl { get; set; }
            public string detailUrl { get; set; }
            public List<object> components { get; set; }
            public List<object> bundleItems { get; set; }
            public List<object> @params { get; set; }
            public List<object> offerings { get; set; }
            public string sellerSku { get; set; }
            public string priceValidUntil { get; set; }
            public int commission { get; set; }
            public int tax { get; set; }
            public object preSaleDate { get; set; }
            public AdditionalInfo additionalInfo { get; set; }
            public string measurementUnit { get; set; }
            public decimal unitMultiplier { get; set; }
            public int sellingPrice { get; set; }
            public bool isGift { get; set; }
            public object shippingPrice { get; set; }
            public int rewardValue { get; set; }
        }

        internal class ClientProfileData
        {
            public string id { get; set; }
            public string email { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string documentType { get; set; }
            public string document { get; set; }
            public string phone { get; set; }
            public string corporateName { get; set; }
            public string tradeName { get; set; }
            public string corporateDocument { get; set; }
            public string stateInscription { get; set; }
            public string corporatePhone { get; set; }
            public bool isCorporate { get; set; }
            public string userProfileId { get; set; }
            public object customerClass { get; set; }
        }

        internal class RatesAndBenefitsData
        {
            public string id { get; set; }
            public List<object> rateAndBenefitsIdentifiers { get; set; }
        }

        internal class Address
        {
            public string addressType { get; set; }
            public string receiverName { get; set; }
            public string addressId { get; set; }
            public string postalCode { get; set; }
            public string city { get; set; }
            public string state { get; set; }
            public string country { get; set; }
            public string street { get; set; }
            public string number { get; set; }
            public string neighborhood { get; set; }
            public string complement { get; set; }
            public object reference { get; set; }
        }

        internal class Sla
        {
            public string id { get; set; }
            public string name { get; set; }
            public string shippingEstimate { get; set; }
            public object deliveryWindow { get; set; }
            public int price { get; set; }
            public string deliveryChannel { get; set; }
        }

        internal class DeliveryId
        {
            public string courierId { get; set; }
            public string courierName { get; set; }
            public string dockId { get; set; }
            public int quantity { get; set; }
            public string warehouseId { get; set; }
        }

        internal class LogisticsInfo
        {
            public int itemIndex { get; set; }
            public string selectedSla { get; set; }
            public object lockTTL { get; set; }
            public int price { get; set; }
            public int listPrice { get; set; }
            public int sellingPrice { get; set; }
            public object deliveryWindow { get; set; }
            public string deliveryCompany { get; set; }
            public string shippingEstimate { get; set; }
            public string shippingEstimateDate { get; set; }
            public List<Sla> slas { get; set; }
            public List<string> shipsTo { get; set; }
            public List<DeliveryId> deliveryIds { get; set; }
            public string deliveryChannel { get; set; }
        }

        internal class ShippingData
        {
            public string id { get; set; }
            public Address address { get; set; }
            public List<LogisticsInfo> logisticsInfo { get; set; }
            public object trackingHints { get; set; }
        }

        internal class ConnectorResponses
        {
            public string Tid { get; set; }
            public string ReturnCode { get; set; }
            public string Message { get; set; }
            public string authId { get; set; }
            public string Nsu { get; set; }
            public string Arp { get; set; }
            public string eci { get; set; }
            public string lr { get; set; }
        }

        internal class Payment
        {
            public string id { get; set; }
            public string paymentSystem { get; set; }
            public string paymentSystemName { get; set; }
            public int value { get; set; }
            public int installments { get; set; }
            public int referenceValue { get; set; }
            public object cardHolder { get; set; }
            public object cardNumber { get; set; }
            public string firstDigits { get; set; }
            public string lastDigits { get; set; }
            public object cvv2 { get; set; }
            public object expireMonth { get; set; }
            public object expireYear { get; set; }
            public object url { get; set; }
            public object giftCardId { get; set; }
            public object giftCardName { get; set; }
            public object giftCardCaption { get; set; }
            public object redemptionCode { get; set; }
            public string group { get; set; }
            public string tid { get; set; }
            public object dueDate { get; set; }
            public ConnectorResponses connectorResponses { get; set; }
        }

        internal class Transaction
        {
            public bool isActive { get; set; }
            public string transactionId { get; set; }
            public List<Payment> payments { get; set; }
        }

        internal class PaymentData
        {
            public List<Transaction> transactions { get; set; }
        }

        internal class PackageAttachment
        {
            public List<object> packages { get; set; }
        }

        internal class Seller
        {
            public string id { get; set; }
            public string name { get; set; }
            public string logo { get; set; }
        }
    }
}
