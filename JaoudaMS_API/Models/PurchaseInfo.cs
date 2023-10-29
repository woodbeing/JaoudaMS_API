﻿using System;
using System.Collections.Generic;

namespace JaoudaMS_API.Models;

public partial class PurchaseInfo
{
    public string Purchase { get; set; } = null!;

    public string Product { get; set; } = null!;

    public string Box { get; set; } = null!;

    public int? Qtt { get; set; }

    public decimal? Price { get; set; }

    public virtual Box BoxNavigation { get; set; } = null!;

    public virtual Product ProductNavigation { get; set; } = null!;

    public virtual Purchase PurchaseNavigation { get; set; } = null!;
}
