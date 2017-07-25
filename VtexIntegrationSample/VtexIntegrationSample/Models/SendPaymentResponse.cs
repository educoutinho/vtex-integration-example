using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enginesoft.VtexIntegrationSample.Models
{
    public class SendPaymentResponse
    {
        public Models.SendPaymentStatusEnum Status { get; set; }

        public string Message { get; set; }

        public string ServiceCode { get; set; }
    }
}
