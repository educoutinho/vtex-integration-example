using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enginesoft.VtexIntegrationSample.Models
{
    public class SendOrderResponse
    {
        public SendOrderStatusEnum Status { get; private set; }

        public string Message { get; private set; }

        public string ServiceCode { get; private set; }

        public string OrderNumber { get; private set; }


        public List<string> ChildOrders { get; private set; }

        public string PaymentTransactionCode { get; private set; }

        public string BankSlipUrl { get; private set; }
        

        public Dictionary<string, string> Cookies { get; private set; }


        //Construtor privado, utilize os métodos estáticos
        private SendOrderResponse()
        {
            this.ChildOrders = new List<string>();
        }

        public static SendOrderResponse CreateSuccessResponse(string serviceCode, string orderNumber, string paymentTransactionCode, string bankSlipUrl, List<string> childOrders, Dictionary<string, string> cookies)
        {
            var response = new SendOrderResponse();
            response.Status = SendOrderStatusEnum.Success;
            response.ServiceCode = serviceCode;
            response.OrderNumber = orderNumber;
            response.PaymentTransactionCode = paymentTransactionCode;
            response.BankSlipUrl = bankSlipUrl;
            response.ChildOrders = childOrders;
            response.Cookies = cookies;
            return response;
        }

        public static SendOrderResponse CreateErrorResponse(SendOrderStatusEnum status, string serviceCode, string message)
        {
            var response = new SendOrderResponse();
            response.Status = status;
            response.ServiceCode = serviceCode;
            response.Message = message;
            return response;
        }
    }
}
