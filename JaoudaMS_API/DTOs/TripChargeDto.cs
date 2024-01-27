namespace JaoudaMS_API.DTOs;

public class TripChargeDto
{
    public string Trip { get; set; } = null!;

    public string Type { get; set; } = null!;

    public decimal? Amount { get; set; }
}
