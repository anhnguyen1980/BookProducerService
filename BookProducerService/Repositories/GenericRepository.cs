using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Reflection;
using BookProducerService.Repositories.Interfaces;
using BookProducerService.Infrastructure.Database;

namespace BookProducerService.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> entities;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;// new ApplicationDbContext();
            entities = this._context.Set<T>();
        }
        public async Task<IEnumerable<T>> GetAll()
        {
            return await entities.ToListAsync();
        }
        public async Task<T> Get(object primaryKey)
        {

            return await entities.FindAsync(primaryKey);
        }
        public async Task<bool> Insert(T entity)
        {
            if (entity == null)
                return false;

            //try
            //{
            await entities.AddAsync(entity);
            return true;
            //}
            //catch (Exception)
            //{

            //    return false;
            //}

        }

        public bool Update(T entity)
        {
            try
            {
                entities.Update(entity);
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }

        public bool Delete(T entity)
        {
            if (entity == null)
            {
                return false;
            }
            entities.Remove(entity);
            return true;
        }
    }
}
