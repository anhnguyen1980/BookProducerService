using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using AutoFixture;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Newtonsoft.Json;
using BookProducerService.Repositories.Interfaces;
using BookProducer.Core.Entities;
using BookProducerService.Services;
using BookProducerService.Models.DTOs;

namespace BookProducerService.UnitTests.Services
{
    public class TaskServiceTests
    {
        private readonly MockRepository mockRepository;

        private readonly Mock<IGenericRepository<TaskHistory>> mockGenericRepository;
        private readonly Mock<IMapper> mockMapper;
        private readonly Fixture _fixture;

        public TaskServiceTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockGenericRepository = this.mockRepository.Create<IGenericRepository<TaskHistory>>();
            this.mockMapper = this.mockRepository.Create<IMapper>();
            _fixture = new Fixture();
        }

        private TaskService CreateService()
        {
            return new TaskService(
                this.mockGenericRepository.Object, mockMapper.Object);
        }

        [Fact]
        public async Task GetTask_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            TaskHistory taskHistory = _fixture.Build<TaskHistory>()
                .Without(f => f.Book)
                .Without(f => f.Status)
                .Create();
            mockGenericRepository.Setup(x => x.Get(taskHistory.Id)).ReturnsAsync(taskHistory);
            TaskDto taskDto = _fixture.Build<TaskDto>().With(x => x.Id, taskHistory.Id).Create();
            mockMapper.Setup(x => x.Map<TaskDto>(taskHistory)).Returns(taskDto);
            var service = this.CreateService();


            // Act
            var result = await service.GetTask(
                taskHistory.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(JsonConvert.SerializeObject(taskDto), JsonConvert.SerializeObject(result));
            this.mockRepository.VerifyAll();
        }
        [Fact]
        public async Task GetTask_StateUnderTest_NotFound()
        {
            // Arrange
            TaskHistory taskHistory = _fixture.Build<TaskHistory>()
                  .Without(f => f.Book)
                  .Without(f => f.Status)
                  .Create();
            mockGenericRepository.Setup(x => x.Get(taskHistory.Id)).ThrowsAsync(new ArgumentException());
            var service = this.CreateService();


            // Act
            var result = await service.GetTask(
                taskHistory.Id);

            // Assert
            Assert.Null(result);
            this.mockRepository.VerifyAll();
        }
        [Fact]
        public async Task GetTasks_StateUnderTest_GetAllTasksFail()
        {
            // Arrange
            var tasks = _fixture.Build<TaskHistory>()
                         .Without(f => f.Book)
                         .Without(f => f.Status)
                         .CreateMany();
            mockGenericRepository.Setup(x => x.GetAll()).ThrowsAsync(new ArgumentException());
            var service = this.CreateService();

            // Act
            var result = await service.GetTasks(
                String.Empty);
            // Assert
            Assert.Null(result);
            this.mockRepository.VerifyAll();
        }
        [Fact]
        public async Task GetTasks_StateUnderTest_GetAllTasksNULL()
        {
            // Arrange
            var tasks = _fixture.Build<TaskHistory>()
                          .Without(f => f.Book)
                          .Without(f => f.Status)
                          .CreateMany();
            mockGenericRepository.Setup(x => x.GetAll()).ReturnsAsync(tasks);

            var taskDtos = tasks.Select(i => new TaskDto()
            {
                Id = i.Id,
                CreatedDate = i.CreatedDate,
                Finish = i.Finish,
                Requested = i.Requested,
                StatusId = i.StatusId,
                UpdatedDate = i.UpdatedDate
            }).ToList();
            mockMapper.Setup(x => x.Map<ICollection<TaskDto>>(tasks)).Returns(taskDtos);
            var service = this.CreateService();

            // Act
            var result = await service.GetTasks(
                null);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(JsonConvert.SerializeObject(taskDtos), JsonConvert.SerializeObject(result));
            this.mockRepository.VerifyAll();
        }
        [Fact]
        public async Task GetTasks_StateUnderTest_GetAllTasksEmpty()
        {
            // Arrange
            var tasks = _fixture.Build<TaskHistory>()
                           .Without(f => f.Book)
                           .Without(f => f.Status)
                           .CreateMany();
            mockGenericRepository.Setup(x => x.GetAll()).ReturnsAsync(tasks);
            var taskDtos = tasks.Select(i => new TaskDto()
            {
                Id = i.Id,
                CreatedDate = i.CreatedDate,
                Finish = i.Finish,
                Requested = i.Requested,
                StatusId = i.StatusId,
                UpdatedDate = i.UpdatedDate
            }).ToList();
            mockMapper.Setup(x => x.Map<ICollection<TaskDto>>(tasks)).Returns(taskDtos);
            var service = this.CreateService();

            // Act
            var result = await service.GetTasks(
                String.Empty);

            // Assert
            Assert.NotNull(result);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task GetTasks_StateUnderTest_GetAllTasksEqualID()
        {
            // Arrange
            var tasks = _fixture.Build<TaskHistory>()
                         .Without(f => f.Book)
                         .Without(f => f.Status)
                         .CreateMany();
            string strFind = tasks.FirstOrDefault().Id.ToString();

            mockGenericRepository.Setup(s => s.GetAll()).ReturnsAsync(tasks);
            var taskDtos = tasks.Select(i => new TaskDto()
            {
                Id = i.Id,
                CreatedDate = i.CreatedDate,
                Finish = i.Finish,
                Requested = i.Requested,
                StatusId = i.StatusId,
                UpdatedDate = i.UpdatedDate
            }).ToList();
            mockMapper.Setup(x => x.Map<ICollection<TaskDto>>(tasks)).Returns(taskDtos);
            var service = this.CreateService();


            // Act
            var result = await service.GetTasks(strFind);

            // Assert
            Assert.Single(result);
            Assert.Contains(result.First(), taskDtos);
            this.mockRepository.VerifyAll();
        }
        [Fact]
        public async Task GetTasks_StateUnderTest_GetAllTasksContainRequest()
        {
            // Arrange
            var tasks = _fixture.Build<TaskHistory>()
                          .Without(f => f.Book)
                          .Without(f => f.Status)
                          .CreateMany();
            string strFind = tasks.FirstOrDefault().Requested.Substring(0, 3);

            mockGenericRepository.Setup(s => s.GetAll()).ReturnsAsync(tasks);
            var taskDtos = tasks.Select(i => new TaskDto()
            {
                Id = i.Id,
                CreatedDate = i.CreatedDate,
                Finish = i.Finish,
                Requested = i.Requested,
                StatusId = i.StatusId,
                UpdatedDate = i.UpdatedDate
            }).ToList();
            mockMapper.Setup(x => x.Map<ICollection<TaskDto>>(tasks)).Returns(taskDtos);
            var service = this.CreateService();
            // Act
            var result = await service.GetTasks(strFind) ;

            // Assert

            Assert.All(result, item => Assert.Contains(strFind, item.Requested));
            this.mockRepository.VerifyAll();
        }
        [Fact]
        public async Task GetTasks_StateUnderTest_GetAllTasksContainFinish()
        {
            // Arrange
            var tasks = _fixture.Build<TaskHistory>()
                          .Without(f => f.Book)
                          .Without(f => f.Status)
                          .CreateMany();
            string strFind = tasks.FirstOrDefault().Finish.Substring(0, 3);

            mockGenericRepository.Setup(s => s.GetAll()).ReturnsAsync(tasks);

            var taskDtos = tasks.Select(i => new TaskDto()
            {
                Id = i.Id,
                CreatedDate = i.CreatedDate,
                Finish = i.Finish,
                Requested = i.Requested,
                StatusId = i.StatusId,
                UpdatedDate = i.UpdatedDate
            }).ToList();
            mockMapper.Setup(x => x.Map<ICollection<TaskDto>>(tasks)).Returns(taskDtos);
            var service = this.CreateService();


            // Act
            var result = await service.GetTasks(strFind) ;

            // Assert

            Assert.All(result, item => Assert.Contains(strFind, item.Finish));
            this.mockRepository.VerifyAll();
        }
        [Fact]
        public async Task InsertTask_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            TaskHistory taskHistory = _fixture.Build<TaskHistory>()
                          .Without(f => f.Book)
                          .Without(f => f.Status)
                          .Create();
            mockGenericRepository.Setup(s => s.Insert(taskHistory)).ReturnsAsync(true);
            var service = this.CreateService();


            // Act
            var result = await service.InsertTask(
                taskHistory);

            // Assert
            Assert.True(result);
            this.mockRepository.VerifyAll();
        }
        [Fact]
        public async Task InsertTask_StateUnderTest_Fail()
        {
            // Arrange
            TaskHistory taskHistory = _fixture.Build<TaskHistory>()
                          .Without(f => f.Book)
                          .Without(f => f.Status)
                          .Create();
            mockGenericRepository.Setup(s => s.Insert(taskHistory)).Throws(new ArgumentException());
            var service = this.CreateService();


            // Act
            var result = await service.InsertTask(
                taskHistory);

            // Assert
            Assert.False(result);
            this.mockRepository.VerifyAll();
        }
        [Fact]
        public async Task UpdateTask_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            TaskHistory taskHistory = _fixture.Build<TaskHistory>()
                          .Without(f => f.Book)
                          .Without(f => f.Status)
                          .Create();
            mockGenericRepository.Setup(s => s.Update(taskHistory)).Returns(true);
            var service = this.CreateService();


            // Act
            var result = await service.UpdateTask(
                taskHistory);

            // Assert
            Assert.True(result);
            this.mockRepository.VerifyAll();
        }
        [Fact]
        public async Task UpdateTask_StateUnderTest_Fail()
        {
            // Arrange
            TaskHistory taskHistory = _fixture.Build<TaskHistory>()
                          .Without(f => f.Book)
                          .Without(f => f.Status)
                          .Create();
            mockGenericRepository.Setup(s => s.Update(taskHistory)).Throws(new ArgumentException());
            var service = this.CreateService();


            // Act
            var result = await service.UpdateTask(
                taskHistory);

            // Assert
            Assert.False(result);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task DeleteTask_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            TaskHistory taskHistory = _fixture.Build<TaskHistory>()
                          .Without(f => f.Book)
                          .Without(f => f.Status)
                          .Create();
            mockGenericRepository.Setup(s => s.Get(taskHistory.Id)).ReturnsAsync(taskHistory);
            mockGenericRepository.Setup(s => s.Delete(taskHistory)).Returns(true);
            var service = this.CreateService();


            // Act
            var result = await service.DeleteTask(
                taskHistory.Id);

            // Assert
            Assert.True(result);
            this.mockRepository.VerifyAll();
        }
        [Fact]
        public async Task DeleteTask_StateUnderTest_Fail()
        {
            // Arrange
            TaskHistory taskHistory = _fixture.Build<TaskHistory>()
                          .Without(f => f.Book)
                          .Without(f => f.Status)
                          .Create();
            mockGenericRepository.Setup(s => s.Get(taskHistory.Id)).ReturnsAsync(taskHistory);
            mockGenericRepository.Setup(s => s.Delete(taskHistory)).Throws(new ArgumentException());
            var service = this.CreateService();


            // Act
            var result = await service.DeleteTask(
               taskHistory.Id);

            // Assert
            Assert.False(result);
            this.mockRepository.VerifyAll();
        }
    }
}
