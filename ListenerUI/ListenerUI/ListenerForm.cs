using AutoTest.FrameWork;
using AutoTest.FrameWork.Converts;
using AutoTestDesktopWFA;
using Gurux.DLMS.Enums;
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
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Threading.Tasks;
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
                                                                                              //Delegate for Line traffic save with path
        public delegate void LineTrafficSaveWithPath(string fileName, string filePath);
        public static event LineTrafficSaveWithPath LineTrafficSaveWithPathEventHandler = delegate { };// add empty delegate!
        public delegate void AppendColoredTextControlWithBox(string message, Color color, bool isBold = false);
        public event AppendColoredTextControlWithBox AppendColoredTextControlNotifier = delegate { }; // add empty delegate!;
        private readonly ConcurrentQueue<(string Message, Color Color, bool IsBold)> _logBuffer2 = new ConcurrentQueue<(string, Color, bool)>();
        private readonly Font BoldFont11 = new Font("Courier New", 10f, FontStyle.Bold);
        private readonly Font RegularFont11 = new Font("Courier New", 10f, FontStyle.Regular);
        private const int MaxLines = 5000;
        private const int TrimLines = 2000;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        private const int EM_GETFIRSTVISIBLELINE = 0xCE;
        private readonly System.Timers.Timer _flushTimer2;
        #endregion

        private TestLogService logService;
        //private TestConfiguration config;
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
        public static WrapperComm WrapperObj = null;
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
        private TextBox txtEk;
        private TextBox txtAk;
        private TextBox txtSystemTitle;
        private Panel decryptPanel;
        public ListenerForm()
        {
            InitializeComponent();
            // Start background flush timer
            _flushTimer2 = new System.Timers.Timer(1000); // flush every 500ms
            _flushTimer2.Elapsed += FlushLogBuffer2;
            _flushTimer2.Start();
            BuildDecryptPopup();
        }
        public ListenerForm(ref WrapperComm _WrapperObj)
        {
            InitializeComponent();

            WrapperObj = _WrapperObj;
            // Start background flush timer
            _flushTimer2 = new System.Timers.Timer(1000); // flush every 500ms
            _flushTimer2.Elapsed += FlushLogBuffer2;
            _flushTimer2.Start();
            BuildDecryptPopup();
        }
        private void ListenerForm_Load(object sender, EventArgs e)
        {
            UISettings();
            StyleDataGrid(dgRawData); StyleDataGrid(dgAlert); StyleDataGrid(dgInstant); StyleDataGrid(dgLS);
            StyleDataGrid(dgTamper); StyleDataGrid(dgBill); StyleDataGrid(dgDE); StyleDataGrid(dgSR); StyleDataGrid(dgCB);

            logService = new TestLogService(rtbPushLogs);
            //InitializeLoggerAndConfigurations();

            PushPacketManager.DeviceID = "GOE12043714";
            finalDataTable.RowChanged += FinalDataTable_RowChanged;
            // Bind log event
            this.AppendColoredTextControlNotifier += TestLogService_AppendColoredTextControlNotifier;
            rtbPushLogs.LinkClicked += rtbPushLogs_LinkClicked;
            cbInstant_Frequency.DrawMode = DrawMode.OwnerDrawFixed;
            cbInstant_Frequency.DrawItem += cbInstant_Frequency_DrawItem;

            pnlProfileSettings.Click += BlockHide;
            grpProfileConfig.Click += BlockHide;

            PushPacketManager._logService = logService;
            PushPacketManager.logBox = rtbPushLogs;


            ComboBox[] comboBoxes = {
                cbTestProfileType, cbInstant_Frequency,cb_SR_Frequency, cb_DE_Frequency, cb_LS_Frequency, cb_CB_Frequency, cb_Bill_Frequency };
            foreach (var cb in comboBoxes)
                cb.SelectedIndex = 0;

            //Default Form Loading 
            rbBtnUserDefinedLog.Checked = true; tabControlProfiles.Visible = false; btnRawData.Visible = false;
            cbXML.Checked = true; cbHexDecrypted.Checked = true; cbHexCiphered.Checked = true;
            AddNodesofPushObjects();
            txtEk.Text = DLMSInfo.TxtEK;
            txtAk.Text = DLMSInfo.TxtAK;
            txtSystemTitle.Text = DLMSInfo.TxtSysT;

        }
        #region Click Events Listener Form
        private void InitializeProfileControls()
        {
            if (!(profileControls is null) && profileControls.Count > 0)
                profileControls.Clear();
            profileControls = new List<(string Name, string DisplayName, string PushSetupClassObis, string ActionScheduleClassObisAtt, TextBox txt_Dest_Add, ComboBox cbFrequency, TextBox txtRandom)>
                                        {
                                        ("Instant",     "Instant",              "00280000190900FF",     "001600000F0004FF04",       txt_Instant_DestIP,     cbInstant_Frequency,    txt_Random_Instant),
                                        ("Alert",       "Alert",                "00280004190900FF",     null,                       txt_Alert_DestIP,       null,                   null),
                                        ("Bill",        "Bill",                 "00280084190900FF",     "001600000F0000FF04",       txt_Bill_DestIP,        cb_Bill_Frequency,      txt_Random_Bill),
                                        ("SR",          "Self Registration",    "00280082190900FF",     "001600000F008EFF04",       txt_SR_DestIP,          cb_SR_Frequency,        txt_Random_SR),
                                        ("DE",          "Daily Energy",         "00280006190900FF",     "001600050F0004FF04",       txt_DE_DestIP,          cb_DE_Frequency,        txt_Random_DE),
                                        ("LS",          "Load Survey",          "00280005190900FF",     "001600040F0004FF04",       txt_LS_DestIP,          cb_LS_Frequency,        txt_Random_LS),
                                        ("CB",          "Current Bill",         "00280000190981FF",     "001600000F0093FF04",       txt_CB_DestIP,          cb_CB_Frequency,        txt_Random_CB),
                                        //("Tamper",      "Tamper",               "00280086190900FF",     "001600000F008FFF04",       txt_Alert_DestIP,       cb_Alert_Frequency,     txt_Random_Alert)
                                        };
        }
        private void btnPushprofileSettings_Click(object sender, EventArgs e)
        {
            pnlProfileSettings.Visible = !pnlProfileSettings.Visible;

            if (pnlProfileSettings.Visible)
            {
                btnPushprofileSettings.Text = "▲ Hide Push Profile Settings";
                btnPushprofileSettings.ForeColor = Color.FromArgb(0, 94, 168);
            }
            else
            {
                btnPushprofileSettings.Text = "▼ Show Push Profile Settings";
                btnPushprofileSettings.ForeColor = Color.Black;
            }
        }
        private void rbBtnDetailLog_CheckedChanged(object sender, EventArgs e)
        {
            if (rbBtnDetailLog.Checked)
            {
                btnNotificationType.Enabled = false; btnNotificationType.Visible = false; tabControlProfiles.Visible = true; btnRawData.Visible = true; btnNotifyDecryptSettings.Visible = false;
                cbXML.Checked = false; cbHexDecrypted.Checked = false; cbHexCiphered.Checked = false;
            }
            else
            {
                btnNotificationType.Enabled = true; btnNotificationType.Visible = true; tabControlProfiles.Visible = false; btnRawData.Visible = false; btnNotifyDecryptSettings.Visible = true;
                cbXML.Checked = true; cbHexDecrypted.Checked = true; cbHexCiphered.Checked = true;
            }
        }
        private void btnNotifyDecryptSettings_Click(object sender, EventArgs e)
        {
            var screenPoint = btnNotifyDecryptSettings.PointToScreen(
                new Point(0, btnNotifyDecryptSettings.Height));

            var clientPoint = this.PointToClient(screenPoint);

            decryptPanel.Location = clientPoint;
            decryptPanel.BringToFront();
            decryptPanel.Visible = true;
        }
        private void btnNotificationType_Click(object sender, EventArgs e)
        {
            cmsNotificationOption.Show(btnNotificationType, 0, btnNotificationType.Height);
        }
        private async void btnStartListener_Click(object sender, EventArgs e)
        {
            try
            {
                btnNotifyDecryptSettings.Enabled = false;
                btnStartListener.Enabled = false;
                btnClearLogs.Enabled = false;
                btnSaveData.Enabled = false;
                btnStopListener.Enabled = true;
                if (rbBtnDetailLog.Checked && !DLMSInfo.IsInterfaceHDLC && !SessionGlobalMeterCommunication.IsMeterConnected)
                {
                    CommonHelper.DisplayErrorMessage("Error", "Connect the meter in Dashboard in WRAPPER Mode");
                    return;
                }
                dgRawData.DataSource = null; lastPushRowCount = 0;
                receivedPushData.Rows.Clear(); finalDataTable.Rows.Clear();
                TCPTestNotifier.showXMLData = cbXML.Checked; TCPTestNotifier.showDecryptedHexData = cbHexDecrypted.Checked; TCPTestNotifier.showCipheredHexData = cbHexCiphered.Checked;
                rbBtnDetailLog.Enabled = false; rbBtnUserDefinedLog.Enabled = false;
                rtbPushLogs.Clear();
                dgRawData.DataSource = null;
                dgRawData.Invalidate();
                string _recData = string.Empty;
                int obisCount = 0;
                if (rbBtnDetailLog.Checked)
                {
                    logService = new TestLogService(rtbPushLogs);
                    logBox = rtbPushLogs;
                    try
                    {
                        await System.Threading.Tasks.Task.Run(() =>
                        {
                            this.Invoke(new Action(() =>
                            {
                                AppendColoredTextControlNotifier("Getting Meter Details and Push profiles...", Color.Black, true);
                                if (rbBtnDetailLog.Checked && !DLMSInfo.IsInterfaceHDLC && SessionGlobalMeterCommunication.IsMeterConnected)
                                {
                                    //if (DLMSProfileGenericHelper.IsScalerandColumnsAvailable)
                                    //{
                                    if (WrapperObj.GetAssociationView(null))
                                    {
                                        _recData = WrapperComm.recData;
                                        WrapperObj.GetScalersAndUnits();
                                        WrapperObj.GetProfileGenericColumns();
                                        //DLMSProfileGenericHelper.IsScalerandColumnsAvailable = true;
                                    }
                                    //}
                                }
                                if (IniTestRun())
                                {
                                    if (DLMSInfo.IsInterfaceHDLC)
                                        ProfileGenericInfo.FillTables();
                                    else
                                        ProfileGenericInfo.FillTables(ref WrapperObj);
                                }
                                else
                                {
                                    btnStartListener.Enabled = true;
                                    rbBtnDetailLog.Enabled = true; rbBtnUserDefinedLog.Enabled = true;
                                    return;
                                }
                            }));
                        });
                    }
                    catch (Exception ex)
                    {
                        log.Error($"{ex.Message.ToString()} => TRACE: {ex.StackTrace.ToString()}");
                        return;
                    }
                    DataTable USdataTable = new DataTable();
                    if (DLMSInfo.IsInterfaceHDLC)
                    {
                        DLMSComm dlmsReader = new DLMSComm(DLMSInfo.comPort, DLMSInfo.BaudRate);
                        try
                        {
                            if (!dlmsReader.SignOnDLMS())
                            {
                                log.Error("Sign ON Failure");
                                AppendColoredTextControlNotifier("Sign ON Failure", Color.Red, true);
                                return;
                            }

                            if (DLMSAssociationLN.IsUSAssociationAvailable)
                                USdataTable = DLMSAssociationLN.US_AssociationDataTable.Copy();
                            else
                            {
                                if (dlmsReader.GetParameter("000F0000280003FF02", (byte)(DLMSInfo.InterFrameTimeout / 1000), (byte)5, (byte)(DLMSInfo.ResponseTimeout / 1000), (byte)0, DateTime.Now, DateTime.Now, string.Empty, 0UL, 0UL))
                                    _recData = dlmsReader.strbldDLMdata.ToString().Trim().Split(' ')[3];
                                if (_recData.Length > 20)
                                    USdataTable = DLMSAssociationLN.GetObjectListTable(_recData, DLMSAssociationLN.AssociationType.Utility_Settings, out obisCount);
                            }
                            PushPacketManager.isCurrentBillAvailable = USdataTable.AsEnumerable().Any(row => row[4].ToString().Contains("0.0.25.9.129.255"));
                        }
                        catch (Exception ex)
                        {
                            log.Error($"{ex.Message.ToString()} => TRACE: {ex.StackTrace.ToString()}");
                        }
                        finally
                        {
                            dlmsReader.SetDISCMode();
                            dlmsReader.Dispose();
                        }
                    }
                    else
                    {
                        USdataTable = DLMSAssociationLN.GetObjectListTable(_recData, DLMSAssociationLN.AssociationType.Utility_Settings, out obisCount);
                        PushPacketManager.isCurrentBillAvailable = USdataTable.AsEnumerable().Any(row => row[4].ToString().Contains("0.0.25.9.129.255"));
                    }
                }
                if (tcpTestNotifier == null)
                {
                    TCPTestNotifier.isUIHandler = true;
                    tcpTestNotifier = new TCPTestNotifier();
                    if (rbBtnDetailLog.Checked)
                        pushPacketManager.InitializePushProfileTables();
                    TCPTestNotifier.AppendColoredTextControlNotifier += OnPushDataReceived;
                    tcpTestNotifier.Connect(logBox);
                    AppendColoredTextControlNotifier($"\n----------------------------*** Listener Port Connected ***----------------------------", Color.DeepPink, true);
                }
                if (pushMonitorTimer == null)
                {
                    pushMonitorTimer = new System.Timers.Timer(1000);
                    pushMonitorTimer.Elapsed += PushMonitorTimer_Elapsed;
                    pushMonitorTimer.Start();
                }
            }
            catch (Exception ex)
            {
                log.Error($"Error in starting Listener {ex.Message} => TRACE: {ex.StackTrace}");
                btnClearLogs.Enabled = true; btnSaveData.Enabled = true;
            }
        }
        private void btnStopListener_Click(object sender, EventArgs e)
        {
            TCPTestNotifier.showXMLData = false; TCPTestNotifier.showDecryptedHexData = false; TCPTestNotifier.showCipheredHexData = false; TCPTestNotifier.useSeparateCredentials = false;
            try
            {
                if (tcpTestNotifier != null)
                {
                    TCPTestNotifier.AppendColoredTextControlNotifier -= OnPushDataReceived;
                    tcpTestNotifier?.StopServer();
                    tcpTestNotifier?.Dispose();
                    tcpTestNotifier = null;
                    pushMonitorTimer?.Stop();
                    pushMonitorTimer?.Dispose();
                    pushMonitorTimer = null;
                    AppendColoredTextControlNotifier($"\n----------------------------*** Listener Port disconnected ***----------------------------", Color.DeepPink, true);
                    rbBtnDetailLog.Enabled = true; rbBtnUserDefinedLog.Enabled = true;
                    TCPTestNotifier.isUIHandler = false;
                }
            }
            catch (Exception ex)
            {
                log.Error($"Error in stopping Listener {ex.Message} => TRACE: {ex.StackTrace}");
            }
            finally
            {
                btnNotifyDecryptSettings.Enabled = true;
                btnStartListener.Enabled = true;
                btnClearLogs.Enabled = true;
                btnSaveData.Enabled = true;
                btnStopListener.Enabled = false;
            }
        }
        private void btnClearLogs_Click(object sender, EventArgs e)
        {
            rtbPushLogs.Clear();
            pushPacketManager.ResetRecPushDT();
            ClearDatagrids();
        }
        private void btnSaveData_Click(object sender, EventArgs e)
        {
            string reportFolder = "";
            try
            {
                //string folderName = AskFolderName($"Push Report {DateTime.Now.ToString("ddMMyyyyHHmmss")}");
                string folderName = $"Push Report {DateTime.Now.ToString("ddMMyyyyHHmmss")}";
                if (string.IsNullOrWhiteSpace(folderName))
                {
                    MessageBox.Show("Invalid folder name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string baseFolder = Path.Combine(logService.LOG_DIRECTORY, $"Push Notifier Reports");
                Directory.CreateDirectory(baseFolder);

                // Create the new folder inside base
                reportFolder = Path.Combine(baseFolder, folderName);
                Directory.CreateDirectory(reportFolder);

                string excelPath = Path.Combine(reportFolder, $"Reports_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.xlsx");
                string rtfPath = Path.Combine(reportFolder, $"Logs_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.rtf");
                LineTrafficSaveWithPathEventHandler($"{"Linetraffic"}_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.rtf", reportFolder);
                DataTableOperations.ExportDataTableToExcelWithDifferentSheet(receivedPushData, excelPath, "Raw Data");
                if (rbBtnDetailLog.Checked)
                    pushPacketManager.ExportReports(excelPath);
                rtbPushLogs.SaveFile(rtfPath, RichTextBoxStreamType.RichText);
                //MessageBox.Show($"Reports saved at:\n{reportFolder}", "Report Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                log.Error("Error in exporting data");
                log.Error($"{ex.Message.ToString()} => TRACE: {ex.StackTrace.ToString()}");
            }
            finally
            {
                Utilities.ShowNotFoundElementsDialog("Report Info", new List<string> { "Reports saved at:", reportFolder }, 200);
            }
        }
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

        //Notification Type CheckBox Click Events
        private void cbHexCiphered_Click(object sender, EventArgs e)
        {
            TCPTestNotifier.showCipheredHexData = cbHexCiphered.Checked;
        }
        private void cbHexDecrypted_Click(object sender, EventArgs e)
        {
            TCPTestNotifier.showDecryptedHexData = cbHexDecrypted.Checked;
        }
        private void cbXML_Click(object sender, EventArgs e)
        {
            TCPTestNotifier.showXMLData = cbXML.Checked;
        }
        #endregion

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
        private void btnGet_PS_AS_Click(object sender, EventArgs e)
        {
            btnGet_PS_AS.Enabled = false;
            btnSet_PS_AS.Enabled = false;
            List<string> profilesToRead = cbTestProfileType.Text == "All"
                ? new List<string> { "Instant", "Alert", "Bill", "SR", "DE", "LS", "CB" }
                : new List<string> { cbTestProfileType.Text };
            DLMSComm DLMSWriter = null;
            try
            {
                InitializeProfileControls();
                foreach (string profile in profilesToRead)
                {
                    var controls = profileControls.Find(p => p.Name == profile);
                    controls.txt_Dest_Add.Text = "";
                    controls.txt_Dest_Add.BackColor = SystemColors.Window;
                    // Push Frequency 
                    if (controls.Name != "Alert" && controls.Name != "Tamper")
                    {
                        controls.cbFrequency.Text = "";
                        controls.cbFrequency.BackColor = SystemColors.Window;
                    }
                    // Randomization Delay 
                    if (controls.txtRandom != null)
                    {
                        controls.txtRandom.Text = "";
                        controls.txtRandom.BackColor = SystemColors.Window;
                    }
                }
                if (DLMSInfo.IsInterfaceHDLC)
                {
                    DLMSWriter = new DLMSComm(DLMSInfo.comPort, DLMSInfo.BaudRate);
                    if (!DLMSWriter.SignOnDLMS())
                    {
                        CommonHelper.DisplayDLMSSignONError();
                        return;
                    }
                    foreach (string profile in profilesToRead)
                    {
                        try
                        {
                            Get_PushFreq_Destination_Randomisation(ref DLMSWriter, profile);
                        }
                        catch (Exception innerEx)
                        {
                            log.Error($"[btnGet_PS_AS_Click] Error reading profile '{profile}': {innerEx.Message}", innerEx);
                            log.Error($"TRACE: {innerEx.StackTrace.ToString()}");
                        }
                    }
                }
                else
                {
                    if (!SessionGlobalMeterCommunication.IsMeterConnected)
                    {
                        CommonHelper.DisplayErrorMessage("Error", "Connect the meter in Dashboard in WRAPPER Mode");
                        return;
                    }
                    foreach (string profile in profilesToRead)
                    {
                        try
                        {
                            Get_PushFreq_Destination_Randomisation(ref DLMSWriter, profile);
                            if (WrapperComm.errorMessage.Contains("Failed to receive reply from the device"))
                            {
                                CommonHelper.DisplayErrorMessage("Error", "Disconnect and Connect again the meter in Dashboard in WRAPPER Mode");
                                break;
                            }
                        }
                        catch (Exception innerEx)
                        {
                            log.Error($"[btnGet_PS_AS_Click] Error reading profile '{profile}': {innerEx.Message}", innerEx);
                            log.Error($"TRACE: {innerEx.StackTrace.ToString()}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message.ToString()} => TRACE: {ex.StackTrace.ToString()}");
                log.Error($"[btnGet_PS_AS_Click] Unexpected error: {ex.Message}", ex);
                MessageBox.Show($"Error fetching Push Setup / Action Schedule details.\n\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnGet_PS_AS.Enabled = true;
                btnSet_PS_AS.Enabled = true;
                if (DLMSInfo.IsInterfaceHDLC)
                {
                    DLMSWriter.SetDISCMode();
                    DLMSWriter.Dispose();
                }
            }
        }
        private void btnSet_PS_AS_Click(object sender, EventArgs e)
        {
            btnGet_PS_AS.Enabled = false;
            btnSet_PS_AS.Enabled = false;
            DLMSComm DLMSWriter = null;
            List<string> profilesToRead = cbTestProfileType.Text == "All"
                ? new List<string> { "Instant", "Alert", "Bill", "SR", "DE", "LS", "CB" }
                : new List<string> { cbTestProfileType.Text };
            try
            {
                InitializeProfileControls();
                foreach (string profile in profilesToRead)
                {
                    var controls = profileControls.Find(p => p.Name == profile);
                    controls.txt_Dest_Add.BackColor = SystemColors.Window;
                    // Push Frequency 
                    if (controls.Name != "Alert" && controls.Name != "Tamper")
                    {
                        controls.cbFrequency.BackColor = SystemColors.Window;
                    }
                    // Randomization Delay 
                    if (controls.txtRandom != null)
                    {
                        controls.txtRandom.BackColor = SystemColors.Window;
                    }
                }
                if (DLMSInfo.IsInterfaceHDLC)
                {
                    DLMSWriter = new DLMSComm(DLMSInfo.comPort, DLMSInfo.BaudRate);
                    if (!DLMSWriter.SignOnDLMS())
                    {
                        CommonHelper.DisplayDLMSSignONError();
                        return;
                    }

                    foreach (string profile in profilesToRead)
                    {
                        try
                        {
                            //var controls = profileControls.Find(p => p.Name == profile);
                            Set_PushFreq_Destination_Randomisation(ref DLMSWriter, profile);
                        }
                        catch (Exception innerEx)
                        {
                            log.Error($"[btnGet_PS_AS_Click] Error reading profile '{profile}': {innerEx.Message}", innerEx);
                            log.Error($"TRACE: {innerEx.StackTrace.ToString()}");
                        }
                    }
                }
                else
                {
                    foreach (string profile in profilesToRead)
                    {
                        try
                        {
                            //var controls = profileControls.Find(p => p.Name == profile);
                            Set_PushFreq_Destination_Randomisation(ref DLMSWriter, profile);
                            if (WrapperComm.errorMessage.Contains("Failed to receive reply from the device"))
                            {
                                CommonHelper.DisplayErrorMessage("Error", "Disconnect and Connect again the meter in Dashboard in WRAPPER Mode");
                                break;
                            }
                        }
                        catch (Exception innerEx)
                        {
                            log.Error($"[btnGet_PS_AS_Click] Error reading profile '{profile}': {innerEx.Message}", innerEx);
                            log.Error($"TRACE: {innerEx.StackTrace.ToString()}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"[btnGet_PS_AS_Click] Sign On Failure");
                log.Error($"{ex.Message.ToString()} => TRACE: {ex.StackTrace.ToString()}");
            }
            finally
            {
                btnGet_PS_AS.Enabled = true;
                btnSet_PS_AS.Enabled = true;
                if (DLMSInfo.IsInterfaceHDLC)
                {
                    DLMSWriter.SetDISCMode();
                    DLMSWriter.Dispose();
                }
            }
        }
        private void cb_Bill_Frequency_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_Bill_Frequency.SelectedItem != null && cb_Bill_Frequency.SelectedItem.ToString() == "Custom")
            {
                txtBillFreq.Visible = true;
                txtBillFreq.Enabled = true;

            }
            else
            {
                txtBillFreq.Visible = false;
                txtBillFreq.Enabled = false;
                txtBillFreq.Text = string.Empty;
            }
        }
        private void BlockHide(object sender, EventArgs e)
        {
            // Do NOTHING HANDLER METHOD added by YS
            // DO NOT REMOVE THIS METHOD, MANDATORY TO PREVENT A FUNCTIONALIY ISSUE
        }
        private void txtBillFreq_Leave(object sender, EventArgs e)
        {
            if (!txtBillFreq.MaskCompleted)
            {
                MessageBox.Show("Incomplete format! Please fill all values.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string text = txtBillFreq.Text;

            int dd = int.Parse(text.Substring(0, 2));  // Day
            int hh = int.Parse(text.Substring(7, 2));  // Hour
            int mm = int.Parse(text.Substring(10, 2)); // Minute
            int ss = int.Parse(text.Substring(13, 2)); // Second

            bool isValid = true;
            string msg = "";

            if (dd < 1 || dd > 31)
            {
                isValid = false;
                msg += "Day must be between 01 and 31.\n";
            }
            if (hh < 0 || hh > 23)
            {
                isValid = false;
                msg += "Hours must be between 00 and 23.\n";
            }
            if (mm < 0 || mm > 59)
            {
                isValid = false;
                msg += "Minutes must be between 00 and 59.\n";
            }
            if (ss < 0 || ss > 59)
            {
                isValid = false;
                msg += "Seconds must be between 00 and 59.\n";
            }

            if (!isValid)
            {
                MessageBox.Show(msg, "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Reset to placeholder format
                txtBillFreq.Text = "__/*/* __:__:__";
                txtBillFreq.SelectionStart = 0;
            }
        }

        #region NEW IMPLEMENTATION
        private void rtbPushLogs_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                string[] ClickedPacket = e.LinkText.Split(' ');
                var dateTimePattern = @"\b\d{2}/\d{2}/\d{4}\s+\d{2}:\d{2}:\d{2}\s?(AM|PM)\b";
                //var dateTimePattern = @"\b\d{2}/\d{2}/\d{4}\s+\d{2}:\d{2}:\d{2}[:\.]\d{3}\s?(AM|PM)\b";
                var match = System.Text.RegularExpressions.Regex.Match(e.LinkText, dateTimePattern);
                if (!match.Success)
                {
                    MessageBox.Show("No valid date/time found in the clicked link.");
                    return;
                }
                string dateTimeText = match.Value.Trim();
                string[] formats = { "dd/MM/yyyy hh:mm:ss tt", "MM/dd/yyyy hh:mm:ss tt" };
                if (!DateTime.TryParseExact(dateTimeText, formats, System.Globalization.CultureInfo.InvariantCulture,
                                            System.Globalization.DateTimeStyles.None, out DateTime parsedDateTime))
                {
                    MessageBox.Show($"Invalid date/time format: {dateTimeText}");
                    return;
                }
                string normalizedDateTime = parsedDateTime.ToString("dd/MM/yyyy hh:mm:ss tt");

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
                    if (e.LinkText.Contains(tab.Text))
                    {
                        foreach (DataGridViewColumn col in grid.Columns)
                        {
                            if (col.HeaderText.Contains(normalizedDateTime))
                            {
                                found = true;
                                tabControlProfiles.SelectedTab = tab;
                                ClearHighlight(grid);
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
                    }
                    if (found)
                        break;
                }
                if (!found)
                {
                    bool IsPacketHeader(DataGridViewRow r)
                    {
                        return r.Cells[1].Value != null &&
                               !string.IsNullOrWhiteSpace(r.Cells[1].Value.ToString()) &&
                               r.Cells[2].Value != null &&
                               !string.IsNullOrWhiteSpace(r.Cells[2].Value.ToString());
                    }
                    foreach (DataGridViewRow row in dgLS.Rows)
                    {
                        if (row.Cells[0].Value != null &&
                            row.Cells[0].Value.ToString().Contains(normalizedDateTime))
                        {
                            found = true;
                            tabControlProfiles.SelectedTab = tbpLS;
                            ClearHighlight(dgLS);
                            dgLS.BeginInvoke(new Action(() =>
                            {
                                int startRow = row.Index;
                                if (startRow < 0 || startRow >= dgLS.Rows.Count)
                                    return;
                                for (int i = startRow; i < dgLS.Rows.Count; i++)
                                {
                                    if (i != startRow && IsPacketHeader(dgLS.Rows[i]))
                                        break;
                                    foreach (DataGridViewCell cell in dgLS.Rows[i].Cells)
                                    {
                                        cell.Style.BackColor = Color.FromArgb(232, 245, 233);
                                    }
                                }
                                dgLS.FirstDisplayedScrollingRowIndex = startRow;
                            }));
                            break;
                        }
                    }
                    /* //Old Method
                    foreach (DataGridViewRow row in dgLS.Rows)
                    {
                        if (row.Cells[0].Value != null &&
                            row.Cells[0].Value.ToString().Contains(normalizedDateTime))
                        {
                            found = true;
                            tabControlProfiles.SelectedTab = tbpLS;
                            ClearHighlight(dgLS);
                            dgLS.BeginInvoke(new Action(() =>
                            {
                                int startRow = row.Index;

                                for (int i = startRow; i < dgLS.Rows.Count; i++)
                                {
                                    if (dgLS.Rows[i].Cells[0].Value == null ||
                                        !dgLS.Rows[i].Cells[0].Value.ToString().Contains(normalizedDateTime))
                                        break;

                                    foreach (DataGridViewCell cell in dgLS.Rows[i].Cells)
                                    {
                                        cell.Style.BackColor = Color.FromArgb(232, 245, 233);
                                    }
                                }

                                dgLS.FirstDisplayedScrollingRowIndex = row.Index;
                            }));
                            break;
                        }
                    }
                    */
                }
                if (!found)
                {
                    MessageBox.Show($"DateTime {normalizedDateTime} not found in any DataGridView headers.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in redirecting to related packet");
                log.Error($"{ex.Message.ToString()} => TRACE: {ex.StackTrace.ToString()}");
            }
            finally
            {

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
        private void ClearHighlight(DataGridView grid)
        {
            foreach (DataGridViewColumn c in grid.Columns)
                c.HeaderCell.Style.BackColor = SystemColors.Control;

            foreach (DataGridViewRow r in grid.Rows)
                foreach (DataGridViewCell cell in r.Cells)
                    cell.Style.BackColor = Color.White;
        }
        #endregion

        #region Helper Methods
        private bool IniTestRun()
        {
            _cancellationToken = new CancellationTokenSource();
            var token = _cancellationToken.Token;
            WrapperInfo.IsCommDelayRequired = false;
            bool iniStatus = false;

            if (DLMSInfo.IsInterfaceHDLC)
                iniStatus = PushSetupInfo.ReadPushSetup(TestConfiguration.CreateDefault());
            else if (!DLMSInfo.IsInterfaceHDLC && !SessionGlobalMeterCommunication.IsMeterConnected)
            {
                CommonHelper.DisplayErrorMessage("Error", "Connect the meter in Dashboard in WRAPPER Mode");
                iniStatus = false;
            }
            else if (rbBtnDetailLog.Checked && !DLMSInfo.IsInterfaceHDLC && SessionGlobalMeterCommunication.IsMeterConnected)
                iniStatus = PushSetupInfo.ReadPushSetup(ref WrapperObj);

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
                    txtBillFreq.Enabled = true;
                    break;

                case "Current Bill":
                    EnableProfile(txt_CB_DestIP, cb_CB_Frequency, txt_Random_CB);
                    break;

                case "Alert":
                    EnableProfile(txt_Alert_DestIP, null, null);
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
            txt_Random_Instant, txt_Random_Bill,
            txt_Random_CB, txt_Random_DE, txt_Random_LS, txt_Random_SR
        };

            foreach (var tb in textBoxes)
                tb.Enabled = false;

            foreach (var cb in comboBoxes)
                cb.Enabled = false;

            foreach (var tb in textBoxes_Randmsn)
                tb.Enabled = false;
            txtBillFreq.Enabled = false;
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
            txt_Random_Instant, txt_Random_Bill,
            txt_Random_CB, txt_Random_DE, txt_Random_LS, txt_Random_SR
            };

            foreach (var tb in textBoxes)
                tb.Enabled = true;

            foreach (var cb in comboBoxes)
                cb.Enabled = true;
            foreach (var tb in textBoxes_Randmsn)
                tb.Enabled = true;
            txtBillFreq.Enabled = true;
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
                //InitializeProfileControls();
                var controls = profileControls.Find(p => p.Name == profileName);
                string actionObis = controls.ActionScheduleClassObisAtt;
                string pushObis = controls.PushSetupClassObis;

                // Destination Address 
                string destAddrHex = "";
                if (DLMSInfo.IsInterfaceHDLC)
                    destAddrHex = SetGetFromMeter.GetDataFromObject(ref DLMSWriter, Convert.ToInt32(pushObis.Substring(0, 4), 16), parse.HexObisToDecObis(pushObis.Substring(4, 12)), 3).Trim();
                else
                    SetGetFromMeter.GetDataFromWrapperObject(ref WrapperObj, (ObjectType)Convert.ToInt32(pushObis.Substring(0, 4), 16), parse.HexObisToDecObis(pushObis.Substring(4, 12)), 3, out destAddrHex);
                if (DLMSInfo.IsInterfaceHDLC && destAddrHex == "0B")
                {
                    controls.txt_Dest_Add.Text = "Object Not Available";
                    controls.txt_Dest_Add.BackColor = Color.Red;
                }
                else if (!DLMSInfo.IsInterfaceHDLC && !string.IsNullOrEmpty(WrapperComm.errorMessage))
                {
                    controls.txt_Dest_Add.Text = WrapperComm.errorMessage;
                    controls.txt_Dest_Add.BackColor = Color.Red;
                    if (WrapperComm.errorMessage.Contains("Failed to receive reply from the device"))
                        return;
                }
                else if (destAddrHex.StartsWith("02"))
                {
                    string[] structArray = parse.GetStructureValueList(destAddrHex.Substring(4)).ToArray();
                    if (structArray.Length > 1)
                    {
                        controls.txt_Dest_Add.Text = parse.GetProfileValueString(structArray[1]);
                        controls.txt_Dest_Add.BackColor = Color.LightGreen;
                    }
                    else
                    {
                        controls.txt_Dest_Add.Text = "Invalid Structure";
                        controls.txt_Dest_Add.BackColor = Color.Red;
                    }
                }
                else
                {
                    controls.txt_Dest_Add.Text = "Unknown";
                    controls.txt_Dest_Add.BackColor = Color.Red;
                }

                // Push Frequency 
                if (controls.Name != "Alert" && controls.Name != "Tamper")
                {
                    string pushFreqHex = "";
                    if (DLMSInfo.IsInterfaceHDLC)
                        pushFreqHex = SetGetFromMeter.GetDataFromObject(ref DLMSWriter, Convert.ToInt32(actionObis.Substring(0, 4), 16), parse.HexObisToDecObis(actionObis.Substring(4, 12)), 4).Trim();
                    else
                        SetGetFromMeter.GetDataFromWrapperObject(ref WrapperObj, (ObjectType)Convert.ToInt32(actionObis.Substring(0, 4), 16), parse.HexObisToDecObis(actionObis.Substring(4, 12)), 4, out pushFreqHex);
                    if (DLMSInfo.IsInterfaceHDLC && pushFreqHex == "0B")
                    {
                        controls.cbFrequency.Text = "Object Not Available";
                        controls.cbFrequency.BackColor = Color.Red;
                    }
                    else if (!DLMSInfo.IsInterfaceHDLC && !string.IsNullOrEmpty(WrapperComm.errorMessage))
                    {
                        controls.cbFrequency.Text = WrapperComm.errorMessage;
                        controls.cbFrequency.BackColor = Color.Red;
                    }
                    else if (pushFreqHex.StartsWith("01"))
                    {
                        string freqValue = string.Empty;
                        if (controls.Name == "Bill")
                        {
                            freqValue = freqHexPatterns.FirstOrDefault(kvp => kvp.Value.Any(v => v.Equals(pushFreqHex, StringComparison.OrdinalIgnoreCase))).Key ?? "Custom";
                            if (freqValue == "Custom")
                            {
                                int hh = Convert.ToInt32(pushFreqHex.Substring(12, 2), 16);
                                int mm = Convert.ToInt32(pushFreqHex.Substring(14, 2), 16);
                                int ss = Convert.ToInt32(pushFreqHex.Substring(16, 2), 16);
                                int dd = Convert.ToInt32(pushFreqHex.Substring(30, 2), 16);
                                txtBillFreq.Text = $"{dd:00}/*/* {hh:00}:{mm:00}:{ss:00}";
                            }
                        }
                        else
                            freqValue = freqHexPatterns.FirstOrDefault(kvp => kvp.Value.Any(v => v.Equals(pushFreqHex, StringComparison.OrdinalIgnoreCase))).Key ?? "Unknown";
                        controls.cbFrequency.Text = freqValue;
                        controls.cbFrequency.BackColor = Color.LightGreen;
                    }
                    else
                    {
                        controls.cbFrequency.Text = "Unknown";
                        controls.cbFrequency.BackColor = Color.Red;
                    }
                }

                // Randomization Delay 
                if (controls.txtRandom != null)
                {
                    string randomizationHex = "";
                    if (DLMSInfo.IsInterfaceHDLC)
                        randomizationHex = SetGetFromMeter.GetDataFromObject(ref DLMSWriter, Convert.ToInt32(pushObis.Substring(0, 4), 16), parse.HexObisToDecObis(pushObis.Substring(4, 12)), 5).Trim();
                    else
                        SetGetFromMeter.GetDataFromWrapperObject(ref WrapperObj, (ObjectType)Convert.ToInt32(pushObis.Substring(0, 4), 16), parse.HexObisToDecObis(pushObis.Substring(4, 12)), 5, out randomizationHex);
                    if (DLMSInfo.IsInterfaceHDLC && randomizationHex == "0B")
                    {
                        controls.txtRandom.Text = "Object Not Available";
                        controls.txtRandom.BackColor = Color.Red;
                    }
                    else if (DLMSInfo.IsInterfaceHDLC && randomizationHex == "0D")
                    {
                        controls.txtRandom.Text = "No Access";
                        controls.txtRandom.BackColor = Color.Red;
                    }
                    else if (!DLMSInfo.IsInterfaceHDLC && !string.IsNullOrEmpty(WrapperComm.errorMessage))
                    {
                        controls.txtRandom.Text = WrapperComm.errorMessage;
                        controls.txtRandom.BackColor = Color.Red;
                    }
                    else
                    {
                        controls.txtRandom.Text = parse.GetProfileValueString(randomizationHex);
                        controls.txtRandom.BackColor = Color.LightGreen;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error in Get_PushFreq_Destination_Randomisation: " + ex.Message, ex);
                log.Error($"{ex.Message.ToString()} => TRACE: {ex.StackTrace.ToString()}");
            }
            finally
            {

            }
        }
        public void Set_PushFreq_Destination_Randomisation(ref DLMSComm DLMSWriter, string profileName)
        {
            try
            {
                //InitializeProfileControls();
                int nRetVal = 0; string sMessage = string.Empty;
                var profile = profileControls.Find(p => p.Name == profileName);
                string actionObis = profile.ActionScheduleClassObisAtt;
                string pushObis = profile.PushSetupClassObis;
                #region Destination Address 

                string destAddHex = $"{DLMSParser.ConvertAsciiToHex(profile.txt_Dest_Add.Text.ToString().Trim())}";
                string length = (destAddHex.Length / 2).ToString("X2");
                destAddHex = $"0203160009{length}{destAddHex}1600";
                if (DLMSInfo.IsInterfaceHDLC)
                    nRetVal = DLMSWriter.SetParameter($"{profile.PushSetupClassObis.Trim()}03", (byte)0, (byte)3, (byte)5, $"{destAddHex}");
                else
                    nRetVal = WrapperObj.SetObjectValue(Convert.ToInt32(profile.PushSetupClassObis.Trim().Substring(0, 4), 16), parse.HexObisToDecObis(profile.PushSetupClassObis.Trim().Substring(4, 12)), 3, destAddHex);
                if (nRetVal == 0)
                {
                    profile.txt_Dest_Add.BackColor = Color.LightGreen;
                    sMessage = sMessage + "Push Setup: Destination Address Set Successfully.";
                }
                else if (nRetVal == 2)
                {
                    profile.txt_Dest_Add.BackColor = Color.Red;
                    sMessage = sMessage + "Push Setup: Destination Address Action Denied.";
                }
                else if (DLMSInfo.IsInterfaceHDLC && nRetVal == 1 || nRetVal == 3)
                {
                    profile.txt_Dest_Add.BackColor = Color.Red;
                    sMessage = sMessage + "Push Setup: Destination Address Error in Setting.";
                }
                else if (!DLMSInfo.IsInterfaceHDLC && nRetVal == 1 || nRetVal == 3)
                {
                    profile.txt_Dest_Add.BackColor = Color.Red;
                    sMessage = sMessage + "Push Setup: Destination Address Error in Setting.";
                    if (WrapperComm.errorMessage.Contains("Failed to receive reply from the device"))
                        return;
                }
                else if (!DLMSInfo.IsInterfaceHDLC && !string.IsNullOrEmpty(WrapperComm.errorMessage))
                {
                    sMessage = sMessage + $" {WrapperComm.errorMessage}";
                    if (WrapperComm.errorMessage.Contains("Failed to receive reply from the device"))
                        return;
                }
                #endregion

                #region Push Frequency 
                string billDateTime = "0100";
                if (profile.Name == "Bill" && !string.IsNullOrEmpty(profile.cbFrequency.Text.ToString()))
                {
                    billDateTime = $"010102020904{int.Parse(txtBillFreq.Text.ToString().Substring(7, 2)):X2}" +
                                $"{int.Parse(txtBillFreq.Text.ToString().Substring(10, 2)):X2}" +
                                $"{int.Parse(txtBillFreq.Text.ToString().Substring(13, 2)):X2}FF0905FFFFFF" +
                                $"{int.Parse(txtBillFreq.Text.ToString().Substring(0, 2)):X2}FF"; //10/*/* 00:00:00
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
                    { "Custom", billDateTime}
                };
                //DLMSWriter.SignOnDLMS();//Why this extra sign ON added?
                if (!string.IsNullOrEmpty(profile.ActionScheduleClassObisAtt))
                {
                    if (DLMSInfo.IsInterfaceHDLC)
                        nRetVal = DLMSWriter.SetParameter($"{profile.ActionScheduleClassObisAtt}", (byte)0, (byte)3, (byte)3, freqHexString[profile.cbFrequency.SelectedItem.ToString().Trim()]);
                    else
                        nRetVal = WrapperObj.SetObjectValue(Convert.ToInt32(profile.ActionScheduleClassObisAtt.Trim().Substring(0, 4), 16), parse.HexObisToDecObis(profile.ActionScheduleClassObisAtt.Trim().Substring(4, 12)), Convert.ToInt32(profile.ActionScheduleClassObisAtt.Trim().Substring(16, 2), 16), freqHexString[profile.cbFrequency.SelectedItem.ToString().Trim()]);
                    if (nRetVal == 0)
                    {
                        profile.cbFrequency.BackColor = Color.LightGreen;
                        sMessage = sMessage + $"{profile} Push Frequency Set Successfully to {profile.cbFrequency.SelectedItem.ToString().Trim()}.";
                    }
                    else if (nRetVal == 2)
                    {
                        profile.cbFrequency.BackColor = Color.Red;
                        sMessage = sMessage + $"{profile} Push Frequency Action Denied.";
                    }
                    else if (nRetVal == 1 || nRetVal == 3)
                    {
                        profile.cbFrequency.BackColor = Color.Red;
                        sMessage = sMessage + $"{profile} Push Frequency Error in Setting.";
                    }
                    else if (!DLMSInfo.IsInterfaceHDLC && !string.IsNullOrEmpty(WrapperComm.errorMessage))
                        sMessage = sMessage + $" {WrapperComm.errorMessage}";
                }
                #endregion

                #region Randomization Delay 
                if (profile.Name != "Alert" && profile.Name != "SR")
                {
                    if (!string.IsNullOrEmpty(profile.txtRandom.Text.Trim()) && int.TryParse(profile.txtRandom.Text.Trim(), out int number))
                    {
                        if (DLMSInfo.IsInterfaceHDLC)
                            nRetVal = DLMSWriter.SetParameter($"{profile.PushSetupClassObis.Trim()}05", (byte)0, (byte)3, (byte)5, $"12{number:X4}");
                        else
                            nRetVal = WrapperObj.SetObjectValue(Convert.ToInt32(profile.PushSetupClassObis.Trim().Substring(0, 4), 16), parse.HexObisToDecObis(profile.PushSetupClassObis.Trim().Substring(4, 12)), 5, $"12{number:X4}");
                        if (nRetVal == 0)
                        {
                            profile.txtRandom.BackColor = Color.LightGreen;
                            sMessage = sMessage + "Push Setup: Randamisation Set Successfully.";
                        }
                        else if (nRetVal == 2)
                        {
                            profile.txtRandom.BackColor = Color.Red;
                            sMessage = sMessage + "Push Setup: Randamisation Action Denied.";
                        }
                        else if (nRetVal == 1 || nRetVal == 3)
                        {
                            profile.txtRandom.BackColor = Color.Red;
                            sMessage = sMessage + "Push Setup: Randamisation Error in Setting.";
                        }
                        else if (!DLMSInfo.IsInterfaceHDLC && !string.IsNullOrEmpty(WrapperComm.errorMessage))
                            sMessage = sMessage + $" {WrapperComm.errorMessage}";
                    }
                }
                #endregion

            }
            catch (Exception ex)
            {
                log.Error("Error in Set_PushFreq_Destination_Randomisation: " + ex.Message, ex);
                log.Error($"{ex.Message.ToString()} => TRACE: {ex.StackTrace.ToString()}");
            }
            finally
            {

            }
        }
        private void BuildDecryptPopup()
        {
            decryptPanel = new Panel
            {
                Size = new Size(280, 150),
                BackColor = Color.White,
                Visible = false,
                BorderStyle = BorderStyle.FixedSingle
            };
            Color primary = Color.FromArgb(0, 94, 168);
            Color accent = Color.FromArgb(245, 134, 52);

            Panel header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 31,
                BackColor = primary
            };

            Label headerText = new Label
            {
                Text = "Decrypt Settings",
                ForeColor = Color.White,
                Font = new Font("Microsoft Sans Serif", 10, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(12, 8)
            };

            Button btnClose = new Button
            {
                Text = "✕",
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Width = 28,
                Height = 22,
                Location = new Point(320, 6)
            };

            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, ev) => decryptPanel.Visible = false;

            header.Controls.Add(headerText);
            header.Controls.Add(btnClose);

            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(5, 12, 5, 12),
                ColumnCount = 2,
                RowCount = 4
            };

            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            Label lblEk = new Label { Text = "Encryption Key (Ek):" };
            txtEk = new TextBox() { Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular) };

            Label lblAk = new Label { Text = "Authentication Key (Ak):" };
            txtAk = new TextBox() { Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular) };

            Label lblTitle = new Label { Text = "System Title" };
            txtSystemTitle = new TextBox() { Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular) };

            Button btnOk = new Button
            {
                Text = "OK",
                BackColor = accent,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Width = 85,
                Font = new Font("Microsoft Sans Serif", 10, FontStyle.Bold)
            };
            btnOk.FlatAppearance.BorderSize = 2;

            btnOk.Click += (s, ev) =>
            {
                TCPTestNotifier.notify_EK = txtEk.Text.Trim();
                TCPTestNotifier.notify_AK = txtAk.Text.Trim();
                TCPTestNotifier.notify_SysT = txtSystemTitle.Text.Trim();
                TCPTestNotifier.useSeparateCredentials = true;
                decryptPanel.Visible = false;
            };

            layout.Controls.Add(lblEk, 0, 0);
            layout.Controls.Add(txtEk, 1, 0);

            layout.Controls.Add(lblAk, 0, 1);
            layout.Controls.Add(txtAk, 1, 1);
            layout.Controls.Add(lblTitle, 0, 2);
            layout.Controls.Add(txtSystemTitle, 1, 2);

            layout.Controls.Add(btnOk, 1, 3);
            decryptPanel.Controls.Add(layout);
            decryptPanel.Controls.Add(header);
            this.Controls.Add(decryptPanel);
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
                                StyleDataGrid(dgRawData);
                                dgRawData.Invalidate();
                                //dgRawData.Refresh();
                            }));
                        }
                        else
                        {
                            dgRawData.DataSource = null;
                            dgRawData.DataSource = receivedPushData;
                            StyleDataGrid(dgRawData);
                            dgRawData.Invalidate();
                            //dgRawData.Refresh();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AppendColoredTextControlNotifier($"Error in PushMonitorTimer: {ex.Message}", Color.Red, true);
                log.Error($"{ex.Message.ToString()} => TRACE: {ex.StackTrace.ToString()}");
            }
        }
        private void FinalDataTable_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            if (rbBtnUserDefinedLog.Checked)
                return;
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
                        StyleDataGrid(targetGrid);
                        targetGrid.Invalidate();
                        targetGrid.Refresh();
                    }));
                }
                else
                {
                    targetGrid.DataSource = null;
                    targetGrid.DataSource = targetTable;
                    //StyleDataGrid(targetGrid);
                    targetGrid.Invalidate();
                    targetGrid.Refresh();
                }
            }
            catch (Exception ex)
            {
                AppendColoredTextControlNotifier($"Error in {PushPacketManager.pushProfile} => {ex.Message}\n{ex.StackTrace}", Color.Red);
                log.Error($"{ex.Message.ToString()} => TRACE: {ex.StackTrace.ToString()}");
            }

        }
        public void OnPushDataReceived(string updatedText, Color color, bool isBold)
        {
            if (rtbPushLogs.InvokeRequired)
            {
                rtbPushLogs.BeginInvoke(new Action(() =>
                {
                    HandlePushLog(updatedText, color, isBold);
                }));
            }
            else
            {
                HandlePushLog(updatedText, color, isBold);
            }
        }
        private bool isRawDataVisible = false;
        private void FlushLogBuffer2(object sender, ElapsedEventArgs e)
        {
            if (IsDisposed) return;

            if (_logBuffer2.IsEmpty) return;

            try
            {
                rtbPushLogs.BeginInvoke(new Action(() =>
                {
                    if (rtbPushLogs.IsDisposed)
                        return;

                    // === Detect if user is at bottom ===
                    int visibleLines = rtbPushLogs.ClientSize.Height / rtbPushLogs.Font.Height;
                    int firstVisibleLine = SendMessage(rtbPushLogs.Handle, EM_GETFIRSTVISIBLELINE, 0, 0);
                    int lastVisibleLine = firstVisibleLine + visibleLines;
                    bool atBottom = (lastVisibleLine >= rtbPushLogs.GetLineFromCharIndex(rtbPushLogs.TextLength));

                    // === Batch dequeue ===
                    List<(string Message, Color Color, bool IsBold)> logsToAppend = new List<(string, Color, bool)>(_logBuffer2.Count);
                    while (_logBuffer2.TryDequeue(out var log))
                        logsToAppend.Add((log.Message, log.Color, log.IsBold));

                    if (logsToAppend.Count == 0)
                        return;

                    // === Suspend layout ===
                    rtbPushLogs.SuspendLayout();

                    // === Append with style (no bulk AppendText) ===
                    // Using SelectedText directly preserves style alignment
                    rtbPushLogs.SelectionStart = rtbPushLogs.TextLength;
                    rtbPushLogs.SelectionLength = 0;

                    foreach (var log in logsToAppend)
                    {
                        // 1. Apply formatting for the new text
                        rtbPushLogs.SelectionFont = log.IsBold ? BoldFont11 : RegularFont11;
                        rtbPushLogs.SelectionColor = log.Color;

                        // 2. Store index BEFORE adding message
                        int messageStart = rtbPushLogs.TextLength;

                        // 3. Append full message text
                        rtbPushLogs.AppendText(log.Message);

                        // 4. Link only the portion between "Device ID" and "Received"
                        string msg = log.Message;
                        int idxDeviceId = msg.IndexOf("Device ID", StringComparison.OrdinalIgnoreCase);
                        int idxReceived = msg.IndexOf("Received", StringComparison.OrdinalIgnoreCase);

                        if (idxDeviceId >= 0 && idxReceived > idxDeviceId)
                        {
                            int linkStart = messageStart + idxDeviceId;
                            int linkLength = (idxReceived + "Received".Length) - idxDeviceId;

                            rtbPushLogs.Select(linkStart, linkLength);
                            rtbPushLogs.SetSelectionLink(true);
                            rtbPushLogs.Select(rtbPushLogs.TextLength, 0);
                        }
                    }

                    // === Trim old lines ===
                    int totalLines = rtbPushLogs.GetLineFromCharIndex(rtbPushLogs.TextLength) + 1;
                    if (totalLines > MaxLines)
                    {
                        int cutOffIndex = rtbPushLogs.GetFirstCharIndexFromLine(TrimLines);
                        if (cutOffIndex > 0)
                        {
                            rtbPushLogs.Select(0, cutOffIndex);
                            rtbPushLogs.SelectedText = string.Empty;
                        }
                    }

                    // === Auto-scroll if needed ===
                    if (atBottom)
                    {
                        rtbPushLogs.SelectionStart = rtbPushLogs.TextLength;
                        rtbPushLogs.ScrollToCaret();
                    }

                    // === Resume layout ===
                    rtbPushLogs.ResumeLayout();
                }));
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message.ToString()} => TRACE: {ex.StackTrace.ToString()}");
            }

            /*
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
            */
        }
        private void TestLogService_AppendColoredTextControlNotifier(string message, Color color, bool isBold = false)
        {
            _logBuffer2.Enqueue((message + Environment.NewLine, color, isBold));
        }
        private void StyleDataGrid(DataGridView grid)
        {
            grid.AutoGenerateColumns = true;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            grid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //grid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.LightSteelBlue;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            grid.EnableHeadersVisualStyles = false;
            grid.BackgroundColor = Color.White;
            grid.GridColor = Color.LightGray;
            grid.SelectionMode = DataGridViewSelectionMode.CellSelect;
            grid.RowHeadersVisible = false;
            //grid.AllowUserToResizeRows = false;
            //grid.AllowUserToOrderColumns = false;
            grid.Font = new Font("Microsoft Sans Serif", 9);
            grid.ColumnHeadersHeight = 35;
            foreach (DataGridViewColumn col in grid.Columns)
            {
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            grid.Invalidate();
        }
        private void UISettings()
        {
            //splitCon_Alert.SplitterDistance = tbpAlert.Width / 4;
            //splitCon_Bill.SplitterDistance = tbpBill.Width / 4;
            //splitCon_CB.SplitterDistance = tbpCB.Width / 4;
            //splitCon_DE.SplitterDistance = tbpDE.Width / 4;
            //splitcon_InstantTab.SplitterDistance = tbpInstant.Width / 4;
            //splitCon_LS.SplitterDistance = tbpLS.Width / 4;
            //splitCon_SR.SplitterDistance = tbpSR.Width / 4;
            //splitCon_Tamper.SplitterDistance = tbpTamper.Width / 4;
            //splitConMain.SplitterDistance = tblMain.Width / 2;
            splitCon_Alert.SplitterDistance = (int)(this.Size.Height * 0.50);
            splitCon_Bill.SplitterDistance = (int)(this.Size.Height * 0.50);
            splitCon_CB.SplitterDistance = (int)(this.Size.Height * 0.50);
            splitCon_DE.SplitterDistance = (int)(this.Size.Height * 0.50);
            splitcon_InstantTab.SplitterDistance = (int)(this.Size.Height * 0.50);
            splitCon_LS.SplitterDistance = (int)(this.Size.Height * 0.50);
            splitCon_SR.SplitterDistance = (int)(this.Size.Height * 0.50);
            splitCon_Tamper.SplitterDistance = (int)(this.Size.Height * 0.50);
            splitConMain.SplitterDistance = (int)(this.Size.Width * 0.45);
            //splitConMain.Invalidate();
            //splitConMain.Refresh();
        }
        private void HandlePushLog(string updatedText, Color color, bool isBold = false)
        {
            AppendColoredTextControlNotifier(updatedText, color, isBold);
            //if (updatedText.Contains("Device ID") && updatedText.Contains("Received"))
            //{
            //    int start = rtbPushLogs.Text.LastIndexOf(updatedText);
            //    if (start >= 0)
            //    {
            //        updatedText = updatedText.Trim();
            //        rtbPushLogs.Select(start, updatedText.Length);
            //        rtbPushLogs.SetSelectionLink(true);
            //        rtbPushLogs.Select(rtbPushLogs.TextLength, 0);
            //    }
            //}
            //rtbPushLogs.ScrollToCaret();
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
        private void ClearDatagrids()
        {
            dgRawData.DataSource = null;
            dgAlert.DataSource = null;
            dgDE.DataSource = null;
            dgLS.DataSource = null;
            dgCB.DataSource = null;
            dgBill.DataSource = null;
            dgInstant.DataSource = null;
            dgSR.DataSource = null;
            dgTamper.DataSource = null;
        }

        #region Push Profile Object Viewer
        private void AddNodesofPushObjects()
        {
            // Clear the TreeView first, if needed
            tvPushObjects.Nodes.Clear();
            // Add the root nodes to the TreeView
            tvPushObjects.Nodes.Add(new TreeNode("Instant - 0.0.25.9.0.255"));
            tvPushObjects.Nodes.Add(new TreeNode("Alert - 0.4.25.9.0.255"));
            tvPushObjects.Nodes.Add(new TreeNode("Bill - 0.132.25.9.0.255"));
            tvPushObjects.Nodes.Add(new TreeNode("Self Registration - 0.130.25.9.0.255"));
            tvPushObjects.Nodes.Add(new TreeNode("Daily Energy - 0.6.25.9.0.255"));
            tvPushObjects.Nodes.Add(new TreeNode("Load Survey - 0.5.25.9.0.255"));
            tvPushObjects.Nodes.Add(new TreeNode("Tamper - 0.134.25.9.0.255"));
            tvPushObjects.Nodes.Add(new TreeNode("Current Bill - 0.0.25.9.129.255"));
        }
        private void tabControlProfiles_DrawItem(object sender, DrawItemEventArgs e)
        {
            #region OPTION 4 Rounded Tab Style (Simulated Rounded Look)

            TabControl tabControl = sender as TabControl;
            tabControl.SuspendLayout();
            bool isSelected = e.Index == tabControl.SelectedIndex;

            System.Drawing.Rectangle rect = e.Bounds;
            rect.Inflate(-2, -4); // simulate padding

            Color fillColor = isSelected ? Color.FromArgb(0, 94, 168) : Color.FromArgb(245, 134, 52);
            Color textColor = isSelected ? Color.White : Color.Black;

            SolidBrush fillBrush = new SolidBrush(fillColor);
            SolidBrush textBrush = new SolidBrush(textColor);

            GraphicsPath path = new GraphicsPath();
            Font tabFont = new Font("Segoe UI", 9F, FontStyle.Bold);

            // Simulated rounded rectangle
            path.AddArc(rect.Left, rect.Top, 12, 12, 180, 90);
            path.AddArc(rect.Right - 12, rect.Top, 12, 12, 270, 90);
            path.AddLine(rect.Right, rect.Bottom, rect.Left, rect.Bottom);
            path.CloseFigure();

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.FillPath(fillBrush, path);
            //e.Graphics.FillRectangle(gradientBrush, rect);
            StringFormat sf = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            e.Graphics.DrawString(tabControl.TabPages[e.Index].Text, tabFont, textBrush, rect, sf);
            tabControl.ResumeLayout();
            #endregion

        }
        private void tvPushObjects_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                DGPushProfile.DataSource = null;
                switch (e.Node.Text.Split('-')[1].Trim())
                {
                    //Instant
                    case "0.0.25.9.0.255":
                        //DGPushProfile.DataSource = PushSetupInfo.InstantDT;
                        IsPushProfileAvailable(PushSetupInfo.InstantDT);
                        break;
                    //Alert
                    case "0.4.25.9.0.255":
                        //DGPushProfile.DataSource = PushSetupInfo.AlertDT;
                        IsPushProfileAvailable(PushSetupInfo.AlertDT);
                        break;
                    //Bill - 0.132.25.9.0.255
                    case "0.132.25.9.0.255":
                        //DGPushProfile.DataSource = PushSetupInfo.BillDT;
                        IsPushProfileAvailable(PushSetupInfo.BillDT);
                        break;
                    //Self Registration
                    case "0.130.25.9.0.255":
                        //DGPushProfile.DataSource = PushSetupInfo.SelfRegDT;
                        IsPushProfileAvailable(PushSetupInfo.SelfRegDT);
                        break;
                    //Daily Energy
                    case "0.6.25.9.0.255":
                        //DGPushProfile.DataSource = PushSetupInfo.DEDT;
                        IsPushProfileAvailable(PushSetupInfo.DEDT);
                        break;
                    //Load Survey - 0.5.25.9.0.255
                    case "0.5.25.9.0.255":
                        //DGPushProfile.DataSource = PushSetupInfo.LSDT;
                        IsPushProfileAvailable(PushSetupInfo.LSDT);
                        break;
                    //Tamper
                    case "0.134.25.9.0.255":
                        //DGPushProfile.DataSource = PushSetupInfo.TamperDT;
                        IsPushProfileAvailable(PushSetupInfo.TamperDT);
                        break;
                    //Current Bill
                    case "0.0.25.9.129.255":

                        break;
                }
                if (DGPushProfile != null && DGPushProfile.Rows.Count > 1)
                {
                    // Change the font size and style for the column headers
                    DGPushProfile.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9, FontStyle.Bold);
                    // Change the background color for the column headers
                    DGPushProfile.ColumnHeadersDefaultCellStyle.BackColor = Color.Lavender;
                    // Make sure the style changes are visible by enabling visual styles
                    DGPushProfile.EnableHeadersVisualStyles = false;
                    DGPushProfile.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9, FontStyle.Regular);
                    DGPushProfile.ScrollBars = ScrollBars.Both;
                }
                if (DGPushProfile.Columns.Count > 0 && DGPushProfile.Rows.Count > 0)
                {
                    if (DGPushProfile.Columns.Count > 7)
                    {
                        // Hide columns beyond first 7
                        for (int i = 7; i < DGPushProfile.Columns.Count; i++)
                        {
                            DGPushProfile.Columns[i].Visible = false;
                        }
                    }
                    foreach (DataGridViewColumn column in DGPushProfile.Columns)
                    {
                        column.Frozen = false;
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                        if (column.Index == 0)
                            column.Width = 40;
                        if (column.Index == 1 || column.Index == 6)
                        {
                            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        }
                        if (column.Index > 3)
                            column.Width = 150;
                        if (column.Index == 6)
                            column.Width = 200;
                        column.SortMode = DataGridViewColumnSortMode.NotSortable;
                    }
                }
                DGPushProfile.Invalidate();
                //DGPushProfile.Refresh();
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message.ToString()} => TRACE: {ex.StackTrace.ToString()}");
            }
        }
        private void IsPushProfileAvailable(DataTable table)
        {
            if (table.Rows.Count < 1)
            {
                DialogResult result = MessageBox.Show("Do you want to read Push Setup Objects?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    if (!DLMSInfo.IsInterfaceHDLC && !SessionGlobalMeterCommunication.IsMeterConnected)
                    {
                        CommonHelper.DisplayErrorMessage("Error", "Connect the meter in Dashboard in WRAPPER Mode");
                        return;
                    }
                    if (DLMSInfo.IsInterfaceHDLC)
                        PushSetupInfo.ReadPushSetup(TestConfiguration.CreateDefault());
                    else if (!DLMSInfo.IsInterfaceHDLC && !SessionGlobalMeterCommunication.IsMeterConnected)
                    {
                        CommonHelper.DisplayErrorMessage("Error", "Connect the meter in Dashboard in WRAPPER Mode");
                    }
                    else if (!DLMSInfo.IsInterfaceHDLC && SessionGlobalMeterCommunication.IsMeterConnected)
                        PushSetupInfo.ReadPushSetup(ref WrapperObj);
                    DGPushProfile.DataSource = table;
                }
                else
                {
                    return;
                }
            }
            else
            {
                DGPushProfile.DataSource = table;
            }
        }
        #endregion

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
            box.SelectionColor = Color.Green;
            CHARFORMAT2_STRUCT cf = new CHARFORMAT2_STRUCT();
            cf.cbSize = (uint)System.Runtime.InteropServices.Marshal.SizeOf(cf);
            cf.dwMask = CFM_LINK;
            cf.dwEffects = link ? (uint)CFE_LINK : 0;
            cf.crTextColor = Color.Red.ToArgb();
            SendMessage(box.Handle, EM_SETCHARFORMAT, SCF_SELECTION, ref cf);
        }
    }
}
