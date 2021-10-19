using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enginesoft.VtexIntegrationSample.Models
{
    public enum GetItemsPriceStatusEnum
    {
        Success,

        Error,

        InvalidToken,

        /// <summary>
        /// Exception ao chamar o Webservice
        /// </summary>
        CriticalError
    }
}
