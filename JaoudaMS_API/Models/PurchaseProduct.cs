using System;
using System.Collections.Generic;

namespace JaoudaMS_API.Models;

public partial class PurchaseProduct
{
    public string Purchase { get; set; } = null!;

    public string Product { get; set; } = null!;

    public short? Qtt { get; set; }

    public decimal? Price { get; set; }

    public virtual Product ProductNavigation { get; set; } = null!;

    public virtual Purchase PurchaseNavigation { get; set; } = null!;
}
