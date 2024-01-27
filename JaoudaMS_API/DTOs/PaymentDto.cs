namespace JaoudaMS_API.DTOs;

public class PaymentDto
{
    public string? Employee { get; set; }

    public byte Month { get; set; }

    public short Year { get; set; }

    public DateTime? Date { get; set; }

    public decimal? Salary { get; set; }

    public decimal? Commission { get; set; }
}
