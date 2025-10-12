/*
 This Class Represent the Profile Generic Class.
 */
using AutoTest.FrameWork.Converts;
using log4net;
using log4net.Util;
using MeterComm;
using MeterComm.DLMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
namespace MeterReader.TestHelperClasses
{
    public static class ProfileGenericInfo
    {
        //Logger
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region Properties
        public static bool IsNameplateAvailable = false;
        public static bool IsInstantAvailable = false;
        public static bool IsBillAvailable = false;
        public static bool IsVoltageAvailable = false;
        public static bool IsCurrentAvailable = false;
        public static bool IsPowerAvailable = false;
        public static bool IsTransactionsAvailable = false;
        public static bool IsNonRolloverAvailable = false;
        public static bool IsOtherAvailable = false;
        public static bool IsControlAvailable = false;
        public static bool IsLSAvailable = false;
        public static bool IsDEAvailable = false;
        #endregion

        #region Profile Data Tables
        /// <summary>
        /// This will hold the Nameplate Profile
        /// </summary>
        public static DataTable nameplateDT { get; set; } = new DataTable();
        /// <summary>
        /// This will hold the Instantaneous Profile 
        /// </summary>
        public static DataTable instantDT { get; set; } = new DataTable();
        /// <summary>
        /// This will hold the Instantaneous Profile 
        /// </summary>
        public static DataTable billingDT { get; set; } = new DataTable();
        /// <summary>
        /// This will hold the Voltage Related Events Profile 
        /// </summary>
        public static DataTable voltageDT { get; set; } = new DataTable();
        /// <summary>
        /// This will hold the Current Related Events Profile 
        /// </summary>
        public static DataTable currentDT { get; set; } = new DataTable();
        /// <summary>
        /// This will hold the Power Related Events Profile 
        /// </summary>
        public static DataTable powerDT { get; set; } = new DataTable();
        /// <summary>
        /// This will hold the Other Tamper Events Profile 
        /// </summary>
        public static DataTable otherDT { get; set; } = new DataTable();
        /// <summary>
        /// This will hold the Transaction Events Profile 
        /// </summary>
        public static DataTable transactionDT { get; set; } = new DataTable();
        /// <summary>
        /// This will hold the NonRollOver Events Profile 
        /// </summary>
        public static DataTable nonrolloverDT { get; set; } = new DataTable();
        /// <summary>
        /// This will hold the Control Events Profile 
        /// </summary>
        public static DataTable controlDT { get; set; } = new DataTable();
        /// <summary>
        /// This will hold the Block Load Profile 
        /// </summary>
        public static DataTable blockloadDT { get; set; } = new DataTable();
        /// <summary>
        /// This will hold the Daily Load Profile 
        /// </summary>
        public static DataTable dailyloadDT { get; set; } = new DataTable();
        public static List<string> profileList { get; } = new List<string>
                                                                {
                                                                "Nameplate Profile-0.0.94.91.10.255",
                                                                "Instantaneous Profile-1.0.94.91.0.255-1.0.94.91.3.255",
                                                                "Billing Profile-1.0.98.1.0.255-1.0.94.91.6.255",
                                                                "Block Load Profile-1.0.99.1.0.255-1.0.94.91.4.255",
                                                                "Daily Load Profile-1.0.99.2.0.255-1.0.94.91.5.255",
                                                                "Voltage Related Events Profile-0.0.99.98.0.255-1.0.94.91.7.255",
                                                                "Current Related Events Profile-0.0.99.98.1.255-1.0.94.91.7.255",
                                                                "Power Related Events Profile-0.0.99.98.2.255-1.0.94.91.7.255",
                                                                "Transaction Events Profile-0.0.99.98.3.255-1.0.94.91.7.255",
                                                                "Other Tamper Events Profile-0.0.99.98.4.255-1.0.94.91.7.255",
                                                                "Non roll over Events Profile-0.0.99.98.5.255-1.0.94.91.7.255",
                                                                "Control Events Profile-0.0.99.98.6.255-1.0.94.91.7.255"
                                                                };
        public static List<string> availableProfileList = new List<string>();
        #endregion

        #region Methods
        /// <summary>
        /// This will Fill all the available profiles tables.
        /// NOTE: DLMSComm object must be dispose before calling this method
        /// </summary>
        public static void FillTables()
        {
            DLMSParser parse = new DLMSParser();
            int inUseEntries = 0;
            TestConfiguration previousConfiguration = TestConfiguration.CreateDefault();
            TestConfiguration _testConfiguration = previousConfiguration.Clone();
            //apply US configuration
            _testConfiguration.AccessMode = 2;
            _testConfiguration.AddressModeText = "0";
            _testConfiguration.IsLNWithCipher = MeterIdentity.GetCipherStatus();
            _testConfiguration.ApplyTestConfiguration();
            //create meter communication object
            DLMSComm DLMSObj = new DLMSComm(DLMSInfo.comPort, DLMSInfo.BaudRate);
            try
            {
                if (!DLMSObj.SignOnDLMS())
                {
                    return;
                }
                availableProfileList.Clear();

                #region STEP-1: Get all available profile
                foreach (string selectedProfile in profileList)
                {
                    string gotProfile = parse.GetProfileValueString(SetGetFromMeter.GetDataFromObject(ref DLMSObj, 7, $"{selectedProfile.Split('-')[1].Trim()}", 1));
                    if (gotProfile.Split('.').Length == 6)
                    {
                        availableProfileList.Add(selectedProfile);
                    }
                    switch (gotProfile)
                    {
                        //Nameplate
                        case "0.0.94.91.10.255":
                            var nameplateData = DLMSObj.GetNameplateProfileDataTable();
                            nameplateDT = ((DataTable)nameplateData["DataTable"]).Copy();
                            IsNameplateAvailable = true;
                            break;
                        //Instantaneous Profile
                        case "1.0.94.91.0.255":
                            var instantData = DLMSObj.GetSingleEntryProfileDataTable($"{selectedProfile.Split('-')[1].Trim()}", $"{selectedProfile.Split('-')[2].Trim()}");
                            instantDT = ((DataTable)instantData["DataTable"]).Copy();
                            IsInstantAvailable = true;
                            break;
                        //Billing Profile
                        case "1.0.98.1.0.255":
                            var billingData = DLMSObj.GetSingleEntryProfileDataTable($"{selectedProfile.Split('-')[1].Trim()}", $"{selectedProfile.Split('-')[2].Trim()}");
                            billingDT = ((DataTable)billingData["DataTable"]).Copy();
                            IsBillAvailable = true;
                            break;
                        //Block Load Profile
                        case "1.0.99.1.0.255":
                            string startDT = string.Empty;
                            string endDT = string.Empty;
                            inUseEntries = Convert.ToInt32(parse.GetProfileValueString(SetGetFromMeter.GetDataFromObject(ref DLMSObj, 7, "1.0.99.1.0.255", 7)));
                            int meterIP = int.Parse(parse.GetProfileValueString(SetGetFromMeter.GetDataFromObject(ref DLMSObj, 7, "1.0.99.1.0.255", 4).Trim())) / 60;
                            //Get Meter RTC
                            string currentRTCData = SetGetFromMeter.GetDataFromObject(ref DLMSObj, 8, "0.0.1.0.0.255", 2).Trim();
                            string deviationBytes = currentRTCData.Substring(currentRTCData.Length - 6, 4);
                            string sData = string.Empty;
                            DateTime writeDateTimeLS = DateTime.ParseExact(parse.GetProfileValueString(currentRTCData), "dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                            writeDateTimeLS = new DateTime(writeDateTimeLS.Year, writeDateTimeLS.Month, writeDateTimeLS.Day, writeDateTimeLS.Hour, writeDateTimeLS.Minute, 59);
                            int secBeforeIPCross = writeDateTimeLS.Second;
                            NextIP(ref writeDateTimeLS, meterIP, secBeforeIPCross, 1);
                            startDT = writeDateTimeLS.ToString("dd/MM/yyyy HH:mm");
                            sData = "090C" + Convert.ToInt16(writeDateTimeLS.Year).ToString("X4") + Convert.ToByte(writeDateTimeLS.Month).ToString("X2") + Convert.ToByte(writeDateTimeLS.Day).ToString("X2");
                            if (Convert.ToByte(writeDateTimeLS.DayOfWeek) == 0)
                                sData += "07";
                            else
                                sData += Convert.ToByte(writeDateTimeLS.DayOfWeek).ToString("X2");
                            sData += Convert.ToByte(writeDateTimeLS.Hour).ToString("X2") + Convert.ToByte(writeDateTimeLS.Minute).ToString("X2") + Convert.ToByte(writeDateTimeLS.Second).ToString("X2") + "00" + deviationBytes + "00";
                            DLMSObj.SetValue(8, "0.0.1.0.0.255", 2, sData, deviationBytes);
                            SetGetFromMeter.Wait(2000);
                            endDT = (DateTime.ParseExact(parse.GetProfileValueString(SetGetFromMeter.GetDataFromObject(ref DLMSObj, 8, "0.0.1.0.0.255", 2).Trim()), "dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture)).ToString("dd/MM/yyyy HH:mm"); ;
                            int finalInUseEntries = Convert.ToInt32(parse.GetProfileValueString(SetGetFromMeter.GetDataFromObject(ref DLMSObj, 7, "1.0.99.1.0.255", 7)));
                            var blockloadData = DLMSObj.GetSingleEntryLSorDEDataTable("1.0.99.1.0.255", "1.0.94.91.4.255", startDT, endDT, 1);
                            blockloadDT = ((DataTable)blockloadData["DataTable"]).Copy();
                            IsLSAvailable = true;
                            break;
                        //Daily Load Profile
                        case "1.0.99.2.0.255":
                            startDT = string.Empty;
                            endDT = string.Empty;
                            inUseEntries = Convert.ToInt32(parse.GetProfileValueString(SetGetFromMeter.GetDataFromObject(ref DLMSObj, 7, "1.0.99.2.0.255", 7)));
                            meterIP = int.Parse(parse.GetProfileValueString(SetGetFromMeter.GetDataFromObject(ref DLMSObj, 7, "1.0.99.2.0.255", 4).Trim())) / 60;
                            //Get Meter RTC
                            currentRTCData = SetGetFromMeter.GetDataFromObject(ref DLMSObj, 8, "0.0.1.0.0.255", 2).Trim();
                            deviationBytes = currentRTCData.Substring(currentRTCData.Length - 6, 4);
                            sData = string.Empty;
                            writeDateTimeLS = DateTime.ParseExact(parse.GetProfileValueString(currentRTCData), "dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                            writeDateTimeLS = new DateTime(writeDateTimeLS.Year, writeDateTimeLS.Month, writeDateTimeLS.Day, writeDateTimeLS.Hour, writeDateTimeLS.Minute, 59);
                            secBeforeIPCross = writeDateTimeLS.Second;
                            NextIP(ref writeDateTimeLS, meterIP, secBeforeIPCross, 1);
                            startDT = $"{writeDateTimeLS.ToString("dd/MM/yyyy ")}00:00";
                            writeDateTimeLS = writeDateTimeLS.AddDays(1);
                            endDT = $"{writeDateTimeLS.ToString("dd/MM/yyyy ")}23:59";
                            sData = "090C" + Convert.ToInt16(writeDateTimeLS.Year).ToString("X4") + Convert.ToByte(writeDateTimeLS.Month).ToString("X2") + Convert.ToByte(writeDateTimeLS.Day).ToString("X2");
                            if (Convert.ToByte(writeDateTimeLS.DayOfWeek) == 0)
                                sData += "07";
                            else
                                sData += Convert.ToByte(writeDateTimeLS.DayOfWeek).ToString("X2");
                            sData += Convert.ToByte(writeDateTimeLS.Hour).ToString("X2") + Convert.ToByte(writeDateTimeLS.Minute).ToString("X2") + Convert.ToByte(writeDateTimeLS.Second).ToString("X2") + "00" + deviationBytes + "00";
                            DLMSObj.SetValue(8, "0.0.1.0.0.255", 2, sData, deviationBytes);
                            SetGetFromMeter.Wait(2000);
                            //endDT = (DateTime.ParseExact(parse.GetProfileValueString(SetGetFromMeter.GetDataFromObject(ref DLMSObj, 8, "0.0.1.0.0.255", 2).Trim()), "dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture)).ToString("dd/MM/yyyy HH:mm"); ;
                            finalInUseEntries = Convert.ToInt32(parse.GetProfileValueString(SetGetFromMeter.GetDataFromObject(ref DLMSObj, 7, "1.0.99.2.0.255", 7)));
                            var dailyloadData = DLMSObj.GetSingleEntryLSorDEDataTable("1.0.99.2.0.255", "1.0.94.91.5.255", startDT, endDT, 1);
                            dailyloadDT = ((DataTable)dailyloadData["DataTable"]).Copy();
                            IsDEAvailable = true;
                            break;
                        //Voltage Related Events Profile
                        case "0.0.99.98.0.255":
                            var voltageData = DLMSObj.GetSingleEntryProfileDataTable($"{selectedProfile.Split('-')[1].Trim()}", $"{selectedProfile.Split('-')[2].Trim()}");
                            voltageDT = ((DataTable)voltageData["DataTable"]).Copy();
                            IsVoltageAvailable = true;
                            break;
                        //Current Related Events Profile
                        case "0.0.99.98.1.255":
                            var currentData = DLMSObj.GetSingleEntryProfileDataTable($"{selectedProfile.Split('-')[1].Trim()}", $"{selectedProfile.Split('-')[2].Trim()}");
                            currentDT = ((DataTable)currentData["DataTable"]).Copy();
                            IsCurrentAvailable = true;
                            break;
                        //Power Related Events Profile
                        case "0.0.99.98.2.255":
                            var powerData = DLMSObj.GetSingleEntryProfileDataTable($"{selectedProfile.Split('-')[1].Trim()}", $"{selectedProfile.Split('-')[2].Trim()}");
                            powerDT = ((DataTable)powerData["DataTable"]).Copy();
                            IsPowerAvailable = true;
                            break;
                        //Transaction Events Profile
                        case "0.0.99.98.3.255":
                            var transactionData = DLMSObj.GetSingleEntryProfileDataTable($"{selectedProfile.Split('-')[1].Trim()}", $"{selectedProfile.Split('-')[2].Trim()}");
                            transactionDT = ((DataTable)transactionData["DataTable"]).Copy();
                            IsTransactionsAvailable = true;
                            break;
                        //Other Tamper Events Profile
                        case "0.0.99.98.4.255":
                            var otherData = DLMSObj.GetSingleEntryProfileDataTable($"{selectedProfile.Split('-')[1].Trim()}", $"{selectedProfile.Split('-')[2].Trim()}");
                            otherDT = ((DataTable)otherData["DataTable"]).Copy();
                            IsOtherAvailable = true;
                            break;
                        //Non roll over Events Profile
                        case "0.0.99.98.5.255":
                            var nonrolloverData = DLMSObj.GetSingleEntryProfileDataTable($"{selectedProfile.Split('-')[1].Trim()}", $"{selectedProfile.Split('-')[2].Trim()}");
                            nonrolloverDT = ((DataTable)nonrolloverData["DataTable"]).Copy();
                            IsNonRolloverAvailable = true;
                            break;
                        //Control Events Profile
                        case "0.0.99.98.6.255":
                            var controlData = DLMSObj.GetSingleEntryProfileDataTable($"{selectedProfile.Split('-')[1].Trim()}", $"{selectedProfile.Split('-')[2].Trim()}");
                            controlDT = ((DataTable)controlData["DataTable"]).Copy();
                            IsControlAvailable = true;
                            break;
                    }
                }
                #endregion

                #region STEP-2: Make Table in standard format of columns SN, Description, IC, OBIS, Attribute, Scaler, Unit, DataType, Remark
                #region Columns Name
                List<string> columHeader = new List<string>();
                columHeader.Add("SN");
                columHeader.Add("Description");
                columHeader.Add("IC");
                columHeader.Add("OBIS");
                columHeader.Add("Attribute");
                columHeader.Add("Scaler");
                columHeader.Add("Unit");
                columHeader.Add("Data Type");
                columHeader.Add("Remark");
                #endregion
                for (int profileIndex = 0; profileIndex < availableProfileList.Count; profileIndex++)
                {
                    DataTable dataTable = new DataTable();
                    foreach (string columnName in columHeader)
                    {
                        dataTable.Columns.Add($"{columnName}", typeof(string));
                    }
                    string serialNumber = "", description = "", interfaceClass = "", obis = "", attribute = "", scaler = "", unit = "", dataType = "", remark = "", scalerData = "";
                    DataTable printeingDT = new DataTable();
                    switch (availableProfileList[profileIndex].Split('-')[1].Trim())
                    {
                        //Nameplate
                        case "0.0.94.91.10.255":
                            printeingDT = nameplateDT.Copy();
                            break;
                        //Instantaneous Profile
                        case "1.0.94.91.0.255":
                            printeingDT = instantDT.Copy();
                            break;
                        //Billing Profile
                        case "1.0.98.1.0.255":
                            printeingDT = billingDT.Copy();
                            break;
                        //Block Load Profile
                        case "1.0.99.1.0.255":
                            printeingDT = blockloadDT.Copy();
                            break;
                        //Daily Load Profile
                        case "1.0.99.2.0.255":
                            printeingDT = dailyloadDT.Copy();
                            break;
                        //Voltage Related Events Profile
                        case "0.0.99.98.0.255":
                            printeingDT = voltageDT.Copy();
                            break;
                        //Current Related Events Profile
                        case "0.0.99.98.1.255":
                            printeingDT = currentDT.Copy();
                            break;
                        //Power Related Events Profile
                        case "0.0.99.98.2.255":
                            printeingDT = powerDT.Copy();
                            break;
                        //Transaction Events Profile
                        case "0.0.99.98.3.255":
                            printeingDT = transactionDT.Copy();
                            break;
                        //Other Tamper Events Profile
                        case "0.0.99.98.4.255":
                            printeingDT = otherDT.Copy();
                            break;
                        //Non roll over Events Profile
                        case "0.0.99.98.5.255":
                            printeingDT = nonrolloverDT.Copy();
                            break;
                        //Control Events Profile
                        case "0.0.99.98.6.255":
                            printeingDT = controlDT.Copy();
                            break;
                    }
                    if (dataTable.Rows.Count > 0)
                        dataTable.Rows.Clear();
                    foreach (DataRow selectedRow in printeingDT.Rows)
                    {
                        serialNumber = selectedRow[0].ToString();
                        description = parse.GetParameterName(selectedRow[2].ToString().Trim(), selectedRow[3].ToString().Trim(), selectedRow[4].ToString().Trim());
                        interfaceClass = selectedRow[2].ToString();
                        obis = selectedRow[3].ToString();
                        attribute = selectedRow[4].ToString();
                        if (!string.IsNullOrEmpty(selectedRow[5].ToString().Trim()) && selectedRow[5].ToString().Trim().Length == 8)
                        {
                            scaler = ProfileSheetConfiguration.standardScalerList.FirstOrDefault(s => s.Contains($"{selectedRow[5].ToString().Trim().Substring(2, 2)}"));
                            unit = ProfileSheetConfiguration.standardUnitList.FirstOrDefault(s => s.Contains($"{selectedRow[5].ToString().Trim().Substring(6, 2)}"));
                        }
                        else if (selectedRow.ItemArray.Length <= 6 && (Convert.ToInt32(interfaceClass) == 3 || Convert.ToInt32(interfaceClass) == 4 || Convert.ToInt32(interfaceClass) == 5))
                        {
                            scalerData = SetGetFromMeter.GetDataFromObject(ref DLMSObj, Convert.ToInt32(interfaceClass), obis, 3);
                            scaler = ProfileSheetConfiguration.standardScalerList.FirstOrDefault(s => s.Contains($"{scalerData.Substring(6, 2)}"));
                            unit = ProfileSheetConfiguration.standardUnitList.FirstOrDefault(s => s.Contains($"{scalerData.Substring(10, 2)}"));
                        }
                        else
                        {
                            scaler = "";
                            unit = "";
                        }
                        if (selectedRow.ItemArray.Length > 6 && !string.IsNullOrEmpty(selectedRow[6].ToString().Trim()))
                            dataType = ProfileSheetConfiguration.commonDataTypesList.FirstOrDefault(s => s.Contains($"{selectedRow[6].ToString().Trim().Substring(0, 2)}"));
                        else if (selectedRow.ItemArray.Length <= 6)
                        {
                            dataType = SetGetFromMeter.GetDataFromObject(ref DLMSObj, Convert.ToInt32(interfaceClass), obis, Convert.ToInt32(attribute)).Substring(0, 2);
                            dataType = ProfileSheetConfiguration.commonDataTypesList.FirstOrDefault(s => s.Contains($"{dataType}"));
                        }
                        else
                            dataType = "";
                        dataTable.Rows.Add(serialNumber, description, interfaceClass, obis, attribute, scaler, unit, dataType, remark);
                    }
                    switch (availableProfileList[profileIndex].Split('-')[1].Trim())
                    {
                        //Nameplate
                        case "0.0.94.91.10.255":
                            nameplateDT.Clear();
                            nameplateDT = dataTable.Copy();
                            break;
                        //Instantaneous Profile
                        case "1.0.94.91.0.255":
                            instantDT.Clear();
                            instantDT = dataTable.Copy();
                            break;
                        //Billing Profile
                        case "1.0.98.1.0.255":
                            billingDT.Clear();
                            billingDT = dataTable.Copy();
                            break;
                        //Block Load Profile
                        case "1.0.99.1.0.255":
                            blockloadDT.Clear();
                            blockloadDT = dataTable.Copy();
                            break;
                        //Daily Load Profile
                        case "1.0.99.2.0.255":
                            dailyloadDT.Clear();
                            dailyloadDT = dataTable.Copy();
                            break;
                        //Voltage Related Events Profile
                        case "0.0.99.98.0.255":
                            voltageDT.Clear();
                            voltageDT = dataTable.Copy();
                            break;
                        //Current Related Events Profile
                        case "0.0.99.98.1.255":
                            currentDT.Clear();
                            currentDT = dataTable.Copy();
                            break;
                        //Power Related Events Profile
                        case "0.0.99.98.2.255":
                            powerDT.Clear();
                            powerDT = dataTable.Copy();
                            break;
                        //Transaction Events Profile
                        case "0.0.99.98.3.255":
                            transactionDT.Clear();
                            transactionDT = dataTable.Copy();
                            break;
                        //Other Tamper Events Profile
                        case "0.0.99.98.4.255":
                            otherDT.Clear();
                            otherDT = dataTable.Copy();
                            break;
                        //Non roll over Events Profile
                        case "0.0.99.98.5.255":
                            nonrolloverDT.Clear();
                            nonrolloverDT = dataTable.Copy();
                            break;
                        //Control Events Profile
                        case "0.0.99.98.6.255":
                            controlDT.Clear();
                            controlDT = dataTable.Copy();
                            break;
                    }

                }
                #endregion

            }
            catch (Exception ex)
            {
                log.Error(ex.Message.ToString());
                log.Error(ex.StackTrace);
            }
            finally
            {
                DLMSObj.SetDISCMode();
                DLMSObj.Dispose();
                SetGetFromMeter.Wait(_testConfiguration.DISCToNDMTimeout);
                previousConfiguration.ApplyTestConfiguration();
            }
        }
        public static void FillTables(ref DLMSComm DLMSObj, string testProfile = "All")
        {
            DLMSParser parse = new DLMSParser();
            int inUseEntries = 0;
            try
            {
                availableProfileList.Clear();

                #region STEP-1: Get all available profile
                foreach (string selectedProfile in profileList)
                {
                    if (selectedProfile == testProfile || testProfile == "All")
                    {
                        string gotProfile = parse.GetProfileValueString(SetGetFromMeter.GetDataFromObject(ref DLMSObj, 7, $"{selectedProfile.Split('-')[1].Trim()}", 1));
                        if (gotProfile.Split('.').Length == 6)
                        {
                            availableProfileList.Add(selectedProfile);
                        }
                        switch (gotProfile)
                        {
                            //Nameplate
                            case "0.0.94.91.10.255":
                                var nameplateData = DLMSObj.GetNameplateProfileDataTable();
                                nameplateDT = ((DataTable)nameplateData["DataTable"]).Copy();
                                IsNameplateAvailable = true;
                                break;
                            //Instantaneous Profile
                            case "1.0.94.91.0.255":
                                var instantData = DLMSObj.GetSingleEntryProfileDataTable($"{selectedProfile.Split('-')[1].Trim()}", $"{selectedProfile.Split('-')[2].Trim()}");
                                instantDT = ((DataTable)instantData["DataTable"]).Copy();
                                IsInstantAvailable = true;
                                break;
                            //Billing Profile
                            case "1.0.98.1.0.255":
                                var billingData = DLMSObj.GetSingleEntryProfileDataTable($"{selectedProfile.Split('-')[1].Trim()}", $"{selectedProfile.Split('-')[2].Trim()}");
                                billingDT = ((DataTable)billingData["DataTable"]).Copy();
                                IsBillAvailable = true;
                                break;
                            //Block Load Profile
                            case "1.0.99.1.0.255":
                                string startDT = string.Empty;
                                string endDT = string.Empty;
                                inUseEntries = Convert.ToInt32(parse.GetProfileValueString(SetGetFromMeter.GetDataFromObject(ref DLMSObj, 7, "1.0.99.1.0.255", 7)));
                                int meterIP = int.Parse(parse.GetProfileValueString(SetGetFromMeter.GetDataFromObject(ref DLMSObj, 7, "1.0.99.1.0.255", 4).Trim())) / 60;
                                //Get Meter RTC
                                string currentRTCData = SetGetFromMeter.GetDataFromObject(ref DLMSObj, 8, "0.0.1.0.0.255", 2).Trim();
                                string deviationBytes = currentRTCData.Substring(currentRTCData.Length - 6, 4);
                                string sData = string.Empty;
                                DateTime writeDateTimeLS = DateTime.ParseExact(parse.GetProfileValueString(currentRTCData), "dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                                writeDateTimeLS = new DateTime(writeDateTimeLS.Year, writeDateTimeLS.Month, writeDateTimeLS.Day, writeDateTimeLS.Hour, writeDateTimeLS.Minute, 59);
                                int secBeforeIPCross = writeDateTimeLS.Second;
                                NextIP(ref writeDateTimeLS, meterIP, secBeforeIPCross, 1);
                                startDT = writeDateTimeLS.ToString("dd/MM/yyyy HH:mm");
                                sData = "090C" + Convert.ToInt16(writeDateTimeLS.Year).ToString("X4") + Convert.ToByte(writeDateTimeLS.Month).ToString("X2") + Convert.ToByte(writeDateTimeLS.Day).ToString("X2");
                                if (Convert.ToByte(writeDateTimeLS.DayOfWeek) == 0)
                                    sData += "07";
                                else
                                    sData += Convert.ToByte(writeDateTimeLS.DayOfWeek).ToString("X2");
                                sData += Convert.ToByte(writeDateTimeLS.Hour).ToString("X2") + Convert.ToByte(writeDateTimeLS.Minute).ToString("X2") + Convert.ToByte(writeDateTimeLS.Second).ToString("X2") + "00" + deviationBytes + "00";
                                DLMSObj.SetValue(8, "0.0.1.0.0.255", 2, sData, deviationBytes);
                                SetGetFromMeter.Wait(2000);
                                endDT = (DateTime.ParseExact(parse.GetProfileValueString(SetGetFromMeter.GetDataFromObject(ref DLMSObj, 8, "0.0.1.0.0.255", 2).Trim()), "dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture)).ToString("dd/MM/yyyy HH:mm"); ;
                                int finalInUseEntries = Convert.ToInt32(parse.GetProfileValueString(SetGetFromMeter.GetDataFromObject(ref DLMSObj, 7, "1.0.99.1.0.255", 7)));
                                var blockloadData = DLMSObj.GetSingleEntryLSorDEDataTable("1.0.99.1.0.255", "1.0.94.91.4.255", startDT, endDT, 1);
                                blockloadDT = ((DataTable)blockloadData["DataTable"]).Copy();
                                IsLSAvailable = true;
                                break;
                            //Daily Load Profile
                            case "1.0.99.2.0.255":
                                startDT = string.Empty;
                                endDT = string.Empty;
                                inUseEntries = Convert.ToInt32(parse.GetProfileValueString(SetGetFromMeter.GetDataFromObject(ref DLMSObj, 7, "1.0.99.2.0.255", 7)));
                                meterIP = int.Parse(parse.GetProfileValueString(SetGetFromMeter.GetDataFromObject(ref DLMSObj, 7, "1.0.99.2.0.255", 4).Trim())) / 60;
                                //Get Meter RTC
                                currentRTCData = SetGetFromMeter.GetDataFromObject(ref DLMSObj, 8, "0.0.1.0.0.255", 2).Trim();
                                deviationBytes = currentRTCData.Substring(currentRTCData.Length - 6, 4);
                                sData = string.Empty;
                                writeDateTimeLS = DateTime.ParseExact(parse.GetProfileValueString(currentRTCData), "dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                                writeDateTimeLS = new DateTime(writeDateTimeLS.Year, writeDateTimeLS.Month, writeDateTimeLS.Day, writeDateTimeLS.Hour, writeDateTimeLS.Minute, 59);
                                secBeforeIPCross = writeDateTimeLS.Second;
                                NextIP(ref writeDateTimeLS, meterIP, secBeforeIPCross, 1);
                                startDT = $"{writeDateTimeLS.ToString("dd/MM/yyyy ")}00:00";
                                writeDateTimeLS = writeDateTimeLS.AddDays(1);
                                endDT = $"{writeDateTimeLS.ToString("dd/MM/yyyy ")}23:59";
                                sData = "090C" + Convert.ToInt16(writeDateTimeLS.Year).ToString("X4") + Convert.ToByte(writeDateTimeLS.Month).ToString("X2") + Convert.ToByte(writeDateTimeLS.Day).ToString("X2");
                                if (Convert.ToByte(writeDateTimeLS.DayOfWeek) == 0)
                                    sData += "07";
                                else
                                    sData += Convert.ToByte(writeDateTimeLS.DayOfWeek).ToString("X2");
                                sData += Convert.ToByte(writeDateTimeLS.Hour).ToString("X2") + Convert.ToByte(writeDateTimeLS.Minute).ToString("X2") + Convert.ToByte(writeDateTimeLS.Second).ToString("X2") + "00" + deviationBytes + "00";
                                //DLMSObj.SetValue(8, "0.0.1.0.0.255", 2, sData, deviationBytes);
                                SetGetFromMeter.Wait(2000);
                                //endDT = (DateTime.ParseExact(parse.GetProfileValueString(SetGetFromMeter.GetDataFromObject(ref DLMSObj, 8, "0.0.1.0.0.255", 2).Trim()), "dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture)).ToString("dd/MM/yyyy HH:mm"); ;
                                finalInUseEntries = Convert.ToInt32(parse.GetProfileValueString(SetGetFromMeter.GetDataFromObject(ref DLMSObj, 7, "1.0.99.2.0.255", 7)));
                                var dailyloadData = DLMSObj.GetSingleEntryLSorDEDataTable("1.0.99.2.0.255", "1.0.94.91.5.255", startDT, endDT, 1);
                                dailyloadDT = ((DataTable)dailyloadData["DataTable"]).Copy();
                                IsDEAvailable = true;
                                break;
                            //Voltage Related Events Profile
                            case "0.0.99.98.0.255":
                                var voltageData = DLMSObj.GetSingleEntryProfileDataTable($"{selectedProfile.Split('-')[1].Trim()}", $"{selectedProfile.Split('-')[2].Trim()}");
                                voltageDT = ((DataTable)voltageData["DataTable"]).Copy();
                                IsVoltageAvailable = true;
                                break;
                            //Current Related Events Profile
                            case "0.0.99.98.1.255":
                                var currentData = DLMSObj.GetSingleEntryProfileDataTable($"{selectedProfile.Split('-')[1].Trim()}", $"{selectedProfile.Split('-')[2].Trim()}");
                                currentDT = ((DataTable)currentData["DataTable"]).Copy();
                                IsCurrentAvailable = true;
                                break;
                            //Power Related Events Profile
                            case "0.0.99.98.2.255":
                                var powerData = DLMSObj.GetSingleEntryProfileDataTable($"{selectedProfile.Split('-')[1].Trim()}", $"{selectedProfile.Split('-')[2].Trim()}");
                                powerDT = ((DataTable)powerData["DataTable"]).Copy();
                                IsPowerAvailable = true;
                                break;
                            //Transaction Events Profile
                            case "0.0.99.98.3.255":
                                var transactionData = DLMSObj.GetSingleEntryProfileDataTable($"{selectedProfile.Split('-')[1].Trim()}", $"{selectedProfile.Split('-')[2].Trim()}");
                                transactionDT = ((DataTable)transactionData["DataTable"]).Copy();
                                IsTransactionsAvailable = true;
                                break;
                            //Other Tamper Events Profile
                            case "0.0.99.98.4.255":
                                var otherData = DLMSObj.GetSingleEntryProfileDataTable($"{selectedProfile.Split('-')[1].Trim()}", $"{selectedProfile.Split('-')[2].Trim()}");
                                otherDT = ((DataTable)otherData["DataTable"]).Copy();
                                IsOtherAvailable = true;
                                break;
                            //Non roll over Events Profile
                            case "0.0.99.98.5.255":
                                var nonrolloverData = DLMSObj.GetSingleEntryProfileDataTable($"{selectedProfile.Split('-')[1].Trim()}", $"{selectedProfile.Split('-')[2].Trim()}");
                                nonrolloverDT = ((DataTable)nonrolloverData["DataTable"]).Copy();
                                IsNonRolloverAvailable = true;
                                break;
                            //Control Events Profile
                            case "0.0.99.98.6.255":
                                var controlData = DLMSObj.GetSingleEntryProfileDataTable($"{selectedProfile.Split('-')[1].Trim()}", $"{selectedProfile.Split('-')[2].Trim()}");
                                controlDT = ((DataTable)controlData["DataTable"]).Copy();
                                IsControlAvailable = true;
                                break;
                        }
                    }
                }
                #endregion

                #region STEP-2: Make Table in standard format of columns SN, Description, IC, OBIS, Attribute, Scaler, Unit, DataType, Remark
                #region Columns Name
                List<string> columHeader = new List<string>();
                columHeader.Add("SN");
                columHeader.Add("Description");
                columHeader.Add("IC");
                columHeader.Add("OBIS");
                columHeader.Add("Attribute");
                columHeader.Add("Scaler");
                columHeader.Add("Unit");
                columHeader.Add("Data Type");
                columHeader.Add("Remark");
                #endregion
                for (int profileIndex = 0; profileIndex < availableProfileList.Count; profileIndex++)
                {
                    DataTable dataTable = new DataTable();
                    foreach (string columnName in columHeader)
                    {
                        dataTable.Columns.Add($"{columnName}", typeof(string));
                    }
                    string serialNumber = "", description = "", interfaceClass = "", obis = "", attribute = "", scaler = "", unit = "", dataType = "", remark = "", scalerData = "";
                    DataTable printeingDT = new DataTable();
                    switch (availableProfileList[profileIndex].Split('-')[1].Trim())
                    {
                        //Nameplate
                        case "0.0.94.91.10.255":
                            printeingDT = nameplateDT.Copy();
                            break;
                        //Instantaneous Profile
                        case "1.0.94.91.0.255":
                            printeingDT = instantDT.Copy();
                            break;
                        //Billing Profile
                        case "1.0.98.1.0.255":
                            printeingDT = billingDT.Copy();
                            break;
                        //Block Load Profile
                        case "1.0.99.1.0.255":
                            printeingDT = blockloadDT.Copy();
                            break;
                        //Daily Load Profile
                        case "1.0.99.2.0.255":
                            printeingDT = dailyloadDT.Copy();
                            break;
                        //Voltage Related Events Profile
                        case "0.0.99.98.0.255":
                            printeingDT = voltageDT.Copy();
                            break;
                        //Current Related Events Profile
                        case "0.0.99.98.1.255":
                            printeingDT = currentDT.Copy();
                            break;
                        //Power Related Events Profile
                        case "0.0.99.98.2.255":
                            printeingDT = powerDT.Copy();
                            break;
                        //Transaction Events Profile
                        case "0.0.99.98.3.255":
                            printeingDT = transactionDT.Copy();
                            break;
                        //Other Tamper Events Profile
                        case "0.0.99.98.4.255":
                            printeingDT = otherDT.Copy();
                            break;
                        //Non roll over Events Profile
                        case "0.0.99.98.5.255":
                            printeingDT = nonrolloverDT.Copy();
                            break;
                        //Control Events Profile
                        case "0.0.99.98.6.255":
                            printeingDT = controlDT.Copy();
                            break;
                    }
                    if (dataTable.Rows.Count > 0)
                        dataTable.Rows.Clear();
                    foreach (DataRow selectedRow in printeingDT.Rows)
                    {
                        serialNumber = selectedRow[0].ToString();
                        description = parse.GetParameterName(selectedRow[2].ToString().Trim(), selectedRow[3].ToString().Trim(), selectedRow[4].ToString().Trim());
                        interfaceClass = selectedRow[2].ToString();
                        obis = selectedRow[3].ToString();
                        attribute = selectedRow[4].ToString();
                        if (!string.IsNullOrEmpty(selectedRow[5].ToString().Trim()) && selectedRow[5].ToString().Trim().Length == 8)
                        {
                            scaler = ProfileSheetConfiguration.standardScalerList.FirstOrDefault(s => s.Contains($"{selectedRow[5].ToString().Trim().Substring(2, 2)}"));
                            unit = ProfileSheetConfiguration.standardUnitList.FirstOrDefault(s => s.Contains($"{selectedRow[5].ToString().Trim().Substring(6, 2)}"));
                        }
                        else if (selectedRow.ItemArray.Length <= 6 && (Convert.ToInt32(interfaceClass) == 3 || Convert.ToInt32(interfaceClass) == 4 || Convert.ToInt32(interfaceClass) == 5))
                        {
                            scalerData = SetGetFromMeter.GetDataFromObject(ref DLMSObj, Convert.ToInt32(interfaceClass), obis, 3);
                            scaler = ProfileSheetConfiguration.standardScalerList.FirstOrDefault(s => s.Contains($"{scalerData.Substring(6, 2)}"));
                            unit = ProfileSheetConfiguration.standardUnitList.FirstOrDefault(s => s.Contains($"{scalerData.Substring(10, 2)}"));
                        }
                        else
                        {
                            scaler = "";
                            unit = "";
                        }
                        if (selectedRow.ItemArray.Length > 6 && !string.IsNullOrEmpty(selectedRow[6].ToString().Trim()))
                            dataType = ProfileSheetConfiguration.commonDataTypesList.FirstOrDefault(s => s.Contains($"{selectedRow[6].ToString().Trim().Substring(0, 2)}"));
                        else if (selectedRow.ItemArray.Length <= 6)
                        {
                            dataType = SetGetFromMeter.GetDataFromObject(ref DLMSObj, Convert.ToInt32(interfaceClass), obis, Convert.ToInt32(attribute)).Substring(0, 2);
                            dataType = ProfileSheetConfiguration.commonDataTypesList.FirstOrDefault(s => s.Contains($"{dataType}"));
                        }
                        else
                            dataType = "";
                        dataTable.Rows.Add(serialNumber, description, interfaceClass, obis, attribute, scaler, unit, dataType, remark);
                    }
                    switch (availableProfileList[profileIndex].Split('-')[1].Trim())
                    {
                        //Nameplate
                        case "0.0.94.91.10.255":
                            nameplateDT.Clear();
                            nameplateDT = dataTable.Copy();
                            break;
                        //Instantaneous Profile
                        case "1.0.94.91.0.255":
                            instantDT.Clear();
                            instantDT = dataTable.Copy();
                            break;
                        //Billing Profile
                        case "1.0.98.1.0.255":
                            billingDT.Clear();
                            billingDT = dataTable.Copy();
                            break;
                        //Block Load Profile
                        case "1.0.99.1.0.255":
                            blockloadDT.Clear();
                            blockloadDT = dataTable.Copy();
                            break;
                        //Daily Load Profile
                        case "1.0.99.2.0.255":
                            dailyloadDT.Clear();
                            dailyloadDT = dataTable.Copy();
                            break;
                        //Voltage Related Events Profile
                        case "0.0.99.98.0.255":
                            voltageDT.Clear();
                            voltageDT = dataTable.Copy();
                            break;
                        //Current Related Events Profile
                        case "0.0.99.98.1.255":
                            currentDT.Clear();
                            currentDT = dataTable.Copy();
                            break;
                        //Power Related Events Profile
                        case "0.0.99.98.2.255":
                            powerDT.Clear();
                            powerDT = dataTable.Copy();
                            break;
                        //Transaction Events Profile
                        case "0.0.99.98.3.255":
                            transactionDT.Clear();
                            transactionDT = dataTable.Copy();
                            break;
                        //Other Tamper Events Profile
                        case "0.0.99.98.4.255":
                            otherDT.Clear();
                            otherDT = dataTable.Copy();
                            break;
                        //Non roll over Events Profile
                        case "0.0.99.98.5.255":
                            nonrolloverDT.Clear();
                            nonrolloverDT = dataTable.Copy();
                            break;
                        //Control Events Profile
                        case "0.0.99.98.6.255":
                            controlDT.Clear();
                            controlDT = dataTable.Copy();
                            break;
                    }

                }
                #endregion

            }
            catch (Exception ex)
            {
                log.Error(ex.Message.ToString());
                log.Error(ex.StackTrace);
            }
        }
        public static void NextIP(ref DateTime writeDateTimeLS, int IP, int secondValue, int beforeMinuteValue)
        {
            // Convert the time slot (IP) to an integer (5, 15, 30, or 60)
            int timeSlot = IP;

            // Get the current minute value
            int minutes = writeDateTimeLS.Minute;

            // Calculate how many minutes are needed to move to the next valid time slot
            int nextTimeSlotMinutes = (timeSlot - (minutes % timeSlot)) % timeSlot;

            // Adjust the DateTime by setting the seconds to `secondValue` and moving to the next time slot
            writeDateTimeLS = writeDateTimeLS
                .AddSeconds(secondValue - writeDateTimeLS.Second) // Adjust seconds
                .AddMinutes(nextTimeSlotMinutes - beforeMinuteValue); // Move to next time slot
        }

        /// <summary>
        /// This will clear all available list, data tables and available flags
        /// </summary>
        public static void ClearAllTables()
        {
            //Clear the available list and data tables
            availableProfileList.Clear();
            nameplateDT.Clear();
            instantDT.Clear();
            billingDT.Clear();
            voltageDT.Clear();
            currentDT.Clear();
            powerDT.Clear();
            otherDT.Clear();
            transactionDT.Clear();
            nonrolloverDT.Clear();
            controlDT.Clear();
            blockloadDT.Clear();
            dailyloadDT.Clear();

            //Clear All available flags
            IsNameplateAvailable = false;
            IsInstantAvailable = false;
            IsBillAvailable = false;
            IsVoltageAvailable = false;
            IsCurrentAvailable = false;
            IsPowerAvailable = false;
            IsTransactionsAvailable = false;
            IsNonRolloverAvailable = false;
            IsOtherAvailable = false;
            IsControlAvailable = false;
            IsLSAvailable = false;
            IsDEAvailable = false;
        }
        #endregion
    }
}
