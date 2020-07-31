using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using AutoFixture;
using System.Linq;
using FluentAssertions;
using Newtonsoft.Json;
using BookProducer.Core.Entities;
using BookProducerService.UnitTests.Repositories.TestFixtures;
using BookProducerService.Repositories;

namespace BookProducerService.UnitTests.Repositories
{
    public class GenericRepositoryTests : IClassFixture<ApplicationDbContextFixture>
    {
        private readonly ApplicationDbContextFixture _dbContextFixture;
        private readonly Fixture _fixture;
        readonly MockRepository mockRepository;

        public GenericRepositoryTests(ApplicationDbContextFixture dbContextFixture)
        {
            _dbContextFixture = dbContextFixture;
            _fixture = new Fixture();
            mockRepository = new MockRepository(MockBehavior.Strict);
        }
        private UnitOfWork CreateUnitOfWork()
        {
            return new UnitOfWork(_dbContextFixture.AppDbContext);
        }

        [Fact]
        public async Task GetAll_StateUnderTest_ExpectedBehavior()
        {
            //Arrange
            var books = this._dbContextFixture.AppDbContext.Books;
            var unitOfWork = CreateUnitOfWork();
            var repositories = unitOfWork.GetRepositoryAsync<Book>();

            //Act
            var result = await repositories.GetAll();
            //Assert
            Assert.Equal(books, result);
            mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Get_StateUnderTest_ExpectedBehavior()
        {
            //Arrange
            var book = _dbContextFixture.AppDbContext.Books.FirstOrDefault();
            var unitOfWork = CreateUnitOfWork();
            var repositories = unitOfWork.GetRepositoryAsync<Book>();
            object id = book.Id;
            //Act
            var result = await repositories.Get(id);
            //Assert
            Assert.NotNull(result);
            Assert.Equal(book, result);
            //  var bookTest=JsonConvert.SerializeObject(book);
            var bookExpect = JsonConvert.SerializeObject(book, Formatting.None,
                          new JsonSerializerSettings()
                          {
                              ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                          });
            var bookActual = JsonConvert.SerializeObject(result, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
            // Assert.Equal(JsonConvert.SerializeObject(book), JsonConvert.SerializeObject(result));//  Newtonsoft.Json.JsonSerializationException : Self referencing loop detected for property 'Status' with type 'BookProducer.Core.Entities.Status'. Path 'TaskHistory.Status.TaskHistory[0]'.
            Assert.Equal(bookExpect, bookActual);
            mockRepository.VerifyAll();
        }
        [Fact]
        public async Task Get_StateUnderTest_NotFound()
        {
            //Arrange
            var unitOfWork = CreateUnitOfWork();
            var repositories = unitOfWork.GetRepositoryAsync<Book>();
            //Act
            var result = await repositories.Get(null);
            //Assert
            result.Should().BeNull();
            mockRepository.VerifyAll();
        }
        //[Fact]
        //public async Task Get_StateUnderTest_ExpectedBehavior_Object()
        //{
        //    //Arrange
        //    var task = _dbContextFixture.AppDbContext.TaskHis.FirstOrDefault();
        //    var unitOfWork = CreateUnitOfWork();
        //    var repositories = unitOfWork.GetRepositoryAsync<TaskHistory>();
        //    //Act
        //    var result = await repositories.Get(task.Id);
        //    //Assert
        //    Assert.NotNull(result);
        //    Assert.Equal(task, result);
        //    //  var bookTest=JsonConvert.SerializeObject(book);
        //    var taskExpect = JsonConvert.SerializeObject(task, Formatting.None,
        //                  new JsonSerializerSettings()
        //                  {
        //                      ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        //                  });
        //    var taskActual = JsonConvert.SerializeObject(result, Formatting.None,
        //                new JsonSerializerSettings()
        //                {
        //                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        //                });
        //    // Assert.Equal(JsonConvert.SerializeObject(book), JsonConvert.SerializeObject(result));//  Newtonsoft.Json.JsonSerializationException : Self referencing loop detected for property 'Status' with type 'BookProducer.Core.Entities.Status'. Path 'TaskHistory.Status.TaskHistory[0]'.
        //    Assert.Equal(taskExpect, taskActual);
        //    mockRepository.VerifyAll();
        //}
        [Fact]
        public async Task Insert_StateUnderTest_ExpectedBehaviorAsync()
        {
            //Arrange
            Book book = _fixture.Build<Book>().Without(f => f.BookGenre).Without(f => f.Author).Without(f => f.TaskHistory).Create();
            var unitOfWork = CreateUnitOfWork();
            var repositories = unitOfWork.GetRepositoryAsync<Book>();
            //Act
            var success = repositories.Insert(book);
            await unitOfWork.CommitAsync();

            var addedBook = _dbContextFixture.AppDbContext.Books.FirstOrDefault(i => i.Id == book.Id);
            // Assert
            success.Should().Equals(true);
            addedBook.Should().NotBeNull();
            addedBook.Should().BeOfType<Book>();
            addedBook.Should().Equals(book);
            var bookExpect = JsonConvert.SerializeObject(book, Formatting.None,
                         new JsonSerializerSettings()
                         {
                             ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                         });
            var bookActual = JsonConvert.SerializeObject(addedBook, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
            Assert.Equal(bookExpect, bookActual);
            mockRepository.VerifyAll();
        }
        [Fact]
        public async void Insert_StateUnderTest_NULL()
        {
            //arrange
            var unitOfWork = CreateUnitOfWork();
            var repositories = unitOfWork.GetRepositoryAsync<Book>();
            //Act
            var success = await repositories.Insert(null);
            // Assert
            success.Should().BeFalse();
            mockRepository.VerifyAll();
        }
        [Fact]
        public async Task Update_StateUnderTest_ExpectedBehaviorAsync()
        {
            //arrange
            var book = _dbContextFixture.AppDbContext.Books.FirstOrDefault();
            book.Title = "new Title";
            book.Description = "new Description";
            var unitOfWork = CreateUnitOfWork();
            var repositories = unitOfWork.GetRepositoryAsync<Book>();
            //Act
            var success = repositories.Update(book);
            await unitOfWork.CommitAsync();

            var result = _dbContextFixture.AppDbContext.Books.FirstOrDefault(i => i.Id == book.Id);
            // Assert
            success.Should().BeTrue();
            result.Should().NotBeNull();
            result.Should().BeOfType<Book>();
            result.Should().Equals(book);
            var bookExpect = JsonConvert.SerializeObject(book, Formatting.None,
                                    new JsonSerializerSettings()
                                    {
                                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                    });
            var bookActual = JsonConvert.SerializeObject(result, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
            Assert.Equal(bookExpect, bookActual);
            mockRepository.VerifyAll();
        }
        [Fact]
        public void Update_StateUnderTest_NULL()
        {
            //arrange
            var unitOfWork = CreateUnitOfWork();
            var repositories = unitOfWork.GetRepositoryAsync<Book>();
            //Act
            var success = repositories.Update(null);
            // Assert
            success.Should().BeFalse();
            mockRepository.VerifyAll();
        }
        [Fact]
        public async Task Delete_StateUnderTest_ExpectedBehaviorAsync()
        {
            //arrange
            var book = _dbContextFixture.AppDbContext.Books.FirstOrDefault();
            var unitOfWork = CreateUnitOfWork();
            var repositories = unitOfWork.GetRepositoryAsync<Book>();
            //Act
            var success = repositories.Delete(book);
            await unitOfWork.CommitAsync();
            var result = _dbContextFixture.AppDbContext.Books.FirstOrDefault(i => i.Id == book.Id);
            // Assert
            success.Should().BeTrue();
            result.Should().BeNull();
            mockRepository.VerifyAll();
        }
        [Fact]
        public void Delete_StateUnderTest_NULL()
        {
            //arrange
            var unitOfWork = CreateUnitOfWork();
            var repositories = unitOfWork.GetRepositoryAsync<Book>();
            //Act
            var success = repositories.Delete(null);
            // Assert
            success.Should().BeFalse();
            mockRepository.VerifyAll();            

        }
    }
}
