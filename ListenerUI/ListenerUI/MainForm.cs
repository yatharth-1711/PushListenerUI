﻿using AutoTestDesktopWFA;
using MeterComm.DLMS;
using MeterReader.DLMSNetSerialCommunication;
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
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace ListenerUI
{
    public partial class MainForm : Form
    {
        public static CancellationTokenSource _cancellationToken;
        public MainForm()
        {
            InitializeComponent();
            AddListenerFormToTabPage();
            cbAccessLevel.SelectedIndex = 2;
        }
        private void AddListenerFormToTabPage()
        {
            if (tabPageWirelessComm != null)
            {
                ListenerForm listenerForm = new ListenerForm();
                listenerForm.TopLevel = false;
                listenerForm.FormBorderStyle = FormBorderStyle.None;
                listenerForm.Dock = DockStyle.Fill;

                tabPageWirelessComm.Controls.Clear();
                tabPageWirelessComm.Controls.Add(listenerForm);
                listenerForm.Show();
            }
            else
            {
                MessageBox.Show("tabPageLCDConfig not found in tabControl2.");
            }
        }
        private async void btnSaveData_Click(object sender, EventArgs e)
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
            //TestConfigLoad(config);
            //try
            //{
            //    await System.Threading.Tasks.Task.Run(() =>
            //    {
            //        this.Invoke(new Action(() =>
            //        {
            //            IniTestRun(config);
            //        }));
            //    });
            //}
            //catch
            //{
            //    return;
            //}
            MessageBox.Show("Meter Settings Saved Succesfully");

        }
        private void TestConfigLoad(TestConfiguration config)
        {
            config.comPort = cbMeterComPort.SelectedItem.ToString();
            config.BaudRate = Convert.ToInt32(cbMeterBaudRate.SelectedItem);
            config.AccessMode = cbAccessLevel.SelectedIndex;
            config.MeterAuthPassword = txtAuthPassword.Text.Trim();
            config.MeterAuthPasswordWrite = txtAuthPasswordWrite.Text.Trim();

            config.FWMeterAuthPasswordWrite = txtAuthPasswordFW.Text.Trim();
            config.AddressModeText = "One Byte";
            config.LogicalAddress = "1";
            config.PhysicalAddress = "256";
            config.WSTx = "1";
            config.WSRx = "1";
            config.IFTx = "Default";
            config.IFRx = "Default";
            config.IsLNWithCipher = checkBox_LN.Checked;
            config.IsWithGMAC = checkBox_GMAC.Checked;
            config.IsLNWithCipherDedicatedKey = checkBox_Dedicated.Checked;
            config.IsWithInvocationCounter = checkBox_InvocationCounter.Checked;
            config.TxtEK = txtEK.Text.Trim();
            config.TxtAK = txtAK.Text.Trim();
            config.TxtSysT = txtSysT.Text.Trim();
            config.MasterKey = txtMasterKey.Text.Trim();
            if (chkConformanceBlock.Checked)
                config.ConformanceBlock = txtConfBlock.Text.Trim();
            else
                config.ConformanceBlock = "62FEDF";
            config.InactivityTimeout = long.Parse(txtInactivityTimeout.Text.Trim());
            config.InterFrameTimeout = long.Parse(txtInterFrameTimeout.Text.Trim());
            config.ResponseTimeout = long.Parse(txtResponseTimeout.Text.Trim());
            config.DISCToNDMTimeout = long.Parse(txtDISCToNDMTimeout.Text.Trim());

            config.clientAddress = 48;
            config.serverAddress = 1;
            //config.ModuleType = cbModuleType.SelectedItem.ToString();
        }
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
            //ProfileGenericInfo.FillTables(); //By YS
            return iniStatus;
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
