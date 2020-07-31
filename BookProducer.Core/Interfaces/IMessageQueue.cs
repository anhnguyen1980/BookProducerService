
using BookStore.Models.Common;
using BookStore.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.MessageQueue.Interfaces
{
    public interface IMessageQueue
    {
        Task<bool> SendBookAsync<T>(IConfiguration configuration, T messsageInfo);
        Task<bool> ReceiveBookAsync(IConfiguration configuration, IBookService bookService, ISaveData saveData);
    }
}
