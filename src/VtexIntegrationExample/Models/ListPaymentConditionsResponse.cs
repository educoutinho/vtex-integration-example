using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enginesoft.VtexIntegrationSample.Models
{
    public class ListPaymentConditionsResponse
    {
        List<Models.PaymentCondition> PaymentConditions { get; set; }

        public ListPaymentConditionsResponse(List<Models.PaymentCondition> paymentConditions)
        {
            this.PaymentConditions = paymentConditions;
        }
    }
}
