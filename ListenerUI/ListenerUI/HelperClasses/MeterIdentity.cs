using AutoTest.FrameWork;
using AutoTest.FrameWork.Converts;
using AutoTestDesktopWFA;
using log4net;
using MeterComm;
using MeterComm.DLMS;
using MeterReader.DLMSNetSerialCommunication;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MeterReader.TestHelperClasses
{
    public static class MeterIdentity
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #region Properties
        public static string ConnectionType { get; set; } = "";
        public static string AccuracyClass { get; set; } = "";
        public static string CategoryWCorCT { get; set; } = "";
        public static double Vref { get; set; } = 0.0;
        public static double Ib { get; set; } = 0.0;
        public static double Imax { get; set; } = 0.0;
        public static string SerialNumber { get; set; } = "";
        public static string FirmwareVersion { get; set; } = "";
        public static string CategoryDLMS { get; set; } = "";
        public static string Manufacturer { get; set; } = "";
        public static string COSEMLogicalDevice { get; set; } = "";
        public static string DeviceID { get; set; } = "";
        public static string SAPAssignment { get; set; } = "";
        public static string LLSPassword { get; set; } = "";
        public static string HLSPasswordUS { get; set; } = "";
        public static string HLSPasswordFW { get; set; } = "";
        public static string EK { get; set; } = "";
        public static string AK { get; set; } = "";
        public static long InActivityTimeout { get; set; } = 0;
        public static long InterFrameTimeout { get; set; } = 0;
        public static long ResponseTimeout { get; set; } = 0;
        public static long DISCToNDMTimeout { get; set; } = 0;
        public static int HDLCBaud { get; set; } = 0;
        public static Dictionary<string, string> UIIdentity { get; set; } = new Dictionary<string, string>();
        public static string MeterConstant { get; set; } = "";

        public static bool IsSaveLineTrafficONDispose = true;

        public static string deviationByte = "014A";

        public static string SIMHostName = "";
        #endregion

        /*public static bool AssignMeterDetails(TestConfiguration _testConfig, CancellationToken token)
        {
            bool result = true;
            bool SignOnDLMSStatus = false;
            string meterRatingData = string.Empty;
            string meterDetails = "";
            TestConfiguration previousConfig = _testConfig.Clone();
            LLSPassword = previousConfig.MeterAuthPassword;
            HLSPasswordUS = previousConfig.MeterAuthPasswordWrite;
            HLSPasswordFW = previousConfig.FWMeterAuthPasswordWrite;
            EK = previousConfig.TxtEK;
            AK = previousConfig.TxtAK;
            InActivityTimeout = previousConfig.InactivityTimeout;
            InterFrameTimeout = previousConfig.InterFrameTimeout;
            ResponseTimeout = previousConfig.ResponseTimeout;
            DISCToNDMTimeout = previousConfig.DISCToNDMTimeout;
            HDLCBaud = previousConfig.BaudRate;
            _testConfig.AccessMode = 2;
            _testConfig.AddressModeText = "0";
            _testConfig.IsLNWithCipher = false;
            _testConfig.ApplyTestConfiguration();
            DLMSComm DLMSObj = new DLMSComm(DLMSInfo.comPort, DLMSInfo.BaudRate);
            DLMSObj.nTryCount = 1;
            DLMSParser parse = new DLMSParser();
            try
            {
                SignOnDLMSStatus = DLMSObj.SignOnDLMS();
                if (!SignOnDLMSStatus)
                {
                    DLMSObj.Dispose();
                    SetGetFromMeter.Wait(_testConfig.DISCToNDMTimeout);
                    _testConfig.IsLNWithCipher = true;
                    _testConfig.ApplyTestConfiguration();
                    DLMSObj = new DLMSComm(DLMSInfo.comPort, DLMSInfo.BaudRate);
                    DLMSObj.nTryCount = 1;
                    SignOnDLMSStatus = DLMSObj.SignOnDLMS();
                    if (!SignOnDLMSStatus)
                    {
                        CommonHelper.DisplayDLMSSignONError();
                        DLMSObj.Dispose();
                        result = false;
                        return result;
                    }
                }
                if (token.IsCancellationRequested)
                {
                    DLMSObj.Dispose();
                    result = false;
                    return result;
                }

                meterRatingData = SetGetFromMeter.GetDataFromObject(ref DLMSObj, 1, "1.0.0.2.0.255", 2);//Firmware Version For Meter
                if (meterRatingData.Substring(2, 2) == "08")
                {
                    meterRatingData = SetGetFromMeter.GetDataFromObject(ref DLMSObj, 1, "0.0.0.2.1.255", 2);//New Firmware Version For Meter
                }
                if (meterRatingData.ToString().Substring(4, 2) == "47" || meterRatingData.ToString().Substring(4, 2) == "54")
                {
                    FirmwareVersion = Utilities.GetCompanyName(meterRatingData.ToString());
                    meterDetails = Utilities.ConvertMeterType(int.Parse(FirmwareVersion.Substring(1, 4), NumberStyles.HexNumber));
                }
                else
                {
                    if (meterRatingData.ToString().Substring(0, 2) == "0A")
                    {
                        FirmwareVersion = Utilities.GetCompanyName(meterRatingData);
                        string temphexString = FirmwareVersion.Substring(6, FirmwareVersion.Length - 6);
                        temphexString = MeterForm.DecimalToHexadimalPostTab(temphexString);
                        meterDetails = Utilities.ConvertMeterType(int.Parse(temphexString, NumberStyles.HexNumber));
                    }
                    else
                    {
                        FirmwareVersion = Utilities.GetCompanyName(meterRatingData.ToString().Substring(0, meterRatingData.ToString().Length - 4)) + Convert.ToInt32(meterRatingData.ToString().Substring(meterRatingData.ToString().Length - 4), 16).ToString();
                        meterDetails = Utilities.ConvertMeterType(int.Parse(meterRatingData.ToString().Substring(meterRatingData.ToString().Length - 4, 4), NumberStyles.HexNumber));
                    }
                }
                ConnectionType = meterDetails.Split(',')[0].Trim();
                AccuracyClass = meterDetails.Split(',')[1].Trim();
                CategoryWCorCT = meterDetails.Split(',')[2].Trim();
                Vref = Convert.ToDouble(meterDetails.Split(',')[3].Trim().Split(' ')[0].Trim());
                Ib = Convert.ToDouble(meterDetails.Split(',')[4].Trim().Split('-')[0].Trim());
                Imax = Convert.ToDouble(meterDetails.Split(',')[4].Trim().Split('-')[1].Trim().Split(' ')[0].Trim());
                if (token.IsCancellationRequested)
                {
                    DLMSObj.Dispose();
                    result = false;
                    return result;
                }
                CategoryDLMS = parse.GetProfileValueString(SetGetFromMeter.GetDataFromObject(ref DLMSObj, 1, "0.0.94.91.11.255", 2));//Meter Category
                if (token.IsCancellationRequested)
                {
                    DLMSObj.Dispose();
                    result = false;
                    return result;
                }
                SerialNumber = parse.GetProfileValueString(SetGetFromMeter.GetDataFromObject(ref DLMSObj, 1, "0.0.96.1.0.255", 2));//Meter Serial Number
                if (token.IsCancellationRequested)
                {
                    DLMSObj.Dispose();
                    result = false;
                    return result;
                }
                Manufacturer = parse.GetProfileValueString(SetGetFromMeter.GetDataFromObject(ref DLMSObj, 1, "0.0.96.1.1.255", 2));//Manufacturer Name
                if (token.IsCancellationRequested)
                {
                    DLMSObj.Dispose();
                    result = false;
                    return result;
                }
                COSEMLogicalDevice = parse.GetProfileValueString(SetGetFromMeter.GetDataFromObject(ref DLMSObj, 1, "0.0.42.0.0.255", 2));//COSEM Logical Device Name
                if (token.IsCancellationRequested)
                {
                    DLMSObj.Dispose();
                    result = false;
                    return result;
                }
                DeviceID = parse.GetProfileValueString(SetGetFromMeter.GetDataFromObject(ref DLMSObj, 1, "0.0.96.1.2.255", 2));//Device ID
                SAPAssignment = parse.GetProfileValueString(SetGetFromMeter.GetDataFromObject(ref DLMSObj, 17, "0.0.41.0.0.255", 2));//SAP Assignment
                if (SAPAssignment.Substring(0, 4) == "0101")
                    SAPAssignment = parse.GetProfileValueString(SAPAssignment.Substring(4));
                if (!(CategoryDLMS == "C1" || CategoryDLMS == "C2" || CategoryDLMS == "C3" || CategoryDLMS == "A" || CategoryDLMS == "B"))
                {
                    SIMHostName = parse.GetProfileValueString(SetGetFromMeter.GetDataFromObject(ref DLMSObj, 48, "0.0.25.7.0.255", 4).Substring(4));
                    if (!string.IsNullOrEmpty(SIMHostName))
                    {
                        WrapperInfo.hostName = SIMHostName.ToString().Trim();
                        _testConfig.hostName = SIMHostName.ToString().Trim();
                    }
                }
                if (token.IsCancellationRequested)
                {
                    DLMSObj.Dispose();
                    result = false;
                    return result;
                }
                string mRTC = SetGetFromMeter.GetDataFromObject(ref DLMSObj, 8, "0.0.1.0.0.255", 2).Trim();
                deviationByte = mRTC.Substring(mRTC.Length - 6, 4);
                DLMSObj.SetDISCMode();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message.ToString());
                result = false;
            }
            finally
            {
                WrapperInfo.IsCommDelayRequired = true;
                DLMSObj.Dispose();
                previousConfig.ApplyTestConfiguration();
                SetGetFromMeter.Wait(_testConfig.DISCToNDMTimeout);
            }
            return result;
        }*/
        public static void PrintIdentificationToSummary(string resultFile)
        {
            File.AppendAllText(resultFile, $"******************" + Environment.NewLine);
            File.AppendAllText(resultFile, $"* Identification *" + Environment.NewLine);
            File.AppendAllText(resultFile, $"******************" + Environment.NewLine);
            File.AppendAllText(resultFile, Environment.NewLine);
            File.AppendAllText(resultFile, $"\tManufacturer:\t\t\t\t{Manufacturer}" + Environment.NewLine);
            File.AppendAllText(resultFile, $"\tDevice ID:\t\t\t\t\t{DeviceID}" + Environment.NewLine);
            File.AppendAllText(resultFile, $"\tSerial Number:\t\t\t\t{SerialNumber}" + Environment.NewLine);
            File.AppendAllText(resultFile, $"\tFirmware Version:\t\t\t{FirmwareVersion}" + Environment.NewLine);
            File.AppendAllText(resultFile, $"\tCOSEM Logical Device:\t\t{COSEMLogicalDevice}" + Environment.NewLine);
            File.AppendAllText(resultFile, $"\tSAP Assignment:\t\t\t\t{SAPAssignment}" + Environment.NewLine);
            File.AppendAllText(resultFile, $"\tDLMS CAT:\t\t\t\t\t{CategoryDLMS}" + Environment.NewLine);
            File.AppendAllText(resultFile, $"\tWire Connection Type:\t\t{ConnectionType}" + Environment.NewLine);
            File.AppendAllText(resultFile, $"\tAccuracy Class:\t\t\t\t{AccuracyClass}" + Environment.NewLine);
            File.AppendAllText(resultFile, $"\tCategory:\t\t\t\t\t{CategoryWCorCT}" + Environment.NewLine);
            File.AppendAllText(resultFile, $"\tReference Voltage (Vref):\t{Vref} V" + Environment.NewLine);
            File.AppendAllText(resultFile, $"\tBase Current (Ib):\t\t\t{Ib} A" + Environment.NewLine);
            File.AppendAllText(resultFile, $"\tMaximum Current (Imax):\t\t{Imax} A" + Environment.NewLine);
            File.AppendAllText(resultFile, Environment.NewLine);
        }
        public static void PrintHDLCDataLinkToSummary(string resultFile)
        {
            File.AppendAllText(resultFile, $"*********************************" + Environment.NewLine);
            File.AppendAllText(resultFile, $"* HDLC and Data Link Layer Info *" + Environment.NewLine);
            File.AppendAllText(resultFile, $"*********************************" + Environment.NewLine);
            File.AppendAllText(resultFile, Environment.NewLine);
            File.AppendAllText(resultFile, $"\tHDLC Baud:\t\t\t\t\t{HDLCBaud}" + Environment.NewLine);
            //File.AppendAllText(resultFile, $"\tLLS Password:\t\t\t\t{LLSPassword}" + Environment.NewLine);
            //File.AppendAllText(resultFile, $"\tHLS Password (US):\t\t\t{HLSPasswordUS}" + Environment.NewLine);
            //File.AppendAllText(resultFile, $"\tHLS Password (FW):\t\t\t{HLSPasswordFW}" + Environment.NewLine);
            //File.AppendAllText(resultFile, $"\tEK:\t\t\t\t\t\t\t{EK}" + Environment.NewLine);
            //File.AppendAllText(resultFile, $"\tAK:\t\t\t\t\t\t\t{AK}" + Environment.NewLine);
            File.AppendAllText(resultFile, $"\tInactivity Timeout:\t\t\t{InActivityTimeout}" + Environment.NewLine);
            File.AppendAllText(resultFile, $"\tInter Frame Timeout:\t\t{InterFrameTimeout}" + Environment.NewLine);
            File.AppendAllText(resultFile, $"\tResponseTimeout:\t\t\t{ResponseTimeout}" + Environment.NewLine);
            File.AppendAllText(resultFile, $"\tDISC To NDM Timeout:\t\t{DISCToNDMTimeout}" + Environment.NewLine);
            File.AppendAllText(resultFile, Environment.NewLine);
        }
        public static void PrintSystemDetailsToSummary(string resultFile)
        {
            File.AppendAllText(resultFile, $"******************************" + Environment.NewLine);
            File.AppendAllText(resultFile, $"* Running System Information *" + Environment.NewLine);
            File.AppendAllText(resultFile, $"******************************" + Environment.NewLine);
            File.AppendAllText(resultFile, Environment.NewLine);
            File.AppendAllText(resultFile, $"\tMachine Name:\t\t\t\t{Environment.MachineName}" + Environment.NewLine);
            File.AppendAllText(resultFile, $"\tMAC Address:\t\t\t\t{GetMacAddress()}" + Environment.NewLine);
            File.AppendAllText(resultFile, $"\tUser Name:\t\t\t\t\t{Environment.UserName}" + Environment.NewLine);
            File.AppendAllText(resultFile, Environment.NewLine);
        }
        static string GetMacAddress()
        {
            var mac = NetworkInterface.GetAllNetworkInterfaces()
               .Where(nic => nic.OperationalStatus == OperationalStatus.Up &&
                             nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
               .Select(nic => nic.GetPhysicalAddress().GetAddressBytes())
               .FirstOrDefault();

            return mac == null || mac.Length == 0
                ? "MAC Address not found"
                : string.Join(":", mac.Select(b => b.ToString("X2")));
        }
        public static bool GetCipherStatus()
        {
            bool result = true;
            switch (CategoryDLMS)
            {
                case "C1":
                case "C2":
                case "C3":
                case "A":
                case "B":
                    result = false;
                    break;
                default:
                    result = true;
                    break;
            }
            return result;
        }
        public static string GetMeterDetails()
        {
            return $"{FirmwareVersion}, {ConnectionType}, {AccuracyClass}, {CategoryWCorCT}, {Vref} V, {Ib}-{Imax} A, {CategoryDLMS}";
        }
        [Serializable]
        public class IdentityInDashboardSettings
        {
            public string ConnectionType { get; set; } = "";
            public string AccuracyClass { get; set; } = "";
            public string CategoryWCorCT { get; set; } = "";
            public string Vref { get; set; } = "";
            public string Ib { get; set; } = "";
            public string Imax { get; set; } = "";
            public string SerialNumber { get; set; } = "";
            public string FirmwareVersion { get; set; } = "";
            public string CategoryDLMS { get; set; } = "";
            public string Manufacturer { get; set; } = "";
            public string COSEMLogicalDevice { get; set; } = "";
            public string DeviceID { get; set; } = "";
            public string SAPAssignment { get; set; } = "";
            public string MeterConstant { get; set; } = "";
        }
    }
}
