namespace JaoudaMS_API.DTOs;

public class PurchaseDto
{
    public string Id { get; set; } = null!;

    public string? Supplier { get; set; }

    public DateTime? Date { get; set; }

    public virtual ICollection<PurchaseBoxDto> PurchaseBoxes { get; set; } = new List<PurchaseBoxDto>();

    public virtual ICollection<PurchaseProductDto> PurchaseProducts { get; set; } = new List<PurchaseProductDto>();

    public virtual ICollection<PurchaseWasteDto> PurchaseWastes { get; set; } = new List<PurchaseWasteDto>();
}
