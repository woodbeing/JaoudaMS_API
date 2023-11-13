namespace JaoudaMS_API.DTOs
{
    public class TripInfoDto
    {
        public string Product { get; set; } = null!;

        public string? Box { get; set; }

        public int? QttOut { get; set; }

        public int? QttBack { get; set; }

        public decimal? Price { get; set; }

        public int? QttUnitSold { get; set; }

        public int? QttUnitIn { get; set; }
    }
}
