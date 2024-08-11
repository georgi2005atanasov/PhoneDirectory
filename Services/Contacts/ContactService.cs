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
            var contacts = from contact in db.Contacts.AsNoTracking()
                           join address in db.Addresses on contact.AddressId equals address.Id
                           join country in db.Countries on address.CountryId equals country.Id
                           join image in db.Images on contact.Id equals image.ContactId into imageGroup
                           from image in imageGroup.DefaultIfEmpty()
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

        public async Task<(List<ContactViewModel>, int deletedContactsCount)> AllDeleted(string? search,
            int page)
        {
            var contacts = from contact in db.Contacts.IgnoreQueryFilters().AsNoTracking()
                           where contact.IsDeleted
                           join address in db.Addresses.IgnoreQueryFilters() on contact.AddressId equals address.Id
                           join country in db.Countries.IgnoreQueryFilters() on address.CountryId equals country.Id
                           join image in db.Images on contact.Id equals image.ContactId into imageGroup
                           from image in imageGroup.DefaultIfEmpty()
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
                CountryId = selectedCountry.Id,
                CreatedOn = DateTime.Now
            };

            var contact = new Contact
            {
                Name = name,
                PhoneNumber = phoneNumber,
                Address = address,
                Email = email,
                Notes = notes,
                CreatedOn = DateTime.Now,
            };

            db.Contacts.Add(contact);

            await db.SaveChangesAsync();

            return contact.Id;
        }

        public async Task<bool> Delete(int contactId)
        {
            var contact = await db.Contacts
                .FirstAsync(x => x.Id == contactId);

            var address = await db.Addresses
                .FirstAsync(x => x.Id == contact.AddressId);

            var image = await db.Images
                .FirstAsync(x => x.ContactId == contactId);

            contact.IsDeleted = true;
            contact.DeletedOn = DateTime.Now;

            address.IsDeleted = true;
            address.DeletedOn = DateTime.Now;

            image.IsDeleted = true;
            image.DeletedOn = DateTime.Now;

            await db.SaveChangesAsync();

            return true;
        }

        public async Task<ContactDetailsViewModel> DetailsById(int contactId,
            bool? isDeleted = false)
        {
            var contactById = await
                (from contact in db.Contacts.IgnoreQueryFilters().AsNoTracking()
                 where contact.Id == contactId
                 where contact.IsDeleted == isDeleted
                 join address in db.Addresses.IgnoreQueryFilters() on contact.AddressId equals address.Id
                 join country in db.Countries on address.CountryId equals country.Id
                 join image in db.Images.IgnoreQueryFilters() on contact.Id equals image.ContactId into imageGroup
                 from image in imageGroup.DefaultIfEmpty()
                 select new ContactDetailsViewModel
                 {
                     Id = contactId,
                     Name = contact.Name,
                     Email = contact.Email,
                     Country = country.Name,
                     CountryPrefix = country.CountryPrefix,
                     PhoneNumber = contact.PhoneNumber,
                     PostalCode = address.PostalCode,
                     Street = address.Street,
                     Notes = contact.Notes,
                     ImageData = image.DetailsContent,
                     IsDeleted = contact.IsDeleted
                 })
                               .FirstAsync();

            return contactById;
        }

        public async Task<int> Edit(int contactId,
            string name,
            string phonePrefix,
            string phoneNumber,
            string? email,
            string? notes,
            string? street,
            int? postalCode)
        {
            var (contact, address, country) = await GetContactDetailsAsync(contactId, isDeleted: false);

            if (contact.Name != name ||
                contact.PhoneNumber != phoneNumber ||
                contact.Notes != notes)
            {
                contact.Name = name;
                contact.PhoneNumber = phoneNumber;
                contact.Notes = notes;
                contact.ModifiedOn = DateTime.Now;
            }

            if (phonePrefix != country.CountryPrefix)
            {
                var newCountry = await db.Countries
                    .FirstAsync(x => x.CountryPrefix == phonePrefix);

                address.Country = newCountry;
                address.ModifiedOn = DateTime.Now;
            }

            if (address.Street != street ||
                address.PostalCode != postalCode)
                address.ModifiedOn = DateTime.Now;

            address.Street = street;
            address.PostalCode = postalCode;

            await db.SaveChangesAsync();

            return contact.Id;
        }

        public async Task<bool> Restore(int contactId)
        {
            var (contact, address, country) = await GetContactDetailsAsync(contactId, isDeleted: true);

            contact.IsDeleted = false;
            contact.DeletedOn = null;

            address.IsDeleted = false;
            address.DeletedOn = null;

            var image = await db.Images
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.ContactId == contact.Id);

            if (image != null)
            {
                image.IsDeleted = false;
                image.DeletedOn = null;
            }

            await db.SaveChangesAsync();

            return true;
        }

        private async Task<(Contact, Address, Country)> GetContactDetailsAsync(int contactId,
            bool isDeleted)
        {
            if (!isDeleted)
            {
                var deletedContact = await db.Contacts
                    .FirstAsync(x => x.Id == contactId);

                var deletedAddress = await db.Addresses
                    .FirstAsync(x => x.Id == deletedContact.AddressId);

                var previousCountry = await db.Countries
                    .FirstAsync(x => x.Id == deletedAddress.CountryId);

                return (deletedContact, deletedAddress, previousCountry);
            }

            var contact = await db.Contacts
                    .IgnoreQueryFilters()
                    .FirstAsync(x => x.Id == contactId);

            var address = await db.Addresses
                .IgnoreQueryFilters()
                .FirstAsync(x => x.Id == contact.AddressId);

            var country = await db.Countries
                .IgnoreQueryFilters()
                .FirstAsync(x => x.Id == address.CountryId);

            return (contact, address, country);
        }
    }
}
