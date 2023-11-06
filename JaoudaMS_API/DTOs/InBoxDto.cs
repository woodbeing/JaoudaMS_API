namespace JaoudaMS_API.DTOs
{
    public class InBoxDto
    {
        public string Product { get; set; } = null!;

        public string Box { get; set; } = null!;

        public int Capacity { get; set; }

        public int? InStock { get; set; }

        public int? Empty { get; set; }
    }
}
