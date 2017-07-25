using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enginesoft.VtexIntegrationSample.Models
{
    public class CompleteOrderResponse
    {
        public CompleteOrderStatusEnum Status { get; set; }

        public string Message { get; set; }

        public string ServiceCode { get; set; }

        //Construtor privado, utilize os métodos estáticos
        private CompleteOrderResponse()
        {
            
        }

        public static CompleteOrderResponse CreateSuccessResponse(string serviceCode)
        {
            var response = new CompleteOrderResponse();
            response.Status = CompleteOrderStatusEnum.Success;
            response.ServiceCode = serviceCode;
            return response;
        }

        public static CompleteOrderResponse CreateErrorResponse(CompleteOrderStatusEnum status, string serviceCode, string message)
        {
            var response = new CompleteOrderResponse();
            response.Status = status;
            response.ServiceCode = serviceCode;
            response.Message = message;
            return response;
        }
    }
}
