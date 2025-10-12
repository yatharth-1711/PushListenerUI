using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MeterReader.NicConfiguration.Enum
{
    public enum NICMasterNumberEnableDisableForSMS
    {
        [XmlEnum("0")]
        Only_By_Register = 0,
        [XmlEnum("1")]
        By_Any_Number = 1 //Default
    }
}
