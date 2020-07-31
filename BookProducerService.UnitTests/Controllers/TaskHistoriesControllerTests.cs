using AutoFixture;
using BookProducer.Core.Entities;
using BookProducerService.Controllers;
using BookProducerService.Models.DTOs;
using BookProducerService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BookProducerService.UnitTests.Controllers
{
    public class TaskHistoriesControllerTests
    {
        private readonly MockRepository mockRepository;

        private readonly Mock<ITaskService> mockTaskService;
        private readonly Fixture _fixture;
        public TaskHistoriesControllerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockTaskService = this.mockRepository.Create<ITaskService>();
            _fixture = new Fixture();
        }

        private TaskHistoriesController CreateTaskHistoriesController()
        {
            return new TaskHistoriesController(
                this.mockTaskService.Object);
        }

        [Fact]
        public async Task GetTaskHis_StateUnderTest_GetAllTasks()
        {
            // Arrange
            var tasks = _fixture.Build<TaskHistory>().Without(f => f.Book).Without(f => f.Status).CreateMany();
            var taskDtos = _fixture.CreateMany<TaskDto>();
            mockTaskService.Setup(s => s.GetTasks(It.IsAny<string>())).ReturnsAsync(taskDtos);
            var taskHistoriesController = this.CreateTaskHistoriesController();

            // Act
            var result = await taskHistoriesController.GetTaskHis("");

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            this.mockRepository.VerifyAll();
        }
        [Fact]
        public async Task GetTaskHis_StateUnderTest_GetAllTasksNotFound()
        {
            // Arrange

            mockTaskService.Setup(s => s.GetTasks(It.IsAny<string>())).ThrowsAsync(new ArgumentException());
            var taskHistoriesController = this.CreateTaskHistoriesController();

            // Act
            var result = await taskHistoriesController.GetTaskHis("");

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
            this.mockRepository.VerifyAll();
        }
        [Fact]
        public async Task GetTaskHis_StateUnderTest_GetAllTasksBystrFind()
        {
            // Arrange
            // var tasks = _fixture.Build<TaskHistory>().Without(f => f.Book).Without(f => f.Status).CreateMany();
            var taskDtos = _fixture.CreateMany<TaskDto>();
            string strFind = taskDtos.FirstOrDefault().Id.ToString();

            mockTaskService.Setup(s => s.GetTasks(strFind)).ReturnsAsync(taskDtos);
            var taskHistoriesController = this.CreateTaskHistoriesController();

            // Act
            var result = await taskHistoriesController.GetTaskHis(strFind);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(JsonConvert.SerializeObject(taskDtos), JsonConvert.SerializeObject(((ObjectResult)result.Result).Value));
            this.mockRepository.VerifyAll();
        }
        [Fact]
        public async Task GetTaskHis_StateUnderTest_GetAllTasksBystrFindNotFound()
        {
            // Arrange

            string strFind = _fixture.Create<string>();

            mockTaskService.Setup(s => s.GetTasks(strFind)).ThrowsAsync(new ArgumentException());
            var taskHistoriesController = this.CreateTaskHistoriesController();

            // Act
            var result = await taskHistoriesController.GetTaskHis(strFind);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
            //var okObjectResult = result.Result as OkObjectResult;
            //var actual = okObjectResult.Value as IEnumerable<TaskHistory>;
            //Assert.All<TaskHistory>(actual, item => Assert.Contains(strFind, item.requested));
            this.mockRepository.VerifyAll();
        }
    }
}
