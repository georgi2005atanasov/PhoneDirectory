namespace PhoneDirectory.Models.Contact
{
    public class ContactViewModel
    {
        public int ContactId { get; set; }

        public string Name { get; set; } = null!;

        public string CountryPrefix { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public byte[]? ImageData { get; set; }
    }
}
