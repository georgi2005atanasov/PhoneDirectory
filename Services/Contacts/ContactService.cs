namespace PhoneDirectory.Services.Contacts
{
    using Microsoft.EntityFrameworkCore;
    using PhoneDirectory.Data;
    using PhoneDirectory.Data.Models;
    using PhoneDirectory.Models.Contact;
    using System.Collections.Generic;

    public class ContactService : IContactService
    {
        private readonly ApplicationDbContext db;

        public ContactService(ApplicationDbContext db)
            => this.db = db;

        public Task<List<ContactViewModel>> All(string? search)
        {
            var contacts = db.Contacts
                .Select(x => new ContactViewModel
                {
                    ContactId = x.Id,
                    Name = x.Name,
                    Prefix = db.Countries
                    .First(y => y.Id == db.Addresses
                                        .First(z => z.Id == x.AddressId)
                                        .CountryId)
                    .CountryPrefix,
                    PhoneNumber = x.PhoneNumber,
                    ImageData = db.Images.First(y => y.ContactId == x.Id).CircleContent
                });

            if (search != null)
                contacts = contacts.Where(x => x.Name.ToLower().Contains(search) ||
                                               x.PhoneNumber.StartsWith(search) ||
                                               x.PhoneNumber.EndsWith(search));

            return contacts.ToListAsync();
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
