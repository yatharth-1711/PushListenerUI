using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MeterReader.NicConfiguration.Enum
{
    public enum MODULEFOTAStatus
    {
        [XmlEnum("0")]
        NotInitialed = 0, //Default
        [XmlEnum("16")]
        Initiated = 16,
        [XmlEnum("32")]
        InProgress = 32,
        [XmlEnum("48")]
        Failed = 48
    }
}
