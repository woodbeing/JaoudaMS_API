using System;
using System.Collections.Generic;

namespace JaoudaMS_API.Models;

public partial class TripWaste
{
    public string Trip { get; set; } = null!;

    public string Product { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string? Qtt { get; set; }

    public virtual Trip TripNavigation { get; set; } = null!;

    public virtual Waste Waste { get; set; } = null!;
}
