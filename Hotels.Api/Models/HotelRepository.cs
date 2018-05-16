using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Hotels.Api.Models
{
    public class HotelRepository : IRepository<Hotel>
    {
        private readonly DbContext context;

        public HotelRepository(DbContext context)
        {
            this.context = context;
        }

        public Hotel GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<Hotel> GetAll()
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
