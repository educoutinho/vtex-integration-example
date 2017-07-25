using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enginesoft.VtexIntegrationSample.Models
{
    public class ShippingTime
    {
        public int Days { get; private set; }

        public bool IsBusinessDays { get; private set; }

        public ShippingTime(int days, bool isBusinessDays)
        {
            this.Days = days;
            this.IsBusinessDays = isBusinessDays;
        }

        public void Increment(ShippingTime shippingTime)
        {
            if (this.IsBusinessDays == shippingTime.IsBusinessDays)
            {
                this.Days = Math.Max(this.Days, shippingTime.Days);
            }
            else if (this.IsBusinessDays && !shippingTime.IsBusinessDays)
            {
                this.Days = Math.Max(this.Days, shippingTime.Days);
            }
            else if (!this.IsBusinessDays && shippingTime.IsBusinessDays)
            {
                this.IsBusinessDays = true;
                this.Days = Math.Max(this.Days, shippingTime.Days);
            }
        }

        public override string ToString()
        {
            return string.Format("ShippingTime -- {0} {1}", this.Days, (this.IsBusinessDays ? "business day(s)" : "day(s)"));
        }

        public string ToStringVtex()
        {
            return string.Concat(this.Days.ToString(), (this.IsBusinessDays ? "bd" : "d"));
        }
    }
}
