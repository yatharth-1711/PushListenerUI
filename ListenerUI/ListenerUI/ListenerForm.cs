using AutoTest.FrameWork.Converts;
using Gurux.DLMS.AMI.Messages.DB;
using ListenerUI.HelperClasses;
using log4net;
using log4net.Util;
using MeterComm;
using MeterComm.DLMS;
using MeterReader.CommonClasses;
using MeterReader.DLMSInterfaceClasses;
using MeterReader.DLMSNetSerialCommunication;
using MeterReader.TestHelperClasses;
using Newtonsoft.Json.Linq;
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
using System.Threading;
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
        private readonly Font BoldFont11 = new Font("Courier New", 9f, FontStyle.Bold);
        private readonly Font RegularFont11 = new Font("Courier New", 9f, FontStyle.Regular);
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
        public static CancellationTokenSource _cancellationToken;

        public ListenerForm()
        {
            InitializeComponent();
            StyleDataGrid(dgRawData); StyleDataGrid(dgAlert); StyleDataGrid(dgInstant); StyleDataGrid(dgLS);
            StyleDataGrid(dgTamper); StyleDataGrid(dgBill); StyleDataGrid(dgDE); StyleDataGrid(dgSR); StyleDataGrid(dgCB);
            InitializeLoggerAndConfigurations();
            PushPacketManager.DeviceID = "GOE12043714";
            finalDataTable.RowChanged += FinalDataTable_RowChanged;
            // Bind log event
            TestLogService.AppendColoredTextControlEventHandler += TestLogService_AppendColoredTextControlEventHandler;
            rtbPushLogs.MouseDown += rtbPushLogs_MouseDown;
            rtbPushLogs.MouseMove += rtbPushLogs_MouseMove;

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
        private async void btnStartListener_Click(object sender, EventArgs e)
        {

            logService = new TestLogService(rtbPushLogs);
            logBox = rtbPushLogs;
            try
            {
                await System.Threading.Tasks.Task.Run(() =>
                {
                    this.Invoke(new Action(() =>
                    {
                        logService.LogMessage(rtbPushLogs, "Getting Meter Details and Push profiles...", Color.Black, true);
                        //IniTestRun(config);
                        //ProfileGenericInfo.FillTables();
                    }));
                });
            }
            catch
            {
                return;
            }
            DLMSComm dlmsReader = new DLMSComm(DLMSInfo.comPort, DLMSInfo.BaudRate);
            try
            {
                if (!dlmsReader.SignOnDLMS())
                {
                    log.Error("Sign ON Failure");
                    logService.LogMessage(rtbPushLogs, "Sign ON Failure", Color.Red, true);
                    return;
                }
                string _recData = string.Empty; int obisCount = 0;
                DataTable USdataTable = new DataTable();
                if (DLMSAssociationLN.IsUSAssociationAvailable)
                    USdataTable = DLMSAssociationLN.US_AssociationDataTable.Copy();
                else
                {
                    if (dlmsReader.GetParameter("000F0000280003FF02", (byte)(config.InterFrameTimeout / 1000), (byte)5, (byte)(config.ResponseTimeout / 1000), (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL))
                        _recData = dlmsReader.strbldDLMdata.ToString().Trim().Split(' ')[3];
                    if (_recData.Length > 20)
                        USdataTable = DLMSAssociationLN.GetObjectListTable(_recData, DLMSAssociationLN.AssociationType.Utility_Settings, out obisCount);
                }
                PushPacketManager.isCurrentBillAvailable = USdataTable.AsEnumerable().Any(row => row[4].ToString().Contains("0.0.25.9.129.255"));
            }
            catch (Exception ex)
            {
                log.Error(ex.Message.ToString());
            }
            finally
            {
                dlmsReader.SetDISCMode();
                dlmsReader.Dispose();
            }
            if (tcpTestNotifier == null)
            {
                tcpTestNotifier = new TCPTestNotifier();
                pushPacketManager.InitializePushProfileTables();
                TCPTestNotifier.LogControlEventHandler += OnPushDataReceived;
                tcpTestNotifier.Connect(logBox);
                logService.LogMessage(logBox, $"\n----------------------------*** Listener Port Connected ***----------------------------", Color.DeepPink, true);
            }
            if (pushMonitorTimer == null)
            {
                pushMonitorTimer = new System.Timers.Timer(1000);
                pushMonitorTimer.Elapsed += PushMonitorTimer_Elapsed;
                pushMonitorTimer.Start();
            }
        }
        private void btnStopListener_Click(object sender, EventArgs e)
        {
            if (tcpTestNotifier != null)
            {
                TCPTestNotifier.LogControlEventHandler -= OnPushDataReceived;
                tcpTestNotifier?.StopServer();
                tcpTestNotifier?.Dispose();
                tcpTestNotifier = null;
                pushMonitorTimer?.Stop();
                pushMonitorTimer?.Dispose();
                pushMonitorTimer = null;
                logService.LogMessage(logBox, $"\n----------------------------*** Listener Port disconnected ***----------------------------", Color.DeepPink, true);
            }

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


        #region NEW IMPLEMENTATION
        private void LogPush(string deviceId, string profile, string timestamp)
        {
            string logEntry = $"Device ID: {deviceId}\t\t{timestamp}: {profile} Push Received";

            // Make this entry look clickable
            rtbPushLogs.SelectionStart = rtbPushLogs.TextLength;
            rtbPushLogs.SelectionColor = Color.Blue;
            rtbPushLogs.SelectionFont = new Font(rtbPushLogs.Font, FontStyle.Underline);
            rtbPushLogs.AppendText(logEntry + Environment.NewLine);
            rtbPushLogs.SelectionColor = rtbPushLogs.ForeColor;
        }
        private void rtbPushLogs_MouseDown(object sender, MouseEventArgs e)
        {
            /*try
            {
                int index = rtbPushLogs.GetCharIndexFromPosition(e.Location);
                int lineIndex = rtbPushLogs.GetLineFromCharIndex(index);
                if (lineIndex < 0 || lineIndex >= rtbPushLogs.Lines.Length)
                    return;

                string clickedLine = rtbPushLogs.Lines[lineIndex].Trim();
                if (string.IsNullOrWhiteSpace(clickedLine))
                    return;
                int receivedIndex = clickedLine.IndexOf("Received");
                if (receivedIndex > 0)
                {
                    var match = System.Text.RegularExpressions.Regex.Match(clickedLine, @"\d{2}/\d{2}/\d{4} \d{2}:\d{2}:\d{2}:\d{3} [AP]M");
                    if (match.Success)
                    {
                        string timestamp = match.Value;
                        if (clickedLine.Contains("Alert Push"))
                            HighlightGridColumn(dgAlert, timestamp);
                        else if (clickedLine.Contains("Instant Push"))
                            HighlightGridColumn(dgInstant, timestamp);
                        else if (clickedLine.Contains("Load Survey Push"))
                            HighlightGridColumn(dgLS, timestamp);
                        else if (clickedLine.Contains("Daily Energy Push"))
                            HighlightGridColumn(dgDE, timestamp);
                        else if (clickedLine.Contains("Self Registration Push"))
                            HighlightGridColumn(dgSR, timestamp);
                        else if (clickedLine.Contains("Billing Push"))
                            HighlightGridColumn(dgBill, timestamp);
                        else if (clickedLine.Contains("Tamper Push"))
                            HighlightGridColumn(dgTamper, timestamp);
                        else if (clickedLine.Contains("Current Bill Push"))
                            HighlightGridColumn(dgCB, timestamp);
                    }
                }
            }
            catch (Exception ex)
            {
                logService.LogMessage(rtbPushLogs, $"Error in rtbPushLogs_MouseDown: {ex.Message}", Color.Red, true);
            }*/
            try
            {
                int index = rtbPushLogs.GetCharIndexFromPosition(e.Location);
                int lineIndex = rtbPushLogs.GetLineFromCharIndex(index);

                if (lineIndex < 0 || lineIndex >= rtbPushLogs.Lines.Length)
                    return;

                string clickedLine = rtbPushLogs.Lines[lineIndex];
                if (!clickedLine.Contains("🔗"))
                    return;

                int linkIndex = clickedLine.IndexOf("🔗");
                int charIndexFromLine = rtbPushLogs.GetFirstCharIndexFromLine(lineIndex) + linkIndex;

                // check if click is near the link
                if (index >= charIndexFromLine - 1 && index <= charIndexFromLine + 2)
                {
                    // Extract timestamp from that line
                    var match = System.Text.RegularExpressions.Regex.Match(
                        clickedLine, @"\d{2}/\d{2}/\d{4} \d{2}:\d{2}:\d{2}:\d{3} [AP]M");

                    if (match.Success)
                    {
                        string timestamp = match.Value;
                        if (clickedLine.Contains("Alert Push"))
                            HighlightGridColumn(dgAlert, timestamp);
                        else if (clickedLine.Contains("Instant Push"))
                            HighlightGridColumn(dgInstant, timestamp);
                        else if (clickedLine.Contains("Load Survey Push"))
                            HighlightGridColumn(dgLS, timestamp);
                        else if (clickedLine.Contains("Daily Energy Push"))
                            HighlightGridColumn(dgDE, timestamp);
                        else if (clickedLine.Contains("Self Registration Push"))
                            HighlightGridColumn(dgSR, timestamp);
                        else if (clickedLine.Contains("Billing Push"))
                            HighlightGridColumn(dgBill, timestamp);
                        else if (clickedLine.Contains("Tamper Push"))
                            HighlightGridColumn(dgTamper, timestamp);
                        else if (clickedLine.Contains("Current Bill Push"))
                            HighlightGridColumn(dgCB, timestamp);
                    }
                }
            }
            catch (Exception ex)
            {
                logService.LogMessage(rtbPushLogs, $"Error handling link click: {ex.Message}", Color.Red, true);
            }

        }
        private void HighlightGridColumn(DataGridView grid, string columnHeader)
        {
            if (grid == null || grid.Columns.Count == 0)
                return;

            var col = grid.Columns
                .Cast<DataGridViewColumn>()
                .FirstOrDefault(c => c.HeaderText.Equals(columnHeader, StringComparison.OrdinalIgnoreCase));

            if (col == null)
                return;

            // Scroll to column
            grid.FirstDisplayedScrollingColumnIndex = col.Index;

            // Temporarily highlight column
            col.DefaultCellStyle.BackColor = Color.Yellow;

            var timer = new System.Windows.Forms.Timer();
            timer.Interval = 1500; // 1.5 seconds
            timer.Tick += (s, e) =>
            {
                col.DefaultCellStyle.BackColor = Color.White;
                timer.Stop();
            };
            timer.Start();
        }
        /*private void HighlightClickableLog(string updatedText)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(updatedText) || !updatedText.Contains("Push Received")) //🔗
                    return;

                string targetTrim = updatedText.Trim();

                // find last non-empty line
                string[] lines = rtbPushLogs.Lines;
                int lastNonEmptyIndex = -1;
                for (int i = lines.Length - 1; i >= 0; i--)
                {
                    if (!string.IsNullOrWhiteSpace(lines[i]))
                    {
                        lastNonEmptyIndex = i;
                        break;
                    }
                }

                if (lastNonEmptyIndex >= 0)
                {
                    string lastLineTrim = lines[lastNonEmptyIndex].Trim();
                    if (lastLineTrim.Equals(targetTrim, StringComparison.Ordinal))
                    {
                        int startIndex = rtbPushLogs.GetFirstCharIndexFromLine(lastNonEmptyIndex);
                        int length = lines[lastNonEmptyIndex].Length;
                        rtbPushLogs.Select(startIndex, length);
                        rtbPushLogs.SelectionColor = Color.Blue;
                        rtbPushLogs.SelectionFont = new Font(rtbPushLogs.Font, FontStyle.Underline);
                        rtbPushLogs.DeselectAll();
                        return;
                    }
                }

                int lastOcc = rtbPushLogs.Text.LastIndexOf(targetTrim, StringComparison.OrdinalIgnoreCase);
                if (lastOcc >= 0)
                {
                    rtbPushLogs.Select(lastOcc, targetTrim.Length);
                    rtbPushLogs.SelectionColor = Color.Blue;
                    rtbPushLogs.SelectionFont = new Font(rtbPushLogs.Font, FontStyle.Underline);
                    rtbPushLogs.DeselectAll();
                }
            }
            catch (Exception ex)
            {
                logService.LogMessage(rtbPushLogs, $"Error styling log: {ex.Message}", Color.Red, true);
            }
        }
        */
        private void AppendClickableLinkIcon(string updatedText)
        {
            try
            {
                string[] lines = rtbPushLogs.Lines;
                if (lines == null || lines.Length == 0)
                    return;

                int lastLineIndex = lines.Length - 1;
                string lastLine = lines[lastLineIndex];

                // Go to end of the last line
                int startIndex = rtbPushLogs.GetFirstCharIndexFromLine(lastLineIndex) + lastLine.Length;

                rtbPushLogs.Select(startIndex, 0);
                rtbPushLogs.SelectionColor = Color.Blue;
                rtbPushLogs.SelectionFont = new Font(rtbPushLogs.Font, FontStyle.Bold);
                rtbPushLogs.SelectedText = " 🔗";
                rtbPushLogs.DeselectAll();
            }
            catch (Exception ex)
            {
                logService.LogMessage(rtbPushLogs, $"Error appending link icon: {ex.Message}", Color.Red, true);
            }
        }

        private void rtbPushLogs_MouseMove(object sender, MouseEventArgs e)
        {
            /*
            int index = rtbPushLogs.GetCharIndexFromPosition(e.Location);
            int line = rtbPushLogs.GetLineFromCharIndex(index);

            if (line >= 0 && line < rtbPushLogs.Lines.Length)
            {
                string text = rtbPushLogs.Lines[line];
                if (text.Contains("Push Received"))
                    rtbPushLogs.Cursor = Cursors.Hand;
                else
                    rtbPushLogs.Cursor = Cursors.IBeam;
            }
            */
            int index = rtbPushLogs.GetCharIndexFromPosition(e.Location);
            int lineIndex = rtbPushLogs.GetLineFromCharIndex(index);

            if (lineIndex >= 0 && lineIndex < rtbPushLogs.Lines.Length)
            {
                string line = rtbPushLogs.Lines[lineIndex];
                if (line.Contains("🔗"))
                {
                    int linkIndex = line.IndexOf("🔗");
                    int charIndexFromLine = rtbPushLogs.GetFirstCharIndexFromLine(lineIndex) + linkIndex;

                    if (index >= charIndexFromLine - 1 && index <= charIndexFromLine + 2)
                    {
                        rtbPushLogs.Cursor = Cursors.Hand;
                        return;
                    }
                }
            }
            rtbPushLogs.Cursor = Cursors.IBeam;
        }

        #endregion

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
        private bool IniTestRun(TestConfiguration _config)
        {
            _cancellationToken = new CancellationTokenSource();
            var token = _cancellationToken.Token;
            WrapperInfo.IsCommDelayRequired = false;
            bool iniStatus = true;
            if (!MeterIdentity.AssignMeterDetails(_config, token))
            {
                iniStatus = false;
                return iniStatus;
            }
            if (MeterIdentity.GetCipherStatus())
            {
                if (!PushSetupInfo.ReadPushSetup(_config))
                {
                    iniStatus = false;
                    return iniStatus;
                }
            }
            return iniStatus;
        }
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
                        }

                        if (dgRawData.InvokeRequired)
                        {
                            dgRawData.Invoke(new Action(() =>
                            {
                                dgRawData.DataSource = null;
                                dgRawData.DataSource = receivedPushData;
                                dgRawData.Refresh();
                            }));
                        }
                        else
                        {
                            dgRawData.DataSource = null;
                            dgRawData.DataSource = receivedPushData;
                            dgRawData.Refresh();
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
            if (e.Action != DataRowAction.Add)
                return;
            try
            {
                string decryptedData = e.Row[4]?.ToString();
                string dataPacket = e.Row[0]?.ToString();
                if (string.IsNullOrEmpty(decryptedData))
                    return;
                DataTable targetTable = new DataTable();
                string profile = string.Empty;
                string DeviceID = dataPacket.Split('-')[2];

                DataTable onlyColumn = pushPacketManager.GeneratePushDataTableFromHex(decryptedData, dataPacket);
                targetTable = pushPacketManager.BuildTargetTableFromPushData(dataPacket, onlyColumn);
                profile = PushPacketManager.pushProfile;
                string saveName = profile.Replace("/", "").Replace(":", "").Replace("-", "_").Trim();

                DataGridView targetGrid = null;
                switch (profile)
                {
                    case "Instant": targetGrid = dgInstant; break;
                    case "Alert": targetGrid = dgAlert; break;
                    case "LS": targetGrid = dgLS; break;
                    case "DE": targetGrid = dgDE; break;
                    case "SR": targetGrid = dgSR; break;
                    case "Billing": targetGrid = dgBill; break;
                    case "Tamper": targetGrid = dgTamper; break;
                    case "Current Bill": targetGrid = dgCB; break;
                }

                if (targetGrid == null)
                    return;
                if (targetGrid.InvokeRequired)
                {
                    targetGrid.Invoke(new Action(() =>
                    {
                        targetGrid.DataSource = null;
                        targetGrid.DataSource = targetTable;
                        targetGrid.Refresh();
                    }));
                }
                else
                {
                    targetGrid.DataSource = null;
                    targetGrid.DataSource = targetTable;
                    targetGrid.Refresh();
                }
            }
            catch (Exception ex)
            {
                logService.LogMessage(logBox, $"Error in {PushPacketManager.pushProfile} => {ex.Message}\n{ex.StackTrace}", Color.Red);
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
                    if (updatedText.Contains("Push Received"))
                        AppendClickableLinkIcon(updatedText);
                    //rtbPushLogs.BeginInvoke(new Action(() => HighlightClickableLog(updatedText)));
                }));
            }
            else
            {
                logService.LogMessage(rtbPushLogs, updatedText, color, true);
                rtbPushLogs.ScrollToCaret();
                // Only append link if it's a push received line
                if (updatedText.Contains("Push Received"))
                    AppendClickableLinkIcon(updatedText);
                //rtbPushLogs.BeginInvoke(new Action(() => HighlightClickableLog(updatedText)));
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
                tabControlProfiles.Visible = false;
                dgRawData.Dock = DockStyle.Fill;

                btnRawData.Text = "Profile Tab View";
            }
            else
            {
                // Show TabControl in fill mode
                tabControlProfiles.Visible = true;
                dgRawData.Visible = false;
                tabControlProfiles.Dock = DockStyle.Fill;

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
        private void StyleDataGrid(DataGridView grid)
        {
            grid.AutoGenerateColumns = true;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            grid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            grid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.LightSteelBlue;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            grid.EnableHeadersVisualStyles = false;
            grid.BackgroundColor = Color.White;
            grid.BorderStyle = BorderStyle.FixedSingle;
            grid.GridColor = Color.LightGray;
            grid.SelectionMode = DataGridViewSelectionMode.CellSelect;
            grid.RowHeadersVisible = false;
            //grid.AllowUserToResizeRows = false;
            grid.AllowUserToOrderColumns = false;
            grid.Font = new Font("Times New Roman", 9);
            grid.ColumnHeadersHeight = 35;
        }
        #endregion
    }
}
