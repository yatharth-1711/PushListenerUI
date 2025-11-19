using AutoTest.FrameWork;
using AutoTest.FrameWork.Converts;
using AutoTestDesktopWFA;
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
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

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
        private Dictionary<string, List<string>> freqHexPatterns = new Dictionary<string, List<string>>
        {
            { "15 Min", new List<string>
                {
                    "010402020904FF0000000905FFFFFFFFFF02020904FF0F00000905FFFFFFFFFF02020904FF1E00000905FFFFFFFFFF02020904FF2D00000905FFFFFFFFFF",
                    "010402020904FF0000FF0905FFFFFFFFFF02020904FF0F00FF0905FFFFFFFFFF02020904FF1E00FF0905FFFFFFFFFF02020904FF2D00FF0905FFFFFFFFFF"
                }
            },
            { "30 Min", new List<string>
                {
                    "010202020904FF0000000905FFFFFFFFFF02020904FF1E00000905FFFFFFFFFF",
                    "010202020904FF0000FF0905FFFFFFFFFF02020904FF1E00FF0905FFFFFFFFFF"
                }
            },
            { "1 Hour", new List<string>
                {
                    "010102020904FF0000000905FFFFFFFFFF",
                    "010102020904FF0000FF0905FFFFFFFFFF"
                }
            },
            { "4 Hour", new List<string>
                {
                    "010602020904000000000905FFFFFFFFFF02020904040000000905FFFFFFFFFF02020904080000000905FFFFFFFFFF020209040C0000000905FFFFFFFFFF02020904100000000905FFFFFFFFFF02020904140000000905FFFFFFFFFF",
                    "010602020904000000FF0905FFFFFFFFFF02020904040000FF0905FFFFFFFFFF02020904080000FF0905FFFFFFFFFF020209040C0000FF0905FFFFFFFFFF02020904100000FF0905FFFFFFFFFF02020904140000FF0905FFFFFFFFFF"
                }
            },
            { "6 Hour", new List<string>
                {
                    "010402020904000000000905FFFFFFFFFF02020904060000000905FFFFFFFFFF020209040C0000000905FFFFFFFFFF02020904120000000905FFFFFFFFFF",
                    "010402020904000000FF0905FFFFFFFFFF02020904060000FF0905FFFFFFFFFF020209040C0000FF0905FFFFFFFFFF02020904120000FF0905FFFFFFFFFF"
                }
            },
            { "8 Hour", new List<string>
                {
                    "010302020904000000000905FFFFFFFFFF02020904080000000905FFFFFFFFFF02020904100000000905FFFFFFFFFF",
                    "010302020904000000FF0905FFFFFFFFFF02020904080000FF0905FFFFFFFFFF02020904100000FF0905FFFFFFFFFF"
                }
            },
            { "12 Hour", new List<string>
                {
                    "010202020904000000000905FFFFFFFFFF020209040C0000000905FFFFFFFFFF",
                    "010202020904000000FF0905FFFFFFFFFF020209040C0000FF0905FFFFFFFFFF"
                }
            },
            { "24 Hour", new List<string>
                {
                    "010102020904000000000905FFFFFFFFFF",
                    "010102020904000000FF0905FFFFFFFFFF"
                }
            },
            { "Disabled", new List<string>
                {
                    "0100"
                }
            }
        };
        public static List<(string Name, string DisplayName, string PushSetupClassObis, string ActionScheduleClassObisAtt, TextBox txt_Dest_Add, ComboBox cbFrequency, TextBox txtRandom)> profileControls;

        public ListenerForm()
        {
            InitializeComponent(); UISettings();
            StyleDataGrid(dgRawData); StyleDataGrid(dgAlert); StyleDataGrid(dgInstant); StyleDataGrid(dgLS);
            StyleDataGrid(dgTamper); StyleDataGrid(dgBill); StyleDataGrid(dgDE); StyleDataGrid(dgSR); StyleDataGrid(dgCB);

            InitializeLoggerAndConfigurations();
            PushPacketManager.DeviceID = "GOE12043714";
            finalDataTable.RowChanged += FinalDataTable_RowChanged;
            // Bind log event
            TestLogService.AppendColoredTextControlEventHandler += TestLogService_AppendColoredTextControlEventHandler;
            rtbPushLogs.LinkClicked += rtbPushLogs_LinkClicked;
            cbInstant_Frequency.DrawMode = DrawMode.OwnerDrawFixed;
            cbInstant_Frequency.DrawItem += cbInstant_Frequency_DrawItem;


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
        private void InitializeProfileControls()
        {
            profileControls = new List<(string Name, string DisplayName, string PushSetupClassObis, string ActionScheduleClassObisAtt, TextBox txt_Dest_Add, ComboBox cbFrequency, TextBox txtRandom)>
                                        {
                                        ("Instant",     "Instant",              "00280000190900FF",     "001600000F0004FF04",       txt_Instant_DestIP,     cbInstant_Frequency,    txt_Random_Instant),
                                        ("Alert",       "Alert",                "00280004190900FF",     null,                       txt_Alert_DestIP,       null,                   txt_Random_Alert),
                                        ("Bill",        "Bill",                 "00280084190900FF",     "001600000F0000FF04",       txt_Bill_DestIP,        cb_Bill_Frequency,      txt_Random_Bill),
                                        ("SR",          "Self Registration",    "00280082190900FF",     "001600000F008EFF04",       txt_SR_DestIP,          cb_SR_Frequency,        txt_Random_SR),
                                        ("DE",          "Daily Energy",         "00280006190900FF",     "001600050F0004FF04",       txt_DE_DestIP,          cb_DE_Frequency,        txt_Random_DE),
                                        ("LS",          "Load Survey",          "00280005190900FF",     "001600040F0004FF04",       txt_LS_DestIP,          cb_LS_Frequency,        txt_Random_LS),
                                        ("CB",          "Current Bill",         "00280000190981FF",     "001600000F0093FF04",       txt_CB_DestIP,          cb_CB_Frequency,        txt_Random_CB),
                                        //("Tamper",      "Tamper",               "00280086190900FF",     "001600000F008FFF04",       txt_Alert_DestIP,       cb_Alert_Frequency,     txt_Random_Alert)
                                        };
        }
        private void InitializeLoggerAndConfigurations()
        {
            logService = new TestLogService(rtbPushLogs);
            config = TestConfiguration.CreateDefault();
        }
        private void cbInstant_Frequency_DrawItem(object sender, DrawItemEventArgs e)
        {
            ComboBox combo = sender as ComboBox;

            if (e.Index < 0) return;

            // Remove default selection highlight
            e.DrawBackground();
            bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

            // Choose background color
            Color bgColor = isSelected ? Color.White : Color.White;
            Color textColor = Color.Black;

            using (SolidBrush brush = new SolidBrush(bgColor))
            {
                e.Graphics.FillRectangle(brush, e.Bounds);
            }

            using (SolidBrush textBrush = new SolidBrush(textColor))
            {
                e.Graphics.DrawString(combo.Items[e.Index].ToString(), combo.Font, textBrush, e.Bounds.X, e.Bounds.Y);
            }

            e.DrawFocusRectangle();
        }
        private async void btnStartListener_Click(object sender, EventArgs e)
        {
            rtbPushLogs.Clear();
            logService = new TestLogService(rtbPushLogs);
            logBox = rtbPushLogs;
            try
            {
                await System.Threading.Tasks.Task.Run(() =>
                {
                    this.Invoke(new Action(() =>
                    {
                        logService.LogMessage(rtbPushLogs, "Getting Meter Details and Push profiles...", Color.Black, true);
                        IniTestRun(config);
                        ProfileGenericInfo.FillTables();
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
            btnGet_PS_AS.Enabled = false;
            btnSet_PS_AS.Enabled = false;
            DLMSComm DLMSWriter = new DLMSComm(DLMSInfo.comPort, DLMSInfo.BaudRate);
            try
            {
                if (!DLMSWriter.SignOnDLMS())
                {
                    CommonHelper.DisplayDLMSSignONError();
                    return;
                }
                List<string> profilesToRead = cbTestProfileType.Text == "All"
                    ? new List<string> { "Instant", "Alert", "Bill", "SR", "DE", "LS", "CB" }
                    : new List<string> { cbTestProfileType.Text };

                foreach (string profile in profilesToRead)
                {
                    try
                    {
                        Get_PushFreq_Destination_Randomisation(ref DLMSWriter, profile);
                    }
                    catch (Exception innerEx)
                    {
                        log.Error($"[btnGet_PS_AS_Click] Error reading profile '{profile}': {innerEx.Message}", innerEx);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"[btnGet_PS_AS_Click] Unexpected error: {ex.Message}", ex);
                MessageBox.Show($"Error fetching Push Setup / Action Schedule details.\n\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnGet_PS_AS.Enabled = true;
                btnSet_PS_AS.Enabled = true;
                DLMSWriter.SetDISCMode();
                DLMSWriter.Dispose();
            }
        }
        private void btnSet_PS_AS_Click(object sender, EventArgs e)
        {
            btnGet_PS_AS.Enabled = false;
            btnSet_PS_AS.Enabled = false;
            DLMSComm DLMSWriter = new DLMSComm(DLMSInfo.comPort, DLMSInfo.BaudRate);
            try
            {
                if (!DLMSWriter.SignOnDLMS())
                {
                    CommonHelper.DisplayDLMSSignONError();
                    return;
                }
                List<string> profilesToRead = cbTestProfileType.Text == "All"
                    ? new List<string> { "Instant", "Alert", "LS", "DE", "SR", "Bill", "CB" }
                    : new List<string> { cbTestProfileType.Text };
                foreach (string profile in profilesToRead)
                {
                    try
                    {
                        var controls = profileControls.Find(p => p.Name == profile);
                        Set_PushFreq_Destination_Randomisation(ref DLMSWriter, profile);
                    }
                    catch (Exception innerEx)
                    {
                        log.Error($"[btnGet_PS_AS_Click] Error reading profile '{profile}': {innerEx.Message}", innerEx);
                    }
                }
            }
            catch
            {
                log.Error($"[btnGet_PS_AS_Click] Sign On Failure");
            }
            finally
            {
                btnGet_PS_AS.Enabled = true;
                btnSet_PS_AS.Enabled = true;
                DLMSWriter.SetDISCMode();
                DLMSWriter.Dispose();
            }
        }

        private void btnClearLogs_Click(object sender, EventArgs e)
        {
            rtbPushLogs.Clear();
        }
        private void btnSaveData_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Title = "Save Push Communication Reports";
                    sfd.Filter = "Excel Files (*.xlsx)|*.xlsx";
                    sfd.FileName = $"Push Reports.xlsx";
                    string filepath = Path.Combine(logService.LOG_DIRECTORY, $"Push Communication Reports\\{sfd.FileName}");
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        pushPacketManager.ExportReports(filepath);
                    }
                    DataTableOperations.ExportDataTableToExcelWithDifferentSheet(receivedPushData, filepath, "Raw Data");
                }
            }
            catch
            {
                log.Error("Eroor in exporting data");
            }
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
        private void rtbPushLogs_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            //MessageBox.Show($"You clicked: {e.LinkText}");
            string[] ClickedPacket = e.LinkText.Split(' ');
            var dateTimePattern = @"\b\d{2}/\d{2}/\d{4}\s+\d{2}:\d{2}:\d{2}:\d{3}\s?(AM|PM)\b";
            //var dateTimePattern = @"\b\d{2}/\d{2}/\d{4}\s+\d{2}:\d{2}:\d{2}[:\.]\d{3}\s?(AM|PM)\b";
            var match = System.Text.RegularExpressions.Regex.Match(e.LinkText, dateTimePattern);
            if (!match.Success)
            {
                MessageBox.Show("No valid date/time found in the clicked link.");
                return;
            }
            string dateTimeText = match.Value.Trim();
            string[] formats = { "dd/MM/yyyy hh:mm:ss:fff tt", "MM/dd/yyyy hh:mm:ss:fff tt" };
            if (!DateTime.TryParseExact(dateTimeText, formats, System.Globalization.CultureInfo.InvariantCulture,
                                        System.Globalization.DateTimeStyles.None, out DateTime parsedDateTime))
            {
                MessageBox.Show($"Invalid date/time format: {dateTimeText}");
                return;
            }
            string normalizedDateTime = parsedDateTime.ToString("dd/MM/yyyy hh:mm:ss:fff tt");

            //DataGridView[] grids = { dgAlert, dgInstant, dgLS, dgDE, dgCB, dgBill, dgTamper, dgSR };
            var gridTabMap = new Dictionary<DataGridView, TabPage>
            {
                { dgInstant, tbpInstant },
                { dgLS, tbpLS },
                { dgDE, tbpDE },
                { dgSR, tbpSR },
                { dgBill, tbpBill },
                { dgCB, tbpCB },
                { dgAlert, tbpAlert },
                { dgTamper, tbpTamper }
            };

            bool found = false;
            foreach (var kvp in gridTabMap)
            {
                DataGridView grid = kvp.Key;
                TabPage tab = kvp.Value;

                foreach (DataGridViewColumn col in grid.Columns)
                {
                    if (col.HeaderText.Contains(normalizedDateTime))
                    {
                        found = true;
                        tabControlProfiles.SelectedTab = tab;
                        foreach (DataGridViewColumn c in grid.Columns)
                        {
                            c.HeaderCell.Style.BackColor = SystemColors.Control;
                            foreach (DataGridViewRow row in grid.Rows)
                                row.Cells[c.Index].Style.BackColor = Color.White;
                        }
                        foreach (DataGridViewColumn c in grid.Columns)
                        {
                            c.HeaderCell.Style.BackColor = SystemColors.Control;
                            foreach (DataGridViewRow row in grid.Rows)
                                row.Cells[c.Index].Style.BackColor = Color.White;
                        }
                        grid.BeginInvoke(new Action(() =>
                        {
                            grid.FirstDisplayedScrollingColumnIndex = col.Index;
                            int nextCol = (col.Index + 1 < grid.Columns.Count) ? col.Index + 1 : col.Index;
                            HighlightColumns(grid, col.Index, nextCol);
                        }));

                        break;
                    }

                    //if (col.HeaderText.Contains(normalizedDateTime))
                    //{
                    //    found = true;
                    //    tabControlProfiles.SelectedTab = tab;
                    //    grid.FirstDisplayedScrollingColumnIndex = col.Index;
                    //    grid.EnableHeadersVisualStyles = false;
                    //    //RESET COLORS
                    //    foreach (DataGridViewColumn c in grid.Columns)
                    //    {
                    //        c.HeaderCell.Style.BackColor = SystemColors.Control;
                    //        foreach (DataGridViewRow row in grid.Rows)
                    //            row.Cells[c.Index].Style.BackColor = Color.White;
                    //    }
                    //    foreach (DataGridViewColumn c in grid.Columns)
                    //    {
                    //        c.HeaderCell.Style.BackColor = SystemColors.Control;
                    //        foreach (DataGridViewRow row in grid.Rows)
                    //            row.Cells[c.Index].Style.BackColor = Color.White;
                    //    }
                    //    HighlightColumns(grid, col.Index, col.Index + 1);
                    //    break;
                    //}
                }


                if (found)
                    break;
            }

            if (!found)
            {
                MessageBox.Show($"DateTime {normalizedDateTime} not found in any DataGridView headers.");
            }

            /*foreach (var grid in grids)
            {
                foreach (DataGridViewColumn col in grid.Columns)
                {
                    if (col.HeaderText.Contains(normalizedDateTime))
                    {
                        found = true;
                        MessageBox.Show($"DateTime {normalizedDateTime} found in {grid.Name} → Column: {col.HeaderText}");
                        break;
                    }
                }

                if (found)
                    break;
            }

            if (!found)
            {
                MessageBox.Show($"DateTime {normalizedDateTime} not found in any DataGridView headers.");
            }*/
        }
        #endregion

        #region Helper Methods
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

                case "LS":
                    EnableProfile(txt_LS_DestIP, cb_LS_Frequency, txt_Random_LS);
                    break;

                case "DE":
                    EnableProfile(txt_DE_DestIP, cb_DE_Frequency, txt_Random_DE);
                    break;

                case "SR":
                    EnableProfile(txt_SR_DestIP, cb_SR_Frequency, txt_Random_SR);
                    break;

                case "Bill":
                    EnableProfile(txt_Bill_DestIP, cb_Bill_Frequency, txt_Random_Bill);
                    break;

                case "Current Bill":
                    EnableProfile(txt_CB_DestIP, cb_CB_Frequency, txt_Random_CB);
                    break;

                case "Alert":
                    EnableProfile(txt_Alert_DestIP, null, txt_Random_Alert);
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
        cbInstant_Frequency, cb_Bill_Frequency,
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
            cbInstant_Frequency, cb_Bill_Frequency,
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
        public void Get_PushFreq_Destination_Randomisation(ref DLMSComm DLMSWriter, string profileName)
        {
            try
            {
                InitializeProfileControls();
                var controls = profileControls.Find(p => p.Name == profileName);
                string actionObis = controls.ActionScheduleClassObisAtt;
                string pushObis = controls.PushSetupClassObis;

                // Destination Address 
                string destAddrHex = SetGetFromMeter.GetDataFromObject(ref DLMSWriter, Convert.ToInt32(pushObis.Substring(0, 4), 16), parse.HexObisToDecObis(pushObis.Substring(4, 12)), 3).Trim();
                if (destAddrHex == "0B")
                {
                    controls.txt_Dest_Add.Text = "Object Not Available";
                }
                else if (destAddrHex.StartsWith("02"))
                {
                    string[] structArray = parse.GetStructureValueList(destAddrHex.Substring(4)).ToArray();
                    if (structArray.Length > 1)
                    {
                        controls.txt_Dest_Add.Text = parse.GetProfileValueString(structArray[1]);
                    }
                    else
                    {
                        controls.txt_Dest_Add.Text = "Invalid Structure";
                    }
                }
                else
                {
                    controls.txt_Dest_Add.Text = "Unknown";
                }

                // Push Frequency 
                if (controls.Name != "Alert" && controls.Name != "Tamper")
                {
                    string pushFreqHex = SetGetFromMeter.GetDataFromObject(ref DLMSWriter, Convert.ToInt32(actionObis.Substring(0, 4), 16), parse.HexObisToDecObis(actionObis.Substring(4, 12)), 4).Trim();
                    if (pushFreqHex == "0B")
                    {
                        controls.cbFrequency.Text = "Object Not Available";
                    }
                    else if (pushFreqHex.StartsWith("01"))
                    {
                        string freqValue = freqHexPatterns.FirstOrDefault(kvp => kvp.Value.Any(v => v.Equals(pushFreqHex, StringComparison.OrdinalIgnoreCase))).Key ?? "Unknown";
                        controls.cbFrequency.Text = freqValue;
                    }
                    else
                    {
                        controls.cbFrequency.Text = "Unknown";
                    }
                }

                // Randomization Delay 
                string randomizationHex = SetGetFromMeter.GetDataFromObject(ref DLMSWriter, Convert.ToInt32(pushObis.Substring(0, 4), 16), parse.HexObisToDecObis(pushObis.Substring(4, 12)), 5).Trim();
                if (randomizationHex == "0B")
                {
                    controls.txtRandom.Text = "Object Not Available";
                }
                else if (randomizationHex == "0D")
                {
                    controls.txtRandom.Text = "No Access";
                }
                else
                {
                    controls.txtRandom.Text = parse.GetProfileValueString(randomizationHex);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error in Get_PushFreq_Destination_Randomisation: " + ex.Message, ex);
            }
            finally
            {

            }
        }
        public void Set_PushFreq_Destination_Randomisation(ref DLMSComm DLMSWriter, string profileName)
        {
            // SetProfilePushFrequency(cbTestProfileType.Text.Trim(), cbInstant_Frequency.Text.Trim());
            // SetProfilePushFrequency(cbTestProfileType.Text.Trim(), "0", cb_Bill_Frequency.Text.Trim());
            try
            {
                InitializeProfileControls();
                int nRetVal = 0; string sMessage = string.Empty;
                var profile = profileControls.Find(p => p.Name == profileName);
                string actionObis = profile.ActionScheduleClassObisAtt;
                string pushObis = profile.PushSetupClassObis;
                DLMSWriter.strbldDLMdata.Clear();
                #region Destination Address 

                string destAddHex = $"{DLMSParser.ConvertAsciiToHex(profile.txt_Dest_Add.Text.ToString().Trim())}";
                string length = (destAddHex.Length / 2).ToString("X2");
                destAddHex = $"0203160009{length}{destAddHex}1600";
                nRetVal = DLMSWriter.SetParameter($"{profile.PushSetupClassObis.Trim()}03", (byte)0, (byte)3, (byte)5, $"{destAddHex}");
                if (nRetVal == 0)
                    sMessage = sMessage + "Push Setup: Destination Address Set Successfully.";
                else if (nRetVal == 2)
                    sMessage = sMessage + "Push Setup: Destination Address Action Denied.";
                else if (nRetVal == 1 || nRetVal == 3)
                    sMessage = sMessage + "Push Setup: Destination Address Error in Setting.";
                #endregion

                #region Push Frequency 
                DLMSWriter.strbldDLMdata.Clear();
                string billDateTime = "0100";
                if (profile.Name == "Bill" && !string.IsNullOrEmpty(profile.cbFrequency.SelectedItem.ToString().Trim()))
                {
                    billDateTime = billDateTime = $"010102020904{int.Parse(profile.cbFrequency.SelectedItem.ToString().Substring(7, 2)):X2}" +
                                $"{int.Parse(profile.cbFrequency.SelectedItem.ToString().Substring(10, 2)):X2}" +
                                $"{int.Parse(profile.cbFrequency.SelectedItem.ToString().Substring(13, 2)):X2}FF0905FFFFFF" +
                                $"{int.Parse(profile.cbFrequency.SelectedItem.ToString().Substring(0, 2)):X2}FF"; //10/*/* 00:00:00

                }
                Dictionary<string, string> freqHexString = new Dictionary<string, string>
                {
                    { "15 Min", "010402020904FF0000000905FFFFFFFFFF02020904FF0F00000905FFFFFFFFFF02020904FF1E00000905FFFFFFFFFF02020904FF2D00000905FFFFFFFFFF" },
                    { "30 Min", "010202020904FF0000000905FFFFFFFFFF02020904FF1E00000905FFFFFFFFFF" },
                    { "1 Hour", "010102020904FF0000000905FFFFFFFFFF" },
                    { "4 Hour", "010602020904000000000905FFFFFFFFFF02020904040000000905FFFFFFFFFF02020904080000000905FFFFFFFFFF020209040C0000000905FFFFFFFFFF02020904100000000905FFFFFFFFFF02020904140000000905FFFFFFFFFF" },
                    { "6 Hour", "010402020904000000000905FFFFFFFFFF02020904060000000905FFFFFFFFFF020209040C0000000905FFFFFFFFFF02020904120000000905FFFFFFFFFF" },
                    { "8 Hour", "010302020904000000000905FFFFFFFFFF02020904080000000905FFFFFFFFFF02020904100000000905FFFFFFFFFF" },
                    { "12 Hour", "010202020904000000000905FFFFFFFFFF020209040C0000000905FFFFFFFFFF"},
                    { "24 Hour", "010102020904000000000905FFFFFFFFFF"},
                    {"Disabled", "0100" },
                    { "0", billDateTime}
                };
                if (!string.IsNullOrEmpty(profile.ActionScheduleClassObisAtt))
                {
                    nRetVal = DLMSWriter.SetParameter($"{profile.ActionScheduleClassObisAtt}", (byte)0, (byte)3, (byte)3, freqHexString[profile.cbFrequency.SelectedItem.ToString().Trim()]);
                    if (nRetVal == 0)
                        sMessage = sMessage + $"{profile} Push Frequency Set Successfully to {profile.cbFrequency.SelectedItem.ToString().Trim()}.";
                    else if (nRetVal == 2)
                        sMessage = sMessage + $"{profile} Push Frequency Action Denied.";
                    else if (nRetVal == 1 || nRetVal == 3)
                        sMessage = sMessage + $"{profile} Push Frequency Error in Setting.";
                }
                #endregion

                #region Randomization Delay 
                if (!string.IsNullOrEmpty(profile.txtRandom.Text.Trim()) && int.TryParse(profile.txtRandom.Text.Trim(), out int number))
                {
                    nRetVal = DLMSWriter.SetParameter($"{profile.PushSetupClassObis.Trim()}05", (byte)0, (byte)3, (byte)5, $"12{number:X4}");
                    if (nRetVal == 0)
                        sMessage = sMessage + "Push Setup: Randamisation Set Successfully.";
                    else if (nRetVal == 2)
                        sMessage = sMessage + "Push Setup: Randamisation Action Denied.";
                    else if (nRetVal == 1 || nRetVal == 3)
                        sMessage = sMessage + "Push Setup: Randamisation Error in Setting.";
                }
                #endregion

            }
            catch (Exception ex)
            {
                log.Error("Error in Set_PushFreq_Destination_Randomisation: " + ex.Message, ex);
            }
            finally
            {

            }
        }
        #endregion

        #region Logging Methods
        private void HighlightColumns(DataGridView grid, int colIndex1, int colIndex2)
        {
            //Color highlight = Color.LightGreen;
            Color highlight = Color.FromArgb(232, 245, 233);

            if (colIndex1 >= 0 && colIndex1 < grid.Columns.Count)
            {
                grid.Columns[colIndex1].HeaderCell.Style.BackColor = highlight;
                foreach (DataGridViewRow row in grid.Rows)
                    row.Cells[colIndex1].Style.BackColor = highlight;
            }

            if (colIndex2 >= 0 && colIndex2 < grid.Columns.Count)
            {
                grid.Columns[colIndex2].HeaderCell.Style.BackColor = highlight;
                foreach (DataGridViewRow row in grid.Rows)
                    row.Cells[colIndex2].Style.BackColor = highlight;
            }
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
                Chart targetChart = null;
                switch (profile)
                {
                    case "Instant": targetGrid = dgInstant; targetChart = chart_Instant; break;
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
                        //PlotEnergyGraphFromDataTable(targetTable, "Cum. Energy-Wh(Imp)");
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
            //if (rtbPushLogs.InvokeRequired)
            //{
            //    rtbPushLogs.BeginInvoke(new Action(() =>
            //    {
            //        HandlePushLog(updatedText, color);
            //    }));
            //}
            //else
            //{
            HandlePushLog(updatedText, color);
            //}
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

            rtbPushLogs.BeginInvoke(new Action(() =>
            {
                bool atBottom = false;
                int visibleLines = rtbPushLogs.ClientSize.Height / rtbPushLogs.Font.Height;
                int firstVisibleLine = SendMessage(rtbPushLogs.Handle, EM_GETFIRSTVISIBLELINE, 0, 0);
                int lastVisibleLine = firstVisibleLine + visibleLines;
                atBottom = (lastVisibleLine >= rtbPushLogs.Lines.Length - 1);

                rtbPushLogs.SuspendLayout();

                while (_logBuffer2.TryDequeue(out var log))
                {
                    int startBeforeAppend = rtbPushLogs.TextLength;

                    rtbPushLogs.SelectionStart = startBeforeAppend;
                    rtbPushLogs.SelectionLength = 0;
                    rtbPushLogs.SelectionFont = log.IsBold ? BoldFont11 : RegularFont11;
                    rtbPushLogs.SelectionColor = log.Color;
                    rtbPushLogs.SelectedText = log.Message;

                    if (log.Message.Contains("Device ID") && log.Message.Contains("Received"))
                    {
                        int start = startBeforeAppend;
                        int length = log.Message.Length;

                        if (start >= 0 && length > 0)
                        {
                            rtbPushLogs.Select(start, length);
                            rtbPushLogs.SetSelectionLink(true);
                            rtbPushLogs.Select(rtbPushLogs.TextLength, 0);
                        }
                    }
                    //rtbPushLogs.SelectionStart = rtbPushLogs.TextLength;
                    //rtbPushLogs.SelectionLength = 0;

                    //rtbPushLogs.SelectionFont = log.IsBold ? BoldFont11 : RegularFont11;

                    //rtbPushLogs.SelectionColor = log.Color;

                    //rtbPushLogs.SelectedText = log.Message;
                }
                if (rtbPushLogs.Lines.Length > MaxLines)
                {
                    int cutOffIndex = rtbPushLogs.GetFirstCharIndexFromLine(TrimLines);
                    rtbPushLogs.Select(0, cutOffIndex);
                    rtbPushLogs.SelectedText = string.Empty;
                }

                if (atBottom)
                {
                    rtbPushLogs.SelectionStart = rtbPushLogs.TextLength;
                    rtbPushLogs.ScrollToCaret();
                }

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
        private void UISettings()
        {
            splitCon_Alert.SplitterDistance = tbpAlert.Width / 4;
            splitCon_Bill.SplitterDistance = tbpBill.Width / 4;
            splitCon_CB.SplitterDistance = tbpCB.Width / 4;
            splitCon_DE.SplitterDistance = tbpDE.Width / 4;
            splitcon_InstantTab.SplitterDistance = tbpInstant.Width / 4;
            splitCon_LS.SplitterDistance = tbpLS.Width / 4;
            splitCon_SR.SplitterDistance = tbpSR.Width / 4;
            splitCon_Tamper.SplitterDistance = tbpTamper.Width / 4;
            splitConMain.SplitterDistance = tblMain.Width / 2;
        }
        private void HandlePushLog(string updatedText, Color color)
        {
            logService.LogMessage(rtbPushLogs, updatedText, color, true);

            if (updatedText.Contains("Device ID") && updatedText.Contains("Received"))
            {
                int start = rtbPushLogs.Text.LastIndexOf(updatedText);
                if (start >= 0)
                {
                    updatedText = updatedText.Trim();
                    rtbPushLogs.Select(start, updatedText.Length);
                    rtbPushLogs.SetSelectionLink(true);
                    rtbPushLogs.Select(rtbPushLogs.TextLength, 0);
                }
            }

            rtbPushLogs.ScrollToCaret();
        }
        #endregion
        private void PlotEnergyGraphFromDataTable(DataTable dataTable, string rowHeaderName, int startColumnIndex = 8)
        {
            if (dataTable == null || dataTable.Columns.Count == 0 || dataTable.Rows.Count == 0)
            {
                MessageBox.Show("Invalid or empty DataTable.");
                return;
            }

            if (string.IsNullOrWhiteSpace(rowHeaderName))
            {
                MessageBox.Show("Row header name must be provided.");
                return;
            }

            // Find target row
            int targetRowIndex = -1;
            for (int r = 0; r < dataTable.Rows.Count; r++)
            {
                if (string.Equals(dataTable.Rows[r][0]?.ToString().Trim(), rowHeaderName.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    targetRowIndex = r;
                    break;
                }
            }

            if (targetRowIndex == -1)
            {
                MessageBox.Show($"Row '{rowHeaderName}' not found in DataTable.");
                return;
            }

            // Parse numeric values from column 9 onward
            var values = new List<double>();
            for (int c = startColumnIndex; c < dataTable.Columns.Count; c++)
            {
                var raw = dataTable.Rows[targetRowIndex][c]?.ToString();
                if (string.IsNullOrWhiteSpace(raw)) continue;

                if (double.TryParse(raw.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double parsed))
                    values.Add(parsed);
            }

            if (values.Count == 0)
            {
                MessageBox.Show($"No numeric values found for row '{rowHeaderName}' starting at column {startColumnIndex + 1}.");
                return;
            }

            // Configure chart
            chart_Instant.Series.Clear();
            chart_Instant.ChartAreas.Clear();
            chart_Instant.Legends.Clear();

            var area = new ChartArea("MainArea");
            area.AxisX.Title = $"Push Sequence (Starting from column {startColumnIndex + 1})";
            area.AxisY.Title = rowHeaderName;
            area.AxisX.LabelStyle.Angle = -45;
            area.AxisX.MajorGrid.Enabled = false;
            area.AxisY.MajorGrid.LineColor = Color.LightGray;
            chart_Instant.ChartAreas.Add(area);

            var series = new Series(rowHeaderName)
            {
                ChartType = SeriesChartType.Line,
                BorderWidth = 2,
                MarkerStyle = MarkerStyle.Circle,
                XValueType = ChartValueType.Int32,
                YValueType = ChartValueType.Double,
                ToolTip = "#VALY Wh at point #INDEX"
            };

            for (int i = 0; i < values.Count; i++)
                series.Points.AddXY(i + 1, values[i]);

            chart_Instant.Series.Add(series);
            chart_Instant.Titles.Clear();
            chart_Instant.Titles.Add($"{rowHeaderName} Over Time");

            chart_Instant.AntiAliasing = AntiAliasingStyles.Graphics;
            chart_Instant.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
            chart_Instant.Legends.Add(new Legend("Legend") { Docking = Docking.Top });

            chart_Instant.Invalidate();
        }


    }
    public static class RichTextBoxExtensions
    {
        const int EM_SETCHARFORMAT = 1092;
        const int SCF_SELECTION = 1;
        const int CFE_LINK = 32;
        const int CFM_LINK = 32;

        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        private struct CHARFORMAT2_STRUCT
        {
            public uint cbSize;
            public uint dwMask;
            public uint dwEffects;
            public int yHeight;
            public int yOffset;
            public int crTextColor;
            public byte bCharSet;
            public byte bPitchAndFamily;
            [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 32)]
            public char[] szFaceName;
            public ushort wWeight;
            public short sSpacing;
            public int crBackColor;
            public int lcid;
            public int dwReserved;
            public short sStyle;
            public short bOutline;
            public short bShadow;
            public short bCondense;
            public short bExtend;
            public int style;
        }

        [System.Runtime.InteropServices.DllImport("user32", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, ref CHARFORMAT2_STRUCT lParam);

        public static void SetSelectionLink(this RichTextBox box, bool link)
        {
            CHARFORMAT2_STRUCT cf = new CHARFORMAT2_STRUCT();
            cf.cbSize = (uint)System.Runtime.InteropServices.Marshal.SizeOf(cf);
            cf.dwMask = CFM_LINK;
            cf.dwEffects = link ? (uint)CFE_LINK : 0;
            SendMessage(box.Handle, EM_SETCHARFORMAT, SCF_SELECTION, ref cf);
        }
    }
}
