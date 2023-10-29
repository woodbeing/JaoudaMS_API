using System;
using System.Collections.Generic;

namespace JaoudaMS_API.Models;

public partial class Box
{
    public string Type { get; set; } = null!;

    public string? SubBox { get; set; }

    public virtual ICollection<InBox> InBoxes { get; set; } = new List<InBox>();

    public virtual ICollection<Box> InverseSubBoxNavigation { get; set; } = new List<Box>();

    public virtual ICollection<PurchaseInfo> PurchaseInfos { get; set; } = new List<PurchaseInfo>();

    public virtual Box? SubBoxNavigation { get; set; }
}
