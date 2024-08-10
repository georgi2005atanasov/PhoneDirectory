using PhoneDirectory.Models.Contact;

namespace PhoneDirectory.Services.Contacts
{
    public interface IContactService
    {
        Task<List<ContactViewModel>> All(string? search);

        Task<int> Create(string name,
            string phonePrefix,
            string phoneNumber,
            string? email,
            string? notes,
            string? street,
            int? postalCode);
    }
}
