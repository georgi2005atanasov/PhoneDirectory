namespace PhoneDirectory.Models.Country
{
    using System.Xml.Serialization;

    [Serializable()]
    [XmlRoot("CountriesCollection")]
    public class CountriesWrapper
    {
        [XmlArray("Countries")]
        [XmlArrayItem("Country", typeof(CountryXmlModel))]
        public List<CountryXmlModel> Countries { get; set; } = new List<CountryXmlModel>();
    }
}
