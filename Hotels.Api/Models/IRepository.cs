using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotels.Api.Models
{
    public interface IRepository<T> where T : Entity
    {
        T GetById(int id);
        IQueryable<T> GetAll();
        void Create(T entity);
        void Update(T entity);
        void DeleteById(int id);
    }
}
