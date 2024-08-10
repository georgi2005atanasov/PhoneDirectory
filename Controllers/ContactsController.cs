namespace PhoneDirectory.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using PhoneDirectory.Data;
    using PhoneDirectory.Models;
    using PhoneDirectory.Models.Contact;
    using PhoneDirectory.Models.Country;
    using PhoneDirectory.Services;
    using PhoneDirectory.Services.Contacts;
    using PhoneDirectory.Services.Images;
    using System.Diagnostics;

    public class ContactsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IContactService contactService;
        private readonly IImageService imageService;

        public ContactsController(ApplicationDbContext db,
            IContactService contactService,
            IImageService imageService)
        {
            this.db = db;
            this.contactService = contactService;
            this.imageService = imageService;
        }

        public async Task<IActionResult> All(string? search)
        {
            var contacts = await contactService.All(search);

            var allContactsModel = new AllContactsViewModel
            {
                Contacts = contacts,
                Search = search
            };

            return View(allContactsModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.CountriesAndPrefixes = await GetCountriesAndPrefixes();
            return View();
        }

        [HttpPost]
        [RequestSizeLimit(2 * 1024 * 1024)]
        public async Task<IActionResult> Create(ContactInputModel contactModel)
        {
            if (contactModel != null && await db.Contacts.AnyAsync(x => x.PhoneNumber == contactModel.PhoneNumber))
                ModelState.AddModelError("PhoneNumber", "Contact with this number already exists!");

            if (ModelState.IsValid)
            {
                var contactId = await contactService.Create(contactModel!.Name,
                    contactModel.PhonePrefix,
                    contactModel.PhoneNumber,
                    contactModel.Email,
                    contactModel.Notes,
                    contactModel.Street,
                    contactModel.PostalCode);

                if (contactModel.Image != null)
                {
                    byte[] smallCircularImage = ImageHelper.CreateCircleImage(contactModel.Image, 55);
                    byte[] resizedImage = ImageHelper.ResizeImage(contactModel.Image, 180, 250);

                    await imageService.Create(resizedImage,
                        smallCircularImage,
                        contactModel.Image.FileName,
                        contactModel.Image.ContentType,
                        contactId);
                }

                return RedirectToAction("All");
            }

            if (contactModel == null)
                ModelState.AddModelError("Image", "The uploaded file exceeds the allowed size limit of 1.99MB.");

            ViewBag.CountriesAndPrefixes = await GetCountriesAndPrefixes();

            return View(contactModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<List<CountryPrefixSelectModel>> GetCountriesAndPrefixes()
        => await db.Countries
            .Select(x => new CountryPrefixSelectModel
            {
                IsoCode = x.IsoCode,
                Prefix = x.CountryPrefix,
                CountryName = x.Name
            })
            .OrderBy(x => x.CountryName)
            .ToListAsync();
    }
}