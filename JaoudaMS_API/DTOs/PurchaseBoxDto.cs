namespace JaoudaMS_API.DTOs;

public class PurchaseBoxDto
{
    public string Purchase { get; set; } = null!;

    public string Box { get; set; } = null!;

    public short? QttIn { get; set; }

    public short? QttSent { get; set; }
}
