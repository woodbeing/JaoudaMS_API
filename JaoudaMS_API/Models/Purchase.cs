using System;
using System.Collections.Generic;

namespace JaoudaMS_API.Models;

public partial class Purchase
{
    public string Id { get; set; } = null!;

    public string? Supplier { get; set; }

    public DateTime? Date { get; set; }

    public virtual ICollection<PurchaseBox> PurchaseBoxes { get; set; } = new List<PurchaseBox>();

    public virtual ICollection<PurchaseProduct> PurchaseProducts { get; set; } = new List<PurchaseProduct>();

    public virtual ICollection<PurchaseWaste> PurchaseWastes { get; set; } = new List<PurchaseWaste>();

    public virtual Supplier? SupplierNavigation { get; set; }
}
