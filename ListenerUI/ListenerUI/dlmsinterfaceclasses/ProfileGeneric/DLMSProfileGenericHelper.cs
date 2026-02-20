using AutoTest.FrameWork.Converts;
using AutoTestDesktopWFA;
using Gurux.DLMS.Enums;
using log4net;
using MeterComm;
using MeterReader.DLMSNetSerialCommunication;
using MeterReader.TestHelperClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace MeterReader.DLMSInterfaceClasses.ProfileGeneric
{
    public class DLMSProfileGenericHelper
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

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
        public static bool IsScalerandColumnsAvailable = false;
        static readonly DLMSParser parse = new DLMSParser();

        public static void SaveToXml(DLMSProfileGenericCollection collection)
        {
            string filePath = System.IO.Path.Combine(Utilities.DLMSClasesXMLFilePath, "ProfileGeneric.xml");
            XmlSerializer serializer = new XmlSerializer(typeof(DLMSProfileGenericCollection));
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, collection);
            }
        }

        public static DLMSProfileGenericCollection LoadFromXml()
        {
            string filePath = System.IO.Path.Combine(Utilities.DLMSClasesXMLFilePath, "ProfileGeneric.xml");
            XmlSerializer serializer = new XmlSerializer(typeof(DLMSProfileGenericCollection));
            using (StreamReader reader = new StreamReader(filePath))
            {
                return (DLMSProfileGenericCollection)serializer.Deserialize(reader);
            }
        }
        public static DLMSProfileGeneric GetProfileByLogicalName(string logicalName)
        {
            var collection = LoadFromXml();
            return collection.Profiles.FirstOrDefault(p => p.logical_name == logicalName);
        }
        public static bool UpdateProfile(string logicalName, Action<DLMSProfileGeneric> updateAction)
        {
            var collection = LoadFromXml();
            var profile = collection.Profiles.FirstOrDefault(p => p.logical_name == logicalName);
            if (profile != null)
            {
                updateAction(profile);
                SaveToXml(collection);
                return true;
            }
            return false;
        }
        public static void ClearAllFlags()
        {
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
            IsScalerandColumnsAvailable = false;
        }

        public static Dictionary<string, object> GetNameplateProfileDataTable(ref DLMSComm DLMSObj)
        {
            //DLMSParser parse = new DLMSParser();
            string recivedObisString = "", recivedValueString = "", profileObis = "";
            DataTable obisDataTable = new DataTable();
            // Add columns to the DataTable
            obisDataTable.Columns.Add("Obis", typeof(string));
            obisDataTable.Columns.Add("Name", typeof(string));
            obisDataTable.Columns.Add("Data", typeof(string));
            DataTable resultDataTable = new DataTable();
            bool result = false;
            byte nWait = 0, nTryCount = 3, nTimeOut = 3;
            string _recData = "";
            try
            {
                profileObis = string.Concat("0.0.94.91.10.255".Trim().Split('.').Select(part => int.Parse(part).ToString("X2")));
                #region Data Getting Logic
                if (!DLMSProfileGenericHelper.IsNameplateAvailable)
                {
                    //Get Profile Objects
                    result = DLMSObj.GetParameter($"0007{profileObis}03", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                    if (result)
                    {
                        recivedObisString = DLMSObj.strbldDLMdata.ToString().Trim();
                        _recData = DLMSObj.strbldDLMdata.ToString().Trim().Split(' ')[3];
                        DLMSProfileGenericHelper.UpdateProfile("0.0.94.91.10.255", profile =>
                        {
                            profile.capture_objects = _recData;
                        });
                        SetProfileStatus(profileObis, true);
                    }
                    else
                    {
                        recivedObisString = "";
                        SetProfileStatus(profileObis, false);
                    }
                    //Get Profile Values
                    result = DLMSObj.GetParameter($"0007{profileObis}02", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                    if (result)
                    {
                        recivedValueString = DLMSObj.strbldDLMdata.ToString().Trim();
                        _recData = DLMSObj.strbldDLMdata.ToString().Trim().Split(' ')[3];
                        DLMSProfileGenericHelper.UpdateProfile("0.0.94.91.10.255", profile =>
                        {
                            profile.buffer = _recData;
                        });
                        SetProfileStatus(profileObis, true);
                    }
                    else
                    {
                        recivedValueString = "";
                        SetProfileStatus(profileObis, false);
                    }
                }
                else
                {
                    recivedObisString = DLMSProfileGenericHelper.GetProfileByLogicalName("0.0.94.91.10.255").capture_objects;
                    recivedValueString = DLMSProfileGenericHelper.GetProfileByLogicalName("0.0.94.91.10.255").buffer;
                }
                #endregion
                obisDataTable = parse.GetParameterTableHorizontal(recivedObisString);
                //obisDataTable = parse.GetClassObisAttScalerListParsing(recivedObisString, option);
                #region Add a new column for serial numbers
                DataColumn snColumn = new DataColumn("SN", typeof(string));
                obisDataTable.Columns.Add(snColumn);
                // Set the new column to be the first column
                snColumn.SetOrdinal(0);
                // Populate the SN column with row numbers
                for (int i = 0; i < obisDataTable.Rows.Count; i++)
                {
                    obisDataTable.Rows[i]["SN"] = i + 1;
                }
                obisDataTable.AcceptChanges();
                #endregion

                resultDataTable = parse.GetBillingValuesDataTableParsing(recivedValueString, obisDataTable);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message.ToString());
                log.Error(ex.StackTrace.ToString());
                // Create a dictionary to hold the DataTable and strings
                return new Dictionary<string, object>
                        {
                            { "DataTable", resultDataTable },
                            { "ProfileObis", recivedObisString},
                            { "ProfileValues", recivedValueString }
                        };
            }

            // Create a dictionary to hold the DataTable and strings
            return new Dictionary<string, object>
                        {
                            { "DataTable", resultDataTable },
                            { "ProfileObis", recivedObisString},
                            { "ProfileValues", recivedValueString }
                        };
        }

        public static Dictionary<string, object> GetNameplateProfileDataTableWrapper(ref WrapperComm WrapperObj)
        {
            //DLMSParser parse = new DLMSParser();
            string recivedObisString = "", recivedValueString = "", profileObis = "";
            DataTable obisDataTable = new DataTable();
            // Add columns to the DataTable
            obisDataTable.Columns.Add("Obis", typeof(string));
            obisDataTable.Columns.Add("Name", typeof(string));
            obisDataTable.Columns.Add("Data", typeof(string));
            DataTable resultDataTable = new DataTable();
            bool result = false;
            byte nWait = 0, nTryCount = 3, nTimeOut = 3;
            string _recData = "";
            try
            {
                profileObis = string.Concat("0.0.94.91.10.255".Trim().Split('.').Select(part => int.Parse(part).ToString("X2")));
                #region Data Getting Logic
                if (!DLMSProfileGenericHelper.IsNameplateAvailable)
                {
                    //Get Profile Objects
                    if (SetGetFromMeter.GetDataFromWrapperObject(ref WrapperObj, ObjectType.ProfileGeneric, "0.0.94.91.10.255", 3, out _recData))
                    {
                        recivedObisString = _recData;
                        DLMSProfileGenericHelper.UpdateProfile("0.0.94.91.10.255", profile =>
                        {
                            profile.capture_objects = _recData;
                        });
                        SetProfileStatus(profileObis, true);
                    }
                    else
                    {
                        recivedObisString = "";
                        SetProfileStatus(profileObis, false);
                    }
                    //Get Profile Values
                    if (SetGetFromMeter.GetDataFromWrapperObject(ref WrapperObj, ObjectType.ProfileGeneric, "0.0.94.91.10.255", 2, out _recData))
                    {
                        recivedValueString = _recData;
                        DLMSProfileGenericHelper.UpdateProfile("0.0.94.91.10.255", profile =>
                        {
                            profile.buffer = _recData;
                        });
                        SetProfileStatus(profileObis, true);
                    }
                    else
                    {
                        recivedValueString = "";
                        SetProfileStatus(profileObis, false);
                    }
                }
                else
                {
                    recivedObisString = DLMSProfileGenericHelper.GetProfileByLogicalName("0.0.94.91.10.255").capture_objects;
                    recivedValueString = DLMSProfileGenericHelper.GetProfileByLogicalName("0.0.94.91.10.255").buffer;
                }
                #endregion
                obisDataTable = parse.GetParameterTableHorizontal(recivedObisString);
                //obisDataTable = parse.GetClassObisAttScalerListParsing(recivedObisString, option);
                #region Add a new column for serial numbers
                DataColumn snColumn = new DataColumn("SN", typeof(string));
                obisDataTable.Columns.Add(snColumn);
                // Set the new column to be the first column
                snColumn.SetOrdinal(0);
                // Populate the SN column with row numbers
                for (int i = 0; i < obisDataTable.Rows.Count; i++)
                {
                    obisDataTable.Rows[i]["SN"] = i + 1;
                }
                obisDataTable.AcceptChanges();
                #endregion

                resultDataTable = parse.GetBillingValuesDataTableParsing(recivedValueString, obisDataTable);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message.ToString());
                log.Error(ex.StackTrace.ToString());
                // Create a dictionary to hold the DataTable and strings
                return new Dictionary<string, object>
                        {
                            { "DataTable", resultDataTable },
                            { "ProfileObis", recivedObisString},
                            { "ProfileValues", recivedValueString }
                        };
            }

            // Create a dictionary to hold the DataTable and strings
            return new Dictionary<string, object>
                        {
                            { "DataTable", resultDataTable },
                            { "ProfileObis", recivedObisString},
                            { "ProfileValues", recivedValueString }
                        };
        }


        #region Methods to GET EVENTS RELATED, INSTANT and LS DE DATA TABLE
        public static Dictionary<string, object> GetProfileDataTable(ref DLMSComm DLMSObj, string profileObis, string scalerObis, int _startIndex = 0, int _endIndex = 0, byte nType = 2, string _startDT = null, string _endDT = null)
        {
            //DLMSParser parse = new DLMSParser();
            string recivedObisString = "", recivedValueString = "", recivedScalerObisString = "", recivedScalerValueString = "", inUseEntries = "", profileEntries = "";
            DataTable obisDataTable = new DataTable();
            DataTable resultDataTable = new DataTable();
            string[] scalerObisArray = null, scalerScalerDataArray = null, mainSourceObisArray = null, finalScalerValuesToFill = null, ScalerMultiFactorArray = null;
            bool result = false;
            byte nWait = 0, nTryCount = 3, nTimeOut = 3;
            bool IsProfileAvaialble = false;
            string _recData = "";
            string profileObisInt = profileObis;
            string scalerObisInt = scalerObis;
            try
            {
                profileObis = string.Concat(profileObis.Trim().Split('.').Select(part => int.Parse(part).ToString("X2")));
                switch (profileObis)
                {
                    case "0000636200FF":
                        IsProfileAvaialble = DLMSProfileGenericHelper.IsVoltageAvailable;
                        break;
                    case "0000636201FF":
                        IsProfileAvaialble = DLMSProfileGenericHelper.IsCurrentAvailable;
                        break;
                    case "0000636202FF":
                        IsProfileAvaialble = DLMSProfileGenericHelper.IsPowerAvailable;
                        break;
                    case "0000636203FF":
                        IsProfileAvaialble = DLMSProfileGenericHelper.IsTransactionsAvailable;
                        break;
                    case "0000636204FF":
                        IsProfileAvaialble = DLMSProfileGenericHelper.IsOtherAvailable;
                        break;
                    case "0000636205FF":
                        IsProfileAvaialble = DLMSProfileGenericHelper.IsNonRolloverAvailable;
                        break;
                    case "0000636206FF":
                        IsProfileAvaialble = DLMSProfileGenericHelper.IsControlAvailable;
                        break;
                    case "0100620100FF":
                        IsProfileAvaialble = DLMSProfileGenericHelper.IsBillAvailable;
                        break;
                    case "01005E5B00FF":
                        IsProfileAvaialble = DLMSProfileGenericHelper.IsInstantAvailable;
                        break;
                    case "0000636281FF":
                        break;
                    case "0100630100FF":
                        IsProfileAvaialble = DLMSProfileGenericHelper.IsLSAvailable;
                        break;
                    case "0100630200FF":
                        IsProfileAvaialble = DLMSProfileGenericHelper.IsDEAvailable;
                        break;
                }
                scalerObis = string.Concat(scalerObis.Trim().Split('.').Select(part => int.Parse(part).ToString("X2")));

                #region Data Getting Logic
                if (!IsProfileAvaialble)
                {
                    //Get In use Entries
                    result = DLMSObj.GetParameter($"0007{profileObis}07", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                    if (result && DLMSObj.strbldDLMdata.ToString().Trim().Split(' ').Length == 4)
                    {
                        _recData = DLMSObj.strbldDLMdata.ToString().Trim().Split(' ')[3];
                        inUseEntries = parse.GetProfileValueString(DLMSObj.strbldDLMdata.ToString().Trim().Split(' ')[3]);
                        DLMSProfileGenericHelper.UpdateProfile(profileObisInt, profile =>
                        {
                            profile.entries_in_use = _recData;
                        });
                    }
                    else
                    {
                        inUseEntries = "";
                        SetProfileStatus(profileObis, false);
                    }
                    //Get In Profile Entries
                    result = DLMSObj.GetParameter($"0007{profileObis}08", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                    if (result && DLMSObj.strbldDLMdata.ToString().Trim().Split(' ').Length == 4)
                    {
                        _recData = DLMSObj.strbldDLMdata.ToString().Trim().Split(' ')[3];
                        profileEntries = parse.GetProfileValueString(DLMSObj.strbldDLMdata.ToString().Trim().Split(' ')[3]);
                        DLMSProfileGenericHelper.UpdateProfile(profileObisInt, profile =>
                        {
                            profile.profile_entries = _recData;
                        });
                    }
                    else
                    {
                        profileEntries = "";
                        SetProfileStatus(profileObis, false);
                    }
                    //Get Profile Objects
                    result = DLMSObj.GetParameter($"0007{profileObis}03", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                    if (result)
                    {
                        recivedObisString = DLMSObj.strbldDLMdata.ToString().Trim();
                        _recData = DLMSObj.strbldDLMdata.ToString().Trim().Split(' ')[3];
                        DLMSProfileGenericHelper.UpdateProfile(profileObisInt, profile =>
                        {
                            profile.capture_objects = _recData;
                        });
                    }
                    else
                    {
                        recivedObisString = "";
                        SetProfileStatus(profileObis, false);
                    }
                    //Get Profile Values
                    if (profileObisInt != "1.0.99.1.0.255" && profileObisInt != "1.0.99.2.0.255")//LS and DE
                        result = DLMSObj.GetParameter($"0007{profileObis}02", nWait, nTryCount, nTimeOut, nType, DateTime.Now, DateTime.Now, string.Empty, (ulong)_startIndex, (ulong)_endIndex);
                    else
                    {
                        if (nType == 0)
                            result = DLMSObj.GetParameter($"0007{profileObis}02", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                        else
                            result = DLMSObj.GetParameter($"0007{profileObis}02", nWait, nTryCount, nTimeOut, (byte)1, DateTime.ParseExact(_startDT, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture), DateTime.ParseExact(_endDT, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture), "12000809060000010000FF0F02120000", 0UL, 0UL);
                    }
                    if (result)
                    {
                        recivedValueString = DLMSObj.strbldDLMdata.ToString().Trim();
                        _recData = DLMSObj.strbldDLMdata.ToString().Trim().Split(' ')[3];
                        DLMSProfileGenericHelper.UpdateProfile(profileObisInt, profile =>
                        {
                            profile.buffer = _recData;
                        });
                    }
                    else
                    {
                        recivedValueString = "";
                        SetProfileStatus(profileObis, false);
                    }
                    //Get Scaler Objects
                    result = DLMSObj.GetParameter($"0007{scalerObis}03", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                    if (result)
                    {
                        recivedScalerObisString = DLMSObj.strbldDLMdata.ToString().Trim();
                        _recData = DLMSObj.strbldDLMdata.ToString().Trim().Split(' ')[3];
                        DLMSProfileGenericHelper.UpdateProfile(scalerObisInt, profile =>
                        {
                            profile.capture_objects = _recData;
                        });
                    }
                    else
                    {
                        recivedScalerObisString = "";
                        SetProfileStatus(profileObis, false);
                    }
                    //Get Scaler Values
                    result = DLMSObj.GetParameter($"0007{scalerObis}02", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                    if (result)
                    {
                        recivedScalerValueString = DLMSObj.strbldDLMdata.ToString().Trim();
                        _recData = DLMSObj.strbldDLMdata.ToString().Trim().Split(' ')[3];
                        DLMSProfileGenericHelper.UpdateProfile(scalerObisInt, profile =>
                        {
                            profile.buffer = _recData;
                        });
                    }
                    else
                    {
                        recivedScalerValueString = "";
                        SetProfileStatus(profileObis, false);
                    }
                }
                else
                {
                    //Get In use Entries
                    result = DLMSObj.GetParameter($"0007{profileObis}07", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                    if (result && DLMSObj.strbldDLMdata.ToString().Trim().Split(' ').Length == 4)
                    {
                        _recData = DLMSObj.strbldDLMdata.ToString().Trim().Split(' ')[3];
                        inUseEntries = parse.GetProfileValueString(DLMSObj.strbldDLMdata.ToString().Trim().Split(' ')[3]);
                        DLMSProfileGenericHelper.UpdateProfile(profileObisInt, profile =>
                        {
                            profile.entries_in_use = _recData;
                        });
                    }
                    else
                    {
                        inUseEntries = "";
                        SetProfileStatus(profileObis, false);
                    }
                    //Get In Profile Entries
                    result = DLMSObj.GetParameter($"0007{profileObis}08", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                    if (result && DLMSObj.strbldDLMdata.ToString().Trim().Split(' ').Length == 4)
                    {
                        _recData = DLMSObj.strbldDLMdata.ToString().Trim().Split(' ')[3];
                        profileEntries = parse.GetProfileValueString(DLMSObj.strbldDLMdata.ToString().Trim().Split(' ')[3]);
                        DLMSProfileGenericHelper.UpdateProfile(profileObisInt, profile =>
                        {
                            profile.profile_entries = _recData;
                        });
                    }
                    else
                    {
                        profileEntries = "";
                        SetProfileStatus(profileObis, false);
                    }
                    //Get Profile Values
                    if (profileObisInt != "1.0.99.1.0.255" && profileObisInt != "1.0.99.2.0.255")
                        result = DLMSObj.GetParameter($"0007{profileObis}02", nWait, nTryCount, nTimeOut, nType, DateTime.Now, DateTime.Now, string.Empty, (ulong)_startIndex, (ulong)_endIndex);
                    else
                    {
                        if (nType == 0)
                            result = DLMSObj.GetParameter($"0007{profileObis}02", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                        else
                            result = DLMSObj.GetParameter($"0007{profileObis}02", nWait, nTryCount, nTimeOut, (byte)1, DateTime.ParseExact(_startDT, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture), DateTime.ParseExact(_endDT, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture), "12000809060000010000FF0F02120000", 0UL, 0UL);
                    }
                    if (result)
                    {
                        recivedValueString = DLMSObj.strbldDLMdata.ToString().Trim();
                        _recData = DLMSObj.strbldDLMdata.ToString().Trim().Split(' ')[3];
                        DLMSProfileGenericHelper.UpdateProfile(profileObisInt, profile =>
                        {
                            profile.buffer = _recData;
                        });
                    }
                    else
                    {
                        recivedValueString = "";
                        SetProfileStatus(profileObis, false);
                    }
                    recivedObisString = GetProfileByLogicalName(profileObisInt).capture_objects;
                    recivedScalerObisString = GetProfileByLogicalName(scalerObisInt).capture_objects;
                    recivedScalerValueString = GetProfileByLogicalName(scalerObisInt).buffer;
                }
                #endregion

                //check weather the object list table is created.
                if (!parse.GetObjectsDataTable(recivedObisString, ref obisDataTable))
                {
                    // Create a dictionary to hold the DataTable and strings
                    return new Dictionary<string, object>
                        {
                            { "DataTable", resultDataTable },
                            { "ProfileObis", recivedObisString},
                            { "ProfileValues", recivedValueString },
                            { "ScalerObis", recivedScalerObisString },
                            { "ScalerValues", recivedScalerValueString },
                            { "InUseEntries", inUseEntries },
                            { "ProfileEntries", profileEntries }
                        };
                }

                #region Append and find the Scaler Units and Scaler Factor
                scalerObisArray = parse.GetScalerProfileObisListParsing(recivedScalerObisString);
                scalerScalerDataArray = parse.GetScalerProfileScalerListParsing(recivedScalerValueString);
                mainSourceObisArray = new string[obisDataTable.Columns.Count];
                for (int i = 0; i < obisDataTable.Columns.Count; i++)
                {
                    mainSourceObisArray[i] = obisDataTable.Columns[i].ColumnName.Split('-')[1].Trim();
                }
                finalScalerValuesToFill = new string[mainSourceObisArray.Length];
                ScalerMultiFactorArray = new string[mainSourceObisArray.Length];
                foreach (var selectedObis in scalerObisArray)
                {
                    // Find the index of the searchString in the stringArray
                    int index = Array.IndexOf(mainSourceObisArray, selectedObis);
                    int scalarIndex = Array.IndexOf(scalerObisArray, selectedObis);
                    if (index != -1)
                    {
                        finalScalerValuesToFill[index] = scalerScalerDataArray[scalarIndex];
                        ScalerMultiFactorArray[index] = (string)parse.ScalerhshTable[scalerScalerDataArray[scalarIndex].Substring(2, 2)];
                        obisDataTable.Columns[index].ColumnName = (!string.IsNullOrEmpty((string)parse.UnithshTable[scalerScalerDataArray[scalarIndex].Substring(6, 2)])) ?
                                                                $"{obisDataTable.Columns[index].ColumnName} - {scalerScalerDataArray[scalarIndex]} - ({(string)parse.UnithshTable[scalerScalerDataArray[scalarIndex].Substring(6, 2)]})" :
                                                                $"{obisDataTable.Columns[index].ColumnName} - {scalerScalerDataArray[scalarIndex]}";
                    }
                }
                #endregion

                resultDataTable = parse.GetEventsLSDEValueDataTableParsing(recivedValueString, obisDataTable);
                // Multiply each row value with the corresponding string array value
                MultiplyRowsWithArray(resultDataTable, ScalerMultiFactorArray);

                #region Add a new column for serial numbers
                DataColumn snColumn = new DataColumn("SN", typeof(string));
                resultDataTable.Columns.Add(snColumn);
                // Set the new column to be the first column
                snColumn.SetOrdinal(0);
                // Populate the SN column with row numbers
                for (int i = 0; i < resultDataTable.Rows.Count; i++)
                {
                    resultDataTable.Rows[i]["SN"] = i + 1;
                }
                resultDataTable.AcceptChanges();
                #endregion
            }
            catch (Exception ex)
            {
                log.Error(ex.Message.ToString());
                SetProfileStatus(profileObis, false);
                // Create a dictionary to hold the DataTable and strings
                return new Dictionary<string, object>
                        {
                            { "DataTable", resultDataTable },
                            { "ProfileObis", recivedObisString},
                            { "ProfileValues", recivedValueString },
                            { "ScalerObis", recivedScalerObisString },
                            { "ScalerValues", recivedScalerValueString },
                            { "InUseEntries", inUseEntries },
                            { "ProfileEntries", profileEntries }
                        };
            }
            SetProfileStatus(profileObis, true);
            // Create a dictionary to hold the DataTable and strings
            return new Dictionary<string, object>
                        {
                            { "DataTable", resultDataTable },
                            { "ProfileObis", recivedObisString},
                            { "ProfileValues", recivedValueString },
                            { "ScalerObis", recivedScalerObisString },
                            { "ScalerValues", recivedScalerValueString },
                            { "InUseEntries", inUseEntries },
                            { "ProfileEntries", profileEntries }
                        };
        }
        #endregion

        #region Methods to GET EVENTS RELATED, INSTANT and LS DE DATA TABLE using Wrapper Object

        public static Dictionary<string, object> GetProfileDataTable(ref WrapperComm WrapperObj, string profileObis, string scalerObis, int _startIndex = 0, int _endIndex = 0, byte nType = 2, string _startDT = null, string _endDT = null)
        {
            DLMSParser parse = new DLMSParser();
            string recivedObisString = "", recivedValueString = "", recivedScalerObisString = "", recivedScalerValueString = "", inUseEntries = "", profileEntries = "";
            DataTable obisDataTable = new DataTable();
            DataTable resultDataTable = new DataTable();
            string[] scalerObisArray = null, scalerScalerDataArray = null, mainSourceObisArray = null, finalScalerValuesToFill = null, ScalerMultiFactorArray = null;
            bool result = false;
            byte nWait = 0, nTryCount = 3, nTimeOut = 3;
            bool IsProfileAvaialble = false;
            string _recData = "";
            string profileObisInt = profileObis;
            string scalerObisInt = scalerObis;
            try
            {
                profileObis = string.Concat(profileObis.Trim().Split('.').Select(part => int.Parse(part).ToString("X2")));
                switch (profileObis)
                {
                    case "0000636200FF":
                        IsProfileAvaialble = DLMSProfileGenericHelper.IsVoltageAvailable;
                        break;
                    case "0000636201FF":
                        IsProfileAvaialble = DLMSProfileGenericHelper.IsCurrentAvailable;
                        break;
                    case "0000636202FF":
                        IsProfileAvaialble = DLMSProfileGenericHelper.IsPowerAvailable;
                        break;
                    case "0000636203FF":
                        IsProfileAvaialble = DLMSProfileGenericHelper.IsTransactionsAvailable;
                        break;
                    case "0000636204FF":
                        IsProfileAvaialble = DLMSProfileGenericHelper.IsOtherAvailable;
                        break;
                    case "0000636205FF":
                        IsProfileAvaialble = DLMSProfileGenericHelper.IsNonRolloverAvailable;
                        break;
                    case "0000636206FF":
                        IsProfileAvaialble = DLMSProfileGenericHelper.IsControlAvailable;
                        break;
                    case "0100620100FF":
                        IsProfileAvaialble = DLMSProfileGenericHelper.IsBillAvailable;
                        break;
                    case "01005E5B00FF":
                        IsProfileAvaialble = DLMSProfileGenericHelper.IsInstantAvailable;
                        break;
                    case "0000636281FF":
                        break;
                    case "0100630100FF":
                        IsProfileAvaialble = DLMSProfileGenericHelper.IsLSAvailable;
                        break;
                    case "0100630200FF":
                        IsProfileAvaialble = DLMSProfileGenericHelper.IsDEAvailable;
                        break;
                }
                scalerObis = string.Concat(scalerObis.Trim().Split('.').Select(part => int.Parse(part).ToString("X2")));

                #region Data Getting Logic
                if (!IsProfileAvaialble)
                {
                    //Get In use Entries
                    WrapperObj.ReadCOSEMObject(ObjectType.ProfileGeneric, profileObisInt, 7);
                    _recData = WrapperComm.recData.Trim();
                    if (_recData.Length > 4)
                    {
                        inUseEntries = parse.GetProfileValueString(_recData);
                        DLMSProfileGenericHelper.UpdateProfile(profileObisInt, profile =>
                        {
                            profile.entries_in_use = _recData;
                        });
                    }
                    else
                    {
                        inUseEntries = "";
                        SetProfileStatus(profileObis, false);
                    }
                    //Get In Profile Entries
                    SetGetFromMeter.Wait(500);
                    WrapperObj.ReadCOSEMObject(ObjectType.ProfileGeneric, profileObisInt, 8);
                    _recData = WrapperComm.recData.Trim();
                    if (_recData.Length > 4)
                    {
                        inUseEntries = parse.GetProfileValueString(_recData);
                        DLMSProfileGenericHelper.UpdateProfile(profileObisInt, profile =>
                        {
                            profile.profile_entries = _recData;
                        });
                    }
                    else
                    {
                        profileEntries = "";
                        SetProfileStatus(profileObis, false);
                    }
                    //Get Profile Objects
                    SetGetFromMeter.Wait(500);
                    WrapperObj.ReadCOSEMObject(ObjectType.ProfileGeneric, profileObisInt, 3);
                    _recData = WrapperComm.recData.Trim();
                    if (_recData.Length > 14)
                    {
                        recivedObisString = _recData;
                        DLMSProfileGenericHelper.UpdateProfile(profileObisInt, profile =>
                        {
                            profile.capture_objects = _recData;
                        });
                    }
                    else
                    {
                        recivedObisString = "";
                        SetProfileStatus(profileObis, false);
                    }
                    //Get Profile Values
                    SetGetFromMeter.Wait(500);
                    if (nType != 0)
                        WrapperObj.ReadProfileGenericBuffer(profileObisInt, _startIndex, _endIndex, nType, _startDT, _endDT);
                    else
                        WrapperObj.ReadCOSEMObject(ObjectType.ProfileGeneric, profileObisInt, 2);
                    _recData = WrapperComm.recData.Trim();
                    if (_recData.Length > 14)
                    {
                        recivedValueString = _recData;
                        DLMSProfileGenericHelper.UpdateProfile(profileObisInt, profile =>
                        {
                            profile.buffer = _recData;
                        });
                    }
                    else
                    {
                        recivedValueString = "";
                        SetProfileStatus(profileObis, false);
                    }
                    //Get Scaler Objects
                    SetGetFromMeter.Wait(500);
                    WrapperObj.ReadCOSEMObject(ObjectType.ProfileGeneric, scalerObisInt, 3);
                    _recData = WrapperComm.recData.Trim();
                    if (_recData.Length > 14)
                    {
                        recivedScalerObisString = _recData;
                        DLMSProfileGenericHelper.UpdateProfile(scalerObisInt, profile =>
                        {
                            profile.capture_objects = _recData;
                        });
                    }
                    else
                    {
                        recivedScalerObisString = "";
                        SetProfileStatus(profileObis, false);
                    }
                    //Get Scaler Values
                    SetGetFromMeter.Wait(500);
                    WrapperObj.ReadCOSEMObject(ObjectType.ProfileGeneric, scalerObisInt, 2);
                    _recData = WrapperComm.recData.Trim();
                    if (_recData.Length > 14)
                    {
                        recivedScalerValueString = _recData;
                        DLMSProfileGenericHelper.UpdateProfile(scalerObisInt, profile =>
                        {
                            profile.buffer = _recData;
                        });
                    }
                    else
                    {
                        recivedScalerValueString = "";
                        SetProfileStatus(profileObis, false);
                    }
                }
                else
                {
                    //Get In use Entries
                    SetGetFromMeter.Wait(500);
                    WrapperObj.ReadCOSEMObject(ObjectType.ProfileGeneric, profileObisInt, 7);
                    _recData = WrapperComm.recData.Trim();
                    if (_recData.Length > 4)
                    {
                        inUseEntries = parse.GetProfileValueString(_recData);
                        DLMSProfileGenericHelper.UpdateProfile(profileObisInt, profile =>
                        {
                            profile.entries_in_use = _recData;
                        });
                    }
                    else
                    {
                        inUseEntries = "";
                        SetProfileStatus(profileObis, false);
                    }
                    //Get In Profile Entries
                    SetGetFromMeter.Wait(500);
                    WrapperObj.ReadCOSEMObject(ObjectType.ProfileGeneric, profileObisInt, 8);
                    _recData = WrapperComm.recData.Trim();
                    if (_recData.Length > 4)
                    {
                        inUseEntries = parse.GetProfileValueString(_recData);
                        DLMSProfileGenericHelper.UpdateProfile(profileObisInt, profile =>
                        {
                            profile.profile_entries = _recData;
                        });
                    }
                    else
                    {
                        profileEntries = "";
                        SetProfileStatus(profileObis, false);
                    }
                    //Get Profile Values
                    SetGetFromMeter.Wait(500);
                    if (nType != 0)
                        WrapperObj.ReadProfileGenericBuffer(profileObisInt, _startIndex, _endIndex, nType, _startDT, _endDT);
                    else
                        WrapperObj.ReadCOSEMObject(ObjectType.ProfileGeneric, profileObisInt, 2);
                    _recData = WrapperComm.recData.Trim();
                    if (_recData.Length > 14)
                    {
                        recivedValueString = _recData;
                        DLMSProfileGenericHelper.UpdateProfile(profileObisInt, profile =>
                        {
                            profile.buffer = _recData;
                        });
                    }
                    else
                    {
                        recivedValueString = "";
                        SetProfileStatus(profileObis, false);
                    }
                    recivedObisString = GetProfileByLogicalName(profileObisInt).capture_objects;
                    recivedScalerObisString = GetProfileByLogicalName(scalerObisInt).capture_objects;
                    recivedScalerValueString = GetProfileByLogicalName(scalerObisInt).buffer;
                }
                #endregion

                //check weather the object list table is created.
                if (!parse.GetObjectsDataTable(recivedObisString, ref obisDataTable))
                {
                    // Create a dictionary to hold the DataTable and strings
                    return new Dictionary<string, object>
                        {
                            { "DataTable", resultDataTable },
                            { "ProfileObis", recivedObisString},
                            { "ProfileValues", recivedValueString },
                            { "ScalerObis", recivedScalerObisString },
                            { "ScalerValues", recivedScalerValueString },
                            { "InUseEntries", inUseEntries },
                            { "ProfileEntries", profileEntries }
                        };
                }

                #region Append and find the Scaler Units and Scaler Factor
                scalerObisArray = parse.GetScalerProfileObisListParsing(recivedScalerObisString);
                scalerScalerDataArray = parse.GetScalerProfileScalerListParsing(recivedScalerValueString);
                mainSourceObisArray = new string[obisDataTable.Columns.Count];
                for (int i = 0; i < obisDataTable.Columns.Count; i++)
                {
                    mainSourceObisArray[i] = obisDataTable.Columns[i].ColumnName.Split('-')[1].Trim();
                }
                finalScalerValuesToFill = new string[mainSourceObisArray.Length];
                ScalerMultiFactorArray = new string[mainSourceObisArray.Length];
                foreach (var selectedObis in scalerObisArray)
                {
                    // Find the index of the searchString in the stringArray
                    int index = Array.IndexOf(mainSourceObisArray, selectedObis);
                    int scalarIndex = Array.IndexOf(scalerObisArray, selectedObis);
                    if (index != -1)
                    {
                        finalScalerValuesToFill[index] = scalerScalerDataArray[scalarIndex];
                        ScalerMultiFactorArray[index] = (string)parse.ScalerhshTable[scalerScalerDataArray[scalarIndex].Substring(2, 2)];
                        obisDataTable.Columns[index].ColumnName = (!string.IsNullOrEmpty((string)parse.UnithshTable[scalerScalerDataArray[scalarIndex].Substring(6, 2)])) ?
                                                                $"{obisDataTable.Columns[index].ColumnName} - {scalerScalerDataArray[scalarIndex]} - ({(string)parse.UnithshTable[scalerScalerDataArray[scalarIndex].Substring(6, 2)]})" :
                                                                $"{obisDataTable.Columns[index].ColumnName} - {scalerScalerDataArray[scalarIndex]}";
                    }
                }
                #endregion

                resultDataTable = parse.GetEventsLSDEValueDataTableParsing(recivedValueString, obisDataTable);
                // Multiply each row value with the corresponding string array value
                MultiplyRowsWithArray(resultDataTable, ScalerMultiFactorArray);

                #region Add a new column for serial numbers
                DataColumn snColumn = new DataColumn("SN", typeof(string));
                resultDataTable.Columns.Add(snColumn);
                // Set the new column to be the first column
                snColumn.SetOrdinal(0);
                // Populate the SN column with row numbers
                for (int i = 0; i < resultDataTable.Rows.Count; i++)
                {
                    resultDataTable.Rows[i]["SN"] = i + 1;
                }
                resultDataTable.AcceptChanges();
                #endregion
            }
            catch (Exception ex)
            {
                log.Error(ex.Message.ToString());
                SetProfileStatus(profileObis, false);
                // Create a dictionary to hold the DataTable and strings
                return new Dictionary<string, object>
                        {
                            { "DataTable", resultDataTable },
                            { "ProfileObis", recivedObisString},
                            { "ProfileValues", recivedValueString },
                            { "ScalerObis", recivedScalerObisString },
                            { "ScalerValues", recivedScalerValueString },
                            { "InUseEntries", inUseEntries },
                            { "ProfileEntries", profileEntries }
                        };
            }
            SetProfileStatus(profileObis, true);
            // Create a dictionary to hold the DataTable and strings
            return new Dictionary<string, object>
                        {
                            { "DataTable", resultDataTable },
                            { "ProfileObis", recivedObisString},
                            { "ProfileValues", recivedValueString },
                            { "ScalerObis", recivedScalerObisString },
                            { "ScalerValues", recivedScalerValueString },
                            { "InUseEntries", inUseEntries },
                            { "ProfileEntries", profileEntries }
                        };
        }
        #endregion

        public static void SetProfileStatus(string profileObis, bool status)
        {
            switch (profileObis)
            {
                case "0000636200FF":
                    DLMSProfileGenericHelper.IsVoltageAvailable = status;
                    break;
                case "0000636201FF":
                    DLMSProfileGenericHelper.IsCurrentAvailable = status;
                    break;
                case "0000636202FF":
                    DLMSProfileGenericHelper.IsPowerAvailable = status;
                    break;
                case "0000636203FF":
                    DLMSProfileGenericHelper.IsTransactionsAvailable = status;
                    break;
                case "0000636204FF":
                    DLMSProfileGenericHelper.IsOtherAvailable = status;
                    break;
                case "0000636205FF":
                    DLMSProfileGenericHelper.IsNonRolloverAvailable = status;
                    break;
                case "0000636206FF":
                    DLMSProfileGenericHelper.IsControlAvailable = status;
                    break;
                case "0100620100FF":
                    DLMSProfileGenericHelper.IsBillAvailable = status;
                    break;
                case "01005E5B00FF":
                    DLMSProfileGenericHelper.IsInstantAvailable = status;
                    break;
                case "0100630100FF":
                    DLMSProfileGenericHelper.IsLSAvailable = status;
                    break;
                case "0100630200FF":
                    DLMSProfileGenericHelper.IsDEAvailable = status;
                    break;
                case "00005E5B0AFF":
                    DLMSProfileGenericHelper.IsNameplateAvailable = status;
                    break;
            }
        }
        public static void MultiplyRowsWithArray(DataTable dataTable, string[] scalerMultiFactorArray)
        {
            double n;
            // Iterate through each row of the DataTable
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                // Iterate through each cell in the row
                for (int j = 0; j < dataTable.Columns.Count; j++)
                {
                    // Check if the string array value at the same index is not empty or null
                    if (!string.IsNullOrEmpty(scalerMultiFactorArray[j]))
                    {
                        if (double.TryParse(dataTable.Rows[i][j].ToString(), out n))
                        {
                            // Multiply the cell value with the corresponding string array value
                            double cellValue = Convert.ToDouble(dataTable.Rows[i][j]);
                            double factor = Convert.ToDouble(scalerMultiFactorArray[j]);
                            dataTable.Rows[i][j] = (cellValue * factor).ToString();
                        }
                    }
                }
            }
        }
        public static void RenameParameterWithUnit(DataTable dataTable, DLMSParser parse)
        {
            // Get the index of the "Scaler" column
            int scalerColumnIndex = dataTable.Columns.IndexOf("Scaler");
            // Get the index of the "Parameter" column
            int parameterColumnIndex = dataTable.Columns.IndexOf("Parameter Name");

            foreach (DataRow row in dataTable.Rows)
            {
                // Get the value of the "Scaler" column for the current row
                string unitString = row.Field<string>(scalerColumnIndex);
                // Check if the scaler value is not null or empty
                if (!string.IsNullOrEmpty(unitString))
                {
                    unitString = (string)parse.UnithshTable[unitString.Substring(6, 2)];
                    if (!string.IsNullOrEmpty(unitString))
                    {
                        // Iterate through each column in the DataTable
                        for (int i = 0; i < dataTable.Columns.Count; i++)
                        {
                            // Check if the column name contains "Data" and it's not the "Scaler" column
                            if (dataTable.Columns[i].ColumnName.Contains("Parameter"))
                            {
                                // Get the value of the current column for the current row
                                string parameterName = row.Field<string>(i);

                                // Multiply the value in the "Scaler" column with the value in the "Data" column
                                if (!string.IsNullOrEmpty(parameterName))
                                {
                                    row[i] = $"{parameterName} ({unitString})";
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void IsProfilesColumnsAvailable(ref WrapperComm WrapperObj)
        {
            if (!IsScalerandColumnsAvailable)
            {
                if (WrapperObj.GetAssociationView(null))
                {
                    WrapperObj.GetScalersAndUnits();
                    WrapperObj.GetProfileGenericColumns();
                    IsScalerandColumnsAvailable = true;
                }
            }
        }
    }
}
