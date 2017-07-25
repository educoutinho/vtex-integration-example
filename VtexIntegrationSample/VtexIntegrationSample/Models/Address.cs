using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enginesoft.VtexIntegrationSample.Models
{
    public class Address
    {
        public string AddressType { get; private set; }

        public string ReceiverName { get; private set; }

        public string Street { get; private set; }

        public string Number { get; private set; }

        public string Complement { get; private set; }

        public string Quarter { get; private set; }

        public string Zipcode { get; private set; }

        public string City { get; private set; }

        public string State { get; private set; }

        public string Country { get; private set; }
        
        public Address(string addressType, string receiverName, string street, string number, string complement, string quarter, string zipcode, string city, string state, string country)
        {
            this.AddressType = addressType;
            this.ReceiverName = receiverName;
            this.Street = street;
            this.Number = number;
            this.Complement = complement;
            this.Quarter = quarter;
            this.Zipcode = zipcode;
            this.City = city;
            this.State = state;
            this.Country = country;
        }
    }
}
