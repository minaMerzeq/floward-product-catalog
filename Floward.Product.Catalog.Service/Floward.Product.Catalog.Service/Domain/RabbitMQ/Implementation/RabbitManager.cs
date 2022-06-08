using Floward.Product.Catalog.Service.Domain.RabbitMQ.Interfaces;
using Microsoft.Extensions.ObjectPool;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Floward.Product.Catalog.Service.Domain.RabbitMQ.Implementation
{
    public class RabbitManager : IRabbitManager
    {
        private readonly DefaultObjectPool<IModel> _objectPool;

        public RabbitManager(IPooledObjectPolicy<IModel> objectPolicy)
        {
            _objectPool = new DefaultObjectPool<IModel>(objectPolicy, Environment.ProcessorCount * 2);
        }

        public void Publish(string message, string exchangeName, string exchangeType, string routeKey)
        {
            if (message == "")
                return;

            var channel = _objectPool.Get();

            try
            {
                channel.ExchangeDeclare(exchangeName, exchangeType, false, false, null);

                var sendBytes = Encoding.UTF8.GetBytes(message);

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.BasicPublish(exchangeName, routeKey, properties, sendBytes);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _objectPool.Return(channel);
            }
        }
    }
}
