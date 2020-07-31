using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookProducer.Core.Interfaces
{
    public interface IMessageQueueClient//:IDisposable
    {
        Task<bool> SendMessageAsync<T>(T messsageInfo);
    }
}
