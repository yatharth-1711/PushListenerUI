using AutoTest.FrameWork;
using AutoTest.FrameWork.Converts;
using log4net;
using log4net.Util;
using MeterComm;
using MeterComm.DLMS;
using OfficeOpenXml;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using LicenseContext = OfficeOpenXml.LicenseContext;
using System.Data.Linq.Mapping;
using System.IO;
using Gurux.DLMS.Objects;
using System.Diagnostics;
using MeterReader.DLMSNetSerialCommunication;
using Gurux.DLMS.Enums;
using AutoTestDesktopWFA;

namespace MeterReader.TestHelperClasses
{
    public static class SetGetFromMeter
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static string sData = string.Empty;
        public static string result = "";
        public static int nRetVal = 100;
        public static DLMSParser parse = new DLMSParser();
        /*public static int GenerateEventID(ref DLMSComm DLMSObj, Int32 eventID, TestConfiguration _testConfig, string dataToSet = "")
        {
            DLMSParser parse = new DLMSParser();
            switch (eventID)
            {
                //Real Time Clock – Date and Time
                case 151:
                    sData = GetDataFromObject(ref DLMSObj, 8, "0.0.1.0.0.255", 2);
                    DateTime dateTime = DateTime.ParseExact(parse.GetProfileValueString(SetGetFromMeter.GetDataFromObject(ref DLMSObj, 8, "0.0.1.0.0.255", 2).Trim()).Trim(), "dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                    dateTime = dateTime.AddSeconds(3);
                    sData = SetGetFromMeter.GetRTCSetString(dateTime);
                    nRetVal = DLMSObj.SetParameter($"0008{string.Concat("0.0.1.0.0.255".Split('.').Select(part => int.Parse(part).ToString("X2")))}02", (byte)0, (byte)3, (byte)3, sData);
                    break;
                //Demand Integration Period
                case 152:
                    sData = GetDataFromObject(ref DLMSObj, 1, "1.0.0.8.0.255", 2);
                    nRetVal = DLMSObj.SetParameter($"0001{string.Concat("1.0.0.8.0.255".Split('.').Select(part => int.Parse(part).ToString("X2")))}02", (byte)0, (byte)3, (byte)3, sData);
                    break;
                //Profile Capture Period
                case 153:
                    sData = GetDataFromObject(ref DLMSObj, 1, "1.0.0.8.4.255", 2);
                    nRetVal = DLMSObj.SetParameter($"0001{string.Concat("1.0.0.8.4.255".Split('.').Select(part => int.Parse(part).ToString("X2")))}02", (byte)0, (byte)3, (byte)3, sData);
                    break;
                //Single-action Schedule for Billing Dates
                case 154:
                    sData = GetDataFromObject(ref DLMSObj, 22, "0.0.15.0.0.255", 4);
                    nRetVal = DLMSObj.SetParameter($"0016{string.Concat("0.0.15.0.0.255".Split('.').Select(part => int.Parse(part).ToString("X2")))}04", (byte)0, (byte)3, (byte)3, sData);
                    break;
                //Activity calendar for time zones
                case 155:
                    nRetVal = Convert.ToInt16(DLMSObj.ActionCmd($"0014{string.Concat("0.0.13.0.0.255".Split('.').Select(part => int.Parse(part).ToString("X2")))}01", (byte)0, (byte)3, (byte)3, "0F00"));
                    break;
                //RS485 device address
                case 156:
                    sData = GetDataFromObject(ref DLMSObj, 23, "0.0.22.0.0.255", 9);
                    nRetVal = DLMSObj.SetParameter($"0017{string.Concat("0.0.15.0.0.255".Split('.').Select(part => int.Parse(part).ToString("X2")))}09", (byte)0, (byte)3, (byte)3, sData);
                    break;
                //Load limit (kW) set
                case 158:
                    sData = GetDataFromObject(ref DLMSObj, 71, "0.0.17.0.0.255", 4);
                    nRetVal = DLMSObj.SetParameter($"00470000110000FF04", (byte)0, (byte)3, (byte)3, sData);
                    break;
                //Load switch connect
                case 159:
                    nRetVal = Convert.ToInt16(DLMSObj.ActionCmd("0046000060030AFF02", (byte)0, (byte)3, (byte)3, string.Empty));
                    break;
                //Load switch disconnect
                case 160:
                    nRetVal = Convert.ToInt16(DLMSObj.ActionCmd("0046000060030AFF01", (byte)0, (byte)3, (byte)3, string.Empty));
                    break;
                //Enabled – load limit function
                case 182:
                    nRetVal = DLMSObj.SetParameter("0046000060030AFF04", (byte)0, (byte)3, (byte)3, "16" + "04");
                    break;
                //Disabled – load limit function
                case 181:
                    nRetVal = DLMSObj.SetParameter("0046000060030AFF04", (byte)0, (byte)3, (byte)3, "16" + "00");
                    break;
                //LLS secret (MR) change
                case 161:
                    byte[] bytes1 = Encoding.ASCII.GetBytes(_testConfig.MeterAuthPassword.Trim().ToString());
                    string empty3 = string.Empty;
                    for (int index = 0; index < bytes1.Length; ++index)
                        empty3 += bytes1[index].ToString("X2");
                    sData = $"09{(empty3.Length / 2).ToString("X2")}" + empty3;
                    nRetVal = DLMSObj.SetParameter($"000F0000280002FF07", (byte)0, (byte)3, (byte)3, sData);
                    break;
                //HLS key (US) change
                case 162:
                    byte[] bytes2 = Encoding.ASCII.GetBytes(_testConfig.MeterAuthPasswordWrite.Trim().ToString());
                    string empty4 = string.Empty;
                    for (int index = 0; index < bytes2.Length; ++index)
                        empty4 += bytes2[index].ToString("X2");
                    sData = $"09{(empty4.Length / 2).ToString("X2")}" + empty4;
                    nRetVal = DLMSObj.SetParameter("000F0000280003FF02", (byte)0, (byte)3, (byte)3, sData);
                    break;
                //HLS key (FW) change
                case 163:
                    byte[] bytes3 = Encoding.ASCII.GetBytes(_testConfig.FWMeterAuthPasswordWrite.Trim().ToString());
                    string empty5 = string.Empty;
                    for (int index = 0; index < bytes3.Length; ++index)
                        empty5 += bytes3[index].ToString("X2");
                    sData = $"09{(empty5.Length / 2).ToString("X2")}" + empty5;
                    nRetVal = DLMSObj.SetParameter("000F0000280005FF02", (byte)0, (byte)3, (byte)3, sData);
                    break;
                //Global key change(encryption and authentication)
                case 164:
                    byte[] bytes4 = Encoding.ASCII.GetBytes(_testConfig.MasterKey.Trim().ToString());
                    byte[] bytes5 = Encoding.ASCII.GetBytes(_testConfig.TxtAK.Trim().ToString());
                    string masterKey = string.Empty;
                    string globalKey = string.Empty;
                    string wrappedKey = string.Empty;
                    for (int index = 0; index < bytes5.Length; ++index)
                    {
                        masterKey += bytes4[index].ToString("X2");
                        globalKey += bytes5[index].ToString("X2");
                    }
                    foreach (byte num3 in AesWrap(masterKey, globalKey))
                        wrappedKey += num3.ToString("X2");
                    nRetVal = DLMSObj.SetParameter("004000002B0003FF02", (byte)0, (byte)3, (byte)3, "0101020216000918" + wrappedKey);
                    break;
                //ESWF change
                case 165:
                    sData = GetDataFromObject(ref DLMSObj, 1, "0.0.94.91.26.255", 2);
                    nRetVal = DLMSObj.SetParameter($"0001{string.Concat("0.0.94.91.26.255".Split('.').Select(part => int.Parse(part).ToString("X2")))}02", (byte)0, (byte)3, (byte)3, sData);
                    break;
                //MD reset
                case 166:
                    string meterInitialRTCData = GetDataFromObject(ref DLMSObj, 8, "0.0.1.0.0.255", 2);
                    string deviationBytes = meterInitialRTCData.Trim().Substring(meterInitialRTCData.Trim().Length - 6, 4);
                    DateTime dateTimeToSet = DateTime.ParseExact(parse.GetProfileValueString(meterInitialRTCData), "dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                    dateTimeToSet = dateTimeToSet.AddDays(1).AddHours(1);
                    dateTimeToSet = new DateTime(dateTimeToSet.Year, dateTimeToSet.Month, dateTimeToSet.Day, dateTimeToSet.Hour, 59, 59);
                    sData = "090C" + Convert.ToInt16(dateTimeToSet.Year).ToString("X4") + Convert.ToByte(dateTimeToSet.Month).ToString("X2") + Convert.ToByte(dateTimeToSet.Day).ToString("X2");
                    if (Convert.ToByte(dateTimeToSet.DayOfWeek) == 0)
                        sData += "07";
                    else
                        sData += Convert.ToByte(dateTimeToSet.DayOfWeek).ToString("X2");
                    sData += Convert.ToByte(dateTimeToSet.Hour).ToString("X2") + Convert.ToByte(dateTimeToSet.Minute).ToString("X2") + Convert.ToByte(dateTimeToSet.Second).ToString("X2") + "00" + deviationBytes + "00";
                    if ((DLMSObj.SetValue(8, "0.0.1.0.0.255", 2, sData, deviationBytes)).Contains("Successfully"))
                    {
                        Wait(5000);
                        nRetVal = DLMSObj.ActionCmd("000900000A0001FF01", (byte)0, (byte)3, (byte)3, string.Empty);
                    }
                    else
                        nRetVal = 1;
                    break;
                //Metering mode
                case 167:
                    sData = GetDataFromObject(ref DLMSObj, 1, "0.0.94.96.19.255", 2);
                    nRetVal = DLMSObj.SetParameter($"0001{string.Concat("0.0.94.96.19.255".Split('.').Select(part => int.Parse(part).ToString("X2")))}02", (byte)0, (byte)3, (byte)3, sData);
                    break;
                //Image activation single action schedule
                case 169:
                    sData = GetDataFromObject(ref DLMSObj, 22, "0.0.15.0.2.255", 4);
                    nRetVal = DLMSObj.SetParameter($"0016{string.Concat("0.0.15.0.2.255".Split('.').Select(part => int.Parse(part).ToString("X2")))}04", (byte)0, (byte)3, (byte)3, sData);
                    break;
                //Configuration change to ‘Forwarded only’ mode
                case 177:
                    sData = "1100";
                    nRetVal = DLMSObj.SetParameter($"0001{string.Concat("0.0.94.96.19.255".Split('.').Select(part => int.Parse(part).ToString("X2")))}02", (byte)0, (byte)3, (byte)3, sData);
                    break;
                //Configuration change to ‘Import-Export’ mode
                case 178:
                    sData = "1101";
                    nRetVal = DLMSObj.SetParameter($"0001{string.Concat("0.0.94.96.19.255".Split('.').Select(part => int.Parse(part).ToString("X2")))}02", (byte)0, (byte)3, (byte)3, sData);
                    break;
            }
            return nRetVal;
        }*/
        public static byte[] AesWrap(string _master, string _global)
        {
            try
            {
                var globalkeystr = _global;
                var masterKeyParam = new KeyParameter(StringToByteArray(_master));
                var globalKeyData = StringToByteArray(globalkeystr);
                Rfc3394WrapEngine wrapEngine = new Rfc3394WrapEngine(new AesEngine());
                wrapEngine.Init(true, masterKeyParam);
                var wrappedData = wrapEngine.Wrap(globalKeyData, 0, globalKeyData.Length);
                return wrappedData;
            }
            catch (Exception)
            {
            }
            return (byte[])null;
        }
        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length).Where<int>((Func<int, bool>)(x => x % 2 == 0)).Select<int, byte>((Func<int, byte>)(x => Convert.ToByte(hex.Substring(x, 2), 16))).ToArray<byte>();
        }
        public static string MDReset(ref DLMSComm DLMSsetter)
        {
            sData = "01120001";
            result = DLMSsetter.SetValue(9, "0.0.10.0.1.255", 1, sData);
            return result;
        }
        public static int SetObjectValue(ref DLMSComm DLMSObj, int _class, string _obis, int _attribute, string sData)
        {
            nRetVal = 100;
            try
            {
                nRetVal = DLMSObj.SetParameter($"{Convert.ToByte(_class).ToString("X4")}{string.Concat(_obis.Trim().Split('.').Select(part => int.Parse(part).ToString("X2")))}{Convert.ToByte(_attribute).ToString("X2")}", (byte)0, (byte)3, (byte)5, sData);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message.ToString());
            }
            return nRetVal;
        }
        public static string GetDataFromObject(ref DLMSComm DLMSReader, int _class, string _obis, int _attribute, bool IsLineTrafficEnabled = true)
        {
            string result = string.Empty;
            DLMSParser parse = new DLMSParser();
            try
            {
                byte bytWait = 0;
                byte bytTimOut = 3;
                string readData = "";
                bool parameter = false;
                parameter = DLMSReader.GetParameter($"{Convert.ToByte(_class).ToString("X4")}{string.Concat(_obis.Trim().Split('.').Select(part => int.Parse(part).ToString("X2")))}{Convert.ToByte(_attribute).ToString("X2")}", bytWait, 3, bytTimOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL, IsLineTrafficEnabled);
                if (parameter)
                {
                    if (DLMSReader.strbldDLMdata.ToString().Trim().Split(' ').Count() > 3)
                    {
                        readData = DLMSReader.strbldDLMdata.ToString().Trim().Split(' ')[3].Trim();
                        result = $"{readData}";
                    }
                    else
                        result = "";
                }
                else
                {
                    log.Error($"Error Getting in Class-{_class} Obis- {_obis} Att. {_attribute}. Received data is: {DLMSReader.strbldDLMdata.ToString().Trim()}");
                }
            }
            catch (Exception ex)
            {
                log.Error("Error in fetching Entries from Utilities.ReadDataFomObject " + ex.Message.ToString());
            }
            return result;
        }
        public static string SetDIP(ref DLMSComm DLMSsetter, Int32 inMinutes)
        {
            Int32 dIPSeconds = inMinutes * 60;
            sData = $"12{dIPSeconds.ToString("X4")}";
            result = DLMSsetter.SetValue(1, "1.0.0.8.0.255", 2, sData);
            return result;
        }
        public static string SetPCP(ref DLMSComm DLMSsetter, Int32 inMinutes)
        {
            Int32 dIPSeconds = inMinutes * 60;
            sData = $"12{dIPSeconds.ToString("X4")}";
            result = DLMSsetter.SetValue(1, "1.0.0.8.4.255", 2, sData);
            return result;
        }
        public static string RTCJump(ref DLMSComm DLMSsetter, ref DateTime DateTimeToSet)
        {
            string Message = string.Empty;
            string meterReadData = string.Empty;
            string deviationBytes = string.Empty;
            meterReadData = GetDataFromObject(ref DLMSsetter, 8, "0.0.1.0.0.255", 2, false);
            deviationBytes = meterReadData.Trim();
            deviationBytes = deviationBytes.Substring(deviationBytes.Length - 6, 4);
            sData = "090C" + Convert.ToInt16(DateTimeToSet.Year).ToString("X4") + Convert.ToByte(DateTimeToSet.Month).ToString("X2") + Convert.ToByte(DateTimeToSet.Day).ToString("X2");
            if (Convert.ToByte(DateTimeToSet.DayOfWeek) == 0)
                sData += "07";
            else
                sData += Convert.ToByte(DateTimeToSet.DayOfWeek).ToString("X2");
            sData += Convert.ToByte(DateTimeToSet.Hour).ToString("X2") + Convert.ToByte(DateTimeToSet.Minute).ToString("X2") + Convert.ToByte(DateTimeToSet.Second).ToString("X2") + "00" + deviationBytes + "00";
            DLMSsetter.strbldDLMdata.Clear();
            Message = DLMSsetter.SetValue(8, "0.0.1.0.0.255", 2, sData, deviationBytes);
            return Message;
        }
        public static void Wait(double nSecValue)
        {
            DateTime dateTime = DateTime.Now.AddMilliseconds(nSecValue);
            while (DateTime.Now < dateTime)
                Application.DoEvents();
        }
        public static void Wait(double nSecValue, CancellationToken token)
        {
            DateTime dateTime = DateTime.Now.AddMilliseconds(nSecValue);
            while (DateTime.Now < dateTime)
            {
                Application.DoEvents();
                if (token.IsCancellationRequested) break;
            }
        }
        public static string SetCalendarActivationDT(ref DLMSComm DLMSsetter, string sData)
        {
            result = string.Empty;
            // sData = string.Empty;
            int nRetVal = 100;

            nRetVal = DLMSsetter.SetParameter("001400000D0000FF0A", (byte)0, (byte)3, (byte)3, sData);
            if (nRetVal == 0)
                result = result + "TOU: Passive Calendar Date and Time Set Successfully, ";
            else if (nRetVal == 2)
                result = result + "TOU: Passive Calendar Date and Time Write Denied, ";
            else if (nRetVal == 1 || nRetVal == 3)
                result = result + "TOU: Passive Calendar Date and Time Error in Setting ";
            return result;
        }
        public static string GetDataStringofActivationDT(ref DateTime DateTimeToSet, string deviationBytes)
        {
            string resultString = string.Empty;
            resultString = "090C" + Convert.ToInt16(DateTimeToSet.Year).ToString("X4") + Convert.ToByte(DateTimeToSet.Month).ToString("X2") + Convert.ToByte(DateTimeToSet.Day).ToString("X2");
            if (Convert.ToByte(DateTimeToSet.DayOfWeek) == 0)
                resultString += "07";
            else
                resultString += Convert.ToByte(DateTimeToSet.DayOfWeek).ToString("X2");
            resultString += Convert.ToByte(DateTimeToSet.Hour).ToString("X2") + Convert.ToByte(DateTimeToSet.Minute).ToString("X2") + Convert.ToByte(DateTimeToSet.Second).ToString("X2") + "00" + MeterIdentity.deviationByte + "00";
            return resultString;
        }
        public static string SetCalendarDayIDProfile(ref DLMSComm DLMSsetter, string sData)
        {
            int nRetVal = 100;
            nRetVal = DLMSsetter.SetParameter("001400000D0000FF09", (byte)0, (byte)3, (byte)3, sData);
            if (nRetVal == 0)
                result = "TOU: Day Profile Set Successfully";
            else if (nRetVal == 2)
                result = "TOU: Day Profile Write Denied, ";
            else if (nRetVal == 1 || nRetVal == 3)
                result = "TOU: Day Profile Error in Setting, ";
            return result;
        }
        public static string SetBillDate(ref DLMSComm DLMSsetter, string sData)
        {
            result = string.Empty;
            int nRetVal = 100;
            nRetVal = DLMSsetter.SetParameter("001600000F0000FF04", (byte)0, (byte)3, (byte)3, sData);
            if (nRetVal == 0)
                result = result + "Bill Date and Time set Successfully ";
            else if (nRetVal == 2)
                result = result + "Bill Date and Time Write Denied, ";
            else if (nRetVal == 1 || nRetVal == 3)
                result = result + "Bill Date and Time Error in Setting ";
            return result;
        }
        /// <summary>
        /// Based on Provided length it will generate the random string.
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GenerateRandomKey(int length = 16)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder result = new StringBuilder(length);
            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                result.Append(chars[random.Next(chars.Length)]);
            }
            return result.ToString();
        }
        /*
        public static void SkipTest(ref TestResult result, TestLogService _logService, ref RichTextBox logBox, string message)
        {
            result.Skipped = true;
            //result.TestStartTime = DateTime.Now;
            result.ResultMessage = message;
            result.Passed = false;
            result.TestEndTime = DateTime.Now;
            result.ExecutionTimeMs = (long)(DateTime.Now - result.TestStartTime).TotalMilliseconds;
            _logService.LogMessage(logBox, message, Color.DeepPink, true);
        }
        /// <summary>
        /// This is called if the User request cancellation token
        /// </summary>
        /// <param name="result"></param>
        /// <param name="_logService"></param>
        /// <param name="logBox"></param>
        /// <param name="stopwatch"></param>
        public static bool StopTest(ref TestResult result, TestLogService _logService, ref RichTextBox logBox, ref Stopwatch stopwatch, System.Threading.CancellationToken token)
        {
            bool IsRequested = false;
            if (token.IsCancellationRequested)
            {
                result.Skipped = false;
                result.TestStartTime = DateTime.Now;
                result.ResultMessage = "Test Stop by User";
                result.Passed = false;
                result.TestEndTime = DateTime.Now;
                result.ExecutionTimeMs = stopwatch.ElapsedMilliseconds;
                _logService.LogMessage(logBox, "Test Stop by User", Color.DeepPink, true);
                IsRequested = true;
            }
            return IsRequested;
        }
        /// <summary>
        /// This Will Reassign the Main Project parameters using the Reference Project Information. 
        /// </summary>
        /// <param name="_testConfig"></param>
        /// <param name="previousConfig"></param>
        /// <returns></returns>
        public static bool PostFGEvents(TestConfiguration _testConfig, TestConfiguration previousConfig)
        {
            bool IsSuccess = false;
            List<bool> status = new List<bool>();
            nRetVal = 100;
            result = "";
            _testConfig.MeterAuthPassword = _testConfig.RefMeterAuthPassword;
            _testConfig.MeterAuthPasswordWrite = _testConfig.RefMeterAuthPasswordWrite;
            _testConfig.FWMeterAuthPasswordWrite = _testConfig.RefFWMeterAuthPasswordWrite;
            _testConfig.TxtEK = _testConfig.RefTxtEK;
            _testConfig.TxtAK = _testConfig.RefTxtAK;
            _testConfig.MasterKey = _testConfig.RefMasterKey;
            _testConfig.AccessMode = 2;
            _testConfig.AddressModeText = "0";
            _testConfig.ApplyTestConfiguration();
            DLMSComm DLMSObj = new DLMSComm(DLMSInfo.comPort, DLMSInfo.BaudRate);
            bool flag = false;
            flag = DLMSObj.SignOnDLMSTrigger();
            if (flag)
            {
                byte[] bytes2 = Encoding.ASCII.GetBytes(previousConfig.MeterAuthPasswordWrite.Trim());
                string empty4 = string.Empty;
                for (int index = 0; index < bytes2.Length; ++index)
                    empty4 += bytes2[index].ToString("X2");
                nRetVal = DLMSObj.SetParameter("000F0000280003FF02", (byte)0, (byte)3, (byte)5, $"09{(empty4.Length / 2).ToString("X2")}" + empty4);
                switch (nRetVal)
                {
                    case 1:
                    case 2:
                        status.Add(false);
                        flag = false;
                        break;
                    default:
                        status.Add(true);
                        break;
                }
                DLMSObj.Dispose();
                Wait(2000);
                _testConfig.MeterAuthPassword = previousConfig.MeterAuthPassword;
                _testConfig.MeterAuthPasswordWrite = previousConfig.MeterAuthPasswordWrite;
                _testConfig.FWMeterAuthPasswordWrite = previousConfig.FWMeterAuthPasswordWrite;
                _testConfig.ApplyTestConfiguration();
                DLMSObj = new DLMSComm(DLMSInfo.comPort, DLMSInfo.BaudRate);
                flag = DLMSObj.SignOnDLMSTrigger();
                if (flag)
                {
                    byte[] bytes1 = Encoding.ASCII.GetBytes(previousConfig.MeterAuthPassword.Trim().ToString());
                    string empty3 = string.Empty;
                    for (int index = 0; index < bytes1.Length; ++index)
                        empty3 += bytes1[index].ToString("X2");
                    nRetVal = DLMSObj.SetParameter("000F0000280002FF07", (byte)0, (byte)3, (byte)5, $"09{(empty3.Length / 2).ToString("X2")}" + empty3);
                    switch (nRetVal)
                    {
                        case 1:
                        case 2:
                            status.Add(false);
                            flag = false;
                            break;
                        default:
                            status.Add(true);
                            break;
                    }
                    DLMSObj.Dispose();
                    Wait(2000);
                    if (MeterIdentity.GetCipherStatus())
                    {
                        _testConfig.MeterAuthPassword = previousConfig.MeterAuthPassword;
                        _testConfig.MeterAuthPasswordWrite = previousConfig.MeterAuthPasswordWrite;
                        _testConfig.FWMeterAuthPasswordWrite = previousConfig.FWMeterAuthPasswordWrite;
                        _testConfig.ApplyTestConfiguration();
                        DLMSObj = new DLMSComm(DLMSInfo.comPort, DLMSInfo.BaudRate);
                        flag = DLMSObj.SignOnDLMSTrigger();
                        if (flag)
                        {
                            byte[] bytes3 = Encoding.ASCII.GetBytes(previousConfig.FWMeterAuthPasswordWrite.Trim().ToString());
                            string empty5 = string.Empty;
                            for (int index = 0; index < bytes3.Length; ++index)
                                empty5 += bytes3[index].ToString("X2");
                            nRetVal = DLMSObj.SetParameter("000F0000280005FF02", (byte)0, (byte)3, (byte)5, $"09{(empty5.Length / 2).ToString("X2")}" + empty5);

                            switch (nRetVal)
                            {
                                case 1:
                                case 2:
                                    status.Add(false);
                                    flag = false;
                                    break;
                                default:
                                    status.Add(true);
                                    break;
                            }
                        }
                        else
                            status.Add(false);
                        DLMSObj.Dispose();
                        Wait(2000);
                        _testConfig.MeterAuthPassword = previousConfig.MeterAuthPassword;
                        _testConfig.MeterAuthPasswordWrite = previousConfig.MeterAuthPasswordWrite;
                        _testConfig.FWMeterAuthPasswordWrite = previousConfig.FWMeterAuthPasswordWrite;
                        _testConfig.ApplyTestConfiguration();
                        DLMSObj = new DLMSComm(DLMSInfo.comPort, DLMSInfo.BaudRate);
                        flag = DLMSObj.SignOnDLMSTrigger();
                        if (flag)
                        {
                            byte[] bytes4 = Encoding.ASCII.GetBytes(_testConfig.MasterKey.Trim());
                            byte[] bytes5 = Encoding.ASCII.GetBytes(previousConfig.RefTxtEK.Trim());
                            string masterKey = string.Empty;
                            string globalKey = string.Empty;
                            string wrappedKey = string.Empty;
                            for (int index = 0; index < bytes5.Length; ++index)
                            {
                                masterKey += bytes4[index].ToString("X2");
                                globalKey += bytes5[index].ToString("X2");
                            }
                            foreach (byte num3 in AesWrap(masterKey, globalKey))
                                wrappedKey += num3.ToString("X2");
                            nRetVal = DLMSObj.SetParameter("004000002B0003FF02", (byte)0, (byte)3, (byte)5, "0101020216000918" + wrappedKey);
                            switch (nRetVal)
                            {
                                case 1:
                                case 2:
                                    status.Add(false);
                                    flag = false;
                                    break;
                                default:
                                    status.Add(true);
                                    break;
                            }
                            DLMSObj.Dispose();
                            Wait(2000);
                        }
                        else
                            status.Add(false);
                    }
                }
                else
                    status.Add(false);
            }
            else
                status.Add(false);
            previousConfig.ApplyTestConfiguration();
            if (status.All(passed => passed))
                IsSuccess = true;
            else
            {
                IsSuccess = false;
                DLMSTestFrm._cancellationToken.Cancel();
            }
            return IsSuccess;
        }*/
        /// <summary>
        /// Just by providing DateTime it will provide DLMS structured Data string of that object for Class ID 8.
        /// </summary>
        /// <param name="dateTimeToSet"></param>
        /// <returns></returns>
        public static string GetRTCSetString(DateTime dateTimeToSet)
        {
            sData = "090C" + Convert.ToInt16(dateTimeToSet.Year).ToString("X4") + Convert.ToByte(dateTimeToSet.Month).ToString("X2") + Convert.ToByte(dateTimeToSet.Day).ToString("X2");
            if (Convert.ToByte(dateTimeToSet.DayOfWeek) == 0)
                sData += "07";
            else
                sData += Convert.ToByte(dateTimeToSet.DayOfWeek).ToString("X2");
            sData += Convert.ToByte(dateTimeToSet.Hour).ToString("X2") + Convert.ToByte(dateTimeToSet.Minute).ToString("X2") + Convert.ToByte(dateTimeToSet.Second).ToString("X2") + "00" + MeterIdentity.deviationByte + "00";
            return sData;
        }
        /// <summary>
        /// This will enable Genus Association
        /// </summary>
        /// <param name="_testConfiguration"></param>
        /// <returns></returns>

        #region For Returning Datatable of Activity Calendar
        //BY YS
        /// <summary>
        /// Converts the Season Profile DLMS string data into a formatted DataTable. 
        /// </summary>
        /// <param name="readDataActive">Hex string representing the active season profile.</param>
        /// <param name="readDataPassive">Hex string representing the passive season profile.</param>
        /// <returns>A DataTable containing the parsed season profile information.</returns>
        public static DataTable SeasonProfileToDataTable(string readDataActive, string readDataPassive)
        {
            DLMSParser parse = new DLMSParser();
            DataTable table = new DataTable();

            // Define columns
            table.Columns.Add("Type", typeof(string));           // "Active" or "Passive"
            table.Columns.Add("Season Name", typeof(string));
            table.Columns.Add("Start DateTime", typeof(string));
            table.Columns.Add("Week Name", typeof(string));

            // Helper method to parse readData and append rows to the table
            void AddSeasonRows(string readData, string type)
            {
                if (string.IsNullOrWhiteSpace(readData)) return;

                string[] seasonsArray = Regex.Split(readData, readData.Substring(4, 4));
                int numberOfSeasons = int.Parse(readData.Substring(2, 2), NumberStyles.HexNumber);

                for (int i = 0; i < numberOfSeasons; i++)
                {
                    string[] structureDataArray = parse.GetStructureValueList(seasonsArray[i + 1]).ToArray();
                    string[] structureValueArray = new string[structureDataArray.Length];

                    for (int j = 0; j < structureDataArray.Length; j++)
                    {
                        structureValueArray[j] = parse.GetProfileValueString(structureDataArray[j]);
                    }

                    string DTString = structureDataArray[1].Trim();

                    string formattedDateTime = $"{Convert.ToInt16(DTString.Substring(10, 2), 16):D2}/" +
                                               $"{Convert.ToInt16(DTString.Substring(8, 2), 16):D2}/" +
                                               $"{Convert.ToInt32(DTString.Substring(4, 4), 16)} | " +
                                               $"{Convert.ToInt16(DTString.Substring(14, 2), 16):D2}:" +
                                               $"{Convert.ToInt16(DTString.Substring(16, 2), 16):D2}:" +
                                               $"{Convert.ToInt16(DTString.Substring(18, 2), 16):D2} | " +
                                               $"{Convert.ToInt16(DTString.Substring(12, 2), 16):D2} | " +
                                               $"{Convert.ToInt16(DTString.Substring(20, 2), 16):D2} | " +
                                               $"{ConvertHexToOffset(DTString.Substring(22, 4))} | " +
                                               $"{Convert.ToInt16(DTString.Substring(26, 2), 16):D2}";

                    DataRow row = table.NewRow();
                    row["Type"] = type;
                    row["Season Name"] = structureValueArray[0];
                    row["Start DateTime"] = formattedDateTime;
                    row["Week Name"] = structureValueArray[2];
                    table.Rows.Add(row);
                }
            }

            // Add both Active and Passive rows
            AddSeasonRows(readDataActive, "Active");
            AddSeasonRows(readDataPassive, "Passive");

            return table;
        }
        /// <summary>
        /// Converts the Week Profile DLMS string data into a structured DataTable.
        /// </summary>
        /// <param name="readDataActive">Hex string representing the active week profile.</param>
        /// <param name="readDataPassive">Hex string representing the passive week profile.</param>
        /// <returns>A DataTable containing the parsed week profile information.</returns>
        public static DataTable WeekProfileToDataTable(string readDataActive, string readDataPassive)
        {
            DLMSParser parse = new DLMSParser();
            DataTable table = new DataTable();

            // Add first column to indicate Active or Passive
            table.Columns.Add("Type", typeof(string));

            // Local method to parse and add data
            void AddWeekRows(string readData, string type)
            {
                if (string.IsNullOrWhiteSpace(readData)) return;

                string[] weekArray = Regex.Split(readData, readData.Substring(4, 4));
                int numberOfWeeks = int.Parse(readData.Substring(2, 2), NumberStyles.HexNumber);

                for (int i = 0; i < numberOfWeeks; i++)
                {
                    string[] structureDataArray = parse.GetStructureValueList(weekArray[i + 1]).ToArray();
                    string[] structureValueArray = new string[structureDataArray.Length];

                    for (int j = 0; j < structureDataArray.Length; j++)
                    {
                        structureValueArray[j] = parse.GetProfileValueString(structureDataArray[j]);
                    }

                    // Dynamically add more columns if needed.
                    string[] dayNames = new[] { "Week Name", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
                    for (int day = table.Columns.Count; day <= structureValueArray.Length; day++)
                    {
                        string columnName = day <= dayNames.Length ? dayNames[day - 1] : $"Extra {day - dayNames.Length + 1}";
                        table.Columns.Add(columnName, typeof(string));
                    }

                    //while (table.Columns.Count - 1 < structureValueArray.Length)
                    //{
                    //    table.Columns.Add($"Value {table.Columns.Count}", typeof(string));
                    //}

                    DataRow row = table.NewRow();
                    row["Type"] = type;

                    for (int j = 0; j < structureValueArray.Length; j++)
                    {
                        row[j + 1] = structureValueArray[j]; // +1 to skip "Type" column
                    }

                    table.Rows.Add(row);
                }
            }

            // Add both Active and Passive rows
            AddWeekRows(readDataActive, "Active");
            AddWeekRows(readDataPassive, "Passive");

            return table;
        }
        /// <summary>
        /// Parses the Day ID Profile DLMS string and returns a detailed DataTable. 
        /// </summary>
        /// <param name="readDataActive">Hex string representing the active day ID profile.</param>
        /// <param name="readDataPassive">Hex string representing the passive day ID profile.</param>
        /// <returns>A DataTable containing parsed day ID profile data.</returns>
        public static DataTable DayIdProfileToDataTable(string readDataActive, string readDataPassive)
        {
            DLMSParser parse = new DLMSParser();
            DataTable table = new DataTable();
            table.Columns.Add("Type", typeof(string));
            table.Columns.Add("Day ID", typeof(string));
            table.Columns.Add("Start Time", typeof(string));
            table.Columns.Add("Script", typeof(string));
            table.Columns.Add("Script Selector", typeof(string));

            void ParseDayId(string readData, string type)
            {
                if (string.IsNullOrWhiteSpace(readData)) return;

                string[] dayIDArray = Regex.Split(readData, readData.Substring(4, 6));
                int numberOfDayID = int.Parse(dayIDArray[0].Substring(2, 2), NumberStyles.HexNumber);

                for (int i = 0; i < numberOfDayID; i++)
                {
                    string dayId = Convert.ToInt32(dayIDArray[i + 1].Substring(0, 2), 16).ToString();
                    string tempStrDayProfile = dayIDArray[i + 1].Substring(2);

                    string[] dayProfileArray = Regex.Split(tempStrDayProfile, tempStrDayProfile.Substring(4, 4));
                    int numberOfDayProfile = int.Parse(tempStrDayProfile.Substring(2, 2), NumberStyles.HexNumber);

                    for (int j = 0; j < numberOfDayProfile; j++)
                    {
                        string[] structureDataArray = parse.GetStructureValueList(dayProfileArray[j + 1]).ToArray();
                        string[] structureValueArray = new string[structureDataArray.Length];

                        for (int k = 0; k < structureDataArray.Length; k++)
                        {
                            if (k == 0)
                            {
                                string hour = int.Parse(structureDataArray[k].Substring(4, 2), NumberStyles.HexNumber).ToString("D2");
                                string min = int.Parse(structureDataArray[k].Substring(6, 2), NumberStyles.HexNumber).ToString("D2");
                                string sec = int.Parse(structureDataArray[k].Substring(8, 2), NumberStyles.HexNumber).ToString("D2");
                                structureValueArray[k] = $"{hour}:{min}:{sec}";
                            }
                            else
                            {
                                structureValueArray[k] = parse.GetProfileValueString(structureDataArray[k]);
                            }
                        }

                        string script = string.Concat(structureValueArray[1].Trim().Split('.').Select(part => int.Parse(part).ToString("X2")));
                        table.Rows.Add(type, dayId, structureValueArray[0], script, structureValueArray[2]);
                    }
                }
            }

            ParseDayId(readDataActive, "Active");
            ParseDayId(readDataPassive, "Passive");

            return table;
        }
        /// <summary>
        /// Merges Season, Week, Day profile DataTables,Calendar Name, Activation Date Time into a single combined DataTable. 
        /// </summary>
        /// <param name="dtSeason">The DataTable containing season profile data.</param>
        /// <param name="dtWeek">The DataTable containing week profile data.</param>
        /// <param name="dtDay">The DataTable containing day ID profile data.</param>
        /// <param name="CalendarName">Calender Name should be passed in  hex format array with first as Active Calender Name and Second String as Passive Calender Name</param>
        /// <param name="ActivationDateTime">Activation Calender can be passed in both parsed or hex string</param>
        /// <returns>A merged DataTable combining season, week, and day profiles.</returns>
        public static DataTable MergedACProfileTable(DataTable dtSeason, DataTable dtWeek, DataTable dtDay, string[] CalendarName = null, string ActivationDateTime = null)
        {
            DataTable result = new DataTable();
            DLMSParser parse = new DLMSParser();

            result.Columns.Add("Section");
            result.Columns.Add("A");
            result.Columns.Add("B");
            result.Columns.Add("C");
            result.Columns.Add("D");
            result.Columns.Add("E");
            result.Columns.Add("F");
            result.Columns.Add("G");
            result.Columns.Add("H");

            // --- Add Activation Time Section ---
            result.Rows.Add("Calendar Activation Time");
            if (ActivationDateTime.Contains(":"))
                result.Rows.Add("Activation Time", ActivationDateTime);
            else
                result.Rows.Add("Activation Time", parse.GetProfileValueString(ActivationDateTime));
            result.Rows.Add();

            // --- Add Calendar Name Section ---
            result.Rows.Add("Calendar Name");
            result.Rows.Add("Active", parse.HexToAscii(CalendarName[0]));
            result.Rows.Add("Passive", parse.HexToAscii(CalendarName[1]));
            result.Rows.Add();

            // --- Add Season Profile Section ---
            result.Rows.Add("Season Profile");
            result.Rows.Add("Type", "Season Name", "Start (dd/MM/yyyy | HH:mm:ss | DOW | HOS | Deviation Byte | Clock Status)", "Week Name");
            foreach (DataRow row in dtSeason.Rows)
            {
                result.Rows.Add(
                    row["Type"],
                    row["Season Name"],
                    row["Start DateTime"],
                    row["Week Name"]
                );
            }
            result.Rows.Add();

            // --- Add Week Profile Section ---
            result.Rows.Add("Week Profile");
            result.Rows.Add("Type", "Week Name", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday");
            foreach (DataRow row in dtWeek.Rows)
            {
                result.Rows.Add(
                    row["Type"],
                    row["Week Name"],
                    row["Monday"],
                    row["Tuesday"],
                    row["Wednesday"],
                    row["Thursday"],
                    row["Friday"],
                    row["Saturday"],
                    row["Sunday"]
                );
            }
            result.Rows.Add();

            // --- Add Day ID Profile Section ---
            result.Rows.Add("Day ID Profile");
            result.Rows.Add("Type", "Day ID", "Start Time", "Script", "Script Selector");
            foreach (DataRow row in dtDay.Rows)
            {
                result.Rows.Add(
                    row["Type"],
                    row["Day ID"],
                    row["Start Time"],
                    row["Script"],
                    row["Script Selector"]
                );
            }
            return result;
        }
        /// <summary>
        /// Downloads the Activity Calendar data into structured Season, Week, and Day profiles, and merges them along with Calendar Names and Activation DateTime into a single combined DataTable.
        /// </summary>
        /// <param name="dtSeason">The DataTable containing season profile data.</param>
        /// <param name="DLMSReader">As Reference</param>
        /// <returns> A merged DataTable containing the full Activity Calendar information </returns>
        public static DataTable DownloadActivityCalender(ref DLMSComm DLMSReader)
        {
            DataTable dtDLMSData = new DataTable();
            DataTable mergedTable = new DataTable();
            DLMSParser parse = new DLMSParser();
            dtDLMSData.Clear();
            dtDLMSData.Columns.Clear();
            dtDLMSData.Columns.Add("Index", typeof(int));
            dtDLMSData.Columns.Add("Data", typeof(string));
            try
            {
                for (int i = 2; i <= 10; i++)
                {
                    string recData = Utilities.GetDataFromObject(ref DLMSReader, 20, "0.0.13.0.0.255", i);
                    dtDLMSData.Rows.Add(i, recData);
                }
                DataTable dtSeason = new DataTable();
                DataTable dtWeek = new DataTable();
                DataTable dtDay = new DataTable();
                string[] calenderName = { dtDLMSData.Rows[0]["Data"].ToString().Substring(4), dtDLMSData.Rows[4]["Data"].ToString().Substring(4) };
                dtSeason = SeasonProfileToDataTable(dtDLMSData.Rows[1]["Data"].ToString(), dtDLMSData.Rows[5]["Data"].ToString());
                dtWeek = WeekProfileToDataTable(dtDLMSData.Rows[2]["Data"].ToString(), dtDLMSData.Rows[6]["Data"].ToString());
                dtDay = DayIdProfileToDataTable(dtDLMSData.Rows[3]["Data"].ToString(), dtDLMSData.Rows[7]["Data"].ToString());
                string ActivationTime = parse.GetProfileValueString(dtDLMSData.Rows[8]["Data"].ToString());
                mergedTable = MergedACProfileTable(dtSeason, dtWeek, dtDay, calenderName, ActivationTime);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message.ToString());
                return mergedTable;
            }
            return mergedTable;
        }
        static int ConvertHexToOffset(string hex)
        {
            // Convert hex string to byte array (Big-Endian)
            byte[] bytes = new byte[]
            {
            Convert.ToByte(hex.Substring(0, 2), 16),
            Convert.ToByte(hex.Substring(2, 2), 16)
            };

            // Convert byte array to signed 16-bit integer
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes); // Ensure correct endianness

            short offsetShort = BitConverter.ToInt16(bytes, 0);
            return offsetShort; // Return offset in minutes
        }
        /// <summary>
        /// Get the Schema of the Activity Calender Data Table with header Attribute and Data
        /// </summary>
        /// <returns></returns>
        public static DataTable GetActivityCalTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Attribute", typeof(Int16));
            dt.Columns.Add("Data", typeof(string));
            dt.Rows.Add(2, "");
            dt.Rows.Add(3, "");
            dt.Rows.Add(4, "");
            dt.Rows.Add(5, "");
            dt.Rows.Add(6, "");
            dt.Rows.Add(7, "");
            dt.Rows.Add(8, "");
            dt.Rows.Add(9, "");
            dt.Rows.Add(10, "");
            return dt;
        }
        /// <summary>
        /// Export pre filled data table activity calender with header Attribute and Data
        /// </summary>
        /// <param name="sourceTable">Data table of activity calender with header Attribute and Data</param>
        /// <param name="filePath">Path where the Data table will be saved.</param>
        public static void ExportActivityCalender(DataTable sourceTable, string filePath)
        {
            DataTable mergedTable = new DataTable();
            DataTable dtSeason = new DataTable();
            DataTable dtWeek = new DataTable();
            DataTable dtDay = new DataTable();
            string[] calenderName = { sourceTable.Rows[0]["Data"].ToString().Substring(4), sourceTable.Rows[4]["Data"].ToString().Substring(4) };
            dtSeason = SeasonProfileToDataTable(sourceTable.Rows[1]["Data"].ToString(), sourceTable.Rows[5]["Data"].ToString());
            dtWeek = WeekProfileToDataTable(sourceTable.Rows[2]["Data"].ToString(), sourceTable.Rows[6]["Data"].ToString());
            dtDay = DayIdProfileToDataTable(sourceTable.Rows[3]["Data"].ToString(), sourceTable.Rows[7]["Data"].ToString());
            string ActivationTime = parse.GetProfileValueString(sourceTable.Rows[8]["Data"].ToString().Trim());
            mergedTable = MergedACProfileTable(dtSeason, dtWeek, dtDay, calenderName, ActivationTime);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Set the license context
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("ActivityCalendar");
                // Load DataTable into worksheet
                worksheet.Cells["A1"].LoadFromDataTable(mergedTable, true);
                // Style the header row
                using (var headerCells = worksheet.Cells[1, 1, 1, mergedTable.Columns.Count])
                {
                    headerCells.Style.Font.Bold = true;
                    headerCells.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerCells.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    headerCells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    headerCells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }

                #region Activation Time Header
                var cell = worksheet.Cells["A2:I2"];
                cell.Merge = true;
                cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                cell.Style.Font.Size = 12;
                cell.Style.Font.Bold = true;
                cell.Style.Font.Name = "Times New Roman";
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Orange);
                cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                //Activation Time Value
                cell = worksheet.Cells["B3:I3"];
                cell.Merge = true;
                cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                cell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                cell = worksheet.Cells["A3:I3"];
                cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                cell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                cell.Style.Font.Size = 11;
                cell.Style.Font.Bold = false;
                cell.Style.Font.Name = "Times New Roman";
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                cell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                #endregion

                #region Calender Name
                cell = worksheet.Cells["A5:I5"];
                cell.Merge = true;
                cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                cell.Style.Font.Size = 12;
                cell.Style.Font.Bold = true;
                cell.Style.Font.Name = "Times New Roman";
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Orange);
                cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                cell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                //Calender Values
                cell = worksheet.Cells["B6:I6"];
                cell.Merge = true;
                cell = worksheet.Cells["B7:I7"];
                cell.Merge = true;
                cell = worksheet.Cells["A6:I7"];
                cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                cell.Style.Font.Size = 11;
                cell.Style.Font.Bold = false;
                cell.Style.Font.Name = "Times New Roman";
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                cell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                #endregion

                #region Seasons Profile
                cell = worksheet.Cells["A9:D9"];
                cell.Merge = true;
                cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                cell.Style.Font.Size = 12;
                cell.Style.Font.Bold = true;
                cell.Style.Font.Name = "Times New Roman";
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Orange);
                cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                cell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                //Season Headers
                cell = worksheet.Cells["A10:D10"];
                cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                cell.Style.Font.Size = 12;
                cell.Style.Font.Bold = true;
                cell.Style.Font.Name = "Times New Roman";
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                cell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //Seasons Value
                cell = worksheet.Cells[$"A11:D{11 + dtSeason.Rows.Count - 1}"];
                cell.Style.Font.Size = 11;
                cell.Style.Font.Bold = false;
                cell.Style.Font.Name = "Times New Roman";
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                cell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                #endregion

                #region Week Profile
                cell = worksheet.Cells[$"A{11 + dtSeason.Rows.Count + 1}:I{11 + dtSeason.Rows.Count + 1}"];
                cell.Merge = true;
                cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                cell.Style.Font.Size = 12;
                cell.Style.Font.Bold = true;
                cell.Style.Font.Name = "Times New Roman";
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Orange);
                cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                cell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                //Week Profile Headers
                cell = worksheet.Cells[$"A{11 + dtSeason.Rows.Count + 2}:I{11 + dtSeason.Rows.Count + 2}"];
                cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                cell.Style.Font.Size = 12;
                cell.Style.Font.Bold = true;
                cell.Style.Font.Name = "Times New Roman";
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                cell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //Week Values
                cell = worksheet.Cells[$"A{11 + dtSeason.Rows.Count + 2 + 1}:I{11 + dtSeason.Rows.Count + 2 + dtWeek.Rows.Count}"];
                cell.Style.Font.Size = 11;
                cell.Style.Font.Bold = false;
                cell.Style.Font.Name = "Times New Roman";
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                cell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                #endregion

                #region Day Id
                cell = worksheet.Cells[$"A{11 + dtSeason.Rows.Count + 3 + dtWeek.Rows.Count + 1}:E{11 + dtSeason.Rows.Count + 3 + dtWeek.Rows.Count + 1}"];
                cell.Merge = true;
                cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                cell.Style.Font.Size = 12;
                cell.Style.Font.Bold = true;
                cell.Style.Font.Name = "Times New Roman";
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Orange);
                cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                cell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                //Day Headers
                cell = worksheet.Cells[$"A{11 + dtSeason.Rows.Count + 3 + dtWeek.Rows.Count + 2}:E{11 + dtSeason.Rows.Count + 3 + dtWeek.Rows.Count + 2}"];
                cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                cell.Style.Font.Size = 12;
                cell.Style.Font.Bold = true;
                cell.Style.Font.Name = "Times New Roman";
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                cell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //Day Values
                cell = worksheet.Cells[$"A{11 + dtSeason.Rows.Count + 3 + dtWeek.Rows.Count + 3}:E{11 + dtSeason.Rows.Count + 3 + dtWeek.Rows.Count + 3 + dtDay.Rows.Count - 1}"];
                cell.Style.Font.Size = 11;
                cell.Style.Font.Bold = false;
                cell.Style.Font.Name = "Times New Roman";
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                cell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                #endregion

                // Auto-fit columns
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                worksheet.Cells[worksheet.Dimension.Address].Style.Font.Name = "Times New Roman";
                // Ensure the directory exists
                string directoryPath = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                FileInfo excelFile = new FileInfo(filePath);
                package.SaveAs(excelFile);
            }
        }
        // End BY YS
        #endregion


        /// <summary>
        /// Return true if object read success.
        /// </summary>
        /// <param name="WrapperObj">WrapperComm Object already signON</param>
        /// <param name="objectType">Which class interface object</param>
        /// <param name="obis">OBIS</param>
        /// <param name="attribute">Attribute</param>
        /// <param name="readData">Received Data from Object</param>
        /// <returns></returns>
        public static bool GetDataFromWrapperObject(ref WrapperComm WrapperObj, ObjectType objectType, string obis, int attribute, out string readData)
        {
            bool IsReceived = false;
            readData = "";
            try
            {
                SetGetFromMeter.Wait(500);
                WrapperObj.ReadCOSEMObject(objectType, obis, attribute);
                readData = WrapperComm.recData.Trim();
                TestStopWatch watch = new TestStopWatch();
                watch.Start();
                while (true)
                {
                    if (string.IsNullOrEmpty(readData))
                    {
                        if (watch.GetElapsedSeconds() > 360)
                        {
                            readData = "";
                            break;
                        }
                        SetGetFromMeter.Wait(500);
                        WrapperObj.ReadCOSEMObject(objectType, obis, attribute);
                        readData = WrapperComm.recData.Trim();
                    }
                    if (!string.IsNullOrEmpty(readData))
                    {
                        IsReceived = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                IsReceived = false;
                log.Error(ex.Message.ToString());
                log.Error(ex.StackTrace.ToString());
            }
            return IsReceived;

        }
        /*public static bool ExecuteFG(ref TestResult result, TestLogService _logService, ref RichTextBox logBox, ref Stopwatch stopwatch, bool IsOnlyDataReset = false)
        {
            bool IsSuccess = false;
            _logService.LogMessage(logBox, $"Performing FG using \"{FGCommandHelper.FGFilePath}\"", Color.DeepPink);
            bool statusofFG = IsOnlyDataReset ? FGCommandHelper.PerformDataResetFG() : FGCommandHelper.PerformFG();
            if (!statusofFG)
            {
                _logService.LogMessage(logBox, $"Following parameters not set in FG process. Kindly provide valid FG File:", Color.Red, true);
                _logService.LogMessage(logBox, $"\tCmdPkt\t\t\tDataPkt", Color.Black, true);
                foreach (var item in FGCommandHelper.FGFailParametersList)
                {
                    _logService.LogMessage(logBox, $"\t{item.CmdPkt}\t{item.DataPkt}", Color.Black, true);
                }
                DLMSTestFrm._cancellationToken.Cancel();
                result.ResultMessage = "FG Execution Failed";
                result.Passed = false;
                stopwatch.Stop();
                result.TestEndTime = DateTime.Now;
                result.ExecutionTimeMs = stopwatch.ElapsedMilliseconds;
            }
            else
                IsSuccess = true;
            return IsSuccess;
        }*/
    }
}
