using Core.Interfaces;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Infrastructure.Frameworks.RabbitMQ
{
    public class RabbitMqSuscriptor : ISuscriptorMensajes
    {
        private readonly IConnection _connection;

        public RabbitMqSuscriptor(IConnection connection)
        {
            _connection = connection;
        }

        public void Subscribir<T>(string topic, Func<T, Task> onMessageReceived)
        {
            using var channel = _connection.CreateModel();
            channel.ExchangeDeclare(exchange: topic, type: "fanout");

            var queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queue: queueName, exchange: topic, routingKey: "");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(body));
                await onMessageReceived(message);
            };

            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }
    }
}
