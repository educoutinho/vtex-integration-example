using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enginesoft.VtexIntegrationSample.Models
{
    public class Address
    {
        public string AddressType { get; set; }

        public string ReceiverName { get; set; }

        public string Street { get; set; }

        public string Number { get; set; }

        public string Complement { get; set; }

        public string Quarter { get; set; }

        public string Zipcode { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }
        
        public Address()
        {

        }

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
