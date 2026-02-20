using AutoTest.FrameWork.Converts;
using Gurux.DLMS.Secure;
using ListenerUI.HelperClasses;
using log4net;
using MeterComm;
using MeterReader.CommonClasses;
using MeterReader.Converter;
using MeterReader.DLMSInterfaceClasses.ActionSchedule;
using MeterReader.DLMSInterfaceClasses.ProfileGeneric;
using MeterReader.DLMSInterfaceClasses.PushSetup;
using MeterReader.DLMSNetSerialCommunication;
using MeterReader.NicConfiguration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace MeterReader.TestHelperClasses
{
    public class PushPacketManager
    {
        #region Event Handlers
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static TestLogService _logService;
        public static string TestName;
        public static RichTextBox logBox;
        DLMSParser parse = new DLMSParser();
        #endregion

        #region PUSH PROCESSING GLOBAL VARIABLE
        public GXDLMSSecureClient client = new GXDLMSSecureClient();
        public Gurux.DLMS.Enums.Security GXSecurity;
        RichTextBox _logBox = new RichTextBox();
        public string input_packet;
        public string[] Enc_packets;
        public string presentAlerts = (string)null;
        public List<string> input_Break_Final = new List<string>();
        public List<string> input_Break_Initial = new List<string>();
        public List<string> input_Break_Profile = new List<string>();
        #endregion

        #region Validation and Verification Variables
        public static int testDurationinMinutes = 30;
        public static int freq_Instant = 0; public static int freq_SR = 0;
        public static int freq_LS = 0; public static int freq_DE = 0;
        public static int freq_CB = 0;
        public static string freq_Bill = "01/*/* 00:00:00";
        public static string TestProfiles = "All"; //Instant,SR,LS,DE,Tamper,Bill,Alert,CB, All
        public static string DeviceID = "";
        public static List<string> priorityOrder = new List<string> { "Bill", "DE", "LS", "SR", "Instant" };
        public static string highestFreqProfile = ""; public static int highestFrequency = 0;
        public static bool isCurrentBillAvailable = true;

        public static double Random_Instant = 0;
        public static double Random_LS = 0;
        public static double Random_DE = 0;
        public static double Random_Bill = 0;
        public static double Random_CB = 0;

        public static List<DateTime> lst_ExpectedPacketTiming_Instant = new List<DateTime>();
        public static List<DateTime> lst_ExpectedPacketTiming_LS = new List<DateTime>();
        public static List<DateTime> lst_ExpectedPacketTiming_DE = new List<DateTime>();
        public static List<DateTime> lst_ExpectedPacketTiming_SR = new List<DateTime>();
        public static List<DateTime> lst_ExpectedPacketTiming_Bill = new List<DateTime>();
        public static List<DateTime> lst_ExpectedPacketTiming_CB = new List<DateTime>();

        public static List<DateTime> lst_ReceivedPacketTiming_Instant = new List<DateTime>();
        public static List<DateTime> lst_ReceivedPacketTiming_LS = new List<DateTime>();
        public static List<DateTime> lst_ReceivedPacketTiming_DE = new List<DateTime>();
        public static List<DateTime> lst_ReceivedPacketTiming_SR = new List<DateTime>();
        public static List<DateTime> lst_ReceivedPacketTiming_Bill = new List<DateTime>();
        public static List<DateTime> lst_ReceivedPacketTiming_Alert = new List<DateTime>();
        public static List<DateTime> lst_ReceivedPacketTiming_Tamper = new List<DateTime>();
        public static List<DateTime> lst_ReceivedPacketTiming_CB = new List<DateTime>();

        #endregion           

        #region Stored Datatables Variables
        //push Setup Datatable
        public static DataTable dtPushSetup_Instant = new DataTable();
        public static DataTable dtPushSetup_Bill = new DataTable();
        public static DataTable dtPushSetup_LS = new DataTable();
        public static DataTable dtPushSetup_DE = new DataTable();
        public static DataTable dtPushSetup_SR = new DataTable();
        public static DataTable dtPushSetup_Alert = new DataTable();
        public static DataTable dtPushSetup_Tamper = new DataTable();

        // ProfileGeneric Datatable
        public static DataTable dtProfileGeneric_Instant = new DataTable();
        public static DataTable dtProfileGeneric_Bill = new DataTable();
        public static DataTable dtProfileGeneric_LS = new DataTable();
        public static DataTable dtProfileGeneric_DE = new DataTable();

        //Appended Datatable 
        public static DataTable dt_Appended_LS = new DataTable();
        public static DataTable dt_Appended_DE = new DataTable();
        public static DataTable dt_Appended_Bill = new DataTable();
        public static DataTable dt_Appended_Instant = new DataTable();
        public static bool isInstantProfile = false;
        public static string pushProfile = null;

        // Received Push Profile Datatable
        public static DataTable dtRec_Push_Instant = new DataTable();
        public static DataTable dtRec_Push_Alert = new DataTable();
        public static DataTable dtRec_Push_LS = new DataTable();
        public static DataTable dtRec_Push_DE = new DataTable();
        public static DataTable dtRec_Push_SR = new DataTable();
        public static DataTable dtRec_Push_Bill = new DataTable();
        public static DataTable dtRec_Push_Tamper = new DataTable();
        public static DataTable dtRec_Push_CB = new DataTable();

        // Received Optical Profile Datatable
        public static DataTable dtRec_Optical_Instant = new DataTable();
        public static DataTable dtRec_Optical_LS = new DataTable();
        public static DataTable dtRec_AppendedOpt_LS = new DataTable();
        public static DataTable dtRec_Optical_DE = new DataTable();
        public static DataTable dtRec_AppendedOpt_DE = new DataTable();
        public static DataTable dtRec_Optical_SR = new DataTable();
        public static DataTable dtRec_Optical_Bill = new DataTable();
        public static DataTable dtRec_AppendedOpt_Bill = new DataTable();
        public static DataTable dtRec_Optical_CB = new DataTable();
        public static DataTable dtRec_AppendedOpt_CB = new DataTable();
        public static DataTable dtRec_Optical_Tamper = new DataTable();

        //Filtered Received Push Datatable
        public static DataTable filteredPushLSDT = new DataTable();
        public static DataTable filteredPushDEDT = new DataTable();
        public static DataTable filteredPushBillDT = new DataTable();
        public static DataTable filteredPushTamperDT = new DataTable();
        public static DataTable filteredPushSRDT = new DataTable();
        #endregion

        public PushPacketManager()
        {
            UpdateProfileDetails();
        }

        public static List<(string Name, string DisplayName, string PushSetupClassObis, string ActionScheduleClassObisAtt, int Frequency, double Randomisation, List<DateTime> ExpectedList, List<DateTime> ReceivedList)> profileDetails =
                  new List<(string Name, string DisplayName, string PushSetupClassObis, string ActionScheduleClassObisAtt, int Frequency, double Randomisation, List<DateTime> ExpectedList, List<DateTime> ReceivedList)>
                    {
                        ("Instant",     "Instant",              "00280000190900FF",     "001600000F0004FF04",       freq_Instant,   Random_Instant, lst_ExpectedPacketTiming_Instant,   lst_ReceivedPacketTiming_Instant),
                        ("LS",          "Load Survey",          "00280005190900FF",     "001600040F0004FF04",       freq_LS,        Random_LS,      lst_ExpectedPacketTiming_LS,        lst_ReceivedPacketTiming_LS),
                        ("DE",          "Daily Energy",         "00280006190900FF",     "001600050F0004FF04",       freq_DE,        Random_DE,      lst_ExpectedPacketTiming_DE,        lst_ReceivedPacketTiming_DE),
                        ("SR",          "Self Registration",    "00280082190900FF",     "001600000F008EFF04",       freq_SR,        0,              lst_ExpectedPacketTiming_SR,        lst_ReceivedPacketTiming_SR),
                        ("CB",          "Current Bill",         "00280000190981FF",     "001600000F0093FF04",       freq_CB,        0,              lst_ExpectedPacketTiming_CB,        lst_ReceivedPacketTiming_CB),
                        ("Bill",        "Bill",                 "00280084190900FF",     "001600000F0000FF04",       0,              0,              lst_ExpectedPacketTiming_Bill,      lst_ReceivedPacketTiming_Bill),
                        ("Alert",       "Alert",                "00280004190900FF",     null,                       0,              0,              null,                               lst_ReceivedPacketTiming_Alert),
                        ("Tamper",      "Tamper",               "00280086190900FF",     "001600000F008FFF04",       0,              0,              null,                               lst_ReceivedPacketTiming_Tamper)
                    };

        #region Initial Settings 
        public bool GetNICandModuleDetails(ref DLMSComm DLMSReader)
        {
            bool isSimAvailable = true;
            try
            {
                _logService.LogMessage(logBox, $"NIC and Module Details", Color.Black, true);
                TestStopWatch watch = new TestStopWatch();
                watch.Start();
                while (watch.GetElapsedSeconds() < 16 * 60)
                {
                    if (string.IsNullOrEmpty(MeterIdentity.SIMHostName))
                    {
                        SetGetFromMeter.Wait(100 * 1000);
                        MeterIdentity.SIMHostName = parse.GetProfileValueString(SetGetFromMeter.GetDataFromObject(ref DLMSReader, 48, "0.0.25.7.0.255", 4).Substring(4));
                        if (!string.IsNullOrEmpty(MeterIdentity.SIMHostName))
                        {
                            WrapperInfo.hostName = MeterIdentity.SIMHostName.ToString().Trim();
                            watch.Stop();
                            watch.Dispose();
                            break;
                        }
                    }
                    else
                    {
                        watch.Stop();
                        watch.Dispose();
                        break;
                    }
                }
                _logService.LogMessage(logBox, _logService.FormatLogLineString("\tSIM HostName", string.IsNullOrEmpty(MeterIdentity.SIMHostName) ? "SIM Not Inserted" : MeterIdentity.SIMHostName), Color.Black);
                //_logService.LogMessage(logBox, _logService.FormatLogLineString("\tSIM HostName", new[] { string.IsNullOrEmpty(MeterIdentity.SIMHostName) ? "SIM Not Inserted" : MeterIdentity.SIMHostName }), Color.Black);
                if (string.IsNullOrEmpty(MeterIdentity.SIMHostName))
                {
                    isSimAvailable = false;
                    return isSimAvailable;
                }
                NicConfigIDs nicConfigIDs = new NicConfigIDs();
                string readData = SetGetFromMeter.GetDataFromObject(ref DLMSReader, 1, "0.0.0.2.128.255", 2);
                nicConfigIDs.AssignAllCurrentNicFwInfo(readData);
                var CurrentNICFw_IDs = NicConfigIDs.currentNICDetails;
                foreach (var key in CurrentNICFw_IDs)
                {
                    if (key.Key == "IMEI" || key.Key == "SIM" || key.Key == "NICFW" || key.Key == "ModuleFW")
                    {
                        //_logService.LogMessage(logBox, _logService.FormatLogLineString($"\t{key.Key}", $"{string.Join(", ", key.Value.Select(inner => $"{parse.HexString2Ascii(inner.Value)}"))}"), Color.Black);
                        _logService.LogMessage(logBox, _logService.FormatLogLineString($"\t{key.Key}", $"{string.Join(", ", key.Value.Select(inner => $"{parse.HexString2Ascii(inner.Value)}"))}"), Color.Black);
                    }
                }
                readData = string.Empty;
                readData = SetGetFromMeter.GetDataFromObject(ref DLMSReader, 1, "0.0.96.12.129.255", 2);
                int networkMode = WrapperParser.GetNetworkMode(readData);
                _logService.LogMessage(logBox, _logService.FormatLogLineString("\tNetwork Mode", networkMode == 2 ? "4G" : networkMode == 0 ? "2G" : "NA"), Color.Black);

            }
            catch (Exception ex)
            {
                log.Error($"[{nameof(GetNICandModuleDetails)}] - {ex.Message}", ex);
            }
            return isSimAvailable;
        }
        /// <summary>
        /// Calculates the best billing date string (dd/*/* HH:mm:ss) for testing a billing procedure,
        /// ensuring it falls within the given test interval whenever possible.
        /// If not possible, it selects the nearest valid billing date after the start of the test time.
        /// </summary>
        public static string GetTestBillingDate(DateTime testStartDate, DateTime testEndDate, string freq_Bill)
        {
            TimeSpan billingTime = TimeSpan.Parse(freq_Bill.Split(' ')[1]);
            if (freq_Bill.Substring(0, 2).ToString() == "00")
                return freq_Bill;
            DateTime? selectedBillingDate = null;
            try
            {
                int daysInMonth = DateTime.DaysInMonth(testStartDate.Year, testStartDate.Month);
                for (int billingDay = 1; billingDay <= daysInMonth; billingDay++)
                {
                    DateTime billingDateOption = new DateTime(testStartDate.Year, testStartDate.Month, billingDay,
                        billingTime.Hours, billingTime.Minutes, billingTime.Seconds);

                    if (billingDateOption >= testStartDate && billingDateOption <= testEndDate)
                    {
                        selectedBillingDate = billingDateOption;
                        break;
                    }
                }
                if (selectedBillingDate == null)
                {
                    int startDay = testStartDate.Day;

                    if (startDay <= daysInMonth)
                    {
                        selectedBillingDate = new DateTime(testStartDate.Year, testStartDate.Month, startDay,
                            billingTime.Hours, billingTime.Minutes, billingTime.Seconds);

                        if (selectedBillingDate < testStartDate)
                            selectedBillingDate = selectedBillingDate.Value.AddDays(1);
                    }
                    else
                    {
                        DateTime lastDayOfMonth = new DateTime(testStartDate.Year, testStartDate.Month, daysInMonth,
                            billingTime.Hours, billingTime.Minutes, billingTime.Seconds);

                        selectedBillingDate = lastDayOfMonth.AddDays(1);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"[{nameof(GetTestBillingDate)}] - {ex.Message}", ex);
            }
            return $"{selectedBillingDate.Value:dd}/*/* {billingTime:hh\\:mm\\:ss}";
        }
        #endregion 

        #region Setting Destination address, Randomisation and Push Frequency   
        /// <summary>
        /// Set Push Destination Address 
        /// </summary>
        /// <param name="DLMSObj"></param>
        /// <param name="profile"></param>
        /// <returns></returns>
        public List<string> SetPushSetUpDestination(ref DLMSComm DLMSObj, string TestProfile)
        {
            _logService.LogMessage(logBox, $"Setting Destination Address:", Color.Brown, true);
            string port = "4059";
            List<string> setResponseDestinationAdd = new List<string>();
            try
            {
                UpdateProfileDetails();
                if (!NetworkHelper.IsIPv6Configured())
                {
                    setResponseDestinationAdd.Add("IPv6 is not configured.");
                    return setResponseDestinationAdd;
                }
                string ipv6Address = NetworkHelper.GetIPv6Address();
                if (string.IsNullOrEmpty(ipv6Address))
                {
                    setResponseDestinationAdd.Add("Unable to fetch IPV6 address.");
                    return setResponseDestinationAdd;
                }
                string destinationAddress = $"[{ipv6Address}]:{port}";
                destinationAddress = DLMSParser.ConvertAsciiToHex(destinationAddress.Trim());
                string destinationAddString = $"0203160009{(destinationAddress.Length / 2).ToString("X2")}{destinationAddress}1600";
                var targets = string.Equals(TestProfile, "All", StringComparison.OrdinalIgnoreCase)
                                            ? profileDetails
                                            : profileDetails.Where(p => p.Name.Equals(TestProfile, StringComparison.OrdinalIgnoreCase))
                                                            .Union(profileDetails.Where(p => p.Name == "Alert"));
                foreach (var profile in targets)
                {
                    if (!isCurrentBillAvailable && profile.Name == "CB")
                        continue;

                    int nRetValue = DLMSObj.SetParameter(profile.PushSetupClassObis + "03", (byte)0, (byte)3, (byte)3, destinationAddString);
                    setResponseDestinationAdd.Add(($"Destination Address for {profile.DisplayName} ({parse.HexObisToDecObis(profile.PushSetupClassObis.Substring(4))}):").PadRight(70) + (nRetValue != 0 ? $"Error In Setting [{ipv6Address}]:{port}" : $"Set Successfully to [{ipv6Address}]:{port}"));
                }
            }
            catch (Exception ex)
            {
                log.Error($"[{nameof(SetPushSetUpDestination)}] - {ex.Message} TRACE:{ex.StackTrace}");
            }
            return setResponseDestinationAdd;
        }
        /// <summary>
        /// Method for logging of set and get response of push frequency  
        /// </summary>
        /// <param name="DLMSReader"></param>
        /// <param name="logBox"></param>
        /// <param name="TestProfiles"></param>
        /// <returns></returns>
        public List<string> LogPushFrequency(ref DLMSComm DLMSReader, RichTextBox logBox, bool isRandomisationApplicable = false)
        {
            List<string> resultPushFrequenctSet = new List<string>();
            string randomisationSetResponse = string.Empty;
            string pushFreqSetResponse = string.Empty;
            _logService.LogMessage(logBox, $"\nSetting Push Frequency for profile(s)", Color.Brown, true);
            try
            {
                UpdateProfileDetails();
                if (!isRandomisationApplicable)
                {
                    Random_Instant = 0;
                    Random_LS = 0;
                    Random_DE = 0;
                    Random_Bill = 0;
                    Random_CB = 0;
                }
                foreach (var profile in profileDetails)
                {
                    if (!isCurrentBillAvailable && profile.Name == "CB") continue;
                    if (profile.Name != "Alert" && profile.Name != "Tamper")    // Push Frequency Set does not applicable for both "Alert" and "Tamper"
                    {
                        pushFreqSetResponse = SetProfilePushFrequency(ref DLMSReader, profile.Name, profile.Frequency);
                        _logService.LogMessage(logBox, $"\t📌 {pushFreqSetResponse}", Color.Black, true);
                        resultPushFrequenctSet.Add(pushFreqSetResponse);
                    }
                    if (isRandomisationApplicable)
                    {
                        if (profile.Name != "Alert" && profile.Name != "Tamper" && profile.Name != "SR")    // Randomisation does not applicable for "Self Registration","Alert" and "Tamper"
                        {
                            randomisationSetResponse = SetRandomisation(ref DLMSReader, profile.Name, profile.Randomisation);
                            _logService.LogMessage(logBox, $"\t{randomisationSetResponse}", randomisationSetResponse.Contains("Successfully") ? Color.Green : Color.Red);
                        }
                    }
                    if (profile.Name != "Alert" && profile.Name != "Tamper")    // Push Frequency Get does not applicable for both "Alert" and "Tamper"
                    {
                        string actionSchedule = parse.GetPushFrequency(GetPushFrequencySchedule(ref DLMSReader, profile.Name));
                        if (actionSchedule == "0B") actionSchedule = "Object Not Available";
                        _logService.LogMessage(logBox, $"\tAction Schedule for {profile.DisplayName}:\n\t{actionSchedule}", actionSchedule.Contains("Object Not Available") ? Color.Red : Color.Black);
                    }
                }
                #region BILL Handled Separately
                //string billScheduleHex = $"010102020904{int.Parse(freq_Bill.Substring(7, 2)):X2}" +
                //                         $"{int.Parse(freq_Bill.Substring(10, 2)):X2}" +
                //                         $"{int.Parse(freq_Bill.Substring(13, 2)):X2}FF0905FFFFFF" +
                //                         $"{int.Parse(freq_Bill.Substring(0, 2)):X2}FF";//10/*/* 00:00:00
                /*if (freq_Bill.Substring(0, 2) == "00")
                {
                    billScheduleHex = "0100"; freq_Bill = "N/A";
                }
                string billResponse = SetProfilePushFrequency(ref DLMSReader, "Bill", 0, billScheduleHex);

                string pushFreqSetResponseBill = billResponse.Contains("Successfully")
                                                ? $"Bill Push Frequency: Set Successfully to {freq_Bill}"
                                                : $"Bill Push Frequency: Error in Setting {freq_Bill}";
                _logService.LogMessage(logBox, $"\t📌 {pushFreqSetResponseBill}", Color.Black, true);


                randomisationSetResponse = SetRandomisation(ref DLMSReader, "Bill", Random_Bill);
                _logService.LogMessage(logBox, $"\t{randomisationSetResponse}", randomisationSetResponse.Contains("Successfully") ? Color.Green : Color.Red);


                string billActionSchedule = parse.GetPushFrequency(GetPushFrequencySchedule(ref DLMSReader, "Bill"));
                _logService.LogMessage(logBox, $"\tAction Schedule for Billing:\n\t{billActionSchedule}", Color.Black);
                resultPushFrequenctSet.Add(pushFreqSetResponseBill);*/
                #endregion
            }
            catch (Exception ex)
            {
                log.Error($"[{nameof(LogPushFrequency)}] - {ex.Message} TRACE:{ex.StackTrace}");
            }
            return resultPushFrequenctSet;
        }
        /// <summary>
        /// NEW
        /// </summary>
        /// <param name="DLMSObj"></param>
        /// <param name="profileName"></param>
        /// <param name="frequency"></param>
        /// <returns></returns> 
        public string SetProfilePushFrequency(ref DLMSComm DLMSObj, string profileName, int frequency)
        {
            string resultMessage = string.Empty; string billDateTime = string.Empty;
            int nRetValue = 100;
            var profile = profileDetails.Find(p => p.Name == profileName);
            Dictionary<Int32, string> actionScheduleStrings = new Dictionary<Int32, string>
            {
                { 15, "010402020904FF0000000905FFFFFFFFFF02020904FF0F00000905FFFFFFFFFF02020904FF1E00000905FFFFFFFFFF02020904FF2D00000905FFFFFFFFFF" },
                { 30, "010202020904FF0000000905FFFFFFFFFF02020904FF1E00000905FFFFFFFFFF" },
                { 60, "010102020904FF0000000905FFFFFFFFFF" },
                { 240, "010602020904000000000905FFFFFFFFFF02020904040000000905FFFFFFFFFF02020904080000000905FFFFFFFFFF020209040C0000000905FFFFFFFFFF02020904100000000905FFFFFFFFFF02020904140000000905FFFFFFFFFF" },
                { 360, "010402020904000000000905FFFFFFFFFF02020904060000000905FFFFFFFFFF020209040C0000000905FFFFFFFFFF02020904120000000905FFFFFFFFFF" },
                { 480, "010302020904000000000905FFFFFFFFFF02020904080000000905FFFFFFFFFF02020904100000000905FFFFFFFFFF" },
                { 720, "010202020904000000000905FFFFFFFFFF020209040C0000000905FFFFFFFFFF"},
                { 1440, "010102020904000000000905FFFFFFFFFF"},
                { 0, "0100"},
                { 1, billDateTime }
            };
            if (profileName == "Bill" && frequency == 1)
            {
                actionScheduleStrings[1] = billDateTime = $"010102020904{int.Parse(freq_Bill.Substring(7, 2)):X2}" +
                        $"{int.Parse(freq_Bill.Substring(10, 2)):X2}" +
                        $"{int.Parse(freq_Bill.Substring(13, 2)):X2}FF0905FFFFFF" +
                        $"{int.Parse(freq_Bill.Substring(0, 2)):X2}FF"; //10/*/* 00:00:00
            }
            if (string.IsNullOrEmpty(profileName))
                return "Error: Unsupported profile '" + profileName + "'.";

            if (!isCurrentBillAvailable && profileName == "CB")
                return "Current Bill Profile not Available";

            string frequencySet = frequency == 0 ? "N/A" : (profileName == "Bill" ? freq_Bill : (frequency / 60 == 0 ? $"{frequency} min" : $"{(frequency / 60.0)} hr"));

            try
            {
                nRetValue = DLMSObj.SetParameter(profile.ActionScheduleClassObisAtt, (byte)0, (byte)3, (byte)3, actionScheduleStrings[frequency]);
                if (nRetValue == 0)
                    resultMessage = string.Format("{0,-45}{1,20}", $"{profileName} Push Frequency ({parse.HexObisToDecObis(profile.ActionScheduleClassObisAtt.Substring(4, 12))}):", $"Set Successfully to {frequencySet}");
                //resultMessage = $"{profileName} Push Frequency:\t Set Successfully to {frequencySet}.";
                else if (nRetValue == 2)
                    resultMessage = string.Format("{0,-45}{1,20}", $"{profileName} Push Frequency ({parse.HexObisToDecObis(profile.ActionScheduleClassObisAtt.Substring(4, 12))}):", $"Action Denied");
                //resultMessage = $"{profileName} Push Frequency:\t Action Denied.";
                else if (nRetValue == 1 || nRetValue == 3)
                    resultMessage = string.Format("{0,-450}{1,20}", $"{profileName} Push Frequency ({parse.HexObisToDecObis(profile.ActionScheduleClassObisAtt.Substring(4, 12))}):", $"Error in Setting");
                //resultMessage = $"{profileName} Push Frequency:\t Error in Setting.";
            }
            catch (Exception ex)
            {
                log.Error($"[{nameof(SetProfilePushFrequency)}] - {ex.Message} TRACE:{ex.StackTrace}");
            }
            return resultMessage;
        }
        /// <summary>
        /// Get Action Schedule for any profile by passing profile name as argument
        /// </summary>
        /// <param name="DLMSObj"></param>
        /// <param name="profile">Instant, SR, LS, DE, Tamper, Bill</param>
        /// <returns></returns>
        public string GetPushFrequencySchedule(ref DLMSComm DLMSObj, string profileName)
        {
            string resultMessage = string.Empty;
            var profile = profileDetails.Find(p => p.Name == profileName); //00000F0004FF
            /* Dictionary<string, string> profileObis = new Dictionary<string, string>
             {
               { "Instant", "00000F0004FF" },
               { "SR", "00000F008EFF" },
               { "LS", "00040F0004FF" },
               { "DE", "00050F0004FF" },
               { "Tamper", "00000F008FFF" },
               { "Bill", "00000F0000FF" },
               { "CB", "00000F0093FF" }
             };*/
            try
            {
                resultMessage = SetGetFromMeter.GetDataFromObject(ref DLMSObj, 22, parse.HexObisToDecObis(profile.ActionScheduleClassObisAtt.Substring(4, 12)), 4).Trim();
            }
            catch (Exception ex)
            {
                log.Error($"[{nameof(GetPushFrequencySchedule)}] - {ex.Message} TRACE:{ex.StackTrace}");
            }
            return resultMessage;
        }
        public string SetRandomisation(ref DLMSComm DLMSObj, string profileName, double Randomisation = 0)
        {
            string resultRandomisationSet = string.Empty;
            try
            {
                if (Randomisation == 0)
                    return string.Format("{0,-30}{1,-30}", $"Randomisation for {profileName}", ": Not Applicable");
                //return $"Randomisation for {profileName}: Not Applicable";
                var profile = profileDetails.Find(p => p.Name == profileName);
                if (profile.Frequency == 0)
                {
                    if (profileName == "Bill" && freq_Bill.Substring(0, 2) == "00")
                        return string.Format("{0,-30}{1,-30}", $"Randomisation for {profileName}", ": Frequency not defined");
                    //return $"Randomisation for {profileName}: Frequency not defined";
                }
                if (Randomisation * 60 <= (((profile.Frequency * 60) / 2) - 1) || profileName == "Bill")
                {
                    string randomisationSetString = $"12{((int)(Randomisation * 60)).ToString("X4")}";
                    if ((isCurrentBillAvailable && profileName == "CB") || profileName != "CB")
                    {
                        string setRandomisation = Randomisation / 60 > 1 ? Randomisation / 60 + " hr" : Randomisation + " min";
                        int nRetValue = DLMSObj.SetParameter(profile.PushSetupClassObis + "05", (byte)0, (byte)3, (byte)3, randomisationSetString);
                        resultRandomisationSet = nRetValue != 0 ? string.Format("{0,-30}{1,-30}{2,-10}", $"Randomisation for {profileName}", ": Error In Setting", $": {setRandomisation}")
                                                                : string.Format("{0,-30}{1,-30}{2,-10}", $"Randomisation for {profileName}", ": Set Successfully to", $": {setRandomisation}");
                        //resultRandomisationSet = nRetValue != 0 ? $"Randomisation for {profileName}:\t Error In Setting: {setRandomisation}"
                        //                                        : $"Randomisation for {profileName}:\t Set Successfully to {setRandomisation}";
                    }
                }
                else
                {
                    resultRandomisationSet = "Randomisation Value does not follow: Randomisation time ≤ (half of the frequency period - 1 second)";
                }

            }
            catch (Exception ex)
            {
                log.Error($"[{nameof(SetRandomisation)}] - {ex.Message} TRACE:{ex.StackTrace}");
            }
            return resultRandomisationSet;
        }
        #endregion

        #region Push Packet Data Handling
        /// <summary>
        /// Initializes Push Setup and Profile Generic DataTables for all push profiles  
        /// (Instant, Alert, LS, DE, SR, Billing, Tamper) with required filtering and formatting.
        /// </summary>
        public void InitializePushProfileTables()
        {
            try
            {
                ResetRecPushDT();
                // Instant
                dtPushSetup_Instant = DataTableOperations.FilterPushSetUpDataTable(PushSetupInfo.InstantDT.Copy());
                isInstantProfile = dtPushSetup_Instant.AsEnumerable().Any(row => row.Field<string>("OBIS")?.Contains("1.0.94.91.0.255") == true);
                if (isInstantProfile)
                {
                    dtPushSetup_Instant = DataTableOperations.RemoveLastRows(DataTableOperations.FilterPushSetUpDataTable(PushSetupInfo.InstantDT.Copy()), 1);
                    dtProfileGeneric_Instant = DataTableOperations.FilterProfileGenericDataTable(ProfileGenericInfo.instantDT.Copy());
                    dt_Appended_Instant = DataTableOperations.AppendDataTables(dtPushSetup_Instant, dtProfileGeneric_Instant);
                }
                // Self Registration
                dtPushSetup_SR = DataTableOperations.FilterPushSetUpDataTable(PushSetupInfo.SelfRegDT.Copy());
                // Alert
                dtPushSetup_Alert = DataTableOperations.FilterPushSetUpDataTable(PushSetupInfo.AlertDT.Copy());
                // Billing
                dtPushSetup_Bill = DataTableOperations.RemoveLastRows(DataTableOperations.FilterPushSetUpDataTable(PushSetupInfo.BillDT.Copy()), 1);
                dtProfileGeneric_Bill = DataTableOperations.FilterProfileGenericDataTable(ProfileGenericInfo.billingDT.Copy());
                dt_Appended_Bill = DataTableOperations.AppendDataTables(dtPushSetup_Bill, dtProfileGeneric_Bill);
                // Load Survey
                dtPushSetup_LS = DataTableOperations.RemoveLastRows(DataTableOperations.FilterPushSetUpDataTable(PushSetupInfo.LSDT.Copy()), 1);
                dtProfileGeneric_LS = DataTableOperations.FilterProfileGenericDataTable(ProfileGenericInfo.blockloadDT.Copy());
                dt_Appended_LS = DataTableOperations.AppendDataTables(dtPushSetup_LS, dtProfileGeneric_LS);
                dt_Appended_LS = TransposeDataTable(dt_Appended_LS);
                // Daily Energy
                dtPushSetup_DE = DataTableOperations.RemoveLastRows(DataTableOperations.FilterPushSetUpDataTable(PushSetupInfo.DEDT.Copy()), 1);
                dtProfileGeneric_DE = DataTableOperations.FilterProfileGenericDataTable(ProfileGenericInfo.dailyloadDT.Copy());
                dt_Appended_DE = DataTableOperations.AppendDataTables(dtPushSetup_DE, dtProfileGeneric_DE);
                // Tamper
                dtPushSetup_Tamper = DataTableOperations.FilterPushSetUpDataTable(PushSetupInfo.TamperDT.Copy());
            }
            catch (Exception ex)
            {
                log.Error($"[{nameof(InitializePushProfileTables)}] - {ex.Message} TRACE:{ex.StackTrace}");
            }
        }
        /// <summary>
        /// Generates a DataTable containing the parsed push data extracted from a decrypted hex packet. Processes the decrypted hex string, and stores the extracted values in a tabular format for further use.
        /// </summary>
        /// <param name="decryptedData">The decrypted hex string of the push notification data.</param>
        /// <param name="dataPacket">The original push notification packet containing metadata such as profile type.</param>
        /// <returns>A DataTable with a single column ("Push Data") containing the parsed values from the decrypted packet.</returns>
        public DataTable GeneratePushDataTableFromHex(string decryptedData, string dataPacket)
        {
            if (dataPacket.Contains("Instant Push")) pushProfile = "Instant";
            else if (dataPacket.Contains("Alert Push")) pushProfile = "Alert";
            else if (dataPacket.Contains("Load Survey Push")) pushProfile = "LS";
            else if (dataPacket.Contains("Daily Energy Push")) pushProfile = "DE";
            else if (dataPacket.Contains("Self Registration Push")) pushProfile = "SR";
            else if (dataPacket.Contains("Billing Push")) pushProfile = "Billing";
            else if (dataPacket.Contains("Tamper Push")) pushProfile = "Tamper";
            else if (dataPacket.Contains("Current Bill Push")) pushProfile = "Current Bill";
            DataTable rawPushPacketTable = new DataTable();
            try
            {
                input_Break_Final.Clear();
                rawPushPacketTable.Columns.Add("Push Data", typeof(string));
                decryptedData = decryptedData.Replace(" ", "").Replace("\n", "");
                GetDataFromDecryptedPacket(decryptedData);
                foreach (var item in input_Break_Final)
                {
                    rawPushPacketTable.Rows.Add(item.ToString());
                }
            }
            catch (Exception ex)
            {
                log.Error($"[{nameof(GeneratePushDataTableFromHex)}] - {ex.Message} TRACE:{ex.StackTrace}");
            }
            return rawPushPacketTable;
        }
        /// <summary>
        /// Parses a decrypted push notification packet based on the currently selected push profile 
        /// It determines the correct Push Setup and Profile Generic DataTables, breaks down the input hex string into parameter data, and extracts profile data values.  
        /// The parsed values are stored in intermediate lists (Push Setup list, profile parameter list, Final merged) 
        /// </summary>
        /// <param name="input">The decrypted hex string representing the push notification packet.</param>
        public void GetDataFromDecryptedPacket(string input)
        {
            DataTable dtPushSetUp = new DataTable();
            DataTable dtProfileGeneric = new DataTable();
            switch (pushProfile)
            {
                case "Instant":
                    if (isInstantProfile)
                    {
                        dtPushSetUp = DataTableOperations.FilterPushSetUpDataTable(PushSetupInfo.InstantDT.Copy());
                        dtProfileGeneric = dtProfileGeneric_Instant.Copy();
                    }
                    else

                    {
                        dtPushSetUp = DataTableOperations.FilterPushSetUpDataTable(PushSetupInfo.InstantDT.Copy());
                        dtProfileGeneric = dtProfileGeneric_Instant;
                    }
                    break;
                case "Alert":
                    dtPushSetUp = dtPushSetup_Alert;
                    break;
                case "LS":
                    dtPushSetUp = DataTableOperations.FilterPushSetUpDataTable(PushSetupInfo.LSDT.Copy());
                    dtProfileGeneric = dtProfileGeneric_LS;
                    break;
                case "DE":
                    dtPushSetUp = DataTableOperations.FilterPushSetUpDataTable(PushSetupInfo.DEDT.Copy());
                    dtProfileGeneric = dtProfileGeneric_DE;
                    break;
                case "SR":
                    dtPushSetUp = dtPushSetup_SR;
                    break;
                case "Current Bill":
                case "Billing":
                    dtPushSetUp = DataTableOperations.FilterPushSetUpDataTable(PushSetupInfo.BillDT.Copy());
                    dtProfileGeneric = dtProfileGeneric_Bill;
                    break;
                case "Tamper":
                    dtPushSetUp = dtPushSetup_Tamper;
                    break;
            }
            int pointer = 0;
            try
            {
                input_Break_Initial.Clear();
                input_Break_Profile.Clear();
                input_Break_Initial.Add(input.Substring(pointer, 2));
                pointer += 2;
                input_Break_Initial.Add(input.Substring(pointer, 8));
                pointer += 8;
                if (input.Substring(pointer, 2) == "0C")
                {
                    input_Break_Initial.Add(input.Substring(pointer, 26));
                    pointer += 26;
                }
                if (input.Length < pointer + 4)
                    return;
                string profileString = input.Substring(pointer);
                if (profileString == "0200" || profileString.Length < 4 || input.Substring(pointer, 2) != "02")
                {
                    //Added ffor alert if 0101 is present in decrypted packet
                    pointer += 4;
                    if (profileString == "0200" || profileString.Length < 4 || input.Substring(pointer, 2) != "02")
                    {
                        return;
                    }
                }
                if (input.Substring(pointer, 2) == "02")
                {
                    pointer += 2;
                    if (dtPushSetUp.Rows.Count == Convert.ToInt32(input.Substring(pointer, 2), 16))
                    {
                        pointer += 2;
                        for (int i = 0; i < dtPushSetUp.Rows.Count; i++)
                        {
                            string value = parse.GetProfileDataString(input, ref pointer);
                            if (!string.IsNullOrEmpty(value))
                            {
                                input_Break_Initial.Add(value);
                            }
                        }
                    }
                }
                if (DataTableOperations.IsStringInColumn(dtPushSetUp, "Class", "7") && input_Break_Initial.Count > 0)
                {
                    string lastValue = input_Break_Initial.Last();
                    int ptr = 4;
                    //if (lastValue.Length > ptr)
                    if (pushProfile == "LS")
                    {
                        while (lastValue.Length > ptr)
                        {
                            if (dtProfileGeneric.Rows.Count == Convert.ToInt32(lastValue.Substring(ptr + 2, 2), 16))
                            {
                                ptr += 4;
                                for (int i = 0; i < dtProfileGeneric.Rows.Count; i++)
                                {
                                    string innerValue = parse.GetProfileDataString(lastValue, ref ptr);
                                    if (!string.IsNullOrEmpty(innerValue))
                                        input_Break_Profile.Add(innerValue);
                                }
                            }
                            input_Break_Final = input_Break_Initial.GetRange(0, input_Break_Initial.Count - 1);
                            input_Break_Final.AddRange(input_Break_Profile);
                        }
                    }
                    else
                    {
                        if (lastValue.Length > ptr)
                        {
                            if (lastValue.Substring(6, 2) == "81")
                            {
                                if (dtProfileGeneric.Rows.Count == Convert.ToInt32(lastValue.Substring(ptr + 4, 2), 16))
                                {
                                    ptr += 6;
                                    for (int i = 0; i < dtProfileGeneric.Rows.Count; i++)
                                    {
                                        string innerValue = parse.GetProfileDataString(lastValue, ref ptr);
                                        if (!string.IsNullOrEmpty(innerValue))
                                            input_Break_Profile.Add(innerValue);
                                    }
                                }
                            }
                            else if (lastValue.Substring(6, 2) == "82")
                            {
                                if (dtProfileGeneric.Rows.Count == Convert.ToInt32(lastValue.Substring(ptr + 4, 4), 16))
                                {
                                    ptr += 8;
                                    for (int i = 0; i < dtProfileGeneric.Rows.Count; i++)
                                    {
                                        string innerValue = parse.GetProfileDataString(lastValue, ref ptr);
                                        if (!string.IsNullOrEmpty(innerValue))
                                            input_Break_Profile.Add(innerValue);
                                    }
                                }
                            }
                            else
                            {
                                if (dtProfileGeneric.Rows.Count == Convert.ToInt32(lastValue.Substring(ptr + 2, 2), 16))
                                {
                                    ptr += 4;
                                    for (int i = 0; i < dtProfileGeneric.Rows.Count; i++)
                                    {
                                        string innerValue = parse.GetProfileDataString(lastValue, ref ptr);
                                        if (!string.IsNullOrEmpty(innerValue))
                                            input_Break_Profile.Add(innerValue);
                                    }
                                }
                            }
                            input_Break_Final = input_Break_Initial.GetRange(0, input_Break_Initial.Count - 1);
                            input_Break_Final.AddRange(input_Break_Profile);
                        }
                    }
                }
                input_Break_Final = (input_Break_Profile.Count > 0) ? input_Break_Final : input_Break_Initial;
            }
            catch (Exception ex)
            {
                log.Error($"[{nameof(GetDataFromDecryptedPacket)}] - {ex.Message} TRACE:{ex.StackTrace}");
            }
        }
        /// <summary>
        /// Builds and updates the target DataTable for the current push profile using the raw push data.  
        /// It selects or initializes the appropriate DataTable structure depending on the push profile adds new "Push Data" and "Push Value" columns identified by the current push packet,
        /// and fills them with parsed values extracted from the raw push data.  
        /// For Instant pushes, additional parsing logic is applied to handle nested data structures.  
        /// The resulting DataTable is cached in the corresponding profile-specific DataTable field for reuse.
        /// </summary>
        /// <param name="dataPacket">The original push packet string, used to identify the push instance.</param>
        /// <param name="rawPushData">A DataTable containing raw push data parsed from the decrypted packet.</param>
        /// <returns>A populated DataTable containing structured push data with appended "Push Data" and "Push Value" columns.</returns>
        public DataTable BuildTargetTableFromPushData(string dataPacket, DataTable rawPushData)
        {
            DataTable targetTable = new DataTable();

            switch (pushProfile)
            {
                case "Instant":
                    if (isInstantProfile)
                        targetTable = (dtRec_Push_Instant.Columns.Count == 0) ? dt_Appended_Instant.Copy() : dtRec_Push_Instant.Copy();
                    else
                        targetTable = (dtRec_Push_Instant.Columns.Count == 0) ? dtPushSetup_Instant.Copy() : dtRec_Push_Instant.Copy();
                    break;
                case "Alert":
                    targetTable = (dtRec_Push_Alert.Columns.Count == 0) ? dtPushSetup_Alert.Copy() : dtRec_Push_Alert.Copy();
                    break;
                case "LS":
                    targetTable = (dtRec_Push_LS.Columns.Count == 0) ? dt_Appended_LS.Copy() : dtRec_Push_LS.Copy();
                    break;
                case "DE":
                    targetTable = (dtRec_Push_DE.Columns.Count == 0) ? dt_Appended_DE.Copy() : dtRec_Push_DE.Copy();
                    break;
                case "SR":
                    targetTable = (dtRec_Push_SR.Columns.Count == 0) ? dtPushSetup_SR.Copy() : dtRec_Push_SR.Copy();
                    break;
                case "Billing":
                    targetTable = (dtRec_Push_Bill.Columns.Count == 0) ? dt_Appended_Bill.Copy() : dtRec_Push_Bill.Copy();
                    break;
                case "Tamper":
                    targetTable = (dtRec_Push_Tamper.Columns.Count == 0) ? dtPushSetup_Tamper.Copy() : dtRec_Push_Tamper.Copy();
                    break;
                case "Current Bill":
                    targetTable = (dtRec_Push_CB.Columns.Count == 0) ? dt_Appended_Bill.Copy() : dtRec_Push_CB.Copy();
                    break;
                default:
                    return targetTable; // If multiple packets received
            }
            try
            {
                string pushIdentifier = dataPacket.Split('-')[0];
                if (pushProfile == "LS")
                    FillRawDataIntoTargetTable(pushIdentifier, rawPushData, targetTable);
                else
                {
                    string columnNameData = $"Push Data-{pushIdentifier}";
                    string columnNameValue = $"Push Value-{pushIdentifier}";
                    targetTable.Columns.Add(columnNameData, typeof(string));
                    targetTable.Columns.Add(columnNameValue, typeof(string));

                    int targetTableRowIndex = 0;
                    int startRow = 3;
                    int rawPushDataCount = rawPushData.Rows.Count;
                    int targetTableRowCount = targetTable.Rows.Count;

                    for (int i = startRow; i < rawPushDataCount; i++, targetTableRowIndex++)
                    {
                        targetTable.Rows[targetTableRowIndex][columnNameData] = rawPushData.Rows[i][0].ToString();
                        targetTable.Rows[targetTableRowIndex][columnNameValue] = parse.GetProfileValueString(rawPushData.Rows[i][0].ToString());
                    }
                    if (pushProfile == "Instant")
                    {
                        if (DataTableOperations.IsStringInColumn(targetTable, "OBIS", "0.0.96.12.131.255"))
                        {
                            int lastRowIndex = DataTableOperations.GetRowIndexByColumnValue(targetTable, "OBIS", "0.0.96.12.131.255");
                            if (lastRowIndex != -1)
                            {
                                List<string> splittedDataStructure = new List<string>();
                                List<string> convertedSplittedData = new List<string>();
                                string existingData = targetTable.Rows[lastRowIndex][columnNameData].ToString();
                                int start = 4;
                                for (int i = 0; i < Convert.ToInt32(existingData.Substring(2, 2), 16); i++)
                                {
                                    splittedDataStructure.Add(parse.GetProfileDataString(existingData, ref start));
                                    convertedSplittedData.Add(parse.GetProfileValueString(splittedDataStructure[i].ToString().Trim()));
                                }
                                targetTable.Rows[lastRowIndex][columnNameValue] = string.Join(", ", convertedSplittedData);
                            }
                        }
                    }
                }
                switch (pushProfile)
                {
                    case "Instant":
                        dtRec_Push_Instant = targetTable;
                        //print_results(targetTable.Copy());
                        break;
                    case "Alert":
                        dtRec_Push_Alert = targetTable;
                        //print_results(targetTable.Copy());
                        break;
                    case "LS":
                        dtRec_Push_LS = targetTable;
                        //print_results(targetTable.Copy());
                        break;
                    case "DE":
                        dtRec_Push_DE = targetTable;
                        //print_results(targetTable.Copy());
                        break;
                    case "SR":
                        dtRec_Push_SR = targetTable;
                        //print_results(targetTable.Copy());
                        break;
                    case "Billing":
                        dtRec_Push_Bill = targetTable;
                        //print_results(targetTable.Copy());
                        break;
                    case "Current Bill":
                        dtRec_Push_CB = targetTable;
                        //print_results(targetTable.Copy());
                        break;
                    case "Tamper":
                        dtRec_Push_Tamper = targetTable;
                        //print_results(targetTable.Copy());
                        break;
                }
            }
            catch (Exception ex)
            {
                log.Error($"[{nameof(BuildTargetTableFromPushData)}] - {ex.Message} TRACE:{ex.StackTrace}");
            }
            return targetTable;
        }
        #endregion

        #region Data Handling for LS
        /// <summary>
        /// Transpose received datatable only for Load Survey
        /// </summary>
        /// <param name="originalTable"></param>
        /// <returns></returns>
        public static DataTable TransposeDataTable(DataTable originalTable)
        {
            DataTable transposedTable = new DataTable();
            transposedTable.Columns.Add(" ");
            for (int i = 0; i < originalTable.Rows.Count; i++)
            {
                transposedTable.Columns.Add($"Row{i + 1}");
            }
            for (int col = 0; col < originalTable.Columns.Count; col++)
            {
                DataRow newRow = transposedTable.NewRow();
                newRow[0] = originalTable.Columns[col].ColumnName;

                for (int row = 0; row < originalTable.Rows.Count; row++)
                {
                    newRow[row + 1] = originalTable.Rows[row][col];
                }

                transposedTable.Rows.Add(newRow);
            }
            return transposedTable;
        }
        private void FillRawDataIntoTargetTable(string pushIdentifier, DataTable rawPushData, DataTable targetTable)
        {
            try
            {
                int startCol = dtPushSetup_LS.Rows.Count + 1;
                int colCount = targetTable.Columns.Count;
                int currentRow = targetTable.Rows.Count;
                int currentCol = startCol; targetTable.Rows.Add();
                targetTable.Rows[currentRow][0] = pushIdentifier;
                for (int i = 1; i < startCol; i++)
                {
                    targetTable.Rows[currentRow][i] = parse.GetProfileValueString(rawPushData.Rows[i + 2][0].ToString());
                }
                for (int i = startCol + 2; i < rawPushData.Rows.Count; i++)
                {
                    DataRow entriesRow = rawPushData.Rows[i];
                    if (targetTable.Rows.Count <= currentRow)
                    {
                        targetTable.Rows.Add(targetTable.NewRow());
                    }
                    targetTable.Rows[currentRow][currentCol] = parse.GetProfileValueString(entriesRow[0].ToString());
                    currentCol++;
                    if (currentCol >= colCount)
                    {
                        currentCol = startCol;
                        currentRow++;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"[{nameof(FillRawDataIntoTargetTable)}] - {ex.Message} TRACE:{ex.StackTrace}");
            }
        }
        #endregion

        #region Validation & Verification Methods
        /// <summary>
        /// Updates the list for all the expected packet for different profiles in a designated time period 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="logBox"></param>
        public Dictionary<string, List<DateTime>> GetExpectedPacketCountAndTimeStamp(DateTime startDate, DateTime endDate, RichTextBox logBox)
        {
            var ExpectedPackets = new Dictionary<string, List<DateTime>>(StringComparer.OrdinalIgnoreCase);
            //List<List<DateTime>> ExpectedPackets = new List<List<DateTime>>();
            lst_ExpectedPacketTiming_Instant.Clear(); lst_ExpectedPacketTiming_LS.Clear(); lst_ExpectedPacketTiming_DE.Clear(); lst_ExpectedPacketTiming_SR.Clear(); lst_ExpectedPacketTiming_Bill.Clear();
            lst_ExpectedPacketTiming_CB.Clear();
            _logService.LogMessage(logBox, $"\nExpected Packets Timings:", Color.Brown, true);
            DateTime dayStart = startDate.Date;
            double minutesSinceMidnight = (startDate - dayStart).TotalMinutes;
            try
            {
                if (TestProfiles == "All" || TestProfiles == "Instant")
                {
                    if (freq_Instant > 0)
                    {
                        double nextMultiple = (Math.Ceiling(minutesSinceMidnight / freq_Instant) * freq_Instant);
                        DateTime firstPacketTime = dayStart.AddMinutes(nextMultiple);
                        DateTime current = firstPacketTime;
                        if (Random_Instant != 0)
                        {
                            current = current.AddMinutes(Random_Instant);
                        }
                        while (current <= endDate)
                        {
                            lst_ExpectedPacketTiming_Instant.Add(current);
                            current = current.AddMinutes(freq_Instant);
                        }
                    }
                }
                if (TestProfiles == "All" || TestProfiles == "LS")
                {
                    if (freq_LS > 0)
                    {
                        //DLMSComm dlmsReader = new DLMSComm(DLMSInfo.comPort, DLMSInfo.BaudRate);
                        //dlmsReader.SignOnDLMS();
                        //int currentLSIP = (Convert.ToInt32(parse.GetProfileValueString(SetGetFromMeter.GetDataFromObject(ref dlmsReader, 1, "1.0.0.8.4.255", 2))) / 60);
                        //dlmsReader.Dispose();
                        double nextMultiple = (Math.Ceiling(minutesSinceMidnight / freq_LS) * freq_LS);
                        //int minutes = (currentLSIP > freq_LS) ? currentLSIP : freq_LS;
                        // double nextMultiple = (Math.Ceiling(minutesSinceMidnight / minutes) * minutes);
                        DateTime firstPacketTime = dayStart.AddMinutes(nextMultiple);
                        DateTime current = firstPacketTime;
                        if (Random_LS != 0)
                        {
                            current = current.AddMinutes(Random_LS);
                        }
                        while (current <= endDate)
                        {
                            lst_ExpectedPacketTiming_LS.Add(current);
                            current = current.AddMinutes(freq_LS);
                            //current = current.AddMinutes(minutes);
                        }
                    }
                }
                if (TestProfiles == "All" || TestProfiles == "DE")
                {
                    if (freq_DE > 0)
                    {
                        double nextMultiple = (Math.Ceiling(minutesSinceMidnight / freq_DE) * freq_DE);
                        DateTime firstPacketTime = dayStart.AddMinutes(nextMultiple);

                        DateTime current = firstPacketTime;
                        if (Random_DE != 0)
                        {
                            current = current.AddMinutes(Random_DE);
                        }
                        while (current <= endDate)
                        {
                            lst_ExpectedPacketTiming_DE.Add(current);
                            current = current.AddMinutes(freq_DE);
                        }
                    }
                }
                if (TestProfiles == "All" || TestProfiles == "SR")
                {
                    if (freq_SR > 0)
                    {
                        double nextMultiple = (Math.Ceiling(minutesSinceMidnight / freq_SR) * freq_SR);
                        DateTime firstPacketTime = dayStart.AddMinutes(nextMultiple);

                        DateTime current = firstPacketTime;
                        while (current <= endDate)
                        {
                            lst_ExpectedPacketTiming_SR.Add(current);
                            current = current.AddMinutes(freq_SR);
                        }
                    }
                }
                if (TestProfiles == "All" || TestProfiles == "Bill")
                {
                    DateTime firstPacketTime = new DateTime(startDate.Year, startDate.Month, int.Parse(freq_Bill.Substring(0, 2)), int.Parse(freq_Bill.Substring(7, 2)), int.Parse(freq_Bill.Substring(10, 2)), int.Parse(freq_Bill.Substring(13, 2)));
                    if (firstPacketTime < startDate)
                    {
                        firstPacketTime = firstPacketTime.AddMonths(1);
                    }
                    DateTime current = firstPacketTime;
                    if (Random_Bill != 0)
                    {
                        current = current.AddMinutes(Random_Bill);
                    }
                    while (startDate <= current && current <= endDate)
                    {
                        lst_ExpectedPacketTiming_Bill.Add(current);
                        current = current.AddMonths(1);
                    }
                }
                if (TestProfiles == "All" || TestProfiles == "Current Bill")
                {
                    if (freq_CB > 0 && isCurrentBillAvailable)
                    {
                        double nextMultiple = (Math.Ceiling(minutesSinceMidnight / freq_CB) * freq_CB);
                        DateTime firstPacketTime = dayStart.AddMinutes(nextMultiple);
                        DateTime current = firstPacketTime;
                        if (Random_CB != 0)
                        {
                            current = current.AddMinutes(Random_CB);
                        }
                        while (current <= endDate)
                        {
                            lst_ExpectedPacketTiming_CB.Add(current);
                            current = current.AddMinutes(freq_CB);
                        }
                    }
                }

                ExpectedPackets["Instant"] = lst_ExpectedPacketTiming_Instant ?? new List<DateTime>();
                ExpectedPackets["Load Survey"] = lst_ExpectedPacketTiming_LS ?? new List<DateTime>();
                ExpectedPackets["Daily Energy"] = lst_ExpectedPacketTiming_DE ?? new List<DateTime>();
                ExpectedPackets["Self Registration"] = lst_ExpectedPacketTiming_SR ?? new List<DateTime>();
                ExpectedPackets["Billing"] = lst_ExpectedPacketTiming_Bill ?? new List<DateTime>();
                ExpectedPackets["Current Bill"] = lst_ExpectedPacketTiming_CB ?? new List<DateTime>();

                List<string> expectedCounts = new List<string>
                {
                    lst_ExpectedPacketTiming_Instant.Count.ToString(),
                    lst_ExpectedPacketTiming_LS.Count.ToString(),
                    lst_ExpectedPacketTiming_DE.Count.ToString(),
                    lst_ExpectedPacketTiming_SR.Count.ToString(),
                    lst_ExpectedPacketTiming_Bill.Count.ToString()
                };
                List<string> profiles = new List<string> { "Instant", "Load Survey", "Daily Energy", "Self Registration", "Billing" };
                if (isCurrentBillAvailable)
                {
                    profiles.Add("Current Bill");
                    expectedCounts.Add(lst_ExpectedPacketTiming_CB.Count.ToString());
                }
                _logService.LogMessage(logBox, _logService.FormatLogLineString("\tPush Profiles", profiles.ToArray()), Color.Black, true);
                _logService.LogMessage(logBox, _logService.FormatLogLineString("\tExpected Packet Count", expectedCounts.ToArray()), Color.Green, true);
                int max = new[]
                {
                    lst_ExpectedPacketTiming_Instant.Count,lst_ExpectedPacketTiming_LS.Count,
                    lst_ExpectedPacketTiming_DE.Count,lst_ExpectedPacketTiming_SR.Count,
                    lst_ExpectedPacketTiming_Bill.Count,lst_ExpectedPacketTiming_CB.Count
                }.Max();
                for (int i = 0; i < max; i++)
                {
                    try
                    {
                        List<string> timings = new List<string>
                        {
                            lst_ExpectedPacketTiming_Instant.Count > i ? lst_ExpectedPacketTiming_Instant[i].ToString(Constants.timeStamp12Hours) : "-",
                            lst_ExpectedPacketTiming_LS.Count > i ? lst_ExpectedPacketTiming_LS[i].ToString(Constants.timeStamp12Hours) : "-",
                            lst_ExpectedPacketTiming_DE.Count > i ? lst_ExpectedPacketTiming_DE[i].ToString(Constants.timeStamp12Hours) : "-",
                            lst_ExpectedPacketTiming_SR.Count > i ? lst_ExpectedPacketTiming_SR[i].ToString(Constants.timeStamp12Hours) : "-",
                            lst_ExpectedPacketTiming_Bill.Count > i ? lst_ExpectedPacketTiming_Bill[i].ToString(Constants.timeStamp12Hours) : "-"
                        };
                        if (isCurrentBillAvailable && lst_ExpectedPacketTiming_CB.Count > 0)
                            timings.Add(lst_ExpectedPacketTiming_CB.Count > i ? lst_ExpectedPacketTiming_CB[i].ToString(Constants.timeStamp12Hours) : "-");
                        _logService.LogMessage(logBox, _logService.FormatLogLineString($"\t{i + 1}. Packet Expected Timing", timings.ToArray()), Color.Black);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"[{nameof(GetExpectedPacketCountAndTimeStamp)}] - {ex.Message} TRACE:{ex.StackTrace}");
            }
            return ExpectedPackets;
        }
        /// <summary>
        /// Extracts the profile wise packets with timestamps from raw data 
        /// </summary>
        /// <param name="finalDataTable"></param>
        /// <param name="logBox"></param>
        public Dictionary<string, List<DateTime>> GetReceivedPacketCountAndTimeStamp(DataTable finalDataTable, RichTextBox logBox)
        {
            var ReceivedPackets = new Dictionary<string, List<DateTime>>(StringComparer.OrdinalIgnoreCase);
            //List<List<DateTime>> ReceivedPackets = new List<List<DateTime>>();
            lst_ReceivedPacketTiming_Instant.Clear(); lst_ReceivedPacketTiming_LS.Clear(); lst_ReceivedPacketTiming_DE.Clear(); lst_ReceivedPacketTiming_CB.Clear();
            lst_ReceivedPacketTiming_SR.Clear(); lst_ReceivedPacketTiming_Bill.Clear(); lst_ReceivedPacketTiming_Alert.Clear(); lst_ReceivedPacketTiming_Tamper.Clear();
            _logService.LogMessage(logBox, $"\nReceived Packets Timings:", Color.Brown, true);
            if (finalDataTable == null || finalDataTable.Rows.Count == 0)
            {
                _logService.LogMessage(logBox, $"\tNo push packets were received for any profile.", Color.Red, true);
                return ReceivedPackets;
            }
            try
            {
                foreach (DataRow row in finalDataTable.Rows)
                {
                    string profileType = row[0]?.ToString();
                    string recPacketTimeStamp = row[1]?.ToString()?.Trim();

                    if (string.IsNullOrWhiteSpace(profileType) || string.IsNullOrWhiteSpace(recPacketTimeStamp))
                        continue;

                    DateTime timing;
                    if (!DateTime.TryParseExact(recPacketTimeStamp, Constants.timeStamp12Hours, CultureInfo.InvariantCulture, DateTimeStyles.None, out timing))
                        continue;

                    //if (TestProfiles == "All" || TestProfiles == "Instant")
                    //{
                    if (profileType.IndexOf("Instant", StringComparison.OrdinalIgnoreCase) >= 0 && profileType.IndexOf(DeviceID, StringComparison.OrdinalIgnoreCase) >= 0)
                        lst_ReceivedPacketTiming_Instant.Add(timing);
                    //}
                    //if (TestProfiles == "All" || TestProfiles == "LS")
                    //{
                    if (profileType.IndexOf("Load Survey", StringComparison.OrdinalIgnoreCase) >= 0 && profileType.IndexOf(DeviceID, StringComparison.OrdinalIgnoreCase) >= 0)
                        lst_ReceivedPacketTiming_LS.Add(timing);
                    //}
                    //if (TestProfiles == "All" || TestProfiles == "DE")
                    //{
                    if (profileType.IndexOf("Daily Energy", StringComparison.OrdinalIgnoreCase) >= 0 && profileType.IndexOf(DeviceID, StringComparison.OrdinalIgnoreCase) >= 0)
                        lst_ReceivedPacketTiming_DE.Add(timing);
                    //}
                    //if (TestProfiles == "All" || TestProfiles == "SR")
                    //{
                    if (profileType.IndexOf("Self Registration", StringComparison.OrdinalIgnoreCase) >= 0 && profileType.IndexOf(DeviceID, StringComparison.OrdinalIgnoreCase) >= 0)
                        lst_ReceivedPacketTiming_SR.Add(timing);
                    //}
                    //if (TestProfiles == "All" || TestProfiles == "Bill")
                    //{
                    if (profileType.IndexOf("Billing", StringComparison.OrdinalIgnoreCase) >= 0 && profileType.IndexOf(DeviceID, StringComparison.OrdinalIgnoreCase) >= 0)
                        lst_ReceivedPacketTiming_Bill.Add(timing);
                    //}
                    //if (TestProfiles == "All" || TestProfiles == "Alert")
                    //{
                    if (profileType.IndexOf("Alert", StringComparison.OrdinalIgnoreCase) >= 0 && profileType.IndexOf(DeviceID, StringComparison.OrdinalIgnoreCase) >= 0)
                        lst_ReceivedPacketTiming_Alert.Add(timing);
                    //}
                    //if (TestProfiles == "All" || TestProfiles == "Current Bill")
                    //{
                    if (profileType.IndexOf("Current Bill", StringComparison.OrdinalIgnoreCase) >= 0 && profileType.IndexOf(DeviceID, StringComparison.OrdinalIgnoreCase) >= 0)
                        lst_ReceivedPacketTiming_CB.Add(timing);
                    //}
                    //if (TestProfiles == "All" || TestProfiles == "Tamper")
                    //{
                    if (profileType.IndexOf("Tamper", StringComparison.OrdinalIgnoreCase) >= 0 && profileType.IndexOf(DeviceID, StringComparison.OrdinalIgnoreCase) >= 0)
                        lst_ReceivedPacketTiming_Tamper.Add(timing);
                    //}
                }
                /*
                if (lst_ReceivedPacketTiming_Instant.Count > 0) ReceivedPackets.Add(lst_ReceivedPacketTiming_Instant);
                if (lst_ReceivedPacketTiming_LS.Count > 0) ReceivedPackets.Add(lst_ReceivedPacketTiming_LS);
                if (lst_ReceivedPacketTiming_DE.Count > 0) ReceivedPackets.Add(lst_ReceivedPacketTiming_DE);
                if (lst_ReceivedPacketTiming_SR.Count > 0) ReceivedPackets.Add(lst_ReceivedPacketTiming_SR);
                if (lst_ReceivedPacketTiming_Bill.Count > 0) ReceivedPackets.Add(lst_ReceivedPacketTiming_Bill);
                if (lst_ReceivedPacketTiming_Alert.Count > 0) ReceivedPackets.Add(lst_ReceivedPacketTiming_Alert);
                if (lst_ReceivedPacketTiming_Tamper.Count > 0) ReceivedPackets.Add(lst_ReceivedPacketTiming_Tamper);
                if (lst_ReceivedPacketTiming_CB.Count > 0) ReceivedPackets.Add(lst_ReceivedPacketTiming_CB);
                */
                ReceivedPackets["Instant"] = lst_ReceivedPacketTiming_Instant ?? new List<DateTime>();
                ReceivedPackets["Load Survey"] = lst_ReceivedPacketTiming_LS ?? new List<DateTime>();
                ReceivedPackets["Daily Energy"] = lst_ReceivedPacketTiming_DE ?? new List<DateTime>();
                ReceivedPackets["Self Registration"] = lst_ReceivedPacketTiming_SR ?? new List<DateTime>();
                ReceivedPackets["Billing"] = lst_ReceivedPacketTiming_Bill ?? new List<DateTime>();
                ReceivedPackets["Current Bill"] = lst_ReceivedPacketTiming_CB ?? new List<DateTime>();

                // _logService.LogMessage(logBox, $"\nReceived Packets Timings:", Color.Brown, true);
                List<string> expectedCounts = new List<string>
                {
                    lst_ExpectedPacketTiming_Instant.Count.ToString(),
                    lst_ExpectedPacketTiming_LS.Count.ToString(),
                    lst_ExpectedPacketTiming_DE.Count.ToString(),
                    lst_ExpectedPacketTiming_SR.Count.ToString(),
                    lst_ExpectedPacketTiming_Bill.Count.ToString()
                };
                List<string> receivedCounts = new List<string>
                {
                    lst_ReceivedPacketTiming_Instant.Count.ToString(),
                    lst_ReceivedPacketTiming_LS.Count.ToString(),
                    lst_ReceivedPacketTiming_DE.Count.ToString(),
                    lst_ReceivedPacketTiming_SR.Count.ToString(),
                    lst_ReceivedPacketTiming_Bill.Count.ToString()
                };
                List<string> profiles = new List<string> { "Instant", "Load Survey", "Daily Energy", "Self Registration", "Billing" };
                if (lst_ReceivedPacketTiming_Alert.Count > 0)
                {
                    profiles.Add("Alert");
                    receivedCounts.Add(lst_ReceivedPacketTiming_Alert.Count.ToString());
                }
                if (lst_ReceivedPacketTiming_Tamper.Count > 0)
                {
                    profiles.Add("Tamper");
                    receivedCounts.Add(lst_ReceivedPacketTiming_Tamper.Count.ToString());
                }
                if (lst_ReceivedPacketTiming_CB.Count > 0)
                {
                    profiles.Add("Current Bill");
                    receivedCounts.Add(lst_ReceivedPacketTiming_CB.Count.ToString());
                }
                _logService.LogMessage(logBox, _logService.FormatLogLineString("\tProfiles", profiles.ToArray()), Color.Black, true);
                _logService.LogMessage(logBox, _logService.FormatLogLineString("\tExpected Packet Count", expectedCounts.ToArray()), Color.Green, true);
                _logService.LogMessage(logBox, _logService.FormatLogLineString("\tReceived Packet Count", receivedCounts.ToArray()), Color.Green, true);

                int max = new[]
                {
                    lst_ReceivedPacketTiming_Instant.Count,lst_ReceivedPacketTiming_LS.Count,
                    lst_ReceivedPacketTiming_DE.Count,lst_ReceivedPacketTiming_SR.Count,
                    lst_ReceivedPacketTiming_Bill.Count,lst_ReceivedPacketTiming_Alert.Count,
                    lst_ReceivedPacketTiming_Tamper.Count,lst_ReceivedPacketTiming_CB.Count
                }.Max();

                for (int i = 0; i < max; i++)
                {
                    try
                    {
                        List<string> timings = new List<string>
                        {
                            lst_ReceivedPacketTiming_Instant.Count > i ? lst_ReceivedPacketTiming_Instant[i].ToString(Constants.timeStamp12Hours) : "-",
                            lst_ReceivedPacketTiming_LS.Count > i ? lst_ReceivedPacketTiming_LS[i].ToString(Constants.timeStamp12Hours) : "-",
                            lst_ReceivedPacketTiming_DE.Count > i ? lst_ReceivedPacketTiming_DE[i].ToString(Constants.timeStamp12Hours) : "-",
                            lst_ReceivedPacketTiming_SR.Count > i ? lst_ReceivedPacketTiming_SR[i].ToString(Constants.timeStamp12Hours) : "-",
                            lst_ReceivedPacketTiming_Bill.Count > i ? lst_ReceivedPacketTiming_Bill[i].ToString(Constants.timeStamp12Hours) : "-"
                        };
                        if (lst_ReceivedPacketTiming_Alert.Count > 0)
                            timings.Add(lst_ReceivedPacketTiming_Alert.Count > i ? lst_ReceivedPacketTiming_Alert[i].ToString(Constants.timeStamp12Hours) : "-");
                        if (lst_ReceivedPacketTiming_Tamper.Count > 0)
                            timings.Add(lst_ReceivedPacketTiming_Tamper.Count > i ? lst_ReceivedPacketTiming_Tamper[i].ToString(Constants.timeStamp12Hours) : "-");
                        if (lst_ReceivedPacketTiming_CB.Count > 0)
                            timings.Add(lst_ReceivedPacketTiming_CB.Count > i ? lst_ReceivedPacketTiming_CB[i].ToString(Constants.timeStamp12Hours) : "-");

                        _logService.LogMessage(logBox, _logService.FormatLogLineString($"\t{i + 1}. Packet Received Timing", timings.ToArray()), Color.Black);

                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"[{nameof(GetReceivedPacketCountAndTimeStamp)}] - {ex.Message} TRACE:{ex.StackTrace}");
            }
            return ReceivedPackets;
        }
        /// <summary>
        /// Returns Passed or Missed packets Timing
        /// </summary>
        /// <param name="lst_ExpectedPacketTiming_Instant"></param>
        /// <param name="lst_ReceivedPacketTiming_Instant"></param>
        /// <returns></returns>
        /*public DataTable ValidateReceivedPacketTimings1(string Profile, List<DateTime> List_ExpectedTimeStamp, List<DateTime> List_ReceivedTimeStamp)
        {
            DataTable dtvalidateMissedPacket = new DataTable();
            dtvalidateMissedPacket.Columns.Add("Profile", typeof(string));
            dtvalidateMissedPacket.Columns.Add("Expected Packet Time", typeof(string));
            dtvalidateMissedPacket.Columns.Add("Received Packet Time", typeof(string));
            dtvalidateMissedPacket.Columns.Add("Result", typeof(string));

            try
            {
                if (List_ExpectedTimeStamp == null || List_ExpectedTimeStamp.Count == 0)
                {
                    if (List_ReceivedTimeStamp.Count == 0)
                    {
                        dtvalidateMissedPacket.Rows.Add(Profile, "-", "-", "No expected packet.");
                        return dtvalidateMissedPacket;
                    }
                    else if (List_ReceivedTimeStamp.Count >= 1)
                    {
                        for (int i = 0; i < List_ReceivedTimeStamp.Count; i++)
                        {
                            string result = i == 0 ? "Additional Packet" : "Unexpected Packet";
                            dtvalidateMissedPacket.Rows.Add(Profile, "-", List_ReceivedTimeStamp[i].ToString(), result);
                        }
                        return dtvalidateMissedPacket;
                    }

                }
                if (List_ReceivedTimeStamp == null)
                    List_ReceivedTimeStamp = new List<DateTime>();
                // 1. Ensure received packets are in increasing order
                for (int i = 1; i < List_ReceivedTimeStamp.Count; i++)
                {
                    if (List_ReceivedTimeStamp[i] < List_ReceivedTimeStamp[i - 1])
                    {
                        dtvalidateMissedPacket.Rows.Add(Profile, "-", "-", "Error: Received packet timings are not in increasing order.");
                        return dtvalidateMissedPacket;
                    }
                }
                int expectedCount = List_ExpectedTimeStamp.Count;
                int receivedCount = List_ReceivedTimeStamp.Count;
                // 2. Special case: only 1 expected timestamp
                if (expectedCount == 1)
                {
                    DateTime expectedTime = List_ExpectedTimeStamp[0];
                    bool matched = false;
                    bool firstEarlyHandled = false;
                    foreach (var received in List_ReceivedTimeStamp)
                    {
                        if (received >= expectedTime)
                        {
                            dtvalidateMissedPacket.Rows.Add(Profile, expectedTime.ToString(Constants.timeStamp12Hours), received.ToString(Constants.timeStamp12Hours), "Passed");
                            matched = true;
                        }
                        else if (!firstEarlyHandled)
                        {
                            dtvalidateMissedPacket.Rows.Add(Profile, expectedTime.ToString(Constants.timeStamp12Hours), received.ToString(Constants.timeStamp12Hours), "Additional Packet");
                            firstEarlyHandled = true;
                        }
                        else
                        {
                            dtvalidateMissedPacket.Rows.Add(Profile, expectedTime.ToString(Constants.timeStamp12Hours), received.ToString(Constants.timeStamp12Hours), "Extra Packet");
                        }
                    }

                    if (!matched)
                    {
                        dtvalidateMissedPacket.Rows.Add(Profile, expectedTime.ToString(Constants.timeStamp12Hours), "-", "Missed Packet");
                    }

                    return dtvalidateMissedPacket;
                }
                // 3. General case
                int receivedIndex = 0;
                for (int i = 0; i < expectedCount - 1; i++)
                {
                    DateTime intervalStart = List_ExpectedTimeStamp[i];
                    DateTime intervalEnd = List_ExpectedTimeStamp[i + 1];
                    bool found = false;

                    while (receivedIndex < receivedCount)
                    {
                        DateTime currentReceived = List_ReceivedTimeStamp[receivedIndex];

                        if (currentReceived < intervalStart)
                        {
                            dtvalidateMissedPacket.Rows.Add(Profile, intervalStart.ToString(Constants.timeStamp12Hours), currentReceived.ToString(Constants.timeStamp12Hours), "Additional Packet");
                            receivedIndex++;
                            continue;
                        }

                        if (currentReceived >= intervalStart && currentReceived < intervalEnd)
                        {
                            dtvalidateMissedPacket.Rows.Add(Profile, intervalStart.ToString(Constants.timeStamp12Hours), currentReceived.ToString(Constants.timeStamp12Hours), "Passed");
                            receivedIndex++;
                            found = true;
                            break;
                        }
                        if (currentReceived >= intervalEnd)
                        {
                            break; // Exit to mark as missed outside
                        }

                        receivedIndex++;
                    }

                    if (!found)
                    {
                        dtvalidateMissedPacket.Rows.Add(Profile, intervalStart.ToString(Constants.timeStamp12Hours), "-", "Missed Packet");
                    }
                }
                // 4. Final expected timestamp check
                DateTime finalExpected = List_ExpectedTimeStamp[expectedCount - 1];
                bool finalMatched = false;
                while (receivedIndex < receivedCount)
                {
                    DateTime currentReceived = List_ReceivedTimeStamp[receivedIndex];

                    if (currentReceived >= finalExpected)
                    {
                        dtvalidateMissedPacket.Rows.Add(Profile, finalExpected.ToString(Constants.timeStamp12Hours), currentReceived.ToString(Constants.timeStamp12Hours), "Passed");
                        receivedIndex++;
                        finalMatched = true;
                        break;
                    }
                    else
                    {
                        dtvalidateMissedPacket.Rows.Add(Profile, finalExpected.ToString(Constants.timeStamp12Hours), currentReceived.ToString(Constants.timeStamp12Hours), "Additional Packet");
                        receivedIndex++;
                    }
                }
                if (!finalMatched)
                {
                    dtvalidateMissedPacket.Rows.Add(Profile, finalExpected.ToString(Constants.timeStamp12Hours), "-", "Missed Packet");
                }
                // 5. Handle any remaining additional packets
                while (receivedIndex < receivedCount)
                {
                    dtvalidateMissedPacket.Rows.Add(Profile, "-", List_ReceivedTimeStamp[receivedIndex].ToString(Constants.timeStamp12Hours), "Additional Packet");
                    receivedIndex++;
                }
            }
            catch (Exception ex)
            {
                log.Error($"[{nameof(ValidateReceivedPacketTimings)}] - {ex.Message}", ex);
            }
            return dtvalidateMissedPacket;
        }
        */
        public DataTable ValidateReceivedPacketTimings(string Profile, List<DateTime> List_ExpectedTimeStamp, List<DateTime> List_ReceivedTimeStamp)
        {
            DataTable dtvalidateMissedPacket = new DataTable();
            dtvalidateMissedPacket.Columns.Add("Profile", typeof(string));
            dtvalidateMissedPacket.Columns.Add("Expected Packet Time", typeof(string));
            dtvalidateMissedPacket.Columns.Add("Received Packet Time", typeof(string));
            dtvalidateMissedPacket.Columns.Add("Result", typeof(string));

            try
            {
                if (List_ExpectedTimeStamp == null || List_ExpectedTimeStamp.Count == 0)
                {
                    if (List_ReceivedTimeStamp.Count == 0)
                    {
                        dtvalidateMissedPacket.Rows.Add(Profile, "-", "-", "No expected packet.");
                        return dtvalidateMissedPacket;
                    }
                    else if (List_ReceivedTimeStamp.Count >= 1)
                    {
                        for (int i = 0; i < List_ReceivedTimeStamp.Count; i++)
                        {
                            string result = i == 0 ? "Additional Packet" : "Unexpected Packet";
                            dtvalidateMissedPacket.Rows.Add(Profile, "-", List_ReceivedTimeStamp[i].ToString(Constants.timeStamp12Hours), result);
                        }
                        return dtvalidateMissedPacket;
                    }
                }
                if (List_ReceivedTimeStamp == null)
                    List_ReceivedTimeStamp = new List<DateTime>();

                // 1. Ensure received packets are in increasing order
                for (int i = 1; i < List_ReceivedTimeStamp.Count; i++)
                {
                    if (List_ReceivedTimeStamp[i] < List_ReceivedTimeStamp[i - 1])
                    {
                        dtvalidateMissedPacket.Rows.Add(Profile, "-", "-", "Error: Received packet timings are not in increasing order.");
                        return dtvalidateMissedPacket;
                    }
                }

                int expectedCount = List_ExpectedTimeStamp.Count;
                int receivedCount = List_ReceivedTimeStamp.Count;

                int receivedIndex = 0;
                bool firstEarlyHandled = false; // Tracks if first early packet already logged

                // 2. Handle packets before the first expected timestamp
                DateTime firstExpected = List_ExpectedTimeStamp[0];
                while (receivedIndex < receivedCount && List_ReceivedTimeStamp[receivedIndex] < firstExpected)
                {
                    if (!firstEarlyHandled)
                    {
                        dtvalidateMissedPacket.Rows.Add(Profile, firstExpected.ToString(Constants.timeStamp12Hours),
                            List_ReceivedTimeStamp[receivedIndex].ToString(Constants.timeStamp12Hours), "Additional Packet");
                        firstEarlyHandled = true;
                    }
                    else
                    {
                        dtvalidateMissedPacket.Rows.Add(Profile, firstExpected.ToString(Constants.timeStamp12Hours),
                            List_ReceivedTimeStamp[receivedIndex].ToString(Constants.timeStamp12Hours), "Unexpected Packets");
                    }
                    receivedIndex++;
                }

                // 3. General case: process expected intervals
                for (int i = 0; i < expectedCount; i++)
                {
                    DateTime expectedTime = List_ExpectedTimeStamp[i];
                    DateTime? nextExpected = (i + 1 < expectedCount) ? List_ExpectedTimeStamp[i + 1] : (DateTime?)null;

                    bool matched = false;

                    while (receivedIndex < receivedCount)
                    {
                        DateTime currentReceived = List_ReceivedTimeStamp[receivedIndex];

                        if (nextExpected.HasValue && currentReceived >= nextExpected.Value)
                        {
                            break;
                        }

                        if (currentReceived >= expectedTime && (!nextExpected.HasValue || currentReceived < nextExpected.Value))
                        {
                            if (!matched)
                            {
                                dtvalidateMissedPacket.Rows.Add(Profile,
                                    expectedTime.ToString(Constants.timeStamp12Hours),
                                    currentReceived.ToString(Constants.timeStamp12Hours),
                                    "Passed");
                                matched = true;
                            }
                            else
                            {
                                dtvalidateMissedPacket.Rows.Add(Profile,
                                    expectedTime.ToString(Constants.timeStamp12Hours),
                                    currentReceived.ToString(Constants.timeStamp12Hours),
                                    "Unexpected Packets");
                            }
                        }
                        else if (currentReceived < expectedTime)
                        {
                            // Already handled early packets
                        }

                        receivedIndex++;
                    }

                    if (!matched)
                    {
                        dtvalidateMissedPacket.Rows.Add(Profile, expectedTime.ToString(Constants.timeStamp12Hours), "-", "Missed Packet");
                    }
                }

                while (receivedIndex < receivedCount)
                {
                    dtvalidateMissedPacket.Rows.Add(Profile, "-",
                        List_ReceivedTimeStamp[receivedIndex].ToString(Constants.timeStamp12Hours),
                        "Unexpected Packets");
                    receivedIndex++;
                }
            }
            catch (Exception ex)
            {
                log.Error($"[{nameof(ValidateReceivedPacketTimings)}] - {ex.Message} TRACE:{ex.StackTrace}");
            }

            return dtvalidateMissedPacket;
        }
        /// <summary>
        /// Creates a report for all profiles in a excel sheet for all the received data  
        /// </summary>
        /// <param name="logBox"></param>
        /// <param name="result"></param>
        public bool MissedPackedValidation()
        {
            bool isMissedPacketValidationPassed = true;
            _logService.LogMessage(logBox, _logService.FormatLogLineString("\t" + "-----------------------------------------------------------------------------------------------------------------"), Color.Black, true);
            _logService.LogMessage(logBox, _logService.FormatLogLineString("\tProfile", "Expected", "Received", "Result"), Color.Black, true);

            var profileChecks = new List<(string ProfileName, string DisplayName, List<DateTime> ExpectedTimings, List<DateTime> ReceivedTimings)>
            {
                ("Instant", "Instant", lst_ExpectedPacketTiming_Instant, lst_ReceivedPacketTiming_Instant),
                ("LS", "Load Survey", lst_ExpectedPacketTiming_LS, lst_ReceivedPacketTiming_LS),
                ("DE", "Daily Energy", lst_ExpectedPacketTiming_DE, lst_ReceivedPacketTiming_DE),
                ("SR", "Self Registration", lst_ExpectedPacketTiming_SR, lst_ReceivedPacketTiming_SR),
                ("Bill", "Billing", lst_ExpectedPacketTiming_Bill, lst_ReceivedPacketTiming_Bill)
            };
            if (lst_ReceivedPacketTiming_CB.Count > 0)
            {
                profileChecks.Add(("Current Bill", "Current Bill", lst_ExpectedPacketTiming_CB, lst_ReceivedPacketTiming_CB));
            }
            try
            {
                foreach (var (profile, displayName, expected, received) in profileChecks)
                {
                    if (TestProfiles == "All" || (TestProfiles == profile) || (received.Count > 0))
                    {
                        var dtmissedPackets = ValidateReceivedPacketTimings(displayName, expected, received);
                        foreach (DataRow row in dtmissedPackets.Rows)
                        {
                            string resultRow = row["Result"].ToString();
                            Color color;
                            if (resultRow.Contains("Missed") || resultRow.Contains("Unexpected"))
                            {
                                isMissedPacketValidationPassed = false;
                                color = Color.Red;
                            }
                            else if (resultRow.Contains("Additional")) color = Color.Orange;
                            else color = Color.Green;
                            bool isBold = resultRow.Contains("Missed");
                            _logService.LogMessage(logBox, _logService.FormatLogLineString("\t" + row[0], row[1].ToString(), row[2].ToString(), row[3].ToString()), color, isBold);
                        }
                        DataTableOperations.ExportDataTableToExcelWithDifferentSheet(dtmissedPackets, Path.Combine(_logService.LOG_DIRECTORY, $"DownloadedData\\{TestName}\\Validation and Verification Reports\\Missed_Packet_Report.xlsx"), $"{displayName}");
                        _logService.LogMessage(logBox, _logService.FormatLogLineString("\t" + "-----------------------------------------------------------------------------------------------------------------"), Color.Black, true);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"[{nameof(MissedPackedValidation)}] - {ex.Message} TRACE:{ex.StackTrace}");
            }
            return isMissedPacketValidationPassed;
        }
        public bool SummarizeTest(Dictionary<string, List<DateTime>> expectedPackets, Dictionary<string, List<DateTime>> receivedPackets,
                                        bool isPriorityPassed, bool isOpticalValidationPassed, bool isMissedPacketValidationPassed,
                                        out List<bool> statusOfTest)
        {
            statusOfTest = new List<bool>();
            //var profileOrder = new List<string> { "Instant", "Load Survey", "Daily Energy", "Self Registration", "Billing", "Current Bill" };
            var profileOrder = new Dictionary<string, string>
            {
                { "Instant", "Instant" },
                { "LS", "Load Survey" },
                { "DE", "Daily Energy" },
                { "SR", "Self Registration" },
                { "Bill", "Billing" },
                { "Current Bill", "Current Bill" }
            };
            try
            {
                _logService.LogMessage(logBox, "\n-----------------------------------------------------*** SUMMARY ***-----------------------------------------------------", Color.DeepPink, true);
                foreach (var profile in profileOrder)
                {
                    if (string.Equals(profile.Key, "Current Bill", StringComparison.OrdinalIgnoreCase) && !PushPacketManager.isCurrentBillAvailable)
                    {
                        continue;
                    }
                    if (profile.Key == TestProfiles || TestProfiles == "All")
                    {
                        expectedPackets.TryGetValue(profile.Value, out var expList);
                        receivedPackets.TryGetValue(profile.Value, out var recList);
                        if (expList == null) expList = new List<DateTime>();
                        if (recList == null) recList = new List<DateTime>();
                        if (expList.Count <= recList.Count)
                        {
                            statusOfTest.Add(true);
                            _logService.LogMessage(logBox, $"\t✓ RECEIVED PACKET COUNT VALIDATION: No of expected packet for {profile.Value} matches with received packets", Color.Green);
                        }
                        else
                        {
                            statusOfTest.Add(false);
                            _logService.LogMessage(logBox, $"\t✗ RECEIVED PACKET COUNT VALIDATION: No of expected packet for {profile.Value} does not match with received packets", Color.Red, true);
                        }
                    }
                }
                if (Random_Instant == 0 && Random_LS == 0 && Random_DE == 0 && Random_CB == 0 && Random_Bill == 0 && receivedPackets.Count > 0)
                {
                    _logService.LogMessage(logBox,
                        isPriorityPassed
                            ? "\t✓ PUSH PACKETS PRIORITY VALIDATION: The sequence of received packets at each expected time interval is correct"
                            : "\t✗ PUSH PACKETS PRIORITY VALIDATION: The sequence of received packets is incorrect and has failed the priority order check.",
                        isPriorityPassed ? Color.Green : Color.Red,
                        !isPriorityPassed);
                }
                if (receivedPackets.Count > 0)
                {
                    _logService.LogMessage(logBox,
                isOpticalValidationPassed
                    ? (TestProfiles != "Instant") ? "\t✓ PUSH AND OPTICAL DATA VALIDATION: The data of received push packets matches with optical data"
                                                  : "\t✓ PUSH PACKET PARAMETER VALIDATION: Instant profile parameter count matches push packet, and energy values are in increasing order."
                    : (TestProfiles != "Instant") ? "\t✗ PUSH AND OPTICAL DATA VALIDATION: The data of received push packets does not match with optical data"
                                                  : "\t✗ PUSH PACKET PARAMETER VALIDATION: Instant profile parameter count or energy order mismatch",
                isOpticalValidationPassed ? Color.Green : Color.Red,
                !isOpticalValidationPassed);
                }
                _logService.LogMessage(logBox,
                isMissedPacketValidationPassed
                    ? "\t✓ EXPECTED PUSH PACKETS VALIDATION: All Expected packets have been received, no missing packet available"
                    : "\t✗ EXPECTED PUSH PACKETS VALIDATION: There are differences in expected packets and received packets",
                isMissedPacketValidationPassed ? Color.Green : Color.Red,
                !isMissedPacketValidationPassed);
                _logService.LogMessage(logBox, "-------------------------------------------------------------------------------------------------------------------------\n", Color.DeepPink, true);
                return statusOfTest.All(x => x);
            }
            catch (Exception ex)
            {
                log.Error($"Error while summarizing the test - {ex.Message} TRACE:{ex.StackTrace}");
                return false;
            }
        }
        #endregion

        #region Priority Verification
        public DataTable BuildExpectedDataTable()
        {
            // Store all profiles
            var profiles = new Dictionary<string, List<DateTime>>()
            {
                { "Instant", lst_ExpectedPacketTiming_Instant },
                { "SR", lst_ExpectedPacketTiming_SR },
                { "LS", lst_ExpectedPacketTiming_LS },
                { "DE", lst_ExpectedPacketTiming_DE },
                { "Bill", lst_ExpectedPacketTiming_Bill },
                { "Current Bill", lst_ExpectedPacketTiming_CB }
            };

            // Find highest frequency profile (shortest interval)
            var freqMap = new Dictionary<string, int>
            {
                { "Instant", freq_Instant },
                { "SR", freq_SR },
                { "LS", freq_LS },
                { "DE", freq_DE },
                { "Current Bill", freq_CB },
                { "Bill", int.MaxValue }
            };
            highestFreqProfile = freqMap.OrderBy(x => x.Value).First().Key;
            highestFrequency = freqMap.OrderBy(x => x.Value).First().Value;
            DataTable table = new DataTable();
            try
            {
                // Create DataTable
                foreach (var profile in profiles.Keys)
                {
                    if (profile == "Current Bill" && !isCurrentBillAvailable) continue;
                    table.Columns.Add(profile, typeof(string));
                }
                foreach (var time in profiles[highestFreqProfile])
                {
                    DataRow row = table.NewRow();

                    // For each profile check if this time exists
                    foreach (var profile in profiles.Keys)
                    {
                        if (profile == "Current Bill" && !isCurrentBillAvailable) continue;
                        if (profiles[profile].Contains(time))
                        {
                            row[profile] = time.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                    }
                    table.Rows.Add(row);
                }
                // Handle same-time conflicts with priority order
                table = ReorderByPriority(table);
                try
                {
                    DataTableOperations.ExportDataTableToExcelWithDifferentSheet(table, Path.Combine(_logService.LOG_DIRECTORY, $"DownloadedData\\{TestName}\\Validation and Verification Reports\\Priority_Order_Report.xlsx"), $"ExpectedProfilesWithCommonTimings");
                }
                catch (Exception ex)
                {
                    log.Error($"Error while exporting expected datatable for priority verification - {ex.Message} TRACE:{ex.StackTrace}");
                }
            }
            catch (Exception ex)
            {
                log.Error($"[{nameof(BuildExpectedDataTable)}] - {ex.Message} TRACE:{ex.StackTrace}");
            }
            return table;
        }
        private static DataTable ReorderByPriority(DataTable table)
        {
            DataTable reordered = table.Clone();
            try
            {

                foreach (DataRow row in table.Rows)
                {
                    // If more than one profile has same datetime, enforce priority
                    var datetimes = row.ItemArray.Where(x => x != DBNull.Value).ToList();

                    if (datetimes.Count > 1)
                    {
                        // reorder by profile priority
                        DataRow newRow = reordered.NewRow();
                        foreach (string profile in priorityOrder)
                        {
                            if (profile == "Current Bill" && !isCurrentBillAvailable) continue;
                            if (row[profile] != DBNull.Value)
                            {
                                newRow[profile] = row[profile];
                            }
                        }
                        reordered.Rows.Add(newRow);
                    }
                    else
                    {
                        reordered.ImportRow(row);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"[{nameof(ReorderByPriority)}] - {ex.Message} TRACE:{ex.StackTrace}");
            }

            return reordered;
        }
        public DataTable ValidateReceivedOrderInWindows()
        {
            _logService.LogMessage(logBox, $"\nPush Packet Priority Validation:", Color.Brown, true);
            // Expected
            var expectedProfiles = new Dictionary<string, List<DateTime>>
            {
                { "Instant", lst_ExpectedPacketTiming_Instant },
                { "LS", lst_ExpectedPacketTiming_LS },
                { "DE", lst_ExpectedPacketTiming_DE },
                { "SR", lst_ExpectedPacketTiming_SR },
                { "Bill", lst_ExpectedPacketTiming_Bill },
                { "Current Bill", lst_ExpectedPacketTiming_CB }
            };
            // Received
            var receivedProfiles = new Dictionary<string, List<DateTime>>
            {
                { "Instant", lst_ReceivedPacketTiming_Instant },
                { "LS", lst_ReceivedPacketTiming_LS },
                { "DE", lst_ReceivedPacketTiming_DE },
                { "SR", lst_ReceivedPacketTiming_SR },
                { "Bill", lst_ReceivedPacketTiming_Bill },
                { "Current Bill", lst_ReceivedPacketTiming_CB }
            };
            DataTable dt_PriorityOrderReport = new DataTable();
            dt_PriorityOrderReport.Columns.Add("WindowStart", typeof(string));
            dt_PriorityOrderReport.Columns.Add("WindowEnd", typeof(string));
            dt_PriorityOrderReport.Columns.Add("ExpectedOrder", typeof(string));
            dt_PriorityOrderReport.Columns.Add("ReceivedOrder", typeof(string));
            dt_PriorityOrderReport.Columns.Add("ReceivedWithTime", typeof(string));
            dt_PriorityOrderReport.Columns.Add("MissingProfiles", typeof(string));
            dt_PriorityOrderReport.Columns.Add("ExtraProfiles", typeof(string));
            dt_PriorityOrderReport.Columns.Add("Status", typeof(string));
            dt_PriorityOrderReport.Columns.Add("Details", typeof(string));
            try
            {
                var baseList = expectedProfiles[highestFreqProfile].OrderBy(x => x).ToList();
                for (int i = 0; i <= baseList.Count - 1; i++)
                {
                    DateTime windowStart = baseList[i];
                    DateTime windowEnd = windowStart.AddMinutes(highestFrequency);

                    var expectedOrder = expectedProfiles
                        .Where(kvp => kvp.Value.Contains(windowStart))
                        .Select(kvp => kvp.Key)
                        .OrderBy(p => priorityOrder.IndexOf(p))
                        .ToList();
                    // if other profile push time in between 
                    //var addOrder = expectedProfiles.SelectMany(kvp => kvp.Value.Where(t => t > windowStart && t < windowEnd).Select(t => new { Profile = kvp.Key }).OrderBy(p => priorityOrder.IndexOf(p)).ToList());
                    var addOrder = expectedProfiles.SelectMany(kvp => kvp.Value.Where(t => t > windowStart && t < windowEnd).Select(t => new { Profile = kvp.Key })).OrderBy(p => priorityOrder.IndexOf(p.Profile)).ToList();
                    expectedOrder.AddRange(addOrder.Select(a => a.Profile));
                    var receivedItems = receivedProfiles
                        .SelectMany(kvp => kvp.Value
                            .Where(t => t >= windowStart && t < windowEnd)
                            .Select(t => new { Time = t, Profile = kvp.Key }))
                        .OrderBy(x => x.Time)
                        .ThenBy(x => priorityOrder.IndexOf(x.Profile))
                        .ToList();

                    var receivedWithTime = receivedItems
                        .Select(x => $"{x.Profile} ({x.Time:hh:mm:ss tt})")
                        .ToList();

                    var receivedOrder = receivedItems
                        .Select(x => x.Profile)
                        .ToList();
                    var missing = expectedOrder.Except(receivedOrder).ToList();
                    var extra = receivedOrder.Except(expectedOrder).ToList();

                    string status = "OK";
                    string details = "";

                    if (missing.Any() || extra.Any())
                    {
                        status = "Mismatch";
                        details = (missing.Count > 0 ? $"Missing: {string.Join(", ", missing)};" : string.Empty) + (extra.Count > 0 ? $"Extra: {string.Join(", ", extra)}" : string.Empty);
                    }
                    else if (receivedOrder.Count > 1)
                    {
                        var indexes = receivedOrder.Select(p => expectedOrder.IndexOf(p)).ToList();
                        //var indexes = receivedOrder.Select(p => priorityOrder.IndexOf(p)).ToList();
                        for (int j = 1; j < indexes.Count; j++)
                        {
                            if (indexes[j] < indexes[j - 1])
                            {
                                status = "Violation";
                                details = $"Expected order: {string.Join(" → ", expectedOrder)}";
                                break;
                            }
                        }
                    }
                    DataRow row = dt_PriorityOrderReport.NewRow();
                    row["WindowStart"] = windowStart.ToString("yyyy-MM-dd HH:mm:ss");
                    row["WindowEnd"] = windowEnd.ToString("yyyy-MM-dd HH:mm:ss");
                    row["ExpectedOrder"] = string.Join(" → ", expectedOrder);
                    row["ReceivedOrder"] = string.Join(" → ", receivedOrder);
                    row["ReceivedWithTime"] = string.Join(" → ", receivedWithTime);
                    row["MissingProfiles"] = string.Join(", ", missing);
                    row["ExtraProfiles"] = string.Join(", ", extra);
                    row["Status"] = status;
                    row["Details"] = details;

                    dt_PriorityOrderReport.Rows.Add(row);
                }
                try
                {
                    _logService.LogMessage(logBox, _logService.FormatLogLineString("\tExpected Timestamp", "Status", "Details"), Color.Black, true);
                    for (int i = 0; i < dt_PriorityOrderReport.Rows.Count; i++)
                    {
                        string[] rowArray = dt_PriorityOrderReport.Rows[i].ItemArray.Select(field => field?.ToString()).ToArray();
                        _logService.LogMessage(logBox, _logService.FormatLogLineString($"\t{rowArray[0]}", rowArray[7], rowArray[8]), rowArray[7].ToString() == "OK" ? Color.Green : Color.Red, rowArray[7].ToString() == "OK" ? false : true);
                    }
                    DataTableOperations.ExportDataTableToExcelWithDifferentSheet(dt_PriorityOrderReport, Path.Combine(_logService.LOG_DIRECTORY, $"DownloadedData\\{TestName}\\Validation and Verification Reports\\Priority_Order_Report.xlsx"), $"PriorityOrderReport");
                }
                catch (Exception ex)
                {
                    log.Error($"Error while exporting expected datatable for priority verification - {ex.Message.ToString()}");
                    log.Error(ex.Message.ToString());
                }
            }
            catch (Exception ex)
            {
                log.Error($"[{nameof(ValidateReceivedOrderInWindows)}] - {ex.Message} TRACE:{ex.StackTrace}");
            }
            return dt_PriorityOrderReport;
        }
        #endregion

        #region Profile verification
        public void GetProfileDataUsingOptical(ref DLMSComm DLMSReaderWriter, string startdate, string enddate)
        {
            _logService.LogMessage(logBox, $"\nPush and Optical Data Validation:", Color.Brown, true);
            //DataTable resultTable = new DataTable();
            //DLMSReaderWriter.SignOnDLMS();
            try
            {
                List<string> excludeProfiles = new List<string> { "Alert", "Tamper"/*, "CB"*/ };
                if (!isCurrentBillAvailable)
                    excludeProfiles.Add("CB");
                foreach (var profiles in profileDetails)
                {
                    if (excludeProfiles.Contains(profiles.Name))
                        continue;
                    if (profiles.ReceivedList.Count > 0)
                    {
                        _logService.LogMessage(logBox, $"\t✓ Downloading {profiles.DisplayName} through Optical", Color.Black);
                        switch (profiles.Name)
                        {
                            case "LS":
                                var LS = DLMSProfileGenericHelper.GetProfileDataTable(ref DLMSReaderWriter, "1.0.99.1.0.255", "1.0.94.91.4.255", 0, 0, (byte)0);
                                dtRec_Optical_LS = (DataTable)LS["DataTable"];
                                DataTableOperations.ExportDataTableToExcelWithDifferentSheet(dtRec_Optical_LS, Path.Combine(_logService.LOG_DIRECTORY, $"DownloadedData\\{TestName}\\Profile Reports\\Optical_{DeviceID}.xlsx"), $"{profiles.DisplayName}");
                                if (dtPushSetup_LS.Rows.Count > 3)
                                    dtRec_AppendedOpt_LS = GetAdditionalPSObjetcs(ref DLMSReaderWriter, dtPushSetup_LS, dtRec_Optical_LS, "LS");
                                break;
                            case "DE":
                                var DE = DLMSProfileGenericHelper.GetProfileDataTable(ref DLMSReaderWriter, "1.0.99.2.0.255", "1.0.94.91.5.255", 0, 0, (byte)0);
                                dtRec_Optical_DE = (DataTable)DE["DataTable"];
                                DataTableOperations.ExportDataTableToExcelWithDifferentSheet(dtRec_Optical_DE, Path.Combine(_logService.LOG_DIRECTORY, $"DownloadedData\\{TestName}\\Profile Reports\\Optical_{DeviceID}.xlsx"), $"{profiles.DisplayName}");
                                if (dtPushSetup_DE.Rows.Count > 3)
                                    dtRec_AppendedOpt_DE = GetAdditionalPSObjetcs(ref DLMSReaderWriter, dtPushSetup_DE, dtRec_Optical_DE, "DE");
                                break;
                            case "Bill":
                                int inUseEntries = Convert.ToInt32(parse.GetProfileValueString(SetGetFromMeter.GetDataFromObject(ref DLMSReaderWriter, 7, "1.0.98.1.0.255", 7)));
                                if (inUseEntries > 1)
                                {
                                    var Bill = DLMSProfileGenericHelper.GetProfileDataTable(ref DLMSReaderWriter, "1.0.98.1.0.255", "1.0.94.91.6.255", 1, inUseEntries - 1, (byte)2);
                                    dtRec_Optical_Bill = (DataTable)Bill["DataTable"];
                                    DataTableOperations.ExportDataTableToExcelWithDifferentSheet(dtRec_Optical_Bill, Path.Combine(_logService.LOG_DIRECTORY, $"DownloadedData\\{TestName}\\Profile Reports\\Optical_{DeviceID}.xlsx"), $"{profiles.DisplayName}");
                                    if (dtPushSetup_Bill.Rows.Count > 3)
                                        dtRec_AppendedOpt_Bill = GetAdditionalPSObjetcs(ref DLMSReaderWriter, dtPushSetup_Bill, dtRec_Optical_Bill, "Bill");
                                }
                                break;
                            case "SR":
                                if (dtPushSetup_SR.Rows.Count > 3 && dtPushSetup_SR.Columns.Count > 4)
                                {
                                    if (dtRec_Optical_SR.Columns.Count == 0)
                                    {
                                        dtRec_Optical_SR.Columns.Add("SN");
                                        for (int i = 3; i < dtPushSetup_SR.Rows.Count; i++)
                                        {
                                            dtRec_Optical_SR.Columns.Add($"{dtPushSetup_SR.Rows[i]["DESCRIPTION"].ToString()}");
                                        }
                                    }
                                    DataRow srRow = dtRec_Optical_SR.NewRow();
                                    srRow["SN"] = dtRec_Optical_SR.Rows.Count + 1;
                                    for (int i = 3; i < dtPushSetup_SR.Rows.Count; i++)
                                    {
                                        int objClass = Convert.ToInt32(dtPushSetup_SR.Rows[i]["CLASS"]);
                                        string obis = dtPushSetup_SR.Rows[i]["OBIS"].ToString();
                                        int attribute = Convert.ToInt32(dtPushSetup_SR.Rows[i]["ATTRIBUTE"]);
                                        string recString = SetGetFromMeter.GetDataFromObject(ref DLMSReaderWriter, objClass, obis, attribute);
                                        if (recString.Length > 4)
                                        {

                                            int counter = 0;
                                            if (recString.Substring(0, 4) == "0101")
                                                counter = 4;
                                            srRow[dtPushSetup_SR.Rows[i]["DESCRIPTION"].ToString()] = parse.GetProfileValueString(recString.Substring(counter));
                                        }
                                        else
                                            srRow[dtPushSetup_SR.Rows[i]["DESCRIPTION"].ToString()] = recString;
                                    }
                                    dtRec_Optical_SR.Rows.Add(srRow);
                                    DataTableOperations.ExportDataTableToExcelWithDifferentSheet(dtRec_Optical_SR, Path.Combine(_logService.LOG_DIRECTORY, $"DownloadedData\\{TestName}\\Profile Reports\\Optical_{DeviceID}.xlsx"), $"{profiles.Name}");
                                }
                                break;
                            case "Instant":
                                var instant = DLMSProfileGenericHelper.GetProfileDataTable(ref DLMSReaderWriter, "1.0.94.91.0.255", "1.0.94.91.3.255", 0, 0, 0);
                                dtRec_Optical_Instant = (DataTable)instant["DataTable"];
                                var networkColumn = dtRec_Optical_Instant.Columns.Cast<DataColumn>().FirstOrDefault(c => c.ColumnName.Contains("0.0.96.12.131.255"));
                                if (networkColumn != null)
                                {
                                    string networkInfoString = dtRec_Optical_Instant.Rows[0][networkColumn].ToString();
                                    if (!string.IsNullOrEmpty(networkInfoString))
                                    {
                                        List<string> splittedDataStructure = new List<string>();
                                        List<string> convertedSplittedData = new List<string>();
                                        int start = 4;
                                        for (int i = 0; i < Convert.ToInt32(networkInfoString.Substring(2, 2), 16); i++)
                                        {
                                            splittedDataStructure.Add(parse.GetProfileDataString(networkInfoString, ref start));
                                            convertedSplittedData.Add(parse.GetProfileValueString(splittedDataStructure[i].ToString().Trim()));
                                        }
                                        dtRec_Optical_Instant.Rows[0][networkColumn] = string.Join(", ", convertedSplittedData);
                                    }
                                }
                                DataTableOperations.ExportDataTableToExcelWithDifferentSheet(dtRec_Optical_Instant, Path.Combine(_logService.LOG_DIRECTORY, $"DownloadedData\\{TestName}\\Profile Reports\\Optical_{DeviceID}.xlsx"), $"{profiles.DisplayName}");
                                break;
                            case "CB":
                                inUseEntries = Convert.ToInt32(parse.GetProfileValueString(SetGetFromMeter.GetDataFromObject(ref DLMSReaderWriter, 7, "1.0.98.1.0.255", 7)));
                                var currentBill = DLMSProfileGenericHelper.GetProfileDataTable(ref DLMSReaderWriter, "1.0.98.1.0.255", "1.0.94.91.6.255", inUseEntries, inUseEntries, (byte)2);
                                dtRec_Optical_CB = (DataTable)currentBill["DataTable"];
                                DataTableOperations.ExportDataTableToExcelWithDifferentSheet(dtRec_Optical_CB, Path.Combine(_logService.LOG_DIRECTORY, $"DownloadedData\\{TestName}\\Profile Reports\\Optical_{DeviceID}.xlsx"), $"{profiles.DisplayName}");
                                if (dtPushSetup_Bill.Rows.Count > 3)
                                    dtRec_AppendedOpt_CB = GetAdditionalPSObjetcs(ref DLMSReaderWriter, dtPushSetup_Bill, dtRec_Optical_CB, "CB");
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        _logService.LogMessage(logBox, $"\t✗ No packet received for {profiles.DisplayName}", Color.Black);
                    }
                }
                UpdateProfileDetails();
                /* if (lst_ReceivedPacketTiming_LS.Count > 0)
                 {
                     //if (TestProfiles == "All" || TestProfiles == "LS" || lst_ReceivedPacketTiming_LS.Count > 0)
                     //{
                     _logService.LogMessage(logBox, $"\tDownloading Load Survey through Optical", Color.Black);
                     //var LS = DLMSProfileGenericHelper.GetProfileDataTable(ref DLMSReaderWriter, "1.0.99.1.0.255", "1.0.94.91.4.255", 0, 0, (byte)1, startdate, enddate);
                     var LS = DLMSProfileGenericHelper.GetProfileDataTable(ref DLMSReaderWriter, "1.0.99.1.0.255", "1.0.94.91.4.255", 0, 0, (byte)0);
                     dtRec_Optical_LS = (DataTable)LS["DataTable"];
                     DataTableOperations.ExportDataTableToExcelWithDifferentSheet(dtRec_Optical_LS, Path.Combine(_logService.LOG_DIRECTORY, $"DownloadedData\\{TestName}\\Profile Reports\\Optical_{DeviceID}.xlsx"), $"Load Survey");
                     //}
                 }
                 if (lst_ReceivedPacketTiming_DE.Count > 0)
                 {
                     //if (TestProfiles == "All" || TestProfiles == "DE" || lst_ReceivedPacketTiming_DE.Count > 0)
                     //{
                     _logService.LogMessage(logBox, $"\tDownloading Daily Energy through Optical", Color.Black);
                     //  var DE = DLMSProfileGenericHelper.GetProfileDataTable(ref DLMSReaderWriter, "1.0.99.2.0.255", "1.0.94.91.5.255", 0, 0, (byte)1, startdate, enddate);
                     var DE = DLMSProfileGenericHelper.GetProfileDataTable(ref DLMSReaderWriter, "1.0.99.2.0.255", "1.0.94.91.5.255", 0, 0, (byte)0);
                     dtRec_Optical_DE = (DataTable)DE["DataTable"];
                     DataTableOperations.ExportDataTableToExcelWithDifferentSheet(dtRec_Optical_DE, Path.Combine(_logService.LOG_DIRECTORY, $"DownloadedData\\{TestName}\\Profile Reports\\Optical_{DeviceID}.xlsx"), $"Daily Energy");
                     //}
                 }
                 if (lst_ReceivedPacketTiming_Bill.Count > 0)
                 {
                     //if (TestProfiles == "All" || TestProfiles == "Bill" || lst_ReceivedPacketTiming_Bill.Count > 0)
                     //{
                     int inUseEntries = Convert.ToInt32(parse.GetProfileValueString(SetGetFromMeter.GetDataFromObject(ref DLMSReaderWriter, 7, "1.0.98.1.0.255", 7)));
                     if (inUseEntries > 1)
                     {
                         _logService.LogMessage(logBox, $"\tDownloading Billing Profile through Optical", Color.Black);
                         var Bill = DLMSProfileGenericHelper.GetProfileDataTable(ref DLMSReaderWriter, "1.0.98.1.0.255", "1.0.94.91.6.255", 1, inUseEntries - 1, (byte)2);
                         dtRec_Optical_Bill = (DataTable)Bill["DataTable"];
                         DataTableOperations.ExportDataTableToExcelWithDifferentSheet(dtRec_Optical_Bill, Path.Combine(_logService.LOG_DIRECTORY, $"DownloadedData\\{TestName}\\Profile Reports\\Optical_{DeviceID}.xlsx"), $"Billing");
                     }
                     //}
                 }
                 if (lst_ReceivedPacketTiming_SR.Count > 0)
                 {
                     //if (TestProfiles == "All" || TestProfiles == "SR" || lst_ReceivedPacketTiming_SR.Count > 0)
                     //{
                     if (dtPushSetup_SR.Rows.Count > 3 && dtPushSetup_SR.Columns.Count > 4)
                     {
                         _logService.LogMessage(logBox, $"\tDownloading Self Registration parameters through Optical", Color.Black);
                         if (dtRec_Optical_SR.Columns.Count == 0)
                         {
                             dtRec_Optical_SR.Columns.Add("SN");
                             for (int i = 3; i < dtPushSetup_SR.Rows.Count; i++)
                             {
                                 dtRec_Optical_SR.Columns.Add($"{dtPushSetup_SR.Rows[i]["DESCRIPTION"].ToString()}");
                             }
                         }
                         DataRow srRow = dtRec_Optical_SR.NewRow();
                         srRow["SN"] = dtRec_Optical_SR.Rows.Count + 1;
                         for (int i = 3; i < dtPushSetup_SR.Rows.Count; i++)
                         {
                             int objClass = Convert.ToInt32(dtPushSetup_SR.Rows[i]["CLASS"]);
                             string obis = dtPushSetup_SR.Rows[i]["OBIS"].ToString();
                             int attribute = Convert.ToInt32(dtPushSetup_SR.Rows[i]["ATTRIBUTE"]);
                             string recString = SetGetFromMeter.GetDataFromObject(ref DLMSReaderWriter, objClass, obis, attribute);
                             if (recString.Length > 4)
                             {
                                 int counter = 0;
                                 if (recString.Substring(0, 4) == "0101")
                                     counter = 4;
                                 srRow[dtPushSetup_SR.Rows[i]["DESCRIPTION"].ToString()] = parse.GetProfileValueString(recString.Substring(counter));
                             }
                             else
                                 srRow[dtPushSetup_SR.Rows[i]["DESCRIPTION"].ToString()] = recString;
                         }
                         dtRec_Optical_SR.Rows.Add(srRow);
                         DataTableOperations.ExportDataTableToExcelWithDifferentSheet(dtRec_Optical_SR, Path.Combine(_logService.LOG_DIRECTORY, $"DownloadedData\\{TestName}\\Profile Reports\\Optical_{DeviceID}.xlsx"), $"SR");
                     }
                     //}
                 }
                 if (lst_ReceivedPacketTiming_Instant.Count > 0)
                 {
                     //if (TestProfiles == "All" || TestProfiles == "Instant" || lst_ReceivedPacketTiming_Instant.Count > 0)
                     //{
                     _logService.LogMessage(logBox, $"\tDownloading Instant profile through Optical", Color.Black);
                     var instant = DLMSProfileGenericHelper.GetProfileDataTable(ref DLMSReaderWriter, "1.0.94.91.0.255", "1.0.94.91.3.255", 0, 0, 0);
                     dtRec_Optical_Instant = (DataTable)instant["DataTable"];
                     var networkColumn = dtRec_Optical_Instant.Columns.Cast<DataColumn>().FirstOrDefault(c => c.ColumnName.Contains("0.0.96.12.131.255"));
                     if (networkColumn != null)
                     {
                         string networkInfoString = dtRec_Optical_Instant.Rows[0][networkColumn].ToString();
                         if (!string.IsNullOrEmpty(networkInfoString))
                         {
                             List<string> splittedDataStructure = new List<string>();
                             List<string> convertedSplittedData = new List<string>();
                             int start = 4;
                             for (int i = 0; i < Convert.ToInt32(networkInfoString.Substring(2, 2), 16); i++)
                             {
                                 splittedDataStructure.Add(parse.GetProfileDataString(networkInfoString, ref start));
                                 convertedSplittedData.Add(parse.GetProfileValueString(splittedDataStructure[i].ToString().Trim()));
                             }
                             dtRec_Optical_Instant.Rows[0][networkColumn] = string.Join(", ", convertedSplittedData);
                         }
                     }
                     DataTableOperations.ExportDataTableToExcelWithDifferentSheet(dtRec_Optical_Instant, Path.Combine(_logService.LOG_DIRECTORY, $"DownloadedData\\{TestName}\\Profile Reports\\Optical_{DeviceID}.xlsx"), $"Instant");
                     //}
                 } */
            }
            catch (Exception ex)
            {
                log.Error($"[{nameof(GetProfileDataUsingOptical)}] - {ex.Message} TRACE:{ex.StackTrace}");
            }
        }
        public bool ValidatePushAndOpticalProfiles()
        {
            bool isOpticalValidationPassed = true;
            List<bool> isAllProfileOpticalPassed = new List<bool>();
            try
            {
                if (lst_ReceivedPacketTiming_LS.Count > 0)
                {
                    //if (TestProfiles == "All" || TestProfiles == "LS" || lst_ReceivedPacketTiming_LS.Count > 0)
                    //{
                    //DataTable filteredPushLSDT = FilterRecPushDT(dtRec_Push_LS.Copy(), dtPushSetup_LS.Rows.Count + 1, dt_Appended_LS.Rows.Count, alternateRow: 1);
                    //isAllProfileOpticalPassed.Add(ComparePushOpticalData(filteredPushLSDT, dtRec_Optical_LS.Copy(), "LS"));
                    DataTable filteredPushLSDT = FilterRecPushDT(dtRec_Push_LS.Copy(), 3 + 1, dt_Appended_LS.Rows.Count, alternateRow: 1);
                    isAllProfileOpticalPassed.Add(ComparePushOpticalData(filteredPushLSDT, (dtPushSetup_LS.Rows.Count > 3) ? dtRec_AppendedOpt_LS.Copy() : dtRec_Optical_LS.Copy(), "LS"));
                    //}
                }
                if (lst_ReceivedPacketTiming_DE.Count > 0)
                {
                    //if (TestProfiles == "All" || TestProfiles == "DE" || lst_ReceivedPacketTiming_DE.Count > 0)
                    //{
                    //DataTable filteredPushDEDT = FilterRecPushDT(TransposeDataTable(dtRec_Push_DE.Copy()), dtPushSetup_DE.Rows.Count + 1, dt_Appended_DE.Columns.Count + 1, alternateRow: 2);
                    //isAllProfileOpticalPassed.Add(ComparePushOpticalData(filteredPushDEDT, dtRec_Optical_DE.Copy(), "DE"));
                    DataTable filteredPushDEDT = FilterRecPushDT(TransposeDataTable(dtRec_Push_DE.Copy()), 3 + 1, dt_Appended_DE.Columns.Count + 1, alternateRow: 2);
                    isAllProfileOpticalPassed.Add(ComparePushOpticalData(filteredPushDEDT, (dtPushSetup_DE.Rows.Count > 3) ? dtRec_AppendedOpt_DE.Copy() : dtRec_Optical_DE.Copy(), "DE"));
                    //}
                }
                if (lst_ReceivedPacketTiming_Bill.Count > 0)
                {
                    //if (TestProfiles == "All" || TestProfiles == "Bill" || lst_ReceivedPacketTiming_Bill.Count > 0)
                    //{
                    //DataTable filteredPushBillDT = FilterRecPushDT(TransposeDataTable(dtRec_Push_Bill.Copy()), dtPushSetup_Bill.Rows.Count + 1, dt_Appended_Bill.Columns.Count + 1, alternateRow: 2);
                    //isAllProfileOpticalPassed.Add(ComparePushOpticalData(filteredPushBillDT, dtRec_Optical_Bill.Copy(), "Bill"));
                    DataTable filteredPushBillDT = FilterRecPushDT(TransposeDataTable(dtRec_Push_Bill.Copy()), 3 + 1, dt_Appended_Bill.Columns.Count + 1, alternateRow: 2);
                    isAllProfileOpticalPassed.Add(ComparePushOpticalData(filteredPushBillDT, (dtPushSetup_Bill.Rows.Count > 3) ? dtRec_AppendedOpt_Bill.Copy() : dtRec_Optical_Bill.Copy(), "Bill"));
                    //}
                }
                if (lst_ReceivedPacketTiming_SR.Count > 0)
                {
                    //if (TestProfiles == "All" || TestProfiles == "SR" || lst_ReceivedPacketTiming_SR.Count > 0)
                    //{
                    DataTable filteredPushSRDT = FilterRecPushDT(TransposeDataTable(dtRec_Push_SR.Copy()), 4, dtPushSetup_SR.Columns.Count + 1, alternateRow: 2);  // colStart =4 as device id, rtc, and send destination
                    isAllProfileOpticalPassed.Add(ComparePushOpticalData(filteredPushSRDT, dtRec_Optical_SR.Copy(), "SR"));
                    //}
                }
                if (lst_ReceivedPacketTiming_Instant.Count > 0)
                {
                    //if (TestProfiles == "All" || TestProfiles == "Instant" || lst_ReceivedPacketTiming_Instant.Count > 0)
                    //{
                    if (isInstantProfile)
                    {
                        DataTable filterPushInstantDT = FilterRecPushDT(TransposeDataTable(dtRec_Push_Instant.Copy()), dtPushSetup_Instant.Rows.Count + 1, dt_Appended_Instant.Columns.Count + 1, alternateRow: 2);  // colStart =4 as device id, rtc, and send destination
                        isAllProfileOpticalPassed.Add(ComparePushOpticalData(filterPushInstantDT, dtRec_Optical_Instant.Copy(), "Instant"));
                    }
                    else
                    {
                        DataTable filterPushInstantDT = FilterRecPushDT(TransposeDataTable(dtRec_Push_Instant.Copy()), 3 /*dtPushSetup_Instant.Rows.Count*/ + 1, dtPushSetup_Instant.Columns.Count + 1, alternateRow: 2); // as per IS , r3 parameters device id, destination and RTC are fixed
                        isAllProfileOpticalPassed.Add(ComparePushOpticalData(filterPushInstantDT, dtRec_Optical_Instant.Copy(), "Instant"));
                    }
                    //}
                }
                if (lst_ReceivedPacketTiming_CB.Count > 0)
                {
                    DataTable filteredPushCBDT = FilterRecPushDT(TransposeDataTable(dtRec_Push_CB.Copy()), 3 + 1, dt_Appended_Bill.Columns.Count + 1, alternateRow: 2);
                    isAllProfileOpticalPassed.Add(ComparePushOpticalData(filteredPushCBDT, (dtPushSetup_Bill.Rows.Count > 3) ? dtRec_AppendedOpt_CB.Copy() : dtRec_Optical_CB.Copy(), "CB"));
                }
                if (isAllProfileOpticalPassed.All(passed => passed))
                {
                    isOpticalValidationPassed = true;
                }
                else
                {
                    isOpticalValidationPassed = false;
                }
            }
            catch (Exception ex)
            {
                log.Error($"[{nameof(ValidatePushAndOpticalProfiles)}] - {ex.Message} TRACE:{ex.StackTrace}");
            }
            return isOpticalValidationPassed;
        }
        private bool ComparePushOpticalData(DataTable pushData, DataTable opticalData, string profileName)
        {
            var profile = profileDetails.Find(p => p.Name == profileName);
            bool isOpticalValidationPassed = true;
            List<string> misMatchedData = new List<string>();
            if (opticalData.Columns.Count > 0)
                opticalData.Columns.RemoveAt(0);

            misMatchedData = BuildAndComparePushOpticalDT(pushData, opticalData, profileName);
            string message = "";
            if (misMatchedData.Count == 0)
            {
                message = (profileName == "Instant" || profileName == "CB") ? "Push Data: Energy values are consistent, non-decreasing, less than optical values and PF within limit." : "Comparison: Push and Optical data match successfully.";
                _logService.LogMessage(logBox, $"\t{profileName} {message}", Color.Black, true);
            }
            else
            {
                isOpticalValidationPassed = false;
                message = (profileName == "Instant" || profileName == "CB") ? "Push Data: Either energy values are inconsistent or PF is not within limit." : "Comparison: Push and Optical data do not match.";
                _logService.LogMessage(logBox, $"\t{profileName} {message}", Color.Red, true);
                foreach (var mismatch in misMatchedData)
                {
                    _logService.LogMessage(logBox, $"\t\t{mismatch}", Color.Red, true);
                }
            }
            return isOpticalValidationPassed;
        }
        /// <summary>
        /// To filter out received push datatable, scaler multiplication and  24 hr date time format
        /// </summary>
        /// <param name="recPushDT"></param>
        /// <param name="colStart"></param>
        /// <param name="rowStart"></param>
        /// <param name="alternateRow"></param>
        /// <returns></returns>
        public DataTable FilterRecPushDT(DataTable recPushDT, int colStart, int rowStart, int alternateRow)
        {
            DataTable result = new DataTable();
            try
            {
                if (recPushDT.Rows.Count == 0 || recPushDT.Columns.Count == 0)
                    return new DataTable();
                // applying scaler in recPushDT
                DataRow scalerRow = recPushDT.Rows[4];
                foreach (DataRow row in recPushDT.Rows)
                {
                    if (recPushDT.Rows.IndexOf(row) >= rowStart)
                    {
                        for (int i = colStart; i < recPushDT.Columns.Count; i++)
                        {
                            string scalerVal = scalerRow[i]?.ToString().Trim() ?? "";
                            if (!string.IsNullOrEmpty(scalerVal) && scalerVal.Length >= 2)
                            {
                                if (parse.ScalerhshTable.ContainsKey(scalerVal.Substring(0, 2)))
                                {
                                    double scalerMultiplier = Convert.ToDouble((string)parse.ScalerhshTable[scalerVal.Substring(0, 2)]);
                                    if (double.TryParse(row[i]?.ToString().Trim(), out double cellValue))
                                    {
                                        row[i] = cellValue * scalerMultiplier;
                                    }
                                }
                            }
                        }
                    }
                }

                for (int col = colStart; col < recPushDT.Columns.Count; col++)
                {
                    string header = recPushDT.Rows[0][col]?.ToString();
                    if (string.IsNullOrWhiteSpace(header))
                        header = $"Col{col}";
                    //string finalHeader = header;
                    int counter = 1;
                    while (result.Columns.Contains(header))
                    {
                        header = $"{header}_{counter}";
                        counter++;
                    }
                    result.Columns.Add(header);
                }
                for (int row = rowStart; row < recPushDT.Rows.Count; row += alternateRow)
                {
                    DataRow newRow = result.NewRow();
                    for (int col = colStart; col < recPushDT.Columns.Count; col++)
                        newRow[col - colStart] = recPushDT.Rows[row][col];
                    result.Rows.Add(newRow);
                }
                foreach (DataRow row in result.Rows)
                {
                    DateTime outTime = DateTime.Now;
                    DateTime dt = DateTime.Now;
                    for (int colIndex = 0; colIndex < result.Columns.Count; colIndex++)
                    {
                        if (DateTime.TryParseExact(row[colIndex].ToString().Trim(), Constants.timeStamp12Hours, null, System.Globalization.DateTimeStyles.None, out outTime))
                        {
                            row[colIndex] = outTime.ToString("dd/MM/yyyy HH:mm:ss");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"[{nameof(FilterRecPushDT)}] - {ex.Message} TRACE:{ex.StackTrace}");
            }
            return result;
        }
        /// <summary>
        /// To compare filtered received push datatable with optical datatable and saving both in single excel sheet
        /// </summary>
        /// <param name="pushData"></param>
        /// <param name="opticalData"></param>
        /// <param name="profileName"></param>
        /// <returns></returns>
        public List<string> BuildAndComparePushOpticalDT(DataTable pushData, DataTable opticalData, string profileName)
        {
            var profile = profileDetails.Find(p => p.Name == profileName);
            List<string> misMatchData = new List<string>();
            DataTable pushOptDT = new DataTable();
            try
            {
                if (pushData.Rows.Count == 0 || opticalData.Rows.Count == 0)
                {
                    misMatchData.Add($"Error in {profileName}: {(pushData.Rows.Count == 0 ? "Push data" : "Optical data")} not received.");
                    return misMatchData;
                }
                if (isInstantProfile || profileName != "Instant")
                {
                    if (pushData.Columns.Count != opticalData.Columns.Count)
                    {
                        misMatchData.Add($"Error in {profileName}: Parameter count mismatch between Push and Optical Datatables.");
                        return misMatchData;
                    }
                }
                if (profileName == "Instant" || profileName == "CB")
                {
                    pushOptDT = pushData;
                    misMatchData = ValidateEnergyColumns(pushData, opticalData);
                }
                /* else
                 {
                     foreach (DataColumn col in pushData.Columns)
                     {
                         pushOptDT.Columns.Add($"(Push)-{col.ColumnName}");
                         pushOptDT.Columns.Add($"(Optical)-{col.ColumnName}");
                     }

                     int pushStartIndex = -1;
                     string pushStartVal = "";
                     if (TestProfiles == "SR" || profileName == "SR")
                     {
                         DataRow pushRow = pushData.Rows[0];
                         DataRow opticalRow = opticalData.Rows[0];
                         DataRow newRow = pushOptDT.NewRow();

                         for (int col = 0; col < pushData.Columns.Count; col++)
                         {
                             string pushVal = pushRow[col]?.ToString()?.Trim() ?? "";
                             string optVal = opticalRow[col]?.ToString()?.Trim() ?? "";

                             newRow[$"(Push)-{pushData.Columns[col].ColumnName}"] = pushVal;
                             newRow[$"(Optical)-{pushData.Columns[col].ColumnName}"] = optVal;
                             if (optVal == "0B" || optVal == "0D") optVal = "0";
                             if (pushVal != optVal)
                             {
                                 misMatchData.Add($"Column '{pushData.Columns[col].ColumnName}': Push='{pushVal}', Optical='{optVal}'");
                             }
                         }
                         pushOptDT.Rows.Add(newRow);
                     }  // as row[0]col[0] is not date time format
                     else
                     {
                         for (int i = 0; i < pushData.Rows.Count; i++)
                         {
                             string value = pushData.Rows[i][0]?.ToString()?.Trim();
                             if (!string.IsNullOrEmpty(value) && DateTime.TryParseExact(value, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime dt))
                             {
                                 pushStartIndex = i;
                                 pushStartVal = value;
                                 break;
                             }
                         }
                         if (pushStartIndex == -1)
                         {
                             misMatchData.Add($"Error in {profileName}: Could not find a valid DateTime in received Push data.");
                             return misMatchData;
                         }

                         int startIndex = -1;
                         for (int i = 0; i < opticalData.Rows.Count; i++)
                         {
                             if (opticalData.Rows[i][0]?.ToString() == pushStartVal)
                             {
                                 startIndex = i;
                                 break;
                             }
                         }
                         if (startIndex == -1)
                         {
                             misMatchData.Add("Error: Could not align Push and Optical DataTables (first column mismatch).");
                             return misMatchData;
                         }

                         for (int i = 0; i < pushData.Rows.Count - pushStartIndex; i++)
                         {
                             int opticalRowIndex = startIndex + i;
                             int pushRowIndex = pushStartIndex + i;
                             if (opticalRowIndex >= opticalData.Rows.Count)
                             {
                                 misMatchData.Add($"Row {i + 1}: Missing row in Optical data (Push Table has more rows).");
                                 break;
                             }

                             DataRow newRow = pushOptDT.NewRow();
                             DataRow pushRow = pushData.Rows[pushRowIndex];
                             DataRow opticalRow = opticalData.Rows[opticalRowIndex];

                             for (int col = 0; col < pushData.Columns.Count; col++)
                             {
                                 string pushVal = pushRow[col]?.ToString()?.Trim() ?? "";
                                 string optVal = opticalRow[col]?.ToString()?.Trim() ?? "";

                                 newRow[$"(Push)-{pushData.Columns[col].ColumnName}"] = pushVal;
                                 newRow[$"(Optical)-{pushData.Columns[col].ColumnName}"] = optVal;

                                 if (pushVal != optVal)
                                 {
                                     misMatchData.Add($"Row {i + 1}, Column '{pushData.Columns[col].ColumnName}': Push='{pushVal}', Optical='{optVal}'");
                                 }
                             }
                             pushOptDT.Rows.Add(newRow);
                         }
                     }
                 } */
                else
                {
                    foreach (DataColumn col in pushData.Columns)
                    {
                        pushOptDT.Columns.Add($"(Optical)-{col.ColumnName}");
                        pushOptDT.Columns.Add($"(Push)-{col.ColumnName}");
                    }
                    pushOptDT.Columns.Add("Status");
                    int pushStartIndex = -1;
                    string pushStartVal = "";
                    if (TestProfiles == "SR" || profileName == "SR")
                    {
                        DataRow opticalRow = opticalData.Rows[0];

                        for (int i = 0; i < pushData.Rows.Count; i++)
                        {
                            DataRow pushRow = pushData.Rows[i];
                            DataRow newRow = pushOptDT.NewRow();
                            bool anyMismatch = false;
                            bool pushMissing = pushRow == null;
                            for (int col = 0; col < pushData.Columns.Count; col++)
                            {
                                string pushVal = pushRow[col]?.ToString()?.Trim() ?? "";
                                string optVal = opticalRow[col]?.ToString()?.Trim() ?? "";

                                newRow[$"(Optical)-{pushData.Columns[col].ColumnName}"] = optVal;
                                newRow[$"(Push)-{pushData.Columns[col].ColumnName}"] = pushVal;
                                if (optVal == "0B" || optVal == "0D") optVal = "0";
                                if (!pushMissing && pushVal != optVal)
                                    anyMismatch = true;
                            }
                            if (pushMissing)
                            {
                                newRow["Status"] = "Push data missing";
                                misMatchData.Add($"Push packet missing at row{i + 1}");
                            }
                            else if (anyMismatch)
                            {
                                newRow["Status"] = "Not match";
                                misMatchData.Add($"Optical and Push value not matched at row{i + 1}");
                            }
                            else
                                newRow["Status"] = "Match";

                            pushOptDT.Rows.Add(newRow);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < pushData.Rows.Count; i++)
                        {
                            //string value = pushData.Rows[i][0]?.ToString()?.Trim();
                            //if (!string.IsNullOrEmpty(value) && DateTime.TryParseExact(value, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime dt))
                            //{
                            //    pushStartIndex = i;
                            //    pushStartVal = value;
                            //    break;
                            //}
                            bool breakLoop = false; // new logic in case of push additional objects
                            for (int col = 0; col < pushData.Columns.Count; col++)
                            {
                                string value = pushData.Rows[i][col]?.ToString()?.Trim();
                                if (!string.IsNullOrEmpty(value) && DateTime.TryParseExact(value, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime dt))
                                {
                                    pushStartIndex = i;
                                    pushStartVal = value;
                                    breakLoop = true;
                                    break;
                                }
                            }
                            if (breakLoop)
                                break;
                        }
                        if (pushStartIndex == -1)
                        {
                            misMatchData.Add($"Error in {profileName}: Could not find a valid DateTime in received Push data.");
                            return misMatchData;
                        }
                        int startIndex = -1;
                        for (int i = 0; i < opticalData.Rows.Count; i++)
                        {
                            //if (opticalData.Rows[i][0]?.ToString() == pushStartVal)
                            //{
                            //    startIndex = i;
                            //    break;
                            //}
                            bool breakLoop = false;  // new logic in case of push additional objects
                            for (int col = 0; col < opticalData.Columns.Count; col++)
                            {
                                if (opticalData.Rows[i][col]?.ToString() == pushStartVal)
                                {
                                    startIndex = i;
                                    breakLoop = true;
                                    break;
                                }
                            }
                            if (breakLoop)
                                break;
                        }
                        if (startIndex == -1)
                        {
                            misMatchData.Add("Error: Could not align Push and Optical DataTables (first column mismatch).");
                            return misMatchData;
                        }

                        DateTime optEndDT = DateTime.Now;
                        DateTime expEndDT = DateTime.Now;
                        //string lastValue = string.Empty;
                        //string endDateTime = string.Empty;
                        bool isExpEndDTAbsent = false;
                        int endIndex = -1;
                        if (profileName == "LS" && lst_ExpectedPacketTiming_LS.Count > 0)
                        {
                            //lastValue = lst_ExpectedPacketTiming_LS.Last().ToString("dd/MM/yyyy HH:mm:ss").Trim();
                            if (Random_LS == 0)
                                expEndDT = lst_ExpectedPacketTiming_LS.Last();
                            else
                                expEndDT = lst_ExpectedPacketTiming_LS.Last().AddMinutes(-Random_LS);
                        }
                        else if (profileName == "DE" && lst_ExpectedPacketTiming_DE.Count > 0)
                        {
                            //lastValue = lst_ExpectedPacketTiming_DE.Last().ToString("dd/MM/yyyy HH:mm:ss").Trim();
                            if (Random_DE == 0)
                                expEndDT = lst_ExpectedPacketTiming_DE.Last();
                            else
                                expEndDT = lst_ExpectedPacketTiming_DE.Last().AddMinutes(-Random_DE);
                        }
                        else if (profileName == "Bill" && lst_ExpectedPacketTiming_Bill.Count > 0)
                        {
                            //lastValue = lst_ExpectedPacketTiming_Bill.Last().ToString("dd/MM/yyyy HH:mm:ss").Trim();
                            if (Random_Bill == 0)
                                expEndDT = lst_ExpectedPacketTiming_Bill.Last();
                            else
                                expEndDT = lst_ExpectedPacketTiming_Bill.Last().AddMinutes(-Random_Bill);
                        }
                        else
                            isExpEndDTAbsent = true;


                        //if (!string.IsNullOrEmpty(lastValue) && DateTime.TryParseExact(lastValue, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out outTime))
                        //{
                        //endDateTime = outTime.ToString("dd/MM/yyyy HH:mm:ss");
                        if (!isExpEndDTAbsent)
                        {
                            for (int i = 0; i < opticalData.Rows.Count; i++)
                            {
                                //if (opticalData.Rows[i][0]?.ToString() == endDateTime)
                                //{
                                //    endIndex = i;
                                //    break;
                                //}
                                bool breakLoop = false;  // new logic in case of push additional objects
                                for (int col = 0; col < opticalData.Columns.Count; col++)
                                {
                                    DateTime.TryParseExact(opticalData.Rows[i][col]?.ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out optEndDT);
                                    if (optEndDT >= expEndDT)
                                    {
                                        endIndex = i - 1;
                                        breakLoop = true;
                                        break;
                                    }
                                }
                                if (breakLoop)
                                    break;
                            }
                            if (endIndex == -1)
                            {
                                endIndex = opticalData.Rows.Count - 1;
                                //misMatchData.Add("Error: Could not align Push and Optical DataTables.");  // in case of LS_freq  - 15, LSIP - 30
                                //return misMatchData;
                            }
                            //}
                        }
                        else
                            endIndex = opticalData.Rows.Count - 1;

                        for (int i = startIndex; i <= endIndex; i++)
                        //for (int i = startIndex; i < opticalData.Rows.Count; i++)
                        {
                            //string opticalTime = opticalData.Rows[i][0]?.ToString()?.Trim();
                            //DataRow opticalRow = opticalData.Rows[i];
                            //DataRow pushRow = pushData.AsEnumerable().FirstOrDefault(r => r[0]?.ToString()?.Trim() == opticalTime);
                            string opticalTime = "";   // new logic in case of push additional objects
                            for (int col = 0; col < opticalData.Columns.Count; col++)
                            {
                                opticalTime = opticalData.Rows[i][col]?.ToString()?.Trim();
                                if ((!string.IsNullOrEmpty(opticalTime)) && (DateTime.TryParseExact(opticalTime, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime dt)))
                                    break;
                            }
                            DataRow opticalRow = opticalData.Rows[i];
                            DataRow pushRow = null;
                            foreach (DataRow row in pushData.Rows)
                            {
                                for (int col = 0; col < pushData.Columns.Count; col++)
                                {
                                    if (row[col]?.ToString()?.Trim() == opticalTime)
                                    {
                                        pushRow = row;
                                        break;
                                    }
                                }
                                if (pushRow != null)
                                    break;
                            }

                            DataRow newRow = pushOptDT.NewRow();
                            bool anyMismatch = false;
                            bool pushMissing = pushRow == null;

                            for (int col = 0; col < pushData.Columns.Count; col++)
                            {
                                string optVal = opticalRow[col]?.ToString()?.Trim() ?? "";
                                string pushVal = pushMissing ? "" : pushRow[col]?.ToString()?.Trim() ?? "";

                                newRow[$"(Optical)-{pushData.Columns[col].ColumnName}"] = optVal;
                                newRow[$"(Push)-{pushData.Columns[col].ColumnName}"] = pushVal;

                                if (!pushMissing && pushVal != optVal)
                                    anyMismatch = true;
                            }
                            if (pushMissing)
                            {
                                newRow["Status"] = "Push data missing";
                                //misMatchData.Add($"Push packet missing {opticalRow[0]?.ToString()?.Trim() ?? ""}");
                                misMatchData.Add($"Push packet missing at row {pushOptDT.Rows.Count + 1}");
                            }
                            else if (anyMismatch)
                            {
                                newRow["Status"] = "Not match";
                                misMatchData.Add($"Optical and Push value not matched at row {pushOptDT.Rows.Count + 1}");
                                //misMatchData.Add($"Optical and Push value not matched at {opticalRow[0]?.ToString()?.Trim() ?? ""}");
                            }
                            else
                                newRow["Status"] = "Match";

                            pushOptDT.Rows.Add(newRow);
                        }
                    }
                }
                DataTableOperations.ExportDataTableToExcelWithDifferentSheet(pushOptDT, Path.Combine(_logService.LOG_DIRECTORY, $"DownloadedData\\{TestName}\\Validation and Verification Reports\\Push_Optical_Reports.xlsx"), $"{profileName} Push Optical");
            }
            catch (Exception ex)
            {
                log.Error($"[{nameof(BuildAndComparePushOpticalDT)}] - {ex.Message} TRACE:{ex.StackTrace}");
                misMatchData.Add($"Exception: {ex.Message}");
            }
            return misMatchData;
        }
        public List<string> ValidateEnergyColumns(DataTable pushDT, DataTable opticalDT)
        {
            List<string> misMatchData = new List<string>();
            if (pushDT == null || pushDT.Rows.Count == 0 || opticalDT == null || opticalDT.Rows.Count == 0)
            {
                misMatchData.Add("No data available");
                return misMatchData;
            }

            var energyColumns = pushDT.Columns.Cast<DataColumn>().Where(c => c.ColumnName.ToLower().Contains("energy"));
            if (pushDT.Rows.Count > 1)
            {
                foreach (var col in energyColumns)
                {
                    for (int i = 1; i < pushDT.Rows.Count; i++)
                    {
                        double previousEnergy = Convert.ToDouble(pushDT.Rows[i - 1][col]);
                        double currentEnergy = Convert.ToDouble(pushDT.Rows[i][col]);

                        if (currentEnergy < previousEnergy)
                            misMatchData.Add($"Column '{col.ColumnName}' row {i + 1}: {currentEnergy} < {previousEnergy}");
                    }
                }
            }

            if (opticalDT.Rows.Count == 1)
            {
                foreach (var col in energyColumns)
                {
                    var opticalCol = opticalDT.Columns.Cast<DataColumn>().FirstOrDefault(c => c.ColumnName.ToLower().Contains(col.ColumnName.ToLower()));
                    if (opticalCol == null)
                        continue;

                    double opticalEnergy = Convert.ToDouble(opticalDT.Rows[0][opticalCol]);
                    double pushLastEnergy = Convert.ToDouble(pushDT.Rows[pushDT.Rows.Count - 1][col]);

                    if (pushLastEnergy > opticalEnergy)
                        misMatchData.Add($"Column '{col.ColumnName}': Push last value {pushLastEnergy} > Optical value {opticalEnergy}");
                }
            }

            var powerFactorColumns = pushDT.Columns.Cast<DataColumn>().Where(c => c.ColumnName.ToLower().Contains("power factor"));
            foreach (var col in powerFactorColumns)
            {
                for (int i = 1; i < pushDT.Rows.Count; i++)
                {
                    double pf = Convert.ToDouble(pushDT.Rows[i][col]);
                    if (!(pf >= -1 && pf <= 1))
                        misMatchData.Add($"Column '{col.ColumnName}' row {i + 1}: Power Factor is {pf}");
                }
            }
            return misMatchData;
        }
        public DataTable GetAdditionalPSObjetcs(ref DLMSComm DLMSReaderWriter, DataTable pushSetUpObjectsDT, DataTable profilesData, string profile)
        {
            int counter = 0;
            DataTable dt_AddPushOjects = new DataTable();
            DataTable dt_FinalOptical = new DataTable();
            try
            {
                if (profile == "LS")   // as in LS, multiple entries in one push packet
                {
                    int currentLSIP = (Convert.ToInt32(parse.GetProfileValueString(SetGetFromMeter.GetDataFromObject(ref DLMSReaderWriter, 1, "1.0.0.8.4.255", 2))) / 60);
                    counter = freq_LS / currentLSIP;
                    if (counter == 0)  // for eg - lsip = 30 min , freq_LS = 15 min
                        counter = 1;
                }

                for (int i = 3; i < pushSetUpObjectsDT.Rows.Count; i++)
                {
                    dt_AddPushOjects.Columns.Add($"{pushSetUpObjectsDT.Rows[i]["DESCRIPTION"].ToString()}");
                }
                DataRow newRow = dt_AddPushOjects.NewRow();
                for (int i = 3; i < pushSetUpObjectsDT.Rows.Count; i++)
                {
                    int objClass = Convert.ToInt32(pushSetUpObjectsDT.Rows[i]["CLASS"]);
                    string obis = pushSetUpObjectsDT.Rows[i]["OBIS"].ToString();
                    int attribute = Convert.ToInt32(pushSetUpObjectsDT.Rows[i]["ATTRIBUTE"]);
                    string recString = SetGetFromMeter.GetDataFromObject(ref DLMSReaderWriter, objClass, obis, attribute);
                    if (recString.Length > 4)
                        newRow[pushSetUpObjectsDT.Rows[i]["DESCRIPTION"].ToString()] = parse.GetProfileValueString(recString);
                    else
                        newRow[pushSetUpObjectsDT.Rows[i]["DESCRIPTION"].ToString()] = recString;
                }
                dt_AddPushOjects.Rows.Add(newRow);

                dt_FinalOptical.Columns.Add("SN", typeof(int));
                foreach (DataColumn col in dt_AddPushOjects.Columns)
                {
                    string colName = col.ColumnName;
                    int suffix = 1;
                    while (dt_FinalOptical.Columns.Contains(colName))
                        colName = $"{col.ColumnName}_{suffix++}";
                    dt_FinalOptical.Columns.Add(colName, col.DataType);
                }
                if (profilesData.Columns.Count > 0 && profilesData.Columns[0].ColumnName.Trim().Equals("SN", StringComparison.OrdinalIgnoreCase))
                    profilesData.Columns.RemoveAt(0);
                foreach (DataColumn col in profilesData.Columns)
                {
                    string colName = col.ColumnName;
                    int suffix = 1;
                    while (dt_FinalOptical.Columns.Contains(colName))
                        colName = $"{col.ColumnName}_{suffix++}";
                    dt_FinalOptical.Columns.Add(colName, col.DataType);
                }

                for (int i = 0; i < profilesData.Rows.Count; i++)
                {
                    DataRow finalRow = dt_FinalOptical.NewRow();
                    finalRow["SN"] = i + 1;
                    int colIndex = 1;
                    DataRow addRow = dt_AddPushOjects.Rows[0];

                    for (int col = 0; col < dt_AddPushOjects.Columns.Count; col++)//1
                    {
                        if (profile == "LS" && counter != 1)
                        {
                            if (i % counter == 0)
                                finalRow[colIndex++] = addRow[col];
                            else
                                finalRow[colIndex++] = "";
                        }
                        else
                        {
                            finalRow[colIndex++] = addRow[col];
                        }
                    }

                    for (int col = 0; col < profilesData.Columns.Count; col++)
                    {
                        finalRow[colIndex++] = profilesData.Rows[i][col];
                    }
                    dt_FinalOptical.Rows.Add(finalRow);
                }
                DataTableOperations.ExportDataTableToExcelWithDifferentSheet(dt_FinalOptical, Path.Combine(_logService.LOG_DIRECTORY, $"DownloadedData\\{TestName}\\Profile Reports\\Optical_{DeviceID}.xlsx"), $"Appended_{profile}");
            }
            catch (Exception ex)
            {
                log.Error($"[{nameof(GetAdditionalPSObjetcs)}] - {ex.Message} - {ex.StackTrace}");
            }
            return dt_FinalOptical;
        }
        #endregion

        #region Additional Methods (NOT IN USE) 
        /// <summary>
        /// For debug and testing purpose, Print datatble in console
        /// </summary>
        /// <param name="data"></param>
        public void print_results(DataTable data)
        {
            Console.WriteLine();
            Dictionary<string, int> colWidths = new Dictionary<string, int>();

            foreach (DataColumn col in data.Columns)
            {
                Console.Write(col.ColumnName);
                var maxLabelSize = data.Rows.OfType<DataRow>()
                        .Select(m => (m.Field<object>(col.ColumnName)?.ToString() ?? "").Length)
                        .OrderByDescending(m => m).FirstOrDefault();

                colWidths.Add(col.ColumnName, maxLabelSize);
                for (int i = 0; i < maxLabelSize - col.ColumnName.Length + 10; i++) Console.Write(" ");
            }

            Console.WriteLine();

            foreach (DataRow dataRow in data.Rows)
            {
                for (int j = 0; j < dataRow.ItemArray.Length; j++)
                {
                    Console.Write(dataRow.ItemArray[j]);
                    for (int i = 0; i < colWidths[data.Columns[j].ColumnName] - dataRow.ItemArray[j].ToString().Length + 10; i++) Console.Write(" ");
                }
                Console.WriteLine();
            }
        }
        #endregion
        public void ResetRecPushDT()
        {
            dtRec_Push_Instant.Reset(); dtRec_Push_Alert.Reset();
            dtRec_Push_Bill.Reset(); dtRec_Push_CB.Reset();
            dtRec_Push_DE.Reset(); dtRec_Push_LS.Reset();
            dtRec_Push_SR.Reset(); dtRec_Push_Tamper.Reset();
        }
        public static void UpdateProfileDetails()
        {
            int BillFreq = freq_Bill.Substring(0, 2) != "00" ? 1 : 0;
            profileDetails[0] = ("Instant", "Instant", $"0028{GetPS_Obis(PushSetupObisCodes.PushSetupObject.Instant)}", $"0016{GetAS_Obis(ActionScheduleObisCodes.ActionScheduleObject.Instant)}04", freq_Instant, Random_Instant, lst_ExpectedPacketTiming_Instant, lst_ReceivedPacketTiming_Instant);
            profileDetails[1] = ("LS", "Load Survey", $"0028{GetPS_Obis(PushSetupObisCodes.PushSetupObject.LoadSurvey)}", $"0016{GetAS_Obis(ActionScheduleObisCodes.ActionScheduleObject.LoadSurvey)}04", freq_LS, Random_LS, lst_ExpectedPacketTiming_LS, lst_ReceivedPacketTiming_LS);
            profileDetails[2] = ("DE", "Daily Energy", $"0028{GetPS_Obis(PushSetupObisCodes.PushSetupObject.DailyEnergy)}", $"0016{GetAS_Obis(ActionScheduleObisCodes.ActionScheduleObject.DailyEnergy)}04", freq_DE, Random_DE, lst_ExpectedPacketTiming_DE, lst_ReceivedPacketTiming_DE);
            profileDetails[3] = ("SR", "Self Registration", $"0028{GetPS_Obis(PushSetupObisCodes.PushSetupObject.SelfRegistration)}", $"0016{GetAS_Obis(ActionScheduleObisCodes.ActionScheduleObject.SelfRegistration)}04", freq_SR, 0, lst_ExpectedPacketTiming_SR, lst_ReceivedPacketTiming_SR);
            profileDetails[4] = ("CB", "Current Bill", $"0028{GetPS_Obis(PushSetupObisCodes.PushSetupObject.CurrentBill)}", $"0016{GetAS_Obis(ActionScheduleObisCodes.ActionScheduleObject.CurrentBill)}04", freq_CB, Random_CB, lst_ExpectedPacketTiming_CB, lst_ReceivedPacketTiming_CB);
            profileDetails[5] = ("Bill", "Bill", $"0028{GetPS_Obis(PushSetupObisCodes.PushSetupObject.Billing)}", $"0016{GetAS_Obis(ActionScheduleObisCodes.ActionScheduleObject.Billing)}04", BillFreq, Random_Bill, lst_ExpectedPacketTiming_Bill, lst_ReceivedPacketTiming_Bill);
            profileDetails[6] = ("Alert", "Alert", $"0028{GetPS_Obis(PushSetupObisCodes.PushSetupObject.Alert)}", null, 0, 0, null, lst_ReceivedPacketTiming_Alert);
            profileDetails[7] = ("Tamper", "Tamper", $"0028{GetPS_Obis(PushSetupObisCodes.PushSetupObject.Tamper)}", $"0016{GetAS_Obis(ActionScheduleObisCodes.ActionScheduleObject.Instant)}04", 0, 0, null, lst_ReceivedPacketTiming_Tamper);

            string GetPS_Obis(PushSetupObisCodes.PushSetupObject obj)
            {
                string obisHex = DLMSParser.ConvertDecObisToHexObis(PushSetupObisCodes.Get(obj));
                return obisHex;
            }
            string GetAS_Obis(ActionScheduleObisCodes.ActionScheduleObject obj)
            {
                string obisHex = DLMSParser.ConvertDecObisToHexObis(ActionScheduleObisCodes.Get(obj));
                return obisHex;
            }
        }
        public void ExportReports(string filepath)
        {
            try
            {
                if (!(dtRec_Push_Alert.Rows.Count < 1 || dtRec_Push_Alert.Columns.Count < 1))
                    DataTableOperations.ExportDataTableToExcelWithDifferentSheet(dtRec_Push_Alert.Copy(), filepath, "Alert");
                if (!(dtRec_Push_Instant.Rows.Count < 1 || dtRec_Push_Instant.Columns.Count < 1))
                    DataTableOperations.ExportDataTableToExcelWithDifferentSheet(dtRec_Push_Instant.Copy(), filepath, "Instant");
                if (!(dtRec_Push_LS.Rows.Count < 1 || dtRec_Push_LS.Columns.Count < 1))
                    DataTableOperations.ExportDataTableToExcelWithDifferentSheet(dtRec_Push_LS.Copy(), filepath, "Load Survey");
                if (!(dtRec_Push_DE.Rows.Count < 1 || dtRec_Push_DE.Columns.Count < 1))
                    DataTableOperations.ExportDataTableToExcelWithDifferentSheet(dtRec_Push_DE.Copy(), filepath, "Daily Energy");
                if (!(dtRec_Push_SR.Rows.Count < 1 || dtRec_Push_SR.Columns.Count < 1))
                    DataTableOperations.ExportDataTableToExcelWithDifferentSheet(dtRec_Push_SR.Copy(), filepath, "Self Registration");
                if (!(dtRec_Push_CB.Rows.Count < 1 || dtRec_Push_CB.Columns.Count < 1))
                    DataTableOperations.ExportDataTableToExcelWithDifferentSheet(dtRec_Push_CB.Copy(), filepath, "Current Bill");
                if (!(dtRec_Push_Bill.Rows.Count < 1 || dtRec_Push_Bill.Columns.Count < 1))
                    DataTableOperations.ExportDataTableToExcelWithDifferentSheet(dtRec_Push_Bill.Copy(), filepath, "Billing");
                if (!(dtRec_Push_Tamper.Rows.Count < 1 || dtRec_Push_Tamper.Columns.Count < 1))
                    DataTableOperations.ExportDataTableToExcelWithDifferentSheet(dtRec_Push_Tamper.Copy(), filepath, "Tamper");
            }
            catch (Exception ex)
            {
                log.Error($"Error in exporting data [{nameof(ExportReports)}] - {ex.Message} TRACE:{ex.StackTrace}");
            }
        }
    }
}