using Microsoft.EntityFrameworkCore;

namespace Hotels.Api.Models
{
    public class HotelsContext : DbContext
    {
        public DbSet<Region> Regions { get; set; }
        public DbSet<Hotel> Hotels { get; set; }

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
