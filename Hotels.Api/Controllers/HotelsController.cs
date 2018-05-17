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
        public IActionResult Add(Hotel hotel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                SetHotelRegion(hotel);
                hotelRepository.Create(hotel);

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                var hotel = hotelRepository.GetById(id);

                if (hotel == null)
                    return NotFound();

                hotelRepository.DeleteById(id);

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var hotels = new List<Hotel>();

            try
            {
                hotels = hotelRepository.GetAll().ToList();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

            hotels.ForEach(h => h.Region.Hotels = null);

            return Ok(hotels);
        }

        [HttpGet("{id}")]
        public IActionResult GetHotel(int id)
        {
            try
            {
                var hotel = hotelRepository.GetById(id);

                if (hotel == null)
                    return NotFound();

                return Ok(hotel);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut("scandic")]
        public IActionResult UpdateScandicHotelsFromFile()
        {
            var parser = new FileParser(hotelRepository);

            try
            {
                var hotelVacancies = parser.GetLatestScandicVacanciesFromFile();

                foreach (var hotelVacancy in hotelVacancies)
                    hotelRepository.Update(hotelVacancy);

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, "Failed getting latest vacancies. " + e.Message);
            }
        }

        [HttpPut("bestwestern")]
        public IActionResult UpdateBestWesternHotelsFromFile()
        {
            var parser = new FileParser(hotelRepository);

            try
            {
                var hotelVacancies = parser.GetLatestBestWesternVacanciesFromFile();

                foreach (var hotelVacancy in hotelVacancies)
                    hotelRepository.Update(hotelVacancy);

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, "Failed getting latest vacancies. " + e.Message);
            }
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
                        Name = "Tidbloms Hotel",
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

        // TODO: Repository
        private void SetHotelRegion(Hotel hotel)
        {
            var region = context.Regions.SingleOrDefault(r => r.RegionCode == hotel.RegionId);

            if (region != null)
                hotel.Region = region;
        }
    }
}
