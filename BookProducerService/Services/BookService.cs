using AutoMapper;
using BookProducer.Core.Entities;
using BookProducerService.Models.DomainModels;
using BookProducerService.Models.DTOs;
using BookProducerService.Repositories.Interfaces;
using BookProducerService.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookProducerService.Services
{
    public class BookService : IBookService
    {
        private readonly IGenericRepository<Book> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITaskService _taskService;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public BookService(IGenericRepository<Book> repository, IUnitOfWork unitOfWork, ITaskService taskService
            , ILogger<BookService> logger, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _taskService = taskService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<bool> SaveBook(MessageInfo messageInfo)
        {
            try
            {
                //save book
                BookDto bookDto = messageInfo.BookDto;
                Book newBook = _mapper.Map<Book>(bookDto);
                Book book = await _repository.Get(newBook.Id);
                if (book == null)
                    await _repository.Insert(newBook);
                else
                {
                    book.Title = newBook.Title;
                    book.Description = newBook.Description;
                    _repository.Update(book);
                }
                //save history


                var taskDto = await _taskService.GetTask(messageInfo.BookDto.TaskId);
                TaskHistory taskHistory = _mapper.Map<TaskHistory>(taskDto);
                string finish = DateTime.Now.ToString("yyyyMMddHHmmss");
                if (taskHistory == null)
                {
                    taskHistory = new TaskHistory
                    {
                        Id = messageInfo.BookDto.Id,
                        Requested = messageInfo.BookDto.Date.ToString("yyyyMMddHHmmssff"),
                        Finish = finish,
                        StatusId = 1
                    };
                    await _taskService.InsertTask(taskHistory);
                }
                else
                {
                    taskHistory.Finish = finish;
                    await _taskService.UpdateTask(taskHistory);
                }
                await _unitOfWork.CommitAsync();

                 _logger.LogInformation("Saved a book successfully");

                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Saved a book fail \n {0}", ex.Message));
                return await Task.FromResult(false);
            }

        }
        public async Task<bool> DeleteBook(Guid id)
        {
            bool res = false;
            try
            {
                Book book = await _repository.Get(id);
                if (book != null)
                {
                    res = _repository.Delete(book);
                    //  await _taskService.DeleteTask(messageInfo.book.taskHistoryid);
                    await _unitOfWork.CommitAsync();
                    _logger.LogInformation($"Deleted a book {id} successfully");
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(string.Format("Deleted a book fail \n {0}", ex.Message));
                res = false;
            }

            return await Task.FromResult(res);
        }
        public async Task<IEnumerable<BookDto>> GetBooks(string strFind)
        {
            try
            {
                IEnumerable<Book> books = await _repository.GetAll();
                var booksDto = _mapper.Map<ICollection<BookDto>>(books);
                if (strFind != null && strFind != "")
                    return booksDto.Where(item => item.Description.Contains(strFind) || item.Title.Contains(strFind) || item.Id.ToString().Equals(strFind)).ToList();
                else
                    return booksDto;
            }
            catch// (Exception ex)
            {

                return null;
            }

        }

        public async Task<BookDto> GetBook(Guid id)
        {
            try
            {
                var book = await _repository.Get(id);
                return _mapper.Map<BookDto>(book);
            }
            catch //(Exception ex)
            {

                return null;
            }

        }

    }
}
