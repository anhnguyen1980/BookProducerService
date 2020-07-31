using AutoFixture;
using BookProducer.Core.Services;
using Microsoft.Azure.ServiceBus;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BookProducer.Core.UnitTests
{
    public class AzureMessageQueueClientTests
    {
      //  private readonly MockRepository mockRepository;
     //   private readonly Fixture fixture;
        public AzureMessageQueueClientTests()
        {
            //fixture = new Fixture();
         //   mockRepository = new MockRepository(MockBehavior.Strict);
        }
        //rem code because the corporate network forbids connecting to the Azure Message Queue 
        //[Fact]
        //private async Task SendMessageAsync_StateUnderTest_ExpectedBehavior()
        //{
        //    //Arrage
        //    var queueClientMock = new Mock<IQueueClient>();
        //    queueClientMock.Setup(x =>
        //    x.SendAsync(It.IsAny<Message>())).Returns(Task.CompletedTask);//.Verifiable();
        //    queueClientMock.Setup(x => x.CloseAsync()).Returns(Task.CompletedTask);
        //    string connectString = "Endpoint=sb://anhnguyen.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=qTKznJRadfRhedcnYAR6NC9CVM9DPEoMbcF9PSIy27Q=";
        //    string queueName = "myQueue";
        //    var service = new AzureMessageQueueClient(connectString, queueName);
        //    //Act
        //    var result = await service.SendMessageAsync(It.IsAny<Type>());
        //    //Assert
        //    Assert.True(result);
        //    mockRepository.VerifyAll();
        //}

    }
}
