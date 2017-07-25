using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enginesoft.VtexIntegrationSample.ModelsVtex
{
    internal class SendOrderResponse
    {
        public OrderForm orderForm { get; set; }
        public TransactionData transactionData { get; set; }
        public List<Order> orders { get; set; }
        
        internal class Message
        {
            public object code { get; set; }
            public string text { get; set; }
            public string status { get; set; }
            public object fields { get; set; }
        }

        internal class Payment
        {
            public string paymentSystem { get; set; }
            public object bin { get; set; }
            public object accountId { get; set; }
            public object tokenId { get; set; }
            public int value { get; set; }
            public int referenceValue { get; set; }
            public object giftCardRedemptionCode { get; set; }
            public object giftCardProvider { get; set; }
            public object giftCardId { get; set; }
        }

        internal class MerchantTransaction
        {
            public string id { get; set; }
            public string transactionId { get; set; }
            public string merchantName { get; set; }
            public List<Payment> payments { get; set; }
        }

        internal class TransactionData
        {
            public List<MerchantTransaction> merchantTransactions { get; set; }
            public string receiverUri { get; set; }
            public string gatewayCallbackTemplatePath { get; set; }
        }

        internal class AdditionalInfo
        {
            public string brandName { get; set; }
            public string brandId { get; set; }
            public object offeringInfo { get; set; }
            public object offeringType { get; set; }
            public object offeringTypeId { get; set; }
        }
        
        internal class ItemAttachment
        {
            public object name { get; set; }
            public object content { get; set; }
        }

        internal class Item
        {
            public string uniqueId { get; set; }
            public string id { get; set; }
            public string productId { get; set; }
            public string refId { get; set; }
            public string ean { get; set; }
            public string name { get; set; }
            public string skuName { get; set; }
            public object modalType { get; set; }
            public string priceValidUntil { get; set; }
            public int tax { get; set; }
            public int price { get; set; }
            public int listPrice { get; set; }
            public object manualPrice { get; set; }
            public int sellingPrice { get; set; }
            public int rewardValue { get; set; }
            public bool isGift { get; set; }
            public AdditionalInfo additionalInfo { get; set; }
            public object preSaleDate { get; set; }
            public string productCategoryIds { get; set; }
            public Dictionary<string,string> productCategories { get; set; }
            public object defaultPicker { get; set; }
            public int handlerSequence { get; set; }
            public bool handling { get; set; }
            public int quantity { get; set; }
            public string seller { get; set; }
            public string imageUrl { get; set; }
            public string detailUrl { get; set; }
            public List<object> components { get; set; }
            public List<object> bundleItems { get; set; }
            public List<object> attachments { get; set; }
            public ItemAttachment itemAttachment { get; set; }
            public List<object> attachmentOfferings { get; set; }
            public List<object> offerings { get; set; }
            public List<object> priceTags { get; set; }
            public string availability { get; set; }
            public string measurementUnit { get; set; }
            public double unitMultiplier { get; set; }
        }

        internal class Seller
        {
            public string id { get; set; }
            public string name { get; set; }
            public string logo { get; set; }
        }

        internal class Total
        {
            public string id { get; set; }
            public string name { get; set; }
            public int value { get; set; }
        }

        internal class Totalizer
        {
            public string id { get; set; }
            public string name { get; set; }
            public int value { get; set; }
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
            public List<object> geoCoordinates { get; set; }
        }

        internal class DeliveryId
        {
            public string courierId { get; set; }
            public string warehouseId { get; set; }
            public string dockId { get; set; }
            public string courierName { get; set; }
            public int quantity { get; set; }
        }

        internal class PickupStoreInfo
        {
            public bool isPickupStore { get; set; }
            public object friendlyName { get; set; }
            public object address { get; set; }
            public object additionalInfo { get; set; }
            public object dockId { get; set; }
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
            public int price { get; set; }
            public int listPrice { get; set; }
            public int tax { get; set; }
            public PickupStoreInfo pickupStoreInfo { get; set; }
        }

        internal class LogisticsInfo
        {
            public int itemIndex { get; set; }
            public string selectedSla { get; set; }
            public List<Sla> slas { get; set; }
            public List<string> shipsTo { get; set; }
            public string itemId { get; set; }
        }

        internal class ShippingData
        {
            public string attachmentId { get; set; }
            public Address address { get; set; }
            public List<LogisticsInfo> logisticsInfo { get; set; }
            public List<object> availableAddresses { get; set; }
        }

        internal class ClientProfileData
        {
            public string attachmentId { get; set; }
            public string email { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
            public object document { get; set; }
            public object documentType { get; set; }
            public string phone { get; set; }
            public string corporateName { get; set; }
            public string tradeName { get; set; }
            public string corporateDocument { get; set; }
            public string stateInscription { get; set; }
            public string corporatePhone { get; set; }
            public bool isCorporate { get; set; }
            public object profileCompleteOnLoading { get; set; }
            public object profileErrorOnLoading { get; set; }
            public object customerClass { get; set; }
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
            public object paymentName { get; set; }
            public object paymentGroupName { get; set; }
            public int value { get; set; }
            public List<Installment> installments { get; set; }
        }

        internal class Validator
        {
            public string regex { get; set; }
            public string mask { get; set; }
            public string cardCodeRegex { get; set; }
            public string cardCodeMask { get; set; }
            public List<int?> weights { get; set; }
            public bool useCvv { get; set; }
            public bool useExpirationDate { get; set; }
            public bool useCardHolderName { get; set; }
            public bool useBillingAddress { get; set; }
        }

        internal class PaymentSystem
        {
            public int id { get; set; }
            public string name { get; set; }
            public string groupName { get; set; }
            public Validator validator { get; set; }
            public string stringId { get; set; }
            public string template { get; set; }
            public bool requiresDocument { get; set; }
            public bool isCustom { get; set; }
            public string description { get; set; }
            public bool requiresAuthentication { get; set; }
            public string dueDate { get; set; }
        }

        internal class MerchantSellerPayment
        {
            public string id { get; set; }
            public int installments { get; set; }
            public int referenceValue { get; set; }
            public int value { get; set; }
            public int interestRate { get; set; }
            public int installmentValue { get; set; }
        }

        internal class Transaction
        {
            public bool isActive { get; set; }
            public string transactionId { get; set; }
            public string merchantName { get; set; }
            public List<object> payments { get; set; }
        }

        internal class PaymentData
        {
            public string attachmentId { get; set; }
            public string transactionId { get; set; }
            public List<object> payments { get; set; }
            public List<object> giftCards { get; set; }
            public List<Transaction> transactions { get; set; }
            public string merchantName { get; set; }
        }

        internal class TemplateOptions
        {
            public bool toggleCorporate { get; set; }
        }

        internal class CurrencyFormatInfo
        {
            public int currencyDecimalDigits { get; set; }
            public string currencyDecimalSeparator { get; set; }
            public string currencyGroupSeparator { get; set; }
            public int currencyGroupSize { get; set; }
            public bool startsWithCurrencySymbol { get; set; }
        }

        internal class StorePreferencesData
        {
            public string countryCode { get; set; }
            public bool checkToSavePersonDataByDefault { get; set; }
            public TemplateOptions templateOptions { get; set; }
            public string timeZone { get; set; }
            public string currencyCode { get; set; }
            public int currencyLocale { get; set; }
            public string currencySymbol { get; set; }
            public CurrencyFormatInfo currencyFormatInfo { get; set; }
        }

        internal class RatesAndBenefitsData
        {
            public string attachmentId { get; set; }
            public List<object> rateAndBenefitsIdentifiers { get; set; }
            public List<object> teaser { get; set; }
        }

        internal class OrderForm
        {
            public string orderFormId { get; set; }
            public string salesChannel { get; set; }
            public bool loggedIn { get; set; }
            public bool isCheckedIn { get; set; }
            public object storeId { get; set; }
            public bool allowManualPrice { get; set; }
            public bool canEditData { get; set; }
            public object userProfileId { get; set; }
            public object userType { get; set; }
            public bool ignoreProfileData { get; set; }
            public int value { get; set; }
            public List<Message> messages { get; set; }
            public List<Item> items { get; set; }
            public List<object> selectableGifts { get; set; }
            public List<object> products { get; set; }
            public List<Totalizer> totalizers { get; set; }
            public ShippingData shippingData { get; set; }
            public ClientProfileData clientProfileData { get; set; }
            public PaymentData paymentData { get; set; }
            public object marketingData { get; set; }
            public List<Seller> sellers { get; set; }
            public object clientPreferencesData { get; set; }
            public object commercialConditionData { get; set; }
            public StorePreferencesData storePreferencesData { get; set; }
            public object giftRegistryData { get; set; }
            public object openTextField { get; set; }
            public object customData { get; set; }
            public object hooksData { get; set; }
            public RatesAndBenefitsData ratesAndBenefitsData { get; set; }
            public object itemsOrdination { get; set; }
        }

        internal class Order
        {
            public string orderId { get; set; }
            public string orderGroup { get; set; }
            public string state { get; set; }
            public bool isCheckedIn { get; set; }
            public string sellerOrderId { get; set; }
            public object storeId { get; set; }
            public int value { get; set; }
            public List<Item> items { get; set; }
            public List<Seller> sellers { get; set; }
            public List<Total> totals { get; set; }
            public ClientProfileData clientProfileData { get; set; }
            public RatesAndBenefitsData ratesAndBenefitsData { get; set; }
            public ShippingData shippingData { get; set; }
            public PaymentData paymentData { get; set; }
            public object clientPreferencesData { get; set; }
            public object commercialConditionData { get; set; }
            public object giftRegistryData { get; set; }
            public object marketingData { get; set; }
            public StorePreferencesData storePreferencesData { get; set; }
            public object openTextField { get; set; }
            public object customData { get; set; }
            public object hooksData { get; set; }
            public object changeData { get; set; }
            public string salesChannel { get; set; }
            public string followUpEmail { get; set; }
            public string creationDate { get; set; }
            public string lastChange { get; set; }
            public string timeZoneCreationDate { get; set; }
            public string timeZoneLastChange { get; set; }
            public bool isCompleted { get; set; }
            public string hostName { get; set; }
            public object merchantName { get; set; }
            public string userType { get; set; }
            public int roundingError { get; set; }
            public bool allowEdition { get; set; }
            public bool allowCancellation { get; set; }
        }
    }
}
