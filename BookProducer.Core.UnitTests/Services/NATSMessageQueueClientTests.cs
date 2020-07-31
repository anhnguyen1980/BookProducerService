using AutoFixture;
using BookProducer.Core.Services;
using Microsoft.Azure.ServiceBus;
using Moq;
using NATS.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BookProducer.Core.UnitTests
{
    public class NATSMessageQueueClientTests
    {
        //private readonly MockRepository mockRepository;
        //private Fixture fixture;
        //public NATSMessageQueueClientTests()
        //{
        //    fixture = new Fixture();
        //    mockRepository = new MockRepository(MockBehavior.Strict);
        //}
        //[Fact]
        //private async Task SendMessageAsync_StateUnderTest_ExpectedBehavior()
        //{ //Can't mock ConnectionFactory because it is sealed class
        //    //Arrage
        //    var cf = new Mock<ConnectionFactory>();
        // //   Mock<ConnectionFactory> cf = mockRepository.Create<ConnectionFactory>();
        //    string connectString = fixture.Create<string>();
        //    string queueName = fixture.Create<string>();
        //    Mock<IConnection> client = mockRepository.Create<IConnection>();
        //    cf.Setup(x => x.CreateConnection(connectString)).Returns(client.Object);//.Verifiable();
        //    client.Setup(x => x.Publish(It.IsAny<string>(), It.IsAny<Byte[]>()));
           
        //    var service = new NATSMessageQueueClient(connectString, queueName);
        //    //Act
        //    var result = await service.SendMessageAsync(It.IsAny<Type>());
        //    //Assert
        //    Assert.True(result);
        //    mockRepository.VerifyAll();
        //}

    }
}
