namespace PhoneDirectory.Data.Models
{
    public class CountryNumberLength
    {
        public int Id { get; set; }

        public int CountryId { get; set; }

        public Country? Country { get; set; }

        public int DigitsCount { get; set; }
    }
}
