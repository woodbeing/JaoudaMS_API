using System;
using System.Collections.Generic;

namespace JaoudaMS_API.Models;

public partial class Purchase
{
    public string Id { get; set; } = null!;

    public string? Supplier { get; set; }

    public DateTime? Date { get; set; }

    public virtual ICollection<PurchaseInfo> PurchaseInfos { get; set; } = new List<PurchaseInfo>();

    public virtual Supplier? SupplierNavigation { get; set; }
}
