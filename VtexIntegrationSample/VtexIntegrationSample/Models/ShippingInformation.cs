using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enginesoft.VtexIntegrationSample.Models
{
    public class ShippingInformation
    {
        public string ShippingTypeID { get; private set; }

        public string Name { get; private set; }

        public ShippingTime ShippingTime { get; private set; }

        public decimal Price { get; private set; }

        public ShippingInformation(string shippingTypeID, string name, decimal price, ShippingTime shippingTime)
        {
            this.ShippingTypeID = shippingTypeID;
            this.Name = name;
            this.Price = price;
            this.ShippingTime = shippingTime;
        }

        public void IncrementShippingTime(decimal price, ShippingTime shippingTime)
        {
            this.Price += price;
            this.ShippingTime.Increment(shippingTime);
        }

        public override string ToString()
        {
            return string.Format("ShippingInformation -- ID={0}, Price={1}", this.ShippingTypeID, this.Price);
        }
    }
}
