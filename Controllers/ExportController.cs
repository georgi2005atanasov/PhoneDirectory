namespace PhoneDirectory.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PhoneDirectory.Services.Contact;
    using PhoneDirectory.Services.Export;
    using System.Text;

    public class ExportController : Controller
    {
        private readonly IExportService exportService;
        private readonly IContactService contactService;

        public ExportController(IExportService exportService,
            IContactService contactService)
        {
            this.exportService = exportService;
            this.contactService = contactService;
        }

        public async Task<IActionResult> CsvContacts(bool deleted, 
            string? search = "")
        {
            try
            {
                var contacts = await contactService.AllForExport(deleted, search);
                var csv = exportService.ContactsCsv(contacts);
                var bytes = Encoding.UTF8.GetBytes(csv);
                var result = new FileContentResult(bytes, "text/csv")
                {
                    FileDownloadName = "contacts.csv"
                };

                return result;
            }
            catch (Exception)
            {
                return RedirectToAction("All", "Contacts");
            }
        }
    }
}
