using BookProducer.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookProducer.Core.Services
{
    public class FactoryClient : IFactoryClient
    {
        private readonly IConfiguration _configuration;
      
        public FactoryClient(IConfiguration configuration)
        {
            _configuration = configuration;
           
        }
        public IMessageQueueClient CreateMessageQueue(string type)
        {
            if (type == "RabbitMQ")
                return new RabbitMQMessageQueueClient(_configuration["RabbitMQ:ConnectionString"], _configuration["RabbitMQ:QueueName"]);
            else if(type == "NATS")
                return new NATSMessageQueueClient(_configuration["NATS:ConnectionString"], _configuration["NATS:QueueName"]);
            else
                return new AzureMessageQueueClient(_configuration["AzureServiceBus:ConnectionString"], _configuration["AzureServiceBus:QueueName"]);
        }
    }
}
