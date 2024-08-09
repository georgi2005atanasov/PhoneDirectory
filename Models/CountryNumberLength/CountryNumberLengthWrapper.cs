namespace PhoneDirectory.Models.CountryNumberLength
{
    using System.Xml.Serialization;

    [Serializable()]
    [XmlRoot("CountriesNumbersLengthsCollection")]
    public class CountryNumberLengthWrapper
    {
        [XmlArray("CountriesNumbersLengths")]
        [XmlArrayItem("CountriesNumbersLength", typeof(CountryNumberLengthXmlModel))]
        public List<CountryNumberLengthXmlModel> CountriesNumbersLengths { get; set; } = new List<CountryNumberLengthXmlModel>();
    }
}
