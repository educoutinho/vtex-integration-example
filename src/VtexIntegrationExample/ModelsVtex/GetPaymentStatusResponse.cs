using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enginesoft.VtexIntegrationSample.ModelsVtex
{
    internal class GetPaymentStatusResponse
    {
        public string id { get; set; }
        public string transactionId { get; set; }
        public string referenceKey { get; set; }
        public Interactions interactions { get; set; }
        public Settlements settlements { get; set; }
        public Payments payments { get; set; }
        public Refunds refunds { get; set; }
        public Cancellations cancellations { get; set; }
        public int timeoutStatus { get; set; }
        public decimal totalRefunds { get; set; }
        public string status { get; set; }
        public decimal value { get; set; }
        public object receiverUri { get; set; }
        public string startDate { get; set; }
        public object authorizationToken { get; set; }
        public object authorizationDate { get; set; }
        public object commitmentToken { get; set; }
        public object commitmentDate { get; set; }
        public object refundingToken { get; set; }
        public object refundingDate { get; set; }
        public string cancelationToken { get; set; }
        public string cancelationDate { get; set; }
        public List<Field> fields { get; set; }
        public string ipAddress { get; set; }
        public object sessionId { get; set; }
        public object macId { get; set; }
        public object vtexFingerprint { get; set; }
        public object chargeback { get; set; }
        public string owner { get; set; }
        public string orderId { get; set; }
        public string userAgent { get; set; }
        public string acceptHeader { get; set; }
        public object antifraudTid { get; set; }
        public object antifraudResponse { get; set; }
        public object antifraudReference { get; set; }
        public string antifraudAffiliationId { get; set; }
        public string channel { get; set; }
        public string salesChannel { get; set; }
        public object urn { get; set; }
        public object softDescriptor { get; set; }
        public bool markedForRecurrence { get; set; }
        public object buyer { get; set; }

        internal class Interactions
        {
            public string href { get; set; }
        }

        internal class Settlements
        {
            public string href { get; set; }
        }

        internal class Payments
        {
            public string href { get; set; }
        }

        internal class Refunds
        {
            public string href { get; set; }
        }

        internal class Cancellations
        {
            public string href { get; set; }
        }

        internal class Field
        {
            public string name { get; set; }
            public string value { get; set; }
        }
    }
}
