using AutoTest.FrameWork.Converts;
using log4net;
using MeterComm.DLMS;
using MeterComm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Util;
using AutoTest.FrameWork;
using System.Text.RegularExpressions;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using MeterReader.DLMSNetSerialCommunication;

namespace MeterReader.TestHelperClasses
{
    public static class PushSetupInfo
    {
        //Logger
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region Properties
        public static bool IsInstantPushAvailable = false;
        public static bool IsAlertPushAvailable = false;
        public static bool IsLSPushAvailable = false;
        public static bool IsDEPushAvailable = false;
        public static bool IsSelfRegPushAvailable = false;
        public static bool IsBillAvailable = false;
        public static bool IsTamperPushAvailable = false;
        public static bool IsPushWriteRequired = true;

        public static DataTable InstantDT { get; set; } = new DataTable();
        public static DataTable AlertDT { get; set; } = new DataTable();
        public static DataTable LSDT { get; set; } = new DataTable();
        public static DataTable DEDT { get; set; } = new DataTable();
        public static DataTable SelfRegDT { get; set; } = new DataTable();
        public static DataTable BillDT { get; set; } = new DataTable();
        public static DataTable TamperDT { get; set; } = new DataTable();
        public static string[] columnArray { get; } = { "SN", "DESCRIPTION", "CLASS", "OBIS", "ATTRIBUTE INDEX", "DATA INDEX", "DATA ANNOTATION INDIVIDUAL", "SCALER MUTIPLIER", "UNIT", "DATA", "VALUE", "REMARK" };
        public static DataTable[] PushSetupDTArray { get; set; } = { InstantDT, AlertDT, LSDT, DEDT, SelfRegDT, BillDT, TamperDT };
        public static List<string> pushSetupList = new List<string>() {
                                                    "0.0.25.9.0.255",
                                                    "0.4.25.9.0.255",
                                                    "0.5.25.9.0.255",
                                                    "0.6.25.9.0.255",
                                                    "0.130.25.9.0.255",
                                                    "0.132.25.9.0.255",
                                                    "0.134.25.9.0.255"};
        #endregion

        /// <summary>
        /// This will initialize all Push Setup DataTable
        /// </summary>
        public static void IniAllPushSetupTableColumns()
        {
            try
            {
                for (int i = 0; i < PushSetupDTArray.Length; i++)
                {
                    if (PushSetupDTArray[i].Columns.Count > 0)
                        PushSetupDTArray[i].Columns.Clear();
                    foreach (var colName in columnArray)
                    {
                        PushSetupDTArray[i].Columns.Add(colName, typeof(string));
                    }
                    PushSetupDTArray[i].AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message.ToString());
            }

        }

        /// <summary>
        /// It reads all available Push Setup objects and update the status of available Push Setup object and corresponding data table. It takes test configuration as input.
        /// </summary>
        /// <param name="_testConfig"></param>
        public static bool ReadPushSetup(TestConfiguration _testConfig)
        {
            bool readStatus = true;
            IniAllPushSetupTableColumns();
            TestConfiguration previousConfig = _testConfig.Clone();
            _testConfig.AccessMode = 2;
            _testConfig.AddressModeText = "0";
            _testConfig.IsLNWithCipher = true;
            _testConfig.ApplyTestConfiguration();
            DLMSComm DLMSObj = new DLMSComm(DLMSInfo.comPort, DLMSInfo.BaudRate);
            DLMSParser parse = new DLMSParser();
            bool SignOnDLMSStatus = false;
            string obisData = "";
            string objectData = "";
            try
            {
                SignOnDLMSStatus = DLMSObj.SignOnDLMS();
                if (!SignOnDLMSStatus)
                {
                    CommonHelper.DisplayDLMSSignONError();
                    DLMSObj.Dispose();
                    previousConfig.ApplyTestConfiguration();
                    _testConfig = previousConfig.Clone();
                    readStatus = false;
                    return readStatus;
                }
                foreach (string selectedOBIS in pushSetupList)
                {
                    obisData = "";
                    obisData = SetGetFromMeter.GetDataFromObject(ref DLMSObj, 40, selectedOBIS, 1);
                    if (obisData != "0B" || obisData != "0D")
                    {
                        obisData = parse.GetProfileValueString(obisData);
                        objectData = SetGetFromMeter.GetDataFromObject(ref DLMSObj, 40, selectedOBIS, 2);
                    }
                    switch (obisData)
                    {
                        ///Instant
                        case "0.0.25.9.0.255":
                            IsInstantPushAvailable = true;
                            FillTable(ref DLMSObj, ref parse, InstantDT, objectData);
                            break;
                        ///Alert
                        case "0.4.25.9.0.255":
                            IsAlertPushAvailable = true;
                            FillTable(ref DLMSObj, ref parse, AlertDT, objectData);
                            break;
                        ///LS
                        case "0.5.25.9.0.255":
                            IsLSPushAvailable = true;
                            FillTable(ref DLMSObj, ref parse, LSDT, objectData);
                            break;
                        ///DE
                        case "0.6.25.9.0.255":
                            IsDEPushAvailable = true;
                            FillTable(ref DLMSObj, ref parse, DEDT, objectData);
                            break;
                        ///Self
                        case "0.130.25.9.0.255":
                            IsSelfRegPushAvailable = true;
                            FillTable(ref DLMSObj, ref parse, SelfRegDT, objectData);
                            break;
                        ///Bill
                        case "0.132.25.9.0.255":
                            IsBillAvailable = true;
                            FillTable(ref DLMSObj, ref parse, BillDT, objectData);
                            break;
                        ///Tamper
                        case "0.134.25.9.0.255":
                            IsTamperPushAvailable = true;
                            FillTable(ref DLMSObj, ref parse, TamperDT, objectData);
                            break;
                    }
                }
                DLMSObj.SetDISCMode();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message.ToString());
            }
            finally
            {
                WrapperInfo.IsCommDelayRequired = true;
                DLMSObj.Dispose();
                previousConfig.ApplyTestConfiguration();
                _testConfig = previousConfig.Clone();
                SetGetFromMeter.Wait(_testConfig.DISCToNDMTimeout);
            }
            return readStatus;
        }

        /// <summary>
        /// Write the Push Setup objects destination address according to object available status. It takes test configuration as input.
        /// </summary>
        /// <param name="_testConfig"></param>
        /// <returns>false if set status is true and not get set. Also if IPv6 not configured it return false else true.</returns>
        public static bool WritePushSetupDestination(TestConfiguration _testConfig)
        {
            string ipv6Address = "";
            string destinationAddress = "";
            bool result = true;
            if (NetworkHelper.IsIPv6Configured())
            {
                ipv6Address = NetworkHelper.GetIPv6Address();
                if (!string.IsNullOrEmpty(ipv6Address))
                {
                    //[2403:8600:2090:14::25]:4059
                    destinationAddress = $"[{ipv6Address}]:4059";
                }
                else
                {
                    result = false;
                    return result;
                }
            }
            TestConfiguration previousConfig = _testConfig.Clone();
            _testConfig.AccessMode = 2;
            _testConfig.AddressModeText = "0";
            _testConfig.IsLNWithCipher = true;
            _testConfig.ApplyTestConfiguration();
            DLMSComm DLMSObj = new DLMSComm(DLMSInfo.comPort, DLMSInfo.BaudRate);
            DLMSParser parse = new DLMSParser();
            bool SignOnDLMSStatus = false;
            int nRetVal = 100;
            try
            {
                SignOnDLMSStatus = DLMSObj.SignOnDLMS();
                if (!SignOnDLMSStatus)
                {
                    CommonHelper.DisplayDLMSSignONError();
                    DLMSObj.Dispose();
                    previousConfig.ApplyTestConfiguration();
                    _testConfig = previousConfig.Clone();
                    result = false;
                    return result;

                }
                destinationAddress = DLMSParser.ConvertAsciiToHex(destinationAddress.Trim());
                List<bool> pushAvailableList = new List<bool>();
                pushAvailableList.Add(IsInstantPushAvailable);
                pushAvailableList.Add(IsAlertPushAvailable);
                pushAvailableList.Add(IsLSPushAvailable);
                pushAvailableList.Add(IsDEPushAvailable);
                pushAvailableList.Add(IsSelfRegPushAvailable);
                pushAvailableList.Add(IsBillAvailable);
                pushAvailableList.Add(IsTamperPushAvailable);
                for (int i = 0; i < pushAvailableList.Count; i++)
                {
                    if (pushAvailableList[i])
                    {
                        nRetVal = SetGetFromMeter.SetObjectValue(ref DLMSObj, 40, pushSetupList[i], 3, $"0203160009{(destinationAddress.Length / 2).ToString("X2")}{destinationAddress}1600");
                        if (nRetVal != 0)
                            result = false;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message.ToString());
            }
            finally
            {
                WrapperInfo.IsCommDelayRequired = true;
                DLMSObj.Dispose();
                previousConfig.ApplyTestConfiguration();
                _testConfig = previousConfig.Clone();
                SetGetFromMeter.Wait(_testConfig.DISCToNDMTimeout);
            }
            return result;
        }

        /// <summary>
        /// Ii takes DLMSComm ref object, DLMSParser ref object, DataTable in which data need to be fill and Array data in hex form.
        /// </summary>
        /// <param name="dLMSComm"></param>
        /// <param name="parse"></param>
        /// <param name="dt"></param>
        /// <param name="recData"></param>
        private static void FillTable(ref DLMSComm dLMSComm, ref DLMSParser parse, DataTable dt, string recData)
        {
            if (recData == "0100")
                return;
            if (dt.Rows.Count > 0)
                dt.Rows.Clear();
            int classID = 0, attIndex = 0, dataIndex = 0;
            string name = "", OBIS = "", dataAnnot = "", scalerData = "", scaler = "", unit = "";
            List<string> ObjectList = new List<string>();
            string[] splittedArray = Regex.Split(recData, "020412");
            int objectCount = Convert.ToInt32(splittedArray[0].Trim().Substring(2), 16);
            for (int i = 1; i < splittedArray.Length; i++)
            {
                classID = Convert.ToInt32(splittedArray[i].Trim().Substring(0, 4), 16);
                OBIS = parse.GetProfileValueString(splittedArray[i].Trim().Substring(4, 16));
                attIndex = Convert.ToInt32(splittedArray[i].Trim().Substring(22, 2), 16);
                dataIndex = Convert.ToInt32(splittedArray[i].Trim().Substring(26, 4), 16);
                name = parse.GetParameterName(classID.ToString(), OBIS);
                if (classID == 3 || classID == 4 || classID == 5)
                {
                    scalerData = SetGetFromMeter.GetDataFromObject(ref dLMSComm, classID, OBIS, 3);
                    scaler = ProfileSheetConfiguration.standardScalerList.FirstOrDefault(s => s.Contains($"{scalerData.Substring(6, 2)}"));
                    //scaler = (string)parse.ScalerFactorhshTable[scalerData.Substring(6, 2)];
                    unit = ProfileSheetConfiguration.standardUnitList.FirstOrDefault(s => s.Contains($"{scalerData.Substring(10, 2)}"));
                    //unit = (string)parse.UnithshTable[scalerData.Substring(10, 2)];
                }
                else
                {
                    scaler = "";
                    unit = "";
                }
                if (classID == 7 && attIndex == 2)
                    dataAnnot = "01:array";
                else
                {
                    dataAnnot = SetGetFromMeter.GetDataFromObject(ref dLMSComm, classID, OBIS, attIndex).Substring(0, 2);
                    dataAnnot = ProfileSheetConfiguration.commonDataTypesList.FirstOrDefault(s => s.Contains($"{dataAnnot}"));
                }
                //"SN", "DESCRIPTION", "CLASS", "OBIS", "ATTRIBUTE INDEX", "DATA INDEX", "DATA ANNOTATION INDIVIDUAL", "SCALER MUTIPLIER", "UNIT", "DATA", "VALUE", "REMARK"
                dt.Rows.Add(i, name, classID, OBIS, attIndex, dataIndex, dataAnnot, scaler, unit, "", "", "");
            }
            dt.AcceptChanges();
        }

        /// <summary>
        /// Clear All the status of the Push Setup objects and clear all the objects data table to blank data table without schema.
        /// </summary>
        public static void ClearPushSetup()
        {
            IsInstantPushAvailable = false;
            IsAlertPushAvailable = false;
            IsLSPushAvailable = false;
            IsDEPushAvailable = false;
            IsSelfRegPushAvailable = false;
            IsBillAvailable = false;
            IsTamperPushAvailable = false;
            for (int i = 0; i < PushSetupDTArray.Length; i++)
            {
                PushSetupDTArray[i] = new DataTable();
            }
        }

        /// <summary>
        /// Get status weather the IPv6 is configured.
        /// </summary>
        /// <returns>true if it is configured else false</returns>
        [Obsolete("IsIPv6Configured is deprecated, please use NetworkHelper.IsIPv6Configured instead.")]
        public static bool IsIPv6Configured()
        {
            return NetworkInterface.GetAllNetworkInterfaces()
                .Any(nic => nic.Supports(NetworkInterfaceComponent.IPv6));
        }

        /// <summary>
        /// Provide the configured IPv6 address.
        /// </summary>
        /// <returns>IPv6 address in string form.</returns>
        [Obsolete("GetIPv6Address is deprecated, please use NetworkHelper.GetIPv6Address instead.")]
        public static string GetIPv6Address()
        {
            foreach (NetworkInterface netInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (netInterface.OperationalStatus == OperationalStatus.Up &&
                    netInterface.Supports(NetworkInterfaceComponent.IPv6))
                {
                    foreach (UnicastIPAddressInformation ip in netInterface.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetworkV6 &&
                            !ip.Address.IsIPv6LinkLocal) // Exclude link-local addresses
                        {
                            return ip.Address.ToString();
                        }
                    }
                }
            }
            return string.Empty;
        }


    }
}
