using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hotels.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotels.Api.Controllers
{
    [Route("api/hotels")]
    public class HotelsController : Controller
    {
        private readonly HotelsContext context;

        public HotelsController(HotelsContext context)
        {
            this.context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Add(Hotel hotel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var region = context.Regions.SingleOrDefault(r => r.RegionCode == hotel.RegionId);

            context.AddRange();

            if (region != null)
                hotel.Region = region;

            context.Add(hotel);
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
            var hotels = await context.Hotels
                .Include(h => h.Region)
                .ToListAsync();

            hotels.ForEach(h => h.Region.Hotels = null);

            return Ok(hotels);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetHotel(int id)
        {
            try
            {
                var hotel = await context.Hotels.SingleOrDefaultAsync(h => h.Id == id);

                if (hotel == null)
                    return NotFound();

                return Ok(hotel);
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
    }
}
