namespace PhoneDirectory.Models.Contact
{
    public class ContactDetailsViewModel : ContactViewModel
    {
        public int Id { get; set; }

        public string? Email { get; set; }

        public string? Street { get; set; }

        public int? PostalCode { get; set; }

        public string? Country { get; set; }

        public string? Notes { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
