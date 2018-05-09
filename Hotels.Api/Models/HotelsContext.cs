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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Om webappen skapar en SamuraiContext så kommer inte detta köras
                // Detta är default. Körs alltså när du använda Update-Database eller från EfSamurai.App-projektet

                optionsBuilder.UseSqlServer("Server = (localdb)\\mssqllocaldb; Database = Hotels; Trusted_Connection = True;");
            }

            optionsBuilder.EnableSensitiveDataLogging();
        }
    }
}
