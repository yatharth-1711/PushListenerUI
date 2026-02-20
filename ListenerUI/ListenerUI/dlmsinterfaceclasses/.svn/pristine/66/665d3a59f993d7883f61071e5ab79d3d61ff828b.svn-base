using System;
using System.Collections.Generic;

namespace MeterReader.DLMSInterfaceClasses.PushSetup
{
    public static class PushSetupObisCodes
    {
        public enum PushSetupObject
        {
            Billing,
            Alert,
            Instant,
            LoadSurvey,
            DailyEnergy,
            SelfRegistration,
            Tamper,
            CurrentBill
        }
        private static readonly Dictionary<PushSetupObject, string> _defaultObis = new Dictionary<PushSetupObject, string>()
                                                                                    {
                                                                                        { PushSetupObject.Instant, "0.0.25.9.0.255" },
                                                                                        { PushSetupObject.Alert, "0.4.25.9.0.255" },
                                                                                        { PushSetupObject.LoadSurvey, "0.5.25.9.0.255" },
                                                                                        { PushSetupObject.DailyEnergy, "0.6.25.9.0.255" },
                                                                                        { PushSetupObject.SelfRegistration, "0.130.25.9.0.255" },
                                                                                        { PushSetupObject.Billing, "0.132.25.9.0.255" },
                                                                                        { PushSetupObject.Tamper, "0.134.25.9.0.255" },
                                                                                        { PushSetupObject.CurrentBill, "0.0.25.9.129.255" }
                                                                                    };

        private static readonly Dictionary<PushSetupObject, string> _currentObis = new Dictionary<PushSetupObject, string>(_defaultObis);
        public static string Get(PushSetupObject obj) => _currentObis[obj];
        public static void Set(PushSetupObject obj, string newObis)
        {
            if (!string.IsNullOrWhiteSpace(newObis))
                _currentObis[obj] = newObis;
        }
        public static void Reset(PushSetupObject obj) => _currentObis[obj] = _defaultObis[obj];
        public static IReadOnlyDictionary<PushSetupObject, string> GetAll() => _currentObis;
    }
}
