using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace AutoTest.FrameWork.Converts
{
    public class DLMSParser
    {
        #region Properties
        CultureInfo provider = CultureInfo.InvariantCulture;
        //Logger
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public Hashtable ErrorhshTable = new Hashtable();
        public Hashtable UnithshTable = new Hashtable();
        public Hashtable ObjhshTable = new Hashtable();
        public Hashtable ScalerhshTable = new Hashtable();
        public Hashtable ScalerFactorhshTable = new Hashtable();
        public Hashtable SendDestinationServicehshTable = new Hashtable();
        public Hashtable SendDestinationMessagehshTable = new Hashtable();
        public Hashtable DataTypeshashTable = new Hashtable();
        public Hashtable IChashTable = new Hashtable();
        public Hashtable AutoDisplayhshTable1P = new Hashtable();//Hash Table for Auto Display Parameter 1P 
        public Hashtable AutoDisplayhshTable3P = new Hashtable();//Hash Table for Auto Display Parameter 3P
        public Hashtable PushDisplayhshTable1P = new Hashtable();//Hash Table for Push Display Parameter 1P
        public Hashtable PushDisplayhshTable3P = new Hashtable();//Hash Table for Push Display Parameter 3P
        public Hashtable SecurityPolicyTable = new Hashtable();//Hash Table for Security Policy Type

        string filePath = System.Windows.Forms.Application.StartupPath + @"\AllObisList.txt";
        public static Dictionary<string, string[]> ObisdataDictionary = new Dictionary<string, string[]>();
        // Reference to the DLMSController's DataGridView
        private DataGridView dataGridView;
        #endregion

        #region Constructor
        public DLMSParser()
        {
            UnitHashTableIni();
            ErrorHashTableIni();
            ObjectsHashTableIni();
            ScalerHashTableIni();
            ScalerFactorHashTableIni();
            SendDestinationServiceHashTableIni();
            SendDestinationMessageHashTableIni();
            DataTypeshashTableIni();
            IChashTableIni();
            ObisdataDictionary = ReadObisListFileToDictionary(filePath);
            AutoDisplay1PHashTableIni();
            PushDisplay1PHashTableIni();
            AutoDisplay3PHashTableIni();
            PushDisplay3PHashTableIni();
            SecurityPolicyTableIni();
        }



        public DLMSParser(DataGridView dataGridView)
        {
            UnitHashTableIni();
            ErrorHashTableIni();
            ObjectsHashTableIni();
            ScalerHashTableIni();
            ScalerFactorHashTableIni();
            SendDestinationServiceHashTableIni();
            SendDestinationMessageHashTableIni();
            DataTypeshashTableIni();
            IChashTableIni();
            ObisdataDictionary = ReadObisListFileToDictionary(filePath);
            this.dataGridView = dataGridView;
            AutoDisplay1PHashTableIni();
            PushDisplay1PHashTableIni();
            AutoDisplay3PHashTableIni();
            PushDisplay3PHashTableIni();
            SecurityPolicyTableIni();
        }
        #endregion

        public void ParseDLMSData(string sData, string filePath, string _sPerator = " ")
        {
            DailyLoadDataParsing(sData, filePath, _sPerator);
        }
        public void ParseObjectListData(string sData, string filePath, string _sPerator = " ")
        {
            ObjectListDataParsing(sData, filePath, _sPerator);
        }
        void ObjectListDataParsing(string LoadData, string parseDLMSFile, string _sSaprator = " ")
        {
            System.IO.StreamWriter Sw = new StreamWriter(parseDLMSFile, true);
            try
            {

                int i = 0, nCount = 0;
                int j = -1, nLength = 0;
                string tempData = String.Empty;
                string strMatch = string.Empty;
                //Voltage Related Events 
                if (LoadData.Substring(5, 12) == "0000636200FF")
                {
                    Sw.Write(' ' + _sSaprator);
                    Sw.Write(' ' + _sSaprator);
                    i = 0;
                    nLength = 36;
                    LoadData = LoadData.Substring(35);
                    strMatch = LoadData.Substring(0, 4);
                    for (; i < LoadData.Length; i++)
                    {
                        while (true)
                        {
                            if (i >= LoadData.Length)
                                break;
                            if (LoadData.Substring(i, 4) == strMatch)
                            {
                                tempData = GateDataFromString(LoadData, ref i, tempData);
                            }
                            if (!string.IsNullOrEmpty(tempData))
                            {
                                //Sw.Write(tempData.PadLeft(25, ' ') + _sSaprator);
                                Sw.Write(tempData + " " + GetObisName(string.Empty, tempData) + _sSaprator);
                            }
                            //if (LoadData.Length > i && LoadData.Substring(i, nLength) == strMatch)
                            //{
                            //    i--;
                            //    break;
                            //}

                        }
                    }
                    Sw.Write("\r\n");
                }
                //Current Related Events
                if (LoadData.Substring(5, 12) == "0000636201FF")
                {
                    Sw.Write(' ' + _sSaprator);
                    Sw.Write(' ' + _sSaprator);
                    i = 0;
                    nLength = 36;
                    LoadData = LoadData.Substring(35);
                    strMatch = LoadData.Substring(0, 4);
                    for (; i < LoadData.Length; i++)
                    {
                        while (true)
                        {
                            if (i >= LoadData.Length)
                                break;
                            if (LoadData.Substring(i, 4) == strMatch)
                            {
                                tempData = GateDataFromString(LoadData, ref i, tempData);
                            }
                            if (!string.IsNullOrEmpty(tempData))
                            {
                                //Sw.Write(tempData.PadLeft(25, ' ') + _sSaprator);
                                Sw.Write(tempData + " " + GetObisName(string.Empty, tempData) + _sSaprator);
                            }
                            //if (LoadData.Length > i && LoadData.Substring(i, nLength) == strMatch)
                            //{
                            //    i--;
                            //    break;
                            //}

                        }
                    }
                    Sw.Write("\r\n");
                }
                //Power Related Events
                if (LoadData.Substring(5, 12) == "0000636202FF")
                {
                    Sw.Write(' ' + _sSaprator);
                    Sw.Write(' ' + _sSaprator);
                    i = 0;
                    nLength = 36;
                    LoadData = LoadData.Substring(35);
                    strMatch = LoadData.Substring(0, 4);
                    for (; i < LoadData.Length; i++)
                    {
                        while (true)
                        {
                            if (i >= LoadData.Length)
                                break;
                            if (LoadData.Substring(i, 4) == strMatch)
                            {
                                tempData = GateDataFromString(LoadData, ref i, tempData);
                            }
                            if (!string.IsNullOrEmpty(tempData))
                            {
                                //Sw.Write(tempData.PadLeft(25, ' ') + _sSaprator);
                                Sw.Write(tempData + " " + GetObisName(string.Empty, tempData) + _sSaprator);
                            }
                            //if (LoadData.Length > i && LoadData.Substring(i, nLength) == strMatch)
                            //{
                            //    i--;
                            //    break;
                            //}

                        }
                    }
                    Sw.Write("\r\n");
                }
                //Transaction Events
                if (LoadData.Substring(5, 12) == "0000636203FF")
                {
                    Sw.Write(' ' + _sSaprator);
                    Sw.Write(' ' + _sSaprator);
                    i = 0;
                    nLength = 36;
                    LoadData = LoadData.Substring(35);
                    strMatch = LoadData.Substring(0, 4);
                    for (; i < LoadData.Length; i++)
                    {
                        while (true)
                        {
                            if (i >= LoadData.Length)
                                break;
                            if (LoadData.Substring(i, 4) == strMatch)
                            {
                                tempData = GateDataFromString(LoadData, ref i, tempData);
                            }
                            if (!string.IsNullOrEmpty(tempData))
                            {
                                //Sw.Write(tempData.PadLeft(25, ' ') + _sSaprator);
                                Sw.Write(tempData + " " + GetObisName(string.Empty, tempData) + _sSaprator);
                            }
                            //if (LoadData.Length > i && LoadData.Substring(i, nLength) == strMatch)
                            //{
                            //    i--;
                            //    break;
                            //}

                        }
                    }
                    Sw.Write("\r\n");
                }
                //Other Tamper Events
                if (LoadData.Substring(5, 12) == "0000636204FF")
                {
                    Sw.Write(' ' + _sSaprator);
                    Sw.Write(' ' + _sSaprator);
                    i = 0;
                    nLength = 36;
                    LoadData = LoadData.Substring(35);
                    strMatch = LoadData.Substring(0, 4);
                    for (; i < LoadData.Length; i++)
                    {
                        while (true)
                        {
                            if (i >= LoadData.Length)
                                break;
                            if (LoadData.Substring(i, 4) == strMatch)
                            {
                                tempData = GateDataFromString(LoadData, ref i, tempData);
                            }
                            if (!string.IsNullOrEmpty(tempData))
                            {
                                //Sw.Write(tempData.PadLeft(25, ' ') + _sSaprator);
                                Sw.Write(tempData + " " + GetObisName(string.Empty, tempData) + _sSaprator);
                            }
                            //if (LoadData.Length > i && LoadData.Substring(i, nLength) == strMatch)
                            //{
                            //    i--;
                            //    break;
                            //}

                        }
                    }
                    Sw.Write("\r\n");
                }
                //Non Roll Over Events
                if (LoadData.Substring(5, 12) == "0000636205FF")
                {
                    Sw.Write(' ' + _sSaprator);
                    Sw.Write(' ' + _sSaprator);
                    i = 0;
                    nLength = 36;
                    LoadData = LoadData.Substring(35);
                    strMatch = LoadData.Substring(0, 4);
                    for (; i < LoadData.Length; i++)
                    {
                        while (true)
                        {
                            if (i >= LoadData.Length)
                                break;
                            if (LoadData.Substring(i, 4) == strMatch)
                            {
                                tempData = GateDataFromString(LoadData, ref i, tempData);
                            }
                            if (!string.IsNullOrEmpty(tempData))
                            {
                                //Sw.Write(tempData.PadLeft(25, ' ') + _sSaprator);
                                Sw.Write(tempData + " " + GetObisName(string.Empty, tempData) + _sSaprator);
                            }
                            //if (LoadData.Length > i && LoadData.Substring(i, nLength) == strMatch)
                            //{
                            //    i--;
                            //    break;
                            //}

                        }
                    }
                    Sw.Write("\r\n");
                }
                //Control Events
                if (LoadData.Substring(5, 12) == "0000636206FF")
                {
                    Sw.Write(' ' + _sSaprator);
                    Sw.Write(' ' + _sSaprator);
                    i = 0;
                    nLength = 36;
                    LoadData = LoadData.Substring(35);
                    strMatch = LoadData.Substring(0, 4);
                    for (; i < LoadData.Length; i++)
                    {
                        while (true)
                        {
                            if (i >= LoadData.Length)
                                break;
                            if (LoadData.Substring(i, 4) == strMatch)
                            {
                                tempData = GateDataFromString(LoadData, ref i, tempData);
                            }
                            if (!string.IsNullOrEmpty(tempData))
                            {
                                //Sw.Write(tempData.PadLeft(25, ' ') + _sSaprator);
                                Sw.Write(tempData + " " + GetObisName(string.Empty, tempData) + _sSaprator);
                            }
                            //if (LoadData.Length > i && LoadData.Substring(i, nLength) == strMatch)
                            //{
                            //    i--;
                            //    break;
                            //}

                        }
                    }
                    Sw.Write("\r\n");
                }
                //Block Load
                if (LoadData.Substring(5, 12) == "0100630100FF")
                {
                    Sw.Write(' ' + _sSaprator);
                    Sw.Write(' ' + _sSaprator);
                    i = 0;
                    nLength = 36;
                    LoadData = LoadData.Substring(35);
                    strMatch = LoadData.Substring(0, 4);
                    for (; i < LoadData.Length; i++)
                    {
                        while (true)
                        {
                            if (i >= LoadData.Length)
                                break;
                            if (LoadData.Substring(i, 4) == strMatch)
                            {
                                tempData = GateDataFromString(LoadData, ref i, tempData);
                            }
                            if (!string.IsNullOrEmpty(tempData))
                            {
                                //Sw.Write(tempData.PadLeft(25, ' ') + _sSaprator);
                                Sw.Write(tempData + " " + GetObisName(string.Empty, tempData) + _sSaprator);
                            }
                            //if (LoadData.Length > i && LoadData.Substring(i, nLength) == strMatch)
                            //{
                            //    i--;
                            //    break;
                            //}

                        }
                    }
                    Sw.Write("\r\n");
                }
                //Daily Load
                if (LoadData.Substring(5, 12) == "0100630200FF")
                {
                    Sw.Write(' ' + _sSaprator);
                    Sw.Write(' ' + _sSaprator);
                    i = 0;
                    nLength = 36;
                    LoadData = LoadData.Substring(35);
                    strMatch = LoadData.Substring(0, 4);
                    for (; i < LoadData.Length; i++)
                    {
                        while (true)
                        {
                            if (i >= LoadData.Length)
                                break;
                            if (LoadData.Substring(i, 4) == strMatch)
                            {
                                tempData = GateDataFromString(LoadData, ref i, tempData);
                            }
                            if (!string.IsNullOrEmpty(tempData))
                            {
                                //Sw.Write(tempData.PadLeft(25, ' ') + _sSaprator);
                                Sw.Write(tempData + " " + GetObisName(string.Empty, tempData) + _sSaprator);
                            }
                            //if (LoadData.Length > i && LoadData.Substring(i, nLength) == strMatch)
                            //{
                            //    i--;
                            //    break;
                            //}

                        }
                    }
                    Sw.Write("\r\n");
                }
                //Billing Profile
                if (LoadData.Substring(5, 12) == "0100620100FF")
                {
                    Sw.Write(' ' + _sSaprator);
                    Sw.Write(' ' + _sSaprator);
                    i = 0;
                    nLength = 36;
                    LoadData = LoadData.Substring(35);
                    strMatch = LoadData.Substring(0, 4);
                    for (; i < LoadData.Length; i++)
                    {
                        while (true)
                        {
                            if (i >= LoadData.Length)
                                break;
                            if (LoadData.Substring(i, 4) == strMatch)
                            {
                                tempData = GateDataFromString(LoadData, ref i, tempData);
                            }
                            if (!string.IsNullOrEmpty(tempData))
                            {
                                //Sw.Write(tempData.PadLeft(25, ' ') + _sSaprator);
                                //tempData = GetObisName(string.Empty, tempData);
                                Sw.Write(tempData + " " + GetObisName(string.Empty, tempData) + _sSaprator);
                            }
                            //if (LoadData.Length > i && LoadData.Substring(i, nLength) == strMatch)
                            //{
                            //    i--;
                            //    break;
                            //}

                        }
                    }
                    Sw.Write("\r\n");
                }
                //Instant Profile
                if (LoadData.Substring(5, 12) == "01005E5B00FF")
                {
                    Sw.Write(' ' + _sSaprator);
                    Sw.Write(' ' + _sSaprator);
                    i = 0;
                    nLength = 36;
                    LoadData = LoadData.Substring(35);
                    strMatch = LoadData.Substring(0, 4);
                    for (; i < LoadData.Length; i++)
                    {
                        while (true)
                        {
                            if (i >= LoadData.Length)
                                break;
                            if (LoadData.Substring(i, 4) == strMatch)
                            {
                                tempData = GateDataFromString(LoadData, ref i, tempData);
                            }
                            if (!string.IsNullOrEmpty(tempData))
                            {
                                //Sw.Write(tempData.PadLeft(25, ' ') + _sSaprator);
                                //tempData = GetObisName(string.Empty, tempData);
                                Sw.Write(tempData + " " + GetObisName(string.Empty, tempData) + _sSaprator);
                            }
                            //if (LoadData.Length > i && LoadData.Substring(i, nLength) == strMatch)
                            //{
                            //    i--;
                            //    break;
                            //}

                        }
                    }
                    Sw.Write("\r\n");
                }
                //Nameplate Profile
                if (LoadData.Substring(5, 12) == "00005E5B0AFF")
                {
                    Sw.Write(' ' + _sSaprator);
                    Sw.Write(' ' + _sSaprator);
                    i = 0;
                    nLength = 36;
                    LoadData = LoadData.Substring(35);
                    strMatch = LoadData.Substring(0, 4);
                    for (; i < LoadData.Length; i++)
                    {
                        while (true)
                        {
                            if (i >= LoadData.Length)
                                break;
                            if (LoadData.Substring(i, 4) == strMatch)
                            {
                                tempData = GateDataFromString(LoadData, ref i, tempData);
                            }
                            if (!string.IsNullOrEmpty(tempData))
                            {
                                //Sw.Write(tempData.PadLeft(25, ' ') + _sSaprator);
                                //tempData = GetObisName(string.Empty, tempData);
                                Sw.Write(tempData + " " + GetObisName(string.Empty, tempData) + _sSaprator);
                            }
                            //if (LoadData.Length > i && LoadData.Substring(i, nLength) == strMatch)
                            //{
                            //    i--;
                            //    break;
                            //}

                        }
                    }
                    Sw.Write("\r\n");
                }

            }
            catch (Exception e)
            {
                log.Error(e.Message.ToString());
                log.Error(e.StackTrace.ToString());
            }
            finally
            {
                if (Sw != null)
                {
                    Sw.Close();
                    Sw.Dispose();
                }
            }
        }
        void DailyLoadDataParsing(string LoadData, string parseDLMSFile, string _sSaprator = " ")
        {
            System.IO.StreamWriter Sw = new StreamWriter(parseDLMSFile);
            try
            {
                int i = 0, nCount = 0;
                int j = -1, nLength = 4; ;
                string tempData = String.Empty;
                string strMatch = string.Empty;


                if (LoadData == "0100" || LoadData == "01820000" || LoadData.Length < 4)
                {
                    Sw.Write(LoadData);
                    Sw.Close();
                    return;
                }

                if (LoadData.Substring(2, 2) == "82")
                {
                    i = 8;
                    strMatch = LoadData.Substring(8, 4);
                    nCount = Convert.ToInt32(LoadData.Substring(4, 4), 16);
                    Sw.Write(LoadData.Substring(0, 8) + " Count = " + nCount.ToString());
                }
                else if (LoadData.Substring(2, 2) == "81")
                {
                    i = 6;
                    strMatch = LoadData.Substring(6, 4);
                    nCount = Convert.ToInt32(LoadData.Substring(4, 2), 16);
                    Sw.Write(LoadData.Substring(0, 6) + " Count = " + nCount.ToString());
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
                    Sw.Write(LoadData.Substring(0, 4) + " Count = " + nCount.ToString());
                }

                j = 0;
                for (; i < LoadData.Length; i++)

                {
                    if (LoadData.Substring(i, nLength) == strMatch)
                    {
                        j++;
                        Sw.Write("\r\n" + "Entry#" + j.ToString().PadRight(5, ' ') + _sSaprator + strMatch + _sSaprator);
                        i += nLength;
                        while (true)
                        {
                            if (i >= LoadData.Length)
                                break;

                            tempData = GateDataFromString(LoadData, ref i, tempData);
                            if (!string.IsNullOrEmpty(tempData))
                            {
                                if (tempData.Length >= 15)// || tempData == "00 - null value")
                                    Sw.Write(tempData.PadLeft(25, ' ') + _sSaprator);
                                else
                                    Sw.Write(tempData.PadLeft(10, ' ') + _sSaprator);
                            }
                            if (LoadData.Length > i && LoadData.Substring(i, nLength) == strMatch)//LoadData.Substring(4, 4))
                            {
                                i--;
                                break;
                            }
                            // i--;
                        }
                    }
                }
                if (j != nCount)
                    Sw.Write("\r\n Count Mismatch.");
            }
            catch (Exception e)
            {
                log.Error(e.Message.ToString());
                log.Error(e.StackTrace.ToString());
            }
            finally
            {
                if (Sw != null)
                {
                    Sw.Close();
                    Sw.Dispose();
                }
            }
        }
        public string GateDataFromString(string strData, ref int nStart, string sData)
        {
            DateTime dt = DateTime.Now;
            string tmpStr = string.Empty;
            if (strData.Substring(nStart, 4) == "0906")
            {
                if ((strData.Length - nStart) < 36)
                {
                    string templocalstring = strData.Substring(nStart, (strData.Length - nStart));
                    templocalstring.PadRight(36, '0');
                    tmpStr = GetObis(templocalstring.PadRight(36, '0').Substring(4, 12));
                    nStart += 36;
                    return tmpStr;
                }
                else
                {
                    tmpStr = GetObis(strData.Substring(nStart + 4, 32));
                    nStart += 36;
                    return tmpStr;
                }
            }
            //BY AAC
            else if (strData.Substring(nStart, 2) == "0A")//For String
            {
                string sizeofSerialNumber = strData.Substring(nStart + 2, 2);
                int lengthofString = Convert.ToInt32(strData.Substring(nStart + 2, 2), 16);
                if (lengthofString == 0)
                {
                    tmpStr = "";
                    nStart += 4;
                }
                else
                {
                    tmpStr = HexString2Ascii(strData.Substring(nStart + 4, lengthofString * 2));
                    nStart += (4 + (lengthofString * 2));
                }
            }
            //END AAC
            else if (strData.Substring(nStart, 2) == "09")
            {
                if (strData.Substring(nStart, 4) == "090C")
                {
                    if (strData.Substring(nStart + 26, 2) == "00" || strData.Substring(nStart + 26, 2) == "FF")
                    {
                        tmpStr = Getdate(strData.Substring(nStart + 4, 24), 0, false);
                        //if (IsDate(tmpStr) == true)
                        //{
                        //   dt = DateTime.ParseExact(tmpStr, "dd/MM/yyyy HH:mm:ss", provider, DateTimeStyles.AssumeLocal);
                        //   tmpStr = dt.ToString("dd/MM/yyyy hh:mm:ss tt");
                        //}
                        nStart += 28;
                    }
                    else if (strData.Substring(nStart, 4) == "090C")
                    {
                        tmpStr = HexStringToAscii(strData.Substring(nStart + 4, 24));
                        nStart += 28;
                    }
                    else if (strData.Substring(nStart + 2, 2) != "0C")
                    {
                        nStart += 2;
                    }
                }
                else if (strData.Substring(nStart, 4) == "0904")//project specific Adani Project (Network Information Parameters (For GPRS NIC) => 0.0.96.99.1.255 1/2)/*
                                                                //1st Byte will be RSRP (Unsigned8)
                                                                //2nd Byte will be RSRQ(Unsigned8)
                                                                //3rd Byte will be SNR(Unsigned8)
                                                                //4th Byte will be RSSI(Unsigned8) */
                {
                    tmpStr = $"RSRP({strData.Substring(nStart + 4, 2)}) RSRQ({strData.Substring(nStart + 4 + 2, 2)}) SNR({strData.Substring(nStart + 4 + 4, 2)}) RSSI({strData.Substring(nStart + 4 + 6, 2)})";
                    nStart += 12;
                }
                //else if (strData.Substring(nStart, 4) == "0905")
                //{
                //    tmpStr = int.Parse(strData.Substring(nStart + 8, 2), NumberStyles.HexNumber).ToString("00");
                //    tmpStr += "/";
                //    tmpStr += int.Parse(strData.Substring(nStart + 4, 4), NumberStyles.HexNumber).ToString("0000");
                //    //tmpStr += HexString2Ascii(strData.Substring(nStart + 4, 4));
                //    nStart += 14;
                //}
                //BY AAC for project CIQD183
                else if (strData.Substring(nStart, 4) == "091F" || strData.Substring(nStart, 4) == "091C")
                {
                    string sizeofname = strData.Substring(nStart + 2, 2);
                    int lengthofString = Convert.ToInt32(strData.Substring(nStart + 2, 2), 16);
                    if (lengthofString == 0)
                    {
                        tmpStr = "";
                        nStart += 4;
                    }
                    else
                    {
                        tmpStr = HexString2Ascii(strData.Substring(nStart + 4, lengthofString * 2));
                        nStart += (4 + (lengthofString * 2));
                    }
                }
                //END AAC
                else if (strData.Substring(nStart).Length > 26)
                {
                    if (strData.Substring(nStart + 26, 2) == "00" || strData.Substring(nStart + 26, 2) == "FF")
                    {
                        tmpStr = Getdate(strData.Substring(nStart + 4, 24), 0, false);
                        if (IsDate(tmpStr) == true)
                        {
                            dt = DateTime.ParseExact(tmpStr, "dd/MM/yyyy HH:mm:ss", provider, DateTimeStyles.AssumeLocal);
                            tmpStr = dt.ToString("dd/MM/yyyy hh:mm:ss tt");
                        }
                        nStart += 28;
                    }
                    else if (strData.Substring(nStart, 4) == "090C")
                    {
                        tmpStr = HexStringToAscii(strData.Substring(nStart + 4, 24));
                        nStart += 28;
                    }
                    else
                    {
                        string sizeofname = strData.Substring(nStart + 2, 2);
                        int lengthofString = Convert.ToInt32(strData.Substring(nStart + 2, 2), 16);
                        if (lengthofString == 0)
                        {
                            tmpStr = "";
                            nStart += 4;
                        }
                        else
                        {
                            tmpStr = HexString2Ascii(strData.Substring(nStart + 4, lengthofString * 2));
                            nStart += (4 + (lengthofString * 2));
                        }
                    }
                    //else if (strData.Substring(nStart + 2, 2) != "0C")
                    //{
                    //    nStart += 2;
                    //}
                }
                else
                {
                    string sizeofname = strData.Substring(nStart + 2, 2);
                    int lengthofString = Convert.ToInt32(strData.Substring(nStart + 2, 2), 16);
                    if (lengthofString == 0)
                    {
                        tmpStr = "";
                        nStart += 4;
                    }
                    else
                    {
                        tmpStr = HexString2Ascii(strData.Substring(nStart + 4, lengthofString * 2));
                        nStart += (4 + (lengthofString * 2));
                    }
                }
            }

            /*
            if (strData.Substring(nStart, 4) == "0906")
            {
                if ((strData.Length - nStart) < 36)
                {
                    string templocalstring = strData.Substring(nStart, (strData.Length - nStart));
                    templocalstring.PadRight(36, '0');
                    tmpStr = GetObis(templocalstring.PadRight(36, '0').Substring(4, 12));
                    nStart += 36;
                    return tmpStr;
                }
                else
                {
                    tmpStr = GetObis(strData.Substring(nStart + 4, 32));
                    nStart += 36;
                    return tmpStr;
                }
            }
            //BY AAC
            else if (strData.Substring(nStart, 2) == "0A")//For String
            {
                string sizeofSerialNumber = strData.Substring(nStart + 2, 2);

                int lengthofString = Convert.ToInt32(strData.Substring(nStart + 2, 2), 16);
                if (lengthofString == 0)
                {
                    tmpStr = " ";
                    nStart += 4;
                }
                else
                {
                    tmpStr = HexString2Ascii(strData.Substring(nStart + 4, lengthofString * 2));
                    nStart += (4 + (lengthofString * 2));
                }

            }
            //END AAC
            else if (strData.Substring(nStart, 2) == "09")
            {

                if (strData.Substring(nStart, 4) == "0905")
                {
                    tmpStr = int.Parse(strData.Substring(nStart + 8, 2), NumberStyles.HexNumber).ToString("00");
                    tmpStr += "/";
                    tmpStr += int.Parse(strData.Substring(nStart + 4, 4), NumberStyles.HexNumber).ToString("0000");
                    //tmpStr += HexString2Ascii(strData.Substring(nStart + 4, 4));
                    nStart += 14;
                }
                //BY AAC for project CIQD183
                else if (strData.Substring(nStart, 4) == "091F")
                {
                    string sizeofname = strData.Substring(nStart + 2, 2);
                    int lengthofString = Convert.ToInt32(strData.Substring(nStart + 2, 2), 16);
                    if (lengthofString == 0)
                    {
                        tmpStr = " ";
                        nStart += 4;
                    }
                    else
                    {
                        tmpStr = HexString2Ascii(strData.Substring(nStart + 4, lengthofString * 2));
                        nStart += (4 + (lengthofString * 2));
                    }
                }
                //END AAC
                else if (strData.Substring(nStart).Length > 26)
                {
                    if (strData.Substring(nStart + 26, 2) == "00" || strData.Substring(nStart + 26, 2) == "FF")
                    {
                        tmpStr = Getdate(strData.Substring(nStart + 4, 24), 0, false);
                        //if (IsDate(tmpStr) == true)
                        //    dt = DateTime.ParseExact(tmpStr, "dd/MM/yyyy hh:mm:ss tt", provider, DateTimeStyles.AssumeLocal);
                        nStart += 28;
                    }
                    else if (strData.Substring(nStart, 4) == "090C")
                    {
                        tmpStr = HexStringToAscii(strData.Substring(nStart + 4, 24));
                        nStart += 28;
                    }
                    else if (strData.Substring(nStart + 2, 2) != "0C")
                    {
                        nStart += 2;
                    }
                }


            }
            */
            else if (strData.Substring(nStart, 2) == "06")
            {
                tmpStr = UInt32.Parse(strData.Substring(nStart + 2, 8), NumberStyles.HexNumber).ToString();
                nStart += 10;
            }
            else if (strData.Substring(nStart, 2) == "05")
            {
                tmpStr = Int32.Parse(strData.Substring(nStart + 2, 8), NumberStyles.HexNumber).ToString();
                nStart += 10;
            }
            else if (strData.Substring(nStart, 2) == "15")
            {
                tmpStr = Int64.Parse(strData.Substring(nStart + 2, 16), NumberStyles.HexNumber).ToString();

                nStart += 18;
            }
            else if (strData.Substring(nStart, 2) == "14")
            {
                tmpStr = Int64.Parse(strData.Substring(nStart + 2, 16), NumberStyles.HexNumber).ToString();
                nStart += 18;
            }
            else if (strData.Substring(nStart, 2) == "12")
            {
                tmpStr = UInt16.Parse(strData.Substring(nStart + 2, 4), NumberStyles.HexNumber).ToString();
                int eventIDinInt = Convert.ToInt32(tmpStr);
                //tmpStr ="("+tmpStr+") "+IDToEventName(eventIDinInt);
                nStart += 6;
            }
            else if (strData.Substring(nStart, 2) == "10")
            {
                tmpStr = Int16.Parse(strData.Substring(nStart + 2, 4), NumberStyles.HexNumber).ToString();
                nStart += 6;
            }
            else if (strData.Substring(nStart, 2) == "11")
            {
                tmpStr = Int16.Parse(strData.Substring(nStart + 2, 2), NumberStyles.HexNumber).ToString();
                nStart += 4;
            }
            //else if (strData.Substring(nStart, 2) == "04")
            //{
            //    if (strData.Substring(nStart + 2, 2) == "82")
            //    {
            //        tmpStr = strData.Substring(nStart + 8, int.Parse(strData.Substring(nStart + 4, 4), NumberStyles.HexNumber) / 4);
            //        nStart += 8 + int.Parse(strData.Substring(nStart + 4, 4), NumberStyles.HexNumber) / 4;
            //    }
            //    else
            //    {
            //        tmpStr = strData.Substring(nStart + 4, int.Parse(strData.Substring(nStart + 2, 2), NumberStyles.HexNumber) / 4);
            //        nStart += 4 + int.Parse(strData.Substring(nStart + 2, 2), NumberStyles.HexNumber) / 4;
            //    }

            //}
            else if (strData.Substring(nStart, 2) == "17")
            {
                tmpStr = ConvertToFloat(strData.Substring(nStart + 2, 8)).ToString();// strData.Substring(nStart + 4, int.Parse(strData.Substring(nStart + 2, 2), NumberStyles.HexNumber) / 4);
                nStart += 10;
            }
            else if (strData.Substring(nStart).Length == 14 && strData.Substring(nStart, 14) == "01010202110101")
            {
                nStart += 14;
                int nCnt = Convert.ToInt32(strData.Substring(nStart, 2), 16);
                //nCnt = 3;
                tmpStr = strData.Substring(nStart + 2, nCnt * 38);
                nStart = nStart + 2 + nCnt * 38;
            }
            else if (strData.Substring(nStart, 2) == "0F")
            {
                tmpStr = UInt32.Parse(strData.Substring(nStart + 2, 2), NumberStyles.HexNumber).ToString();
                nStart += 4;
            }
            else if (strData.Substring(nStart, 2) == "00")
            {
                //dt = dt.AddMinutes(LSIP);
                //tmpStr = dt.ToString("dd/MM/yyyy HH:mm:ss");
                tmpStr = "00 - null value";
                nStart += 2;
            }
            else if (strData.Substring(nStart, 2) == "01")
            {
                // tmpStr = UInt32.Parse(strData.Substring(nStart + 2, 2), NumberStyles.HexNumber).ToString();
                nStart += 2;
            }
            else if (strData.Substring(nStart, 2) == "02")
            {
                #region NEW LOGIC               
                if (strData.Substring(nStart + 4, 2) == "0A")
                {
                    int lengthofString = Convert.ToInt32(strData.Substring(nStart + 2, 2), 16);
                    string tempOctatStr = strData.Substring(nStart + 4);
                    Int32 totalByteCount = 0;
                    Int32 tempNStart = 0;
                    for (int i = 1; i <= lengthofString; i++)
                    {
                        Int32 visibalLength = 0;
                        visibalLength = Convert.ToInt32(tempOctatStr.Substring(tempNStart + 2, 2), 16);
                        totalByteCount = (visibalLength * 2);
                        if (visibalLength == 0)
                            tmpStr += " ";
                        else
                            tmpStr += (tempOctatStr.Substring(tempNStart + 4, totalByteCount) + "-");
                        tempNStart += (4 + (visibalLength * 2));
                    }
                    tmpStr = strData.Substring(nStart, tempNStart);
                    nStart += tempNStart + 4;
                }
                #endregion
                // tmpStr = UInt32.Parse(strData.Substring(nStart + 2, 2), NumberStyles.HexNumber).ToString();
                else if (strData.Substring(nStart, 4) == "020A")
                {
                    tmpStr = strData.Substring(nStart, 56);
                    tmpStr = GetProfileValueString(tmpStr);
                    nStart += 56;
                }
                else
                    nStart += 2;
            }
            //BY AAC
            else if (strData.Substring(nStart, 2) == "03")
            {
                tmpStr = Int16.Parse(strData.Substring(nStart + 2, 2), NumberStyles.HexNumber).ToString();
                nStart += 4;
            }
            else if (strData.Substring(nStart, 2) == "08")
            {
                //tmpStr = Int16.Parse(strData.Substring(nStart + 2, 22), NumberStyles.HexNumber).ToString();
                nStart += 34;
            }
            //END AAC
            else if (strData.Substring(nStart, 2) == "16")
            {
                // tmpStr = UInt32.Parse(strData.Substring(nStart + 2, 2), NumberStyles.HexNumber).ToString();
                nStart += 2;
            }
            else if (strData.Substring(nStart, 2) == "21")
            {
                // tmpStr = UInt32.Parse(strData.Substring(nStart + 2, 2), NumberStyles.HexNumber).ToString();
                nStart += 2;
            }
            else if (strData.Substring(nStart, 2) == "23")
            {
                // tmpStr = UInt32.Parse(strData.Substring(nStart + 2, 2), NumberStyles.HexNumber).ToString();
                nStart += 2;
            }
            else if (strData.Substring(nStart, 2) == "FF")
            {
                // tmpStr = UInt32.Parse(strData.Substring(nStart + 2, 2), NumberStyles.HexNumber).ToString();
                nStart += 2;
            }
            else if (strData.Substring(nStart, 2) == "04")
            {
                string tmpHexStr;
                string length = strData.Substring(nStart + 2, 2);
                if (strData.Substring(nStart + 2, 2) == "82")
                {
                    tmpHexStr = strData.Substring(nStart + 8, int.Parse(strData.Substring(nStart + 4, 4), NumberStyles.HexNumber) / 4);
                    tmpStr = ConvertHexToBinary(tmpHexStr);
                    nStart += 8 + int.Parse(strData.Substring(nStart + 4, 4), NumberStyles.HexNumber) / 4;
                }
                else
                {
                    tmpHexStr = strData.Substring(nStart + 4, int.Parse(strData.Substring(nStart + 2, 2), NumberStyles.HexNumber) / 4);
                    tmpStr = ConvertHexToBinary(tmpHexStr);
                    nStart += 4 + int.Parse(strData.Substring(nStart + 2, 2), NumberStyles.HexNumber) / 4;
                }
                if (length == "20")
                    tmpStr = MeterHeath(tmpStr);
            }
            return tmpStr;
        }
        private string MeterHeath(string cellValue)
        {
            string tempStr = string.Empty;
            if (!string.IsNullOrEmpty(cellValue) && cellValue != "11111111111111111111111111111111")
            {
                tempStr = $"[{cellValue}]=>";
                if (cellValue.Substring(0, 1) == "1")
                    tempStr += "Power fail happened in the LSIP,";
                if (cellValue.Substring(1, 1) == "1")
                    tempStr += "Relays disconnected in the LSIP,";
                if (cellValue.Substring(2, 1) == "1")
                    tempStr += "Error 18,";
                if (cellValue.Substring(3, 1) == "1")
                    tempStr += "Error 17,";
                if (cellValue.Substring(4, 1) == "1")
                    tempStr += "Error 16,";
                if (cellValue.Substring(5, 1) == "1")
                    tempStr += "Error 15,";
                if (cellValue.Substring(6, 1) == "1")
                    tempStr += "Error 14,";
                if (cellValue.Substring(7, 1) == "1")
                    tempStr += "Error 13,";
                if (cellValue.Substring(8, 1) == "1")
                    tempStr += "Error 4 - Sim invalid,";
                if (cellValue.Substring(9, 1) == "1")
                    tempStr += "Error 5 - No GSM Network coverage,";
                if (cellValue.Substring(10, 1) == "1")
                    tempStr += "Error 6 - GPRS Network registration failure,";
                if (cellValue.Substring(11, 1) == "1")
                    tempStr += "Error 9 - GPRS connection not establish, ";
                if (cellValue.Substring(12, 1) == "1")
                    tempStr += "Error 12 - Any Key mismatch b/w Meter and NIC, ";
                switch (cellValue.Substring(13, 3))
                {
                    case "001":
                        tempStr += "Error 1 - Meter NIC communication failure, ";
                        break;
                    case "010":
                        tempStr += "Error 2 - Modem initialization failure, ";
                        break;
                    case "011":
                        tempStr += "Error 3 - SIM not detected, ";
                        break;
                    case "100":
                        tempStr += "Error 7 - GPRS registration denied, ";
                        break;
                    case "101":
                        tempStr += "Error 8 - No APN configured, ";
                        break;
                    case "110":
                        tempStr += "Error 10 - HES IP/Port not configured, ";
                        break;
                    case "111":
                        tempStr += "Error 11 - HES port not open, ";
                        break;
                }
                string rssiMinima = cellValue.Substring(16, 5);
                tempStr += $"RSSI Min = {Convert.ToInt32(rssiMinima, 2)}, ";
                string rssiAvg = cellValue.Substring(21, 5);
                tempStr += $"RSSI Avg = {Convert.ToInt32(rssiAvg, 2)}, ";
                if (cellValue.Substring(28, 1) == "1")
                    tempStr += "Optical Comm. happened in the LSIP, ";
                switch (cellValue.Substring(29, 2))
                {
                    //case "00":
                    //    tempStr += "Error 1 - Meter NIC communication failure, ";
                    //    break;
                    case "01":
                        tempStr += "RTC Sync - Retard(IC8, Method-6), ";
                        break;
                    case "10":
                        tempStr += "RTC Sync - Advance(IC8, Method-6), ";
                        break;
                    case "11":
                        tempStr += "RTC difference high , ";
                        break;
                }
                if (cellValue.Substring(31, 1) == "1")
                    tempStr += "Temperature > 70'C or configured threshold, ";
            }
            else
                tempStr = $"{cellValue}";
            return tempStr;
        }
        public static string ConformanceServicesSupported(string hexString)
        {
            string confString = string.Empty;
            string binString = ConvertHexToBinary(hexString);
            if (binString.Substring(0, 1) == "1")
                confString += "reserved-zero, ";
            if (binString.Substring(1, 1) == "1")
                confString += "general-protection, ";
            if (binString.Substring(2, 1) == "1")
                confString += "general-block-transfer, ";
            if (binString.Substring(3, 1) == "1")
                confString += "read, ";
            if (binString.Substring(4, 1) == "1")
                confString += "write, ";
            if (binString.Substring(5, 1) == "1")
                confString += "unconfirmed-write, ";
            if (binString.Substring(6, 1) == "1")
                confString += "delta-value-encoding, ";
            if (binString.Substring(7, 1) == "1")
                confString += "reserved-seven, ";
            if (binString.Substring(8, 1) == "1")
                confString += "attribute0-supported-with-set, ";
            if (binString.Substring(9, 1) == "1")
                confString += "priority-mgmt-supported, ";
            if (binString.Substring(10, 1) == "1")
                confString += "attribute0-supported-with-get, ";
            if (binString.Substring(11, 1) == "1")
                confString += "block-transfer-with-get-or-read, ";
            if (binString.Substring(12, 1) == "1")
                confString += "block-transfer-with-set-or-write, ";
            if (binString.Substring(13, 1) == "1")
                confString += "block-transfer-with-action, ";
            if (binString.Substring(14, 1) == "1")
                confString += "multiple-references, ";
            if (binString.Substring(15, 1) == "1")
                confString += "information-report, ";
            if (binString.Substring(16, 1) == "1")
                confString += "data-notification, ";
            if (binString.Substring(17, 1) == "1")
                confString += "access, ";
            if (binString.Substring(18, 1) == "1")
                confString += "parameterized-access, ";
            if (binString.Substring(19, 1) == "1")
                confString += "get, ";
            if (binString.Substring(20, 1) == "1")
                confString += "set, ";
            if (binString.Substring(21, 1) == "1")
                confString += "selective-access, ";
            if (binString.Substring(22, 1) == "1")
                confString += "event-notification, ";
            if (binString.Substring(23, 1) == "1")
                confString += "action, ";
            confString = confString.Trim();
            if (confString.EndsWith(","))
                confString = confString.Substring(0, confString.Length - 1);
            return confString;
        }
        public string ESWFStatus(string cellValue)
        {
            string tempStr = string.Empty;
            if (!string.IsNullOrEmpty(cellValue))
            {
                tempStr = $"[{cellValue}]=>";
                if (cellValue.Substring(0, 1) == "1") tempStr += "R Phase- Voltage Missing,";
                if (cellValue.Substring(1, 1) == "1") tempStr += "Y Phase- Voltage Missing,";
                if (cellValue.Substring(2, 1) == "1") tempStr += "B Phase- Voltage Missing,";
                if (cellValue.Substring(3, 1) == "1") tempStr += "Over Voltage,";
                if (cellValue.Substring(4, 1) == "1") tempStr += "Low Voltage,";
                if (cellValue.Substring(5, 1) == "1") tempStr += "Voltage Unbalance,";
                if (cellValue.Substring(6, 1) == "1") tempStr += "R Phase- current reverse,";
                if (cellValue.Substring(7, 1) == "1") tempStr += "Y Phase- current reverse,";
                if (cellValue.Substring(8, 1) == "1") tempStr += "B Phase- current reverse,";
                if (cellValue.Substring(9, 1) == "1") tempStr += "Current Unbalance,";
                if (cellValue.Substring(10, 1) == "1") tempStr += "Current Bypass/Short,";
                if (cellValue.Substring(11, 1) == "1") tempStr += "Over current in any phase, ";
                if (cellValue.Substring(12, 1) == "1") tempStr += "Very Low PF, ";
                if (cellValue.Substring(51, 1) == "1") tempStr += "Earth Loading (1P),";
                if (cellValue.Substring(61, 1) == "1") tempStr += "Module cover restoration,";
                if (cellValue.Substring(62, 1) == "1") tempStr += "Neutral Miss/Single wire restoration(1P),";
                if (cellValue.Substring(63, 1) == "1") tempStr += "Unauthorized export of energy,";
                if (cellValue.Substring(64, 1) == "1") tempStr += "R Phase - Voltage Low,";
                if (cellValue.Substring(65, 1) == "1") tempStr += "Y Phase - Voltage Low,";
                if (cellValue.Substring(66, 1) == "1") tempStr += "B Phase - Voltage Low,";
                if (cellValue.Substring(67, 1) == "1") tempStr += "R Phase - Voltage High,";
                if (cellValue.Substring(68, 1) == "1") tempStr += "Y Phase - Voltage High,";
                if (cellValue.Substring(69, 1) == "1") tempStr += "B Phase - Voltage High,";
                if (cellValue.Substring(70, 1) == "1") tempStr += "Over Frequency,";
                if (cellValue.Substring(71, 1) == "1") tempStr += "Under Frequency,";
                if (cellValue.Substring(72, 1) == "1") tempStr += "R Phase - Voltage Swell,";
                if (cellValue.Substring(73, 1) == "1") tempStr += "Y Phase - Voltage Swell,";
                if (cellValue.Substring(74, 1) == "1") tempStr += "B Phase - Voltage Swell,";
                if (cellValue.Substring(75, 1) == "1") tempStr += "R Phase - Voltage Sag,";
                if (cellValue.Substring(76, 1) == "1") tempStr += "Y Phase - Voltage Sag,";
                if (cellValue.Substring(77, 1) == "1") tempStr += "B Phase - Voltage Sag,";
                if (cellValue.Substring(78, 1) == "1") tempStr += "Micro Abnormal Reset,";
                if (cellValue.Substring(79, 1) == "1") tempStr += "Relay Switch Weld,";
                if (cellValue.Substring(81, 1) == "1") tempStr += "Influence of permanent magnet or ac/dc electromagnet, ";
                if (cellValue.Substring(82, 1) == "1") tempStr += "Neutral disturbance- HF,dc or alternate method, ";
                if (cellValue.Substring(83, 1) == "1") tempStr += "Meter cover open, ";
                if (cellValue.Substring(84, 1) == "1") tempStr += "Meter load disconnected, ";
                else
                    tempStr += "Meter load connected, ";
                if (cellValue.Substring(85, 1) == "1") tempStr += "Last Gasp- Occurrence, ";
                if (cellValue.Substring(86, 1) == "1") tempStr += "First Breath- Restoration, ";
                if (cellValue.Substring(87, 1) == "1") tempStr += "Increment in Billing counter (Manual/MRI reset), ";
                if (cellValue.Substring(89, 1) == "1") tempStr += "Neutral Miss,";
                if (cellValue.Substring(90, 1) == "1") tempStr += "LRCF Availed,";
                if (cellValue.Substring(91, 1) == "1") tempStr += "NIC Firmware upgraded,";
                if (cellValue.Substring(92, 1) == "1") tempStr += "Module Firmware upgraded,";
                if (cellValue.Substring(93, 1) == "1") tempStr += "Firmware Upgrade,";
                if (cellValue.Substring(94, 1) == "1") tempStr += "Invalid Voltage,";
                if (cellValue.Substring(95, 1) == "1") tempStr += "High input on DI1,";
                if (cellValue.Substring(96, 1) == "1") tempStr += "High input on DI2,";
                if (cellValue.Substring(97, 1) == "1") tempStr += "High input on DI3,";
                if (cellValue.Substring(98, 1) == "1") tempStr += "High input on DI4,";
                if (cellValue.Substring(99, 1) == "1") tempStr += "Password Authentication Failure,";
                if (cellValue.Substring(100, 1) == "1") tempStr += "R Phase-Current without voltage, ";
                if (cellValue.Substring(101, 1) == "1") tempStr += "Y Phase-Current without voltage, ";
                if (cellValue.Substring(102, 1) == "1") tempStr += "B Phase-Current without voltage, ";
                if (cellValue.Substring(103, 1) == "1") tempStr += "RTC battery - Low battery status,";
                if (cellValue.Substring(104, 1) == "1") tempStr += "Invalid Phase association,";
                if (cellValue.Substring(105, 1) == "1") tempStr += "Reverse Phase Sequence,";
                if (cellValue.Substring(106, 1) == "1") tempStr += "ESD/Jammer/Microwave, ";
                if (cellValue.Substring(107, 1) == "1") tempStr += "R-Phase CT Open,";
                if (cellValue.Substring(108, 1) == "1") tempStr += "Y-Phase CT Open,";
                if (cellValue.Substring(109, 1) == "1") tempStr += "B-Phase CT Open,";
                if (cellValue.Substring(110, 1) == "1") tempStr += "Module Cover Open, ";
                if (cellValue.Substring(111, 1) == "1") tempStr += "Over Load, ";
                if (cellValue.Substring(112, 1) == "1") tempStr += "Current mismatch (1P),";
                if (cellValue.Substring(113, 1) == "1") tempStr += "Current High THD, ";
                if (cellValue.Substring(114, 1) == "1") tempStr += "Voltage High THD, ";
                if (cellValue.Substring(116, 1) == "1") tempStr += "High Temperature, ";
                if (cellValue.Substring(117, 1) == "1") tempStr += "Frequency Variation,";
                if (cellValue.Substring(118, 1) == "1") tempStr += "Terminal Cover Open,";
                if (cellValue.Substring(119, 1) == "1") tempStr += "Main battery- Low battery status,";
                if (cellValue.Substring(120, 1) == "1") tempStr += "R-Phase Over current,";
                if (cellValue.Substring(121, 1) == "1") tempStr += "Y-Phase Over current,";
                if (cellValue.Substring(122, 1) == "1") tempStr += "B-Phase Over current,";
                if (cellValue.Substring(123, 1) == "1") tempStr += "High Neutral Current, ";
                if (cellValue.Substring(124, 1) == "1") tempStr += "R-Phase relay disconnected/R-Phase relay connected,";
                if (cellValue.Substring(125, 1) == "1") tempStr += "Y-Phase relay disconnected/Y-Phase relay connected,";
                if (cellValue.Substring(126, 1) == "1") tempStr += "B-Phase relay disconnected/B-Phase relay connected,";
                if (cellValue.Substring(127, 1) == "1") tempStr += "Neutral relay disconnected/Neutral relay connected,";
            }
            else
                tempStr = $"{cellValue}";
            return tempStr;
        }
        public string[] GetDlmObjectListParsing(string LoadData)
        {
            string[] list;
            int count;
            string[] splitedData = LoadData.Split(' ');
            LoadData = splitedData[3].Trim();
            if (LoadData.Substring(2, 2) == "82")
            {
                list = Regex.Split(LoadData, LoadData.Substring(8, 4));
                count = int.Parse(list[0].Substring(4, 4), NumberStyles.HexNumber);
            }
            else if (LoadData.Substring(2, 2) == "81")
            {
                list = Regex.Split(LoadData, LoadData.Substring(6, 4));
                count = int.Parse(list[0].Substring(4, 2), NumberStyles.HexNumber);
            }
            else
            {
                list = Regex.Split(LoadData, LoadData.Substring(4, 4));
                count = int.Parse(list[0].Substring(2, 2), NumberStyles.HexNumber);
            }
            string[] classValue = new string[count];
            string[] obisValue = new string[count];
            string[] attributeValue = new string[count];
            for (int i = 1; i < list.Length; i++)
            {

                int j = i - 1;
                list[i] = list[i].PadRight(32, '0');
                classValue[j] = (int.Parse(list[i].Substring(2, 4), NumberStyles.HexNumber)).ToString();
                obisValue[j] = GetObis(list[i].Substring(10, 12));
                attributeValue[j] = (int.Parse(list[i].Substring(24, 2), NumberStyles.HexNumber)).ToString();
                list[i] = $"{classValue[j]}-{obisValue[j]}-{attributeValue[j]}";
            }
            list = list.Skip(1).ToArray();
            //string[] list = new string[200];
            //int i = 0, nCount = 0;
            //int j = 0, nLength = 0;
            //string tempData = String.Empty;
            //string strMatch = string.Empty;
            //i = 0;
            //nLength = 36;
            //LoadData = LoadData.Substring(35);
            //strMatch = LoadData.Substring(0, 4);
            //for (; i < LoadData.Length; i++)
            //{
            //    while (true)
            //    {
            //        if (i >= LoadData.Length)
            //            break;
            //        if (LoadData.Substring(i, 4) == strMatch)
            //        {
            //            tempData = GateDataFromString(LoadData, ref i, tempData);
            //        }
            //        if (!string.IsNullOrEmpty(tempData))
            //        {
            //            //list[j] = tempData.Trim()+ "  |  " + GetObisName(string.Empty, tempData.Trim());
            //            list[j] = tempData.Trim();
            //            j++;
            //        }
            //    }
            //}
            return list;
        }
        public DataTable GetProfileObjectTable(string LoadData, string Profile)
        {
            // Create a DataTable
            DataTable dataTable = new DataTable();
            // Add columns to the DataTable
            dataTable.Columns.Add("Class", typeof(string));
            dataTable.Columns.Add("Obis", typeof(string));
            dataTable.Columns.Add("Attribute", typeof(string));
            try
            {
                if (LoadData.Substring(0, 20) == "0007 01005E5B00FF 03" ||
                    LoadData.Substring(0, 20) == "0007 01005E5B03FF 03" ||
                    LoadData.Substring(0, 20) == "0007 00005E5B0AFF 03" ||
                    LoadData.Substring(0, 20) == "0007 0000636200FF 03" ||
                    LoadData.Substring(0, 20) == "0007 0000636201FF 03" ||
                    LoadData.Substring(0, 20) == "0007 0000636202FF 03" ||
                    LoadData.Substring(0, 20) == "0007 0000636203FF 03" ||
                    LoadData.Substring(0, 20) == "0007 0000636204FF 03" ||
                    LoadData.Substring(0, 20) == "0007 0000636205FF 03" ||
                    LoadData.Substring(0, 20) == "0007 0000636206FF 03" ||
                    LoadData.Substring(0, 20) == "0007 0000636281FF 03" ||
                    LoadData.Substring(0, 20) == "0007 01005E5B07FF 03" ||
                    LoadData.Substring(0, 20) == "0007 0100620100FF 03" ||
                    LoadData.Substring(0, 20) == "0007 01005E5B06FF 03" ||
                    LoadData.Substring(0, 20) == "0007 0100630100FF 03" ||
                    LoadData.Substring(0, 20) == "0007 01005E5B04FF 03" ||
                    LoadData.Substring(0, 20) == "0007 0100630200FF 03" ||
                    LoadData.Substring(0, 20) == "0007 01005E5B05FF 03"
                    )
                {
                    string[] splitedData = LoadData.Split(' ');
                    string[] obisArray;
                    int count;
                    if (splitedData[3].Trim() == "0100" || splitedData[3].Trim() == "01820000" || splitedData[3].Trim() == "0B" || splitedData[3].Trim().Length < 4)
                    {
                        return dataTable;
                    }
                    if (splitedData[3].Trim().Substring(2, 2) == "82")
                    {
                        obisArray = Regex.Split(splitedData[3].ToString(), splitedData[3].Substring(8, 4));
                        count = int.Parse(obisArray[0].Substring(4, 4), NumberStyles.HexNumber);
                    }
                    else if (splitedData[3].Trim().Substring(2, 2) == "81")
                    {
                        obisArray = Regex.Split(splitedData[3].ToString(), splitedData[3].Substring(6, 4));
                        count = int.Parse(obisArray[0].Substring(4, 2), NumberStyles.HexNumber);
                    }
                    else
                    {
                        obisArray = Regex.Split(splitedData[3].ToString(), splitedData[3].Substring(4, 4));
                        count = int.Parse(obisArray[0].Substring(2, 2), NumberStyles.HexNumber);
                    }
                    //string[] obisArray = Regex.Split(splitedData[3].ToString(), splitedData[3].Substring(4, 4));//old
                    //int count = int.Parse(obisArray[0].Substring(2, 2), NumberStyles.HexNumber);//old
                    string[] classValue = new string[count];
                    string[] obisValue = new string[count];
                    string[] attributeValue = new string[count];
                    for (int i = 1; i < obisArray.Length; i++)
                    {
                        DataRow row = dataTable.NewRow();
                        int j = i - 1;
                        obisArray[i] = obisArray[i].PadRight(32, '0');
                        classValue[j] = (int.Parse(obisArray[i].Substring(2, 4), NumberStyles.HexNumber)).ToString();
                        row["Class"] = classValue[j];
                        obisValue[j] = GetObis(obisArray[i].Substring(10, 12));
                        row["Obis"] = obisValue[j];
                        attributeValue[j] = (int.Parse(obisArray[i].Substring(24, 2), NumberStyles.HexNumber)).ToString();
                        row["Attribute"] = attributeValue[j];
                        dataTable.Rows.Add(row);
                    }
                }


            }
            catch
            {
                return dataTable;
            }
            return dataTable;
        }

        public bool GetObjectsDataTable(string LoadData, ref DataTable dataTable)
        {
            bool IsConvertedSuccessful = true;
            string[] obisArray = null;
            int count = 0;
            string[] splitedData = LoadData.Split(' ');
            if (splitedData.Length > 3)
            {
                if (splitedData[3] == "0100" || splitedData[3] == "01820000" || splitedData[3].Length < 4)
                {
                    return false;
                }
                else if (splitedData[3].Substring(2, 2) == "81")
                {
                    count = int.Parse(splitedData[3].Substring(4, 2), NumberStyles.HexNumber);
                    obisArray = Regex.Split(splitedData[3].ToString(), splitedData[3].Substring(6, 4));
                }
                else if (splitedData[3].Substring(2, 2) == "82")
                {
                    count = int.Parse(splitedData[3].Substring(4, 4), NumberStyles.HexNumber);
                    obisArray = Regex.Split(splitedData[3].ToString(), splitedData[3].Substring(8, 4));
                }
                else
                {
                    count = int.Parse(splitedData[3].Substring(2, 2), NumberStyles.HexNumber);
                    obisArray = Regex.Split(splitedData[3].ToString(), splitedData[3].Substring(4, 4));
                }
            }
            else
            {
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
                    obisValue[j] = GetObis(obisArray[i].PadRight(32, '0').Substring(10, 12));
                    attributeValue[j] = (int.Parse(obisArray[i].Substring(24, 2), NumberStyles.HexNumber)).ToString();
                    namevalue[j] = GetObisName(classValue[j], obisValue[j], attributeValue[j]);
                    dataTable.Columns.Add($"{classValue[j]} - {obisValue[j]} - {attributeValue[j]} - {namevalue[j]}", typeof(string));
                }
            }
            else
            {
                IsConvertedSuccessful = false;
            }
            return IsConvertedSuccessful;
        }

        public DataTable GetClassObisAttScalerListParsing(string LoadData, string profile)
        {
            // Create a DataTable
            DataTable dataTable = new DataTable();
            try
            {
                if (profile == "Load Survey Vertical" || profile == "Load Survey Vertical-AllData")
                {
                    string[] splitedData = LoadData.Split(' ');
                    //string[] obisArray = Regex.Split(splitedData[3].ToString(), splitedData[3].Substring(4, 4));
                    //int count = int.Parse(obisArray[0].Substring(2, 2), NumberStyles.HexNumber);
                    string[] obisArray;
                    int count = 0;
                    if (splitedData[3].Substring(2, 2) == "81")
                    {
                        count = int.Parse(splitedData[3].Substring(4, 2), NumberStyles.HexNumber);
                        obisArray = Regex.Split(splitedData[3].ToString(), splitedData[3].Substring(6, 4));
                    }
                    else if (splitedData[3].Substring(2, 2) == "82")
                    {
                        count = int.Parse(splitedData[3].Substring(4, 4), NumberStyles.HexNumber);
                        obisArray = Regex.Split(splitedData[3].ToString(), splitedData[3].Substring(8, 4));
                    }
                    else
                    {
                        count = int.Parse(splitedData[3].Substring(2, 2), NumberStyles.HexNumber);
                        obisArray = Regex.Split(splitedData[3].ToString(), splitedData[3].Substring(4, 4));
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
                            obisValue[j] = GetObis(obisArray[i].PadRight(32, '0').Substring(10, 12));
                            attributeValue[j] = (int.Parse(obisArray[i].Substring(24, 2), NumberStyles.HexNumber)).ToString();
                            //namevalue[j] = GetObisName(string.Empty, obisValue[j]);//old
                            namevalue[j] = GetObisName(classValue[j], obisValue[j], attributeValue[j]);
                            scalerValue[j] = obisArray[i].Substring(28);
                            dataTable.Columns.Add($"{classValue[j]} - {obisValue[j]} - {attributeValue[j]} - {namevalue[j]}", typeof(string));
                        }
                    }
                    else
                    {
                        MessageBox.Show("Count Mismatch in getting object list");
                    }
                }
                else if (profile == "Daily Energy Vertical" || profile == "Daily Energy Vertical-AllData")
                {
                    string[] splitedData = LoadData.Split(' ');
                    //string[] obisArray = Regex.Split(splitedData[3].ToString(), splitedData[3].Substring(4, 4));
                    //int count = int.Parse(obisArray[0].Substring(2, 2), NumberStyles.HexNumber);
                    string[] obisArray;
                    int count = 0;
                    if (splitedData[3].Substring(2, 2) == "81")
                    {
                        count = int.Parse(splitedData[3].Substring(4, 2), NumberStyles.HexNumber);
                        obisArray = Regex.Split(splitedData[3].ToString(), splitedData[3].Substring(6, 4));
                    }
                    else if (splitedData[3].Substring(2, 2) == "82")
                    {
                        count = int.Parse(splitedData[3].Substring(4, 4), NumberStyles.HexNumber);
                        obisArray = Regex.Split(splitedData[3].ToString(), splitedData[3].Substring(8, 4));
                    }
                    else
                    {
                        count = int.Parse(splitedData[3].Substring(2, 2), NumberStyles.HexNumber);
                        obisArray = Regex.Split(splitedData[3].ToString(), splitedData[3].Substring(4, 4));
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
                            obisValue[j] = GetObis(obisArray[i].PadRight(32, '0').Substring(10, 12));
                            attributeValue[j] = (int.Parse(obisArray[i].Substring(24, 2), NumberStyles.HexNumber)).ToString();
                            //namevalue[j] = GetObisName(string.Empty, obisValue[j]);//old
                            namevalue[j] = GetObisName(classValue[j], obisValue[j], attributeValue[j]);
                            scalerValue[j] = obisArray[i].Substring(28);
                            dataTable.Columns.Add($"{classValue[j]} - {obisValue[j]} - {attributeValue[j]} - {namevalue[j]}", typeof(string));
                        }
                    }
                    else
                    {
                        MessageBox.Show("Count Mismatch in getting object list");
                    }
                }
                else if (LoadData.Substring(0, 20) == "0007 0000636200FF 03" ||
                    LoadData.Substring(0, 20) == "0007 0000636201FF 03" ||
                    LoadData.Substring(0, 20) == "0007 0000636202FF 03" ||
                    LoadData.Substring(0, 20) == "0007 0000636203FF 03" ||
                    LoadData.Substring(0, 20) == "0007 0000636204FF 03" ||
                    LoadData.Substring(0, 20) == "0007 0000636205FF 03" ||
                    LoadData.Substring(0, 20) == "0007 0000636206FF 03" ||
                    LoadData.Substring(0, 20) == "0007 0000636281FF 03")
                {
                    string[] splitedData = LoadData.Split(' ');
                    //string[] obisArray = Regex.Split(splitedData[3].ToString(), splitedData[3].Substring(4, 4));
                    //int count = int.Parse(obisArray[0].Substring(2, 2), NumberStyles.HexNumber);
                    string[] obisArray;
                    int count = 0;
                    if (splitedData[3].Substring(2, 2) == "81")
                    {
                        count = int.Parse(splitedData[3].Substring(4, 2), NumberStyles.HexNumber);
                        obisArray = Regex.Split(splitedData[3].ToString(), splitedData[3].Substring(6, 4));
                    }
                    else if (splitedData[3].Substring(2, 2) == "82")
                    {
                        count = int.Parse(splitedData[3].Substring(4, 4), NumberStyles.HexNumber);
                        obisArray = Regex.Split(splitedData[3].ToString(), splitedData[3].Substring(8, 4));
                    }
                    else
                    {
                        count = int.Parse(splitedData[3].Substring(2, 2), NumberStyles.HexNumber);
                        obisArray = Regex.Split(splitedData[3].ToString(), splitedData[3].Substring(4, 4));
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
                            obisValue[j] = GetObis(obisArray[i].PadRight(32, '0').Substring(10, 12));
                            attributeValue[j] = (int.Parse(obisArray[i].Substring(24, 2), NumberStyles.HexNumber)).ToString();
                            //namevalue[j] = GetObisName(string.Empty, obisValue[j]);//old
                            namevalue[j] = GetObisName(classValue[j], obisValue[j], attributeValue[j]);
                            dataTable.Columns.Add($"{classValue[j]} - {obisValue[j]} - {attributeValue[j]} - {namevalue[j]}", typeof(string));
                        }
                    }
                    else
                    {
                        MessageBox.Show("Count Mismatch in getting object list");
                    }
                }
                //For Billing Profile
                else if (LoadData.Substring(0, 20) == "0007 0100620100FF 03")
                {
                    string[] splitedData = LoadData.Split(' ');
                    int count;
                    string[] obisArray;
                    if (splitedData[3].Substring(2, 2) == "81")
                    {
                        count = int.Parse(splitedData[3].Substring(4, 2), NumberStyles.HexNumber);
                        obisArray = Regex.Split(splitedData[3].ToString(), splitedData[3].Substring(6, 4));
                    }
                    else if (splitedData[3].Substring(2, 2) == "82")
                    {
                        count = int.Parse(splitedData[3].Substring(4, 4), NumberStyles.HexNumber);
                        obisArray = Regex.Split(splitedData[3].ToString(), splitedData[3].Substring(8, 4));
                    }
                    else
                    {
                        count = int.Parse(splitedData[3].Substring(2, 2), NumberStyles.HexNumber);
                        obisArray = Regex.Split(splitedData[3].ToString(), splitedData[3].Substring(4, 4));
                    }
                    string[] classValue = new string[count];
                    string[] obisValue = new string[count];
                    string[] attributeValue = new string[count];
                    string[] namevalue = new string[count];
                    string[] scalerValue = new string[count];
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
                            classValue[j] = (int.Parse(obisArray[i].Substring(2, 4), NumberStyles.HexNumber)).ToString();
                            row["Class"] = classValue[j];
                            obisValue[j] = GetObis(obisArray[i].PadRight(32, '0').Substring(10, 12));
                            row["Obis"] = obisValue[j];
                            attributeValue[j] = (int.Parse(obisArray[i].Substring(24, 2), NumberStyles.HexNumber)).ToString();
                            row["Attribute"] = attributeValue[j];
                            //namevalue[j] = GetObisName(string.Empty, obisValue[j]);//old
                            namevalue[j] = GetObisName(classValue[j], obisValue[j], attributeValue[j]);
                            row["Parameter Name"] = namevalue[j];
                            //scalerValue[j] = obisArray[i].Substring(20, 8);
                            //row["Scaler"] = scalerValue[j];
                            dataTable.Rows.Add(row);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Count Mismatch in getting object list");
                    }

                }//Billing Profile
                else if (LoadData.Substring(0, 20) == "0007 01005E5B00FF 03" || LoadData.Substring(0, 20) == "0007 00005E5B0AFF 03")//Instantaneous
                {
                    string[] splitedData = LoadData.Split(' ');
                    //string[] obisArray = Regex.Split(splitedData[3].ToString(), splitedData[3].Substring(4, 4));
                    //int count = int.Parse(obisArray[0].Substring(2, 2), NumberStyles.HexNumber);
                    string[] obisArray;
                    int count = 0;
                    if (splitedData[3].Substring(2, 2) == "81")
                    {
                        count = int.Parse(splitedData[3].Substring(4, 2), NumberStyles.HexNumber);
                        obisArray = Regex.Split(splitedData[3].ToString(), splitedData[3].Substring(6, 4));
                    }
                    else if (splitedData[3].Substring(2, 2) == "82")
                    {
                        count = int.Parse(splitedData[3].Substring(4, 4), NumberStyles.HexNumber);
                        obisArray = Regex.Split(splitedData[3].ToString(), splitedData[3].Substring(8, 4));
                    }
                    else
                    {
                        count = int.Parse(splitedData[3].Substring(2, 2), NumberStyles.HexNumber);
                        obisArray = Regex.Split(splitedData[3].ToString(), splitedData[3].Substring(4, 4));
                    }
                    string[] classValue = new string[count];
                    string[] obisValue = new string[count];
                    string[] attributeValue = new string[count];
                    string[] namevalue = new string[count];
                    string[] scalerValue = new string[count];
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
                            obisValue[j] = GetObis(obisArray[i].Substring(10, 12));
                            row["Obis"] = obisValue[j];
                            attributeValue[j] = (int.Parse(obisArray[i].Substring(24, 2), NumberStyles.HexNumber)).ToString();
                            row["Attribute"] = attributeValue[j];
                            //namevalue[j] = GetObisName(string.Empty, obisValue[j]);//old
                            namevalue[j] = GetObisName(classValue[j], obisValue[j], attributeValue[j]);
                            row["Parameter Name"] = namevalue[j];
                            //scalerValue[j] = (int.Parse(obisArray[i].Substring(28), NumberStyles.HexNumber)).ToString();
                            //row["Scaler"] = scalerValue[j];
                            row["Scaler"] = "";
                            dataTable.Rows.Add(row);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Count Mismatch in getting object list");
                    }
                }
                //For Daily Energy
                else if (profile == "Daily Energy")
                {
                    string[] splitedData = LoadData.Split(' ');
                    //string[] obisArray = Regex.Split(splitedData[3].ToString(), splitedData[3].Substring(4, 4));
                    //int count = int.Parse(obisArray[0].Substring(2, 2), NumberStyles.HexNumber);
                    string[] obisArray;
                    int count = 0;
                    if (splitedData[3].Substring(2, 2) == "81")
                    {
                        count = int.Parse(splitedData[3].Substring(4, 2), NumberStyles.HexNumber);
                        obisArray = Regex.Split(splitedData[3].ToString(), splitedData[3].Substring(6, 4));
                    }
                    else if (splitedData[3].Substring(2, 2) == "82")
                    {
                        count = int.Parse(splitedData[3].Substring(4, 4), NumberStyles.HexNumber);
                        obisArray = Regex.Split(splitedData[3].ToString(), splitedData[3].Substring(8, 4));
                    }
                    else
                    {
                        count = int.Parse(splitedData[3].Substring(2, 2), NumberStyles.HexNumber);
                        obisArray = Regex.Split(splitedData[3].ToString(), splitedData[3].Substring(4, 4));
                    }
                    string[] classValue = new string[count];
                    string[] obisValue = new string[count];
                    string[] attributeValue = new string[count];
                    string[] namevalue = new string[count];
                    string[] scalerValue = new string[count];
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
                            classValue[j] = (int.Parse(obisArray[i].Substring(2, 4), NumberStyles.HexNumber)).ToString();
                            row["Class"] = classValue[j];
                            obisValue[j] = GetObis(obisArray[i].PadRight(32, '0').Substring(10, 12));
                            row["Obis"] = obisValue[j];
                            attributeValue[j] = (int.Parse(obisArray[i].Substring(24, 2), NumberStyles.HexNumber)).ToString();
                            row["Attribute"] = attributeValue[j];
                            //namevalue[j] = GetObisName(string.Empty, obisValue[j]);//old
                            namevalue[j] = GetObisName(classValue[j], obisValue[j], attributeValue[j]);
                            row["Parameter Name"] = namevalue[j];
                            //scalerValue[j] = obisArray[i].Substring(20, 8);
                            //row["Scaler"] = scalerValue[j];
                            dataTable.Rows.Add(row);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Count Mismatch in getting object list");
                    }

                }
                //For Load Survey
                else if (profile == "Load Survey")
                {
                    string[] splitedData = LoadData.Split(' ');
                    //string[] obisArray = Regex.Split(splitedData[3].ToString(), splitedData[3].Substring(4, 4));
                    //int count = int.Parse(obisArray[0].Substring(2, 2), NumberStyles.HexNumber);
                    string[] obisArray;
                    int count = 0;
                    if (splitedData[3].Substring(2, 2) == "81")
                    {
                        count = int.Parse(splitedData[3].Substring(4, 2), NumberStyles.HexNumber);
                        obisArray = Regex.Split(splitedData[3].ToString(), splitedData[3].Substring(6, 4));
                    }
                    else if (splitedData[3].Substring(2, 2) == "82")
                    {
                        count = int.Parse(splitedData[3].Substring(4, 4), NumberStyles.HexNumber);
                        obisArray = Regex.Split(splitedData[3].ToString(), splitedData[3].Substring(8, 4));
                    }
                    else
                    {
                        count = int.Parse(splitedData[3].Substring(2, 2), NumberStyles.HexNumber);
                        obisArray = Regex.Split(splitedData[3].ToString(), splitedData[3].Substring(4, 4));
                    }
                    string[] classValue = new string[count];
                    string[] obisValue = new string[count];
                    string[] attributeValue = new string[count];
                    string[] namevalue = new string[count];
                    string[] scalerValue = new string[count];
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
                            classValue[j] = (int.Parse(obisArray[i].Substring(2, 4), NumberStyles.HexNumber)).ToString();
                            row["Class"] = classValue[j];
                            obisValue[j] = GetObis(obisArray[i].PadRight(32, '0').Substring(10, 12));
                            row["Obis"] = obisValue[j];
                            attributeValue[j] = (int.Parse(obisArray[i].Substring(24, 2), NumberStyles.HexNumber)).ToString();
                            row["Attribute"] = attributeValue[j];
                            //namevalue[j] = GetObisName(string.Empty, obisValue[j]);//old
                            namevalue[j] = GetObisName(classValue[j], obisValue[j], attributeValue[j]);
                            row["Parameter Name"] = namevalue[j];
                            //scalerValue[j] = obisArray[i].Substring(20, 8);
                            //row["Scaler"] = scalerValue[j];
                            dataTable.Rows.Add(row);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Count Mismatch in getting object list");
                    }
                }
            }
            catch
            {
                return dataTable;
            }
            return dataTable;
        }

        public DataTable GetParameterTableHorizontal(string LoadData)
        {
            // Create a DataTable
            DataTable dataTable = new DataTable();
            try
            {
                string[] splitedData = LoadData.Split(' ');
                if (splitedData.Length > 3)
                {

                    if (LoadData.Substring(0, 20) == "0007 0000636200FF 03" ||
                    LoadData.Substring(0, 20) == "0007 0000636201FF 03" ||
                    LoadData.Substring(0, 20) == "0007 0000636202FF 03" ||
                    LoadData.Substring(0, 20) == "0007 0000636203FF 03" ||
                    LoadData.Substring(0, 20) == "0007 0000636204FF 03" ||
                    LoadData.Substring(0, 20) == "0007 0000636205FF 03" ||
                    LoadData.Substring(0, 20) == "0007 0000636206FF 03" ||
                    LoadData.Substring(0, 20) == "0007 0000636281FF 03")
                    {
                        splitedData = LoadData.Split(' ');
                        int count;
                        string[] obisArray;
                        if (splitedData[3].Substring(2, 2) == "81")
                        {
                            count = int.Parse(splitedData[3].Substring(4, 2), NumberStyles.HexNumber);
                            obisArray = Regex.Split(splitedData[3].ToString(), splitedData[3].Substring(6, 4));
                        }
                        else if (splitedData[3].Substring(2, 2) == "82")
                        {
                            count = int.Parse(splitedData[3].Substring(4, 4), NumberStyles.HexNumber);
                            obisArray = Regex.Split(splitedData[3].ToString(), splitedData[3].Substring(8, 4));
                        }
                        else
                        {
                            count = int.Parse(splitedData[3].Substring(2, 2), NumberStyles.HexNumber);
                            obisArray = Regex.Split(splitedData[3].ToString(), splitedData[3].Substring(4, 4));
                        }
                        string[] classValue = new string[count];
                        string[] obisValue = new string[count];
                        string[] attributeValue = new string[count];
                        string[] namevalue = new string[count];
                        string[] scalerValue = new string[count];
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
                                classValue[j] = (int.Parse(obisArray[i].Substring(2, 4), NumberStyles.HexNumber)).ToString();
                                row["Class"] = classValue[j];
                                obisValue[j] = GetObis(obisArray[i].PadRight(32, '0').Substring(10, 12));
                                row["Obis"] = obisValue[j];
                                attributeValue[j] = (int.Parse(obisArray[i].Substring(24, 2), NumberStyles.HexNumber)).ToString();
                                row["Attribute"] = attributeValue[j];
                                //namevalue[j] = GetObisName(string.Empty, obisValue[j]);//old
                                namevalue[j] = GetObisName(classValue[j], obisValue[j], attributeValue[j]);
                                row["Parameter Name"] = namevalue[j];
                                //scalerValue[j] = obisArray[i].Substring(20, 8);
                                //row["Scaler"] = scalerValue[j];
                                dataTable.Rows.Add(row);
                            }
                        }
                        else
                        {
                            log.Error("Count Mismatch in getting object list");
                        }
                    }
                    //For Billing Profile
                    else if (LoadData.Substring(0, 20) == "0007 0100620100FF 03")
                    {
                        splitedData = LoadData.Split(' ');
                        int count;
                        string[] obisArray;
                        if (splitedData[3].Substring(2, 2) == "81")
                        {
                            count = int.Parse(splitedData[3].Substring(4, 2), NumberStyles.HexNumber);
                            obisArray = Regex.Split(splitedData[3].ToString(), splitedData[3].Substring(6, 4));
                        }
                        else if (splitedData[3].Substring(2, 2) == "82")
                        {
                            count = int.Parse(splitedData[3].Substring(4, 4), NumberStyles.HexNumber);
                            obisArray = Regex.Split(splitedData[3].ToString(), splitedData[3].Substring(8, 4));
                        }
                        else
                        {
                            count = int.Parse(splitedData[3].Substring(2, 2), NumberStyles.HexNumber);
                            obisArray = Regex.Split(splitedData[3].ToString(), splitedData[3].Substring(4, 4));
                        }
                        string[] classValue = new string[count];
                        string[] obisValue = new string[count];
                        string[] attributeValue = new string[count];
                        string[] namevalue = new string[count];
                        string[] scalerValue = new string[count];
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
                                classValue[j] = (int.Parse(obisArray[i].Substring(2, 4), NumberStyles.HexNumber)).ToString();
                                row["Class"] = classValue[j];
                                obisValue[j] = GetObis(obisArray[i].PadRight(32, '0').Substring(10, 12));
                                row["Obis"] = obisValue[j];
                                attributeValue[j] = (int.Parse(obisArray[i].Substring(24, 2), NumberStyles.HexNumber)).ToString();
                                row["Attribute"] = attributeValue[j];
                                //namevalue[j] = GetObisName(string.Empty, obisValue[j]);//old
                                namevalue[j] = GetObisName(classValue[j], obisValue[j], attributeValue[j]);
                                row["Parameter Name"] = namevalue[j];
                                //scalerValue[j] = obisArray[i].Substring(20, 8);
                                //row["Scaler"] = scalerValue[j];
                                dataTable.Rows.Add(row);
                            }
                        }
                        else
                        {
                            log.Error("Count Mismatch in getting object list");
                        }

                    }//Billing Profile
                    else if (LoadData.Substring(0, 20) == "0007 01005E5B00FF 03" || LoadData.Substring(0, 20) == "0007 00005E5B0AFF 03")//Instantaneous
                    {
                        splitedData = LoadData.Split(' ');
                        //string[] obisArray = Regex.Split(splitedData[3].ToString(), splitedData[3].Substring(4, 4));
                        //int count = int.Parse(obisArray[0].Substring(2, 2), NumberStyles.HexNumber);
                        string[] obisArray;
                        int count = 0;
                        if (splitedData[3].Substring(2, 2) == "81")
                        {
                            count = int.Parse(splitedData[3].Substring(4, 2), NumberStyles.HexNumber);
                            obisArray = Regex.Split(splitedData[3].ToString(), splitedData[3].Substring(6, 4));
                        }
                        else if (splitedData[3].Substring(2, 2) == "82")
                        {
                            count = int.Parse(splitedData[3].Substring(4, 4), NumberStyles.HexNumber);
                            obisArray = Regex.Split(splitedData[3].ToString(), splitedData[3].Substring(8, 4));
                        }
                        else
                        {
                            count = int.Parse(splitedData[3].Substring(2, 2), NumberStyles.HexNumber);
                            obisArray = Regex.Split(splitedData[3].ToString(), splitedData[3].Substring(4, 4));
                        }
                        string[] classValue = new string[count];
                        string[] obisValue = new string[count];
                        string[] attributeValue = new string[count];
                        string[] namevalue = new string[count];
                        string[] scalerValue = new string[count];
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
                                obisValue[j] = GetObis(obisArray[i].Substring(10, 12));
                                row["Obis"] = obisValue[j];
                                attributeValue[j] = (int.Parse(obisArray[i].Substring(24, 2), NumberStyles.HexNumber)).ToString();
                                row["Attribute"] = attributeValue[j];
                                //namevalue[j] = GetObisName(string.Empty, obisValue[j]);//old
                                namevalue[j] = GetObisName(classValue[j], obisValue[j], attributeValue[j]);
                                row["Parameter Name"] = namevalue[j];
                                //scalerValue[j] = (int.Parse(obisArray[i].Substring(28), NumberStyles.HexNumber)).ToString();
                                //row["Scaler"] = scalerValue[j];
                                row["Scaler"] = "";
                                dataTable.Rows.Add(row);
                            }
                        }
                        else
                        {
                            log.Error("Count Mismatch in getting object list");
                        }
                    }//Instantaneous
                    else if (LoadData.Substring(0, 20) == "0007 0100630100FF 03" || LoadData.Substring(0, 20) == "0007 0100630200FF 03")//1.0.99.1.0.255
                    {
                        splitedData = LoadData.Split(' ');
                        int count;
                        string[] obisArray;
                        if (splitedData[3].Substring(2, 2) == "81")
                        {
                            count = int.Parse(splitedData[3].Substring(4, 2), NumberStyles.HexNumber);
                            obisArray = Regex.Split(splitedData[3].ToString(), splitedData[3].Substring(6, 4));
                        }
                        else if (splitedData[3].Substring(2, 2) == "82")
                        {
                            count = int.Parse(splitedData[3].Substring(4, 4), NumberStyles.HexNumber);
                            obisArray = Regex.Split(splitedData[3].ToString(), splitedData[3].Substring(8, 4));
                        }
                        else
                        {
                            count = int.Parse(splitedData[3].Substring(2, 2), NumberStyles.HexNumber);
                            obisArray = Regex.Split(splitedData[3].ToString(), splitedData[3].Substring(4, 4));
                        }
                        string[] classValue = new string[count];
                        string[] obisValue = new string[count];
                        string[] attributeValue = new string[count];
                        string[] namevalue = new string[count];
                        string[] scalerValue = new string[count];
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
                                classValue[j] = (int.Parse(obisArray[i].Substring(2, 4), NumberStyles.HexNumber)).ToString();
                                row["Class"] = classValue[j];
                                obisValue[j] = GetObis(obisArray[i].PadRight(32, '0').Substring(10, 12));
                                row["Obis"] = obisValue[j];
                                attributeValue[j] = (int.Parse(obisArray[i].Substring(24, 2), NumberStyles.HexNumber)).ToString();
                                row["Attribute"] = attributeValue[j];
                                //namevalue[j] = GetObisName(string.Empty, obisValue[j]);//old
                                namevalue[j] = GetObisName(classValue[j], obisValue[j], attributeValue[j]);
                                row["Parameter Name"] = namevalue[j];
                                //scalerValue[j] = obisArray[i].Substring(20, 8);
                                //row["Scaler"] = scalerValue[j];
                                dataTable.Rows.Add(row);
                            }
                        }
                    }
                }
                else
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
                            obisValue[j] = GetObis(obisArray[i].Substring(10, 12));
                            row["Obis"] = obisValue[j];
                            attributeValue[j] = (int.Parse(obisArray[i].Substring(24, 2), NumberStyles.HexNumber)).ToString();
                            row["Attribute"] = attributeValue[j];
                            //namevalue[j] = GetObisName(string.Empty, obisValue[j]);//old
                            namevalue[j] = GetObisName(classValue[j], obisValue[j], attributeValue[j]);
                            row["Parameter Name"] = namevalue[j];
                            //scalerValue[j] = (int.Parse(obisArray[i].Substring(28), NumberStyles.HexNumber)).ToString();
                            //row["Scaler"] = scalerValue[j];
                            row["Scaler"] = "";
                            dataTable.Rows.Add(row);
                        }
                    }
                    else
                    {
                        log.Error("Count Mismatch in getting object list");
                    }
                }
            }
            catch
            {
                return dataTable;
            }
            return dataTable;
        }

        /// <summary>
        /// Used for DLM Format Data Conversion
        /// </summary>
        /// <param name="LoadData"></param>
        /// <returns></returns>
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
                    obisArray[i - 1] = GetObis(obisDataArray[i].PadRight(32, '0').Substring(10, 12));
                }
            }
            else
            {
                MessageBox.Show("Count Mismatch in getting object list");
            }
            return obisArray;
        }
        public string[] GetScalerProfileObjectsArray(string LoadData)
        {
            string[] obisDataArray;
            string[] splitedData = LoadData.Split(' ');
            if (splitedData.Length < 3)
            {
                if (LoadData == "0100" || LoadData == "01820000" || LoadData.Length < 4)
                {
                    log.Info($"Data not present for {LoadData.Substring(0, 20)}");
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
                    obisArray[i - 1] = $"{GetProfileValueString(obisDataArray[i].Substring(0, 6))}-{GetObis(obisDataArray[i].PadRight(32, '0').Substring(10, 12))}-{GetProfileValueString(obisDataArray[i].Substring(22, 4))}";
                }
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
            #region OLD
            //string[] ScalerArray = null;
            //string[] splitedData = LoadData.Split(' ');
            //if (splitedData.Length < 3)
            //{
            //    if (LoadData == "0100" || LoadData == "01820000" || LoadData.Length < 4)
            //    {
            //        log.Info($"Data not present for {LoadData.Substring(0, 20)}");
            //        //MessageBox.Show($"Data not present for {LoadData.Substring(0, 20)}", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            //        return ScalerArray;
            //    }
            //}
            //else
            //{
            //    if (splitedData[3] == "0100" || splitedData[3] == "01820000" || splitedData[3].Length < 4)
            //    {
            //        log.Info($"Data not present for {LoadData.Substring(0, 20)}");
            //        //MessageBox.Show($"Data not present for {LoadData.Substring(0, 20)}", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            //        return ScalerArray;
            //    }
            //    LoadData = LoadData.Split(' ')[3];
            //}
            //int i = 0, nCount = 0;
            //int j = -1, nLength = 4, k = 0;
            //string tempData = String.Empty;
            //string strMatch = string.Empty;

            //if (LoadData.Substring(2, 2) == "82")
            //{
            //    i = 8;
            //    strMatch = LoadData.Substring(8, 4);
            //    nCount = Convert.ToInt32(LoadData.Substring(4, 4), 16);
            //}
            //else if (LoadData.Substring(2, 2) == "81")
            //{
            //    i = 6;
            //    strMatch = LoadData.Substring(6, 4);
            //    nCount = Convert.ToInt32(LoadData.Substring(4, 2), 16);
            //}
            //else if (LoadData.Substring(0, 8) == "00000001")
            //{
            //    i = 24;
            //    strMatch = LoadData.Substring(24, 4);
            //}
            //else
            //{
            //    i = 4;
            //    strMatch = LoadData.Substring(4, 4);
            //    if (strMatch.Substring(2, 2) == "81")
            //    {
            //        strMatch = LoadData.Substring(4, 6);
            //        nLength = 6;
            //        ScalerArray = new string[Convert.ToInt32(LoadData.Substring(8, 2), 16)];
            //        nCount = Convert.ToInt32(LoadData.Substring(2, 2), 16);
            //    }
            //    else if (strMatch.Substring(2, 2) == "82")
            //    {
            //        strMatch = LoadData.Substring(4, 8);
            //        nLength = 8;
            //        nCount = Convert.ToInt32(LoadData.Substring(2, 2), 16);
            //        ScalerArray = new string[Convert.ToInt32(strMatch.Substring(4, 4), 16)];
            //    }
            //    else
            //    {
            //        nCount = Convert.ToInt32(LoadData.Substring(2, 2), 16);
            //        ScalerArray = new string[Convert.ToInt32(strMatch.Substring(2, 2), 16)];
            //    }
            //}
            //j = 0;
            //for (; i < LoadData.Length; i++)
            //{
            //    if (LoadData.Substring(i, nLength) == strMatch)
            //    {
            //        i += nLength;
            //        while (true)
            //        {
            //            if (i >= LoadData.Length)
            //                break;
            //            if (LoadData.Substring(i, 2) == "0F")
            //            {
            //                tempData = LoadData.Substring(i, 8);
            //                ScalerArray[k] = tempData;
            //                i += 8;
            //                k++;
            //            }
            //            else
            //            {
            //                i++;
            //            }
            //            if (LoadData.Length > (i + nLength) && LoadData.Substring(i, nLength) == strMatch)//LoadData.Substring(4, 4))
            //            {
            //                i--;
            //                break;
            //            }
            //        }
            //        j++;
            //    }

            //}



            //string[] splitedData = LoadData.Split(' ');
            //string[] ScalerDataArray = Regex.Split(splitedData[3].ToString(), splitedData[3].Substring(8, 4));
            //string[] ScalerArray = new string[ScalerDataArray.Length - 1];
            // Copy elements from original array starting from index 1
            //Array.Copy(ScalerDataArray, 1, ScalerArray, 0, ScalerDataArray.Length - 1);

            #endregion
            return ScalerArray;
        }
        public DataTable GetNameplateValueandDataTableParsing(string LoadData, DataTable obisDataTable)
        {
            // Create a DataTable
            DataTable dataTable = new DataTable();
            // Merge DataTables
            DataTable mergedDataTable = new DataTable();
            try
            {
                if (LoadData.Substring(0, 20) == "0007 0000636202FF 02")
                {
                    if (LoadData == "0100" || LoadData == "01820000" || LoadData.Length < 4)
                    {
                        log.Info($"Data not present for {LoadData.Substring(0, 20)}");
                        //MessageBox.Show($"Data not present for {LoadData.Substring(0, 20)}", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return obisDataTable;
                    }
                    string[] splitedData = LoadData.Split(' ');
                    string valueDataString = splitedData[3];
                    int count = int.Parse(valueDataString.Substring(6, 2), NumberStyles.HexNumber);
                    string[] dataValues = new string[count];
                    string[] actualValues = new string[count];

                    int nStart = 8;
                    int tempint = 0;
                    int tempValueCount = 0;
                    while (nStart < valueDataString.Length)
                    {

                        dataValues[tempValueCount] = GetProfileDataString(valueDataString, ref nStart);
                        if (obisDataTable.Rows.Count != tempValueCount)
                            tempValueCount++;
                    }
                    for (int i = 0; i < dataValues.Length; i++)
                    {
                        DataRow row = dataTable.NewRow();
                        actualValues[i] = GetProfileValueString(dataValues[i]);
                        row["Data"] = dataValues[i];
                        row["Value"] = actualValues[i];
                        dataTable.Rows.Add(row);
                    }

                    mergedDataTable = MergeDataTables(obisDataTable, dataTable);



                }
                else
                {
                    if (LoadData == "0100" || LoadData == "01820000" || LoadData.Length < 4)
                    {
                        MessageBox.Show("Data not present", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return obisDataTable;
                    }
                    dataTable.Columns.Add("Data", typeof(string));
                    dataTable.Columns.Add("Value", typeof(string));
                    string[] splitedData = LoadData.Split(' ');
                    string valueDataString = splitedData[3];
                    int count = int.Parse(valueDataString.Substring(6, 2), NumberStyles.HexNumber);
                    string[] dataValues = new string[count];
                    string[] actualValues = new string[count];
                    if (obisDataTable.Rows.Count != count)
                    {
                        MessageBox.Show("Count Mismatch in Instant/Billing Profile Attribute 2.", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                    else
                    {
                        int nStart = 8;
                        int tempint = 0;
                        int tempValueCount = 0;
                        while (nStart < valueDataString.Length)
                        {

                            dataValues[tempValueCount] = GetProfileDataString(valueDataString, ref nStart);
                            if (obisDataTable.Rows.Count != tempValueCount)
                                tempValueCount++;
                        }
                        for (int i = 0; i < dataValues.Length; i++)
                        {
                            DataRow row = dataTable.NewRow();
                            actualValues[i] = GetProfileValueString(dataValues[i]);
                            row["Data"] = dataValues[i];
                            row["Value"] = actualValues[i];
                            dataTable.Rows.Add(row);
                        }

                        mergedDataTable = MergeDataTables(obisDataTable, dataTable);
                    }
                }
            }
            catch
            {
                return obisDataTable;
            }
            return mergedDataTable;
        }
        public DataTable GetEventsValuesDataTableParsing(string LoadData, DataTable obisDataTable, string profile)
        {
            string objectName = LoadData.Substring(0, 20);
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
                if (LoadData.Substring(0, 20) == "0007 0000636200FF 02" ||
                         LoadData.Substring(0, 20) == "0007 0000636201FF 02" ||
                         LoadData.Substring(0, 20) == "0007 0000636202FF 02" ||
                         LoadData.Substring(0, 20) == "0007 0000636203FF 02" ||
                         LoadData.Substring(0, 20) == "0007 0000636204FF 02" ||
                         LoadData.Substring(0, 20) == "0007 0000636205FF 02" ||
                         LoadData.Substring(0, 20) == "0007 0000636206FF 02" ||
                         profile == "Daily Energy Vertical" ||
                         profile == "Daily Energy Vertical-AllData" ||
                         profile == "Load Survey Vertical" || profile == "Load Survey Vertical-AllData" ||
                         profile == "Mode of Relay Operation Profile" ||
                         profile == "Instantaneous" ||
                         profile == "Billing Profile" ||
                         profile == "Daily Energy" ||
                         profile == "Load Survey")
                {
                    LoadData = LoadData.Split(' ')[3];
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
                                tempData = GateDataFromString(LoadData, ref i, tempData);
                                if (!string.IsNullOrEmpty(tempData))
                                {
                                    if (k == 1)
                                    {
                                        if (profile.Contains("Events") || profile == "Mode of Relay Operation Profile")
                                            row[k] = tempData + " - " + IDToEventName(Convert.ToInt32(tempData));
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
                else if (LoadData.Substring(0, 20) == "0007 0000636201FF 02")
                {
                    string[] splitedData = LoadData.Split(' ');
                    string valueDataString = splitedData[3];
                    if (valueDataString == "0100" || valueDataString == "01820000" || valueDataString.Length < 4)
                    {
                        MessageBox.Show("Data not present", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return obisDataTable;
                    }

                    string[] valuesRows = Regex.Split(valueDataString, splitedData[3].Substring(4, 4));
                    int count = int.Parse(valuesRows[0].Substring(2, 2), NumberStyles.HexNumber);
                    if ((valuesRows.Count() - 1) != count)
                    {
                        MessageBox.Show("Count Mismatch in Attribute 2.", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return dataTable;
                    }
                    else
                    {

                        for (int i = 1; i <= count; i++)
                        {
                            DataRow row = dataTable.NewRow();
                            int nStart = 0;
                            int tempint = 0;
                            int tempValueCount = 0;
                            string tempData = string.Empty;
                            while (nStart < valuesRows[i].Length)
                            {
                                for (int j = 0; j < obisTableColumnsCount; j++)
                                {
                                    tempData = GetProfileDataString(valuesRows[i], ref nStart);
                                    if (j == 1)
                                    {
                                        string eventID = GetProfileValueString(tempData);
                                        row[j] = eventID + " - " + IDToEventName(Convert.ToInt32(eventID));
                                    }
                                    else
                                        row[j] = GetProfileValueString(tempData);
                                }

                            }
                            dataTable.Rows.Add(row);
                        }

                    }
                }
                else if (LoadData.Substring(0, 20) == "0007 0000636202FF 02")
                {
                    string[] splitedData = LoadData.Split(' ');
                    string valueDataString = splitedData[3];
                    if (valueDataString == "0100" || valueDataString == "01820000" || valueDataString.Length < 4)
                    {
                        MessageBox.Show("Data not present", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return obisDataTable;
                    }

                    int count = int.Parse(valueDataString.Substring(6, 2), NumberStyles.HexNumber);
                    string[] valuesRows = Regex.Split(valueDataString, "0203");
                    if ((valuesRows.Count() - 1) != count)
                    {
                        MessageBox.Show("Count Mismatch in Attribute 2.", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return dataTable;
                    }
                    else
                    {

                        for (int i = 1; i <= count; i++)
                        {
                            DataRow row = dataTable.NewRow();
                            int nStart = 0;
                            int tempint = 0;
                            int tempValueCount = 0;
                            string tempData = string.Empty;
                            while (nStart < valuesRows[i].Length)
                            {
                                for (int j = 0; j < obisTableColumnsCount; j++)
                                {
                                    tempData = GetProfileDataString(valuesRows[i], ref nStart);
                                    if (j == 1)
                                    {
                                        string eventID = GetProfileValueString(tempData);
                                        row[j] = eventID + " - " + IDToEventName(Convert.ToInt32(eventID));
                                    }
                                    else
                                        row[j] = GetProfileValueString(tempData);
                                }

                            }
                            dataTable.Rows.Add(row);
                            //dataGridView.DataSource = dataTable;
                            //dataGridView.Refresh();
                        }

                    }
                }
                else if (LoadData.Substring(0, 20) == "0007 0000636203FF 02")
                {
                    string[] splitedData = LoadData.Split(' ');
                    string valueDataString = splitedData[3];
                    if (valueDataString == "0100" || valueDataString == "01820000" || valueDataString.Length < 4)
                    {
                        MessageBox.Show("Data not present", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return obisDataTable;
                    }

                    string[] valuesRows = Regex.Split(valueDataString, "0203");
                    int count = int.Parse(valuesRows[0].Substring(2, 2), NumberStyles.HexNumber);
                    if ((valuesRows.Count() - 1) != count)
                    {
                        MessageBox.Show("Count Mismatch in Attribute 2.", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return dataTable;
                    }
                    else
                    {

                        for (int i = 1; i <= count; i++)
                        {
                            DataRow row = dataTable.NewRow();
                            int nStart = 0;
                            int tempint = 0;
                            int tempValueCount = 0;
                            string tempData = string.Empty;
                            while (nStart < valuesRows[i].Length)
                            {
                                for (int j = 0; j < obisTableColumnsCount; j++)
                                {
                                    tempData = GetProfileDataString(valuesRows[i], ref nStart);
                                    if (j == 1)
                                    {
                                        string eventID = GetProfileValueString(tempData);
                                        row[j] = eventID + " - " + IDToEventName(Convert.ToInt32(eventID));
                                    }
                                    else
                                        row[j] = GetProfileValueString(tempData);
                                }

                            }
                            dataTable.Rows.Add(row);
                            //dataGridView.DataSource = dataTable;
                            //dataGridView.Refresh();
                        }

                    }
                }
                else if (LoadData.Substring(0, 20) == "0007 0000636204FF 02")
                {
                    string[] splitedData = LoadData.Split(' ');
                    string valueDataString = splitedData[3];
                    if (valueDataString == "0100" || valueDataString == "01820000" || valueDataString.Length < 4)
                    {
                        MessageBox.Show("Data not present", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return obisDataTable;
                    }

                    string[] valuesRows = Regex.Split(valueDataString, "0212");
                    int count = int.Parse(valuesRows[0].Substring(2, 2), NumberStyles.HexNumber);
                    if ((valuesRows.Count() - 1) != count)
                    {
                        MessageBox.Show("Count Mismatch in Attribute 2.", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return dataTable;
                    }
                    else
                    {

                        for (int i = 1; i <= count; i++)
                        {
                            DataRow row = dataTable.NewRow();
                            int nStart = 0;
                            int tempint = 0;
                            int tempValueCount = 0;
                            string tempData = string.Empty;
                            while (nStart < valuesRows[i].Length)
                            {
                                for (int j = 0; j < obisTableColumnsCount; j++)
                                {
                                    tempData = GetProfileDataString(valuesRows[i], ref nStart);
                                    if (j == 1)
                                    {
                                        string eventID = GetProfileValueString(tempData);
                                        row[j] = eventID + " - " + IDToEventName(Convert.ToInt32(eventID));
                                    }
                                    else
                                        row[j] = GetProfileValueString(tempData);
                                }

                            }
                            dataTable.Rows.Add(row);
                            //dataGridView.DataSource = dataTable;
                            //dataGridView.Refresh();
                        }

                    }
                }
                else if (LoadData.Substring(0, 20) == "0007 0000636205FF 02")
                {
                    string[] splitedData = LoadData.Split(' ');
                    string valueDataString = splitedData[3];
                    if (valueDataString == "0100" || valueDataString == "01820000" || valueDataString.Length < 4)
                    {
                        MessageBox.Show("Data not present", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return obisDataTable;
                    }
                    string[] valuesRows = Regex.Split(valueDataString, "0203");
                    int count = int.Parse(valuesRows[0].Substring(2, 2), NumberStyles.HexNumber);
                    if ((valuesRows.Count() - 1) != count)
                    {
                        MessageBox.Show("Count Mismatch in Attribute 2.", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return dataTable;
                    }
                    else
                    {

                        for (int i = 1; i <= count; i++)
                        {
                            DataRow row = dataTable.NewRow();
                            int nStart = 0;
                            int tempint = 0;
                            int tempValueCount = 0;
                            string tempData = string.Empty;
                            while (nStart < valuesRows[i].Length)
                            {
                                for (int j = 0; j < obisTableColumnsCount; j++)
                                {
                                    tempData = GetProfileDataString(valuesRows[i], ref nStart);
                                    if (j == 1)
                                    {
                                        string eventID = GetProfileValueString(tempData);
                                        row[j] = eventID + " - " + IDToEventName(Convert.ToInt32(eventID));
                                    }
                                    else
                                        row[j] = GetProfileValueString(tempData);
                                }

                            }
                            dataTable.Rows.Add(row);
                            //dataGridView.DataSource = dataTable;
                            //dataGridView.Refresh();
                        }

                    }
                }
                else if (LoadData.Substring(0, 20) == "0007 0000636206FF 02")
                {
                    string[] splitedData = LoadData.Split(' ');
                    string valueDataString = splitedData[3];
                    if (valueDataString == "0100" || valueDataString == "01820000" || valueDataString.Length < 4)
                    {
                        MessageBox.Show("Data not present", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return obisDataTable;
                    }
                    string[] valuesRows = Regex.Split(valueDataString, "0203");
                    int count = int.Parse(valuesRows[0].Substring(2, 2), NumberStyles.HexNumber);
                    if ((valuesRows.Count() - 1) != count)
                    {
                        MessageBox.Show("Count Mismatch in Attribute 2.", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return dataTable;
                    }
                    else
                    {

                        for (int i = 1; i <= count; i++)
                        {
                            DataRow row = dataTable.NewRow();
                            int nStart = 0;
                            int tempint = 0;
                            int tempValueCount = 0;
                            string tempData = string.Empty;
                            while (nStart < valuesRows[i].Length)
                            {
                                for (int j = 0; j < obisTableColumnsCount; j++)
                                {
                                    tempData = GetProfileDataString(valuesRows[i], ref nStart);
                                    if (j == 1)
                                    {
                                        string eventID = GetProfileValueString(tempData);
                                        row[j] = eventID + " - " + IDToEventName(Convert.ToInt32(eventID));
                                    }
                                    else
                                        row[j] = GetProfileValueString(tempData);
                                }

                            }
                            dataTable.Rows.Add(row);
                            //dataGridView.DataSource = dataTable;
                            //dataGridView.Refresh();
                        }

                    }
                }
                else if (LoadData.Substring(0, 20) == "0007 0100620100FF 02")
                {
                    string[] splitedData = LoadData.Split(' ');
                    string valueDataString = splitedData[3];
                    if (valueDataString == "0100" || valueDataString == "01820000" || valueDataString.Length < 4)
                    {
                        log.Info($"Data not present for {objectName}");
                        //MessageBox.Show($"Data not present for {objectName}", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return obisDataTable;
                    }

                    string[] valuesRows = Regex.Split(valueDataString, "0277");
                    int count = int.Parse(valuesRows[0].Substring(2, 2), NumberStyles.HexNumber);
                    if ((valuesRows.Count() - 1) != count)
                    {
                        MessageBox.Show("Count Mismatch in Attribute 2.", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return dataTable;
                    }
                    else
                    {

                        for (int i = 1; i <= count; i++)
                        {
                            DataRow row = dataTable.NewRow();
                            int nStart = 0;
                            int tempint = 0;
                            int tempValueCount = 0;
                            string tempData = string.Empty;
                            while (nStart < valuesRows[i].Length)
                            {
                                for (int j = 0; j < obisTableColumnsCount; j++)
                                {
                                    tempData = GetProfileDataString(valuesRows[i], ref nStart);
                                    row[j] = GetProfileValueString(tempData);
                                }

                            }
                            dataTable.Rows.Add(row);
                            //dataGridView.DataSource = dataTable;
                            //dataGridView.Refresh();
                        }

                    }
                }
                else if (LoadData.Substring(0, 2) == "01")
                {
                    int i = 0, nCount = 0;
                    int j = -1, nLength = 4, k = 0;
                    string tempData = String.Empty;
                    string strMatch = string.Empty;
                    if (LoadData == "0100" || LoadData == "01820000" || LoadData.Length < 4)
                    {
                        log.Info($"Data not present for {profile}");
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
                                tempData = GateDataFromString(LoadData, ref i, tempData);
                                if (!string.IsNullOrEmpty(tempData))
                                {
                                    if (k == 1)
                                    {
                                        if (profile.Contains("Events") || profile == "Mode of Relay Operation Profile" ||
                                            profile.Contains("0.0.99.98.0.255") ||
                                            profile.Contains("0.0.99.98.1.255") ||
                                            profile.Contains("0.0.99.98.2.255") ||
                                            profile.Contains("0.0.99.98.3.255") ||
                                            profile.Contains("0.0.99.98.4.255") ||
                                            profile.Contains("0.0.99.98.5.255") ||
                                            profile.Contains("0.0.99.98.6.255"))
                                            row[k] = tempData + " - " + IDToEventName(Convert.ToInt32(tempData));
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
                log.Error($"There is some error in converting {profile} buffer data. " + ex.Message.ToString());
                //MessageBox.Show($"There is some error in converting {profile} buffer data", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            return dataTable;
        }
        public DataTable GetEventsLSDEValueDataTableParsing(string LoadData, DataTable obisDataTable)
        {
            string objectName = LoadData.Substring(0, 20);
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
                string[] splitedData = LoadData.Split(' ');
                if (splitedData.Length > 3)
                {
                    LoadData = splitedData[3];
                }
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
                            tempData = GateDataFromString(LoadData, ref i, tempData);
                            if (!string.IsNullOrEmpty(tempData))
                            {
                                if (k == 1)
                                {
                                    if (obisDataTable.Columns[1].ColumnName.Contains("Event"))
                                        row[k] = tempData + " - " + IDToEventName(Convert.ToInt32(tempData));
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
            catch (Exception ex)
            {
                log.Error($"There is some error in converting buffer data. " + ex.Message.ToString());
            }
            return dataTable;
        }
        public DataTable GetBillingValuesDataTableParsing(string LoadData, DataTable obisDataTable)
        {
            // Create a DataTable
            string objectName = LoadData.Substring(0, 20);
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
                    scalerMultiplyFactor[n] = (string)ScalerhshTable[scalerDataArray[n].Substring(2, 2)];
                }
                else
                    scalerMultiplyFactor[n] = "";

            }
            string[] splitedData = LoadData.Split(' ');
            if (splitedData.Length > 3)
                LoadData = LoadData.Split(' ')[3];
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
                        //tempData = GetProfileDataString(LoadData, ref i);
                        //BY Ys 
                        string tag = LoadData.Substring(i, 2);
                        if (tag == "01")
                        {
                            int arrayStart = i;
                            SkipDataByTag(LoadData, ref i);
                            int arrayend = i;
                            string hexStringArray = LoadData.Substring(arrayStart, arrayend - arrayStart);
                            actualValues[k] = hexStringArray;
                        }
                        else
                        {
                            tempData = GetProfileDataString(LoadData, ref i);
                            if (!string.IsNullOrEmpty(tempData))
                            {
                                dataValues[k] = tempData;
                                string scaler = obisDataTable.Rows[k]["Scaler"].ToString().Trim();

                                if (string.IsNullOrEmpty(scaler))
                                {
                                    actualValues[k] = GetProfileValueString(dataValues[k]);
                                }
                                else
                                {
                                    if (string.IsNullOrEmpty((string)UnithshTable[scaler.Trim().Substring(6, 2)]) && (string)UnithshTable[scaler.Trim().Substring(6, 2)] == "1")
                                    {
                                        actualDataValue = GetProfileValueString(dataValues[k]);
                                        actualValues[k] = actualDataValue;
                                    }
                                    else
                                    {
                                        try
                                        {
                                            actualDataValue = GetProfileValueString(dataValues[k]);
                                            double scaledValue = Convert.ToDouble((string)ScalerhshTable[scaler.Trim().Substring(2, 2)]) * Convert.ToDouble(actualDataValue);
                                            actualValues[k] = scaledValue.ToString();
                                        }
                                        catch (Exception ex)
                                        {
                                            actualValues[k] = actualDataValue;
                                        }

                                    }
                                }
                                #region old
                                //if (string.IsNullOrEmpty((string)obisDataTable.Rows[k]["Scaler"].ToString()))
                                //{
                                //    actualValues[k] = GetProfileValueString(dataValues[k]);
                                //}
                                //else
                                //{
                                //    actualDataValue = GetProfileValueString(dataValues[k]);
                                //    if (string.IsNullOrEmpty((string)UnithshTable[(string)obisDataTable.Rows[k]["Scaler"].ToString().Substring(6, 2)]) || (string)UnithshTable[(string)obisDataTable.Rows[k]["Scaler"].ToString().Substring(6, 2)] == "1")
                                //    {
                                //        actualValues[k] = actualDataValue;
                                //    }
                                //    else
                                //    {
                                //        double scaledValue = Convert.ToDouble((string)ScalerhshTable[(string)obisDataTable.Rows[k]["Scaler"].ToString().Substring(2, 2)]) * Convert.ToDouble(actualDataValue);
                                //        actualValues[k] = scaledValue.ToString();
                                //    }
                                //}
                                #endregion
                                k++;
                            }
                        }
                        //end by YS

                    }


                    //for (int l = 0; l < dataValues.Length; l++)
                    //{
                    //    if (string.IsNullOrEmpty((string)obisDataTable.Rows[l]["Scaler"].ToString()))
                    //    {
                    //        actualValues[l] = GetProfileValueString(dataValues[l]);
                    //    }
                    //    else
                    //    {
                    //        string actualDataValue = GetProfileValueString(dataValues[l]);
                    //        if (string.IsNullOrEmpty((string)UnithshTable[(string)obisDataTable.Rows[l]["Scaler"].ToString().Substring(6, 2)]) || (string)UnithshTable[(string)obisDataTable.Rows[l]["Scaler"].ToString().Substring(6, 2)] == "1")
                    //        {
                    //            actualValues[l] = actualDataValue;
                    //        }
                    //        else 
                    //        {
                    //            double scaledValue = Convert.ToDouble((string)ScalerhshTable[(string)obisDataTable.Rows[l]["Scaler"].ToString().Substring(2, 2)]) * Convert.ToDouble(actualDataValue);
                    //            actualValues[l] = scaledValue.ToString();
                    //        }


                    //        //actualValues[l] = GetProfileValueString(dataValues[l]);
                    //        //actualValues[j] = GetProfileValueString(dataValues[j]) + " " + (string)UnithshTable[(string)obisDataTable.Rows[j]["Scaler"].ToString().Substring(6, 2)];
                    //        //string temp=(string)UnithshTable[(string)obisDataTable.Rows[j]["Scaler"].ToString().Substring(6,2)];
                    //    }
                    //}


                    // Join the array as a new column to the existing DataTable
                    //JoinArrayAsColumn(obisDataTable, $"Entry {j+1} Data", dataValues);
                    //JoinArrayAsColumn(obisDataTable, $"Entry {j+1} Value", actualValues);
                    JoinDataAndValuesArrayAsColumn(obisDataTable, $"Entry {j + 1} Data", $"Entry {j + 1} Value", dataValues, actualValues);
                    j++;
                    i--;
                    k = 0;
                    //if (LoadData.Length > (i + nLength) && LoadData.Substring(i, nLength) == strMatch)//LoadData.Substring(4, 4))
                    //{
                    //    i--;
                    //}
                }


            }
            return obisDataTable;

        }
        public void SkipDataByTag(string data, ref int index)
        {
            if (index + 2 > data.Length) return;

            string tag = data.Substring(index, 2);
            switch (tag)
            {
                case "01":
                    index += 2;
                    int arrayCount = Convert.ToInt32(data.Substring(index, 2), 16);
                    index += 2;
                    for (int i = 0; i < arrayCount; i++)
                    {
                        SkipDataByTag(data, ref index);
                    }
                    break;

                case "02":
                    index += 2;
                    int structCount = Convert.ToInt32(data.Substring(index, 2), 16);
                    index += 2;
                    for (int i = 0; i < structCount; i++)
                    {
                        SkipDataByTag(data, ref index);
                    }
                    break;

                default:
                    GetProfileDataString(data, ref index);
                    break;
            }
        }
        public string GetStructureDataString(string strData, ref int nStart, StringBuilder temp)
        {
            if (strData.Substring(nStart, 2) == "02")
            {
                int numberofValues = Convert.ToInt32(strData.Substring(nStart + 2, 2), 16);
                nStart += 4;
                temp.Append(strData.Substring(nStart - 4, 4));
                for (int i = 0; i < numberofValues; i++)
                {
                    int initialnStart = nStart;
                    switch (strData.Substring(nStart, 2))
                    {
                        case "02":
                            {
                                temp.Append(strData.Substring(initialnStart, nStart - initialnStart));
                                GetStructureDataString(strData, ref nStart, temp);
                                break;
                            }
                        case "09":
                        case "0A":
                            nStart += (4 + Convert.ToInt32(strData.Substring(nStart + 2, 2), 16) * 2);
                            temp.Append(strData.Substring(initialnStart, nStart - initialnStart));
                            break;
                        case "12":
                        case "10":
                            nStart += 6;
                            temp.Append(strData.Substring(initialnStart, nStart - initialnStart));
                            break;
                        case "06":
                        case "17":
                        case "05":
                            nStart += 10;
                            temp.Append(strData.Substring(initialnStart, nStart - initialnStart));
                            break;
                        case "11":
                        case "03":
                        case "0F":
                        case "16":
                            nStart += 4;
                            temp.Append(strData.Substring(initialnStart, nStart - initialnStart));
                            break;
                        case "04":
                            int bitCount;
                            if (strData.Substring(nStart + 2, 2) == "82")
                            {
                                bitCount = int.Parse(strData.Substring(nStart + 4, 4), NumberStyles.HexNumber) / 4;
                                temp.Append(strData.Substring(nStart, 8 + bitCount));
                                nStart += (8 + bitCount);
                            }
                            else
                            {
                                bitCount = int.Parse(strData.Substring(nStart + 2, 2), NumberStyles.HexNumber) / 4;
                                temp.Append(strData.Substring(nStart, 4 + bitCount));
                                nStart += (4 + bitCount);
                            }
                            break;
                        case "18":
                            temp.Append(strData.Substring(nStart, 18));
                            nStart += 18;
                            break;

                    }
                }
            }
            //tempString += strData.Substring(initialnStart, nStart - initialnStart);
            return temp.ToString();
        }
        public string GetProfileDataString(string strData, ref int nStart)
        {
            string str = string.Empty;
            if (strData.Substring(nStart, 2) == "06" || strData.Substring(nStart, 2) == "17" || strData.Substring(nStart, 2) == "05")
            {
                str = strData.Substring(nStart, 10);
                nStart += 10;
            }
            else if (strData.Substring(nStart, 2) == "18")
            {
                str = strData.Substring(nStart, 18);
                nStart += 18;
            }
            else if (strData.Substring(nStart, 2) == "12" || strData.Substring(nStart, 2) == "10")
            {
                str = strData.Substring(nStart, 6);
                nStart += 6;
            }
            else if (strData.Substring(nStart, 2) == "11" || strData.Substring(nStart, 2) == "03" || strData.Substring(nStart, 2) == "0F" || strData.Substring(nStart, 2) == "16")
            {
                str = strData.Substring(nStart, 4);
                nStart += 4;
            }
            else if (strData.Substring(nStart, 2) == "15" || strData.Substring(nStart, 2) == "14")
            {
                str = strData.Substring(nStart, 18);
                nStart += 18;
            }
            /*
            else if (strData.Substring(nStart, 2) == "09")
            {
                str = strData.Substring(nStart, int.Parse(strData.Substring(nStart + 2, 2), NumberStyles.HexNumber) * 2 + 4);
                nStart += int.Parse(strData.Substring(nStart + 2, 2), NumberStyles.HexNumber) * 2 + 4;
            }
           */
            else if (strData.Substring(nStart, 2) == "04")
            {
                int bitCount;
                if (strData.Substring(nStart + 2, 2) == "82")
                {
                    bitCount = int.Parse(strData.Substring(nStart + 4, 4), NumberStyles.HexNumber) / 4;
                    str = strData.Substring(nStart, 8 + bitCount);
                    nStart += (8 + bitCount);
                }
                else
                {
                    bitCount = int.Parse(strData.Substring(nStart + 2, 2), NumberStyles.HexNumber) / 4;
                    str = strData.Substring(nStart, 4 + bitCount);
                    nStart += (4 + bitCount);
                }
                //nStart += (4 + bitCount);
            }
            /*
            //else if (strData.Substring(nStart, 2) == "09")
            //{
            //    if (strData.Substring(nStart + 26, 2) == "00" || strData.Substring(nStart + 26, 2) == "FF")
            //    {
            //        str = Getdate(strData.Substring(nStart + 4, 24), 0, false);
            //        if (IsDate(str) == true)
            //            dt = DateTime.ParseExact(str, "dd/MM/yyyy HH:mm:ss", provider, DateTimeStyles.AssumeLocal);
            //        nStart += 28;
            //    }
            //    else if (strData.Substring(nStart, 4) == "090C")
            //    {
            //        str = HexStringToAscii(strData.Substring(nStart + 4, 28));
            //        nStart += 32;
            //    }
            //    else if (strData.Substring(nStart + 2, 2) != "0C")
            //    {
            //        nStart += 2;
            //    }

            //}
            */
            //BY AAC
            else if (strData.Substring(nStart, 2) == "0A" || strData.Substring(nStart, 2) == "09")//For Octet String and Visible String
            {
                str = strData.Substring(nStart, 4 + (Convert.ToInt32(strData.Substring(nStart + 2, 2), 16) * 2));
                nStart += (4 + Convert.ToInt32(strData.Substring(nStart + 2, 2), 16) * 2);
                /*
                string sizeofSerialNumber = strData.Substring(nStart + 2, 2);            
                int lengthofString = Convert.ToInt32(strData.Substring(nStart + 2, 2), 16);
                if (lengthofString == 0)
                {
                    str = strData.Substring(nStart, 4);
                    nStart += 4;
                }
                else
                {
                    str = strData.Substring(nStart, 4 + (lengthofString * 2));
                    nStart += (4 + (lengthofString * 2));
                }
                */
            }
            /*
            //else if (strData.Substring(nStart, 2) == "0A" || strData.Substring(nStart, 2) == "09")
            //{
            //  str = strData.Substring(nStart, int.Parse(strData.Substring(nStart + 2, 2), NumberStyles.HexNumber) * 2 + 4);
            //  nStart += int.Parse(strData.Substring(nStart + 2, 2), NumberStyles.HexNumber) * 2 + 4;
            //}
            //else if (strData.Substring(nStart, 4) == "0202")
            //{
            //    str = strData.Substring(nStart, 20);
            //    nStart += 20;
            //}
            //else if (strData.Substring(nStart, 4) == "0209" || strData.Substring(nStart, 4) == "0208")
            //{
            //    if (strData.Substring(nStart + 4, 2) == "0A")
            //    {
            //        int noofParameters = Convert.ToInt32(strData.Substring(nStart + 2, 2), 16);
            //        Int32 totalByteCount = 4;
            //        Int32 tempNStart = nStart + 4;
            //        for (int index = 1; index <= noofParameters; index++)
            //        {
            //            Int32 visibalLength = 0;
            //            visibalLength = Convert.ToInt32(strData.Substring(tempNStart + 2, 2), 16);
            //            totalByteCount += (4 + (visibalLength * 2));
            //            tempNStart += (4 + (visibalLength * 2));
            //        }
            //        str = strData.Substring(nStart, totalByteCount);
            //        nStart = tempNStart;

            //    }
            //}
            //else if (strData.Substring(nStart, 4) == "020A")
            //{
            //    str = strData.Substring(nStart, 56);
            //    nStart += 56;
            //}
            */
            else if (strData.Substring(nStart, 2) == "02")
            {
                StringBuilder temp = new StringBuilder();
                str = GetStructureDataString(strData, ref nStart, temp);
            }
            // str = strData.Substring(initialnStart, nStart - initialnStart);
            //nStart += 78;
            else if (strData.Substring(nStart, 2) == "01")
            {
                int numberofValues = Convert.ToInt32(strData.Substring(nStart + 2, 2), 16);
                int initialnStart = nStart;
                //By YS For Push Billing profile
                if (numberofValues == 1)
                    str = strData.Substring(initialnStart); //No of elements in array is 1 return its complete value
                else
                {
                    nStart += 4;
                    for (int i = 0; i < numberofValues; i++)
                    {
                        string innerValue = GetProfileDataString(strData, ref nStart);
                    }
                    str = strData.Substring(initialnStart, nStart - initialnStart);
                }
            }
            return str;
        }

        public string GetProfileValueString(string strData)
        {
            DateTime dt = DateTime.Now;
            string tmpStr = string.Empty;
            int nStart = 0;
            if (strData.Length < 4)
                return tmpStr;
            if (strData.Substring(nStart, 4) == "0906")
            {
                if ((strData.Length - nStart) < 36)
                {
                    string templocalstring = strData.Substring(nStart, (strData.Length - nStart));
                    templocalstring.PadRight(36, '0');
                    tmpStr = GetObis(templocalstring.PadRight(36, '0').Substring(4, 12));
                    nStart += 36;
                    return tmpStr;
                }
                else
                {
                    tmpStr = GetObis(strData.Substring(nStart + 4, 32));
                    nStart += 36;
                    return tmpStr;
                }
            }
            //BY AAC
            else if (strData.Substring(nStart, 2) == "0A")//For String
            {
                string sizeofSerialNumber = strData.Substring(nStart + 2, 2);
                int lengthofString = Convert.ToInt32(strData.Substring(nStart + 2, 2), 16);
                if (lengthofString == 0)
                {
                    tmpStr = "";
                    nStart += 4;
                }
                else
                {
                    tmpStr = HexString2Ascii(strData.Substring(nStart + 4, lengthofString * 2));
                    nStart += (4 + (lengthofString * 2));
                }
            }
            //END AAC
            else if (strData.Substring(nStart, 2) == "09")
            {
                if (strData.Substring(nStart, 4) == "090C")
                {
                    if (strData.Substring(nStart + 26, 2) == "00" || strData.Substring(nStart + 26, 2) == "FF")
                    {
                        tmpStr = Getdate(strData.Substring(nStart + 4, 24), 0, false);
                        if (IsDate(tmpStr) == true)
                        {
                            dt = DateTime.ParseExact(tmpStr, "dd/MM/yyyy HH:mm:ss", provider, DateTimeStyles.AssumeLocal);
                            tmpStr = dt.ToString("dd/MM/yyyy hh:mm:ss tt");
                        }
                        nStart += 28;
                    }
                    else if (strData.Substring(nStart, 4) == "090C")
                    {
                        tmpStr = HexStringToAscii(strData.Substring(nStart + 4, 24));
                        nStart += 28;
                    }
                    else if (strData.Substring(nStart + 2, 2) != "0C")
                    {
                        nStart += 2;
                    }
                }
                else if (strData.Substring(nStart, 4) == "0904")//project specific Adani Project (Network Information Parameters (For GPRS NIC) => 0.0.96.99.1.255 1/2)/*
                                                                //1st Byte will be RSRP (Unsigned8)
                                                                //2nd Byte will be RSRQ(Unsigned8)
                                                                //3rd Byte will be SNR(Unsigned8)
                                                                //4th Byte will be RSSI(Unsigned8) */
                {
                    tmpStr = $"RSRP({strData.Substring(nStart + 4, 2)}) RSRQ({strData.Substring(nStart + 4 + 2, 2)}) SNR({strData.Substring(nStart + 4 + 4, 2)}) RSSI({strData.Substring(nStart + 4 + 6, 2)})";
                    nStart += 12;
                }
                //else if (strData.Substring(nStart, 4) == "0905")
                //{
                //    tmpStr = int.Parse(strData.Substring(nStart + 8, 2), NumberStyles.HexNumber).ToString("00");
                //    tmpStr += "/";
                //    tmpStr += int.Parse(strData.Substring(nStart + 4, 4), NumberStyles.HexNumber).ToString("0000");
                //    //tmpStr += HexString2Ascii(strData.Substring(nStart + 4, 4));
                //    nStart += 14;
                //}
                //BY AAC for project CIQD183
                else if (strData.Substring(nStart, 4) == "091F" || strData.Substring(nStart, 4) == "091C")
                {
                    string sizeofname = strData.Substring(nStart + 2, 2);
                    int lengthofString = Convert.ToInt32(strData.Substring(nStart + 2, 2), 16);
                    if (lengthofString == 0)
                    {
                        tmpStr = "";
                        nStart += 4;
                    }
                    else
                    {
                        tmpStr = HexString2Ascii(strData.Substring(nStart + 4, lengthofString * 2));
                        nStart += (4 + (lengthofString * 2));
                    }
                }
                //END AAC
                else if (strData.Substring(nStart).Length > 26)
                {
                    if (strData.Substring(nStart + 26, 2) == "00" || strData.Substring(nStart + 26, 2) == "FF")
                    {
                        tmpStr = Getdate(strData.Substring(nStart + 4, 24), 0, false);
                        if (IsDate(tmpStr) == true)
                        {
                            dt = DateTime.ParseExact(tmpStr, "dd/MM/yyyy HH:mm:ss", provider, DateTimeStyles.AssumeLocal);
                            tmpStr = dt.ToString("dd/MM/yyyy hh:mm:ss tt");
                        }
                        nStart += 28;
                    }
                    else if (strData.Substring(nStart, 4) == "090C")
                    {
                        tmpStr = HexStringToAscii(strData.Substring(nStart + 4, 24));
                        nStart += 28;
                    }
                    else
                    {
                        string sizeofname = strData.Substring(nStart + 2, 2);
                        int lengthofString = Convert.ToInt32(strData.Substring(nStart + 2, 2), 16);
                        if (lengthofString == 0)
                        {
                            tmpStr = "";
                            nStart += 4;
                        }
                        else
                        {
                            tmpStr = HexString2Ascii(strData.Substring(nStart + 4, lengthofString * 2));
                            nStart += (4 + (lengthofString * 2));
                        }
                    }
                    //else if (strData.Substring(nStart + 2, 2) != "0C")
                    //{
                    //    nStart += 2;
                    //}
                }
                else
                {
                    string sizeofname = strData.Substring(nStart + 2, 2);
                    int lengthofString = Convert.ToInt32(strData.Substring(nStart + 2, 2), 16);
                    if (lengthofString == 0)
                    {
                        tmpStr = "";
                        nStart += 4;
                    }
                    else
                    {
                        tmpStr = HexString2Ascii(strData.Substring(nStart + 4, lengthofString * 2));
                        nStart += (4 + (lengthofString * 2));
                    }
                }
            }
            else if (strData.Substring(nStart, 2) == "06")
            {
                tmpStr = UInt32.Parse(strData.Substring(nStart + 2, 8), NumberStyles.HexNumber).ToString();
                nStart += 10;
            }
            else if (strData.Substring(nStart, 2) == "05")
            {
                tmpStr = Int32.Parse(strData.Substring(nStart + 2, 8), NumberStyles.HexNumber).ToString();
                nStart += 10;
            }
            else if (strData.Substring(nStart, 2) == "15")
            {
                tmpStr = Int64.Parse(strData.Substring(nStart + 2, 16), NumberStyles.HexNumber).ToString();
                nStart += 18;
            }
            else if (strData.Substring(nStart, 2) == "14")
            {
                tmpStr = Int64.Parse(strData.Substring(nStart + 2, 16), NumberStyles.HexNumber).ToString();
                nStart += 18;
            }
            else if (strData.Substring(nStart, 2) == "12")
            {
                tmpStr = UInt16.Parse(strData.Substring(nStart + 2, 4), NumberStyles.HexNumber).ToString();
                int eventIDinInt = Convert.ToInt32(tmpStr);
                //tmpStr ="("+tmpStr+") "+IDToEventName(eventIDinInt);
                nStart += 6;
            }
            else if (strData.Substring(nStart, 2) == "10")
            {
                tmpStr = Int16.Parse(strData.Substring(nStart + 2, 4), NumberStyles.HexNumber).ToString();
                nStart += 6;
            }
            else if (strData.Substring(nStart, 2) == "11")
            {
                tmpStr = Int16.Parse(strData.Substring(nStart + 2, 2), NumberStyles.HexNumber).ToString();
                nStart += 4;
            }
            /* else if (strData.Substring(nStart, 2) == "04")
             {
                 string tmpHexStr;
                 string length = strData.Substring(nStart + 2, 2);
                 if (strData.Substring(nStart + 2, 2) == "82")
                 {
                     tmpHexStr = strData.Substring(nStart + 8, int.Parse(strData.Substring(nStart + 4, 4), NumberStyles.HexNumber) / 4);
                     tmpStr = ConvertHexToBinary(tmpHexStr);
                     nStart += 8 + int.Parse(strData.Substring(nStart + 4, 4), NumberStyles.HexNumber) / 4;
                 }
                 else
                 {
                     tmpHexStr = strData.Substring(nStart + 4, int.Parse(strData.Substring(nStart + 2, 2), NumberStyles.HexNumber) / 4);
                     tmpStr = ConvertHexToBinary(tmpHexStr);
                     nStart += 4 + int.Parse(strData.Substring(nStart + 2, 2), NumberStyles.HexNumber) / 4;
                 }
                 if (length == "20")
                     tmpStr = MeterHeath(tmpStr);

             } */
            else if (strData.Substring(nStart, 2) == "17")
            {
                //tmpStr = ConvertToDataType17(strData.Substring(nStart + 2, 8)).ToString();
                tmpStr = ConvertToFloat(strData.Substring(nStart + 2, 8)).ToString();// strData.Substring(nStart + 4, int.Parse(strData.Substring(nStart + 2, 2), NumberStyles.HexNumber) / 4);
                nStart += 10;
            }
            else if (strData.Substring(nStart, 2) == "0F")
            {
                tmpStr = UInt32.Parse(strData.Substring(nStart + 2, 2), NumberStyles.HexNumber).ToString();
                nStart += 4;
            }
            else if (strData.Substring(nStart, 2) == "00")
            {
                //dt = dt.AddMinutes(LSIP);
                //tmpStr = dt.ToString("dd/MM/yyyy HH:mm:ss");
                tmpStr = "00 - null value";
                nStart += 2;
            }
            else if (strData.Substring(nStart, 2) == "01")
            {
                tmpStr = strData;
                return tmpStr;
                //if (strData == "0100")
                //{
                //    tmpStr = strData;
                //    return tmpStr;
                //}
                //if (strData.Length >= 8)
                //{
                //    if (strData.Substring(nStart, 14) == "01010202110101")
                //    {
                //        nStart += 14;
                //        int nCnt = Convert.ToInt32(strData.Substring(nStart, 2), 16);
                //        //nCnt = 3;
                //        tmpStr = strData.Substring(nStart + 2, nCnt * 38);
                //        nStart = nStart + 2 + nCnt * 38;
                //    }
                //    else
                //    {
                //        tmpStr = strData;
                //        nStart += 2;
                //    }
                //}
            }
            else if (strData.Substring(nStart, 2) == "02")
            {
                //strData = "02090A0830444441314532310A032D38320A032D31300A032D35350A01390A0A4C54452042414E4420310A04303230320A0534303437300A00";
                int lengthofString = Convert.ToInt32(strData.Substring(nStart + 2, 2), 16);
                string tempOctatStr = strData.Substring(nStart + 4);
                if (strData.Substring(4, 2) == "0A")
                {
                    //List<string> Parameters = new List<string>();
                    Int32 totalByteCount = 0;
                    Int32 tempNStart = 0;
                    for (int i = 1; i <= lengthofString; i++)
                    {
                        Int32 visibalLength = 0;
                        visibalLength = Convert.ToInt32(tempOctatStr.Substring(tempNStart + 2, 2), 16);
                        totalByteCount = (visibalLength * 2);
                        if (visibalLength == 0)
                            //Parameters.Add("");
                            tmpStr += " ";
                        else
                            //Parameters.Add(tempOctatStr.Substring(tempNStart + 4, totalByteCount));
                            tmpStr += (tempOctatStr.Substring(tempNStart + 4, totalByteCount) + "-");
                        tempNStart += (4 + (visibalLength * 2));
                    }
                    //string[] octatStringArray = Regex.Split(tempOctatStr, "0A");
                    //int bytesCount = 0;
                    //for (int i = 1; i <= lengthofString; i++)
                    //{
                    //    int tempCount = 0;
                    //    tempCount = Convert.ToInt32(octatStringArray[i].Substring(0, 2), 16);
                    //    if (tempCount == 0)
                    //    {
                    //        tmpStr += " ";

                    //    }
                    //    else
                    //        tmpStr += HexString2Ascii(octatStringArray[i].Substring(2)) + "-";
                    //}
                }
                else
                {
                    for (int j = 0; j < lengthofString; j++)
                    {
                        tmpStr += GetProfileValueString(tempOctatStr) + " ";
                        switch (tempOctatStr.Substring(0, 2))
                        {
                            case "06":
                                tempOctatStr = tempOctatStr.Substring(10);
                                break;
                            case "11":
                                tempOctatStr = tempOctatStr.Substring(4);
                                break;
                            case "09":
                                if (tempOctatStr.Substring(0, 4) == "090C")
                                    tempOctatStr = tempOctatStr.Substring(28);
                                else
                                {
                                    string sizeofname = tempOctatStr.Substring(nStart + 2, 2);
                                    int lengthofOctetString = Convert.ToInt32(tempOctatStr.Substring(nStart + 2, 2), 16);
                                    if (lengthofOctetString == 0)
                                    {
                                        //tmpStr = " ";
                                        //nStart += 4;
                                        tempOctatStr = tempOctatStr.Substring(4);
                                    }
                                    else
                                    {
                                        tempOctatStr = tempOctatStr.Substring((4 + (lengthofOctetString * 2)));
                                        //tmpStr = HexString2Ascii(strData.Substring(nStart + 4, lengthofOctetString * 2));
                                        //nStart += (4 + (lengthofOctetString * 2));
                                    }
                                }
                                break;
                            case "12":
                                tempOctatStr = tempOctatStr.Substring(6);
                                break;
                            case "0F":
                                tempOctatStr = tempOctatStr.Substring(4);
                                break;
                            case "16":
                                tempOctatStr = tempOctatStr.Substring(4);
                                break;
                            case "04":
                                tempOctatStr = tempOctatStr.Substring((int.Parse(tempOctatStr.Substring(2, 2), NumberStyles.HexNumber) / 4) + 4);
                                break;
                            case "0A":
                                tempOctatStr = tempOctatStr.Substring((Convert.ToInt32(tempOctatStr.Substring(2, 2), 16) * 2) + 4);
                                //tempOctatStr = tempOctatStr.Substring((int.Parse(tempOctatStr.Substring(2, 2), NumberStyles.HexNumber)) + 4);
                                break;

                        }
                    }
                }
                //str = strData.Substring(nStart, bytesCount + (lengthofString * 4) + 4);
                //nStart += str.Length;
                //nStart += 2;
            }
            //BY AAC
            else if (strData.Substring(nStart, 2) == "03")
            {
                tmpStr = Int16.Parse(strData.Substring(nStart + 2, 2), NumberStyles.HexNumber).ToString();
                nStart += 4;
            }
            else if (strData.Substring(nStart, 2) == "08")
            {
                //tmpStr = Int16.Parse(strData.Substring(nStart + 2, 22), NumberStyles.HexNumber).ToString();
                nStart += 34;
            }
            //END AAC
            else if (strData.Substring(nStart, 2) == "16")
            {
                tmpStr = UInt32.Parse(strData.Substring(nStart + 2, 2), NumberStyles.HexNumber).ToString();
                nStart += 4;
            }
            else if (strData.Substring(nStart, 2) == "21")
            {
                // tmpStr = UInt32.Parse(strData.Substring(nStart + 2, 2), NumberStyles.HexNumber).ToString();
                nStart += 2;
            }
            else if (strData.Substring(nStart, 2) == "23")
            {
                // tmpStr = UInt32.Parse(strData.Substring(nStart + 2, 2), NumberStyles.HexNumber).ToString();
                nStart += 2;
            }
            else if (strData.Substring(nStart, 2) == "FF")
            {
                // tmpStr = UInt32.Parse(strData.Substring(nStart + 2, 2), NumberStyles.HexNumber).ToString();
                nStart += 2;
            }
            //else if (strData.Substring(nStart, 2) == "04")
            //{
            //    int bitCount;
            //    if (strData.Substring(nStart + 2, 2) == "82")
            //    {
            //        bitCount = int.Parse(strData.Substring(nStart + 4, 4), NumberStyles.HexNumber) / 4;
            //        tmpStr = strData.Substring(nStart, 8 + bitCount);
            //        nStart += (8 + bitCount);
            //    }
            //    else
            //    {
            //        bitCount = int.Parse(strData.Substring(nStart + 2, 2), NumberStyles.HexNumber) / 4;
            //        tmpStr = strData.Substring(nStart, 4 + bitCount);
            //        nStart += (4 + bitCount);
            //    }

            //    //nStart += (4 + bitCount);
            //}
            else if (strData.Substring(nStart, 2) == "04")
            {
                string tmpHexStr;
                string length = strData.Substring(nStart + 2, 2);
                if (strData.Substring(nStart + 2, 2) == "82")
                {
                    tmpHexStr = strData.Substring(nStart + 8, int.Parse(strData.Substring(nStart + 4, 4), NumberStyles.HexNumber) / 4);
                    tmpStr = ConvertHexToBinary(tmpHexStr);
                    nStart += 8 + int.Parse(strData.Substring(nStart + 4, 4), NumberStyles.HexNumber) / 4;
                }
                else
                {
                    tmpHexStr = strData.Substring(nStart + 4, int.Parse(strData.Substring(nStart + 2, 2), NumberStyles.HexNumber) / 4);
                    tmpStr = ConvertHexToBinary(tmpHexStr);
                    nStart += 4 + int.Parse(strData.Substring(nStart + 2, 2), NumberStyles.HexNumber) / 4;
                }
                if (length == "20")
                    tmpStr = MeterHeath(tmpStr);
                else if (tmpStr.Length == 128)
                    tmpStr = ESWFStatus(tmpStr);
            }
            return tmpStr;
        }

        /// <summary>
        /// Just pass the structure "02" type string for example if structure string is "02031600091C5B666465613A396365613A3565343A6430303A3A30315D3A363030361600"
        /// then just pass the "1600091C5B666465613A396365613A3565343A6430303A3A30315D3A363030361600" except the "0203"
        /// </summary>
        /// <param name="refData"></param>
        /// <returns></returns>
        public List<string> GetStructureValueList(string refData)
        {
            List<string> structureDataArray = new List<string>();
            int i = 0;
            while (i < refData.Length)
            {
                switch (refData.Substring(i, 2))
                {
                    case "01":
                        //structureDataArray.Add(refData.Substring(i, 4));
                        i += 4;
                        break;
                    case "02":
                        i += 4;
                        break;
                    case "03":
                        structureDataArray.Add(refData.Substring(i, 4));
                        i += 4;
                        break;
                    case "04":
                        int bitCount = int.Parse(refData.Substring(i + 2, 2), NumberStyles.HexNumber) / 4;
                        structureDataArray.Add(refData.Substring(i, 4 + bitCount));
                        i += 4 + bitCount;
                        break;
                    case "05":
                        structureDataArray.Add(refData.Substring(i, 10));
                        i += 10;
                        break;
                    case "06":
                        structureDataArray.Add(refData.Substring(i, 10));
                        i += 10;
                        break;
                    case "09":
                        structureDataArray.Add(refData.Substring(i, (4 + (int.Parse(refData.Substring(i + 2, 2), NumberStyles.HexNumber) * 2))));
                        i += (4 + (int.Parse(refData.Substring(i + 2, 2), NumberStyles.HexNumber) * 2));
                        break;
                    case "0A":
                        structureDataArray.Add(refData.Substring(i, (4 + (int.Parse(refData.Substring(i + 2, 2), NumberStyles.HexNumber) * 2))));
                        i += (4 + (int.Parse(refData.Substring(i + 2, 2), NumberStyles.HexNumber) * 2));
                        break;
                    case "10":
                        structureDataArray.Add(refData.Substring(i, 6));
                        i += 6;
                        break;
                    case "11":
                        structureDataArray.Add(refData.Substring(i, 4));
                        i += 4;
                        break;
                    case "12":
                        structureDataArray.Add(refData.Substring(i, 6));
                        i += 6;
                        break;
                    case "14":
                        structureDataArray.Add(refData.Substring(i, 18));
                        i += 18;
                        break;
                    case "15":
                        structureDataArray.Add(refData.Substring(i, 18));
                        i += 18;
                        break;
                    case "16":
                        structureDataArray.Add(refData.Substring(i, 4));
                        i += 4;
                        break;
                    case "17":
                        structureDataArray.Add(refData.Substring(i, 10));
                        i += 10;
                        break;
                    case "18":
                        structureDataArray.Add(refData.Substring(i, 18));
                        i += 18;
                        break;
                    case "0F":
                        structureDataArray.Add(refData.Substring(i, 4));
                        i += 4;
                        break;
                }
            }
            return structureDataArray;
        }
        public string[] GetDlmDataListParsing(string LoadData)
        {
            string[] list = new string[200];
            int i = 0, nCount = 0;
            int j = 0, nLength = 4; ;

            string tempData = String.Empty;
            string strMatch = string.Empty;
            if (LoadData == "0100" || LoadData == "01820000" || LoadData.Length < 4)
            {
                list[j] = LoadData;
                return list;
            }
            i = 4;
            strMatch = LoadData.Substring(4, 4);
            for (; i < LoadData.Length; i++)
            {
                if (LoadData.Substring(i, nLength) == strMatch)
                {
                    i += nLength;
                    while (true)
                    {
                        if (i >= LoadData.Length)
                            break;
                        tempData = GateDataFromString(LoadData, ref i, tempData);
                        if (!string.IsNullOrEmpty(tempData))
                        {
                            list[j] = tempData.Trim();
                            j++;
                        }
                        if (LoadData.Length > (i + nLength) && LoadData.Substring(i, nLength) == strMatch)//LoadData.Substring(4, 4))
                        {
                            i--;
                            break;
                        }
                    }
                }
            }
            return list;
        }
        public string[] GetDlmLSDEDataListParsing(string LoadData, string Entryoption)
        {
            string[] list = new string[200];
            int selectedindex = 0;
            int i = 0, nCount = 0;
            int j = 0, nLength = 4; ;
            int entry = 0;
            string tempData = String.Empty;
            string strMatch = string.Empty;
            if (Entryoption == "Second Last Entry")
                selectedindex = 1;
            else if (Entryoption == "Second Last Entry")
                selectedindex = 0;
            if (LoadData == "0100" || LoadData == "01820000" || LoadData.Length < 4)
            {
                list[j] = LoadData;
                return list;
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
            for (; i < LoadData.Length; i++)
            {
                if (LoadData.Substring(i, nLength) == strMatch)
                {
                    entry++;
                    i += nLength;
                    if (entry == nCount || entry == (nCount - selectedindex))
                    {
                        while (true)
                        {
                            if (i >= LoadData.Length)
                                break;
                            tempData = GateDataFromString(LoadData, ref i, tempData);
                            if (!string.IsNullOrEmpty(tempData))
                            {
                                list[j] = tempData.Trim();
                                j++;
                            }
                            if (LoadData.Length > (i + nLength) && LoadData.Substring(i, nLength) == strMatch)//LoadData.Substring(4, 4))
                            {
                                i--;
                                break;
                            }
                        }
                    }
                }
            }
            return list;
        }
        public string[] GetSingleEntryDataArray(string rowData)
        {
            List<string> list = new List<string>();
            string entryData = string.Empty;
            if (!(rowData.Split(' ').Length >= 3) && rowData.Split(' ')[3].Trim().Substring(0, 4) != "0101")
                return list.ToArray();
            entryData = rowData.Split(' ')[3].Trim();
            int count = Convert.ToInt32(entryData.Substring(6, 2), 16);
            entryData = entryData.Substring(8);
            int nStart = 0;
            int nLength = 0;
            while (nStart < entryData.Length)
            {
                nLength = GetDataTypeLength(entryData);
            }
            return list.ToArray();
        }
        /// <summary>
        /// This will help in identifying the length of the data based on data type
        /// </summary>
        /// <param name="dataString"></param>
        /// <returns></returns>
        public int GetDataTypeLength(string dataString)
        {
            int dataLength = 0;
            if (!string.IsNullOrEmpty(dataString) && dataString.Length > 2)
            {
                switch (dataString.Substring(0, 2))
                {
                    case "09":
                        dataLength = (Convert.ToInt32(dataString.Substring(2, 2), 16) * 2) + 4;
                        break;
                    case "0A":
                        dataLength = (Convert.ToInt32(dataString.Substring(2, 2), 16) * 2) + 4;
                        break;
                    case "03":
                        dataLength = 4;
                        break;
                    case "04":
                        if (dataString.Substring(2, 2) == "82")
                            dataLength = (int.Parse(dataString.Substring(4, 4), NumberStyles.HexNumber) / 4) + 8;
                        else
                            dataLength = (int.Parse(dataString.Substring(2, 2), NumberStyles.HexNumber) / 4) + 4; ;
                        break;
                    case "05":
                        dataLength = 10;
                        break;
                    case "06":
                        dataLength = 10;
                        break;
                    case "0F":
                        dataLength = 4;
                        break;
                    case "10":
                        dataLength = 6;
                        break;
                    case "11":
                        dataLength = 4;
                        break;
                    case "12":
                        dataLength = 6;
                        break;
                    case "14":
                        dataLength = 18;   // Integer64
                        break;
                    case "15":
                        dataLength = 18;   // Unsigned64
                        break;
                    case "16":
                        dataLength = 4;
                        break;
                    case "17":
                        dataLength = 10;
                        break;
                    case "18":
                        dataLength = 18;
                        break;
                    case "19":
                        dataLength = 26;
                        break;
                    case "1A":
                        dataLength = 12;
                        break;
                    case "1B":
                        dataLength = 10;
                        break;
                    case "20":
                        dataLength = 6;   // unsigned16 
                        break;
                    case "21":
                        dataLength = 10;
                        break;
                }
            }
            return dataLength;
        }

        /// <summary>
        /// This will help in showing single action schedule parameters default push frequency
        /// </summary>
        /// <param name="dataString"></param>
        /// <returns></returns>
        public string GetPushFrequency(string dataString)
        {
            DLMSParser parse = new DLMSParser();
            string readData = string.Empty;
            try
            {
                if (dataString == "0B")
                {
                    log.Error("Object not Available");
                    return dataString;
                }
                else if (dataString.Substring(0, 2) != "0B" && dataString.Substring(0, 2) == "01" && dataString.Substring(2, 2) == "00")
                {
                    readData = "Disabled";
                    return readData;
                }
                else if (dataString.Substring(0, 2) != "0B" && dataString.Substring(0, 2) == "01" && dataString.Substring(2, 2) != "00")
                {
                    int totalArrays = int.Parse(dataString.Substring(2, 2), NumberStyles.HexNumber);
                    string[] entryArray = Regex.Split(dataString.Substring(4), "0202");
                    try
                    {
                        for (int i = 1; i < entryArray.Length; i++)
                        {
                            string hour = entryArray[i].Substring(4, 2) == "FF" ? "*" : int.Parse(entryArray[i].Substring(4, 2), NumberStyles.HexNumber).ToString("00");
                            string minute = entryArray[i].Substring(6, 2) == "FF" ? "*" : int.Parse(entryArray[i].Substring(6, 2), NumberStyles.HexNumber).ToString("00");
                            string second = entryArray[i].Substring(8, 2) == "FF" ? "*" : int.Parse(entryArray[i].Substring(8, 2), NumberStyles.HexNumber).ToString("00");

                            string year = entryArray[i].Substring(16, 4) == "FFFF" ? "*" : int.Parse(entryArray[i].Substring(16, 4), NumberStyles.HexNumber).ToString("0000");
                            string month = entryArray[i].Substring(20, 2) == "FF" ? "*" : int.Parse(entryArray[i].Substring(20, 2), NumberStyles.HexNumber).ToString("00");
                            string day = entryArray[i].Substring(22, 2) == "FF" ? "*" : int.Parse(entryArray[i].Substring(22, 2), NumberStyles.HexNumber).ToString("00");

                            readData += $"{day}/{month}/{year} {hour}:{minute}:{second} \n\t";
                        }
                    }
                    catch { }
                }
                return readData.Trim();
            }
            catch (Exception ex)
            {
                log.Error("Error in Read Data" + ex.Message.ToString());
                return readData;
            }
        }

        #region Helper Methods
        public static string ConvertAsciiToHex(string asciiString)
        {
            StringBuilder hexBuilder = new StringBuilder();

            foreach (char c in asciiString)
            {
                // Convert each character to its hexadecimal representation
                string hexValue = ((int)c).ToString("X2");
                hexBuilder.Append(hexValue);
            }

            return hexBuilder.ToString();
        }
        public static string ConvertHexToBinary(string hexString)
        {
            // Convert the hexadecimal string to a byte array
            byte[] byteArray = new byte[hexString.Length / 2];
            for (int i = 0; i < byteArray.Length; i++)
            {
                byteArray[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            // Convert the byte array to a binary string
            return string.Concat(Array.ConvertAll(byteArray, b => Convert.ToString(b, 2).PadLeft(8, '0')));
        }
        public string Getdate(string tmpdate, int index, bool Postfix)
        {
            try
            {
                if (Postfix == true)
                    if (int.Parse(tmpdate.Substring(14 + index, 2), NumberStyles.HexNumber) == 255)
                        return int.Parse(tmpdate.Substring(6 + index, 2), NumberStyles.HexNumber).ToString("00") + "/" + int.Parse(tmpdate.Substring(4 + index, 2), NumberStyles.HexNumber).ToString("00") + "/" + int.Parse(tmpdate.Substring(0 + index, 4), NumberStyles.HexNumber).ToString("0000") + " " + int.Parse(tmpdate.Substring(10 + index, 2), NumberStyles.HexNumber).ToString("00") + ":" + int.Parse(tmpdate.Substring(12 + index, 2), NumberStyles.HexNumber).ToString("00") + "  DD/MM/YYYY HH:MM";
                    else
                        return int.Parse(tmpdate.Substring(6 + index, 2), NumberStyles.HexNumber).ToString("00") + "/" + int.Parse(tmpdate.Substring(4 + index, 2), NumberStyles.HexNumber).ToString("00") + "/" + int.Parse(tmpdate.Substring(0 + index, 4), NumberStyles.HexNumber).ToString("0000") + " " + int.Parse(tmpdate.Substring(10 + index, 2), NumberStyles.HexNumber).ToString("00") + ":" + int.Parse(tmpdate.Substring(12 + index, 2), NumberStyles.HexNumber).ToString("00") + ":" + int.Parse(tmpdate.Substring(14 + index, 2), NumberStyles.HexNumber).ToString("00") + "  DD/MM/YYYY hh:mm:ss";
                else
                    return int.Parse(tmpdate.Substring(6 + index, 2), NumberStyles.HexNumber).ToString("00") + "/" + int.Parse(tmpdate.Substring(4 + index, 2), NumberStyles.HexNumber).ToString("00") + "/" + int.Parse(tmpdate.Substring(0 + index, 4), NumberStyles.HexNumber).ToString("0000") + " " + int.Parse(tmpdate.Substring(10 + index, 2), NumberStyles.HexNumber).ToString("00") + ":" + int.Parse(tmpdate.Substring(12 + index, 2), NumberStyles.HexNumber).ToString("00") + ":" + int.Parse(tmpdate.Substring(14 + index, 2), NumberStyles.HexNumber).ToString("00");

            }
            catch (Exception ex)
            {
                string errorMgs = ex.Message;
                return errorMgs;
            }
        }
        public static string GetObis(string tempObis)
        {
            string sResult = string.Empty;
            tempObis = int.Parse(tempObis.Substring(0, 2), NumberStyles.HexNumber).ToString() + "."
                    + int.Parse(tempObis.Substring(2, 2), NumberStyles.HexNumber).ToString() + "."
                    + int.Parse(tempObis.Substring(4, 2), NumberStyles.HexNumber).ToString() + "."
                    + int.Parse(tempObis.Substring(6, 2), NumberStyles.HexNumber).ToString() + "."
                    + int.Parse(tempObis.Substring(8, 2), NumberStyles.HexNumber).ToString() + "."
                    + int.Parse(tempObis.Substring(10, 2), NumberStyles.HexNumber).ToString();
            sResult = tempObis;
            return sResult;
        }
        public double ConvertToDataType17(string hexString)
        {
            double dRetval;
            long longtemp;
            if (long.TryParse(hexString, System.Globalization.NumberStyles.HexNumber, null, out longtemp))
            {
                // Step 1: Convert the hex string to a byte array
                byte[] byteArray = ConvertHexStringToByteArray(hexString);
                // Step 2: Interpret the byte array as an Unsigned32 integer in Big-Endian
                // Ensure the byte array is in Big-Endian format
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(byteArray);
                }
                float unsignedValue = BitConverter.ToSingle(byteArray, 0);
                dRetval = Convert.ToDouble(unsignedValue);
            }
            else
                dRetval = -1;
            return dRetval;
        }
        public double ConvertToFloat(string sRetVal)
        {
            double dRetval;
            long longtemp;
            if (long.TryParse(sRetVal, System.Globalization.NumberStyles.HexNumber, null, out longtemp))
            {
                var b = BitConverter.GetBytes(longtemp);
                sRetVal = (BitConverter.ToSingle(b, 0).ToString());
                dRetval = Math.Round(Convert.ToDouble(BitConverter.ToSingle(b, 0)), 3);
            }
            else
            {
                dRetval = -1;
            }
            return dRetval;
        }
        static byte[] ConvertHexStringToByteArray(string hex)
        {
            int length = hex.Length;
            byte[] bytes = new byte[length / 2];
            for (int i = 0; i < length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }

        public string IDToEventName(Int32 ID)
        {
            string eventName = string.Empty;
            switch (ID)
            {
                case 1:
                    eventName = "RΦ PT Link Miss-O";
                    break;
                case 2:
                    eventName = "RΦ PT Link Miss-R";
                    break;
                case 3:
                    eventName = "YΦ PT Link Miss-O";
                    break;
                case 4:
                    eventName = "YΦ PT Link Miss-R";
                    break;
                case 5:
                    eventName = "BΦ PT Link Miss-O";
                    break;
                case 6:
                    eventName = "BΦ PT Link Miss-R";
                    break;
                case 7:
                    eventName = "Over Voltage-O";
                    break;
                case 8:
                    eventName = "Over Voltage-R";
                    break;
                case 9:
                    eventName = "Low Voltage-O";
                    break;
                case 10:
                    eventName = "Low Voltage-R";
                    break;
                case 11:
                    eventName = "Voltage Unbal-O";
                    break;
                case 12:
                    eventName = "Voltage Unbal-R";
                    break;
                case 51:
                    eventName = "RΦ CT Reverse-0";
                    break;
                case 52:
                    eventName = "RΦ CT Reverse-R";
                    break;
                case 53:
                    eventName = "YΦ CT Reverse-0";
                    break;
                case 54:
                    eventName = "YΦ CT Reverse-R";
                    break;
                case 55:
                    eventName = "BΦ CT Reverse-0";
                    break;
                case 56:
                    eventName = "BΦ CT Reverse-R";
                    break;
                case 57:
                    eventName = "RΦ CT Open-O";
                    break;
                case 58:
                    eventName = "RΦ CT Open-R";
                    break;
                case 59:
                    eventName = "YΦ CT Open-O";
                    break;
                case 60:
                    eventName = "YΦ CT Open-R";
                    break;
                case 61:
                    eventName = "BΦ CT Open-O";
                    break;
                case 62:
                    eventName = "BΦ CT Open-R";
                    break;
                case 63:
                    eventName = "Current Unbal-0";
                    break;
                case 64:
                    eventName = "Current Unbal-R";
                    break;
                case 65:
                    eventName = "CT By Pass-O";
                    break;
                case 66:
                    eventName = "CT By Pass-R";
                    break;
                case 67:
                    eventName = "Over Current-O";
                    break;
                case 68:
                    eventName = "Over Current-R";
                    break;
                case 69:
                    eventName = "Earth Loading-0";
                    break;
                case 70:
                    eventName = "Earth Loading-R";
                    break;
                case 71:
                    eventName = "Over Load-0";
                    break;
                case 72:
                    eventName = "Over Load-R";
                    break;
                case 99:
                    eventName = "High Neutral Current-O";
                    break;
                case 100:
                    eventName = "High Neutral Current-R";
                    break;
                case 101:
                    eventName = "Power Failure-O";
                    break;
                case 102:
                    eventName = "Power Failure-R";
                    break;
                case 105:
                    eventName = "Aux Power Failure-O";
                    break;
                case 106:
                    eventName = "Aux Power Failure-R";
                    break;
                case 120:
                    eventName = "Over Voltage threshold Set";
                    break;
                case 126:
                    eventName = "Under Voltage threshold Set";
                    break;
                case (int)sbyte.MaxValue:
                    eventName = "Periodic Push schedule change";
                    break;
                case 145:
                    eventName = "Over Current threshold Set";
                    break;
                case 148:
                    eventName = "Configuration for Maintenance Mode Time";
                    break;
                case 149:
                    eventName = "Street Light Configuration and  for Manual or Astronomical timings";
                    break;
                case 150:
                    eventName = "Configure Smart Meter/Street Light/Prepayment Mode";
                    break;
                case 151:
                    eventName = "RTC Change";
                    break;
                case 152:
                    eventName = "Demand Int Pd Set";
                    break;
                case 153:
                    eventName = "Profile Capture Pd Set";
                    break;
                case 154:
                    eventName = "Single-action Schedule for Billing Dates";
                    break;
                case 155:
                    eventName = "Activity Calendar for Time Zones";
                    break;
                case 156:
                    eventName = "RS485 device address";
                    break;
                case 157:
                    eventName = "New Firmware Activated";
                    break;
                case 158:
                    eventName = "Load limit (kW) set";
                    break;
                case 159:
                    eventName = "Load switch connect";
                    break;
                case 160:
                    eventName = "Load switch disconnect";
                    break;
                case 161:
                    eventName = "LLS secret (MR) change";
                    break;
                case 162:
                    eventName = "HLS key (US) change";
                    break;
                case 163:
                    eventName = "HLS key (FW) change";
                    break;
                case 164:
                    eventName = "Global key change";
                    break;
                case 165:
                    eventName = "ESWF change";
                    break;
                case 166:
                    eventName = "MD reset ";
                    break;
                case 167:
                    eventName = "IPv4 setup for 2-way comm.";
                    break;
                case 168:
                    eventName = "Set GPRS APN Set";
                    break;
                case 169:
                    eventName = "Single Action Schedule for Image Activation";
                    break;
                case 170:
                    eventName = "Push setup for Test GPRS Set";
                    break;
                case 171:
                    eventName = "Push setup for Power Outage Set";
                    break;
                case 172:
                    eventName = "Software Reset";
                    break;
                case 173:
                    eventName = "Potential Free Configuration Set";
                    break;
                case 174:
                    eventName = "Push setup for Instant Set";
                    break;
                case 177:
                    eventName = "Configuration change to ‘Forwarded only’ mode";
                    break;
                case 178:
                    eventName = "Configuration change to ‘Import-Export’ mode";
                    break;
                case 181:
                    eventName = "Enabled  - load limit function";
                    break;
                case 182:
                    eventName = "Disabled - load limit function";
                    break;
                case 195:
                    eventName = "Alert Configuration for Tamper Events";
                    break;
                case 196:
                    eventName = "Mobile No Config for sending SMS of alerts";
                    break;
                case 197:
                    eventName = "Display Push Sequence Set";
                    break;
                case 199:
                    eventName = "Display Auto Sequence Set";
                    break;
                case 201:
                    eventName = "Magnet Tamper-0";
                    break;
                case 202:
                    eventName = "Magnet Tamper-R";
                    break;
                case 203:
                    eventName = "Neutral Disturb-0";
                    break;
                case 204:
                    eventName = "Neutral Disturb-R";
                    break;
                case 205:
                    eventName = "Very Low PF-0";
                    break;
                case 206:
                    eventName = "Very Low PF-R";
                    break;
                case 207:
                    eventName = "35 kV Tamper-0";
                    break;
                case 208:
                    eventName = "35 kV Tamper-R";
                    break;
                case 209:
                    eventName = "Plug in Communication Module Removal-O";
                    break;
                case 210:
                    eventName = "Plug in Communication Module Removal-R";
                    break;
                case 211:
                    eventName = "Abnormal Frequency-O";
                    break;
                case 212:
                    eventName = "Abnormal Frequency-R";
                    break;
                case 213:
                    eventName = "35 kV Tamper-0";
                    break;
                case 214:
                    eventName = "35 kV Tamper-R";
                    break;
                case 215:
                    eventName = "Overload -O";
                    break;
                case 216:
                    eventName = "Overload -R";
                    break;
                case 235:
                    eventName = "R Phase Relay Weld - O";
                    break;
                case 236:
                    eventName = "R Phase Relay Weld - R";
                    break;
                case 237:
                    eventName = "Y Phase Relay Weld - O";
                    break;
                case 238:
                    eventName = "Y Phase Relay Weld - R";
                    break;
                case 239:
                    eventName = "B Phase Relay Weld - O";
                    break;
                case 240:
                    eventName = "B Phase Relay Weld - R";
                    break;
                case 241:
                    eventName = "R Phase Relay Open - O";
                    break;
                case 242:
                    eventName = "R Phase Relay Open - R";
                    break;
                case 243:
                    eventName = "Y Phase Relay Open - O";
                    break;
                case 244:
                    eventName = "Y Phase Relay Open - R";
                    break;
                case 245:
                    eventName = "B Phase Relay Open - O";
                    break;
                case 246:
                    eventName = "B Phase Relay Open - R";
                    break;
                //249	ESD (35kV/Jammer) – occurrence 
                case 249:
                    eventName = "ESD (35kV/Jammer) – O";
                    break;
                //250	ESD (35kV/Jammer) – restoration
                case 250:
                    eventName = "ESD (35kV/Jammer) – R";
                    break;
                case 251:
                    eventName = "Meter Cover Open-O";
                    break;
                case 252:
                    eventName = "Meter Cover Open-R";
                    break;
                case 253:
                    eventName = "Relay Malfunction-O";
                    break;
                case 254:
                    eventName = "Relay Malfunction-R";
                    break;
                case (int)byte.MaxValue:
                    eventName = "Terminal Cover Open-O";
                    break;
                case 256:
                    eventName = "Terminal Cover Open-R";
                    break;
                case 301:
                    eventName = "Meter Disconnected";
                    break;
                case 302:
                    eventName = "Meter Connected";
                    break;
                case 303:
                    eventName = "O/L Lockout Disc.";
                    break;
                case 304:
                    eventName = "O/L Lockout Conn.";
                    break;
                case 305:
                    eventName = "O/C Lockout Disc.";
                    break;
                case 306:
                    eventName = "O/C Lockout Conn.";
                    break;
                //311	Relays Disconnection Due To Balance 
                case 311:
                    eventName = "Relays Disconnection Due To Balance";
                    break;
                //312 Relays Connection Due To Balance(credit ok)
                case 312:
                    eventName = "Relays Connection Due To Balance(credit ok)";
                    break;
                //348 Relays Connection Due To Emergency Restore
                case 348:
                    eventName = "Relays Connection Due To Emergency Restore";
                    break;
                //349 Relays Disconnection Due To Tamper
                case 349:
                    eventName = "Relays Disconnection Due To Tamper";
                    break;
                //350 Relays Connection Due To Tamper OK
                case 350:
                    eventName = "Relays Connection Due To Tamper OK";
                    break;
                case 351:
                    eventName = "RTC Invalid – occur";
                    break;
                case 352:
                    eventName = "Memory Fail – occur";
                    break;
                case 353:
                    eventName = "RTC Battery low – occur";
                    break;
                case 401:
                    eventName = "R phase Normal Conn.";
                    break;
                case 402:
                    eventName = "R phase Normal Disc.";
                    break;
                case 403:
                    eventName = "R phase Maintenance Mode Conn.";
                    break;
                case 404:
                    eventName = "R phase Maintenance Mode Disc.";
                    break;
                case 405:
                    eventName = "R phase OverLoad Disc.";
                    break;
                case 406:
                    eventName = "R phase Underload Conn.";
                    break;
                case 407:
                    eventName = "R phase OverCurrent Disc.";
                    break;
                case 408:
                    eventName = "R phase UnderCurrent Conn.";
                    break;
                case 409:
                    eventName = "R phase Remote Conn.";
                    break;
                case 410:
                    eventName = "R phase Remote Disc.";
                    break;
                case 501:
                    eventName = "Y phase Normal Conn.";
                    break;
                case 502:
                    eventName = "Y phase Normal Disc.";
                    break;
                case 503:
                    eventName = "Y phase Maintenance Mode Conn.";
                    break;
                case 504:
                    eventName = "Y phase Maintenance Mode Disc.";
                    break;
                case 505:
                    eventName = "Y phase OverLoad Disc.";
                    break;
                case 506:
                    eventName = "Y phase Underload Conn.";
                    break;
                case 507:
                    eventName = "Y phase OverCurrent Disc.";
                    break;
                case 508:
                    eventName = "Y phase UnderCurrent Conn.";
                    break;
                case 509:
                    eventName = "Y phase Remote Conn.";
                    break;
                case 510:
                    eventName = "Y phase Remote Disc.";
                    break;
                case 601:
                    eventName = "B phase Normal Conn.";
                    break;
                case 602:
                    eventName = "B phase Normal Disc.";
                    break;
                case 603:
                    eventName = "B phase Maintenance Mode Conn.";
                    break;
                case 604:
                    eventName = "B phase Maintenance Mode Disc.";
                    break;
                case 605:
                    eventName = "B phase OverLoad Disc.";
                    break;
                case 606:
                    eventName = "B phase Underload Conn.";
                    break;
                case 607:
                    eventName = "B phase OverCurrent Disc.";
                    break;
                case 608:
                    eventName = "B phase UnderCurrent Conn.";
                    break;
                case 609:
                    eventName = "B phase Remote Conn.";
                    break;
                case 610:
                    eventName = "B phase Remote Disc.";
                    break;
                case 751:
                    eventName = "Last token recharge amount(Prepaid mode)";
                    break;
                case 752:
                    eventName = "Last token recharge time(Prepaid mode)";
                    break;
                case 753:
                    eventName = "Total amount at last recharge(Prepaid mode)";
                    break;
                case 754:
                    eventName = "Current balance amount(Prepaid mode)";
                    break;
                case 755:
                    eventName = "Current balance time(Prepaid mode)";
                    break;
                case 760:
                    eventName = "Display parameter change(Auto / Push Mode)";
                    break;
                case 768:
                    eventName = "Load Control Parameters configuration(Passive Relay time)";
                    break;
                case 801:
                    eventName = "ESD / Jammer / Microwave - O";
                    break;
                case 802:
                    eventName = "ESD / Jammer / Microwave - R";
                    break;
                case 805:
                    eventName = "APN Set";
                    break;
                case 806:
                    eventName = "Current High THD tamper threshold Set";
                    break;
                case 807:
                    eventName = "Voltage High THD tamper threshold Set";
                    break;
                case 808:
                    eventName = "Tamper data Push schedule change";
                    break;
                case 811:
                    eventName = "Tamper Push Destination IP Changed";
                    break;
                case 817:
                    eventName = "Instant Push Destination IP Changed";
                    break;
                case 818:
                    eventName = "Two Way push Destination IP Changed";
                    break;
                case 819:
                    eventName = "Alert Push Destination IP Changed";
                    break;
                case 820:
                    eventName = "Bill Push Destination IP Changed";
                    break;
                case 821:
                    eventName = "Daily Energy Push Destination IP Changed";
                    break;
                case 822:
                    eventName = "Load Survey Push Destination IP Changed";
                    break;
                case 823:
                    eventName = "Two way push Action schedule Changed";
                    break;
                case 826:
                    eventName = "Event Enable/ Disable Set";
                    break;
                case 829:
                    eventName = "Temperature rise Persistence time Set";
                    break;
                case 830:
                    eventName = "Temperature rise threshold Set";
                    break;
                case 831:
                    eventName = "Over Current persistence time Set";
                    break;
                case 832:
                    eventName = "Under voltage persistence time Set";
                    break;
                case 833:
                    eventName = "Over voltage persistence time Set";
                    break;
                case 845:
                    eventName = "Daily Load Profile Push schedule change";
                    break;
                case 846:
                    eventName = "Block Load Profile Push schedule change";
                    break;
                case 879:
                    eventName = "Current High THD-O";
                    break;
                case 880:
                    eventName = "Current High THD-R";
                    break;
                case 881:
                    eventName = "Voltage High THD-O";
                    break;
                case 882:
                    eventName = "Voltage High THD-R";
                    break;
                //940	Prepaid local relay connect functionality parameters changed
                case 940:
                    eventName = "Prepaid local relay connect functionality parameters changed";
                    break;
                //945	Randomization Delay Interval – Bill
                case 945:
                    eventName = "Randomization Delay Interval – Bill";
                    break;
                //946 Randomization Delay Interval – DE
                case 946:
                    eventName = "Randomization Delay Interval – DE";
                    break;
                //947 Randomization Delay Interval – LS
                case 947:
                    eventName = "Randomization Delay Interval – LS";
                    break;
                //948 Randomization Delay Interval – Instant
                case 948:
                    eventName = "Randomization Delay Interval – Instant";
                    break;
                case 949:
                    eventName = "RTC Sync Disable";
                    break;
                case 950:
                    eventName = "RTC Sync Enable";
                    break;
                case 951:
                    eventName = "High temperature-O";
                    break;
                case 952:
                    eventName = "High temperature-R";
                    break;
            }
            return eventName;
        }
        static DataTable MergeDataTables(DataTable dt1, DataTable dt2)
        {
            // Create new DataTable to hold merged data
            DataTable mergedDataTable = new DataTable();

            // Add columns from dt1 to merged DataTable
            foreach (DataColumn column in dt1.Columns)
            {
                mergedDataTable.Columns.Add(column.ColumnName, column.DataType);
            }

            // Add columns from dt2 to merged DataTable
            foreach (DataColumn column in dt2.Columns)
            {
                mergedDataTable.Columns.Add(column.ColumnName, column.DataType);
            }

            // Iterate over rows of both DataTables and merge them
            int rowCount = Math.Min(dt1.Rows.Count, dt2.Rows.Count);
            for (int i = 0; i < rowCount; i++)
            {
                DataRow newRow = mergedDataTable.NewRow();

                // Copy values from dt1 row
                foreach (DataColumn column in dt1.Columns)
                {
                    newRow[column.ColumnName] = dt1.Rows[i][column.ColumnName];
                }

                // Copy values from dt2 row
                foreach (DataColumn column in dt2.Columns)
                {
                    newRow[column.ColumnName] = dt2.Rows[i][column.ColumnName];
                }

                mergedDataTable.Rows.Add(newRow);
            }

            return mergedDataTable;
        }
        public bool IsDate(string strDate)
        {
            DateTime dateValue;
            string formats = "dd/MM/yyyy HH:mm:ss";
            try
            {
                if (DateTime.TryParseExact(strDate, formats,
                             new CultureInfo("en-US"),
                             DateTimeStyles.None,
                             out dateValue))
                {
                    DateTime dt = DateTime.ParseExact(strDate, "dd/MM/yyyy HH:mm:ss", provider, DateTimeStyles.AssumeLocal);
                    if (dt != DateTime.MinValue && dt != DateTime.MaxValue)
                        return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
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
        //BY AAC
        public string HexStringToAscii(string hexString)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hexString.Length; i += 2)
            {
                string hexPair = hexString.Substring(i, 2);
                byte byteValue = Convert.ToByte(hexPair, 16);
                char charValue = Convert.ToChar(byteValue);
                sb.Append(charValue);
            }
            return sb.ToString();
        }
        /// <summary>
        /// Handles 00 to 2F as 0 to /
        /// Normal conversion for other ASCII characters
        /// Ensures valid hex input(even length, correct format)
        /// Automatically removes spaces between hex pairs
        /// </summary>
        /// <param name="hex"></param>
        /// <returns> conversion properly maps hex 00 to 2F into their custom-defined ASCII equivalents while handling regular ASCII characters correctly</returns>
        public string HexToAscii(string hex)
        {
            try
            {
                hex = hex.Replace(" ", ""); // Remove spaces if any
                if (hex.Length % 2 != 0)
                    return "Invalid Hex"; // Ensure even-length hex string

                StringBuilder ascii = new StringBuilder();
                for (int i = 0; i < hex.Length; i += 2)
                {
                    string hexPair = hex.Substring(i, 2);
                    int intValue = Convert.ToInt32(hexPair, 16);

                    // Handle special case: Convert hex "00" to "0", "01" to "1", ..., "2F" to "/"
                    if (intValue >= 0x00 && intValue <= 0x2F)
                    {
                        ascii.Append((char)('0' + intValue)); // Convert to corresponding character
                    }
                    else
                    {
                        ascii.Append((char)intValue); // Normal ASCII conversion
                    }
                }
                return ascii.ToString();
            }
            catch
            {
                return "Invalid Hex"; // Handle invalid input
            }
        }

        public static byte[] StringToByteArray(string hex)
        {
            try
            {
                return Enumerable.Range(0, hex.Length).Where<int>((Func<int, bool>)(x => x % 2 == 0)).Select<int, byte>((Func<int, byte>)(x => Convert.ToByte(hex.Substring(x, 2), 16))).ToArray<byte>();
            }
            catch (Exception ex)
            {
                return (byte[])null;
            }
        }

        public static string GetObisName(string _classDic = null, string _obisDic = null, string _attributeDic = "0")
        {
            string ObisName = "";
            if (ObisdataDictionary.Count < 100)
            {
                string filePath = System.Windows.Forms.Application.StartupPath + @"\AllObisList.txt";
                ObisdataDictionary = ReadObisListFileToDictionary(filePath);
            }
            ObisName = GetObisKeyNameByValue(ObisdataDictionary, _classDic, _obisDic, _attributeDic);
            return ObisName;
        }
        public string GetParameterName(string _classDic = null, string _obisDic = null, string _attributeDic = "0")
        {
            string ObisName = GetObisKeyNameByValue(ObisdataDictionary, _classDic, _obisDic, _attributeDic);
            return ObisName;
        }
        public static Dictionary<string, string[]> ReadObisListFileToDictionary(string filePath)
        {
            Dictionary<string, string[]> dataDictionary = new Dictionary<string, string[]>();
            try
            {
                // Read all lines from the text file
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        // Split the line by delimiter '|'
                        string[] parts = line.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length >= 4)
                        {
                            string name = parts[0].Trim();
                            string className = parts[1].Trim();
                            string obisName = parts[2].Trim();
                            string attribute = parts[3].Trim();
                            dataDictionary[$"{name} | {className} | {obisName} | {attribute}"] = new string[] { className, obisName, attribute };
                        }
                    }
                }
                /*
                using (StreamReader reader = new StreamReader(filePath))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        if (!string.IsNullOrEmpty(line))
                        {
                            string[] parts = line.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                            if (parts.Length >= 4)
                            {
                                string name = parts[0].Trim();
                                string className = parts[1].Trim();
                                string obisName = parts[2].Trim();
                                string attribute = parts[3].Trim();

                                dataDictionary[$"{name} | {className} | {obisName} | {attribute}"] = new string[] { className, obisName, attribute };
                            }
                        }
                    }
                }*/
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message.ToString()} => {ex.StackTrace.ToString()}");
                Console.WriteLine($"Error reading the file: {ex.Message}");
            }

            return dataDictionary;
        }
        static string GetObisKeyNameByValue(Dictionary<string, string[]> dictionary, string value1, string value2, string value3)
        {
            string matchingKey = string.Empty;
            if (string.IsNullOrEmpty(value2))
                return "Obis Not Available";
            else if (string.IsNullOrEmpty(value1) && (!string.IsNullOrEmpty(value2)) && value3 == "0")
            {
                foreach (var entry in dictionary)
                {
                    if (entry.Value.Length >= 2 && (entry.Value[1] == value2 && entry.Value[2] == "0"))
                    {
                        matchingKey = entry.Key.Split('|')[0].Trim();
                        break;
                    }
                }
            }
            else if (string.IsNullOrEmpty(value1) && (!string.IsNullOrEmpty(value2)) && (!string.IsNullOrEmpty(value3)))
            {
                foreach (var entry in dictionary)
                {
                    if (entry.Value.Length >= 2 && (entry.Value[1] == value2 && entry.Value[2] == value3))
                    {
                        matchingKey = entry.Key.Split('|')[0].Trim();
                        break;
                    }
                }
            }
            else if (value3 == "0")
            {
                foreach (var entry in dictionary)
                {
                    if (entry.Value.Length >= 2 && entry.Value[0] == value1 && entry.Value[1] == value2)
                    {
                        matchingKey = entry.Key.Split('|')[0].Trim();
                        break;
                    }
                }
            }
            else if (value3 != "0")
            {
                foreach (var entry in dictionary)
                {
                    if (entry.Value.Length >= 2 && entry.Value[0] == value1 && entry.Value[1] == value2 && entry.Value[2] == value3)
                    {
                        matchingKey = entry.Key.Split('|')[0].Trim();
                        break;
                    }
                }
            }
            if (string.IsNullOrEmpty(matchingKey))
            {
                matchingKey = "Name not Available (Man. specific)";
            }

            return matchingKey;
        }
        public string DecimalToHex(string decimalNumberString)
        {
            // Convert decimal to hexadecimal
            int decimalNumber = int.Parse(decimalNumberString);
            char[] hexChars = "0123456789ABCDEF".ToCharArray();
            string hex = "";
            while (decimalNumber > 0)
            {
                int remainder = decimalNumber % 16;
                hex = hexChars[remainder] + hex;
                decimalNumber /= 16;
            }
            return hex;
        }
        static void JoinArrayAsColumn(DataTable dataTable, string columnName, string[] values)
        {
            // Create a new DataColumn with the specified column name
            DataColumn newColumn = new DataColumn(columnName, typeof(string));
            dataTable.Columns.Add(newColumn);

            // Populate the new column with values from the array
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                if (i < values.Length)
                {
                    dataTable.Rows[i][columnName] = values[i];
                }
                else
                {
                    // Handle the case where the array is smaller than the number of rows in the DataTable
                    dataTable.Rows[i][columnName] = DBNull.Value;
                }
            }
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

        public static bool TryParseToInteger(string input, out int result)
        {
            result = 0;

            if (string.IsNullOrWhiteSpace(input))
                return false;

            input = input.Trim();

            // If contains only 0-9 => Decimal
            if (input.All(char.IsDigit))
            {
                return int.TryParse(input, out result);
            }

            // If contains only valid hex digits (0-9, A-F/a-f)
            if (input.All(Uri.IsHexDigit))
            {
                return int.TryParse(input, System.Globalization.NumberStyles.HexNumber, null, out result);
            }

            // Otherwise, invalid format
            return false;
        }

        //BY YS
        public string HexObisToDecObis(string hex)
        {
            if (string.IsNullOrWhiteSpace(hex) || hex.Length != 12)
                throw new ArgumentException("OBIS hex must be exactly 12 characters (6 bytes).", nameof(hex));
            var parts = Enumerable.Range(0, hex.Length / 2)
                                  .Select(i => Convert.ToInt32(hex.Substring(i * 2, 2), 16))
                                  .ToArray();

            return string.Join(".", parts);
        }
        #endregion

        #region Standard Hash Tables 
        public void UnitHashTableIni()
        {
            UnithshTable.Add("01", "a");
            UnithshTable.Add("02", "mo");
            UnithshTable.Add("03", "wk");
            UnithshTable.Add("04", "d");
            UnithshTable.Add("05", "h");
            UnithshTable.Add("06", "min.");
            UnithshTable.Add("07", "s");
            UnithshTable.Add("08", "°");
            UnithshTable.Add("09", "°C");
            UnithshTable.Add("1B", "W");
            UnithshTable.Add("1C", "VA");
            UnithshTable.Add("1D", "var");
            UnithshTable.Add("1E", "Wh");
            UnithshTable.Add("1F", "VAh");
            UnithshTable.Add("20", "varh");
            UnithshTable.Add("21", "A");
            UnithshTable.Add("23", "V");
            UnithshTable.Add("2C", "Hz");
            UnithshTable.Add("48", "dB");
            UnithshTable.Add("FF", "unitless");
            UnithshTable.Add("FE", "other unit");
            UnithshTable.Add("38", "%");

        }
        public void ErrorHashTableIni()
        {
            ErrorhshTable.Add("0B", "Object Unavailable");
            ErrorhshTable.Add("0D", "Access Violated");
            ErrorhshTable.Add("04", "Object Undefined");
            ErrorhshTable.Add("", "Data Not Present");
        }
        public void ObjectsHashTableIni()
        {
            ObjhshTable.Add("00", "NA");
            ObjhshTable.Add("01", "R");
            ObjhshTable.Add("02", "W");
            ObjhshTable.Add("03", "RW");
        }
        public void ScalerHashTableIni()
        {
            ScalerhshTable.Add("FB", "0.00001");
            ScalerhshTable.Add("FC", "0.0001");
            ScalerhshTable.Add("FD", "0.001");
            ScalerhshTable.Add("FE", "0.01");
            ScalerhshTable.Add("FF", "0.1");
            ScalerhshTable.Add("00", "1");
            ScalerhshTable.Add("01", "10");
            ScalerhshTable.Add("02", "100");
            ScalerhshTable.Add("03", "1000");
            ScalerhshTable.Add("04", "10000");
        }
        private void ScalerFactorHashTableIni()
        {
            ScalerFactorhshTable.Add("FA", "-6");
            ScalerFactorhshTable.Add("FB", "-5");
            ScalerFactorhshTable.Add("FC", "-4");
            ScalerFactorhshTable.Add("FD", "-3");
            ScalerFactorhshTable.Add("FE", "-2");
            ScalerFactorhshTable.Add("FF", "-1");
            ScalerFactorhshTable.Add("00", "1");
            ScalerFactorhshTable.Add("01", "1");
            ScalerFactorhshTable.Add("02", "2");
            ScalerFactorhshTable.Add("03", "3");
            ScalerFactorhshTable.Add("04", "4");
            ScalerFactorhshTable.Add("05", "5");
            ScalerFactorhshTable.Add("06", "6");
        }
        public void SendDestinationServiceHashTableIni()
        {
            SendDestinationServicehshTable.Add("0", "Tcp");
            SendDestinationServicehshTable.Add("1", "Udp");
            SendDestinationServicehshTable.Add("2", "Ftp");
            SendDestinationServicehshTable.Add("3", "Smtp");
            SendDestinationServicehshTable.Add("4", "Sms");
            SendDestinationServicehshTable.Add("5", "Hdlc");
            SendDestinationServicehshTable.Add("6", "MBus");
            SendDestinationServicehshTable.Add("7", "ZigBee");
        }
        public void SendDestinationMessageHashTableIni()
        {
            SendDestinationMessagehshTable.Add("0", "CosemApdu");
            SendDestinationMessagehshTable.Add("1", "CosemApduXml");
            SendDestinationMessagehshTable.Add("2", "ManufacturerSpecific");
        }
        public void DataTypeshashTableIni()
        {
            DataTypeshashTable.Add("00", "null-data");//0
            DataTypeshashTable.Add("03", "boolean");//3
            DataTypeshashTable.Add("04", "bit-string");//4
            DataTypeshashTable.Add("05", "double-long");//5
            DataTypeshashTable.Add("06", "double-long-unsigned");//6
            DataTypeshashTable.Add("09", "octet-string");//9
            DataTypeshashTable.Add("0A", "visible-string");//10
            DataTypeshashTable.Add("0C", "utf8-string");//12
            DataTypeshashTable.Add("0D", "bcd");//13
            DataTypeshashTable.Add("0F", "integer");//15
            DataTypeshashTable.Add("10", "long");//16
            DataTypeshashTable.Add("11", "unsigned");//17
            DataTypeshashTable.Add("12", "long-unsigned");//18
            DataTypeshashTable.Add("14", "long64");//20
            DataTypeshashTable.Add("15", "long64-unsigned");//21
            DataTypeshashTable.Add("16", "enum");//22
            DataTypeshashTable.Add("17", "float32");//23
            DataTypeshashTable.Add("18", "float64");//24
            DataTypeshashTable.Add("19", "date-time");//25
            DataTypeshashTable.Add("1A", "date");//26
            DataTypeshashTable.Add("1B", "time");//27
            DataTypeshashTable.Add("1C", "delta-integer");//28
            DataTypeshashTable.Add("1D", "delta-long");//29
            DataTypeshashTable.Add("1E", "delta-double-long");//30
            DataTypeshashTable.Add("1F", "delta-unsigned");//31
            DataTypeshashTable.Add("20", "delta-long-unsigned");//32
            DataTypeshashTable.Add("21", "delta-double-long-unsigned [");//33
            DataTypeshashTable.Add("01", "array");//1
            DataTypeshashTable.Add("02", "structure");//2
            DataTypeshashTable.Add("13", "compact-array");//19
        }
        public void IChashTableIni()
        {
            IChashTable.Add("8", "Clock");
            IChashTable.Add("1", "Data");
            IChashTable.Add("15", "Association LN");
            IChashTable.Add("17", "SAP Assignment");
            IChashTable.Add("3", "Register");
            IChashTable.Add("22", "Single Action Schedule");
            IChashTable.Add("7", "Profile Generic");
            IChashTable.Add("9", "Script Table");
            IChashTable.Add("20", "Activity Calendar");
            IChashTable.Add("4", "Extended Register");
            IChashTable.Add("70", "Disconnect Control");
            IChashTable.Add("71", "Limiter");
            IChashTable.Add("23", "IEC HDLC setup");
            IChashTable.Add("45", "GPRS Modem Setup");
            IChashTable.Add("40", "Push Setup");
            IChashTable.Add("41", "TCP UDP Setup");
            IChashTable.Add("42", "IPv4 Setup");
            IChashTable.Add("48", "IPv6 Setup");
            IChashTable.Add("64", "Security Setup");
            IChashTable.Add("18", "Image Transfer");
        }
        public void AutoDisplay1PHashTableIni()
        {
            AutoDisplayhshTable1P.Clear();
            AutoDisplayhshTable1P.Add("0", "000 - LCD Test");
            AutoDisplayhshTable1P.Add("1", "001");
            AutoDisplayhshTable1P.Add("2", "002 - Time");
            AutoDisplayhshTable1P.Add("3", "003 - Date");
            AutoDisplayhshTable1P.Add("4", "004 - Phase Voltage");
            AutoDisplayhshTable1P.Add("5", "005 - Phase Current");
            AutoDisplayhshTable1P.Add("6", "006 - Neutral Current");
            AutoDisplayhshTable1P.Add("7", "007 - Frequency");
            AutoDisplayhshTable1P.Add("8", "008 - Instantaneous PF ");
            AutoDisplayhshTable1P.Add("9", "009 - Active Power");
            AutoDisplayhshTable1P.Add("10", "010 - Apparent Power");
            AutoDisplayhshTable1P.Add("11", "011");
            AutoDisplayhshTable1P.Add("12", "012");
            AutoDisplayhshTable1P.Add("13", "013 - Cumulative Active Import Energy");
            AutoDisplayhshTable1P.Add("14", "014 - Cumulative Apparent Import Energy");
            AutoDisplayhshTable1P.Add("15", "015");
            AutoDisplayhshTable1P.Add("16", "016 - Cumulative Active Export Energy");
            AutoDisplayhshTable1P.Add("17", "017");
            AutoDisplayhshTable1P.Add("18", "018");
            AutoDisplayhshTable1P.Add("19", "019");
            AutoDisplayhshTable1P.Add("20", "020");
            AutoDisplayhshTable1P.Add("21", "021 - High Resolution Active Energy");
            AutoDisplayhshTable1P.Add("22", "022");
            AutoDisplayhshTable1P.Add("23", "023");
            AutoDisplayhshTable1P.Add("24", "024");
            AutoDisplayhshTable1P.Add("25", "025");
            AutoDisplayhshTable1P.Add("26", "026");
            AutoDisplayhshTable1P.Add("27", "027");
            AutoDisplayhshTable1P.Add("28", "028");
            AutoDisplayhshTable1P.Add("29", "029");
            AutoDisplayhshTable1P.Add("30", "030");
            AutoDisplayhshTable1P.Add("31", "031 - MD Active Import");
            AutoDisplayhshTable1P.Add("32", "032");
            AutoDisplayhshTable1P.Add("33", "033");
            AutoDisplayhshTable1P.Add("34", "034 - Last Bill MD Active Import");
            AutoDisplayhshTable1P.Add("35", "035");
            AutoDisplayhshTable1P.Add("36", "036");
            AutoDisplayhshTable1P.Add("37", "037");
            AutoDisplayhshTable1P.Add("38", "038");
            AutoDisplayhshTable1P.Add("39", "039");
            AutoDisplayhshTable1P.Add("40", "040");
            AutoDisplayhshTable1P.Add("41", "041 - MD Apparent Import");
            AutoDisplayhshTable1P.Add("42", "042");
            AutoDisplayhshTable1P.Add("43", "043");
            AutoDisplayhshTable1P.Add("44", "044 - Last Bill MD Apparent Import");
            AutoDisplayhshTable1P.Add("45", "045");
            AutoDisplayhshTable1P.Add("46", "046");
            AutoDisplayhshTable1P.Add("47", "047");
            AutoDisplayhshTable1P.Add("48", "048");
            AutoDisplayhshTable1P.Add("49", "049");
            AutoDisplayhshTable1P.Add("50", "050");
            AutoDisplayhshTable1P.Add("51", "051 - Last Bill Active Import Energy");
            AutoDisplayhshTable1P.Add("52", "052");
            AutoDisplayhshTable1P.Add("53", "053");
            AutoDisplayhshTable1P.Add("54", "054");
            AutoDisplayhshTable1P.Add("55", "055");
            AutoDisplayhshTable1P.Add("56", "056");
            AutoDisplayhshTable1P.Add("57", "057");
            AutoDisplayhshTable1P.Add("58", "058");
            AutoDisplayhshTable1P.Add("59", "059");
            AutoDisplayhshTable1P.Add("60", "060");
            AutoDisplayhshTable1P.Add("61", "061");
            AutoDisplayhshTable1P.Add("62", "062");
            AutoDisplayhshTable1P.Add("63", "063 - Last Bill Average PF Import");
            AutoDisplayhshTable1P.Add("64", "064");
            AutoDisplayhshTable1P.Add("65", "065");
            AutoDisplayhshTable1P.Add("66", "066");
            AutoDisplayhshTable1P.Add("67", "067 - Last Bill Cumulative Power On Duration");
            AutoDisplayhshTable1P.Add("68", "068");
            AutoDisplayhshTable1P.Add("69", "069 - Cumulative Power On Duration");
            AutoDisplayhshTable1P.Add("70", "070");
            AutoDisplayhshTable1P.Add("71", "071");
            AutoDisplayhshTable1P.Add("72", "072");
            AutoDisplayhshTable1P.Add("73", "073");
            AutoDisplayhshTable1P.Add("74", "074");
            AutoDisplayhshTable1P.Add("75", "075");
            AutoDisplayhshTable1P.Add("76", "076");
            AutoDisplayhshTable1P.Add("77", "077");
            AutoDisplayhshTable1P.Add("78", "078");
            AutoDisplayhshTable1P.Add("79", "079");
            AutoDisplayhshTable1P.Add("80", "080");
            AutoDisplayhshTable1P.Add("81", "081");
            AutoDisplayhshTable1P.Add("82", "082");
            AutoDisplayhshTable1P.Add("83", "083");
            AutoDisplayhshTable1P.Add("84", "084");
            AutoDisplayhshTable1P.Add("85", "085");
            AutoDisplayhshTable1P.Add("86", "086");
            AutoDisplayhshTable1P.Add("87", "087");
            AutoDisplayhshTable1P.Add("88", "088");
            AutoDisplayhshTable1P.Add("89", "089");
            AutoDisplayhshTable1P.Add("90", "090");
            AutoDisplayhshTable1P.Add("91", "091");
            AutoDisplayhshTable1P.Add("92", "092");
            AutoDisplayhshTable1P.Add("93", "093");
            AutoDisplayhshTable1P.Add("94", "094");
            AutoDisplayhshTable1P.Add("95", "095");
            AutoDisplayhshTable1P.Add("96", "096");
            AutoDisplayhshTable1P.Add("97", "097");
            AutoDisplayhshTable1P.Add("98", "098");
            AutoDisplayhshTable1P.Add("99", "099");
            AutoDisplayhshTable1P.Add("100", "100");
            AutoDisplayhshTable1P.Add("101", "101");
            AutoDisplayhshTable1P.Add("102", "102");
            AutoDisplayhshTable1P.Add("103", "103");
            AutoDisplayhshTable1P.Add("104", "104");
            AutoDisplayhshTable1P.Add("105", "105");
            AutoDisplayhshTable1P.Add("106", "106");
            AutoDisplayhshTable1P.Add("107", "107");
            AutoDisplayhshTable1P.Add("108", "108");
            AutoDisplayhshTable1P.Add("109", "109");
            AutoDisplayhshTable1P.Add("110", "110");
            AutoDisplayhshTable1P.Add("111", "111");
            AutoDisplayhshTable1P.Add("112", "112");
            AutoDisplayhshTable1P.Add("113", "113");
            AutoDisplayhshTable1P.Add("114", "114");
            AutoDisplayhshTable1P.Add("115", "115");
            AutoDisplayhshTable1P.Add("116", "116");
            AutoDisplayhshTable1P.Add("117", "117");
            AutoDisplayhshTable1P.Add("118", "118");
            AutoDisplayhshTable1P.Add("119", "119");
            AutoDisplayhshTable1P.Add("120", "120");
            AutoDisplayhshTable1P.Add("121", "121");
            AutoDisplayhshTable1P.Add("122", "122");
            AutoDisplayhshTable1P.Add("123", "123");
            AutoDisplayhshTable1P.Add("124", "124");
            AutoDisplayhshTable1P.Add("125", "125");
            AutoDisplayhshTable1P.Add("126", "126");
            AutoDisplayhshTable1P.Add("127", "127");
            AutoDisplayhshTable1P.Add("128", "128");
            AutoDisplayhshTable1P.Add("129", "129 - Cumulative Apparent Export Energy");
            AutoDisplayhshTable1P.Add("130", "130");
            AutoDisplayhshTable1P.Add("131", "131");
            AutoDisplayhshTable1P.Add("132", "132");
            AutoDisplayhshTable1P.Add("133", "133");
            AutoDisplayhshTable1P.Add("134", "134");
            AutoDisplayhshTable1P.Add("135", "135");
            AutoDisplayhshTable1P.Add("136", "136");
            AutoDisplayhshTable1P.Add("137", "137");
            AutoDisplayhshTable1P.Add("138", "138");
            AutoDisplayhshTable1P.Add("139", "139");
            AutoDisplayhshTable1P.Add("140", "140");
            AutoDisplayhshTable1P.Add("141", "141");
            AutoDisplayhshTable1P.Add("142", "142");
            AutoDisplayhshTable1P.Add("143", "143");
            AutoDisplayhshTable1P.Add("144", "144");
            AutoDisplayhshTable1P.Add("145", "145");
            AutoDisplayhshTable1P.Add("146", "146");
            AutoDisplayhshTable1P.Add("147", "147");
            AutoDisplayhshTable1P.Add("148", "148");
            AutoDisplayhshTable1P.Add("149", "149");
            AutoDisplayhshTable1P.Add("150", "150");
            AutoDisplayhshTable1P.Add("151", "151");
            AutoDisplayhshTable1P.Add("152", "152");
            AutoDisplayhshTable1P.Add("153", "153");
            AutoDisplayhshTable1P.Add("154", "154");
            AutoDisplayhshTable1P.Add("155", "155");
            AutoDisplayhshTable1P.Add("156", "156");
            AutoDisplayhshTable1P.Add("157", "157");
            AutoDisplayhshTable1P.Add("158", "158");
            AutoDisplayhshTable1P.Add("159", "159");
            AutoDisplayhshTable1P.Add("160", "160");
            AutoDisplayhshTable1P.Add("161", "161");
            AutoDisplayhshTable1P.Add("162", "162");
            AutoDisplayhshTable1P.Add("163", "163");
            AutoDisplayhshTable1P.Add("164", "164");
            AutoDisplayhshTable1P.Add("165", "165");
            AutoDisplayhshTable1P.Add("166", "166");
            AutoDisplayhshTable1P.Add("167", "167");
            AutoDisplayhshTable1P.Add("168", "168");
            AutoDisplayhshTable1P.Add("169", "169");
            AutoDisplayhshTable1P.Add("170", "170");
            AutoDisplayhshTable1P.Add("171", "171");
            AutoDisplayhshTable1P.Add("172", "172");
            AutoDisplayhshTable1P.Add("173", "173");
            AutoDisplayhshTable1P.Add("174", "174");
            AutoDisplayhshTable1P.Add("175", "175");
            AutoDisplayhshTable1P.Add("176", "176");
            AutoDisplayhshTable1P.Add("177", "177");
            AutoDisplayhshTable1P.Add("178", "178");
            AutoDisplayhshTable1P.Add("179", "179");
            AutoDisplayhshTable1P.Add("180", "180");
            AutoDisplayhshTable1P.Add("181", "181");
            AutoDisplayhshTable1P.Add("182", "182");
            AutoDisplayhshTable1P.Add("183", "183");
            AutoDisplayhshTable1P.Add("184", "184");
            AutoDisplayhshTable1P.Add("185", "185");
            AutoDisplayhshTable1P.Add("186", "186");
            AutoDisplayhshTable1P.Add("187", "187");
            AutoDisplayhshTable1P.Add("188", "188");
            AutoDisplayhshTable1P.Add("189", "189");
            AutoDisplayhshTable1P.Add("190", "190");
            AutoDisplayhshTable1P.Add("191", "191");
            AutoDisplayhshTable1P.Add("192", "192");
            AutoDisplayhshTable1P.Add("193", "193");
            AutoDisplayhshTable1P.Add("194", "194");
            AutoDisplayhshTable1P.Add("195", "195");
            AutoDisplayhshTable1P.Add("196", "196");
            AutoDisplayhshTable1P.Add("197", "197");
            AutoDisplayhshTable1P.Add("198", "198");
            AutoDisplayhshTable1P.Add("199", "199");
            AutoDisplayhshTable1P.Add("200", "200");
            AutoDisplayhshTable1P.Add("201", "201");
            AutoDisplayhshTable1P.Add("202", "202");
            AutoDisplayhshTable1P.Add("203", "203");
            AutoDisplayhshTable1P.Add("204", "204");
            AutoDisplayhshTable1P.Add("205", "205");
            AutoDisplayhshTable1P.Add("206", "206");
            AutoDisplayhshTable1P.Add("207", "207");
            AutoDisplayhshTable1P.Add("208", "208");
            AutoDisplayhshTable1P.Add("209", "209");
            AutoDisplayhshTable1P.Add("210", "210");
            AutoDisplayhshTable1P.Add("211", "211");
            AutoDisplayhshTable1P.Add("212", "212");
            AutoDisplayhshTable1P.Add("213", "213");
            AutoDisplayhshTable1P.Add("214", "214");
            AutoDisplayhshTable1P.Add("215", "215");
            AutoDisplayhshTable1P.Add("216", "216");
            AutoDisplayhshTable1P.Add("217", "217");
            AutoDisplayhshTable1P.Add("218", "218");
            AutoDisplayhshTable1P.Add("219", "219");
            AutoDisplayhshTable1P.Add("220", "220");
            AutoDisplayhshTable1P.Add("221", "221");
            AutoDisplayhshTable1P.Add("222", "222");
            AutoDisplayhshTable1P.Add("223", "223");
            AutoDisplayhshTable1P.Add("224", "224");
            AutoDisplayhshTable1P.Add("225", "225");
            AutoDisplayhshTable1P.Add("226", "226");
            AutoDisplayhshTable1P.Add("227", "227");
            AutoDisplayhshTable1P.Add("228", "228");
            AutoDisplayhshTable1P.Add("229", "229");
            AutoDisplayhshTable1P.Add("230", "230");
            AutoDisplayhshTable1P.Add("231", "231");
            AutoDisplayhshTable1P.Add("232", "232");
            AutoDisplayhshTable1P.Add("233", "233");
            AutoDisplayhshTable1P.Add("234", "234");
            AutoDisplayhshTable1P.Add("235", "235");
            AutoDisplayhshTable1P.Add("236", "236");
            AutoDisplayhshTable1P.Add("237", "237");
            AutoDisplayhshTable1P.Add("238", "238");
            AutoDisplayhshTable1P.Add("239", "239");
            AutoDisplayhshTable1P.Add("240", "240");
            AutoDisplayhshTable1P.Add("241", "241");
            AutoDisplayhshTable1P.Add("242", "242");
            AutoDisplayhshTable1P.Add("243", "243");
            AutoDisplayhshTable1P.Add("244", "244");
            AutoDisplayhshTable1P.Add("245", "245");
            AutoDisplayhshTable1P.Add("246", "246");
            AutoDisplayhshTable1P.Add("247", "247");
            AutoDisplayhshTable1P.Add("248", "248");
            AutoDisplayhshTable1P.Add("249", "249");
            AutoDisplayhshTable1P.Add("250", "250");
            AutoDisplayhshTable1P.Add("251", "251");
            AutoDisplayhshTable1P.Add("252", "252");
            AutoDisplayhshTable1P.Add("253", "253");
            AutoDisplayhshTable1P.Add("254", "254");
            AutoDisplayhshTable1P.Add("255", "255");
        }
        public void PushDisplay1PHashTableIni()
        {
            PushDisplayhshTable1P.Clear();
            PushDisplayhshTable1P.Add("0", "000 - LCD Test");
            PushDisplayhshTable1P.Add("1", "001");
            PushDisplayhshTable1P.Add("2", "002 - Time");
            PushDisplayhshTable1P.Add("3", "003 - Date");
            PushDisplayhshTable1P.Add("4", "004 - Phase Voltage");
            PushDisplayhshTable1P.Add("5", "005 - Phase Current");
            PushDisplayhshTable1P.Add("6", "006 - Neutral Current");
            PushDisplayhshTable1P.Add("7", "007 - Frequency");
            PushDisplayhshTable1P.Add("8", "008 - Instantaneous PF ");
            PushDisplayhshTable1P.Add("9", "009 - Active Power");
            PushDisplayhshTable1P.Add("10", "010 - Apparent Power");
            PushDisplayhshTable1P.Add("11", "011");
            PushDisplayhshTable1P.Add("12", "012");
            PushDisplayhshTable1P.Add("13", "013 - Cumulative Active Import Energy");
            PushDisplayhshTable1P.Add("14", "014 - Cumulative Apparent Import Energy");
            PushDisplayhshTable1P.Add("15", "015");
            PushDisplayhshTable1P.Add("16", "016 - Cumulative Active Export Energy");
            PushDisplayhshTable1P.Add("17", "017");
            PushDisplayhshTable1P.Add("18", "018");
            PushDisplayhshTable1P.Add("19", "019");
            PushDisplayhshTable1P.Add("20", "020");
            PushDisplayhshTable1P.Add("21", "021 - High Resolution Active Energy");
            PushDisplayhshTable1P.Add("22", "022");
            PushDisplayhshTable1P.Add("23", "023");
            PushDisplayhshTable1P.Add("24", "024");
            PushDisplayhshTable1P.Add("25", "025");
            PushDisplayhshTable1P.Add("26", "026");
            PushDisplayhshTable1P.Add("27", "027");
            PushDisplayhshTable1P.Add("28", "028");
            PushDisplayhshTable1P.Add("29", "029");
            PushDisplayhshTable1P.Add("30", "030");
            PushDisplayhshTable1P.Add("31", "031 - MD Active Import");
            PushDisplayhshTable1P.Add("32", "032");
            PushDisplayhshTable1P.Add("33", "033");
            PushDisplayhshTable1P.Add("34", "034 - Last Bill MD Active Import");
            PushDisplayhshTable1P.Add("35", "035");
            PushDisplayhshTable1P.Add("36", "036");
            PushDisplayhshTable1P.Add("37", "037");
            PushDisplayhshTable1P.Add("38", "038");
            PushDisplayhshTable1P.Add("39", "039");
            PushDisplayhshTable1P.Add("40", "040");
            PushDisplayhshTable1P.Add("41", "041 - MD Apparent Import");
            PushDisplayhshTable1P.Add("42", "042");
            PushDisplayhshTable1P.Add("43", "043");
            PushDisplayhshTable1P.Add("44", "044 - Last Bill MD Apparent Import");
            PushDisplayhshTable1P.Add("45", "045");
            PushDisplayhshTable1P.Add("46", "046");
            PushDisplayhshTable1P.Add("47", "047");
            PushDisplayhshTable1P.Add("48", "048");
            PushDisplayhshTable1P.Add("49", "049");
            PushDisplayhshTable1P.Add("50", "050");
            PushDisplayhshTable1P.Add("51", "051 - Last Bill Active Import Energy");
            PushDisplayhshTable1P.Add("52", "052");
            PushDisplayhshTable1P.Add("53", "053");
            PushDisplayhshTable1P.Add("54", "054");
            PushDisplayhshTable1P.Add("55", "055");
            PushDisplayhshTable1P.Add("56", "056");
            PushDisplayhshTable1P.Add("57", "057");
            PushDisplayhshTable1P.Add("58", "058");
            PushDisplayhshTable1P.Add("59", "059");
            PushDisplayhshTable1P.Add("60", "060");
            PushDisplayhshTable1P.Add("61", "061");
            PushDisplayhshTable1P.Add("62", "062");
            PushDisplayhshTable1P.Add("63", "063 - Last Bill Average PF Import");
            PushDisplayhshTable1P.Add("64", "064");
            PushDisplayhshTable1P.Add("65", "065");
            PushDisplayhshTable1P.Add("66", "066");
            PushDisplayhshTable1P.Add("67", "067 - Last Bill Cumulative Power On Duration");
            PushDisplayhshTable1P.Add("68", "068");
            PushDisplayhshTable1P.Add("69", "069 - Cumulative Power On Duration");
            PushDisplayhshTable1P.Add("70", "070");
            PushDisplayhshTable1P.Add("71", "071");
            PushDisplayhshTable1P.Add("72", "072");
            PushDisplayhshTable1P.Add("73", "073");
            PushDisplayhshTable1P.Add("74", "074");
            PushDisplayhshTable1P.Add("75", "075");
            PushDisplayhshTable1P.Add("76", "076");
            PushDisplayhshTable1P.Add("77", "077");
            PushDisplayhshTable1P.Add("78", "078");
            PushDisplayhshTable1P.Add("79", "079");
            PushDisplayhshTable1P.Add("80", "080");
            PushDisplayhshTable1P.Add("81", "081");
            PushDisplayhshTable1P.Add("82", "082");
            PushDisplayhshTable1P.Add("83", "083");
            PushDisplayhshTable1P.Add("84", "084");
            PushDisplayhshTable1P.Add("85", "085");
            PushDisplayhshTable1P.Add("86", "086");
            PushDisplayhshTable1P.Add("87", "087");
            PushDisplayhshTable1P.Add("88", "088");
            PushDisplayhshTable1P.Add("89", "089");
            PushDisplayhshTable1P.Add("90", "090");
            PushDisplayhshTable1P.Add("91", "091");
            PushDisplayhshTable1P.Add("92", "092");
            PushDisplayhshTable1P.Add("93", "093");
            PushDisplayhshTable1P.Add("94", "094");
            PushDisplayhshTable1P.Add("95", "095");
            PushDisplayhshTable1P.Add("96", "096");
            PushDisplayhshTable1P.Add("97", "097");
            PushDisplayhshTable1P.Add("98", "098");
            PushDisplayhshTable1P.Add("99", "099");
            PushDisplayhshTable1P.Add("100", "100");
            PushDisplayhshTable1P.Add("101", "101");
            PushDisplayhshTable1P.Add("102", "102");
            PushDisplayhshTable1P.Add("103", "103");
            PushDisplayhshTable1P.Add("104", "104");
            PushDisplayhshTable1P.Add("105", "105");
            PushDisplayhshTable1P.Add("106", "106");
            PushDisplayhshTable1P.Add("107", "107");
            PushDisplayhshTable1P.Add("108", "108");
            PushDisplayhshTable1P.Add("109", "109");
            PushDisplayhshTable1P.Add("110", "110");
            PushDisplayhshTable1P.Add("111", "111");
            PushDisplayhshTable1P.Add("112", "112");
            PushDisplayhshTable1P.Add("113", "113");
            PushDisplayhshTable1P.Add("114", "114");
            PushDisplayhshTable1P.Add("115", "115");
            PushDisplayhshTable1P.Add("116", "116");
            PushDisplayhshTable1P.Add("117", "117");
            PushDisplayhshTable1P.Add("118", "118");
            PushDisplayhshTable1P.Add("119", "119");
            PushDisplayhshTable1P.Add("120", "120");
            PushDisplayhshTable1P.Add("121", "121");
            PushDisplayhshTable1P.Add("122", "122");
            PushDisplayhshTable1P.Add("123", "123");
            PushDisplayhshTable1P.Add("124", "124");
            PushDisplayhshTable1P.Add("125", "125");
            PushDisplayhshTable1P.Add("126", "126");
            PushDisplayhshTable1P.Add("127", "127");
            PushDisplayhshTable1P.Add("128", "128");
            PushDisplayhshTable1P.Add("129", "129 - Cumulative Apparent Export Energy");
            PushDisplayhshTable1P.Add("130", "130");
            PushDisplayhshTable1P.Add("131", "131");
            PushDisplayhshTable1P.Add("132", "132");
            PushDisplayhshTable1P.Add("133", "133");
            PushDisplayhshTable1P.Add("134", "134");
            PushDisplayhshTable1P.Add("135", "135");
            PushDisplayhshTable1P.Add("136", "136");
            PushDisplayhshTable1P.Add("137", "137");
            PushDisplayhshTable1P.Add("138", "138");
            PushDisplayhshTable1P.Add("139", "139");
            PushDisplayhshTable1P.Add("140", "140");
            PushDisplayhshTable1P.Add("141", "141");
            PushDisplayhshTable1P.Add("142", "142");
            PushDisplayhshTable1P.Add("143", "143");
            PushDisplayhshTable1P.Add("144", "144");
            PushDisplayhshTable1P.Add("145", "145");
            PushDisplayhshTable1P.Add("146", "146");
            PushDisplayhshTable1P.Add("147", "147");
            PushDisplayhshTable1P.Add("148", "148");
            PushDisplayhshTable1P.Add("149", "149");
            PushDisplayhshTable1P.Add("150", "150");
            PushDisplayhshTable1P.Add("151", "151");
            PushDisplayhshTable1P.Add("152", "152");
            PushDisplayhshTable1P.Add("153", "153");
            PushDisplayhshTable1P.Add("154", "154");
            PushDisplayhshTable1P.Add("155", "155");
            PushDisplayhshTable1P.Add("156", "156");
            PushDisplayhshTable1P.Add("157", "157");
            PushDisplayhshTable1P.Add("158", "158");
            PushDisplayhshTable1P.Add("159", "159");
            PushDisplayhshTable1P.Add("160", "160");
            PushDisplayhshTable1P.Add("161", "161");
            PushDisplayhshTable1P.Add("162", "162");
            PushDisplayhshTable1P.Add("163", "163");
            PushDisplayhshTable1P.Add("164", "164");
            PushDisplayhshTable1P.Add("165", "165");
            PushDisplayhshTable1P.Add("166", "166");
            PushDisplayhshTable1P.Add("167", "167");
            PushDisplayhshTable1P.Add("168", "168");
            PushDisplayhshTable1P.Add("169", "169");
            PushDisplayhshTable1P.Add("170", "170");
            PushDisplayhshTable1P.Add("171", "171");
            PushDisplayhshTable1P.Add("172", "172");
            PushDisplayhshTable1P.Add("173", "173");
            PushDisplayhshTable1P.Add("174", "174");
            PushDisplayhshTable1P.Add("175", "175");
            PushDisplayhshTable1P.Add("176", "176");
            PushDisplayhshTable1P.Add("177", "177");
            PushDisplayhshTable1P.Add("178", "178");
            PushDisplayhshTable1P.Add("179", "179");
            PushDisplayhshTable1P.Add("180", "180");
            PushDisplayhshTable1P.Add("181", "181");
            PushDisplayhshTable1P.Add("182", "182");
            PushDisplayhshTable1P.Add("183", "183");
            PushDisplayhshTable1P.Add("184", "184");
            PushDisplayhshTable1P.Add("185", "185");
            PushDisplayhshTable1P.Add("186", "186");
            PushDisplayhshTable1P.Add("187", "187");
            PushDisplayhshTable1P.Add("188", "188");
            PushDisplayhshTable1P.Add("189", "189");
            PushDisplayhshTable1P.Add("190", "190");
            PushDisplayhshTable1P.Add("191", "191");
            PushDisplayhshTable1P.Add("192", "192");
            PushDisplayhshTable1P.Add("193", "193");
            PushDisplayhshTable1P.Add("194", "194");
            PushDisplayhshTable1P.Add("195", "195");
            PushDisplayhshTable1P.Add("196", "196");
            PushDisplayhshTable1P.Add("197", "197");
            PushDisplayhshTable1P.Add("198", "198");
            PushDisplayhshTable1P.Add("199", "199");
            PushDisplayhshTable1P.Add("200", "200");
            PushDisplayhshTable1P.Add("201", "201");
            PushDisplayhshTable1P.Add("202", "202");
            PushDisplayhshTable1P.Add("203", "203");
            PushDisplayhshTable1P.Add("204", "204");
            PushDisplayhshTable1P.Add("205", "205");
            PushDisplayhshTable1P.Add("206", "206");
            PushDisplayhshTable1P.Add("207", "207");
            PushDisplayhshTable1P.Add("208", "208");
            PushDisplayhshTable1P.Add("209", "209");
            PushDisplayhshTable1P.Add("210", "210");
            PushDisplayhshTable1P.Add("211", "211");
            PushDisplayhshTable1P.Add("212", "212");
            PushDisplayhshTable1P.Add("213", "213");
            PushDisplayhshTable1P.Add("214", "214");
            PushDisplayhshTable1P.Add("215", "215");
            PushDisplayhshTable1P.Add("216", "216");
            PushDisplayhshTable1P.Add("217", "217");
            PushDisplayhshTable1P.Add("218", "218");
            PushDisplayhshTable1P.Add("219", "219");
            PushDisplayhshTable1P.Add("220", "220");
            PushDisplayhshTable1P.Add("221", "221");
            PushDisplayhshTable1P.Add("222", "222");
            PushDisplayhshTable1P.Add("223", "223");
            PushDisplayhshTable1P.Add("224", "224");
            PushDisplayhshTable1P.Add("225", "225");
            PushDisplayhshTable1P.Add("226", "226");
            PushDisplayhshTable1P.Add("227", "227");
            PushDisplayhshTable1P.Add("228", "228");
            PushDisplayhshTable1P.Add("229", "229");
            PushDisplayhshTable1P.Add("230", "230");
            PushDisplayhshTable1P.Add("231", "231");
            PushDisplayhshTable1P.Add("232", "232");
            PushDisplayhshTable1P.Add("233", "233");
            PushDisplayhshTable1P.Add("234", "234");
            PushDisplayhshTable1P.Add("235", "235");
            PushDisplayhshTable1P.Add("236", "236");
            PushDisplayhshTable1P.Add("237", "237");
            PushDisplayhshTable1P.Add("238", "238");
            PushDisplayhshTable1P.Add("239", "239");
            PushDisplayhshTable1P.Add("240", "240");
            PushDisplayhshTable1P.Add("241", "241");
            PushDisplayhshTable1P.Add("242", "242");
            PushDisplayhshTable1P.Add("243", "243");
            PushDisplayhshTable1P.Add("244", "244");
            PushDisplayhshTable1P.Add("245", "245");
            PushDisplayhshTable1P.Add("246", "246");
            PushDisplayhshTable1P.Add("247", "247");
            PushDisplayhshTable1P.Add("248", "248");
            PushDisplayhshTable1P.Add("249", "249");
            PushDisplayhshTable1P.Add("250", "250");
            PushDisplayhshTable1P.Add("251", "251");
            PushDisplayhshTable1P.Add("252", "252");
            PushDisplayhshTable1P.Add("253", "253");
            PushDisplayhshTable1P.Add("254", "254");
            PushDisplayhshTable1P.Add("255", "255");
        }
        public void AutoDisplay3PHashTableIni()
        {
            AutoDisplayhshTable3P.Clear();
            AutoDisplayhshTable3P.Add("0", "000 - LCD Test");
            AutoDisplayhshTable3P.Add("1", "001 - Time");
            AutoDisplayhshTable3P.Add("2", "002 - Date");
            AutoDisplayhshTable3P.Add("3", "003 - R Phase Voltage");
            AutoDisplayhshTable3P.Add("4", "004 - Y Phase Voltage");
            AutoDisplayhshTable3P.Add("5", "005 - B Phase Voltage");
            AutoDisplayhshTable3P.Add("6", "006 - R Phase Current");
            AutoDisplayhshTable3P.Add("7", "007 - Y Phase Current");
            AutoDisplayhshTable3P.Add("8", "008 - B Phase Current");
            AutoDisplayhshTable3P.Add("9", "009 - Instantaneous PF");
            AutoDisplayhshTable3P.Add("10", "010");
            AutoDisplayhshTable3P.Add("11", "011");
            AutoDisplayhshTable3P.Add("12", "012");
            AutoDisplayhshTable3P.Add("13", "013 - Frequency");
            AutoDisplayhshTable3P.Add("14", "014");
            AutoDisplayhshTable3P.Add("15", "015");
            AutoDisplayhshTable3P.Add("16", "016");
            AutoDisplayhshTable3P.Add("17", "017");
            AutoDisplayhshTable3P.Add("18", "018");
            AutoDisplayhshTable3P.Add("19", "019 - Cumulative Active Import Energy");
            AutoDisplayhshTable3P.Add("20", "020 - Cumulative Active Export Energy");
            AutoDisplayhshTable3P.Add("21", "021 - Cumulative Apparent Import Energy");
            AutoDisplayhshTable3P.Add("22", "022 - Cumulative Apparent Export Energy");
            AutoDisplayhshTable3P.Add("23", "023");
            AutoDisplayhshTable3P.Add("24", "024");
            AutoDisplayhshTable3P.Add("25", "025");
            AutoDisplayhshTable3P.Add("26", "026");
            AutoDisplayhshTable3P.Add("27", "027 - Last Bill Active Import Energy");
            AutoDisplayhshTable3P.Add("28", "028");
            AutoDisplayhshTable3P.Add("29", "029");
            AutoDisplayhshTable3P.Add("30", "030 - Last Bill Average PF Import");
            AutoDisplayhshTable3P.Add("31", "031 - Last Bill MD Active Import");
            AutoDisplayhshTable3P.Add("32", "032 - Last Bill MD Apparent Import");
            AutoDisplayhshTable3P.Add("33", "033");
            AutoDisplayhshTable3P.Add("34", "034");
            AutoDisplayhshTable3P.Add("35", "035 - MD Active Import");
            AutoDisplayhshTable3P.Add("36", "036 - MD Apparent Import");
            AutoDisplayhshTable3P.Add("37", "037");
            AutoDisplayhshTable3P.Add("38", "038");
            AutoDisplayhshTable3P.Add("39", "039");
            AutoDisplayhshTable3P.Add("40", "040");
            AutoDisplayhshTable3P.Add("41", "041");
            AutoDisplayhshTable3P.Add("42", "042");
            AutoDisplayhshTable3P.Add("43", "043");
            AutoDisplayhshTable3P.Add("44", "044 - Active Power");
            AutoDisplayhshTable3P.Add("45", "045");
            AutoDisplayhshTable3P.Add("46", "046 - Apparent Power");
            AutoDisplayhshTable3P.Add("47", "047");
            AutoDisplayhshTable3P.Add("48", "048");
            AutoDisplayhshTable3P.Add("49", "049");
            AutoDisplayhshTable3P.Add("50", "050");
            AutoDisplayhshTable3P.Add("51", "051");
            AutoDisplayhshTable3P.Add("52", "052");
            AutoDisplayhshTable3P.Add("53", "053");
            AutoDisplayhshTable3P.Add("54", "054");
            AutoDisplayhshTable3P.Add("55", "055");
            AutoDisplayhshTable3P.Add("56", "056");
            AutoDisplayhshTable3P.Add("57", "057");
            AutoDisplayhshTable3P.Add("58", "058");
            AutoDisplayhshTable3P.Add("59", "059");
            AutoDisplayhshTable3P.Add("60", "060");
            AutoDisplayhshTable3P.Add("61", "061");
            AutoDisplayhshTable3P.Add("62", "062");
            AutoDisplayhshTable3P.Add("63", "063");
            AutoDisplayhshTable3P.Add("64", "064");
            AutoDisplayhshTable3P.Add("65", "065");
            AutoDisplayhshTable3P.Add("66", "066");
            AutoDisplayhshTable3P.Add("67", "067");
            AutoDisplayhshTable3P.Add("68", "068");
            AutoDisplayhshTable3P.Add("69", "069");
            AutoDisplayhshTable3P.Add("70", "070");
            AutoDisplayhshTable3P.Add("71", "071");
            AutoDisplayhshTable3P.Add("72", "072");
            AutoDisplayhshTable3P.Add("73", "073");
            AutoDisplayhshTable3P.Add("74", "074");
            AutoDisplayhshTable3P.Add("75", "075");
            AutoDisplayhshTable3P.Add("76", "076");
            AutoDisplayhshTable3P.Add("77", "077");
            AutoDisplayhshTable3P.Add("78", "078");
            AutoDisplayhshTable3P.Add("79", "079");
            AutoDisplayhshTable3P.Add("80", "080");
            AutoDisplayhshTable3P.Add("81", "081");
            AutoDisplayhshTable3P.Add("82", "082");
            AutoDisplayhshTable3P.Add("83", "083");
            AutoDisplayhshTable3P.Add("84", "084");
            AutoDisplayhshTable3P.Add("85", "085");
            AutoDisplayhshTable3P.Add("86", "086");
            AutoDisplayhshTable3P.Add("87", "087");
            AutoDisplayhshTable3P.Add("88", "088");
            AutoDisplayhshTable3P.Add("89", "089");
            AutoDisplayhshTable3P.Add("90", "090");
            AutoDisplayhshTable3P.Add("91", "091");
            AutoDisplayhshTable3P.Add("92", "092");
            AutoDisplayhshTable3P.Add("93", "093");
            AutoDisplayhshTable3P.Add("94", "094");
            AutoDisplayhshTable3P.Add("95", "095");
            AutoDisplayhshTable3P.Add("96", "096");
            AutoDisplayhshTable3P.Add("97", "097");
            AutoDisplayhshTable3P.Add("98", "098");
            AutoDisplayhshTable3P.Add("99", "099");
            AutoDisplayhshTable3P.Add("100", "100");
            AutoDisplayhshTable3P.Add("101", "101");
            AutoDisplayhshTable3P.Add("102", "102");
            AutoDisplayhshTable3P.Add("103", "103");
            AutoDisplayhshTable3P.Add("104", "104");
            AutoDisplayhshTable3P.Add("105", "105");
            AutoDisplayhshTable3P.Add("106", "106");
            AutoDisplayhshTable3P.Add("107", "107");
            AutoDisplayhshTable3P.Add("108", "108");
            AutoDisplayhshTable3P.Add("109", "109");
            AutoDisplayhshTable3P.Add("110", "110");
            AutoDisplayhshTable3P.Add("111", "111");
            AutoDisplayhshTable3P.Add("112", "112");
            AutoDisplayhshTable3P.Add("113", "113");
            AutoDisplayhshTable3P.Add("114", "114");
            AutoDisplayhshTable3P.Add("115", "115");
            AutoDisplayhshTable3P.Add("116", "116");
            AutoDisplayhshTable3P.Add("117", "117");
            AutoDisplayhshTable3P.Add("118", "118");
            AutoDisplayhshTable3P.Add("119", "119");
            AutoDisplayhshTable3P.Add("120", "120");
            AutoDisplayhshTable3P.Add("121", "121");
            AutoDisplayhshTable3P.Add("122", "122");
            AutoDisplayhshTable3P.Add("123", "123");
            AutoDisplayhshTable3P.Add("124", "124");
            AutoDisplayhshTable3P.Add("125", "125");
            AutoDisplayhshTable3P.Add("126", "126");
            AutoDisplayhshTable3P.Add("127", "127");
            AutoDisplayhshTable3P.Add("128", "128");
            AutoDisplayhshTable3P.Add("129", "129");
            AutoDisplayhshTable3P.Add("130", "130");
            AutoDisplayhshTable3P.Add("131", "131");
            AutoDisplayhshTable3P.Add("132", "132");
            AutoDisplayhshTable3P.Add("133", "133");
            AutoDisplayhshTable3P.Add("134", "134");
            AutoDisplayhshTable3P.Add("135", "135");
            AutoDisplayhshTable3P.Add("136", "136");
            AutoDisplayhshTable3P.Add("137", "137");
            AutoDisplayhshTable3P.Add("138", "138");
            AutoDisplayhshTable3P.Add("139", "139");
            AutoDisplayhshTable3P.Add("140", "140");
            AutoDisplayhshTable3P.Add("141", "141");
            AutoDisplayhshTable3P.Add("142", "142");
            AutoDisplayhshTable3P.Add("143", "143");
            AutoDisplayhshTable3P.Add("144", "144");
            AutoDisplayhshTable3P.Add("145", "145 - Cumulative Power Off Duration");
            AutoDisplayhshTable3P.Add("146", "146");
            AutoDisplayhshTable3P.Add("147", "147");
            AutoDisplayhshTable3P.Add("148", "148 - Cumulative Reactive Q1 Energy");
            AutoDisplayhshTable3P.Add("149", "149 - Cumulative Reactive Q2 Energy");
            AutoDisplayhshTable3P.Add("150", "150 - Cumulative Reactive Q3 Energy");
            AutoDisplayhshTable3P.Add("151", "151 - Cumulative Reactive Q4 Energy");
            AutoDisplayhshTable3P.Add("152", "152");
            AutoDisplayhshTable3P.Add("153", "153");
            AutoDisplayhshTable3P.Add("154", "154");
            AutoDisplayhshTable3P.Add("155", "155");
            AutoDisplayhshTable3P.Add("156", "156 - Last Bill Reactive Q1 Energy");
            AutoDisplayhshTable3P.Add("157", "157");
            AutoDisplayhshTable3P.Add("158", "158");
            AutoDisplayhshTable3P.Add("159", "159");
            AutoDisplayhshTable3P.Add("160", "160");
            AutoDisplayhshTable3P.Add("161", "161");
            AutoDisplayhshTable3P.Add("162", "162");
            AutoDisplayhshTable3P.Add("163", "163");
            AutoDisplayhshTable3P.Add("164", "164");
            AutoDisplayhshTable3P.Add("165", "165");
            AutoDisplayhshTable3P.Add("166", "166");
            AutoDisplayhshTable3P.Add("167", "167");
            AutoDisplayhshTable3P.Add("168", "168");
            AutoDisplayhshTable3P.Add("169", "169");
            AutoDisplayhshTable3P.Add("170", "170");
            AutoDisplayhshTable3P.Add("171", "171");
            AutoDisplayhshTable3P.Add("172", "172");
            AutoDisplayhshTable3P.Add("173", "173");
            AutoDisplayhshTable3P.Add("174", "174");
            AutoDisplayhshTable3P.Add("175", "175");
            AutoDisplayhshTable3P.Add("176", "176");
            AutoDisplayhshTable3P.Add("177", "177");
            AutoDisplayhshTable3P.Add("178", "178");
            AutoDisplayhshTable3P.Add("179", "179");
            AutoDisplayhshTable3P.Add("180", "180");
            AutoDisplayhshTable3P.Add("181", "181");
            AutoDisplayhshTable3P.Add("182", "182");
            AutoDisplayhshTable3P.Add("183", "183");
            AutoDisplayhshTable3P.Add("184", "184");
            AutoDisplayhshTable3P.Add("185", "185");
            AutoDisplayhshTable3P.Add("186", "186");
            AutoDisplayhshTable3P.Add("187", "187");
            AutoDisplayhshTable3P.Add("188", "188");
            AutoDisplayhshTable3P.Add("189", "189");
            AutoDisplayhshTable3P.Add("190", "190");
            AutoDisplayhshTable3P.Add("191", "191");
            AutoDisplayhshTable3P.Add("192", "192");
            AutoDisplayhshTable3P.Add("193", "193");
            AutoDisplayhshTable3P.Add("194", "194");
            AutoDisplayhshTable3P.Add("195", "195");
            AutoDisplayhshTable3P.Add("196", "196 - Last Bill Cumulative Power Off Duration");
            AutoDisplayhshTable3P.Add("197", "197");
            AutoDisplayhshTable3P.Add("198", "198");
            AutoDisplayhshTable3P.Add("199", "199");
            AutoDisplayhshTable3P.Add("200", "200");
            AutoDisplayhshTable3P.Add("201", "201");
            AutoDisplayhshTable3P.Add("202", "202");
            AutoDisplayhshTable3P.Add("203", "203");
            AutoDisplayhshTable3P.Add("204", "204");
            AutoDisplayhshTable3P.Add("205", "205");
            AutoDisplayhshTable3P.Add("206", "206");
            AutoDisplayhshTable3P.Add("207", "207");
            AutoDisplayhshTable3P.Add("208", "208");
            AutoDisplayhshTable3P.Add("209", "209");
            AutoDisplayhshTable3P.Add("210", "210");
            AutoDisplayhshTable3P.Add("211", "211");
            AutoDisplayhshTable3P.Add("212", "212");
            AutoDisplayhshTable3P.Add("213", "213");
            AutoDisplayhshTable3P.Add("214", "214");
            AutoDisplayhshTable3P.Add("215", "215");
            AutoDisplayhshTable3P.Add("216", "216");
            AutoDisplayhshTable3P.Add("217", "217");
            AutoDisplayhshTable3P.Add("218", "218");
            AutoDisplayhshTable3P.Add("219", "219");
            AutoDisplayhshTable3P.Add("220", "220");
            AutoDisplayhshTable3P.Add("221", "221");
            AutoDisplayhshTable3P.Add("222", "222");
            AutoDisplayhshTable3P.Add("223", "223");
            AutoDisplayhshTable3P.Add("224", "224");
            AutoDisplayhshTable3P.Add("225", "225");
            AutoDisplayhshTable3P.Add("226", "226");
            AutoDisplayhshTable3P.Add("227", "227");
            AutoDisplayhshTable3P.Add("228", "228");
            AutoDisplayhshTable3P.Add("229", "229");
            AutoDisplayhshTable3P.Add("230", "230");
            AutoDisplayhshTable3P.Add("231", "231");
            AutoDisplayhshTable3P.Add("232", "232");
            AutoDisplayhshTable3P.Add("233", "233");
            AutoDisplayhshTable3P.Add("234", "234");
            AutoDisplayhshTable3P.Add("235", "235");
            AutoDisplayhshTable3P.Add("236", "236");
            AutoDisplayhshTable3P.Add("237", "237 - High Resolution Active Import Energy");
            AutoDisplayhshTable3P.Add("238", "238");
            AutoDisplayhshTable3P.Add("239", "239");
            AutoDisplayhshTable3P.Add("240", "240");
            AutoDisplayhshTable3P.Add("241", "241");
            AutoDisplayhshTable3P.Add("242", "242");
            AutoDisplayhshTable3P.Add("243", "243");
            AutoDisplayhshTable3P.Add("244", "244");
            AutoDisplayhshTable3P.Add("245", "245");
            AutoDisplayhshTable3P.Add("246", "246");
            AutoDisplayhshTable3P.Add("247", "247");
            AutoDisplayhshTable3P.Add("248", "248");
            AutoDisplayhshTable3P.Add("249", "249");
            AutoDisplayhshTable3P.Add("250", "250");
            AutoDisplayhshTable3P.Add("251", "251");
            AutoDisplayhshTable3P.Add("252", "252");
            AutoDisplayhshTable3P.Add("253", "253");
            AutoDisplayhshTable3P.Add("254", "254");
            AutoDisplayhshTable3P.Add("255", "255");


        }
        public void PushDisplay3PHashTableIni()
        {
            PushDisplayhshTable3P.Clear();
            PushDisplayhshTable3P.Add("0", "000 - LCD Test");
            PushDisplayhshTable3P.Add("1", "001 - Time");
            PushDisplayhshTable3P.Add("2", "002 - Date");
            PushDisplayhshTable3P.Add("3", "003 - R Phase Voltage");
            PushDisplayhshTable3P.Add("4", "004 - Y Phase Voltage");
            PushDisplayhshTable3P.Add("5", "005 - B Phase Voltage");
            PushDisplayhshTable3P.Add("6", "006 - R Phase Current");
            PushDisplayhshTable3P.Add("7", "007 - Y Phase Current");
            PushDisplayhshTable3P.Add("8", "008 - B Phase Current");
            PushDisplayhshTable3P.Add("9", "009 - Instantaneous PF");
            PushDisplayhshTable3P.Add("10", "010");
            PushDisplayhshTable3P.Add("11", "011");
            PushDisplayhshTable3P.Add("12", "012");
            PushDisplayhshTable3P.Add("13", "013 - Frequency");
            PushDisplayhshTable3P.Add("14", "014");
            PushDisplayhshTable3P.Add("15", "015");
            PushDisplayhshTable3P.Add("16", "016");
            PushDisplayhshTable3P.Add("17", "017");
            PushDisplayhshTable3P.Add("18", "018");
            PushDisplayhshTable3P.Add("19", "019 - Cumulative Active Import Energy");
            PushDisplayhshTable3P.Add("20", "020 - Cumulative Active Export Energy");
            PushDisplayhshTable3P.Add("21", "021 - Cumulative Apparent Import Energy");
            PushDisplayhshTable3P.Add("22", "022 - Cumulative Apparent Export Energy");
            PushDisplayhshTable3P.Add("23", "023");
            PushDisplayhshTable3P.Add("24", "024");
            PushDisplayhshTable3P.Add("25", "025");
            PushDisplayhshTable3P.Add("26", "026");
            PushDisplayhshTable3P.Add("27", "027 - Last Bill Active Import Energy");
            PushDisplayhshTable3P.Add("28", "028");
            PushDisplayhshTable3P.Add("29", "029");
            PushDisplayhshTable3P.Add("30", "030 - Last Bill Average PF Import");
            PushDisplayhshTable3P.Add("31", "031 - Last Bill MD Active Import");
            PushDisplayhshTable3P.Add("32", "032 - Last Bill MD Apparent Import");
            PushDisplayhshTable3P.Add("33", "033");
            PushDisplayhshTable3P.Add("34", "034");
            PushDisplayhshTable3P.Add("35", "035 - MD Active Import");
            PushDisplayhshTable3P.Add("36", "036 - MD Apparent Import");
            PushDisplayhshTable3P.Add("37", "037");
            PushDisplayhshTable3P.Add("38", "038");
            PushDisplayhshTable3P.Add("39", "039");
            PushDisplayhshTable3P.Add("40", "040");
            PushDisplayhshTable3P.Add("41", "041");
            PushDisplayhshTable3P.Add("42", "042");
            PushDisplayhshTable3P.Add("43", "043");
            PushDisplayhshTable3P.Add("44", "044 - Active Power");
            PushDisplayhshTable3P.Add("45", "045");
            PushDisplayhshTable3P.Add("46", "046 - Apparent Power");
            PushDisplayhshTable3P.Add("47", "047");
            PushDisplayhshTable3P.Add("48", "048");
            PushDisplayhshTable3P.Add("49", "049");
            PushDisplayhshTable3P.Add("50", "050");
            PushDisplayhshTable3P.Add("51", "051");
            PushDisplayhshTable3P.Add("52", "052");
            PushDisplayhshTable3P.Add("53", "053");
            PushDisplayhshTable3P.Add("54", "054");
            PushDisplayhshTable3P.Add("55", "055");
            PushDisplayhshTable3P.Add("56", "056");
            PushDisplayhshTable3P.Add("57", "057");
            PushDisplayhshTable3P.Add("58", "058");
            PushDisplayhshTable3P.Add("59", "059");
            PushDisplayhshTable3P.Add("60", "060");
            PushDisplayhshTable3P.Add("61", "061");
            PushDisplayhshTable3P.Add("62", "062");
            PushDisplayhshTable3P.Add("63", "063");
            PushDisplayhshTable3P.Add("64", "064");
            PushDisplayhshTable3P.Add("65", "065");
            PushDisplayhshTable3P.Add("66", "066");
            PushDisplayhshTable3P.Add("67", "067");
            PushDisplayhshTable3P.Add("68", "068");
            PushDisplayhshTable3P.Add("69", "069");
            PushDisplayhshTable3P.Add("70", "070");
            PushDisplayhshTable3P.Add("71", "071");
            PushDisplayhshTable3P.Add("72", "072");
            PushDisplayhshTable3P.Add("73", "073");
            PushDisplayhshTable3P.Add("74", "074");
            PushDisplayhshTable3P.Add("75", "075");
            PushDisplayhshTable3P.Add("76", "076");
            PushDisplayhshTable3P.Add("77", "077");
            PushDisplayhshTable3P.Add("78", "078");
            PushDisplayhshTable3P.Add("79", "079");
            PushDisplayhshTable3P.Add("80", "080");
            PushDisplayhshTable3P.Add("81", "081");
            PushDisplayhshTable3P.Add("82", "082");
            PushDisplayhshTable3P.Add("83", "083");
            PushDisplayhshTable3P.Add("84", "084");
            PushDisplayhshTable3P.Add("85", "085");
            PushDisplayhshTable3P.Add("86", "086");
            PushDisplayhshTable3P.Add("87", "087");
            PushDisplayhshTable3P.Add("88", "088");
            PushDisplayhshTable3P.Add("89", "089");
            PushDisplayhshTable3P.Add("90", "090");
            PushDisplayhshTable3P.Add("91", "091");
            PushDisplayhshTable3P.Add("92", "092");
            PushDisplayhshTable3P.Add("93", "093");
            PushDisplayhshTable3P.Add("94", "094");
            PushDisplayhshTable3P.Add("95", "095");
            PushDisplayhshTable3P.Add("96", "096");
            PushDisplayhshTable3P.Add("97", "097");
            PushDisplayhshTable3P.Add("98", "098");
            PushDisplayhshTable3P.Add("99", "099");
            PushDisplayhshTable3P.Add("100", "100");
            PushDisplayhshTable3P.Add("101", "101");
            PushDisplayhshTable3P.Add("102", "102");
            PushDisplayhshTable3P.Add("103", "103");
            PushDisplayhshTable3P.Add("104", "104");
            PushDisplayhshTable3P.Add("105", "105");
            PushDisplayhshTable3P.Add("106", "106");
            PushDisplayhshTable3P.Add("107", "107");
            PushDisplayhshTable3P.Add("108", "108");
            PushDisplayhshTable3P.Add("109", "109");
            PushDisplayhshTable3P.Add("110", "110");
            PushDisplayhshTable3P.Add("111", "111");
            PushDisplayhshTable3P.Add("112", "112");
            PushDisplayhshTable3P.Add("113", "113");
            PushDisplayhshTable3P.Add("114", "114");
            PushDisplayhshTable3P.Add("115", "115");
            PushDisplayhshTable3P.Add("116", "116");
            PushDisplayhshTable3P.Add("117", "117");
            PushDisplayhshTable3P.Add("118", "118");
            PushDisplayhshTable3P.Add("119", "119");
            PushDisplayhshTable3P.Add("120", "120");
            PushDisplayhshTable3P.Add("121", "121");
            PushDisplayhshTable3P.Add("122", "122");
            PushDisplayhshTable3P.Add("123", "123");
            PushDisplayhshTable3P.Add("124", "124");
            PushDisplayhshTable3P.Add("125", "125");
            PushDisplayhshTable3P.Add("126", "126");
            PushDisplayhshTable3P.Add("127", "127");
            PushDisplayhshTable3P.Add("128", "128");
            PushDisplayhshTable3P.Add("129", "129");
            PushDisplayhshTable3P.Add("130", "130");
            PushDisplayhshTable3P.Add("131", "131");
            PushDisplayhshTable3P.Add("132", "132");
            PushDisplayhshTable3P.Add("133", "133");
            PushDisplayhshTable3P.Add("134", "134");
            PushDisplayhshTable3P.Add("135", "135");
            PushDisplayhshTable3P.Add("136", "136");
            PushDisplayhshTable3P.Add("137", "137");
            PushDisplayhshTable3P.Add("138", "138");
            PushDisplayhshTable3P.Add("139", "139");
            PushDisplayhshTable3P.Add("140", "140");
            PushDisplayhshTable3P.Add("141", "141");
            PushDisplayhshTable3P.Add("142", "142");
            PushDisplayhshTable3P.Add("143", "143");
            PushDisplayhshTable3P.Add("144", "144");
            PushDisplayhshTable3P.Add("145", "145 - Cumulative Power Off Duration");
            PushDisplayhshTable3P.Add("146", "146");
            PushDisplayhshTable3P.Add("147", "147");
            PushDisplayhshTable3P.Add("148", "148 - Cumulative Reactive Q1 Energy");
            PushDisplayhshTable3P.Add("149", "149 - Cumulative Reactive Q2 Energy");
            PushDisplayhshTable3P.Add("150", "150 - Cumulative Reactive Q3 Energy");
            PushDisplayhshTable3P.Add("151", "151 - Cumulative Reactive Q4 Energy");
            PushDisplayhshTable3P.Add("152", "152");
            PushDisplayhshTable3P.Add("153", "153");
            PushDisplayhshTable3P.Add("154", "154");
            PushDisplayhshTable3P.Add("155", "155");
            PushDisplayhshTable3P.Add("156", "156 - Last Bill Reactive Q1 Energy");
            PushDisplayhshTable3P.Add("157", "157");
            PushDisplayhshTable3P.Add("158", "158");
            PushDisplayhshTable3P.Add("159", "159");
            PushDisplayhshTable3P.Add("160", "160");
            PushDisplayhshTable3P.Add("161", "161");
            PushDisplayhshTable3P.Add("162", "162");
            PushDisplayhshTable3P.Add("163", "163");
            PushDisplayhshTable3P.Add("164", "164");
            PushDisplayhshTable3P.Add("165", "165");
            PushDisplayhshTable3P.Add("166", "166");
            PushDisplayhshTable3P.Add("167", "167");
            PushDisplayhshTable3P.Add("168", "168");
            PushDisplayhshTable3P.Add("169", "169");
            PushDisplayhshTable3P.Add("170", "170");
            PushDisplayhshTable3P.Add("171", "171");
            PushDisplayhshTable3P.Add("172", "172");
            PushDisplayhshTable3P.Add("173", "173");
            PushDisplayhshTable3P.Add("174", "174");
            PushDisplayhshTable3P.Add("175", "175");
            PushDisplayhshTable3P.Add("176", "176");
            PushDisplayhshTable3P.Add("177", "177");
            PushDisplayhshTable3P.Add("178", "178");
            PushDisplayhshTable3P.Add("179", "179");
            PushDisplayhshTable3P.Add("180", "180");
            PushDisplayhshTable3P.Add("181", "181");
            PushDisplayhshTable3P.Add("182", "182");
            PushDisplayhshTable3P.Add("183", "183");
            PushDisplayhshTable3P.Add("184", "184");
            PushDisplayhshTable3P.Add("185", "185");
            PushDisplayhshTable3P.Add("186", "186");
            PushDisplayhshTable3P.Add("187", "187");
            PushDisplayhshTable3P.Add("188", "188");
            PushDisplayhshTable3P.Add("189", "189");
            PushDisplayhshTable3P.Add("190", "190");
            PushDisplayhshTable3P.Add("191", "191");
            PushDisplayhshTable3P.Add("192", "192");
            PushDisplayhshTable3P.Add("193", "193");
            PushDisplayhshTable3P.Add("194", "194");
            PushDisplayhshTable3P.Add("195", "195");
            PushDisplayhshTable3P.Add("196", "196 - Last Bill Cumulative Power Off Duration");
            PushDisplayhshTable3P.Add("197", "197");
            PushDisplayhshTable3P.Add("198", "198");
            PushDisplayhshTable3P.Add("199", "199");
            PushDisplayhshTable3P.Add("200", "200");
            PushDisplayhshTable3P.Add("201", "201");
            PushDisplayhshTable3P.Add("202", "202");
            PushDisplayhshTable3P.Add("203", "203");
            PushDisplayhshTable3P.Add("204", "204");
            PushDisplayhshTable3P.Add("205", "205");
            PushDisplayhshTable3P.Add("206", "206");
            PushDisplayhshTable3P.Add("207", "207");
            PushDisplayhshTable3P.Add("208", "208");
            PushDisplayhshTable3P.Add("209", "209");
            PushDisplayhshTable3P.Add("210", "210");
            PushDisplayhshTable3P.Add("211", "211");
            PushDisplayhshTable3P.Add("212", "212");
            PushDisplayhshTable3P.Add("213", "213");
            PushDisplayhshTable3P.Add("214", "214");
            PushDisplayhshTable3P.Add("215", "215");
            PushDisplayhshTable3P.Add("216", "216");
            PushDisplayhshTable3P.Add("217", "217");
            PushDisplayhshTable3P.Add("218", "218");
            PushDisplayhshTable3P.Add("219", "219");
            PushDisplayhshTable3P.Add("220", "220");
            PushDisplayhshTable3P.Add("221", "221");
            PushDisplayhshTable3P.Add("222", "222");
            PushDisplayhshTable3P.Add("223", "223");
            PushDisplayhshTable3P.Add("224", "224");
            PushDisplayhshTable3P.Add("225", "225");
            PushDisplayhshTable3P.Add("226", "226");
            PushDisplayhshTable3P.Add("227", "227");
            PushDisplayhshTable3P.Add("228", "228");
            PushDisplayhshTable3P.Add("229", "229");
            PushDisplayhshTable3P.Add("230", "230");
            PushDisplayhshTable3P.Add("231", "231");
            PushDisplayhshTable3P.Add("232", "232");
            PushDisplayhshTable3P.Add("233", "233");
            PushDisplayhshTable3P.Add("234", "234");
            PushDisplayhshTable3P.Add("235", "235");
            PushDisplayhshTable3P.Add("236", "236");
            PushDisplayhshTable3P.Add("237", "237 - High Resolution Active Import Energy");
            PushDisplayhshTable3P.Add("238", "238");
            PushDisplayhshTable3P.Add("239", "239");
            PushDisplayhshTable3P.Add("240", "240");
            PushDisplayhshTable3P.Add("241", "241");
            PushDisplayhshTable3P.Add("242", "242");
            PushDisplayhshTable3P.Add("243", "243");
            PushDisplayhshTable3P.Add("244", "244");
            PushDisplayhshTable3P.Add("245", "245");
            PushDisplayhshTable3P.Add("246", "246");
            PushDisplayhshTable3P.Add("247", "247");
            PushDisplayhshTable3P.Add("248", "248");
            PushDisplayhshTable3P.Add("249", "249");
            PushDisplayhshTable3P.Add("250", "250");
            PushDisplayhshTable3P.Add("251", "251");
            PushDisplayhshTable3P.Add("252", "252");
            PushDisplayhshTable3P.Add("253", "253");
            PushDisplayhshTable3P.Add("254", "254");
            PushDisplayhshTable3P.Add("255", "255");
        }
        private void SecurityPolicyTableIni()
        {
            SecurityPolicyTable.Add("0", "unused");
            SecurityPolicyTable.Add("1", "unused");
            SecurityPolicyTable.Add("2", "authenticated request");
            SecurityPolicyTable.Add("3", "encrypted request");
            SecurityPolicyTable.Add("4", "digitally signed request");
            SecurityPolicyTable.Add("5", "authenticated response");
            SecurityPolicyTable.Add("6", "encrypted response");
            SecurityPolicyTable.Add("7", "digitally signed response");
        }
        #endregion
    }
}
