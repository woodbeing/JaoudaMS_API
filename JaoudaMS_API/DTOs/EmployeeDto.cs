namespace JaoudaMS_API.DTOs;

public class EmployeeDto
{
    public string Cin { get; set; } = null!;

    public string? Type { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public string? Tel { get; set; }

    public decimal? Salary { get; set; }

    public decimal? Commission { get; set; }

    public virtual ICollection<PaymentDto> Payments { get; set; } = new List<PaymentDto>();
}
