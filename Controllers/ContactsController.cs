namespace PhoneDirectory.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using PhoneDirectory.Data;
    using PhoneDirectory.Models.Contact;
    using PhoneDirectory.Models.Country;
    using PhoneDirectory.Services;
    using PhoneDirectory.Services.Contact;
    using PhoneDirectory.Services.Image;
    using static WebConstants.Contact;

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

        public async Task<IActionResult> All(string? search,
            int page = 1)
        {
            try
            {
                var (contacts, allContactsCount) = await contactService.All(search, page);

                var allContactsModel = new AllContactsViewModel
                {
                    Contacts = contacts,
                    Search = search,
                    Page = page,
                    AllContactsCount = allContactsCount
                };

                return View(allContactsModel);
            }
            catch (Exception)
            {
                return RedirectToAction("All");
            }
        }

        public async Task<IActionResult> Deleted(string? search,
            int page = 1)
        {
            try
            {
                var (contacts, allContactsCount) = await contactService.AllDeleted(search, page);

                var deletedContacts = new AllContactsViewModel
                {
                    Contacts = contacts,
                    Search = search,
                    Page = page,
                    AllContactsCount = allContactsCount
                };

                return View(deletedContacts);
            }
            catch (Exception)
            {
                return RedirectToAction("All");
            }
        }

        public async Task<IActionResult> Details(int id, 
            bool? isDeleted = false)
        {
            try
            {
                var contact = await contactService.DetailsById(id, isDeleted);

                return View(contact);
            }
            catch (Exception)
            {
                TempData["Message"] = CouldNotGetContact;
                TempData["MessageType"] = WebConstants.DangerMessage;
                return RedirectToAction("All");
            }
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
                ModelState.AddModelError("PhoneNumber", ContactAlreadyExists);

            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
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

                        await transaction.CommitAsync();

                        TempData["Message"] = ContactAddedSuccessfully;
                        TempData["MessageType"] = WebConstants.SuccessMessage;

                        return RedirectToAction("All");
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                    }
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    TempData["Message"] = ErrorWhileAddingContact;
                    TempData["MessageType"] = WebConstants.DangerMessage;
                    return RedirectToAction("All");
                }
            }

            if (contactModel == null)
                ModelState.AddModelError("Image", ImageTooBig);

            ViewBag.CountriesAndPrefixes = await GetCountriesAndPrefixes();

            return View(contactModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                ViewBag.CountriesAndPrefixes = await GetCountriesAndPrefixes();

                var detailsModel = await contactService.DetailsById(id);
                var editModel = new EditContactViewModel(detailsModel);

                return View(editModel);
            }
            catch (Exception)
            {
                TempData["Message"] = ContactDoesNotExists;
                TempData["MessageType"] = WebConstants.DangerMessage;
                return RedirectToAction("All");
            }
        }

        [HttpPut]
        [RequestSizeLimit(4 * 1024 * 1024)]
        public async Task<IActionResult> Edit(EditContactViewModel contactModel)
        {
            if (contactModel != null &&
                await db.Contacts
                        .AnyAsync(x => x.PhoneNumber == contactModel.PhoneNumber &&
                                  x.Id != contactModel.Id))
                ModelState.AddModelError("PhoneNumber", ContactAlreadyExists);
            
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        var contactId = await contactService.Edit(contactModel!.Id,
                            contactModel.Name,
                            contactModel.CountryPrefix,
                            contactModel.PhoneNumber,
                            contactModel.Email,
                            contactModel.Notes,
                            contactModel.Street,
                            contactModel.PostalCode);

                        if (contactModel.UploadedImage != null)
                        {
                            byte[] smallCircularImage = ImageHelper.CreateCircleImage(contactModel.UploadedImage, 55);
                            byte[] resizedImage = ImageHelper.ResizeImage(contactModel.UploadedImage, 180, 250);

                            await imageService.ChangeImage(contactModel.UploadedImage.FileName,
                                contactModel.UploadedImage.ContentType,
                                resizedImage,
                                smallCircularImage,
                                contactId);
                        }

                        await transaction.CommitAsync();

                        TempData["Message"] = ContactEditedSuccessfully;
                        TempData["MessageType"] = WebConstants.SuccessMessage;

                        return RedirectToAction("All");
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                    }
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    TempData["Message"] = ErrorWhileEditing;
                    TempData["MessageType"] = WebConstants.DangerMessage;
                    return RedirectToAction("All");
                }
            }

            if (contactModel == null)
                ModelState.AddModelError("Image", ImageTooBig);

            ViewBag.CountriesAndPrefixes = await GetCountriesAndPrefixes();

            return View(contactModel);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var detailsModel = await contactService.Delete(id);

                TempData["Message"] = ContactDeletedSuccessfully;
                TempData["MessageType"] = WebConstants.SuccessMessage;

                return RedirectToAction("All");
            }
            catch (Exception)
            {
                TempData["Message"] = CouldNotDeleteContact;
                TempData["MessageType"] = WebConstants.DangerMessage;
                return RedirectToAction("All");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Restore(int id)
        {
            try
            {
                var result = await contactService.Restore(id);

                TempData["Message"] = ContactRestoredSuccessfully;
                TempData["MessageType"] = WebConstants.SuccessMessage;

                return RedirectToAction("Deleted");
            }
            catch (Exception)
            {
                TempData["Message"] = ErrorWhileRestoringContact;
                TempData["MessageType"] = WebConstants.DangerMessage;
                return RedirectToAction("Deleted");
            }
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