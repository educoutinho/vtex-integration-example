using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enginesoft.VtexIntegrationSample.Models
{
    public class ClientIntegration
    {
        public int ClientID { get; set; }


        public string CompanyName { get; set; }


        public string ContactFirstName { get; set; }

        public string ContactLastName { get; set; }

        public string ContactCpf { get; set; }


        public string Cnpj { get; set; }

        public string Ie { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
        
        public Address BillingAddress { get; set; }

        public ClientIntegration()
        {

        }

        public ClientIntegration(int clientID, string companyName, string contactFirstName, string contactLastName, string contactCpf, string cnpj, string ie, string email, string phone, Address billingAddress)
        {
            this.ClientID = clientID;
            this.CompanyName = companyName;

            this.ContactFirstName = contactFirstName;
            this.ContactLastName = contactLastName;
            this.ContactCpf = contactCpf;

            this.Cnpj = cnpj;
            this.Ie = ie;
            this.Email = email;
            this.Phone = phone;
            this.BillingAddress = billingAddress;
        }
    }
}
