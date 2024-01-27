namespace JaoudaMS_API.DTOs;

public class PurchaseProductDto
{
    public string Purchase { get; set; } = null!;

    public string Product { get; set; } = null!;

    public short? Qtt { get; set; }

    public decimal? Price { get; set; }
}
