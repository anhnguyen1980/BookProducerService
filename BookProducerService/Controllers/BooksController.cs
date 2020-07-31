using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using Microsoft.AspNetCore.Cors;
using BookProducer.Core.Interfaces;
using BookProducerService.Services.Interfaces;
using BookProducerService.Models.DTOs;
using BookProducerService.Models.DomainModels;

namespace BookProducerService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  //  [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly ILogger<BooksController> _logger;
        //private readonly IConfiguration _configuration;
        private readonly IClient _messageQueueService;

        public BooksController(IBookService bookService, IClient messageQueueService, ILogger<BooksController> logger)
        {
            _bookService = bookService;
             _logger = logger;
            // _configuration = configuration;
            _messageQueueService = messageQueueService;
        }

        [HttpGet]      
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks(string strFind)
        {

            _logger.LogInformation("Get all books");
            try
            {
                IEnumerable<BookDto> books = await _bookService.GetBooks(strFind);
                return Ok(books);
            }
            catch (Exception ex)
            {
                _logger.LogError($"A error occurred");
                return NotFound(ex.InnerException);
                // throw;
            }

        }

        [HttpPost]
        public async Task<IActionResult> PostBook([FromBody] BookDto book)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid data.");
               
                MessageInfo messageInfo = new MessageInfo
                {
                    ActionType = "POST",
                    BookDto = book
                };

                await _messageQueueService.SendMessageAsync(messageInfo);// 
                                                                         // SendMessageAsync(messageInfo);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException);
            }


        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(Guid id, string title, string description)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid value");

            BookDto bookDto = await _bookService.GetBook(id);
            if (bookDto == null)
            {
                return NotFound();
            }
            try
            {
                bookDto.Title = title;
                bookDto.Description = description;

                MessageInfo messageInfo = new MessageInfo
                {
                    ActionType = "PUT",
                    BookDto = bookDto
                };

                await _messageQueueService.SendMessageAsync(messageInfo);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException);
                //throw ex;
            }

        }
        // DELETE: api/Books/5
        [HttpDelete("{id}")]
      //  [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteBook(Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid id");
            var book = await _bookService.GetBook(id); //_context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            try
            {
                MessageInfo messageInfo = new MessageInfo
                {
                    ActionType = "DELETE",
                    BookDto = book
                };
                await _messageQueueService.SendMessageAsync(messageInfo);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException);
                // throw;
            }

        }

    }
}
