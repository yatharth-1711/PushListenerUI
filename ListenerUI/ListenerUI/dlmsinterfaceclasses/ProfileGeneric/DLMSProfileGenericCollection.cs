using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MeterReader.DLMSInterfaceClasses.ProfileGeneric
{
    [Serializable]
    [XmlRoot("ProfileGeneric")]
    public class DLMSProfileGenericCollection
    {
        [XmlElement("Profile")]
        public List<DLMSProfileGeneric> Profiles { get; set; } = new List<DLMSProfileGeneric>();
    }
}
