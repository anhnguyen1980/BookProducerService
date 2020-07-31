using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Newtonsoft.Json;
using System.Net.Http;
using BookProducerService.Services.Interfaces;
using BookProducer.Core.Interfaces;
using BookProducerService.Models.DTOs;
using BookProducerService.Models.DomainModels;
using Microsoft.Extensions.Logging;
using BookProducerService.Controllers;

namespace BookProducerService.UnitTests.Controllers
{
    public class BooksControllerTests
    {
        private readonly MockRepository mockRepository;

        //private readonly Mock<IConfiguration> mockConfiguration;
        private readonly Mock<IBookService> mockBookService;
        private readonly Mock<IClient> mockMessageQueueService;
        private readonly Mock<ILogger<BooksController>> mockLogger;

        private readonly Fixture _fixture;
        public BooksControllerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            //this.mockConfiguration = this.mockRepository.Create<IConfiguration>();
            this.mockBookService = this.mockRepository.Create<IBookService>();
            this.mockMessageQueueService = this.mockRepository.Create<IClient>();
            this.mockLogger = new Mock<ILogger<BooksController>>();

            _fixture = new Fixture();
        }

        private BooksController CreateBooksController()
        {
            return new BooksController(
              //  this.mockConfiguration.Object,
                this.mockBookService.Object, mockMessageQueueService.Object,mockLogger.Object);
        }

        [Fact]
        public async Task GetBooks_StateUnderTest_GetAllBooksNotFound()
        {
            // Arrange

            mockBookService.Setup(s => s.GetBooks(It.IsAny<string>())).ThrowsAsync(new ArgumentException());//.ReturnsAsync(books);
            var booksController = this.CreateBooksController();

            // Act
            var result = await booksController.GetBooks("");

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);            
            this.mockRepository.VerifyAll();
        }
        [Fact]
        public async Task GetBooks_StateUnderTest_GetAllBooks()
        {
            // Arrange
           // List<Book> books = new List<Book>();
            // _fixture.AddManyTo(books);
            var books = _fixture.Build<BookDto>().CreateMany();

            mockBookService.Setup(s => s.GetBooks(It.IsAny<string>())).ReturnsAsync(books);
            var booksController = this.CreateBooksController();
            // Act
            var result = await booksController.GetBooks("");

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(JsonConvert.SerializeObject(books), JsonConvert.SerializeObject(((ObjectResult)result.Result).Value));
            this.mockRepository.VerifyAll();
        }
        [Fact]
        public async Task PostBook_StateUnderTest_ModeStateError()
        {
            // Arrange
            var booksController = this.CreateBooksController();
            booksController.ModelState.AddModelError("test", "test Post");

            //string title = _fixture.Create<string>();
            //string description = _fixture.Create<string>();
            var book = _fixture.Build<BookDto>().Create();
            // Act
            var result = await booksController.PostBook(book);
                          // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            this.mockRepository.VerifyAll();
        }
        [Fact]
        public async Task PostBook_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            mockMessageQueueService.Setup(x => x.SendMessageAsync(It.IsAny<MessageInfo>())).Returns(Task.CompletedTask);

            var booksController = this.CreateBooksController();
            var book = _fixture.Build<BookDto>().Create();

            // Act
            var result = await booksController.PostBook(
                book);

            // Assert
            Assert.IsType<OkResult>(result);
            this.mockRepository.VerifyAll();
        }
        [Fact]
        public async Task PostBook_StateUnderTest_UnExpectedBehavior()
        {
            // Arrange

            //string title = _fixture.Create<string>();
            //string description = _fixture.Create<string>();
            mockMessageQueueService.Setup(x => x.SendMessageAsync(It.IsAny<MessageInfo>())).ThrowsAsync(new ArgumentException());
            var booksController = this.CreateBooksController();
            var book = _fixture.Build<BookDto>().Create();
            // Act
            var result = await booksController.PostBook(book);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            this.mockRepository.VerifyAll();
        }
        [Fact]
        public async Task PutBook_StateUnderTest_ModeStateError()
        {
            // Arrange
            var booksController = this.CreateBooksController();
            booksController.ModelState.AddModelError("test", "test Put");

            string title = _fixture.Create<string>();
            string description = _fixture.Create<string>();

            // Act
            var result = await booksController.PutBook(_fixture.Create<Guid>(),
                title,
                description);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            this.mockRepository.VerifyAll();
        }
        [Fact]
        public async Task PutBook_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid id = Guid.Parse("54a89b5a-8302-48a5-98e9-879d0f99eb7e");

            mockBookService.Setup(x => x.GetBook(id)).ReturnsAsync(new BookDto());
            mockMessageQueueService.Setup(x => x.SendMessageAsync(It.IsAny<MessageInfo>())).Returns(Task.CompletedTask);

            var booksController = this.CreateBooksController();
            string title = _fixture.Create<string>();
            string description = _fixture.Create<string>();


            // Act
            var result = await booksController.PutBook(
                id,
                title,
                description);

            // Assert
            Assert.IsType<OkResult>(result);
            this.mockRepository.VerifyAll();
        }
        [Fact]
        public async Task PutBook_StateUnderTest_UnExpectedBehavior()
        {
            // Arrange
            Guid id = _fixture.Create<Guid>();

            mockBookService.Setup(x => x.GetBook(id)).ReturnsAsync(new BookDto());
            mockMessageQueueService.Setup(x => x.SendMessageAsync(It.IsAny<MessageInfo>())).ThrowsAsync(new ArgumentException());

            var booksController = this.CreateBooksController();

            string title = _fixture.Create<string>();
            string description = _fixture.Create<string>();

            // Act
            var result = await booksController.PutBook(
                id,
                title,
                description);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            this.mockRepository.VerifyAll();
        }
        [Fact]
        public async Task PutBook_StateUnderTest_NotFound()
        {
            // Arrange
            Guid id = _fixture.Create<Guid>();
            BookDto book = null;
            mockBookService.Setup(x => x.GetBook(id)).ReturnsAsync(book);

            var booksController = this.CreateBooksController();
            string title = _fixture.Create<string>();
            string description = _fixture.Create<string>();

            // Act
            var result = await booksController.PutBook(
                id,
                title,
                description);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            this.mockRepository.VerifyAll();
        }
        [Fact]
        public async Task DeleteBook_StateUnderTest_ModeStateError()
        {
            // Arrange
            var booksController = this.CreateBooksController();
            booksController.ModelState.AddModelError("test", "test Delete");

            //string title = _fixture.Create<string>();
            //string description = _fixture.Create<string>();

            // Act
            var result = await booksController.DeleteBook(_fixture.Create<Guid>());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            this.mockRepository.VerifyAll();
        }
        [Fact]
        public async Task DeleteBook_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid id = Guid.Parse("54a89b5a-8302-48a5-98e9-879d0f99eb7e");
            mockMessageQueueService.Setup(x => x.SendMessageAsync(It.IsAny<MessageInfo>())).Returns(Task.CompletedTask);

            mockBookService.Setup(x => x.GetBook(id)).ReturnsAsync(new BookDto());
            var booksController = this.CreateBooksController();

            // Act
            var result = await booksController.DeleteBook(
                id);

            // Assert
            Assert.IsType<OkResult>(result);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task DeleteBook_StateUnderTest_NotFound()
        {
            // Arrange
            Guid id = _fixture.Create<Guid>();
            BookDto book = null;
            mockBookService.Setup(x => x.GetBook(id)).ReturnsAsync(book);
            var booksController = this.CreateBooksController();

            // Act
            var result = await booksController.DeleteBook(
                id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task DeleteBook_StateUnderTest_UnExpectedBehavior()
        {
            // Arrange
            Guid id = _fixture.Create<Guid>();
            mockMessageQueueService.Setup(x => x.SendMessageAsync(It.IsAny<MessageInfo>())).ThrowsAsync(new ArgumentException());
            mockBookService.Setup(x => x.GetBook(id)).ReturnsAsync(new BookDto());
            var booksController = this.CreateBooksController();
            // Ac
            var result = await booksController.DeleteBook(
                id);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            this.mockRepository.VerifyAll();
        }
    }
}
