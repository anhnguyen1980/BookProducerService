using BookProducer.Core.Interfaces;
using NATS.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookProducer.Core.Services
{
    public class NATSMessageQueueClient : IMessageQueueClient
    {
        private readonly string _connectionString;
        private readonly string _queueName;
        private IConnection _client;
        public NATSMessageQueueClient(string connectionString, string queueName)
        {
            _connectionString = connectionString;
            _queueName = queueName;
        }
        public Task<bool> SendMessageAsync<T>(T messsageInfo)
        {
            try
            {
                // Create a new connection factory to create
                // a connection.
                ConnectionFactory cf = new ConnectionFactory();
                // Creates a live connection to the default
                // NATS Server running locally
                _client = cf.CreateConnection(_connectionString);
                string message = JsonConvert.SerializeObject(messsageInfo);
                var data = Encoding.UTF8.GetBytes(message);
                _client.Publish(_queueName, data);
                return Task.FromResult(true);
            }
            catch (Exception)
            {

                return Task.FromResult(false);
            }

        }

    }
}
