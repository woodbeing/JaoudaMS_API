namespace JaoudaMS_API.DTOs
{
    public class TripDto
    {
        public string Id { get; set; } = null!;

        public string? Truck { get; set; }

        public string? Driver { get; set; }

        public string? Seller { get; set; }

        public string? Helper { get; set; }

        public string? Zone { get; set; }

        public DateTime? Date { get; set; }

        public decimal? Charges { get; set; }

        public bool? IsActive { get; set; }
    }
}
