using DevExpress.Internal.WinApi.Windows.UI.Notifications;
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

        #endregion

        private TestLogService logService;
        private TestConfiguration config;
        private TestStopWatch stopWatch;
        private TCPTestNotifier tcpTestNotifier;
        PushPacketManager pushPacketManager = new PushPacketManager();
        private System.Timers.Timer pushMonitorTimer;
        RichTextBox logBox = new RichTextBox();
        private DataTable receivedPushData = new DataTable();
        public DataTable finalDataTable = new DataTable();
        private int lastPushRowCount = 0;
        private readonly ConcurrentQueue<(string Message, Color Color, bool IsBold)> _logBuffer2 = new ConcurrentQueue<(string, Color, bool)>();

        public ListenerForm()
        {
            InitializeComponent();
            InitializeLoggerAndConfigurations();
            PushPacketManager.DeviceID = "GOE12043714";
            finalDataTable.RowChanged += FinalDataTable_RowChanged;
        }
        private void InitializeLoggerAndConfigurations()
        {
            logService = new TestLogService(rtbPushLogs);
            config = TestConfiguration.CreateDefault();
        }
        private void btnStartListener_Click(object sender, EventArgs e)
        {
            TestLogService.AppendColoredTextControlEventHandler += TestLogService_AppendColoredTextControlEventHandler;
            logService = new TestLogService(rtbPushLogs);
            logBox = rtbPushLogs;
            DLMSComm dlmsReader = new DLMSComm(DLMSInfo.comPort, DLMSInfo.BaudRate);
            if (!dlmsReader.SignOnDLMS())
            {
                log.Error("Sign ON Failure");
                logService.LogMessage(rtbPushLogs, "Sign ON Failure", Color.Red, true);
            }
            logService.LogMessage(rtbPushLogs, "Meter Sign On Successful", Color.Green);
            //rtbPushLogs.AppendText("Meter Sign On: Successful");
        }
        private void btnStopListener_Click(object sender, EventArgs e)
        {

        }
        private void btnClearLogs_Click(object sender, EventArgs e)
        {
            rtbPushLogs.Clear();
        }
        private void btnSaveData_Click(object sender, EventArgs e)
        {
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
        private void TestLogService_AppendColoredTextControlEventHandler(string message, Color color, bool isBold = false)
        {
            _logBuffer2.Enqueue((message + Environment.NewLine, color, isBold));
        }
    }
}
