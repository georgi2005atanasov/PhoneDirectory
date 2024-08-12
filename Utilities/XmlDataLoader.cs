namespace PhoneDirectory.Utilities
{
    using System.Xml.Serialization;
    using PhoneDirectory.Models.Country;

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
    }
}
