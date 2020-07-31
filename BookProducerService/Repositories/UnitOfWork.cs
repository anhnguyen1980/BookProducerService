using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

using System.Diagnostics.CodeAnalysis;
using BookProducerService.Repositories.Interfaces;
using BookProducerService.Infrastructure.Database;
using System.Collections.Generic;
using System;

namespace BookProducerService.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private Dictionary<Type, object> _repositories;
        private bool disposed = false;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }
       // [ExcludeFromCodeCoverage]
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }
       // [ExcludeFromCodeCoverage]
        public void Dispose()
        {
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }
        public IGenericRepository<TEntity> GetRepositoryAsync<TEntity>() where TEntity : class
        {
            if (_repositories == null) _repositories = new Dictionary<Type, object>();

            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type)) _repositories[type] = new GenericRepository<TEntity>(_context);
            return (IGenericRepository<TEntity>)_repositories[type];
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
