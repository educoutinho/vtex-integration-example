using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enginesoft.VtexIntegrationSample.Models
{
    /// <summary>
    /// Dados do tipo de forma de pagamento disponível, ex: 10 parcelas para Cartão Amex (Cartão Amex = tipo de forma de pagamento)
    /// </summary>
    public class PaymentCondition
    {
        public string PaymentConditionCode { get; private set; }

        public string PaymentConditionGroupCode { get; private set; }

        public string Name { get; private set; }
        
        public Models.PaymentTypesEnum PaymentConditionTypeID { get; private set; }
                
        public Decimal Value { get; private set; }
        
        public List<PaymentInstallment> PaymentInstallmentsList { get; private set; }

        public PaymentCondition(string paymentConditionCode, string paymentConditionGroupCode, string name, Models.PaymentTypesEnum paymentConditionTypeID, decimal value, List<PaymentInstallment> paymentInstallmentsList)
        {
            this.PaymentConditionCode = paymentConditionCode;
            this.PaymentConditionGroupCode = paymentConditionGroupCode;
            this.Name = name;
            this.PaymentConditionTypeID = paymentConditionTypeID;
            this.Value = value;
            this.PaymentInstallmentsList = paymentInstallmentsList;
        }

        public void UpdatePaymentConditionTypeID(Models.PaymentTypesEnum paymentConditionTypeID)
        {
            this.PaymentConditionTypeID = paymentConditionTypeID;
        }
    }
}
