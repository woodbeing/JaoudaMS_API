﻿namespace JaoudaMS_API.DTOs;

public class TripDto
{
    public string Id { get; set; } = null!;

    public string? Truck { get; set; }

    public string? Driver { get; set; }

    public string? Seller { get; set; }

    public string? Helper { get; set; }

    public string? Zone { get; set; }

    public DateTime? Date { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<TripBoxDto> TripBoxes { get; set; } = new List<TripBoxDto>();

    public virtual ICollection<TripChargeDto> TripCharges { get; set; } = new List<TripChargeDto>();

    public virtual ICollection<TripProductDto> TripProducts { get; set; } = new List<TripProductDto>();

    public virtual ICollection<TripWasteDto> TripWastes { get; set; } = new List<TripWasteDto>();
}
