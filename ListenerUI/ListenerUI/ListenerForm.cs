using AutoTest.FrameWork.Converts;
using Gurux.DLMS.AMI.Messages.DB;
using ListenerUI.HelperClasses;
using log4net;
using log4net.Util;
using MeterComm;
using MeterComm.DLMS;
using MeterReader.CommonClasses;
using MeterReader.DLMSNetSerialCommunication;
using MeterReader.TestHelperClasses;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows.Forms;

namespace ListenerUI
{
    public partial class ListenerForm : Form
    {
        #region Delegates and Handlers
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        //Delegate for Line traffic save
        public delegate void LineTrafficClear();
        public static event LineTrafficClear LineTrafficClearEventHandler = delegate { };// add empty delegate!
        //Delegate for Line traffic save
        public delegate void LineTrafficSave();
        public static event LineTrafficSave LineTrafficSaveEventHandler = delegate { };// add empty delegate!
        //Delegate for Line traffic messages
        public delegate void LineTrafficControl(string updatedText, string status);
        public static event LineTrafficControl LineTrafficControlEventHandler = delegate { }; // add empty delegate!;
                                                                                              //Delegate for Execution traffic messages 
        public delegate void AppendColoredTextControl(string message, Color color, bool isBold = false);
        public static event AppendColoredTextControl AppendColoredTextControlEventHandler = delegate { }; // add empty delegate!;
        private readonly ConcurrentQueue<(string Message, Color Color, bool IsBold)> _logBuffer2 = new ConcurrentQueue<(string, Color, bool)>();
        private readonly Font BoldFont11 = new Font("Courier New", 11f, FontStyle.Bold);
        private readonly Font RegularFont11 = new Font("Courier New", 11f, FontStyle.Regular);
        private const int MaxLines = 1000;
        private const int TrimLines = 200;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        private const int EM_GETFIRSTVISIBLELINE = 0xCE;
        private readonly System.Timers.Timer _flushTimer2;
        #endregion

        private TestLogService logService;
        private TestConfiguration config;
        private TestStopWatch stopWatch;
        private TCPTestNotifier tcpTestNotifier;
        PushPacketManager pushPacketManager = new PushPacketManager();
        DLMSParser parse = new DLMSParser();
        private System.Timers.Timer pushMonitorTimer;
        RichTextBox logBox = new RichTextBox();
        private DataTable receivedPushData = new DataTable();
        public DataTable finalDataTable = new DataTable();
        private int lastPushRowCount = 0;

        public ListenerForm()
        {
            InitializeComponent();
            InitializeLoggerAndConfigurations();
            PushPacketManager.DeviceID = "GOE12043714";
            finalDataTable.RowChanged += FinalDataTable_RowChanged;
            // Bind log event
            TestLogService.AppendColoredTextControlEventHandler += TestLogService_AppendColoredTextControlEventHandler;
            PushPacketManager._logService = logService;
            PushPacketManager.logBox = rtbPushLogs;
            // Start background flush timer
            _flushTimer2 = new System.Timers.Timer(500); // flush every 500ms
            _flushTimer2.Elapsed += FlushLogBuffer2;
            _flushTimer2.Start();

            ComboBox[] comboBoxes = {
                cbTestProfileType, cbInstant_Frequency,cb_SR_Frequency, cb_DE_Frequency, cb_LS_Frequency, cb_CB_Frequency };
            foreach (var cb in comboBoxes)
                cb.SelectedIndex = 0;
        }

        private Dictionary<string, (TextBox txtDestIP, ComboBox cbFrequency)> profileControls;
        private void InitializeProfileControls()
        {
            profileControls = new Dictionary<string, (TextBox, ComboBox)>
            {
                { "Instant", (txt_Instant_DestIP, cbInstant_Frequency) },
                { "Alert", (txt_Alert_DestIP, cb_Alert_Frequency) },
                { "Billing", (txt_Bill_DestIP, cb_Bill_Frequency) },
                { "Self Registration", (txt_SR_DestIP, cb_SR_Frequency) },
                { "Daily Energy", (txt_DE_DestIP, cb_DE_Frequency) },
                { "Load Survey", (txt_LS_DestIP, cb_LS_Frequency) },
                { "Current Bill", (txt_CB_DestIP, cb_CB_Frequency) },
                { "Tamper", (txt_Alert_DestIP, cb_Alert_Frequency) } // Tamper shares Alert controls
            };
        }
        private void InitializeLoggerAndConfigurations()
        {
            logService = new TestLogService(rtbPushLogs);
            config = TestConfiguration.CreateDefault();
        }
        private void btnStartListener_Click(object sender, EventArgs e)
        {
            logService = new TestLogService(rtbPushLogs);
            logBox = rtbPushLogs;
            DLMSComm dlmsReader = new DLMSComm(DLMSInfo.comPort, DLMSInfo.BaudRate);
            if (!dlmsReader.SignOnDLMS())
            {
                log.Error("Sign ON Failure");
                logService.LogMessage(rtbPushLogs, "Sign ON Failure", Color.Red, true);
            }
            logService.LogMessage(rtbPushLogs, "Meter Sign On Successful", Color.Green);
        }
        private void btnStopListener_Click(object sender, EventArgs e)
        {

        }
        private void btnGet_PS_AS_Click(object sender, EventArgs e)
        {

        }
        private void btnSet_PS_AS_Click(object sender, EventArgs e)
        {

        }
        private void btnClearLogs_Click(object sender, EventArgs e)
        {
            rtbPushLogs.Clear();
        }
        private void btnSaveData_Click(object sender, EventArgs e)
        {
        }
        private void btnPushprofileSettings_Click(object sender, EventArgs e)
        {
            pnlProfileSettings.Visible = !pnlProfileSettings.Visible;

            if (pnlProfileSettings.Visible)
            {
                btnPushprofileSettings.Text = "▲ Hide Push Profile Settings";
                btnPushprofileSettings.ForeColor = Color.Blue;
            }
            else
            {
                btnPushprofileSettings.Text = "▼ Show Push Profile Settings";
                btnPushprofileSettings.ForeColor = Color.Black;
            }
        }

        #region Helper Methods
        private void cbTestProfileType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedProfile = cbTestProfileType.SelectedItem.ToString();
            DisableAllProfiles();

            switch (selectedProfile)
            {
                case "All":
                    EnableAllProfiles();
                    break;

                case "Instant":
                    EnableProfile(txt_Instant_DestIP, cbInstant_Frequency, txt_Random_Instant);
                    break;

                case "Load Survey":
                    EnableProfile(txt_LS_DestIP, cb_LS_Frequency, txt_Random_LS);
                    break;

                case "Daily Energy":
                    EnableProfile(txt_DE_DestIP, cb_DE_Frequency, txt_Random_DE);
                    break;

                case "Self Registration":
                    EnableProfile(txt_SR_DestIP, cb_SR_Frequency, txt_Random_SR);
                    break;

                case "Billing":
                    EnableProfile(txt_Bill_DestIP, cb_Bill_Frequency, txt_Random_Bill);
                    break;

                case "Current Bill":
                    EnableProfile(txt_CB_DestIP, cb_CB_Frequency, txt_Random_CB);
                    break;

                case "Alert":
                    EnableProfile(txt_Alert_DestIP, cb_Alert_Frequency, txt_Random_Alert);
                    break;

                default:
                    break;
            }
        }
        private void DisableAllProfiles()
        {
            TextBox[] textBoxes = {
        txt_Instant_DestIP, txt_Alert_DestIP, txt_Bill_DestIP,
        txt_SR_DestIP, txt_DE_DestIP, txt_LS_DestIP, txt_CB_DestIP
    };

            ComboBox[] comboBoxes = {
        cbInstant_Frequency, cb_Alert_Frequency, cb_Bill_Frequency,
        cb_SR_Frequency, cb_DE_Frequency, cb_LS_Frequency, cb_CB_Frequency
    };

            TextBox[] textBoxes_Randmsn = {
            txt_Random_Instant, txt_Random_Alert, txt_Random_Bill,
            txt_Random_CB, txt_Random_DE, txt_Random_LS, txt_Random_SR
        };

            foreach (var tb in textBoxes)
                tb.Enabled = false;

            foreach (var cb in comboBoxes)
                cb.Enabled = false;

            foreach (var tb in textBoxes_Randmsn)
                tb.Enabled = false;
        }
        private void EnableAllProfiles()
        {
            TextBox[] textBoxes = {
            txt_Instant_DestIP, txt_Alert_DestIP, txt_Bill_DestIP,
            txt_SR_DestIP, txt_DE_DestIP, txt_LS_DestIP, txt_CB_DestIP
        };

            ComboBox[] comboBoxes = {
            cbInstant_Frequency, cb_Alert_Frequency, cb_Bill_Frequency,
            cb_SR_Frequency, cb_DE_Frequency, cb_LS_Frequency, cb_CB_Frequency
        };
            TextBox[] textBoxes_Randmsn = {
            txt_Random_Instant, txt_Random_Alert, txt_Random_Bill,
            txt_Random_CB, txt_Random_DE, txt_Random_LS, txt_Random_SR
        };

            foreach (var tb in textBoxes)
                tb.Enabled = true;

            foreach (var cb in comboBoxes)
                cb.Enabled = true;
            foreach (var tb in textBoxes_Randmsn)
                tb.Enabled = true;
        }
        private void EnableProfile(TextBox txtBox, ComboBox comboBox, TextBox txtBox_Randmsn)
        {
            txtBox.Enabled = true;
            comboBox.Enabled = true;
            txtBox_Randmsn.Enabled = true;
        }
        #endregion

        #region Logging Methods
        public void PushMonitorTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                var table = TCPTestNotifier.ListenerDataTable.Copy();
                if (table != null && table.Rows.Count > lastPushRowCount)
                {
                    var newRows = table.AsEnumerable().Skip(lastPushRowCount).CopyToDataTable();
                    lastPushRowCount = table.Rows.Count;

                    lock (receivedPushData)
                    {
                        if (receivedPushData.Columns.Count == 0)
                        {
                            foreach (DataColumn col in newRows.Columns)
                            {
                                receivedPushData.Columns.Add(col.ColumnName, col.DataType);
                                finalDataTable.Columns.Add(col.ColumnName, col.DataType);
                            }
                        }
                        foreach (DataRow row in newRows.Rows)
                        {
                            receivedPushData.ImportRow(row);
                            finalDataTable.ImportRow(row);
                            dgRawData.DataSource = receivedPushData;
                            //New
                            if (dgRawData.InvokeRequired)
                            {
                                dgRawData.Invoke(new Action(() =>
                                {
                                    dgRawData.DataSource = dgRawData;
                                    dgRawData.Refresh();
                                }));
                            }
                            else
                            {
                                dgRawData.DataSource = dgRawData;
                                dgRawData.Refresh();
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                logService.LogMessage(rtbPushLogs, $"Error in PushMonitorTimer: {ex.Message}", Color.Red, true);
            }
        }
        private void FinalDataTable_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            if (e.Action == DataRowAction.Add)
            {
                try
                {
                    string decryptedData = e.Row[4]?.ToString();
                    string dataPacket = e.Row[0]?.ToString();
                    DataTable targetTable = new DataTable();
                    string profile = string.Empty;
                    string DeviceID = dataPacket.Split('-')[2];
                    if (!string.IsNullOrEmpty(decryptedData))
                    {
                        DataTable onlyColumn = pushPacketManager.GeneratePushDataTableFromHex(decryptedData, dataPacket);
                        targetTable = pushPacketManager.BuildTargetTableFromPushData(dataPacket, onlyColumn);
                        profile = PushPacketManager.pushProfile;
                        string saveName = profile.Replace("/", "").Replace(":", "").Replace("-", "_").Trim();
                    }
                }
                catch (Exception ex)
                {
                    logService.LogMessage(logBox, $"Error in {PushPacketManager.pushProfile} => {ex.Message}\n{ex.StackTrace}", Color.Red);
                }
            }
        }
        public void OnPushDataReceived(string updatedText, Color color)
        {
            if (rtbPushLogs.InvokeRequired)
            {
                rtbPushLogs.BeginInvoke(new Action(() =>
                {
                    logService.LogMessage(rtbPushLogs, updatedText, color, true);
                    rtbPushLogs.ScrollToCaret();
                }));
            }
            else
            {
                logService.LogMessage(rtbPushLogs, updatedText, color, true);
                rtbPushLogs.ScrollToCaret();
            }
        }

        private bool isRawDataVisible = false;
        private void btnRawData_Click(object sender, EventArgs e)
        {
            isRawDataVisible = !isRawDataVisible;
            if (isRawDataVisible)
            {
                // Show DataGridView in fill mode
                dgRawData.Visible = true;
                tabConProfileTabs.Visible = false;
                dgRawData.Dock = DockStyle.Fill;

                btnRawData.Text = "Profile Tab View";
            }
            else
            {
                // Show TabControl in fill mode
                tabConProfileTabs.Visible = true;
                dgRawData.Visible = false;
                tabConProfileTabs.Dock = DockStyle.Fill;

                btnRawData.Text = "Raw Data View";
            }
        }
        private void FlushLogBuffer2(object sender, ElapsedEventArgs e)
        {
            if (IsDisposed) return;

            if (_logBuffer2.IsEmpty) return;

            BeginInvoke(new Action(() =>
            {
                // Check if user is at bottom before appending
                bool atBottom = false;
                int visibleLines = rtbPushLogs.ClientSize.Height / rtbPushLogs.Font.Height;
                int firstVisibleLine = SendMessage(rtbPushLogs.Handle, EM_GETFIRSTVISIBLELINE, 0, 0);
                int lastVisibleLine = firstVisibleLine + visibleLines;
                atBottom = (lastVisibleLine >= rtbPushLogs.Lines.Length - 1);

                // Stop repainting until all logs are appended
                rtbPushLogs.SuspendLayout();

                while (_logBuffer2.TryDequeue(out var log))
                {
                    rtbPushLogs.SelectionStart = rtbPushLogs.TextLength;
                    rtbPushLogs.SelectionLength = 0;

                    // Set font (cached)
                    rtbPushLogs.SelectionFont = log.IsBold ? BoldFont11 : RegularFont11;

                    // Set color
                    rtbPushLogs.SelectionColor = log.Color;

                    //// Append text
                    //rtbPushLogs.AppendText(log.Message);
                    // Append text with style preserved
                    rtbPushLogs.SelectedText = log.Message;
                }

                //// Reset to default
                //rtbPushLogs.SelectionColor = rtbPushLogs.ForeColor;


                // Trim old lines if needed
                if (rtbPushLogs.Lines.Length > MaxLines)
                {
                    int cutOffIndex = rtbPushLogs.GetFirstCharIndexFromLine(TrimLines);
                    rtbPushLogs.Select(0, cutOffIndex);
                    rtbPushLogs.SelectedText = string.Empty;
                }

                // If user was already at bottom, auto-scroll
                if (atBottom)
                {
                    rtbPushLogs.SelectionStart = rtbPushLogs.TextLength;
                    rtbPushLogs.ScrollToCaret();
                }

                // Resume painting
                rtbPushLogs.ResumeLayout();
            }));
        }
        private void TestLogService_AppendColoredTextControlEventHandler(string message, Color color, bool isBold = false)
        {
            _logBuffer2.Enqueue((message + Environment.NewLine, color, isBold));
        }
        #endregion
    }
}
