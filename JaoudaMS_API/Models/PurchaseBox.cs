using System;
using System.Collections.Generic;

namespace JaoudaMS_API.Models;

public partial class PurchaseBox
{
    public string Purchase { get; set; } = null!;

    public string Box { get; set; } = null!;

    public short? QttIn { get; set; }

    public short? QttSent { get; set; }

    public virtual Box BoxNavigation { get; set; } = null!;

    public virtual Purchase PurchaseNavigation { get; set; } = null!;
}
