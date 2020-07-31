using BookProducer.Core.Entities;
using BookProducerService.Infrastructure.Database;
using BookProducerService.Repositories;
using BookProducerService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookProducerService.UnitTests.Models.DomainModels
{
    public class TestHelper:IDisposable
    {
        public ApplicationDbContext applicationDbContext { get; set; }
        public TestHelper()
        {
            DbContextOptions<ApplicationDbContext> options;
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseInMemoryDatabase(databaseName: "BookDBMemory");
            options = builder.Options;
            applicationDbContext = new ApplicationDbContext(options);
            applicationDbContext.Database.EnsureDeleted();
            applicationDbContext.Database.EnsureCreated();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IGenericRepository<Book> GetInMemoryBookRepository()
        {
            return new GenericRepository<Book>(applicationDbContext);
        }

    }
}
