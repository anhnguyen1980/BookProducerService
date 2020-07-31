using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookProducerService.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
       Task< IEnumerable<T>> GetAll();
        Task<T> Get(object id);
       Task< bool> Insert(T entity);
        bool Update(T entity);
        bool Delete(T entity);
    }
}
