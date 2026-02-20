using AutoTestDesktopWFA;
using MeterComm.DLMS;
using MeterReader.DLMSNetSerialCommunication;
//using MeterReader.HelperForms;
using MeterReader.TestHelperClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ListenerUI
{
    public partial class MainForm : Form
    {
        public static CancellationTokenSource _cancellationToken;
        public MainForm()
        {
            InitializeComponent();

            cbAccessLevel.SelectedIndex = 2;
            cbMeterBaudRate.SelectedIndex = 0;
        }


        private void btnSaveData_Click(object sender, EventArgs e)
        {
            TestConfiguration config = new TestConfiguration();
            config = TestConfiguration.CreateDefault();
            DLMSInfo.WSRx = "1";
            DLMSInfo.WSTx = "1";
            DLMSInfo.IFRx = "Default";
            DLMSInfo.IFTx = "Default";
            DLMSInfo.TxtEK = txtEK.Text.Trim();
            DLMSInfo.TxtAK = txtAK.Text.Trim();
            DLMSInfo.AccessMode = cbAccessLevel.SelectedIndex;
            DLMSInfo.MeterAuthPasswordWrite = txtAuthPasswordWrite.Text.Trim();
            DLMSInfo.MeterAuthPassword = txtAuthPassword.Text.Trim();
            config.comPort = DLMSInfo.comPort = cbMeterComPort.SelectedItem.ToString();
            DLMSInfo.BaudRate = Convert.ToInt32(cbMeterBaudRate.SelectedItem.ToString());


        }

        private void cbMeterComPort_MouseClick(object sender, MouseEventArgs e)
        {
            RefreshComPorts();
        }
        private void RefreshComPorts()
        {
            string[] ports = SerialPort.GetPortNames();
            cbMeterComPort.Items.Clear();
            cbMeterComPort.Items.AddRange(ports);
        }
    }
}
