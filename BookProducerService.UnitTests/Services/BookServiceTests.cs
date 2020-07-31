using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using AutoMapper;
using BookProducerService.Repositories.Interfaces;
using BookProducer.Core.Entities;
using BookProducerService.Services.Interfaces;
using BookProducerService.Services;
using BookProducerService.Models.DomainModels;
using BookProducerService.Models.DTOs;

namespace BookProducerService.UnitTests.Services
{
    public class BookServiceTests
    {
        private readonly MockRepository mockRepository;

        private readonly Mock<IGenericRepository<Book>> mockGenericRepository;
        private readonly Mock<IUnitOfWork> mockUnitOfWork;
        private readonly Mock<ITaskService> mockTaskService;
        private readonly Mock<ILogger<BookService>> mockLogger;
        private readonly Mock<IMapper> mockMapper;
        private readonly Fixture _fixture;
        public BookServiceTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockGenericRepository = this.mockRepository.Create<IGenericRepository<Book>>();
            this.mockUnitOfWork = this.mockRepository.Create<IUnitOfWork>();
            this.mockTaskService = this.mockRepository.Create<ITaskService>();
            this.mockLogger = new Mock<ILogger<BookService>>();
            //  this.mockLogger = this.mockRepository.Create<ILogger<BookService>>();
            this.mockMapper = this.mockRepository.Create<IMapper>();
            _fixture = new Fixture();
        }

        private BookService CreateService()
        {
            return new BookService(
                this.mockGenericRepository.Object,
                this.mockUnitOfWork.Object, mockTaskService.Object, mockLogger.Object, mockMapper.Object);
        }

        [Fact]
        public async Task AddBook_StateUnderTest_ExpectedBehavior()
        {
            MessageInfo messageInfo = new MessageInfo()
            {
                ActionType = "POST",
                BookDto = _fixture.Create<BookDto>()
            };
            Book newBook = new Book()
            {
                Id = messageInfo.BookDto.Id,
                AuthorId = messageInfo.BookDto.AuthorId,
                Date = messageInfo.BookDto.Date,
                Description = messageInfo.BookDto.Description,
                Title = messageInfo.BookDto.Title,
                TaskId = messageInfo.BookDto.TaskId
            };
            mockMapper.Setup(m => m.Map<Book>(messageInfo.BookDto)).Returns(newBook);
            Book book = null;
            mockGenericRepository.Setup(x => x.Get(messageInfo.BookDto.Id)).ReturnsAsync(book);
            mockGenericRepository.Setup(x => x.Insert(newBook)).ReturnsAsync(true);

            //    TaskHistory taskHistory = null;// new TaskHistory();
            //TaskDto taskDto = new TaskDto()
            //{
            //    Id = messageInfo.BookDto.TaskId,
            //    CreatedDate = DateTime.Now,
            //    Finish = "",
            //    Requested = "",
            //    StatusId = 1,
            //    UpdatedDate = DateTime.Now
            //};
            TaskHistory task = null;
            TaskDto taskDto = null;
            mockTaskService.Setup(x => x.GetTask(messageInfo.BookDto.TaskId)).ReturnsAsync(taskDto);
            mockMapper.Setup(x => x.Map<TaskHistory>(taskDto)).Returns(task);
            mockTaskService.Setup(x => x.InsertTask(It.IsAny<TaskHistory>())).ReturnsAsync(true);//It.IsAny<TaskHistory>())

            mockUnitOfWork.Setup(x => x.CommitAsync()).Returns(Task.CompletedTask);
            var service = this.CreateService();
            // Act
            var result = await service.SaveBook(
                messageInfo);

            // Assert         
            Assert.True(result);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task AddBook_StateUnderTest_Fail()
        {
            // Arrange
            MessageInfo messageInfo = new MessageInfo()
            {
                ActionType = "POST",
                BookDto = _fixture.Build<BookDto>()
                            .Create()
            };
            Book newBook = new Book()
            {
                Id = messageInfo.BookDto.Id,
                AuthorId = messageInfo.BookDto.AuthorId,
                Date = DateTime.Now,
                Description = "",
                TaskId = messageInfo.BookDto.TaskId,
                Title = ""

            };
            mockMapper.Setup(m => m.Map<Book>(messageInfo.BookDto)).Returns(newBook);
            Book book = null;
            mockGenericRepository.Setup(x => x.Get(messageInfo.BookDto.Id)).ReturnsAsync(book);
            mockGenericRepository.Setup(x => x.Insert(newBook)).ReturnsAsync(false);

            TaskHistory task = null;
            TaskDto taskDto = null;
            mockTaskService.Setup(x => x.GetTask(messageInfo.BookDto.TaskId)).ReturnsAsync(taskDto);
            mockMapper.Setup(x => x.Map<TaskHistory>(taskDto)).Returns(task);

            mockTaskService.Setup(x => x.InsertTask(It.IsAny<TaskHistory>())).ReturnsAsync(false);
            mockUnitOfWork.Setup(x => x.CommitAsync()).ThrowsAsync(new ArgumentException());

            var service = this.CreateService();
            // Act
            var result = await service.SaveBook(
                messageInfo);

            // Assert
            Assert.False(result);
            this.mockRepository.VerifyAll();
        }
        [Fact]
        public async Task UpdateBook_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            MessageInfo messageInfo = new MessageInfo()
            {
                ActionType = "PUT",
                BookDto = _fixture.Build<BookDto>()
                           .Create()
            };
            Book newBook = new Book()
            {
                Id = messageInfo.BookDto.Id,
                AuthorId = Guid.NewGuid(),
                Date = DateTime.Now,
                Description = "",
                TaskId = messageInfo.BookDto.TaskId,
                Title = ""

            };
            mockMapper.Setup(x => x.Map<Book>(messageInfo.BookDto)).Returns(newBook);
            mockGenericRepository.Setup(x => x.Get(messageInfo.BookDto.Id)).ReturnsAsync(newBook);
            mockGenericRepository.Setup(x => x.Update(newBook)).Returns(true);

            TaskDto taskDto = new TaskDto()
            {
                Id = messageInfo.BookDto.TaskId,
                CreatedDate = DateTime.Now,
                Finish = "",
                Requested = "",
                StatusId = 1,
                UpdatedDate = DateTime.Now
            };
            TaskHistory task = new TaskHistory()
            {
                Id = taskDto.Id,
                CreatedDate = DateTime.Now,
                Finish = "",
                Requested = "",
                StatusId = 1,
                UpdatedDate = DateTime.Now,

            };

            mockTaskService.Setup(x => x.GetTask(messageInfo.BookDto.TaskId)).ReturnsAsync(taskDto);
            mockMapper.Setup(x => x.Map<TaskHistory>(taskDto)).Returns(task);
            mockTaskService.Setup(x => x.UpdateTask(task)).ReturnsAsync(true);

            mockUnitOfWork.Setup(x => x.CommitAsync()).Returns(Task.CompletedTask);

            var service = this.CreateService();


            // Act
            var result = await service.SaveBook(
                messageInfo);

            // Assert
            Assert.True(result);
            this.mockRepository.VerifyAll();
        }
        [Fact]
        public async Task UpdateBook_StateUnderTest_Fail()
        {
            // Arrange
            MessageInfo messageInfo = new MessageInfo()
            {
                ActionType = "PUT",
                BookDto = _fixture.Build<BookDto>()
                         .Create()
            };
            Book newBook = new Book()
            {
                Id = messageInfo.BookDto.Id,
                AuthorId = Guid.NewGuid(),
                Date = DateTime.Now,
                Description = "",
                TaskId = messageInfo.BookDto.TaskId,
                Title = ""

            };
            mockMapper.Setup(x => x.Map<Book>(messageInfo.BookDto)).Returns(newBook);
            mockGenericRepository.Setup(x => x.Get(messageInfo.BookDto.Id)).ReturnsAsync(newBook);
            mockGenericRepository.Setup(x => x.Update(newBook)).Returns(false);

            TaskDto taskDto = _fixture.Build<TaskDto>().Create();
            TaskHistory task = new TaskHistory()
            {
                Id = taskDto.Id,
                CreatedDate = DateTime.Now,
                Finish = "",
                Requested = "",
                StatusId = 1,
                UpdatedDate = DateTime.Now,

            };
            mockTaskService.Setup(x => x.GetTask(messageInfo.BookDto.TaskId)).ReturnsAsync(taskDto);
            mockMapper.Setup(x => x.Map<TaskHistory>(taskDto)).Returns(task);
            mockTaskService.Setup(x => x.UpdateTask(task)).ReturnsAsync(false);

            mockUnitOfWork.Setup(x => x.CommitAsync()).ThrowsAsync(new ArgumentException());

            var service = this.CreateService();


            // Act
            var result = await service.SaveBook(
                messageInfo);

            // Assert
            Assert.False(result);
            this.mockRepository.VerifyAll();
        }
        [Fact]
        public async Task DeleteBook_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            // Guid id = Guid.Parse("d48d3522-364a-48e7-a7de-e3f685eec637");

            Book book = _fixture.Build<Book>()
                         .Without(f => f.TaskHistory).Without(f => f.Author).Without(f => f.BookGenre).Create();

            mockGenericRepository.Setup(x => x.Get(book.Id)).ReturnsAsync(book);
            mockGenericRepository.Setup(x => x.Delete(book)).Returns(true);
            mockUnitOfWork.Setup(x => x.CommitAsync()).Returns(Task.CompletedTask);

            var service = this.CreateService();


            // Act
            var result = await service.DeleteBook(book.Id);

            // Assert
            Assert.True(result);
            this.mockRepository.VerifyAll();
        }
        [Fact]
        public async Task DeleteBook_StateUnderTest_Fail()
        {
            // Arrange
            Book book = _fixture.Build<Book>()
                         .Without(f => f.TaskHistory).Without(f => f.Author).Without(f => f.BookGenre).Create();

            mockGenericRepository.Setup(x => x.Get(book.Id)).ReturnsAsync(book);
            mockGenericRepository.Setup(x => x.Delete(book)).Throws(new ArgumentException());
            //mockUnitOfWork.Setup(x => x.CommitAsync()).Returns(Task.CompletedTask);

            var service = this.CreateService();


            // Act
            var result = await service.DeleteBook(book.Id);

            // Assert
            Assert.False(result);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task GetBooks_StateUnderTest_GetAllBooksFail()
        {
            // Arrange

            mockGenericRepository.Setup(s => s.GetAll()).ThrowsAsync(new ArgumentException());//.ReturnsAsync(books);
            var service = this.CreateService();
            // Act
            var result = await service.GetBooks(string.Empty);

            // Assert
            Assert.Null(result);
            this.mockRepository.VerifyAll();
        }
        [Fact]
        public async Task GetBooks_StateUnderTest_GetAllBooksEmpty()
        {
            // Arrange
            var books = _fixture.Build<Book>()
                         .Without(f => f.TaskHistory).Without(f => f.Author).Without(f => f.BookGenre).CreateMany();

            mockGenericRepository.Setup(s => s.GetAll()).ReturnsAsync(books);
            var bookDTOs = _fixture.CreateMany<BookDto>();
            mockMapper.Setup(x => x.Map<ICollection<BookDto>>(books)).Returns(bookDTOs.ToArray());
            var service = this.CreateService();
            // Act
            var result = await service.GetBooks(string.Empty);

            // Assert

            Assert.NotNull(result);
            Assert.Equal(bookDTOs, result);
            this.mockRepository.VerifyAll();
        }
        [Fact]
        public async Task GetBooks_StateUnderTest_GetAllBooksNULL()
        {
            // Arrange
            var books = _fixture.Build<Book>()
                         .Without(f => f.TaskHistory).Without(f => f.Author).Without(f => f.BookGenre).CreateMany();

            mockGenericRepository.Setup(s => s.GetAll()).ReturnsAsync(books);
            var bookDTOs = _fixture.CreateMany<BookDto>();
            mockMapper.Setup(x => x.Map<ICollection<BookDto>>(books)).Returns(bookDTOs.ToArray());
            var service = this.CreateService();
            // Act
            var result = await service.GetBooks(null);

            // Assert

            Assert.NotNull(result);
            Assert.Equal(bookDTOs, result);
            this.mockRepository.VerifyAll();
        }
        [Fact]
        public async Task GetBooks_StateUnderTest_GetAllBooksEqualID()
        {
            // Arrange
            var books = _fixture.Build<Book>()
                           .Without(f => f.TaskHistory).Without(f => f.Author).Without(f => f.BookGenre).CreateMany();

            mockGenericRepository.Setup(s => s.GetAll()).ReturnsAsync(books);
            var bookDTOs = _fixture.CreateMany<BookDto>();
            mockMapper.Setup(x => x.Map<ICollection<BookDto>>(books)).Returns(bookDTOs.ToArray());
            string strFind = bookDTOs.FirstOrDefault().Id.ToString();
            var service = this.CreateService();

            // Act
            var result = await service.GetBooks(strFind);

            // Assert
            Assert.Single(result);
            Assert.Contains(bookDTOs, n => n.Id == result.FirstOrDefault().Id);
            this.mockRepository.VerifyAll();
        }
        [Fact]
        public async Task GetBooks_StateUnderTest_GetAllBooksContainTitle()
        {
            // Arrange
            var books = _fixture.Build<Book>()
                            .Without(f => f.TaskHistory).Without(f => f.Author).Without(f => f.BookGenre).CreateMany();
            var bookDTOs = _fixture.CreateMany<BookDto>();
            mockMapper.Setup(x => x.Map<ICollection<BookDto>>(books)).Returns(bookDTOs.ToArray());

            string strFind = bookDTOs.FirstOrDefault().Title.Substring(5);

            mockGenericRepository.Setup(s => s.GetAll()).ReturnsAsync(books);
            var service = this.CreateService();


            // Act
            var result = await service.GetBooks(strFind);
            
            // Assert
            Assert.Single(result);
            Assert.All(result, item => Assert.Contains(strFind, item.Title));//title of result contains strFind 
            Assert.Contains(bookDTOs, x => x.Title.Contains( result.FirstOrDefault().Title));//list contains resultRT
            this.mockRepository.VerifyAll();
        }
        [Fact]
        public async Task GetBooks_StateUnderTest_GetAllBooksContainDescription()
        {
            // Arrange
            var books = _fixture.Build<Book>()
                            .Without(f => f.TaskHistory).Without(f => f.Author).Without(f => f.BookGenre).CreateMany();
            string strFind = books.FirstOrDefault().Description.Substring(0, 5);

            mockGenericRepository.Setup(s => s.GetAll()).ReturnsAsync(books);
            var bookDtos = _fixture.CreateMany<BookDto>();
            mockMapper.Setup(x => x.Map<ICollection<BookDto>>(books)).Returns(bookDtos.ToArray());
            var service = this.CreateService();

            // Act
            var result = await service.GetBooks(strFind);

            // Assert

            Assert.All(result, item => Assert.Contains(strFind, item.Description));
            Assert.Equal(bookDtos, result);
            this.mockRepository.VerifyAll();
        }
        [Fact]
        public async Task GetBook_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Book book = _fixture.Build<Book>()
                            .Without(f => f.TaskHistory).Without(f => f.Author).Without(f => f.BookGenre).Create();
            mockGenericRepository.Setup(x => x.Get(book.Id)).ReturnsAsync(book);
            BookDto bookDto = _fixture.Create<BookDto>();
            mockMapper.Setup(x => x.Map<BookDto>(book)).Returns(bookDto);
            var service = this.CreateService();
            // Act
            var result = await service.GetBook(
                book.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(bookDto, result);
            this.mockRepository.VerifyAll();
        }
        [Fact]
        public async Task GetBook_StateUnderTest_NotFound()
        {
            // Arrange
            Book book = _fixture.Build<Book>()
                           .Without(f => f.TaskHistory).Without(f => f.Author).Without(f => f.BookGenre).Create();
            mockGenericRepository.Setup(x => x.Get(book.Id)).ThrowsAsync(new ArgumentException());
            var service = this.CreateService();


            // Act
            var result = await service.GetBook(
                book.Id);

            // Assert
            Assert.Null(result);
            this.mockRepository.VerifyAll();
        }
    }
}
