
using System.Xml.Serialization;
namespace Indali.Security.Enum
{
    public enum Standard
    {
        [XmlEnum("0")] DLMS,
        [XmlEnum("1")] India,
        [XmlEnum("2")] Italy,
        [XmlEnum("3")] SaudiArabia,
    }
}
