using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enginesoft.VtexIntegrationSample.ModelsVtex
{
    internal class ProductAndSkuIdsResponse
    {
        public Dictionary<string, List<int>> data { get; set; }
        public Range range { get; set; }
        
        internal class Range
        {
            public int total { get; set; }
            public int from { get; set; }
            public int to { get; set; }
        }
    }
}
