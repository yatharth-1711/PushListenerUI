using MeterReader.DLMSInterfaceClasses.ProfileGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MeterReader.DLMSInterfaceClasses.ImageTransfer
{
    [Serializable]
    [XmlRoot("ImageTransfer")]
    public class DLMSImageTransferCollection
    {
        [XmlElement("Image")]
        public List<DLMSImageTransfer> Images { get; set; } = new List<DLMSImageTransfer>();
    }
}
