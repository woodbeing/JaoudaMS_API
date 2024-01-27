namespace JaoudaMS_API.DTOs;

public class TripProductDto
{
    public string Trip { get; set; } = null!;

    public string Product { get; set; } = null!;

    public short? QttOut { get; set; }

    public short? QttSold { get; set; }

    public decimal? Price { get; set; }
}
