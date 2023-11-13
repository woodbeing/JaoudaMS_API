using System;
using System.Collections.Generic;

namespace JaoudaMS_API.Models;

public partial class Box
{
    public string Type { get; set; } = null!;

    public virtual ICollection<InBox> InBoxes { get; set; } = new List<InBox>();

    public virtual ICollection<PurchaseInfo> PurchaseInfos { get; set; } = new List<PurchaseInfo>();

    public virtual ICollection<TripInfo> TripInfos { get; set; } = new List<TripInfo>();
}
