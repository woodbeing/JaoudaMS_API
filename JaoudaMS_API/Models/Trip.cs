using System;
using System.Collections.Generic;

namespace JaoudaMS_API.Models;

public partial class Trip
{
    public string Id { get; set; } = null!;

    public string? Truck { get; set; }

    public string? Driver { get; set; }

    public string? Seller { get; set; }

    public string? Helper { get; set; }

    public string? Zone { get; set; }

    public DateTime? Date { get; set; }

    public decimal? Charges { get; set; }

    public bool? IsActive { get; set; }

    public virtual Employee? DriverNavigation { get; set; }

    public virtual Employee? HelperNavigation { get; set; }

    public virtual Employee? SellerNavigation { get; set; }

    public virtual ICollection<TripInfo> TripInfos { get; set; } = new List<TripInfo>();

    public virtual ICollection<TripWaste> TripWastes { get; set; } = new List<TripWaste>();

    public virtual Truck? TruckNavigation { get; set; }
}
