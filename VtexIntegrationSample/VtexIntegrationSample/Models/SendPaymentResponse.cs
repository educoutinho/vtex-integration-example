using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enginesoft.VtexIntegrationSample.Models
{
    public class SendPaymentResponse
    {
        public Models.SendPaymentStatusEnum Status { get; private set; }

        public string Message { get; private set; }

        public string ServiceCode { get; private set; }

        private SendPaymentResponse()
        {

        }

        public static SendPaymentResponse CreateSuccessResponse()
        {
            var obj = new SendPaymentResponse();
            obj.Status = SendPaymentStatusEnum.Success;
            return obj;
        }

        public static SendPaymentResponse CreateErrorResponse(Models.SendPaymentStatusEnum status, string message, string serviceCode)
        {
            var obj = new SendPaymentResponse();
            obj.Status = status;
            obj.Message = message;
            obj.ServiceCode = serviceCode;
            return obj;
        }
    }
}
