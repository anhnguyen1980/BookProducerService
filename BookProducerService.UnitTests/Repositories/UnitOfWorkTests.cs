using AutoFixture;
using BookProducerService.Infrastructure.Database;
using BookProducerService.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace BookProducerService.UnitTests.Repositories
{
    public class UnitOfWorkTests
    {
        private readonly ApplicationDbContext _inMemoryDBContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly MockRepository mockRepository;
        public UnitOfWorkTests()
        {
            mockRepository = new MockRepository(MockBehavior.Strict);
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                           .UseInMemoryDatabase(databaseName: "BookDBMemory")
                           .Options;
            _inMemoryDBContext = new ApplicationDbContext(dbContextOptions);
            _unitOfWork = new UnitOfWork(_inMemoryDBContext);
        }

        [Fact]
        public void Commit_StateUnderTest_ExpectedBehavior()
        {
            // Arrange

            // Act
            _unitOfWork.Commit();

            // Assert
            Assert.True(true);
            mockRepository.VerifyAll();
        }

        [Fact]
        public async Task CommitAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange

            // Act
            await _unitOfWork.CommitAsync();
            // Assert
            Assert.True(true);
            mockRepository.VerifyAll();
        }
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Dispose_StateUnderTest_ExpectedBehavior(bool disposed)
        {
            // Arrange
            if (!disposed)
                _inMemoryDBContext.Dispose();
            // Act
            _unitOfWork.Dispose();
            _unitOfWork.Dispose();

            // Assert
            Assert.True(true);
        }
    }
}
