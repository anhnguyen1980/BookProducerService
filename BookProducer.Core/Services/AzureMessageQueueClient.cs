using BookProducer.Core.Interfaces;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookProducer.Core.Services
{
  public  class AzureMessageQueueClient: IMessageQueueClient
    {
        private readonly string _connectionString;
        private readonly string _queueName;
       // private bool disposed = false;

        public AzureMessageQueueClient(string connectionString, string queueName)
        {
            _connectionString = connectionString;
            _queueName = queueName;
        }

        //public void Dispose()
        //{
        //    Dispose(true);
        //    GC.SuppressFinalize(this._queueClient);
        //}
        //protected virtual void Dispose(bool disposing)
        //{
        //    if (disposed)
        //        return;

        //    if (disposing)
        //    {
        //        if (!_queueClient.IsClosedOrClosing)
        //        {
        //            _queueClient.CloseAsync().Wait();
        //        }
        //    }

        //    disposed = true;
        //}
        public async Task<bool> SendMessageAsync<T>( T messsageInfo)
        {
            //string ServiceBusConnectionString = configuration["AzureServiceBus:ConnectionString"];//; @"Endpoint=sb://anhnguyen.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=qTKznJRadfRhedcnYAR6NC9CVM9DPEoMbcF9PSIy27Q=";
            //string QueueName = configuration["AzureServiceBus:QueueName"];
            IQueueClient queueClient = new QueueClient(_connectionString, _queueName);
            try
            {
                string messageBody = JsonConvert.SerializeObject(messsageInfo);
                var message = new Message(Encoding.UTF8.GetBytes(messageBody));

                await queueClient.SendAsync(message);
                await queueClient.CloseAsync();
            return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
          
        }
    }
}
