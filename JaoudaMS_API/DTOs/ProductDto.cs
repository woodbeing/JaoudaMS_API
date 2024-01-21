namespace JaoudaMS_API.DTOs
{
    public class ProductDto
    {
        public string Id { get; set; } = null!;

        public string? Designation { get; set; }

        public string? Genre { get; set; }

        public decimal? Price { get; set; }

        public decimal? CommissionDriver { get; set; }

        public decimal? CommissionSeller { get; set; }
    }
}
