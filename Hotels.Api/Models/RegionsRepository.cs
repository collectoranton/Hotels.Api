using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Hotels.Api.Models
{
    public class RegionsRepository : IRepository<Region>
    {
        private readonly DbContext context;

        public RegionsRepository(DbContext context)
        {
            this.context = context;
        }

        public Region GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<Region> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public void Create()
        {
            throw new System.NotImplementedException();
        }

        public void Update()
        {
            throw new System.NotImplementedException();
        }

        public void Delete()
        {
            throw new System.NotImplementedException();
        }
    }
}
