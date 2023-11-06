using JaoudaMS_API.Models;

namespace JaoudaMS_API.DTOs
{
    public class SupplierDto
    {
        public string Id { get; set; } = null!;

        public string? Address { get; set; }

        public string? Tel { get; set; }

        public string? Email { get; set; }
    }
}
