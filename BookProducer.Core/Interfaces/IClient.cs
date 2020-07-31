using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookProducer.Core.Interfaces
{
  public   interface IClient
    {
        Task SendMessageAsync<T>(T messageInfo);
    }
}
