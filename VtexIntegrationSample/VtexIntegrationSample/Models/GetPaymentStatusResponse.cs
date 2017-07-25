using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enginesoft.VtexIntegrationSample.Models
{
    public class GetPaymentStatusResponse
    {
        public Models.GetPaymentStatusEnum Status { get; set; }

        public string Message { get; set; }

        public string ServiceCode { get; set; }

        public PaymentStatusEnum PaymentStatus { get; set; }
    }
}
