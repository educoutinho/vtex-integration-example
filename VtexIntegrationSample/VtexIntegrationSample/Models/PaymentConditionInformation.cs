using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enginesoft.VtexIntegrationSample.Models
{
    /// <summary>
    /// Forma de pagamento selecionada, ex: 100,00 no Cartão Amex (tipo de forma de pagamento) em 2x
    /// </summary>
    public class PaymentConditionInformation
    {
        public PaymentCondition PaymentCondition { get; private set; }
                
        public Decimal Value { get; private set; }


        public int InstallmentQuantity { get; private set; }

        public decimal InstallmentValue { get; private set; }


        public string DocumentNumber { get; private set; }
                
        public string CardNumber { get; private set; }

        public string HolderName { get; private set; }

        public int DueYear { get; private set; }

        public int DueMonth { get; private set; }
        
        public string ValidationCode { get; private set; }
        
        
        public PaymentConditionInformation(PaymentCondition paymentCondition, decimal value, int installmentsQuantity, decimal installmentsValue)
        {
            this.PaymentCondition = paymentCondition;
            this.Value = value;
            this.InstallmentQuantity = installmentsQuantity;
            this.InstallmentValue = installmentsValue;
        }
        
        public void SetValue(decimal value)
        {
            this.Value = value;
        }

        public void SetInstallmentValue(int installmentQuantity, decimal installmentValue)
        {
            this.InstallmentQuantity = installmentQuantity;
            this.InstallmentValue = installmentValue;
        }

        public void SetCreditCardInformation(string cardNumber, string holderNumer, int dueYear, int dueMonth, string validationCode, string documentNumber)
        {
            this.CardNumber = cardNumber;
            this.HolderName = holderNumer;
            this.DueYear = dueYear;
            this.DueMonth = dueMonth;
            this.ValidationCode = validationCode;
            this.DocumentNumber = documentNumber;
        }

        public string GetDueDateVtexTest()
        {
            string month = this.DueMonth.ToString("00");
            string year = this.DueYear.ToString("00");

            string text = string.Format("{0}/{1}", month.Substring(0, 2), year.Substring(year.Length - 2));
            return text;
        }
    }
}
