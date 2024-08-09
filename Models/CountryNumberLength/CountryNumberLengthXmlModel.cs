namespace PhoneDirectory.Models.CountryNumberLength
{
    using System.Xml.Serialization;

    [Serializable()]
    public class CountryNumberLengthXmlModel
    {
        [XmlElementAttribute("Id")]
        public int Id { get; set; }

        [XmlElementAttribute("CountryId")]
        public int CountryId { get; set; }

        [XmlElementAttribute("DigitsCount")]
        public int DigitsCount { get; set; }
    }
}
