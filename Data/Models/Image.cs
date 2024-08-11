namespace PhoneDirectory.Data.Models
{
    using PhoneDirectory.Data.Models.Base;

    public class Image : DeletableEntity
    {
        public int Id { get; set; }

        public string? OriginalFileName { get; set; } = null!;

        public string OriginalType { get; set; } = null!;

        public byte[]? CircleContent { get; set; }

        public byte[]? DetailsContent { get; set; }

        public int? ContactId { get; set; }

        public Contact? Contact { get; set; }
    }
}
