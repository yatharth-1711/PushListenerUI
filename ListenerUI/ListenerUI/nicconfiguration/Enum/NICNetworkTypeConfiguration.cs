using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MeterReader.NicConfiguration.Enum
{
    public enum NICNetworkTypeConfiguration
    {
        [XmlEnum("0")]
        Auto_NW_Selection = 0, //Default 
        [XmlEnum("1")]
        Manual_NW_Selection = 1
    }
}
