using System;
using System.Collections.Generic;

namespace JaoudaMS_API.Models;

public partial class Truck
{
    public string Matricula { get; set; } = null!;

    public string? Type { get; set; }

    public virtual ICollection<Trip> Trips { get; set; } = new List<Trip>();
}
