using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enginesoft.VtexIntegrationSample.Models
{
    public class PaymentInstallment
    {
        public int Count { get; private set; }

        public decimal Value { get; private set; }

        public decimal Total { get; private set; }

        public decimal InstallmentRate { get; private set; }

        public PaymentInstallment(int count, decimal value, decimal total, decimal installmentRate)
        {
            this.Count = count;
            this.Value = value;
            this.Total = total;
            this.InstallmentRate = installmentRate;
        }
    }
}
