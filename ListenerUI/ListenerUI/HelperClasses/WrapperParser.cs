using AutoTest.FrameWork.Converts;
using Gurux.Common;
using Gurux.DLMS;
using Gurux.DLMS.Objects;
using MeterReader.NicConfiguration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace MeterReader.Converter
{
    public class WrapperParser
    {
        CultureInfo provider = CultureInfo.InvariantCulture;
        //Logger
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static DLMSParser parse;
        public WrapperParser()
        {
            parse = new DLMSParser();
        }

        #region Profile Generic OBIS
        public const string Instantaneous = "1.0.94.91.0.255";
        public const string InstantaneousScaler = "1.0.94.91.3.255";
        public const string Billing = "1.0.98.1.0.255";
        public const string BillingScaler = "1.0.94.91.6.255";
        public const string DailyLoad = "1.0.99.2.0.255";
        public const string DailyLoadScaler = "1.0.94.91.5.255";
        public const string BlockLoad = "1.0.99.1.0.255";
        public const string BlockLoadScaler = "1.0.94.91.4.255";
        public const string VoltageEvents = "0.0.99.98.0.255";
        public const string CurrentEvents = "0.0.99.98.1.255";
        public const string PowerEvents = "0.0.99.98.2.255";
        public const string TransactionsEvents = "0.0.99.98.3.255";
        public const string NonRollOverEvents = "0.0.99.98.4.255";
        public const string OtherEvents = "0.0.99.98.5.255";
        public const string ControlEvents = "0.0.99.98.6.255";
        public const string EventsScaler = "1.0.94.91.7.255";
        public const string Nameplate = "0.0.94.91.10.255";
        #endregion

        #region Association OBIS
        public const string CurrentAssociation = "0.0.40.0.0.255";
        public const string PCAssociation = "0.0.40.0.1.255";
        public const string MRAssociation = "0.0.40.0.2.255";
        public const string USAssociation = "0.0.40.0.3.255";
        public const string PUSHAssociation = "0.0.40.0.4.255";
        public const string FWAssociation = "0.0.40.0.5.255";
        #endregion
        public enum DownloadOption
        {
            [XmlEnum("0")]
            AllData = 0,
            [XmlEnum("1")]
            AllDataWithoutLS = 1,
            [XmlEnum("2")]
            EventsData = 2,
            [XmlEnum("3")]
            LSData = 3,
            [XmlEnum("4")]
            DEData = 4,
            [XmlEnum("5")]
            Instantaneous = 5,
            [XmlEnum("6")]
            Billing = 6,
            [XmlEnum("7")]
            Nameplate = 7,
            [XmlEnum("8")]
            VoltageEvents = 8,
            [XmlEnum("9")]
            CurrentEvents = 9,
            [XmlEnum("10")]
            PowerEvents = 10,
            [XmlEnum("11")]
            TransactionsEvents = 11,
            [XmlEnum("12")]
            NonRollOverEvents = 12,
            [XmlEnum("13")]
            OtherEvents = 13,
            [XmlEnum("14")]
            ControlEvents = 14,
        }
        public List<string> GetProfileDownloadOBISList(DownloadOption option)
        {
            List<string> list = new List<string>();
            switch (option)
            {
                case DownloadOption.AllData:
                    list.Add(Instantaneous);
                    list.Add(InstantaneousScaler);
                    list.Add(Billing);
                    list.Add(BillingScaler);
                    list.Add(DailyLoad);
                    list.Add(DailyLoadScaler);
                    list.Add(BlockLoad);
                    list.Add(BlockLoadScaler);
                    list.Add(VoltageEvents);
                    list.Add(CurrentEvents);
                    list.Add(PowerEvents);
                    list.Add(TransactionsEvents);
                    list.Add(NonRollOverEvents);
                    list.Add(OtherEvents);
                    list.Add(ControlEvents);
                    list.Add(EventsScaler);
                    list.Add(Nameplate);
                    break;
                case DownloadOption.AllDataWithoutLS:
                    list.Add(Instantaneous);
                    list.Add(InstantaneousScaler);
                    list.Add(Billing);
                    list.Add(BillingScaler);
                    list.Add(DailyLoad);
                    list.Add(DailyLoadScaler);
                    list.Add(VoltageEvents);
                    list.Add(CurrentEvents);
                    list.Add(PowerEvents);
                    list.Add(TransactionsEvents);
                    list.Add(NonRollOverEvents);
                    list.Add(OtherEvents);
                    list.Add(ControlEvents);
                    list.Add(EventsScaler);
                    list.Add(Nameplate);
                    break;
                case DownloadOption.EventsData:
                    list.Add(VoltageEvents);
                    list.Add(CurrentEvents);
                    list.Add(PowerEvents);
                    list.Add(TransactionsEvents);
                    list.Add(NonRollOverEvents);
                    list.Add(OtherEvents);
                    list.Add(ControlEvents);
                    list.Add(EventsScaler);
                    break;
                case DownloadOption.LSData:
                    list.Add(BlockLoad);
                    list.Add(BlockLoadScaler);
                    break;
                case DownloadOption.DEData:
                    list.Add(DailyLoad);
                    list.Add(DailyLoadScaler);
                    break;
                case DownloadOption.Instantaneous:
                    list.Add(Instantaneous);
                    list.Add(InstantaneousScaler);
                    break;
                case DownloadOption.Billing:
                    list.Add(Billing);
                    list.Add(BillingScaler);
                    break;
                case DownloadOption.Nameplate:
                    list.Add(Nameplate);
                    break;
                case DownloadOption.VoltageEvents:
                    list.Add(VoltageEvents);
                    list.Add(EventsScaler);
                    break;
                case DownloadOption.CurrentEvents:
                    list.Add(CurrentEvents);
                    list.Add(EventsScaler);
                    break;
                case DownloadOption.PowerEvents:
                    list.Add(PowerEvents);
                    list.Add(EventsScaler);
                    break;
                case DownloadOption.TransactionsEvents:
                    list.Add(TransactionsEvents);
                    list.Add(EventsScaler);
                    break;
                case DownloadOption.NonRollOverEvents:
                    list.Add(NonRollOverEvents);
                    list.Add(EventsScaler);
                    break;
                case DownloadOption.OtherEvents:
                    list.Add(OtherEvents);
                    list.Add(EventsScaler);
                    break;
                case DownloadOption.ControlEvents:
                    list.Add(ControlEvents);
                    list.Add(EventsScaler);
                    break;
            }
            return list;
        }
        public DataTable GetParameterTableHorizontal(string LoadData, string profileObis)
        {
            // Create a DataTable
            DataTable dataTable = new DataTable();
            try
            {
                string[] obisArray;
                int count = 0;
                if (LoadData.Substring(2, 2) == "81")
                {
                    count = int.Parse(LoadData.Substring(4, 2), NumberStyles.HexNumber);
                    obisArray = Regex.Split(LoadData.ToString(), LoadData.Substring(6, 4));
                }
                else if (LoadData.Substring(2, 2) == "82")
                {
                    count = int.Parse(LoadData.Substring(4, 4), NumberStyles.HexNumber);
                    obisArray = Regex.Split(LoadData.ToString(), LoadData.Substring(8, 4));
                }
                else
                {
                    count = int.Parse(LoadData.Substring(2, 2), NumberStyles.HexNumber);
                    obisArray = Regex.Split(LoadData.ToString(), LoadData.Substring(4, 4));
                }
                string[] classValue = new string[count];
                string[] obisValue = new string[count];
                string[] attributeValue = new string[count];
                string[] namevalue = new string[count];
                string[] scalerValue = new string[count];
                dataTable.Columns.Add("SN", typeof(string));
                dataTable.Columns.Add("Parameter Name", typeof(string));
                dataTable.Columns.Add("Class", typeof(string));
                dataTable.Columns.Add("Obis", typeof(string));
                dataTable.Columns.Add("Attribute", typeof(string));
                dataTable.Columns.Add("Scaler", typeof(string));
                if (count == (obisArray.Length - 1))
                {
                    for (int i = 1; i < obisArray.Length; i++)
                    {
                        DataRow row = dataTable.NewRow();
                        int j = i - 1;
                        obisArray[i] = obisArray[i].PadRight(32, '0');
                        classValue[j] = (int.Parse(obisArray[i].Substring(2, 4), NumberStyles.HexNumber)).ToString();
                        row["Class"] = classValue[j];
                        obisValue[j] = DLMSParser.GetObis(obisArray[i].Substring(10, 12));
                        row["Obis"] = obisValue[j];
                        attributeValue[j] = (int.Parse(obisArray[i].Substring(24, 2), NumberStyles.HexNumber)).ToString();
                        row["Attribute"] = attributeValue[j];
                        namevalue[j] = DLMSParser.GetObisName(classValue[j], obisValue[j], attributeValue[j]);
                        row["Parameter Name"] = namevalue[j];
                        row["Scaler"] = "";
                        row["SN"] = i;
                        dataTable.Rows.Add(row);
                    }
                }
                else
                {
                    log.Error("Count Mismatch in getting object list");
                }
            }
            catch
            {
                return dataTable;
            }
            return dataTable;
        }
        public string[] GetScalerProfileObisListParsing(string LoadData)
        {
            string[] obisDataArray;
            string[] splitedData = LoadData.Split(' ');
            if (splitedData.Length < 3)
            {
                if (LoadData == "0100" || LoadData == "01820000" || LoadData.Length < 4)
                {
                    log.Info($"Data not present for {LoadData.Substring(0, 20)}");
                    //MessageBox.Show($"Data not present for {LoadData.Substring(0, 20)}", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return null;
                }
                if (LoadData.Trim().Substring(2, 2) == "82" && LoadData.Trim().Substring(8, 2) == "02")
                {
                    obisDataArray = Regex.Split(LoadData.ToString(), LoadData.Substring(8, 4));
                }
                else
                    obisDataArray = Regex.Split(LoadData.ToString(), LoadData.Substring(4, 4));
            }
            else
            {
                if (splitedData[3] == "0100" || splitedData[3] == "01820000" || splitedData[3].Length < 4)
                {
                    log.Info($"Data not present for {LoadData.Substring(0, 20)}");
                    //MessageBox.Show($"Data not present for {LoadData.Substring(0, 20)}", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return null;
                }
                if (splitedData[3].Trim().Substring(2, 2) == "82" && splitedData[3].Trim().Substring(8, 2) == "02")
                {
                    obisDataArray = Regex.Split(splitedData[3].ToString(), splitedData[3].Substring(8, 4));
                }
                else
                    obisDataArray = Regex.Split(splitedData[3].ToString(), splitedData[3].Substring(4, 4));

            }
            string[] obisArray = new string[obisDataArray.Length - 1];
            int count = 0;
            if (obisDataArray[0].Substring(2, 2) == "82" && obisDataArray[0].Length == 8)
            {
                count = int.Parse(obisDataArray[0].Substring(4, 4), NumberStyles.HexNumber);
            }
            else if (obisDataArray[0].Substring(2, 2) == "81" && obisDataArray[0].Length >= 6)
            {
                count = int.Parse(obisDataArray[0].Substring(4, 2), NumberStyles.HexNumber);
            }
            else
                count = int.Parse(obisDataArray[0].Substring(2, 2), NumberStyles.HexNumber);
            if (count == (obisDataArray.Length - 1))
            {
                for (int i = 1; i < obisDataArray.Length; i++)
                {
                    obisArray[i - 1] = DLMSParser.GetObis(obisDataArray[i].PadRight(32, '0').Substring(10, 12));
                }
            }
            else
            {
                //MessageBox.Show("Count Mismatch in getting object list");
                log.Error("Count Mismatch in getting object list");
            }
            return obisArray;
        }
        public string[] GetScalerProfileScalerListParsing(string LoadData)
        {
            string[] ScalerArray = null;
            int count;
            string[] splitedData = LoadData.Split(' ');
            if (splitedData.Length < 3)
            {
                if (LoadData.StartsWith("01"))
                {
                    if (LoadData == "0100" || LoadData == "01820000" || LoadData.Length < 4)
                    {
                        log.Info($"Data not present for {LoadData.Substring(0, 20)}");
                        //MessageBox.Show($"Data not present for {LoadData.Substring(0, 20)}", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return ScalerArray;
                    }
                    else if (LoadData.Substring(6, 2) == "82")
                    {
                        ScalerArray = Regex.Split(LoadData, LoadData.Substring(12, 6));
                        count = int.Parse(ScalerArray[0].Substring(8, 4), NumberStyles.HexNumber);
                    }
                    else if (LoadData.Substring(6, 2) == "81")
                    {
                        ScalerArray = Regex.Split(LoadData, LoadData.Substring(10, 6));
                        count = int.Parse(ScalerArray[0].Substring(8, 2), NumberStyles.HexNumber);
                    }
                    else
                    {
                        ScalerArray = Regex.Split(LoadData, LoadData.Substring(8, 6));
                        count = int.Parse(ScalerArray[0].Substring(6, 2), NumberStyles.HexNumber);
                    }
                }
                else
                {
                    log.Info($"Data not present for {LoadData}");
                    //MessageBox.Show($"Data not present for {LoadData.Substring(0, 20)}", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return ScalerArray;
                }

            }
            else
            {
                LoadData = splitedData[3].Trim();
                if (LoadData.Substring(6, 2) == "82")
                {
                    ScalerArray = Regex.Split(LoadData, LoadData.Substring(12, 6));
                    count = int.Parse(ScalerArray[0].Substring(8, 4), NumberStyles.HexNumber);
                }
                else if (LoadData.Substring(6, 2) == "81")
                {
                    ScalerArray = Regex.Split(LoadData, LoadData.Substring(10, 6));
                    count = int.Parse(ScalerArray[0].Substring(8, 2), NumberStyles.HexNumber);
                }
                else
                {
                    ScalerArray = Regex.Split(LoadData, LoadData.Substring(8, 6));
                    count = int.Parse(ScalerArray[0].Substring(6, 2), NumberStyles.HexNumber);
                }
            }
            ScalerArray = ScalerArray.Skip(1).ToArray();
            for (int i = 0; i < ScalerArray.Length; i++)
            {
                ScalerArray[i] = $"0F{ScalerArray[i]}";
            }
            /*
            string[] ScalerArray = null;
            //string[] splitedData = LoadData.Split(' ');
            if (LoadData == "0100" || LoadData == "01820000" || LoadData.Length < 4)
            {
                log.Info($"Data not present for {LoadData.Substring(0, 20)}");
                //MessageBox.Show($"Data not present for {LoadData.Substring(0, 20)}", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return ScalerArray;
            }
            //   LoadData = LoadData.Split(' ')[3];
            int i = 0, nCount = 0;
            int j = -1, nLength = 4, k = 0;
            string tempData = String.Empty;
            string strMatch = string.Empty;

            if (LoadData.Substring(2, 2) == "82")
            {
                i = 8;
                strMatch = LoadData.Substring(8, 4);
                nCount = Convert.ToInt32(LoadData.Substring(4, 4), 16);
            }
            else if (LoadData.Substring(2, 2) == "81")
            {
                i = 6;
                strMatch = LoadData.Substring(6, 4);
                nCount = Convert.ToInt32(LoadData.Substring(4, 2), 16);
            }
            else if (LoadData.Substring(0, 8) == "00000001")
            {
                i = 24;
                strMatch = LoadData.Substring(24, 4);
            }
            else
            {
                i = 4;
                strMatch = LoadData.Substring(8, 4);
                if (strMatch.Substring(2, 2) == "81")
                {
                    strMatch = LoadData.Substring(4, 6);
                    nLength = 6;
                }
                else if (strMatch.Substring(2, 2) == "82")
                {
                    strMatch = LoadData.Substring(4, 8);
                    nLength = 8;
                }
                if (strMatch.Length == 8 && strMatch.Substring(2, 2) == "82")
                    nCount = Convert.ToInt32(strMatch.Substring(4, 4), 16);
                else
                    nCount = Convert.ToInt32(LoadData.Substring(2, 2), 16);
                ScalerArray = new string[Convert.ToInt32(LoadData.Substring(6, 2), 16)];
            }
            j = 0;
            for (; i < LoadData.Length; i++)
            {
                if (LoadData.Substring(i, nLength) == strMatch)
                {
                    i += nLength;
                    while (true)
                    {
                        if (i >= LoadData.Length)
                            break;
                        if (LoadData.Substring(i, 2) == "0F")
                        {
                            tempData = LoadData.Substring(i, 8);
                            ScalerArray[k] = tempData;
                            i += 8;
                            k++;
                        }
                        else
                        {
                            i++;
                        }
                        if (LoadData.Length > (i + nLength) && LoadData.Substring(i, nLength) == strMatch)//LoadData.Substring(4, 4))
                        {
                            i--;
                            break;
                        }
                    }
                    j++;
                }

            }
            */
            return ScalerArray;
        }
        public DataTable GetValuesDataTableParsing(string LoadData, string profileObis, DataTable obisDataTable)
        {
            // Create a DataTable
            string objectName = profileObis;
            DataTable dataTable = new DataTable();
            // Merge DataTables
            DataTable mergedDataTable = new DataTable();
            // Get the index of the "Scaler" column
            int scalerColumnIndex = obisDataTable.Columns.IndexOf("Scaler");
            // Iterate through each row of the DataTable
            //string[] scalerMultiplyFactorArray=new string[dataTable.Rows.Count];
            string[] scalerDataArray = obisDataTable.AsEnumerable()
                                         .Select(row => row.Field<string>(scalerColumnIndex)) // Change the type to match the third column's type
                                         .ToArray();
            string[] scalerMultiplyFactor = new string[scalerDataArray.Length];
            for (int n = 0; n < scalerDataArray.Length; n++)
            {
                if (!string.IsNullOrEmpty(scalerDataArray[n]))
                {
                    scalerMultiplyFactor[n] = (string)parse.ScalerhshTable[scalerDataArray[n].Substring(2, 2)];
                }
                else
                    scalerMultiplyFactor[n] = "";

            }
            int i = 0, nCount = 0;
            int j = -1, nLength = 4, k = 0;
            string tempData = String.Empty;
            string strMatch = string.Empty;
            int parameterCount = 0;
            int entriesCount = 0;
            if (LoadData == "0100" || LoadData == "01820000" || LoadData.Length < 4)
            {
                log.Info($"Data not present for {objectName}");
                //MessageBox.Show($"Data not present for {objectName}", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return obisDataTable;
            }
            if (LoadData.Substring(2, 2) == "82")
            {
                i = 8;
                strMatch = LoadData.Substring(8, 4);
                nCount = Convert.ToInt32(LoadData.Substring(4, 4), 16);
                parameterCount = Convert.ToInt32(strMatch.Substring(2, 2), 16);
            }
            else if (LoadData.Substring(2, 2) == "81")
            {
                i = 6;
                strMatch = LoadData.Substring(6, 4);
                nCount = Convert.ToInt32(LoadData.Substring(4, 2), 16);
            }
            else if (LoadData.Substring(0, 8) == "00000001")
            {
                i = 24;
                strMatch = LoadData.Substring(24, 4);
            }
            else
            {
                i = 4;
                strMatch = LoadData.Substring(4, 4);
                if (strMatch.Substring(2, 2) == "81")
                {
                    strMatch = LoadData.Substring(4, 6);
                    nLength = 6;
                    parameterCount = Convert.ToInt32(LoadData.Substring(8, 2), 16);
                    nCount = Convert.ToInt32(LoadData.Substring(2, 2), 16);
                }
                else if (strMatch.Substring(2, 2) == "82")
                {
                    strMatch = LoadData.Substring(4, 8);
                    nLength = 8;
                    nCount = Convert.ToInt32(LoadData.Substring(2, 2), 16);
                    //parameterCount = Convert.ToInt32(strMatch.Substring(2, 2), 16);//old
                    parameterCount = Convert.ToInt32(strMatch.Substring(4, 4), 16);
                }
                else
                {
                    nCount = Convert.ToInt32(LoadData.Substring(2, 2), 16);
                    parameterCount = Convert.ToInt32(strMatch.Substring(2, 2), 16);
                }
            }

            j = 0;
            string[] dataValues = new string[parameterCount];
            string[] actualValues = new string[parameterCount];
            string actualDataValue = string.Empty;
            for (; i < LoadData.Length; i++)
            {
                if (LoadData.Substring(i, nLength) == strMatch)
                {
                    DataRow row = dataTable.NewRow();
                    i += nLength;
                    while (true)
                    {
                        if (i >= LoadData.Length)
                            break;
                        if (k >= parameterCount)
                            break;
                        tempData = parse.GetProfileDataString(LoadData, ref i);

                        if (!string.IsNullOrEmpty(tempData))
                        {
                            dataValues[k] = tempData;
                            string scaler = obisDataTable.Rows[k]["Scaler"].ToString().Trim();

                            if (string.IsNullOrEmpty(scaler))
                            {
                                actualValues[k] = parse.GetProfileValueString(dataValues[k]);
                            }
                            else
                            {
                                if (string.IsNullOrEmpty((string)parse.UnithshTable[scaler.Trim().Substring(6, 2)]) && (string)parse.UnithshTable[scaler.Trim().Substring(6, 2)] == "1")
                                {
                                    actualDataValue = parse.GetProfileValueString(dataValues[k]);
                                    actualValues[k] = actualDataValue;
                                }
                                else
                                {
                                    try
                                    {
                                        actualDataValue = parse.GetProfileValueString(dataValues[k]);
                                        double scaledValue = Convert.ToDouble((string)parse.ScalerhshTable[scaler.Trim().Substring(2, 2)]) * Convert.ToDouble(actualDataValue);
                                        actualValues[k] = scaledValue.ToString();
                                    }
                                    catch (Exception ex)
                                    {
                                        actualValues[k] = actualDataValue;
                                    }

                                }
                            }
                            k++;
                        }

                    }
                    parse.JoinDataAndValuesArrayAsColumn(obisDataTable, $"Entry {j + 1} Data", $"Entry {j + 1} Value", dataValues, actualValues);
                    j++;
                    i--;
                    k = 0;
                }
            }
            return obisDataTable;
        }
        public bool GetObjectsDataTable(string LoadData, ref DataTable dataTable)
        {
            bool IsConvertedSuccessful = true;
            //string[] splitedData = LoadData.Split(' ');
            string[] obisArray = null;
            int count = 0;
            if (LoadData == "0100" || LoadData == "01820000" || LoadData.Length < 4)
            {
                return false;
            }
            else if (LoadData.Substring(2, 2) == "81")
            {
                count = int.Parse(LoadData.Substring(4, 2), NumberStyles.HexNumber);
                obisArray = Regex.Split(LoadData.ToString(), LoadData.Substring(6, 4));
            }
            else if (LoadData.Substring(2, 2) == "82")
            {
                count = int.Parse(LoadData.Substring(4, 4), NumberStyles.HexNumber);
                obisArray = Regex.Split(LoadData.ToString(), LoadData.Substring(8, 4));
            }
            else
            {
                count = int.Parse(LoadData.Substring(2, 2), NumberStyles.HexNumber);
                obisArray = Regex.Split(LoadData.ToString(), LoadData.Substring(4, 4));
            }
            string[] classValue = new string[count];
            string[] obisValue = new string[count];
            string[] attributeValue = new string[count];
            string[] namevalue = new string[count];
            string[] scalerValue = new string[count];
            if (count == (obisArray.Length - 1))
            {
                for (int i = 1; i < obisArray.Length; i++)
                {
                    int j = i - 1;
                    classValue[j] = (int.Parse(obisArray[i].Substring(2, 4), NumberStyles.HexNumber)).ToString();
                    obisValue[j] = DLMSParser.GetObis(obisArray[i].PadRight(32, '0').Substring(10, 12));
                    attributeValue[j] = (int.Parse(obisArray[i].Substring(24, 2), NumberStyles.HexNumber)).ToString();
                    namevalue[j] = DLMSParser.GetObisName(classValue[j], obisValue[j], attributeValue[j]);
                    dataTable.Columns.Add($"{classValue[j]} - {obisValue[j]} - {attributeValue[j]} - {namevalue[j]}", typeof(string));
                }
            }
            else
            {
                IsConvertedSuccessful = false;
            }
            return IsConvertedSuccessful;
        }
        public DataTable GetEventsValuesDataTableParsing(string LoadData, DataTable obisDataTable, string profileOBIS)
        {
            string objectName = parse.GetParameterName(null, profileOBIS, "2");
            // Create a DataTable
            DataTable dataTable = new DataTable();
            try
            {
                foreach (DataColumn column in obisDataTable.Columns)
                {
                    dataTable.Columns.Add(column.ColumnName, typeof(string));

                }
                string name = obisDataTable.Columns[0].ColumnName;
                int obisTableColumnsCount = obisDataTable.Columns.Count;
                if (profileOBIS == "0.0.99.98.0.255" ||
                        profileOBIS == "0.0.99.98.1.255" ||
                         profileOBIS == "0.0.99.98.2.255" ||
                         profileOBIS == "0.0.99.98.3.255" ||
                         profileOBIS == "0.0.99.98.4.255" ||
                        profileOBIS == "0.0.99.98.5.255" ||
                        profileOBIS == "0.0.99.98.6.255" ||
                        profileOBIS == "1.0.98.1.0.255" ||
                        profileOBIS == "1.0.99.1.0.255" ||
                        profileOBIS == "1.0.99.2.0.255" ||
                        profileOBIS == "1.0.94.91.0.255"
                        )
                {
                    int i = 0, nCount = 0;
                    int j = -1, nLength = 4, k = 0;
                    string tempData = String.Empty;
                    string strMatch = string.Empty;
                    if (LoadData == "0100" || LoadData == "01820000" || LoadData.Length < 4)
                    {
                        log.Info($"Data not present for {objectName}");
                        //MessageBox.Show($"Data not present for {objectName}", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return obisDataTable;
                    }
                    if (LoadData.Substring(2, 2) == "82")
                    {
                        i = 8;
                        strMatch = LoadData.Substring(8, 4);
                        nCount = Convert.ToInt32(LoadData.Substring(4, 4), 16);
                    }
                    else if (LoadData.Substring(2, 2) == "81")
                    {
                        i = 6;
                        strMatch = LoadData.Substring(6, 4);
                        nCount = Convert.ToInt32(LoadData.Substring(4, 2), 16);
                    }
                    else if (LoadData.Substring(0, 8) == "00000001")
                    {
                        i = 24;
                        strMatch = LoadData.Substring(24, 4);
                    }
                    else
                    {
                        i = 4;
                        strMatch = LoadData.Substring(4, 4);
                        if (strMatch.Substring(2, 2) == "81")
                        {
                            strMatch = LoadData.Substring(4, 6);
                            nLength = 6;
                        }
                        else if (strMatch.Substring(2, 2) == "82")
                        {
                            strMatch = LoadData.Substring(4, 8);
                            nLength = 8;
                        }
                        nCount = Convert.ToInt32(LoadData.Substring(2, 2), 16);
                    }
                    j = 0;
                    for (; i < LoadData.Length; i++)
                    {
                        if (LoadData.Substring(i, nLength) == strMatch)
                        {
                            DataRow row = dataTable.NewRow();
                            i += nLength;
                            while (true)
                            {
                                if (i >= LoadData.Length)
                                    break;
                                tempData = parse.GateDataFromString(LoadData, ref i, tempData);
                                if (!string.IsNullOrEmpty(tempData))
                                {
                                    if (k == 1)
                                    {
                                        if (profileOBIS.Contains("0.0.99.98"))
                                            row[k] = tempData + " - " + parse.IDToEventName(Convert.ToInt32(tempData));
                                        else
                                            row[k] = tempData;
                                    }
                                    else
                                        row[k] = tempData;
                                    k++;
                                }
                                if (LoadData.Length > (i + nLength) && LoadData.Substring(i, nLength) == strMatch)//LoadData.Substring(4, 4))
                                {
                                    i--;
                                    break;
                                }
                            }
                            dataTable.Rows.Add(row);
                            j++;
                            k = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"There is some error in converting {profileOBIS} buffer data. " + ex.Message.ToString());
                //MessageBox.Show($"There is some error in converting {profile} buffer data", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            return dataTable;
        }
        public DataTable GetNameplateClassObisAttTable(string LoadData, string profileObis)
        {
            // Create a DataTable
            DataTable dataTable = new DataTable();
            try
            {
                string[] obisArray;
                int count = 0;
                if (LoadData.Substring(2, 2) == "81")
                {
                    count = int.Parse(LoadData.Substring(4, 2), NumberStyles.HexNumber);
                    obisArray = Regex.Split(LoadData.ToString(), LoadData.Substring(6, 4));
                }
                else if (LoadData.Substring(2, 2) == "82")
                {
                    count = int.Parse(LoadData.Substring(4, 4), NumberStyles.HexNumber);
                    obisArray = Regex.Split(LoadData.ToString(), LoadData.Substring(8, 4));
                }
                else
                {
                    count = int.Parse(LoadData.Substring(2, 2), NumberStyles.HexNumber);
                    obisArray = Regex.Split(LoadData.ToString(), LoadData.Substring(4, 4));
                }
                string[] classValue = new string[count];
                string[] obisValue = new string[count];
                string[] attributeValue = new string[count];
                string[] namevalue = new string[count];
                dataTable.Columns.Add("SN", typeof(string));
                dataTable.Columns.Add("Parameter Name", typeof(string));
                dataTable.Columns.Add("Class", typeof(string));
                dataTable.Columns.Add("Obis", typeof(string));
                dataTable.Columns.Add("Attribute", typeof(string));
                dataTable.Columns.Add("Data", typeof(string));
                dataTable.Columns.Add("Value", typeof(string));
                if (count == (obisArray.Length - 1))
                {
                    for (int i = 1; i < obisArray.Length; i++)
                    {
                        DataRow row = dataTable.NewRow();
                        int j = i - 1;
                        obisArray[i] = obisArray[i].PadRight(32, '0');
                        classValue[j] = (int.Parse(obisArray[i].Substring(2, 4), NumberStyles.HexNumber)).ToString();
                        row["Class"] = classValue[j];
                        obisValue[j] = DLMSParser.GetObis(obisArray[i].Substring(10, 12));
                        row["Obis"] = obisValue[j];
                        attributeValue[j] = (int.Parse(obisArray[i].Substring(24, 2), NumberStyles.HexNumber)).ToString();
                        row["Attribute"] = attributeValue[j];
                        namevalue[j] = DLMSParser.GetObisName(classValue[j], obisValue[j], attributeValue[j]);
                        row["Parameter Name"] = namevalue[j];
                        row["SN"] = i;
                        row["Data"] = "";
                        row["Value"] = "";
                        dataTable.Rows.Add(row);
                    }
                }
                else
                {
                    log.Error("Count Mismatch in getting object list");
                }
            }
            catch
            {
                return dataTable;
            }
            return dataTable;
        }
        public DataTable GetNameplateValuesDataTable(string LoadData, DataTable obisDataTable)
        {
            // Create a DataTable
            string objectName = "Nameplate Profile";
            DataTable dataTable = new DataTable();
            // Merge DataTables
            DataTable mergedDataTable = new DataTable();
            // Get the index of the "Data" column
            int dataColumnIndex = obisDataTable.Columns.IndexOf("Data");
            int valueColumnIndex = obisDataTable.Columns.IndexOf("Value");


            int i = 0, nCount = 0;
            int j = -1, nLength = 4, k = 0;
            string tempData = String.Empty;
            string strMatch = string.Empty;
            int parameterCount = 0;
            int entriesCount = 0;
            if (LoadData == "0100" || LoadData == "01820000" || LoadData.Length < 4)
            {
                log.Info($"Data not present for {objectName}");
                //MessageBox.Show($"Data not present for {objectName}", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return obisDataTable;
            }
            if (LoadData.Substring(2, 2) == "82")
            {
                i = 8;
                strMatch = LoadData.Substring(8, 4);
                nCount = Convert.ToInt32(LoadData.Substring(4, 4), 16);
                parameterCount = Convert.ToInt32(strMatch.Substring(2, 2), 16);
            }
            else if (LoadData.Substring(2, 2) == "81")
            {
                i = 6;
                strMatch = LoadData.Substring(6, 4);
                nCount = Convert.ToInt32(LoadData.Substring(4, 2), 16);
            }
            else if (LoadData.Substring(0, 8) == "00000001")
            {
                i = 24;
                strMatch = LoadData.Substring(24, 4);
            }
            else
            {
                i = 4;
                strMatch = LoadData.Substring(4, 4);
                if (strMatch.Substring(2, 2) == "81")
                {
                    strMatch = LoadData.Substring(4, 6);
                    nLength = 6;
                    parameterCount = Convert.ToInt32(LoadData.Substring(8, 2), 16);
                    nCount = Convert.ToInt32(LoadData.Substring(2, 2), 16);
                }
                else if (strMatch.Substring(2, 2) == "82")
                {
                    strMatch = LoadData.Substring(4, 8);
                    nLength = 8;
                    nCount = Convert.ToInt32(LoadData.Substring(2, 2), 16);
                    //parameterCount = Convert.ToInt32(strMatch.Substring(2, 2), 16);//old
                    parameterCount = Convert.ToInt32(strMatch.Substring(4, 4), 16);
                }
                else
                {
                    nCount = Convert.ToInt32(LoadData.Substring(2, 2), 16);
                    parameterCount = Convert.ToInt32(strMatch.Substring(2, 2), 16);
                }
            }

            j = 0;
            string[] dataValues = new string[parameterCount];
            string[] actualValues = new string[parameterCount];
            string actualDataValue = string.Empty;
            for (; i < LoadData.Length; i++)
            {
                if (LoadData.Substring(i, nLength) == strMatch)
                {
                    i += nLength;
                    while (true)
                    {
                        if (i >= LoadData.Length)
                            break;
                        if (k >= parameterCount)
                            break;
                        tempData = parse.GetProfileDataString(LoadData, ref i);

                        if (!string.IsNullOrEmpty(tempData))
                        {
                            dataValues[k] = tempData;
                            try
                            {
                                actualValues[k] = parse.GetProfileValueString(dataValues[k]);
                            }
                            catch (Exception ex)
                            {
                                actualValues[k] = dataValues[k];
                            }
                            k++;
                        }

                    }

                    //JoinDataAndValuesArrayAsColumn(obisDataTable, $"Entry {j + 1} Data", $"Entry {j + 1} Value", dataValues, actualValues);
                    j++;
                    i--;
                    k = 0;
                }
                try
                {
                    for (int rowIndex = 0; rowIndex < obisDataTable.Rows.Count; rowIndex++)
                    {
                        obisDataTable.Rows[rowIndex][dataColumnIndex] = dataValues[rowIndex];
                        obisDataTable.Rows[rowIndex][valueColumnIndex] = actualValues[rowIndex];
                    }
                    obisDataTable.AcceptChanges();
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message.ToString());
                }

            }

            return obisDataTable;

        }
        public void JoinDataAndValuesArrayAsColumn(DataTable dataTable, string dataColumnName, string valueColumnName, string[] dataArray, string[] valueArray)
        {
            // Create a new DataColumn with the specified column name
            DataColumn newDataColumn = new DataColumn(dataColumnName, typeof(string));
            DataColumn newValueColumn = new DataColumn(valueColumnName, typeof(string));
            dataTable.Columns.Add(newDataColumn);
            dataTable.Columns.Add(newValueColumn);

            // Populate the new column with values from the array
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                if (i < dataArray.Length)
                {
                    dataTable.Rows[i][dataColumnName] = dataArray[i];
                    dataTable.Rows[i][valueColumnName] = valueArray[i];
                }
                else
                {
                    // Handle the case where the array is smaller than the number of rows in the DataTable
                    dataTable.Rows[i][dataColumnName] = DBNull.Value;
                    dataTable.Rows[i][valueColumnName] = DBNull.Value;
                }
            }
        }

        [Obsolete("GetObjectList is deprecated, please use DLMSAssociation.GetObjectListTable instead.")]
        public DataTable GetObjectList(string meterDataRead)
        {
            DLMSParser parse = new DLMSParser();
            DataTable objectsDataTable = new DataTable();
            try
            {
                objectsDataTable.Columns.Add("SN", typeof(string));
                objectsDataTable.Columns.Add("Class", typeof(string));
                objectsDataTable.Columns.Add("Version", typeof(string));
                objectsDataTable.Columns.Add("OBIS", typeof(string));
                objectsDataTable.Columns.Add("Parameter Name", typeof(string));
                objectsDataTable.Columns.Add("Access", typeof(string));
                string[] obiscode = (string[])null;
                int obiscnt = 0;

                if (meterDataRead != "0100")
                    ObjectListGetObisCode(meterDataRead.Substring(4, meterDataRead.Length - 4), out obiscode, out obiscnt);
                int num1 = !(meterDataRead.Substring(2, 2) == "82") ? (!(meterDataRead.Substring(2, 2) == "81") ? int.Parse(meterDataRead.Substring(2, 2), NumberStyles.HexNumber) : int.Parse(meterDataRead.Substring(4, 2), NumberStyles.HexNumber)) : int.Parse(meterDataRead.Substring(6, 4), NumberStyles.HexNumber);
                if (obiscnt != num1)
                {
                    int num2 = (int)MessageBox.Show("OBIS Code Count Mismatch in Object List.", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                for (int index = 1; index <= obiscnt; ++index)
                {
                    string tempClass = Int32.Parse(obiscode[index].Substring(0, 4), NumberStyles.HexNumber).ToString();
                    string tempObis = Int32.Parse(obiscode[index].Substring(8, 2), NumberStyles.HexNumber).ToString() + "." +
                        Int32.Parse(obiscode[index].Substring(10, 2), NumberStyles.HexNumber).ToString() + "." +
                        Int32.Parse(obiscode[index].Substring(12, 2), NumberStyles.HexNumber).ToString() + "." +
                        Int32.Parse(obiscode[index].Substring(14, 2), NumberStyles.HexNumber).ToString() + "." +
                        Int32.Parse(obiscode[index].Substring(16, 2), NumberStyles.HexNumber).ToString() + "." +
                        Int32.Parse(obiscode[index].Substring(18, 2), NumberStyles.HexNumber).ToString();
                    objectsDataTable.Rows.Add((object)index.ToString(),//
                        tempClass,
                         Int32.Parse(obiscode[index].Substring(5, 2), NumberStyles.HexNumber).ToString(),
                        tempObis,
                        $"{DLMSParser.GetObisName(tempClass, tempObis)}",
                        (object)obiscode[index].Substring(21, obiscode[index].Length - 21));
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message.ToString());
            }
            return objectsDataTable;
        }
        public void ObjectListGetObisCode(string objectlist, out string[] obiscode, out int obiscnt)
        {
            try
            {
                string[] strArray1 = Regex.Split(objectlist, "02041200");
                obiscode = new string[strArray1.Length];
                obiscnt = strArray1.Length - 1;
                for (int index1 = 1; index1 < strArray1.Length; ++index1)
                {
                    obiscode[index1] = "00" + strArray1[index1].Substring(0, 2) + " " + strArray1[index1].Substring(4, 2) + " " + strArray1[index1].Substring(10, 12) + " " + strArray1[index1].Substring(28, 2);
                    string[] strArray2 = Regex.Split(strArray1[index1].Substring(30, strArray1[index1].Length - 30), "0203");
                    for (int index2 = 1; index2 < strArray2.Length && strArray2[index2].Length > 5; ++index2)
                    {
                        ref string local = ref obiscode[index1];
                        local = local + " " + $"{int.Parse(strArray2[index2].Substring(2, 2), NumberStyles.HexNumber)}-" + parse.ObjhshTable[(object)strArray2[index2].Substring(6, 2)]?.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                obiscode = (string[])null;
                obiscnt = 0;
                int num = (int)MessageBox.Show(ex.Message);
            }
        }

        public int GetTotalEntriesCount(string LoadData)
        {
            int i = 0, nCount = 0;
            int j = -1, nLength = 4; ;
            try
            {
                string tempData = String.Empty;
                string strMatch = string.Empty;
                if (LoadData == "0100" || LoadData == "01820000" || LoadData.Length < 4)
                {
                    return 0;
                }
                if (LoadData.Substring(2, 2) == "82")
                {
                    i = 8;
                    strMatch = LoadData.Substring(8, 4);
                    nCount = Convert.ToInt32(LoadData.Substring(4, 4), 10);
                }
                else if (LoadData.Substring(2, 2) == "81")
                {
                    i = 6;
                    strMatch = LoadData.Substring(6, 4);
                    nCount = Convert.ToInt32(LoadData.Substring(4, 2), 10);
                }
                else if (LoadData.Substring(0, 8) == "00000001")
                {
                    i = 24;
                    strMatch = LoadData.Substring(24, 4);
                    nCount = Convert.ToInt32(LoadData.Substring(2, 2), 10);
                }
                else
                {
                    i = 4;
                    strMatch = LoadData.Substring(4, 4);
                    if (strMatch.Substring(2, 2) == "81")
                    {
                        strMatch = LoadData.Substring(4, 6);
                        nLength = 6;
                    }
                    else if (strMatch.Substring(2, 2) == "82")
                    {
                        strMatch = LoadData.Substring(4, 8);
                        nLength = 8;
                    }
                    nCount = Convert.ToInt32(LoadData.Substring(2, 2), 10);

                }
                return nCount;
            }
            catch (Exception e)
            {
                log.Error(e.Message.ToString());
                log.Error(e.StackTrace.ToString());
                return nCount;
            }
        }

        public static int GetNetworkMode(string LoadData)
        {
            if (parse is null)
                parse = new DLMSParser();
            int nValue = 0;
            if (LoadData.Length < 4 || LoadData.Substring(2, 2) == "00")
            {
                nValue = 100;
            }
            List<string> items = parse.GetStructureValueList(LoadData);
            if (items.Count == 5)
            {
                nValue = Convert.ToInt32(parse.GetProfileValueString(items[3]));
            }
            else
                nValue = 100;
            return nValue;
        }

        public Dictionary<string, string> GetProfileObisPair()
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            keyValuePairs.Add(Nameplate, "");
            keyValuePairs.Add(Instantaneous, InstantaneousScaler);
            keyValuePairs.Add(Billing, BillingScaler);
            keyValuePairs.Add(DailyLoad, DailyLoadScaler);
            keyValuePairs.Add(BlockLoad, BlockLoadScaler);
            keyValuePairs.Add(VoltageEvents, EventsScaler);
            keyValuePairs.Add(CurrentEvents, EventsScaler);
            keyValuePairs.Add(PowerEvents, EventsScaler);
            keyValuePairs.Add(TransactionsEvents, EventsScaler);
            keyValuePairs.Add(OtherEvents, EventsScaler);
            keyValuePairs.Add(NonRollOverEvents, EventsScaler);
            keyValuePairs.Add(ControlEvents, EventsScaler);
            return keyValuePairs;
        }
        public Dictionary<string, DataTable> GetProfilesDataTable(Dictionary<string, object> referenceDictionary)
        {
            string recivedObisString = "", recivedValueString = "", recivedScalerObisString = "", recivedScalerValueString = "", inUseEntries = "", profileEntries = "", option = "";
            Dictionary<string, DataTable> profilesTableList = new Dictionary<string, DataTable>();
            Dictionary<string, string> keyValuePairsProfile = GetProfileObisPair();
            foreach (var kvp in keyValuePairsProfile)
            {
                string profileKey = kvp.Key.Trim();
                string profileScalerValue = kvp.Value.Trim();
                // Reset for each profile
                recivedObisString = "";
                recivedValueString = "";
                recivedScalerObisString = "";
                recivedScalerValueString = "";
                string profileName = "";
                // Find matching entries in referenceDictionary
                //Dictionary<string, object> matches = new Dictionary<string, object>();
                //foreach (var entry in referenceDictionary)
                //{
                //    string[] parts = entry.Key.Split('|');
                //    if (parts.Length < 3)
                //        continue;
                //    if (parts[0].Contains(profileKey) || parts[0].Contains(profileScalerValue))
                //    {
                //        matches.Add(entry.Key, entry.Value);
                //    }
                //}
                // Find matching entries in referenceDictionary
                var matches = referenceDictionary
                    .Where(r => r.Key.Contains(profileKey) || r.Key.Contains(profileScalerValue));

                if (matches.Count() < 2)
                    continue;
                foreach (var match in matches)
                {
                    if (match.Key.ToString().Split('|')[0].Contains(profileKey) && match.Key.ToString().Split('|')[1].Contains("3"))
                    {
                        recivedObisString = match.Value.ToString().Trim();
                        profileName = match.Key.ToString().Split('|')[2].Split('-')[0];
                    }
                    else if (match.Key.ToString().Split('|')[0].Contains(profileKey) && match.Key.ToString().Split('|')[1].Contains("2"))
                    {
                        recivedValueString = match.Value.ToString().Trim();
                    }
                    else if (match.Key.ToString().Split('|')[0].Contains(profileScalerValue) && match.Key.ToString().Split('|')[1].Contains("3"))
                    {
                        recivedScalerObisString = match.Value.ToString().Trim();
                    }
                    else if (match.Key.ToString().Split('|')[0].Contains(profileScalerValue) && match.Key.ToString().Split('|')[1].Contains("2"))
                    {
                        recivedScalerValueString = match.Value.ToString().Trim();
                    }
                }
                var tabledata = GetProfileDataTable(profileKey, recivedObisString, recivedValueString, recivedScalerObisString, recivedScalerValueString);
                profilesTableList.Add(profileName, ((DataTable)tabledata["DataTable"]).Copy());
            }
            return profilesTableList;
        }
        public Dictionary<string, object> GetProfileDataTable(string profileObis, string _recivedObisString, string _recivedValueString, string _recivedScalerObisString, string _recivedScalerValueString)
        {
            DLMSParser DLMSparse = new DLMSParser();
            string recivedObisString = "", recivedValueString = "", recivedScalerObisString = "", recivedScalerValueString = "", inUseEntries = "", profileEntries = "", option = "";
            DataTable obisDataTable = new DataTable();
            DataTable resultDataTable = new DataTable();
            string[] scalerObisArray = null, scalerScalerDataArray = null, mainSourceObisArray = null, finalScalerValuesToFill = null, ScalerMultiFactorArray = null;
            try
            {

                //Get Profile Objects
                recivedObisString = _recivedObisString;
                //Get Profile Values
                recivedValueString = _recivedValueString;
                //Get Scaler Objects
                recivedScalerObisString = _recivedScalerObisString;
                //Get Scaler Values
                recivedScalerValueString = _recivedScalerValueString;

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
                if (!string.IsNullOrEmpty(recivedScalerObisString) && !string.IsNullOrEmpty(recivedScalerValueString))
                {
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
                            ScalerMultiFactorArray[index] = (string)DLMSparse.ScalerhshTable[scalerScalerDataArray[scalarIndex].Substring(2, 2)];
                            obisDataTable.Columns[index].ColumnName = (!string.IsNullOrEmpty((string)DLMSparse.UnithshTable[scalerScalerDataArray[scalarIndex].Substring(6, 2)])) ?
                                                                    $"{obisDataTable.Columns[index].ColumnName} - {scalerScalerDataArray[scalarIndex]} - ({(string)DLMSparse.UnithshTable[scalerScalerDataArray[scalarIndex].Substring(6, 2)]})" :
                                                                    $"{obisDataTable.Columns[index].ColumnName} - {scalerScalerDataArray[scalarIndex]}";
                        }
                    }
                }
                #endregion
                if (recivedValueString.Length != 4 && recivedValueString != "0100")
                {
                    resultDataTable = parse.GetEventsValuesDataTableParsing(recivedValueString, obisDataTable, profileObis);
                    // Multiply each row value with the corresponding string array value
                    if (!string.IsNullOrEmpty(recivedScalerObisString) && !string.IsNullOrEmpty(recivedScalerValueString))
                    {
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
                    }
                }
                else
                    resultDataTable = obisDataTable.Copy();
                resultDataTable.AcceptChanges();
                #endregion
            }
            catch (Exception ex)
            {
                log.Error(ex.Message.ToString());
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
        static void MultiplyRowsWithArray(DataTable dataTable, string[] scalerMultiFactorArray)
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

        #region NIC Related Method  
        /// <summary>
        /// Build NIC and Module FOTA Strings. Arg 1 should be true for negative cases 
        /// </summary>
        /// <param name="isNegativeCase"></param>
        /// <param name="NICFOTAReset"></param>
        /// <param name="ModuleFOTAReset"></param>
        /// <returns></returns>
        public List<string> BuildFotaConfigurationString(bool isNegativeCase = false, bool NICFOTAReset = false, bool ModuleFOTAReset = false)
        {
            List<string> nicConfigData = new List<string>();
            byte[] NICFOTA = (byte[])null;
            byte[] ModuleFOTA = (byte[])null;
            try
            {
                if (NICFOTAReset)
                    NICFOTA = Encoding.ASCII.GetBytes("Zero");
                else
                    NICFOTA = GetNICFotaDataBytes(isNegativeCase);


                if (ModuleFOTAReset)
                    ModuleFOTA = Encoding.ASCII.GetBytes("Zero");
                else
                    ModuleFOTA = GetModuleFotaDataBytes(isNegativeCase);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message.ToString());
            }
            try
            {
                nicConfigData = CalculateFotaHexString(NICFOTA, ModuleFOTA);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message.ToString());
            }
            return nicConfigData;
        }
        public byte[] GetNICFotaDataBytes(bool isNegativeCase)
        {
            Dictionary<int, string> nicDetails = new Dictionary<int, string>();
            nicDetails = FOTAHelper.nicFOTAData;
            byte[] NICDataArray = (byte[])null;
            try
            {
                int TransMode = 0;
                if (nicDetails[7] == "Active")
                    TransMode = 0;
                else if (nicDetails[7] == "Passive")
                    TransMode = 1;
                string ftpPort = nicDetails[6].ToString();
                //NICDataArray = CreateFotaByteSequence(fotaInfo.FWFileName, fotaInfo.FTPAddress, ftpPort, fotaInfo.FTPUserName, fotaInfo.FTPPassword, TransMode);
                if (isNegativeCase)
                    NICDataArray = CreateFotaByteSequence(nicDetails[2], nicDetails[4], ftpPort, nicDetails[3], nicDetails[5], TransMode);
                else
                    NICDataArray = CreateFotaByteSequence(nicDetails[1], nicDetails[4], ftpPort, nicDetails[3], nicDetails[5], TransMode);
            }
            catch
            {
            }
            return NICDataArray;
        }
        public byte[] GetModuleFotaDataBytes(bool isNegativeCase)
        {
            Dictionary<int, string> moduleDetails = new Dictionary<int, string>();
            moduleDetails = FOTAHelper.moduleFOTAData;
            byte[] ModuleDataArray = (byte[])null;
            try
            {
                int TransMode = 0;
                if (moduleDetails[7] == "Active")
                    TransMode = 0;
                else if (moduleDetails[7] == "Passive")
                    TransMode = 1;
                string ftpPort = moduleDetails[6].ToString();
                //ModuleDataArray = CreateFotaByteSequence(fotaInfo.FWFileName, fotaInfo.FTPAddress, ftpPort, fotaInfo.FTPUserName, fotaInfo.FTPPassword, TransMode);
                if (isNegativeCase)
                    ModuleDataArray = CreateFotaByteSequence(moduleDetails[2], moduleDetails[4], ftpPort, moduleDetails[3], moduleDetails[5], TransMode);
                else
                    ModuleDataArray = CreateFotaByteSequence(moduleDetails[1], moduleDetails[4], ftpPort, moduleDetails[3], moduleDetails[5], TransMode);
            }
            catch
            {
            }
            return ModuleDataArray;
        }
        public List<string> CalculateFotaHexString(byte[] NICFOTA, byte[] ModuleFOTA)
        {
            List<string> setString = new List<string>();
            int num = 0;
            if (NICFOTA != null)
            {
                byte[] bytes = new byte[1] { Convert.ToByte(NICFOTA.Length) };
                string str6 = !(GXCommon.ToHex(NICFOTA) == "5A 65 72 6F") ? "0202110D09" + GXCommon.ToHex(bytes) + GXCommon.ToHex(NICFOTA) : "0202110D0900";
                //setString += str6;
                str6 = "0101" + str6;
                setString.Add(str6);
                ++num;
            }
            if (ModuleFOTA != null)
            {
                byte[] bytes = new byte[1] { Convert.ToByte(ModuleFOTA.Length) };
                string str7 = !(GXCommon.ToHex(ModuleFOTA) == "5A 65 72 6F") ? "0202110E09" + GXCommon.ToHex(bytes) + GXCommon.ToHex(ModuleFOTA) : "0202110E0900";
                //setString += str7;
                str7 = "0101" + str7;
                setString.Add(str7);
                ++num;
            }
            return setString;
        }
        public byte[] CreateFotaByteSequence(string FileName, string FTPAddress, string Port, string Username, string Password, int TransMode)
        {
            byte[] fotaBytes = (byte[])null;
            try
            {
                string str1 = EncodeStringWithLengthPrefix(FileName).Trim();
                string str2 = EncodeStringWithLengthPrefix(FTPAddress).Trim();
                ushort result;
                ushort.TryParse(Port, out result);
                string str3 = "02" + GXCommon.ToHex(ConvertInt32ToByteArray(result)).Trim();
                string str4 = EncodeStringWithLengthPrefix(Username).Trim();
                string str5 = EncodeStringWithLengthPrefix(Password).Trim();
                string str6 = "01" + GXCommon.ToHex(new byte[1]
                {
          Convert.ToByte(TransMode)
                });
                fotaBytes = GXCommon.HexToBytes("06" + str1 + str2 + str3 + str4 + str5 + str6);
            }
            catch
            {
            }
            return fotaBytes;
        }
        public string EncodeStringWithLengthPrefix(string Data)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(Data);
            return GXCommon.ToHex(new byte[1] { Convert.ToByte(bytes.Length) }) + GXCommon.ToHex(bytes);
        }
        public static byte[] ConvertInt32ToByteArray(ushort I32)
        {
            byte[] bytes = BitConverter.GetBytes(I32);
            Array.Reverse((Array)bytes);
            return bytes;
        }
        /// <summary>
        /// This method is used to convert hex string to ascii text
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public string HexString2Ascii(string hexString)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i <= hexString.Length - 2; i += 2)
            {
                sb.Append(Convert.ToString(Convert.ToChar(Int32.Parse(hexString.Substring(i, 2), System.Globalization.NumberStyles.HexNumber))));
            }
            return sb.ToString();
        }
        #endregion

        public static GXDateTime[] ParseSingleActionScheduleExecutionTimeHex(string hex)
        {
            byte[] data = GXCommon.HexToBytes(hex);
            List<GXDateTime> list = new List<GXDateTime>();

            if (data.Length < 2) // minimal DLMS array = 2 bytes: 01 00
                return list.ToArray();

            int pos = 0;
            byte arrayTag = data[pos++]; // should be 0x01 (array)
            byte count = data[pos++];    // number of elements

            if (count == 0)
                return list.ToArray(); // empty array, return empty GXDateTime[]

            for (int i = 0; i < count; i++)
            {
                if (pos + 9 > data.Length)
                    break; // not enough bytes for a full element

                // Each element: structure tag (skip) + 2 octet strings
                byte structTag = data[pos++]; // structure tag, usually 0x02
                if (structTag != 0x02)
                    throw new Exception("Invalid structure tag in execution_time");

                // Time (4 bytes)
                byte[] timeBytes = new byte[4];
                pos = pos + 3;
                Array.Copy(data, pos, timeBytes, 0, 4);
                pos += 4;

                // Date (5 bytes)
                byte[] dateBytes = new byte[5];
                pos = pos + 2;
                Array.Copy(data, pos, dateBytes, 0, 5);
                pos += 5;

                int year = (dateBytes[0] == 0xFF) ? 65535 : 2000 + dateBytes[0];
                int month = (dateBytes[2] == 0xFF) ? 0 : dateBytes[2];
                int day = (dateBytes[3] == 0xFF) ? 0 : dateBytes[3];

                int hour = (timeBytes[0] == 0xFF) ? -1 : timeBytes[0];
                int minute = (timeBytes[1] == 0xFF) ? -1 : timeBytes[1];
                int second = (timeBytes[2] == 0xFF) ? -1 : timeBytes[2];
                int hundredths = (timeBytes[3] == 0xFF) ? -1 : timeBytes[3];
                GXDateTime dt = new GXDateTime(year, month, day, hour, minute, second, hundredths * 10);
                list.Add(new GXDateTime(dt));
            }

            return list.ToArray();
        }
        /// <summary>
        /// This Converts the user provided Season profile DLMS hex string to array of type GXDLMSSeasonProfile
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public GXDLMSSeasonProfile[] ParseSeasonProfile(string hex)
        {
            byte[] data = GXCommon.HexToBytes(hex);
            List<GXDLMSSeasonProfile> seasons = new List<GXDLMSSeasonProfile>();
            int offset = 0;
            if (data.Length == 0)
                return seasons.ToArray(); // Zero seasons

            if (data[offset++] != 0x01) throw new Exception("Expected array tag");
            int arrayLength = data[offset++]; // Could be 0 for zero seasons
            if (arrayLength == 0)
                return seasons.ToArray(); // Zero seasons
            //offset++;
            if (data[offset++] != 0x02) throw new Exception("Expected array start");

            while (offset < data.Length)
            {
                if (data[offset++] != 0x03) break;

                // Season Name
                if (data[offset++] != 0x09) throw new Exception("Expected Octet String for Season Name");
                int nameLength = data[offset++];
                string seasonName = Encoding.ASCII.GetString(data, offset, nameLength);
                offset += nameLength;
                offset++;
                // Start Time (GXDateTime)
                if (data[offset++] != 0x0C) throw new Exception("Expected DateTime for Start Time");
                byte[] dtBytes = new byte[12];
                Array.Copy(data, offset, dtBytes, 0, 12);

                int year = (dtBytes[0] == 0xFF) ? 65535 : 2000 + dtBytes[0];
                int month = (dtBytes[2] == 0xFF) ? 0 : dtBytes[2];
                int day = (dtBytes[3] == 0xFF) ? 0 : dtBytes[3];
                int hour = (dtBytes[4] == 0xFF) ? -1 : dtBytes[4];
                int minute = (dtBytes[5] == 0xFF) ? -1 : dtBytes[5];
                int second = (dtBytes[6] == 0xFF) ? -1 : dtBytes[6];
                int hundredths = (dtBytes[7] == 0xFF) ? -1 : dtBytes[7];

                GXDateTime startTime = new GXDateTime(year, month, day, hour, minute, second, hundredths * 10);
                offset += 12;

                // Week Name
                if (data[offset++] != 0x09) throw new Exception("Expected Octet String for Week Name");
                int weekLength = data[offset++];
                string weekName = Encoding.ASCII.GetString(data, offset, weekLength);
                offset += weekLength;

                seasons.Add(new GXDLMSSeasonProfile(seasonName, startTime, weekName));
                offset++;
            }

            return seasons.ToArray();
        }
        /// <summary>
        /// This Converts the user provided Week profile DLMS hex string to array of type GXDLMSWeekProfile
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public GXDLMSWeekProfile[] ParseWeekProfile(string hex)
        {
            byte[] data = GXCommon.HexToBytes(hex);
            List<GXDLMSWeekProfile> weeks = new List<GXDLMSWeekProfile>();
            int offset = 0;

            if (data.Length == 0)
                return weeks.ToArray(); // zero weeks

            if (data[offset++] != 0x01) throw new Exception("Expected array tag");
            offset++; // skip array count
            if (data[offset++] != 0x02) throw new Exception("Expected array start");

            while (offset < data.Length)
            {
                if (data[offset++] != 0x08) break; // structure start

                // Week Name
                if (data[offset++] != 0x09) throw new Exception("Expected Octet String for Week Name");
                int nameLength = data[offset++];
                string weekNameStr = Encoding.ASCII.GetString(data, offset, nameLength);
                offset += nameLength;

                // Day IDs array
                if (data[offset++] != 0x11) throw new Exception("Expected Array for Day IDs");
                int monday = data[offset++];
                offset++;
                int tuesday = data[offset++];
                offset++;
                int wednesday = data[offset++];
                offset++;
                int thursday = data[offset++];
                offset++;
                int friday = data[offset++];
                offset++;
                int saturday = data[offset++];
                offset++;
                int sunday = data[offset++];
                GXDLMSWeekProfile weekProfile = new GXDLMSWeekProfile(weekNameStr, monday, tuesday, wednesday, thursday, friday, saturday, sunday);
                weeks.Add(weekProfile);
                offset++;
                if (offset >= data.Length) break;
            }

            return weeks.ToArray();
        }
        /// <summary>
        /// This Converts the user provided Day profile DLMS hex string to array of type GXDLMSDayProfile
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public GXDLMSDayProfile[] ParseDayProfile(string hex)
        {
            byte[] data = GXCommon.HexToBytes(hex);
            List<GXDLMSDayProfile> dayProfiles = new List<GXDLMSDayProfile>();
            int offset = 0;

            if (data.Length == 0)
                return dayProfiles.ToArray(); // zero day profiles

            // Array tag
            if (data[offset++] != 0x01) throw new Exception("Expected array tag");

            // Array count (number of day_profiles)
            int dayProfileCount = data[offset++];

            for (int i = 0; i < dayProfileCount; i++)
            {
                // Structure tag for each day_profile
                if (data[offset++] != 0x02) throw new Exception("Expected structure tag for day_profile");

                int structureCount = data[offset++]; // should be 2: day_id + day_schedule
                if (structureCount != 2) throw new Exception("Expected structure count 2 for day_profile");

                // First element: day_id (unsigned)
                if (data[offset++] != 0x11) throw new Exception("Expected unsigned tag for day_id");
                int dayId = data[offset++];

                // Second element: day_schedule array
                if (data[offset++] != 0x01) throw new Exception("Expected array tag for day_schedule");
                int actionCount = data[offset++]; // number of day_profile_actions
                List<GXDLMSDayProfileAction> actions = new List<GXDLMSDayProfileAction>();

                for (int j = 0; j < actionCount; j++)
                {
                    // Each day_profile_action is a structure
                    if (data[offset++] != 0x02) throw new Exception("Expected structure tag for day_profile_action");
                    int actionStructureCount = data[offset++]; // should be 3: start_time, script_ln, script_selector
                    if (actionStructureCount != 3) throw new Exception("Expected structure count 3 for day_profile_action");

                    // Start Time: 4-byte octet-string
                    if (data[offset++] != 0x09 && data[offset - 1] != 0x0C) throw new Exception("Expected octet-string tag for start_time");
                    byte[] timeBytes = new byte[4];
                    offset++;
                    Array.Copy(data, offset, timeBytes, 0, 4);
                    offset += 4;

                    int hour = timeBytes[0] == 0xFF ? -1 : timeBytes[0];
                    int minute = timeBytes[1] == 0xFF ? -1 : timeBytes[1];
                    int second = timeBytes[2] == 0xFF ? -1 : timeBytes[2];
                    int hundredths = timeBytes[3] == 0xFF ? -1 : timeBytes[3];
                    GXDateTime gXDateTime = new GXDateTime(0, 0, 0, hour, minute, second, hundredths * 10);
                    GXTime startTime = new GXTime(gXDateTime);

                    // Script Logical Name: octet-string
                    if (data[offset++] != 0x09) throw new Exception("Expected octet-string tag for script_logical_name");
                    int lnLength = data[offset++];
                    byte[] scriptLogicalName = new byte[lnLength];
                    Array.Copy(data, offset, scriptLogicalName, 0, lnLength);
                    string scriptName = $"{scriptLogicalName[0]}.{scriptLogicalName[1]}.{scriptLogicalName[2]}.{scriptLogicalName[3]}.{scriptLogicalName[4]}.{scriptLogicalName[5]}";
                    offset += lnLength;
                    // Script Selector: long-unsigned
                    if (data[offset++] != 0x12) throw new Exception("Expected long-unsigned tag for script_selector");
                    int scriptSelector = (data[offset++] << 8) + data[offset++];
                    actions.Add(new GXDLMSDayProfileAction(startTime, scriptName, Convert.ToUInt16(scriptSelector)));
                }

                GXDLMSDayProfile dayProfile = new GXDLMSDayProfile(dayId, actions.ToArray());
                dayProfiles.Add(dayProfile);
            }

            return dayProfiles.ToArray();
        }

    }
}
