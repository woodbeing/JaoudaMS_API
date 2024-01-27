using System;
using System.Collections.Generic;

namespace JaoudaMS_API.Models;

public partial class TripBox
{
    public string Trip { get; set; } = null!;

    public string Box { get; set; } = null!;

    public short? QttOut { get; set; }

    public short? QttIn { get; set; }

    public virtual Box BoxNavigation { get; set; } = null!;

    public virtual Trip TripNavigation { get; set; } = null!;
}
