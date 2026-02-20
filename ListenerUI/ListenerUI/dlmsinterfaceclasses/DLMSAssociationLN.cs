using AutoTest.FrameWork.Converts;
using log4net;
using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MeterReader.DLMSInterfaceClasses
{
    public static class DLMSAssociationLN
    {
        #region Logger
        //Logger
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Flags
        public static bool IsCurrentAssociationAvailable { get; set; } = false;
        public static bool IsPCAssociationAvailable { get; set; } = false;
        public static bool IsMRAssociationAvailable { get; set; } = false;
        public static bool IsUSAssociationAvailable { get; set; } = false;
        public static bool IsPUSHAssociationAvailable { get; set; } = false;
        public static bool IsFWAssociationAvailable { get; set; } = false;
        #endregion

        #region Association Version
        public static int CurrentAssociationVersion { get; set; } = 0;
        public static int PCAssociationVersion { get; set; } = 0;
        public static int MRAssociationVersion { get; set; } = 0;
        public static int USAssociationVersion { get; set; } = 0;
        public static int PUSHAssociationVersion { get; set; } = 0;
        public static int FWAssociationVersion { get; set; } = 0;
        #endregion

        #region Association Version
        public static string CurrentAssociationOBIS { get; set; } = "0000280000FF";
        public static string PCAssociationOBIS { get; set; } = "0000280001FF";
        public static string MRAssociationOBIS { get; set; } = "0000280002FF";
        public static string USAssociationOBIS { get; set; } = "0000280003FF";
        public static string PUSHAssociationOBIS { get; set; } = "0000280004FF";
        public static string FWAssociationOBIS { get; set; } = "0000280005FF";
        #endregion

        #region Association Object List Data
        public static string CurrentAssociationObjectDataString { get; set; } = "";
        public static string PCAssociationObjectDataString { get; set; } = "";
        public static string MRAssociationObjectDataString { get; set; } = "";
        public static string USAssociationObjectDataString { get; set; } = "";
        public static string PUSHAssociationObjectDataString { get; set; } = "";
        public static string FWAssociationObjectDataString { get; set; } = "";
        #endregion


        #region Data tables
        public static DataTable Current_AssociationDataTable { get; set; } = new DataTable();
        public static DataTable PC_AssociationDataTable { get; set; } = new DataTable();
        public static DataTable MR_AssociationDataTable { get; set; } = new DataTable();
        public static DataTable US_AssociationDataTable { get; set; } = new DataTable();
        public static DataTable PUSH_AssociationDataTable { get; set; } = new DataTable();
        public static DataTable FW_AssociationDataTable { get; set; } = new DataTable();
        public static string[] columnArray { get; } = { "SN", "CLASS ID", "VERSION", "OBIS", "DESCRIPTION", "ATTRIBUTE ACCESS", "METHOD ACCESS" };
        public static DataTable[] AssociationsDTArray { get; set; } = { Current_AssociationDataTable, PC_AssociationDataTable, MR_AssociationDataTable, US_AssociationDataTable, PUSH_AssociationDataTable, FW_AssociationDataTable };
        #endregion

        #region Methods
        public static void InitializeColumnsAssociationTables()
        {
            try
            {
                for (int i = 0; i < AssociationsDTArray.Length; i++)
                {
                    if (AssociationsDTArray[i].Columns.Count > 0)
                        AssociationsDTArray[i].Columns.Clear();
                    foreach (var colName in columnArray)
                    {
                        AssociationsDTArray[i].Columns.Add(colName, typeof(string));
                    }
                    AssociationsDTArray[i].AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message.ToString());
            }
        }
        public static DataTable GetObjectListTable(string LoadData, AssociationType associationType, out int obisCount)
        {
            DataTable dataTable = Current_AssociationDataTable.Clone();
            obisCount = 0;
            try
            {
                if (LoadData == "0100" || LoadData.Length < 15)
                    return dataTable;
                List<string> objectList = Regex.Split(LoadData, "02041200").ToList();
                obisCount = objectList.Count - 1;
                if (objectList.Count < 2)
                    return dataTable;
                int count = 0;
                if (objectList[0].Substring(2, 2) == "81")
                {
                    count = int.Parse(objectList[0].Substring(4, 2), NumberStyles.HexNumber);
                }
                else if (objectList[0].Substring(2, 2) == "82")
                {
                    count = int.Parse(objectList[0].Substring(4, 4), NumberStyles.HexNumber);
                }
                else
                {
                    count = int.Parse(objectList[0].Substring(2, 2), NumberStyles.HexNumber);
                }
                if (count != obisCount)
                    log.Error($"Count Mismatch: Array Length is {count} and actual OBIS present is {obisCount}");
                obisCount = count;
                SetAssociationVersion(LoadData, associationType);
                int associationVersion = 0;
                switch (associationType)
                {
                    case AssociationType.Current_Association:
                        associationVersion = CurrentAssociationVersion;
                        break;
                    case AssociationType.Public_Client:
                        associationVersion = PCAssociationVersion;
                        break;
                    case AssociationType.Meter_Reader:
                        associationVersion = MRAssociationVersion;
                        break;
                    case AssociationType.Utility_Settings:
                        associationVersion = USAssociationVersion;
                        break;
                    case AssociationType.PUSH:
                        associationVersion = PUSHAssociationVersion;
                        break;
                    case AssociationType.Firmware_Upgrade:
                        associationVersion = FWAssociationVersion;
                        break;
                }
                for (int i = 1; i < obisCount + 1; i++)
                {
                    string CLASSID = "00" + objectList[i].Substring(0, 2);
                    string VERSION = objectList[i].Substring(4, 2);
                    //string OBIS = DLMSParser.GetObis(objectList[i].Substring(10, 12));
                    string OBIS = objectList[i].Substring(10, 12);
                    string attributeAccessData = objectList[i].Substring(28);
                    StringBuilder attributeAccess = new StringBuilder();
                    StringBuilder methodAccess = new StringBuilder();
                    int attCount = Convert.ToInt32(attributeAccessData.Substring(0, 2), 16);
                    attributeAccess.Append(attCount + " ");
                    string strMatch = attributeAccessData.Substring(2, 6);
                    int j = 2;
                    int nLength = 6;
                    while (attCount > 0)
                    {
                        if (attributeAccessData.Substring(j, nLength) == strMatch)
                        {
                            j += nLength;
                            while (true)
                            {
                                attributeAccess.Append(Convert.ToInt32(attributeAccessData.Substring(j, 2), 16));
                                attributeAccess.Append("-");
                                j += 4;
                                attributeAccess.Append(GetAttributeAccessMode(associationVersion, attributeAccessData.Substring(j, 2)));
                                j += 2;
                                if (attributeAccessData.Substring(j, 2) == "00")
                                {
                                    attributeAccess.Append(" ");
                                    j += 2;
                                    break;
                                }
                                else
                                {
                                    j += 2;
                                    int accessSelectorCount = Convert.ToInt32(attributeAccessData.Substring(j, 2));
                                    j += 2;
                                    attributeAccess.Append($"(access_selectors:");
                                    for (int selectorCount = 0; selectorCount < accessSelectorCount; selectorCount++)
                                    {
                                        j += 2;
                                        attributeAccess.Append(Convert.ToInt32(attributeAccessData.Substring(j, 2), 16));
                                        if (selectorCount < accessSelectorCount - 1)
                                            attributeAccess.Append(",");
                                        j += 2;
                                    }
                                    attributeAccess.Append(") ");
                                    break;
                                }
                            }
                        }
                        attCount--;
                    }
                    /*
                    //for (int attIndex = 1; attIndex <= attCount; attIndex++)
                    //{
                    //    string entry = attributeAccessData.Substring(j, nLength);
                    //    attributeAccess.Append(Convert.ToInt32(entry.Substring(6, 2), 16));
                    //    attributeAccess.Append("-");
                    //    attributeAccess.Append(GetAttributeAccessMode(associationVersion, entry.Substring(10, 2)));
                    //    attributeAccess.Append(" ");
                    //    j += nLength;
                    //}*/
                    string methodAccessData = attributeAccessData.Substring(j);
                    if (methodAccessData.Length > 3 && methodAccessData.Substring(2, 2) != "00")
                    {
                        methodAccessData = methodAccessData.Substring(2);
                        int methodCount = Convert.ToInt32(methodAccessData.Substring(0, 2), 16);
                        methodAccess.Append(methodCount + " ");
                        nLength = 12;
                        j = 2;
                        for (int attIndex = 1; attIndex <= methodCount; attIndex++)
                        {
                            string entry = methodAccessData.Substring(j, nLength);
                            methodAccess.Append(Convert.ToInt32(entry.Substring(6, 2), 16));
                            methodAccess.Append("-");
                            methodAccess.Append(GetMethodAccessMode(associationVersion, entry.Substring(10, 2)));
                            methodAccess.Append(" ");
                            j += nLength;
                        }
                    }
                    dataTable.Rows.Add(i, CLASSID, VERSION, OBIS, Convert.ToInt32(CLASSID, 16).ToString() + " - " + DLMSParser.GetObis(OBIS) + " - " + DLMSParser.GetObisName(Convert.ToInt32(CLASSID, 16).ToString(), DLMSParser.GetObis(OBIS)), attributeAccess.ToString().Trim(), methodAccess.ToString().Trim());
                }
                switch (associationType)
                {
                    case AssociationType.Current_Association:
                        Current_AssociationDataTable = dataTable.Copy();
                        IsCurrentAssociationAvailable = true;
                        CurrentAssociationObjectDataString = LoadData;
                        break;
                    case AssociationType.Public_Client:
                        PC_AssociationDataTable = dataTable.Copy();
                        IsPCAssociationAvailable = true;
                        PCAssociationObjectDataString = LoadData;
                        break;
                    case AssociationType.Meter_Reader:
                        MR_AssociationDataTable = dataTable.Copy();
                        IsMRAssociationAvailable = true;
                        MRAssociationObjectDataString = LoadData;
                        break;
                    case AssociationType.Utility_Settings:
                        US_AssociationDataTable = dataTable.Copy();
                        IsUSAssociationAvailable = true;
                        USAssociationObjectDataString = LoadData;
                        break;
                    case AssociationType.PUSH:
                        PUSH_AssociationDataTable = dataTable.Copy();
                        IsPUSHAssociationAvailable = true;
                        PUSHAssociationObjectDataString = LoadData;
                        break;
                    case AssociationType.Firmware_Upgrade:
                        FW_AssociationDataTable = dataTable.Copy();
                        IsFWAssociationAvailable = true;
                        FWAssociationObjectDataString = LoadData;
                        break;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message.ToString());
                log.Error(ex.StackTrace);
            }
            return dataTable;

        }
        public static string GetAttributeAccessMode(int associationVersion, string hexValue)
        {
            string mode = "";
            int enumValue = Convert.ToInt32(hexValue, 16);
            switch (associationVersion)
            {
                case 0:
                    if (Enum.IsDefined(typeof(AttributeAccessModeVer0), enumValue))
                        mode = ((AttributeAccessModeVer1)enumValue).ToString();
                    else
                        mode = "undefined"; // Or any custom fall back value
                    break;
                case 1:
                    if (Enum.IsDefined(typeof(AttributeAccessModeVer1), enumValue))
                        mode = ((AttributeAccessModeVer1)enumValue).ToString();
                    else
                        mode = "undefined"; // Or any custom fall back value
                    break;
                case 2:
                    if (Enum.IsDefined(typeof(AttributeAccessModeVer2), enumValue))
                        mode = ((AttributeAccessModeVer1)enumValue).ToString();
                    else
                        mode = "undefined"; // Or any custom fall back value
                    break;
                case 3:
                    if (enumValue == 0)
                    {
                        mode = "no_access";
                        break;
                    }
                    string bitString = new string(Convert.ToString(enumValue, 2).PadLeft(hexValue.Length * 4, '0').Reverse().ToArray());
                    if (bitString.Substring(0, 1) == "1")
                        mode += "read_access,";
                    if (bitString.Substring(1, 1) == "1")
                        mode += "write_access,";
                    if (bitString.Substring(2, 1) == "1")
                        mode += "authenticated_request,";
                    if (bitString.Substring(3, 1) == "1")
                        mode += "encrypted_request,";
                    if (bitString.Substring(4, 1) == "1")
                        mode += "digitally_signed_request,";
                    if (bitString.Substring(5, 1) == "1")
                        mode += "authenticated_response,";
                    if (bitString.Substring(6, 1) == "1")
                        mode += "encrypted_response,";
                    if (bitString.Substring(7, 1) == "1")
                        mode += "digitally_signed_response,";
                    if (mode.EndsWith(","))
                    {
                        mode = mode.Substring(0, mode.Length - 1);
                    }
                    break;
            }
            return mode;
        }
        public static string GetMethodAccessMode(int associationVersion, string hexValue)
        {
            string mode = "";
            int enumValue = Convert.ToInt32(hexValue, 16);
            switch (associationVersion)
            {
                case 0:
                    if (Enum.IsDefined(typeof(MethodAccessModeVer0), enumValue))
                        mode = ((MethodAccessModeVer0)enumValue).ToString();
                    else
                        mode = "undefined"; // Or any custom fall back value
                    break;
                case 1:
                    if (Enum.IsDefined(typeof(MethodAccessModeVer1), enumValue))
                        mode = ((MethodAccessModeVer1)enumValue).ToString();
                    else
                        mode = "undefined"; // Or any custom fall back value
                    break;
                case 2:
                    if (Enum.IsDefined(typeof(MethodAccessModeVer2), enumValue))
                        mode = ((MethodAccessModeVer2)enumValue).ToString();
                    else
                        mode = "undefined"; // Or any custom fall back value
                    break;
                case 3:
                    if (enumValue == 0)
                    {
                        mode = "no_access";
                        break;
                    }
                    string bitString = new string(Convert.ToString(enumValue, 2).PadLeft(hexValue.Length * 4, '0').Reverse().ToArray());
                    if (bitString.Substring(0, 1) == "1")
                        mode += "access,";
                    if (bitString.Substring(1, 1) == "1")
                        mode += "not_used,";
                    if (bitString.Substring(2, 1) == "1")
                        mode += "authenticated_request,";
                    if (bitString.Substring(3, 1) == "1")
                        mode += "encrypted_request,";
                    if (bitString.Substring(4, 1) == "1")
                        mode += "digitally_signed_request,";
                    if (bitString.Substring(5, 1) == "1")
                        mode += "authenticated_response,";
                    if (bitString.Substring(6, 1) == "1")
                        mode += "encrypted_response,";
                    if (bitString.Substring(7, 1) == "1")
                        mode += "digitally_signed_response,";
                    if (mode.EndsWith(","))
                    {
                        mode = mode.Substring(0, mode.Length - 1);
                    }
                    break;
            }
            return mode;
        }
        public static List<string> FindVersionBytes(string input)
        {
            List<string> result = new List<string>();
            string[] obisList = { "0906" + CurrentAssociationOBIS, "0906" + PCAssociationOBIS, "0906" + MRAssociationOBIS, "0906" + USAssociationOBIS, "0906" + PUSHAssociationOBIS, "0906" + FWAssociationOBIS };
            foreach (string obis in obisList)
            {
                int index = input.IndexOf(obis);
                if (index != -1 && index >= 2)
                {
                    string prefix = input.Substring(index - 2, 2);
                    result.Add(prefix);
                }
                else
                {
                    result.Add("100");
                }
            }
            return result;
        }
        public static void SetAssociationVersion(string LoadData, AssociationType associationType)
        {
            List<string> prefixes = FindVersionBytes(LoadData);
            switch (associationType)
            {
                case AssociationType.Current_Association:
                    if (prefixes[0] != "100")
                        CurrentAssociationVersion = Convert.ToInt32(prefixes[0], 16);
                    break;
                case AssociationType.Public_Client:
                    if (prefixes[1] != "100")
                        PCAssociationVersion = Convert.ToInt32(prefixes[1], 16);
                    break;
                case AssociationType.Meter_Reader:
                    if (prefixes[2] != "100")
                        MRAssociationVersion = Convert.ToInt32(prefixes[2], 16);
                    break;
                case AssociationType.Utility_Settings:
                    if (prefixes[3] != "100")
                        USAssociationVersion = Convert.ToInt32(prefixes[3], 16);
                    break;
                case AssociationType.PUSH:
                    if (prefixes[4] != "100")
                        PUSHAssociationVersion = Convert.ToInt32(prefixes[4], 16);
                    break;
                case AssociationType.Firmware_Upgrade:
                    if (prefixes[4] != "100")
                        FWAssociationVersion = Convert.ToInt32(prefixes[5], 16);
                    break;
            }
        }

        public static void ClearAllAssociations()
        {
            IsCurrentAssociationAvailable = false;
            IsPCAssociationAvailable = false;
            IsMRAssociationAvailable = false;
            IsUSAssociationAvailable = false;
            IsPUSHAssociationAvailable = false;
            IsFWAssociationAvailable = false;
            CurrentAssociationObjectDataString = "";
            PCAssociationObjectDataString = "";
            MRAssociationObjectDataString = "";
            USAssociationObjectDataString = "";
            PUSHAssociationObjectDataString = "";
            FWAssociationObjectDataString = "";
            try
            {
                for (int i = 0; i < AssociationsDTArray.Length; i++)
                {
                    if (AssociationsDTArray[i].Rows.Count > 0)
                        AssociationsDTArray[i].Rows.Clear();
                    AssociationsDTArray[i].AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message.ToString());
            }
        }
        public static AssociationType GetAssociationTypeByObis(string Obis)
        {
            AssociationType type = AssociationType.Current_Association;
            switch (Obis)
            {
                case "0.0.40.0.0.255":
                    type = AssociationType.Current_Association;
                    break;
                case "0.0.40.0.1.255":
                    type = AssociationType.Public_Client;
                    break;
                case "0.0.40.0.2.255":
                    type = AssociationType.Meter_Reader;
                    break;
                case "0.0.40.0.3.255":
                    type = AssociationType.Utility_Settings;
                    break;
                case "0.0.40.0.4.255":
                    type = AssociationType.PUSH;
                    break;
                case "0.0.40.0.5.255":
                    type = AssociationType.Firmware_Upgrade;
                    break;
            }
            return type;
        }
        #endregion

        #region Enum
        /// <summary>
        /// Association LN (class_id = 15, version = 0)
        /// </summary>
        public enum AttributeAccessModeVer0
        {
            no_access = 0,
            read_only = 1,
            write_only = 2,
            read_and_write = 3
        }
        /// <summary>
        /// Association LN (class_id = 15, version = 1)
        /// </summary>
        public enum AttributeAccessModeVer1
        {
            no_access = 0,
            read_only = 1,
            write_only = 2,
            read_and_write = 3,
            authenticated_read_only = 4,
            authenticated_write_only = 5,
            authenticated_read_and_write = 6
        }
        /// <summary>
        /// Association LN (class_id = 15, version = 2)
        /// </summary>
        public enum AttributeAccessModeVer2
        {
            no_access = 0,
            read_only = 1,
            write_only = 2,
            read_and_write = 3,
            authenticated_read_only = 4,
            authenticated_write_only = 5,
            authenticated_read_and_write = 6
        }
        /// <summary>
        /// Association LN (class_id = 15, version = 0)
        /// </summary>
        public enum MethodAccessModeVer0
        {
            no_access = 0,
            access = 1
        }
        /// <summary>
        /// Association LN (class_id = 15, version = 1)
        /// </summary>
        public enum MethodAccessModeVer1
        {
            no_access = 0,
            access = 1,
            authenticated_access = 1
        }
        /// <summary>
        /// Association LN (class_id = 15, version = 2)
        /// </summary>
        public enum MethodAccessModeVer2
        {
            no_access = 0,
            access = 1,
            authenticated_access = 1
        }
        public enum AssociationType
        {
            Current_Association,
            Public_Client,
            Meter_Reader,
            Utility_Settings,
            PUSH,
            Firmware_Upgrade
        }
        #endregion
    }
}
