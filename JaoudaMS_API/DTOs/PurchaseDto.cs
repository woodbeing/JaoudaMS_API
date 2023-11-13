namespace JaoudaMS_API.DTOs
{
    public class PurchaseDto
    {
        public string Id { get; set; } = null!;

        public string? Supplier { get; set; }

        public DateTime? Date { get; set; }

        public virtual ICollection<PurchaseInfoDto> PurchaseInfos { get; set; } = new List<PurchaseInfoDto>();
    }
}
