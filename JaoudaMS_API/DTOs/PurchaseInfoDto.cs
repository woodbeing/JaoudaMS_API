namespace JaoudaMS_API.DTOs
{
    public class PurchaseInfoDto
    {
        public string Product { get; set; } = null!;

        public string Box { get; set; } = null!;

        public int? Qtt { get; set; }

        public decimal? Price { get; set; }
    }
}
