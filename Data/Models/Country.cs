namespace PhoneDirectory.Data.Models
{
    public class Country
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string IsoCode { get; set; } = null!;

        public string CountryPrefix { get; set; } = null!;
    }
}
