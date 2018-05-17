using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Hotels.Api.Models
{
    public class HotelRepository : IRepository<Hotel>
    {
        private readonly HotelsContext context;

        public HotelRepository(HotelsContext context)
        {
            this.context = context;
        }

        public Hotel GetById(int id)
        {
            var hotel = context.Hotels.Include(h => h.Region).SingleOrDefault(h => h.Id == id);

            return hotel ?? throw new ArgumentException($"No hotel found with id: {id}", nameof(id));
        }

        public Hotel GetByName(string name)
        {
            var hotel = context.Hotels.Include(h => h.Region).SingleOrDefault(h => h.Name == name);

            return hotel ?? throw new ArgumentException($"No hotel found with name: {name}", nameof(name));
        }

        public IQueryable<Hotel> GetAll() => context.Hotels.Include(h => h.Region);

        public void Create(Hotel hotel)
        {
            if (hotel == null)
                throw new ArgumentNullException(nameof(hotel));

            context.Add(hotel);
            context.SaveChanges();
        }

        public void Update(Hotel hotel)
        {
            if (hotel == null)
                throw new ArgumentNullException(nameof(hotel));

            context.Update(hotel);
            context.SaveChanges();
        }

        public void DeleteById(int id)
        {
            context.Remove(GetById(id));
            context.SaveChanges();
        }
    }
}
