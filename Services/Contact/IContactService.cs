namespace PhoneDirectory.Services.Contact
{
    using PhoneDirectory.Models.Contact;

    public interface IContactService
    {
        Task<(List<ContactViewModel>, int allContactsCount)> All(string? search,
            int page);

        Task<(List<ContactViewModel>, int deletedContactsCount)> AllDeleted(string? search,
            int page);

        Task<List<ContactDetailsViewModel>> AllForExport(bool deleted,
            string? search);

        Task<int> Create(string name,
            string phonePrefix,
            string phoneNumber,
            string? email,
            string? notes,
            string? street,
            int? postalCode);

        Task<int> Edit(int contactId,
            string name,
            string phonePrefix,
            string phoneNumber,
            string? email,
            string? notes,
            string? street,
            int? postalCode);

        Task<bool> Delete(int contactId);

        Task<ContactDetailsViewModel> DetailsById(int contactId, bool? deletedOnly = false);

        Task<bool> Restore(int contactId);
    }
}
