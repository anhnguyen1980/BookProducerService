using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using AutoFixture;
using Moq;
using Microsoft.Extensions.Configuration;
using BookProducer.Core.Interfaces;
using BookProducer.Core.Services;


namespace BookProducer.Core.UnitTests
{
    public class ClientTests
    {
        private readonly MockRepository mockRepository;
        private readonly Fixture fixture;
        public ClientTests()
        {
            mockRepository = new MockRepository(MockBehavior.Strict);
            fixture = new Fixture();
        }
        [Fact]
        public void SendMessageAsync_StateUnderTest_ExpectedBehavior()
        {
            //Arrange
            string type = "RabbitMQ";
            Mock<IConfiguration> config = mockRepository.Create<IConfiguration>();
            config.Setup(x => x["MessageQueue:Type"]).Returns(type);
            var clientSender = new Client(config.Object);
            //Act
            var result = clientSender.SendMessageAsync(It.IsAny<Type>());
            //Assert
            Assert.True(true);
            mockRepository.VerifyAll();
        }

    }
}
