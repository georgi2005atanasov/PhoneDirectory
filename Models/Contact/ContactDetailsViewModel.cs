namespace PhoneDirectory.Models.Contact
{
    public class ContactDetailsViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Email { get; set; }

        public string? Street { get; set; }

        public int? PostalCode { get; set; }

        public string? Country { get; set; }

        public string PhoneNumber { get; set; } = null!;

        public string? Notes { get; set; }

        public byte[]? ImageData { get; set; }
    }
}
