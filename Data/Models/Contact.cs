namespace PhoneDirectory.Data.Models
{
    using PhoneDirectory.Data.Models.Base;
    using System.ComponentModel.DataAnnotations;
    using static DataConstants.Contact;
    using static ErrorMessages.Contact;

    public class Contact : DeletableEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = NameIsRequired)]
        [StringLength(NameMaxLength, ErrorMessage = ExceededNameLength)]
        public string Name { get; set; } = null!;

        [EmailAddress(ErrorMessage = InvalidEmailAddress)]
        [StringLength(EmailMaxLength, ErrorMessage = EmailTooLong)]
        public string? Email { get; set; }

        public int? AddressId { get; set; }

        public Address? Address { get; set; }

        [Required(ErrorMessage = RequiredPhoneNumber)]
        [Phone(ErrorMessage = InvalidPhoneNumber)]
        public string PhoneNumber { get; set; } = null!;

        [StringLength(NotesMaxLength, ErrorMessage = NotesTooLong)]
        public string? Notes { get; set; }
    }
}