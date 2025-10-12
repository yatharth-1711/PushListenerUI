using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MeterReader.DLMSInterfaceClasses.ProfileGeneric.DLMSProfileGenericObisCodes;

/*
 * Example Usage
 * string obis = ObisProfileHelper.ToObisString(ObisProfile.BillingProfile);
 * Convert string to enum
 * ObisProfile? profile = ProfileObisHelper.FromObisString("0.0.99.98.1.255");
 * if (profile != null){Console.WriteLine(profile.Value.ToString());}
 */
namespace MeterReader.DLMSInterfaceClasses.ProfileGeneric
{
    public class DLMSProfileGenericObisCodes
    {
        public const string NameplateProfile = "0.0.94.91.10.255";

        public const string VoltageRelatedEventsProfile = "0.0.99.98.0.255";

        public const string CurrentRelatedEventsProfile = "0.0.99.98.1.255";

        public const string PowerRelatedEventsProfile = "0.0.99.98.2.255";

        public const string TransactionsRelatedEventsProfile = "0.0.99.98.3.255";

        public const string OtherTamperEventsProfile = "0.0.99.98.4.255";

        public const string NonRollOverEventsProfile = "0.0.99.98.5.255";

        public const string ControlEventsProfile = "0.0.99.98.6.255";

        public const string InstantaneousProfile = "1.0.94.91.0.255";

        public const string BillingProfile = "1.0.98.1.0.255";

        public const string BlockLoadProfile = "1.0.99.1.0.255";

        public const string DailyLoadProfile = "1.0.99.2.0.255";

        public const string ScalerInstantaneousProfile = "1.0.94.91.3.255";

        public const string ScalerBlockLoadProfile = "1.0.94.91.4.255";

        public const string ScalerDailyLoadProfile = "1.0.94.91.5.255";

        public const string ScalerBillingProfile = "1.0.94.91.6.255";

        public const string ScalerEventsProfile = "1.0.94.91.7.255";
        public enum ObisCodeProfile
        {
            NameplateProfile,
            VoltageRelatedEventsProfile,
            CurrentRelatedEventsProfile,
            PowerRelatedEventsProfile,
            TransactionsRelatedEventsProfile,
            OtherTamperEventsProfile,
            NonRollOverEventsProfile,
            ControlEventsProfile,
            InstantaneousProfile,
            BillingProfile,
            BlockLoadProfile,
            DailyLoadProfile,
            ScalerInstantaneousProfile,
            ScalerBlockLoadProfile,
            ScalerDailyLoadProfile,
            ScalerBillingProfile,
            ScalerEventsProfile
        }
    }
    public static class ProfileObisHelper
    {
        public static string ToObisString(ObisCodeProfile profile)
        {
            switch (profile)
            {
                case ObisCodeProfile.NameplateProfile:
                    return "0.0.94.91.10.255";
                case ObisCodeProfile.VoltageRelatedEventsProfile:
                    return "0.0.99.98.0.255";
                case ObisCodeProfile.CurrentRelatedEventsProfile:
                    return "0.0.99.98.1.255";
                case ObisCodeProfile.PowerRelatedEventsProfile:
                    return "0.0.99.98.2.255";
                case ObisCodeProfile.TransactionsRelatedEventsProfile:
                    return "0.0.99.98.3.255";
                case ObisCodeProfile.OtherTamperEventsProfile:
                    return "0.0.99.98.4.255";
                case ObisCodeProfile.NonRollOverEventsProfile:
                    return "0.0.99.98.5.255";
                case ObisCodeProfile.ControlEventsProfile:
                    return "0.0.99.98.6.255";
                case ObisCodeProfile.InstantaneousProfile:
                    return "1.0.94.91.0.255";
                case ObisCodeProfile.BillingProfile:
                    return "1.0.98.1.0.255";
                case ObisCodeProfile.BlockLoadProfile:
                    return "1.0.99.1.0.255";
                case ObisCodeProfile.DailyLoadProfile:
                    return "1.0.99.2.0.255";
                case ObisCodeProfile.ScalerInstantaneousProfile:
                    return "1.0.94.91.3.255";
                case ObisCodeProfile.ScalerBlockLoadProfile:
                    return "1.0.94.91.4.255";
                case ObisCodeProfile.ScalerDailyLoadProfile:
                    return "1.0.94.91.5.255";
                case ObisCodeProfile.ScalerBillingProfile:
                    return "1.0.94.91.6.255";
                case ObisCodeProfile.ScalerEventsProfile:
                    return "1.0.94.91.7.255";
                default:
                    throw new ArgumentOutOfRangeException(nameof(profile), profile, null);
            }
        }

        public static ObisCodeProfile? FromObisString(string obis)
        {
            switch (obis)
            {
                case "0.0.94.91.10.255":
                    return ObisCodeProfile.NameplateProfile;
                case "0.0.99.98.0.255":
                    return ObisCodeProfile.VoltageRelatedEventsProfile;
                case "0.0.99.98.1.255":
                    return ObisCodeProfile.CurrentRelatedEventsProfile;
                case "0.0.99.98.2.255":
                    return ObisCodeProfile.PowerRelatedEventsProfile;
                case "0.0.99.98.3.255":
                    return ObisCodeProfile.TransactionsRelatedEventsProfile;
                case "0.0.99.98.4.255":
                    return ObisCodeProfile.OtherTamperEventsProfile;
                case "0.0.99.98.5.255":
                    return ObisCodeProfile.NonRollOverEventsProfile;
                case "0.0.99.98.6.255":
                    return ObisCodeProfile.ControlEventsProfile;
                case "1.0.94.91.0.255":
                    return ObisCodeProfile.InstantaneousProfile;
                case "1.0.98.1.0.255":
                    return ObisCodeProfile.BillingProfile;
                case "1.0.99.1.0.255":
                    return ObisCodeProfile.BlockLoadProfile;
                case "1.0.99.2.0.255":
                    return ObisCodeProfile.DailyLoadProfile;
                case "1.0.94.91.3.255":
                    return ObisCodeProfile.ScalerInstantaneousProfile;
                case "1.0.94.91.4.255":
                    return ObisCodeProfile.ScalerBlockLoadProfile;
                case "1.0.94.91.5.255":
                    return ObisCodeProfile.ScalerDailyLoadProfile;
                case "1.0.94.91.6.255":
                    return ObisCodeProfile.ScalerBillingProfile;
                case "1.0.94.91.7.255":
                    return ObisCodeProfile.ScalerEventsProfile;
                default:
                    return null;
            }
        }
    }

}
