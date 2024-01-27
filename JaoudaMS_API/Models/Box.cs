using System;
using System.Collections.Generic;

namespace JaoudaMS_API.Models;

public partial class Box
{
    public string Id { get; set; } = null!;

    public string? Designation { get; set; }

    public string? Type { get; set; }

    public short? InStock { get; set; }

    public short? Empty { get; set; }

    public short? Sent { get; set; }

    public virtual ICollection<PurchaseBox> PurchaseBoxes { get; set; } = new List<PurchaseBox>();

    public virtual ICollection<TripBox> TripBoxes { get; set; } = new List<TripBox>();
}
