namespace Hotels.Api.Controllers
{
    public class Hotel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Region Region { get; set; }
        public int RegionId { get; set; }
        public int Vacancies { get; set; }
    }
}