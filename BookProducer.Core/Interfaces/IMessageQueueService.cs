using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.MessageQueue.Interfaces
{
   public interface IMessageQueueService
    {
        public Task SendMessageAsync<T>(T messageInfo);
    }
}
