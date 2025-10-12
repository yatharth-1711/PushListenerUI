using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MeterReader.NicConfiguration.Enum
{
    public enum NICServerMode
    {
        [XmlEnum("0")]
        Disable = 0,
        [XmlEnum("1")]
        Enable = 1 //Default
    }
}
