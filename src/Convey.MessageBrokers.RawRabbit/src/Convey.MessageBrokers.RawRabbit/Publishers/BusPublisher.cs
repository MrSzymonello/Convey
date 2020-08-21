using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RawRabbit;
using RawRabbit.Enrichers.MessageContext;
using RawRabbit.Operations.Publish.Context;

namespace Convey.MessageBrokers.RawRabbit.Publishers
{
    internal sealed class BusPublisher : IBusPublisher
    {
        private readonly IBusClient _busClient;

        public BusPublisher(IBusClient busClient)
        {
            _busClient = busClient;
        }

        public Task PublishAsync<T>(T message, string messageId = null, string correlationId = null,
            string spanContext = null, object messageContext = null, IDictionary<string, object> headers = null, string routingKey = null)
            where T : class
        {
            if (routingKey != null)
            {
                return _busClient.PublishAsync(message, ctx => ctx.UseMessageContext(messageContext).UsePublishConfiguration(cfg => cfg.WithRoutingKey(routingKey)));
            }

            return _busClient.PublishAsync(message, ctx => ctx.UseMessageContext(messageContext));
        }
    }
}