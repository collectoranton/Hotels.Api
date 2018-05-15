using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotels.Api.Controllers
{
    [Route("api/regions")]
    public class RegionsController : Controller
    {
        private readonly HotelsContext context;

        public RegionsController(HotelsContext context)
        {
            this.context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Add(Region region)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            context.Add(region);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var region = await context.Regions.SingleOrDefaultAsync(r => r.Id == id);

                if (region == null)
                    return NotFound();

                context.Regions.Remove(region);
                await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var regions = await context.Regions
                .Include(r => r.Hotels)
                .ToListAsync();

            regions.ForEach(r => r.Hotels.ForEach(h => h.Region = null));

            return Ok(regions);
        }

        [HttpGet("{regionCode}")]
        public async Task<IActionResult> GetRegionFromRegionCode(int regionCode)
        {
            try
            {
                var region = await context.Regions.SingleOrDefaultAsync(r => r.RegionCode == regionCode);

                if (region == null)
                    return NotFound();

                return Ok(region);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        // TODO: Delete
        [HttpGet("hello")]
        public IActionResult GetHello()
        {
            return Ok("Hello there, delete me");
        }

        [HttpDelete("reseed")]
        public  IActionResult Reseed()
        {
            try
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                
                context.Regions.AddRange(
                    new Region
                    {
                        Name = "Göteborg Centrum",
                        RegionCode = 50
                    },
                    new Region
                    {
                        Name = "Göteborg Hisingen",
                        RegionCode = 60
                    },
                    new Region
                    {
                        Name = "Helsingborg",
                        RegionCode = 70
                    });

                context.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
