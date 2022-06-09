using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Product.Catalog.Service.Domain.RabbitMQ.Interfaces
{
    public interface IRabbitManager
    {
        void Publish(string message, string exchangeName, string exchangeType, string routeKey);
    }
}
