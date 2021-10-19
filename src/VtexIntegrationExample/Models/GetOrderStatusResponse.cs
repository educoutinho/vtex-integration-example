using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enginesoft.VtexIntegrationSample.Models
{
    public class GetOrderStatusResponse
    {
        public GetOrderRequestStatusEnum RequestStatus { get; private set; }

        public OrderStatusEnum OrderStatus { get; private set; }

        public string Message { get; private set; }

        public string ServiceCode { get; private set; }

        public DateTime? OrderDate { get; private set; }

        public DateTime? PaymentDate { get; private set; }

        public DateTime? BillingDate { get; private set; }

        public DateTime? DeliveryDate { get; private set; }

        public DateTime? CloseDate { get; private set; }

        public DateTime? CancelDate { get; private set; }

        //Construtor privado, utilize os métodos estáticos
        private GetOrderStatusResponse()
        {
            
        }

        public static GetOrderStatusResponse CreateError(GetOrderRequestStatusEnum requestStatus, string message, string serviceCode)
        {
            GetOrderStatusResponse obj = new GetOrderStatusResponse();
            obj.RequestStatus = requestStatus;
            obj.ServiceCode = serviceCode;
            obj.Message = message;
            return obj;
        }

        public static GetOrderStatusResponse CreateSuccess(OrderStatusEnum orderStatus, 
            DateTime? orderDate, DateTime? paymentDate, DateTime? billingDate, DateTime? deliveryDate, DateTime? closeDate, DateTime? cancelDate)
        {
            GetOrderStatusResponse obj = new GetOrderStatusResponse();
            obj.RequestStatus = GetOrderRequestStatusEnum.Success;
            obj.OrderStatus = orderStatus;
            obj.OrderDate = orderDate;
            obj.PaymentDate = paymentDate;
            obj.BillingDate = billingDate;
            obj.DeliveryDate = deliveryDate;
            obj.CloseDate = closeDate;
            obj.CancelDate = cancelDate;
            return obj;
        }
    }
}
