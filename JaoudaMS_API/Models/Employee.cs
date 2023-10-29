using System;
using System.Collections.Generic;

namespace JaoudaMS_API.Models;

public partial class Employee
{
    public string Cin { get; set; } = null!;

    public string? Type { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public string? Tel { get; set; }

    public decimal? Salary { get; set; }

    public decimal? Commission { get; set; }

    public virtual ICollection<Trip> TripDriverNavigations { get; set; } = new List<Trip>();

    public virtual ICollection<Trip> TripHelperNavigations { get; set; } = new List<Trip>();

    public virtual ICollection<Trip> TripSellerNavigations { get; set; } = new List<Trip>();
}
