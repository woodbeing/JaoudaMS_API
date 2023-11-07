using System;
using System.Collections.Generic;

namespace JaoudaMS_API.Models;

public partial class Payment
{
    public string Employee { get; set; } = null!;

    public byte Month { get; set; }

    public short Year { get; set; }

    public DateTime? Date { get; set; }

    public decimal? Salary { get; set; }

    public decimal? Commission { get; set; }

    public virtual Employee EmployeeNavigation { get; set; } = null!;
}
