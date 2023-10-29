using System;
using System.Collections.Generic;

namespace JaoudaMS_API.Models;

public partial class Waste
{
    public string Product { get; set; } = null!;

    public string Type { get; set; } = null!;

    public int? Qtt { get; set; }

    public virtual Product ProductNavigation { get; set; } = null!;

    public virtual ICollection<TripWaste> TripWastes { get; set; } = new List<TripWaste>();
}
