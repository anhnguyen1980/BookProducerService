using BookProducer.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookProducer.Core.Services
{
    public class RabbitMQMessageQueueClient : IMessageQueueClient
    {
        private readonly string _connectionString;
        private readonly string _queueName;

        public RabbitMQMessageQueueClient(string connectionString, string queueName)
        {
            _connectionString = connectionString;
            _queueName = queueName;
        }
        public Task<bool> SendMessageAsync<T>(T messsageInfo)
        {
            try
            {
                var factory = new ConnectionFactory() { HostName = _connectionString };//"localhost"
                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                channel.QueueDeclare(queue: _queueName,
                              durable: false,
                              exclusive: false,
                              autoDelete: false,
                              arguments: null);
                string message = JsonConvert.SerializeObject(messsageInfo);
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "",
                                       routingKey: _queueName,
                                       basicProperties: null,
                                       body: body);

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return Task.FromResult(true);
        }
    }
}
