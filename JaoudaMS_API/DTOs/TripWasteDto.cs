﻿namespace JaoudaMS_API.DTOs;

public class TripWasteDto
{
    public string Trip { get; set; } = null!;

    public string Product { get; set; } = null!;

    public string Type { get; set; } = null!;

    public short? Qtt { get; set; }
}
