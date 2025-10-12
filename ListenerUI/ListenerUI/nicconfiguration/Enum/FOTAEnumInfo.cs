using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MeterReader.NicConfiguration.Enum
{

    public enum FOTAEnumInfo
    {
        [XmlEnum("1")]
        MainFileName = 1,
        [XmlEnum("2")]
        TestFileName = 2,
        [XmlEnum("3")]
        FTPUser = 3,
        [XmlEnum("4")]
        FTPAddress = 4,
        [XmlEnum("5")]
        FTPPassword = 5,
        [XmlEnum("6")]
        FTPPort = 6,
        [XmlEnum("7")]
        FTPTransactionMode = 7,
        [XmlEnum("8")]
        MainFWName = 8,
        [XmlEnum("9")]
        TestFWName = 9,

    }
}
