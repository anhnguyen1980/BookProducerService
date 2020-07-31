using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookProducerService.Services.Interfaces
{
    public interface IBaseService<T> where T : class
    {
        T Get(object id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
