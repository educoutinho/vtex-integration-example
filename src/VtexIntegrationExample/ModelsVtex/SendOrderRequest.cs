using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enginesoft.VtexIntegrationSample.ModelsVtex
{
    internal class SendOrderRequest
    {
        public List<Item> items { get; set; }
        public ClientProfileData clientProfileData { get; set; }
        public ShippingData shippingData { get; set; }
        public PaymentData paymentData { get; set; }
        public List<object> giftcards { get; set; }
        public MarketingData marketingData { get; set; }

        internal class Item
        {
            public string id { get; set; }
            public int quantity { get; set; }
            public string seller { get; set; }
            public int price { get; set; }
            public List<object> bundleItems { get; set; }
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
            public string reference { get; set; }
            public List<object> geoCoordinates { get; set; }
        }

        internal class LogisticsInfo
        {
            public int itemIndex { get; set; }
            public string selectedSla { get; set; }
            public string lockTTL { get; set; }
            public string shippingEstimate { get; set; }
            public int price { get; set; }
            public List<object> deliveryWindow { get; set; }
        }

        internal class ShippingData
        {
            public string id { get; set; }
            public Address address { get; set; }
            public List<LogisticsInfo> logisticsInfo { get; set; }
        }

        internal class Payment
        {
            public int value { get; set; }
            public int referenceValue { get; set; }
            public int installments { get; set; }
            public string paymentSystem { get; set; }
        }

        internal class PaymentData
        {
            public List<Payment> payments { get; set; }
        }

        internal class MarketingData
        {
            public string utmSource { get; set; }
        }
    }
}
