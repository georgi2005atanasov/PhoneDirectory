namespace PhoneDirectory.Models.Country
{
    using System.Xml.Serialization;

    [Serializable()]
    public class CountryXmlModel
    {
        [XmlElementAttribute("Id")]
        public int Id { get; set; }

        [XmlElementAttribute("Name")]
        public string Name { get; set; } = null!;

        [XmlElementAttribute("PhonePrefix")]
        public string PhonePrefix { get; set; } = null!;

        [XmlElementAttribute("IsoCode")]
        public string IsoCode { get; set; } = null!;
    }
}
