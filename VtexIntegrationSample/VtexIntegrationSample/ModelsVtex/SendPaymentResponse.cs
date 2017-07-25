using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enginesoft.VtexIntegrationSample.ModelsVtex
{
    internal class SendPaymentResponse
    {
        public string code { get; set; }
        public string message { get; set; }
        public object exception { get; set; }
    }
}
