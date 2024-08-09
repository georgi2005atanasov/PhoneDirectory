namespace PhoneDirectory.Data.Models
{
    public class Image
    {
        public int Id { get; set; }

        public string? OriginalFileName { get; set; } = null!;

        public byte[]? CircleContent { get; set; }

        public byte[]? DetailsContent { get; set; }

        public int? ContactId { get; set; }

        public Contact? Contact { get; set; }
    }
}
