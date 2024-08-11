using PhoneDirectory.Models.Contact;

namespace PhoneDirectory.Services.Contacts
{
    public interface IContactService
    {
        Task<(List<ContactViewModel>, int allContactsCount)> All(string? search,
            int page);

        Task<int> Create(string name,
            string phonePrefix,
            string phoneNumber,
            string? email,
            string? notes,
            string? street,
            int? postalCode);
    }
}
