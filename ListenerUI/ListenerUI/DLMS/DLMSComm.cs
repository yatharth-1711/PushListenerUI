using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using MeterComm;
using System.Windows.Forms;
using AutoTest.FrameWork;
using AutoTest.FrameWork.Converts;
using AP_Source;
using System.Data;
using Org.BouncyCastle.Ocsp;
using System.Diagnostics.Eventing.Reader;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using System.Drawing;
using System.Runtime.InteropServices.ComTypes;
using System.Reflection;
using Org.BouncyCastle.X509.Extension;
using System.Runtime.InteropServices.WindowsRuntime;
using log4net;
using MeterComm.DLMS;
using System.Threading;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using ProgressBar = System.Windows.Forms.ProgressBar;
using System.Diagnostics;
using Org.BouncyCastle.Utilities.Encoders;
using log4net.Util;
using AesLib;

namespace MeterComm
{
    public class DLMSComm : IDisposable
    {
        #region Declaration
        //Logger
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        //delegate for Error Text
        public delegate void LineTrafficControl(string updatedText, string status);
        //initial  Event
        public static event LineTrafficControl LineTrafficControlEventHandler = delegate { }; // add empty delegate!;
        //Delegate for Line traffic save
        public delegate void LineTrafficSave();
        public static event LineTrafficSave LineTrafficSaveEventHandler = delegate { };// add empty delegate!
        MeterSerialCommunication oSC = null;
        // Reference to the MeterForm's DataGridView
        private DataGridView dataGridView;
        private ProgressBar PB_PGRead;
        private static string deviationByteDT = string.Empty;

        public byte bytAddMode = 0;
        private byte[] keyBytes = new byte[16];
        private byte[] plainText = new byte[16];
        private byte[] cipherText = new byte[16];
        private DLMSClass DCl = new DLMSClass();
        private byte[] nPkt = new byte[1024 * 4];
        private byte[] nRcvPkt = new byte[1024 * 4];
        private int nCounter = 0;
        private byte[] Ps = new byte[16];
        private byte[] Ps1 = new byte[16];
        private byte nRecv;
        private byte nRecvLast;
        private byte nRecvCntr;
        private byte nSent;
        private byte nSentLast;
        private byte nSentCntr;
        private byte nRetLSH;
        private byte[] buffer = new byte[1024 * 4];
        private int pktLength = 0;
        private byte nWait = 0;
        public byte nTryCount = 3;//5
        public byte nTimeOut = 3;
        private bool bGSMConnect = false;
        private int nCommandCounter = 0;
        private string sDLMRawData = string.Empty;
        private string sDedicatedkey = string.Empty;
        private string sDedicatedkeyinHEX;
        private string sEncryptkeyinHEX;
        private string sSYSTaginHEX;
        public bool bigSNRM = false;

        private long intConfBlk = 0;
        private int MeterType = 1;
        private int MtrCatagory = 2;
        private string temp = string.Empty;
        public StringBuilder strbldDLMdata = new StringBuilder();
        private byte WSTx { get; set; } = 0;
        private byte WSRx = 0;
        private int ISTx = 0;
        private int ISRx = 0;
        private int Fromvalue;
        private int Tovalue;
        private ulong FromEntry;
        private ulong ToEntry;
        private bool SelectAccess = false;
        private bool bSequence = false;
        //private string strLTFile;
        //private StreamWriter swLT;
        //private string strErrorFile;
        //private StreamWriter swError;
        private static Random random = new Random();
        public string commandString = string.Empty;
        public string responseString = string.Empty;
        public string conformanceBlockString = string.Empty;//this will hold the conformance block hex string
        public static CancellationToken token;
        public bool IsRRFrame = false;
        public bool IsUnknownCommandIdentifier = false;
        public bool IsN_R_is_1 = false;
        #endregion

        #region Constructor

        public DLMSComm(string ComPort, int BaudRate)
        {
            try
            {
                //oSC = new SerialCommunication(ComPort, BaudRate);// old
                oSC = new MeterSerialCommunication(ComPort, BaudRate);
                oSC.WaitForRespose = 0;
                oSC.AlwaysOpenPort = true;
                this.WSTx = Convert.ToByte(DLMSInfo.WSTx);
                this.WSRx = Convert.ToByte(DLMSInfo.WSRx);
                if (DLMSInfo.IFTx != "Default")
                    this.ISTx = (int)Convert.ToInt16(DLMSInfo.IFTx);
                if (DLMSInfo.IFRx != "Default")
                    this.ISRx = (int)Convert.ToInt16(DLMSInfo.IFRx);
                if (DLMSInfo.IFTx != "Default")
                    bigSNRM = true;
                //this.strErrorFile = Application.StartupPath + @"\LineTraffic\\" + $"ERROR_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.txt";
                //this.strLTFile = Application.StartupPath + @"\LineTraffic\\" + $"LINETRAFFIC_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.txt";
                if (DLMSInfo.AddressModeText == "1")
                    bytAddMode = Convert.ToByte(3);
                else
                    bytAddMode = Convert.ToByte(0);
            }
            catch
            { }
        }
        public DLMSComm(string ComPort, int BaudRate, DataGridView dataGridView, ProgressBar PB_PGRead)
        {
            //oSC = new SerialCommunication(ComPort, BaudRate);// old
            oSC = new MeterSerialCommunication(ComPort, BaudRate);
            oSC.WaitForRespose = 0;
            oSC.AlwaysOpenPort = true;
            this.dataGridView = dataGridView;
            this.PB_PGRead = PB_PGRead;
            this.WSTx = Convert.ToByte(DLMSInfo.WSTx);
            this.WSRx = Convert.ToByte(DLMSInfo.WSRx);
            if (DLMSInfo.IFTx != "Default")
                this.ISTx = (int)Convert.ToInt16(DLMSInfo.IFTx);
            if (DLMSInfo.IFRx != "Default")
                this.ISRx = (int)Convert.ToInt16(DLMSInfo.IFRx);
            if (DLMSInfo.IFTx != "Default")
                bigSNRM = true;
            //this.strErrorFile = Application.StartupPath + @"\LineTraffic\\" + $"ERROR_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.txt";
            //this.strLTFile = Application.StartupPath + @"\LineTraffic\\" + $"LINETRAFFIC_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.txt";
            if (DLMSInfo.AddressModeText == "1")
                bytAddMode = Convert.ToByte(3);
            else
                bytAddMode = Convert.ToByte(0);
        }
        #endregion

        #region DLMS Methods
        public void AddressInit(int nAssLevel)
        {
            int num1 = (nAssLevel != 3 ? Convert.ToInt32((Convert.ToInt32(nAssLevel) + 1) * 16 << 1) : Convert.ToInt32((Convert.ToInt32(3) + 1) * 16 << 1)) + 1;
            this.nPkt[0] = (byte)126;//7E
            this.nPkt[1] = (byte)160;//A0
            if (this.bytAddMode == (byte)0)
            {
                //this.nPkt[3] = Convert.ToByte((Convert.ToInt32(Convert.ToInt32(this.CmbLDA.Text) << 1) + 1) % 256);
                this.nPkt[3] = Convert.ToByte((Convert.ToInt32(Convert.ToInt32(DLMSInfo.LogicalAddress) << 1) + 1) % 256);
                this.nPkt[4] = Convert.ToByte(num1);
            }
            else
            {
                int num2 = Convert.ToInt32(Convert.ToInt32(DLMSInfo.LogicalAddress) << 1);
                int num3 = Convert.ToInt32(Convert.ToInt32(DLMSInfo.PhysicalAddress) << 1) + 1;
                this.nPkt[3] = Convert.ToByte(num2 / 256);
                this.nPkt[4] = Convert.ToByte(num2 % 256);
                this.nPkt[6] = Convert.ToByte(num3 % 256);
                this.nPkt[5] = Convert.ToByte(num3 / 256 << 1);
                this.nPkt[7] = Convert.ToByte(num1);

                //int int32 = Convert.ToInt32(Convert.ToInt32(DLMSInfo.LogicalAddress) << 1);
                //int num2 = Convert.ToInt32(Convert.ToInt32(DLMSInfo.PhysicalAddress) << 1) + 1;
                //this.nPkt[3] = Convert.ToByte(int32 / 256);
                //this.nPkt[4] = Convert.ToByte(int32 % 256);
                //this.nPkt[6] = Convert.ToByte(num2 % 256);
                //this.nPkt[5] = Convert.ToByte(num2 / 256 << 1);
                //this.nPkt[7] = Convert.ToByte(num1);
            }
            this.nRecv = (byte)0;
            this.nRecvLast = (byte)0;
            this.nRecvCntr = (byte)0;
            this.nSent = (byte)0;
            this.nSentLast = (byte)0;
            this.nSentCntr = (byte)0;
            //this.nCommandCounter = DateTime.Now.Second;
        }
        public void AddressInit()
        {
            int num1 = Convert.ToInt32((Convert.ToInt32(DLMSInfo.AccessMode) + 1) * 16 << 1) + 1;
            this.nPkt[0] = (byte)126;
            this.nPkt[1] = (byte)160;
            if (this.bytAddMode == (byte)0)
            {
                //this.CmbTyp.SelectedIndex 0=Meter 1 Modem
                //this.nPkt[3] = Convert.ToByte(((this.CmbTyp.SelectedIndex != 0 ? 20 : Convert.ToInt32(Convert.ToInt32(DLMSInfo.LogicalAddress) << 1)) + 1) % 256);
                this.nPkt[3] = Convert.ToByte(((1 != 0 ? 20 : Convert.ToInt32(Convert.ToInt32(DLMSInfo.LogicalAddress) << 1)) + 1) % 256);

                this.nPkt[4] = Convert.ToByte(num1);
            }
            else
            {
                int num2 = Convert.ToInt32(Convert.ToInt32(DLMSInfo.LogicalAddress) << 1);
                int num3 = Convert.ToInt32(Convert.ToInt32(DLMSInfo.PhysicalAddress) << 1) + 1;
                this.nPkt[3] = Convert.ToByte(num2 / 256);
                this.nPkt[4] = Convert.ToByte(num2 % 256);
                this.nPkt[6] = Convert.ToByte(num3 % 256);
                this.nPkt[5] = Convert.ToByte(num3 / 256 << 1);
                this.nPkt[7] = Convert.ToByte(num1);

                //int num2 = 1 != 0 ? 20 : Convert.ToInt32(Convert.ToInt32(DLMSInfo.LogicalAddress) << 1);
                //int num3 = Convert.ToInt32(Convert.ToInt32(DLMSInfo.PhysicalAddress) << 1) + 1;
                //this.nPkt[3] = Convert.ToByte(num2 / 256);
                //this.nPkt[4] = Convert.ToByte(num2 % 256);
                //this.nPkt[6] = Convert.ToByte(num3 % 256);
                //this.nPkt[5] = Convert.ToByte(num3 / 256 << 1);
                //this.nPkt[7] = Convert.ToByte(num1);
            }
            this.nRecv = (byte)0;
            this.nRecvLast = (byte)0;
            this.nRecvCntr = (byte)0;
            this.nSent = (byte)0;
            this.nSentLast = (byte)0;
            this.nSentCntr = (byte)0;
        }
        public void AddressInitFG()
        {
            int num1 = Convert.ToInt32((Convert.ToInt32(3) + 1) * 16 << 1) + 1;
            this.nPkt[0] = (byte)126;
            this.nPkt[1] = (byte)160;
            if (this.bytAddMode == (byte)0)
            {
                this.nPkt[3] = Convert.ToByte((Convert.ToInt32(2) + 1) % 256);
                this.nPkt[4] = Convert.ToByte(num1);
            }
            else
            {
                int int32 = Convert.ToInt32(2);
                int num2 = Convert.ToInt32(Convert.ToInt32(2)) + 1;
                this.nPkt[3] = Convert.ToByte(int32 / 256);
                this.nPkt[4] = Convert.ToByte(int32 % 256);
                this.nPkt[6] = Convert.ToByte(num2 % 256);
                this.nPkt[5] = Convert.ToByte(num2 / 256 << 1);
                this.nPkt[7] = Convert.ToByte(num1);
            }
            this.nRecv = (byte)0;
            this.nRecvLast = (byte)0;
            this.nRecvCntr = (byte)0;
            this.nSent = (byte)0;
            this.nSentLast = (byte)0;
            this.nSentCntr = (byte)0;
        }
        public bool SetNRM(bool bigSNRM, bool IsLineTrafficEnabled = true)
        {
            bool flag1 = false;
            byte num1 = Convert.ToByte(5 + (int)this.bytAddMode);
            nPkt[2] = Convert.ToByte(7 + (int)this.bytAddMode);
            byte[] nPkt1 = this.nPkt;
            int index1 = (int)num1;
            byte num2 = (byte)(index1 + 1);
            nPkt1[index1] = (byte)83;
            nPkt[(int)num2 + 2] = (byte)126;
            DCl.fcs(ref this.nPkt, (int)Convert.ToByte(5 + (int)this.bytAddMode), (byte)1);
            ClearBuffer();
            this.temp = string.Empty;
            for (int index2 = 0; index2 < (num2 + 3); ++index2)
                this.temp += this.nPkt[index2].ToString("X2");
            this.temp += "\r\n";
            SendPkt(this.nPkt, (ushort)Convert.ToByte((int)num2 + 3));
            DateTime now1 = DateTime.Now;
            DateTime now2;
            TimeSpan timeSpan;
            while (true)
            {
                Wait(5.0);
                Application.DoEvents();
                DataReceive();
                if (this.nCounter <= 2 || (int)this.nRcvPkt[2] + 2 > this.nCounter || !this.DCl.fcs(ref this.nRcvPkt, int.Parse(this.nRcvPkt[2].ToString()), (byte)0))
                {
                    now2 = DateTime.Now;
                    timeSpan = now2.Subtract(now1);
                    if (timeSpan.Seconds > (int)this.nTimeOut)
                        goto label_5;
                }
                else
                    break;
            }
            flag1 = true;
        label_5:
            this.temp = string.Empty;
            for (int index2 = 0; index2 < this.nCounter; ++index2)
                this.temp += this.nRcvPkt[index2].ToString("X2");
            this.temp += "\r\n";
            //this.swLT.Write("(R)\t" + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss:ff tt") + "\t" + this.temp);
            byte num3 = Convert.ToByte(5 + (int)this.bytAddMode);
            this.nPkt[2] = Convert.ToByte(7 + (int)this.bytAddMode);
            byte[] nPkt2 = this.nPkt;
            int index3 = (int)num3;
            byte num4 = (byte)(index3 + 1);
            nPkt2[index3] = (byte)147;
            this.ClearBuffer();
            flag1 = false;
            if (!bigSNRM)
            {
                this.DCl.fcs(ref this.nPkt, (int)Convert.ToByte((int)this.bytAddMode + 5), (byte)1);
                this.nPkt[(int)num4 + 2] = (byte)126;
            }
            else
            {
                byte num5 = Convert.ToByte(8 + (int)this.bytAddMode);
                byte[] nPkt3 = this.nPkt;
                int index4 = (int)num5;
                byte num6 = (byte)(index4 + 1);
                nPkt3[index4] = (byte)129;
                byte[] nPkt4 = this.nPkt;
                int index5 = (int)num6;
                byte num7 = (byte)(index5 + 1);
                nPkt4[index5] = (byte)128;
                byte[] nPkt5 = this.nPkt;
                int index6 = (int)num7;
                byte num8 = (byte)(index6 + 1);
                nPkt5[index6] = (byte)18;
                byte[] nPkt6 = this.nPkt;
                int index7 = (int)num8;
                byte num9 = (byte)(index7 + 1);
                nPkt6[index7] = (byte)5;
                byte num10;
                if (this.ISTx < (int)byte.MaxValue)
                {
                    byte[] nPkt7 = this.nPkt;
                    int index8 = (int)num9;
                    byte num11 = (byte)(index8 + 1);
                    nPkt7[index8] = (byte)1;
                    byte[] nPkt8 = this.nPkt;
                    int index9 = (int)num11;
                    num10 = (byte)(index9 + 1);
                    int num12 = (int)Convert.ToByte(this.ISTx);
                    nPkt8[index9] = (byte)num12;
                }
                else
                {
                    byte[] nPkt9 = this.nPkt;
                    int index10 = (int)num9;
                    byte num13 = (byte)(index10 + 1);
                    nPkt9[index10] = (byte)2;
                    byte[] nPkt10 = this.nPkt;
                    int index11 = (int)num13;
                    byte num14 = (byte)(index11 + 1);
                    int num15 = (int)Convert.ToByte(this.ISTx / 256);
                    nPkt10[index11] = (byte)num15;
                    byte[] nPkt11 = this.nPkt;
                    int index12 = (int)num14;
                    num10 = (byte)(index12 + 1);
                    int num16 = (int)Convert.ToByte(this.ISTx % 256);
                    nPkt11[index12] = (byte)num16;
                }
                byte[] nPkt12 = this.nPkt;
                int index13 = (int)num10;
                byte num17 = (byte)(index13 + 1);
                nPkt12[index13] = (byte)6;
                byte num18;
                if (this.ISRx < (int)byte.MaxValue)
                {
                    byte[] nPkt13 = this.nPkt;
                    int index14 = (int)num17;
                    byte num19 = (byte)(index14 + 1);
                    nPkt13[index14] = (byte)1;
                    byte[] nPkt14 = this.nPkt;
                    int index15 = (int)num19;
                    num18 = (byte)(index15 + 1);
                    int num20 = (int)Convert.ToByte(this.ISRx);
                    nPkt14[index15] = (byte)num20;
                }
                else
                {
                    byte[] nPkt15 = this.nPkt;
                    int index16 = (int)num17;
                    byte num21 = (byte)(index16 + 1);
                    nPkt15[index16] = (byte)2;
                    byte[] nPkt16 = this.nPkt;
                    int index17 = (int)num21;
                    byte num22 = (byte)(index17 + 1);
                    int num23 = (int)Convert.ToByte(this.ISRx / 256);
                    nPkt16[index17] = (byte)num23;
                    byte[] nPkt17 = this.nPkt;
                    int index18 = (int)num22;
                    num18 = (byte)(index18 + 1);
                    int num24 = (int)Convert.ToByte(this.ISRx % 256);
                    nPkt17[index18] = (byte)num24;
                }
                byte[] nPkt18 = this.nPkt;
                int index19 = (int)num18;
                byte num25 = (byte)(index19 + 1);
                nPkt18[index19] = (byte)7;
                byte[] nPkt19 = this.nPkt;
                int index20 = (int)num25;
                byte num26 = (byte)(index20 + 1);
                nPkt19[index20] = (byte)4;
                byte[] nPkt20 = this.nPkt;
                int index21 = (int)num26;
                byte num27 = (byte)(index21 + 1);
                nPkt20[index21] = (byte)0;
                byte[] nPkt21 = this.nPkt;
                int index22 = (int)num27;
                byte num28 = (byte)(index22 + 1);
                nPkt21[index22] = (byte)0;
                byte[] nPkt22 = this.nPkt;
                int index23 = (int)num28;
                byte num29 = (byte)(index23 + 1);
                nPkt22[index23] = (byte)0;
                byte[] nPkt23 = this.nPkt;
                int index24 = (int)num29;
                byte num30 = (byte)(index24 + 1);
                int wsTx = (int)this.WSTx;
                nPkt23[index24] = (byte)wsTx;
                byte[] nPkt24 = this.nPkt;
                int index25 = (int)num30;
                byte num31 = (byte)(index25 + 1);
                nPkt24[index25] = (byte)8;
                byte[] nPkt25 = this.nPkt;
                int index26 = (int)num31;
                byte num32 = (byte)(index26 + 1);
                nPkt25[index26] = (byte)4;
                byte[] nPkt26 = this.nPkt;
                int index27 = (int)num32;
                byte num33 = (byte)(index27 + 1);
                nPkt26[index27] = (byte)0;
                byte[] nPkt27 = this.nPkt;
                int index28 = (int)num33;
                byte num34 = (byte)(index28 + 1);
                nPkt27[index28] = (byte)0;
                byte[] nPkt28 = this.nPkt;
                int index29 = (int)num34;
                byte num35 = (byte)(index29 + 1);
                nPkt28[index29] = (byte)0;
                byte[] nPkt29 = this.nPkt;
                int index30 = (int)num35;
                num4 = (byte)(index30 + 1);
                int wsRx = (int)this.WSRx;
                nPkt29[index30] = (byte)wsRx;
                this.nPkt[2] = Convert.ToByte((int)num4 + 1);
                this.DCl.fcs(ref this.nPkt, (int)Convert.ToByte((int)this.bytAddMode + 5), (byte)1);
                this.DCl.fcs(ref this.nPkt, (int)Convert.ToByte((int)num4 - 1), (byte)1);
                this.nPkt[(int)num4 + 2] = (byte)126;
            }
            byte num36 = 0;
            bool flag2;
            do
            {
                this.ClearBuffer();
                flag2 = false;
                this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)num4 + 3));
                if (IsLineTrafficEnabled)
                {
                    LineTrafficControlEventHandler("\r\n     SNRM\r\n", "Send");
                    if (bigSNRM)
                        LineTrafficControlEventHandler("     Proposed Window Size Tx = " + this.WSTx.ToString() + ", Window Size Rx = " + this.WSRx.ToString() + ", Info Field Size Tx = " + this.ISTx.ToString() + ", Info Field Size Rx = " + this.ISRx.ToString(), "Send");
                }
                if (IsLineTrafficEnabled)
                    SendDataPrint(nPkt, Convert.ToByte((int)num4 + 3));
                commandString = string.Empty;
                for (int index2 = 0; index2 < (num4 + 3); ++index2)
                    commandString += this.nPkt[index2].ToString("X2");
                DateTime now3 = DateTime.Now;
                do
                {
                    this.Wait(5);
                    Application.DoEvents();
                    this.DataReceive();
                    if (this.nCounter > 2 && (int)this.nRcvPkt[2] + 2 <= this.nCounter && this.nRcvPkt[(int)this.nRcvPkt[2] + 1] == (byte)126 && this.DCl.fcs(ref this.nRcvPkt, int.Parse(this.nRcvPkt[2].ToString()), (byte)0))
                    {
                        flag2 = true;
                        break;
                    }
                    now2 = DateTime.Now;
                    timeSpan = now2.Subtract(now3);
                    if (timeSpan.Seconds > (int)this.nTimeOut && (int)num36 < (int)this.nTryCount)
                    {
                        ++num36;
                        break;
                    }
                }
                while (!flag2);
            }
            while (!flag2 && (int)num36 != (int)this.nTryCount);
            RecvDataPrint(nRcvPkt, nCounter, IsLineTrafficEnabled);
            responseString = string.Empty;
            for (int index31 = 0; index31 < this.nCounter; ++index31)
                responseString += this.nRcvPkt[index31].ToString("X2");
            //this.swLT.Write("(R)\t" + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss:ff tt") + "\t" + this.temp);
            if (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] != (byte)115)
                return false;
            byte num37 = 11;
            int tempWSTx = 0, tempWSRx = 0, tempISTx = 0, tempISRx = 0;
            if (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37)] == (byte)5 && this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 1)] == (byte)1)
            {
                this.ISRx = (int)this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 2)];
                num37 += (byte)3;
            }
            else if (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37)] == (byte)5 && this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 1)] == (byte)2)
            {
                //this.ISRx = int.Parse(this.nRcvPkt[(int)this.bytAddMode + (int)num37 + 2].ToString("X2") + this.nRcvPkt[(int)this.bytAddMode + (int)num37 + 3].ToString("X2"), NumberStyles.HexNumber);
                tempISRx = int.Parse(this.nRcvPkt[(int)this.bytAddMode + (int)num37 + 2].ToString("X2") + this.nRcvPkt[(int)this.bytAddMode + (int)num37 + 3].ToString("X2"), NumberStyles.HexNumber);
                num37 += (byte)4;
            }
            if (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37)] == (byte)6 && this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 1)] == (byte)1)
            {
                this.ISTx = (int)this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 2)];
                num37 += (byte)3;
            }
            else if (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37)] == (byte)6 && this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 1)] == (byte)2)
            {
                //this.ISTx = int.Parse(this.nRcvPkt[(int)this.bytAddMode + (int)num37 + 2].ToString("X2") + this.nRcvPkt[(int)this.bytAddMode + (int)num37 + 3].ToString("X2"), NumberStyles.HexNumber);
                tempISTx = int.Parse(this.nRcvPkt[(int)this.bytAddMode + (int)num37 + 2].ToString("X2") + this.nRcvPkt[(int)this.bytAddMode + (int)num37 + 3].ToString("X2"), NumberStyles.HexNumber);
                num37 += (byte)4;
            }
            //this.WSTx = this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 5)];
            //this.WSRx = this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 11)];
            tempWSTx = this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 5)];
            tempWSRx = this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 11)];
            if (IsLineTrafficEnabled)
            {
                //LineTrafficControlEventHandler("\r\n", "Send");
                LineTrafficControlEventHandler("     Negotiated Window Size Tx = " + tempWSTx.ToString() +
                    ", Window Size Rx = " + tempWSRx.ToString() + ", Info Field Size Tx = " + tempISTx.ToString() + ", Info Field Size Rx = " + tempISRx.ToString() + "\r\n\r\n", "Receive");
            }
            return flag2;
        }
        public bool SetNRM_CTT(bool bigSNRM, bool IsLineTrafficEnabled = true)
        {
            bool flag1 = false;
            byte num1 = Convert.ToByte(5 + (int)this.bytAddMode);
            nPkt[2] = Convert.ToByte(7 + (int)this.bytAddMode);
            byte[] nPkt1 = this.nPkt;
            int index1 = (int)num1;
            byte num2 = (byte)(index1 + 1);
            nPkt1[index1] = (byte)83;
            nPkt[(int)num2 + 2] = (byte)126;
            DCl.fcs(ref this.nPkt, (int)Convert.ToByte(5 + (int)this.bytAddMode), (byte)1);
            ClearBuffer();
            DateTime now1 = DateTime.Now;
            DateTime now2;
            TimeSpan timeSpan;
            byte num3 = Convert.ToByte(5 + (int)this.bytAddMode);
            this.nPkt[2] = Convert.ToByte(7 + (int)this.bytAddMode);
            byte[] nPkt2 = this.nPkt;
            int index3 = (int)num3;
            byte num4 = (byte)(index3 + 1);
            if (IsRRFrame)
                nPkt2[index3] = (byte)17;
            else
                nPkt2[index3] = (byte)147;
            this.ClearBuffer();
            flag1 = false;
            if (!bigSNRM)
            {
                this.DCl.fcs(ref this.nPkt, (int)Convert.ToByte((int)this.bytAddMode + 5), (byte)1);
                this.nPkt[(int)num4 + 2] = (byte)126;
            }
            else
            {
                byte num5 = Convert.ToByte(8 + (int)this.bytAddMode);
                byte[] nPkt3 = this.nPkt;
                int index4 = (int)num5;
                byte num6 = (byte)(index4 + 1);
                nPkt3[index4] = (byte)129;
                byte[] nPkt4 = this.nPkt;
                int index5 = (int)num6;
                byte num7 = (byte)(index5 + 1);
                nPkt4[index5] = (byte)128;
                byte[] nPkt5 = this.nPkt;
                int index6 = (int)num7;
                byte num8 = (byte)(index6 + 1);
                nPkt5[index6] = (byte)18;
                byte[] nPkt6 = this.nPkt;
                int index7 = (int)num8;
                byte num9 = (byte)(index7 + 1);
                nPkt6[index7] = (byte)5;
                byte num10;
                if (this.ISTx < (int)byte.MaxValue)
                {
                    byte[] nPkt7 = this.nPkt;
                    int index8 = (int)num9;
                    byte num11 = (byte)(index8 + 1);
                    nPkt7[index8] = (byte)1;
                    byte[] nPkt8 = this.nPkt;
                    int index9 = (int)num11;
                    num10 = (byte)(index9 + 1);
                    int num12 = (int)Convert.ToByte(this.ISTx);
                    nPkt8[index9] = (byte)num12;
                }
                else
                {
                    byte[] nPkt9 = this.nPkt;
                    int index10 = (int)num9;
                    byte num13 = (byte)(index10 + 1);
                    nPkt9[index10] = (byte)2;
                    byte[] nPkt10 = this.nPkt;
                    int index11 = (int)num13;
                    byte num14 = (byte)(index11 + 1);
                    int num15 = (int)Convert.ToByte(this.ISTx / 256);
                    nPkt10[index11] = (byte)num15;
                    byte[] nPkt11 = this.nPkt;
                    int index12 = (int)num14;
                    num10 = (byte)(index12 + 1);
                    int num16 = (int)Convert.ToByte(this.ISTx % 256);
                    nPkt11[index12] = (byte)num16;
                }
                byte[] nPkt12 = this.nPkt;
                int index13 = (int)num10;
                byte num17 = (byte)(index13 + 1);
                nPkt12[index13] = (byte)6;
                byte num18;
                if (this.ISRx < (int)byte.MaxValue)
                {
                    byte[] nPkt13 = this.nPkt;
                    int index14 = (int)num17;
                    byte num19 = (byte)(index14 + 1);
                    nPkt13[index14] = (byte)1;
                    byte[] nPkt14 = this.nPkt;
                    int index15 = (int)num19;
                    num18 = (byte)(index15 + 1);
                    int num20 = (int)Convert.ToByte(this.ISRx);
                    nPkt14[index15] = (byte)num20;
                }
                else
                {
                    byte[] nPkt15 = this.nPkt;
                    int index16 = (int)num17;
                    byte num21 = (byte)(index16 + 1);
                    nPkt15[index16] = (byte)2;
                    byte[] nPkt16 = this.nPkt;
                    int index17 = (int)num21;
                    byte num22 = (byte)(index17 + 1);
                    int num23 = (int)Convert.ToByte(this.ISRx / 256);
                    nPkt16[index17] = (byte)num23;
                    byte[] nPkt17 = this.nPkt;
                    int index18 = (int)num22;
                    num18 = (byte)(index18 + 1);
                    int num24 = (int)Convert.ToByte(this.ISRx % 256);
                    nPkt17[index18] = (byte)num24;
                }
                byte[] nPkt18 = this.nPkt;
                int index19 = (int)num18;
                byte num25 = (byte)(index19 + 1);
                nPkt18[index19] = (byte)7;
                byte[] nPkt19 = this.nPkt;
                int index20 = (int)num25;
                byte num26 = (byte)(index20 + 1);
                nPkt19[index20] = (byte)4;
                byte[] nPkt20 = this.nPkt;
                int index21 = (int)num26;
                byte num27 = (byte)(index21 + 1);
                nPkt20[index21] = (byte)0;
                byte[] nPkt21 = this.nPkt;
                int index22 = (int)num27;
                byte num28 = (byte)(index22 + 1);
                nPkt21[index22] = (byte)0;
                byte[] nPkt22 = this.nPkt;
                int index23 = (int)num28;
                byte num29 = (byte)(index23 + 1);
                nPkt22[index23] = (byte)0;
                byte[] nPkt23 = this.nPkt;
                int index24 = (int)num29;
                byte num30 = (byte)(index24 + 1);
                int wsTx = (int)this.WSTx;
                nPkt23[index24] = (byte)wsTx;
                byte[] nPkt24 = this.nPkt;
                int index25 = (int)num30;
                byte num31 = (byte)(index25 + 1);
                nPkt24[index25] = (byte)8;
                byte[] nPkt25 = this.nPkt;
                int index26 = (int)num31;
                byte num32 = (byte)(index26 + 1);
                nPkt25[index26] = (byte)4;
                byte[] nPkt26 = this.nPkt;
                int index27 = (int)num32;
                byte num33 = (byte)(index27 + 1);
                nPkt26[index27] = (byte)0;
                byte[] nPkt27 = this.nPkt;
                int index28 = (int)num33;
                byte num34 = (byte)(index28 + 1);
                nPkt27[index28] = (byte)0;
                byte[] nPkt28 = this.nPkt;
                int index29 = (int)num34;
                byte num35 = (byte)(index29 + 1);
                nPkt28[index29] = (byte)0;
                byte[] nPkt29 = this.nPkt;
                int index30 = (int)num35;
                num4 = (byte)(index30 + 1);
                int wsRx = (int)this.WSRx;
                nPkt29[index30] = (byte)wsRx;
                this.nPkt[2] = Convert.ToByte((int)num4 + 1);
                this.DCl.fcs(ref this.nPkt, (int)Convert.ToByte((int)this.bytAddMode + 5), (byte)1);
                this.DCl.fcs(ref this.nPkt, (int)Convert.ToByte((int)num4 - 1), (byte)1);
                this.nPkt[(int)num4 + 2] = (byte)126;
            }
            byte num36 = 0;
            bool flag2;
            do
            {
                this.ClearBuffer();
                flag2 = false;
                this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)num4 + 3));
                if (IsLineTrafficEnabled)
                {
                    LineTrafficControlEventHandler("\r\n     SNRM\r\n", "Send");
                    if (bigSNRM)
                        LineTrafficControlEventHandler("     Proposed Window Size Tx = " + this.WSTx.ToString() + ", Window Size Rx = " + this.WSRx.ToString() + ", Info Field Size Tx = " + this.ISTx.ToString() + ", Info Field Size Rx = " + this.ISRx.ToString(), "Send");
                }
                if (IsLineTrafficEnabled)
                    SendDataPrint(nPkt, Convert.ToByte((int)num4 + 3));
                commandString = string.Empty;
                for (int index2 = 0; index2 < (num4 + 3); ++index2)
                    commandString += this.nPkt[index2].ToString("X2");
                DateTime now3 = DateTime.Now;
                do
                {
                    this.Wait(5);
                    Application.DoEvents();
                    this.DataReceive();
                    if (this.nCounter > 2 && (int)this.nRcvPkt[2] + 2 <= this.nCounter && this.nRcvPkt[(int)this.nRcvPkt[2] + 1] == (byte)126 && this.DCl.fcs(ref this.nRcvPkt, int.Parse(this.nRcvPkt[2].ToString()), (byte)0))
                    {
                        flag2 = true;
                        break;
                    }
                    now2 = DateTime.Now;
                    timeSpan = now2.Subtract(now3);
                    if (timeSpan.Seconds > (int)this.nTimeOut && (int)num36 < (int)this.nTryCount)
                    {
                        ++num36;
                        break;
                    }
                }
                while (!flag2);
            }
            while (!flag2 && (int)num36 != (int)this.nTryCount);
            RecvDataPrint(nRcvPkt, nCounter, IsLineTrafficEnabled);
            responseString = string.Empty;
            for (int index31 = 0; index31 < this.nCounter; ++index31)
                responseString += this.nRcvPkt[index31].ToString("X2");
            //this.swLT.Write("(R)\t" + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss:ff tt") + "\t" + this.temp);

            if (IsRRFrame && (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] == (byte)17 /*|| this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] == (byte)151*/))
                return true;
            if (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] != (byte)115)
                return false;
            byte num37 = 11;
            int tempWSTx = 0, tempWSRx = 0, tempISTx = 0, tempISRx = 0;
            if (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37)] == (byte)5 && this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 1)] == (byte)1)
            {
                this.ISRx = (int)this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 2)];
                num37 += (byte)3;
            }
            else if (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37)] == (byte)5 && this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 1)] == (byte)2)
            {
                //this.ISRx = int.Parse(this.nRcvPkt[(int)this.bytAddMode + (int)num37 + 2].ToString("X2") + this.nRcvPkt[(int)this.bytAddMode + (int)num37 + 3].ToString("X2"), NumberStyles.HexNumber);
                tempISRx = int.Parse(this.nRcvPkt[(int)this.bytAddMode + (int)num37 + 2].ToString("X2") + this.nRcvPkt[(int)this.bytAddMode + (int)num37 + 3].ToString("X2"), NumberStyles.HexNumber);
                num37 += (byte)4;
            }
            if (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37)] == (byte)6 && this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 1)] == (byte)1)
            {
                this.ISTx = (int)this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 2)];
                num37 += (byte)3;
            }
            else if (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37)] == (byte)6 && this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 1)] == (byte)2)
            {
                //this.ISTx = int.Parse(this.nRcvPkt[(int)this.bytAddMode + (int)num37 + 2].ToString("X2") + this.nRcvPkt[(int)this.bytAddMode + (int)num37 + 3].ToString("X2"), NumberStyles.HexNumber);
                tempISTx = int.Parse(this.nRcvPkt[(int)this.bytAddMode + (int)num37 + 2].ToString("X2") + this.nRcvPkt[(int)this.bytAddMode + (int)num37 + 3].ToString("X2"), NumberStyles.HexNumber);
                num37 += (byte)4;
            }
            //this.WSTx = this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 5)];
            //this.WSRx = this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 11)];
            tempWSTx = this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 5)];
            tempWSRx = this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 11)];
            if (IsLineTrafficEnabled)
            {
                //LineTrafficControlEventHandler("\r\n", "Send");
                LineTrafficControlEventHandler("     Negotiated Window Size Tx = " + tempWSTx.ToString() +
                    ", Window Size Rx = " + tempWSRx.ToString() + ", Info Field Size Tx = " + tempISTx.ToString() + ", Info Field Size Rx = " + tempISRx.ToString() + "\r\n\r\n", "Receive");
            }
            return flag2;
        }

        public string GetSNRM(bool bigSNRM)
        {
            bool flag1 = false;
            byte num1 = Convert.ToByte(5 + (int)this.bytAddMode);
            nPkt[2] = Convert.ToByte(7 + (int)this.bytAddMode);
            byte[] nPkt1 = this.nPkt;
            int index1 = (int)num1;
            byte num2 = (byte)(index1 + 1);
            nPkt1[index1] = (byte)83;
            nPkt[(int)num2 + 2] = (byte)126;
            DCl.fcs(ref this.nPkt, (int)Convert.ToByte(5 + (int)this.bytAddMode), (byte)1);
            ClearBuffer();
            DateTime now1 = DateTime.Now;
            DateTime now2;
            TimeSpan timeSpan;
            byte num3 = Convert.ToByte(5 + (int)this.bytAddMode);
            this.nPkt[2] = Convert.ToByte(7 + (int)this.bytAddMode);
            byte[] nPkt2 = this.nPkt;
            int index3 = (int)num3;
            byte num4 = (byte)(index3 + 1);
            if (IsRRFrame)
                nPkt2[index3] = (byte)17;//11
            else if (IsN_R_is_1)
                nPkt2[index3] = (byte)49;//31
            else if (IsUnknownCommandIdentifier)
                nPkt2[index3] = (byte)255;//FF
            else
                nPkt2[index3] = (byte)147;//93
            this.ClearBuffer();
            flag1 = false;
            if (!bigSNRM)
            {
                this.DCl.fcs(ref this.nPkt, (int)Convert.ToByte((int)this.bytAddMode + 5), (byte)1);
                this.nPkt[(int)num4 + 2] = (byte)126;
            }
            else
            {
                byte num5 = Convert.ToByte(8 + (int)this.bytAddMode);
                byte[] nPkt3 = this.nPkt;
                int index4 = (int)num5;
                byte num6 = (byte)(index4 + 1);
                nPkt3[index4] = (byte)129;
                byte[] nPkt4 = this.nPkt;
                int index5 = (int)num6;
                byte num7 = (byte)(index5 + 1);
                nPkt4[index5] = (byte)128;
                byte[] nPkt5 = this.nPkt;
                int index6 = (int)num7;
                byte num8 = (byte)(index6 + 1);
                nPkt5[index6] = (byte)18;
                byte[] nPkt6 = this.nPkt;
                int index7 = (int)num8;
                byte num9 = (byte)(index7 + 1);
                nPkt6[index7] = (byte)5;
                byte num10;
                if (this.ISTx < (int)byte.MaxValue)
                {
                    byte[] nPkt7 = this.nPkt;
                    int index8 = (int)num9;
                    byte num11 = (byte)(index8 + 1);
                    nPkt7[index8] = (byte)1;
                    byte[] nPkt8 = this.nPkt;
                    int index9 = (int)num11;
                    num10 = (byte)(index9 + 1);
                    int num12 = (int)Convert.ToByte(this.ISTx);
                    nPkt8[index9] = (byte)num12;
                }
                else
                {
                    byte[] nPkt9 = this.nPkt;
                    int index10 = (int)num9;
                    byte num13 = (byte)(index10 + 1);
                    nPkt9[index10] = (byte)2;
                    byte[] nPkt10 = this.nPkt;
                    int index11 = (int)num13;
                    byte num14 = (byte)(index11 + 1);
                    int num15 = (int)Convert.ToByte(this.ISTx / 256);
                    nPkt10[index11] = (byte)num15;
                    byte[] nPkt11 = this.nPkt;
                    int index12 = (int)num14;
                    num10 = (byte)(index12 + 1);
                    int num16 = (int)Convert.ToByte(this.ISTx % 256);
                    nPkt11[index12] = (byte)num16;
                }
                byte[] nPkt12 = this.nPkt;
                int index13 = (int)num10;
                byte num17 = (byte)(index13 + 1);
                nPkt12[index13] = (byte)6;
                byte num18;
                if (this.ISRx < (int)byte.MaxValue)
                {
                    byte[] nPkt13 = this.nPkt;
                    int index14 = (int)num17;
                    byte num19 = (byte)(index14 + 1);
                    nPkt13[index14] = (byte)1;
                    byte[] nPkt14 = this.nPkt;
                    int index15 = (int)num19;
                    num18 = (byte)(index15 + 1);
                    int num20 = (int)Convert.ToByte(this.ISRx);
                    nPkt14[index15] = (byte)num20;
                }
                else
                {
                    byte[] nPkt15 = this.nPkt;
                    int index16 = (int)num17;
                    byte num21 = (byte)(index16 + 1);
                    nPkt15[index16] = (byte)2;
                    byte[] nPkt16 = this.nPkt;
                    int index17 = (int)num21;
                    byte num22 = (byte)(index17 + 1);
                    int num23 = (int)Convert.ToByte(this.ISRx / 256);
                    nPkt16[index17] = (byte)num23;
                    byte[] nPkt17 = this.nPkt;
                    int index18 = (int)num22;
                    num18 = (byte)(index18 + 1);
                    int num24 = (int)Convert.ToByte(this.ISRx % 256);
                    nPkt17[index18] = (byte)num24;
                }
                byte[] nPkt18 = this.nPkt;
                int index19 = (int)num18;
                byte num25 = (byte)(index19 + 1);
                nPkt18[index19] = (byte)7;
                byte[] nPkt19 = this.nPkt;
                int index20 = (int)num25;
                byte num26 = (byte)(index20 + 1);
                nPkt19[index20] = (byte)4;
                byte[] nPkt20 = this.nPkt;
                int index21 = (int)num26;
                byte num27 = (byte)(index21 + 1);
                nPkt20[index21] = (byte)0;
                byte[] nPkt21 = this.nPkt;
                int index22 = (int)num27;
                byte num28 = (byte)(index22 + 1);
                nPkt21[index22] = (byte)0;
                byte[] nPkt22 = this.nPkt;
                int index23 = (int)num28;
                byte num29 = (byte)(index23 + 1);
                nPkt22[index23] = (byte)0;
                byte[] nPkt23 = this.nPkt;
                int index24 = (int)num29;
                byte num30 = (byte)(index24 + 1);
                int wsTx = (int)this.WSTx;
                nPkt23[index24] = (byte)wsTx;
                byte[] nPkt24 = this.nPkt;
                int index25 = (int)num30;
                byte num31 = (byte)(index25 + 1);
                nPkt24[index25] = (byte)8;
                byte[] nPkt25 = this.nPkt;
                int index26 = (int)num31;
                byte num32 = (byte)(index26 + 1);
                nPkt25[index26] = (byte)4;
                byte[] nPkt26 = this.nPkt;
                int index27 = (int)num32;
                byte num33 = (byte)(index27 + 1);
                nPkt26[index27] = (byte)0;
                byte[] nPkt27 = this.nPkt;
                int index28 = (int)num33;
                byte num34 = (byte)(index28 + 1);
                nPkt27[index28] = (byte)0;
                byte[] nPkt28 = this.nPkt;
                int index29 = (int)num34;
                byte num35 = (byte)(index29 + 1);
                nPkt28[index29] = (byte)0;
                byte[] nPkt29 = this.nPkt;
                int index30 = (int)num35;
                num4 = (byte)(index30 + 1);
                int wsRx = (int)this.WSRx;
                nPkt29[index30] = (byte)wsRx;
                this.nPkt[2] = Convert.ToByte((int)num4 + 1);
                this.DCl.fcs(ref this.nPkt, (int)Convert.ToByte((int)this.bytAddMode + 5), (byte)1);
                this.DCl.fcs(ref this.nPkt, (int)Convert.ToByte((int)num4 - 1), (byte)1);
                this.nPkt[(int)num4 + 2] = (byte)126;
            }
            byte num36 = 0;
            bool flag2;
            //SendDataPrint(nPkt, Convert.ToByte((int)num4 + 3));
            commandString = string.Empty;
            for (int index2 = 0; index2 < (num4 + 3); ++index2)
                commandString += this.nPkt[index2].ToString("X2");
            //do
            //{
            //    this.ClearBuffer();
            //    flag2 = false;
            //    this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)num4 + 3));
            //    if (IsLineTrafficEnabled)
            //    {
            //        LineTrafficControlEventHandler("\r\n     SNRM\r\n", "Send");
            //        if (bigSNRM)
            //            LineTrafficControlEventHandler("     Proposed Window Size Tx = " + this.WSTx.ToString() + ", Window Size Rx = " + this.WSRx.ToString() + ", Info Field Size Tx = " + this.ISTx.ToString() + ", Info Field Size Rx = " + this.ISRx.ToString(), "Send");
            //    }
            //    if (IsLineTrafficEnabled)
            //        SendDataPrint(nPkt, Convert.ToByte((int)num4 + 3));
            //    commandString = string.Empty;
            //    for (int index2 = 0; index2 < (num4 + 3); ++index2)
            //        commandString += this.nPkt[index2].ToString("X2");
            //    DateTime now3 = DateTime.Now;
            //    do
            //    {
            //        this.Wait(5);
            //        Application.DoEvents();
            //        this.DataReceive();
            //        if (this.nCounter > 2 && (int)this.nRcvPkt[2] + 2 <= this.nCounter && this.nRcvPkt[(int)this.nRcvPkt[2] + 1] == (byte)126 && this.DCl.fcs(ref this.nRcvPkt, int.Parse(this.nRcvPkt[2].ToString()), (byte)0))
            //        {
            //            flag2 = true;
            //            break;
            //        }
            //        now2 = DateTime.Now;
            //        timeSpan = now2.Subtract(now3);
            //        if (timeSpan.Seconds > (int)this.nTimeOut && (int)num36 < (int)this.nTryCount)
            //        {
            //            ++num36;
            //            break;
            //        }
            //    }
            //    while (!flag2);
            //}
            //while (!flag2 && (int)num36 != (int)this.nTryCount);
            //RecvDataPrint(nRcvPkt, nCounter, IsLineTrafficEnabled);
            //responseString = string.Empty;
            //for (int index31 = 0; index31 < this.nCounter; ++index31)
            //    responseString += this.nRcvPkt[index31].ToString("X2");
            //this.swLT.Write("(R)\t" + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss:ff tt") + "\t" + this.temp);

            //if (IsRRFrame && (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] == (byte)17 /*|| this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] == (byte)151*/))
            //    return true;
            //if (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] != (byte)115)
            //    return false;
            //byte num37 = 11;
            //int tempWSTx = 0, tempWSRx = 0, tempISTx = 0, tempISRx = 0;
            //if (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37)] == (byte)5 && this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 1)] == (byte)1)
            //{
            //    this.ISRx = (int)this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 2)];
            //    num37 += (byte)3;
            //}
            //else if (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37)] == (byte)5 && this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 1)] == (byte)2)
            //{
            //    //this.ISRx = int.Parse(this.nRcvPkt[(int)this.bytAddMode + (int)num37 + 2].ToString("X2") + this.nRcvPkt[(int)this.bytAddMode + (int)num37 + 3].ToString("X2"), NumberStyles.HexNumber);
            //    tempISRx = int.Parse(this.nRcvPkt[(int)this.bytAddMode + (int)num37 + 2].ToString("X2") + this.nRcvPkt[(int)this.bytAddMode + (int)num37 + 3].ToString("X2"), NumberStyles.HexNumber);
            //    num37 += (byte)4;
            //}
            //if (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37)] == (byte)6 && this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 1)] == (byte)1)
            //{
            //    this.ISTx = (int)this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 2)];
            //    num37 += (byte)3;
            //}
            //else if (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37)] == (byte)6 && this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 1)] == (byte)2)
            //{
            //    //this.ISTx = int.Parse(this.nRcvPkt[(int)this.bytAddMode + (int)num37 + 2].ToString("X2") + this.nRcvPkt[(int)this.bytAddMode + (int)num37 + 3].ToString("X2"), NumberStyles.HexNumber);
            //    tempISTx = int.Parse(this.nRcvPkt[(int)this.bytAddMode + (int)num37 + 2].ToString("X2") + this.nRcvPkt[(int)this.bytAddMode + (int)num37 + 3].ToString("X2"), NumberStyles.HexNumber);
            //    num37 += (byte)4;
            //}
            ////this.WSTx = this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 5)];
            ////this.WSRx = this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 11)];
            //tempWSTx = this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 5)];
            //tempWSRx = this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 11)];
            //if (IsLineTrafficEnabled)
            //{
            //    //LineTrafficControlEventHandler("\r\n", "Send");
            //    LineTrafficControlEventHandler("     Negotiated Window Size Tx = " + tempWSTx.ToString() +
            //        ", Window Size Rx = " + tempWSRx.ToString() + ", Info Field Size Tx = " + tempISTx.ToString() + ", Info Field Size Rx = " + tempISRx.ToString() + "\r\n\r\n", "Receive");
            //}
            //return flag2;
            return commandString.Trim();
        }

        public bool SetNRM(byte nWait, byte nTryCount, byte nTimeOut)
        {
            bool flag1 = false;
            byte num1 = Convert.ToByte(5 + (int)this.bytAddMode);
            this.nPkt[2] = Convert.ToByte(7 + (int)this.bytAddMode);
            byte[] nPkt1 = this.nPkt;
            int index1 = (int)num1;
            byte num2 = (byte)(index1 + 1);
            nPkt1[index1] = (byte)83;
            this.nPkt[(int)num2 + 2] = (byte)126;
            this.DCl.fcs(ref this.nPkt, (ushort)Convert.ToByte(5 + (int)this.bytAddMode), (byte)1);
            this.ClearBuffer();
            this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)num2 + 3));
            DateTime now1 = DateTime.Now;
            do
            {
                this.Wait(25.0);//60->25
                Application.DoEvents();
                this.DataReceive();
                if (this.nCounter > 2 && (int)this.nRcvPkt[2] + 2 <= this.nCounter)
                {
                    flag1 = true;
                    break;
                }
            }
            while (DateTime.Now.Subtract(now1).Seconds <= 4);
            this.temp = string.Empty;
            for (int i = 0; i < this.nCounter; ++i)
                this.temp += this.nRcvPkt[i].ToString("X2");
            this.temp += "\r\n";
            //this.Response1.SelectionColor = Color.Red;
            //this.Response1.AppendText("<-- " + this.temp);
            if (!flag1)
                return false;
            this.nPkt[2] = Convert.ToByte(7 + (int)this.bytAddMode);
            byte num3 = Convert.ToByte(5 + (int)this.bytAddMode);
            byte[] nPkt2 = this.nPkt;
            int index2 = (int)num3;
            byte num4 = (byte)(index2 + 1);
            nPkt2[index2] = (byte)147;
            this.nPkt[(int)num4 + 2] = (byte)126;
            this.DCl.fcs(ref this.nPkt, (ushort)Convert.ToByte((int)this.bytAddMode + 5), (byte)1);
            byte num5 = 0;
            bool flag2;
            do
            {
                this.ClearBuffer();
                flag2 = false;
                this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)num4 + 3));
                DateTime now2 = DateTime.Now;
                LineTrafficControlEventHandler("\r\n     SNRM\r\n", "Send");
                LineTrafficControlEventHandler("     Proposed Window Size Tx = " + this.WSTx.ToString() + ", Window Size Rx = " + this.WSRx.ToString() + ", Info Field Size Tx = " + this.ISTx.ToString() + ", Info Field Size Rx = " + this.ISRx.ToString(), "Send");
                SendDataPrint(nPkt, Convert.ToByte((int)num4 + 3));
                do
                {
                    this.Wait((double)nWait);
                    Application.DoEvents();
                    this.DataReceive();
                    if (this.nCounter > 2 && (int)this.nRcvPkt[2] + 2 <= this.nCounter && this.nRcvPkt[(int)this.nRcvPkt[2] + 1] == (byte)126)
                    {
                        flag2 = true;
                        break;
                    }
                    if (DateTime.Now.Subtract(now2).Seconds > (int)nTimeOut && (int)num5 < (int)nTryCount)
                    {
                        ++num5;
                        break;
                    }
                }
                while (!flag2);
            }
            while (!flag2 && (int)num5 != (int)nTryCount);
            RecvDataPrint(nRcvPkt, nCounter);
            byte num37 = 11;
            int tempWSTx = 0, tempWSRx = 0, tempISTx = 0, tempISRx = 0;
            if (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37)] == (byte)5 && this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 1)] == (byte)1)
            {
                this.ISRx = (int)this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 2)];
                num37 += (byte)3;
            }
            else if (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37)] == (byte)5 && this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 1)] == (byte)2)
            {
                //this.ISRx = int.Parse(this.nRcvPkt[(int)this.bytAddMode + (int)num37 + 2].ToString("X2") + this.nRcvPkt[(int)this.bytAddMode + (int)num37 + 3].ToString("X2"), NumberStyles.HexNumber);
                tempISRx = int.Parse(this.nRcvPkt[(int)this.bytAddMode + (int)num37 + 2].ToString("X2") + this.nRcvPkt[(int)this.bytAddMode + (int)num37 + 3].ToString("X2"), NumberStyles.HexNumber);
                num37 += (byte)4;
            }
            if (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37)] == (byte)6 && this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 1)] == (byte)1)
            {
                this.ISTx = (int)this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 2)];
                num37 += (byte)3;
            }
            else if (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37)] == (byte)6 && this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 1)] == (byte)2)
            {
                //this.ISTx = int.Parse(this.nRcvPkt[(int)this.bytAddMode + (int)num37 + 2].ToString("X2") + this.nRcvPkt[(int)this.bytAddMode + (int)num37 + 3].ToString("X2"), NumberStyles.HexNumber);
                tempISTx = int.Parse(this.nRcvPkt[(int)this.bytAddMode + (int)num37 + 2].ToString("X2") + this.nRcvPkt[(int)this.bytAddMode + (int)num37 + 3].ToString("X2"), NumberStyles.HexNumber);
                num37 += (byte)4;
            }
            tempWSTx = this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 5)];
            tempWSRx = this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 11)];
            LineTrafficControlEventHandler("     Negotiated Window Size Tx = " + tempWSTx.ToString() +
                                                ", Window Size Rx = " + tempWSRx.ToString() +
                                                ", Info Field Size Tx = " + tempISTx.ToString() +
                                                ", Info Field Size Rx = " + tempISRx.ToString() + "\r\n\r\n", "Receive");

            //LineTrafficControlEventHandler("     Negotiated Window Size Tx = " + this.WSTx.ToString() + ", Window Size Rx = " + this.WSRx.ToString() + ", Info Field Size Tx = " + this.ISTx.ToString() + ", Info Field Size Rx = " + this.ISRx.ToString() + "\r\n\r\n", "Receive");
            //LineTrafficControlEventHandler("\r\n", "Send");
            //this.temp = string.Empty;
            //for (int i = 0; i < this.nCounter; ++i)
            //    this.temp += this.nRcvPkt[i].ToString("X2");
            //this.temp += "\r\n";
            //this.Response1.SelectionColor = Color.Red;
            //this.Response1.AppendText("<-- " + this.temp);
            return this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] == (byte)115 && flag2;
        }
        public bool SetNRMFG(byte nWait, byte nTryCount, byte nTimeOut, bool IsLineTrafficEnabled = true)
        {
            bool flag1 = false;
            byte num1 = Convert.ToByte(5 + (int)this.bytAddMode);
            this.nPkt[2] = Convert.ToByte(7 + (int)this.bytAddMode);
            byte[] nPkt1 = this.nPkt;
            int index1 = (int)num1;
            byte num2 = (byte)(index1 + 1);
            nPkt1[index1] = (byte)83;
            this.nPkt[(int)num2 + 2] = (byte)126;
            this.DCl.fcs(ref this.nPkt, Convert.ToByte(5 + (int)this.bytAddMode), (byte)1);
            this.ClearBuffer();
            this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)num2 + 3));
            DateTime now1 = DateTime.Now;
            DateTime now2;
            TimeSpan timeSpan;
            while (true)
            {
                this.Wait(25.0);//60->25
                Application.DoEvents();
                this.DataReceive();
                if (this.nCounter <= 2 || (int)this.nRcvPkt[2] + 2 > this.nCounter)
                {
                    now2 = DateTime.Now;
                    timeSpan = now2.Subtract(now1);
                    if (timeSpan.Seconds > 4)
                        goto label_5;
                }
                else
                    break;
            }
            flag1 = true;
        label_5:
            //this.temp = string.Empty;
            //for (int index2 = 0; index2 < this.nCounter; ++index2)
            //    this.temp += this.nRcvPkt[index2].ToString("X2");
            //this.temp += "\r\n";
            byte num3 = Convert.ToByte(5 + (int)this.bytAddMode);
            this.nPkt[2] = Convert.ToByte(7 + (int)this.bytAddMode);
            byte[] nPkt2 = this.nPkt;
            int index3 = (int)num3;
            byte num4 = (byte)(index3 + 1);
            nPkt2[index3] = (byte)147;
            this.nPkt[(int)num4 + 2] = (byte)126;
            this.DCl.fcs(ref this.nPkt, Convert.ToByte((int)this.bytAddMode + 5), (byte)1);
            byte num5 = 0;
            bool flag2;
            do
            {
                this.ClearBuffer();
                flag2 = false;
                this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)num4 + 3));
                if (IsLineTrafficEnabled)
                    LineTrafficControlEventHandler("\r\n     SNRM\r\n", "Send");
                if (IsLineTrafficEnabled)
                    SendDataPrint(nPkt, Convert.ToByte((int)num4 + 3));
                commandString = string.Empty;
                for (int index2 = 0; index2 < (num4 + 3); ++index2)
                    commandString += this.nPkt[index2].ToString("X2");
                DateTime now3 = DateTime.Now;
                do
                {
                    this.Wait((double)nWait);
                    Application.DoEvents();
                    this.DataReceive();
                    if (this.nCounter > 2 && (int)this.nRcvPkt[2] + 2 <= this.nCounter && this.nRcvPkt[(int)this.nRcvPkt[2] + 1] == (byte)126)
                    {
                        flag2 = true;
                        break;
                    }
                    now2 = DateTime.Now;
                    timeSpan = now2.Subtract(now3);
                    if (timeSpan.Seconds > (int)nTimeOut && (int)num5 < (int)nTryCount)
                    {
                        ++num5;
                        break;
                    }
                }
                while (!flag2);
            }
            while (!flag2 && (int)num5 != (int)nTryCount);
            //this.temp = string.Empty;
            //for (int index4 = 0; index4 < this.nCounter; ++index4)
            //    this.temp += this.nRcvPkt[index4].ToString("X2");
            //this.temp += "\r\n";
            RecvDataPrint(nRcvPkt, nCounter, IsLineTrafficEnabled);
            byte num37 = 11;
            int tempWSTx = 0, tempWSRx = 0, tempISTx = 0, tempISRx = 0;
            if (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37)] == (byte)5 && this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 1)] == (byte)1)
            {
                this.ISRx = (int)this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 2)];
                num37 += (byte)3;
            }
            else if (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37)] == (byte)5 && this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 1)] == (byte)2)
            {
                //this.ISRx = int.Parse(this.nRcvPkt[(int)this.bytAddMode + (int)num37 + 2].ToString("X2") + this.nRcvPkt[(int)this.bytAddMode + (int)num37 + 3].ToString("X2"), NumberStyles.HexNumber);
                tempISRx = int.Parse(this.nRcvPkt[(int)this.bytAddMode + (int)num37 + 2].ToString("X2") + this.nRcvPkt[(int)this.bytAddMode + (int)num37 + 3].ToString("X2"), NumberStyles.HexNumber);
                num37 += (byte)4;
            }
            if (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37)] == (byte)6 && this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 1)] == (byte)1)
            {
                this.ISTx = (int)this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 2)];
                num37 += (byte)3;
            }
            else if (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37)] == (byte)6 && this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 1)] == (byte)2)
            {
                //this.ISTx = int.Parse(this.nRcvPkt[(int)this.bytAddMode + (int)num37 + 2].ToString("X2") + this.nRcvPkt[(int)this.bytAddMode + (int)num37 + 3].ToString("X2"), NumberStyles.HexNumber);
                tempISTx = int.Parse(this.nRcvPkt[(int)this.bytAddMode + (int)num37 + 2].ToString("X2") + this.nRcvPkt[(int)this.bytAddMode + (int)num37 + 3].ToString("X2"), NumberStyles.HexNumber);
                num37 += (byte)4;
            }
            //this.WSTx = this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 5)];
            //this.WSRx = this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 11)];
            tempWSTx = this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 5)];
            tempWSRx = this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 11)];
            if (IsLineTrafficEnabled)
            {
                LineTrafficControlEventHandler("     Negotiated Window Size Tx = " + tempWSTx.ToString() +
                    ", Window Size Rx = " + tempWSRx.ToString() + ", Info Field Size Tx = " + tempISTx.ToString() + ", Info Field Size Rx = " + tempISRx.ToString() + "\r\n\r\n", "Receive");
            }

            //if (IsLineTrafficEnabled)
            //    LineTrafficControlEventHandler("\r\n", "Send");
            responseString = string.Empty;
            for (int index31 = 0; index31 < this.nCounter; ++index31)
                responseString += this.nRcvPkt[index31].ToString("X2");
            return this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] == (byte)115 && flag2;
        }
        public bool SetNRM_PUSH(bool bigSNRM, bool IsLineTrafficEnabled = true)
        {
            bool flag1 = false;
            byte num1 = Convert.ToByte(5 + (int)this.bytAddMode);
            nPkt[2] = Convert.ToByte(7 + (int)this.bytAddMode);
            byte[] nPkt1 = this.nPkt;
            int index1 = (int)num1;
            byte num2 = (byte)(index1 + 1);
            nPkt1[index1] = (byte)83;
            nPkt[(int)num2 + 2] = (byte)126;
            DCl.fcs(ref this.nPkt, (int)Convert.ToByte(5 + (int)this.bytAddMode), (byte)1);
            ClearBuffer();
            this.temp = string.Empty;
            for (int index2 = 0; index2 < (num2 + 3); ++index2)
                this.temp += this.nPkt[index2].ToString("X2");
            this.temp += "\r\n";
            SendPkt(this.nPkt, (ushort)Convert.ToByte((int)num2 + 3));
            DateTime now1 = DateTime.Now;
            DateTime now2;
            TimeSpan timeSpan;
            while (true)
            {
                Wait(5.0);
                Application.DoEvents();
                DataReceive();
                if (this.nCounter <= 2 || (int)this.nRcvPkt[2] + 2 > this.nCounter || !this.DCl.fcs(ref this.nRcvPkt, int.Parse(this.nRcvPkt[2].ToString()), (byte)0))
                {
                    now2 = DateTime.Now;
                    timeSpan = now2.Subtract(now1);
                    if (timeSpan.Seconds > (int)this.nTimeOut)
                        goto label_5;
                }
                else
                    break;
            }
            flag1 = true;
        label_5:
            this.temp = string.Empty;
            for (int index2 = 0; index2 < this.nCounter; ++index2)
                this.temp += this.nRcvPkt[index2].ToString("X2");
            this.temp += "\r\n";
            //this.swLT.Write("(R)\t" + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss:ff tt") + "\t" + this.temp);
            byte num3 = Convert.ToByte(5 + (int)this.bytAddMode);
            this.nPkt[2] = Convert.ToByte(7 + (int)this.bytAddMode);
            byte[] nPkt2 = this.nPkt;
            int index3 = (int)num3;
            byte num4 = (byte)(index3 + 1);
            nPkt2[index3] = (byte)147;
            this.ClearBuffer();
            flag1 = false;
            if (!bigSNRM)
            {
                this.DCl.fcs(ref this.nPkt, (int)Convert.ToByte((int)this.bytAddMode + 5), (byte)1);
                this.nPkt[(int)num4 + 2] = (byte)126;
            }
            else
            {
                byte num5 = Convert.ToByte(8 + (int)this.bytAddMode);
                byte[] nPkt3 = this.nPkt;
                int index4 = (int)num5;
                byte num6 = (byte)(index4 + 1);
                nPkt3[index4] = (byte)129;
                byte[] nPkt4 = this.nPkt;
                int index5 = (int)num6;
                byte num7 = (byte)(index5 + 1);
                nPkt4[index5] = (byte)128;
                byte[] nPkt5 = this.nPkt;
                int index6 = (int)num7;
                byte num8 = (byte)(index6 + 1);
                nPkt5[index6] = (byte)18;
                byte[] nPkt6 = this.nPkt;
                int index7 = (int)num8;
                byte num9 = (byte)(index7 + 1);
                nPkt6[index7] = (byte)5;
                byte num10;
                if (this.ISTx < (int)byte.MaxValue)
                {
                    byte[] nPkt7 = this.nPkt;
                    int index8 = (int)num9;
                    byte num11 = (byte)(index8 + 1);
                    nPkt7[index8] = (byte)1;
                    byte[] nPkt8 = this.nPkt;
                    int index9 = (int)num11;
                    num10 = (byte)(index9 + 1);
                    int num12 = (int)Convert.ToByte(this.ISTx);
                    nPkt8[index9] = (byte)num12;
                }
                else
                {
                    byte[] nPkt9 = this.nPkt;
                    int index10 = (int)num9;
                    byte num13 = (byte)(index10 + 1);
                    nPkt9[index10] = (byte)2;
                    byte[] nPkt10 = this.nPkt;
                    int index11 = (int)num13;
                    byte num14 = (byte)(index11 + 1);
                    int num15 = (int)Convert.ToByte(this.ISTx / 256);
                    nPkt10[index11] = (byte)num15;
                    byte[] nPkt11 = this.nPkt;
                    int index12 = (int)num14;
                    num10 = (byte)(index12 + 1);
                    int num16 = (int)Convert.ToByte(this.ISTx % 256);
                    nPkt11[index12] = (byte)num16;
                }
                byte[] nPkt12 = this.nPkt;
                int index13 = (int)num10;
                byte num17 = (byte)(index13 + 1);
                nPkt12[index13] = (byte)6;
                byte num18;
                if (this.ISRx < (int)byte.MaxValue)
                {
                    byte[] nPkt13 = this.nPkt;
                    int index14 = (int)num17;
                    byte num19 = (byte)(index14 + 1);
                    nPkt13[index14] = (byte)1;
                    byte[] nPkt14 = this.nPkt;
                    int index15 = (int)num19;
                    num18 = (byte)(index15 + 1);
                    int num20 = (int)Convert.ToByte(this.ISRx);
                    nPkt14[index15] = (byte)num20;
                }
                else
                {
                    byte[] nPkt15 = this.nPkt;
                    int index16 = (int)num17;
                    byte num21 = (byte)(index16 + 1);
                    nPkt15[index16] = (byte)2;
                    byte[] nPkt16 = this.nPkt;
                    int index17 = (int)num21;
                    byte num22 = (byte)(index17 + 1);
                    int num23 = (int)Convert.ToByte(this.ISRx / 256);
                    nPkt16[index17] = (byte)num23;
                    byte[] nPkt17 = this.nPkt;
                    int index18 = (int)num22;
                    num18 = (byte)(index18 + 1);
                    int num24 = (int)Convert.ToByte(this.ISRx % 256);
                    nPkt17[index18] = (byte)num24;
                }
                byte[] nPkt18 = this.nPkt;
                int index19 = (int)num18;
                byte num25 = (byte)(index19 + 1);
                nPkt18[index19] = (byte)7;
                byte[] nPkt19 = this.nPkt;
                int index20 = (int)num25;
                byte num26 = (byte)(index20 + 1);
                nPkt19[index20] = (byte)4;
                byte[] nPkt20 = this.nPkt;
                int index21 = (int)num26;
                byte num27 = (byte)(index21 + 1);
                nPkt20[index21] = (byte)0;
                byte[] nPkt21 = this.nPkt;
                int index22 = (int)num27;
                byte num28 = (byte)(index22 + 1);
                nPkt21[index22] = (byte)0;
                byte[] nPkt22 = this.nPkt;
                int index23 = (int)num28;
                byte num29 = (byte)(index23 + 1);
                nPkt22[index23] = (byte)0;
                byte[] nPkt23 = this.nPkt;
                int index24 = (int)num29;
                byte num30 = (byte)(index24 + 1);
                int wsTx = (int)this.WSTx;
                nPkt23[index24] = (byte)wsTx;
                byte[] nPkt24 = this.nPkt;
                int index25 = (int)num30;
                byte num31 = (byte)(index25 + 1);
                nPkt24[index25] = (byte)8;
                byte[] nPkt25 = this.nPkt;
                int index26 = (int)num31;
                byte num32 = (byte)(index26 + 1);
                nPkt25[index26] = (byte)4;
                byte[] nPkt26 = this.nPkt;
                int index27 = (int)num32;
                byte num33 = (byte)(index27 + 1);
                nPkt26[index27] = (byte)0;
                byte[] nPkt27 = this.nPkt;
                int index28 = (int)num33;
                byte num34 = (byte)(index28 + 1);
                nPkt27[index28] = (byte)0;
                byte[] nPkt28 = this.nPkt;
                int index29 = (int)num34;
                byte num35 = (byte)(index29 + 1);
                nPkt28[index29] = (byte)0;
                byte[] nPkt29 = this.nPkt;
                int index30 = (int)num35;
                num4 = (byte)(index30 + 1);
                int wsRx = (int)this.WSRx;
                nPkt29[index30] = (byte)wsRx;
                this.nPkt[2] = Convert.ToByte((int)num4 + 1);
                this.DCl.fcs(ref this.nPkt, (int)Convert.ToByte((int)this.bytAddMode + 5), (byte)1);
                this.DCl.fcs(ref this.nPkt, (int)Convert.ToByte((int)num4 - 1), (byte)1);
                this.nPkt[(int)num4 + 2] = (byte)126;
            }
            byte num36 = 0;
            bool flag2;
            do
            {
                this.ClearBuffer();
                flag2 = false;
                this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)num4 + 3));
                DateTime now3 = DateTime.Now;
                if (IsLineTrafficEnabled)
                {
                    LineTrafficControlEventHandler("\r\n     SNRM\r\n", "Send");
                    if (bigSNRM)
                        LineTrafficControlEventHandler("     Proposed Window Size Tx = " + this.WSTx.ToString() + ", Window Size Rx = " + this.WSRx.ToString() + ", Info Field Size Tx = " + this.ISTx.ToString() + ", Info Field Size Rx = " + this.ISRx.ToString(), "Send");
                }
                if (IsLineTrafficEnabled)
                    SendDataPrint(nPkt, Convert.ToByte((int)num4 + 3));
                commandString = string.Empty;
                for (int index2 = 0; index2 < (num4 + 3); ++index2)
                    commandString += this.nPkt[index2].ToString("X2");
                do
                {
                    this.Wait(5);
                    Application.DoEvents();
                    this.DataReceive();
                    if (this.nCounter > 2 && (int)this.nRcvPkt[2] + 2 <= this.nCounter && this.nRcvPkt[(int)this.nRcvPkt[2] + 1] == (byte)126 && this.DCl.fcs(ref this.nRcvPkt, int.Parse(this.nRcvPkt[2].ToString()), (byte)0))
                    {
                        flag2 = true;
                        break;
                    }
                    now2 = DateTime.Now;
                    timeSpan = now2.Subtract(now3);
                    if (timeSpan.Seconds > (int)this.nTimeOut && (int)num36 < (int)this.nTryCount)
                    {
                        ++num36;
                        break;
                    }
                }
                while (!flag2);
            }
            while (!flag2 && (int)num36 != (int)this.nTryCount);

            RecvDataPrint(nRcvPkt, nCounter, IsLineTrafficEnabled);
            if (IsLineTrafficEnabled)
                LineTrafficControlEventHandler("\r\n", "Send");
            responseString = string.Empty;
            for (int index31 = 0; index31 < this.nCounter; ++index31)
                responseString += this.nRcvPkt[index31].ToString("X2");
            //this.swLT.Write("(R)\t" + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss:ff tt") + "\t" + this.temp);
            if (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] != (byte)115)
                return false;
            byte num37 = 11;
            int tempWSTx = 0, tempWSRx = 0, tempISTx = 0, tempISRx = 0;
            if (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37)] == (byte)5 && this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 1)] == (byte)1)
            {
                this.ISRx = (int)this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 2)];
                num37 += (byte)3;
            }
            else if (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37)] == (byte)5 && this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 1)] == (byte)2)
            {
                //this.ISRx = int.Parse(this.nRcvPkt[(int)this.bytAddMode + (int)num37 + 2].ToString("X2") + this.nRcvPkt[(int)this.bytAddMode + (int)num37 + 3].ToString("X2"), NumberStyles.HexNumber);
                tempISRx = int.Parse(this.nRcvPkt[(int)this.bytAddMode + (int)num37 + 2].ToString("X2") + this.nRcvPkt[(int)this.bytAddMode + (int)num37 + 3].ToString("X2"), NumberStyles.HexNumber);
                num37 += (byte)4;
            }
            if (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37)] == (byte)6 && this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 1)] == (byte)1)
            {
                this.ISTx = (int)this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 2)];
                num37 += (byte)3;
            }
            else if (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37)] == (byte)6 && this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 1)] == (byte)2)
            {
                //this.ISTx = int.Parse(this.nRcvPkt[(int)this.bytAddMode + (int)num37 + 2].ToString("X2") + this.nRcvPkt[(int)this.bytAddMode + (int)num37 + 3].ToString("X2"), NumberStyles.HexNumber);
                tempISTx = int.Parse(this.nRcvPkt[(int)this.bytAddMode + (int)num37 + 2].ToString("X2") + this.nRcvPkt[(int)this.bytAddMode + (int)num37 + 3].ToString("X2"), NumberStyles.HexNumber);
                num37 += (byte)4;
            }
            //this.WSTx = this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 5)];
            //this.WSRx = this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 11)];
            tempWSTx = this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 5)];
            tempWSRx = this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + (int)num37 + 11)];
            if (IsLineTrafficEnabled)
            {
                //LineTrafficControlEventHandler("\r\n", "Send");
                LineTrafficControlEventHandler("     Negotiated Window Size Tx = " + tempWSTx.ToString() +
                    ", Window Size Rx = " + tempWSRx.ToString() + ", Info Field Size Tx = " + tempISTx.ToString() + ", Info Field Size Rx = " + tempISRx.ToString() + "\r\n\r\n", "Receive");
            }
            return flag2;
        }

        public int AARQ(bool IsLineTrafficEnabled = true)
        {
            commandString = "";
            responseString = "";
            byte num1 = Convert.ToByte((int)this.bytAddMode + 8);
            //this.nCommandCounter = 8;
            this.nRetLSH = Convert.ToByte((int)this.nRecvCntr << 5);
            this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nRetLSH | 16);
            this.nRetLSH = Convert.ToByte((int)this.nSentCntr << 1);
            this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nRetLSH | (int)this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)]);
            byte[] nPkt1 = this.nPkt;
            int index1 = (int)num1;
            byte num2 = (byte)(index1 + 1);
            nPkt1[index1] = (byte)230;//E6
            byte[] nPkt2 = this.nPkt;
            int index2 = (int)num2;
            byte num3 = (byte)(index2 + 1);
            nPkt2[index2] = (byte)230;//E6
            byte[] nPkt3 = this.nPkt;
            int index3 = (int)num3;
            byte num4 = (byte)(index3 + 1);
            nPkt3[index3] = (byte)0;//00
            byte[] nPkt4 = this.nPkt;
            int index4 = (int)num4;
            byte num5 = (byte)(index4 + 1);
            nPkt4[index4] = (byte)96;//60
            byte[] nPkt5 = this.nPkt;
            int index5 = (int)num5;
            byte num6 = (byte)(index5 + 1);
            nPkt5[index5] = (byte)0;//00
            byte[] nPkt6 = this.nPkt;
            int index6 = (int)num6;
            byte num7 = (byte)(index6 + 1);
            nPkt6[index6] = (byte)161;//A1
            byte[] nPkt7 = this.nPkt;
            int index7 = (int)num7;
            byte num8 = (byte)(index7 + 1);
            nPkt7[index7] = (byte)9;//09
            byte[] nPkt8 = this.nPkt;
            int index8 = (int)num8;
            byte num9 = (byte)(index8 + 1);
            nPkt8[index8] = (byte)6;//06
            byte[] nPkt9 = this.nPkt;
            int index9 = (int)num9;
            byte num10 = (byte)(index9 + 1);
            nPkt9[index9] = (byte)7;//07
            byte[] nPkt10 = this.nPkt;
            int index10 = (int)num10;
            byte num11 = (byte)(index10 + 1);
            nPkt10[index10] = (byte)96;//60
            byte[] nPkt11 = this.nPkt;
            int index11 = (int)num11;
            byte num12 = (byte)(index11 + 1);
            nPkt11[index11] = (byte)133;//
            byte[] nPkt12 = this.nPkt;
            int index12 = (int)num12;
            byte num13 = (byte)(index12 + 1);
            nPkt12[index12] = (byte)116;//85
            byte[] nPkt13 = this.nPkt;
            int index13 = (int)num13;
            byte num14 = (byte)(index13 + 1);
            nPkt13[index13] = (byte)5;//05
            byte[] nPkt14 = this.nPkt;
            int index14 = (int)num14;
            byte num15 = (byte)(index14 + 1);
            nPkt14[index14] = (byte)8;//08
            byte[] nPkt15 = this.nPkt;
            int index15 = (int)num15;
            byte num16 = (byte)(index15 + 1);
            nPkt15[index15] = (byte)1;//01
            byte num17;
            if (DLMSInfo.IsLNWithCipher || DLMSInfo.IsLNWithCipherDedicatedKey)
            {
                byte[] nPkt16 = this.nPkt;
                int index16 = (int)num16;
                num17 = (byte)(index16 + 1);
                nPkt16[index16] = (byte)3;
            }
            else
            {
                byte[] nPkt17 = this.nPkt;
                int index17 = (int)num16;
                num17 = (byte)(index17 + 1);
                nPkt17[index17] = (byte)1;
            }
            if (DLMSInfo.AccessMode != 0)
            {
                ASCIIEncoding asciiEncoding = new ASCIIEncoding();
                if (DLMSInfo.IsLNWithCipher)
                {
                    if (DLMSInfo.TxtSysT.Trim().Length == 16)
                    {
                        byte[] nPkt18 = this.nPkt;
                        int index18 = (int)num17;
                        byte num18 = (byte)(index18 + 1);
                        nPkt18[index18] = (byte)166;
                        byte[] nPkt19 = this.nPkt;
                        int index19 = (int)num18;
                        byte num19 = (byte)(index19 + 1);
                        int num20 = (int)Convert.ToByte(DLMSInfo.TxtSysT.Trim().Length / 2 + 2);
                        nPkt19[index19] = (byte)num20;
                        byte[] nPkt20 = this.nPkt;
                        int index20 = (int)num19;
                        byte num21 = (byte)(index20 + 1);
                        nPkt20[index20] = (byte)4;
                        byte[] nPkt21 = this.nPkt;
                        int index21 = (int)num21;
                        num17 = (byte)(index21 + 1);
                        int num22 = (int)Convert.ToByte(DLMSInfo.TxtSysT.Trim().Length / 2);
                        nPkt21[index21] = (byte)num22;
                        this.Ps = StringToByteArray(DLMSInfo.TxtSysT.Trim());
                        for (int index22 = 0; index22 < DLMSInfo.TxtSysT.Trim().Length / 2; ++index22)
                            this.nPkt[(int)num17++] = this.Ps[index22];
                    }
                    else
                    {
                        byte[] nPkt22 = this.nPkt;
                        int index23 = (int)num17;
                        byte num23 = (byte)(index23 + 1);
                        nPkt22[index23] = (byte)166;
                        byte[] nPkt23 = this.nPkt;
                        int index24 = (int)num23;
                        byte num24 = (byte)(index24 + 1);
                        int num25 = (int)Convert.ToByte(DLMSInfo.TxtSysT.Trim().Length + 2);
                        nPkt23[index24] = (byte)num25;
                        byte[] nPkt24 = this.nPkt;
                        int index25 = (int)num24;
                        byte num26 = (byte)(index25 + 1);
                        nPkt24[index25] = (byte)4;
                        byte[] nPkt25 = this.nPkt;
                        int index26 = (int)num26;
                        num17 = (byte)(index26 + 1);
                        int num27 = (int)Convert.ToByte(DLMSInfo.TxtSysT.Trim().Length);
                        nPkt25[index26] = (byte)num27;
                        this.Ps = asciiEncoding.GetBytes(DLMSInfo.TxtSysT.Trim());
                        for (int index27 = 0; index27 < DLMSInfo.TxtSysT.Trim().Length; ++index27)
                            this.nPkt[(int)num17++] = this.Ps[index27];
                    }
                }
                byte[] nPkt26 = this.nPkt;
                int index28 = (int)num17;
                byte num28 = (byte)(index28 + 1);
                nPkt26[index28] = (byte)138;
                byte[] nPkt27 = this.nPkt;
                int index29 = (int)num28;
                byte num29 = (byte)(index29 + 1);
                nPkt27[index29] = (byte)2;
                byte[] nPkt28 = this.nPkt;
                int index30 = (int)num29;
                byte num30 = (byte)(index30 + 1);
                nPkt28[index30] = (byte)7;
                byte[] nPkt29 = this.nPkt;
                int index31 = (int)num30;
                byte num31 = (byte)(index31 + 1);
                nPkt29[index31] = (byte)128;
                byte[] nPkt30 = this.nPkt;
                int index32 = (int)num31;
                byte num32 = (byte)(index32 + 1);
                nPkt30[index32] = (byte)139;
                byte[] nPkt31 = this.nPkt;
                int index33 = (int)num32;
                byte num33 = (byte)(index33 + 1);
                nPkt31[index33] = (byte)7;
                byte[] nPkt32 = this.nPkt;
                int index34 = (int)num33;
                byte num34 = (byte)(index34 + 1);
                nPkt32[index34] = (byte)96;//0x60
                byte[] nPkt33 = this.nPkt;
                int index35 = (int)num34;
                byte num35 = (byte)(index35 + 1);
                nPkt33[index35] = (byte)133;//0x85
                byte[] nPkt34 = this.nPkt;
                int index36 = (int)num35;
                byte num36 = (byte)(index36 + 1);
                nPkt34[index36] = (byte)116;//0x74
                byte[] nPkt35 = this.nPkt;
                int index37 = (int)num36;
                byte num37 = (byte)(index37 + 1);
                nPkt35[index37] = (byte)5;//0x05
                byte[] nPkt36 = this.nPkt;
                int index38 = (int)num37;
                byte num38 = (byte)(index38 + 1);
                nPkt36[index38] = (byte)8;//0x08
                byte[] nPkt37 = this.nPkt;
                int index39 = (int)num38;
                byte num39 = (byte)(index39 + 1);
                nPkt37[index39] = (byte)2;//0x02
                byte num40;
                if (DLMSInfo.IsLNWithCipher && DLMSInfo.AccessMode == 2 && DLMSInfo.IsWithGMAC)
                {
                    byte[] nPkt38 = this.nPkt;
                    int index40 = (int)num39;
                    num40 = (byte)(index40 + 1);
                    nPkt38[index40] = (byte)5;
                }
                else if (DLMSInfo.IsLNWithCipher && DLMSInfo.AccessMode == 2 && !DLMSInfo.IsWithGMAC)
                {
                    byte[] nPkt39 = this.nPkt;
                    int index41 = (int)num39;
                    num40 = (byte)(index41 + 1);
                    nPkt39[index41] = (byte)2;
                }
                #region Condition for FW ASSOCIATION
                else if (DLMSInfo.IsLNWithCipher && DLMSInfo.AccessMode == 4 && !DLMSInfo.IsWithGMAC)
                {
                    byte[] nPkt39 = this.nPkt;
                    int index41 = (int)num39;
                    num40 = (byte)(index41 + 1);
                    nPkt39[index41] = (byte)2;
                }
                #endregion
                else
                {
                    byte[] nPkt40 = this.nPkt;
                    int index42 = (int)num39;
                    num40 = (byte)(index42 + 1);
                    #region New logic to handle FW Association
                    int num41;
                    if (DLMSInfo.AccessMode == 4)
                        num41 = (int)Convert.ToByte(DLMSInfo.AccessMode - 2);
                    else
                        num41 = (int)Convert.ToByte(DLMSInfo.AccessMode);
                    #endregion
                    //int num41 = (int)Convert.ToByte(DLMSInfo.AccessMode);//old
                    nPkt40[index42] = (byte)num41;
                }
                byte[] nPkt41 = this.nPkt;
                int index43 = (int)num40;
                byte num42 = (byte)(index43 + 1);
                nPkt41[index43] = (byte)172;//0xAC
                byte[] nPkt42 = this.nPkt;
                int index44 = (int)num42;
                byte num43 = (byte)(index44 + 1);
                int num44 = (int)Convert.ToByte(2 + DLMSInfo.MeterAuthPasswordWrite.Length);
                nPkt42[index44] = (byte)num44;
                byte[] nPkt43 = this.nPkt;
                int index45 = (int)num43;
                byte num45 = (byte)(index45 + 1);
                nPkt43[index45] = (byte)128;
                byte[] nPkt44 = this.nPkt;
                int index46 = (int)num45;
                num17 = (byte)(index46 + 1);
                int num46 = (int)Convert.ToByte(DLMSInfo.MeterAuthPasswordWrite.Length);
                nPkt44[index46] = (byte)num46;
                this.Ps = DLMSInfo.AccessMode != 1 ? asciiEncoding.GetBytes("GNSRAPDRP-" + DateTime.Now.ToString("HHmmss")) : asciiEncoding.GetBytes(DLMSInfo.MeterAuthPasswordWrite);
                for (int index47 = 0; index47 < DLMSInfo.MeterAuthPasswordWrite.Length; ++index47)
                    this.nPkt[(int)num17++] = this.Ps[index47];
            }
            byte num47;
            if (DLMSInfo.IsLNWithCipher || DLMSInfo.IsLNWithCipherDedicatedKey)
            {
                byte num48 = 0;
                if (DLMSInfo.IsLNWithCipherDedicatedKey)
                    num48 = (byte)17;
                byte[] nPkt45 = this.nPkt;
                int index48 = (int)num17;
                byte num49 = (byte)(index48 + 1);
                nPkt45[index48] = (byte)190;
                byte[] nPkt46 = this.nPkt;
                int index49 = (int)num49;
                byte num50 = (byte)(index49 + 1);
                int num51 = (int)Convert.ToByte(35 + (int)num48);
                nPkt46[index49] = (byte)num51;
                byte[] nPkt47 = this.nPkt;
                int index50 = (int)num50;
                byte num52 = (byte)(index50 + 1);
                nPkt47[index50] = (byte)4;
                byte[] nPkt48 = this.nPkt;
                int index51 = (int)num52;
                byte num53 = (byte)(index51 + 1);
                int num54 = (int)Convert.ToByte(33 + (int)num48);
                nPkt48[index51] = (byte)num54;
                byte[] nPkt49 = this.nPkt;
                int index52 = (int)num53;
                byte num55 = (byte)(index52 + 1);
                nPkt49[index52] = (byte)33;
                byte[] nPkt50 = this.nPkt;
                int index53 = (int)num55;
                byte num56 = (byte)(index53 + 1);
                int num57 = (int)Convert.ToByte(31 + (int)num48);
                nPkt50[index53] = (byte)num57;
                byte[] nPkt51 = this.nPkt;
                int index54 = (int)num56;
                byte num58 = (byte)(index54 + 1);
                nPkt51[index54] = (byte)48;
                byte[] nPkt52 = this.nPkt;
                int index55 = (int)num58;
                byte num59 = (byte)(index55 + 1);
                nPkt52[index55] = (byte)0;
                byte[] nPkt53 = this.nPkt;
                int index56 = (int)num59;
                byte num60 = (byte)(index56 + 1);
                nPkt53[index56] = (byte)0;
                byte[] nPkt54 = this.nPkt;
                int index57 = (int)num60;
                byte num61 = (byte)(index57 + 1);
                int num62 = (int)Convert.ToByte(this.nCommandCounter / 256);
                nPkt54[index57] = (byte)num62;
                byte[] nPkt55 = this.nPkt;
                int index58 = (int)num61;
                num47 = (byte)(index58 + 1);
                int num63 = (int)Convert.ToByte(this.nCommandCounter % 256);
                nPkt55[index58] = (byte)num63;
                ASCIIEncoding asciiEncoding = new ASCIIEncoding();
                this.sEncryptkeyinHEX = string.Empty;
                if (DLMSInfo.TxtEK.Trim().Length == 32)
                {
                    this.sEncryptkeyinHEX = DLMSInfo.TxtEK.Trim();
                }
                else
                {
                    this.Ps1 = asciiEncoding.GetBytes(DLMSInfo.TxtEK.Trim());
                    for (int index59 = 0; index59 < this.Ps1.Length; ++index59)
                        this.sEncryptkeyinHEX += this.Ps1[index59].ToString("X2");
                }
                if (DLMSInfo.IsLNWithCipherDedicatedKey)
                {
                    this.sDedicatedkey = RandomString(16);
                    this.Ps1 = asciiEncoding.GetBytes(this.sDedicatedkey);
                    this.sDedicatedkeyinHEX = string.Empty;
                    for (int index60 = 0; index60 < this.Ps1.Length; ++index60)
                        this.sDedicatedkeyinHEX += this.Ps1[index60].ToString("X2");
                    string[] strArray = new string[5]
                    {
                    "30",
                    this.nCommandCounter++.ToString("X8"),
                    "010110",
                    this.sDedicatedkeyinHEX,
                    $"0000065F1F0400{DLMSInfo.ConformanceBlock}FFFF"//OLD Conformance block
                    };
                    #region OLD Conformance Block
                    //        string[] strArray = new string[5]
                    //        {
                    //"30",
                    //this.nCommandCounter++.ToString("X8"),
                    //"010110",
                    //this.sDedicatedkeyinHEX,
                    //"0000065F1F040000181DFFFF"//OLD Conformance block
                    //        };
                    #endregion
                    foreach (byte num64 in this.Encrypt(string.Concat(strArray), this.sEncryptkeyinHEX))
                        this.nPkt[(int)num47++] = num64;
                    this.sEncryptkeyinHEX = this.sDedicatedkeyinHEX;
                }
                else
                {
                    foreach (byte num65 in this.Encrypt("30" + this.nCommandCounter++.ToString("X8") + $"01000000065F1F0400{DLMSInfo.ConformanceBlock}FFFF", this.sEncryptkeyinHEX))
                        this.nPkt[(int)num47++] = num65;
                    #region OLD Conformance Block
                    //foreach (byte num65 in this.Encrypt("30" + this.nCommandCounter++.ToString("X8") + "01000000065F1F040000181DFFFF", this.sEncryptkeyinHEX))
                    //    this.nPkt[(int)num47++] = num65;
                    #endregion
                }
            }
            else
            {
                byte[] nPkt56 = this.nPkt;
                int index61 = (int)num17;
                byte num66 = (byte)(index61 + 1);
                nPkt56[index61] = (byte)190;//BE
                byte[] nPkt57 = this.nPkt;
                int index62 = (int)num66;
                byte num67 = (byte)(index62 + 1);
                nPkt57[index62] = (byte)16;//10
                byte[] nPkt58 = this.nPkt;
                int index63 = (int)num67;
                byte num68 = (byte)(index63 + 1);
                nPkt58[index63] = (byte)4;//04
                byte[] nPkt59 = this.nPkt;
                int index64 = (int)num68;
                num47 = (byte)(index64 + 1);
                nPkt59[index64] = (byte)14;//0E
                foreach (byte num69 in StringToByteArray($"01000000065F1F0400{DLMSInfo.ConformanceBlock}FFFF"))
                    this.nPkt[(int)num47++] = num69;
                #region OLD Conformance Block
                //foreach (byte num69 in StringToByteArray("01000000065F1F040000181DFFFF"))
                //    this.nPkt[(int)num47++] = num69;
                #endregion
            }
            this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 12)] = Convert.ToByte((int)num47 + 1 - 14 - (int)this.bytAddMode);
            this.nPkt[2] = Convert.ToByte((int)num47 + 1);
            this.DCl.fcs(ref this.nPkt, (int)Convert.ToByte((int)this.bytAddMode + 5), (byte)1);
            this.DCl.fcs(ref this.nPkt, (int)Convert.ToByte((int)num47 - 1), (byte)1);
            this.nPkt[(int)num47 + 2] = (byte)126;//7E
            this.ClearBuffer();
            //this.Wait(60.0);//old 100 new 60
            bool flag = false;
            commandString = "";
            for (int i = 0; i < ((int)num47 + 3); i++)
            {
                commandString += nPkt[i].ToString("X2");
            }
            this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)num47 + 3));
            DateTime now = DateTime.Now;
            if (IsLineTrafficEnabled)
            {
                LineTrafficControlEventHandler("     AARQ", "Send");
                LineTrafficControlEventHandler($"     Proposed CONFORMANCE BLOCK SERVICES [{DLMSInfo.ConformanceBlock}]: {DLMSParser.ConformanceServicesSupported(DLMSInfo.ConformanceBlock.Trim())}", "Send");
                SendDataPrint(nPkt, Convert.ToByte((int)num47 + 3));
            }
            while (true)
            {
                this.Wait(50.0);//100->4->50
                Application.DoEvents();
                this.DataReceive();
                if (this.nCounter <= 2 || (int)this.nRcvPkt[2] + 2 > this.nCounter)
                {
                    if (DateTime.Now.Subtract(now).Seconds > nTimeOut + 3)
                        goto label_48;
                }
                else
                    break;
            }
            RecvDataPrint(nRcvPkt, nCounter, IsLineTrafficEnabled);
            responseString = "";
            for (int recIndex = 0; recIndex < nCounter; recIndex++)
            {
                responseString += nRcvPkt[recIndex].ToString("X2");
            }
            //  if (IsLineTrafficEnabled)
            //     LineTrafficControlEventHandler("\r\n", "Send");
            flag = true;
            this.FrameType();
        label_48:
            this.temp = string.Empty;
            for (int index65 = 0; index65 < this.nCounter; ++index65)
                this.temp += this.nRcvPkt[index65].ToString("X2");
            this.temp += "\r\n";
            //this.swLT.Write("(R)\t" + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss:ff tt") + "\t" + this.temp);
            int num70 = 13;
            int num71 = 1;
            var stopwatch = Stopwatch.StartNew();
            while (true)
            {
                if (this.nRcvPkt[(int)this.bytAddMode + num70] == (byte)161)
                    num70 += 2 + (int)this.nRcvPkt[(int)this.bytAddMode + num70 + 1];
                else if (this.nRcvPkt[(int)this.bytAddMode + num70] == (byte)162)
                    num70 += 2 + (int)this.nRcvPkt[(int)this.bytAddMode + num70 + 1];
                else if (this.nRcvPkt[(int)this.bytAddMode + num70] == (byte)163)
                {
                    num71 = (int)this.nRcvPkt[(int)this.bytAddMode + num70 + (int)this.nRcvPkt[(int)this.bytAddMode + num70 + 1] + 1];
                    num70 += 2 + (int)this.nRcvPkt[(int)this.bytAddMode + num70 + 1];
                }
                else if (this.nRcvPkt[(int)this.bytAddMode + num70] == (byte)164)
                {
                    this.sSYSTaginHEX = string.Empty;
                    for (int index66 = 0; index66 < (int)Convert.ToInt16(this.nRcvPkt[(int)this.bytAddMode + num70 + 3].ToString(), 16); ++index66)
                        this.sSYSTaginHEX += this.nRcvPkt[(int)this.bytAddMode + num70 + 4 + index66].ToString("X2");
                    num70 += 2 + (int)this.nRcvPkt[(int)this.bytAddMode + num70 + 1];
                }
                else if (this.nRcvPkt[(int)this.bytAddMode + num70] == (byte)136)
                    num70 += 2 + (int)this.nRcvPkt[(int)this.bytAddMode + num70 + 1];
                else if (this.nRcvPkt[(int)this.bytAddMode + num70] == (byte)137)
                    num70 += 2 + (int)this.nRcvPkt[(int)this.bytAddMode + num70 + 1];
                else if (this.nRcvPkt[(int)this.bytAddMode + num70] == (byte)170)
                    num70 += 2 + (int)this.nRcvPkt[(int)this.bytAddMode + num70 + 1];
                else if (this.nRcvPkt[(int)this.bytAddMode + num70] == (byte)190)
                    break;
                if (stopwatch.ElapsedMilliseconds > ((nTimeOut + 3) * 1000))
                {
                    stopwatch.Stop();
                    break;
                }
            }
            if (this.nRcvPkt[(int)this.bytAddMode + num70] == (byte)190)
            {
                int num72 = 0;
                if (DLMSInfo.IsLNWithCipher)
                {
                    string empty = string.Empty;
                    for (int index67 = (int)this.bytAddMode + num70 + 6; index67 < this.nCounter - 3; ++index67)
                        empty += this.nRcvPkt[index67].ToString("X2");
                    byte[] sourceArray = this.Decrypt(empty, this.sEncryptkeyinHEX);
                    num72 = empty.Length / 2 - sourceArray.Length;
                    Array.Copy((Array)sourceArray, 0, (Array)this.nRcvPkt, (int)this.bytAddMode + num70 + 6, sourceArray.Length);
                    this.nRcvPkt[(int)this.bytAddMode + num70 + 1] -= Convert.ToByte(num72);
                }
                int num73 = num70 + 2;
                if (this.nRcvPkt[(int)this.bytAddMode + num73] == (byte)4)
                {
                    this.nRcvPkt[(int)this.bytAddMode + num73 + 1] -= Convert.ToByte(num72);
                    num73 += 2;
                }
                if (this.nRcvPkt[(int)this.bytAddMode + num73] == (byte)40)
                {
                    this.nRcvPkt[(int)this.bytAddMode + num73 + 1] -= Convert.ToByte(num72);
                    num73 += 2;
                }
                if (this.nRcvPkt[(int)this.bytAddMode + num73] == (byte)8)
                    num73 += 3;
                if (this.nRcvPkt[(int)this.bytAddMode + num73] == (byte)95 && this.nRcvPkt[(int)this.bytAddMode + num73 + 1] == (byte)31)
                {
                    int num74 = num73 + 3;
                    this.intConfBlk = long.Parse(this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + num74)].ToString("X2") + this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + num74 + 1)].ToString("X2") + this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + num74 + 2)].ToString("X2") + this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + num74 + 3)].ToString("X2"), NumberStyles.HexNumber);
                    this.conformanceBlockString = $"{this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + num74 + 1)].ToString("X2") + this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + num74 + 2)].ToString("X2") + this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + num74 + 3)].ToString("X2")}";//BY AAC
                    if (IsLineTrafficEnabled)
                    {
                        LineTrafficControlEventHandler($"     Negotiated CONFORMANCE BLOCK SERVICES [{conformanceBlockString}]: {DLMSParser.ConformanceServicesSupported(conformanceBlockString.Trim())}", "Receive");
                        LineTrafficControlEventHandler("\r\n", "Send");
                    }
                }
            }
            if (!flag || num71 != 0 && num71 != 14 || this.nCounter <= 27)
                return 1;
            //if (!(DLMSInfo.AccessMode == 2))//Utility Setting OLD ONE
            if (!(DLMSInfo.AccessMode == 2) && !(DLMSInfo.AccessMode == 4))//Utility Setting and FW
                return 0;
            string empty1 = string.Empty;
            string sActionData;
            if (!DLMSInfo.IsLNWithCipher || DLMSInfo.IsLNWithCipher && !DLMSInfo.IsWithGMAC)
            {
                this.intConfBlk = long.Parse(this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 71)].ToString("X2") + this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 72)].ToString("X2") + this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 73)].ToString("X2"), NumberStyles.HexNumber);
                this.keyBytes = StrToByteArray(DLMSInfo.MeterAuthPasswordWrite.Trim());
                Aes aes = new Aes(Aes.KeySize.Bits128, this.keyBytes);
                if (this.nRcvPkt[(int)this.bytAddMode + 62] == (byte)18 && this.nRcvPkt[(int)this.bytAddMode + 64] == (byte)16)
                {
                    for (int index68 = 0; index68 < 16; ++index68)
                        this.plainText[index68] = this.nRcvPkt[index68 + (int)Convert.ToByte((int)this.bytAddMode + 65)];
                }
                else
                {
                    for (int index69 = 0; index69 < 16; ++index69)
                        this.plainText[index69] = this.nRcvPkt[index69 + (int)Convert.ToByte((int)this.bytAddMode + 53)];
                }
                aes.Cipher(this.plainText, this.cipherText);
                if (this.cipherText.Length == 0)
                {
                    sActionData = "0";
                }
                else
                {
                    sActionData = "0109" + this.cipherText.Length.ToString("X2");
                    for (int index70 = 0; index70 < this.cipherText.Length; ++index70)
                        sActionData += this.cipherText[index70].ToString("X2");
                }
            }
            else
            {
                string empty2 = string.Empty;
                for (int index71 = 0; index71 < 16; ++index71)
                    empty2 += this.nRcvPkt[index71 + (int)Convert.ToByte((int)this.bytAddMode + 53)].ToString("X2");
                this.cipherText = this.Encrypt("10" + this.nCommandCounter++.ToString("X8") + empty2, this.sEncryptkeyinHEX);
                sActionData = "01091110" + (this.nCommandCounter - 1).ToString("X8");
                for (int index72 = 0; index72 < 12; ++index72)
                    sActionData += Convert.ToByte(this.cipherText[index72]).ToString("X2");
            }
            return !this.ActionCmd(sActionData, IsLineTrafficEnabled) ? 2 : 0;
        }
        public string GetAARQ()
        {
            commandString = "";
            responseString = "";
            byte num1 = Convert.ToByte((int)this.bytAddMode + 8);
            //this.nCommandCounter = 8;
            this.nRetLSH = Convert.ToByte((int)this.nRecvCntr << 5);
            this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nRetLSH | 16);
            this.nRetLSH = Convert.ToByte((int)this.nSentCntr << 1);
            this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nRetLSH | (int)this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)]);
            /*
                E6E600//LLC
                60//Tag of AARQ
                1D//encoding Length
                A109060760857405080101
                BE10040E
                01000000065F1F0400
                60FEDF//Conformance Block
                FFFF//Max Pdu Size Value
             */
            #region Info
            byte[] nPkt1 = this.nPkt;
            int index1 = (int)num1;
            byte num2 = (byte)(index1 + 1);
            nPkt1[index1] = (byte)230;//E6
            byte[] nPkt2 = this.nPkt;
            int index2 = (int)num2;
            byte num3 = (byte)(index2 + 1);
            nPkt2[index2] = (byte)230;//E6
            byte[] nPkt3 = this.nPkt;
            int index3 = (int)num3;
            byte num4 = (byte)(index3 + 1);
            nPkt3[index3] = (byte)0;//00
            byte[] nPkt4 = this.nPkt;
            int index4 = (int)num4;
            byte num5 = (byte)(index4 + 1);
            nPkt4[index4] = (byte)96;//60
            byte[] nPkt5 = this.nPkt;
            int index5 = (int)num5;
            byte num6 = (byte)(index5 + 1);
            nPkt5[index5] = (byte)0;//00
            byte[] nPkt6 = this.nPkt;
            int index6 = (int)num6;
            byte num7 = (byte)(index6 + 1);
            nPkt6[index6] = (byte)161;//A1
            byte[] nPkt7 = this.nPkt;
            int index7 = (int)num7;
            byte num8 = (byte)(index7 + 1);
            nPkt7[index7] = (byte)9;//09
            byte[] nPkt8 = this.nPkt;
            int index8 = (int)num8;
            byte num9 = (byte)(index8 + 1);
            nPkt8[index8] = (byte)6;//06
            byte[] nPkt9 = this.nPkt;
            int index9 = (int)num9;
            byte num10 = (byte)(index9 + 1);
            nPkt9[index9] = (byte)7;//07
            byte[] nPkt10 = this.nPkt;
            int index10 = (int)num10;
            byte num11 = (byte)(index10 + 1);
            nPkt10[index10] = (byte)96;//60
            byte[] nPkt11 = this.nPkt;
            int index11 = (int)num11;
            byte num12 = (byte)(index11 + 1);
            nPkt11[index11] = (byte)133;//
            byte[] nPkt12 = this.nPkt;
            int index12 = (int)num12;
            byte num13 = (byte)(index12 + 1);
            nPkt12[index12] = (byte)116;//85
            byte[] nPkt13 = this.nPkt;
            int index13 = (int)num13;
            byte num14 = (byte)(index13 + 1);
            nPkt13[index13] = (byte)5;//05
            byte[] nPkt14 = this.nPkt;
            int index14 = (int)num14;
            byte num15 = (byte)(index14 + 1);
            nPkt14[index14] = (byte)8;//08
            byte[] nPkt15 = this.nPkt;
            int index15 = (int)num15;
            byte num16 = (byte)(index15 + 1);
            nPkt15[index15] = (byte)1;//01
            byte num17;
            if (DLMSInfo.IsLNWithCipher || DLMSInfo.IsLNWithCipherDedicatedKey)
            {
                byte[] nPkt16 = this.nPkt;
                int index16 = (int)num16;
                num17 = (byte)(index16 + 1);
                nPkt16[index16] = (byte)3;
            }
            else
            {
                byte[] nPkt17 = this.nPkt;
                int index17 = (int)num16;
                num17 = (byte)(index17 + 1);
                nPkt17[index17] = (byte)1;//01
            }
            if (DLMSInfo.AccessMode != 0)
            {
                ASCIIEncoding asciiEncoding = new ASCIIEncoding();
                if (DLMSInfo.IsLNWithCipher)
                {
                    if (DLMSInfo.TxtSysT.Trim().Length == 16)
                    {
                        byte[] nPkt18 = this.nPkt;
                        int index18 = (int)num17;
                        byte num18 = (byte)(index18 + 1);
                        nPkt18[index18] = (byte)166;
                        byte[] nPkt19 = this.nPkt;
                        int index19 = (int)num18;
                        byte num19 = (byte)(index19 + 1);
                        int num20 = (int)Convert.ToByte(DLMSInfo.TxtSysT.Trim().Length / 2 + 2);
                        nPkt19[index19] = (byte)num20;
                        byte[] nPkt20 = this.nPkt;
                        int index20 = (int)num19;
                        byte num21 = (byte)(index20 + 1);
                        nPkt20[index20] = (byte)4;
                        byte[] nPkt21 = this.nPkt;
                        int index21 = (int)num21;
                        num17 = (byte)(index21 + 1);
                        int num22 = (int)Convert.ToByte(DLMSInfo.TxtSysT.Trim().Length / 2);
                        nPkt21[index21] = (byte)num22;
                        this.Ps = StringToByteArray(DLMSInfo.TxtSysT.Trim());
                        for (int index22 = 0; index22 < DLMSInfo.TxtSysT.Trim().Length / 2; ++index22)
                            this.nPkt[(int)num17++] = this.Ps[index22];
                    }
                    else
                    {
                        byte[] nPkt22 = this.nPkt;
                        int index23 = (int)num17;
                        byte num23 = (byte)(index23 + 1);
                        nPkt22[index23] = (byte)166;
                        byte[] nPkt23 = this.nPkt;
                        int index24 = (int)num23;
                        byte num24 = (byte)(index24 + 1);
                        int num25 = (int)Convert.ToByte(DLMSInfo.TxtSysT.Trim().Length + 2);
                        nPkt23[index24] = (byte)num25;
                        byte[] nPkt24 = this.nPkt;
                        int index25 = (int)num24;
                        byte num26 = (byte)(index25 + 1);
                        nPkt24[index25] = (byte)4;
                        byte[] nPkt25 = this.nPkt;
                        int index26 = (int)num26;
                        num17 = (byte)(index26 + 1);
                        int num27 = (int)Convert.ToByte(DLMSInfo.TxtSysT.Trim().Length);
                        nPkt25[index26] = (byte)num27;
                        this.Ps = asciiEncoding.GetBytes(DLMSInfo.TxtSysT.Trim());
                        for (int index27 = 0; index27 < DLMSInfo.TxtSysT.Trim().Length; ++index27)
                            this.nPkt[(int)num17++] = this.Ps[index27];
                    }
                }
                byte[] nPkt26 = this.nPkt;
                int index28 = (int)num17;
                byte num28 = (byte)(index28 + 1);
                nPkt26[index28] = (byte)138;
                byte[] nPkt27 = this.nPkt;
                int index29 = (int)num28;
                byte num29 = (byte)(index29 + 1);
                nPkt27[index29] = (byte)2;
                byte[] nPkt28 = this.nPkt;
                int index30 = (int)num29;
                byte num30 = (byte)(index30 + 1);
                nPkt28[index30] = (byte)7;
                byte[] nPkt29 = this.nPkt;
                int index31 = (int)num30;
                byte num31 = (byte)(index31 + 1);
                nPkt29[index31] = (byte)128;
                byte[] nPkt30 = this.nPkt;
                int index32 = (int)num31;
                byte num32 = (byte)(index32 + 1);
                nPkt30[index32] = (byte)139;
                byte[] nPkt31 = this.nPkt;
                int index33 = (int)num32;
                byte num33 = (byte)(index33 + 1);
                nPkt31[index33] = (byte)7;
                byte[] nPkt32 = this.nPkt;
                int index34 = (int)num33;
                byte num34 = (byte)(index34 + 1);
                nPkt32[index34] = (byte)96;//0x60
                byte[] nPkt33 = this.nPkt;
                int index35 = (int)num34;
                byte num35 = (byte)(index35 + 1);
                nPkt33[index35] = (byte)133;//0x85
                byte[] nPkt34 = this.nPkt;
                int index36 = (int)num35;
                byte num36 = (byte)(index36 + 1);
                nPkt34[index36] = (byte)116;//0x74
                byte[] nPkt35 = this.nPkt;
                int index37 = (int)num36;
                byte num37 = (byte)(index37 + 1);
                nPkt35[index37] = (byte)5;//0x05
                byte[] nPkt36 = this.nPkt;
                int index38 = (int)num37;
                byte num38 = (byte)(index38 + 1);
                nPkt36[index38] = (byte)8;//0x08
                byte[] nPkt37 = this.nPkt;
                int index39 = (int)num38;
                byte num39 = (byte)(index39 + 1);
                nPkt37[index39] = (byte)2;//0x02
                byte num40;
                if (DLMSInfo.IsLNWithCipher && DLMSInfo.AccessMode == 2 && DLMSInfo.IsWithGMAC)
                {
                    byte[] nPkt38 = this.nPkt;
                    int index40 = (int)num39;
                    num40 = (byte)(index40 + 1);
                    nPkt38[index40] = (byte)5;
                }
                else if (DLMSInfo.IsLNWithCipher && DLMSInfo.AccessMode == 2 && !DLMSInfo.IsWithGMAC)
                {
                    byte[] nPkt39 = this.nPkt;
                    int index41 = (int)num39;
                    num40 = (byte)(index41 + 1);
                    nPkt39[index41] = (byte)2;
                }
                #region Condition for FW ASSOCIATION
                else if (DLMSInfo.IsLNWithCipher && DLMSInfo.AccessMode == 4 && !DLMSInfo.IsWithGMAC)
                {
                    byte[] nPkt39 = this.nPkt;
                    int index41 = (int)num39;
                    num40 = (byte)(index41 + 1);
                    nPkt39[index41] = (byte)2;
                }
                #endregion
                else
                {
                    byte[] nPkt40 = this.nPkt;
                    int index42 = (int)num39;
                    num40 = (byte)(index42 + 1);
                    #region New logic to handle FW Association
                    int num41;
                    if (DLMSInfo.AccessMode == 4)
                        num41 = (int)Convert.ToByte(DLMSInfo.AccessMode - 2);
                    else
                        num41 = (int)Convert.ToByte(DLMSInfo.AccessMode);
                    #endregion
                    //int num41 = (int)Convert.ToByte(DLMSInfo.AccessMode);//old
                    nPkt40[index42] = (byte)num41;
                }
                byte[] nPkt41 = this.nPkt;
                int index43 = (int)num40;
                byte num42 = (byte)(index43 + 1);
                nPkt41[index43] = (byte)172;//0xAC
                byte[] nPkt42 = this.nPkt;
                int index44 = (int)num42;
                byte num43 = (byte)(index44 + 1);
                int num44 = (int)Convert.ToByte(2 + DLMSInfo.MeterAuthPasswordWrite.Length);
                nPkt42[index44] = (byte)num44;
                byte[] nPkt43 = this.nPkt;
                int index45 = (int)num43;
                byte num45 = (byte)(index45 + 1);
                nPkt43[index45] = (byte)128;
                byte[] nPkt44 = this.nPkt;
                int index46 = (int)num45;
                num17 = (byte)(index46 + 1);
                int num46 = (int)Convert.ToByte(DLMSInfo.MeterAuthPasswordWrite.Length);
                nPkt44[index46] = (byte)num46;
                this.Ps = DLMSInfo.AccessMode != 1 ? asciiEncoding.GetBytes("GNSRAPDRP-" + DateTime.Now.ToString("HHmmss")) : asciiEncoding.GetBytes(DLMSInfo.MeterAuthPasswordWrite);
                for (int index47 = 0; index47 < DLMSInfo.MeterAuthPasswordWrite.Length; ++index47)
                    this.nPkt[(int)num17++] = this.Ps[index47];
            }
            byte num47;
            if (DLMSInfo.IsLNWithCipher || DLMSInfo.IsLNWithCipherDedicatedKey)
            {
                byte num48 = 0;
                if (DLMSInfo.IsLNWithCipherDedicatedKey)
                    num48 = (byte)17;
                byte[] nPkt45 = this.nPkt;
                int index48 = (int)num17;
                byte num49 = (byte)(index48 + 1);
                nPkt45[index48] = (byte)190;
                byte[] nPkt46 = this.nPkt;
                int index49 = (int)num49;
                byte num50 = (byte)(index49 + 1);
                int num51 = (int)Convert.ToByte(35 + (int)num48);
                nPkt46[index49] = (byte)num51;
                byte[] nPkt47 = this.nPkt;
                int index50 = (int)num50;
                byte num52 = (byte)(index50 + 1);
                nPkt47[index50] = (byte)4;
                byte[] nPkt48 = this.nPkt;
                int index51 = (int)num52;
                byte num53 = (byte)(index51 + 1);
                int num54 = (int)Convert.ToByte(33 + (int)num48);
                nPkt48[index51] = (byte)num54;
                byte[] nPkt49 = this.nPkt;
                int index52 = (int)num53;
                byte num55 = (byte)(index52 + 1);
                nPkt49[index52] = (byte)33;
                byte[] nPkt50 = this.nPkt;
                int index53 = (int)num55;
                byte num56 = (byte)(index53 + 1);
                int num57 = (int)Convert.ToByte(31 + (int)num48);
                nPkt50[index53] = (byte)num57;
                byte[] nPkt51 = this.nPkt;
                int index54 = (int)num56;
                byte num58 = (byte)(index54 + 1);
                nPkt51[index54] = (byte)48;
                byte[] nPkt52 = this.nPkt;
                int index55 = (int)num58;
                byte num59 = (byte)(index55 + 1);
                nPkt52[index55] = (byte)0;
                byte[] nPkt53 = this.nPkt;
                int index56 = (int)num59;
                byte num60 = (byte)(index56 + 1);
                nPkt53[index56] = (byte)0;
                byte[] nPkt54 = this.nPkt;
                int index57 = (int)num60;
                byte num61 = (byte)(index57 + 1);
                int num62 = (int)Convert.ToByte(this.nCommandCounter / 256);
                nPkt54[index57] = (byte)num62;
                byte[] nPkt55 = this.nPkt;
                int index58 = (int)num61;
                num47 = (byte)(index58 + 1);
                int num63 = (int)Convert.ToByte(this.nCommandCounter % 256);
                nPkt55[index58] = (byte)num63;
                ASCIIEncoding asciiEncoding = new ASCIIEncoding();
                this.sEncryptkeyinHEX = string.Empty;
                if (DLMSInfo.TxtEK.Trim().Length == 32)
                {
                    this.sEncryptkeyinHEX = DLMSInfo.TxtEK.Trim();
                }
                else
                {
                    this.Ps1 = asciiEncoding.GetBytes(DLMSInfo.TxtEK.Trim());
                    for (int index59 = 0; index59 < this.Ps1.Length; ++index59)
                        this.sEncryptkeyinHEX += this.Ps1[index59].ToString("X2");
                }
                if (DLMSInfo.IsLNWithCipherDedicatedKey)
                {
                    this.sDedicatedkey = RandomString(16);
                    this.Ps1 = asciiEncoding.GetBytes(this.sDedicatedkey);
                    this.sDedicatedkeyinHEX = string.Empty;
                    for (int index60 = 0; index60 < this.Ps1.Length; ++index60)
                        this.sDedicatedkeyinHEX += this.Ps1[index60].ToString("X2");
                    string[] strArray = new string[5]
                    {
                    "30",
                    this.nCommandCounter++.ToString("X8"),
                    "010110",
                    this.sDedicatedkeyinHEX,
                    $"0000065F1F0400{DLMSInfo.ConformanceBlock}FFFF"//OLD Conformance block
                    };
                    #region OLD Conformance Block
                    //        string[] strArray = new string[5]
                    //        {
                    //"30",
                    //this.nCommandCounter++.ToString("X8"),
                    //"010110",
                    //this.sDedicatedkeyinHEX,
                    //"0000065F1F040000181DFFFF"//OLD Conformance block
                    //        };
                    #endregion
                    foreach (byte num64 in this.Encrypt(string.Concat(strArray), this.sEncryptkeyinHEX))
                        this.nPkt[(int)num47++] = num64;
                    this.sEncryptkeyinHEX = this.sDedicatedkeyinHEX;
                }
                else
                {
                    foreach (byte num65 in this.Encrypt("30" + this.nCommandCounter++.ToString("X8") + $"01000000065F1F0400{DLMSInfo.ConformanceBlock}FFFF", this.sEncryptkeyinHEX))
                        this.nPkt[(int)num47++] = num65;
                    #region OLD Conformance Block
                    //foreach (byte num65 in this.Encrypt("30" + this.nCommandCounter++.ToString("X8") + "01000000065F1F040000181DFFFF", this.sEncryptkeyinHEX))
                    //    this.nPkt[(int)num47++] = num65;
                    #endregion
                }
            }
            else
            {
                byte[] nPkt56 = this.nPkt;
                int index61 = (int)num17;
                byte num66 = (byte)(index61 + 1);
                nPkt56[index61] = (byte)190;//BE
                byte[] nPkt57 = this.nPkt;
                int index62 = (int)num66;
                byte num67 = (byte)(index62 + 1);
                nPkt57[index62] = (byte)16;//10
                byte[] nPkt58 = this.nPkt;
                int index63 = (int)num67;
                byte num68 = (byte)(index63 + 1);
                nPkt58[index63] = (byte)4;//04
                byte[] nPkt59 = this.nPkt;
                int index64 = (int)num68;
                num47 = (byte)(index64 + 1);
                nPkt59[index64] = (byte)14;//0E
                foreach (byte num69 in StringToByteArray($"01000000065F1F0400{DLMSInfo.ConformanceBlock}FFFF"))
                    this.nPkt[(int)num47++] = num69;
                #region OLD Conformance Block
                //foreach (byte num69 in StringToByteArray("01000000065F1F040000181DFFFF"))
                //    this.nPkt[(int)num47++] = num69;
                #endregion
            }
            #endregion
            this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 12)] = Convert.ToByte((int)num47 + 1 - 14 - (int)this.bytAddMode);
            this.nPkt[2] = Convert.ToByte((int)num47 + 1);
            this.DCl.fcs(ref this.nPkt, (int)Convert.ToByte((int)this.bytAddMode + 5), (byte)1);
            this.DCl.fcs(ref this.nPkt, (int)Convert.ToByte((int)num47 - 1), (byte)1);
            this.nPkt[(int)num47 + 2] = (byte)126;//7E
            this.ClearBuffer();
            //this.Wait(60.0);//old 100 new 60
            bool flag = false;
            commandString = "";
            for (int i = 0; i < ((int)num47 + 3); i++)
            {
                commandString += nPkt[i].ToString("X2");
            }
            return commandString;
        }

        public int AARQ(byte bytAsslevel, string strPsd, int nWait, byte nTryCount, byte nTimeOut)
        {
            byte num1 = Convert.ToByte((int)this.bytAddMode + 8);
            this.nRetLSH = Convert.ToByte((int)this.nRecvCntr << 5);
            this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nRetLSH | 16);
            this.nRetLSH = Convert.ToByte((int)this.nSentCntr << 1);
            this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nRetLSH | (int)this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)]);
            byte[] nPkt1 = this.nPkt;
            int index1 = (int)num1;
            byte num2 = (byte)(index1 + 1);
            nPkt1[index1] = (byte)230;//E6
            byte[] nPkt2 = this.nPkt;
            int index2 = (int)num2;
            byte num3 = (byte)(index2 + 1);
            nPkt2[index2] = (byte)230;//E6
            byte[] nPkt3 = this.nPkt;
            int index3 = (int)num3;
            byte num4 = (byte)(index3 + 1);
            nPkt3[index3] = (byte)0;//00
            byte[] nPkt4 = this.nPkt;
            int index4 = (int)num4;
            byte num5 = (byte)(index4 + 1);
            nPkt4[index4] = (byte)96;//60
            byte[] nPkt5 = this.nPkt;
            int index5 = (int)num5;
            byte num6 = (byte)(index5 + 1);
            nPkt5[index5] = (byte)0;//00
            byte[] nPkt6 = this.nPkt;
            int index6 = (int)num6;
            byte num7 = (byte)(index6 + 1);
            nPkt6[index6] = (byte)161;//A1
            byte[] nPkt7 = this.nPkt;
            int index7 = (int)num7;
            byte num8 = (byte)(index7 + 1);
            nPkt7[index7] = (byte)9;//09
            byte[] nPkt8 = this.nPkt;
            int index8 = (int)num8;
            byte num9 = (byte)(index8 + 1);
            nPkt8[index8] = (byte)6;//06
            byte[] nPkt9 = this.nPkt;
            int index9 = (int)num9;
            byte num10 = (byte)(index9 + 1);
            nPkt9[index9] = (byte)7;//07
            byte[] nPkt10 = this.nPkt;
            int index10 = (int)num10;
            byte num11 = (byte)(index10 + 1);
            nPkt10[index10] = (byte)96;//60
            byte[] nPkt11 = this.nPkt;
            int index11 = (int)num11;
            byte num12 = (byte)(index11 + 1);
            nPkt11[index11] = (byte)133;//85
            byte[] nPkt12 = this.nPkt;
            int index12 = (int)num12;
            byte num13 = (byte)(index12 + 1);
            nPkt12[index12] = (byte)116;//74
            byte[] nPkt13 = this.nPkt;
            int index13 = (int)num13;
            byte num14 = (byte)(index13 + 1);
            nPkt13[index13] = (byte)5;//05
            byte[] nPkt14 = this.nPkt;
            int index14 = (int)num14;
            byte num15 = (byte)(index14 + 1);
            nPkt14[index14] = (byte)8;//08
            byte[] nPkt15 = this.nPkt;
            int index15 = (int)num15;
            byte num16 = (byte)(index15 + 1);
            nPkt15[index15] = (byte)1;//01
            byte num17;
            if (DLMSInfo.IsLNWithCipher || DLMSInfo.IsLNWithCipherDedicatedKey)
            {
                byte[] nPkt16 = this.nPkt;
                int index16 = (int)num16;
                num17 = (byte)(index16 + 1);
                nPkt16[index16] = (byte)3;
            }
            else
            {
                byte[] nPkt17 = this.nPkt;
                int index17 = (int)num16;
                num17 = (byte)(index17 + 1);
                nPkt17[index17] = (byte)1;//01
            }
            if (bytAsslevel == (byte)0)
            {
                this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 12)] = (byte)29;
            }
            else
            {
                ASCIIEncoding asciiEncoding = new ASCIIEncoding();
                if (DLMSInfo.IsLNWithCipher || DLMSInfo.IsLNWithCipherDedicatedKey)
                {
                    byte[] nPkt18 = this.nPkt;
                    int index18 = (int)num17;
                    byte num18 = (byte)(index18 + 1);
                    nPkt18[index18] = (byte)166;
                    byte[] nPkt19 = this.nPkt;
                    int index19 = (int)num18;
                    byte num19 = (byte)(index19 + 1);
                    int num20 = (int)Convert.ToByte(DLMSInfo.TxtSysT.Trim().Length + 2);
                    nPkt19[index19] = (byte)num20;
                    byte[] nPkt20 = this.nPkt;
                    int index20 = (int)num19;
                    byte num21 = (byte)(index20 + 1);
                    nPkt20[index20] = (byte)4;
                    byte[] nPkt21 = this.nPkt;
                    int index21 = (int)num21;
                    num17 = (byte)(index21 + 1);
                    int num22 = (int)Convert.ToByte(DLMSInfo.TxtSysT.Trim().Length);
                    nPkt21[index21] = (byte)num22;
                    this.Ps = asciiEncoding.GetBytes(DLMSInfo.TxtSysT.Trim());
                    for (int index22 = 0; index22 < DLMSInfo.TxtSysT.Trim().Length; ++index22)
                        this.nPkt[(int)num17++] = this.Ps[index22];
                }
                byte[] nPkt22 = this.nPkt;
                int index23 = (int)num17;
                byte num23 = (byte)(index23 + 1);
                nPkt22[index23] = (byte)138;
                byte[] nPkt23 = this.nPkt;
                int index24 = (int)num23;
                byte num24 = (byte)(index24 + 1);
                nPkt23[index24] = (byte)2;
                byte[] nPkt24 = this.nPkt;
                int index25 = (int)num24;
                byte num25 = (byte)(index25 + 1);
                nPkt24[index25] = (byte)7;
                byte[] nPkt25 = this.nPkt;
                int index26 = (int)num25;
                byte num26 = (byte)(index26 + 1);
                nPkt25[index26] = (byte)128;
                byte[] nPkt26 = this.nPkt;
                int index27 = (int)num26;
                byte num27 = (byte)(index27 + 1);
                nPkt26[index27] = (byte)139;
                byte[] nPkt27 = this.nPkt;
                int index28 = (int)num27;
                byte num28 = (byte)(index28 + 1);
                nPkt27[index28] = (byte)7;
                byte[] nPkt28 = this.nPkt;
                int index29 = (int)num28;
                byte num29 = (byte)(index29 + 1);
                nPkt28[index29] = (byte)96;
                byte[] nPkt29 = this.nPkt;
                int index30 = (int)num29;
                byte num30 = (byte)(index30 + 1);
                nPkt29[index30] = (byte)133;
                byte[] nPkt30 = this.nPkt;
                int index31 = (int)num30;
                byte num31 = (byte)(index31 + 1);
                nPkt30[index31] = (byte)116;
                byte[] nPkt31 = this.nPkt;
                int index32 = (int)num31;
                byte num32 = (byte)(index32 + 1);
                nPkt31[index32] = (byte)5;
                byte[] nPkt32 = this.nPkt;
                int index33 = (int)num32;
                byte num33 = (byte)(index33 + 1);
                nPkt32[index33] = (byte)8;
                byte[] nPkt33 = this.nPkt;
                int index34 = (int)num33;
                byte num34 = (byte)(index34 + 1);
                nPkt33[index34] = (byte)2;
                byte num35;
                if ((DLMSInfo.IsLNWithCipher || DLMSInfo.IsLNWithCipherDedicatedKey) && DLMSInfo.AccessMode == 2)
                {
                    byte[] nPkt34 = this.nPkt;
                    int index35 = (int)num34;
                    num35 = (byte)(index35 + 1);
                    nPkt34[index35] = (byte)2;
                }
                else if (DLMSInfo.AccessMode == 3)
                {
                    byte[] nPkt35 = this.nPkt;
                    int index36 = (int)num34;
                    num35 = (byte)(index36 + 1);
                    nPkt35[index36] = (byte)1;
                }
                else
                {
                    byte[] nPkt36 = this.nPkt;
                    int index37 = (int)num34;
                    num35 = (byte)(index37 + 1);
                    int num36 = (int)bytAsslevel;
                    nPkt36[index37] = (byte)num36;
                }
                byte[] nPkt37 = this.nPkt;
                int index38 = (int)num35;
                byte num37 = (byte)(index38 + 1);
                nPkt37[index38] = (byte)172;
                byte[] nPkt38 = this.nPkt;
                int index39 = (int)num37;
                byte num38 = (byte)(index39 + 1);
                int num39 = (int)Convert.ToByte(2 + strPsd.Length);
                nPkt38[index39] = (byte)num39;
                byte[] nPkt39 = this.nPkt;
                int index40 = (int)num38;
                byte num40 = (byte)(index40 + 1);
                nPkt39[index40] = (byte)128;
                byte[] nPkt40 = this.nPkt;
                int index41 = (int)num40;
                num17 = (byte)(index41 + 1);
                int num41 = (int)Convert.ToByte(strPsd.Length);
                nPkt40[index41] = (byte)num41;
                this.Ps = bytAsslevel == (byte)1 || bytAsslevel == (byte)3 ? asciiEncoding.GetBytes(strPsd) : asciiEncoding.GetBytes("GNSRAPDRP-" + DateTime.Now.ToString("HHmmss"));
                for (int index42 = 0; index42 < strPsd.Length; ++index42)
                    this.nPkt[(int)num17++] = this.Ps[index42];
            }
            byte num42;
            if (DLMSInfo.IsLNWithCipher || DLMSInfo.IsLNWithCipherDedicatedKey)
            {
                byte num43 = 0;
                if (DLMSInfo.IsLNWithCipher || DLMSInfo.IsLNWithCipherDedicatedKey)
                    num43 = (byte)17;
                byte[] nPkt41 = this.nPkt;
                int index43 = (int)num17;
                byte num44 = (byte)(index43 + 1);
                nPkt41[index43] = (byte)190;
                byte[] nPkt42 = this.nPkt;
                int index44 = (int)num44;
                byte num45 = (byte)(index44 + 1);
                int num46 = (int)Convert.ToByte(35 + (int)num43);
                nPkt42[index44] = (byte)num46;
                byte[] nPkt43 = this.nPkt;
                int index45 = (int)num45;
                byte num47 = (byte)(index45 + 1);
                nPkt43[index45] = (byte)4;
                byte[] nPkt44 = this.nPkt;
                int index46 = (int)num47;
                byte num48 = (byte)(index46 + 1);
                int num49 = (int)Convert.ToByte(33 + (int)num43);
                nPkt44[index46] = (byte)num49;
                byte[] nPkt45 = this.nPkt;
                int index47 = (int)num48;
                byte num50 = (byte)(index47 + 1);
                nPkt45[index47] = (byte)33;
                byte[] nPkt46 = this.nPkt;
                int index48 = (int)num50;
                byte num51 = (byte)(index48 + 1);
                int num52 = (int)Convert.ToByte(31 + (int)num43);
                nPkt46[index48] = (byte)num52;
                byte[] nPkt47 = this.nPkt;
                int index49 = (int)num51;
                byte num53 = (byte)(index49 + 1);
                nPkt47[index49] = (byte)48;
                byte[] nPkt48 = this.nPkt;
                int index50 = (int)num53;
                byte num54 = (byte)(index50 + 1);
                nPkt48[index50] = (byte)0;
                byte[] nPkt49 = this.nPkt;
                int index51 = (int)num54;
                byte num55 = (byte)(index51 + 1);
                nPkt49[index51] = (byte)0;
                byte[] nPkt50 = this.nPkt;
                int index52 = (int)num55;
                byte num56 = (byte)(index52 + 1);
                int num57 = (int)Convert.ToByte(this.nCommandCounter / 256);
                nPkt50[index52] = (byte)num57;
                byte[] nPkt51 = this.nPkt;
                int index53 = (int)num56;
                num42 = (byte)(index53 + 1);
                int num58 = (int)Convert.ToByte(this.nCommandCounter % 256);
                nPkt51[index53] = (byte)num58;
                ASCIIEncoding asciiEncoding = new ASCIIEncoding();
                this.sEncryptkeyinHEX = string.Empty;
                this.Ps1 = asciiEncoding.GetBytes(DLMSInfo.TxtEK.Trim());
                for (int index54 = 0; index54 < this.Ps1.Length; ++index54)
                    this.sEncryptkeyinHEX += this.Ps1[index54].ToString("X2");
                if (DLMSInfo.IsLNWithCipher || DLMSInfo.IsLNWithCipherDedicatedKey)
                {
                    this.sDedicatedkey = RandomString(16);
                    this.Ps1 = asciiEncoding.GetBytes(this.sDedicatedkey);
                    this.sDedicatedkeyinHEX = string.Empty;
                    for (int index55 = 0; index55 < this.Ps1.Length; ++index55)
                        this.sDedicatedkeyinHEX += this.Ps1[index55].ToString("X2");
                    string[] strArray = new string[5]
                    {
            "30",
            this.nCommandCounter++.ToString("X8"),
            "010110",
            this.sDedicatedkeyinHEX,
            "0000065F1F040000181DFFFF"
                    };
                    foreach (byte num59 in this.Encrypt(string.Concat(strArray), this.sEncryptkeyinHEX))
                        this.nPkt[(int)num42++] = num59;
                    this.sEncryptkeyinHEX = this.sDedicatedkeyinHEX;
                }
                else
                {
                    foreach (byte num60 in this.Encrypt("30" + this.nCommandCounter++.ToString("X8") + "01000000065F1F040000181DFFFF", this.sEncryptkeyinHEX))
                        this.nPkt[(int)num42++] = num60;
                }
            }
            else
            {
                byte[] nPkt52 = this.nPkt;
                int index56 = (int)num17;
                byte num61 = (byte)(index56 + 1);
                nPkt52[index56] = (byte)190;//BE
                byte[] nPkt53 = this.nPkt;
                int index57 = (int)num61;
                byte num62 = (byte)(index57 + 1);
                nPkt53[index57] = (byte)16;//10
                byte[] nPkt54 = this.nPkt;
                int index58 = (int)num62;
                byte num63 = (byte)(index58 + 1);
                nPkt54[index58] = (byte)4;//04
                byte[] nPkt55 = this.nPkt;
                int index59 = (int)num63;
                num42 = (byte)(index59 + 1);
                nPkt55[index59] = (byte)14;//0E
                foreach (byte num64 in StringToByteArray("01000000065F1F040000181DFFFF"))
                    this.nPkt[(int)num42++] = num64;
            }
            this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 12)] = Convert.ToByte((int)num42 + 1 - 14);
            this.nPkt[2] = Convert.ToByte((int)num42 + 1);
            this.DCl.fcs(ref this.nPkt, (ushort)Convert.ToByte((int)this.bytAddMode + 5), (byte)1);
            this.DCl.fcs(ref this.nPkt, (ushort)Convert.ToByte((int)num42 - 1), (byte)1);
            this.nPkt[(int)num42 + 2] = (byte)126;
            byte num65 = 0;
            bool flag;
            do
            {
                this.ClearBuffer();
                flag = false;
                this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)num42 + 3));
                DateTime now = DateTime.Now;
                LineTrafficControlEventHandler("     AARQ", "Send");
                SendDataPrint(nPkt, Convert.ToByte((int)num42 + 3));
                do
                {
                    this.Wait((double)nWait);
                    Application.DoEvents();
                    this.DataReceive();
                    if (this.nCounter > 2 && (int)this.nRcvPkt[2] + 2 <= this.nCounter && this.DCl.fcs(ref this.nRcvPkt, int.Parse(this.nRcvPkt[2].ToString()), (byte)0))
                    {
                        flag = true;
                        this.FrameType();
                        break;
                    }
                    if (DateTime.Now.Subtract(now).Seconds > (int)nTimeOut && (int)num65 < (int)nTryCount)
                    {
                        ++num65;
                        break;
                    }
                }
                while (!flag);
            }
            while (!flag && (int)num65 != (int)nTryCount);
            RecvDataPrint(nRcvPkt, nCounter);
            LineTrafficControlEventHandler("\r\n", "Send");
            this.temp = string.Empty;
            for (int i = 0; i < this.nCounter; ++i)
                this.temp += this.nRcvPkt[i].ToString("X2");
            this.temp += "\r\n";
            //this.Response1.SelectionColor = Color.Red;
            //this.Response1.AppendText("<-- " + this.temp);
            int num66 = 13;
            if (this.nRcvPkt[(int)this.bytAddMode + num66] == (byte)161)
                num66 += 2 + (int)this.nRcvPkt[(int)this.bytAddMode + num66 + 1];
            if (this.nRcvPkt[(int)this.bytAddMode + num66] == (byte)162)
                num66 += 2 + (int)this.nRcvPkt[(int)this.bytAddMode + num66 + 1];
            if (this.nRcvPkt[(int)this.bytAddMode + num66] == (byte)163)
            {
                int num67 = (int)this.nRcvPkt[(int)this.bytAddMode + num66 + (int)this.nRcvPkt[(int)this.bytAddMode + num66 + 1] + 1];
                num66 += 2 + (int)this.nRcvPkt[(int)this.bytAddMode + num66 + 1];
            }
            if (this.nRcvPkt[(int)this.bytAddMode + num66] == (byte)136)
                num66 += 2 + (int)this.nRcvPkt[(int)this.bytAddMode + num66 + 1];
            if (this.nRcvPkt[(int)this.bytAddMode + num66] == (byte)137)
                num66 += 2 + (int)this.nRcvPkt[(int)this.bytAddMode + num66 + 1];
            if (this.nRcvPkt[(int)this.bytAddMode + num66] == (byte)170)
                num66 += 2 + (int)this.nRcvPkt[(int)this.bytAddMode + num66 + 1];
            if (this.nRcvPkt[(int)this.bytAddMode + num66] == (byte)164)
            {
                this.sSYSTaginHEX = string.Empty;
                for (int index60 = 0; index60 < (int)Convert.ToInt16(this.nRcvPkt[(int)this.bytAddMode + num66 + 3].ToString(), 16); ++index60)
                    this.sSYSTaginHEX += this.nRcvPkt[(int)this.bytAddMode + num66 + 4 + index60].ToString("X2");
                num66 += 2 + (int)this.nRcvPkt[(int)this.bytAddMode + num66 + 1];
            }
            if (this.nRcvPkt[(int)this.bytAddMode + num66] == (byte)190)
            {
                int num68 = 0;
                if (DLMSInfo.IsLNWithCipher || DLMSInfo.IsLNWithCipherDedicatedKey)
                {
                    string empty = string.Empty;
                    for (int index61 = (int)this.bytAddMode + num66 + 6; index61 < this.nCounter - 3; ++index61)
                        empty += this.nRcvPkt[index61].ToString("X2");
                    byte[] sourceArray = this.Decrypt(empty, this.sEncryptkeyinHEX);
                    num68 = empty.Length / 2 - sourceArray.Length;
                    Array.Copy((Array)sourceArray, 0, (Array)this.nRcvPkt, (int)this.bytAddMode + num66 + 6, sourceArray.Length);
                    this.nRcvPkt[(int)this.bytAddMode + num66 + 1] -= Convert.ToByte(num68);
                }
                int num69 = num66 + 2;
                if (this.nRcvPkt[(int)this.bytAddMode + num69] == (byte)4)
                {
                    this.nRcvPkt[(int)this.bytAddMode + num69 + 1] -= Convert.ToByte(num68);
                    num69 += 2;
                }
                if (this.nRcvPkt[(int)this.bytAddMode + num69] == (byte)40)
                {
                    this.nRcvPkt[(int)this.bytAddMode + num69 + 1] -= Convert.ToByte(num68);
                    num69 += 2;
                }
                if (this.nRcvPkt[(int)this.bytAddMode + num69] == (byte)8)
                    num69 += 3;
                if (this.nRcvPkt[(int)this.bytAddMode + num69] == (byte)95 && this.nRcvPkt[(int)this.bytAddMode + num69 + 1] == (byte)31)
                {
                    int num70 = num69 + 3;
                    this.intConfBlk = long.Parse(this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + num70)].ToString("X2") + this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + num70 + 1)].ToString("X2") + this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + num70 + 2)].ToString("X2") + this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + num70 + 3)].ToString("X2"), NumberStyles.HexNumber);
                }
            }
            if (!flag || this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 28)] != (byte)0 || this.nCounter <= 27)
                return 1;
            if (bytAsslevel != (byte)2)
                return 0;
            string empty1 = string.Empty;
            this.intConfBlk = long.Parse(this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 71)].ToString("X2") + this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 72)].ToString("X2") + this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 73)].ToString("X2"), NumberStyles.HexNumber);
            this.keyBytes = StrToByteArray(DLMSInfo.MeterAuthPasswordWrite.Trim());
            Aes aes = new Aes(Aes.KeySize.Bits128, this.keyBytes);
            for (int index62 = 0; index62 < 16; ++index62)
                this.plainText[index62] = this.nRcvPkt[index62 + (int)Convert.ToByte((int)this.bytAddMode + 53)];
            aes.Cipher(this.plainText, this.cipherText);
            string sActionData;
            if (this.cipherText.Length == 0)
            {
                sActionData = "0";
            }
            else
            {
                sActionData = "0109" + this.cipherText.Length.ToString("X2");
                for (int index63 = 0; index63 < this.cipherText.Length; ++index63)
                    sActionData += this.cipherText[index63].ToString("X2");
            }
            if (this.ActionCmd(sActionData))
                return 0;
            //this.LblStatus.Text = "Authentication Fail";
            return 2;
        }
        private int AARQFG(byte nWait, byte nTryCount, byte nTimeOut, bool IsLineTrafficEnabled = true)
        {
            byte num1 = Convert.ToByte((int)this.bytAddMode + 8);
            this.nRetLSH = Convert.ToByte((int)this.nRecvCntr << 5);
            this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nRetLSH | 16);
            this.nRetLSH = Convert.ToByte((int)this.nSentCntr << 1);
            this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nRetLSH | (int)this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)]);
            byte[] nPkt1 = this.nPkt;
            int index1 = (int)num1;
            byte num2 = (byte)(index1 + 1);
            nPkt1[index1] = (byte)230;
            byte[] nPkt2 = this.nPkt;
            int index2 = (int)num2;
            byte num3 = (byte)(index2 + 1);
            nPkt2[index2] = (byte)230;
            byte[] nPkt3 = this.nPkt;
            int index3 = (int)num3;
            byte num4 = (byte)(index3 + 1);
            nPkt3[index3] = (byte)0;
            byte[] nPkt4 = this.nPkt;
            int index4 = (int)num4;
            byte num5 = (byte)(index4 + 1);
            nPkt4[index4] = (byte)96;
            byte[] nPkt5 = this.nPkt;
            int index5 = (int)num5;
            byte num6 = (byte)(index5 + 1);
            nPkt5[index5] = (byte)0;
            byte[] nPkt6 = this.nPkt;
            int index6 = (int)num6;
            byte num7 = (byte)(index6 + 1);
            nPkt6[index6] = (byte)161;
            byte[] nPkt7 = this.nPkt;
            int index7 = (int)num7;
            byte num8 = (byte)(index7 + 1);
            nPkt7[index7] = (byte)9;
            byte[] nPkt8 = this.nPkt;
            int index8 = (int)num8;
            byte num9 = (byte)(index8 + 1);
            nPkt8[index8] = (byte)6;
            byte[] nPkt9 = this.nPkt;
            int index9 = (int)num9;
            byte num10 = (byte)(index9 + 1);
            nPkt9[index9] = (byte)7;
            byte[] nPkt10 = this.nPkt;
            int index10 = (int)num10;
            byte num11 = (byte)(index10 + 1);
            nPkt10[index10] = (byte)96;
            byte[] nPkt11 = this.nPkt;
            int index11 = (int)num11;
            byte num12 = (byte)(index11 + 1);
            nPkt11[index11] = (byte)133;
            byte[] nPkt12 = this.nPkt;
            int index12 = (int)num12;
            byte num13 = (byte)(index12 + 1);
            nPkt12[index12] = (byte)116;
            byte[] nPkt13 = this.nPkt;
            int index13 = (int)num13;
            byte num14 = (byte)(index13 + 1);
            nPkt13[index13] = (byte)5;
            byte[] nPkt14 = this.nPkt;
            int index14 = (int)num14;
            byte num15 = (byte)(index14 + 1);
            nPkt14[index14] = (byte)8;
            byte[] nPkt15 = this.nPkt;
            int index15 = (int)num15;
            byte num16 = (byte)(index15 + 1);
            nPkt15[index15] = (byte)1;
            byte[] nPkt16 = this.nPkt;
            int index16 = (int)num16;
            byte num17 = (byte)(index16 + 1);
            nPkt16[index16] = (byte)1;
            byte[] nPkt17 = this.nPkt;
            int index17 = (int)num17;
            byte num18 = (byte)(index17 + 1);
            nPkt17[index17] = (byte)138;
            byte[] nPkt18 = this.nPkt;
            int index18 = (int)num18;
            byte num19 = (byte)(index18 + 1);
            nPkt18[index18] = (byte)2;
            byte[] nPkt19 = this.nPkt;
            int index19 = (int)num19;
            byte num20 = (byte)(index19 + 1);
            nPkt19[index19] = (byte)7;
            byte[] nPkt20 = this.nPkt;
            int index20 = (int)num20;
            byte num21 = (byte)(index20 + 1);
            nPkt20[index20] = (byte)128;
            byte[] nPkt21 = this.nPkt;
            int index21 = (int)num21;
            byte num22 = (byte)(index21 + 1);
            nPkt21[index21] = (byte)139;
            byte[] nPkt22 = this.nPkt;
            int index22 = (int)num22;
            byte num23 = (byte)(index22 + 1);
            nPkt22[index22] = (byte)7;
            byte[] nPkt23 = this.nPkt;
            int index23 = (int)num23;
            byte num24 = (byte)(index23 + 1);
            nPkt23[index23] = (byte)96;
            byte[] nPkt24 = this.nPkt;
            int index24 = (int)num24;
            byte num25 = (byte)(index24 + 1);
            nPkt24[index24] = (byte)133;
            byte[] nPkt25 = this.nPkt;
            int index25 = (int)num25;
            byte num26 = (byte)(index25 + 1);
            nPkt25[index25] = (byte)116;
            byte[] nPkt26 = this.nPkt;
            int index26 = (int)num26;
            byte num27 = (byte)(index26 + 1);
            nPkt26[index26] = (byte)5;
            byte[] nPkt27 = this.nPkt;
            int index27 = (int)num27;
            byte num28 = (byte)(index27 + 1);
            nPkt27[index27] = (byte)8;
            byte[] nPkt28 = this.nPkt;
            int index28 = (int)num28;
            byte num29 = (byte)(index28 + 1);
            nPkt28[index28] = (byte)2;
            byte[] nPkt29 = this.nPkt;
            int index29 = (int)num29;
            byte num30 = (byte)(index29 + 1);
            nPkt29[index29] = (byte)1;
            byte[] nPkt30 = this.nPkt;
            int index30 = (int)num30;
            byte num31 = (byte)(index30 + 1);
            nPkt30[index30] = (byte)172;
            byte[] nPkt31 = this.nPkt;
            int index31 = (int)num31;
            byte num32 = (byte)(index31 + 1);
            nPkt31[index31] = (byte)10;
            byte[] nPkt32 = this.nPkt;
            int index32 = (int)num32;
            byte num33 = (byte)(index32 + 1);
            nPkt32[index32] = (byte)128;
            byte[] nPkt33 = this.nPkt;
            int index33 = (int)num33;
            byte num34 = (byte)(index33 + 1);
            nPkt33[index33] = (byte)8;
            this.Ps = new ASCIIEncoding().GetBytes(DLMSInfo.MeterAuthPassword);
            for (int index34 = 0; index34 < DLMSInfo.MeterAuthPassword.Length; ++index34)
                this.nPkt[(int)num34++] = this.Ps[index34];
            this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 12)] = Convert.ToByte(46 + DLMSInfo.MeterAuthPassword.Length);
            byte[] nPkt34 = this.nPkt;
            int index35 = (int)num34;
            byte num35 = (byte)(index35 + 1);
            nPkt34[index35] = (byte)190;
            byte[] nPkt35 = this.nPkt;
            int index36 = (int)num35;
            byte num36 = (byte)(index36 + 1);
            nPkt35[index36] = (byte)16;
            byte[] nPkt36 = this.nPkt;
            int index37 = (int)num36;
            byte num37 = (byte)(index37 + 1);
            nPkt36[index37] = (byte)4;
            byte[] nPkt37 = this.nPkt;
            int index38 = (int)num37;
            byte num38 = (byte)(index38 + 1);
            nPkt37[index38] = (byte)14;
            byte[] nPkt38 = this.nPkt;
            int index39 = (int)num38;
            byte num39 = (byte)(index39 + 1);
            nPkt38[index39] = (byte)1;
            byte[] nPkt39 = this.nPkt;
            int index40 = (int)num39;
            byte num40 = (byte)(index40 + 1);
            nPkt39[index40] = (byte)0;
            byte[] nPkt40 = this.nPkt;
            int index41 = (int)num40;
            byte num41 = (byte)(index41 + 1);
            nPkt40[index41] = (byte)0;
            byte[] nPkt41 = this.nPkt;
            int index42 = (int)num41;
            byte num42 = (byte)(index42 + 1);
            nPkt41[index42] = (byte)0;
            byte[] nPkt42 = this.nPkt;
            int index43 = (int)num42;
            byte num43 = (byte)(index43 + 1);
            nPkt42[index43] = (byte)6;
            byte[] nPkt43 = this.nPkt;
            int index44 = (int)num43;
            byte num44 = (byte)(index44 + 1);
            nPkt43[index44] = (byte)95;
            byte[] nPkt44 = this.nPkt;
            int index45 = (int)num44;
            byte num45 = (byte)(index45 + 1);
            nPkt44[index45] = (byte)31;
            byte[] nPkt45 = this.nPkt;
            int index46 = (int)num45;
            byte num46 = (byte)(index46 + 1);
            nPkt45[index46] = (byte)4;
            byte[] nPkt46 = this.nPkt;
            int index47 = (int)num46;
            byte num47 = (byte)(index47 + 1);
            nPkt46[index47] = (byte)0;
            byte[] nPkt47 = this.nPkt;
            int index48 = (int)num47;
            byte num48 = (byte)(index48 + 1);
            nPkt47[index48] = (byte)0;
            byte[] nPkt48 = this.nPkt;
            int index49 = (int)num48;
            byte num49 = (byte)(index49 + 1);
            nPkt48[index49] = (byte)24;
            byte[] nPkt49 = this.nPkt;
            int index50 = (int)num49;
            byte num50 = (byte)(index50 + 1);
            nPkt49[index50] = (byte)29;
            byte[] nPkt50 = this.nPkt;
            int index51 = (int)num50;
            byte num51 = (byte)(index51 + 1);
            nPkt50[index51] = byte.MaxValue;
            byte[] nPkt51 = this.nPkt;
            int index52 = (int)num51;
            byte num52 = (byte)(index52 + 1);
            nPkt51[index52] = byte.MaxValue;
            this.nPkt[2] = Convert.ToByte((int)num52 + 1);
            this.DCl.fcs(ref this.nPkt, Convert.ToByte((int)this.bytAddMode + 5), (byte)1);
            this.DCl.fcs(ref this.nPkt, Convert.ToByte((int)num52 - 1), (byte)1);
            this.nPkt[(int)num52 + 2] = (byte)126;
            byte num53 = 0;
            bool flag;
            do
            {
                this.ClearBuffer();
                flag = false;
                commandString = "";
                for (int i = 0; i < ((int)num52 + 3); i++)
                {
                    commandString += nPkt[i].ToString("X2");
                }
                this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)num52 + 3));
                if (IsLineTrafficEnabled)
                {
                    LineTrafficControlEventHandler("     AARQ", "Send");
                    LineTrafficControlEventHandler($"     Proposed CONFORMANCE BLOCK SERVICES [{"00181D"}]: {DLMSParser.ConformanceServicesSupported("00181D")}", "Send");

                    SendDataPrint(nPkt, Convert.ToByte((int)num52 + 3));
                }
                DateTime now = DateTime.Now;
                do
                {
                    this.Wait((double)nWait);
                    Application.DoEvents();
                    this.DataReceive();
                    if (this.nCounter > 2 && (int)this.nRcvPkt[2] + 2 <= this.nCounter)
                    {
                        flag = true;
                        this.FrameTypeFG();
                        break;
                    }
                    if (DateTime.Now.Subtract(now).Seconds > (int)nTimeOut && (int)num53 < (int)nTryCount)
                    {
                        ++num53;
                        break;
                    }
                }
                while (!flag);
            }
            while (!flag && (int)num53 != (int)nTryCount);
            this.temp = string.Empty;
            for (int index53 = 0; index53 < this.nCounter; ++index53)
                this.temp += this.nRcvPkt[index53].ToString("X2") + " ";
            if (string.IsNullOrEmpty(temp))
                temp = "(R)" + "  " + "NULL" + " " + DateTime.Now.ToString(Constants.timeStampFormate) + Environment.NewLine;
            else
                temp = "(R)" + "  " + temp + " " + DateTime.Now.ToString(Constants.timeStampFormate) + Environment.NewLine;
            if (IsLineTrafficEnabled)
                LineTrafficControlEventHandler(temp, "Receive");
            int num70 = 13;
            int num71 = 1;
            var stopwatch = Stopwatch.StartNew();
            while (true)
            {
                if (this.nRcvPkt[(int)this.bytAddMode + num70] == (byte)161)
                    num70 += 2 + (int)this.nRcvPkt[(int)this.bytAddMode + num70 + 1];
                else if (this.nRcvPkt[(int)this.bytAddMode + num70] == (byte)162)
                    num70 += 2 + (int)this.nRcvPkt[(int)this.bytAddMode + num70 + 1];
                else if (this.nRcvPkt[(int)this.bytAddMode + num70] == (byte)163)
                {
                    num71 = (int)this.nRcvPkt[(int)this.bytAddMode + num70 + (int)this.nRcvPkt[(int)this.bytAddMode + num70 + 1] + 1];
                    num70 += 2 + (int)this.nRcvPkt[(int)this.bytAddMode + num70 + 1];
                }
                else if (this.nRcvPkt[(int)this.bytAddMode + num70] == (byte)164)
                {
                    this.sSYSTaginHEX = string.Empty;
                    for (int index66 = 0; index66 < (int)Convert.ToInt16(this.nRcvPkt[(int)this.bytAddMode + num70 + 3].ToString(), 16); ++index66)
                        this.sSYSTaginHEX += this.nRcvPkt[(int)this.bytAddMode + num70 + 4 + index66].ToString("X2");
                    num70 += 2 + (int)this.nRcvPkt[(int)this.bytAddMode + num70 + 1];
                }
                else if (this.nRcvPkt[(int)this.bytAddMode + num70] == (byte)136)
                    num70 += 2 + (int)this.nRcvPkt[(int)this.bytAddMode + num70 + 1];
                else if (this.nRcvPkt[(int)this.bytAddMode + num70] == (byte)137)
                    num70 += 2 + (int)this.nRcvPkt[(int)this.bytAddMode + num70 + 1];
                else if (this.nRcvPkt[(int)this.bytAddMode + num70] == (byte)170)
                    num70 += 2 + (int)this.nRcvPkt[(int)this.bytAddMode + num70 + 1];
                else if (this.nRcvPkt[(int)this.bytAddMode + num70] == (byte)190)
                    break;
                if (stopwatch.ElapsedMilliseconds > (10 * 1000))
                {
                    stopwatch.Stop();
                    break;
                }
            }
            if (this.nRcvPkt[(int)this.bytAddMode + num70] == (byte)190)
            {
                int num72 = 0;
                int num73 = num70 + 2;
                if (this.nRcvPkt[(int)this.bytAddMode + num73] == (byte)4)
                {
                    this.nRcvPkt[(int)this.bytAddMode + num73 + 1] -= Convert.ToByte(num72);
                    num73 += 2;
                }
                if (this.nRcvPkt[(int)this.bytAddMode + num73] == (byte)40)
                {
                    this.nRcvPkt[(int)this.bytAddMode + num73 + 1] -= Convert.ToByte(num72);
                    num73 += 2;
                }
                if (this.nRcvPkt[(int)this.bytAddMode + num73] == (byte)8)
                    num73 += 3;
                if (this.nRcvPkt[(int)this.bytAddMode + num73] == (byte)95 && this.nRcvPkt[(int)this.bytAddMode + num73 + 1] == (byte)31)
                {
                    int num74 = num73 + 3;
                    this.intConfBlk = long.Parse(this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + num74)].ToString("X2") + this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + num74 + 1)].ToString("X2") + this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + num74 + 2)].ToString("X2") + this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + num74 + 3)].ToString("X2"), NumberStyles.HexNumber);
                    this.conformanceBlockString = $"{this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + num74 + 1)].ToString("X2") + this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + num74 + 2)].ToString("X2") + this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + num74 + 3)].ToString("X2")}";//BY AAC
                    if (IsLineTrafficEnabled)
                    {
                        LineTrafficControlEventHandler($"     Negotiated CONFORMANCE BLOCK SERVICES [{conformanceBlockString}]: {DLMSParser.ConformanceServicesSupported(conformanceBlockString.Trim())}", "Receive");
                        LineTrafficControlEventHandler("\r\n", "Send");
                    }
                }
            }
            responseString = "";
            for (int recIndex = 0; recIndex < nCounter; recIndex++)
            {
                responseString += nRcvPkt[recIndex].ToString("X2");
            }
            return flag && this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 28)] == (byte)0 && this.nCounter > 27 ? 0 : 1;
        }

        //Trigger type get parameter
        public bool GetParameter(
            string sWhichData,
            byte nWait,
            byte nTryCount,
            byte nTimeOut,
            byte nType,
            DateTime dateStartDate,
            DateTime dateEndDate,
            string sOBISSelect,
            ulong nFrom,
            ulong nTo,
            bool IsLineTrafficEnabled = true,
            string sOBISlist = "0100")
        {
            if (IsLineTrafficEnabled)
                LineTrafficControlEventHandler($"     GET CLASS-{Convert.ToInt32(sWhichData.Substring(0, 4), 16)} | OBIS-{DLMSParser.GetObis(sWhichData.Substring(4, 12))} [{DLMSParser.GetObisName(Convert.ToInt32(sWhichData.Substring(0, 4), 16).ToString(), DLMSParser.GetObis(sWhichData.Substring(4, 12)), Convert.ToInt32(sWhichData.Substring(sWhichData.Length - 2), 16).ToString())}] | Attribute-{Convert.ToInt32(sWhichData.Substring(sWhichData.Length - 2), 16)}", "Send");
            bool flag1 = false;
            long num1 = 0;
            byte num2 = Convert.ToByte((int)this.bytAddMode + 8);
            byte num3 = 0;
            string empty = string.Empty;
            StringBuilder stringBuilder = new StringBuilder();
            this.strbldDLMdata.Remove(0, this.strbldDLMdata.Length);
            //DLM file OBIS code and Attribute ID
            strbldDLMdata.Append($"\r\n{sWhichData.Substring(0, 4)} {sWhichData.Substring(4, 12)} {sWhichData.Substring(sWhichData.Length - 2)} ");
            //++this.PB1.Value;
            //if (this.PB1.Value == 100)
            //  this.PB1.Value = 0;
            this.nRetLSH = Convert.ToByte((int)this.nRecvCntr << 5);
            this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nRetLSH | 16);
            this.nRetLSH = Convert.ToByte((int)this.nSentCntr << 1);
            this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nRetLSH | (int)this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)]);
            byte[] nPkt1 = this.nPkt;
            int index1 = (int)num2;
            byte num4 = (byte)(index1 + 1);
            nPkt1[index1] = (byte)230;//E6
            byte[] nPkt2 = this.nPkt;
            int index2 = (int)num4;
            byte num5 = (byte)(index2 + 1);
            nPkt2[index2] = (byte)230;//E6
            byte[] nPkt3 = this.nPkt;
            int index3 = (int)num5;
            byte num6 = (byte)(index3 + 1);
            nPkt3[index3] = (byte)0;//00
            string hex1;
            int num7;
            switch (nType)
            {
                case 0:
                    hex1 = "C001C1" + sWhichData + "00";
                    break;
                case 1:
                    if (sOBISSelect == "12000809060000010000FF0F02120000")
                    {
                        string[] strArray = new string[20];
                        strArray[0] = "C00181";
                        strArray[1] = sWhichData;
                        strArray[2] = "010102040204";
                        strArray[3] = sOBISSelect;
                        strArray[4] = "090C";
                        strArray[5] = dateStartDate.Year.ToString("X4");
                        strArray[6] = dateStartDate.Month.ToString("X2");
                        num7 = dateStartDate.Day;
                        strArray[7] = num7.ToString("X2");
                        strArray[8] = "FF";
                        num7 = dateStartDate.Hour;
                        strArray[9] = num7.ToString("X2");
                        num7 = dateStartDate.Minute;
                        strArray[10] = num7.ToString("X2");
                        strArray[11] = "0000800000090C";
                        num7 = dateEndDate.Year;
                        strArray[12] = num7.ToString("X4");
                        num7 = dateEndDate.Month;
                        strArray[13] = num7.ToString("X2");
                        num7 = dateEndDate.Day;
                        strArray[14] = num7.ToString("X2");
                        strArray[15] = "FF";
                        num7 = dateEndDate.Hour;
                        strArray[16] = num7.ToString("X2");
                        num7 = dateEndDate.Minute;
                        strArray[17] = num7.ToString("X2");
                        strArray[18] = "00008000000100";
                        strArray[19] = sOBISlist;
                        hex1 = string.Concat(strArray);
                        break;
                    }
                    hex1 = "C00181" + sWhichData + "010102040204" + sOBISSelect + "06" + nFrom.ToString("X8") + "06" + nTo.ToString("X8") + sOBISlist;
                    break;
                case 2:
                    //hex1 = "C00181" + sWhichData + "0102020406" + nFrom.ToString("X8") + "06" + nTo.ToString("X8") + "12" + nFrom.ToString("X4") + "12" + this.Tovalue.ToString("X4");
                    hex1 = "C00181" + sWhichData + "0102020406" + nFrom.ToString("X8") + "06" + nTo.ToString("X8") + "120001" + "120000";
                    break;
                default:
                    hex1 = "C00181" + sWhichData + "0102020406" + this.FromEntry.ToString("X8") + "06" + this.ToEntry.ToString("X8") + "12" + this.Fromvalue.ToString("X4") + "12" + this.Tovalue.ToString("X4");
                    break;
            }
            //this.swLT.Write("\t" + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss:ff tt") + "\t" + "Command : -- >> " + hex1 + "\r\n");
            commandString = "Command  : -- >> " + hex1 + "\r\n";
            if (DLMSInfo.IsLNWithCipher || DLMSInfo.IsLNWithCipherDedicatedKey)
            {
                if (DLMSInfo.AccessMode == 1 || DLMSInfo.AccessMode == 3)
                    num3 = (byte)32;
                else if (DLMSInfo.AccessMode == 2 || DLMSInfo.AccessMode == 4)
                    num3 = (byte)48;
                byte num8;
                if (DLMSInfo.IsLNWithCipherDedicatedKey)
                {
                    byte[] nPkt4 = this.nPkt;
                    int index4 = (int)num6;
                    num8 = (byte)(index4 + 1);
                    nPkt4[index4] = (byte)208;
                }
                else
                {
                    byte[] nPkt5 = this.nPkt;
                    int index5 = (int)num6;
                    num8 = (byte)(index5 + 1);
                    nPkt5[index5] = (byte)200;
                }
                byte[] nPkt6 = this.nPkt;
                int index6 = (int)num8;
                byte num9 = (byte)(index6 + 1);
                nPkt6[index6] = (byte)0;
                byte[] nPkt7 = this.nPkt;
                int index7 = (int)num9;
                byte num10 = (byte)(index7 + 1);
                int num11 = (int)num3;
                nPkt7[index7] = (byte)num11;
                byte[] nPkt8 = this.nPkt;
                int index8 = (int)num10;
                byte num12 = (byte)(index8 + 1);
                nPkt8[index8] = (byte)0;
                byte[] nPkt9 = this.nPkt;
                int index9 = (int)num12;
                byte num13 = (byte)(index9 + 1);
                nPkt9[index9] = (byte)0;
                byte[] nPkt10 = this.nPkt;
                int index10 = (int)num13;
                byte num14 = (byte)(index10 + 1);
                int num15 = (int)Convert.ToByte(this.nCommandCounter / 256);
                nPkt10[index10] = (byte)num15;
                byte[] nPkt11 = this.nPkt;
                int index11 = (int)num14;
                num6 = (byte)(index11 + 1);
                int num16 = (int)Convert.ToByte(this.nCommandCounter % 256);
                nPkt11[index11] = (byte)num16;
                string str1 = num3.ToString("X2");
                num7 = this.nCommandCounter++;
                string str2 = num7.ToString("X8");
                string str3 = hex1;
                byte[] numArray = this.Encrypt(str1 + str2 + str3, this.sEncryptkeyinHEX);
                this.nPkt[(int)num6 - 6] = Convert.ToByte(numArray.Length + 5);
                for (int index12 = 0; index12 < numArray.Length; ++index12)
                    this.nPkt[(int)num6++] = numArray[index12];
            }
            else
            {
                foreach (byte num17 in StringToByteArray(hex1))
                    this.nPkt[(int)num6++] = num17;
                commandString = "Command  : -- >> " + hex1 + "\r\n";
            }
            this.nPkt[1] = (byte)160;
            this.nPkt[2] = Convert.ToByte((int)num6 + 1);
            this.DCl.fcs(ref this.nPkt, (int)Convert.ToByte((int)this.bytAddMode + 5), (byte)1);
            this.DCl.fcs(ref this.nPkt, (int)Convert.ToByte((int)num6 - 1), (byte)1);
            this.nPkt[(int)num6 + 2] = (byte)126;
            byte num18 = 0;
            bool flag2;
            DateTime now1;
            //LineTrafficControlEventHandler($"     GET CLASS-{Convert.ToInt32(sWhichData.Substring(0, 4), 16)} | OBIS-{DLMSParser.GetObis(sWhichData.Substring(4, 12))} [{DLMSParser.GetObisName(Convert.ToInt32(sWhichData.Substring(0, 4), 16).ToString(), DLMSParser.GetObis(sWhichData.Substring(4, 12)))}] | Attribute-{Convert.ToInt32(sWhichData.Substring(sWhichData.Length - 2), 16)}", "Send");
            do
            {
                this.Wait((double)nWait);
                this.ClearBuffer();
                flag2 = false;
                this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)num6 + 3));
                if (IsLineTrafficEnabled)
                    SendDataPrint(nPkt, Convert.ToByte((int)num6 + 3));
                //LineTrafficControlEventHandler($"     {commandString}", "Command");
                DateTime now2 = DateTime.Now;
                while (true)
                {
                    Application.DoEvents();
                    this.DataReceive();
                    num7 = (int)this.nRcvPkt[1] & 7;
                    this.pktLength = int.Parse(num7.ToString("X2") + this.nRcvPkt[2].ToString("X2"), NumberStyles.HexNumber);
                    if (this.nCounter <= 2 || this.pktLength + 2 > this.nCounter || this.nRcvPkt[this.pktLength + 1] != (byte)126)
                    {
                        now1 = DateTime.Now;
                        if (now1.Subtract(now2).Seconds <= (int)nTimeOut || (int)num18 >= (int)nTryCount)
                        {
                            if ((int)num18 == (int)nTryCount)
                                goto label_38;
                        }
                        else
                            goto label_26;
                    }
                    else
                        break;
                }
                RecvDataPrint(nRcvPkt, nCounter, IsLineTrafficEnabled);
                if (IsLineTrafficEnabled)
                {
                    LineTrafficControlEventHandler($"     {commandString}", "Command");
                    ResponsePrint();
                }
                //LineTrafficControlEventHandler($"     {responseString}\r\n", "Response");
                //LineTrafficControlEventHandler("\r\n", "Send");
                flag2 = true;
                num18 = (byte)0;
                this.FrameType();
                goto label_38;
            label_26:
                if (DLMSInfo.CmbDirect == 1 && this.nCounter > 0)
                {
                    if (this.nRcvPkt[0] != (byte)126)
                        this.ClearBuffer();
                    flag2 = false;
                    this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)this.bytAddMode + 27));
                    if (IsLineTrafficEnabled)
                        SendDataPrint(nPkt, Convert.ToByte((int)this.bytAddMode + 27));
                    //LineTrafficControlEventHandler($"     {commandString}", "Command");
                    DateTime now3 = DateTime.Now;
                    while (true)
                    {
                        Application.DoEvents();
                        this.DataReceive();
                        num7 = (int)this.nRcvPkt[1] & 7;
                        this.pktLength = int.Parse(num7.ToString("X2") + this.nRcvPkt[2].ToString("X2"), NumberStyles.HexNumber);
                        if (this.pktLength + 2 > this.nCounter || this.nRcvPkt[this.pktLength + 1] != (byte)126)
                        {
                            now1 = DateTime.Now;
                            if (now1.Subtract(now3).Seconds > (int)nTimeOut)
                                goto label_33;
                        }
                        else
                            break;
                    }
                    RecvDataPrint(nRcvPkt, nCounter, IsLineTrafficEnabled);
                    if (IsLineTrafficEnabled)
                    {
                        LineTrafficControlEventHandler($"     {commandString}", "Command");
                        ResponsePrint();
                    }
                    flag2 = true;
                    oSC.DiscardInputOutputBuffer();
                    num18 = (byte)0;
                    this.FrameType();
                    goto label_38;
                label_33:
                    ++num18;
                }
                else
                    ++num18;
                label_38:
                if (flag2)
                {
                    this.temp = string.Empty;
                    num7 = (int)this.nRcvPkt[1] & 7;
                    this.pktLength = int.Parse(num7.ToString("X2") + this.nRcvPkt[2].ToString("X2"), NumberStyles.HexNumber);
                    for (int index13 = 0; index13 < this.pktLength + 2; ++index13)
                        this.temp += this.nRcvPkt[index13].ToString("X2");
                    this.temp += "\r\n";
                    //this.swLT.Write("(R)\t" + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss:ff tt") + "\t" + this.temp);
                    responseString = temp.Trim();
                    responseString = $"Response : -- >> {responseString.Substring(22, responseString.Length - 28)}\r\n";
                }
            }
            while (!flag2 && (int)num18 != (int)nTryCount);
            if (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] == (byte)151 || ((int)this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] & 1) == 1)
                return false;
            for (int index14 = (int)this.bytAddMode + 11; index14 < this.nCounter - 3; ++index14)
                stringBuilder.Append(this.nRcvPkt[index14].ToString("X2"));
            while (((int)this.nRcvPkt[1] & 168) == 168)
            {
                this.nPkt[2] = Convert.ToByte((int)this.bytAddMode + 7);
                this.nRetLSH = Convert.ToByte((int)this.nRecvCntr << 5);
                this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nRetLSH | 16);
                this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] | 1);
                this.DCl.fcs(ref this.nPkt, (int)Convert.ToByte((int)this.bytAddMode + 5), (byte)1);
                this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 8)] = (byte)126;
                byte num19 = 0;
                bool flag3;
                do
                {
                    this.Wait((double)nWait);
                    this.ClearBuffer();
                    flag3 = false;
                    this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)this.bytAddMode + 9));
                    if (IsLineTrafficEnabled)
                        SendDataPrint(nPkt, Convert.ToByte((int)this.bytAddMode + 9));
                    //LineTrafficControlEventHandler($"     {commandString}", "Command");
                    DateTime now4 = DateTime.Now;
                    while (true)
                    {
                        Application.DoEvents();
                        this.DataReceive();
                        num7 = (int)this.nRcvPkt[1] & 7;
                        this.pktLength = int.Parse(num7.ToString("X2") + this.nRcvPkt[2].ToString("X2"), NumberStyles.HexNumber);
                        if (this.nCounter <= 2 || this.pktLength + 2 > this.nCounter || this.nRcvPkt[this.pktLength + 1] != (byte)126)
                        {
                            now1 = DateTime.Now;
                            if (now1.Subtract(now4).Seconds <= (int)nTimeOut || (int)num19 >= (int)nTryCount)
                            {
                                if ((int)num19 == (int)nTryCount)
                                    goto label_72;
                            }
                            else
                                goto label_57;
                        }
                        else
                            break;
                    }
                    RecvDataPrint(nRcvPkt, nCounter, IsLineTrafficEnabled);
                    LineTrafficControlEventHandler($"     {commandString}", "Command");
                    ResponsePrint();
                    flag3 = true;
                    num19 = (byte)0;
                    for (int index15 = (int)Convert.ToByte((int)this.bytAddMode + 8); index15 < this.pktLength - 1; ++index15)
                        stringBuilder.Append(this.nRcvPkt[index15].ToString("X2"));
                    this.FrameType();
                    goto label_72;
                label_57:
                    if (DLMSInfo.CmbDirect == 1 && this.nCounter > 0)
                    {
                        if (this.nRcvPkt[0] != (byte)126)
                            this.ClearBuffer();
                        flag3 = false;
                        this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)this.bytAddMode + 9));
                        if (IsLineTrafficEnabled)
                            SendDataPrint(nPkt, Convert.ToByte((int)this.bytAddMode + 9));
                        //LineTrafficControlEventHandler($"     {commandString}", "Command");
                        DateTime now5 = DateTime.Now;
                        while (true)
                        {
                            Application.DoEvents();
                            this.DataReceive();
                            num7 = (int)this.nRcvPkt[1] & 7;
                            this.pktLength = int.Parse(num7.ToString("X2") + this.nRcvPkt[2].ToString("X2"), NumberStyles.HexNumber);
                            if (this.pktLength + 2 > this.nCounter || this.nRcvPkt[this.pktLength + 1] != (byte)126)
                            {
                                now1 = DateTime.Now;
                                if (now1.Subtract(now5).Seconds > (int)nTimeOut)
                                    goto label_67;
                            }
                            else
                                break;
                        }
                        RecvDataPrint(nRcvPkt, nCounter, IsLineTrafficEnabled);
                        LineTrafficControlEventHandler($"     {commandString}", "Command");
                        ResponsePrint();
                        flag3 = true;
                        oSC.DiscardInputOutputBuffer();
                        num19 = (byte)0;
                        for (int index16 = (int)Convert.ToByte((int)this.bytAddMode + 8); index16 < this.pktLength - 1; ++index16)
                            stringBuilder.Append(this.nRcvPkt[index16].ToString("X2"));
                        this.FrameType();
                        goto label_72;
                    label_67:
                        ++num19;
                    }
                    else
                        ++num19;
                    label_72:
                    //if (flag3)
                    //{
                    //    this.temp = string.Empty;
                    //    for (int index17 = 0; index17 < this.pktLength + 2; ++index17)
                    //        this.temp += this.nRcvPkt[index17].ToString("X2");
                    //    this.temp += "\r\n";
                    //    //this.swLT.Write("(R)\t" + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss:ff tt") + "\t" + this.temp);
                    //}
                    if (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] == (byte)151)
                        goto label_78;
                }
                while (!flag3 && (int)num19 != (int)nTryCount);
                goto label_81;
            label_78:
                return false;
            label_81:
                if (!flag3 || this.nRcvPkt[1] != (byte)160)
                {
                    if (!flag3)
                        return false;
                }
                else
                    break;
            }
            if (stringBuilder.ToString().StartsWith("CC") || stringBuilder.ToString().StartsWith("D4"))
            {
                if (stringBuilder.ToString().StartsWith("CC82") || stringBuilder.ToString().StartsWith("D482"))
                    stringBuilder.Remove(0, 8);
                else if (stringBuilder.ToString().StartsWith("CC81") || stringBuilder.ToString().StartsWith("D481"))
                    stringBuilder.Remove(0, 6);
                else
                    stringBuilder.Remove(0, 4);
                byte[] numArray = this.Decrypt(stringBuilder.ToString(), this.sEncryptkeyinHEX);
                stringBuilder.Length = 0;
                for (int index18 = 0; index18 < numArray.Length; ++index18)
                    stringBuilder.Append(numArray[index18].ToString("X2"));
                //this.swLT.Write("\t" + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss:ff tt") + "\t" + "Response : -- >> " + stringBuilder.ToString() + "\r\n");
                responseString = "Response : -- >> " + stringBuilder.ToString() + "\r\n";
            }
            if (stringBuilder.ToString().StartsWith("C401") && !(stringBuilder.ToString().Length < 8))
                stringBuilder.Remove(0, 8);
            if (stringBuilder.ToString().StartsWith("C402"))
            {
                num1 = Convert.ToInt64(stringBuilder.ToString().Substring(8, 8), 16);
                flag1 = !Convert.ToBoolean(Convert.ToInt16(stringBuilder.ToString().Substring(6, 2)));
                if (stringBuilder.ToString().Substring(18, 2) == "82")
                    stringBuilder.Remove(0, 24);
                else if (stringBuilder.ToString().Substring(18, 2) == "81")
                    stringBuilder.Remove(0, 22);
                else
                    stringBuilder.Remove(0, 20);
            }
            this.strbldDLMdata.Append(stringBuilder.ToString());
            stringBuilder.Length = 0;
            while (flag1)
            {
                if (token.IsCancellationRequested)
                    return false;
                this.temp = string.Empty;
                flag1 = false;
                this.nRetLSH = Convert.ToByte((int)this.nRecvCntr << 5);
                this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nRetLSH | 16);
                this.nRetLSH = Convert.ToByte((int)this.nSentCntr << 1);
                this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nRetLSH | (int)this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)]);
                byte num20 = Convert.ToByte((int)this.bytAddMode + 8);
                byte[] nPkt12 = this.nPkt;
                int index19 = (int)num20;
                byte num21 = (byte)(index19 + 1);
                nPkt12[index19] = (byte)230;
                byte[] nPkt13 = this.nPkt;
                int index20 = (int)num21;
                byte num22 = (byte)(index20 + 1);
                nPkt13[index20] = (byte)230;
                byte[] nPkt14 = this.nPkt;
                int index21 = (int)num22;
                byte num23 = (byte)(index21 + 1);
                nPkt14[index21] = (byte)0;
                string hex2 = "C00281" + num1.ToString("X8");
                commandString = "Command : -- >> " + hex2 + "\r\n";
                if (DLMSInfo.IsLNWithCipher)
                {
                    //this.swLT.Write("\t" + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss:ff tt") + "\t" + "Command : -- >> " + hex2 + "\r\n");
                    commandString = "Command : -- >> " + hex2 + "\r\n";
                    byte num24;
                    if (DLMSInfo.IsLNWithCipherDedicatedKey)
                    {
                        byte[] nPkt15 = this.nPkt;
                        int index22 = (int)num23;
                        num24 = (byte)(index22 + 1);
                        nPkt15[index22] = (byte)208;
                    }
                    else
                    {
                        byte[] nPkt16 = this.nPkt;
                        int index23 = (int)num23;
                        num24 = (byte)(index23 + 1);
                        nPkt16[index23] = (byte)200;
                    }
                    byte[] nPkt17 = this.nPkt;
                    int index24 = (int)num24;
                    byte num25 = (byte)(index24 + 1);
                    nPkt17[index24] = (byte)0;
                    byte[] nPkt18 = this.nPkt;
                    int index25 = (int)num25;
                    byte num26 = (byte)(index25 + 1);
                    int num27 = (int)num3;
                    nPkt18[index25] = (byte)num27;
                    byte[] nPkt19 = this.nPkt;
                    int index26 = (int)num26;
                    byte num28 = (byte)(index26 + 1);
                    nPkt19[index26] = (byte)0;
                    byte[] nPkt20 = this.nPkt;
                    int index27 = (int)num28;
                    byte num29 = (byte)(index27 + 1);
                    nPkt20[index27] = (byte)0;
                    byte[] nPkt21 = this.nPkt;
                    int index28 = (int)num29;
                    byte num30 = (byte)(index28 + 1);
                    int num31 = (int)Convert.ToByte(this.nCommandCounter / 256);
                    nPkt21[index28] = (byte)num31;
                    byte[] nPkt22 = this.nPkt;
                    int index29 = (int)num30;
                    num23 = (byte)(index29 + 1);
                    int num32 = (int)Convert.ToByte(this.nCommandCounter % 256);
                    nPkt22[index29] = (byte)num32;
                    string str4 = num3.ToString("X2");
                    num7 = this.nCommandCounter++;
                    string str5 = num7.ToString("X8");
                    string str6 = hex2;
                    byte[] numArray = this.Encrypt(str4 + str5 + str6, this.sEncryptkeyinHEX);
                    this.nPkt[(int)num23 - 6] = Convert.ToByte(numArray.Length + 5);
                    for (int index30 = 0; index30 < numArray.Length; ++index30)
                        this.nPkt[(int)num23++] = numArray[index30];
                }
                else
                {
                    foreach (byte num33 in StringToByteArray(hex2))
                        this.nPkt[(int)num23++] = num33;
                    //commandString = "Command : -- >> " + hex2 + "\r\n";
                }
                this.nPkt[2] = Convert.ToByte((int)num23 + 1);
                this.DCl.fcs(ref this.nPkt, (int)Convert.ToByte((int)this.bytAddMode + 5), (byte)1);
                this.DCl.fcs(ref this.nPkt, (int)Convert.ToByte((int)num23 - 1), (byte)1);
                this.nPkt[(int)num23 + 2] = (byte)126;
                byte num34 = 0;
                bool flag4;
                do
                {
                    this.Wait((double)nWait);
                    this.ClearBuffer();
                    flag4 = false;
                    this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)num23 + 3));
                    if (IsLineTrafficEnabled)
                        SendDataPrint(nPkt, Convert.ToByte((int)num23 + 3));
                    //LineTrafficControlEventHandler($"     {commandString}", "Command");
                    //SendDataPrint(nPkt, Convert.ToByte((int)num23 + 3));
                    DateTime now6 = DateTime.Now;
                    while (true)
                    {
                        Application.DoEvents();
                        this.DataReceive();
                        num7 = (int)this.nRcvPkt[1] & 7;
                        this.pktLength = int.Parse(num7.ToString("X2") + this.nRcvPkt[2].ToString("X2"), NumberStyles.HexNumber);
                        if (this.nCounter <= 2 || this.pktLength + 2 > this.nCounter || this.nRcvPkt[this.pktLength + 1] != (byte)126)
                        {
                            now1 = DateTime.Now;
                            if (now1.Subtract(now6).Seconds <= (int)nTimeOut || (int)num34 >= (int)nTryCount)
                            {
                                if ((int)num34 == (int)nTryCount)
                                    goto label_131;
                            }
                            else
                                goto label_119;
                        }
                        else
                            break;
                    }
                    RecvDataPrint(nRcvPkt, nCounter, IsLineTrafficEnabled);
                    if (IsLineTrafficEnabled)
                    {
                        LineTrafficControlEventHandler($"     {commandString}", "Command");
                        ResponsePrint();
                    }
                    flag4 = true;
                    num34 = (byte)0;
                    this.FrameType();
                    goto label_131;
                label_119:
                    if (DLMSInfo.CmbDirect == 1 && this.nCounter > 0)
                    {
                        if (this.nRcvPkt[0] != (byte)126)
                            this.ClearBuffer();
                        flag4 = false;
                        this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)this.bytAddMode + 21));
                        DateTime now7 = DateTime.Now;
                        if (IsLineTrafficEnabled)
                            SendDataPrint(nPkt, Convert.ToByte((int)this.bytAddMode + 21));
                        //LineTrafficControlEventHandler($"     {commandString}", "Command");
                        while (true)
                        {
                            Application.DoEvents();
                            this.DataReceive();
                            num7 = (int)this.nRcvPkt[1] & 7;
                            this.pktLength = int.Parse(num7.ToString("X2") + this.nRcvPkt[2].ToString("X2"), NumberStyles.HexNumber);
                            if (this.pktLength + 2 > this.nCounter || this.nRcvPkt[this.pktLength + 1] != (byte)126)
                            {
                                now1 = DateTime.Now;
                                if (now1.Subtract(now7).Seconds > (int)nTimeOut)
                                    goto label_126;
                            }
                            else
                                break;
                        }
                        RecvDataPrint(nRcvPkt, nCounter, IsLineTrafficEnabled);
                        LineTrafficControlEventHandler($"     {commandString}", "Command");
                        ResponsePrint();
                        flag4 = true;
                        oSC.DiscardInputOutputBuffer();
                        num34 = (byte)0;
                        this.FrameType();
                        goto label_131;
                    label_126:
                        ++num34;
                    }
                    else
                        ++num34;
                    label_131:
                    if (flag4)
                    {
                        this.temp = string.Empty;
                        for (int index31 = 0; index31 < this.pktLength + 2; ++index31)
                            this.temp += this.nRcvPkt[index31].ToString("X2");
                        this.temp += "\r\n";
                        // this.swLT.Write("(R)\t" + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss:ff tt") + "\t" + this.temp);
                    }
                    if (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] == (byte)151)
                        goto label_137;
                }
                while (!flag4 && (int)num34 != (int)nTryCount);
                goto label_140;
            label_137:
                return false;
            label_140:
                if (!flag4)
                    return false;
                for (int index32 = (int)this.bytAddMode + 11; index32 < this.nCounter - 3; ++index32)
                    stringBuilder.Append(this.nRcvPkt[index32].ToString("X2"));
                while (((int)this.nRcvPkt[1] & 168) == 168)
                {
                    this.nPkt[2] = Convert.ToByte((int)this.bytAddMode + 7);
                    this.nRetLSH = Convert.ToByte((int)this.nRecvCntr << 5);
                    this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nRetLSH | 16);
                    this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] | 1);
                    this.DCl.fcs(ref this.nPkt, (int)Convert.ToByte((int)this.bytAddMode + 5), (byte)1);
                    this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 8)] = (byte)126;
                    this.temp = string.Empty;
                    byte num35 = 0;
                    bool flag5;
                    do
                    {
                        flag5 = false;
                        this.Wait((double)nWait);
                        this.ClearBuffer();
                        this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)this.bytAddMode + 9));
                        DateTime now8 = DateTime.Now;
                        if (IsLineTrafficEnabled)
                            SendDataPrint(nPkt, Convert.ToByte((int)this.bytAddMode + 9));
                        while (true)
                        {
                            Application.DoEvents();
                            this.DataReceive();
                            num7 = (int)this.nRcvPkt[1] & 7;
                            this.pktLength = int.Parse(num7.ToString("X2") + this.nRcvPkt[2].ToString("X2"), NumberStyles.HexNumber);
                            if (this.nCounter <= 2 || this.pktLength + 2 > this.nCounter || this.nRcvPkt[this.pktLength + 1] != (byte)126)
                            {
                                now1 = DateTime.Now;
                                if (now1.Subtract(now8).Seconds <= (int)nTimeOut || (int)num35 >= (int)nTryCount)
                                {
                                    if ((int)num35 == (int)nTryCount)
                                        goto label_167;
                                }
                                else
                                    goto label_152;
                            }
                            else
                                break;
                        }
                        RecvDataPrint(nRcvPkt, nCounter, IsLineTrafficEnabled);
                        LineTrafficControlEventHandler($"     {commandString}", "Command");
                        ResponsePrint();
                        flag5 = true;
                        num35 = (byte)0;
                        for (int index33 = (int)Convert.ToByte((int)this.bytAddMode + 8); index33 < this.pktLength - 1; ++index33)
                            stringBuilder.Append(this.nRcvPkt[index33].ToString("X2"));
                        this.FrameType();
                        goto label_167;
                    label_152:
                        if (DLMSInfo.CmbDirect == 1 && this.nCounter > 0)
                        {
                            if (this.nRcvPkt[0] != (byte)126)
                                this.ClearBuffer();
                            flag5 = false;
                            this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)this.bytAddMode + 9));
                            if (IsLineTrafficEnabled)
                                SendDataPrint(nPkt, Convert.ToByte((int)this.bytAddMode + 9));
                            DateTime now9 = DateTime.Now;
                            while (true)
                            {
                                Application.DoEvents();
                                this.DataReceive();
                                num7 = (int)this.nRcvPkt[1] & 7;
                                this.pktLength = int.Parse(num7.ToString("X2") + this.nRcvPkt[2].ToString("X2"), NumberStyles.HexNumber);
                                if (this.pktLength + 2 > this.nCounter || this.nRcvPkt[this.pktLength + 1] != (byte)126)
                                {
                                    now1 = DateTime.Now;
                                    if (now1.Subtract(now9).Seconds > (int)nTimeOut)
                                        goto label_162;
                                }
                                else
                                    break;
                            }
                            flag5 = true;
                            oSC.DiscardInputOutputBuffer();
                            num35 = (byte)0;
                            for (int index34 = (int)Convert.ToByte((int)this.bytAddMode + 8); index34 < this.pktLength - 1; ++index34)
                                stringBuilder.Append(this.nRcvPkt[index34].ToString("X2"));
                            this.FrameType();
                            goto label_167;
                        label_162:
                            ++num35;
                        }
                        else
                            ++num35;
                        label_167:
                        if (flag5)
                        {
                            this.temp = string.Empty;
                            for (int index35 = 0; index35 < this.pktLength + 2; ++index35)
                                this.temp += this.nRcvPkt[index35].ToString("X2");
                            this.temp += "\r\n";
                            // this.swLT.Write("(R)\t" + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss:ff tt") + "\t" + this.temp);
                        }
                        if (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] == (byte)151)
                            goto label_173;
                    }
                    while (!flag5 && (int)num35 != (int)nTryCount);
                    goto label_176;
                label_173:
                    return false;
                label_176:
                    if (!flag5)
                        return false;
                }
                if (stringBuilder.ToString().StartsWith("CC") || stringBuilder.ToString().StartsWith("D4"))
                {
                    if (stringBuilder.ToString().StartsWith("CC82") || stringBuilder.ToString().StartsWith("D482"))
                        stringBuilder.Remove(0, 8);
                    else if (stringBuilder.ToString().StartsWith("CC81") || stringBuilder.ToString().StartsWith("D481"))
                        stringBuilder.Remove(0, 6);
                    else
                        stringBuilder.Remove(0, 4);
                    byte[] numArray = this.Decrypt(stringBuilder.ToString(), this.sEncryptkeyinHEX);
                    stringBuilder.Length = 0;
                    for (int index36 = 0; index36 < numArray.Length; ++index36)
                        stringBuilder.Append(numArray[index36].ToString("X2"));
                    // this.swLT.Write("\t" + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss:ff tt") + "\t" + "Response : -- >> " + stringBuilder.ToString() + "\r\n");
                    responseString = "Response : -- >> " + stringBuilder.ToString() + "\r\n";
                }
                if (stringBuilder.ToString().StartsWith("C401"))
                    stringBuilder.Remove(0, 8);
                if (stringBuilder.ToString().StartsWith("C402"))
                {
                    num1 = Convert.ToInt64(stringBuilder.ToString().Substring(8, 8), 16);
                    flag1 = !Convert.ToBoolean(Convert.ToInt16(stringBuilder.ToString().Substring(6, 2)));
                    if (stringBuilder.ToString().Substring(18, 2) == "82")
                        stringBuilder.Remove(0, 24);
                    else if (stringBuilder.ToString().Substring(18, 2) == "81")
                        stringBuilder.Remove(0, 22);
                    else
                        stringBuilder.Remove(0, 20);
                }
                this.strbldDLMdata.Append(stringBuilder.ToString());
                stringBuilder.Length = 0;
            }
            //LineTrafficControlEventHandler($"     {commandString}", "Command");
            //LineTrafficControlEventHandler($"     {responseString}\r\n", "Response");
            return true;
        }

        //This is for the Meter Configuration Form
        public bool GetParameter(
          byte nClassID,
          string sOBISCode,
          byte nAttribID,
          byte nWait,
          byte nTryCount,
          byte nTimeOut,
          bool isDLM,
          out StringBuilder strbldDLMdata)
        {
            bool flag1 = false;
            long num1 = 0;
            byte num2 = Convert.ToByte((int)this.bytAddMode + 8);
            strbldDLMdata = new StringBuilder();
            this.nPkt[2] = Convert.ToByte((int)this.bytAddMode + 25);
            this.nRetLSH = Convert.ToByte((int)this.nRecvCntr << 5);
            this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nRetLSH | 16);
            this.nRetLSH = Convert.ToByte((int)this.nSentCntr << 1);
            this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nRetLSH | (int)this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)]);
            byte[] nPkt1 = this.nPkt;
            int index1 = (int)num2;
            byte num3 = (byte)(index1 + 1);
            nPkt1[index1] = (byte)230;
            byte[] nPkt2 = this.nPkt;
            int index2 = (int)num3;
            byte num4 = (byte)(index2 + 1);
            nPkt2[index2] = (byte)230;
            byte[] nPkt3 = this.nPkt;
            int index3 = (int)num4;
            byte num5 = (byte)(index3 + 1);
            nPkt3[index3] = (byte)0;
            byte[] nPkt4 = this.nPkt;
            int index4 = (int)num5;
            byte num6 = (byte)(index4 + 1);
            nPkt4[index4] = (byte)192;
            byte[] nPkt5 = this.nPkt;
            int index5 = (int)num6;
            byte num7 = (byte)(index5 + 1);
            nPkt5[index5] = (byte)1;
            byte[] nPkt6 = this.nPkt;
            int index6 = (int)num7;
            byte num8 = (byte)(index6 + 1);
            nPkt6[index6] = (byte)129;
            byte[] nPkt7 = this.nPkt;
            int index7 = (int)num8;
            byte num9 = (byte)(index7 + 1);
            nPkt7[index7] = (byte)0;
            byte[] nPkt8 = this.nPkt;
            int index8 = (int)num9;
            byte num10 = (byte)(index8 + 1);
            int num11 = (int)nClassID;
            nPkt8[index8] = (byte)num11;
            for (int index9 = 0; index9 < 6; ++index9)
                this.nPkt[(int)num10++] = Convert.ToByte(sOBISCode.Substring(index9 * 2, 2), 16);
            byte[] nPkt9 = this.nPkt;
            int index10 = (int)num10;
            byte num12 = (byte)(index10 + 1);
            int num13 = (int)nAttribID;
            nPkt9[index10] = (byte)num13;
            byte[] nPkt10 = this.nPkt;
            int index11 = (int)num12;
            byte num14 = (byte)(index11 + 1);
            nPkt10[index11] = (byte)0;
            this.DCl.fcs(ref this.nPkt, Convert.ToByte((int)this.bytAddMode + 5), (byte)1);
            this.DCl.fcs(ref this.nPkt, Convert.ToByte((int)num14 - 1), (byte)1);
            this.nPkt[(int)num14 + 2] = (byte)126;
            if (isDLM)
                strbldDLMdata.Append("\r\n00" + nClassID.ToString("X2") + " " + sOBISCode + " " + nAttribID.ToString("X2") + " ");
            byte num15 = 0;
            bool flag2;
            int num16;
            DateTime now1;
            do
            {
                this.Wait((double)nWait);
                this.ClearBuffer();
                flag2 = false;
                this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)num14 + 3));
                DateTime now2 = DateTime.Now;
                while (true)
                {
                    Application.DoEvents();
                    this.DataReceive();
                    num16 = int.Parse(((int)this.nRcvPkt[1] & 7).ToString("X2") + this.nRcvPkt[2].ToString("X2"), NumberStyles.HexNumber);
                    if (this.nCounter <= 2 || num16 + 2 > this.nCounter || this.nRcvPkt[num16 + 1] != (byte)126)
                    {
                        now1 = DateTime.Now;
                        if (now1.Subtract(now2).Seconds <= (int)nTimeOut || (int)num15 >= (int)nTryCount)
                        {
                            if ((int)num15 == (int)nTryCount)
                                goto label_12;
                        }
                        else
                            goto label_9;
                    }
                    else
                        break;
                }
                flag2 = true;
                num15 = (byte)0;
                this.FrameType();
                goto label_12;
            label_9:
                ++num15;
            label_12:;
            }
            while (!flag2 && (int)num15 != (int)nTryCount);
            if (!flag2 || this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] == (byte)151 || ((int)this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] & 1) == 1)
                return false;
            if (flag2 && this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 11)] == (byte)196 && this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 12)] == (byte)2)
            {
                num1 = long.Parse(this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 15)].ToString("X2") + this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 16)].ToString("X2") + this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 17)].ToString("X2") + this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 18)].ToString("X2"), NumberStyles.HexNumber);
                flag1 = !Convert.ToBoolean(this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 14)]);
            }
            if (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 11)] == (byte)196 && this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 12)] == (byte)2)
            {
                if (this.nRcvPkt[(int)this.bytAddMode + 20] == (byte)130)
                {
                    for (int index12 = (int)Convert.ToByte((int)this.bytAddMode + 23); index12 < num16 - 1; ++index12)
                        strbldDLMdata.Append(this.nRcvPkt[index12].ToString("X2"));
                }
                else if (this.nRcvPkt[(int)this.bytAddMode + 20] == (byte)129)
                {
                    for (int index13 = (int)Convert.ToByte((int)this.bytAddMode + 22); index13 < num16 - 1; ++index13)
                        strbldDLMdata.Append(this.nRcvPkt[index13].ToString("X2"));
                }
                else
                {
                    for (int index14 = (int)Convert.ToByte((int)this.bytAddMode + 21); index14 < num16 - 1; ++index14)
                        strbldDLMdata.Append(this.nRcvPkt[index14].ToString("X2"));
                }
            }
            else
            {
                for (int index15 = (int)Convert.ToByte((int)this.bytAddMode + 15); index15 < num16 - 1; ++index15)
                    strbldDLMdata.Append(this.nRcvPkt[index15].ToString("X2"));
            }
            while (((int)this.nRcvPkt[1] & 168) == 168)
            {
                this.nPkt[2] = Convert.ToByte((int)this.bytAddMode + 7);
                this.nRetLSH = Convert.ToByte((int)this.nRecvCntr << 5);
                this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nRetLSH | 16);
                this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] | 1);
                this.DCl.fcs(ref this.nPkt, Convert.ToByte((int)this.bytAddMode + 5), (byte)1);
                this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 8)] = (byte)126;
                byte num17 = 0;
                bool flag3;
                do
                {
                    this.Wait((double)nWait);
                    this.ClearBuffer();
                    flag3 = false;
                    this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)this.bytAddMode + 9));
                    DateTime now3 = DateTime.Now;
                    int num18;
                    while (true)
                    {
                        Application.DoEvents();
                        this.DataReceive();
                        num18 = int.Parse(((int)this.nRcvPkt[1] & 7).ToString("X2") + this.nRcvPkt[2].ToString("X2"), NumberStyles.HexNumber);
                        if (this.nCounter <= 2 || num18 + 2 > this.nCounter || this.nRcvPkt[num18 + 1] != (byte)126)
                        {
                            now1 = DateTime.Now;
                            if (now1.Subtract(now3).Seconds <= (int)nTimeOut || (int)num17 >= (int)nTryCount)
                            {
                                if ((int)num17 == (int)nTryCount)
                                    goto label_43;
                            }
                            else
                                goto label_40;
                        }
                        else
                            break;
                    }
                    flag3 = true;
                    num17 = (byte)0;
                    for (int index16 = (int)Convert.ToByte((int)this.bytAddMode + 8); index16 < num18 - 1; ++index16)
                        strbldDLMdata.Append(this.nRcvPkt[index16].ToString("X2"));
                    this.FrameType();
                    goto label_43;
                label_40:
                    ++num17;
                label_43:
                    if (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] == (byte)151 || ((int)this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] & 1) == 1)
                        goto label_44;
                }
                while (!flag3 && (int)num17 != (int)nTryCount);
                goto label_47;
            label_44:
                return false;
            label_47:
                if (!flag3 || this.nRcvPkt[1] != (byte)160)
                {
                    if (!flag3)
                        return false;
                }
                else
                    break;
            }
            while (flag1)
            {
                flag1 = false;
                this.nPkt[2] = Convert.ToByte((int)this.bytAddMode + 19);
                this.nRetLSH = Convert.ToByte((int)this.nRecvCntr << 5);
                this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nRetLSH | 16);
                this.nRetLSH = Convert.ToByte((int)this.nSentCntr << 1);
                this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nRetLSH | (int)this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)]);
                byte num19 = Convert.ToByte((int)this.bytAddMode + 8);
                byte[] nPkt11 = this.nPkt;
                int index17 = (int)num19;
                byte num20 = (byte)(index17 + 1);
                nPkt11[index17] = (byte)230;
                byte[] nPkt12 = this.nPkt;
                int index18 = (int)num20;
                byte num21 = (byte)(index18 + 1);
                nPkt12[index18] = (byte)230;
                byte[] nPkt13 = this.nPkt;
                int index19 = (int)num21;
                byte num22 = (byte)(index19 + 1);
                nPkt13[index19] = (byte)0;
                byte[] nPkt14 = this.nPkt;
                int index20 = (int)num22;
                byte num23 = (byte)(index20 + 1);
                nPkt14[index20] = (byte)192;
                byte[] nPkt15 = this.nPkt;
                int index21 = (int)num23;
                byte num24 = (byte)(index21 + 1);
                nPkt15[index21] = (byte)2;
                byte[] nPkt16 = this.nPkt;
                int index22 = (int)num24;
                byte num25 = (byte)(index22 + 1);
                nPkt16[index22] = (byte)129;
                byte[] nPkt17 = this.nPkt;
                int index23 = (int)num25;
                byte num26 = (byte)(index23 + 1);
                nPkt17[index23] = (byte)0;
                byte[] nPkt18 = this.nPkt;
                int index24 = (int)num26;
                byte num27 = (byte)(index24 + 1);
                nPkt18[index24] = (byte)0;
                byte[] nPkt19 = this.nPkt;
                int index25 = (int)num27;
                byte num28 = (byte)(index25 + 1);
                int num29 = (int)Convert.ToByte(num1 / 256L);
                nPkt19[index25] = (byte)num29;
                byte[] nPkt20 = this.nPkt;
                int index26 = (int)num28;
                byte num30 = (byte)(index26 + 1);
                int num31 = (int)Convert.ToByte(num1 % 256L);
                nPkt20[index26] = (byte)num31;
                this.DCl.fcs(ref this.nPkt, Convert.ToByte((int)this.bytAddMode + 5), (byte)1);
                this.DCl.fcs(ref this.nPkt, Convert.ToByte((int)num30 - 1), (byte)1);
                this.nPkt[(int)num30 + 2] = (byte)126;
                byte num32 = 0;
                bool flag4;
                int num33;
                do
                {
                    this.ClearBuffer();
                    flag4 = false;
                    this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)num30 + 3));
                    this.Wait((double)nWait);
                    DateTime now4 = DateTime.Now;
                    while (true)
                    {
                        Application.DoEvents();
                        this.DataReceive();
                        num33 = int.Parse(((int)this.nRcvPkt[1] & 7).ToString("X2") + this.nRcvPkt[2].ToString("X2"), NumberStyles.HexNumber);
                        if (this.nCounter <= 2 || num33 + 2 > this.nCounter || this.nRcvPkt[num33 + 1] != (byte)126)
                        {
                            now1 = DateTime.Now;
                            if (now1.Subtract(now4).Seconds <= (int)nTimeOut || (int)num32 >= (int)nTryCount)
                            {
                                if ((int)num32 == (int)nTryCount)
                                    goto label_58;
                            }
                            else
                                goto label_55;
                        }
                        else
                            break;
                    }
                    flag4 = true;
                    num32 = (byte)0;
                    this.FrameType();
                    goto label_58;
                label_55:
                    ++num32;
                label_58:
                    if (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] == (byte)151 || ((int)this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] & 1) == 1)
                        goto label_59;
                }
                while (!flag4 && (int)num32 != (int)nTryCount);
                goto label_62;
            label_59:
                return false;
            label_62:
                if (flag4 && this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 11)] == (byte)196 && this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 12)] == (byte)2)
                {
                    num1 = long.Parse(this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 15)].ToString("X2") + this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 16)].ToString("X2") + this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 17)].ToString("X2") + this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 18)].ToString("X2"), NumberStyles.HexNumber);
                    flag1 = !Convert.ToBoolean(this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 14)]);
                }
                if (!flag4)
                    return false;
                if (this.nRcvPkt[(int)this.bytAddMode + 20] == (byte)130)
                {
                    for (int index27 = (int)Convert.ToByte((int)this.bytAddMode + 23); index27 < num33 - 1; ++index27)
                        strbldDLMdata.Append(this.nRcvPkt[index27].ToString("X2"));
                }
                else if (this.nRcvPkt[(int)this.bytAddMode + 20] == (byte)129)
                {
                    for (int index28 = (int)Convert.ToByte((int)this.bytAddMode + 22); index28 < num33 - 1; ++index28)
                        strbldDLMdata.Append(this.nRcvPkt[index28].ToString("X2"));
                }
                else
                {
                    for (int index29 = (int)Convert.ToByte((int)this.bytAddMode + 21); index29 < num33 - 1; ++index29)
                        strbldDLMdata.Append(this.nRcvPkt[index29].ToString("X2"));
                }
                while (((int)this.nRcvPkt[1] & 168) == 168)
                {
                    this.nPkt[2] = Convert.ToByte((int)this.bytAddMode + 7);
                    this.nRetLSH = Convert.ToByte((int)this.nRecvCntr << 5);
                    this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nRetLSH | 16);
                    this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] | 1);
                    this.DCl.fcs(ref this.nPkt, Convert.ToByte((int)this.bytAddMode + 5), (byte)1);
                    this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 8)] = (byte)126;
                    byte num34 = 0;
                    bool flag5;
                    do
                    {
                        flag5 = false;
                        this.Wait((double)nWait);
                        this.ClearBuffer();
                        this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)this.bytAddMode + 9));
                        DateTime now5 = DateTime.Now;
                        int num35;
                        while (true)
                        {
                            Application.DoEvents();
                            this.DataReceive();
                            num35 = int.Parse(((int)this.nRcvPkt[1] & 7).ToString("X2") + this.nRcvPkt[2].ToString("X2"), NumberStyles.HexNumber);
                            if (this.nCounter <= 2 || num35 + 2 > this.nCounter || this.nRcvPkt[num35 + 1] != (byte)126)
                            {
                                now1 = DateTime.Now;
                                if (now1.Subtract(now5).Seconds <= (int)nTimeOut || (int)num34 >= (int)nTryCount)
                                {
                                    if ((int)num34 == (int)nTryCount)
                                        goto label_87;
                                }
                                else
                                    goto label_84;
                            }
                            else
                                break;
                        }
                        flag5 = true;
                        num34 = (byte)0;
                        for (int index30 = (int)Convert.ToByte((int)this.bytAddMode + 8); index30 < num35 - 1; ++index30)
                            strbldDLMdata.Append(this.nRcvPkt[index30].ToString("X2"));
                        this.FrameType();
                        goto label_87;
                    label_84:
                        ++num34;
                    label_87:
                        if (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] == (byte)151 || ((int)this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] & 1) == 1)
                            goto label_88;
                    }
                    while (!flag5 && (int)num34 != (int)nTryCount);
                    goto label_91;
                label_88:
                    return false;
                label_91:
                    if (!flag5)
                        return false;
                }
            }
            return true;
        }

        //PUSH type get parameter
        public bool GetParameter_PUSH(
            string sWhichData,
            byte nWait,
            byte nTryCount,
            byte nTimeOut,
            byte nType,
            DateTime dateStartDate,
            DateTime dateEndDate,
            string sOBISSelect,
            ulong nFrom,
            ulong nTo,
            bool IsLineTrafficEnabled = true,
            string sOBISlist = "0100")
        {
            if (IsLineTrafficEnabled)
                LineTrafficControlEventHandler($"     GET CLASS-{Convert.ToInt32(sWhichData.Substring(0, 4), 16)} | OBIS-{DLMSParser.GetObis(sWhichData.Substring(4, 12))} [{DLMSParser.GetObisName(Convert.ToInt32(sWhichData.Substring(0, 4), 16).ToString(), DLMSParser.GetObis(sWhichData.Substring(4, 12)), Convert.ToInt32(sWhichData.Substring(sWhichData.Length - 2), 16).ToString())}] | Attribute-{Convert.ToInt32(sWhichData.Substring(sWhichData.Length - 2), 16)}", "Send");
            bool flag1 = false;
            long num1 = 0;
            byte num2 = Convert.ToByte((int)this.bytAddMode + 8);
            byte num3 = 0;
            string empty = string.Empty;
            StringBuilder stringBuilder = new StringBuilder();
            this.strbldDLMdata.Remove(0, this.strbldDLMdata.Length);
            //DLM file OBIS code and Attribute ID
            strbldDLMdata.Append($"\r\n{sWhichData.Substring(0, 4)} {sWhichData.Substring(4, 12)} {sWhichData.Substring(sWhichData.Length - 2)} ");
            //++this.PB1.Value;
            //if (this.PB1.Value == 100)
            //  this.PB1.Value = 0;
            this.nRetLSH = Convert.ToByte((int)this.nRecvCntr << 5);
            this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nRetLSH | 16);
            this.nRetLSH = Convert.ToByte((int)this.nSentCntr << 1);
            this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nRetLSH | (int)this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)]);
            byte[] nPkt1 = this.nPkt;
            int index1 = (int)num2;
            byte num4 = (byte)(index1 + 1);
            nPkt1[index1] = (byte)230;//E6
            byte[] nPkt2 = this.nPkt;
            int index2 = (int)num4;
            byte num5 = (byte)(index2 + 1);
            nPkt2[index2] = (byte)230;//E6
            byte[] nPkt3 = this.nPkt;
            int index3 = (int)num5;
            byte num6 = (byte)(index3 + 1);
            nPkt3[index3] = (byte)0;//00
            string hex1;
            int num7;
            switch (nType)
            {
                case 0:
                    hex1 = "C001C1" + sWhichData + "00";
                    break;
                case 1:
                    if (sOBISSelect == "12000809060000010000FF0F02120000")
                    {
                        string[] strArray = new string[20];
                        strArray[0] = "C00181";
                        strArray[1] = sWhichData;
                        strArray[2] = "010102040204";
                        strArray[3] = sOBISSelect;
                        strArray[4] = "090C";
                        strArray[5] = dateStartDate.Year.ToString("X4");
                        strArray[6] = dateStartDate.Month.ToString("X2");
                        num7 = dateStartDate.Day;
                        strArray[7] = num7.ToString("X2");
                        strArray[8] = "FF";
                        num7 = dateStartDate.Hour;
                        strArray[9] = num7.ToString("X2");
                        num7 = dateStartDate.Minute;
                        strArray[10] = num7.ToString("X2");
                        strArray[11] = "0000800000090C";
                        num7 = dateEndDate.Year;
                        strArray[12] = num7.ToString("X4");
                        num7 = dateEndDate.Month;
                        strArray[13] = num7.ToString("X2");
                        num7 = dateEndDate.Day;
                        strArray[14] = num7.ToString("X2");
                        strArray[15] = "FF";
                        num7 = dateEndDate.Hour;
                        strArray[16] = num7.ToString("X2");
                        num7 = dateEndDate.Minute;
                        strArray[17] = num7.ToString("X2");
                        strArray[18] = "00008000000100";
                        strArray[19] = sOBISlist;
                        hex1 = string.Concat(strArray);
                        break;
                    }
                    hex1 = "C00181" + sWhichData + "010102040204" + sOBISSelect + "06" + nFrom.ToString("X8") + "06" + nTo.ToString("X8") + sOBISlist;
                    break;
                case 2:
                    //hex1 = "C00181" + sWhichData + "0102020406" + nFrom.ToString("X8") + "06" + nTo.ToString("X8") + "12" + nFrom.ToString("X4") + "12" + this.Tovalue.ToString("X4");
                    hex1 = "C00181" + sWhichData + "0102020406" + nFrom.ToString("X8") + "06" + nTo.ToString("X8") + "120001" + "120000";
                    break;
                default:
                    hex1 = "C00181" + sWhichData + "0102020406" + this.FromEntry.ToString("X8") + "06" + this.ToEntry.ToString("X8") + "12" + this.Fromvalue.ToString("X4") + "12" + this.Tovalue.ToString("X4");
                    break;
            }
            //this.swLT.Write("\t" + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss:ff tt") + "\t" + "Command : -- >> " + hex1 + "\r\n");
            commandString = "Command  : -- >> " + hex1 + "\r\n";
            ASCIIEncoding asciiEncoding = new ASCIIEncoding();
            if (DLMSInfo.IsLNWithCipher || DLMSInfo.IsLNWithCipherDedicatedKey)
            {

                if (DLMSInfo.AccessMode == 1)
                    num3 = (byte)32;//0x20
                else if (DLMSInfo.AccessMode == 2 || DLMSInfo.AccessMode == 4 || DLMSInfo.AccessMode == 3)
                    num3 = (byte)48;//0x30
                byte num8;
                byte num9;
                if (DLMSInfo.IsLNWithCipher && DLMSInfo.AccessMode == 3)
                {
                    byte[] nPkt4 = this.nPkt;
                    int index4 = (int)num6;
                    num7 = (byte)(index4 + 1);
                    nPkt4[index4] = (byte)219;//0xDB
                    byte[] nPkt5 = this.nPkt;
                    int index5 = (int)num7;
                    num8 = (byte)(index5 + 1);
                    int num22 = (int)Convert.ToByte(DLMSInfo.TxtSysT.Trim().Length);
                    nPkt5[index5] = (byte)num22;
                    this.Ps = asciiEncoding.GetBytes(DLMSInfo.TxtSysT.Trim());
                    for (int index22 = 0; index22 < DLMSInfo.TxtSysT.Trim().Length; ++index22)
                        this.nPkt[(int)num8++] = this.Ps[index22];
                }
                else if (DLMSInfo.IsLNWithCipherDedicatedKey)
                {
                    byte[] nPkt4 = this.nPkt;
                    int index4 = (int)num6;
                    num8 = (byte)(index4 + 1);
                    nPkt4[index4] = (byte)208;//0xD0
                }
                else
                {
                    byte[] nPkt5 = this.nPkt;
                    int index5 = (int)num6;
                    num8 = (byte)(index5 + 1);
                    nPkt5[index5] = (byte)200;//0xC8
                }
                byte[] nPkt6 = this.nPkt;
                int index6 = (int)num8;
                num9 = (byte)(index6 + 1);
                nPkt6[index6] = (byte)0;//0x00
                byte[] nPkt7 = this.nPkt;
                int index7 = (int)num9;
                byte num10 = (byte)(index7 + 1);
                int num11 = (int)num3;
                nPkt7[index7] = (byte)num11;
                byte[] nPkt8 = this.nPkt;
                int index8 = (int)num10;
                byte num12 = (byte)(index8 + 1);
                nPkt8[index8] = (byte)0;//0x00
                byte[] nPkt9 = this.nPkt;
                int index9 = (int)num12;
                byte num13 = (byte)(index9 + 1);
                nPkt9[index9] = (byte)0;//0x00
                byte[] nPkt10 = this.nPkt;
                int index10 = (int)num13;
                byte num14 = (byte)(index10 + 1);
                int num15 = (int)Convert.ToByte(this.nCommandCounter / 256);
                nPkt10[index10] = (byte)num15;
                byte[] nPkt11 = this.nPkt;
                int index11 = (int)num14;
                num6 = (byte)(index11 + 1);
                int num16 = (int)Convert.ToByte(this.nCommandCounter % 256);
                nPkt11[index11] = (byte)num16;
                string str1 = num3.ToString("X2");
                num7 = this.nCommandCounter++;
                string str2 = num7.ToString("X8");
                string str3 = hex1;
                this.sEncryptkeyinHEX = string.Empty;
                this.Ps1 = asciiEncoding.GetBytes(DLMSInfo.TxtEK.Trim());
                for (int index54 = 0; index54 < this.Ps1.Length; ++index54)
                    this.sEncryptkeyinHEX += this.Ps1[index54].ToString("X2");
                byte[] numArray = this.Encrypt(str1 + str2 + str3, this.sEncryptkeyinHEX);
                this.nPkt[(int)num6 - 6] = Convert.ToByte(numArray.Length + 5);
                for (int index12 = 0; index12 < numArray.Length; ++index12)
                    this.nPkt[(int)num6++] = numArray[index12];
            }
            else
            {
                foreach (byte num17 in StringToByteArray(hex1))
                    this.nPkt[(int)num6++] = num17;
                commandString = "Command  : -- >> " + hex1 + "\r\n";
            }
            this.nPkt[1] = (byte)160;
            this.nPkt[2] = Convert.ToByte((int)num6 + 1);
            this.DCl.fcs(ref this.nPkt, (int)Convert.ToByte((int)this.bytAddMode + 5), (byte)1);
            this.DCl.fcs(ref this.nPkt, (int)Convert.ToByte((int)num6 - 1), (byte)1);
            this.nPkt[(int)num6 + 2] = (byte)126;
            byte num18 = 0;
            bool flag2;
            DateTime now1;
            //LineTrafficControlEventHandler($"     GET CLASS-{Convert.ToInt32(sWhichData.Substring(0, 4), 16)} | OBIS-{DLMSParser.GetObis(sWhichData.Substring(4, 12))} [{DLMSParser.GetObisName(Convert.ToInt32(sWhichData.Substring(0, 4), 16).ToString(), DLMSParser.GetObis(sWhichData.Substring(4, 12)))}] | Attribute-{Convert.ToInt32(sWhichData.Substring(sWhichData.Length - 2), 16)}", "Send");
            do
            {
                this.Wait((double)nWait);
                this.ClearBuffer();
                flag2 = false;
                this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)num6 + 3));
                if (IsLineTrafficEnabled)
                    SendDataPrint(nPkt, Convert.ToByte((int)num6 + 3));
                //LineTrafficControlEventHandler($"     {commandString}", "Command");
                DateTime now2 = DateTime.Now;
                while (true)
                {
                    Application.DoEvents();
                    this.DataReceive();
                    num7 = (int)this.nRcvPkt[1] & 7;
                    this.pktLength = int.Parse(num7.ToString("X2") + this.nRcvPkt[2].ToString("X2"), NumberStyles.HexNumber);
                    if (this.nCounter <= 2 || this.pktLength + 2 > this.nCounter || this.nRcvPkt[this.pktLength + 1] != (byte)126)
                    {
                        now1 = DateTime.Now;
                        if (now1.Subtract(now2).Seconds <= (int)nTimeOut || (int)num18 >= (int)nTryCount)
                        {
                            if ((int)num18 == (int)nTryCount)
                                goto label_38;
                        }
                        else
                            goto label_26;
                    }
                    else
                        break;
                }
                RecvDataPrint(nRcvPkt, nCounter, IsLineTrafficEnabled);
                if (IsLineTrafficEnabled)
                {
                    LineTrafficControlEventHandler($"     {commandString}", "Command");
                    ResponsePrint();
                }
                //LineTrafficControlEventHandler($"     {responseString}\r\n", "Response");
                //LineTrafficControlEventHandler("\r\n", "Send");
                flag2 = true;
                num18 = (byte)0;
                this.FrameType();
                goto label_38;
            label_26:
                if (DLMSInfo.CmbDirect == 1 && this.nCounter > 0)
                {
                    if (this.nRcvPkt[0] != (byte)126)
                        this.ClearBuffer();
                    flag2 = false;
                    this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)this.bytAddMode + 27));
                    if (IsLineTrafficEnabled)
                        SendDataPrint(nPkt, Convert.ToByte((int)this.bytAddMode + 27));
                    //LineTrafficControlEventHandler($"     {commandString}", "Command");
                    DateTime now3 = DateTime.Now;
                    while (true)
                    {
                        Application.DoEvents();
                        this.DataReceive();
                        num7 = (int)this.nRcvPkt[1] & 7;
                        this.pktLength = int.Parse(num7.ToString("X2") + this.nRcvPkt[2].ToString("X2"), NumberStyles.HexNumber);
                        if (this.pktLength + 2 > this.nCounter || this.nRcvPkt[this.pktLength + 1] != (byte)126)
                        {
                            now1 = DateTime.Now;
                            if (now1.Subtract(now3).Seconds > (int)nTimeOut)
                                goto label_33;
                        }
                        else
                            break;
                    }
                    RecvDataPrint(nRcvPkt, nCounter, IsLineTrafficEnabled);
                    if (IsLineTrafficEnabled)
                    {
                        LineTrafficControlEventHandler($"     {commandString}", "Command");
                        ResponsePrint();
                    }
                    flag2 = true;
                    oSC.DiscardInputOutputBuffer();
                    num18 = (byte)0;
                    this.FrameType();
                    goto label_38;
                label_33:
                    ++num18;
                }
                else
                    ++num18;
                label_38:
                if (flag2)
                {
                    this.temp = string.Empty;
                    num7 = (int)this.nRcvPkt[1] & 7;
                    this.pktLength = int.Parse(num7.ToString("X2") + this.nRcvPkt[2].ToString("X2"), NumberStyles.HexNumber);
                    for (int index13 = 0; index13 < this.pktLength + 2; ++index13)
                        this.temp += this.nRcvPkt[index13].ToString("X2");
                    this.temp += "\r\n";
                    //this.swLT.Write("(R)\t" + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss:ff tt") + "\t" + this.temp);
                    responseString = temp.Trim();
                    responseString = $"Response : -- >> {responseString.Substring(22, responseString.Length - 28)}\r\n";
                }
            }
            while (!flag2 && (int)num18 != (int)nTryCount);
            if (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] == (byte)151 || ((int)this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] & 1) == 1)
                return false;
            for (int index14 = (int)this.bytAddMode + 11; index14 < this.nCounter - 3; ++index14)
                stringBuilder.Append(this.nRcvPkt[index14].ToString("X2"));
            while (((int)this.nRcvPkt[1] & 168) == 168)
            {
                this.nPkt[2] = Convert.ToByte((int)this.bytAddMode + 7);
                this.nRetLSH = Convert.ToByte((int)this.nRecvCntr << 5);
                this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nRetLSH | 16);
                this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] | 1);
                this.DCl.fcs(ref this.nPkt, (int)Convert.ToByte((int)this.bytAddMode + 5), (byte)1);
                this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 8)] = (byte)126;
                byte num19 = 0;
                bool flag3;
                do
                {
                    this.Wait((double)nWait);
                    this.ClearBuffer();
                    flag3 = false;
                    this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)this.bytAddMode + 9));
                    if (IsLineTrafficEnabled)
                        SendDataPrint(nPkt, Convert.ToByte((int)this.bytAddMode + 9));
                    //LineTrafficControlEventHandler($"     {commandString}", "Command");
                    DateTime now4 = DateTime.Now;
                    while (true)
                    {
                        Application.DoEvents();
                        this.DataReceive();
                        num7 = (int)this.nRcvPkt[1] & 7;
                        this.pktLength = int.Parse(num7.ToString("X2") + this.nRcvPkt[2].ToString("X2"), NumberStyles.HexNumber);
                        if (this.nCounter <= 2 || this.pktLength + 2 > this.nCounter || this.nRcvPkt[this.pktLength + 1] != (byte)126)
                        {
                            now1 = DateTime.Now;
                            if (now1.Subtract(now4).Seconds <= (int)nTimeOut || (int)num19 >= (int)nTryCount)
                            {
                                if ((int)num19 == (int)nTryCount)
                                    goto label_72;
                            }
                            else
                                goto label_57;
                        }
                        else
                            break;
                    }
                    RecvDataPrint(nRcvPkt, nCounter, IsLineTrafficEnabled);
                    LineTrafficControlEventHandler($"     {commandString}", "Command");
                    ResponsePrint();
                    flag3 = true;
                    num19 = (byte)0;
                    for (int index15 = (int)Convert.ToByte((int)this.bytAddMode + 8); index15 < this.pktLength - 1; ++index15)
                        stringBuilder.Append(this.nRcvPkt[index15].ToString("X2"));
                    this.FrameType();
                    goto label_72;
                label_57:
                    if (DLMSInfo.CmbDirect == 1 && this.nCounter > 0)
                    {
                        if (this.nRcvPkt[0] != (byte)126)
                            this.ClearBuffer();
                        flag3 = false;
                        this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)this.bytAddMode + 9));
                        if (IsLineTrafficEnabled)
                            SendDataPrint(nPkt, Convert.ToByte((int)this.bytAddMode + 9));
                        //LineTrafficControlEventHandler($"     {commandString}", "Command");
                        DateTime now5 = DateTime.Now;
                        while (true)
                        {
                            Application.DoEvents();
                            this.DataReceive();
                            num7 = (int)this.nRcvPkt[1] & 7;
                            this.pktLength = int.Parse(num7.ToString("X2") + this.nRcvPkt[2].ToString("X2"), NumberStyles.HexNumber);
                            if (this.pktLength + 2 > this.nCounter || this.nRcvPkt[this.pktLength + 1] != (byte)126)
                            {
                                now1 = DateTime.Now;
                                if (now1.Subtract(now5).Seconds > (int)nTimeOut)
                                    goto label_67;
                            }
                            else
                                break;
                        }
                        RecvDataPrint(nRcvPkt, nCounter, IsLineTrafficEnabled);
                        LineTrafficControlEventHandler($"     {commandString}", "Command");
                        ResponsePrint();
                        flag3 = true;
                        oSC.DiscardInputOutputBuffer();
                        num19 = (byte)0;
                        for (int index16 = (int)Convert.ToByte((int)this.bytAddMode + 8); index16 < this.pktLength - 1; ++index16)
                            stringBuilder.Append(this.nRcvPkt[index16].ToString("X2"));
                        this.FrameType();
                        goto label_72;
                    label_67:
                        ++num19;
                    }
                    else
                        ++num19;
                    label_72:
                    if (flag3)
                    {
                        this.temp = string.Empty;
                        for (int index17 = 0; index17 < this.pktLength + 2; ++index17)
                            this.temp += this.nRcvPkt[index17].ToString("X2");
                        this.temp += "\r\n";
                        //this.swLT.Write("(R)\t" + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss:ff tt") + "\t" + this.temp);
                    }
                    if (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] == (byte)151)
                        goto label_78;
                }
                while (!flag3 && (int)num19 != (int)nTryCount);
                goto label_81;
            label_78:
                return false;
            label_81:
                if (!flag3 || this.nRcvPkt[1] != (byte)160)
                {
                    if (!flag3)
                        return false;
                }
                else
                    break;
            }
            if (stringBuilder.ToString().StartsWith("DB08"))
            {
                this.sSYSTaginHEX = stringBuilder.ToString().Substring(4, 16);
                stringBuilder.Remove(0, 20);
                if (stringBuilder.ToString().StartsWith("82"))
                {
                    stringBuilder.Remove(0, 6);
                }
                this.sEncryptkeyinHEX = string.Empty;
                this.Ps1 = asciiEncoding.GetBytes(DLMSInfo.TxtEK.Trim());
                for (int index54 = 0; index54 < this.Ps1.Length; ++index54)
                    this.sEncryptkeyinHEX += this.Ps1[index54].ToString("X2");
                byte[] numArray = this.Decrypt(stringBuilder.ToString(), this.sEncryptkeyinHEX);
                stringBuilder.Length = 0;
                for (int index18 = 0; index18 < numArray.Length; ++index18)
                    stringBuilder.Append(numArray[index18].ToString("X2"));
                responseString = "Response : -- >> " + stringBuilder.ToString() + "\r\n";
            }
            if (stringBuilder.ToString().StartsWith("CC") || stringBuilder.ToString().StartsWith("D4"))
            {
                if (stringBuilder.ToString().StartsWith("CC82") || stringBuilder.ToString().StartsWith("D482"))
                    stringBuilder.Remove(0, 8);
                else if (stringBuilder.ToString().StartsWith("CC81") || stringBuilder.ToString().StartsWith("D481"))
                    stringBuilder.Remove(0, 6);
                else
                    stringBuilder.Remove(0, 4);
                byte[] numArray = this.Decrypt(stringBuilder.ToString(), this.sEncryptkeyinHEX);
                stringBuilder.Length = 0;
                for (int index18 = 0; index18 < numArray.Length; ++index18)
                    stringBuilder.Append(numArray[index18].ToString("X2"));
                //this.swLT.Write("\t" + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss:ff tt") + "\t" + "Response : -- >> " + stringBuilder.ToString() + "\r\n");
                responseString = "Response : -- >> " + stringBuilder.ToString() + "\r\n";
            }
            if (stringBuilder.ToString().StartsWith("C401") && !(stringBuilder.ToString().Length < 8))
                stringBuilder.Remove(0, 8);
            if (stringBuilder.ToString().StartsWith("C402"))
            {
                num1 = Convert.ToInt64(stringBuilder.ToString().Substring(8, 8), 16);
                flag1 = !Convert.ToBoolean(Convert.ToInt16(stringBuilder.ToString().Substring(6, 2)));
                if (stringBuilder.ToString().Substring(18, 2) == "82")
                    stringBuilder.Remove(0, 24);
                else if (stringBuilder.ToString().Substring(18, 2) == "81")
                    stringBuilder.Remove(0, 22);
                else
                    stringBuilder.Remove(0, 20);
            }
            this.strbldDLMdata.Append(stringBuilder.ToString());
            stringBuilder.Length = 0;
            while (flag1)
            {
                this.temp = string.Empty;
                flag1 = false;
                this.nRetLSH = Convert.ToByte((int)this.nRecvCntr << 5);
                this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nRetLSH | 16);
                this.nRetLSH = Convert.ToByte((int)this.nSentCntr << 1);
                this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nRetLSH | (int)this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)]);
                byte num20 = Convert.ToByte((int)this.bytAddMode + 8);
                byte[] nPkt12 = this.nPkt;
                int index19 = (int)num20;
                byte num21 = (byte)(index19 + 1);
                nPkt12[index19] = (byte)230;//0xE6
                byte[] nPkt13 = this.nPkt;
                int index20 = (int)num21;
                byte num22 = (byte)(index20 + 1);
                nPkt13[index20] = (byte)230;//0xE6
                byte[] nPkt14 = this.nPkt;
                int index21 = (int)num22;
                byte num23 = (byte)(index21 + 1);
                nPkt14[index21] = (byte)0;//0x00
                string hex2 = "C00281" + num1.ToString("X8");
                commandString = "Command : -- >> " + hex2 + "\r\n";
                byte num24;//7
                byte num225 = 0;
                if (DLMSInfo.IsLNWithCipher)
                {
                    //this.swLT.Write("\t" + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss:ff tt") + "\t" + "Command : -- >> " + hex2 + "\r\n");
                    commandString = "Command : -- >> " + hex2 + "\r\n";
                    if (DLMSInfo.AccessMode == 3)
                    {
                        byte[] nPkt15 = this.nPkt;
                        int index22 = (int)num23;//6
                        num24 = (byte)(index22 + 1);
                        nPkt15[index22] = (byte)219;//0xDB
                        byte[] nPkt16 = this.nPkt;
                        int index23 = (int)num24;
                        num225 = (byte)(index23 + 1);
                        int num222 = (int)Convert.ToByte(DLMSInfo.TxtSysT.Trim().Length);
                        nPkt16[index23] = (byte)num222;
                        this.Ps = asciiEncoding.GetBytes(DLMSInfo.TxtSysT.Trim());
                        for (int index222 = 0; index222 < DLMSInfo.TxtSysT.Trim().Length; ++index222)
                            this.nPkt[(int)num225++] = this.Ps[index222];
                    }
                    else if (DLMSInfo.IsLNWithCipherDedicatedKey)
                    {
                        byte[] nPkt15 = this.nPkt;
                        int index22 = (int)num23;
                        num24 = (byte)(index22 + 1);
                        nPkt15[index22] = (byte)208;
                    }
                    else
                    {
                        byte[] nPkt16 = this.nPkt;
                        int index23 = (int)num23;
                        num24 = (byte)(index23 + 1);
                        nPkt16[index23] = (byte)200;
                    }
                    byte[] nPkt17 = this.nPkt;
                    //int index24 = (int)num24;//OLD
                    int index24 = (int)num225;//NEW
                    byte num25 = (byte)(index24 + 1);//OLD
                    nPkt17[index24] = (byte)0;//0x00
                    byte[] nPkt18 = this.nPkt;
                    int index25 = (int)num25;
                    byte num26 = (byte)(index25 + 1);//OLD
                    int num27 = (int)num3;
                    nPkt18[index25] = (byte)num27;
                    byte[] nPkt19 = this.nPkt;
                    int index26 = (int)num26;
                    byte num28 = (byte)(index26 + 1);
                    nPkt19[index26] = (byte)0;//0x00
                    byte[] nPkt20 = this.nPkt;
                    int index27 = (int)num28;
                    byte num29 = (byte)(index27 + 1);
                    nPkt20[index27] = (byte)0;//0x00
                    byte[] nPkt21 = this.nPkt;
                    int index28 = (int)num29;
                    byte num30 = (byte)(index28 + 1);
                    int num31 = (int)Convert.ToByte(this.nCommandCounter / 256);
                    nPkt21[index28] = (byte)num31;
                    byte[] nPkt22 = this.nPkt;
                    int index29 = (int)num30;
                    num23 = (byte)(index29 + 1);
                    int num32 = (int)Convert.ToByte(this.nCommandCounter % 256);
                    nPkt22[index29] = (byte)num32;
                    string str4 = num3.ToString("X2");
                    num7 = this.nCommandCounter++;
                    string str5 = num7.ToString("X8");
                    string str6 = hex2;
                    byte[] numArray = this.Encrypt(str4 + str5 + str6, this.sEncryptkeyinHEX);
                    this.nPkt[(int)num23 - 6] = Convert.ToByte(numArray.Length + 5);
                    for (int index30 = 0; index30 < numArray.Length; ++index30)
                        this.nPkt[(int)num23++] = numArray[index30];
                }
                else
                {
                    foreach (byte num33 in StringToByteArray(hex2))
                        this.nPkt[(int)num23++] = num33;
                    //commandString = "Command : -- >> " + hex2 + "\r\n";
                }
                this.nPkt[2] = Convert.ToByte((int)num23 + 1);
                this.DCl.fcs(ref this.nPkt, (int)Convert.ToByte((int)this.bytAddMode + 5), (byte)1);
                this.DCl.fcs(ref this.nPkt, (int)Convert.ToByte((int)num23 - 1), (byte)1);
                this.nPkt[(int)num23 + 2] = (byte)126;
                byte num34 = 0;
                bool flag4;
                do
                {
                    this.ClearBuffer();
                    flag4 = false;
                    this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)num23 + 3));
                    if (IsLineTrafficEnabled)
                        SendDataPrint(nPkt, Convert.ToByte((int)num23 + 3));
                    //LineTrafficControlEventHandler($"     {commandString}", "Command");
                    this.Wait((double)nWait);
                    //SendDataPrint(nPkt, Convert.ToByte((int)num23 + 3));
                    DateTime now6 = DateTime.Now;
                    while (true)
                    {
                        Application.DoEvents();
                        this.DataReceive();
                        num7 = (int)this.nRcvPkt[1] & 7;
                        this.pktLength = int.Parse(num7.ToString("X2") + this.nRcvPkt[2].ToString("X2"), NumberStyles.HexNumber);
                        if (this.nCounter <= 2 || this.pktLength + 2 > this.nCounter || this.nRcvPkt[this.pktLength + 1] != (byte)126)
                        {
                            now1 = DateTime.Now;
                            if (now1.Subtract(now6).Seconds <= (int)nTimeOut || (int)num34 >= (int)nTryCount)
                            {
                                if ((int)num34 == (int)nTryCount)
                                    goto label_131;
                            }
                            else
                                goto label_119;
                        }
                        else
                            break;
                    }
                    RecvDataPrint(nRcvPkt, nCounter, IsLineTrafficEnabled);
                    if (IsLineTrafficEnabled)
                    {
                        LineTrafficControlEventHandler($"     {commandString}", "Command");
                        ResponsePrint();
                    }
                    flag4 = true;
                    num34 = (byte)0;
                    this.FrameType();
                    goto label_131;
                label_119:
                    if (DLMSInfo.CmbDirect == 1 && this.nCounter > 0)
                    {
                        if (this.nRcvPkt[0] != (byte)126)
                            this.ClearBuffer();
                        flag4 = false;
                        this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)this.bytAddMode + 21));
                        DateTime now7 = DateTime.Now;
                        if (IsLineTrafficEnabled)
                            SendDataPrint(nPkt, Convert.ToByte((int)this.bytAddMode + 21));
                        //LineTrafficControlEventHandler($"     {commandString}", "Command");
                        while (true)
                        {
                            Application.DoEvents();
                            this.DataReceive();
                            num7 = (int)this.nRcvPkt[1] & 7;
                            this.pktLength = int.Parse(num7.ToString("X2") + this.nRcvPkt[2].ToString("X2"), NumberStyles.HexNumber);
                            if (this.pktLength + 2 > this.nCounter || this.nRcvPkt[this.pktLength + 1] != (byte)126)
                            {
                                now1 = DateTime.Now;
                                if (now1.Subtract(now7).Seconds > (int)nTimeOut)
                                    goto label_126;
                            }
                            else
                                break;
                        }
                        RecvDataPrint(nRcvPkt, nCounter, IsLineTrafficEnabled);
                        LineTrafficControlEventHandler($"     {commandString}", "Command");
                        ResponsePrint();
                        flag4 = true;
                        oSC.DiscardInputOutputBuffer();
                        num34 = (byte)0;
                        this.FrameType();
                        goto label_131;
                    label_126:
                        ++num34;
                    }
                    else
                        ++num34;
                    label_131:
                    if (flag4)
                    {
                        this.temp = string.Empty;
                        for (int index31 = 0; index31 < this.pktLength + 2; ++index31)
                            this.temp += this.nRcvPkt[index31].ToString("X2");
                        this.temp += "\r\n";
                        // this.swLT.Write("(R)\t" + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss:ff tt") + "\t" + this.temp);
                    }
                    if (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] == (byte)151)
                        goto label_137;
                }
                while (!flag4 && (int)num34 != (int)nTryCount);
                goto label_140;
            label_137:
                return false;
            label_140:
                if (!flag4)
                    return false;
                for (int index32 = (int)this.bytAddMode + 11; index32 < this.nCounter - 3; ++index32)
                    stringBuilder.Append(this.nRcvPkt[index32].ToString("X2"));
                while (((int)this.nRcvPkt[1] & 168) == 168)
                {
                    this.nPkt[2] = Convert.ToByte((int)this.bytAddMode + 7);
                    this.nRetLSH = Convert.ToByte((int)this.nRecvCntr << 5);
                    this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nRetLSH | 16);
                    this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] | 1);
                    this.DCl.fcs(ref this.nPkt, (int)Convert.ToByte((int)this.bytAddMode + 5), (byte)1);
                    this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 8)] = (byte)126;
                    this.temp = string.Empty;
                    byte num35 = 0;
                    bool flag5;
                    do
                    {
                        flag5 = false;
                        this.Wait((double)nWait);
                        this.ClearBuffer();
                        this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)this.bytAddMode + 9));
                        DateTime now8 = DateTime.Now;
                        if (IsLineTrafficEnabled)
                            SendDataPrint(nPkt, Convert.ToByte((int)this.bytAddMode + 9));
                        while (true)
                        {
                            Application.DoEvents();
                            this.DataReceive();
                            num7 = (int)this.nRcvPkt[1] & 7;
                            this.pktLength = int.Parse(num7.ToString("X2") + this.nRcvPkt[2].ToString("X2"), NumberStyles.HexNumber);
                            if (this.nCounter <= 2 || this.pktLength + 2 > this.nCounter || this.nRcvPkt[this.pktLength + 1] != (byte)126)
                            {
                                now1 = DateTime.Now;
                                if (now1.Subtract(now8).Seconds <= (int)nTimeOut || (int)num35 >= (int)nTryCount)
                                {
                                    if ((int)num35 == (int)nTryCount)
                                        goto label_167;
                                }
                                else
                                    goto label_152;
                            }
                            else
                                break;
                        }
                        RecvDataPrint(nRcvPkt, nCounter, IsLineTrafficEnabled);
                        LineTrafficControlEventHandler($"     {commandString}", "Command");
                        ResponsePrint();
                        flag5 = true;
                        num35 = (byte)0;
                        for (int index33 = (int)Convert.ToByte((int)this.bytAddMode + 8); index33 < this.pktLength - 1; ++index33)
                            stringBuilder.Append(this.nRcvPkt[index33].ToString("X2"));
                        this.FrameType();
                        goto label_167;
                    label_152:
                        if (DLMSInfo.CmbDirect == 1 && this.nCounter > 0)
                        {
                            if (this.nRcvPkt[0] != (byte)126)
                                this.ClearBuffer();
                            flag5 = false;
                            this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)this.bytAddMode + 9));
                            if (IsLineTrafficEnabled)
                                SendDataPrint(nPkt, Convert.ToByte((int)this.bytAddMode + 9));
                            DateTime now9 = DateTime.Now;
                            while (true)
                            {
                                Application.DoEvents();
                                this.DataReceive();
                                num7 = (int)this.nRcvPkt[1] & 7;
                                this.pktLength = int.Parse(num7.ToString("X2") + this.nRcvPkt[2].ToString("X2"), NumberStyles.HexNumber);
                                if (this.pktLength + 2 > this.nCounter || this.nRcvPkt[this.pktLength + 1] != (byte)126)
                                {
                                    now1 = DateTime.Now;
                                    if (now1.Subtract(now9).Seconds > (int)nTimeOut)
                                        goto label_162;
                                }
                                else
                                    break;
                            }
                            flag5 = true;
                            oSC.DiscardInputOutputBuffer();
                            num35 = (byte)0;
                            for (int index34 = (int)Convert.ToByte((int)this.bytAddMode + 8); index34 < this.pktLength - 1; ++index34)
                                stringBuilder.Append(this.nRcvPkt[index34].ToString("X2"));
                            this.FrameType();
                            goto label_167;
                        label_162:
                            ++num35;
                        }
                        else
                            ++num35;
                        label_167:
                        if (flag5)
                        {
                            this.temp = string.Empty;
                            for (int index35 = 0; index35 < this.pktLength + 2; ++index35)
                                this.temp += this.nRcvPkt[index35].ToString("X2");
                            this.temp += "\r\n";
                            // this.swLT.Write("(R)\t" + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss:ff tt") + "\t" + this.temp);
                        }
                        if (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] == (byte)151)
                            goto label_173;
                    }
                    while (!flag5 && (int)num35 != (int)nTryCount);
                    goto label_176;
                label_173:
                    return false;
                label_176:
                    if (!flag5)
                        return false;
                }
                if (stringBuilder.ToString().StartsWith("DB08"))
                {
                    this.sSYSTaginHEX = stringBuilder.ToString().Substring(4, 16);
                    stringBuilder.Remove(0, 20);
                    if (stringBuilder.ToString().StartsWith("82"))
                    {
                        stringBuilder.Remove(0, 6);
                    }
                    this.sEncryptkeyinHEX = string.Empty;
                    this.Ps1 = asciiEncoding.GetBytes(DLMSInfo.TxtEK.Trim());
                    for (int index54 = 0; index54 < this.Ps1.Length; ++index54)
                        this.sEncryptkeyinHEX += this.Ps1[index54].ToString("X2");
                    byte[] numArray = this.Decrypt(stringBuilder.ToString(), this.sEncryptkeyinHEX);
                    stringBuilder.Length = 0;
                    for (int index18 = 0; index18 < numArray.Length; ++index18)
                        stringBuilder.Append(numArray[index18].ToString("X2"));
                    responseString = "Response : -- >> " + stringBuilder.ToString() + "\r\n";
                }
                if (stringBuilder.ToString().StartsWith("CC") || stringBuilder.ToString().StartsWith("D4"))
                {
                    if (stringBuilder.ToString().StartsWith("CC82") || stringBuilder.ToString().StartsWith("D482"))
                        stringBuilder.Remove(0, 8);
                    else if (stringBuilder.ToString().StartsWith("CC81") || stringBuilder.ToString().StartsWith("D481"))
                        stringBuilder.Remove(0, 6);
                    else
                        stringBuilder.Remove(0, 4);
                    byte[] numArray = this.Decrypt(stringBuilder.ToString(), this.sEncryptkeyinHEX);
                    stringBuilder.Length = 0;
                    for (int index36 = 0; index36 < numArray.Length; ++index36)
                        stringBuilder.Append(numArray[index36].ToString("X2"));
                    // this.swLT.Write("\t" + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss:ff tt") + "\t" + "Response : -- >> " + stringBuilder.ToString() + "\r\n");
                    responseString = "Response : -- >> " + stringBuilder.ToString() + "\r\n";
                }
                if (stringBuilder.ToString().StartsWith("C401"))
                    stringBuilder.Remove(0, 8);
                if (stringBuilder.ToString().StartsWith("C402"))
                {
                    num1 = Convert.ToInt64(stringBuilder.ToString().Substring(8, 8), 16);
                    flag1 = !Convert.ToBoolean(Convert.ToInt16(stringBuilder.ToString().Substring(6, 2)));
                    if (stringBuilder.ToString().Substring(18, 2) == "82")
                        stringBuilder.Remove(0, 24);
                    else if (stringBuilder.ToString().Substring(18, 2) == "81")
                        stringBuilder.Remove(0, 22);
                    else
                        stringBuilder.Remove(0, 20);
                }
                this.strbldDLMdata.Append(stringBuilder.ToString());
                stringBuilder.Length = 0;
            }
            //LineTrafficControlEventHandler($"     {commandString}", "Command");
            //LineTrafficControlEventHandler($"     {responseString}\r\n", "Response");
            return true;
        }

        public int SetParameter(
            string sWhichData,
            byte nWait,
            byte nTryCount,
            byte nTimeOut,
            string strDataTx)
        {
            LineTrafficControlEventHandler($"     SET CLASS-{Convert.ToInt32(sWhichData.Substring(0, 4), 16)} | OBIS-{DLMSParser.GetObis(sWhichData.Substring(4, 12))} [{DLMSParser.GetObisName(Convert.ToInt32(sWhichData.Substring(0, 4), 16).ToString(), DLMSParser.GetObis(sWhichData.Substring(4, 12)), Convert.ToInt32(sWhichData.Substring(sWhichData.Length - 2), 16).ToString())}] | Attribute-{Convert.ToInt32(sWhichData.Substring(sWhichData.Length - 2), 16)}", "Send");
            byte num1 = Convert.ToByte((int)this.bytAddMode + 8);
            DateTime now1 = DateTime.Now;
            string empty = string.Empty;
            StringBuilder stringBuilder = new StringBuilder();
            this.nRetLSH = Convert.ToByte((int)this.nRecvCntr << 5);
            this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nRetLSH | 16);
            this.nRetLSH = Convert.ToByte((int)this.nSentCntr << 1);
            this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nRetLSH | (int)this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)]);
            byte[] nPkt1 = this.nPkt;
            int index1 = (int)num1;
            byte num2 = (byte)(index1 + 1);
            nPkt1[index1] = (byte)230;//0xE6
            byte[] nPkt2 = this.nPkt;
            int index2 = (int)num2;
            byte num3 = (byte)(index2 + 1);
            nPkt2[index2] = (byte)230;//0xE6
            byte[] nPkt3 = this.nPkt;
            int index3 = (int)num3;
            byte num4 = (byte)(index3 + 1);
            nPkt3[index3] = (byte)0;//0x00
            string hex = sWhichData == "004000002B0003FF01" || sWhichData == "000F0000280003FF02" || sWhichData == "000F0000280005FF02" || sWhichData == "004000002B0003FF02" ? "C30181" + sWhichData + "00" : "C10181" + sWhichData + "00";
            if (strDataTx != "")
                hex += strDataTx.Replace(" ", "");
            this.temp = "Command  : -- >> " + hex + "\r\n";
            commandString = temp;
            if (DLMSInfo.IsLNWithCipher)
            {
                byte num5 = 48;
                byte num6;
                if (hex.StartsWith("C3"))
                {
                    byte[] nPkt4 = this.nPkt;
                    int index4 = (int)num4;
                    num6 = (byte)(index4 + 1);
                    nPkt4[index4] = (byte)211;//0xD3
                }
                else
                {
                    byte[] nPkt5 = this.nPkt;
                    int index5 = (int)num4;
                    num6 = (byte)(index5 + 1);
                    nPkt5[index5] = (byte)209;//0xD3
                }
                byte[] nPkt6 = this.nPkt;
                int index6 = (int)num6;
                byte num7 = (byte)(index6 + 1);
                nPkt6[index6] = (byte)0;//0x00
                byte[] nPkt7 = this.nPkt;
                int index7 = (int)num7;
                byte num8 = (byte)(index7 + 1);
                int num9 = (int)num5;
                nPkt7[index7] = (byte)num9;
                byte[] nPkt8 = this.nPkt;
                int index8 = (int)num8;
                byte num10 = (byte)(index8 + 1);
                nPkt8[index8] = (byte)0;//0x00
                byte[] nPkt9 = this.nPkt;
                int index9 = (int)num10;
                byte num11 = (byte)(index9 + 1);
                nPkt9[index9] = (byte)0;//0x00
                byte[] nPkt10 = this.nPkt;
                int index10 = (int)num11;
                byte num12 = (byte)(index10 + 1);
                int num13 = (int)Convert.ToByte(this.nCommandCounter / 256);
                nPkt10[index10] = (byte)num13;
                byte[] nPkt11 = this.nPkt;
                int index11 = (int)num12;
                num4 = (byte)(index11 + 1);
                int num14 = (int)Convert.ToByte(this.nCommandCounter % 256);
                nPkt11[index11] = (byte)num14;
                byte[] numArray = this.Encrypt(num5.ToString("X2") + this.nCommandCounter++.ToString("X8") + hex, this.sEncryptkeyinHEX);
                this.nPkt[(int)num4 - 6] = Convert.ToByte(numArray.Length + 5);
                for (int index12 = 0; index12 < numArray.Length; ++index12)
                    this.nPkt[(int)num4++] = numArray[index12];
            }
            else
            {
                foreach (byte num15 in StringToByteArray(hex))
                    this.nPkt[(int)num4++] = num15;
            }
            this.nPkt[2] = Convert.ToByte((int)num4 + 1);
            this.DCl.fcs(ref this.nPkt, Convert.ToByte((int)this.bytAddMode + 5), (byte)1);
            this.DCl.fcs(ref this.nPkt, Convert.ToByte((int)num4 - 1), (byte)1);
            this.nPkt[(int)num4 + 2] = (byte)126;
            byte num16 = 0;
            bool flag;
            do
            {
                this.ClearBuffer();
                flag = false;
                this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)num4 + 3));
                SendDataPrint(nPkt, Convert.ToByte((int)num4 + 3));
                DateTime now2 = DateTime.Now;
                do
                {
                    //this.ClearBuffer();
                    //this.Wait(100);
                    Application.DoEvents();
                    this.DataReceive();
                    if (this.nCounter > 2 && this.nRcvPkt[(int)this.nRcvPkt[2] + 1] == (byte)126 && (int)this.nRcvPkt[2] + 2 == this.nCounter)
                    {
                        flag = true;
                        this.FrameType();
                    }
                    if (DateTime.Now.Subtract(now2).Seconds > (int)nTimeOut && (int)num16 < (int)nTryCount)
                    {
                        ++num16;
                        break;
                    }
                }
                while (!flag);
            }
            while (!flag && (int)num16 != (int)nTryCount);
            this.temp = string.Empty;
            for (int index13 = 0; index13 < this.nCounter; ++index13)
                this.temp += this.nRcvPkt[index13].ToString("X2");
            this.temp += "\r\n";
            for (int index14 = (int)this.bytAddMode + 11; index14 < this.nCounter - 3; ++index14)
                stringBuilder.Append(this.nRcvPkt[index14].ToString("X2"));
            if (stringBuilder.ToString().StartsWith("D5") || stringBuilder.ToString().StartsWith("D7"))
            {
                if (stringBuilder.ToString().StartsWith("D582") || stringBuilder.ToString().StartsWith("D782"))
                    stringBuilder.Remove(0, 8);
                else
                    stringBuilder.Remove(0, 4);
                byte[] numArray = this.Decrypt(stringBuilder.ToString(), this.sEncryptkeyinHEX);
                stringBuilder.Length = 0;
                for (int index15 = 0; index15 < numArray.Length; ++index15)
                    stringBuilder.Append(numArray[index15].ToString("X2"));
                this.temp = "Response : -- >> " + stringBuilder.ToString() + "\r\n";
                responseString = temp;
            }
            RecvDataPrint(nRcvPkt, nCounter);
            responseString = $"Response : -- >> {stringBuilder.ToString()}\r\n\r\n";
            LineTrafficControlEventHandler($"     {commandString}", "Command");
            LineTrafficControlEventHandler($"     {responseString}", "Response");
            if (stringBuilder.ToString().Contains("C5018100") || stringBuilder.ToString().Contains("C701C100") || stringBuilder.ToString().Contains("C7018100"))
                return 0;
            return stringBuilder.ToString().Contains("C501810103") ? 2 : 1;
        }

        #region Activity Set

        public int ActivityCalSetParameter(
          byte nClassID,
          byte nAttribID,
          string sOBISCode,
          byte nWait,
          byte nTryCount,
          byte nTimeOut,
          string sDataPkt)
        {
            LineTrafficControlEventHandler($"     SET CLASS-{Convert.ToInt32(nClassID)} | OBIS-{DLMSParser.GetObis(sOBISCode)} [{DLMSParser.GetObisName(Convert.ToInt32(nClassID).ToString(), DLMSParser.GetObis(sOBISCode), Convert.ToInt32(nAttribID).ToString())}] | Attribute-{Convert.ToInt32(nAttribID)}", "Send");
            bool flag = false;
            byte num1 = Convert.ToByte((int)this.bytAddMode + 8);
            string empty = string.Empty;
            DateTime now1 = DateTime.Now;
            byte[] nPkt1 = this.nPkt;
            int index1 = (int)num1;
            byte num2 = (byte)(index1 + 1);
            nPkt1[index1] = (byte)230;
            byte[] nPkt2 = this.nPkt;
            int index2 = (int)num2;
            byte num3 = (byte)(index2 + 1);
            nPkt2[index2] = (byte)230;
            byte[] nPkt3 = this.nPkt;
            int index3 = (int)num3;
            byte num4 = (byte)(index3 + 1);
            nPkt3[index3] = (byte)0;
            byte num5;
            if (sOBISCode == "0000280003FF" || sOBISCode == "0100636200FF" || sOBISCode == "0100636201FF" || sOBISCode == "0100636204FF" || sOBISCode == "0100608016FF" || sOBISCode == "0100620100FF")
            {
                byte[] nPkt4 = this.nPkt;
                int index4 = (int)num4;
                num5 = (byte)(index4 + 1);
                nPkt4[index4] = (byte)195;
            }
            else
            {
                byte[] nPkt5 = this.nPkt;
                int index5 = (int)num4;
                num5 = (byte)(index5 + 1);
                nPkt5[index5] = (byte)193;
            }
            byte[] nPkt6 = this.nPkt;
            int index6 = (int)num5;
            byte num6 = (byte)(index6 + 1);
            nPkt6[index6] = (byte)1;
            byte[] nPkt7 = this.nPkt;
            int index7 = (int)num6;
            byte num7 = (byte)(index7 + 1);
            nPkt7[index7] = (byte)129;
            byte[] nPkt8 = this.nPkt;
            int index8 = (int)num7;
            byte num8 = (byte)(index8 + 1);
            nPkt8[index8] = (byte)0;
            byte[] nPkt9 = this.nPkt;
            int index9 = (int)num8;
            byte num9 = (byte)(index9 + 1);
            int num10 = (int)nClassID;
            nPkt9[index9] = (byte)num10;
            for (byte index10 = 0; index10 < (byte)6; ++index10)
                this.nPkt[(int)num9++] = Convert.ToByte(sOBISCode.Substring((int)index10 * 2, 2), 16);
            byte[] nPkt10 = this.nPkt;
            int index11 = (int)num9;
            byte num11 = (byte)(index11 + 1);
            int num12 = (int)nAttribID;
            nPkt10[index11] = (byte)num12;
            byte num13;
            if (sDataPkt == string.Empty)
            {
                byte[] nPkt11 = this.nPkt;
                int index12 = (int)num11;
                num13 = (byte)(index12 + 1);
                nPkt11[index12] = (byte)0;//0x00
            }
            else
            {
                byte[] nPkt12 = this.nPkt;
                int index13 = (int)num11;
                num13 = (byte)(index13 + 1);
                nPkt12[index13] = (byte)1;//0x01
            }
            for (int index14 = 0; index14 <= sDataPkt.Length / 169; ++index14)
            {
                string str;
                if (sDataPkt.Length > (index14 + 1) * 168)
                {
                    str = sDataPkt.Substring(index14 * 168, 168);
                    this.nPkt[1] = (byte)168;//0xA8
                }
                else
                {
                    str = sDataPkt.Substring(index14 * 168);
                    this.nPkt[1] = (byte)160;//0xA0
                }
                byte num14 = index14 != 0 ? Convert.ToByte((int)this.bytAddMode + 8) : Convert.ToByte((int)this.bytAddMode + 24);
                this.nRetLSH = Convert.ToByte((int)this.nRecvCntr << 5);
                this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nRetLSH | 16);
                this.nRetLSH = Convert.ToByte((int)this.nSentCntr << 1);
                this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nRetLSH | (int)this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)]);
                for (byte index15 = 0; (int)index15 < str.Length / 2; ++index15)
                    this.nPkt[(int)num14++] = Convert.ToByte(str.Substring((int)index15 * 2, 2), 16);
                this.DCl.fcs(ref this.nPkt, Convert.ToByte((int)this.bytAddMode + 5), (byte)1);
                this.nPkt[2] = Convert.ToByte((int)num14 + 1);
                this.DCl.fcs(ref this.nPkt, Convert.ToByte((int)num14 - 1), (byte)1);
                this.nPkt[(int)num14 + 2] = (byte)126;
                this.ClearBuffer();
                byte num15 = 0;
                do
                {
                    flag = false;
                    this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)num14 + 3));
                    SendDataPrint(nPkt, Convert.ToByte((int)num14 + 3));
                    commandString = "Command  : -- >> " + $"{str.Trim()}\r\n";
                    LineTrafficControlEventHandler($"     {commandString}", "Command");
                    DateTime now2 = DateTime.Now;
                    do
                    {
                        Application.DoEvents();
                        this.DataReceive();
                        if (this.nCounter > 2 && this.nRcvPkt[(int)this.nRcvPkt[2] + 1] == (byte)126 && (int)this.nRcvPkt[2] + 2 == this.nCounter)
                        {
                            flag = true;
                            this.FrameType();
                        }
                        if (DateTime.Now.Subtract(now2).Seconds > (int)nTimeOut && (int)num15 < (int)nTryCount)
                        {
                            ++num15;
                            break;
                        }
                    }
                    while (!flag);
                }
                while (!flag && (int)num15 != (int)nTryCount);
                this.temp = string.Empty;
                for (byte index16 = 0; (int)index16 < this.nCounter; ++index16)
                    this.temp += this.nRcvPkt[(int)index16].ToString("X2");
                this.temp += "\r\n";
                responseString = temp;
                Application.DoEvents();
                if (!flag)
                    break;
            }
            RecvDataPrint(nRcvPkt, nCounter);
            responseString = $"Response : -- >> {responseString}\r\n\r\n";
            LineTrafficControlEventHandler($"     {responseString}", "Response");
            commandString = "Command  : -- >> " + $"{sDataPkt.Trim()}\r\n";
            if (flag && this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 14)] == (byte)0)
                return 0;
            return flag && this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 14)] == (byte)1 && this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 15)] == (byte)3 ? 2 : 1;
        }

        #endregion
        //This is for the Meter Configuration Form
        public int SetParameter(
          byte nClassID,
          byte nAttribID,
          string sOBISCode,
          byte nWait,
          byte nTryCount,
          byte nTimeOut,
          byte[] data)
        {
            byte num1 = Convert.ToByte((int)this.bytAddMode + 8);
            DateTime now1 = DateTime.Now;
            this.nRetLSH = Convert.ToByte((int)this.nRecvCntr << 5);
            this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nRetLSH | 16);
            this.nRetLSH = Convert.ToByte((int)this.nSentCntr << 1);
            this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nRetLSH | (int)this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)]);
            byte[] nPkt1 = this.nPkt;
            int index1 = (int)num1;
            byte num2 = (byte)(index1 + 1);
            nPkt1[index1] = (byte)230;
            byte[] nPkt2 = this.nPkt;
            int index2 = (int)num2;
            byte num3 = (byte)(index2 + 1);
            nPkt2[index2] = (byte)230;
            byte[] nPkt3 = this.nPkt;
            int index3 = (int)num3;
            byte num4 = (byte)(index3 + 1);
            nPkt3[index3] = (byte)0;
            byte[] nPkt4 = this.nPkt;
            int index4 = (int)num4;
            byte num5 = (byte)(index4 + 1);
            nPkt4[index4] = (byte)193;
            byte[] nPkt5 = this.nPkt;
            int index5 = (int)num5;
            byte num6 = (byte)(index5 + 1);
            nPkt5[index5] = (byte)1;
            byte[] nPkt6 = this.nPkt;
            int index6 = (int)num6;
            byte num7 = (byte)(index6 + 1);
            nPkt6[index6] = (byte)129;
            byte[] nPkt7 = this.nPkt;
            int index7 = (int)num7;
            byte num8 = (byte)(index7 + 1);
            nPkt7[index7] = (byte)0;
            byte[] nPkt8 = this.nPkt;
            int index8 = (int)num8;
            byte num9 = (byte)(index8 + 1);
            int num10 = (int)nClassID;
            nPkt8[index8] = (byte)num10;
            for (int index9 = 0; index9 < 6; ++index9)
                this.nPkt[(int)num9++] = Convert.ToByte(sOBISCode.Substring(index9 * 2, 2), 16);
            byte[] nPkt9 = this.nPkt;
            int index10 = (int)num9;
            byte num11 = (byte)(index10 + 1);
            int num12 = (int)nAttribID;
            nPkt9[index10] = (byte)num12;
            byte[] nPkt10 = this.nPkt;
            int index11 = (int)num11;
            byte num13 = (byte)(index11 + 1);
            nPkt10[index11] = (byte)0;
            foreach (byte num14 in data)
                this.nPkt[(int)num13++] = num14;
            this.nPkt[2] = Convert.ToByte((int)num13 + 1);
            this.DCl.fcs(ref this.nPkt, Convert.ToByte((int)this.bytAddMode + 5), (byte)1);
            this.DCl.fcs(ref this.nPkt, Convert.ToByte((int)num13 - 1), (byte)1);
            this.nPkt[(int)num13 + 2] = (byte)126;
            byte num15 = 0;
            bool flag;
            do
            {
                this.ClearBuffer();
                flag = false;
                this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)num13 + 3));
                DateTime now2 = DateTime.Now;
                do
                {
                    //this.ClearBuffer();
                    //this.Wait(100);
                    Application.DoEvents();
                    this.DataReceive();
                    if (this.nCounter > 2 && this.nRcvPkt[(int)this.nRcvPkt[2] + 1] == (byte)126 && (int)this.nRcvPkt[2] + 2 == this.nCounter)
                    {
                        flag = true;
                        this.FrameType();
                    }
                    if (DateTime.Now.Subtract(now2).Seconds > (int)nTimeOut && (int)num15 < (int)nTryCount)
                    {
                        ++num15;
                        break;
                    }
                }
                while (!flag);
            }
            while (!flag && (int)num15 != (int)nTryCount);
            this.temp = string.Empty;
            for (int index12 = 0; index12 < this.nCounter; ++index12)
                this.temp += this.nRcvPkt[index12].ToString("X2");
            this.temp += "\r\n";
            if (flag && this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 14)] == (byte)0)
                return 0;
            return flag && this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 14)] == (byte)1 && this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 15)] == (byte)3 ? 2 : 1;
        }

        //This is for the Meter Configuration Form
        public int SetParameter(
          byte nClassID,
          byte nAttribID,
          string sOBISCode,
          byte nWait,
          byte nTryCount,
          byte nTimeOut,
          string sDataPkt,
          bool IsLineTrafficEnabled = true)
        {
            string str1 = nClassID.ToString("X4") + sOBISCode + nAttribID.ToString("X2");
            LineTrafficControlEventHandler($"     SET CLASS-{Convert.ToInt32(str1.Substring(0, 4), 16)} | OBIS-{DLMSParser.GetObis(str1.Substring(4, 12))} [{DLMSParser.GetObisName(Convert.ToInt32(str1.Substring(0, 4), 16).ToString(), DLMSParser.GetObis(str1.Substring(4, 12)), Convert.ToInt32(str1.Substring(str1.Length - 2), 16).ToString())}] | Attribute-{Convert.ToInt32(str1.Substring(str1.Length - 2), 16)}", "Send");
            StringBuilder stringBuilder = new StringBuilder();
            bool flag = false;
            byte num1 = Convert.ToByte((int)this.bytAddMode + 8);
            string empty = string.Empty;
            DateTime now1 = DateTime.Now;
            byte[] nPkt1 = this.nPkt;
            int index1 = (int)num1;
            byte num2 = (byte)(index1 + 1);
            nPkt1[index1] = (byte)230;
            byte[] nPkt2 = this.nPkt;
            int index2 = (int)num2;
            byte num3 = (byte)(index2 + 1);
            nPkt2[index2] = (byte)230;
            byte[] nPkt3 = this.nPkt;
            int index3 = (int)num3;
            byte num4 = (byte)(index3 + 1);
            nPkt3[index3] = (byte)0;
            byte num5;
            if (sOBISCode == "0000280003FF" || sOBISCode == "0100636200FF" || sOBISCode == "0100636201FF" || sOBISCode == "0100636204FF" || sOBISCode == "0100608016FF" || sOBISCode == "0100620100FF")
            {
                byte[] nPkt4 = this.nPkt;
                int index4 = (int)num4;
                num5 = (byte)(index4 + 1);
                nPkt4[index4] = (byte)195;
            }
            else
            {
                byte[] nPkt5 = this.nPkt;
                int index5 = (int)num4;
                num5 = (byte)(index5 + 1);
                nPkt5[index5] = (byte)193;
            }
            byte[] nPkt6 = this.nPkt;
            int index6 = (int)num5;
            byte num6 = (byte)(index6 + 1);
            nPkt6[index6] = (byte)1;
            byte[] nPkt7 = this.nPkt;
            int index7 = (int)num6;
            byte num7 = (byte)(index7 + 1);
            nPkt7[index7] = (byte)129;
            byte[] nPkt8 = this.nPkt;
            int index8 = (int)num7;
            byte num8 = (byte)(index8 + 1);
            nPkt8[index8] = (byte)0;
            byte[] nPkt9 = this.nPkt;
            int index9 = (int)num8;
            byte num9 = (byte)(index9 + 1);
            int num10 = (int)nClassID;
            nPkt9[index9] = (byte)num10;
            for (byte index10 = 0; index10 < (byte)6; ++index10)
                this.nPkt[(int)num9++] = Convert.ToByte(sOBISCode.Substring((int)index10 * 2, 2), 16);
            byte[] nPkt10 = this.nPkt;
            int index11 = (int)num9;
            byte num11 = (byte)(index11 + 1);
            int num12 = (int)nAttribID;
            nPkt10[index11] = (byte)num12;
            byte num13;
            if (sDataPkt == string.Empty)
            {
                byte[] nPkt11 = this.nPkt;
                int index12 = (int)num11;
                num13 = (byte)(index12 + 1);
                nPkt11[index12] = (byte)0;
            }
            else
            {
                byte[] nPkt12 = this.nPkt;
                int index13 = (int)num11;
                num13 = (byte)(index13 + 1);
                nPkt12[index13] = (byte)1;
            }
            for (int index14 = 0; index14 <= sDataPkt.Length / 169; ++index14)
            {
                string str;
                if (sDataPkt.Length > (index14 + 1) * 168)
                {
                    str = sDataPkt.Substring(index14 * 168, 168);
                    this.nPkt[1] = (byte)168;
                }
                else
                {
                    str = sDataPkt.Substring(index14 * 168);
                    this.nPkt[1] = (byte)160;
                }
                byte num14 = index14 != 0 ? Convert.ToByte((int)this.bytAddMode + 8) : Convert.ToByte((int)this.bytAddMode + 24);
                this.nRetLSH = Convert.ToByte((int)this.nRecvCntr << 5);
                this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nRetLSH | 16);
                this.nRetLSH = Convert.ToByte((int)this.nSentCntr << 1);
                this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nRetLSH | (int)this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)]);
                for (byte index15 = 0; (int)index15 < str.Length / 2; ++index15)
                    this.nPkt[(int)num14++] = Convert.ToByte(str.Substring((int)index15 * 2, 2), 16);
                this.DCl.fcs(ref this.nPkt, Convert.ToByte((int)this.bytAddMode + 5), (byte)1);
                this.nPkt[2] = Convert.ToByte((int)num14 + 1);
                this.DCl.fcs(ref this.nPkt, Convert.ToByte((int)num14 - 1), (byte)1);
                this.nPkt[(int)num14 + 2] = (byte)126;
                this.ClearBuffer();
                byte num15 = 0;
                commandString = "";
                for (int index17 = (int)this.bytAddMode + 11; index17 < (int)num14; ++index17)
                    stringBuilder.Append(this.nPkt[index17].ToString("X2"));
                commandString = $"Command  : -- >> {stringBuilder.ToString()}";
                do
                {
                    flag = false;
                    this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)num14 + 3));
                    SendDataPrint(nPkt, Convert.ToByte((int)num14 + 3));
                    DateTime now2 = DateTime.Now;
                    do
                    {
                        //this.ClearBuffer();
                        //this.Wait(100);
                        Application.DoEvents();
                        this.DataReceive();
                        if (this.nCounter > 2 && this.nRcvPkt[(int)this.nRcvPkt[2] + 1] == (byte)126 && (int)this.nRcvPkt[2] + 2 == this.nCounter)
                        {
                            flag = true;
                            this.FrameType();
                        }
                        if (DateTime.Now.Subtract(now2).Seconds > (int)nTimeOut && (int)num15 < (int)nTryCount)
                        {
                            ++num15;
                            break;
                        }
                    }
                    while (!flag);
                }
                while (!flag && (int)num15 != (int)nTryCount);
                this.temp = string.Empty;
                for (byte index16 = 0; (int)index16 < this.nCounter; ++index16)
                    this.temp += this.nRcvPkt[(int)index16].ToString("X2") + " ";
                stringBuilder.Length = 0;
                for (int index17 = (int)this.bytAddMode + 11; index17 < this.nCounter - 3; ++index17)
                    stringBuilder.Append(this.nRcvPkt[index17].ToString("X2"));
                if (string.IsNullOrEmpty(temp))
                    temp = "(R)" + "  " + "NULL" + " " + DateTime.Now.ToString(Constants.timeStampFormate) + Environment.NewLine;
                else
                    temp = "(R)" + "  " + temp + " " + DateTime.Now.ToString(Constants.timeStampFormate) + Environment.NewLine;
                if (IsLineTrafficEnabled)
                    LineTrafficControlEventHandler(temp, "Receive");
                responseString = "";
                responseString = $"Response : -- >> {stringBuilder.ToString()}\r\n\r\n";
                LineTrafficControlEventHandler($"     {commandString}", "Command");
                LineTrafficControlEventHandler($"     {responseString}", "Response");
                Application.DoEvents();
                if (!flag)
                    break;
            }
            if (flag && this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 14)] == (byte)0)
                return 0;
            return flag && this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 14)] == (byte)1 && this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 15)] == (byte)3 ? 2 : 1;
        }

        public string SetValue(Int32 _class, string _obis, Int32 _attribute, string dataToSet = null, string _deviationByte = null)
        {
            string sMessage = string.Empty;
            int nRetVal = 100;
            string objectName = string.Empty;
            string sData = string.Empty;
            if (_obis.Contains("TOD") || _obis.Contains("STOD"))
                objectName = $"{_class.ToString("X4")}{string.Concat(_obis.Split('-')[0].Split('.').Select(part => int.Parse(part).ToString("X2")))}{_attribute.ToString("X2")}";
            else
                objectName = $"{_class.ToString("X4")}{string.Concat(_obis.Split('.').Select(part => int.Parse(part).ToString("X2")))}{_attribute.ToString("X2")}";
            try
            {
                switch (_obis)
                {
                    //RTC - Date and Time   0.0.1.0.0.255
                    case "0.0.1.0.0.255":
                        nRetVal = SetParameter(objectName, nWait, nTryCount, nTimeOut, dataToSet);
                        if (nRetVal == 0)
                            sMessage = sMessage + "RTC Set Successfully";
                        else if (nRetVal == 2)
                            sMessage = sMessage + "RTC Write Denied";
                        else if (nRetVal == 1 || nRetVal == 3)
                            sMessage = sMessage + "RTC Error in Setting";
                        break;
                    //Demand integration period   1.0.0.8.0.255
                    case "1.0.0.8.0.255":
                        nRetVal = SetParameter(objectName, nWait, nTryCount, nTimeOut, dataToSet);
                        if (nRetVal == 0)
                            sMessage = sMessage + "Demand Int Period: Set Successfully";
                        else if (nRetVal == 2)
                            sMessage = sMessage + "Demand Int Period: Write Denied";
                        else if (nRetVal == 1 || nRetVal == 3)
                            sMessage = sMessage + "Demand Int Period: Error in Setting";
                        break;
                    //Profile capture period  1.0.0.8.4.255
                    case "1.0.0.8.4.255":
                        nRetVal = SetParameter(objectName, nWait, nTryCount, nTimeOut, dataToSet);
                        if (nRetVal == 0)
                            sMessage = sMessage + "Profile Capture Period: Set Successfully";
                        else if (nRetVal == 2)
                            sMessage = sMessage + "Profile Capture Period: Write Denied";
                        else if (nRetVal == 1 || nRetVal == 3)
                            sMessage = sMessage + "Profile Capture Period: Error in Setting";
                        break;
                    //Metering mode   0.0.94.96.19.255
                    case "0.0.94.96.19.255":
                        nRetVal = SetParameter(objectName, nWait, nTryCount, nTimeOut, dataToSet);
                        if (nRetVal == 0)
                            sMessage = sMessage + "Metering Mode: Set Successfully";
                        else if (nRetVal == 2)
                            sMessage = sMessage + "Metering Mode: Write Denied";
                        else if (nRetVal == 1 || nRetVal == 3)
                            sMessage = sMessage + "Metering Mode: Error in Setting";
                        break;
                    //Payment mode    0.0.94.96.20.255
                    case "0.0.94.96.20.255":
                        nRetVal = SetParameter(objectName, nWait, nTryCount, nTimeOut, dataToSet);
                        if (nRetVal == 0)
                            sMessage = sMessage + "Payment Mode: Set Successfully";
                        else if (nRetVal == 2)
                            sMessage = sMessage + "Payment Mode: Action Denied";
                        else if (nRetVal == 1 || nRetVal == 3)
                            sMessage = sMessage + "Payment Mode: Error in Setting";
                        break;
                    //Last token recharge amount  0.0.94.96.21.255
                    case "0.0.94.96.21.255":
                        nRetVal = SetParameter(objectName, nWait, nTryCount, nTimeOut, dataToSet);
                        if (nRetVal == 0)
                            sMessage = sMessage + "Last Token Recharge Amount: Set Successfully";
                        else if (nRetVal == 2)
                            sMessage = sMessage + "Last Token Recharge Amount: Action Denied";
                        else if (nRetVal == 1 || nRetVal == 3)
                            sMessage = sMessage + "Last Token Recharge Amount: Error in Setting";
                        break;
                    //Last token recharge time    0.0.94.96.22.255
                    case "0.0.94.96.22.255":
                        nRetVal = SetParameter(objectName, nWait, nTryCount, nTimeOut, dataToSet);
                        if (nRetVal == 0)
                            sMessage = sMessage + "Last Token Recharge Time: Set Successfully";
                        else if (nRetVal == 2)
                            sMessage = sMessage + "Last Token Recharge Time: Write Denied";
                        else if (nRetVal == 1 || nRetVal == 3)
                            sMessage = sMessage + "Last Token Recharge Time: Error in Setting";
                        break;
                    //Total amount at last recharge   0.0.94.96.23.255
                    case "0.0.94.96.23.255":
                        nRetVal = SetParameter(objectName, nWait, nTryCount, nTimeOut, dataToSet);
                        if (nRetVal == 0)
                            sMessage = sMessage + "Total Amount at Last Recharge: Set Successfully";
                        else if (nRetVal == 2)
                            sMessage = sMessage + "Total Amount at Last Recharge: Action Denied";
                        else if (nRetVal == 1 || nRetVal == 3)
                            sMessage = sMessage + "Total Amount at Last Recharge: Error in Setting";
                        break;
                    //Current balance amount  0.0.94.96.24.255
                    case "0.0.94.96.24.255":
                        nRetVal = SetParameter(objectName, nWait, nTryCount, nTimeOut, dataToSet);
                        if (nRetVal == 0)
                            sMessage = sMessage + "Current Balance Amount: Set Successfully";
                        else if (nRetVal == 2)
                            sMessage = sMessage + "Current Balance Amount: Action Denied";
                        else if (nRetVal == 1 || nRetVal == 3)
                            sMessage = sMessage + "Current Balance Amount: Error in Setting";
                        break;
                    //Current balance time    0.0.94.96.25.255
                    case "0.0.94.96.25.255":
                        nRetVal = SetParameter(objectName, nWait, nTryCount, nTimeOut, dataToSet);
                        if (nRetVal == 0)
                            sMessage = sMessage + "Current Balance Time: Set Successfully";
                        else if (nRetVal == 2)
                            sMessage = sMessage + "Current Balance Time: Write Denied";
                        else if (nRetVal == 1 || nRetVal == 3)
                            sMessage = sMessage + "Current Balance Time: Error in Setting";
                        break;
                    //LCD Display  Auto Parameters	1.0.96.128.0.255	1/2	Octet string [For example, if user want to set parameter no 3,4,5,6 in auto mode then data will be 09 04 03 04 05 06.]
                    case "1.0.96.128.0.255":
                        nRetVal = SetParameter(objectName, nWait, nTryCount, nTimeOut, dataToSet);
                        if (nRetVal == 0)
                            sMessage = sMessage + "LCD Display Auto Parameters: Set Successfully";
                        else if (nRetVal == 2)
                            sMessage = sMessage + "LCD Display Auto Parameters: Action Denied";
                        else if (nRetVal == 1 || nRetVal == 3)
                            sMessage = sMessage + "LCD Display Auto Parameters: Error in Setting";

                        break;
                    //LCD Display  Push Parameters	1.0.96.128.1.255	1/2	Octet string [For example, if user want to set parameter no 3,4,5,6 in auto mode then data will be 09 04 03 04 05 06.]
                    case "1.0.96.128.1.255":
                        nRetVal = SetParameter(objectName, nWait, nTryCount, nTimeOut, dataToSet);
                        if (nRetVal == 0)
                            sMessage = sMessage + "LCD Display Push Parameters: Set Successfully";
                        else if (nRetVal == 2)
                            sMessage = sMessage + "LCD Display Push Parameters: Action Denied";
                        else if (nRetVal == 1 || nRetVal == 3)
                            sMessage = sMessage + "LCD Display Push Parameters: Error in Setting";
                        break;
                    //Calculation LED Configuration (1.0.96.128.3.255)
                    case "1.0.96.128.3.255":
                        nRetVal = SetParameter(objectName, nWait, nTryCount, nTimeOut, dataToSet);
                        if (nRetVal == 0)
                            sMessage = sMessage + "Calculation LED Configuration: Set Successfully";
                        else if (nRetVal == 2)
                            sMessage = sMessage + "Calculation LED Configuration: Write Denied";
                        else if (nRetVal == 1 || nRetVal == 3)
                            sMessage = sMessage + "Calculation LED Configuration: Error in Setting";

                        break;
                    //App. Energy Calculation Method (1.0.0.11.6.255)
                    case "1.0.0.11.6.255":
                        nRetVal = SetParameter(objectName, nWait, nTryCount, nTimeOut, dataToSet);
                        string methodString;
                        methodString = int.Parse(dataToSet.Substring(2, 2)) == 0 ? "Lag only" : "Lag + Lead";
                        if (nRetVal == 0)
                            sMessage = sMessage + $"Apparent Calculation ({methodString}) Method: Set Successfully";
                        else if (nRetVal == 2)
                            sMessage = sMessage + $"Apparent Calculation ({methodString}) Method: Write Denied";
                        else if (nRetVal == 1 || nRetVal == 3)
                            sMessage = sMessage + $"Apparent Calculation ({methodString}) Method: Error in Setting";
                        break;
                    //MD Reset (0.0.10.0.1.255)
                    case "0.0.10.0.1.255":
                        nRetVal = ActionCmd("000900000A0001FF01", nWait, nTryCount, nTimeOut, dataToSet);//OLD
                        if (nRetVal == 1)
                            sMessage = sMessage + "MD Reset Error in Write Data";
                        else if (nRetVal == 2)
                            sMessage = sMessage + "MD Reset Write Access Denied";
                        else
                            sMessage = sMessage + "MD Reset Set Successfully";
                        break;
                    ////Single - action schedule for billing dates    0.0.15.0.0.255
                    //case "":
                    //    break;
                    ////Load limit(W)  0.0.17.0.0.255
                    //case "":
                    //    break;
                    ////Activity calander for time zones    0.0.13.0.0.255
                    //case "":
                    //    break;
                    ////Enable / disable load limit function  0.0.96.3.10.255
                    //case "":
                    //    break;
                    //Relay connect / disconnect    0.0.96.3.10.255
                    case "0.0.96.3.10.255":
                        if (_attribute == 1) // 0 means Disconnect relay 
                        {
                            //nRetVal = SetParameter("00070100620100FF02", nWait, nTryCount, nTimeOut, string.Empty);
                            nRetVal = ActionCmd("0046000060030AFF01", nWait, nTryCount, nTimeOut, string.Empty);
                            if (nRetVal == 0)
                                sMessage = sMessage + "Relay Operation: Relay Disconnected Successfully";
                            else if (nRetVal == 2)
                                sMessage = sMessage + "Relay Operation: Write Denied";
                            else if (nRetVal == 1 || nRetVal == 3)
                                sMessage = sMessage + "Relay Operation: Error in Setting";
                        }
                        else if (_attribute == 2) // 1 means Connect relay
                        {
                            //nRetVal = SetParameter("00070100620100FF02", nWait, nTryCount, nTimeOut, string.Empty);
                            nRetVal = ActionCmd("0046000060030AFF02", nWait, nTryCount, nTimeOut, string.Empty);
                            if (nRetVal == 0)
                                sMessage = sMessage + "Relay Operation: Relay Connected Successfully";
                            else if (nRetVal == 2)
                                sMessage = sMessage + "Relay Operation: Write Denied";
                            else if (nRetVal == 1 || nRetVal == 3)
                                sMessage = sMessage + "Relay Operation: Error in Setting";
                        }
                        break;
                    //Billing Dates (0.0.15.0.0.255)
                    case "0.0.15.0.0.255":
                        string billingdate = dataToSet.Split('-')[0].Trim();
                        string CmbBill = billingdate;
                        DateTime billingTime = DateTime.ParseExact(dataToSet.Split('-')[1].Trim(), "HH:mm", null);
                        int? cmbBillingCycle = GetIndexOfBillingCycle(dataToSet.Split('-')[2].Trim());
                        dataToSet = "0101090CFFFFFF" + Convert.ToInt32(CmbBill).ToString("X2") + "FF";
                        dataToSet += Convert.ToByte(billingTime.Hour).ToString("X2") + Convert.ToByte(billingTime.Minute).ToString("X2") + "0000" + _deviationByte + "00";
                        nRetVal = SetParameter("001600000F0000FF04", nWait, nTryCount, nTimeOut, dataToSet);
                        if (nRetVal == 0)
                            sMessage = sMessage + "Billing Date: Set Successfully";
                        else if (nRetVal == 2)
                            sMessage = sMessage + "Billing Date: Write Denied";
                        else if (nRetVal == 1 || nRetVal == 3)
                        {
                            nRetVal = SetParameter("001600000F0000FF04", nWait, nTryCount, nTimeOut, dataToSet.Substring(4));
                            if (nRetVal == 0)
                                sMessage = sMessage + "Billing Date: Set Successfully";
                            else if (nRetVal == 2)
                                sMessage = sMessage + "Billing Date: Write Denied";
                            else if (nRetVal == 1 || nRetVal == 3)
                            {
                                dataToSet = "010102020904" + Convert.ToByte(billingTime.Hour).ToString("X2") + Convert.ToByte(billingTime.Minute).ToString("X2") + "0000";
                                dataToSet += "0905FFFFFF" + Convert.ToInt32(CmbBill).ToString("X2") + "FF";
                                nRetVal = SetParameter("001600000F0000FF04", nWait, nTryCount, nTimeOut, dataToSet);
                                if (nRetVal == 0)
                                    sMessage = sMessage + "Billing Date: Set Successfully";
                                else if (nRetVal == 2)
                                    sMessage = sMessage + "Billing Date: Write Denied";
                                else if (nRetVal == 1 || nRetVal == 3)
                                {
                                    nRetVal = SetParameter("001600000F0000FF04", nWait, nTryCount, nTimeOut, dataToSet.Substring(4));
                                    if (nRetVal == 0)
                                        sMessage = sMessage + "Billing Date: Set Successfully";
                                    else if (nRetVal == 2)
                                        sMessage = sMessage + "Billing Date: Write Denied";
                                    else if (nRetVal == 1 || nRetVal == 3)
                                        sMessage = sMessage + "Billing Date: Error in Setting";
                                }
                            }
                        }

                        nRetVal = SetParameter("00010100000806FF02", nWait, nTryCount, nTimeOut, "11" + Convert.ToInt32(cmbBillingCycle).ToString("X2"));
                        if (nRetVal == 0)
                            sMessage = sMessage + " | Billing cycle Set Successfully";
                        else if (nRetVal == 2)
                            sMessage = sMessage + " | Billing Cycle: Action Denied";
                        else if (nRetVal == 1 || nRetVal == 3)
                            sMessage = sMessage + " | Billing Cycle: Error in Setting";
                        break;
                    //Load Limit Values (0.0.17.0.0.255)
                    case "0.0.17.0.0.255":
                        int? cmbLLT_SelectedIndex = GetIndexOfLoadlimit(dataToSet.Split('-')[0]);
                        string loadLimitValue = dataToSet.Split('-')[1];
                        string loadLimitMinimalOver = dataToSet.Split('-')[2];
                        string loadLimitMinimalUnder = dataToSet.Split('-')[3];
                        if (cmbLLT_SelectedIndex == 0) // 0 to disable Load Limit Setting
                        {
                            nRetVal = SetParameter("0046000060030AFF04", nWait, nTryCount, nTimeOut, "16" + Convert.ToInt32(cmbLLT_SelectedIndex).ToString("X2"));
                            if (nRetVal == 0)
                                sMessage = sMessage + "Load Limit Function Disabled! Relay will be Connected Always";
                            else if (nRetVal == 2)
                                sMessage = sMessage + "Load Limit: Action Denied";
                            else if (nRetVal == 1 || nRetVal == 3)
                                sMessage = sMessage + "Load Limit: Error in Setting";
                        }
                        else if (cmbLLT_SelectedIndex == 1) // 1 to enable Load limit Setting+               
                        {
                            nRetVal = SetParameter("0046000060030AFF04", nWait, nTryCount, nTimeOut, "16" + "04");
                            if (nRetVal == 0)
                                sMessage = sMessage + "Load Limit Function Enabled!";
                            else if (nRetVal == 2)
                                sMessage = sMessage + "Load Limit: Action Denied";
                            else if (nRetVal == 1 || nRetVal == 3)
                                sMessage = sMessage + "Load Limit: Error in Setting";
                        }
                        //load limit Value
                        if (loadLimitValue != "NA")
                        {
                            nRetVal = SetParameter("00470000110000FF04", nWait, nTryCount, nTimeOut, "06" + Convert.ToInt32(loadLimitValue).ToString("X8"));
                            if (nRetVal == 0)
                                sMessage = sMessage + "\r\nLoad Limit Value Updated";
                            else if (nRetVal == 2)
                                sMessage = sMessage + "\r\nLoad Limit Value Write Denied";
                            else if (nRetVal == 1 || nRetVal == 3)
                                sMessage = sMessage + "\r\nLoad Limit Value Error in Setting";
                        }
                        if (loadLimitMinimalOver != "NA")
                        {
                            nRetVal = SetParameter("00470000110000FF06", nWait, nTryCount, nTimeOut, "06" + Convert.ToInt32(loadLimitMinimalOver).ToString("X8"));
                            if (nRetVal == 0)
                                sMessage = sMessage + "\r\nLoad Limit Occurence Time Updated";
                            else if (nRetVal == 2)
                                sMessage = sMessage + "\r\nLoad Limit Minimal Over Value Write Denied";
                            else if (nRetVal == 1 || nRetVal == 3)
                                sMessage = sMessage + "\r\nLoad Limit Minimal Over Value Error in Setting";
                        }
                        if (loadLimitMinimalUnder != "NA")
                        {
                            nRetVal = SetParameter("00470000110000FF07", nWait, nTryCount, nTimeOut, "06" + Convert.ToInt32(loadLimitMinimalUnder).ToString("X8"));
                            if (nRetVal == 0)
                                sMessage = sMessage + "\r\nLoad Limit Restoration Time Updated";
                            else if (nRetVal == 2)
                                sMessage = sMessage + "\r\nLoad Limit Minimal Under Value Write Denied";
                            else if (nRetVal == 1 || nRetVal == 3)
                                sMessage = sMessage + "\r\nLoad Limit Minimal Under Value Error in Setting";
                        }
                        break;
                    //Passive Relay Time
                    case "1.0.96.128.30.255":
                        nRetVal = SetParameter(objectName, nWait, nTryCount, nTimeOut, dataToSet);
                        if (nRetVal == 0)
                            sMessage = sMessage + "Passive Relay Time: Set Successfully";
                        else if (nRetVal == 2)
                            sMessage = sMessage + "Passive Relay Time: Write Denied";
                        else if (nRetVal == 1 || nRetVal == 3)
                            sMessage = sMessage + "Passive Relay Time: Error in Setting";
                        break;
                    //Gprs Setup APN 
                    case "0.0.25.4.0.255":
                        nRetVal = SetParameter(objectName, nWait, nTryCount, nTimeOut, dataToSet);
                        if (nRetVal == 0)
                            sMessage = sMessage + "Gprs Setup: APN Set Successfully";
                        else if (nRetVal == 2)
                            sMessage = sMessage + "Gprs Setup: APN Action Denied";
                        else if (nRetVal == 1 || nRetVal == 3)
                            sMessage = sMessage + "Gprs Setup: APN Error in Setting";
                        break;
                    //TOD 0.0.13.0.0.255
                    case "0.0.13.0.0.255-TOD":
                        string _dayofweek = dataToSet.Substring(dataToSet.Length - 6, 2);
                        //TOU day profile table passive
                        //first set by day id "0"
                        dataToSet = dataToSet.Substring(0, dataToSet.Length - 14);
                        dataToSet = Regex.Replace(dataToSet, "010102021101", "010102021100");
                        //nRetVal = SetParameter("001400000D0000FF09", nWait, nTryCount, nTimeOut, dataToSet.Substring(0, dataToSet.Length - 14));
                        nRetVal = SetParameter("001400000D0000FF09", nWait, nTryCount, nTimeOut, dataToSet);
                        if (nRetVal == 0)
                            sMessage = sMessage + "TOU: Day Profile Set Successfully by Day ID: 0, ";
                        else if (nRetVal != 0)
                        {
                            dataToSet = Regex.Replace(dataToSet, "010102021100", "010102021101");
                            nRetVal = SetParameter("001400000D0000FF09", nWait, nTryCount, nTimeOut, dataToSet);
                            if (nRetVal == 0)
                                sMessage = sMessage + "TOU: Day Profile Set Successfully by Day ID: 1, ";
                            else if (nRetVal == 2)
                                sMessage = sMessage + "TOU: Day Profile Write Denied, ";
                            else if (nRetVal == 1 || nRetVal == 3)
                                sMessage = sMessage + "TOU: Day Profile Error in Setting, ";
                        }
                        if (nRetVal == 0)
                        {
                            //TOU season profile passive
                            nRetVal = SetParameter("001400000D0000FF07", nWait, nTryCount, nTimeOut, "010102030907475345534F4E31090CFFFF0B08FF000000FF" + _deviationByte + "000907475745454B3031");
                            if (nRetVal != 0)
                            {
                                nRetVal = SetParameter("001400000D0000FF07", nWait, nTryCount, nTimeOut, "010102030907475345534F4E31090CFFFF0B08FF000000FF" + _deviationByte + "000907475745454B3031");
                            }
                            if (nRetVal == 0)
                                sMessage = sMessage + "TOU: Season Profile Set Successfully, ";
                            else if (nRetVal == 2)
                                sMessage = sMessage + "TOU: Season Profile Write Denied, ";
                            else if (nRetVal == 1 || nRetVal == 3)
                                sMessage = sMessage + "TOU: Season Profile Error in Setting, ";
                            if (nRetVal == 0 || nRetVal == 1)
                            {
                                if (sMessage.Contains("Successfully by Day ID: 1"))
                                {
                                    //TOU week profile table passive
                                    nRetVal = SetParameter("001400000D0000FF08", nWait, nTryCount, nTimeOut, "010102080907475745454B30311101110111011101110111011101");
                                }
                                else
                                {
                                    nRetVal = SetParameter("001400000D0000FF08", nWait, nTryCount, nTimeOut, "010102080907475745454B3031 1100110011001100110011001100");
                                }
                                if (nRetVal == 0)
                                    sMessage = sMessage + "TOU: Week Profile Set Successfully, ";
                                else if (nRetVal == 2)
                                    sMessage = sMessage + "TOU: Week Profile Write Denied, ";
                                else if (nRetVal == 1 || nRetVal == 3)
                                    sMessage = sMessage + "TOU: Week Profile Error in Setting, ";
                                if (nRetVal == 0 || nRetVal == 1)
                                {
                                    //TOU calendar_name_passive [name as GENUS]
                                    nRetVal = SetParameter("001400000D0000FF06", nWait, nTryCount, nTimeOut, "090547454E5553");
                                    if (nRetVal == 0)
                                        sMessage = sMessage + "TOU: Calendar Name Set Successfully, ";
                                    else if (nRetVal == 2)
                                        sMessage = sMessage + "TOU: Calendar Name Write Denied, ";
                                    else if (nRetVal == 1 || nRetVal == 3)
                                        sMessage = sMessage + "TOU: Calendar Name Error in Setting, ";
                                    if (nRetVal == 0)
                                    {
                                        //TOU activate passive calendar time
                                        sData = string.Empty;
                                        sData = "090C" + dataToSet.Substring(dataToSet.Length - 14, 4) + dataToSet.Substring(dataToSet.Length - 10, 2) + dataToSet.Substring(dataToSet.Length - 8, 2);//+"00";// Year+Month+Day
                                        if (Convert.ToInt32(_dayofweek) == 0)
                                            sData += "07";
                                        else
                                            sData += _dayofweek;
                                        sData += dataToSet.Substring(dataToSet.Length - 4, 2) + dataToSet.Substring(dataToSet.Length - 2, 2) + "0000" + _deviationByte + "00";

                                        nRetVal = SetParameter("001400000D0000FF0A", nWait, nTryCount, nTimeOut, sData);
                                        if (nRetVal == 0)
                                            sMessage = sMessage + "TOU: Passive Calender Date and Time Set Successfully, ";
                                        else if (nRetVal == 2)
                                            sMessage = sMessage + "TOU: Passive Calender Date and Time Write Denied, ";
                                        else if (nRetVal == 1 || nRetVal == 3)
                                            sMessage = sMessage + "TOU: Passive Calender Date and Time Error in Setting ";
                                    }
                                }
                            }
                        }
                        break;
                    //STOD 0.0.13.0.0.255
                    case "0.0.13.0.0.255-STOD":
                        string ApplicableTimeString = string.Empty;
                        string sTODSpecialDayData = string.Empty;
                        string[] SeasonalTODString = Regex.Split(dataToSet, "-");
                        string[] sTODData = Regex.Split(SeasonalTODString[0], "4B4B4B4B");
                        if (SeasonalTODString[2] == "1")
                        {
                            sTODSpecialDayData = SeasonalTODString[3];
                            ApplicableTimeString = SeasonalTODString[1];
                            ApplicableTimeString = ApplicableTimeString.Replace(";", "");
                            ApplicableTimeString = ApplicableTimeString.Replace(":", "");
                            ApplicableTimeString = ApplicableTimeString.Replace(",", "");
                        }
                        else
                        {
                            ApplicableTimeString = SeasonalTODString[1];
                            ApplicableTimeString = ApplicableTimeString.Replace(";", "");
                            ApplicableTimeString = ApplicableTimeString.Replace(":", "");
                            ApplicableTimeString = ApplicableTimeString.Replace(",", "");
                        }
                        bool bChange = false;
                        if (sTODData[0].Length > 4)
                        {
                            //nRetVal = SetParameter("001400000D0000FF09", nWait, nTryCount, nTimeOut, sTODData[0]);//old
                            nRetVal = ActivityCalSetParameter((byte)20, (byte)9, "00000D0000FF", nWait, nTryCount, nTimeOut, sTODData[0]);
                            if (nRetVal == 0)
                                sMessage = sMessage + "TOU: Day Profile Set Successfully by Day ID: 0, ";
                            else if (nRetVal != 0)
                            {
                                string[] splittedTODs = Regex.Split(sTODData[0], "020211");
                                for (int i = 1; i < splittedTODs.Length; i++)
                                {
                                    splittedTODs[i] = "020211" + (int.Parse(splittedTODs[i].Substring(0, 2), NumberStyles.HexNumber) + 1).ToString("X2") + splittedTODs[i].Substring(2);
                                }
                                //nRetVal = SetParameter("001400000D0000FF09", nWait, nTryCount, nTimeOut, splittedTODs.Aggregate((current, next) => current + next));//old
                                nRetVal = ActivityCalSetParameter((byte)20, (byte)9, "00000D0000FF", nWait, nTryCount, nTimeOut, splittedTODs.Aggregate((current, next) => current + next));
                                if (nRetVal == 0)
                                    sMessage = sMessage + "TOU: Day Profile Set Successfully by Day ID: 1, ";
                                else if (nRetVal == 2)
                                    sMessage = sMessage + "TOU: Day Profile Write Denied, ";
                                else if (nRetVal == 1 || nRetVal == 3)
                                    sMessage = sMessage + "TOU: Day Profile Error in Setting, ";
                            }
                        }
                        else
                            nRetVal = 1;

                        if (nRetVal == 0)
                        {
                            sTODData[2] = sTODData[2].Replace("065345534F4E", "07475345534F4E").Replace("06475745454B", "07475745454B30");
                            nRetVal = SetParameter("001400000D0000FF07", nWait, nTryCount, nTimeOut, sTODData[2]);
                            if (nRetVal != 0)
                            {
                                nRetVal = SetParameter("001400000D0000FF07", nWait, nTryCount, nTimeOut, sTODData[2]);
                                bChange = true;
                            }
                            if (nRetVal == 0)
                                sMessage = sMessage + "TOU: Season Profile Set Successfully, ";
                            else if (nRetVal == 2)
                                sMessage = sMessage + "TOU: Season Profile Write Denied, ";
                            else if (nRetVal == 1 || nRetVal == 3)
                                sMessage = sMessage + "TOU: Season Profile Error in Setting, ";
                            if (nRetVal == 0 || nRetVal == 1)
                            {
                                string finalWeekProfileData = "";
                                sTODData[3] = sTODData[3].Replace("06475745454B", "07475745454B30");
                                if (sMessage.Contains("Successfully by Day ID: 1"))
                                {
                                    string[] splttedWeek = Regex.Split(sTODData[3], "020809");
                                    finalWeekProfileData = splttedWeek[0];
                                    for (int i = 1; i < splttedWeek.Length; i++)
                                    {
                                        string startwith = splttedWeek[i].Substring(0, splttedWeek[i].Length - 28);
                                        string endwith = splttedWeek[i].Substring(splttedWeek[i].Length - 28);
                                        string final = "";
                                        for (int j = 0; j < endwith.Length;)
                                        {
                                            j += 2;
                                            final += "11";
                                            final += (int.Parse(endwith.Substring(j, 2), NumberStyles.HexNumber) + 1).ToString("X2");
                                            j += 2;
                                        }
                                        finalWeekProfileData = finalWeekProfileData + "020809" + startwith + final;
                                    }
                                    //nRetVal = SetParameter("001400000D0000FF08", nWait, nTryCount, nTimeOut, finalWeekProfileData);//old
                                    nRetVal = ActivityCalSetParameter((byte)20, (byte)8, "00000D0000FF", nWait, nTryCount, nTimeOut, finalWeekProfileData);
                                }
                                else
                                    nRetVal = ActivityCalSetParameter((byte)20, (byte)8, "00000D0000FF", nWait, nTryCount, nTimeOut, sTODData[3]);
                                //nRetVal = SetParameter("001400000D0000FF08", nWait, nTryCount, nTimeOut, sTODData[3]);//old

                                if (nRetVal != 0)
                                {
                                    //sTODData[3] = sTODData[3].Replace("06475745454B", "07475745454B30");
                                    nRetVal = SetParameter("001400000D0000FF08", nWait, nTryCount, nTimeOut, sTODData[3]);
                                }
                                if (nRetVal == 0)
                                    sMessage = sMessage + "TOU: Week Profile Set Successfully, ";
                                else if (nRetVal == 2)
                                    sMessage = sMessage + "TOU: Week Profile Write Denied, ";
                                else if (nRetVal == 1 || nRetVal == 3)
                                    sMessage = sMessage + "TOU: Week Profile Error in Setting, ";

                                if (nRetVal == 0 || nRetVal == 1)
                                {
                                    nRetVal = SetParameter("001400000D0000FF06", nWait, nTryCount, nTimeOut, sTODData[1]);
                                    if (nRetVal == 0)
                                        sMessage = sMessage + "TOU: Calendar Name Set Successfully, ";
                                    else if (nRetVal == 2)
                                        sMessage = sMessage + "TOU: Calendar Name Write Denied, ";
                                    else if (nRetVal == 1 || nRetVal == 3)
                                        sMessage = sMessage + "TOU: Calendar Name Error in Setting, ";
                                    if (nRetVal == 0)
                                    {
                                        sData = string.Empty;
                                        sData = "090C" + Convert.ToInt16(ApplicableTimeString.Substring(4, 4)).ToString("X4") + Convert.ToByte(ApplicableTimeString.Substring(2, 2)).ToString("X2") + Convert.ToByte(ApplicableTimeString.Substring(0, 2)).ToString("X2");
                                        if (Convert.ToInt32(ApplicableTimeString.Substring(14, 2)) == 0)
                                            sData += "07";
                                        else
                                            sData += Convert.ToByte(ApplicableTimeString.Substring(14, 2)).ToString("X2");
                                        sData += Convert.ToByte(ApplicableTimeString.Substring(8, 2)).ToString("X2") + Convert.ToByte(ApplicableTimeString.Substring(10, 2)).ToString("X2") + "0000" + _deviationByte + "00";

                                        nRetVal = SetParameter("001400000D0000FF0A", nWait, nTryCount, nTimeOut, sData);
                                        if (nRetVal == 0)
                                            sMessage = sMessage + "TOU: Passive Calender Date and Time Set Successfully, ";
                                        else if (nRetVal == 2)
                                            sMessage = sMessage + "TOU: Passive Calender Date and Time Write Denied, ";
                                        else if (nRetVal == 1 || nRetVal == 3)
                                            sMessage = sMessage + "TOU: Passive Calender Date and Time Error in Setting ";
                                    }
                                }
                            }
                        }
                        break;
                    //Special Days 0.0.11.0.0.255
                    case "0.0.11.0.0.255":
                        int SDOperationCheck = Convert.ToInt16(dataToSet.Substring(0, 2));
                        dataToSet = dataToSet.Substring(2);
                        sData = string.Empty;
                        if (SDOperationCheck == 1)
                        {

                            int SpecialDaysCount = Convert.ToInt16(dataToSet.Substring(2, 2));
                            dataToSet = dataToSet.Substring(4);
                            for (int i = 0; i < SpecialDaysCount; i++)
                            {
                                sData = dataToSet.Substring(28 * i, 28);
                                nRetVal = SetParameter(objectName, nWait, nTryCount, nTimeOut, "0101" + sData);
                                if (nRetVal == 0)
                                    sMessage = sMessage + "Special Days Index: " + Convert.ToInt32(sData.Substring(6, 4)) + " Set Successfully,";
                                else
                                    sMessage = sMessage + "Special Days Index: " + Convert.ToInt32(sData.Substring(6, 4)) + " Error in Setting,";
                            }
                        }
                        else if (SDOperationCheck == 0)
                        {
                            int SpecialDaysDeleteCount = Convert.ToInt16(dataToSet.Substring(0, 2));
                            dataToSet = dataToSet.Substring(2);
                            for (int i = 0; i < SpecialDaysDeleteCount; i++)
                            {
                                sData = dataToSet.Substring(6 * i, 6);
                                nRetVal = SetParameter(objectName, nWait, nTryCount, nTimeOut, "0201" + sData);
                                if (nRetVal == 0)
                                    sMessage = sMessage + "Special Days Index: " + Convert.ToInt32(sData.Substring(2, 4)) + " Deleted Successfully,";
                                else
                                    sMessage = sMessage + "Special Days Index: " + Convert.ToInt32(sData.Substring(2, 4)) + " Error in Deletion,";
                            }
                        }
                        break;
                        ////Image transfer  0.0.44.0.0.255
                        //case "":
                        //    break;
                        ////Image activation single action schedule 0.0.15.0.2.255
                        //case "":
                        //    break;

                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message.ToString());
                sMessage = "Meter not connected. Please confirm power cable or password";
            }
            return sMessage;
        }
        public int? GetIndexOfLoadlimit(string llt)
        {
            int? temp = null;
            if (llt == "Enable")
            {
                temp = 1;   // 1 to enable Load Limit Setting
            }
            else if (llt == "Disable")
            {
                temp = 0;   // 0 to disable Load Limit Setting
            }
            else if (llt == "NA")
            {
                temp = 2;   // No Change
            }
            return temp;
        }
        public int? GetIndexOfBillingCycle(string billCycle)
        {
            int? temp = null;
            if (billCycle == "Monthly")
            {
                temp = 1;   // 1 to set Monthly billing Cycle
            }
            else if (billCycle == "Odd")
            {
                temp = 2;   // 2 to set Odd Months billing Cycle
            }
            else if (billCycle == "Even")
            {
                temp = 3;   // 3 to set Odd Months billing Cycle
            }
            return temp;
        }
        public void ClearBuffer()
        {
            for (int index = 0; index <= this.nCounter; ++index)
                this.nRcvPkt[index] = (byte)0;
            this.nCounter = 0;
        }
        private bool SendPkt(byte[] buffer, ushort length)
        {
            DateTime now = DateTime.Now;
            string empty = string.Empty;
            oSC.Write(buffer, 0, length);
            //for (int offset = 0; offset < (int)length; ++offset)
            //{
            //    oSC.Write(buffer, offset, 1);
            //    empty += buffer[offset].ToString("X2");
            //}
            //this.temp = string.Empty;
            //for (int i = 0; i < length; i++)
            //{
            //    //    ComPort.Write(buffer, i, 1);
            //    temp = temp + buffer[i].ToString("X2");
            //}
            //swLT.Write("(S)\t" + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss:fff tt") + "\t" + temp + "\r\n");
            //temp = "(S)" + " " + temp + " " + DateTime.Now.ToString(Constants.timeStampFormate) + "\r\n";
            //LineTrafficControlEventHandler(temp, "Send");
            return true;
        }
        private bool SendPkt(byte[] buffer, int length)
        {
            DateTime now = DateTime.Now;
            string empty = string.Empty;
            oSC.Write(buffer, 0, length);
            //for (int offset = 0; offset < (int)length; ++offset)
            //{
            //    oSC.Write(buffer, offset, 1);
            //    empty += buffer[offset].ToString("X2");
            //}
            //this.temp = string.Empty;
            //for (int i = 0; i < length; i++)
            //{
            //    //    ComPort.Write(buffer, i, 1);
            //    temp = temp + buffer[i].ToString("X2");
            //}
            //swLT.Write("(S)\t" + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss:fff tt") + "\t" + temp + "\r\n");
            //temp = "(S)" + " " + temp + " " + DateTime.Now.ToString(Constants.timeStampFormate) + "\r\n";
            //LineTrafficControlEventHandler(temp, "Send");
            return true;
        }

        private void DataReceive()
        {
            try
            {
                byte[] bytes = oSC.GetBufferDataArray();
                string temp = string.Empty;
                //for (int i = 0; i < bytes.Length; i++)
                //{
                //    //    ComPort.Write(buffer, i, 1);
                //    temp = temp + bytes[i].ToString("X2");
                //}
                nCounter = bytes.Length;
                if (bytes != null)
                    Array.Copy(bytes, nRcvPkt, bytes.Length);
            }
            catch (Exception)
            { }
            //try
            //{
            //    int num = oSC.ReadBuffer(buffer, 0, 64);
            //    for (int index = 0; index < num; ++index)
            //        this.nRcvPkt[this.nCounter++] = this.buffer[index];
            //}
            //catch (Exception ex)
            //{
            //}
        }
        public void Wait(double nSecValue)
        {
            DateTime dateTime = DateTime.Now.AddMilliseconds(nSecValue);
            while (DateTime.Now < dateTime)
                Application.DoEvents();
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
        public static string RandomString(int length)
        {
            return new string(Enumerable.Repeat<string>("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", length).Select<string, char>((Func<string, char>)(s => s[random.Next(s.Length)])).ToArray<char>());
        }
        public byte[] Encrypt(string encryptedLine, string sEK)
        {
            byte[] byteArray1 = StringToByteArray(encryptedLine.Substring(10));
            byte[] byteArray2 = StringToByteArray(encryptedLine.Substring(0, 2));
            byte[] byteArray3 = StringToByteArray("000000000000000000000000");
            byte[] array1 = (byte[])null;
            if (byteArray2[0] == (byte)32)
            {
                Array.Resize<byte>(ref array1, byteArray1.Length + byteArray3.Length);
                Array.Copy((Array)byteArray1, (Array)array1, byteArray1.Length);
                Array.Copy((Array)byteArray3, 0, (Array)array1, byteArray1.Length, byteArray3.Length);
            }
            else
            {
                Array.Resize<byte>(ref array1, byteArray1.Length);
                Array.Copy((Array)byteArray1, (Array)array1, byteArray1.Length);
            }
            byte[] byteArray4 = StringToByteArray(sEK);
            byte[] sourceArray = DLMSInfo.TxtAK.Trim().Length != 32 ? Encoding.ASCII.GetBytes(DLMSInfo.TxtAK.Trim()) : StringToByteArray(DLMSInfo.TxtAK.Trim());
            byte[] numArray = Combine(DLMSInfo.TxtSysT.Trim().Length != 16 ? Encoding.ASCII.GetBytes(DLMSInfo.TxtSysT.Trim()) : StringToByteArray(DLMSInfo.TxtSysT.Trim()), StringToByteArray(encryptedLine.Substring(2, 8)));
            byte[] array2 = (byte[])null;
            if (byteArray2[0] == (byte)16)
            {
                Array.Resize<byte>(ref array2, byteArray2.Length + sourceArray.Length + array1.Length);
                Array.Copy((Array)byteArray2, (Array)array2, 1);
                Array.ConstrainedCopy((Array)sourceArray, 0, (Array)array2, 1, sourceArray.Length);
                Array.ConstrainedCopy((Array)array1, 0, (Array)array2, 1 + sourceArray.Length, array1.Length);
            }
            else if (byteArray2[0] == (byte)48)
            {
                Array.Resize<byte>(ref array2, byteArray2.Length + sourceArray.Length);
                Array.Copy((Array)byteArray2, (Array)array2, 1);
                Array.ConstrainedCopy((Array)sourceArray, 0, (Array)array2, 1, sourceArray.Length);
            }
            byte[] array3 = new byte[16];
            GcmBlockCipher cipher = new GcmBlockCipher((IBlockCipher)new AesFastEngine());
            AeadParameters parameters1 = new AeadParameters(new KeyParameter(byteArray4), 96, numArray, array2);
            cipher.Init(true, (ICipherParameters)parameters1);
            if (byteArray2[0] == (byte)16)
            {
                GMac gmac = new GMac(cipher);
                ParametersWithIV parameters2 = new ParametersWithIV((ICipherParameters)new KeyParameter(byteArray4), numArray);
                gmac.Init((ICipherParameters)parameters2);
                gmac.BlockUpdate(array2, 0, array2.Length);
                gmac.DoFinal(array3, 0);
                WriteFileContent(array3);
            }
            else
            {
                try
                {
                    Array.Resize<byte>(ref array3, cipher.GetOutputSize(array1.Length));
                    int outOff = cipher.ProcessBytes(array1, 0, array1.Length, array3, 0);
                    cipher.DoFinal(array3, outOff);
                }
                catch (InvalidCipherTextException ex)
                {
                    Console.WriteLine(ex.Message);
                    Array.Resize<byte>(ref array3, array3.Length + 3);
                    Array.Copy((Array)Encoding.ASCII.GetBytes("err"), 0, (Array)array3, array3.Length - 3, 3);
                }
                finally
                {
                    if (byteArray2[0] == (byte)32)
                        Array.Resize<byte>(ref array3, encryptedLine.Length / 2 - 6);
                    WriteFileContent(array3);
                }
            }
            return array3;
        }
        public byte[] Decrypt(string encryptedLine, string sEK)
        {
            byte[] byteArray1 = StringToByteArray(encryptedLine.Substring(10));
            byte[] byteArray2 = StringToByteArray(encryptedLine.Substring(0, 2));
            byte[] byteArray3 = StringToByteArray("000000000000000000000000");
            byte[] array1 = (byte[])null;
            if (byteArray2[0] == (byte)32)
            {
                Array.Resize<byte>(ref array1, byteArray1.Length + byteArray3.Length);
                Array.Copy((Array)byteArray1, (Array)array1, byteArray1.Length);
                Array.Copy((Array)byteArray3, 0, (Array)array1, byteArray1.Length, byteArray3.Length);
            }
            else
            {
                Array.Resize<byte>(ref array1, byteArray1.Length);
                Array.Copy((Array)byteArray1, (Array)array1, byteArray1.Length);
            }
            byte[] byteArray4 = StringToByteArray(sEK);
            byte[] sourceArray = DLMSInfo.TxtAK.Trim().Length != 32 ? Encoding.ASCII.GetBytes(DLMSInfo.TxtAK.Trim()) : StringToByteArray(DLMSInfo.TxtAK.Trim());
            byte[] numArray = Combine(StringToByteArray(this.sSYSTaginHEX), StringToByteArray(encryptedLine.Substring(2, 8)));
            byte[] array2 = (byte[])null;
            if (byteArray2[0] == (byte)16)
            {
                Array.Resize<byte>(ref array2, byteArray2.Length + sourceArray.Length + array1.Length - 12);
                Array.Copy((Array)byteArray2, (Array)array2, 1);
                Array.ConstrainedCopy((Array)sourceArray, 0, (Array)array2, 1, sourceArray.Length);
                Array.ConstrainedCopy((Array)array1, 0, (Array)array2, 1 + sourceArray.Length, array1.Length - 12);
            }
            else if (byteArray2[0] == (byte)48)
            {
                Array.Resize<byte>(ref array2, byteArray2.Length + sourceArray.Length);
                Array.Copy((Array)byteArray2, (Array)array2, 1);
                Array.ConstrainedCopy((Array)sourceArray, 0, (Array)array2, 1, sourceArray.Length);
            }
            byte[] array3 = new byte[16];
            GcmBlockCipher cipher = new GcmBlockCipher((IBlockCipher)new AesFastEngine());
            AeadParameters parameters1 = new AeadParameters(new KeyParameter(byteArray4), 96, numArray, array2);
            cipher.Init(false, (ICipherParameters)parameters1);
            if (byteArray2[0] == (byte)16)
            {
                GMac gmac = new GMac(cipher);
                ParametersWithIV parameters2 = new ParametersWithIV((ICipherParameters)new KeyParameter(byteArray4), numArray);
                gmac.Init((ICipherParameters)parameters2);
                gmac.BlockUpdate(array2, 0, array2.Length);
                gmac.DoFinal(array3, 0);
                WriteFileContent(array3);
            }
            else
            {
                try
                {
                    Array.Resize<byte>(ref array3, cipher.GetOutputSize(array1.Length));
                    int outOff = cipher.ProcessBytes(array1, 0, array1.Length, array3, 0);
                    cipher.DoFinal(array3, outOff);
                }
                catch (InvalidCipherTextException ex)
                {
                    Console.WriteLine(ex.Message);
                    Array.Resize<byte>(ref array3, array3.Length);
                }
                finally
                {
                    WriteFileContent(array3);
                }
            }
            return array3;
        }
        public static byte[] Combine(byte[] first, byte[] second)
        {
            byte[] dst = new byte[first.Length + second.Length];
            Buffer.BlockCopy((Array)first, 0, (Array)dst, 0, first.Length);
            Buffer.BlockCopy((Array)second, 0, (Array)dst, first.Length, second.Length);
            return dst;
        }
        public static void WriteFileContent(byte[] fileContent)
        {
        }
        private void FrameType()
        {
            this.pktLength = int.Parse(((int)this.nRcvPkt[1] & 7).ToString("X2") + this.nRcvPkt[2].ToString("X2"), NumberStyles.HexNumber);
            if (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] != (byte)115 && ((int)this.nRcvPkt[1] & 168) == 160)
            {
                this.nRecv = Convert.ToByte((int)this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] >> 5);
                this.nSent = Convert.ToByte((int)this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] >> 1 & 7);
            }
            if (((int)this.nRcvPkt[1] & 168) == 160 && this.pktLength > 9 + (int)this.bytAddMode || ((int)this.nRcvPkt[1] & 168) == 168)
                this.nRecvCntr = this.nRecvCntr != (byte)7 ? ++this.nRecvCntr : (byte)0;
            if (((int)this.nRcvPkt[1] & 168) != 168)
            {
                if (this.nRecvLast != (byte)7)
                {
                    if ((int)this.nRecv - (int)this.nRecvLast == 1)
                        this.nSentCntr = this.nSentCntr != (byte)7 ? ++this.nSentCntr : (byte)0;
                }
                else if ((int)this.nRecvLast - (int)this.nRecv == 7)
                    this.nSentCntr = this.nSentCntr != (byte)7 ? ++this.nSentCntr : (byte)0;
            }
            this.nRecvLast = this.nRecv;
            this.nSentLast = this.nSent;
        }
        private void FrameTypeFG()
        {
            this.pktLength = int.Parse(((int)this.nRcvPkt[1] & 7).ToString("X2") + this.nRcvPkt[2].ToString("X2"), NumberStyles.HexNumber);
            if (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] != (byte)115 && ((int)this.nRcvPkt[1] & 168) == 160)
            {
                this.nRecv = Convert.ToByte((int)this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] >> 5);
                this.nSent = Convert.ToByte((int)this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] >> 1 & 7);
            }
            if (((int)this.nRcvPkt[1] & 168) == 160 && this.pktLength > 9 + (int)this.bytAddMode || ((int)this.nRcvPkt[1] & 168) == 168)
                this.nRecvCntr = this.nRecvCntr != (byte)7 ? ++this.nRecvCntr : (byte)0;
            if (((int)this.nRcvPkt[1] & 168) != 168)
            {
                if (this.nRecvLast != (byte)7)
                {
                    if ((int)this.nRecv - (int)this.nRecvLast == 1)
                        this.nSentCntr = this.nSentCntr != (byte)7 ? ++this.nSentCntr : (byte)0;
                }
                else if ((int)this.nRecvLast - (int)this.nRecv == 7)
                    this.nSentCntr = this.nSentCntr != (byte)7 ? ++this.nSentCntr : (byte)0;
            }
            this.nRecvLast = this.nRecv;
            this.nSentLast = this.nSent;
        }

        public static byte[] StrToByteArray(string str) => new ASCIIEncoding().GetBytes(str);
        public bool ActionCmd(string sActionData, bool IsLineTrafficEnabled = true)
        {
            if (IsLineTrafficEnabled)
                LineTrafficControlEventHandler($"     AUTHENTICATING", "Send");
            byte num1 = Convert.ToByte((int)this.bytAddMode + 8);
            string empty1 = string.Empty;
            this.nRetLSH = Convert.ToByte((int)this.nRecvCntr << 5);
            this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nRetLSH | 16);
            this.nRetLSH = Convert.ToByte((int)this.nSentCntr << 1);
            this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nRetLSH | (int)this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)]);
            byte[] nPkt1 = this.nPkt;
            int index1 = (int)num1;
            byte num2 = (byte)(index1 + 1);
            nPkt1[index1] = (byte)230;
            byte[] nPkt2 = this.nPkt;
            int index2 = (int)num2;
            byte num3 = (byte)(index2 + 1);
            nPkt2[index2] = (byte)230;
            byte[] nPkt3 = this.nPkt;
            int index3 = (int)num3;
            byte num4 = (byte)(index3 + 1);
            nPkt3[index3] = (byte)0;
            string hex = "C301C1000F0000280000FF01" + sActionData;
            string sEK = string.Empty;
            ASCIIEncoding asciiEncoding = new ASCIIEncoding();
            if (DLMSInfo.TxtEK.Trim().Length == 32)
            {
                sEK = DLMSInfo.TxtEK.Trim();
            }
            else
            {
                this.Ps1 = asciiEncoding.GetBytes(DLMSInfo.TxtEK.Trim());
                for (int index4 = 0; index4 < this.Ps1.Length; ++index4)
                    sEK += this.Ps1[index4].ToString("X2");
            }
            if (DLMSInfo.IsLNWithCipher || DLMSInfo.IsLNWithCipherDedicatedKey)
            {
                //this.swLT.Write("\t" + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss:ff tt") + "\t" + "Command : -- >> " + hex + "\r\n");
                byte[] nPkt4 = this.nPkt;
                int index5 = (int)num4;
                byte num5 = (byte)(index5 + 1);
                nPkt4[index5] = (byte)203;
                byte[] nPkt5 = this.nPkt;
                int index6 = (int)num5;
                byte num6 = (byte)(index6 + 1);
                nPkt5[index6] = (byte)0;
                byte[] nPkt6 = this.nPkt;
                int index7 = (int)num6;
                byte num7 = (byte)(index7 + 1);
                nPkt6[index7] = (byte)48;
                byte[] nPkt7 = this.nPkt;
                int index8 = (int)num7;
                byte num8 = (byte)(index8 + 1);
                nPkt7[index8] = (byte)0;
                byte[] nPkt8 = this.nPkt;
                int index9 = (int)num8;
                byte num9 = (byte)(index9 + 1);
                nPkt8[index9] = (byte)0;
                byte[] nPkt9 = this.nPkt;
                int index10 = (int)num9;
                byte num10 = (byte)(index10 + 1);
                int num11 = (int)Convert.ToByte(this.nCommandCounter / 256);
                nPkt9[index10] = (byte)num11;
                byte[] nPkt10 = this.nPkt;
                int index11 = (int)num10;
                num4 = (byte)(index11 + 1);
                int num12 = (int)Convert.ToByte(this.nCommandCounter % 256);
                nPkt10[index11] = (byte)num12;
                byte[] numArray = this.Encrypt("30" + this.nCommandCounter++.ToString("X8") + hex, sEK);
                this.nPkt[(int)num4 - 6] = Convert.ToByte(numArray.Length + 5);
                for (int index12 = 0; index12 < numArray.Length; ++index12)
                    this.nPkt[(int)num4++] = numArray[index12];
            }
            else
            {
                foreach (byte num13 in StringToByteArray(hex))
                    this.nPkt[(int)num4++] = num13;
            }
            this.nPkt[2] = Convert.ToByte((int)num4 + 1);
            this.DCl.fcs(ref this.nPkt, (int)Convert.ToByte((int)this.bytAddMode + 5), (byte)1);
            this.DCl.fcs(ref this.nPkt, (int)Convert.ToByte((int)num4 - 1), (byte)1);
            this.nPkt[(int)num4 + 2] = (byte)126;
            this.ClearBuffer();
            bool flag = false;
            this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)num4 + 3));
            if (IsLineTrafficEnabled)
                SendDataPrint(nPkt, Convert.ToByte((int)num4 + 3));
            DateTime now = DateTime.Now;
            while (true)
            {
                this.Wait(50);//100->50
                Application.DoEvents();
                this.DataReceive();
                if (this.nCounter <= 2 || (int)this.nRcvPkt[2] + 2 > this.nCounter || this.nRcvPkt[(int)this.nRcvPkt[2] + 1] != (byte)126)
                {
                    if (DateTime.Now.Subtract(now).Seconds > 4)
                        goto label_17;
                }
                else
                    break;
            }
            flag = true;
            this.FrameType();
        label_17:
            if (this.nCounter > 25)
            {
                if (!DLMSInfo.IsLNWithCipher)
                {
                    Aes aes = new Aes(Aes.KeySize.Bits128, this.keyBytes);
                    int num14 = 0;
                    if (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 16)] == (byte)9 && this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 17)] == (byte)16)
                        num14 = 18;
                    else if (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 17)] == (byte)9 && this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 18)] == (byte)16)
                        num14 = 19;
                    for (int index13 = 0; index13 < 16; ++index13)
                        this.cipherText[index13] = this.nRcvPkt[index13 + (int)Convert.ToByte((int)this.bytAddMode + num14)];
                    aes.InvCipher(this.cipherText, this.plainText);
                    for (int index14 = 0; index14 < 16; ++index14)
                    {
                        if ((int)this.plainText[index14] != (int)this.Ps[index14])
                        {
                            flag = false;
                            break;
                        }
                        flag = true;
                        if (!flag)
                            break;
                    }
                }
                else
                {
                    string empty2 = string.Empty;
                    string empty3 = string.Empty;
                    flag = false;
                    for (int index15 = this.nRcvPkt[(int)this.bytAddMode + 12] != (byte)130 ? (this.nRcvPkt[(int)this.bytAddMode + 12] != (byte)129 ? (int)this.bytAddMode + 13 : (int)this.bytAddMode + 14) : (int)this.bytAddMode + 15; index15 < this.nCounter - 3; ++index15)
                        empty2 += this.nRcvPkt[index15].ToString("X2");
                    byte[] numArray1 = this.Decrypt(empty2, sEK);
                    if (numArray1.Length > 5)
                    {
                        if (!DLMSInfo.IsWithGMAC)
                        {
                            Aes aes = new Aes(Aes.KeySize.Bits128, this.keyBytes);
                            for (int index16 = 0; index16 < 16; ++index16)
                                this.cipherText[index16] = numArray1[index16 + (int)Convert.ToByte(8)];
                            Array.Clear((Array)this.plainText, 0, this.plainText.Length);
                            aes.InvCipher(this.cipherText, this.plainText);
                            for (int index17 = 0; index17 < 16; ++index17)
                            {
                                if ((int)this.plainText[index17] != (int)this.Ps[index17])
                                {
                                    flag = false;
                                    break;
                                }
                                flag = true;
                                if (!flag)
                                    break;
                            }
                        }
                        else
                        {
                            string empty4 = string.Empty;
                            for (int index18 = 8; index18 < numArray1.Length; ++index18)
                            {
                                empty4 += numArray1[index18].ToString("X2");
                                if (index18 == 12)
                                {
                                    for (int index19 = 0; index19 < this.Ps.Length; ++index19)
                                        empty4 += this.Ps[index19].ToString("X2");
                                }
                            }
                            byte[] numArray2 = this.Decrypt(empty4, this.sEncryptkeyinHEX);
                            string empty5 = string.Empty;
                            for (int index20 = 0; index20 < numArray2.Length - 4; ++index20)
                                empty5 += numArray2[index20].ToString("X2");
                            if (empty4.Contains(empty5))
                                flag = true;
                        }
                    }
                    else
                        flag = false;
                }
            }
            else
                flag = false;
            this.temp = string.Empty;
            for (int index21 = 0; index21 < this.nCounter; ++index21)
                this.temp += this.nRcvPkt[index21].ToString("X2") + " ";
            this.temp += "\r\n";
            if (IsLineTrafficEnabled)
                LineTrafficControlEventHandler("(R)" + "  " + temp.Trim() + " " + DateTime.Now.ToString(Constants.timeStampFormate) + Environment.NewLine + Environment.NewLine, "Receive");
            //this.swLT.Write("(R)\t" + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss:ff tt") + "\t" + this.temp);
            return flag;
        }
        //Normal
        public int ActionCmd(
          string sWhichData,
          byte nWait,
          byte nTryCount,
          byte nTimeOut,
          string strDataTx)
        {
            LineTrafficControlEventHandler($"     ACTION CLASS-{Convert.ToInt32(sWhichData.Substring(0, 4), 16)} | OBIS-{DLMSParser.GetObis(sWhichData.Substring(4, 12))} [{DLMSParser.GetObisName(Convert.ToInt32(sWhichData.Substring(0, 4), 16).ToString(), DLMSParser.GetObis(sWhichData.Substring(4, 12)), Convert.ToInt32(sWhichData.Substring(sWhichData.Length - 2), 16).ToString())}] | Method-{Convert.ToInt32(sWhichData.Substring(sWhichData.Length - 2), 16)}", "Send");
            //LineTrafficControlEventHandler($"     ACTION CLASS-{Convert.ToInt32(sWhichData.Substring(0, 4))} | OBIS-{DLMSParser.GetObis(sWhichData.Substring(4, 12))} [{DLMSParser.GetObisName(Convert.ToInt32(sWhichData.Substring(0, 4)).ToString(), DLMSParser.GetObis(sWhichData.Substring(4, 12)), Convert.ToInt32(sWhichData.Substring(sWhichData.Length - 2), 16).ToString())}] | Attribute-{sWhichData.Substring(16, 2)}", "Send");
            byte num1 = Convert.ToByte((int)this.bytAddMode + 8);
            DateTime now1 = DateTime.Now;
            string empty = string.Empty;
            StringBuilder stringBuilder = new StringBuilder();
            this.nRetLSH = Convert.ToByte((int)this.nRecvCntr << 5);
            this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nRetLSH | 16);
            this.nRetLSH = Convert.ToByte((int)this.nSentCntr << 1);
            this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nRetLSH | (int)this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)]);
            byte[] nPkt1 = this.nPkt;
            int index1 = (int)num1;
            byte num2 = (byte)(index1 + 1);
            nPkt1[index1] = (byte)230;
            byte[] nPkt2 = this.nPkt;
            int index2 = (int)num2;
            byte num3 = (byte)(index2 + 1);
            nPkt2[index2] = (byte)230;
            byte[] nPkt3 = this.nPkt;
            int index3 = (int)num3;
            byte num4 = (byte)(index3 + 1);
            nPkt3[index3] = (byte)0;
            //string hex = "C30181" + sWhichData + "00";
            string hex = "";
            if (sWhichData.Contains("000B00000B0000FF"))
                hex = "C30181" + "000B00000B0000FF";
            else if (sWhichData.Contains("001400000D0000FF01"))
                hex = "C30181" + "001400000D0000FF01" + "01" + strDataTx.Trim().Replace(" ", "");
            else if (sWhichData.Contains("001200002C0000FF01"))//Image transfer initialization
                hex = "C30181" + sWhichData + strDataTx.Trim().Replace(" ", "");
            else if (sWhichData.Contains("001200002C0000FF02"))//Image transfer to Meter
                hex = "C30181" + sWhichData + "01" + strDataTx.Trim().Replace(" ", "");
            else if (!string.IsNullOrEmpty(strDataTx))
            {
                hex = "C30181" + sWhichData + strDataTx.Trim().Replace(" ", "");
            }
            else
                hex = "C30181" + sWhichData + "00";
            //if (strDataTx != "")
            //    hex += strDataTx.Replace(" ", "");
            this.temp = "Command  : -- >> " + hex + "\r\n";
            commandString = temp;
            if (DLMSInfo.IsLNWithCipher || DLMSInfo.IsWithGMAC)
            {
                byte num5 = 48;
                this.temp = "Command  : -- >> " + hex + "\r\n";
                commandString = temp;
                byte[] nPkt4 = this.nPkt;
                int index4 = (int)num4;
                byte num6 = (byte)(index4 + 1);
                nPkt4[index4] = (byte)211;
                byte[] nPkt5 = this.nPkt;
                int index5 = (int)num6;
                byte num7 = (byte)(index5 + 1);
                nPkt5[index5] = (byte)0;
                byte[] nPkt6 = this.nPkt;
                int index6 = (int)num7;
                byte num8 = (byte)(index6 + 1);
                int num9 = (int)num5;
                nPkt6[index6] = (byte)num9;
                byte[] nPkt7 = this.nPkt;
                int index7 = (int)num8;
                byte num10 = (byte)(index7 + 1);
                nPkt7[index7] = (byte)0;
                byte[] nPkt8 = this.nPkt;
                int index8 = (int)num10;
                byte num11 = (byte)(index8 + 1);
                nPkt8[index8] = (byte)0;
                byte[] nPkt9 = this.nPkt;
                int index9 = (int)num11;
                byte num12 = (byte)(index9 + 1);
                int num13 = (int)Convert.ToByte(this.nCommandCounter / 256);
                nPkt9[index9] = (byte)num13;
                byte[] nPkt10 = this.nPkt;
                int index10 = (int)num12;
                num4 = (byte)(index10 + 1);
                int num14 = (int)Convert.ToByte(this.nCommandCounter % 256);
                nPkt10[index10] = (byte)num14;
                byte[] numArray = this.Encrypt(num5.ToString("X2") + this.nCommandCounter++.ToString("X8") + hex, this.sEncryptkeyinHEX);
                this.nPkt[(int)num4 - 6] = Convert.ToByte(numArray.Length + 5);
                for (int index11 = 0; index11 < numArray.Length; ++index11)
                    this.nPkt[(int)num4++] = numArray[index11];
            }
            else
            {
                foreach (byte num15 in StringToByteArray(hex))
                    this.nPkt[(int)num4++] = num15;
            }
            this.nPkt[2] = Convert.ToByte((int)num4 + 1);
            this.DCl.fcs(ref this.nPkt, Convert.ToByte((int)this.bytAddMode + 5), (byte)1);
            this.DCl.fcs(ref this.nPkt, Convert.ToByte((int)num4 - 1), (byte)1);
            this.nPkt[(int)num4 + 2] = (byte)126;
            byte num16 = 0;
            bool flag;
            do
            {
                this.ClearBuffer();
                flag = false;
                this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)num4 + 3));
                SendDataPrint(nPkt, Convert.ToByte((int)num4 + 3));
                DateTime now2 = DateTime.Now;
                do
                {
                    Application.DoEvents();
                    this.DataReceive();
                    if (this.nCounter > 2 && (int)this.nRcvPkt[2] + 2 == this.nCounter)
                    {
                        flag = true;
                        this.FrameType();
                    }
                    if (DateTime.Now.Subtract(now2).Seconds > (int)nTimeOut && (int)num16 < (int)nTryCount)
                    {
                        ++num16;
                        break;
                    }
                }
                while (!flag);
            }
            while (!flag && (int)num16 != (int)nTryCount);
            this.temp = string.Empty;
            for (int index12 = 0; index12 < this.nCounter; ++index12)
                this.temp += this.nRcvPkt[index12].ToString("X2");
            this.temp += "\r\n";
            for (int index13 = (int)this.bytAddMode + 11; index13 < this.nCounter - 3; ++index13)
                stringBuilder.Append(this.nRcvPkt[index13].ToString("X2"));
            if (stringBuilder.ToString().StartsWith("D7"))
            {
                if (stringBuilder.ToString().StartsWith("D782"))
                    stringBuilder.Remove(0, 8);
                else
                    stringBuilder.Remove(0, 4);
                byte[] numArray = this.Decrypt(stringBuilder.ToString(), this.sEncryptkeyinHEX);
                stringBuilder.Length = 0;
                for (int index14 = 0; index14 < numArray.Length; ++index14)
                    stringBuilder.Append(numArray[index14].ToString("X2"));
                this.temp = "Response : -- >> " + stringBuilder.ToString() + "\r\n";
                responseString = temp;
            }
            RecvDataPrint(nRcvPkt, nCounter);
            responseString = $"Response : -- >> {stringBuilder.ToString()}\r\n\r\n";
            LineTrafficControlEventHandler($"     {commandString}", "Command");
            LineTrafficControlEventHandler($"     {responseString}\r\n", "Response");
            if (stringBuilder.ToString().Contains("C7018100") || stringBuilder.ToString().Contains("C701C10000") || stringBuilder.ToString().Contains("C701C10000"))
                return 0;
            return stringBuilder.ToString() == "C701810103" ? 2 : 1;
        }
        //Trigger
        public bool ActionCmd(string sWhichData, string strDataTx)
        {
            LineTrafficControlEventHandler($"     ACTION CLASS-{Convert.ToInt32(sWhichData.Substring(0, 4), 16)} | OBIS-{DLMSParser.GetObis(sWhichData.Substring(4, 12))} [{DLMSParser.GetObisName(Convert.ToInt32(sWhichData.Substring(0, 4), 16).ToString(), DLMSParser.GetObis(sWhichData.Substring(4, 12)), Convert.ToInt32(sWhichData.Substring(sWhichData.Length - 2), 16).ToString())}] | Method-{Convert.ToInt32(sWhichData.Substring(sWhichData.Length - 2), 16)}", "Send");
            //LineTrafficControlEventHandler($"     ACTION CLASS-{Convert.ToInt32(sWhichData.Substring(0, 4))} | OBIS-{DLMSParser.GetObis(sWhichData.Substring(4, 12))} [{DLMSParser.GetObisName(Convert.ToInt32(sWhichData.Substring(0, 4)).ToString(), DLMSParser.GetObis(sWhichData.Substring(4, 12)), Convert.ToInt32(sWhichData.Substring(sWhichData.Length - 2), 16).ToString())}] | Attribute-{sWhichData.Substring(16, 2)}", "Send");
            int num1 = (int)Convert.ToByte((int)this.bytAddMode + 8);
            string empty1 = string.Empty;
            string empty2 = string.Empty;
            StringBuilder stringBuilder = new StringBuilder();
            byte num2 = 0;
            this.nRetLSH = Convert.ToByte((int)this.nRecvCntr << 5);
            this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nRetLSH | 16);
            this.nRetLSH = Convert.ToByte((int)this.nSentCntr << 1);
            this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nRetLSH | (int)this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)]);
            byte[] nPkt1 = this.nPkt;
            int index1 = num1;
            int num3 = index1 + 1;
            nPkt1[index1] = (byte)230;
            byte[] nPkt2 = this.nPkt;
            int index2 = num3;
            int num4 = index2 + 1;
            nPkt2[index2] = (byte)230;
            byte[] nPkt3 = this.nPkt;
            int index3 = num4;
            int num5 = index3 + 1;
            nPkt3[index3] = (byte)0;
            //string hex = "C30181" + sWhichData + "00";
            string hex = "";
            if (sWhichData.Contains("000B00000B0000FF"))
                hex = "C30181" + "000B00000B0000FF";
            else if (sWhichData.Contains("001400000D0000FF01"))
                hex = "C30181" + "001400000D0000FF01" + "01";
            else
                hex = "C30181" + sWhichData + "00";
            if (strDataTx != "")
                hex += strDataTx.Replace(" ", "");
            this.temp = "Command  : -- >> " + hex + "\r\n";
            commandString = temp;
            if (DLMSInfo.IsLNWithCipher || DLMSInfo.IsWithGMAC)
            {
                if (DLMSInfo.AccessMode == 1)
                    num2 = (byte)32;
                else if (DLMSInfo.AccessMode == 2)
                    num2 = (byte)48;
                this.temp = "Command  : -- >> " + hex + "\r\n";
                commandString = temp;
                //this.Response1.SelectionColor = Color.Green;
                //this.Response1.AppendText("--> " + this.temp);
                byte[] nPkt4 = this.nPkt;
                int index4 = num5;
                int num6 = index4 + 1;
                nPkt4[index4] = (byte)211;
                byte[] nPkt5 = this.nPkt;
                int index5 = num6;
                int num7 = index5 + 1;
                nPkt5[index5] = (byte)0;
                byte[] nPkt6 = this.nPkt;
                int index6 = num7;
                int num8 = index6 + 1;
                int num9 = (int)num2;
                nPkt6[index6] = (byte)num9;
                byte[] nPkt7 = this.nPkt;
                int index7 = num8;
                int num10 = index7 + 1;
                nPkt7[index7] = (byte)0;
                byte[] nPkt8 = this.nPkt;
                int index8 = num10;
                int num11 = index8 + 1;
                nPkt8[index8] = (byte)0;
                byte[] nPkt9 = this.nPkt;
                int index9 = num11;
                int num12 = index9 + 1;
                int num13 = (int)Convert.ToByte(this.nCommandCounter / 256);
                nPkt9[index9] = (byte)num13;
                byte[] nPkt10 = this.nPkt;
                int index10 = num12;
                num5 = index10 + 1;
                int num14 = (int)Convert.ToByte(this.nCommandCounter % 256);
                nPkt10[index10] = (byte)num14;
                byte[] numArray = this.Encrypt(num2.ToString("X2") + this.nCommandCounter++.ToString("X8") + hex, this.sEncryptkeyinHEX);
                this.nPkt[num5 - 6] = Convert.ToByte(numArray.Length + 5);
                for (int index11 = 0; index11 < numArray.Length; ++index11)
                    this.nPkt[num5++] = numArray[index11];
            }
            else
            {
                foreach (byte num15 in StringToByteArray(hex))
                    this.nPkt[num5++] = num15;
            }
            this.nPkt[2] = Convert.ToByte(num5 + 1);
            this.DCl.fcs(ref this.nPkt, (ushort)Convert.ToByte((int)this.bytAddMode + 5), (byte)1);
            this.DCl.fcs(ref this.nPkt, Convert.ToUInt16(num5 - 1), (byte)1);
            this.nPkt[num5 + 2] = (byte)126;
            this.ClearBuffer();
            bool flag = false;
            this.SendPkt(this.nPkt, Convert.ToUInt16(num5 + 3));
            DateTime now = DateTime.Now;
            SendDataPrint(nPkt, Convert.ToByte((int)num5 + 3));
            do
            {
                this.Wait(100.0);
                Application.DoEvents();
                this.DataReceive();
                if (this.nCounter > 2 && (int)this.nRcvPkt[2] + 2 <= this.nCounter && this.nRcvPkt[(int)this.nRcvPkt[2] + 1] == (byte)126)
                {
                    flag = true;
                    this.FrameType();
                    break;
                }
            }
            while (DateTime.Now.Subtract(now).Seconds <= 4);
            if (!flag)
                return false;
            this.temp = string.Empty;
            for (int index12 = 0; index12 < this.nCounter; ++index12)
                this.temp += this.nRcvPkt[index12].ToString("X2");
            this.temp += "\r\n";
            //this.Response1.SelectionColor = Color.Red;
            //this.Response1.AppendText("<-- " + this.temp);
            for (int index13 = (int)this.bytAddMode + 11; index13 < this.nCounter - 3; ++index13)
                stringBuilder.Append(this.nRcvPkt[index13].ToString("X2"));
            if (stringBuilder.ToString().StartsWith("D7"))
            {
                if (stringBuilder.ToString().StartsWith("D782"))
                    stringBuilder.Remove(0, 8);
                else
                    stringBuilder.Remove(0, 4);
                byte[] numArray = this.Decrypt(stringBuilder.ToString(), this.sEncryptkeyinHEX);
                stringBuilder.Length = 0;
                for (int index14 = 0; index14 < numArray.Length; ++index14)
                    stringBuilder.Append(numArray[index14].ToString("X2"));
                this.temp = "Response : -- >> " + stringBuilder.ToString() + "\r\n";
                //this.Response1.SelectionColor = Color.Red;
                //this.Response1.AppendText("<-- " + this.temp);
            }
            responseString = $"Response : -- >> {stringBuilder.ToString()}\r\n\r\n";
            RecvDataPrint(nRcvPkt, nCounter);
            LineTrafficControlEventHandler($"     {commandString}", "Command");
            LineTrafficControlEventHandler($"     {responseString}", "Response");
            if (stringBuilder.ToString().Contains("C7018100") || stringBuilder.ToString().Contains("C701C100") || stringBuilder.ToString().Contains("C701C10000"))
                return true;
            return stringBuilder.ToString() == "C701810103" && false;
        }

        public bool ImageActionCmd(byte ClassID, string sOBISCode, byte nAttribID, string strDataTx)
        {
            string str1 = ClassID.ToString("X4") + sOBISCode + nAttribID.ToString("X2");
            LineTrafficControlEventHandler($"     ACTION CLASS-{Convert.ToInt32(str1.Substring(0, 4), 16)} | OBIS-{DLMSParser.GetObis(str1.Substring(4, 12))} [{DLMSParser.GetObisName(Convert.ToInt32(str1.Substring(0, 4), 16).ToString(), DLMSParser.GetObis(str1.Substring(4, 12)), Convert.ToInt32(str1.Substring(str1.Length - 2), 16).ToString())}] | Method-{Convert.ToInt32(str1.Substring(str1.Length - 2), 16)}", "Send");

            int num1 = (int)Convert.ToByte((int)this.bytAddMode + 8);
            DateTime now1 = DateTime.Now;
            string empty1 = string.Empty;
            StringBuilder stringBuilder = new StringBuilder();
            this.nRetLSH = Convert.ToByte((int)this.nRecvCntr << 5);
            this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nRetLSH | 16);
            this.nRetLSH = Convert.ToByte((int)this.nSentCntr << 1);
            this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nRetLSH | (int)this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)]);
            byte[] nPkt1 = this.nPkt;
            int index1 = num1;
            int num2 = index1 + 1;
            nPkt1[index1] = (byte)230;
            byte[] nPkt2 = this.nPkt;
            int index2 = num2;
            int num3 = index2 + 1;
            nPkt2[index2] = (byte)230;
            byte[] nPkt3 = this.nPkt;
            int index3 = num3;
            int num4 = index3 + 1;
            nPkt3[index3] = (byte)0;
            string hex1 = !(strDataTx == string.Empty) ? "C301C1" + str1 + "01" + strDataTx.Replace(" ", "") : "C30181" + str1 + "00";
            this.temp = "Command  : -- >> " + hex1 + "\r\n";
            commandString = temp;
            if (DLMSInfo.IsLNWithCipher || DLMSInfo.IsWithGMAC)
            {
                string empty2 = string.Empty;
                byte num5 = 48;
                string str2 = num5.ToString("X2");
                int num6 = this.nCommandCounter++;
                string str3 = num6.ToString("X8");
                string str4 = hex1;
                byte[] numArray = this.Encrypt(str2 + str3 + str4, this.sEncryptkeyinHEX);
                string hex2;
                if (numArray.Length > 128)
                {
                    num6 = numArray.Length + 5;
                    string str5 = num6.ToString("X4");
                    string str6 = num5.ToString("X2");
                    num6 = this.nCommandCounter - 1;
                    string str7 = num6.ToString("X8");
                    hex2 = "D382" + str5 + str6 + str7;
                }
                else
                {
                    num6 = numArray.Length + 5;
                    string str8 = num6.ToString("X2");
                    string str9 = num5.ToString("X2");
                    num6 = this.nCommandCounter - 1;
                    string str10 = num6.ToString("X8");
                    hex2 = "D3" + str8 + str9 + str10;
                }
                foreach (byte num7 in StringToByteArray(hex2))
                    this.nPkt[num4++] = num7;
                for (int index4 = 0; index4 < numArray.Length; ++index4)
                    this.nPkt[num4++] = numArray[index4];
            }
            else
            {
                foreach (byte num8 in StringToByteArray(hex1))
                    this.nPkt[num4++] = num8;
            }
            this.nPkt[1] = Convert.ToByte(160 | (int)Convert.ToByte((num4 + 1) / 256));
            this.nPkt[2] = Convert.ToByte((num4 + 1) % 256);
            this.DCl.fcs(ref this.nPkt, Convert.ToUInt16((int)this.bytAddMode + 5), (byte)1);
            this.DCl.fcs(ref this.nPkt, Convert.ToUInt16(num4 - 1), (byte)1);
            this.nPkt[num4 + 2] = (byte)126;
            bool flag = false;
            byte num16 = 0;
            do
            {
                this.ClearBuffer();
                flag = false;
                this.SendPkt(this.nPkt, Convert.ToInt32(num4 + 3));
                //SendDataPrint(nPkt, Convert.ToByte((int)num4 + 3));
                string sentData = string.Empty;
                for (int index = 0; index < (num4 + 3); index++)
                {
                    sentData = sentData + nPkt[index].ToString("X2") + " ";
                }
                sentData = "(S)" + "  " + sentData + " " + DateTime.Now.ToString(Constants.timeStampFormate) + "\r\n";
                LineTrafficControlEventHandler(sentData, "Send");
                DateTime now2 = DateTime.Now;
                do
                {
                    //this.ClearBuffer();
                    //this.Wait(100);
                    Application.DoEvents();
                    this.DataReceive();
                    if (this.nCounter > 2 && this.nRcvPkt[(int)this.nRcvPkt[2] + 1] == (byte)126 && (int)this.nRcvPkt[2] + 2 == this.nCounter)
                    {
                        flag = true;
                        this.FrameType();
                    }
                    if (DateTime.Now.Subtract(now2).Seconds > (int)nTimeOut && (int)num16 < (int)nTryCount)
                    {
                        ++num16;
                        break;
                    }
                }
                while (!flag);
            }
            while (!flag && (int)num16 != (int)nTryCount);

            //this.SendPkt(this.nPkt, Convert.ToInt32(num4 + 3));
            //DateTime now2 = DateTime.Now;
            //string sentData = string.Empty;
            //for (int index = 0; index < (num4 + 3); index++)
            //{
            //    sentData = sentData + nPkt[index].ToString("X2") + " ";
            //}
            //sentData = "(S)" + "  " + sentData + " " + DateTime.Now.ToString(Constants.timeStampFormate) + "\r\n";
            //LineTrafficControlEventHandler(sentData, "Send");
            //do
            //{
            //    Thread.Sleep(10);
            //    this.DataReceive();
            //    if (this.nCounter > 2 && (int)this.nRcvPkt[2] + 2 == this.nCounter)
            //    {
            //        this.FrameType();
            //        flag = true;
            //        break;
            //    }
            //}
            //while (DateTime.Now.Subtract(now2).TotalSeconds < 4);
            if (!flag)
                return false;
            for (int index5 = (int)this.bytAddMode + 11; index5 < this.nCounter - 3; ++index5)
                stringBuilder.Append(this.nRcvPkt[index5].ToString("X2"));
            if (stringBuilder.ToString().StartsWith("D7"))
            {
                if (stringBuilder.ToString().StartsWith("D782"))
                    stringBuilder.Remove(0, 8);
                else
                    stringBuilder.Remove(0, 4);
                byte[] numArray = this.Decrypt(stringBuilder.ToString(), this.sEncryptkeyinHEX);
                stringBuilder.Length = 0;
                for (int index6 = 0; index6 < numArray.Length; ++index6)
                    stringBuilder.Append(numArray[index6].ToString("X2"));
                this.temp = "Response : -- >> " + stringBuilder.ToString() + "\r\n";
            }
            RecvDataPrint(nRcvPkt, nCounter);
            responseString = $"Response : -- >> {stringBuilder.ToString()}\r\n\r\n";
            LineTrafficControlEventHandler($"     {commandString}", "Command");
            LineTrafficControlEventHandler($"     {responseString}", "Response");
            if (stringBuilder.ToString().Contains("C7018100") || stringBuilder.ToString().Contains("C701810000") || stringBuilder.ToString().Contains("C701C100") || stringBuilder.ToString().Contains("C701C10000") || stringBuilder.ToString().Contains("C401C100"))
                return true;
            return stringBuilder.ToString() == "C701810103" && false;
        }

        #endregion

        #region User Methods
        public bool SNRMforCTT(bool IsSkipAddressIni = false)
        {
            bool IsSuccessful = false;
            try
            {
                bigSNRM = false;
                if (!IsSkipAddressIni)
                {
                    AddressInit(DLMSInfo.AccessMode);
                    WSTx = Convert.ToByte(DLMSInfo.WSTx);
                    WSRx = Convert.ToByte(DLMSInfo.WSRx);
                    if (DLMSInfo.WSTx != "1" || DLMSInfo.WSRx != "1")
                        bigSNRM = true;
                    if (DLMSInfo.IFTx != "Default")
                        this.ISTx = (int)Convert.ToInt16(DLMSInfo.IFTx);
                    else
                        this.ISTx = 128;
                    if (DLMSInfo.IFRx != "Default")
                        this.ISRx = (int)Convert.ToInt16(DLMSInfo.IFRx);
                    else
                        this.ISRx = 128;
                    if (DLMSInfo.IFTx != "Default" || DLMSInfo.IFRx != "Default")
                        bigSNRM = true;
                }
                IsSuccessful = SetNRM_CTT(bigSNRM, true);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.Error(ex.StackTrace);
            }
            return IsSuccessful;
        }
        public bool AddressIniCTT()
        {
            bool IsSuccessful = false;
            try
            {
                bigSNRM = false;
                AddressInit(DLMSInfo.AccessMode);
                WSTx = Convert.ToByte(DLMSInfo.WSTx);
                WSRx = Convert.ToByte(DLMSInfo.WSRx);
                if (DLMSInfo.WSTx != "1" || DLMSInfo.WSRx != "1")
                    bigSNRM = true;
                if (DLMSInfo.IFTx != "Default")
                    this.ISTx = (int)Convert.ToInt16(DLMSInfo.IFTx);
                else
                    this.ISTx = 128;
                if (DLMSInfo.IFRx != "Default")
                    this.ISRx = (int)Convert.ToInt16(DLMSInfo.IFRx);
                else
                    this.ISRx = 128;
                if (DLMSInfo.IFTx != "Default" || DLMSInfo.IFRx != "Default")
                    bigSNRM = true;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.Error(ex.StackTrace);
            }
            return IsSuccessful;
        }

        public bool SignOnDLMS(bool IsLineTrafficEnabled = true)
        {
            int nRetVal = 100;
            int num3 = 100;
            string text = string.Empty;
            string str1;
            //this.strErrorFile = "\\Genus\\Gxt\\INDALI\\" + DateTime.Now.ToString("ddmmyyyyHHmmss") + "_ERROR.txt";
            //this.strLTFile = "\\Genus\\Gxt\\INDALI\\" + DateTime.Now.ToString("ddmmyyyyHHmmss") + "_LINETRAFFIC.txt";
            //this.swError = new StreamWriter(this.strErrorFile, true);
            //this.swLT = new StreamWriter(this.strLTFile, true);
            bool isSuccessfull = true;
            try
            {
                if (isSuccessfull)
                {
                    //Wait(100);//OLD
                    AddressInit(DLMSInfo.AccessMode);
                    bigSNRM = false;
                    WSTx = Convert.ToByte(DLMSInfo.WSTx);
                    WSRx = Convert.ToByte(DLMSInfo.WSRx);
                    //if (DLMSInfo.IFTx != "Default")
                    //    this.ISTx = (int)Convert.ToInt16(DLMSInfo.IFTx);
                    //if (DLMSInfo.IFRx != "Default")
                    //    this.ISRx = (int)Convert.ToInt16(DLMSInfo.IFRx);
                    //if (DLMSInfo.IFTx != "Default")
                    //    bigSNRM = true;
                    if (DLMSInfo.WSTx != "1" || DLMSInfo.WSRx != "1")
                        bigSNRM = true;
                    if (DLMSInfo.IFTx != "Default")
                    {
                        this.ISTx = (int)Convert.ToInt16(DLMSInfo.IFTx);
                        bigSNRM = true;
                    }
                    else
                        this.ISTx = 128;
                    if (DLMSInfo.IFRx != "Default")
                    {
                        this.ISRx = (int)Convert.ToInt16(DLMSInfo.IFRx);
                        bigSNRM = true;
                    }
                    else
                        this.ISRx = 128;

                    isSuccessfull = SetNRM(bigSNRM, IsLineTrafficEnabled);
                    if (!isSuccessfull)
                        text = "Meter Not Responding Properly, Please Try Again. ";
                    //else
                    //    LineTrafficControlEventHandler("     Window Size Tx = " + this.WSTx.ToString() + ", Window Size Rx = " + this.WSRx.ToString() + ", Info Field Size Tx = " + this.ISTx.ToString() + ", Info Field Size Rx = " + this.ISRx.ToString() + "\r\n\r\n", "Receive");
                }
                if (isSuccessfull)
                {
                    nRetVal = this.AARQ(IsLineTrafficEnabled);
                    switch (nRetVal)
                    {
                        case 0:
                            isSuccessfull = true;
                            break;
                        case 1:
                            text = " Association Fail.";
                            isSuccessfull = false;
                            break;
                        case 2:
                            text = " Authentication Fail.";
                            isSuccessfull = false;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                isSuccessfull = false;
                log.Error(ex.Message.ToString());
                CommonHelper.signOnErrors = text;
                return isSuccessfull;
            }
            CommonHelper.signOnErrors = text;
            return isSuccessfull;
        }
        public bool SignOnDLMSTrigger()
        {
            int nRetVal = 100;
            int num3 = 100;
            string text = string.Empty;
            string str1;
            //this.strErrorFile = "\\Genus\\Gxt\\INDALI\\" + DateTime.Now.ToString("ddmmyyyyHHmmss") + "_ERROR.txt";
            //this.strLTFile = "\\Genus\\Gxt\\INDALI\\" + DateTime.Now.ToString("ddmmyyyyHHmmss") + "_LINETRAFFIC.txt";
            //this.swError = new StreamWriter(this.strErrorFile);
            //this.swLT = new StreamWriter(this.strLTFile);
            bool isSuccessfull = true;
            if (isSuccessfull)
            {
                //Wait(100);
                AddressInit(DLMSInfo.AccessMode);
                bigSNRM = false;
                WSTx = Convert.ToByte(DLMSInfo.WSTx);
                WSRx = Convert.ToByte(DLMSInfo.WSRx);
                //if (DLMSInfo.IFTx != "Default")
                //    this.ISTx = (int)Convert.ToInt16(DLMSInfo.IFTx);
                //if (DLMSInfo.IFRx != "Default")
                //    this.ISRx = (int)Convert.ToInt16(DLMSInfo.IFRx);
                //if (DLMSInfo.IFTx != "Default")
                //    bigSNRM = true;
                if (DLMSInfo.WSTx != "1" || DLMSInfo.WSRx != "1")
                    bigSNRM = true;
                if (DLMSInfo.IFTx != "Default")
                {
                    this.ISTx = (int)Convert.ToInt16(DLMSInfo.IFTx);
                    bigSNRM = true;
                }
                else
                    this.ISTx = 128;
                if (DLMSInfo.IFRx != "Default")
                {
                    this.ISRx = (int)Convert.ToInt16(DLMSInfo.IFRx);
                    bigSNRM = true;
                }
                else
                    this.ISRx = 128;
                isSuccessfull = SetNRM(0, (byte)3, 3);
                if (!isSuccessfull)
                    text = "Meter Not Responding Properly, Please Try Again. ";
                //else if (bigSNRM)
                //{
                //    //int num7 = (int)MessageBox.Show(" Window Size Tx = " + this.WSTx.ToString() + "\r\n Window Size Rx = " + this.WSRx.ToString() + "\r\n Info Field Size Tx = " + this.ISTx.ToString() + "\r\n Info Field Size Rx = " + this.ISRx.ToString());
                //    LineTrafficControlEventHandler("     Window Size Tx = " + this.WSTx.ToString() + ", Window Size Rx = " + this.WSRx.ToString() + ", Info Field Size Tx = " + this.ISTx.ToString() + ", Info Field Size Rx = " + this.ISRx.ToString() + "\r\n\r\n", "Receive");
                //}
            }
            if (isSuccessfull)
            {
                nRetVal = this.AARQ(Convert.ToByte(DLMSInfo.AccessMode), DLMSInfo.MeterAuthPasswordWrite, 0, (byte)3, (byte)3);
                switch (nRetVal)
                {
                    case 0:
                        isSuccessfull = true;
                        break;
                    case 1:
                        text = "Association Fail.";
                        isSuccessfull = false;
                        break;
                    case 2:
                        text = "Authentication Fail.";
                        isSuccessfull = false;
                        break;
                }
            }
            CommonHelper.signOnErrors = text;
            return isSuccessfull;
        }
        public bool SignOnDLMSFG()
        {
            int nRetVal = 100;
            int num3 = 100;
            string text = string.Empty;
            string str1;
            //this.strErrorFile = "\\Genus\\Gxt\\INDALI\\" + DateTime.Now.ToString("ddmmyyyyHHmmss") + "_ERROR.txt";
            //this.strLTFile = "\\Genus\\Gxt\\INDALI\\" + DateTime.Now.ToString("ddmmyyyyHHmmss") + "_LINETRAFFIC.txt";
            //this.swError = new StreamWriter(this.strErrorFile);
            // this.swLT = new StreamWriter(this.strLTFile);
            bool isSuccessfull = true;
            if (isSuccessfull)
            {
                //Wait(100);
                AddressInitFG();
                //bigSNRM = false;
                //WSTx = Convert.ToByte(DLMSInfo.WSTx);
                //WSRx = Convert.ToByte(DLMSInfo.WSRx);
                //if (DLMSInfo.IFTx != "Default")
                //    this.ISTx = (int)Convert.ToInt16(DLMSInfo.IFTx);
                //if (DLMSInfo.IFRx != "Default")
                //    this.ISRx = (int)Convert.ToInt16(DLMSInfo.IFRx);
                //if (DLMSInfo.IFTx != "Default")
                //    bigSNRM = true;
                isSuccessfull = SetNRMFG(5, (byte)3, 3);
                if (!isSuccessfull)
                    text = "Meter Not Responding Properly, Please Try Again. ";
                else if (bigSNRM)
                {
                    //int num7 = (int)MessageBox.Show(" Window Size Tx = " + this.WSTx.ToString() + "\r\n Window Size Rx = " + this.WSRx.ToString() + "\r\n Info Field Size Tx = " + this.ISTx.ToString() + "\r\n Info Field Size Rx = " + this.ISRx.ToString());
                    LineTrafficControlEventHandler("     Window Size Tx = " + this.WSTx.ToString() + ", Window Size Rx = " + this.WSRx.ToString() + ", Info Field Size Tx = " + this.ISTx.ToString() + ", Info Field Size Rx = " + this.ISRx.ToString() + "\r\n\r\n", "Receive");

                }

            }
            if (isSuccessfull)
            {
                //Wait(100);
                nRetVal = AARQFG((byte)5, (byte)3, (byte)3);
                switch (nRetVal)
                {
                    case 0:
                        isSuccessfull = true;
                        break;
                    case 1:
                        text = "Association Fail.";
                        isSuccessfull = false;
                        break;
                    case 2:
                        text = "Authentication Fail.";
                        isSuccessfull = false;
                        break;
                }
            }
            CommonHelper.signOnErrors = text;
            return isSuccessfull;
        }

        public bool SignOnDLMS_PUSH(bool IsLineTrafficEnabled = true)
        {
            int nRetVal = 100;
            int num3 = 100;
            string text = string.Empty;
            string str1;
            //this.strErrorFile = "\\Genus\\Gxt\\INDALI\\" + DateTime.Now.ToString("ddmmyyyyHHmmss") + "_ERROR.txt";
            //this.strLTFile = "\\Genus\\Gxt\\INDALI\\" + DateTime.Now.ToString("ddmmyyyyHHmmss") + "_LINETRAFFIC.txt";
            //this.swError = new StreamWriter(this.strErrorFile, true);
            //this.swLT = new StreamWriter(this.strLTFile, true);
            bool isSuccessfull = true;
            try
            {
                if (isSuccessfull)
                {
                    //Wait(100);
                    AddressInit(DLMSInfo.AccessMode);
                    bigSNRM = false;
                    WSTx = Convert.ToByte(DLMSInfo.WSTx);
                    WSRx = Convert.ToByte(DLMSInfo.WSRx);
                    if (DLMSInfo.IFTx != "Default")
                        this.ISTx = (int)Convert.ToInt16(DLMSInfo.IFTx);
                    if (DLMSInfo.IFRx != "Default")
                        this.ISRx = (int)Convert.ToInt16(DLMSInfo.IFRx);
                    if (DLMSInfo.IFTx != "Default")
                        bigSNRM = true;
                    isSuccessfull = SetNRM_PUSH(bigSNRM, IsLineTrafficEnabled);
                    if (DLMSInfo.AccessMode == 3)
                        return isSuccessfull;
                    if (!isSuccessfull)
                        text = "Meter Not Responding Properly, Please Try Again. ";
                    else if (bigSNRM)
                    {
                        LineTrafficControlEventHandler("     Window Size Tx = " + this.WSTx.ToString() + ", Window Size Rx = " + this.WSRx.ToString() + ", Info Field Size Tx = " + this.ISTx.ToString() + ", Info Field Size Rx = " + this.ISRx.ToString() + "\r\n\r\n", "Receive");
                        //int num7 = (int)MessageBox.Show(" Window Size Tx = " + this.WSTx.ToString() + "\r\n Window Size Rx = " + this.WSRx.ToString() + "\r\n Info Field Size Tx = " + this.ISTx.ToString() + "\r\n Info Field Size Rx = " + this.ISRx.ToString());
                    }
                }
                if (isSuccessfull)
                {
                    nRetVal = this.AARQ(IsLineTrafficEnabled);
                    switch (nRetVal)
                    {
                        case 0:
                            isSuccessfull = true;
                            break;
                        case 1:
                            text = "Association Fail.";
                            isSuccessfull = false;
                            break;
                        case 2:
                            text = "Authentication Fail.";
                            isSuccessfull = false;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                isSuccessfull = false;
                log.Error(ex.Message.ToString());
                CommonHelper.signOnErrors = text;
                return isSuccessfull;
            }
            CommonHelper.signOnErrors = text;
            return isSuccessfull;
        }
        public bool MeterFGCommand(byte bType)
        {
            byte num1 = 8;
            this.nPkt[2] = (byte)25;
            this.nRetLSH = Convert.ToByte((int)this.nRecvCntr << 5);
            this.nPkt[5] = Convert.ToByte((int)this.nRetLSH | 16);
            this.nRetLSH = Convert.ToByte((int)this.nSentCntr << 1);
            this.nPkt[5] = Convert.ToByte((int)this.nRetLSH | (int)this.nPkt[5]);
            this.DCl.fcs(ref this.nPkt, (byte)5, (byte)1);
            byte[] nPkt1 = this.nPkt;
            int index1 = (int)num1;
            byte num2 = (byte)(index1 + 1);
            nPkt1[index1] = (byte)230;
            byte[] nPkt2 = this.nPkt;
            int index2 = (int)num2;
            byte num3 = (byte)(index2 + 1);
            nPkt2[index2] = (byte)230;
            byte[] nPkt3 = this.nPkt;
            int index3 = (int)num3;
            byte num4 = (byte)(index3 + 1);
            nPkt3[index3] = (byte)0;
            byte[] nPkt4 = this.nPkt;
            int index4 = (int)num4;
            byte num5 = (byte)(index4 + 1);
            nPkt4[index4] = (byte)195;
            byte[] nPkt5 = this.nPkt;
            int index5 = (int)num5;
            byte num6 = (byte)(index5 + 1);
            nPkt5[index5] = (byte)1;
            byte[] nPkt6 = this.nPkt;
            int index6 = (int)num6;
            byte num7 = (byte)(index6 + 1);
            nPkt6[index6] = (byte)129;
            byte[] nPkt7 = this.nPkt;
            int index7 = (int)num7;
            byte num8 = (byte)(index7 + 1);
            nPkt7[index7] = (byte)0;
            byte[] nPkt8 = this.nPkt;
            int index8 = (int)num8;
            byte num9 = (byte)(index8 + 1);
            nPkt8[index8] = (byte)9;
            byte[] nPkt9 = this.nPkt;
            int index9 = (int)num9;
            byte num10 = (byte)(index9 + 1);
            nPkt9[index9] = (byte)0;
            byte[] nPkt10 = this.nPkt;
            int index10 = (int)num10;
            byte num11 = (byte)(index10 + 1);
            nPkt10[index10] = (byte)0;
            byte[] nPkt11 = this.nPkt;
            int index11 = (int)num11;
            byte num12 = (byte)(index11 + 1);
            nPkt11[index11] = (byte)10;
            byte[] nPkt12 = this.nPkt;
            int index12 = (int)num12;
            byte num13 = (byte)(index12 + 1);
            nPkt12[index12] = (byte)0;
            byte[] nPkt13 = this.nPkt;
            int index13 = (int)num13;
            byte num14 = (byte)(index13 + 1);
            nPkt13[index13] = (byte)0;
            byte[] nPkt14 = this.nPkt;
            int index14 = (int)num14;
            byte num15 = (byte)(index14 + 1);
            nPkt14[index14] = byte.MaxValue;
            byte[] nPkt15 = this.nPkt;
            int index15 = (int)num15;
            byte num16 = (byte)(index15 + 1);
            int num17 = (int)bType;
            nPkt15[index15] = (byte)num17;
            byte[] nPkt16 = this.nPkt;
            int index16 = (int)num16;
            byte num18 = (byte)(index16 + 1);
            nPkt16[index16] = (byte)0;
            this.DCl.fcs(ref this.nPkt, Convert.ToByte((int)num18 - 1), (byte)1);
            this.nPkt[(int)num18 + 2] = (byte)126;
            this.ClearBuffer();
            bool flag = false;
            this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)num18 + 3));
            DateTime now = DateTime.Now;
            while (true)
            {
                this.Wait(100.0);
                Application.DoEvents();
                this.DataReceive();
                if (this.nCounter <= 2 || (int)this.nRcvPkt[2] + 2 > this.nCounter || this.nRcvPkt[(int)this.nRcvPkt[2] + 1] != (byte)126)
                {
                    if (DateTime.Now.Subtract(now).Seconds > 300)
                        goto label_5;
                }
                else
                    break;
            }
            flag = true;
            FrameType();
        label_5:
            return flag;
        }

        public bool SetDISCMode(bool IsLineTrafficEnabled = true)
        {
            if (IsLineTrafficEnabled)
                LineTrafficControlEventHandler($"     DISCONNECT COMMAND", "Send");
            bool flag = false;
            byte num1 = Convert.ToByte((int)this.bytAddMode + 5);
            this.nPkt[2] = Convert.ToByte((int)this.bytAddMode + 7);
            byte[] nPkt = this.nPkt;
            int index1 = (int)num1;
            byte num2 = (byte)(index1 + 1);
            nPkt[index1] = (byte)83;
            this.nPkt[(int)num2 + 2] = (byte)126;
            this.DCl.fcs(ref this.nPkt, Convert.ToByte((int)this.bytAddMode + 5), (byte)1);
            this.Wait(50.0);//old 1500
            this.ClearBuffer();
            this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)num2 + 3));
            if (IsLineTrafficEnabled)
                SendDataPrint(nPkt, Convert.ToByte((int)num2 + 3));
            commandString = string.Empty;
            for (int index2 = 0; index2 < (num2 + 3); ++index2)
                commandString += this.nPkt[index2].ToString("X2");
            DateTime now = DateTime.Now;
            while (true)
            {
                Application.DoEvents();
                this.DataReceive();
                if (this.nCounter <= 1 || (int)this.nRcvPkt[2] + 2 != this.nCounter || this.nRcvPkt[(int)this.nRcvPkt[2] + 1] != (byte)126)
                {
                    if (DateTime.Now.Subtract(now).Seconds > 10)
                        goto label_5;
                }
                else
                    break;
            }
            RecvDataPrint(nRcvPkt, nCounter, IsLineTrafficEnabled);
            for (int index31 = 0; index31 < this.nCounter; ++index31)
                responseString += this.nRcvPkt[index31].ToString("X2");
            flag = true;
            this.FrameType();
        label_5:
            this.temp = string.Empty;
            for (int index2 = 0; index2 < this.nCounter; ++index2)
                this.temp += this.nRcvPkt[index2].ToString("X2");
            this.temp += "\r\n";
            //if (flag && (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] == (byte)115 || this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] == (byte)31))//new condition
            if (flag && this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] == (byte)115)
            {
                if (IsLineTrafficEnabled)
                    LineTrafficControlEventHandler($"     SUCCESSFULLY DISCONNECTED\r\n\r\n", "Response");
            }
            else if (flag && this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] == (byte)31)
            {
                if (IsLineTrafficEnabled)
                    LineTrafficControlEventHandler($"     ALREADY DISCONNECTED\r\n\r\n", "Response");
            }
            else
            {
                if (IsLineTrafficEnabled)
                    LineTrafficControlEventHandler($"     ERROR IN DISCONNECTION\r\n\r\n", "Response");
            }
            return flag && (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] == (byte)115 || this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] == (byte)31);
        }
        public DataTable GetNameplateDataTable(string ProfileObis, string Profile = "null")
        {
            // Create a DataTable
            DataTable obisDataTable = new DataTable();
            DataTable resultDataTable = new DataTable();
            string recivedObisString = string.Empty;
            string recivedValueString = string.Empty;
            bool result = false;
            result = false;
            strbldDLMdata.Clear();
            result = GetParameter("000700005E5B0AFF03", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
            if (result == false)
            {
                log.Error($"Error Getting {Profile} Att. - 3. Received data is: {strbldDLMdata.ToString().Trim()}");
                return obisDataTable;
            }
            else
                recivedObisString = strbldDLMdata.ToString().Trim();
            strbldDLMdata.Clear();

            result = false;
            result = GetParameter("000700005E5B0AFF02", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
            if (result == false)
            {
                log.Error($"Error Getting {Profile} Att. - 2. Received data is: {strbldDLMdata.ToString().Trim()}");
                return obisDataTable;
            }
            else
                recivedValueString = strbldDLMdata.ToString().Trim();
            strbldDLMdata.Clear();

            DLMSParser parse = new DLMSParser();
            // Add columns to the DataTable
            obisDataTable.Columns.Add("Obis", typeof(string));
            obisDataTable.Columns.Add("Name", typeof(string));
            obisDataTable.Columns.Add("Data", typeof(string));
            obisDataTable = parse.GetClassObisAttScalerListParsing(recivedObisString, Profile);
            resultDataTable = parse.GetNameplateValueandDataTableParsing(recivedValueString, obisDataTable);
            FormatGrid(Profile, resultDataTable);
            PB_PGUpdate(25);
            resultDataTable.Columns.Add("Individual-Data", typeof(string));
            resultDataTable.Columns.Add("Individual-Value", typeof(string));
            resultDataTable.Columns.Add("Individual-Scaler", typeof(string));
            int individualDataColumnIndex = FindColumnIndexByName(dataGridView, "Individual-Data");
            int individualScalerColumnIndex = FindColumnIndexByName(dataGridView, "Individual-Scaler");
            int individualValueColumnIndex = FindColumnIndexByName(dataGridView, "Individual-Value");
            FormatGrid(Profile, resultDataTable);
            string[] class_Instantaneous = resultDataTable.AsEnumerable()
                                             .Select(row => row.Field<string>(1)) // Change the type to match the column's type
                                             .ToArray();
            string[] obis_Instantaneous = resultDataTable.AsEnumerable()
                                     .Select(row => row.Field<string>(2)) // Change the type to match the column's type
                                     .ToArray();
            string[] attribute_Instantaneous = resultDataTable.AsEnumerable()
                                     .Select(row => row.Field<string>(3)) // Change the type to match the column's type
                                     .ToArray();
            string dataIndividual = string.Empty;
            string scalerIndividual = string.Empty;
            string valueIndividual = string.Empty;
            PB_PGUpdate(50);
            for (int i = 0; i < resultDataTable.Rows.Count; i++)
            {
                result = false;
                result = GetParameter($"{Convert.ToByte(class_Instantaneous[i].Trim()).ToString("X4")}{string.Concat(obis_Instantaneous[i].Trim().Split('.').Select(part => int.Parse(part).ToString("X2")))}{Convert.ToByte(attribute_Instantaneous[i].Trim()).ToString("X2")}", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                if (result == true)
                {
                    dataIndividual = strbldDLMdata.ToString().Trim().Split(' ')[3];
                    dataGridView.Rows[i].Cells[individualDataColumnIndex].Value = strbldDLMdata.ToString().Trim().Split(' ')[3];
                    valueIndividual = parse.GetProfileValueString(dataIndividual);
                    dataGridView.Rows[i].Cells[individualValueColumnIndex].Value = valueIndividual;
                }
                else
                {
                    log.Error($"Error Getting Individual Object for {Profile} Class - {class_Instantaneous[i].Trim()} Obis - {obis_Instantaneous[i].Trim()} Attribute - {attribute_Instantaneous[i].Trim()}. Received data is: {strbldDLMdata.ToString().Trim()}");
                    //MessageBox.Show($"Error Getting Individual Object for {Profile} \nClass - {class_Instantaneous[i].Trim()} \nObis - {obis_Instantaneous[i].Trim()} \nAttribute - {attribute_Instantaneous[i].Trim()}.", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
                strbldDLMdata.Clear();
                if (i > 0)
                {
                    result = false;
                    result = GetParameter($"{Convert.ToByte(class_Instantaneous[i].Trim()).ToString("X4")}{string.Concat(obis_Instantaneous[i].Trim().Split('.').Select(part => int.Parse(part).ToString("X2")))}{Convert.ToByte(3).ToString("X2")}", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                    if (result == true)
                    {
                        scalerIndividual = strbldDLMdata.ToString().Trim().Split(' ')[3];
                        if (scalerIndividual == "0B")
                        {
                            dataGridView.Rows[i].Cells[individualScalerColumnIndex].Value = "";
                            scalerIndividual = "";
                            valueIndividual = parse.GetProfileValueString(dataIndividual);
                            dataGridView.Rows[i].Cells[individualValueColumnIndex].Value = valueIndividual;
                        }
                        else if (scalerIndividual.Substring(0, 4) == "0202")
                        {

                            scalerIndividual = scalerIndividual.Substring(4);
                            if ((obis_Instantaneous[i].Trim() == "1.0.1.6.0.255" && obis_Instantaneous[i].Trim() == "5") || (obis_Instantaneous[i].Trim() == "1.0.9.6.0.255" && obis_Instantaneous[i].Trim() == "5"))//MD-W(Imp) Date Time and MD-VA(Imp) Date Time
                            {
                                scalerIndividual = "";
                                dataGridView.Rows[i].Cells[individualScalerColumnIndex].Value = scalerIndividual;

                            }
                            else
                            {
                                dataGridView.Rows[i].Cells[individualScalerColumnIndex].Value = scalerIndividual;
                                if (string.IsNullOrEmpty((string)parse.UnithshTable[scalerIndividual.Trim().Substring(6, 2)]) && (string)parse.UnithshTable[scalerIndividual.Trim().Substring(6, 2)] == "1")
                                {
                                    valueIndividual = parse.GetProfileValueString(dataIndividual);
                                    dataGridView.Rows[i].Cells[individualValueColumnIndex].Value = valueIndividual;
                                }
                                else
                                {
                                    try
                                    {
                                        valueIndividual = parse.GetProfileValueString(dataIndividual);
                                        double scaledValue = Convert.ToDouble((string)parse.ScalerhshTable[scalerIndividual.Trim().Substring(2, 2)]) * Convert.ToDouble(valueIndividual);
                                        dataGridView.Rows[i].Cells[individualValueColumnIndex].Value = scaledValue.ToString();
                                        valueIndividual = scaledValue.ToString();
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex.ToString());
                                        //MessageBox.Show(ex.Message, "ERROR");
                                        dataGridView.Rows[i].Cells[individualValueColumnIndex].Value = valueIndividual;
                                    }

                                }
                            }
                        }
                        else
                        {
                            dataGridView.Rows[i].Cells[individualScalerColumnIndex].Value = scalerIndividual;
                            valueIndividual = parse.GetProfileValueString(dataIndividual);
                            dataGridView.Rows[i].Cells[individualValueColumnIndex].Value = valueIndividual;
                        }
                    }
                    else
                    {
                        log.Error($"Error Getting Individual Object for {Profile} Class - {class_Instantaneous[i].Trim()} Obis - {obis_Instantaneous[i].Trim()} Attribute - {3}. Received data is: {strbldDLMdata.ToString().Trim()}");
                        //MessageBox.Show($"Error Getting Individual Object for {Profile} \nClass - {class_Instantaneous[i].Trim()} \nObis - {obis_Instantaneous[i].Trim()} \nAttribute - {3}.", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                    strbldDLMdata.Clear();
                }
                PB_PGUpdate(MapRange(i, 0, resultDataTable.Rows.Count, 50, 90));
            }
            dataGridView.Invalidate();
            resultDataTable = ConvertDataGridViewToDataTable(dataGridView);
            return resultDataTable;
        }
        public DataTable ReadProfileGenericEvents(string option = "Power Related Events", int _startIndex = 0, int _endIndex = 0, byte nType = 2)
        {
            // Create a DataTable
            DataTable obisDataTable = new DataTable();
            // Add columns to the DataTable
            obisDataTable.Columns.Add("Obis", typeof(string));
            obisDataTable.Columns.Add("Name", typeof(string));
            obisDataTable.Columns.Add("Data", typeof(string));
            DataTable resultDataTable = new DataTable();
            DLMSParser parse = new DLMSParser(dataGridView);
            string recivedObisString = string.Empty;
            string recivedValueString = string.Empty;
            string recivedScalerObisString = string.Empty;
            string recivedScalerValueString = string.Empty;
            bool result = false;
            string[] scalerObisArray = null;
            string[] scalerScalerDataArray = null;
            string[] mainSourceObisArray = null;
            string[] finalScalerValuesToFill = null;
            string[] ScalerMultiFactorArray = null;
            result = false;
            switch (option)
            {
                case "Voltage Related Events":
                    result = GetParameter("00070000636200FF03", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                    break;
                case "Current Related Events":
                    result = GetParameter("00070000636201FF03", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                    break;
                case "Power Related Events":
                    result = GetParameter("00070000636202FF03", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                    break;
                case "Transaction Events":
                    result = GetParameter("00070000636203FF03", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                    break;
                case "Other Tamper Events":
                    result = GetParameter("00070000636204FF03", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                    break;
                case "Non Roll Over Events":
                    result = GetParameter("00070000636205FF03", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                    break;
                case "Control Events":
                    result = GetParameter("00070000636206FF03", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                    break;
                case "Billing Profile":
                    result = GetParameter("00070100620100FF03", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                    break;
                case "Instantaneous":
                    result = GetParameter("000701005E5B00FF03", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                    break;
                case "Mode of Relay Operation Profile":
                    result = GetParameter("00070000636281FF03", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                    break;
            }
            PB_PGUpdate(20);
            if (result == false)
            {
                log.Error($"Error Getting {option} Att. - 3. Received data is: {strbldDLMdata.ToString().Trim()}");
                return obisDataTable;
            }
            else
                recivedObisString = strbldDLMdata.ToString().Trim();
            strbldDLMdata.Clear();
            if (recivedObisString.Split(' ')[3].Trim() == "0B")
            {
                log.Error($"Error Getting {option} Att. - 3. Received data is: {strbldDLMdata.ToString().Trim()}");
                MessageBox.Show($"{option} not present in this meter", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return obisDataTable;
            }
            obisDataTable = parse.GetClassObisAttScalerListParsing(recivedObisString, option);
            if (option == "Billing Profile")
            {
                dataGridView.DataSource = null;
                dataGridView.Columns.Clear();
                dataGridView.DataSource = obisDataTable;
                setRowNumber(dataGridView);
                dataGridView.ScrollBars = ScrollBars.Both;
                dataGridView.Columns[0].Width = 40;
                dataGridView.Columns[1].Width = 250;
                dataGridView.Columns[2].Width = 60;
                dataGridView.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView.Columns[3].Width = 180;
                dataGridView.Columns[4].Width = 60;
                dataGridView.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView.Columns[5].Width = 80;
                dataGridView.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView.Refresh();

                result = false;
                result = GetParameter("000701005E5B06FF03", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                if (result == false)
                {
                    log.Error($"Error Getting {option} Att. - 3. Received data is: {strbldDLMdata.ToString().Trim()}");
                    return obisDataTable;
                }
                else
                    recivedScalerObisString = strbldDLMdata.ToString().Trim();
                strbldDLMdata.Clear();
                result = false;

                result = GetParameter("000701005E5B06FF02", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                if (result == false)
                {
                    log.Error($"Error Getting {option} Att. - 2. Received data is: {strbldDLMdata.ToString().Trim()}");
                    return obisDataTable;
                }
                else
                    recivedScalerValueString = strbldDLMdata.ToString().Trim();
                strbldDLMdata.Clear();
                //scalerObisArray = new string[int.Parse(recivedScalerObisString.Substring(23, 2), NumberStyles.HexNumber)];
                scalerObisArray = parse.GetScalerProfileObisListParsing(recivedScalerObisString);
                scalerScalerDataArray = parse.GetScalerProfileScalerListParsing(recivedScalerValueString);
                mainSourceObisArray = obisDataTable.AsEnumerable()
                                         .Select(row => row.Field<string>(2)) // Change the type to match the third column's type
                                         .ToArray();
                finalScalerValuesToFill = new string[mainSourceObisArray.Length];
                foreach (var selectedObis in scalerObisArray)
                {
                    // Find the index of the searchString in the stringArray
                    int index = Array.IndexOf(mainSourceObisArray, selectedObis);
                    int scalarIndex = Array.IndexOf(scalerObisArray, selectedObis);
                    if (index == -1)
                    {
                        MessageBox.Show("Object present in Scaler Profile not available in Billing Profile", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        break;
                    }
                    if (index > mainSourceObisArray.Length)
                        break;
                    else
                    {
                        finalScalerValuesToFill[index] = scalerScalerDataArray[scalarIndex];
                        dataGridView.Rows[index].Cells[5].Value = scalerScalerDataArray[scalarIndex];
                    }
                }
                dataGridView.Refresh();
            }
            else if (option == "Instantaneous")
            {
                dataGridView.DataSource = null;
                dataGridView.Columns.Clear();
                dataGridView.DataSource = obisDataTable;
                setRowNumber(dataGridView);
                dataGridView.ScrollBars = ScrollBars.Both;
                foreach (DataGridViewColumn column in dataGridView.Columns)
                {
                    // Set alignment of cell data to center
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    column.Frozen = false;
                    //column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    if (column.Index <= 5)
                        column.Frozen = true;
                    else
                    {
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                        if (column.HeaderText.Contains("Data"))
                            column.Width = 140;
                        else if (column.HeaderText.Contains("Value"))
                            column.Width = 145;
                        else
                            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    }
                    if (column.HeaderText.Contains("Parameter") || column.HeaderText.Contains("Data"))
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                }
                dataGridView.Columns[0].Width = 40;
                dataGridView.Columns[1].Width = 180;
                dataGridView.Columns[2].Width = 45;
                dataGridView.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView.Columns[3].Width = 100;
                dataGridView.Columns[4].Width = 60;
                dataGridView.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView.Columns[5].Width = 70;
                dataGridView.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                result = false;
                result = GetParameter("000701005E5B03FF03", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                if (result == false)
                {
                    log.Error($"Error Getting {option} Att. - 3. Received data is: {strbldDLMdata.ToString().Trim()}");
                    return obisDataTable;
                }
                else
                    recivedScalerObisString = strbldDLMdata.ToString().Trim();
                strbldDLMdata.Clear();
                result = false;
                result = GetParameter("000701005E5B03FF02", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                if (result == false)
                {
                    log.Error($"Error Getting {option} Att. - 3. Received data is: {strbldDLMdata.ToString().Trim()}");
                    return obisDataTable;
                }
                else
                    recivedScalerValueString = strbldDLMdata.ToString().Trim();
                strbldDLMdata.Clear();
                //scalerObisArray = new string[int.Parse(recivedScalerObisString.Substring(23, 2), NumberStyles.HexNumber)];
                scalerObisArray = parse.GetScalerProfileObisListParsing(recivedScalerObisString);
                scalerScalerDataArray = parse.GetScalerProfileScalerListParsing(recivedScalerValueString);
                mainSourceObisArray = obisDataTable.AsEnumerable()
                                         .Select(row => row.Field<string>(2)) // Change the type to match the third column's type
                                         .ToArray();
                finalScalerValuesToFill = new string[mainSourceObisArray.Length];
                ScalerMultiFactorArray = new string[mainSourceObisArray.Length];
                foreach (var selectedObis in scalerObisArray)
                {
                    // Find the index of the searchString in the stringArray
                    int index = Array.IndexOf(mainSourceObisArray, selectedObis);
                    int scalarIndex = Array.IndexOf(scalerObisArray, selectedObis);
                    if (index == -1)
                    {
                        MessageBox.Show("Object present in Scaler Profile not available in Billing Profile", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        break;
                    }
                    if (index > mainSourceObisArray.Length)
                        break;
                    else
                    {
                        finalScalerValuesToFill[index] = scalerScalerDataArray[scalarIndex];
                        dataGridView.Rows[index].Cells[5].Value = scalerScalerDataArray[scalarIndex];
                    }
                }
                //dataGridView.Refresh();
            }
            else if (option.Contains("Events") || option == "Mode of Relay Operation Profile")
            {
                result = false;
                if (option == "Mode of Relay Operation Profile")
                    result = GetParameter("0007000063629BFF03", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                else
                    result = GetParameter("000701005E5B07FF03", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                if (result == false)
                {
                    log.Error($"Error Getting {option} Att. - 3. Received data is: {strbldDLMdata.ToString().Trim()}");
                    return obisDataTable;
                }
                else
                    recivedScalerObisString = strbldDLMdata.ToString().Trim();
                strbldDLMdata.Clear();
                result = false;
                if (option == "Mode of Relay Operation Profile")
                    result = GetParameter("0007000063629BFF02", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                else
                    result = GetParameter("000701005E5B07FF02", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                if (result == false)
                {
                    log.Error($"Error Getting {option} Att. - 3. Received data is: {strbldDLMdata.ToString().Trim()}");
                    return obisDataTable;
                }
                else
                    recivedScalerValueString = strbldDLMdata.ToString().Trim();
                strbldDLMdata.Clear();
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
                    if (index == -1)
                    {

                    }
                    else
                    {
                        finalScalerValuesToFill[index] = scalerScalerDataArray[scalarIndex];
                        ScalerMultiFactorArray[index] = (string)parse.ScalerhshTable[scalerScalerDataArray[scalarIndex].Substring(2, 2)];
                        obisDataTable.Columns[index].ColumnName = (!string.IsNullOrEmpty((string)parse.UnithshTable[scalerScalerDataArray[scalarIndex].Substring(6, 2)])) ?
                                                                $"{obisDataTable.Columns[index].ColumnName} - {scalerScalerDataArray[scalarIndex]} - ({(string)parse.UnithshTable[scalerScalerDataArray[scalarIndex].Substring(6, 2)]})" :
                                                                $"{obisDataTable.Columns[index].ColumnName} - {scalerScalerDataArray[scalarIndex]}";
                    }
                }
                PB_PGUpdate(30);
                if (option == "Power Related Events")
                {
                    FormatGrid(option, obisDataTable);
                }
                else if (option == "Transaction Events")
                {
                    FormatGrid(option, obisDataTable);
                }
                else if (option == "Other Tamper Events")
                {
                    FormatGrid(option, obisDataTable);
                }
                else if (option == "Non Roll Over Events")
                {
                    FormatGrid(option, obisDataTable);
                }
                else if (option == "Control Events")
                {
                    FormatGrid(option, obisDataTable);
                }
                else
                {
                    dataGridView.DataSource = null;
                    dataGridView.Columns.Clear();
                    dataGridView.DataSource = obisDataTable;
                    setRowNumber(dataGridView);
                    dataGridView.ScrollBars = ScrollBars.Both;
                    foreach (DataGridViewColumn column in dataGridView.Columns)
                    {
                        // Set alignment of cell data to center
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        column.Frozen = false;
                        //column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        column.Width = 170;
                        //column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                        if (column.Index < 2)
                            column.Frozen = true;
                    }
                    dataGridView.Columns[0].Width = 40;
                    dataGridView.Refresh();
                }
            }
            result = false;
            PB_PGUpdate(40);
            switch (option)
            {
                case "Voltage Related Events":
                    result = GetParameter("00070000636200FF02", nWait, nTryCount, nTimeOut, nType, DateTime.Now, DateTime.Now, string.Empty, (ulong)_startIndex, (ulong)_endIndex);
                    //result = GetSelectiveParameter(7, "0000636200FF", 2, bytWait, 3, bytTimOut, option, Convert.ToString(_startIndex), Convert.ToString(_endIndex), null, null);
                    break;
                case "Current Related Events":
                    result = GetParameter("00070000636201FF02", nWait, nTryCount, nTimeOut, nType, DateTime.Now, DateTime.Now, string.Empty, (ulong)_startIndex, (ulong)_endIndex);
                    //    result = GetSelectiveParameter(7, "0000636201FF", 2, bytWait, 3, bytTimOut, option, Convert.ToString(_startIndex), Convert.ToString(_endIndex), null, null);
                    break;
                case "Power Related Events":
                    result = GetParameter("00070000636202FF02", nWait, nTryCount, nTimeOut, nType, DateTime.Now, DateTime.Now, string.Empty, (ulong)_startIndex, (ulong)_endIndex);
                    //result = GetSelectiveParameter(7, "0000636202FF", 2, bytWait, 3, bytTimOut, option, Convert.ToString(_startIndex), Convert.ToString(_endIndex), null, null);
                    break;
                case "Transaction Events":
                    result = GetParameter("00070000636203FF02", nWait, nTryCount, nTimeOut, nType, DateTime.Now, DateTime.Now, string.Empty, (ulong)_startIndex, (ulong)_endIndex);
                    //result = GetSelectiveParameter(7, "0000636203FF", 2, bytWait, 3, bytTimOut, option, Convert.ToString(_startIndex), Convert.ToString(_endIndex), null, null);
                    break;
                case "Other Tamper Events":
                    result = GetParameter("00070000636204FF02", nWait, nTryCount, nTimeOut, nType, DateTime.Now, DateTime.Now, string.Empty, (ulong)_startIndex, (ulong)_endIndex);
                    //result = GetSelectiveParameter(7, "0000636204FF", 2, bytWait, 3, bytTimOut, option, Convert.ToString(_startIndex), Convert.ToString(_endIndex), null, null);
                    break;
                case "Non Roll Over Events":
                    result = GetParameter("00070000636205FF02", nWait, nTryCount, nTimeOut, nType, DateTime.Now, DateTime.Now, string.Empty, (ulong)_startIndex, (ulong)_endIndex);
                    //result = GetSelectiveParameter(7, "0000636205FF", 2, bytWait, 3, bytTimOut, option, Convert.ToString(_startIndex), Convert.ToString(_endIndex), null, null);
                    break;
                case "Control Events":
                    result = GetParameter("00070000636206FF02", nWait, nTryCount, nTimeOut, nType, DateTime.Now, DateTime.Now, string.Empty, (ulong)_startIndex, (ulong)_endIndex);
                    //result = GetSelectiveParameter(7, "0000636206FF", 2, bytWait, 3, bytTimOut, option, Convert.ToString(_startIndex), Convert.ToString(_endIndex), null, null);
                    break;
                case "Billing Profile":
                    result = GetParameter("00070100620100FF02", nWait, nTryCount, nTimeOut, nType, DateTime.Now, DateTime.Now, string.Empty, (ulong)_startIndex, (ulong)_endIndex);
                    //result = GetSelectiveParameter(7, "0100620100FF", 2, bytWait, 3, bytTimOut, option, Convert.ToString(_startIndex), Convert.ToString(_endIndex), null, null);
                    break;
                case "Instantaneous":
                    result = GetParameter("000701005E5B00FF02", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                    //result = GetSelectiveParameter(7, "01005E5B00FF", 2, bytWait, 3, bytTimOut, option, Convert.ToString(_startIndex), Convert.ToString(_endIndex), null, null);
                    break;
                case "Mode of Relay Operation Profile":
                    result = GetParameter("00070000636281FF02", nWait, nTryCount, nTimeOut, nType, DateTime.Now, DateTime.Now, string.Empty, (ulong)_startIndex, (ulong)_endIndex);
                    //result = GetSelectiveParameter(7, "0000636281FF", 2, bytWait, 3, bytTimOut, option, Convert.ToString(_startIndex), Convert.ToString(_endIndex), null, null);
                    break;
            }
            //result = GetSelectiveParameter(7, "0000636202FF", 2, bytWait, 3, bytTimOut, option, Convert.ToString(_startIndex), Convert.ToString(_endIndex), null, null);
            if (result == false)
            {
                log.Error($"Error Getting {option} Selective Data of Att. - 2. Received data is: {strbldDLMdata.ToString().Trim()}");
                return resultDataTable;
            }
            else
                recivedValueString = strbldDLMdata.ToString().Trim();
            strbldDLMdata.Clear();
            PB_PGUpdate(50);
            if (option == "Billing Profile" || option == "Instantaneous")
            {
                resultDataTable = parse.GetBillingValuesDataTableParsing(recivedValueString, obisDataTable);
                // Multiply each row value with the corresponding values in "Data" columns
                RenameParameterWithUnit(resultDataTable, parse);
                if (option == "Billing Profile")
                    //MultiplyScalerWithData(resultDataTable, parse);
                    FormatGrid(option, resultDataTable);
                if (option == "Instantaneous")
                {
                    resultDataTable.Columns.Add("Individual-Data", typeof(string));
                    resultDataTable.Columns.Add("Individual-Value", typeof(string));
                    resultDataTable.Columns.Add("Individual-Scaler", typeof(string));
                    int individualDataColumnIndex = FindColumnIndexByName(dataGridView, "Individual-Data");
                    int individualScalerColumnIndex = FindColumnIndexByName(dataGridView, "Individual-Scaler");
                    int individualValueColumnIndex = FindColumnIndexByName(dataGridView, "Individual-Value");
                    FormatGrid(option, resultDataTable);
                    //dataGridView.Refresh();
                    // Find the column index by column name
                    int referenceDataColumnIndex = FindColumnIndexByName(dataGridView, "Entry 1 Data");
                    int referenceValueColumnIndex = FindColumnIndexByName(dataGridView, "Entry 1 Value");
                    int referenceScalerColumnIndex = FindColumnIndexByName(dataGridView, "Scaler");

                    string[] class_Instantaneous = resultDataTable.AsEnumerable()
                                             .Select(row => row.Field<string>(1)) // Change the type to match the column's type
                                             .ToArray();
                    string[] obis_Instantaneous = resultDataTable.AsEnumerable()
                                             .Select(row => row.Field<string>(2)) // Change the type to match the column's type
                                             .ToArray();
                    string[] attribute_Instantaneous = resultDataTable.AsEnumerable()
                                             .Select(row => row.Field<string>(3)) // Change the type to match the column's type
                                             .ToArray();
                    string[] refData = resultDataTable.AsEnumerable()
                                             .Select(row => row.Field<string>(5)) // Change the type to match the column's type
                                             .ToArray();
                    string[] refScaler = resultDataTable.AsEnumerable()
                                             .Select(row => row.Field<string>(4)) // Change the type to match the column's type
                                             .ToArray();
                    string[] refValue = resultDataTable.AsEnumerable()
                                             .Select(row => row.Field<string>(6)) // Change the type to match the column's type
                                             .ToArray();
                    string dataIndividual = string.Empty;
                    string scalerIndividual = string.Empty;
                    string valueIndividual = string.Empty;
                    for (int i = 0; i < resultDataTable.Rows.Count; i++)
                    {
                        PB_PGUpdate(MapRange(i, 0, resultDataTable.Rows.Count, 50, 90));
                        //dataGridView.Rows[i].Cells[individualDataColumnIndex].Style.BackColor = System.Drawing.Color.LightBlue;
                        //dataGridView.Rows[i].Cells[individualScalerColumnIndex].Style.BackColor = System.Drawing.Color.LightBlue;
                        //dataGridView.Rows[i].Cells[individualValueColumnIndex].Style.BackColor = System.Drawing.Color.LightBlue;
                        result = false;
                        result = GetParameter($"{Convert.ToByte(class_Instantaneous[i].Trim()).ToString("X4")}{string.Concat(obis_Instantaneous[i].Trim().Split('.').Select(part => int.Parse(part).ToString("X2")))}{Convert.ToByte(attribute_Instantaneous[i].Trim()).ToString("X2")}", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                        if (result == true)
                        {
                            dataIndividual = strbldDLMdata.ToString().Trim().Split(' ')[3];
                            dataGridView.Rows[i].Cells[individualDataColumnIndex].Value = strbldDLMdata.ToString().Trim().Split(' ')[3];
                            valueIndividual = parse.GetProfileValueString(dataIndividual);
                            dataGridView.Rows[i].Cells[individualValueColumnIndex].Value = valueIndividual;
                            strbldDLMdata.Clear();
                        }
                        else
                        {
                            log.Error($"Error Getting Individual Object for {option} Class - {class_Instantaneous[i].Trim()} Obis - {obis_Instantaneous[i].Trim()} Attribute - {attribute_Instantaneous[i].Trim()}. Received data is: {strbldDLMdata.ToString().Trim()}");
                            MessageBox.Show($"Error Getting Individual Object for {option} \nClass - {class_Instantaneous[i].Trim()} \nObis - {obis_Instantaneous[i].Trim()} \nAttribute - {attribute_Instantaneous[i].Trim()}.", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                        if (i > 0 && (class_Instantaneous[i].Trim() == "3" || class_Instantaneous[i].Trim() == "4") && attribute_Instantaneous[i] == "2")
                        {
                            result = false;
                            result = GetParameter($"{Convert.ToByte(class_Instantaneous[i].Trim()).ToString("X4")}{string.Concat(obis_Instantaneous[i].Trim().Split('.').Select(part => int.Parse(part).ToString("X2")))}{Convert.ToByte(3).ToString("X2")}", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                            if (result == true)
                            {
                                scalerIndividual = strbldDLMdata.ToString().Trim().Split(' ')[3];
                                if (scalerIndividual == "0B")
                                {
                                    dataGridView.Rows[i].Cells[individualScalerColumnIndex].Value = "";
                                    scalerIndividual = "";
                                    valueIndividual = parse.GetProfileValueString(dataIndividual);
                                    dataGridView.Rows[i].Cells[individualValueColumnIndex].Value = valueIndividual.Trim();
                                }
                                else if (scalerIndividual.Substring(0, 4) == "0202")
                                {

                                    scalerIndividual = scalerIndividual.Substring(4);
                                    if ((mainSourceObisArray[i].Trim() == "1.0.1.6.0.255" && attribute_Instantaneous[i].Trim() == "5") || (mainSourceObisArray[i].Trim() == "1.0.9.6.0.255" && attribute_Instantaneous[i].Trim() == "5"))//MD-W(Imp) Date Time and MD-VA(Imp) Date Time
                                    {
                                        scalerIndividual = "";
                                        dataGridView.Rows[i].Cells[individualScalerColumnIndex].Value = scalerIndividual;

                                    }
                                    else
                                    {
                                        dataGridView.Rows[i].Cells[individualScalerColumnIndex].Value = scalerIndividual;
                                        if (string.IsNullOrEmpty((string)parse.UnithshTable[scalerIndividual.Trim().Substring(6, 2)]) && (string)parse.UnithshTable[scalerIndividual.Trim().Substring(6, 2)] == "1")
                                        {
                                            valueIndividual = parse.GetProfileValueString(dataIndividual);
                                            dataGridView.Rows[i].Cells[individualValueColumnIndex].Value = valueIndividual.Trim();
                                        }
                                        else
                                        {
                                            try
                                            {
                                                valueIndividual = parse.GetProfileValueString(dataIndividual);
                                                double scaledValue = Convert.ToDouble((string)parse.ScalerhshTable[scalerIndividual.Trim().Substring(2, 2)]) * Convert.ToDouble(valueIndividual);
                                                dataGridView.Rows[i].Cells[individualValueColumnIndex].Value = scaledValue.ToString();
                                                valueIndividual = scaledValue.ToString();
                                            }
                                            catch (Exception ex)
                                            {
                                                log.Error(ex.Message.ToString());
                                                //MessageBox.Show(ex.Message, "ERROR");
                                                dataGridView.Rows[i].Cells[individualValueColumnIndex].Value = valueIndividual;
                                            }

                                        }



                                    }



                                    //dataGridView.Rows[i].Cells[individualValueColumnIndex].Value = parse.GetProfileValueString(dataIndividual);

                                }
                                else
                                {
                                    dataGridView.Rows[i].Cells[individualScalerColumnIndex].Value = scalerIndividual;
                                    valueIndividual = parse.GetProfileValueString(dataIndividual);
                                    dataGridView.Rows[i].Cells[individualValueColumnIndex].Value = valueIndividual.Trim();
                                }
                            }
                            else
                            {
                                log.Error($"Error Getting Individual Object for {option} Class - {class_Instantaneous[i].Trim()} Obis - {obis_Instantaneous[i].Trim()} Attribute - {3}. Received data is: {strbldDLMdata.ToString().Trim()}");
                                MessageBox.Show($"Error Getting Individual Object for {option} \nClass - {class_Instantaneous[i].Trim()} \nObis - {obis_Instantaneous[i].Trim()} \nAttribute - {3}.", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            }
                            strbldDLMdata.Clear();
                        }

                    }
                }
            }
            else
            {
                resultDataTable = parse.GetEventsValuesDataTableParsing(recivedValueString, obisDataTable, option);
                PB_PGUpdate(60);
                // Multiply each row value with the corresponding string array value
                MultiplyRowsWithArray(resultDataTable, ScalerMultiFactorArray);
                if (option == "Power Related Events")
                {
                    FormatGrid(option, resultDataTable);
                }
                else if (option == "Transaction Events")
                {
                    FormatGrid(option, resultDataTable);
                }
                else if (option == "Other Tamper Events")
                {
                    FormatGrid(option, resultDataTable);
                }
                else if (option == "Non Roll Over Events")
                {
                    FormatGrid(option, resultDataTable);
                }
                else if (option == "Control Events")
                {
                    FormatGrid(option, resultDataTable);
                }
                else
                {
                    dataGridView.DataSource = null;
                    dataGridView.Columns.Clear();
                    dataGridView.DataSource = resultDataTable;
                    setRowNumber(dataGridView);
                    dataGridView.ScrollBars = ScrollBars.Both;
                    foreach (DataGridViewColumn column in dataGridView.Columns)
                    {
                        // Set alignment of cell data to center
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        column.Frozen = false;
                        //column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        column.Width = 170;
                        //column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                        if (column.Index < 2)
                            column.Frozen = true;
                    }
                    //dataGridView.Refresh();
                    dataGridView.Columns[0].Width = 40;
                }
                PB_PGUpdate(70);
            }
            PB_PGUpdate(80);
            dataGridView.Invalidate();
            resultDataTable = ConvertDataGridViewToDataTable(dataGridView);
            return resultDataTable;
        }
        public DataTable ReadLoadProfileProfileGenericVertical(string profile = "", string _startDT = null, string _endDT = null, string sOBISlist = "0100")
        {
            // Create a DataTable
            DataTable obisDataTable = new DataTable();
            // Add columns to the DataTable
            obisDataTable.Columns.Add("Obis", typeof(string));
            obisDataTable.Columns.Add("Name", typeof(string));
            obisDataTable.Columns.Add("Data", typeof(string));
            DataTable resultDataTable = new DataTable();
            DLMSParser parse = new DLMSParser(dataGridView);
            string recivedObisString = string.Empty;
            string recivedValueString = string.Empty;
            string recivedScalerObisString = string.Empty;
            string recivedScalerValueString = string.Empty;
            bool result = false;
            string[] scalerObisArray = null;
            string[] scalerScalerDataArray = null;
            string[] mainSourceObisArray = null;
            string[] finalScalerValuesToFill = null;
            string[] ScalerMultiFactorArray = null;
            result = false;
            switch (profile)
            {
                case "Load Survey Vertical":
                    result = GetParameter("00070100630100FF03", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                    //result = GetParameter2(7, "0100630100FF", 3, bytWait, 3, bytTimOut);
                    break;
                case "Load Survey Vertical-AllData":
                    result = GetParameter("00070100630100FF03", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                    //result = GetParameter2(7, "0100630100FF", 3, bytWait, 3, bytTimOut);
                    break;
                case "Daily Energy Vertical":
                    result = GetParameter("00070100630200FF03", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                    //result = GetParameter2(7, "0100630200FF", 3, bytWait, 3, bytTimOut);
                    break;
                case "Daily Energy Vertical-AllData":
                    result = GetParameter("00070100630200FF03", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                    //result = GetParameter2(7, "0100630200FF", 3, bytWait, 3, bytTimOut);
                    break;
            }
            if (result == false)
            {
                log.Error($"Error Getting {profile} Att. - 3. Received data is: {strbldDLMdata.ToString().Trim()}");
                return obisDataTable;
            }
            else
                recivedObisString = strbldDLMdata.ToString().Trim();
            strbldDLMdata.Clear();
            obisDataTable = parse.GetClassObisAttScalerListParsing(recivedObisString, profile);
            if (profile == "Load Survey Vertical" || profile == "Load Survey Vertical-AllData")
            {
                dataGridView.DataSource = null;
                dataGridView.Columns.Clear();
                dataGridView.DataSource = obisDataTable;
                setRowNumber(dataGridView);
                dataGridView.ScrollBars = ScrollBars.Both;
                foreach (DataGridViewColumn column in dataGridView.Columns)
                {
                    // Set alignment of cell data to center
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    column.Frozen = false;
                    //column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    //column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    if (column.Index < 2)
                        column.Frozen = true;
                    if (column.Index > 0)
                        column.Width = 160;
                }
                dataGridView.Columns[0].Width = 40;
                dataGridView.Refresh();
                result = false;
                result = GetParameter("000701005E5B04FF03", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                //result = GetParameter2(7, "01005E5B04FF", 3, bytWait, 3, bytTimOut);
                if (result == false)
                {
                    log.Error($"Error Getting {profile} Att. - 3. Received data is: {strbldDLMdata.ToString().Trim()}");
                    return obisDataTable;
                }
                else
                    recivedScalerObisString = strbldDLMdata.ToString().Trim();
                strbldDLMdata.Clear();
                result = false;
                result = GetParameter("000701005E5B04FF02", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                //result = GetParameter2(7, "01005E5B04FF", 2, bytWait, 3, bytTimOut);
                if (result == false)
                {
                    log.Error($"Error Getting {profile} Att. - 3. Received data is: {strbldDLMdata.ToString().Trim()}");
                    return obisDataTable;
                }
                else
                    recivedScalerValueString = strbldDLMdata.ToString().Trim();
                strbldDLMdata.Clear();

                scalerObisArray = parse.GetScalerProfileObisListParsing(recivedScalerObisString);
                scalerScalerDataArray = parse.GetScalerProfileScalerListParsing(recivedScalerValueString);
                string[] headerNames = new string[obisDataTable.Columns.Count];
                mainSourceObisArray = new string[obisDataTable.Columns.Count];
                for (int i = 0; i < obisDataTable.Columns.Count; i++)
                {
                    headerNames[i] = obisDataTable.Columns[i].ColumnName;
                    mainSourceObisArray[i] = headerNames[i].Split('-')[1].Trim();
                }
                finalScalerValuesToFill = new string[mainSourceObisArray.Length];
                ScalerMultiFactorArray = new string[mainSourceObisArray.Length];
                foreach (var selectedObis in scalerObisArray)
                {
                    // Find the index of the searchString in the stringArray
                    int index = Array.IndexOf(mainSourceObisArray, selectedObis);
                    int scalarIndex = Array.IndexOf(scalerObisArray, selectedObis);
                    if (index == -1)
                    {

                    }
                    else
                    {
                        finalScalerValuesToFill[index] = scalerScalerDataArray[scalarIndex];
                        ScalerMultiFactorArray[index] = (string)parse.ScalerhshTable[scalerScalerDataArray[scalarIndex].Substring(2, 2)];
                    }
                }
                // Iterate through the columns to get the header names
                foreach (DataColumn column in obisDataTable.Columns)
                {
                    int index = obisDataTable.Columns.IndexOf(column);
                    string headerName = column.ColumnName;
                    string unitText = (!string.IsNullOrEmpty(finalScalerValuesToFill[index])) ? (string)parse.UnithshTable[finalScalerValuesToFill[index].Substring(6, 2)] : "";
                    obisDataTable.Columns[headerName].ColumnName = (string.IsNullOrEmpty(unitText)) ? $"{headerName}" : $"{headerName} ({finalScalerValuesToFill[index]}) ({unitText})";
                }
                foreach (DataGridViewColumn column in dataGridView.Columns)
                {
                    // Set alignment of cell data to center
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    column.Frozen = false;
                    //column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    //column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    if (column.Index < 2)
                        column.Frozen = true;
                    if (column.Index > 0)
                        column.Width = 160;
                }
                dataGridView.Columns[0].Width = 40;
                dataGridView.Refresh();
            }
            if (profile == "Daily Energy Vertical" || profile == "Daily Energy Vertical-AllData")
            {
                dataGridView.DataSource = null;
                dataGridView.Columns.Clear();
                dataGridView.DataSource = obisDataTable;
                setRowNumber(dataGridView);
                dataGridView.ScrollBars = ScrollBars.Both;
                foreach (DataGridViewColumn column in dataGridView.Columns)
                {
                    // Set alignment of cell data to center
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    column.Frozen = false;
                    //column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    //column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    if (column.Index < 2)
                        column.Frozen = true;
                    if (column.Index > 0)
                        column.Width = 160;
                }
                dataGridView.Columns[0].Width = 40;
                dataGridView.Refresh();
                result = GetParameter("000701005E5B05FF03", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                //result = GetParameter2(7, "01005E5B05FF", 3, bytWait, 3, bytTimOut);
                if (result == false)
                {
                    log.Error($"Error Getting {profile} Att. - 3. Received data is: {strbldDLMdata.ToString().Trim()}");
                    return obisDataTable;
                }
                else
                    recivedScalerObisString = strbldDLMdata.ToString().Trim();
                strbldDLMdata.Clear();
                result = false;
                result = GetParameter("000701005E5B05FF02", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                //result = GetParameter2(7, "01005E5B05FF", 2, bytWait, 3, bytTimOut);
                if (result == false)
                {
                    log.Error($"Error Getting {profile} Att. - 3. Received data is: {strbldDLMdata.ToString().Trim()}");
                    return obisDataTable;
                }
                else
                    recivedScalerValueString = strbldDLMdata.ToString().Trim();
                strbldDLMdata.Clear();

                scalerObisArray = parse.GetScalerProfileObisListParsing(recivedScalerObisString);
                scalerScalerDataArray = parse.GetScalerProfileScalerListParsing(recivedScalerValueString);
                string[] headerNames = new string[obisDataTable.Columns.Count];
                mainSourceObisArray = new string[obisDataTable.Columns.Count];
                for (int i = 0; i < obisDataTable.Columns.Count; i++)
                {
                    mainSourceObisArray[i] = obisDataTable.Columns[i].ColumnName.Split('-')[1].Trim();
                }

                //for (int i = 0; i < obisDataTable.Columns.Count; i++)
                //{
                //    headerNames[i] = obisDataTable.Columns[i].ColumnName;
                //    mainSourceObisArray[i] = headerNames[i].Split('-')[1].Trim();
                //}
                finalScalerValuesToFill = new string[mainSourceObisArray.Length];
                ScalerMultiFactorArray = new string[mainSourceObisArray.Length];
                foreach (var selectedObis in scalerObisArray)
                {
                    // Find the index of the searchString in the stringArray
                    int index = Array.IndexOf(mainSourceObisArray, selectedObis);
                    int scalarIndex = Array.IndexOf(scalerObisArray, selectedObis);
                    if (index == -1)
                    {

                    }
                    else
                    {
                        finalScalerValuesToFill[index] = scalerScalerDataArray[scalarIndex];
                        if (string.IsNullOrEmpty(finalScalerValuesToFill[index]))
                        {
                            ScalerMultiFactorArray[index] = "";
                        }
                        else
                        {
                            ScalerMultiFactorArray[index] = (string)parse.ScalerhshTable[scalerScalerDataArray[scalarIndex].Substring(2, 2)];
                            obisDataTable.Columns[index].ColumnName = (!string.IsNullOrEmpty((string)parse.UnithshTable[scalerScalerDataArray[scalarIndex].Substring(6, 2)])) ?
                                                                    $"{obisDataTable.Columns[index].ColumnName} - {scalerScalerDataArray[scalarIndex]} - ({(string)parse.UnithshTable[scalerScalerDataArray[scalarIndex].Substring(6, 2)]})" :
                                                                    $"{obisDataTable.Columns[index].ColumnName} - {scalerScalerDataArray[scalarIndex]}";
                        }
                        //obisDataTable.Columns[index].ColumnName = (!string.IsNullOrEmpty((string)parse.UnithshTable[scalerScalerDataArray[scalarIndex].Substring(6, 2)])) ?
                        //                                            $"{obisDataTable.Columns[index].ColumnName} - {scalerScalerDataArray[scalarIndex]} - ({(string)parse.UnithshTable[scalerScalerDataArray[scalarIndex].Substring(6, 2)]})" :
                        //                                            $"{obisDataTable.Columns[index].ColumnName} - {scalerScalerDataArray[scalarIndex]}";

                    }
                }
                foreach (DataGridViewColumn column in dataGridView.Columns)
                {
                    // Set alignment of cell data to center
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    column.Frozen = false;
                    //column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    //column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    if (column.Index < 2)
                        column.Frozen = true;
                    if (column.Index > 0)
                        column.Width = 160;
                }
                dataGridView.Columns[0].Width = 40;
                dataGridView.Refresh();
            }

            result = false;
            switch (profile)
            {
                case "Load Survey Vertical":
                    result = GetParameter("00070100630100FF02", nWait, nTryCount, nTimeOut, (byte)1, DateTime.ParseExact(_startDT, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture), DateTime.ParseExact(_endDT, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture), "12000809060000010000FF0F02120000", 0UL, 0UL);
                    //result = GetSelectiveParameter(7, "0100630100FF", 2, bytWait, 3, bytTimOut, profile, null, null, _startDT, _endDT);
                    break;
                case "Load Survey Vertical-AllData":
                    result = GetParameter("00070100630100FF02", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                    //result = GetParameter2(7, "0100630100FF", 2, bytWait, 3, bytTimOut);
                    break;
                case "Daily Energy Vertical":
                    result = GetParameter("00070100630200FF02", nWait, nTryCount, nTimeOut, (byte)1, DateTime.ParseExact(_startDT, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture), DateTime.ParseExact(_endDT, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture), "12000809060000010000FF0F02120000", 0UL, 0UL);
                    //result = GetSelectiveParameter(7, "0100630200FF", 2, bytWait, 3, bytTimOut, profile, null, null, _startDT, _endDT);
                    break;
                case "Daily Energy Vertical-AllData":
                    result = GetParameter("00070100630200FF02", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                    //result = GetParameter2(7, "0100630200FF", 2, bytWait, 3, bytTimOut);
                    break;
            }
            if (result == false)
            {
                log.Error($"Error Getting {profile} Att. - 2. Received data Start Date <{_startDT}> End Date <{_endDT}> is: {strbldDLMdata.ToString().Trim()}");
                return resultDataTable;
            }
            else
                recivedValueString = strbldDLMdata.ToString().Trim();
            strbldDLMdata.Clear();
            resultDataTable = parse.GetEventsValuesDataTableParsing(recivedValueString, obisDataTable, profile);
            // Multiply each row value with the corresponding string array value
            MultiplyRowsWithArray(resultDataTable, ScalerMultiFactorArray);
            FormatGrid(profile, resultDataTable);
            return resultDataTable;
        }
        public DataTable ReadLoadProfileProfileGenericHorizontal(string profile = "", string _startDT = null, string _endDT = null)
        {
            // Create a DataTable
            DataTable obisDataTable = new DataTable();
            // Add columns to the DataTable
            obisDataTable.Columns.Add("Obis", typeof(string));
            obisDataTable.Columns.Add("Name", typeof(string));
            obisDataTable.Columns.Add("Data", typeof(string));
            DataTable resultDataTable = new DataTable();
            DLMSParser parse = new DLMSParser(dataGridView);
            string recivedObisString = string.Empty;
            string recivedValueString = string.Empty;
            string recivedScalerObisString = string.Empty;
            string recivedScalerValueString = string.Empty;
            bool result = false;
            string[] scalerObisArray = null;
            string[] scalerScalerDataArray = null;
            result = false;
            switch (profile)
            {
                case "Daily Energy":
                    result = GetParameter("00070100630200FF03", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                    //result = GetParameter2(7, "0100630200FF", 3, bytWait, 3, bytTimOut);
                    break;
                case "Load Survey":
                    result = GetParameter("00070100630100FF03", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                    //result = GetParameter2(7, "0100630100FF", 3, bytWait, 3, bytTimOut);
                    break;
            }
            if (result == false)
            {
                log.Error($"Error Getting {profile} Att. - 3. Received data is: {strbldDLMdata.ToString().Trim()}");
                return obisDataTable;
            }
            else
                recivedObisString = strbldDLMdata.ToString().Trim();
            strbldDLMdata.Clear();
            obisDataTable = parse.GetClassObisAttScalerListParsing(recivedObisString, profile);
            if (profile == "Daily Energy")
            {
                dataGridView.DataSource = null;
                dataGridView.Columns.Clear();
                dataGridView.DataSource = obisDataTable;
                setRowNumber(dataGridView);
                dataGridView.ScrollBars = ScrollBars.Both;
                dataGridView.Columns[0].Width = 40;
                dataGridView.Columns[1].Width = 250;
                dataGridView.Columns[2].Width = 60;
                dataGridView.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView.Columns[3].Width = 170;
                dataGridView.Columns[4].Width = 60;
                dataGridView.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView.Columns[5].Width = 80;
                dataGridView.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView.Refresh();
                result = false;
                result = GetParameter("000701005E5B05FF03", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                //result = GetParameter2(7, "01005E5B05FF", 3, bytWait, 3, bytTimOut);
                if (result == false)
                {
                    log.Error($"Error Getting {profile} Att. - 3. Received data is: {strbldDLMdata.ToString().Trim()}");
                    return obisDataTable;
                }
                else
                    recivedScalerObisString = strbldDLMdata.ToString().Trim();
                strbldDLMdata.Clear();
                result = false;
                result = GetParameter("000701005E5B05FF02", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                //result = GetParameter2(7, "01005E5B05FF", 2, bytWait, 3, bytTimOut);
                if (result == false)
                {
                    log.Error($"Error Getting {profile} Att. - 2. Received data is: {strbldDLMdata.ToString().Trim()}");
                    return obisDataTable;
                }
                else
                    recivedScalerValueString = strbldDLMdata.ToString().Trim();
                strbldDLMdata.Clear();

                scalerObisArray = parse.GetScalerProfileObisListParsing(recivedScalerObisString);
                scalerScalerDataArray = parse.GetScalerProfileScalerListParsing(recivedScalerValueString);
                string[] mainSourceObisArray = obisDataTable.AsEnumerable()
                                         .Select(row => row.Field<string>(2)) // Change the type to match the third column's type
                                         .ToArray();
                string[] finalScalerValuesToFill = new string[mainSourceObisArray.Length];
                foreach (var selectedObis in scalerObisArray)
                {
                    // Find the index of the searchString in the stringArray
                    int index = Array.IndexOf(mainSourceObisArray, selectedObis);
                    int scalarIndex = Array.IndexOf(scalerObisArray, selectedObis);
                    if (index == -1)
                    {
                        MessageBox.Show("Object present in Scaler Profile not available in Billing Profile", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        break;
                    }
                    if (index > mainSourceObisArray.Length)
                        break;
                    else
                    {
                        finalScalerValuesToFill[index] = scalerScalerDataArray[scalarIndex];
                        dataGridView.Rows[index].Cells[5].Value = scalerScalerDataArray[scalarIndex];
                    }
                }
                dataGridView.Refresh();

            }
            if (profile == "Load Survey")
            {
                dataGridView.DataSource = null;
                dataGridView.Columns.Clear();
                dataGridView.DataSource = obisDataTable;
                setRowNumber(dataGridView);
                dataGridView.ScrollBars = ScrollBars.Both;
                dataGridView.Columns[0].Width = 40;
                dataGridView.Columns[1].Width = 250;
                dataGridView.Columns[2].Width = 60;
                dataGridView.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView.Columns[3].Width = 170;
                dataGridView.Columns[4].Width = 60;
                dataGridView.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView.Columns[5].Width = 80;
                dataGridView.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView.Refresh();
                result = false;
                result = GetParameter("000701005E5B04FF03", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                //result = GetParameter2(7, "01005E5B04FF", 3, bytWait, 3, bytTimOut);
                if (result == false)
                {
                    log.Error($"Error Getting {profile} Att. - 3. Received data is: {strbldDLMdata.ToString().Trim()}");
                    return obisDataTable;
                }
                else
                    recivedScalerObisString = strbldDLMdata.ToString().Trim();
                strbldDLMdata.Clear();
                result = false;
                result = GetParameter("000701005E5B04FF02", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                //result = GetParameter2(7, "01005E5B04FF", 2, bytWait, 3, bytTimOut);
                if (result == false)
                {
                    log.Error($"Error Getting {profile} Att. - 3. Received data is: {strbldDLMdata.ToString().Trim()}");
                    return obisDataTable;
                }
                else
                    recivedScalerValueString = strbldDLMdata.ToString().Trim();
                strbldDLMdata.Clear();

                scalerObisArray = parse.GetScalerProfileObisListParsing(recivedScalerObisString);
                scalerScalerDataArray = parse.GetScalerProfileScalerListParsing(recivedScalerValueString);
                string[] mainSourceObisArray = obisDataTable.AsEnumerable()
                                         .Select(row => row.Field<string>(2)) // Change the type to match the third column's type
                                         .ToArray();
                string[] finalScalerValuesToFill = new string[mainSourceObisArray.Length];
                foreach (var selectedObis in scalerObisArray)
                {
                    // Find the index of the searchString in the stringArray
                    int index = Array.IndexOf(mainSourceObisArray, selectedObis);
                    int scalarIndex = Array.IndexOf(scalerObisArray, selectedObis);
                    if (index == -1)
                    {
                        MessageBox.Show("Object present in Scaler Profile not available in Billing Profile", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        break;
                    }
                    if (index > mainSourceObisArray.Length)
                        break;
                    else
                    {
                        finalScalerValuesToFill[index] = scalerScalerDataArray[scalarIndex];
                        dataGridView.Rows[index].Cells[5].Value = scalerScalerDataArray[scalarIndex];
                    }
                }
                dataGridView.Refresh();

            }
            result = false;
            switch (profile)
            {
                case "Daily Energy":
                    result = GetParameter("00070100630200FF02", nWait, nTryCount, nTimeOut, (byte)1, DateTime.ParseExact(_startDT, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture), DateTime.ParseExact(_endDT, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture), "12000809060000010000FF0F02120000", 0UL, 0UL);
                    //result = GetSelectiveParameter(7, "0100630200FF", 2, bytWait, 3, bytTimOut, profile, null, null, _startDT, _endDT);
                    break;
                case "Load Survey":
                    result = GetParameter("00070100630100FF02", nWait, nTryCount, nTimeOut, (byte)1, DateTime.ParseExact(_startDT, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture), DateTime.ParseExact(_endDT, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture), "12000809060000010000FF0F02120000", 0UL, 0UL);
                    //result = GetSelectiveParameter(7, "0100630100FF", 2, bytWait, 3, bytTimOut, profile, null, null, _startDT, _endDT);
                    break;
            }
            if (result == false)
            {
                log.Error($"Error Getting {profile} Att. - 2. Received data Start Date <{_startDT}> End Date <{_endDT}> is: {strbldDLMdata.ToString().Trim()}");
                return resultDataTable;
            }
            else
                recivedValueString = strbldDLMdata.ToString().Trim();
            strbldDLMdata.Clear();
            resultDataTable = parse.GetBillingValuesDataTableParsing(recivedValueString, obisDataTable);//old
            RenameParameterWithUnit(resultDataTable, parse);
            MultiplyScalerWithData(resultDataTable, parse);
            FormatGrid(profile, resultDataTable);
            return obisDataTable;
        }
        public bool AllProfileObjectList()
        {
            //// Create a DataTable
            DataTable resultDataTable = new DataTable();
            DLMSParser parse = new DLMSParser(dataGridView);
            bool result = false;
            string recivedObisString = string.Empty;
            string[] scalerObisArray = null;
            #region Add columns to the DataGridView
            dataGridView.DataSource = null;
            dataGridView.Columns.Clear();
            dataGridView.Columns.Add("SN", "SN");//1
            dataGridView.Columns.Add("Instant-C", "Instant-C");//2
            dataGridView.Columns.Add("Instant-O", "Instant-O");//3
            dataGridView.Columns.Add("Instant-A", "Instant-A");//4
            dataGridView.Columns.Add("Instant-S-C", "Instant-S-C");//5
            dataGridView.Columns.Add("Instant-S-O", "Instant-S-O");//6
            dataGridView.Columns.Add("Instant-S-A", "Instant-S-A");//7
            dataGridView.Columns.Add("Nameplate-C", "Nameplate-C");//8
            dataGridView.Columns.Add("Nameplate-O", "Nameplate-O");//9
            dataGridView.Columns.Add("Nameplate-A", "Nameplate-A");//10
            dataGridView.Columns.Add("Voltage-C", "Voltage-C");//11
            dataGridView.Columns.Add("Voltage-O", "Voltage-O");//12
            dataGridView.Columns.Add("Voltage-A", "Voltage-A");//13
            dataGridView.Columns.Add("Current-C", "Current-C");//14
            dataGridView.Columns.Add("Current-O", "Current-O");//15
            dataGridView.Columns.Add("Current-A", "Current-A");//16
            dataGridView.Columns.Add("Power-C", "Power-C");//17
            dataGridView.Columns.Add("Power-O", "Power-O");//18
            dataGridView.Columns.Add("Power-A", "Power-A");//19
            dataGridView.Columns.Add("Transaction-C", "Transaction-C");//20
            dataGridView.Columns.Add("Transaction-O", "Transaction-O");//21
            dataGridView.Columns.Add("Transaction-A", "Transaction-A");//22
            dataGridView.Columns.Add("Other-C", "Other-C");//23
            dataGridView.Columns.Add("Other-O", "Other-O");//24
            dataGridView.Columns.Add("Other-A", "Other-A");//25
            dataGridView.Columns.Add("NonRollOver-C", "NonRollOver-C");//26
            dataGridView.Columns.Add("NonRollOver-O", "NonRollOver-O");//27
            dataGridView.Columns.Add("NonRollOver-A", "NonRollOver-A");//28
            dataGridView.Columns.Add("Control-C", "Control-C");//29
            dataGridView.Columns.Add("Control-O", "Control-O");//30
            dataGridView.Columns.Add("Control-A", "Control-A");//31
            dataGridView.Columns.Add("ModeOfRelayO-C", "ModeOfRelayO-C");//32
            dataGridView.Columns.Add("ModeOfRelayO-O", "ModeOfRelayO-O");//33
            dataGridView.Columns.Add("ModeOfRelayO-A", "ModeOfRelayO-A");//34
            dataGridView.Columns.Add("Events-S-C", "Events-S-C");//35
            dataGridView.Columns.Add("Events-S-O", "Events-S-O");//36
            dataGridView.Columns.Add("Events-S-A", "Events-S-A");//37
            dataGridView.Columns.Add("Billing-C", "Billing-C");//38
            dataGridView.Columns.Add("Billing-O", "Billing-O");//39
            dataGridView.Columns.Add("Billing-A", "Billing-A");//40
            dataGridView.Columns.Add("Billing-S-C", "Billing-S-C");//41
            dataGridView.Columns.Add("Billing-S-O", "Billing-S-O");//42
            dataGridView.Columns.Add("Billing-S-A", "Billing-S-A");//43
            dataGridView.Columns.Add("LS-C", "LS-C");//44
            dataGridView.Columns.Add("LS-O", "LS-O");//45
            dataGridView.Columns.Add("LS-A", "LS-A");//46
            dataGridView.Columns.Add("LS-S-C", "LS-S-C");//47
            dataGridView.Columns.Add("LS-S-O", "LS-S-O");//48
            dataGridView.Columns.Add("LS-S-A", "LS-S-A");//49
            dataGridView.Columns.Add("DE-C", "DE-C");//50
            dataGridView.Columns.Add("DE-O", "DE-O");//51
            dataGridView.Columns.Add("DE-A", "DE-A");//52
            dataGridView.Columns.Add("DE-S-C", "DE-S-C");//53
            dataGridView.Columns.Add("DE-S-O", "DE-S-O");//54
            dataGridView.Columns.Add("DE-S-A", "DE-S-A");//55
            dataGridView.Columns.Add("Remarks", "Remarks");//56
            for (int i = 1; i <= 200; i++)
            {
                dataGridView.Rows.Add(i.ToString()); // Add a new row and populate the first cell with the SN value
            }
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                // Set alignment of cell data to center
                column.Frozen = false;
                //column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                if (column.Index < 1)
                    column.Frozen = true;
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                if (column.HeaderText.Contains("-O") || column.HeaderText.Contains("Remarks"))
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                else
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column.SortMode = DataGridViewColumnSortMode.NotSortable;

                #region Colum color for different profile
                //instant
                if (column.Index >= 1 && column.Index <= 6)
                    for (int j = 0; j < dataGridView.RowCount; j++)
                    {
                        dataGridView.Rows[j].Cells[column.Index].Style.BackColor = Color.LightBlue;
                    }
                //nameplate
                if (column.Index >= 7 && column.Index <= 9)
                    for (int j = 0; j < dataGridView.RowCount; j++)
                    {
                        dataGridView.Rows[j].Cells[column.Index].Style.BackColor = Color.LightGreen;
                    }
                //voltage
                if (column.Index >= 10 && column.Index <= 12)
                    for (int j = 0; j < dataGridView.RowCount; j++)
                    {
                        dataGridView.Rows[j].Cells[column.Index].Style.BackColor = Color.LightCyan;
                    }
                //current
                if (column.Index >= 13 && column.Index <= 15)
                    for (int j = 0; j < dataGridView.RowCount; j++)
                    {
                        dataGridView.Rows[j].Cells[column.Index].Style.BackColor = Color.LightGoldenrodYellow;
                    }
                //power               
                if (column.Index >= 16 && column.Index <= 18)
                    for (int j = 0; j < dataGridView.RowCount; j++)
                    {
                        dataGridView.Rows[j].Cells[column.Index].Style.BackColor = Color.LightPink;
                    }
                //Transactions
                if (column.Index >= 19 && column.Index <= 21)
                    for (int j = 0; j < dataGridView.RowCount; j++)
                    {
                        dataGridView.Rows[j].Cells[column.Index].Style.BackColor = Color.LightSkyBlue;
                    }
                //Other
                if (column.Index >= 22 && column.Index <= 24)
                    for (int j = 0; j < dataGridView.RowCount; j++)
                    {
                        dataGridView.Rows[j].Cells[column.Index].Style.BackColor = Color.LemonChiffon;
                    }
                //Non Roll Over
                if (column.Index >= 25 && column.Index <= 27)
                    for (int j = 0; j < dataGridView.RowCount; j++)
                    {
                        dataGridView.Rows[j].Cells[column.Index].Style.BackColor = Color.LightBlue;
                    }
                //Control
                if (column.Index >= 28 && column.Index <= 30)
                    for (int j = 0; j < dataGridView.RowCount; j++)
                    {
                        dataGridView.Rows[j].Cells[column.Index].Style.BackColor = Color.LightCoral;
                    }
                //Relay
                if (column.Index >= 31 && column.Index <= 33)
                    for (int j = 0; j < dataGridView.RowCount; j++)
                    {
                        dataGridView.Rows[j].Cells[column.Index].Style.BackColor = Color.LightCyan;
                    }
                //Events
                if (column.Index >= 34 && column.Index <= 36)
                    for (int j = 0; j < dataGridView.RowCount; j++)
                    {
                        dataGridView.Rows[j].Cells[column.Index].Style.BackColor = Color.LightGoldenrodYellow;
                    }
                //Billing
                if (column.Index >= 37 && column.Index <= 42)
                    for (int j = 0; j < dataGridView.RowCount; j++)
                    {
                        dataGridView.Rows[j].Cells[column.Index].Style.BackColor = Color.LightGray;
                    }
                //LS
                if (column.Index >= 43 && column.Index <= 48)
                    for (int j = 0; j < dataGridView.RowCount; j++)
                    {
                        dataGridView.Rows[j].Cells[column.Index].Style.BackColor = Color.LightGreen;
                    }
                //DE
                if (column.Index >= 49 && column.Index <= 54)
                    for (int j = 0; j < dataGridView.RowCount; j++)
                    {
                        dataGridView.Rows[j].Cells[column.Index].Style.BackColor = Color.LightPink;
                    }
                #endregion
            }
            PB_PGUpdate(20);
            #endregion

            string[] Profiles = { "7-01005E5B00FF-3" , //instant
                "7-01005E5B03FF-3" , //instant scaler
                "7-00005E5B0AFF-3" , //nameplate
                "7-0000636200FF-3" , //voltage
                "7-0000636201FF-3" , //current
                "7-0000636202FF-3" , //power
                "7-0000636203FF-3" , //transaction
                "7-0000636204FF-3" , //other
                "7-0000636205FF-3" , //non roll over
                "7-0000636206FF-3" , //control
                "7-0000636281FF-3" , //relay profile
                "7-01005E5B07FF-3" , //Events
                "7-0100620100FF-3" , //billing
                "7-01005E5B06FF-3" , //billing scaler
                "7-0100630100FF-3" , //LS
                "7-01005E5B04FF-3" , //LS scaler
                "7-0100630200FF-3" , //DE
                "7-01005E5B05FF-3"}; //DE scaler

            foreach (var Profile in Profiles)
            {
                byte _class = Convert.ToByte(Profile.ToString().Trim().Split('-')[0]);
                string _obis = Profile.ToString().Trim().Split('-')[1];
                byte _attribute = Convert.ToByte(Profile.ToString().Trim().Split('-')[2]);
                result = GetParameter($"0007{Profile.ToString().Trim().Split('-')[1]}03", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                //result = GetParameter2(_class, _obis, _attribute, bytWait, 3, bytTimOut);
                if (result == false)
                {
                    log.Error($"Error Getting {Profile} Class - {_class} Obis - {_obis} Att. - {_attribute}. Received data is: {strbldDLMdata.ToString().Trim()}");
                    MessageBox.Show("Error in getting Objects. Click OK to Continue.", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                else
                {
                    recivedObisString = strbldDLMdata.ToString().Trim();
                    strbldDLMdata.Clear();
                    string[] Class;
                    string[] Obis;
                    string[] Attribute;
                    if (result == true)
                    {
                        if (Profile == "7-01005E5B00FF-3" ||
                            Profile == "7-01005E5B03FF-3" ||
                            Profile == "7-00005E5B0AFF-3" ||
                            Profile == "7-0000636200FF-3" ||
                            Profile == "7-0000636201FF-3" ||
                            Profile == "7-0000636202FF-3" ||
                            Profile == "7-0000636203FF-3" ||
                            Profile == "7-0000636204FF-3" ||
                            Profile == "7-0000636205FF-3" ||
                            Profile == "7-0000636206FF-3" ||
                            Profile == "7-0000636281FF-3" ||
                            Profile == "7-01005E5B07FF-3" ||
                            Profile == "7-0100620100FF-3" ||
                            Profile == "7-01005E5B06FF-3" ||
                            Profile == "7-0100630100FF-3" ||
                            Profile == "7-01005E5B04FF-3" ||
                            Profile == "7-0100630200FF-3" ||
                            Profile == "7-01005E5B05FF-3"
                            )
                        {
                            resultDataTable = parse.GetProfileObjectTable(recivedObisString, Profile);
                            Class = resultDataTable.AsEnumerable()
                                                .Select(row => row.Field<string>(0)) // Change the type to match the column's type
                                                .ToArray();
                            Obis = resultDataTable.AsEnumerable()
                                                .Select(row => row.Field<string>(1)) // Change the type to match the column's type
                                                .ToArray();
                            Attribute = resultDataTable.AsEnumerable()
                                                .Select(row => row.Field<string>(2)) // Change the type to match the column's type
                                                .ToArray();
                            switch (Profile)
                            {
                                case "7-01005E5B00FF-3":
                                    for (int i = 0; i < Class.Length; i++)
                                    {
                                        dataGridView.Rows[i].Cells[1].Value = Class[i];
                                        dataGridView.Rows[i].Cells[2].Value = Obis[i];
                                        dataGridView.Rows[i].Cells[3].Value = Attribute[i];
                                    }
                                    PB_PGUpdate(25);
                                    break;
                                case "7-01005E5B03FF-3":
                                    for (int i = 0; i < Class.Length; i++)
                                    {
                                        dataGridView.Rows[i].Cells[4].Value = Class[i];
                                        dataGridView.Rows[i].Cells[5].Value = Obis[i];
                                        dataGridView.Rows[i].Cells[6].Value = Attribute[i];
                                    }
                                    PB_PGUpdate(30);
                                    break;
                                case "7-00005E5B0AFF-3":
                                    for (int i = 0; i < Class.Length; i++)
                                    {
                                        dataGridView.Rows[i].Cells[7].Value = Class[i];
                                        dataGridView.Rows[i].Cells[8].Value = Obis[i];
                                        dataGridView.Rows[i].Cells[9].Value = Attribute[i];
                                    }
                                    PB_PGUpdate(40);
                                    break;
                                case "7-0000636200FF-3":
                                    for (int i = 0; i < Class.Length; i++)
                                    {
                                        dataGridView.Rows[i].Cells[10].Value = Class[i];
                                        dataGridView.Rows[i].Cells[11].Value = Obis[i];
                                        dataGridView.Rows[i].Cells[12].Value = Attribute[i];
                                    }
                                    PB_PGUpdate(45);
                                    break;
                                case "7-0000636201FF-3":
                                    for (int i = 0; i < Class.Length; i++)
                                    {
                                        dataGridView.Rows[i].Cells[13].Value = Class[i];
                                        dataGridView.Rows[i].Cells[14].Value = Obis[i];
                                        dataGridView.Rows[i].Cells[15].Value = Attribute[i];
                                    }
                                    PB_PGUpdate(50);
                                    break;
                                case "7-0000636202FF-3":
                                    for (int i = 0; i < Class.Length; i++)
                                    {
                                        dataGridView.Rows[i].Cells[16].Value = Class[i];
                                        dataGridView.Rows[i].Cells[17].Value = Obis[i];
                                        dataGridView.Rows[i].Cells[18].Value = Attribute[i];
                                    }
                                    PB_PGUpdate(55);
                                    break;
                                case "7-0000636203FF-3":
                                    for (int i = 0; i < Class.Length; i++)
                                    {
                                        dataGridView.Rows[i].Cells[19].Value = Class[i];
                                        dataGridView.Rows[i].Cells[20].Value = Obis[i];
                                        dataGridView.Rows[i].Cells[21].Value = Attribute[i];
                                    }
                                    PB_PGUpdate(60);
                                    break;
                                case "7-0000636204FF-3":
                                    for (int i = 0; i < Class.Length; i++)
                                    {
                                        dataGridView.Rows[i].Cells[22].Value = Class[i];
                                        dataGridView.Rows[i].Cells[23].Value = Obis[i];
                                        dataGridView.Rows[i].Cells[24].Value = Attribute[i];
                                    }
                                    PB_PGUpdate(65);
                                    break;
                                case "7-0000636205FF-3":
                                    for (int i = 0; i < Class.Length; i++)
                                    {
                                        dataGridView.Rows[i].Cells[25].Value = Class[i];
                                        dataGridView.Rows[i].Cells[26].Value = Obis[i];
                                        dataGridView.Rows[i].Cells[27].Value = Attribute[i];
                                    }
                                    PB_PGUpdate(70);
                                    break;
                                case "7-0000636206FF-3":
                                    for (int i = 0; i < Class.Length; i++)
                                    {
                                        dataGridView.Rows[i].Cells[28].Value = Class[i];
                                        dataGridView.Rows[i].Cells[29].Value = Obis[i];
                                        dataGridView.Rows[i].Cells[30].Value = Attribute[i];
                                    }
                                    PB_PGUpdate(75);
                                    break;
                                case "7-0000636281FF-3":
                                    for (int i = 0; i < Class.Length; i++)
                                    {
                                        dataGridView.Rows[i].Cells[31].Value = Class[i];
                                        dataGridView.Rows[i].Cells[32].Value = Obis[i];
                                        dataGridView.Rows[i].Cells[33].Value = Attribute[i];
                                    }
                                    PB_PGUpdate(80);
                                    break;
                                case "7-01005E5B07FF-3":
                                    for (int i = 0; i < Class.Length; i++)
                                    {
                                        dataGridView.Rows[i].Cells[34].Value = Class[i];
                                        dataGridView.Rows[i].Cells[35].Value = Obis[i];
                                        dataGridView.Rows[i].Cells[36].Value = Attribute[i];
                                    }
                                    PB_PGUpdate(83);
                                    break;
                                case "7-0100620100FF-3":
                                    for (int i = 0; i < Class.Length; i++)
                                    {
                                        dataGridView.Rows[i].Cells[37].Value = Class[i];
                                        dataGridView.Rows[i].Cells[38].Value = Obis[i];
                                        dataGridView.Rows[i].Cells[39].Value = Attribute[i];
                                    }
                                    PB_PGUpdate(86);
                                    break;
                                case "7-01005E5B06FF-3":
                                    for (int i = 0; i < Class.Length; i++)
                                    {
                                        dataGridView.Rows[i].Cells[40].Value = Class[i];
                                        dataGridView.Rows[i].Cells[41].Value = Obis[i];
                                        dataGridView.Rows[i].Cells[42].Value = Attribute[i];
                                    }
                                    PB_PGUpdate(89);
                                    break;
                                case "7-0100630100FF-3":
                                    for (int i = 0; i < Class.Length; i++)
                                    {
                                        dataGridView.Rows[i].Cells[43].Value = Class[i];
                                        dataGridView.Rows[i].Cells[44].Value = Obis[i];
                                        dataGridView.Rows[i].Cells[45].Value = Attribute[i];
                                    }
                                    PB_PGUpdate(91);
                                    break;
                                case "7-01005E5B04FF-3":
                                    for (int i = 0; i < Class.Length; i++)
                                    {
                                        dataGridView.Rows[i].Cells[46].Value = Class[i];
                                        dataGridView.Rows[i].Cells[47].Value = Obis[i];
                                        dataGridView.Rows[i].Cells[48].Value = Attribute[i];
                                    }
                                    PB_PGUpdate(93);
                                    break;
                                case "7-0100630200FF-3":
                                    for (int i = 0; i < Class.Length; i++)
                                    {
                                        dataGridView.Rows[i].Cells[49].Value = Class[i];
                                        dataGridView.Rows[i].Cells[50].Value = Obis[i];
                                        dataGridView.Rows[i].Cells[51].Value = Attribute[i];
                                    }
                                    PB_PGUpdate(95);
                                    break;
                                case "7-01005E5B05FF-3":
                                    for (int i = 0; i < Class.Length; i++)
                                    {
                                        dataGridView.Rows[i].Cells[52].Value = Class[i];
                                        dataGridView.Rows[i].Cells[53].Value = Obis[i];
                                        dataGridView.Rows[i].Cells[54].Value = Attribute[i];
                                    }
                                    PB_PGUpdate(97);
                                    break;
                            }
                        }
                    }
                }
            }
            return true;
        }
        public bool ReadObjectList(DataTable dataTable, string meterDataRead, bool attributeDownload)
        {
            DLMSParser parse = new DLMSParser(dataGridView);
            // Add columns to the DataGridView
            dataGridView.DataSource = null;
            dataGridView.DataSource = dataTable;

            dataGridView.Columns[0].Width = 40;//SN
            dataGridView.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataGridView.Columns[1].Width = 70;//Class
            dataGridView.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataGridView.Columns[2].Width = 70;//Version
            dataGridView.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataGridView.Columns[3].Width = 100;//OBIS
            dataGridView.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataGridView.Columns[4].Width = 350;
            dataGridView.Columns[5].Width = 200;//Attribute Access
            dataGridView.Columns[6].Width = 200;//Method Access

            for (int index = 0; index < dataTable.Rows.Count; ++index)
            {
                if (dataGridView.Rows[index].Cells[4].Value.ToString().Contains("Name not Available"))
                {
                    dataGridView.Rows[index].Cells[4].Style.BackColor = Color.Red;
                }
            }
            dataGridView.Refresh();
            #region Individual Data
            if (attributeDownload)
            {
                dataGridView.Columns.Add("SCALER", "SCALER");
                dataGridView.Columns[7].Width = 70;//Scaler
                dataGridView.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                int individualScalerColumnIndex = FindColumnIndexByName(dataGridView, "SCALER");

                string[] classArray = new string[dataGridView.Rows.Count];
                string[] obisArray = new string[dataGridView.Rows.Count];
                string[] classArrayInHEX = new string[dataGridView.Rows.Count];
                bool parameter = false;
                string tempRenameScaler = string.Empty;
                for (int index = 0; index < dataGridView.Rows.Count; index++)
                {
                    classArrayInHEX[index] = dataGridView.Rows[index].Cells[1].Value.ToString().Trim();
                    classArray[index] = int.Parse(classArrayInHEX[index], NumberStyles.HexNumber).ToString();
                    obisArray[index] = dataGridView.Rows[index].Cells[3].Value.ToString().Trim();
                    string[] strArray;
                    switch (classArray[index])
                    {
                        case "3":
                        case "4":
                        case "5":
                            parameter = GetParameter($"{classArrayInHEX[index]}{obisArray[index]}03", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                            if (parameter)
                            {
                                dataGridView.Rows[index].Cells[7].Value = strbldDLMdata.ToString().Trim().Split(' ')[3].Substring(4);
                            }
                            break;
                    }
                    tempRenameScaler = (string)dataGridView.Rows[index].Cells[individualScalerColumnIndex].Value;
                    if (!string.IsNullOrEmpty(tempRenameScaler))
                    {
                        if (tempRenameScaler.Substring(0, 2) == "0F")
                        {
                            if (!string.IsNullOrEmpty((string)parse.UnithshTable[tempRenameScaler.Substring(6, 2)]))
                            {
                                dataGridView.Rows[index].Cells[4].Value = dataGridView.Rows[index].Cells[4].Value + " ( " + (string)parse.UnithshTable[tempRenameScaler.Substring(6, 2)] + " )";
                            }
                        }
                    }
                }
            }
            #endregion
            if (dataGridView.Columns.Count > 4)
            {
                foreach (DataGridViewColumn column in dataGridView.Columns)
                {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                    if (column.Index < 5)
                        column.Frozen = true;
                }
            }
            dataGridView.Invalidate();
            return true;
        }
        public bool ReadObjectListAllAttributes()
        {
            DLMSParser parse = new DLMSParser(dataGridView);
            dataGridView.Columns.Add("Attribute-1 Data", "Attribute-1 Data");
            dataGridView.Columns.Add("Attribute-1 Value", "Attribute-1 Value");
            dataGridView.Columns.Add("Attribute-2 Data", "Attribute-2 Data");
            dataGridView.Columns.Add("Attribute-2 Value", "Attribute-2 Value");
            dataGridView.Columns.Add("Attribute-3 Data", "Attribute-3 Data");
            dataGridView.Columns.Add("Attribute-3 Value", "Attribute-3 Value");
            dataGridView.Columns.Add("Attribute-4 Data", "Attribute-4 Data");
            dataGridView.Columns.Add("Attribute-4 Value", "Attribute-4 Value");
            dataGridView.Columns.Add("Attribute-5 Data", "Attribute-5 Data");
            dataGridView.Columns.Add("Attribute-5 Value", "Attribute-5 Value");
            dataGridView.Columns.Add("Attribute-6 Data", "Attribute-6 Data");
            dataGridView.Columns.Add("Attribute-6 Value", "Attribute-6 Value");
            dataGridView.Columns.Add("Attribute-7 Data", "Attribute-7 Data");
            dataGridView.Columns.Add("Attribute-7 Value", "Attribute-7 Value");
            dataGridView.Columns.Add("Attribute-8 Data", "Attribute-8 Data");
            dataGridView.Columns.Add("Attribute-8 Value", "Attribute-8 Value");
            dataGridView.Columns.Add("Attribute-9 Data", "Attribute-9 Data");
            dataGridView.Columns.Add("Attribute-9 Value", "Attribute-9 Value");
            dataGridView.Columns.Add("Attribute-10 Data", "Attribute-10 Data");
            dataGridView.Columns.Add("Attribute-10 Value", "Attribute-10 Value");
            dataGridView.Columns.Add("Attribute-10 Data", "Attribute-11 Data");
            dataGridView.Columns.Add("Attribute-10 Value", "Attribute-11 Value");
            dataGridView.Refresh();
            int classColumnIndex = FindColumnIndexByName(dataGridView, "CLASS ID");
            int obisColumnIndex = FindColumnIndexByName(dataGridView, "OBIS");
            int accessColumnIndex = FindColumnIndexByName(dataGridView, "ATTRIBUTE ACCESS");
            string[] classArray = new string[dataGridView.Rows.Count];
            string[] classArrayInHEX = new string[dataGridView.Rows.Count];
            string[] obisArray = new string[dataGridView.Rows.Count];
            string[] accessArray = new string[dataGridView.Rows.Count];
            //if (!SignOnDLMS())
            //{
            //    CommonHelper.DisplayDLMSSignONError();
            //    PB_PGUpdate(100);
            //    return false;
            //}
            int attributeStartColumn = FindColumnIndexByName(dataGridView, "Attribute-1 Data");
            bool parameter = false;
            dataGridView.SuspendLayout();
            for (int index = 0; index < dataGridView.Rows.Count; index++)
            {
                classArrayInHEX[index] = dataGridView.Rows[index].Cells[1].Value.ToString().Trim();
                classArray[index] = int.Parse(classArrayInHEX[index], NumberStyles.HexNumber).ToString();
                obisArray[index] = dataGridView.Rows[index].Cells[3].Value.ToString().Trim();
                accessArray[index] = dataGridView.Rows[index].Cells[5].Value.ToString().Trim();
                string[] accessAttributeArray = accessArray[index].Split(' ');
                for (int i = 1; i <= accessAttributeArray.Length - 1; i++)
                {
                    if (classArray[index] == "7" && i == 2)
                    {
                        i++;
                        attributeStartColumn += 2;
                    }
                    else if (classArray[index] == "7" && i == 3)
                    {
                        i++;
                        attributeStartColumn += 2;
                    }
                    if (classArray[index] == "15" && i == 2)
                    {
                        i++;
                        attributeStartColumn += 2;
                    }
                    parameter = GetParameter($"{classArrayInHEX[index]}{obisArray[index]}{i.ToString("X2")}", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                    if (parameter)
                    {
                        string data = strbldDLMdata.ToString().Trim().Split(' ')[3];
                        dataGridView.Rows[index].Cells[attributeStartColumn].Value = data;
                        if (data == "0B" || data == "0D")
                            dataGridView.Rows[index].Cells[attributeStartColumn].Style.BackColor = System.Drawing.Color.Green;
                        else
                        {
                            if (!accessAttributeArray[i].Trim().Contains("read"))
                                dataGridView.Rows[index].Cells[attributeStartColumn].Style.BackColor = System.Drawing.Color.Red;
                        }
                        string value = string.Empty;
                        if (obisArray[index] == "0000608000FF" || obisArray[index] == "0000608001FF" || obisArray[index] == "0100608000FF" || obisArray[index] == "0100608001FF")//LCD PArameters Auto Push
                        {
                            string tempString = string.Empty;
                            int nStart = 4;
                            while (nStart <= data.Substring(4).Length + 2)
                            {
                                tempString += int.Parse(data.Substring(nStart, 2), NumberStyles.HexNumber);
                                nStart += 2;
                                if (nStart <= data.Substring(4).Length + 2)
                                    tempString += ",";
                            }
                            value = tempString;
                            dataGridView.Rows[index].Cells[attributeStartColumn + 1].Value = value;
                        }
                        else
                        {
                            value = parse.GetProfileValueString(data);
                            dataGridView.Rows[index].Cells[attributeStartColumn + 1].Value = value;
                        }
                        if (string.IsNullOrEmpty(value))
                            dataGridView.Rows[index].Cells[attributeStartColumn + 1].Style.BackColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        MessageBox.Show($"There is some error in getting data from Meter for Class - {index}  OBIS - {DLMSParser.GetObis(obisArray[index])} Attribute - {i}. Kindly Retry!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    attributeStartColumn += 2;
                }
                attributeStartColumn = FindColumnIndexByName(dataGridView, "Attribute-1 Data"); ;
            }
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            dataGridView.ResumeLayout();
            return true;
        }
        public bool ReadDLMSObjectsList(string meterDataRead)
        {
            DLMSParser parse = new DLMSParser(dataGridView);
            // Add columns to the DataGridView
            dataGridView.DataSource = null;
            dataGridView.Columns.Clear();
            dataGridView.Columns.Add("SN", "SN");
            dataGridView.Columns.Add("Class", "Class");
            dataGridView.Columns.Add("Ver.", "Ver.");
            dataGridView.Columns.Add("OBIS Code", "OBIS Code");
            dataGridView.Columns.Add("Parameter Name", "Parameter Name");
            dataGridView.Columns.Add("Access", "Access");
            //dataGridView.Columns.Add("Data", "Data");
            dataGridView.Columns.Add("Scaler", "Scaler");
            //dataGridView.Columns.Add("Value", "Value");

            dataGridView.Columns[0].Width = 30;//SN
            dataGridView.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataGridView.Columns[1].Width = 40;//Class
            dataGridView.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataGridView.Columns[2].Width = 35;//Version
            dataGridView.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataGridView.Columns[3].Width = 90;//OBIS
            dataGridView.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataGridView.Columns[4].Width = 300;//Parameter
            dataGridView.Columns[5].Width = 200;//Access

            //dataGridView.Columns[6].Width = 230;//Data
            //dataGridView.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dataGridView.Columns[6].Width = 70;//Scaler
            dataGridView.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            //dataGridView.Columns[8].Width = 150;//Value
            //dataGridView.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            //int individualDataColumnIndex = FindColumnIndexByName(dataGridView, "Data");
            int individualScalerColumnIndex = FindColumnIndexByName(dataGridView, "Scaler");
            // int individualValueColumnIndex = FindColumnIndexByName(dataGridView, "Value");

            string[] obiscode = (string[])null;
            int obiscnt = 0;

            if (meterDataRead != "0100")
                ObjectListGetObisCode(meterDataRead.Substring(4, meterDataRead.Length - 4), out obiscode, out obiscnt);
            if (obiscode == null)
                return true;
            int num1 = !(meterDataRead.Substring(2, 2) == "82") ? (!(meterDataRead.Substring(2, 2) == "81") ? int.Parse(meterDataRead.Substring(2, 2), NumberStyles.HexNumber) : int.Parse(meterDataRead.Substring(4, 2), NumberStyles.HexNumber)) : int.Parse(meterDataRead.Substring(6, 4), NumberStyles.HexNumber);
            if (obiscnt != num1)
            {
                int num2 = (int)MessageBox.Show("OBIS Code Count Mismatch in Object List.", "ERROR !!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            for (int index = 1; index <= obiscnt; ++index)
            {
                string tempClass = int.Parse(obiscode[index].Substring(0, 4), NumberStyles.HexNumber).ToString();
                string tempObis = int.Parse(obiscode[index].Substring(8, 2), NumberStyles.HexNumber).ToString() + "." +
                    int.Parse(obiscode[index].Substring(10, 2), NumberStyles.HexNumber).ToString() + "." +
                    int.Parse(obiscode[index].Substring(12, 2), NumberStyles.HexNumber).ToString() + "." +
                    int.Parse(obiscode[index].Substring(14, 2), NumberStyles.HexNumber).ToString() + "." +
                    int.Parse(obiscode[index].Substring(16, 2), NumberStyles.HexNumber).ToString() + "." +
                    int.Parse(obiscode[index].Substring(18, 2), NumberStyles.HexNumber).ToString();
                dataGridView.Rows.Add((object)index.ToString(), (object)obiscode[index].Substring(0, 4), (object)obiscode[index].Substring(5, 2), (object)obiscode[index].Substring(8, 12), $"{tempClass} - {tempObis} - {DLMSParser.GetObisName(tempClass, tempObis)}", (object)obiscode[index].Substring(21, obiscode[index].Length - 21));

                if (dataGridView.Rows[index - 1].Cells[3].Value.ToString().Contains("OBIS"))
                {
                    dataGridView.Rows[index - 1].Cells[3].Style.BackColor = Color.Red;
                }
            }
            dataGridView.Refresh();
            #region Individual Data
            string[] classArray = new string[dataGridView.Rows.Count];
            string[] classArrayInHEX = new string[dataGridView.Rows.Count];
            string[] obisArray = new string[dataGridView.Rows.Count];
            bool parameter = false;
            string tempRenameScaler = string.Empty;
            for (int index = 0; index < dataGridView.Rows.Count; index++)
            {
                classArrayInHEX[index] = dataGridView.Rows[index].Cells[1].Value.ToString().Trim();
                classArray[index] = int.Parse(classArrayInHEX[index], NumberStyles.HexNumber).ToString();
                obisArray[index] = dataGridView.Rows[index].Cells[3].Value.ToString().Trim();
                string[] strArray;
                switch (classArray[index])
                {
                    case "3":
                        double num10 = 1.0;
                        strArray = Regex.Split(dataGridView.Rows[index].Cells[5].Value.ToString(), " ");
                        parameter = GetParameter($"{classArrayInHEX[index]}{obisArray[index]}03", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                        //                        parameter = GetParameter2(Convert.ToByte(classArray[index]), obisArray[index], 3, bytWait, 3, bytTimOut);
                        if (parameter)
                        {
                            dataGridView.Rows[index].Cells[6].Value = strbldDLMdata.ToString().Trim().Split(' ')[3].Substring(4);
                            if (strArray[3] == "R")
                                num10 = Math.Pow(10.0, Convert.ToDouble(sbyte.Parse(strbldDLMdata.ToString().Trim().Split(' ')[3].Substring(6, 2), NumberStyles.HexNumber)));

                        }
                        break;
                    case "4":
                        double num11 = 1.0;
                        strArray = Regex.Split(dataGridView.Rows[index].Cells[5].Value.ToString(), " ");
                        parameter = GetParameter($"{classArrayInHEX[index]}{obisArray[index]}03", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                        //parameter = GetParameter2(Convert.ToByte(classArray[index]), obisArray[index], 3, bytWait, 3, bytTimOut);
                        if (parameter)
                        {
                            dataGridView.Rows[index].Cells[6].Value = strbldDLMdata.ToString().Trim().Split(' ')[3].Substring(4);
                            if (strArray[3] == "R")
                                num11 = Math.Pow(10.0, Convert.ToDouble(sbyte.Parse(strbldDLMdata.ToString().Trim().Split(' ')[3].Substring(6, 2), NumberStyles.HexNumber)));

                        }
                        break;
                }
                tempRenameScaler = (string)dataGridView.Rows[index].Cells[individualScalerColumnIndex].Value;
                if (!string.IsNullOrEmpty(tempRenameScaler))
                {
                    if (tempRenameScaler.Substring(0, 2) == "0F")
                    {
                        if (!string.IsNullOrEmpty((string)parse.UnithshTable[tempRenameScaler.Substring(6, 2)]))
                        {
                            dataGridView.Rows[index].Cells[4].Value = dataGridView.Rows[index].Cells[4].Value + " ( " + (string)parse.UnithshTable[tempRenameScaler.Substring(6, 2)] + " )";
                        }
                    }
                }
            }
            #endregion
            if (dataGridView.Columns.Count > 4)
            {
                foreach (DataGridViewColumn column in dataGridView.Columns)
                {
                    //if (column.Index < 6)
                    //    column.Frozen = true;
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
            }
            return true;
        }
        public DataTable GetDataForTamper(string ProfileObis, string Profile = "null", int _startIndex = 0, int _endIndex = 0)
        {
            // Create a DataTable
            DataTable dataTable = new DataTable();
            // Example arrays for "Obis" and "Value"
            string[] obisArray = new string[200];
            string[] valueArray1 = new string[200];
            string[] valueArray2 = new string[200];
            string[] valueArray3 = new string[200];
            string[] valueArray4 = new string[200];
            string recivedObisString = string.Empty;
            string recivedValueString1 = string.Empty;
            string recivedValueString2 = string.Empty;
            string recivedValueString3 = string.Empty;
            string recivedValueString4 = string.Empty;
            bool result = false;
            result = false;
            result = GetParameter($"0007{ProfileObis}03", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
            //result = GetParameter2(7, ProfileObis, 3, bytWait, 3, bytTimOut);
            if (result == false)
            {
                return dataTable;
            }
            else
                recivedObisString = strbldDLMdata.ToString().Trim();
            strbldDLMdata.Clear();
            DLMSParser parse = new DLMSParser();
            // Add columns to the DataTable
            dataTable.Columns.Add("Obis", typeof(string));
            obisArray = parse.GetDlmObjectListParsing(recivedObisString);
            result = false;
            int entriescount = 0;
            switch (_endIndex - _startIndex + 1)
            {
                case 1:
                    entriescount = 1;
                    dataTable.Columns.Add("Data1", typeof(string));
                    result = GetParameter($"0007{ProfileObis}02", nWait, nTryCount, nTimeOut, (byte)2, DateTime.Now, DateTime.Now, string.Empty, (ulong)_startIndex, (ulong)_endIndex);
                    //result = GetSelectiveParameter(7, ProfileObis, 2, bytWait, 3, bytTimOut, Profile, Convert.ToString(_startIndex), Convert.ToString(_endIndex), null, null);
                    if (result == false)
                    {
                        return dataTable;
                    }
                    else
                        recivedValueString1 = strbldDLMdata.ToString();
                    strbldDLMdata.Clear();
                    valueArray1 = parse.GetDlmDataListParsing(recivedValueString1.Substring(23));
                    break;
                case 2:
                    entriescount = 2;
                    dataTable.Columns.Add("Data1", typeof(string));
                    dataTable.Columns.Add("Data2", typeof(string));
                    result = GetParameter($"0007{ProfileObis}02", nWait, nTryCount, nTimeOut, (byte)2, DateTime.Now, DateTime.Now, string.Empty, ((ulong)_startIndex) + 1, (ulong)_endIndex);
                    //result = GetSelectiveParameter(7, ProfileObis, 2, bytWait, 3, bytTimOut, Profile, Convert.ToString(_startIndex + 1), Convert.ToString(_endIndex), null, null);
                    if (result == false)
                    {
                        return dataTable;
                    }
                    else
                        recivedValueString1 = strbldDLMdata.ToString();
                    strbldDLMdata.Clear();
                    valueArray1 = parse.GetDlmDataListParsing(recivedValueString1.Substring(23));
                    result = GetParameter($"0007{ProfileObis}02", nWait, nTryCount, nTimeOut, (byte)2, DateTime.Now, DateTime.Now, string.Empty, (ulong)_startIndex, ((ulong)_endIndex) - 1);
                    //result = GetSelectiveParameter(7, ProfileObis, 2, bytWait, 3, bytTimOut, Profile, Convert.ToString(_startIndex), Convert.ToString(_endIndex - 1), null, null);
                    if (result == false)
                    {
                        return dataTable;
                    }
                    else
                        recivedValueString2 = strbldDLMdata.ToString();
                    strbldDLMdata.Clear();
                    valueArray2 = parse.GetDlmDataListParsing(recivedValueString2.Substring(23));
                    break;
                case 3:
                    entriescount = 3;
                    dataTable.Columns.Add("Data1", typeof(string));
                    dataTable.Columns.Add("Data2", typeof(string));
                    dataTable.Columns.Add("Data3", typeof(string));
                    result = GetParameter($"0007{ProfileObis}02", nWait, nTryCount, nTimeOut, (byte)2, DateTime.Now, DateTime.Now, string.Empty, ((ulong)_startIndex) + 2, (ulong)_endIndex);
                    //result = GetSelectiveParameter(7, ProfileObis, 2, bytWait, 3, bytTimOut, Profile, Convert.ToString(_startIndex + 2), Convert.ToString(_endIndex), null, null);
                    if (result == false)
                    {
                        return dataTable;
                    }
                    else
                        recivedValueString1 = strbldDLMdata.ToString();
                    strbldDLMdata.Clear();
                    valueArray1 = parse.GetDlmDataListParsing(recivedValueString1.Substring(23));
                    result = GetParameter($"0007{ProfileObis}02", nWait, nTryCount, nTimeOut, (byte)2, DateTime.Now, DateTime.Now, string.Empty, ((ulong)_startIndex) + 1, ((ulong)_endIndex) - 1);
                    //result = GetSelectiveParameter(7, ProfileObis, 2, bytWait, 3, bytTimOut, Profile, Convert.ToString(_startIndex + 1), Convert.ToString(_endIndex - 1), null, null);
                    if (result == false)
                    {
                        return dataTable;
                    }
                    else
                        recivedValueString2 = strbldDLMdata.ToString();
                    strbldDLMdata.Clear();
                    valueArray2 = parse.GetDlmDataListParsing(recivedValueString2.Substring(23));
                    result = GetParameter($"0007{ProfileObis}02", nWait, nTryCount, nTimeOut, (byte)2, DateTime.Now, DateTime.Now, string.Empty, (ulong)_startIndex, ((ulong)_endIndex) - 2);
                    //result = GetSelectiveParameter(7, ProfileObis, 2, bytWait, 3, bytTimOut, Profile, Convert.ToString(_startIndex), Convert.ToString(_endIndex - 2), null, null);
                    if (result == false)
                    {
                        return dataTable;
                    }
                    else
                        recivedValueString3 = strbldDLMdata.ToString();
                    strbldDLMdata.Clear();
                    valueArray3 = parse.GetDlmDataListParsing(recivedValueString3.Substring(23));
                    break;
                case 4:
                    entriescount = 4;
                    dataTable.Columns.Add("Data1", typeof(string));
                    dataTable.Columns.Add("Data2", typeof(string));
                    dataTable.Columns.Add("Data3", typeof(string));
                    dataTable.Columns.Add("Data4", typeof(string));
                    result = GetParameter($"0007{ProfileObis}02", nWait, nTryCount, nTimeOut, (byte)2, DateTime.Now, DateTime.Now, string.Empty, ((ulong)_startIndex) + 3, (ulong)_endIndex);
                    //result = GetSelectiveParameter(7, ProfileObis, 2, bytWait, 3, bytTimOut, Profile, Convert.ToString(_startIndex + 3), Convert.ToString(_endIndex), null, null);
                    if (result == false)
                    {
                        return dataTable;
                    }
                    else
                        recivedValueString1 = strbldDLMdata.ToString();
                    strbldDLMdata.Clear();
                    valueArray1 = parse.GetDlmDataListParsing(recivedValueString1.Substring(23));
                    result = GetParameter($"0007{ProfileObis}02", nWait, nTryCount, nTimeOut, (byte)2, DateTime.Now, DateTime.Now, string.Empty, ((ulong)_startIndex) + 2, ((ulong)_endIndex - 1));
                    //result = GetSelectiveParameter(7, ProfileObis, 2, bytWait, 3, bytTimOut, Profile, Convert.ToString(_startIndex + 2), Convert.ToString(_endIndex - 1), null, null);
                    if (result == false)
                    {
                        return dataTable;
                    }
                    else
                        recivedValueString2 = strbldDLMdata.ToString();
                    strbldDLMdata.Clear();
                    valueArray2 = parse.GetDlmDataListParsing(recivedValueString2.Substring(23));
                    result = GetParameter($"0007{ProfileObis}02", nWait, nTryCount, nTimeOut, (byte)2, DateTime.Now, DateTime.Now, string.Empty, ((ulong)_startIndex + 1), ((ulong)_endIndex) - 2);
                    //result = GetSelectiveParameter(7, ProfileObis, 2, bytWait, 3, bytTimOut, Profile, Convert.ToString(_startIndex + 1), Convert.ToString(_endIndex - 2), null, null);
                    if (result == false)
                    {
                        return dataTable;
                    }
                    else
                        recivedValueString3 = strbldDLMdata.ToString();
                    strbldDLMdata.Clear();
                    valueArray3 = parse.GetDlmDataListParsing(recivedValueString3.Substring(23));
                    result = GetParameter($"0007{ProfileObis}02", nWait, nTryCount, nTimeOut, (byte)2, DateTime.Now, DateTime.Now, string.Empty, (ulong)_startIndex, ((ulong)_endIndex) - 3);
                    //result = GetSelectiveParameter(7, ProfileObis, 2, bytWait, 3, bytTimOut, Profile, Convert.ToString(_startIndex), Convert.ToString(_endIndex - 3), null, null);
                    if (result == false)
                    {
                        return dataTable;
                    }
                    else
                        recivedValueString4 = strbldDLMdata.ToString();
                    strbldDLMdata.Clear();
                    valueArray4 = parse.GetDlmDataListParsing(recivedValueString4.Substring(23));
                    break;
            }
            // Populate the DataTable with data
            for (int i = 0; i < obisArray.Length; i++)
            {
                DataRow row = dataTable.NewRow();
                row["Obis"] = obisArray[i];
                switch (entriescount)
                {
                    case 1:
                        row["Data1"] = valueArray1[i];
                        break;
                    case 2:
                        row["Data1"] = valueArray1[i];
                        row["Data2"] = valueArray2[i];
                        break;
                    case 3:
                        row["Data1"] = valueArray1[i];
                        row["Data2"] = valueArray2[i];
                        row["Data3"] = valueArray3[i];
                        break;
                    case 4:
                        row["Data1"] = valueArray1[i];
                        row["Data2"] = valueArray2[i];
                        row["Data3"] = valueArray3[i];
                        row["Data4"] = valueArray4[i];
                        break;
                }
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }
        static void RenameParameterWithUnit(DataTable dataTable, DLMSParser parse)
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
        static void MultiplyScalerWithData(DataTable dataTable, DLMSParser parse)
        {
            // Get the index of the "Scaler" column
            int scalerColumnIndex = dataTable.Columns.IndexOf("Scaler");

            // Iterate through each row of the DataTable
            foreach (DataRow row in dataTable.Rows)
            {
                // Get the value of the "Scaler" column for the current row
                string scalerData = row.Field<string>(scalerColumnIndex);

                // Check if the scaler value is not null or empty
                if (!string.IsNullOrEmpty(scalerData))
                {
                    string scalerValue = (string)parse.ScalerhshTable[scalerData.Substring(2, 2)];
                    // Iterate through each column in the DataTable
                    for (int i = 0; i < dataTable.Columns.Count; i++)
                    {
                        // Check if the column name contains "Data" and it's not the "Scaler" column
                        if (dataTable.Columns[i].ColumnName.Contains("Value") && i != scalerColumnIndex)
                        {
                            // Get the value of the current column for the current row
                            string dataValue = row.Field<string>(i);

                            // Multiply the value in the "Scaler" column with the value in the "Data" column
                            if (!string.IsNullOrEmpty(dataValue))
                            {
                                if (string.IsNullOrEmpty((string)parse.UnithshTable[scalerData.Substring(6, 2)]) || (string)parse.UnithshTable[scalerData.Substring(6, 2)] == "1")
                                {
                                    row[i] = dataValue;
                                }
                                else
                                {
                                    double scaledValue = Convert.ToDouble(scalerValue) * Convert.ToDouble(dataValue);
                                    row[i] = scaledValue.ToString();
                                }
                            }
                        }
                    }
                }
            }
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
        private void FormatGrid(string profile, DataTable dataTable)
        {
            switch (profile)
            {
                case "Instantaneous":
                    try
                    {
                        dataGridView.DataSource = null;
                        dataGridView.Columns.Clear();
                        dataGridView.DataSource = dataTable;
                        setRowNumber(dataGridView);
                        dataGridView.ScrollBars = ScrollBars.Both;
                        foreach (DataGridViewColumn column in dataGridView.Columns)
                        {
                            // Set alignment of cell data to center
                            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            column.Frozen = false;
                            if (column.Index <= 5)
                                column.Frozen = true;
                            else
                            {
                                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                                if (column.HeaderText.Contains("Data"))
                                    column.Width = 140;
                                else if (column.HeaderText.Contains("Value"))
                                    column.Width = 145;
                                else
                                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                            }
                            if (column.HeaderText.Contains("Parameter") || column.HeaderText.Contains("Data"))
                                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        }
                        dataGridView.Columns[0].Width = 40;
                        dataGridView.Columns[1].Width = 180;
                        dataGridView.Columns[2].Width = 45;
                        dataGridView.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView.Columns[3].Width = 100;
                        dataGridView.Columns[4].Width = 60;
                        dataGridView.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView.Columns[5].Width = 70;
                        dataGridView.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                    catch
                    {
                        break;
                    }
                    break;
                case "Billing Profile":
                    try
                    {
                        dataGridView.DataSource = null;
                        dataGridView.Columns.Clear();
                        dataGridView.DataSource = dataTable;
                        dataGridView.Columns[0].Width = 40;
                        dataGridView.Columns[1].Width = 250;
                        dataGridView.Columns[2].Width = 60;
                        dataGridView.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView.Columns[3].Width = 180;
                        dataGridView.Columns[4].Width = 60;
                        dataGridView.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView.Columns[5].Width = 80;
                        dataGridView.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        setRowNumber(dataGridView);
                        dataGridView.ScrollBars = ScrollBars.Both;
                        foreach (DataGridViewColumn column in dataGridView.Columns)
                        {
                            // Set alignment of cell data to center
                            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            column.Frozen = false;
                            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                            if (column.Index <= 5)
                                column.Frozen = true;
                            else
                            {
                                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                            }
                            if (column.Index > 5)
                                column.Width = 180;
                            if (column.HeaderText.Contains("Parameter") || column.HeaderText.Contains("Data"))
                                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        }
                        dataGridView.Columns[0].Width = 40;
                    }
                    catch
                    {
                        break;
                    }
                    break;
                case "Nameplate":
                    try
                    {
                        dataGridView.DataSource = null;
                        dataGridView.Columns.Clear();
                        dataGridView.DataSource = dataTable;
                        setRowNumber(dataGridView);
                        dataGridView.ScrollBars = ScrollBars.Both;
                        //foreach (DataGridViewColumn column in dataGridView.Columns)
                        //{
                        //    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        //}
                        foreach (DataGridViewColumn column in dataGridView.Columns)
                        {
                            // Set alignment of cell data to center
                            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            column.Frozen = false;
                            //column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                            if (column.Index <= 5)
                                column.Frozen = true;
                            else
                            {
                                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                                if (column.HeaderText.Contains("Data"))
                                    column.Width = 160;
                                else if (column.HeaderText.Contains("Value"))
                                    column.Width = 160;
                                else
                                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            }
                            if (column.HeaderText.Contains("Parameter") || column.HeaderText.Contains("Data"))
                                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        }
                        dataGridView.Columns[0].Width = 40;
                        dataGridView.Columns[1].Width = 180;
                        dataGridView.Columns[2].Width = 45;
                        dataGridView.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView.Columns[3].Width = 100;
                        dataGridView.Columns[4].Width = 60;
                        dataGridView.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dataGridView.Columns[5].Width = 50;
                        dataGridView.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                    catch
                    {
                        break;
                    }
                    break;
                case "Daily Energy":
                    try
                    {
                        dataGridView.DataSource = null;
                        dataGridView.Columns.Clear();
                        dataGridView.DataSource = dataTable;
                        setRowNumber(dataGridView);
                        dataGridView.ScrollBars = ScrollBars.Both;
                        foreach (DataGridViewColumn column in dataGridView.Columns)
                        {
                            // Set alignment of cell data to center
                            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            column.Frozen = false;
                            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                            if (column.Index <= 5)
                                column.Frozen = true;
                            if (column.HeaderText.Contains("Parameter") || column.HeaderText.Contains("Data"))
                                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        }
                        dataGridView.Columns[0].Width = 40;
                    }
                    catch
                    {
                        break;
                    }
                    break;
                case "Load Survey":
                    try
                    {
                        dataGridView.DataSource = null;
                        dataGridView.Columns.Clear();
                        dataGridView.DataSource = dataTable;
                        setRowNumber(dataGridView);
                        dataGridView.ScrollBars = ScrollBars.Both;
                        foreach (DataGridViewColumn column in dataGridView.Columns)
                        {
                            // Set alignment of cell data to center
                            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            column.Frozen = false;
                            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                            if (column.Index <= 5)
                                column.Frozen = true;
                            if (column.HeaderText.Contains("Parameter") || column.HeaderText.Contains("Data"))
                                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        }
                        dataGridView.Columns[0].Width = 40;
                    }
                    catch
                    {
                        break;
                    }
                    break;
                case "Load Survey Vertical":
                    dataGridView.DataSource = null;
                    dataGridView.Columns.Clear();
                    dataGridView.DataSource = dataTable;
                    setRowNumber(dataGridView);
                    dataGridView.ScrollBars = ScrollBars.Both;
                    foreach (DataGridViewColumn column in dataGridView.Columns)
                    {
                        // Set alignment of cell data to center
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        column.Frozen = false;
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                        if (column.Index < 2)
                            column.Frozen = true;
                        if (column.Index > 0)
                            column.Width = 150;
                    }
                    dataGridView.Columns[0].Width = 40;
                    break;
                case "Load Survey Vertical-AllData":
                    dataGridView.DataSource = null;
                    dataGridView.Columns.Clear();
                    dataGridView.DataSource = dataTable;
                    setRowNumber(dataGridView);
                    dataGridView.ScrollBars = ScrollBars.Both;
                    foreach (DataGridViewColumn column in dataGridView.Columns)
                    {
                        // Set alignment of cell data to center
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        column.Frozen = false;
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                        if (column.Index < 2)
                            column.Frozen = true;
                        if (column.Index > 0)
                            column.Width = 150;
                    }
                    dataGridView.Columns[0].Width = 40;
                    break;
                case "Daily Energy Vertical":
                    dataGridView.DataSource = null;
                    dataGridView.Columns.Clear();
                    dataGridView.DataSource = dataTable;
                    setRowNumber(dataGridView);
                    dataGridView.ScrollBars = ScrollBars.Both;
                    foreach (DataGridViewColumn column in dataGridView.Columns)
                    {
                        // Set alignment of cell data to center
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        column.Frozen = false;
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                        if (column.Index < 2)
                            column.Frozen = true;
                        if (column.Index > 0)
                            column.Width = 150;
                    }
                    dataGridView.Columns[0].Width = 40;
                    break;
                case "Daily Energy Vertical-AllData":
                    dataGridView.DataSource = null;
                    dataGridView.Columns.Clear();
                    dataGridView.DataSource = dataTable;
                    setRowNumber(dataGridView);
                    dataGridView.ScrollBars = ScrollBars.Both;
                    foreach (DataGridViewColumn column in dataGridView.Columns)
                    {
                        // Set alignment of cell data to center
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        column.Frozen = false;
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                        if (column.Index < 2)
                            column.Frozen = true;
                        if (column.Index > 0)
                            column.Width = 150;
                    }
                    dataGridView.Columns[0].Width = 40;
                    break;
                case "Power Related Events":
                    dataGridView.DataSource = null;
                    dataGridView.Columns.Clear();
                    dataGridView.DataSource = dataTable;
                    setRowNumber(dataGridView);
                    foreach (DataGridViewColumn column in dataGridView.Columns)
                    {
                        // Set alignment of cell data to center
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        column.Frozen = false;
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    }
                    dataGridView.Columns[0].Width = 40;
                    break;
                case "Transaction Events":
                    dataGridView.DataSource = null;
                    dataGridView.Columns.Clear();
                    dataGridView.DataSource = dataTable;
                    setRowNumber(dataGridView);
                    foreach (DataGridViewColumn column in dataGridView.Columns)
                    {
                        // Set alignment of cell data to center
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        column.Frozen = false;
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    }
                    dataGridView.Columns[0].Width = 40;
                    dataGridView.Refresh();
                    break;
                case "Other Tamper Events":
                    dataGridView.DataSource = null;
                    dataGridView.Columns.Clear();
                    dataGridView.DataSource = dataTable;
                    setRowNumber(dataGridView);
                    dataGridView.ScrollBars = ScrollBars.Both;
                    foreach (DataGridViewColumn column in dataGridView.Columns)
                    {
                        // Set alignment of cell data to center
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        column.Frozen = false;
                        if (column.Index < 3)
                            column.Frozen = true;
                        else
                        {
                            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                        }
                        if (column.Index > 0)
                            column.Width = 150;
                    }
                    dataGridView.Columns[0].Width = 40;
                    break;
                case "Non Roll Over Events":

                    dataGridView.DataSource = null;
                    dataGridView.Columns.Clear();
                    dataGridView.DataSource = dataTable;
                    setRowNumber(dataGridView);
                    foreach (DataGridViewColumn column in dataGridView.Columns)
                    {
                        // Set alignment of cell data to center
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        column.Frozen = false;
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    }
                    dataGridView.Columns[0].Width = 40;
                    break;
                case "Control Events":
                    dataGridView.DataSource = null;
                    dataGridView.Columns.Clear();
                    dataGridView.DataSource = dataTable;
                    setRowNumber(dataGridView);
                    foreach (DataGridViewColumn column in dataGridView.Columns)
                    {
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        column.Frozen = false;
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    }
                    dataGridView.Columns[0].Width = 40;
                    break;
            }
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        private void setRowNumber(DataGridView DG)
        {
            string[] rowNumbers = new string[DG.RowCount + 1];
            foreach (DataGridViewRow row in DG.Rows)
            {
                rowNumbers[row.Index + 1] = (row.Index + 1).ToString();
            }
            // Create a new DataGridViewTextBoxColumn
            DataGridViewTextBoxColumn newColumn = new DataGridViewTextBoxColumn();
            newColumn.HeaderText = "SN"; // Set the header text for the new column
            // Add the new column to the beginning of the DataGridView's columns
            DG.Columns.Insert(0, newColumn);
            // Populate the cells of the new column with the elements of the string array
            for (int i = 0; i < rowNumbers.Length - 1; i++)
            {
                DG.Rows[i].Cells[0].Value = rowNumbers[i + 1];
            }
            foreach (DataGridViewColumn column in DG.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            // Set the alignment for the specified column
            DG.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DG.Invalidate();
        }
        private void PB_PGUpdate(int _value)
        {
            PB_PGRead.Value = _value;
            PB_PGRead.Invalidate();
            PB_PGRead.Update();
        }
        public static int MapRange(double value, double minValue, double maxValue, int lowerRange, int upperRange)
        {
            // Map the value from the range [minValue, maxValue] to the range [50, 90]
            // Formula: ((value - minValue) / (maxValue - minValue)) * (newMax - newMin) + newMin
            return (int)(((value - minValue) / (maxValue - minValue)) * (upperRange - lowerRange) + lowerRange);
        }
        static int FindColumnIndexByName(DataGridView dataGridView, string columnName)
        {
            // Iterate through the columns and compare names
            for (int i = 0; i < dataGridView.Columns.Count; i++)
            {
                if (dataGridView.Columns[i].Name == columnName)
                {
                    // Column found, return its index
                    return i;
                }
            }

            // Column not found, return -1
            return -1;
        }
        private void SendDataPrint(byte[] packets, byte length)
        {
            string sentData = string.Empty;
            for (int index = 0; index < length; index++)
            {
                sentData = sentData + packets[index].ToString("X2") + " ";
            }
            sentData = "(S)" + "  " + sentData + " " + DateTime.Now.ToString(Constants.timeStampFormate) + "\r\n";
            LineTrafficControlEventHandler(sentData, "Send");
        }
        private void NewSendDataPrint(byte[] packets, int length)
        {
            string sentData = string.Empty;
            for (int index = 0; index < length; index++)
            {
                sentData = sentData + packets[index].ToString("X2") + " ";
            }
            sentData = "(S)" + "  " + sentData + " " + DateTime.Now.ToString(Constants.timeStampFormate) + "\r\n";
            LineTrafficControlEventHandler(sentData, "Send");
        }
        private void RecvDataPrint(byte[] packets, int length, bool IsLineTrafficEnabled = true)
        {
            string receivedData = string.Empty;
            int byteModeIndex = 0;
            for (int recIndex = 0; recIndex < length; recIndex++)
            {
                receivedData = receivedData + packets[recIndex].ToString("X2") + " ";
            }
            responseString = string.Empty;

            if (bytAddMode != 0)
                byteModeIndex = 14;
            else
                byteModeIndex = 11;
            for (int recIndex = byteModeIndex; recIndex < length - 3; recIndex++)
            {
                responseString = responseString + packets[recIndex].ToString("X2");
            }
            if (string.IsNullOrEmpty(receivedData))
                receivedData = "(R)" + "  " + "NULL" + " " + DateTime.Now.ToString(Constants.timeStampFormate) + Environment.NewLine;
            else
                receivedData = "(R)" + "  " + receivedData + " " + DateTime.Now.ToString(Constants.timeStampFormate) + Environment.NewLine;
            if (IsLineTrafficEnabled)
                LineTrafficControlEventHandler(receivedData, "Receive");
        }
        private void ResponsePrint()
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = (int)this.bytAddMode + 11; i < this.nCounter - 3; ++i)
                stringBuilder.Append(this.nRcvPkt[i].ToString("X2"));
            if (stringBuilder.ToString().StartsWith("CC") || stringBuilder.ToString().StartsWith("D4"))
            {
                if (stringBuilder.ToString().StartsWith("CC82") || stringBuilder.ToString().StartsWith("D482"))
                    stringBuilder.Remove(0, 8);
                else if (stringBuilder.ToString().StartsWith("CC81") || stringBuilder.ToString().StartsWith("D481"))
                    stringBuilder.Remove(0, 6);
                else
                    stringBuilder.Remove(0, 4);
                byte[] numArray = this.Decrypt(stringBuilder.ToString(), this.sEncryptkeyinHEX);
                stringBuilder.Length = 0;
                for (int index18 = 0; index18 < numArray.Length; ++index18)
                    stringBuilder.Append(numArray[index18].ToString("X2"));
                //this.swLT.Write("\t" + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss:ff tt") + "\t" + "Response : -- >> " + stringBuilder.ToString() + "\r\n");
                responseString = "Response : -- >> " + stringBuilder.ToString() + "\r\n";
            }
            else
                responseString = "Response : -- >> " + responseString + "\r\n";
            LineTrafficControlEventHandler($"     {responseString}\r\n", "Response");
        }
        public void ObjectListGetObisCode(string objectlist, out string[] obiscode, out int obiscnt)
        {
            try
            {
                DLMSParser parse = new DLMSParser(dataGridView);
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
        internal string Getdate(string tmpdate, int index, bool Postfix)
        {
            try
            {
                if (Postfix == true)
                    if (int.Parse(tmpdate.Substring(14 + index, 2), System.Globalization.NumberStyles.HexNumber) == 255)
                        return int.Parse(tmpdate.Substring(6 + index, 2), System.Globalization.NumberStyles.HexNumber).ToString("00") + "/" + int.Parse(tmpdate.Substring(4 + index, 2), System.Globalization.NumberStyles.HexNumber).ToString("00") + "/" + int.Parse(tmpdate.Substring(0 + index, 4), System.Globalization.NumberStyles.HexNumber).ToString("0000") + " " + int.Parse(tmpdate.Substring(10 + index, 2), System.Globalization.NumberStyles.HexNumber).ToString("00") + ":" + int.Parse(tmpdate.Substring(12 + index, 2), System.Globalization.NumberStyles.HexNumber).ToString("00") + "  DD/MM/YYYY HH:MM";
                    else
                        return int.Parse(tmpdate.Substring(6 + index, 2), System.Globalization.NumberStyles.HexNumber).ToString("00") + "/" + int.Parse(tmpdate.Substring(4 + index, 2), System.Globalization.NumberStyles.HexNumber).ToString("00") + "/" + int.Parse(tmpdate.Substring(0 + index, 4), System.Globalization.NumberStyles.HexNumber).ToString("0000") + " " + int.Parse(tmpdate.Substring(10 + index, 2), System.Globalization.NumberStyles.HexNumber).ToString("00") + ":" + int.Parse(tmpdate.Substring(12 + index, 2), System.Globalization.NumberStyles.HexNumber).ToString("00") + ":" + int.Parse(tmpdate.Substring(14 + index, 2), System.Globalization.NumberStyles.HexNumber).ToString("00") + "  DD/MM/YYYY HH:MM:SS";
                else
                    return int.Parse(tmpdate.Substring(6 + index, 2), System.Globalization.NumberStyles.HexNumber).ToString("00") + "/" + int.Parse(tmpdate.Substring(4 + index, 2), System.Globalization.NumberStyles.HexNumber).ToString("00") + "/" + int.Parse(tmpdate.Substring(0 + index, 4), System.Globalization.NumberStyles.HexNumber).ToString("0000") + " " + int.Parse(tmpdate.Substring(10 + index, 2), System.Globalization.NumberStyles.HexNumber).ToString("00") + ":" + int.Parse(tmpdate.Substring(12 + index, 2), System.Globalization.NumberStyles.HexNumber).ToString("00") + ":" + int.Parse(tmpdate.Substring(14 + index, 2), System.Globalization.NumberStyles.HexNumber).ToString("00");
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        static DataTable ConvertDataGridViewToDataTable(DataGridView dataGridView)
        {
            DataTable dataTable = new DataTable();

            // Add columns to DataTable
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                dataTable.Columns.Add(column.HeaderText, column.ValueType ?? typeof(string));
            }
            // Add rows to DataTable
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                DataRow dataRow = dataTable.NewRow();
                foreach (DataGridViewCell cell in row.Cells)
                {
                    dataRow[cell.ColumnIndex] = cell.Value;
                }
                dataTable.Rows.Add(dataRow);
            }
            return dataTable;
        }
        #endregion

        #region Methods to GET EVENTS RELATED and LS DE DATA TABLE
        public Dictionary<string, object> GetProfileDataTable(string profileObis, string scalerObis, int _startIndex = 0, int _endIndex = 0, byte nType = 2, string _startDT = null, string _endDT = null)
        {
            DLMSParser parse = new DLMSParser();
            string recivedObisString = "", recivedValueString = "", recivedScalerObisString = "", recivedScalerValueString = "", inUseEntries = "", profileEntries = "", option = "";
            DataTable obisDataTable = new DataTable();
            DataTable resultDataTable = new DataTable();
            string[] scalerObisArray = null, scalerScalerDataArray = null, mainSourceObisArray = null, finalScalerValuesToFill = null, ScalerMultiFactorArray = null;
            bool result = false;
            try
            {
                profileObis = string.Concat(profileObis.Trim().Split('.').Select(part => int.Parse(part).ToString("X2")));
                switch (profileObis)
                {
                    case "0000636200FF":
                        option = "Voltage Related Events";
                        break;
                    case "0000636201FF":
                        option = "Current Related Events";
                        break;
                    case "0000636202FF":
                        option = "Power Related Events";
                        break;
                    case "0000636203FF":
                        option = "Transaction Events";
                        break;
                    case "0000636204FF":
                        option = "Other Tamper Events";
                        break;
                    case "0000636205FF":
                        option = "Non Roll Over Events";
                        break;
                    case "0000636206FF":
                        option = "Control Events";
                        break;
                    case "0100620100FF":
                        option = "Billing Profile";
                        break;
                    case "01005E5B00FF":
                        option = "Instantaneous";
                        break;
                    case "0000636281FF":
                        option = "Mode of Relay Operation Profile";
                        break;
                    case "0100630100FF":
                        option = "Load Survey";
                        break;
                    case "0100630200FF":
                        option = "Daily Energy";
                        break;
                }
                scalerObis = string.Concat(scalerObis.Trim().Split('.').Select(part => int.Parse(part).ToString("X2")));

                #region Data Getting Logic
                //Get In use Entries
                result = GetParameter($"0007{profileObis}07", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                if (result && strbldDLMdata.ToString().Trim().Split(' ').Length == 4)
                {
                    inUseEntries = parse.GetProfileValueString(strbldDLMdata.ToString().Trim().Split(' ')[3]);
                }
                else
                    inUseEntries = "";
                //Get In Profile Entries
                result = GetParameter($"0007{profileObis}08", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                if (result && strbldDLMdata.ToString().Trim().Split(' ').Length == 4)
                {
                    profileEntries = parse.GetProfileValueString(strbldDLMdata.ToString().Trim().Split(' ')[3]);
                }
                else
                    profileEntries = "";
                //Get Profile Objects
                result = GetParameter($"0007{profileObis}03", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                if (result)
                    recivedObisString = strbldDLMdata.ToString().Trim();
                else
                    recivedObisString = "";
                //Get Profile Values
                if (option != "Load Survey" && option != "Daily Energy")
                    result = GetParameter($"0007{profileObis}02", nWait, nTryCount, nTimeOut, nType, DateTime.Now, DateTime.Now, string.Empty, (ulong)_startIndex, (ulong)_endIndex);
                else
                {
                    if (nType == 0)
                        result = GetParameter($"0007{profileObis}02", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                    else
                        result = GetParameter($"0007{profileObis}02", nWait, nTryCount, nTimeOut, (byte)1, DateTime.ParseExact(_startDT, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture), DateTime.ParseExact(_endDT, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture), "12000809060000010000FF0F02120000", 0UL, 0UL);
                }
                if (result)
                    recivedValueString = strbldDLMdata.ToString().Trim();
                else
                    recivedValueString = "";
                //Get Scaler Objects
                result = GetParameter($"0007{scalerObis}03", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                if (result)
                    recivedScalerObisString = strbldDLMdata.ToString().Trim();
                else
                    recivedScalerObisString = "";
                //Get Scaler Values
                result = GetParameter($"0007{scalerObis}02", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                if (result)
                    recivedScalerValueString = strbldDLMdata.ToString().Trim();
                else
                    recivedScalerValueString = "";
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

                resultDataTable = parse.GetEventsValuesDataTableParsing(recivedValueString, obisDataTable, option);
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
        #endregion

        #region TESTING FRAMEWORK
        public string SendReceiveTransparent(string strDataTx, Int64 interFrameTimeOut = 0)
        {
            string result = string.Empty;
            StringBuilder stringBuilder = new StringBuilder();
            this.strbldDLMdata.Remove(0, this.strbldDLMdata.Length);
            byte[] tempnPkt = StringToByteArray(strDataTx);
            for (int i = 0; i < tempnPkt.Length; i++)
            {
                nPkt[i] = tempnPkt[i];
            }
            //this.nPkt = StringToByteArray(strDataTx);
            //DateTime now1 = DateTime.Now;
            bool flag2 = false;
            this.ClearBuffer();
            //nRcvPkt = new byte[1024];
            this.nCounter = 0;
            this.WritePkt(this.nPkt, tempnPkt.Length);
            DateTime now2 = DateTime.Now;
            //if (interFrameTimeOut > 0)
            //    this.Wait(interFrameTimeOut);                  
            do
            {
                this.Wait(20);
                this.DataReceive();
                if ((this.nCounter > 2 && (int)this.nRcvPkt[2] + 2 <= this.nCounter) || (DateTime.Now.Subtract(now2).TotalMilliseconds > interFrameTimeOut))
                {
                    flag2 = true;
                    break;
                }
            }
            while (!flag2);
            RecvDataPrint(nRcvPkt, nCounter);
            for (int recIndex = 0; recIndex < nCounter; recIndex++)
            {
                result = result + nRcvPkt[recIndex].ToString("X2");
            }
            return result;
        }
        private void WritePkt(byte[] buffer, int length)
        {
            try
            {
                string empty = string.Empty;
                oSC.Write(buffer, 0, length);
                DateTime now = DateTime.Now;
                string sentData = string.Empty;
                for (int index = 0; index < length; index++)
                {
                    sentData = sentData + buffer[index].ToString("X2") + " ";
                }
                sentData = "(S)" + "  " + sentData + " " + now.ToString(Constants.timeStampFormate) + "\r\n";
                LineTrafficControlEventHandler(sentData, "Send");
            }
            catch (Exception ex) { log.Error(ex.Message.ToString()); }
        }

        private void ReadPkt()
        {
            try
            {
                DateTime now = DateTime.Now;
                byte[] bytes = oSC.GetBufferDataArray();
                string temp = string.Empty;
                nCounter = bytes.Length;
                if (bytes != null)
                    Array.Copy(bytes, nRcvPkt, bytes.Length);
                string recData = string.Empty;
                for (int index = 0; index < nCounter; index++)
                {
                    recData = recData + buffer[index].ToString("X2") + " ";
                }
                if (string.IsNullOrEmpty(recData))
                    recData = "(R)" + "  " + "NULL" + " " + now.ToString(Constants.timeStampFormate) + Environment.NewLine;
                else
                    recData = "(R)" + "  " + recData + " " + now.ToString(Constants.timeStampFormate) + Environment.NewLine;
                LineTrafficControlEventHandler(recData, "Receive");
            }
            catch (Exception ex) { log.Error(ex.Message.ToString()); }
        }

        #region SINGLE ENTRY PROFILE
        /// <summary>
        /// This Takes profile obis and its scaler obis and return single entry in horizontal form
        /// </summary>
        /// <param name="profileObis"></param>
        /// <param name="scalerObis"></param>
        /// <returns> It return the DataTable, ProfileObis, ProfileValues, ScalerObis and ScalerValues.</returns>
        public Dictionary<string, object> GetSingleEntryProfileDataTable(string profileObis, string scalerObis)
        {
            DLMSParser parse = new DLMSParser();
            string recivedObisString = "", recivedValueString = "", recivedScalerObisString = "", recivedScalerValueString = "";
            DataTable obisDataTable = new DataTable();
            // Add columns to the DataTable
            obisDataTable.Columns.Add("Obis", typeof(string));
            obisDataTable.Columns.Add("Name", typeof(string));
            obisDataTable.Columns.Add("Data", typeof(string));
            DataTable resultDataTable = new DataTable();
            string[] scalerObisArray = null, scalerScalerDataArray = null, mainSourceObisArray = null, finalScalerValuesToFill = null, ScalerMultiFactorArray = null;
            bool result = false;
            try
            {
                profileObis = string.Concat(profileObis.Trim().Split('.').Select(part => int.Parse(part).ToString("X2")));
                scalerObis = string.Concat(scalerObis.Trim().Split('.').Select(part => int.Parse(part).ToString("X2")));
                #region Data Getting Logic
                //Get Profile Objects
                result = GetParameter($"0007{profileObis}03", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                if (result)
                    recivedObisString = strbldDLMdata.ToString().Trim();
                else
                    recivedObisString = "";
                //Get Profile Values
                if (profileObis == "01005E5B00FF" || profileObis == "0000636205FF")//For Instant and Non Roll Over
                    result = GetParameter($"0007{profileObis}02", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                else
                    result = GetParameter($"0007{profileObis}02", nWait, nTryCount, nTimeOut, (byte)2, DateTime.Now, DateTime.Now, string.Empty, 1, 1);
                if (result)
                    recivedValueString = strbldDLMdata.ToString().Trim();
                else
                    recivedValueString = "";
                //Get Scaler Objects
                result = GetParameter($"0007{scalerObis}03", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                if (result)
                    recivedScalerObisString = strbldDLMdata.ToString().Trim();
                else
                    recivedScalerObisString = "";
                //Get Scaler Values
                result = GetParameter($"0007{scalerObis}02", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                if (result)
                    recivedScalerValueString = strbldDLMdata.ToString().Trim();
                else
                    recivedScalerValueString = "";

                obisDataTable = parse.GetParameterTableHorizontal(recivedObisString);
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
                scalerObisArray = parse.GetScalerProfileObisListParsing(recivedScalerObisString);
                scalerScalerDataArray = parse.GetScalerProfileScalerListParsing(recivedScalerValueString);
                mainSourceObisArray = obisDataTable.AsEnumerable()
                                         .Select(row => row.Field<string>(3)) // Change the type to match the third column's type
                                         .ToArray();
                finalScalerValuesToFill = new string[mainSourceObisArray.Length];
                ScalerMultiFactorArray = new string[mainSourceObisArray.Length];
                foreach (var selectedObis in scalerObisArray)
                {
                    // Find the index of the searchString in the stringArray
                    int index = Array.IndexOf(mainSourceObisArray, selectedObis);
                    int scalarIndex = Array.IndexOf(scalerObisArray, selectedObis);
                    if (index != -1)
                    {
                        if (index > mainSourceObisArray.Length)
                            break;
                        else
                        {
                            finalScalerValuesToFill[index] = scalerScalerDataArray[scalarIndex];
                            //dataGridView.Rows[index].Cells[5].Value = scalerScalerDataArray[scalarIndex];
                            obisDataTable.Rows[index][5] = scalerScalerDataArray[scalarIndex];
                        }
                    }
                }
                resultDataTable = parse.GetBillingValuesDataTableParsing(recivedValueString, obisDataTable);
                // Multiply each row value with the corresponding values in "Data" columns
                RenameParameterWithUnit(resultDataTable, parse);
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
                            { "ScalerValues", recivedScalerValueString }
                        };
            }

            // Create a dictionary to hold the DataTable and strings
            return new Dictionary<string, object>
                        {
                            { "DataTable", resultDataTable },
                            { "ProfileObis", recivedObisString},
                            { "ProfileValues", recivedValueString },
                            { "ScalerObis", recivedScalerObisString },
                            { "ScalerValues", recivedScalerValueString }
                        };
        }
        public Dictionary<string, object> GetSingleEntryLSorDEDataTable(string profileObis, string scalerObis, string _startDT = null, string _endDT = null, int nType = 0)
        {
            DLMSParser parse = new DLMSParser();
            string recivedObisString = "", recivedValueString = "", recivedScalerObisString = "", recivedScalerValueString = "";
            DataTable obisDataTable = new DataTable();
            // Add columns to the DataTable
            obisDataTable.Columns.Add("Obis", typeof(string));
            obisDataTable.Columns.Add("Name", typeof(string));
            obisDataTable.Columns.Add("Data", typeof(string));
            DataTable resultDataTable = new DataTable();
            string[] scalerObisArray = null, scalerScalerDataArray = null, mainSourceObisArray = null, finalScalerValuesToFill = null, ScalerMultiFactorArray = null;
            bool result = false;
            try
            {
                profileObis = string.Concat(profileObis.Trim().Split('.').Select(part => int.Parse(part).ToString("X2")));
                scalerObis = string.Concat(scalerObis.Trim().Split('.').Select(part => int.Parse(part).ToString("X2")));
                #region Data Getting Logic
                //Get Profile Objects
                result = GetParameter($"0007{profileObis}03", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                if (result)
                    recivedObisString = strbldDLMdata.ToString().Trim();
                else
                    recivedObisString = "";
                //Get Profile Values
                if (nType == 0)
                    result = GetParameter($"0007{profileObis}02", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                else
                    result = GetParameter($"0007{profileObis}02", nWait, nTryCount, nTimeOut, (byte)1, DateTime.ParseExact(_startDT, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture), DateTime.ParseExact(_endDT, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture), "12000809060000010000FF0F02120000", 0UL, 0UL);
                if (result)
                    recivedValueString = strbldDLMdata.ToString().Trim();
                else
                    recivedValueString = "";
                //Get Scaler Objects
                result = GetParameter($"0007{scalerObis}03", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                if (result)
                    recivedScalerObisString = strbldDLMdata.ToString().Trim();
                else
                    recivedScalerObisString = "";
                //Get Scaler Values
                result = GetParameter($"0007{scalerObis}02", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                if (result)
                    recivedScalerValueString = strbldDLMdata.ToString().Trim();
                else
                    recivedScalerValueString = "";

                obisDataTable = parse.GetParameterTableHorizontal(recivedObisString);
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
                scalerObisArray = parse.GetScalerProfileObisListParsing(recivedScalerObisString);
                scalerScalerDataArray = parse.GetScalerProfileScalerListParsing(recivedScalerValueString);
                mainSourceObisArray = obisDataTable.AsEnumerable()
                                         .Select(row => row.Field<string>(3)) // Change the type to match the third column's type
                                         .ToArray();
                finalScalerValuesToFill = new string[mainSourceObisArray.Length];
                ScalerMultiFactorArray = new string[mainSourceObisArray.Length];
                foreach (var selectedObis in scalerObisArray)
                {
                    // Find the index of the searchString in the stringArray
                    int index = Array.IndexOf(mainSourceObisArray, selectedObis);
                    int scalarIndex = Array.IndexOf(scalerObisArray, selectedObis);
                    if (index != -1)
                    {
                        if (index > mainSourceObisArray.Length)
                            break;
                        else
                        {
                            finalScalerValuesToFill[index] = scalerScalerDataArray[scalarIndex];
                            //dataGridView.Rows[index].Cells[5].Value = scalerScalerDataArray[scalarIndex];
                            obisDataTable.Rows[index][5] = scalerScalerDataArray[scalarIndex];
                        }
                    }
                }
                resultDataTable = parse.GetBillingValuesDataTableParsing(recivedValueString, obisDataTable);
                // Multiply each row value with the corresponding values in "Data" columns
                RenameParameterWithUnit(resultDataTable, parse);
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
                            { "ScalerValues", recivedScalerValueString }
                        };
            }

            // Create a dictionary to hold the DataTable and strings
            return new Dictionary<string, object>
                        {
                            { "DataTable", resultDataTable },
                            { "ProfileObis", recivedObisString},
                            { "ProfileValues", recivedValueString },
                            { "ScalerObis", recivedScalerObisString },
                            { "ScalerValues", recivedScalerValueString }
                        };
        }



        #endregion

        #region NAMEPLATE PROFILE
        public Dictionary<string, object> GetNameplateProfileDataTable()
        {
            DLMSParser parse = new DLMSParser();
            string recivedObisString = "", recivedValueString = "", recivedScalerObisString = "", recivedScalerValueString = "", profileObis = "", scalerObis = "", option = "";
            DataTable obisDataTable = new DataTable();
            // Add columns to the DataTable
            obisDataTable.Columns.Add("Obis", typeof(string));
            obisDataTable.Columns.Add("Name", typeof(string));
            obisDataTable.Columns.Add("Data", typeof(string));
            DataTable resultDataTable = new DataTable();
            string[] scalerObisArray = null, scalerScalerDataArray = null, mainSourceObisArray = null, finalScalerValuesToFill = null, ScalerMultiFactorArray = null;
            bool result = false;
            try
            {
                option = "Nameplate Profile";
                profileObis = string.Concat("0.0.94.91.10.255".Trim().Split('.').Select(part => int.Parse(part).ToString("X2")));
                //scalerObis = string.Concat("1.0.94.91.3.255".Trim().Split('.').Select(part => int.Parse(part).ToString("X2")));
                #region Data Getting Logic
                //Get Profile Objects
                result = GetParameter($"0007{profileObis}03", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                if (result)
                    recivedObisString = strbldDLMdata.ToString().Trim();
                else
                    recivedObisString = "";
                //Get Profile Values

                result = GetParameter($"0007{profileObis}02", nWait, nTryCount, nTimeOut, (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL);
                if (result)
                    recivedValueString = strbldDLMdata.ToString().Trim();
                else
                    recivedValueString = "";
                obisDataTable = parse.GetClassObisAttScalerListParsing(recivedObisString, option);
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
                /*
                 scalerObisArray = parse.GetScalerProfileObisListParsing(recivedScalerObisString);
                 scalerScalerDataArray = parse.GetScalerProfileScalerListParsing(recivedScalerValueString);
                 mainSourceObisArray = obisDataTable.AsEnumerable()
                                          .Select(row => row.Field<string>(3)) // Change the type to match the third column's type
                                          .ToArray();
                 finalScalerValuesToFill = new string[mainSourceObisArray.Length];
                 ScalerMultiFactorArray = new string[mainSourceObisArray.Length];
                 foreach (var selectedObis in scalerObisArray)
                 {
                     // Find the index of the searchString in the stringArray
                     int index = Array.IndexOf(mainSourceObisArray, selectedObis);
                     int scalarIndex = Array.IndexOf(scalerObisArray, selectedObis);
                     if (index != -1)
                     {
                         if (index > mainSourceObisArray.Length)
                             break;
                         else
                         {
                             finalScalerValuesToFill[index] = scalerScalerDataArray[scalarIndex];
                             //dataGridView.Rows[index].Cells[5].Value = scalerScalerDataArray[scalarIndex];
                             obisDataTable.Rows[index][5] = scalerScalerDataArray[scalarIndex];
                         }
                     }
                 }
                 */
                resultDataTable = parse.GetBillingValuesDataTableParsing(recivedValueString, obisDataTable);
                // Multiply each row value with the corresponding values in "Data" columns
                //RenameParameterWithUnit(resultDataTable, parse);
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

        #endregion

        #region Invocation ID priority Negotiated
        public bool GetParameterInvokeId(
          string sWhichData,
          byte nWait,
          byte nTryCount,
          byte nTimeOut,
          byte nType,
          DateTime dateStartDate,
          DateTime dateEndDate,
          string sOBISSelect,
          ulong nFrom,
          ulong nTo,
          bool IsLineTrafficEnabled = true,
          string sOBISlist = "0100",
          string invokeIdPriority = "C1")
        {
            if (IsLineTrafficEnabled)
                LineTrafficControlEventHandler($"     GET CLASS-{Convert.ToInt32(sWhichData.Substring(0, 4), 16)} | OBIS-{DLMSParser.GetObis(sWhichData.Substring(4, 12))} [{DLMSParser.GetObisName(Convert.ToInt32(sWhichData.Substring(0, 4), 16).ToString(), DLMSParser.GetObis(sWhichData.Substring(4, 12)), Convert.ToInt32(sWhichData.Substring(sWhichData.Length - 2), 16).ToString())}] | Attribute-{Convert.ToInt32(sWhichData.Substring(sWhichData.Length - 2), 16)}", "Send");
            bool flag1 = false;
            long num1 = 0;
            byte num2 = Convert.ToByte((int)this.bytAddMode + 8);
            byte num3 = 0;
            string empty = string.Empty;
            StringBuilder stringBuilder = new StringBuilder();
            this.strbldDLMdata.Remove(0, this.strbldDLMdata.Length);
            //DLM file OBIS code and Attribute ID
            strbldDLMdata.Append($"\r\n{sWhichData.Substring(0, 4)} {sWhichData.Substring(4, 12)} {sWhichData.Substring(sWhichData.Length - 2)} ");
            //++this.PB1.Value;
            //if (this.PB1.Value == 100)
            //  this.PB1.Value = 0;
            this.nRetLSH = Convert.ToByte((int)this.nRecvCntr << 5);
            this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nRetLSH | 16);
            this.nRetLSH = Convert.ToByte((int)this.nSentCntr << 1);
            this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nRetLSH | (int)this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)]);
            byte[] nPkt1 = this.nPkt;
            int index1 = (int)num2;
            byte num4 = (byte)(index1 + 1);
            nPkt1[index1] = (byte)230;//E6
            byte[] nPkt2 = this.nPkt;
            int index2 = (int)num4;
            byte num5 = (byte)(index2 + 1);
            nPkt2[index2] = (byte)230;//E6
            byte[] nPkt3 = this.nPkt;
            int index3 = (int)num5;
            byte num6 = (byte)(index3 + 1);
            nPkt3[index3] = (byte)0;//00
            string hex1;
            int num7;
            switch (nType)
            {
                case 0:
                    hex1 = $"C001{invokeIdPriority}" + sWhichData + "00";
                    break;
                case 1:
                    if (sOBISSelect == "12000809060000010000FF0F02120000")
                    {
                        string[] strArray = new string[20];
                        strArray[0] = "C00181";
                        strArray[1] = sWhichData;
                        strArray[2] = "010102040204";
                        strArray[3] = sOBISSelect;
                        strArray[4] = "090C";
                        strArray[5] = dateStartDate.Year.ToString("X4");
                        strArray[6] = dateStartDate.Month.ToString("X2");
                        num7 = dateStartDate.Day;
                        strArray[7] = num7.ToString("X2");
                        strArray[8] = "FF";
                        num7 = dateStartDate.Hour;
                        strArray[9] = num7.ToString("X2");
                        num7 = dateStartDate.Minute;
                        strArray[10] = num7.ToString("X2");
                        strArray[11] = "0000800000090C";
                        num7 = dateEndDate.Year;
                        strArray[12] = num7.ToString("X4");
                        num7 = dateEndDate.Month;
                        strArray[13] = num7.ToString("X2");
                        num7 = dateEndDate.Day;
                        strArray[14] = num7.ToString("X2");
                        strArray[15] = "FF";
                        num7 = dateEndDate.Hour;
                        strArray[16] = num7.ToString("X2");
                        num7 = dateEndDate.Minute;
                        strArray[17] = num7.ToString("X2");
                        strArray[18] = "00008000000100";
                        strArray[19] = sOBISlist;
                        hex1 = string.Concat(strArray);
                        break;
                    }
                    hex1 = "C00181" + sWhichData + "010102040204" + sOBISSelect + "06" + nFrom.ToString("X8") + "06" + nTo.ToString("X8") + sOBISlist;
                    break;
                case 2:
                    //hex1 = "C00181" + sWhichData + "0102020406" + nFrom.ToString("X8") + "06" + nTo.ToString("X8") + "12" + nFrom.ToString("X4") + "12" + this.Tovalue.ToString("X4");
                    hex1 = "C00181" + sWhichData + "0102020406" + nFrom.ToString("X8") + "06" + nTo.ToString("X8") + "120001" + "120000";
                    break;
                default:
                    hex1 = "C00181" + sWhichData + "0102020406" + this.FromEntry.ToString("X8") + "06" + this.ToEntry.ToString("X8") + "12" + this.Fromvalue.ToString("X4") + "12" + this.Tovalue.ToString("X4");
                    break;
            }
            //this.swLT.Write("\t" + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss:ff tt") + "\t" + "Command : -- >> " + hex1 + "\r\n");
            commandString = "Command  : -- >> " + hex1 + "\r\n";
            if (DLMSInfo.IsLNWithCipher || DLMSInfo.IsLNWithCipherDedicatedKey)
            {
                if (DLMSInfo.AccessMode == 1 || DLMSInfo.AccessMode == 3)
                    num3 = (byte)32;
                else if (DLMSInfo.AccessMode == 2)
                    num3 = (byte)48;
                byte num8;
                if (DLMSInfo.IsLNWithCipherDedicatedKey)
                {
                    byte[] nPkt4 = this.nPkt;
                    int index4 = (int)num6;
                    num8 = (byte)(index4 + 1);
                    nPkt4[index4] = (byte)208;
                }
                else
                {
                    byte[] nPkt5 = this.nPkt;
                    int index5 = (int)num6;
                    num8 = (byte)(index5 + 1);
                    nPkt5[index5] = (byte)200;
                }
                byte[] nPkt6 = this.nPkt;
                int index6 = (int)num8;
                byte num9 = (byte)(index6 + 1);
                nPkt6[index6] = (byte)0;
                byte[] nPkt7 = this.nPkt;
                int index7 = (int)num9;
                byte num10 = (byte)(index7 + 1);
                int num11 = (int)num3;
                nPkt7[index7] = (byte)num11;
                byte[] nPkt8 = this.nPkt;
                int index8 = (int)num10;
                byte num12 = (byte)(index8 + 1);
                nPkt8[index8] = (byte)0;
                byte[] nPkt9 = this.nPkt;
                int index9 = (int)num12;
                byte num13 = (byte)(index9 + 1);
                nPkt9[index9] = (byte)0;
                byte[] nPkt10 = this.nPkt;
                int index10 = (int)num13;
                byte num14 = (byte)(index10 + 1);
                int num15 = (int)Convert.ToByte(this.nCommandCounter / 256);
                nPkt10[index10] = (byte)num15;
                byte[] nPkt11 = this.nPkt;
                int index11 = (int)num14;
                num6 = (byte)(index11 + 1);
                int num16 = (int)Convert.ToByte(this.nCommandCounter % 256);
                nPkt11[index11] = (byte)num16;
                string str1 = num3.ToString("X2");
                num7 = this.nCommandCounter++;
                string str2 = num7.ToString("X8");
                string str3 = hex1;
                byte[] numArray = this.Encrypt(str1 + str2 + str3, this.sEncryptkeyinHEX);
                this.nPkt[(int)num6 - 6] = Convert.ToByte(numArray.Length + 5);
                for (int index12 = 0; index12 < numArray.Length; ++index12)
                    this.nPkt[(int)num6++] = numArray[index12];
            }
            else
            {
                foreach (byte num17 in StringToByteArray(hex1))
                    this.nPkt[(int)num6++] = num17;
                commandString = "Command  : -- >> " + hex1 + "\r\n";
            }
            this.nPkt[2] = Convert.ToByte((int)num6 + 1);
            this.DCl.fcs(ref this.nPkt, (int)Convert.ToByte((int)this.bytAddMode + 5), (byte)1);
            this.DCl.fcs(ref this.nPkt, (int)Convert.ToByte((int)num6 - 1), (byte)1);
            this.nPkt[(int)num6 + 2] = (byte)126;
            byte num18 = 0;
            bool flag2;
            DateTime now1;
            //LineTrafficControlEventHandler($"     GET CLASS-{Convert.ToInt32(sWhichData.Substring(0, 4), 16)} | OBIS-{DLMSParser.GetObis(sWhichData.Substring(4, 12))} [{DLMSParser.GetObisName(Convert.ToInt32(sWhichData.Substring(0, 4), 16).ToString(), DLMSParser.GetObis(sWhichData.Substring(4, 12)))}] | Attribute-{Convert.ToInt32(sWhichData.Substring(sWhichData.Length - 2), 16)}", "Send");
            do
            {
                this.Wait((double)nWait);
                this.ClearBuffer();
                flag2 = false;
                this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)num6 + 3));
                if (IsLineTrafficEnabled)
                    SendDataPrint(nPkt, Convert.ToByte((int)num6 + 3));
                //LineTrafficControlEventHandler($"     {commandString}", "Command");
                DateTime now2 = DateTime.Now;
                while (true)
                {
                    Application.DoEvents();
                    this.DataReceive();
                    num7 = (int)this.nRcvPkt[1] & 7;
                    this.pktLength = int.Parse(num7.ToString("X2") + this.nRcvPkt[2].ToString("X2"), NumberStyles.HexNumber);
                    if (this.nCounter <= 2 || this.pktLength + 2 > this.nCounter || this.nRcvPkt[this.pktLength + 1] != (byte)126)
                    {
                        now1 = DateTime.Now;
                        if (now1.Subtract(now2).Seconds <= (int)nTimeOut || (int)num18 >= (int)nTryCount)
                        {
                            if ((int)num18 == (int)nTryCount)
                                goto label_38;
                        }
                        else
                            goto label_26;
                    }
                    else
                        break;
                }
                RecvDataPrint(nRcvPkt, nCounter, IsLineTrafficEnabled);
                if (IsLineTrafficEnabled)
                {
                    LineTrafficControlEventHandler($"     {commandString}", "Command");
                    ResponsePrint();
                }
                //LineTrafficControlEventHandler($"     {responseString}\r\n", "Response");
                //LineTrafficControlEventHandler("\r\n", "Send");
                flag2 = true;
                num18 = (byte)0;
                this.FrameType();
                goto label_38;
            label_26:
                if (DLMSInfo.CmbDirect == 1 && this.nCounter > 0)
                {
                    if (this.nRcvPkt[0] != (byte)126)
                        this.ClearBuffer();
                    flag2 = false;
                    this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)this.bytAddMode + 27));
                    if (IsLineTrafficEnabled)
                        SendDataPrint(nPkt, Convert.ToByte((int)this.bytAddMode + 27));
                    //LineTrafficControlEventHandler($"     {commandString}", "Command");
                    DateTime now3 = DateTime.Now;
                    while (true)
                    {
                        Application.DoEvents();
                        this.DataReceive();
                        num7 = (int)this.nRcvPkt[1] & 7;
                        this.pktLength = int.Parse(num7.ToString("X2") + this.nRcvPkt[2].ToString("X2"), NumberStyles.HexNumber);
                        if (this.pktLength + 2 > this.nCounter || this.nRcvPkt[this.pktLength + 1] != (byte)126)
                        {
                            now1 = DateTime.Now;
                            if (now1.Subtract(now3).Seconds > (int)nTimeOut)
                                goto label_33;
                        }
                        else
                            break;
                    }
                    RecvDataPrint(nRcvPkt, nCounter, IsLineTrafficEnabled);
                    if (IsLineTrafficEnabled)
                    {
                        LineTrafficControlEventHandler($"     {commandString}", "Command");
                        ResponsePrint();
                    }
                    flag2 = true;
                    oSC.DiscardInputOutputBuffer();
                    num18 = (byte)0;
                    this.FrameType();
                    goto label_38;
                label_33:
                    ++num18;
                }
                else
                    ++num18;
                label_38:
                if (flag2)
                {
                    this.temp = string.Empty;
                    num7 = (int)this.nRcvPkt[1] & 7;
                    this.pktLength = int.Parse(num7.ToString("X2") + this.nRcvPkt[2].ToString("X2"), NumberStyles.HexNumber);
                    for (int index13 = 0; index13 < this.pktLength + 2; ++index13)
                        this.temp += this.nRcvPkt[index13].ToString("X2");
                    this.temp += "\r\n";
                    //this.swLT.Write("(R)\t" + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss:ff tt") + "\t" + this.temp);
                    responseString = temp.Trim();
                    responseString = $"Response : -- >> {responseString.Substring(22, responseString.Length - 28)}\r\n";
                }
            }
            while (!flag2 && (int)num18 != (int)nTryCount);
            if (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] == (byte)151 || ((int)this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] & 1) == 1)
                return false;
            for (int index14 = (int)this.bytAddMode + 11; index14 < this.nCounter - 3; ++index14)
                stringBuilder.Append(this.nRcvPkt[index14].ToString("X2"));
            while (((int)this.nRcvPkt[1] & 168) == 168)
            {
                this.nPkt[2] = Convert.ToByte((int)this.bytAddMode + 7);
                this.nRetLSH = Convert.ToByte((int)this.nRecvCntr << 5);
                this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nRetLSH | 16);
                this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] | 1);
                this.DCl.fcs(ref this.nPkt, (int)Convert.ToByte((int)this.bytAddMode + 5), (byte)1);
                this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 8)] = (byte)126;
                byte num19 = 0;
                bool flag3;
                do
                {
                    this.Wait((double)nWait);
                    this.ClearBuffer();
                    flag3 = false;
                    this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)this.bytAddMode + 9));
                    if (IsLineTrafficEnabled)
                        SendDataPrint(nPkt, Convert.ToByte((int)this.bytAddMode + 9));
                    //LineTrafficControlEventHandler($"     {commandString}", "Command");
                    DateTime now4 = DateTime.Now;
                    while (true)
                    {
                        Application.DoEvents();
                        this.DataReceive();
                        num7 = (int)this.nRcvPkt[1] & 7;
                        this.pktLength = int.Parse(num7.ToString("X2") + this.nRcvPkt[2].ToString("X2"), NumberStyles.HexNumber);
                        if (this.nCounter <= 2 || this.pktLength + 2 > this.nCounter || this.nRcvPkt[this.pktLength + 1] != (byte)126)
                        {
                            now1 = DateTime.Now;
                            if (now1.Subtract(now4).Seconds <= (int)nTimeOut || (int)num19 >= (int)nTryCount)
                            {
                                if ((int)num19 == (int)nTryCount)
                                    goto label_72;
                            }
                            else
                                goto label_57;
                        }
                        else
                            break;
                    }
                    RecvDataPrint(nRcvPkt, nCounter, IsLineTrafficEnabled);
                    LineTrafficControlEventHandler($"     {commandString}", "Command");
                    ResponsePrint();
                    flag3 = true;
                    num19 = (byte)0;
                    for (int index15 = (int)Convert.ToByte((int)this.bytAddMode + 8); index15 < this.pktLength - 1; ++index15)
                        stringBuilder.Append(this.nRcvPkt[index15].ToString("X2"));
                    this.FrameType();
                    goto label_72;
                label_57:
                    if (DLMSInfo.CmbDirect == 1 && this.nCounter > 0)
                    {
                        if (this.nRcvPkt[0] != (byte)126)
                            this.ClearBuffer();
                        flag3 = false;
                        this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)this.bytAddMode + 9));
                        if (IsLineTrafficEnabled)
                            SendDataPrint(nPkt, Convert.ToByte((int)this.bytAddMode + 9));
                        //LineTrafficControlEventHandler($"     {commandString}", "Command");
                        DateTime now5 = DateTime.Now;
                        while (true)
                        {
                            Application.DoEvents();
                            this.DataReceive();
                            num7 = (int)this.nRcvPkt[1] & 7;
                            this.pktLength = int.Parse(num7.ToString("X2") + this.nRcvPkt[2].ToString("X2"), NumberStyles.HexNumber);
                            if (this.pktLength + 2 > this.nCounter || this.nRcvPkt[this.pktLength + 1] != (byte)126)
                            {
                                now1 = DateTime.Now;
                                if (now1.Subtract(now5).Seconds > (int)nTimeOut)
                                    goto label_67;
                            }
                            else
                                break;
                        }
                        RecvDataPrint(nRcvPkt, nCounter, IsLineTrafficEnabled);
                        LineTrafficControlEventHandler($"     {commandString}", "Command");
                        ResponsePrint();
                        flag3 = true;
                        oSC.DiscardInputOutputBuffer();
                        num19 = (byte)0;
                        for (int index16 = (int)Convert.ToByte((int)this.bytAddMode + 8); index16 < this.pktLength - 1; ++index16)
                            stringBuilder.Append(this.nRcvPkt[index16].ToString("X2"));
                        this.FrameType();
                        goto label_72;
                    label_67:
                        ++num19;
                    }
                    else
                        ++num19;
                    label_72:
                    if (flag3)
                    {
                        this.temp = string.Empty;
                        for (int index17 = 0; index17 < this.pktLength + 2; ++index17)
                            this.temp += this.nRcvPkt[index17].ToString("X2");
                        this.temp += "\r\n";
                        //this.swLT.Write("(R)\t" + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss:ff tt") + "\t" + this.temp);
                    }
                    if (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] == (byte)151)
                        goto label_78;
                }
                while (!flag3 && (int)num19 != (int)nTryCount);
                goto label_81;
            label_78:
                return false;
            label_81:
                if (!flag3 || this.nRcvPkt[1] != (byte)160)
                {
                    if (!flag3)
                        return false;
                }
                else
                    break;
            }
            if (stringBuilder.ToString().StartsWith("CC") || stringBuilder.ToString().StartsWith("D4"))
            {
                if (stringBuilder.ToString().StartsWith("CC82") || stringBuilder.ToString().StartsWith("D482"))
                    stringBuilder.Remove(0, 8);
                else if (stringBuilder.ToString().StartsWith("CC81") || stringBuilder.ToString().StartsWith("D481"))
                    stringBuilder.Remove(0, 6);
                else
                    stringBuilder.Remove(0, 4);
                byte[] numArray = this.Decrypt(stringBuilder.ToString(), this.sEncryptkeyinHEX);
                stringBuilder.Length = 0;
                for (int index18 = 0; index18 < numArray.Length; ++index18)
                    stringBuilder.Append(numArray[index18].ToString("X2"));
                //this.swLT.Write("\t" + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss:ff tt") + "\t" + "Response : -- >> " + stringBuilder.ToString() + "\r\n");
                responseString = "Response : -- >> " + stringBuilder.ToString() + "\r\n";
            }
            if (stringBuilder.ToString().StartsWith("C401"))
                stringBuilder.Remove(0, 8);
            if (stringBuilder.ToString().StartsWith("C402"))
            {
                num1 = Convert.ToInt64(stringBuilder.ToString().Substring(8, 8), 16);
                flag1 = !Convert.ToBoolean(Convert.ToInt16(stringBuilder.ToString().Substring(6, 2)));
                if (stringBuilder.ToString().Substring(18, 2) == "82")
                    stringBuilder.Remove(0, 24);
                else if (stringBuilder.ToString().Substring(18, 2) == "81")
                    stringBuilder.Remove(0, 22);
                else
                    stringBuilder.Remove(0, 20);
            }
            this.strbldDLMdata.Append(stringBuilder.ToString());
            stringBuilder.Length = 0;
            while (flag1)
            {
                this.temp = string.Empty;
                flag1 = false;
                this.nRetLSH = Convert.ToByte((int)this.nRecvCntr << 5);
                this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nRetLSH | 16);
                this.nRetLSH = Convert.ToByte((int)this.nSentCntr << 1);
                this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nRetLSH | (int)this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)]);
                byte num20 = Convert.ToByte((int)this.bytAddMode + 8);
                byte[] nPkt12 = this.nPkt;
                int index19 = (int)num20;
                byte num21 = (byte)(index19 + 1);
                nPkt12[index19] = (byte)230;
                byte[] nPkt13 = this.nPkt;
                int index20 = (int)num21;
                byte num22 = (byte)(index20 + 1);
                nPkt13[index20] = (byte)230;
                byte[] nPkt14 = this.nPkt;
                int index21 = (int)num22;
                byte num23 = (byte)(index21 + 1);
                nPkt14[index21] = (byte)0;
                string hex2 = "C00281" + num1.ToString("X8");
                commandString = "Command : -- >> " + hex2 + "\r\n";
                if (DLMSInfo.IsLNWithCipher)
                {
                    //this.swLT.Write("\t" + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss:ff tt") + "\t" + "Command : -- >> " + hex2 + "\r\n");
                    commandString = "Command : -- >> " + hex2 + "\r\n";
                    byte num24;
                    if (DLMSInfo.IsLNWithCipherDedicatedKey)
                    {
                        byte[] nPkt15 = this.nPkt;
                        int index22 = (int)num23;
                        num24 = (byte)(index22 + 1);
                        nPkt15[index22] = (byte)208;
                    }
                    else
                    {
                        byte[] nPkt16 = this.nPkt;
                        int index23 = (int)num23;
                        num24 = (byte)(index23 + 1);
                        nPkt16[index23] = (byte)200;
                    }
                    byte[] nPkt17 = this.nPkt;
                    int index24 = (int)num24;
                    byte num25 = (byte)(index24 + 1);
                    nPkt17[index24] = (byte)0;
                    byte[] nPkt18 = this.nPkt;
                    int index25 = (int)num25;
                    byte num26 = (byte)(index25 + 1);
                    int num27 = (int)num3;
                    nPkt18[index25] = (byte)num27;
                    byte[] nPkt19 = this.nPkt;
                    int index26 = (int)num26;
                    byte num28 = (byte)(index26 + 1);
                    nPkt19[index26] = (byte)0;
                    byte[] nPkt20 = this.nPkt;
                    int index27 = (int)num28;
                    byte num29 = (byte)(index27 + 1);
                    nPkt20[index27] = (byte)0;
                    byte[] nPkt21 = this.nPkt;
                    int index28 = (int)num29;
                    byte num30 = (byte)(index28 + 1);
                    int num31 = (int)Convert.ToByte(this.nCommandCounter / 256);
                    nPkt21[index28] = (byte)num31;
                    byte[] nPkt22 = this.nPkt;
                    int index29 = (int)num30;
                    num23 = (byte)(index29 + 1);
                    int num32 = (int)Convert.ToByte(this.nCommandCounter % 256);
                    nPkt22[index29] = (byte)num32;
                    string str4 = num3.ToString("X2");
                    num7 = this.nCommandCounter++;
                    string str5 = num7.ToString("X8");
                    string str6 = hex2;
                    byte[] numArray = this.Encrypt(str4 + str5 + str6, this.sEncryptkeyinHEX);
                    this.nPkt[(int)num23 - 6] = Convert.ToByte(numArray.Length + 5);
                    for (int index30 = 0; index30 < numArray.Length; ++index30)
                        this.nPkt[(int)num23++] = numArray[index30];
                }
                else
                {
                    foreach (byte num33 in StringToByteArray(hex2))
                        this.nPkt[(int)num23++] = num33;
                    //commandString = "Command : -- >> " + hex2 + "\r\n";
                }
                this.nPkt[2] = Convert.ToByte((int)num23 + 1);
                this.DCl.fcs(ref this.nPkt, (int)Convert.ToByte((int)this.bytAddMode + 5), (byte)1);
                this.DCl.fcs(ref this.nPkt, (int)Convert.ToByte((int)num23 - 1), (byte)1);
                this.nPkt[(int)num23 + 2] = (byte)126;
                byte num34 = 0;
                bool flag4;
                do
                {
                    this.ClearBuffer();
                    flag4 = false;
                    this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)num23 + 3));
                    if (IsLineTrafficEnabled)
                        SendDataPrint(nPkt, Convert.ToByte((int)num23 + 3));
                    //LineTrafficControlEventHandler($"     {commandString}", "Command");
                    this.Wait((double)nWait);
                    //SendDataPrint(nPkt, Convert.ToByte((int)num23 + 3));
                    DateTime now6 = DateTime.Now;
                    while (true)
                    {
                        Application.DoEvents();
                        this.DataReceive();
                        num7 = (int)this.nRcvPkt[1] & 7;
                        this.pktLength = int.Parse(num7.ToString("X2") + this.nRcvPkt[2].ToString("X2"), NumberStyles.HexNumber);
                        if (this.nCounter <= 2 || this.pktLength + 2 > this.nCounter || this.nRcvPkt[this.pktLength + 1] != (byte)126)
                        {
                            now1 = DateTime.Now;
                            if (now1.Subtract(now6).Seconds <= (int)nTimeOut || (int)num34 >= (int)nTryCount)
                            {
                                if ((int)num34 == (int)nTryCount)
                                    goto label_131;
                            }
                            else
                                goto label_119;
                        }
                        else
                            break;
                    }
                    RecvDataPrint(nRcvPkt, nCounter, IsLineTrafficEnabled);
                    if (IsLineTrafficEnabled)
                    {
                        LineTrafficControlEventHandler($"     {commandString}", "Command");
                        ResponsePrint();
                    }
                    flag4 = true;
                    num34 = (byte)0;
                    this.FrameType();
                    goto label_131;
                label_119:
                    if (DLMSInfo.CmbDirect == 1 && this.nCounter > 0)
                    {
                        if (this.nRcvPkt[0] != (byte)126)
                            this.ClearBuffer();
                        flag4 = false;
                        this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)this.bytAddMode + 21));
                        DateTime now7 = DateTime.Now;
                        if (IsLineTrafficEnabled)
                            SendDataPrint(nPkt, Convert.ToByte((int)this.bytAddMode + 21));
                        //LineTrafficControlEventHandler($"     {commandString}", "Command");
                        while (true)
                        {
                            Application.DoEvents();
                            this.DataReceive();
                            num7 = (int)this.nRcvPkt[1] & 7;
                            this.pktLength = int.Parse(num7.ToString("X2") + this.nRcvPkt[2].ToString("X2"), NumberStyles.HexNumber);
                            if (this.pktLength + 2 > this.nCounter || this.nRcvPkt[this.pktLength + 1] != (byte)126)
                            {
                                now1 = DateTime.Now;
                                if (now1.Subtract(now7).Seconds > (int)nTimeOut)
                                    goto label_126;
                            }
                            else
                                break;
                        }
                        RecvDataPrint(nRcvPkt, nCounter, IsLineTrafficEnabled);
                        LineTrafficControlEventHandler($"     {commandString}", "Command");
                        ResponsePrint();
                        flag4 = true;
                        oSC.DiscardInputOutputBuffer();
                        num34 = (byte)0;
                        this.FrameType();
                        goto label_131;
                    label_126:
                        ++num34;
                    }
                    else
                        ++num34;
                    label_131:
                    if (flag4)
                    {
                        this.temp = string.Empty;
                        for (int index31 = 0; index31 < this.pktLength + 2; ++index31)
                            this.temp += this.nRcvPkt[index31].ToString("X2");
                        this.temp += "\r\n";
                        // this.swLT.Write("(R)\t" + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss:ff tt") + "\t" + this.temp);
                    }
                    if (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] == (byte)151)
                        goto label_137;
                }
                while (!flag4 && (int)num34 != (int)nTryCount);
                goto label_140;
            label_137:
                return false;
            label_140:
                if (!flag4)
                    return false;
                for (int index32 = (int)this.bytAddMode + 11; index32 < this.nCounter - 3; ++index32)
                    stringBuilder.Append(this.nRcvPkt[index32].ToString("X2"));
                while (((int)this.nRcvPkt[1] & 168) == 168)
                {
                    this.nPkt[2] = Convert.ToByte((int)this.bytAddMode + 7);
                    this.nRetLSH = Convert.ToByte((int)this.nRecvCntr << 5);
                    this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nRetLSH | 16);
                    this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] | 1);
                    this.DCl.fcs(ref this.nPkt, (int)Convert.ToByte((int)this.bytAddMode + 5), (byte)1);
                    this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 8)] = (byte)126;
                    this.temp = string.Empty;
                    byte num35 = 0;
                    bool flag5;
                    do
                    {
                        flag5 = false;
                        this.Wait((double)nWait);
                        this.ClearBuffer();
                        this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)this.bytAddMode + 9));
                        DateTime now8 = DateTime.Now;
                        if (IsLineTrafficEnabled)
                            SendDataPrint(nPkt, Convert.ToByte((int)this.bytAddMode + 9));
                        while (true)
                        {
                            Application.DoEvents();
                            this.DataReceive();
                            num7 = (int)this.nRcvPkt[1] & 7;
                            this.pktLength = int.Parse(num7.ToString("X2") + this.nRcvPkt[2].ToString("X2"), NumberStyles.HexNumber);
                            if (this.nCounter <= 2 || this.pktLength + 2 > this.nCounter || this.nRcvPkt[this.pktLength + 1] != (byte)126)
                            {
                                now1 = DateTime.Now;
                                if (now1.Subtract(now8).Seconds <= (int)nTimeOut || (int)num35 >= (int)nTryCount)
                                {
                                    if ((int)num35 == (int)nTryCount)
                                        goto label_167;
                                }
                                else
                                    goto label_152;
                            }
                            else
                                break;
                        }
                        RecvDataPrint(nRcvPkt, nCounter, IsLineTrafficEnabled);
                        LineTrafficControlEventHandler($"     {commandString}", "Command");
                        ResponsePrint();
                        flag5 = true;
                        num35 = (byte)0;
                        for (int index33 = (int)Convert.ToByte((int)this.bytAddMode + 8); index33 < this.pktLength - 1; ++index33)
                            stringBuilder.Append(this.nRcvPkt[index33].ToString("X2"));
                        this.FrameType();
                        goto label_167;
                    label_152:
                        if (DLMSInfo.CmbDirect == 1 && this.nCounter > 0)
                        {
                            if (this.nRcvPkt[0] != (byte)126)
                                this.ClearBuffer();
                            flag5 = false;
                            this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)this.bytAddMode + 9));
                            if (IsLineTrafficEnabled)
                                SendDataPrint(nPkt, Convert.ToByte((int)this.bytAddMode + 9));
                            DateTime now9 = DateTime.Now;
                            while (true)
                            {
                                Application.DoEvents();
                                this.DataReceive();
                                num7 = (int)this.nRcvPkt[1] & 7;
                                this.pktLength = int.Parse(num7.ToString("X2") + this.nRcvPkt[2].ToString("X2"), NumberStyles.HexNumber);
                                if (this.pktLength + 2 > this.nCounter || this.nRcvPkt[this.pktLength + 1] != (byte)126)
                                {
                                    now1 = DateTime.Now;
                                    if (now1.Subtract(now9).Seconds > (int)nTimeOut)
                                        goto label_162;
                                }
                                else
                                    break;
                            }
                            flag5 = true;
                            oSC.DiscardInputOutputBuffer();
                            num35 = (byte)0;
                            for (int index34 = (int)Convert.ToByte((int)this.bytAddMode + 8); index34 < this.pktLength - 1; ++index34)
                                stringBuilder.Append(this.nRcvPkt[index34].ToString("X2"));
                            this.FrameType();
                            goto label_167;
                        label_162:
                            ++num35;
                        }
                        else
                            ++num35;
                        label_167:
                        if (flag5)
                        {
                            this.temp = string.Empty;
                            for (int index35 = 0; index35 < this.pktLength + 2; ++index35)
                                this.temp += this.nRcvPkt[index35].ToString("X2");
                            this.temp += "\r\n";
                            // this.swLT.Write("(R)\t" + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss:ff tt") + "\t" + this.temp);
                        }
                        if (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] == (byte)151)
                            goto label_173;
                    }
                    while (!flag5 && (int)num35 != (int)nTryCount);
                    goto label_176;
                label_173:
                    return false;
                label_176:
                    if (!flag5)
                        return false;
                }
                if (stringBuilder.ToString().StartsWith("CC") || stringBuilder.ToString().StartsWith("D4"))
                {
                    if (stringBuilder.ToString().StartsWith("CC82") || stringBuilder.ToString().StartsWith("D482"))
                        stringBuilder.Remove(0, 8);
                    else if (stringBuilder.ToString().StartsWith("CC81") || stringBuilder.ToString().StartsWith("D481"))
                        stringBuilder.Remove(0, 6);
                    else
                        stringBuilder.Remove(0, 4);
                    byte[] numArray = this.Decrypt(stringBuilder.ToString(), this.sEncryptkeyinHEX);
                    stringBuilder.Length = 0;
                    for (int index36 = 0; index36 < numArray.Length; ++index36)
                        stringBuilder.Append(numArray[index36].ToString("X2"));
                    // this.swLT.Write("\t" + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss:ff tt") + "\t" + "Response : -- >> " + stringBuilder.ToString() + "\r\n");
                    responseString = "Response : -- >> " + stringBuilder.ToString() + "\r\n";
                }
                if (stringBuilder.ToString().StartsWith("C401"))
                    stringBuilder.Remove(0, 8);
                if (stringBuilder.ToString().StartsWith("C402"))
                {
                    num1 = Convert.ToInt64(stringBuilder.ToString().Substring(8, 8), 16);
                    flag1 = !Convert.ToBoolean(Convert.ToInt16(stringBuilder.ToString().Substring(6, 2)));
                    if (stringBuilder.ToString().Substring(18, 2) == "82")
                        stringBuilder.Remove(0, 24);
                    else if (stringBuilder.ToString().Substring(18, 2) == "81")
                        stringBuilder.Remove(0, 22);
                    else
                        stringBuilder.Remove(0, 20);
                }
                this.strbldDLMdata.Append(stringBuilder.ToString());
                stringBuilder.Length = 0;
            }
            //LineTrafficControlEventHandler($"     {commandString}", "Command");
            //LineTrafficControlEventHandler($"     {responseString}\r\n", "Response");
            return true;
        }

        #endregion
        #endregion

        #region Multiple Read
        public bool GetMultipleParameter(
            List<string> DLMSObjectsList,
            string sWhichData,
            byte nWait,
            byte nTryCount,
            byte nTimeOut,
            byte nType,
            DateTime dateStartDate,
            DateTime dateEndDate,
            string sOBISSelect,
            ulong nFrom,
            ulong nTo,
            bool IsLineTrafficEnabled = true,
            string sOBISlist = "0100")
        {
            StringBuilder obisString = new StringBuilder();
            obisString.Append($"{DLMSObjectsList.Count.ToString("X2")}");
            for (int obisCount = 0; obisCount < DLMSObjectsList.Count; obisCount++)
            {
                string[] splitData = DLMSObjectsList[obisCount].Split('-');
                obisString.Append(Convert.ToInt32(splitData[0].Trim()).ToString("X4"));
                obisString.Append(string.Concat(splitData[1].Trim().Split('.').Select(part => int.Parse(part).ToString("X2"))));
                obisString.Append(Convert.ToInt32(splitData[2].Trim()).ToString("X2"));
                obisString.Append("00");
            }
            sWhichData = obisString.ToString().Trim();



            //if (IsLineTrafficEnabled)
            //    LineTrafficControlEventHandler($"     GET CLASS-{Convert.ToInt32(sWhichData.Substring(0, 4), 16)} | OBIS-{DLMSParser.GetObis(sWhichData.Substring(4, 12))} [{DLMSParser.GetObisName(Convert.ToInt32(sWhichData.Substring(0, 4), 16).ToString(), DLMSParser.GetObis(sWhichData.Substring(4, 12)), Convert.ToInt32(sWhichData.Substring(sWhichData.Length - 2), 16).ToString())}] | Attribute-{Convert.ToInt32(sWhichData.Substring(sWhichData.Length - 2), 16)}", "Send");
            bool flag1 = false;
            long num1 = 0;
            byte num2 = Convert.ToByte((int)this.bytAddMode + 8);
            byte num3 = 0;
            string empty = string.Empty;
            StringBuilder stringBuilder = new StringBuilder();
            this.strbldDLMdata.Remove(0, this.strbldDLMdata.Length);
            //DLM file OBIS code and Attribute ID
            //strbldDLMdata.Append($"\r\n{sWhichData.Substring(0, 4)} {sWhichData.Substring(4, 12)} {sWhichData.Substring(sWhichData.Length - 2)} ");
            this.nRetLSH = Convert.ToByte((int)this.nRecvCntr << 5);
            this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nRetLSH | 16);
            this.nRetLSH = Convert.ToByte((int)this.nSentCntr << 1);
            this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nRetLSH | (int)this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)]);
            byte[] nPkt1 = this.nPkt;
            int index1 = (int)num2;
            byte num4 = (byte)(index1 + 1);
            nPkt1[index1] = (byte)230;//E6
            byte[] nPkt2 = this.nPkt;
            int index2 = (int)num4;
            byte num5 = (byte)(index2 + 1);
            nPkt2[index2] = (byte)230;//E6
            byte[] nPkt3 = this.nPkt;
            int index3 = (int)num5;
            byte num6 = (byte)(index3 + 1);
            nPkt3[index3] = (byte)0;//00
            string hex1;
            int num7;
            switch (nType)
            {
                case 0:
                    hex1 = "C003C1" + sWhichData;
                    //hex1 = "C001C1" + sWhichData + "00";
                    break;
                case 1:
                    if (sOBISSelect == "12000809060000010000FF0F02120000")
                    {
                        string[] strArray = new string[20];
                        strArray[0] = "C00181";
                        strArray[1] = sWhichData;
                        strArray[2] = "010102040204";
                        strArray[3] = sOBISSelect;
                        strArray[4] = "090C";
                        strArray[5] = dateStartDate.Year.ToString("X4");
                        strArray[6] = dateStartDate.Month.ToString("X2");
                        num7 = dateStartDate.Day;
                        strArray[7] = num7.ToString("X2");
                        strArray[8] = "FF";
                        num7 = dateStartDate.Hour;
                        strArray[9] = num7.ToString("X2");
                        num7 = dateStartDate.Minute;
                        strArray[10] = num7.ToString("X2");
                        strArray[11] = "0000800000090C";
                        num7 = dateEndDate.Year;
                        strArray[12] = num7.ToString("X4");
                        num7 = dateEndDate.Month;
                        strArray[13] = num7.ToString("X2");
                        num7 = dateEndDate.Day;
                        strArray[14] = num7.ToString("X2");
                        strArray[15] = "FF";
                        num7 = dateEndDate.Hour;
                        strArray[16] = num7.ToString("X2");
                        num7 = dateEndDate.Minute;
                        strArray[17] = num7.ToString("X2");
                        strArray[18] = "00008000000100";
                        strArray[19] = sOBISlist;
                        hex1 = string.Concat(strArray);
                        break;
                    }
                    hex1 = "C00181" + sWhichData + "010102040204" + sOBISSelect + "06" + nFrom.ToString("X8") + "06" + nTo.ToString("X8") + sOBISlist;
                    break;
                case 2:
                    //hex1 = "C00181" + sWhichData + "0102020406" + nFrom.ToString("X8") + "06" + nTo.ToString("X8") + "12" + nFrom.ToString("X4") + "12" + this.Tovalue.ToString("X4");
                    hex1 = "C00181" + sWhichData + "0102020406" + nFrom.ToString("X8") + "06" + nTo.ToString("X8") + "120001" + "120000";
                    break;
                default:
                    hex1 = "C00181" + sWhichData + "0102020406" + this.FromEntry.ToString("X8") + "06" + this.ToEntry.ToString("X8") + "12" + this.Fromvalue.ToString("X4") + "12" + this.Tovalue.ToString("X4");
                    break;
            }
            //this.swLT.Write("\t" + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss:ff tt") + "\t" + "Command : -- >> " + hex1 + "\r\n");
            commandString = "Command  : -- >> " + hex1 + "\r\n";
            if (DLMSInfo.IsLNWithCipher || DLMSInfo.IsLNWithCipherDedicatedKey)
            {
                if (DLMSInfo.AccessMode == 1 || DLMSInfo.AccessMode == 3)
                    num3 = (byte)32;
                else if (DLMSInfo.AccessMode == 2 || DLMSInfo.AccessMode == 4)
                    num3 = (byte)48;
                byte num8;
                if (DLMSInfo.IsLNWithCipherDedicatedKey)
                {
                    byte[] nPkt4 = this.nPkt;
                    int index4 = (int)num6;
                    num8 = (byte)(index4 + 1);
                    nPkt4[index4] = (byte)208;
                }
                else
                {
                    byte[] nPkt5 = this.nPkt;
                    int index5 = (int)num6;
                    num8 = (byte)(index5 + 1);
                    nPkt5[index5] = (byte)200;
                }
                byte[] nPkt6 = this.nPkt;
                int index6 = (int)num8;
                byte num9 = (byte)(index6 + 1);
                nPkt6[index6] = (byte)0;
                byte[] nPkt7 = this.nPkt;
                int index7 = (int)num9;
                byte num10 = (byte)(index7 + 1);
                int num11 = (int)num3;
                nPkt7[index7] = (byte)num11;
                byte[] nPkt8 = this.nPkt;
                int index8 = (int)num10;
                byte num12 = (byte)(index8 + 1);
                nPkt8[index8] = (byte)0;
                byte[] nPkt9 = this.nPkt;
                int index9 = (int)num12;
                byte num13 = (byte)(index9 + 1);
                nPkt9[index9] = (byte)0;
                byte[] nPkt10 = this.nPkt;
                int index10 = (int)num13;
                byte num14 = (byte)(index10 + 1);
                int num15 = (int)Convert.ToByte(this.nCommandCounter / 256);
                nPkt10[index10] = (byte)num15;
                byte[] nPkt11 = this.nPkt;
                int index11 = (int)num14;
                num6 = (byte)(index11 + 1);
                int num16 = (int)Convert.ToByte(this.nCommandCounter % 256);
                nPkt11[index11] = (byte)num16;
                string str1 = num3.ToString("X2");
                num7 = this.nCommandCounter++;
                string str2 = num7.ToString("X8");
                string str3 = hex1;
                byte[] numArray = this.Encrypt(str1 + str2 + str3, this.sEncryptkeyinHEX);
                this.nPkt[(int)num6 - 6] = Convert.ToByte(numArray.Length + 5);
                for (int index12 = 0; index12 < numArray.Length; ++index12)
                    this.nPkt[(int)num6++] = numArray[index12];
            }
            else
            {
                foreach (byte num17 in StringToByteArray(hex1))
                    this.nPkt[(int)num6++] = num17;
                commandString = "Command  : -- >> " + hex1 + "\r\n";
            }
            this.nPkt[2] = Convert.ToByte((int)num6 + 1);
            this.DCl.fcs(ref this.nPkt, (int)Convert.ToByte((int)this.bytAddMode + 5), (byte)1);
            this.DCl.fcs(ref this.nPkt, (int)Convert.ToByte((int)num6 - 1), (byte)1);
            this.nPkt[(int)num6 + 2] = (byte)126;
            byte num18 = 0;
            bool flag2;
            DateTime now1;
            //LineTrafficControlEventHandler($"     GET CLASS-{Convert.ToInt32(sWhichData.Substring(0, 4), 16)} | OBIS-{DLMSParser.GetObis(sWhichData.Substring(4, 12))} [{DLMSParser.GetObisName(Convert.ToInt32(sWhichData.Substring(0, 4), 16).ToString(), DLMSParser.GetObis(sWhichData.Substring(4, 12)))}] | Attribute-{Convert.ToInt32(sWhichData.Substring(sWhichData.Length - 2), 16)}", "Send");
            do
            {
                this.Wait((double)nWait);
                this.ClearBuffer();
                flag2 = false;
                this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)num6 + 3));
                if (IsLineTrafficEnabled)
                    SendDataPrint(nPkt, Convert.ToByte((int)num6 + 3));
                //LineTrafficControlEventHandler($"     {commandString}", "Command");
                DateTime now2 = DateTime.Now;
                while (true)
                {
                    Application.DoEvents();
                    this.DataReceive();
                    num7 = (int)this.nRcvPkt[1] & 7;
                    this.pktLength = int.Parse(num7.ToString("X2") + this.nRcvPkt[2].ToString("X2"), NumberStyles.HexNumber);
                    if (this.nCounter <= 2 || this.pktLength + 2 > this.nCounter || this.nRcvPkt[this.pktLength + 1] != (byte)126)
                    {
                        now1 = DateTime.Now;
                        if (now1.Subtract(now2).Seconds <= (int)nTimeOut || (int)num18 >= (int)nTryCount)
                        {
                            if ((int)num18 == (int)nTryCount)
                                goto label_38;
                        }
                        else
                            goto label_26;
                    }
                    else
                        break;
                }
                RecvDataPrint(nRcvPkt, nCounter, IsLineTrafficEnabled);
                if (IsLineTrafficEnabled)
                {
                    LineTrafficControlEventHandler($"     {commandString}", "Command");
                    ResponsePrint();
                }
                //LineTrafficControlEventHandler($"     {responseString}\r\n", "Response");
                //LineTrafficControlEventHandler("\r\n", "Send");
                flag2 = true;
                num18 = (byte)0;
                this.FrameType();
                goto label_38;
            label_26:
                if (DLMSInfo.CmbDirect == 1 && this.nCounter > 0)
                {
                    if (this.nRcvPkt[0] != (byte)126)
                        this.ClearBuffer();
                    flag2 = false;
                    this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)this.bytAddMode + 27));
                    if (IsLineTrafficEnabled)
                        SendDataPrint(nPkt, Convert.ToByte((int)this.bytAddMode + 27));
                    //LineTrafficControlEventHandler($"     {commandString}", "Command");
                    DateTime now3 = DateTime.Now;
                    while (true)
                    {
                        Application.DoEvents();
                        this.DataReceive();
                        num7 = (int)this.nRcvPkt[1] & 7;
                        this.pktLength = int.Parse(num7.ToString("X2") + this.nRcvPkt[2].ToString("X2"), NumberStyles.HexNumber);
                        if (this.pktLength + 2 > this.nCounter || this.nRcvPkt[this.pktLength + 1] != (byte)126)
                        {
                            now1 = DateTime.Now;
                            if (now1.Subtract(now3).Seconds > (int)nTimeOut)
                                goto label_33;
                        }
                        else
                            break;
                    }
                    RecvDataPrint(nRcvPkt, nCounter, IsLineTrafficEnabled);
                    if (IsLineTrafficEnabled)
                    {
                        LineTrafficControlEventHandler($"     {commandString}", "Command");
                        ResponsePrint();
                    }
                    flag2 = true;
                    oSC.DiscardInputOutputBuffer();
                    num18 = (byte)0;
                    this.FrameType();
                    goto label_38;
                label_33:
                    ++num18;
                }
                else
                    ++num18;
                label_38:
                if (flag2)
                {
                    this.temp = string.Empty;
                    num7 = (int)this.nRcvPkt[1] & 7;
                    this.pktLength = int.Parse(num7.ToString("X2") + this.nRcvPkt[2].ToString("X2"), NumberStyles.HexNumber);
                    for (int index13 = 0; index13 < this.pktLength + 2; ++index13)
                        this.temp += this.nRcvPkt[index13].ToString("X2");
                    this.temp += "\r\n";
                    //this.swLT.Write("(R)\t" + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss:ff tt") + "\t" + this.temp);
                    responseString = temp.Trim();
                    responseString = $"Response : -- >> {responseString.Substring(22, responseString.Length - 28)}\r\n";
                }
            }
            while (!flag2 && (int)num18 != (int)nTryCount);
            if (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] == (byte)151 || ((int)this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] & 1) == 1)
                return false;
            for (int index14 = (int)this.bytAddMode + 11; index14 < this.nCounter - 3; ++index14)
                stringBuilder.Append(this.nRcvPkt[index14].ToString("X2"));
            while (((int)this.nRcvPkt[1] & 168) == 168)
            {
                this.nPkt[2] = Convert.ToByte((int)this.bytAddMode + 7);
                this.nRetLSH = Convert.ToByte((int)this.nRecvCntr << 5);
                this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nRetLSH | 16);
                this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] | 1);
                this.DCl.fcs(ref this.nPkt, (int)Convert.ToByte((int)this.bytAddMode + 5), (byte)1);
                this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 8)] = (byte)126;
                byte num19 = 0;
                bool flag3;
                do
                {
                    this.Wait((double)nWait);
                    this.ClearBuffer();
                    flag3 = false;
                    this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)this.bytAddMode + 9));
                    if (IsLineTrafficEnabled)
                        SendDataPrint(nPkt, Convert.ToByte((int)this.bytAddMode + 9));
                    //LineTrafficControlEventHandler($"     {commandString}", "Command");
                    DateTime now4 = DateTime.Now;
                    while (true)
                    {
                        Application.DoEvents();
                        this.DataReceive();
                        num7 = (int)this.nRcvPkt[1] & 7;
                        this.pktLength = int.Parse(num7.ToString("X2") + this.nRcvPkt[2].ToString("X2"), NumberStyles.HexNumber);
                        if (this.nCounter <= 2 || this.pktLength + 2 > this.nCounter || this.nRcvPkt[this.pktLength + 1] != (byte)126)
                        {
                            now1 = DateTime.Now;
                            if (now1.Subtract(now4).Seconds <= (int)nTimeOut || (int)num19 >= (int)nTryCount)
                            {
                                if ((int)num19 == (int)nTryCount)
                                    goto label_72;
                            }
                            else
                                goto label_57;
                        }
                        else
                            break;
                    }
                    RecvDataPrint(nRcvPkt, nCounter, IsLineTrafficEnabled);
                    LineTrafficControlEventHandler($"     {commandString}", "Command");
                    ResponsePrint();
                    flag3 = true;
                    num19 = (byte)0;
                    for (int index15 = (int)Convert.ToByte((int)this.bytAddMode + 8); index15 < this.pktLength - 1; ++index15)
                        stringBuilder.Append(this.nRcvPkt[index15].ToString("X2"));
                    this.FrameType();
                    goto label_72;
                label_57:
                    if (DLMSInfo.CmbDirect == 1 && this.nCounter > 0)
                    {
                        if (this.nRcvPkt[0] != (byte)126)
                            this.ClearBuffer();
                        flag3 = false;
                        this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)this.bytAddMode + 9));
                        if (IsLineTrafficEnabled)
                            SendDataPrint(nPkt, Convert.ToByte((int)this.bytAddMode + 9));
                        //LineTrafficControlEventHandler($"     {commandString}", "Command");
                        DateTime now5 = DateTime.Now;
                        while (true)
                        {
                            Application.DoEvents();
                            this.DataReceive();
                            num7 = (int)this.nRcvPkt[1] & 7;
                            this.pktLength = int.Parse(num7.ToString("X2") + this.nRcvPkt[2].ToString("X2"), NumberStyles.HexNumber);
                            if (this.pktLength + 2 > this.nCounter || this.nRcvPkt[this.pktLength + 1] != (byte)126)
                            {
                                now1 = DateTime.Now;
                                if (now1.Subtract(now5).Seconds > (int)nTimeOut)
                                    goto label_67;
                            }
                            else
                                break;
                        }
                        RecvDataPrint(nRcvPkt, nCounter, IsLineTrafficEnabled);
                        LineTrafficControlEventHandler($"     {commandString}", "Command");
                        ResponsePrint();
                        flag3 = true;
                        oSC.DiscardInputOutputBuffer();
                        num19 = (byte)0;
                        for (int index16 = (int)Convert.ToByte((int)this.bytAddMode + 8); index16 < this.pktLength - 1; ++index16)
                            stringBuilder.Append(this.nRcvPkt[index16].ToString("X2"));
                        this.FrameType();
                        goto label_72;
                    label_67:
                        ++num19;
                    }
                    else
                        ++num19;
                    label_72:
                    if (flag3)
                    {
                        this.temp = string.Empty;
                        for (int index17 = 0; index17 < this.pktLength + 2; ++index17)
                            this.temp += this.nRcvPkt[index17].ToString("X2");
                        this.temp += "\r\n";
                        //this.swLT.Write("(R)\t" + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss:ff tt") + "\t" + this.temp);
                    }
                    if (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] == (byte)151)
                        goto label_78;
                }
                while (!flag3 && (int)num19 != (int)nTryCount);
                goto label_81;
            label_78:
                return false;
            label_81:
                if (!flag3 || this.nRcvPkt[1] != (byte)160)
                {
                    if (!flag3)
                        return false;
                }
                else
                    break;
            }
            if (stringBuilder.ToString().StartsWith("CC") || stringBuilder.ToString().StartsWith("D4"))
            {
                if (stringBuilder.ToString().StartsWith("CC82") || stringBuilder.ToString().StartsWith("D482"))
                    stringBuilder.Remove(0, 8);
                else if (stringBuilder.ToString().StartsWith("CC81") || stringBuilder.ToString().StartsWith("D481"))
                    stringBuilder.Remove(0, 6);
                else
                    stringBuilder.Remove(0, 4);
                byte[] numArray = this.Decrypt(stringBuilder.ToString(), this.sEncryptkeyinHEX);
                stringBuilder.Length = 0;
                for (int index18 = 0; index18 < numArray.Length; ++index18)
                    stringBuilder.Append(numArray[index18].ToString("X2"));
                //this.swLT.Write("\t" + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss:ff tt") + "\t" + "Response : -- >> " + stringBuilder.ToString() + "\r\n");
                responseString = "Response : -- >> " + stringBuilder.ToString() + "\r\n";
            }
            if (stringBuilder.ToString().StartsWith("C401"))
                stringBuilder.Remove(0, 8);
            if (stringBuilder.ToString().StartsWith("C402"))
            {
                num1 = Convert.ToInt64(stringBuilder.ToString().Substring(8, 8), 16);
                flag1 = !Convert.ToBoolean(Convert.ToInt16(stringBuilder.ToString().Substring(6, 2)));
                if (stringBuilder.ToString().Substring(18, 2) == "82")
                    stringBuilder.Remove(0, 24);
                else if (stringBuilder.ToString().Substring(18, 2) == "81")
                    stringBuilder.Remove(0, 22);
                else
                    stringBuilder.Remove(0, 20);
            }
            this.strbldDLMdata.Append(stringBuilder.ToString());
            stringBuilder.Length = 0;
            while (flag1)
            {
                this.temp = string.Empty;
                flag1 = false;
                this.nRetLSH = Convert.ToByte((int)this.nRecvCntr << 5);
                this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nRetLSH | 16);
                this.nRetLSH = Convert.ToByte((int)this.nSentCntr << 1);
                this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nRetLSH | (int)this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)]);
                byte num20 = Convert.ToByte((int)this.bytAddMode + 8);
                byte[] nPkt12 = this.nPkt;
                int index19 = (int)num20;
                byte num21 = (byte)(index19 + 1);
                nPkt12[index19] = (byte)230;
                byte[] nPkt13 = this.nPkt;
                int index20 = (int)num21;
                byte num22 = (byte)(index20 + 1);
                nPkt13[index20] = (byte)230;
                byte[] nPkt14 = this.nPkt;
                int index21 = (int)num22;
                byte num23 = (byte)(index21 + 1);
                nPkt14[index21] = (byte)0;
                string hex2 = "C00281" + num1.ToString("X8");
                commandString = "Command : -- >> " + hex2 + "\r\n";
                if (DLMSInfo.IsLNWithCipher)
                {
                    //this.swLT.Write("\t" + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss:ff tt") + "\t" + "Command : -- >> " + hex2 + "\r\n");
                    commandString = "Command : -- >> " + hex2 + "\r\n";
                    byte num24;
                    if (DLMSInfo.IsLNWithCipherDedicatedKey)
                    {
                        byte[] nPkt15 = this.nPkt;
                        int index22 = (int)num23;
                        num24 = (byte)(index22 + 1);
                        nPkt15[index22] = (byte)208;
                    }
                    else
                    {
                        byte[] nPkt16 = this.nPkt;
                        int index23 = (int)num23;
                        num24 = (byte)(index23 + 1);
                        nPkt16[index23] = (byte)200;
                    }
                    byte[] nPkt17 = this.nPkt;
                    int index24 = (int)num24;
                    byte num25 = (byte)(index24 + 1);
                    nPkt17[index24] = (byte)0;
                    byte[] nPkt18 = this.nPkt;
                    int index25 = (int)num25;
                    byte num26 = (byte)(index25 + 1);
                    int num27 = (int)num3;
                    nPkt18[index25] = (byte)num27;
                    byte[] nPkt19 = this.nPkt;
                    int index26 = (int)num26;
                    byte num28 = (byte)(index26 + 1);
                    nPkt19[index26] = (byte)0;
                    byte[] nPkt20 = this.nPkt;
                    int index27 = (int)num28;
                    byte num29 = (byte)(index27 + 1);
                    nPkt20[index27] = (byte)0;
                    byte[] nPkt21 = this.nPkt;
                    int index28 = (int)num29;
                    byte num30 = (byte)(index28 + 1);
                    int num31 = (int)Convert.ToByte(this.nCommandCounter / 256);
                    nPkt21[index28] = (byte)num31;
                    byte[] nPkt22 = this.nPkt;
                    int index29 = (int)num30;
                    num23 = (byte)(index29 + 1);
                    int num32 = (int)Convert.ToByte(this.nCommandCounter % 256);
                    nPkt22[index29] = (byte)num32;
                    string str4 = num3.ToString("X2");
                    num7 = this.nCommandCounter++;
                    string str5 = num7.ToString("X8");
                    string str6 = hex2;
                    byte[] numArray = this.Encrypt(str4 + str5 + str6, this.sEncryptkeyinHEX);
                    this.nPkt[(int)num23 - 6] = Convert.ToByte(numArray.Length + 5);
                    for (int index30 = 0; index30 < numArray.Length; ++index30)
                        this.nPkt[(int)num23++] = numArray[index30];
                }
                else
                {
                    foreach (byte num33 in StringToByteArray(hex2))
                        this.nPkt[(int)num23++] = num33;
                    //commandString = "Command : -- >> " + hex2 + "\r\n";
                }
                this.nPkt[2] = Convert.ToByte((int)num23 + 1);
                this.DCl.fcs(ref this.nPkt, (int)Convert.ToByte((int)this.bytAddMode + 5), (byte)1);
                this.DCl.fcs(ref this.nPkt, (int)Convert.ToByte((int)num23 - 1), (byte)1);
                this.nPkt[(int)num23 + 2] = (byte)126;
                byte num34 = 0;
                bool flag4;
                do
                {
                    this.ClearBuffer();
                    flag4 = false;
                    this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)num23 + 3));
                    if (IsLineTrafficEnabled)
                        SendDataPrint(nPkt, Convert.ToByte((int)num23 + 3));
                    //LineTrafficControlEventHandler($"     {commandString}", "Command");
                    this.Wait((double)nWait);
                    //SendDataPrint(nPkt, Convert.ToByte((int)num23 + 3));
                    DateTime now6 = DateTime.Now;
                    while (true)
                    {
                        Application.DoEvents();
                        this.DataReceive();
                        num7 = (int)this.nRcvPkt[1] & 7;
                        this.pktLength = int.Parse(num7.ToString("X2") + this.nRcvPkt[2].ToString("X2"), NumberStyles.HexNumber);
                        if (this.nCounter <= 2 || this.pktLength + 2 > this.nCounter || this.nRcvPkt[this.pktLength + 1] != (byte)126)
                        {
                            now1 = DateTime.Now;
                            if (now1.Subtract(now6).Seconds <= (int)nTimeOut || (int)num34 >= (int)nTryCount)
                            {
                                if ((int)num34 == (int)nTryCount)
                                    goto label_131;
                            }
                            else
                                goto label_119;
                        }
                        else
                            break;
                    }
                    RecvDataPrint(nRcvPkt, nCounter, IsLineTrafficEnabled);
                    if (IsLineTrafficEnabled)
                    {
                        LineTrafficControlEventHandler($"     {commandString}", "Command");
                        ResponsePrint();
                    }
                    flag4 = true;
                    num34 = (byte)0;
                    this.FrameType();
                    goto label_131;
                label_119:
                    if (DLMSInfo.CmbDirect == 1 && this.nCounter > 0)
                    {
                        if (this.nRcvPkt[0] != (byte)126)
                            this.ClearBuffer();
                        flag4 = false;
                        this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)this.bytAddMode + 21));
                        DateTime now7 = DateTime.Now;
                        if (IsLineTrafficEnabled)
                            SendDataPrint(nPkt, Convert.ToByte((int)this.bytAddMode + 21));
                        //LineTrafficControlEventHandler($"     {commandString}", "Command");
                        while (true)
                        {
                            Application.DoEvents();
                            this.DataReceive();
                            num7 = (int)this.nRcvPkt[1] & 7;
                            this.pktLength = int.Parse(num7.ToString("X2") + this.nRcvPkt[2].ToString("X2"), NumberStyles.HexNumber);
                            if (this.pktLength + 2 > this.nCounter || this.nRcvPkt[this.pktLength + 1] != (byte)126)
                            {
                                now1 = DateTime.Now;
                                if (now1.Subtract(now7).Seconds > (int)nTimeOut)
                                    goto label_126;
                            }
                            else
                                break;
                        }
                        RecvDataPrint(nRcvPkt, nCounter, IsLineTrafficEnabled);
                        LineTrafficControlEventHandler($"     {commandString}", "Command");
                        ResponsePrint();
                        flag4 = true;
                        oSC.DiscardInputOutputBuffer();
                        num34 = (byte)0;
                        this.FrameType();
                        goto label_131;
                    label_126:
                        ++num34;
                    }
                    else
                        ++num34;
                    label_131:
                    if (flag4)
                    {
                        this.temp = string.Empty;
                        for (int index31 = 0; index31 < this.pktLength + 2; ++index31)
                            this.temp += this.nRcvPkt[index31].ToString("X2");
                        this.temp += "\r\n";
                        // this.swLT.Write("(R)\t" + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss:ff tt") + "\t" + this.temp);
                    }
                    if (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] == (byte)151)
                        goto label_137;
                }
                while (!flag4 && (int)num34 != (int)nTryCount);
                goto label_140;
            label_137:
                return false;
            label_140:
                if (!flag4)
                    return false;
                for (int index32 = (int)this.bytAddMode + 11; index32 < this.nCounter - 3; ++index32)
                    stringBuilder.Append(this.nRcvPkt[index32].ToString("X2"));
                while (((int)this.nRcvPkt[1] & 168) == 168)
                {
                    this.nPkt[2] = Convert.ToByte((int)this.bytAddMode + 7);
                    this.nRetLSH = Convert.ToByte((int)this.nRecvCntr << 5);
                    this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nRetLSH | 16);
                    this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] = Convert.ToByte((int)this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] | 1);
                    this.DCl.fcs(ref this.nPkt, (int)Convert.ToByte((int)this.bytAddMode + 5), (byte)1);
                    this.nPkt[(int)Convert.ToByte((int)this.bytAddMode + 8)] = (byte)126;
                    this.temp = string.Empty;
                    byte num35 = 0;
                    bool flag5;
                    do
                    {
                        flag5 = false;
                        this.Wait((double)nWait);
                        this.ClearBuffer();
                        this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)this.bytAddMode + 9));
                        DateTime now8 = DateTime.Now;
                        if (IsLineTrafficEnabled)
                            SendDataPrint(nPkt, Convert.ToByte((int)this.bytAddMode + 9));
                        while (true)
                        {
                            Application.DoEvents();
                            this.DataReceive();
                            num7 = (int)this.nRcvPkt[1] & 7;
                            this.pktLength = int.Parse(num7.ToString("X2") + this.nRcvPkt[2].ToString("X2"), NumberStyles.HexNumber);
                            if (this.nCounter <= 2 || this.pktLength + 2 > this.nCounter || this.nRcvPkt[this.pktLength + 1] != (byte)126)
                            {
                                now1 = DateTime.Now;
                                if (now1.Subtract(now8).Seconds <= (int)nTimeOut || (int)num35 >= (int)nTryCount)
                                {
                                    if ((int)num35 == (int)nTryCount)
                                        goto label_167;
                                }
                                else
                                    goto label_152;
                            }
                            else
                                break;
                        }
                        RecvDataPrint(nRcvPkt, nCounter, IsLineTrafficEnabled);
                        LineTrafficControlEventHandler($"     {commandString}", "Command");
                        ResponsePrint();
                        flag5 = true;
                        num35 = (byte)0;
                        for (int index33 = (int)Convert.ToByte((int)this.bytAddMode + 8); index33 < this.pktLength - 1; ++index33)
                            stringBuilder.Append(this.nRcvPkt[index33].ToString("X2"));
                        this.FrameType();
                        goto label_167;
                    label_152:
                        if (DLMSInfo.CmbDirect == 1 && this.nCounter > 0)
                        {
                            if (this.nRcvPkt[0] != (byte)126)
                                this.ClearBuffer();
                            flag5 = false;
                            this.SendPkt(this.nPkt, (ushort)Convert.ToByte((int)this.bytAddMode + 9));
                            if (IsLineTrafficEnabled)
                                SendDataPrint(nPkt, Convert.ToByte((int)this.bytAddMode + 9));
                            DateTime now9 = DateTime.Now;
                            while (true)
                            {
                                Application.DoEvents();
                                this.DataReceive();
                                num7 = (int)this.nRcvPkt[1] & 7;
                                this.pktLength = int.Parse(num7.ToString("X2") + this.nRcvPkt[2].ToString("X2"), NumberStyles.HexNumber);
                                if (this.pktLength + 2 > this.nCounter || this.nRcvPkt[this.pktLength + 1] != (byte)126)
                                {
                                    now1 = DateTime.Now;
                                    if (now1.Subtract(now9).Seconds > (int)nTimeOut)
                                        goto label_162;
                                }
                                else
                                    break;
                            }
                            flag5 = true;
                            oSC.DiscardInputOutputBuffer();
                            num35 = (byte)0;
                            for (int index34 = (int)Convert.ToByte((int)this.bytAddMode + 8); index34 < this.pktLength - 1; ++index34)
                                stringBuilder.Append(this.nRcvPkt[index34].ToString("X2"));
                            this.FrameType();
                            goto label_167;
                        label_162:
                            ++num35;
                        }
                        else
                            ++num35;
                        label_167:
                        if (flag5)
                        {
                            this.temp = string.Empty;
                            for (int index35 = 0; index35 < this.pktLength + 2; ++index35)
                                this.temp += this.nRcvPkt[index35].ToString("X2");
                            this.temp += "\r\n";
                            // this.swLT.Write("(R)\t" + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss:ff tt") + "\t" + this.temp);
                        }
                        if (this.nRcvPkt[(int)Convert.ToByte((int)this.bytAddMode + 5)] == (byte)151)
                            goto label_173;
                    }
                    while (!flag5 && (int)num35 != (int)nTryCount);
                    goto label_176;
                label_173:
                    return false;
                label_176:
                    if (!flag5)
                        return false;
                }
                if (stringBuilder.ToString().StartsWith("CC") || stringBuilder.ToString().StartsWith("D4"))
                {
                    if (stringBuilder.ToString().StartsWith("CC82") || stringBuilder.ToString().StartsWith("D482"))
                        stringBuilder.Remove(0, 8);
                    else if (stringBuilder.ToString().StartsWith("CC81") || stringBuilder.ToString().StartsWith("D481"))
                        stringBuilder.Remove(0, 6);
                    else
                        stringBuilder.Remove(0, 4);
                    byte[] numArray = this.Decrypt(stringBuilder.ToString(), this.sEncryptkeyinHEX);
                    stringBuilder.Length = 0;
                    for (int index36 = 0; index36 < numArray.Length; ++index36)
                        stringBuilder.Append(numArray[index36].ToString("X2"));
                    // this.swLT.Write("\t" + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss:ff tt") + "\t" + "Response : -- >> " + stringBuilder.ToString() + "\r\n");
                    responseString = "Response : -- >> " + stringBuilder.ToString() + "\r\n";
                }
                if (stringBuilder.ToString().StartsWith("C401"))
                    stringBuilder.Remove(0, 8);
                if (stringBuilder.ToString().StartsWith("C402"))
                {
                    num1 = Convert.ToInt64(stringBuilder.ToString().Substring(8, 8), 16);
                    flag1 = !Convert.ToBoolean(Convert.ToInt16(stringBuilder.ToString().Substring(6, 2)));
                    if (stringBuilder.ToString().Substring(18, 2) == "82")
                        stringBuilder.Remove(0, 24);
                    else if (stringBuilder.ToString().Substring(18, 2) == "81")
                        stringBuilder.Remove(0, 22);
                    else
                        stringBuilder.Remove(0, 20);
                }
                this.strbldDLMdata.Append(stringBuilder.ToString());
                stringBuilder.Length = 0;
            }
            //LineTrafficControlEventHandler($"     {commandString}", "Command");
            //LineTrafficControlEventHandler($"     {responseString}\r\n", "Response");
            return true;
        }

        #endregion
        public void Dispose()
        {
            if (oSC != null)
            {
                oSC.ClosePort();
                oSC.Dispose();
            }
            oSC = null;
            //this.swError.Close();
            // this.swLT.Close();
            LineTrafficSaveEventHandler();
        }
    }
}
