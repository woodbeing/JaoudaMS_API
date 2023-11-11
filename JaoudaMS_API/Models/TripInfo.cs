using System;
using System.Collections.Generic;

namespace JaoudaMS_API.Models;

public partial class TripInfo
{
    public string Trip { get; set; } = null!;

    public string Product { get; set; } = null!;

    public string Box { get; set; } = null!;

    public int? QttOut { get; set; }

    public int? QttBack { get; set; }

    public decimal? Price { get; set; }

    public int? QttUnitSold { get; set; }

    public int? QttUnitIn { get; set; }

    public virtual Box BoxNavigation { get; set; } = null!;

    public virtual Product ProductNavigation { get; set; } = null!;

    public virtual Trip TripNavigation { get; set; } = null!;
}
