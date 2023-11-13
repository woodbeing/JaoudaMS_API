using System;
using System.Collections.Generic;

namespace JaoudaMS_API.Models;

public partial class TripCharge
{
    public string Trip { get; set; } = null!;

    public string Type { get; set; } = null!;

    public decimal? Amount { get; set; }

    public virtual Trip TripNavigation { get; set; } = null!;
}
