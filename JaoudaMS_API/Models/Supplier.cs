using System;
using System.Collections.Generic;

namespace JaoudaMS_API.Models;

public partial class Supplier
{
    public string Id { get; set; } = null!;

    public string? Address { get; set; }

    public string? Tel { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
}
