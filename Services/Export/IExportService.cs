namespace PhoneDirectory.Services.Export
{
    using PhoneDirectory.Models.Contact;

    public interface IExportService
    {
        string ContactsCsv(IEnumerable<ContactDetailsViewModel> contacts);
    }
}