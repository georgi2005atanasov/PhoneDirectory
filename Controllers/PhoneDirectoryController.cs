namespace PhoneDirectory.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using PhoneDirectory.Data;
    using PhoneDirectory.Models;
    using PhoneDirectory.Models.Contact;
    using PhoneDirectory.Models.Country;
    using System.Diagnostics;

    public class PhoneDirectoryController : Controller
    {
        private readonly ApplicationDbContext db;

        public PhoneDirectoryController(ApplicationDbContext db)
            => this.db = db;

        public IActionResult All()
        {
            return View();
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
            if (ModelState.IsValid)
            {
                // SAVE
                return RedirectToAction("All");
            }

            if (contactModel == null)
                ModelState.AddModelError("Image", "The uploaded file exceeds the allowed size limit of 2MB.");

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