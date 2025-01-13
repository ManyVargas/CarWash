using Core.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Frameworks.RabbitMQ
{
    public class RabbitMqPublicador : IPublicadorMensajes
    {
        private readonly IConnection _connection;

        public RabbitMqPublicador(IConnection connection)
        {
            _connection = connection;
        }
        public async Task PublicarAsync<T>(string topic, T message)
        {
            using var channel = _connection.CreateModel();
            channel.ExchangeDeclare(exchange: topic, type: "fanout");

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            channel.BasicPublish(exchange: topic, routingKey: "", basicProperties: null, body: body);

            Console.WriteLine("Mensaje enviado correctamente a RabbitMQ");
            await Task.CompletedTask;
        }
    }
}
