using System;
using Xunit;
using AutoFixture;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Net.Http.Headers;
using System.Text;
using System.Net;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BookProducerService;
using BookProducer.Core.Entities;
using BookProducerService.APITest.Fixtures;
using BookProducer.APITest.Orders;

namespace BookProducer.APITest.Controllers
{
    [TestCaseOrderer("BookStoreAPITest.TestCaseOrderer.PriorityOrderer", "BookStoreAPITest")]
    public class BooksControllerTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly Fixture _fixture;
        private readonly HttpClient _client;
        public BooksControllerTest(CustomWebApplicationFactory<Startup> webApplicationFactory)
        {
            _client = webApplicationFactory.CreateClient();
            _fixture = new Fixture();
        }

        [Fact, TestPriority(0)]
        public async Task GetBooks_StateUnderTest_GetAllBooks()
        {
            // Arrange
            var url = "api/Books";
            var httpResponse = await _client.GetAsync(url);//.ConfigureAwait(false);
            // MUST be successful.
            httpResponse.EnsureSuccessStatusCode();
            // Act

            var stringReponse = await httpResponse.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ICollection<Book>>(stringReponse);
            //  result = result.OrderBy(i => i.Id).ToList();
            // Assert
            var expectResult = new Helpers.StoredDataHelper().GetBooks();

            Assert.Equal(JsonConvert.SerializeObject(expectResult), JsonConvert.SerializeObject(result));
            Assert.Equal(System.Net.HttpStatusCode.OK, httpResponse.StatusCode);
            Assert.True(result.Count > 0, $"The actual Books Count had {result.Count} elements");
        }

        [Fact, TestPriority(3)]
        public async Task DeleteBook_StateUnderTest_ExpectedBehaviorAsync()
        {
            // Arrange
            var book = new BookProducer.APITest.Helpers.StoredDataHelper().GetBookFirst();
            if (book == null)
                book = _fixture.Create<Book>();

            var url = $"api/Books/{book.Id}";
            //Act
            var response = await _client.DeleteAsync(url);
            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact, TestPriority(1)]
        public async Task PostBook_StateUnderTest_ExpectedBehaviorAsync()
        {
            //// Arrange
            var newBook = _fixture.Build<Book>().Without(f => f.TaskHistory).Without(f => f.Author).Without(f => f.BookGenre).Create();
            var url = "api/Books";
            //Act
            var response = await _client.PostAsync(url, new StringContent(
                    JsonConvert.SerializeObject(newBook), UnicodeEncoding.UTF8, "application/json"));
            //var value = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact, TestPriority(2)]
        public async Task PutBook_StateUnderTest_ExpectedBehaviorAsync()
        {
            var book = new BookProducer.APITest.Helpers.StoredDataHelper().GetBookFirst();
            if (book == null)
                book = _fixture.Create<Book>();
            book.Title = "Title put order";
            book.Description = "Description put order";
            var url = $"api/Books/{book.Id}";
            //Act
            var response = await _client.PutAsync(url, new StringContent(
                    JsonConvert.SerializeObject(book), UnicodeEncoding.UTF8, "application/json"));
            //var value = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
