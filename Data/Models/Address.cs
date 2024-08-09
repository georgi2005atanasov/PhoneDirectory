namespace PhoneDirectory.Data.Models
{
    public class Address
    {
        public int Id { get; set; }

        public string? Street { get; set; }

        public int? PostalCode { get; set; }

        public int? CountryId { get; set; }

        public Country? Country { get; set; }
    }
}
