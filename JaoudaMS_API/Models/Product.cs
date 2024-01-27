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

    public int? Stock { get; set; }

    public virtual ICollection<PurchaseProduct> PurchaseProducts { get; set; } = new List<PurchaseProduct>();

    public virtual ICollection<TripProduct> TripProducts { get; set; } = new List<TripProduct>();

    public virtual ICollection<Waste> Wastes { get; set; } = new List<Waste>();
}
