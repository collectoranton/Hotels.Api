using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Hotels.Api.Controllers
{
    public class HotelsContext : DbContext
    {
        public DbSet<Region> Regions { get; set; }

        public HotelsContext(DbContextOptions<HotelsContext> context)
            : base (context)
        {
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    //optionsBuilder.UseSqlServer("Server = (localdb)\\mssqllocaldb; Database = Hotels; Trusted_Connection = True;");
            
        //    //optionsBuilder.EnableSensitiveDataLogging();
        //}
    }
}
