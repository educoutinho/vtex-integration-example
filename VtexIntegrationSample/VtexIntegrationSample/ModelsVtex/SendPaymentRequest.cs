using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enginesoft.VtexIntegrationSample.ModelsVtex
{
    internal class SendPaymentRequest
    {
        public string callbackUrl { get; set; }
        public string paymentsArray { get; set; }        
    }
}
