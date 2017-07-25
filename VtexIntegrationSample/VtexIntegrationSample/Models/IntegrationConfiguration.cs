using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enginesoft.VtexIntegrationSample.Models
{
    public class IntegrationConfiguration
    {
        public string CommerceApiUrl { get; set; }

        public string PaymentApiUrl { get; set; }


        public string ServiceKey { get; set; }

        public string ServiceToken { get; set; }


        public string OrderServiceKey { get; set; }

        public string OrderServiceToken { get; set; }


        /// <summary>
        /// Código do afiliado cadastrado na VTEX
        /// </summary>
        public string PartnerCode { get; set; }

        /// <summary>
        /// Código da política comercial cadastrada na VTEX, ex: 2. A política comercial é um filtro de quais produtos serão exibidos para nós
        /// </summary>
        public string TradePolicyCode { get; set; }

        public string MerchantName { get; set; }
    }
}
