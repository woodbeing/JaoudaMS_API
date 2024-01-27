namespace JaoudaMS_API.DTOs;

public class PurchaseWasteDto
{
    public string Purchase { get; set; } = null!;

    public string Product { get; set; } = null!;

    public string Type { get; set; } = null!;

    public short? Qtt { get; set; }
}
