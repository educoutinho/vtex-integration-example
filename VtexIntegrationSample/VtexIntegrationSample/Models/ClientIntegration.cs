using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enginesoft.VtexIntegrationSample.Models
{
    public class ClientIntegration
    {
        public int ClientID { get; private set; }
        

        public string CompanyName { get; private set; }

        public string ContactFirstName { get; private set; }

        public string ContactLastName { get; private set; }

        
        public string Cnpj { get; private set; }
        
        public string Email { get; private set; }

        public string Phone { get; private set; }


        //public string AddressStreet { get; private set; }

        //public string AddressNumber { get; private set; }

        //public string AddressComplement { get; private set; }

        //public string AddressQuarter { get; private set; }

        //public string AddressZipcode { get; private set; }

        //public string AddressCity { get; private set; }

        //public string AddressState { get; private set; }

        //public string AddressCountry { get; private set; }

        public Address BillingAddress { get; private set; }


        public string AccessKey { get; private set; }

        /// <summary>
        /// Filial Expedição
        /// </summary>
        public int SubsidiaryShipping { get; private set; }

        /// <summary>
        /// Filial Faturamento
        /// </summary>
        public int SubsidiaryBilling { get; private set; }
        
        public ClientIntegration(int clientID, string companyName, string contactFirstName, string contactLastName, string cnpj, string email, string phone, Address billingAddress)
        {
            this.ClientID = clientID;
            this.CompanyName = companyName;
            this.ContactFirstName = contactFirstName;
            this.ContactLastName = contactLastName;
            this.Cnpj = cnpj;
            this.Email = email;
            this.Phone = phone;
            this.BillingAddress = billingAddress;
        }
    }
}
