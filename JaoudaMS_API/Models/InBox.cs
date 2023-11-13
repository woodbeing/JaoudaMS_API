using System;
using System.Collections.Generic;

namespace JaoudaMS_API.Models;

public partial class InBox
{
    public string Product { get; set; } = null!;

    public string Box { get; set; } = null!;

    public int? Capacity { get; set; }

    public byte? Coefficient { get; set; }

    public int? InStock { get; set; }

    public int? Empty { get; set; }

    public virtual Box BoxNavigation { get; set; } = null!;

    public virtual Product ProductNavigation { get; set; } = null!;
}
