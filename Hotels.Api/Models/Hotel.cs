namespace Hotels.Api.Models
{
    public class Hotel : Entity
    {
        public string Name { get; set; }
        public Region Region { get; set; }
        public int RegionId { get; set; }
        public int Vacancies { get; set; }
    }
}