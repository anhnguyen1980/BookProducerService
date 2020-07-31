using System;
using System.Collections.Generic;
using System.Text;

namespace BookProducer.Core.Interfaces
{
  public  interface IFactoryClient
    {
        public IMessageQueueClient CreateMessageQueue(string type);
    }
}
