using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enginesoft.VtexIntegrationSample.Models
{
    public enum OrderStatusEnum
    {
        New = 1,

        Paid = 20,

        Billed = 30,

        Delivered = 40,

        Closed = 50,

        Canceled = 60,

        Unavailable = 100
    }
}
