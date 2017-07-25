using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enginesoft.VtexIntegrationSample.ModelsVtex
{
    internal class SkuPriceRequest
    {
        public string postalCode { get; set; }
        public string country { get; set; }
        public List<Item> items { get; set; }

        internal class Item
        {
            public string id { get; set; }
            public int quantity { get; set; }
            public string seller { get; set; }
        }
    }
}
