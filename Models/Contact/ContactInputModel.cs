namespace PhoneDirectory.Models.Contact
{
    using System.ComponentModel.DataAnnotations;
    using static Data.DataConstants.Contact;
    using static Data.ErrorMessages.Contact;

    public class ContactInputModel
    {
        [Required(ErrorMessage = NameIsRequired)]
        [StringLength(NameMaxLength, ErrorMessage = ExceededNameLength)]
        public string Name { get; set; } = null!;

        [EmailAddress(ErrorMessage = InvalidEmailAddress)]
        [StringLength(EmailMaxLength, ErrorMessage = EmailTooLong)]
        public string? Email { get; set; }

        public string PhonePrefix { get; set; } = null!;

        [Required(ErrorMessage = RequiredPhoneNumber)]
        [Phone(ErrorMessage = InvalidPhoneNumber)]
        public string PhoneNumber { get; set; } = null!;

        public IFormFile? Image { get; set; }

        [StringLength(NotesMaxLength, ErrorMessage = NotesTooLong)]
        public string? Notes { get; set; }

        public string? Street { get; set; }

        public int? PostalCode { get; set; }
    }
}
