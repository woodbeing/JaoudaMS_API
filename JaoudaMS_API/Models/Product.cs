using System;
using System.Collections.Generic;

namespace JaoudaMS_API.Models;

public partial class Product
{
    public string Id { get; set; } = null!;

    public string? Designation { get; set; }

    public string? Genre { get; set; }

    public decimal? Price { get; set; }

    public decimal? CommissionDriver { get; set; }

    public decimal? CommissionSeller { get; set; }

    public virtual ICollection<InBox> InBoxes { get; set; } = new List<InBox>();

    public virtual ICollection<PurchaseInfo> PurchaseInfos { get; set; } = new List<PurchaseInfo>();

    public virtual ICollection<TripInfo> TripInfos { get; set; } = new List<TripInfo>();

    public virtual ICollection<Waste> Wastes { get; set; } = new List<Waste>();
}
