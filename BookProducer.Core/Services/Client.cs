using BookProducer.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookProducer.Core.Services
{
    public class Client : IClient
    {
        private readonly IConfiguration _configuration;
        public Client(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendMessageAsync<T>(T messageInfo)
        {
            try
            {
                IFactoryClient factory = new FactoryClient(_configuration);
                IMessageQueueClient message = factory.CreateMessageQueue(_configuration["MessageQueue:Type"]);
                await message.SendMessageAsync(messageInfo);
                            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}
