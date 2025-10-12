using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MeterReader.NicConfiguration.Enum
{
    public enum NICAPNProtocol
    {
        /// <summary>
        /// APN Protocol as IPv4
        /// </summary>
        [XmlEnum("1")]
        IPv4 = 1,
        /// <summary>
        /// APN Protocol as IPv6
        /// </summary>
        [XmlEnum("2")]
        IPv6 = 2,
        /// <summary>
        /// APN Protocol as IPv4 or IPv6 (Auto)
        /// </summary>
        [XmlEnum("3")]
        Auto = 3
    }
}
