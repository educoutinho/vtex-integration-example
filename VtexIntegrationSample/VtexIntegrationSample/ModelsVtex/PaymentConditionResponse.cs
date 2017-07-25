using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enginesoft.VtexIntegrationSample.ModelsVtex
{
    internal class PaymentConditionResponse
    {
        public int id { get; set; }
        public string name { get; set; }
        public int connectorId { get; set; }
        public bool requiresDocument { get; set; }
        public string implementation { get; set; }
        public string connectorImplementation { get; set; }
        public object antifraudConnectorImplementation { get; set; }
        public string groupName { get; set; }
        public bool redirect { get; set; }
        public bool isCustom { get; set; }
        public bool isSelfAuthorized { get; set; }
        public bool requiresAuthentication { get; set; }
        public bool allowInstallments { get; set; }
        public bool allowMultiple { get; set; }
        public bool allowIssuer { get; set; }
        public bool allowCountry { get; set; }
        public bool allowCommercialPolicy { get; set; }
        public bool allowCommercialCondition { get; set; }
        public bool allowPeriod { get; set; }
        public bool isAvailable { get; set; }
        public object description { get; set; }
        public Validator validator { get; set; }
        public string dueDate { get; set; }
        public bool allowNotification { get; set; }
        public object affiliationId { get; set; }
        public object availablePayments { get; set; }

        internal class Validator
        {
            public string regex { get; set; }
            public string mask { get; set; }
            public string cardCodeMask { get; set; }
            public string cardCodeRegex { get; set; }
            public List<int> weights { get; set; }
            public bool useCvv { get; set; }
            public bool useExpirationDate { get; set; }
            public bool useCardHolderName { get; set; }
            public bool useBillingAddress { get; set; }
            public object validCardLengths { get; set; }
        }
    }
}
