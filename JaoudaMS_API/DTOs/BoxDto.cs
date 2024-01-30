namespace JaoudaMS_API.DTOs;

public class BoxDto
{
    public string Id { get; set; } = null!;

    public string? Designation { get; set; }

    public string? Type { get; set; }

    public short? InStock { get; set; }

    public short? Empty { get; set; }
}
