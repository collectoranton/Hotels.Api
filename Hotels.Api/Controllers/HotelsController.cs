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
        private readonly HotelRepository hotelRepository;

        public HotelsController(HotelsContext context, HotelRepository hotelRepository)
        {
            this.context = context;
            this.hotelRepository = hotelRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Add(Hotel hotel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            SetHotelRegion(hotel);

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

        [HttpPut]
        public IActionResult UpdateScandicHotelsFromFile()
        {
            var parser = new FileParser(hotelRepository);

            var success = parser.UpdateScandicHotelsFromFile();

            if (success)
                return Ok();

            return NotFound();
        }

        // TODO: Delete
        [HttpGet("hello")]
        public IActionResult GetHello()
        {
            return Ok("Hello there, delete me");
        }

        [HttpDelete("reseed")]
        public IActionResult Reseed()
        {
            try
            {
                var list = new List<Hotel>
                {
                    new Hotel
                    {
                        RegionId = 50,
                        Name = "Scandic Rubinen",
                        Vacancies = 15
                    },
                    new Hotel
                    {
                        RegionId = 50,
                        Name = "Scandic Opalen",
                        Vacancies = 20
                    },
                    new Hotel
                    {
                        RegionId = 60,
                        Name = "Scandic Backadal",
                        Vacancies = 5
                    },
                    new Hotel
                    {
                        RegionId = 70,
                        Name = "Scandic Helsingborg North",
                        Vacancies = 2
                    },
                    new Hotel
                    {
                        RegionId = 50,
                        Name = "Hotell Eggers",
                        Vacancies = 100
                    },
                    new Hotel
                    {
                        RegionId = 50,
                        Name = "Tidaholms Hotel",
                        Vacancies = 200
                    },
                    new Hotel
                    {
                        RegionId = 70,
                        Name = "Hotell Duxiana",
                        Vacancies = 5
                    }
                };

                list.ForEach(SetHotelRegion);

                context.AddRange(list);
                context.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        private void SetHotelRegion(Hotel hotel)
        {
            var region = context.Regions.SingleOrDefault(r => r.RegionCode == hotel.RegionId);

            if (region != null)
                hotel.Region = region;
        }
    }
}
