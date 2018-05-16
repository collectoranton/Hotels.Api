using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Hotels.Api.Models
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        private readonly DbContext context;

        private DbSet<T> dbSet => context.Set<T>();
        public IQueryable<T> Entities => dbSet;
        
        public Repository(DbContext context)
        {
            this.context = context;
        }

        public T GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Create(T entity)
        {
            throw new NotImplementedException();
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
