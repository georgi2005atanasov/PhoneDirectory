
namespace PhoneDirectory.Services.Export
{
    using PhoneDirectory.Models.Contact;
    using PhoneDirectory.Utilities;
    using System.Text;

    public class ExportService : IExportService
    {
        public string ContactsCsv(IEnumerable<ContactDetailsViewModel> contacts)
        {
            var csv = new StringBuilder();
            csv.AppendLine("Id,Name,PhoneNumber,Email,CreatedOn,ModifiedOn,IsDeleted,DeletedOn,Country,Street,PostalCode,Notes");

            foreach (var contact in contacts)
            {
                csv.AppendLine($"{CsvHelper.EscapeCsvValue(contact.Id.ToString())},{CsvHelper.EscapeCsvValue(contact.Name)},{CsvHelper.EscapeCsvValue(contact.CountryPrefix)}{CsvHelper.EscapeCsvValue(contact.PhoneNumber)},{CsvHelper.EscapeCsvValue(contact.Email ?? "")},{CsvHelper.EscapeCsvValue(contact.CreatedOn.ToString())},{CsvHelper.EscapeCsvValue(contact.ModifiedOn.ToString() ?? "")},{CsvHelper.EscapeCsvValue(contact.IsDeleted.ToString() ?? "")},{CsvHelper.EscapeCsvValue(contact.DeletedOn.ToString() ?? "")},{CsvHelper.EscapeCsvValue(contact.Country ?? "")},{CsvHelper.EscapeCsvValue(contact.Street ?? "")},{CsvHelper.EscapeCsvValue(contact.PostalCode.ToString() ?? "")},{CsvHelper.EscapeCsvValue(contact.Notes ?? "")}");
            }

            return csv.ToString();
        }
    }
}
