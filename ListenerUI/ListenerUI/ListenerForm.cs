using log4net.Util;
using MeterReader.CommonClasses;
using MeterReader.DLMSNetSerialCommunication;
using MeterReader.TestHelperClasses;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ListenerUI
{
    public partial class ListenerForm : Form
    {
        private readonly TestLogService _logService = new TestLogService();
        private readonly TestConfiguration _testConfiguration;
        private TCPTestNotifier tcpTestNotifier;
        PushPacketManager pushPacketManager = new PushPacketManager();
        private readonly string[] profileNames =
        {
            "Instant", "Load Survey", "Daily Energy",
            "Self Registration", "Billing", "Current Bill", "Event Log"
        };
        public ListenerForm()
        {
            InitializeComponent();
        }

        // Append log with timestamp + color
        public void AppendLog(string message, Color color)
        {
            rtbPushLogs.ScrollToCaret();
        }
        private void btnStartListener_Click(object sender, EventArgs e)
        {
            if (tcpTestNotifier == null)
            {
                tcpTestNotifier = new TCPTestNotifier();
                pushPacketManager.InitializePushProfileTables();
                TCPTestNotifier.LogControlEventHandler += OnPushDataReceived;
                tcpTestNotifier.Connect(rtbPushLogs);
                _logService.LogMessage(rtbPushLogs, $"\n-----------------------------*** Listener Port Connected at {DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt")} ***----------------------------", Color.DeepPink, true);
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
                _logService.LogMessage(rtbPushLogs, $"\n----------------------------*** Listener Port Disconnected at {DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt")} ***--------------------------", Color.DeepPink, true);
            }
            AppendLog("Listener stopped.", Color.Red);
        }
        private void btnClearLogs_Click(object sender, EventArgs e)
        {
            rtbPushLogs.Clear();
        }
        private void btnSaveData_Click(object sender, EventArgs e)
        {
            AppendLog("Data saved successfully.", Color.LightBlue);
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
                    _logService.LogMessage(rtbPushLogs, $"Error in {PushPacketManager.pushProfile} => {ex.Message}\n{ex.StackTrace}", Color.Red);
                }
            }
        }
        public void OnPushDataReceived(string updatedText, Color color)
        {
            if (rtbPushLogs.InvokeRequired)
            {
                rtbPushLogs.BeginInvoke(new Action(() =>
                {
                    _logService.LogMessage(rtbPushLogs, updatedText, color, true);
                    rtbPushLogs.ScrollToCaret();
                }));
            }
            else
            {
                _logService.LogMessage(rtbPushLogs, updatedText, color, true);
                rtbPushLogs.ScrollToCaret();
            }
        }

        private void linkLabelPushprofileSettings_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
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
    }
}
