using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MeterReader.TestHelperClasses.ManufacturerConfigurableParameters
{
    [Serializable]
    [XmlRoot("ManufacturerParameters")]
    public class ManufacturerConfigParameterCollection
    {
        [XmlElement("Parameter")]
        public List<ManufacturerParameterInfo> Parameters { get; set; } = new List<ManufacturerParameterInfo>();
    }
}
