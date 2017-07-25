using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enginesoft.VtexIntegrationSample.ModelsVtex
{
    public class SendPaymentArrayItemRequest
    {
        internal class Fields
        {
            public string document { get; set; }
            public string accountId { get; set; }
            public string addressId { get; set; }
            public string cardNumber { get; set; }
            public string holderName { get; set; }
            public string dueDate { get; set; }
            public string validationCode { get; set; }
        }

        internal class Transaction
        {
            public string id { get; set; }
            public string merchantName { get; set; }
            public object payments { get; set; }
        }

        internal class Payment
        {
            public int paymentSystem { get; set; }
            public string paymentSystemName { get; set; }
            public string groupName { get; set; }
            public string currencyCode { get; set; }
            public int installments { get; set; }
            public int value { get; set; }
            public int installmentsInterestRate { get; set; }
            public int installmentsValue { get; set; }
            public int referenceValue { get; set; }
            public Fields fields { get; set; }
            public Transaction transaction { get; set; }
        }
    }
}
