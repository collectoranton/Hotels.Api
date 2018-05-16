using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Hotels.Api.Models
{
    public class RegionRepository : IRepository<Region>
    {
        private readonly HotelsContext context;

        public RegionRepository(HotelsContext context)
        {
            this.context = context;
        }

        public Region GetById(int id)
        {
            return context.Regions.SingleOrDefault(r => r.Id == id);
        }

        public IQueryable<Region> GetAll()
        {
            return context.Regions.Include(r => r.Hotels);
        }

        public void Create(Region region)
        {
            context.Regions.Add(region);
            context.SaveChanges();
        }

        public void Update(Region region)
        {
            context.Regions.Update(region);
            context.SaveChanges();
        }

        public void DeleteById(int id)
        {
            var region = context.Regions.SingleOrDefault(r => r.Id == id);
            if (region != null) context.Regions.Remove(region);
            context.SaveChanges();
        }
    }
}
