using System.ComponentModel.DataAnnotations;

namespace Hotels.Api.Controllers
{
    public class Region
    {
        public int Id { get; set; }
        [Required]
        public int? RegionCode { get; set; }
        [Required]
        public string Name { get; set; }
    }
}