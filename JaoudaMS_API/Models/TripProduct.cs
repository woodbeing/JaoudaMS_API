using System;
using System.Collections.Generic;

namespace JaoudaMS_API.Models;

public partial class TripProduct
{
    public string Trip { get; set; } = null!;

    public string Product { get; set; } = null!;

    public short? QttOut { get; set; }

    public short? QttSold { get; set; }

    public decimal? Price { get; set; }

    public virtual Product ProductNavigation { get; set; } = null!;

    public virtual Trip TripNavigation { get; set; } = null!;
}
