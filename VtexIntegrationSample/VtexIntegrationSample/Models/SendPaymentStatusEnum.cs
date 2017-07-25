using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enginesoft.VtexIntegrationSample.Models
{
    public enum SendPaymentStatusEnum
    {
        Success,

        Error,

        InvalidToken,

        /// <summary>
        /// Exception ao chamar o webservice, não foi possível fazer a chamada
        /// </summary>
        CriticalError
    }
}
