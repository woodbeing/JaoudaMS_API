using System;
using System.Collections.Generic;

namespace JaoudaMS_API.Models;

public partial class PurchaseWaste
{
    public string Purchase { get; set; } = null!;

    public string Product { get; set; } = null!;

    public string Type { get; set; } = null!;

    public short? Qtt { get; set; }

    public virtual Purchase PurchaseNavigation { get; set; } = null!;

    public virtual Waste Waste { get; set; } = null!;
}
