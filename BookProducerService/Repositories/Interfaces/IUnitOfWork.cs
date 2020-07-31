using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BookProducerService.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
        Task CommitAsync();

        
    }
}
