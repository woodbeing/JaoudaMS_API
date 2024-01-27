namespace JaoudaMS_API.DTOs;

public class TripBoxDto
{
    public string Trip { get; set; } = null!;

    public string Box { get; set; } = null!;

    public short? QttOut { get; set; }

    public short? QttIn { get; set; }
}
