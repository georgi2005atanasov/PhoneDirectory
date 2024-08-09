namespace PhoneDirectory.Infrastructure
{
    using System.Xml;
    using System.Xml.Serialization;
    using PhoneDirectory.Models.Country;
    using PhoneDirectory.Models.CountryNumberLength;

    public class XmlDataLoader
    {
        public List<CountryXmlModel> LoadCountries(string path)
        {
            CountriesWrapper countriesWrapper;
            XmlSerializer serializer = new XmlSerializer(typeof(CountriesWrapper));

            StreamReader reader = new StreamReader(path);
            countriesWrapper = (CountriesWrapper)serializer.Deserialize(reader)!;
            reader.Close();

            return countriesWrapper.Countries;
        }

        public List<CountryNumberLengthXmlModel> LoadCountriesNumbersLengths(string xmlFilePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CountryNumberLengthWrapper));

            var xmlReaderSettings = new XmlReaderSettings { IgnoreWhitespace = true };

            using (var reader = XmlReader.Create(xmlFilePath, xmlReaderSettings))
            {
                var lengthsWrapper = (CountryNumberLengthWrapper?)serializer.Deserialize(reader);
                return lengthsWrapper?.CountriesNumbersLengths ?? new List<CountryNumberLengthXmlModel>();
            }
        }
    }
}
