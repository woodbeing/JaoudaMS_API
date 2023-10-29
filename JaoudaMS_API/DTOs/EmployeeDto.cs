namespace JaoudaMS_API.DTOs
{
    public class EmployeeDto
    {
        public string Cin { get; set; } = null!;

        public string? Type { get; set; }

        public string? Name { get; set; }

        public string? Adress { get; set; }

        public string? Tel { get; set; }

        public decimal? Salary { get; set; }

        public decimal? Comission { get; set; }
    }
}
