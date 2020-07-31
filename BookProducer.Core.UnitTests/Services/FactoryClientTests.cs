using AutoFixture;
using BookProducer.Core.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
namespace BookProducer.Core.UnitTests
{
    public class FactoryClientTests
    {
        private readonly MockRepository _mockRepository;
        private readonly Fixture _fixture;

        public FactoryClientTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _fixture = new Fixture();
        }
        [Theory]
        [InlineData("RabbitMQ","RabbitMQ")]
        [InlineData("NATS","NATS")]
        [InlineData("AzureServiceBus","Azure")]
        public void CreateMessageQueue_StateUnderTest_ExpectedBehavior(string type, string typeName)
        {
            //Arrange
            Mock<IConfiguration> config = _mockRepository.Create<IConfiguration>();
            string connectString = _fixture.Create<string>();
            string queueName = _fixture.Create<string>();
            config.Setup(x => x[$"{type}:ConnectionString"]).Returns(connectString);
            config.Setup(x => x[$"{type}:QueueName"]).Returns(queueName);
            var factory = new FactoryClient(config.Object);
            //Act
            var result = factory.CreateMessageQueue(type);
            //Assert
            Assert.StartsWith(typeName, result.GetType().Name);
            this._mockRepository.VerifyAll();
        }
    }
}
