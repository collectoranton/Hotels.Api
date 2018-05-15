using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hotels.Api.Models
{
    public class Region : Entity
    {
        [Required]
        public int? RegionCode { get; set; }
        [Required]
        public string Name { get; set; }
        public List<Hotel> Hotels { get; set; }
    }
}