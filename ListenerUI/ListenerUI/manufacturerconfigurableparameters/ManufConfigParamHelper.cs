using AutoTestDesktopWFA;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MeterReader.TestHelperClasses.ManufacturerConfigurableParameters
{
    public class ManufConfigParamHelper
    {
        public static void SaveToXml(ManufacturerConfigParameterCollection collection)
        {
            string filePath = System.IO.Path.Combine(Utilities.RefXMLFilePath, "ManufacturerConfigParameters.xml");
            XmlSerializer serializer = new XmlSerializer(typeof(ManufacturerConfigParameterCollection));
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, collection);
            }
        }

        public static ManufacturerConfigParameterCollection LoadFromXml()
        {
            string filePath = System.IO.Path.Combine(Utilities.RefXMLFilePath, "ManufacturerConfigParameters.xml");
            XmlSerializer serializer = new XmlSerializer(typeof(ManufacturerConfigParameterCollection));
            using (StreamReader reader = new StreamReader(filePath))
            {
                return (ManufacturerConfigParameterCollection)serializer.Deserialize(reader);
            }
        }
    }
}
