using System;
using System.Collections.Generic;
using static MeterReader.DLMSInterfaceClasses.PushSetup.PushSetupObisCodes;

namespace MeterReader.DLMSInterfaceClasses.ActionSchedule
{
    public static class ActionScheduleObisCodes
    {
        public enum ActionScheduleObject
        {
            Billing,
            ImageActivation,
            Instant,
            LoadSurvey,
            DailyEnergy,
            SelfRegistration,
            Tamper,
            CurrentBill
        }
        private static readonly Dictionary<ActionScheduleObject, string> _defaultObis = new Dictionary<ActionScheduleObject, string>()
                                                                                        {
                                                                                            { ActionScheduleObject.Billing, "0.0.15.0.0.255" },
                                                                                            { ActionScheduleObject.ImageActivation, "0.0.15.0.2.255" },
                                                                                            { ActionScheduleObject.Instant, "0.0.15.0.4.255" },
                                                                                            { ActionScheduleObject.LoadSurvey, "0.4.15.0.4.255" },
                                                                                            { ActionScheduleObject.DailyEnergy, "0.5.15.0.4.255" },
                                                                                            { ActionScheduleObject.SelfRegistration, "0.0.15.0.142.255" },
                                                                                            { ActionScheduleObject.Tamper, "0.0.15.0.143.255" },
                                                                                            { ActionScheduleObject.CurrentBill, "0.0.15.0.147.255" }
                                                                                        };

        private static readonly Dictionary<ActionScheduleObject, string> _currentObis = new Dictionary<ActionScheduleObject, string>(_defaultObis);
        public static string Get(ActionScheduleObject obj) => _currentObis[obj];
        public static void Set(ActionScheduleObject obj, string newObis)
        {
            if (!string.IsNullOrWhiteSpace(newObis))
                _currentObis[obj] = newObis;
        }
        public static void Reset(ActionScheduleObject obj) => _currentObis[obj] = _defaultObis[obj];
        public static IReadOnlyDictionary<ActionScheduleObject, string> GetAll() => _currentObis;
    }
}
