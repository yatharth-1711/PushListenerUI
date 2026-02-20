using System;

namespace MeterReader.DLMSInterfaceClasses.ProfileGeneric
{
    [Serializable]
    public class DLMSProfileGeneric
    {
        public string logical_name { get; set; }
        public string buffer { get; set; }
        public string capture_objects { get; set; }
        public string capture_period { get; set; }
        public string sort_method { get; set; }
        public string sort_object { get; set; }
        public string entries_in_use { get; set; }
        public string profile_entries { get; set; }
    }
}
