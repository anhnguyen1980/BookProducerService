using AutoFixture;
using BookProducer.Core.Entities;
using BookProducerService.Infrastructure.Database;
using BookProducerService.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookProducerService.UnitTests.Repositories.TestFixtures
{
    public class GenericRepositoriesFixture
    {
        private readonly ApplicationDbContext _inMemoryDBContext;
        private readonly GenericRepository<Book> _repositories;
        private readonly UnitOfWork _unitOfWork;
        private readonly Fixture _fixture;
        public GenericRepository<Book> Repositories
        {
            get { return _repositories; }
        }

        public UnitOfWork UnitOfWork
        {
            get { return _unitOfWork; }
        }
        public ApplicationDbContext InMemoryDBContext
        {
            get { return _inMemoryDBContext; }
        }
        public GenericRepositoriesFixture()
        {
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                            .UseInMemoryDatabase(databaseName: "BookDBMemory")
                            .Options;
            _inMemoryDBContext = new ApplicationDbContext(dbContextOptions);
            _repositories = new GenericRepository<Book>(_inMemoryDBContext);
            _unitOfWork = new UnitOfWork(_inMemoryDBContext);
            _fixture = new Fixture();
            AddValueToRepo();
        }
        private async void AddValueToRepo()
        {
            // List<Book> books = _fixture.CreateMany<Book>().ToList();
            var books = _fixture.Build<Book>().Without(f => f.BookGenre).Without(f => f.Author).Without(f => f.TaskHistory).CreateMany();
            await _inMemoryDBContext.AddRangeAsync(books);

            await _unitOfWork.CommitAsync();

            foreach (var item in books)
            {
                _inMemoryDBContext.Entry(item).State = EntityState.Detached;
            }

        }
    }
}
