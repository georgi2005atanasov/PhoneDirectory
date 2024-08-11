namespace PhoneDirectory.Services.Contacts
{
    using Microsoft.EntityFrameworkCore;
    using PhoneDirectory.Data;
    using PhoneDirectory.Data.Models;
    using PhoneDirectory.Models.Contact;
    using System.Collections.Generic;
    using static WebConstants;

    public class ContactService : IContactService
    {
        private readonly ApplicationDbContext db;

        public ContactService(ApplicationDbContext db)
            => this.db = db;

        public async Task<(List<ContactViewModel>, int)> All(string? search,
            int page)
        {
            var contacts = from contact in db.Contacts
                           join address in db.Addresses on contact.AddressId equals address.Id
                           join country in db.Countries on address.CountryId equals country.Id
                           join image in db.Images on contact.Id equals image.ContactId
                           select new ContactViewModel
                           {
                               ContactId = contact.Id,
                               Name = contact.Name,
                               Prefix = country.CountryPrefix,
                               PhoneNumber = contact.PhoneNumber,
                               ImageData = image.CircleContent
                           };

            if (search != null)
                contacts = contacts.Where(x => x.Name.ToLower().Contains(search) ||
                                               x.PhoneNumber.StartsWith(search) ||
                                               x.PhoneNumber.EndsWith(search) ||
                                               x.Prefix.StartsWith(search));

            var contactsCount = await contacts.CountAsync();

            return (await contacts
                .OrderBy(x => x.Name)
                .Skip(ItemsPerPage * (page - 1))
                .Take(ItemsPerPage)
                .ToListAsync(), contactsCount);
        }

        public async Task<int> Create(string name,
            string phonePrefix,
            string phoneNumber,
            string? email,
            string? notes,
            string? street,
            int? postalCode)
        {
            var selectedCountry = await db.Countries
                .FirstAsync(x => x.CountryPrefix == phonePrefix);

            var address = new Address
            {
                PostalCode = postalCode,
                Street = street,
                CountryId = selectedCountry.Id
            };

            var contact = new Contact
            {
                Name = name,
                PhoneNumber = phoneNumber,
                Address = address,
                Email = email,
                Notes = notes,
            };

            db.Contacts.Add(contact);

            await db.SaveChangesAsync();

            return contact.Id;
        }
    }
}
