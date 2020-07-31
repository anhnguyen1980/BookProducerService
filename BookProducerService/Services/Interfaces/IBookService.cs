using BookProducerService.Models.DomainModels;
using BookProducerService.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookProducerService.Services.Interfaces
{
    public interface IBookService
    {
       //Task< bool> InsertBook(Book book);
       //Task<bool> UpdateBook(Book book);
       Task<bool> SaveBook(MessageInfo messageInfo);

       Task< bool> DeleteBook(Guid id);
        Task<IEnumerable<BookDto>> GetBooks(string strFind);
        Task<BookDto> GetBook(Guid id);
    }
}
