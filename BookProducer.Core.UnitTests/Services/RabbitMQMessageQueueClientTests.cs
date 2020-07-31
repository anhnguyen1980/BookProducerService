using AutoFixture;
using BookProducer.Core.Services;
using Microsoft.Azure.ServiceBus;
using Moq;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BookProducer.Core.UnitTests
{
    public class RabbitMQMessageQueueClientTests
    {
        //private readonly MockRepository mockRepository;
        //private Fixture fixture;
        //public RabbitMQMessageQueueClientTests()
        //{
        //    fixture = new Fixture();
            
        //    mockRepository = new MockRepository(MockBehavior.Strict);
        //}
        //[Fact]
        //private async Task SendMessageAsync_StateUnderTest_ExpectedBehavior()
        //{
        //    //Arrage
        //    string connectString = fixture.Create<string>();
        //    string queueName = fixture.Create<string>();

        //    var mockConFactory = this.mockRepository.Create<ConnectionFactory>();
        //    Mock<IConnection> mockConnect = this.mockRepository.Create<IConnection>();
        //    mockConFactory.Setup(x =>
        //    x.CreateConnection(It.IsAny<string>())).Returns(mockConnect.Object);

        //    Mock<IModel> mockChanel = this.mockRepository.Create<IModel>();
        //    mockConnect.Setup(x => x.CreateModel()).Returns(mockChanel.Object);
           
        //    mockChanel.Setup(x => x.QueueDeclare(queueName, false, false, false, null));
        //    mockChanel.Setup(x => x.BasicPublish("", queueName, null, It.IsAny<byte[]>()));

        //    var service = new RabbitMQMessageQueueClient(connectString, queueName);
        //    //Act
        //    var result = await service.SendMessageAsync(It.IsAny<Type>());
        //    //Assert
        //    Assert.True(result);
        //    mockRepository.VerifyAll();
        //}

    }
}
