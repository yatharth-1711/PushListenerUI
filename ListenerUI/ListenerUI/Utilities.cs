using AutoTest.FrameWork;
using AutoTest.FrameWork.Converts;
using MeterComm;
using Microsoft.CSharp;
using Org.BouncyCastle.Utilities.Collections;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoTestDesktopWFA
{
    public static class Utilities
    {
        static Utilities()
        {
            //Add Global Variable
            RegistersDistionaryGlobal.Add("GC: Scaling Factor", "");
            RegistersDistionaryGlobal.Add("GC: G2", "");
            RegistersDistionaryGlobal.Add("GC: G3", "");
            RegistersDistionaryGlobal.Add("GC: Vref", "");
            RegistersDistionaryGlobal.Add("GC: Ibasic", "");
            RegistersDistionaryGlobal.Add("GC: Imax", "");
        }

        #region constants values

        //If any Property Having null or Wrong value
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static List<string> errorOfDifferent = new List<string>();
        public static string DateStringForeadAndSet = "dd/MM/yyyy HH:mm:ss";
        private static Dictionary<string, DataTable> dlmsICTableCollection = new Dictionary<string, DataTable>();

        //public static string ConnectionString = "Data Source=DEMOSERVER\\GENUS_VAL;Initial Catalog=AutoTest_Demo1;User ID=sa;Password=db@71146-igs";
        //public static string ConnectionString = "Data Source=192.10.8.1\\Genus_Val;Initial Catalog=AutoTest_Demo1;User ID=sa;Password=db@71146-igs";
        //public static string ConnectionString = "Data Source=192.10.10.205;Initial Catalog=AutoTest_Demo1;User ID=sa;Password=db@genus_MPLS#1";
        //public static string ConnectionString = "Data Source=1000RND2004023\\SQLEXPRESS;Initial Catalog=AutoTest;Integrated Security=True";
        //public static string ConnectionString = ConfigurationManager.ConnectionStrings[1].ConnectionString;      
        public static string ProjectName = null;

        public static bool IsDirtyCase = false;
        /// <summary>
        /// 
        /// </summary>
        public static string touString = "";


        /// <summary>
        /// 
        /// </summary>
        public static string DataSelectedBy = "";

        /// <summary>
        /// 
        /// </summary>
        public static string strLTFile = Application.StartupPath + @"\\LineTraffic";

        /// <summary>
        /// 
        /// </summary>
        public static string strUseability = Application.StartupPath + @"\\Reports\\Useability\\";

        /// <summary>
        /// 
        /// </summary>
        public static string strLTFileNme = "/" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_LINETRAFFIC.txt";


        /// <summary>
        /// Folder
        /// </summary>
        public static string dir = @"c:\temp";

        /// <summary>
        /// 
        /// </summary>
        public static string timeStampFormate = string.Empty;

        /// <summary>
        /// FileName
        /// </summary>
        public static string fileName = "SelectedControl.bin";
        /// <summary>
        /// 
        /// </summary>
        public static string pdfFileLoacation = Application.StartupPath + @"\PDF\";
        /// <summary>
        /// 
        /// </summary>
        public static string htmlFileLoacation = Application.StartupPath + @"\HTML\";
        /// <summary>
        /// 
        /// </summary>
        public static string commonFolderProject = Application.StartupPath + @"\Images\folderopen.ico";
        /// <summary>
        /// 
        /// </summary>
        public static string projectFolderProject = Application.StartupPath + @"\Images\Project.ico";
        /// <summary>
        /// 
        /// </summary>
        public static string testCaseFolderProject = Application.StartupPath + @"\Images\Case.ico";
        /// <summary>
        /// 
        /// </summary>
        public static string oprationIcon = Application.StartupPath + @"\Images\Opration.ico";

        /// <summary>
        /// 
        /// </summary>
        public static string calibrationFilePath = Application.StartupPath + @"\\CalibrationFiles";

        /// <summary>
        /// 
        /// </summary>
        public static string calibrationFileName = "";

        /// <summary>
        /// 
        /// </summary>
        public static string powerSourceName = "";
        /// <summary>
        /// 
        /// </summary>
        public static string eventLogFilePath = Application.StartupPath + @"\EventLogFiles";
        /// <summary>
        /// 
        /// </summary>
        public static string AllDataFilePath = Application.StartupPath + @"\AllDataFiles\";
        /// <summary>
        /// 
        /// </summary>
        public static string BillingDataFilePath = Application.StartupPath + @"\BillingDataFiles\";
        /// <summary>
        /// 
        /// </summary>
        public static string SettingsFilePath = Application.StartupPath + @"\Settings\";

        public static string LineTrafficFilePath = Application.StartupPath + @"\LineTraffic\";
        public static string DLMSTestExecutionsFilePath = Application.StartupPath + @"\DLMSTestExecutions\";
        public static string rtfFileName = string.Empty;

        /// <summary>
        /// This Holds the All Ref XML Files
        /// </summary>
        public static string RefXMLFilePath = Application.StartupPath + @"\RefXML\";

        /// <summary>
        /// This Hold All DLMS Interface Classes XML
        /// </summary>
        public static string DLMSClasesXMLFilePath = Application.StartupPath + @"\DLMSClasesXML\";

        /// <summary>
        /// 
        /// </summary>
        public static string logFilePath = Application.StartupPath + @"\Log\";
        /// <summary>
        /// 
        /// </summary>
        public static string stopDataFilePath = Application.StartupPath + @"\AllDataFiles\Data\StopExecution";
        /// <summary>
        /// 
        /// </summary>
        public static string ProfileData = Application.StartupPath + @"\AllDataFiles\Data\ProfileData";
        /// <summary>
        /// 
        /// </summary>
        public static string LoadSurveyMissData = Application.StartupPath + @"\AllDataFiles\Data\LoadSurveyMiss";
        /// <summary>
        /// 
        /// </summary>
        public static string ParseDLMSFileData = Application.StartupPath + @"\ParseDLMSData";
        /// <summary>
        /// 
        /// </summary>
        public static string ExportJSONData = Application.StartupPath + @"\ExportJSONData";
        /// <summary>
        /// 
        /// </summary>
        public static string eventLog = "EventLog.txt";
        /// <summary>
        /// 
        /// </summary>
        public static string eventDataToWrite = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public static Boolean isCalibrated = false;
        /// <summary>
        /// 
        /// </summary>
        public static string[] baudRateArray = new string[] { "9600", "19200", "38400", "57600", "115200" };
        /// <summary>
        /// s
        /// </summary>
        public static string[] ReadDataProfileArray = new string[] { "AllData", "Without Load survey Data", "BillingData", "Event Data", "Load Survey", "Daily Energy", "Instant", "Voltage Events", "Power Events", "Transaction Events", "Current Events", "Other Tamper Events", "Non Roll Over Events", "Control Events" };
        public static string[] ReadSelectiveDataProfileArray = new string[] { "Block Load", "Daily Load", "Voltage Related Events", "Current Related Events", "Power Related Events", "Transaction Events", "Other Tamper Events", "Non Roll Over Events", "Control Events", "Billing Profile" };
        /// <summary>
        /// s
        /// </summary>
        public static string[] ReadBillingProfileArray = new string[] { "BillingData" };
        /// <summary>
        /// 
        /// </summary>
        public static string[] EnergyModeArray = new string[] { "Active", "Reactive", "Apparent" };

        public static string[] EnergyModeArrayofImpulse = new string[] { "Active", "Reactive lag", "Reactive lead" };
        /// <summary>
        /// 
        /// </summary>
        public static string[] AccessLevelArray = new string[] { "Public Client", "Meter Reader", "Utility Settings" };
        /// <summary>
        /// 
        /// </summary>
        public static string[] LogicalAddressArray = new string[] { "1", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30" };
        /// <summary>
        /// 
        /// </summary>
        public static string[] AddressModeArray = new string[] { "One Byte", "Four Byte" };
        /// <summary>
        /// 
        /// </summary>
        public static string[] TempRegisterArray = new string[] { "One Byte" };
        /// <summary>
        /// 
        /// </summary>
        public static string[] GetDataArray = new string[] { "Entry", "Range" };
        /// <summary>
        /// 
        /// </summary>
        public static string[] GetProfiileArray = new string[] { "Load Survey", "Daily Energy" };
        /// <summary>
        /// 
        /// </summary>
        public static string[] GetIntegrationPeriodArray = new string[] { "5", "10", "15", "20", "30", "60" };

        /// <summary>
        /// Harmonic Related Quantities
        /// </summary>
        //public static string[] GetHarmonicOrder = new string[] {"2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31"};
        //public static string[] GetHarmonicMagnitude = new string[] { "5", "10", "15", "20", "30", "40", "50", "60", "70", "80"};
        //public static string[] GetHarmonicMagnitude = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", 
        //"26", "27", "28", "29", "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "50" };
        //public static string[] GetHarmonicAngle = new string[] { "0", "45", "60", "90", "120", "150", "180"};
        //public static string[] GetHarmonicAngle = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25",
        //"26", "27", "28", "29", "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "50",
        //"51", "52", "53", "54", "55", "56", "57", "58", "59", "60", "61", "62", "63", "64", "65", "66", "67", "68", "69", "70", "71", "72", "73", "74", "75", 
        //"76", "77", "78", "79", "80", "81", "82", "83", "84", "85", "86", "87", "88", "89", "90", "91", "92", "93", "94", "95", "96", "97", "98", "99", "100", 
        //"101", "102", "103", "104", "105", "106", "107", "108", "109", "110", "111", "112", "113", "114", "115", "116", "117", "118", "119", "120", "121", "122", "123", "124", "125", 
        //"126", "127", "128", "129", "130", "131", "132", "133", "134", "135", "136", "137", "138", "139", "140", "141", "142", "143", "144", "145", "146", "147", "148", "149", "150", 
        //"151", "152", "153", "154", "155", "156", "157", "158", "159", "160", "161", "162", "163", "164", "165", "166", "167", "168", "169", "170", "171", "172", "173", "174", "175", 
        //"176", "177", "178", "179", "180", "181", "182", "183", "184", "185", "186", "187", "188", "189", "190", "191", "192", "193", "194", "195", "196", "197", "198", "199", "200", 
        //"201", "202", "203", "204", "205", "206", "207", "208", "209", "210", "211", "212", "213", "214", "215", "216", "217", "218", "219", "220", "221", "222", "223", "224", "225", 
        //"226", "227", "228", "229", "230", "231", "232", "233", "234", "235", "236", "237", "238", "239", "240", "241", "242", "243", "244", "245", "246", "247", "248", "249", "250", 
        //"251", "252", "253", "254", "255", "256", "257", "258", "259", "260", "261", "262", "263", "264", "265", "266", "267", "268", "269", "270", "271", "272", "273", "274", "275", 
        //"276", "277", "278", "279", "280", "281", "282", "283", "284", "285", "286", "287", "288", "289", "290", "291", "292", "293", "294", "295", "296", "297", "298", "299", "300", 
        //"301", "302", "303", "304", "305", "306", "307", "308", "309", "310", "311", "312", "313", "314", "315", "316", "317", "318", "319", "320", "321", "322", "323", "324", "325", 
        //"326", "327", "328", "329", "330", "331", "332", "333", "334", "335", "336", "337", "338", "339", "340", "341", "342", "343", "344", "345", "346", "347", "348", "349", "350", 
        //"351", "352", "353", "354", "355", "356", "357", "358", "359", "360" };

        /// <summary>
        /// Billing Cycle 
        /// </summary>
        public static string[] GetBillingCycle = new string[] { "Monthly", "Odd", "Even" };     //by YS
        /// <summary>
        /// Load Limit
        /// </summary>
        public static string[] GetLoadLimit = new string[] { "Enable", "Disable" };
        /// <summary>
        /// 
        /// </summary>
        public static string[] GetRelayOperation = new string[] { "Connect", "Disconnect" };

        /// <summary>
        /// 
        /// </summary>
        public static CloneableDictionary<string, string> RegistersDistionary = new CloneableDictionary<string, string>();
        /// <summary>
        /// 
        /// </summary>
        public static CloneableDictionary<string, string> RegistersDistionaryGlobal = new CloneableDictionary<string, string>();

        /// <summary>
        /// 
        /// </summary>
        public static string[] OperationArray = new string[] { "+", "-", "*", "/", "!=", "Bitwise AND", "Bitwise OR", "Sqrt", "Sin", "Cos", "Tan", "Sin Inverse", "Cos Inverse", "Tan Inverse" };
        /// <summary>
        /// 
        /// </summary>
        public static string[] DateTimeForRTCOptions = new string[] { "Meter RTC + Minutes", "Current DateTime", "Custom", "Wild Card" };
        /// <summary>
        /// 
        /// </summary   
        public static string[] DemandInitialPeriodList = new string[] { "05 Min", "10 Min", "15 Min", "20 Min", "30 Min", "60 Min" };
        /// <summary>
        /// 
        /// </summary>
        public static string[] DateFunctionalList = new string[] { "Normal", "Miss" };

        /// <summary>
        /// 
        /// </summary>
        public static string[] SetBillDateList = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "28", "29", "30", "31", "FD", "FE" };

        /// <summary>
        /// 
        /// </summary>
        //public static Int32[] _arrIntCommonProjects = new Int32[] {60, 94};
        public static Int32[] _arrIntCommonProjects = new Int32[] { 426, 425, 424, 423, 422, 421, 420, 419, 418, 60, 94, 268, 272, 269, 271, 273, 278 };

        /// <summary>
        /// 
        /// </summary>
        public static string[] MaxDemandInitialPeriodList = new string[] { "Yes", "No" };
        /// <summary>
        /// 
        /// </summary>
        public static string[] ABSOptionArray = new string[] { "No", "Yes" };
        /// <summary>
        /// 
        /// </summary>
        //public static string[] DemandProfileCapturePeriodList = new string[] { "15 Min", "30 Min", "60 Min" };
        public static string[] DemandProfileCapturePeriodList = new string[] { "05 Min", "10 Min", "15 Min", "20 Min", "30 Min", "60 Min" }; // By AAC

        /// <summary>
        /// 
        /// </summary>
        public static string[] DayLightSaviDeviationngArray = new string[] {  "-120 Min","-115 Min","-110 Min","-105 Min","-100 Min",
            "-095 Min","-090 Min","-085 Min","-080 Min","-075 Min","-070 Min","-065 Min","-060 Min","-055 Min","-050 Min","-045 Min",
            "-040 Min","-035 Min","-030 Min","-025 Min","-020 Min","-015 Min","-010 Min","-005 Min"," 000 Min"," 005 Min"," 010 Min",
            " 015 Min"," 020 Min"," 025 Min"," 030 Min"," 035 Min"," 040 Min"," 045 Min"," 050 Min"," 055 Min"," 060 Min"," 065 Min",
            " 070 Min"," 075 Min"," 080 Min"," 085 Min"," 090 Min"," 095 Min"," 100 Min"," 105 Min"," 110 Min"," 115 Min"," 120 Min" };
        /// <summary>
        /// 
        /// </summary>
        public static string[] CurrentRefValueArray = new string[] { "Percent of Imax", "Percent of Ib", "Direct Value" };

        /// <summary>
        /// 
        /// </summary>
        public static string[] VlotageRefValueArray = new string[] { "Percent of Vref", "Direct Value" };


        /// <summary>
        /// 
        /// </summary>
        public static string[] OperationBinaryArray = new string[] { ">", "<", ">=", "<=", "==", "!=" };

        /// <summary>
        /// 
        /// </summary>
        public static string[] TrigonoArray = new string[] { "Sin", "Cos", "Tan", "Sin Inverse", "Cos Inverse", "Tan Inverse" };


        /// <summary>
        /// 
        /// </summary>
        public static string[] MathFunctionArray = new string[] { "Sqrt" };

        /// <summary>
        /// 
        /// </summary>
        public static string[] DosageTypeArray = new string[] { "Active", "Reactive", "Apparent" };
        /// <summary>
        /// 
        /// </summary>
        public static string[] MeteringModeValueArray = new string[] { "0Forwarded", "1NetMetering" };
        /// <summary>
        /// 
        /// </summary>
        public static string[] ApparentCalValueArray = new string[] { "Lag only", "Lag + Lead" };
        /// <summary>
        /// 
        /// </summary>
        public static string[] CalLEDConfigValueArray = new string[] { "kvar Total", "kVAh", "kvar Lag", "kvar Lead", "kWh Export" };

        public static string MeterType = string.Empty;
        public static double Vref = 0.0;
        public static double Ib = 0.0;
        public static double Imax = 0.0;
        public static string MeterClass = string.Empty;
        public static string MeterConnectionType = string.Empty;
        public static string MeterCategoryType = string.Empty; //(WC/CT)
        public static string MeterSerialNumber = string.Empty;
        public static string MeterCategory = string.Empty; //C1 C2 C3 D1 D2 D3 D4 A B
        /// <summary>
        /// This string contains the array of Selective data by Entry
        /// 0 index -> Read start index
        /// 1 index -> Number of Count
        /// </summary>
        //public static string[] SelectiveDataByEntryArray = new string[] { "", "" };

        //public static string[] SelectiveDataByRangeArray = new string[] { "", "" };

        /// <summary>
        /// 
        /// </summary>
        public static Boolean IsMeterSendingData = false;
        /// <summary>
        /// 
        /// </summary>
        public static Boolean QuickSetting = false;
        /// <summary>
        /// 
        /// </summary>
        public static bool IsPostValid = false;

        /// <summary>
        /// Folder
        /// </summary>
        public static int TestcaseId = 0;

        /// <summary>
        /// Folder
        /// </summary>
        public static TreeView tvSelectedTree = null;
        /// <summary>
        /// Folder
        /// </summary>
        public static int? inputEnergy = null;
        /// <summary>
        /// Folder
        /// </summary>
        public static int seletedExecutionID = 0;
        /// <summary>
        /// 
        /// </summary>
        public static Dictionary<string, string> powerSourceCurrentValueList = null;
        public static Dictionary<string, double> powerSourceAppliedParameters = null;
        /// <summary>
        /// Folder
        /// </summary>
        public static Dictionary<int, IList> Dic_execution_Collection = new Dictionary<int, IList>();
        /// <summary>
        /// 
        /// </summary>
        public static List<string> OutputRegisterList = new List<string>();

        public static int executionId = int.MinValue;
        #endregion

        #region By YS
        public static int ConvertHexToDecimal(string hexString)
        {

            int decimalValue = Convert.ToInt32(hexString, 16);
            return decimalValue;
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
        #endregion By YS

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="cb"></param>
        public static void loadComboBox(string[] data, ComboBox cb, int selectedIndex = 0,
            bool isDefault = false, string defaultTextValue = "", bool isFileMenuCombo = false)
        {
            //Delete if anything exist
            cb.Items.Clear();
            //This will add the value to 
            if (isDefault)
                cb.Items.Insert(0, defaultTextValue);
            //
            foreach (string x in data)
            {
                cb.Items.Add(x);
            }
            if (isFileMenuCombo && (!string.IsNullOrEmpty(SessionPowerSourceCommunication.CalibrationFileName)))
            {
                int index2 = Array.IndexOf(data, SessionPowerSourceCommunication.CalibrationFileName);
                cb.SelectedIndex = index2 + 1;//Because we are inserting zero index
            }
            else
            {
                //if(cb.SelectedIndex != -1)
                //   cb.SelectedIndex = selectedIndex;
                cb.SelectedIndex = cb.FindString(defaultTextValue);
            }
        }

        public static string ReadDataFromObjectLNCipher(int _class, string _obis, int _attribute)
        {
            string result = string.Empty;
            DLMSParser parse = new DLMSParser();
            DLMSComm DLMSReader = new DLMSComm(SessionGlobalMeterCommunication.comPort, SessionGlobalMeterCommunication.BaudRate);
            try
            {
                if (!DLMSReader.SignOnDLMS())
                {
                    CommonHelper.DisplayDLMSSignONError();
                    return result;
                }
                byte bytWait = 0;
                byte bytTimOut = 3;
                string readData;
                bool parameter = false;
                parameter = DLMSReader.GetParameter($"{Convert.ToByte(_class).ToString("X4")}{string.Concat(_obis.Trim().Split('.').Select(part => int.Parse(part).ToString("X2")))}{Convert.ToByte(_attribute).ToString("X2")}", bytWait, bytTimOut, 3, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                if (parameter)
                {
                    readData = DLMSReader.strbldDLMdata.ToString().Trim().Split(' ')[3].Trim();
                    result = $"{readData}";
                }
                else
                {
                    log.Error($"Error Getting in Class-{_class} Obis- {_obis} Att. {_attribute}. Received data is: {DLMSReader.strbldDLMdata.ToString().Trim()}");
                }
                DLMSReader.SetDISCMode();
            }
            catch (Exception ex)
            {
                log.Error("Error in fetching Entries from Utilities.ReadDataFromObjectLNCipher " + ex.Message.ToString());
            }
            finally
            {
                DLMSReader.Dispose();
            }
            return result;
        }
        public static string GetDataFromObject(ref DLMSComm DLMSReader, int _class, string _obis, int _attribute, bool IsLineTrafficEnabled = true)
        {
            string result = string.Empty;
            try
            {
                byte bytWait = 0;
                byte bytTimOut = 3;
                string readData;
                bool parameter = false;

                //parameter = DLMSReader.GetParameter($"{Convert.ToByte(_class).ToString("X4")}{string.Concat(_obis.Trim().Split('.').Select(part => int.Parse(part).ToString("X2")))}{Convert.ToByte(_attribute).ToString("X2")}", bytWait, bytTimOut, 3, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);//OLD

                parameter = DLMSReader.GetParameter($"{Convert.ToByte(_class).ToString("X4")}{string.Concat(_obis.Trim().Split('.').Select(part => int.Parse(part).ToString("X2")))}{Convert.ToByte(_attribute).ToString("X2")}", bytWait, (byte)2, bytTimOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL, IsLineTrafficEnabled);
                if (parameter)
                {
                    readData = DLMSReader.strbldDLMdata.ToString().Trim().Split(' ')[3].Trim();
                    result = $"{readData}";
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="myRtb"></param>
        /// <param name="word"></param>
        /// <param name="color"></param>
        public static void HighlightText(this RichTextBox myRtb, string word, Color color)
        {

            if (word == string.Empty)
                return;

            int s_start = myRtb.SelectionStart, startIndex = 0, index;

            while ((index = myRtb.Text.IndexOf(word, startIndex)) != -1)
            {
                myRtb.Select(index, word.Length);
                myRtb.SelectionColor = color;

                startIndex = index + word.Length;
            }

            myRtb.SelectionStart = s_start;
            myRtb.SelectionLength = 0;
            myRtb.SelectionColor = Color.Black;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ComapanyName"></param>
        /// <returns></returns>
        public static string GetCompanyName(string ComapanyName)
        {
            string Name = string.Empty;
            for (int i = 4; i < ComapanyName.Length; i += 2)
            {
                if (ComapanyName.Substring(i, 2) != "00")
                    Name += Convert.ToChar(int.Parse(ComapanyName.Substring(i, 2), NumberStyles.HexNumber)).ToString();
            }
            return Name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="intMtrTyp"></param>
        /// <returns></returns>
        public static string ConvertMeterType(int intMtrTyp)
        {
            string retval = string.Empty;
            string binaryvalue = ToBinary(intMtrTyp).PadLeft(16, '0');

            if (binaryvalue.Substring(0, 1) == "0")
                retval = retval + "1P-";
            else
                retval = retval + "3P-";

            if (binaryvalue.Substring(1, 2) == "00")
                retval = retval + "1W,";
            else if (binaryvalue.Substring(1, 2) == "01")
                retval = retval + "2W,";
            else if (binaryvalue.Substring(1, 2) == "10")
                retval = retval + "3W,";
            else if (binaryvalue.Substring(1, 2) == "11")
                retval = retval + "4W,";

            if (binaryvalue.Substring(3, 2) == "00")
                retval = retval + "Class 0.2S,";
            else if (binaryvalue.Substring(3, 2) == "01")
                retval = retval + "Class 0.5S,";
            else if (binaryvalue.Substring(3, 2) == "10")
                retval = retval + "Class 1,";
            else if (binaryvalue.Substring(3, 2) == "11")
                retval = retval + "Class 2,";

            if (binaryvalue.Substring(5, 1) == "0")
                retval = retval + "CT,";
            else
                retval = retval + "WC,";

            if (binaryvalue.Substring(6, 3) == "000")
                retval = retval + "63.5 V,";
            else if (binaryvalue.Substring(6, 3) == "001")
                retval = retval + "110 V,";
            else if (binaryvalue.Substring(6, 3) == "010")
                retval = retval + "120 V,";
            else if (binaryvalue.Substring(6, 3) == "011")
                retval = retval + "220 V,";
            else if (binaryvalue.Substring(6, 3) == "100")
                retval = retval + "230 V,";
            else if (binaryvalue.Substring(6, 3) == "101")
                retval = retval + "240 V,";
            else if (binaryvalue.Substring(6, 3) == "110")
                retval = retval + "Reserved,";
            else if (binaryvalue.Substring(6, 3) == "111")
                retval = retval + "Reserved,";

            if (binaryvalue.Substring(9, 3) == "000")
                retval = retval + "1-";
            else if (binaryvalue.Substring(9, 3) == "001")
                retval = retval + "2.5-";
            else if (binaryvalue.Substring(9, 3) == "010")
                retval = retval + "5-";
            else if (binaryvalue.Substring(9, 3) == "011")
                retval = retval + "10-";
            else if (binaryvalue.Substring(9, 3) == "100")
                retval = retval + "15-";
            else if (binaryvalue.Substring(9, 3) == "101")
                retval = retval + "20-";
            else if (binaryvalue.Substring(9, 3) == "110")
                retval = retval + "30-";
            else if (binaryvalue.Substring(9, 3) == "111")
                retval = retval + "40-";

            if (binaryvalue.Substring(12, 4) == "0000")
                retval = retval + "1.2 A";
            else if (binaryvalue.Substring(12, 4) == "0001")
                retval = retval + "2 A";
            else if (binaryvalue.Substring(12, 4) == "0010")
                retval = retval + "6 A";
            else if (binaryvalue.Substring(12, 4) == "0011")
                retval = retval + "10 A";
            else if (binaryvalue.Substring(12, 4) == "0100")
                retval = retval + "20 A";
            else if (binaryvalue.Substring(12, 4) == "0101")
                retval = retval + "30 A";
            else if (binaryvalue.Substring(12, 4) == "0110")
                retval = retval + "40 A";
            else if (binaryvalue.Substring(12, 4) == "0111")
                retval = retval + "60 A";
            else if (binaryvalue.Substring(12, 4) == "1000")
                retval = retval + "80 A";
            else if (binaryvalue.Substring(12, 4) == "1001")
                retval = retval + "100 A";
            else if (binaryvalue.Substring(12, 4) == "1010")
                retval = retval + "120 A";
            else if (binaryvalue.Substring(12, 4) == "1011")
                retval = retval + "7.5 A";
            else if (binaryvalue.Substring(12, 4) == "1100")
                retval = retval + "200 A";
            else if (binaryvalue.Substring(12, 4) == "1101")
                retval = retval + "300 A";
            MeterType = retval;
            //3P-4W,Class 1,WC,240 V,10-60 A
            try
            {
                MeterConnectionType = MeterType.Split(',')[0].Trim();
                MeterClass = MeterType.Split(',')[1].Trim();
                MeterCategoryType = MeterType.Split(',')[2].Trim();
                Vref = Convert.ToDouble(MeterType.Split(',')[3].Trim().Split(' ')[0].Trim());
                Ib = Convert.ToDouble(MeterType.Split(',')[4].Trim().Split('-')[0].Trim());
                Imax = Convert.ToDouble(MeterType.Split(',')[4].Trim().Split('-')[1].Trim().Split(' ')[0].Trim());
            }
            catch { }
            return retval;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Decimal"></param>
        /// <returns></returns>
        public static string ToBinary(Int64 Decimal)
        {
            // Declare a few variables we're going to need
            Int64 BinaryHolder;
            char[] BinaryArray;
            string BinaryResult = "";

            while (Decimal > 0)
            {
                BinaryHolder = Decimal % 2;
                BinaryResult += BinaryHolder;
                Decimal = Decimal / 2;
            }

            // The algoritm gives us the binary number in reverse order (mirrored)
            // We store it in an array so that we can reverse it back to normal
            BinaryArray = BinaryResult.ToCharArray();
            Array.Reverse(BinaryArray);
            BinaryResult = new string(BinaryArray);

            return BinaryResult;
        }

        #region helper method for date time range check
        public static bool IsDateTimeRangeCorrect(string refData)
        {
            int month = int.Parse(refData.Substring(8, 2), System.Globalization.NumberStyles.HexNumber);
            if (!(month <= 12) || !(month >= 1))
            {
                return false;
            }
            int day = day = int.Parse(refData.Substring(10, 2), System.Globalization.NumberStyles.HexNumber);
            if (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12)
            {
                if (!(day <= 31) || !(day >= 1))
                {
                    return false;
                }
            }
            else if (month == 4 || month == 6 || month == 9 || month == 11)
            {
                if (!(day <= 30) || !(day >= 1))
                {
                    return false;
                }
            }
            else if (month == 2)
            {
                if (int.Parse(refData.Substring(4, 2), System.Globalization.NumberStyles.HexNumber) % 4 == 0)
                {
                    if (!(day <= 29) || !(day >= 1))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!(day <= 28) || !(day >= 1))
                    {
                        return false;
                    }
                }
            }
            int hour = int.Parse(refData.Substring(14, 2), System.Globalization.NumberStyles.HexNumber);
            if (!(hour <= 23) || !(hour >= 0))
            {
                return false;
            }
            int min = int.Parse(refData.Substring(16, 2), System.Globalization.NumberStyles.HexNumber);
            if (!(min <= 59) || !(hour >= 0))
            {
                return false;
            }
            int sec = int.Parse(refData.Substring(18, 2), System.Globalization.NumberStyles.HexNumber);
            if (!(sec <= 59) || !(sec >= 0))
            {
                return false;
            }
            int valueDayOfWeek = int.Parse(refData.Substring(12, 2), System.Globalization.NumberStyles.HexNumber);
            if (!(valueDayOfWeek != 0 && (valueDayOfWeek >= 1 && valueDayOfWeek <= 7 || valueDayOfWeek == 255)))
            {
                return false;
            }
            return true;
        }
        #endregion

        public static void ShowNotFoundElementsDialog(string dialogText, List<string> notFoundElements)
        {
            using (var dialog = new Form())
            {
                dialog.ShowIcon = false; // Hide the icon of the dialog form
                dialog.ControlBox = false; // Disable the control box of the dialog form
                dialog.Size = new System.Drawing.Size(600, 400); // Set width to 400 and height to 300
                dialog.Text = dialogText;
                dialog.StartPosition = FormStartPosition.CenterScreen;

                var textBox = new TextBox();
                textBox.Font = new System.Drawing.Font(textBox.Font.FontFamily, 12); // Set font size to 12
                textBox.Multiline = true;
                textBox.ReadOnly = true;
                textBox.ScrollBars = ScrollBars.Vertical;
                textBox.Dock = DockStyle.Fill;

                foreach (var element in notFoundElements)
                {
                    textBox.AppendText(element + Environment.NewLine);
                }

                dialog.Controls.Add(textBox);

                var closeButton = new Button();
                closeButton.Text = "Close";
                closeButton.Height = 30; // Set the height to 50 pixels
                closeButton.Font = new System.Drawing.Font(closeButton.Font.FontFamily, 12, System.Drawing.FontStyle.Bold); // Set font size to 12 and make it bold
                closeButton.Dock = DockStyle.Bottom;
                closeButton.Click += (sender, e) => dialog.Close();

                dialog.Controls.Add(closeButton);

                dialog.ShowDialog();
            }
        }
        public static string ShowComboBoxDialog(IWin32Window owner, string formText, string[] addRange)
        {
            // Create a new form for the dialog
            Form dialog = new Form
            {
                Text = formText,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                TopMost = true,
                MinimizeBox = false,
                MaximizeBox = false,
                Width = 300,
                Height = 150
            };

            // Initialize ComboBox
            ComboBox cmbCategories = new ComboBox
            {
                Location = new System.Drawing.Point(30, 20),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbCategories.Items.AddRange(addRange);
            dialog.Controls.Add(cmbCategories);
            cmbCategories.SelectedIndex = 0;
            // Variable to hold the selected category
            string selectedCategory = null;

            // Initialize OK button
            Button btnOK = new Button
            {
                Text = "OK",
                Location = new System.Drawing.Point(60, 60),
                DialogResult = DialogResult.OK
            };
            btnOK.Click += (sender, e) =>
            {
                selectedCategory = cmbCategories.SelectedItem?.ToString();
                dialog.DialogResult = DialogResult.OK;
                dialog.Close();
            };
            dialog.Controls.Add(btnOK);

            // Initialize Cancel button
            Button btnCancel = new Button
            {
                Text = "Cancel",
                Location = new System.Drawing.Point(150, 60),
                DialogResult = DialogResult.Cancel
            };
            btnCancel.Click += (sender, e) =>
            {
                selectedCategory = null;
                dialog.DialogResult = DialogResult.Cancel;
                dialog.Close();
            };
            dialog.Controls.Add(btnCancel);

            // Show the dialog and return the selected category or null if canceled
            return dialog.ShowDialog(owner) == DialogResult.OK ? selectedCategory : null;
        }
        public static string ShowComboBoxDialog(IWin32Window owner, string formText, string label1Text, string label2Text, string[] addRange1, string[] addRange2)
        {
            // Create a new form for the dialog
            Form dialog = new Form
            {
                Text = formText,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                TopMost = true,
                MinimizeBox = false,
                MaximizeBox = false,
                Width = 500,
                Height = 140
            };
            Label label1 = new Label
            {
                Location = new System.Drawing.Point(25, 10),
                Width = 200,
                Text = label1Text,
            };
            Label label2 = new Label
            {
                Location = new System.Drawing.Point(260, 10),
                Width = 200,
                Text = label2Text,
            };
            dialog.Controls.Add(label1);
            dialog.Controls.Add(label2);
            // Initialize ComboBox1
            ComboBox cmb1 = new ComboBox
            {
                Location = new System.Drawing.Point(25, 35),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmb1.Items.AddRange(addRange1);
            dialog.Controls.Add(cmb1);
            cmb1.SelectedIndex = 0;

            // Initialize ComboBox2
            ComboBox cmb2 = new ComboBox
            {
                Location = new System.Drawing.Point(260, 35),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmb2.Items.AddRange(addRange2);
            dialog.Controls.Add(cmb2);
            cmb2.SelectedIndex = 0;
            // Variable to hold the selected category
            string Vref_Imax = null;

            // Initialize OK button
            Button btnOK = new Button
            {
                Text = "OK",
                Width = 100,
                Location = new System.Drawing.Point(80, 70),
                DialogResult = DialogResult.OK
            };
            btnOK.Click += (sender, e) =>
            {
                Vref_Imax = cmb1.SelectedItem?.ToString() + "|" + cmb2.SelectedItem?.ToString();
                dialog.DialogResult = DialogResult.OK;
                dialog.Close();
            };
            dialog.Controls.Add(btnOK);

            // Initialize Cancel button
            Button btnCancel = new Button
            {
                Text = "Cancel",
                Width = 100,
                Location = new System.Drawing.Point(310, 70),
                DialogResult = DialogResult.Cancel
            };
            btnCancel.Click += (sender, e) =>
            {
                Vref_Imax = null;
                dialog.DialogResult = DialogResult.Cancel;
                dialog.Close();
            };
            dialog.Controls.Add(btnCancel);

            // Show the dialog and return the selected category or null if canceled
            return dialog.ShowDialog(owner) == DialogResult.OK ? Vref_Imax : null;
        }
        public static string ShowComboBoxDialogAttributeCommandType(IWin32Window owner, string formText, string[] addRange)
        {
            // Create a new form for the dialog
            Form dialog = new Form
            {
                Text = formText,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                TopMost = true,
                MinimizeBox = false,
                MaximizeBox = false,
                Width = 300,
                Height = 150
            };

            // Initialize ComboBox
            ComboBox cmbCategories = new ComboBox
            {
                Location = new System.Drawing.Point(30, 20),
                Width = 100,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbCategories.Items.AddRange(addRange);
            dialog.Controls.Add(cmbCategories);
            cmbCategories.SelectedIndex = 0;
            // Variable to hold the selected category
            string selectedCategory = null;

            // Initialize ComboBox for command type
            ComboBox cmbCommandType = new ComboBox()
            {
                Location = new System.Drawing.Point(150, 20),
                Width = 100,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbCommandType.Items.AddRange(new string[] { "SET", "ACTION" });
            dialog.Controls.Add(cmbCommandType);
            cmbCommandType.SelectedIndex = 0;

            // Initialize OK button
            Button btnOK = new Button
            {
                Text = "OK",
                Location = new System.Drawing.Point(60, 60),
                DialogResult = DialogResult.OK
            };
            btnOK.Click += (sender, e) =>
            {
                selectedCategory = $"{cmbCategories.SelectedItem?.ToString()}-{cmbCommandType.SelectedItem?.ToString()}";
                dialog.DialogResult = DialogResult.OK;
                dialog.Close();
            };
            dialog.Controls.Add(btnOK);

            // Initialize Cancel button
            Button btnCancel = new Button
            {
                Text = "Cancel",
                Location = new System.Drawing.Point(150, 60),
                DialogResult = DialogResult.Cancel
            };
            btnCancel.Click += (sender, e) =>
            {
                selectedCategory = null;
                dialog.DialogResult = DialogResult.Cancel;
                dialog.Close();
            };
            dialog.Controls.Add(btnCancel);

            // Show the dialog and return the selected category or null if canceled
            return dialog.ShowDialog(owner) == DialogResult.OK ? selectedCategory : null;
        }

    }
}
