using AutoTest.FrameWork.Converts;
using AutoTestDesktopWFA;
using Gurux.DLMS;
using Gurux.DLMS.Enums;
using Gurux.DLMS.Secure;
using Indali.Common;
using Indali.Security.Enum;
using log4net;
using MeterComm.DLMS;
using meterReader.AesGcmParameter;
using MeterReader.CommonClasses;
using MeterReader.DLMSNetSerialCommunication;
using MeterReader.TestHelperClasses;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Reflection.Metadata;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace MeterReader.HelperForms
{
    public partial class PushPacketDecrypterFrm : Form
    {
        #region Global Variables and Handlers
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public GXDLMSSecureClient client = new GXDLMSSecureClient();
        public string LOG_DIRECTORY { get; set; } = @"C:\IndaliDlmsTestLogs";
        public delegate void AppendColoredTextControlWithBox(string message, Color color, bool isBold = false);
        public event AppendColoredTextControlWithBox AppendColoredTextControlNotifier = delegate { }; // add empty delegate!;
        private readonly ConcurrentQueue<(string Message, Color Color, bool IsBold)> _logBuffer2 = new ConcurrentQueue<(string, Color, bool)>();
        public byte[] EK;
        public byte[] AK;
        public byte[] ST;
        public Gurux.DLMS.Enums.Security GXSecurity;
        public byte[] cipherText;
        public uint IC = 0;
        int instantCount = 0;
        int lsCount = 0;
        int deCount = 0;
        int billCount = 0;
        int srCount = 0;
        int cbCount = 0;
        int alertCount = 0;
        int tamperCount = 0;
        DLMSParser parse = new DLMSParser();
        public List<string> input_Break_Final = new List<string>();
        public List<string> input_Break_Initial = new List<string>();
        public List<string> input_Break_Profile = new List<string>();
        bool _packetsLoaded = false;
        DataTable dtDecryptedPacketDetail = new DataTable();
        DataTable dtDecryptedSummary = new DataTable();
        private string versionName = "Push Packet Decrypter v2.0";
        #endregion
        public PushPacketDecrypterFrm()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.KeyDown += Form_KeyDown;
            this.AppendColoredTextControlNotifier += TestLogService_AppendColoredTextControlNotifier;

        }
        private void PushPacketDecrypter_Load(object sender, EventArgs e)
        {
            this.Text = versionName;
            StyleControl(dgPacketsSummary);
            StyleControl(dgPacketsDetail);
            txtSystemTitle.Text = DLMSInfo.TxtSysT;
            //txtSystemTitle.Text = "GOE00000";
            txtBlockCipherKey.Text = DLMSInfo.TxtEK;
            //txtBlockCipherKey.Text = "RsEbEkAkgjV97abc";
            txtAuthenticationKey.Text = DLMSInfo.TxtAK;
            //txtAuthenticationKey.Text = "RsEbEkAkgjV97abc";
            pnlSummarizedPacketTable.Width = (int)(this.Size.Width * 0.6);
            dtDecryptedSummary.Columns.Add("Timestamp", typeof(string));
            dtDecryptedSummary.Columns.Add("Type", typeof(string));
            dtDecryptedSummary.Columns.Add("Packet Length", typeof(string));
            dtDecryptedSummary.Columns.Add("Device ID", typeof(string));
            dtDecryptedSummary.Columns.Add("Status", typeof(string));
            dtDecryptedSummary.Columns.Add("Decrypted Data", typeof(string));
            btnAlert.Tag = "Alert";
            btnBill.Tag = "Billing";
            btnDE.Tag = "Daily Energy";
            btnLS.Tag = "Load Survey";
            btnCB.Tag = "Current Bill";
            btnSR.Tag = "Self Registration";
            btnInstant.Tag = "Instant";
            btnTamper.Tag = "Tamper";
            btnTotalCount.Tag = "Total";
        }
        private void btnImportFile_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Title = "Select packet file";
                    ofd.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                    ofd.Multiselect = false;

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        string fileContent = File.ReadAllText(ofd.FileName);
                        txtCipherPacket.Clear();
                        txtCipherPacket.Text = fileContent;
                        lblFilePath.Text = $" Showing packets from {ofd.FileName}";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to import file.\n\nDetails: {ex.Message}",
                    "Import Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            btnDecrypt.Enabled = false;
            toolStripProgressBar.Visible = true;
            toolStripProgressBar.Minimum = 0;
            toolStripProgressBar.Value = 0;
            toolStripStatusLabel.Text = string.Empty;
            toolStripStatusLabel.ForeColor = System.Drawing.Color.FromArgb(0, 94, 168);
            if (chkIsDecryptedPacket.Checked)
            {
                ResetLBLPacketCount();
                try
                {
                    string rawText = txtCipherPacket.Text;
                    string[] lines = rawText.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    //foreach (string line in lines)
                    StringBuilder multiBlock = new StringBuilder();
                    string currentPacket = null;
                    List<string> packets = new List<string>();
                    toolStripProgressBar.Maximum = lines.Length;
                    for (int i = 0; i < lines.Length; i++)
                    {
                        string normalized = lines[i].Trim().Replace(" ", "").ToUpper();

                        if (normalized.StartsWith("0F800000"))
                        {
                            if (!string.IsNullOrEmpty(currentPacket))
                            {
                                packets.Add(currentPacket);
                            }
                            currentPacket = normalized;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(currentPacket))
                            {
                                currentPacket += normalized;
                            }
                        }
                        toolStripProgressBar.Value++;
                        Application.DoEvents();
                        if (i % 2 == 0)
                            toolStripStatusLabel.Text = $"Analyzing Packets..";
                        else
                            toolStripStatusLabel.Text = $"🔍 Analyzing packets…";
                    }
                    //for last packet
                    if (!string.IsNullOrEmpty(currentPacket))
                    {
                        packets.Add(currentPacket);
                    }
                    toolStripProgressBar.Value = 0;
                    toolStripProgressBar.Maximum = packets.Count;
                    foreach (var item in packets)
                    {
                        try
                        {
                            string DecryptedData = item;
                            string decodedXml = null;
                            string typeOfData = string.Empty;

                            TCPTestNotifier tcp = new TCPTestNotifier();
                            List<byte> receivedBytes = new List<byte>();

                            string cleaned = new string(item.Where(c => !char.IsWhiteSpace(c)).ToArray());

                            for (int i = 0; i < cleaned.Length; i += 2)
                                receivedBytes.Add(Convert.ToByte(cleaned.Substring(i, 2), 16));

                            decodedXml = tcp.DecodePushDataToXML(receivedBytes.ToArray(), receivedBytes.Count);

                            // PUSH TYPE
                            if (DecryptedData.Contains("0000190900FF")) typeOfData = "Instant Push";
                            else if (DecryptedData.Contains("0004190900FF")) typeOfData = "Alert Push";
                            else if (DecryptedData.Contains("0005190900FF")) typeOfData = "Load Survey Push";
                            else if (DecryptedData.Contains("0006190900FF")) typeOfData = "Daily Energy Push";
                            else if (DecryptedData.Contains("0082190900FF")) typeOfData = "Self Registration Push";
                            else if (DecryptedData.Contains("0084190900FF")) typeOfData = "Billing Push";
                            else if (DecryptedData.Contains("0086190900FF")) typeOfData = "Tamper Push";
                            else if (DecryptedData.Contains("0000190981FF")) typeOfData = "Current Bill Push";
                            else typeOfData = "Unknown Push Type";

                            string sData = DecryptedData.Replace(" ", "");

                            int counter = 10;
                            DateTime packetSendingTime = DateTime.ParseExact(
                                parse.Getdate(sData.Substring(12, 24), 0, false),
                                "dd/MM/yyyy HH:mm:ss",
                                CultureInfo.InvariantCulture
                            );

                            counter = 36;
                            while (sData.Substring(counter, 2) != "02")
                                counter += 4;

                            counter += 4;
                            int length = Convert.ToInt32(sData.Substring(counter + 2, 2), 16) * 2;

                            string deviceId = parse.GetProfileValueString(
                                sData.Substring(counter, length + 4)
                            );

                            GetPacketCounter(typeOfData);
                            Color pushColor = GetPushColor(typeOfData);

                            AppendColoredText(txtPlainResult, $"\n{packetSendingTime:dd/MM/yyyy hh:mm:ss tt} - {typeOfData}\n", pushColor, true);
                            AppendColoredText(txtPlainResult, DecryptedData + "\n", pushColor, false);
                            AppendColoredText(txtXmlResult, $"\n{packetSendingTime:dd/MM/yyyy hh:mm:ss tt} - {typeOfData}\n", pushColor, true);
                            AppendColoredText(txtXmlResult, decodedXml + "\n", pushColor, false);

                            dtDecryptedSummary.Rows.Add(packetSendingTime.ToString("dd/MM/yyyy hh:mm:ss tt"), typeOfData, item.Length.ToString(), deviceId, "Success", DecryptedData);
                        }
                        catch (Exception ex)
                        {
                            dtDecryptedSummary.Rows.Add(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"), "Unknown", item.Length.ToString(), "", "Failed", ex.Message);
                            AppendColoredText(txtPlainResult, $"\n[ERROR] {ex.Message}\n", Color.Red, true);
                        }
                        finally
                        {
                            toolStripProgressBar.Value++;
                            Application.DoEvents();
                            toolStripStatusLabel.Text = $"🔐 Decrypting packets… {toolStripProgressBar.Value}/{toolStripProgressBar.Maximum}";
                        }
                    }
                    dgPacketsSummary.DataSource = dtDecryptedSummary;
                    //Decrypted Packets Detail
                    toolStripProgressBar.Value = 0;
                    toolStripProgressBar.Maximum = dtDecryptedSummary.Rows.Count;
                    for (int i = 0; i < dtDecryptedSummary.Rows.Count; i++)
                    {
                        try
                        {
                            DataTable rawPushPacketTable = new DataTable();
                            rawPushPacketTable.Columns.Add($"Push Data", typeof(string));
                            if (dtDecryptedSummary.Rows[i]["Status"].ToString() != "Success")
                            {
                                rawPushPacketTable.Rows.Add("Corrupted Packet Data");
                                while (dtDecryptedPacketDetail.Rows.Count <= i)
                                {
                                    dtDecryptedPacketDetail.Rows.Add();
                                }
                                dtDecryptedPacketDetail.Rows[i][0] = "Corrupted Packet Data";
                                continue;
                            }
                            List<string> details = new List<string>();
                            int totalProfileParameters = 0;
                            string input = dtDecryptedSummary.Rows[i]["Decrypted Data"].ToString().Replace(" ", "");
                            int pointer = 0;
                            input_Break_Initial.Clear();
                            input_Break_Profile.Clear();
                            input_Break_Initial.Add(input.Substring(pointer, 2));
                            pointer += 2;
                            input_Break_Initial.Add(input.Substring(pointer, 8));
                            pointer += 8;
                            if (input.Substring(pointer, 2) == "0C")
                            {
                                input_Break_Initial.Add(input.Substring(pointer, 26));
                                pointer += 26;
                            }
                            if (input.Length < pointer + 4)
                                return;
                            string profileString = input.Substring(pointer);
                            if (profileString == "0200" || profileString.Length < 4 || input.Substring(pointer, 2) != "02")
                            {
                                pointer += 4;
                                if (profileString == "0200" || profileString.Length < 4 || input.Substring(pointer, 2) != "02")
                                {
                                    return;
                                }
                            }
                            if (input.Substring(pointer, 2) == "02")
                            {
                                pointer += 2;
                                totalProfileParameters = Convert.ToInt32(input.Substring(pointer, 2), 16);
                                pointer += 2;
                                for (int j = 0; j < totalProfileParameters; j++)
                                {
                                    string value = parse.GetProfileDataString(input, ref pointer);
                                    if (!string.IsNullOrEmpty(value))
                                    {
                                        input_Break_Initial.Add(value);
                                    }
                                }

                            }
                            string lastValue = input_Break_Initial.Last().Trim();
                            if ((lastValue.StartsWith("02") || lastValue.StartsWith("01")) && input_Break_Initial.Count > 0)
                            {
                                int ptr = 4;
                                string packet = dtDecryptedSummary.Rows[i]["Type"].ToString();
                                if (packet == "Load Survey Push")
                                {
                                    totalProfileParameters = Convert.ToInt32(lastValue.Substring(ptr + 2, 2), 16);
                                    while (lastValue.Length > ptr)
                                    {
                                        ptr += 4;
                                        for (int j = 0; j < totalProfileParameters; j++)
                                        {
                                            string innerValue = parse.GetProfileDataString(lastValue, ref ptr);
                                            if (!string.IsNullOrEmpty(innerValue))
                                                input_Break_Profile.Add(innerValue);
                                        }
                                        input_Break_Final = input_Break_Initial.GetRange(0, input_Break_Initial.Count - 1);
                                        input_Break_Final.AddRange(input_Break_Profile);
                                    }
                                }
                                else
                                {
                                    if (lastValue.Length > ptr)
                                    {
                                        if (lastValue.Substring(6, 2) == "81")
                                        {
                                            lastValue = lastValue.Replace("\n", "");
                                            totalProfileParameters = Convert.ToInt32(lastValue.Substring(ptr + 4, 2), 16);
                                            ptr += 6;
                                            for (int j = 0; j < totalProfileParameters; j++)
                                            {
                                                string innerValue = parse.GetProfileDataString(lastValue, ref ptr);
                                                if (!string.IsNullOrEmpty(innerValue))
                                                    input_Break_Profile.Add(innerValue);
                                            }
                                        }
                                        else if (lastValue.Substring(6, 2) == "82")
                                        {
                                            lastValue = lastValue.Replace("\n", "");
                                            totalProfileParameters = Convert.ToInt32(lastValue.Substring(ptr + 4, 4), 16);
                                            ptr += 8;
                                            for (int j = 0; j < totalProfileParameters; j++)
                                            {
                                                string innerValue = parse.GetProfileDataString(lastValue, ref ptr);
                                                if (!string.IsNullOrEmpty(innerValue))
                                                    input_Break_Profile.Add(innerValue);
                                            }
                                        }
                                        else
                                        {
                                            lastValue = lastValue.Replace("\n", "");
                                            totalProfileParameters = Convert.ToInt32(lastValue.Substring(ptr + 2, 2), 16);
                                            {
                                                ptr += 4;
                                                for (int j = 0; j < totalProfileParameters; j++)
                                                {
                                                    string innerValue = parse.GetProfileDataString(lastValue, ref ptr);
                                                    if (!string.IsNullOrEmpty(innerValue))
                                                        input_Break_Profile.Add(innerValue);
                                                }
                                            }
                                        }
                                        input_Break_Final = input_Break_Initial.GetRange(0, input_Break_Initial.Count - 1);
                                        input_Break_Final.AddRange(input_Break_Profile);
                                    }
                                }
                            }
                            input_Break_Final = (input_Break_Profile.Count > 0) ? input_Break_Final : input_Break_Initial;
                            foreach (var item in input_Break_Final)
                            {
                                rawPushPacketTable.Rows.Add(item.ToString());
                            }
                            string columnNameData = $"Push Data-{dtDecryptedSummary.Rows[i]["Timestamp"]}-{dtDecryptedSummary.Rows[i]["Type"]}";
                            string columnNameValue = $"Push Value-{dtDecryptedSummary.Rows[i]["Timestamp"]}-{dtDecryptedSummary.Rows[i]["Type"]}";
                            if (!dtDecryptedPacketDetail.Columns.Contains(columnNameData))
                                dtDecryptedPacketDetail.Columns.Add(columnNameData, typeof(string));
                            if (!dtDecryptedPacketDetail.Columns.Contains(columnNameValue))
                                dtDecryptedPacketDetail.Columns.Add(columnNameValue, typeof(string));

                            string[] data = new string[2] { "Data Notification", "Long Invoke Id and Priority" };
                            for (int k = 0; k < 2; k++)
                            {
                                if (rawPushPacketTable.Rows.Count > dtDecryptedPacketDetail.Rows.Count)
                                    dtDecryptedPacketDetail.Rows.Add();
                                dtDecryptedPacketDetail.Rows[k][columnNameData] = rawPushPacketTable.Rows[k][0].ToString();
                                dtDecryptedPacketDetail.Rows[k][columnNameValue] = data[k].ToString();
                            }
                            for (int k = 2; k < rawPushPacketTable.Rows.Count; k++)
                            {
                                if (rawPushPacketTable.Rows.Count > dtDecryptedPacketDetail.Rows.Count)
                                    dtDecryptedPacketDetail.Rows.Add();
                                dtDecryptedPacketDetail.Rows[k][columnNameData] = rawPushPacketTable.Rows[k][0].ToString();
                                dtDecryptedPacketDetail.Rows[k][columnNameValue] = parse.GetProfileValueString(rawPushPacketTable.Rows[k][0].ToString());
                            }
                            // dgPacketsDetail.DataSource = dtDecryptedPacketDetail;
                            _packetsLoaded = true;
                            if (dgPacketsSummary.Rows.Count > 0)
                            {
                                dgPacketsSummary.Rows[0].Selected = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            AppendColoredText(txtPlainResult, $"\n[ERROR] Packet {i + 1}: {ex.Message}\n", Color.Red, true);
                        }

                        finally
                        {
                            toolStripProgressBar.Value++;
                            Application.DoEvents();
                            toolStripStatusLabel.Text = $"📦 Processing decrypted packets… {toolStripProgressBar.Value}/{toolStripProgressBar.Maximum}";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Decryption failed: {ex.Message}", "Decryption Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                try
                {
                    ResetLBLPacketCount();
                    string rawText = txtCipherPacket.Text;
                    //bool indaliFile = rawText.Contains("CIPHERED DATA")||rawText.Contains("Encrypted Packet");
                    bool indaliFile = rawText?.IndexOf("CIPHERED DATA", StringComparison.OrdinalIgnoreCase) >= 0
                   || rawText?.IndexOf("Encrypted Packet", StringComparison.OrdinalIgnoreCase) >= 0;
                    if (string.IsNullOrWhiteSpace(rawText))
                    {
                        MessageBox.Show("No data to decrypt.", "Warning",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if (string.IsNullOrEmpty(txtBlockCipherKey.Text) || string.IsNullOrEmpty(txtSystemTitle.Text) || string.IsNullOrEmpty(txtAuthenticationKey.Text))
                    {
                        MessageBox.Show("Invalid Keys", "Invalid Key",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    EK = Encoding.ASCII.GetBytes(txtBlockCipherKey.Text.Trim());
                    AK = Encoding.ASCII.GetBytes(txtAuthenticationKey.Text.Trim());
                    ST = Encoding.ASCII.GetBytes(txtSystemTitle.Text.Trim());
                    GXSecurity = Security.AuthenticationEncryption;
                    List<string> packets = new List<string>();
                    string[] lines = rawText
                        .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                    int blockCount = 0;
                    StringBuilder multiBlock = new StringBuilder();
                    toolStripProgressBar.Maximum = lines.Length;
                    foreach (string line in lines)
                    {
                        try
                        {
                            string trimmed = line.Trim();
                            string normalized = trimmed.Replace(" ", "").ToUpper();

                            // strip timestamp at end if present
                            if (normalized.StartsWith("00010001") &&
                                normalized.Length > 24 &&
                                (normalized.EndsWith("AM") || normalized.EndsWith("PM")))
                            {
                                normalized = normalized.Substring(0, normalized.Length - 20);
                                trimmed = trimmed.Substring(0, trimmed.Length - 22).Trim();
                            }

                            if (!normalized.StartsWith("00010001"))
                                continue;

                            if (blockCount == 0 &&
                                normalized.Length >= 20 &&
                                normalized.Substring(16, 2) == "E0" &&
                                normalized.Substring(18, 1) != "8" && !indaliFile)
                            {
                                blockCount = Convert.ToInt32(normalized.Substring(18, 2), 16);

                                multiBlock.Clear();
                                multiBlock.Append(trimmed);
                                blockCount--;

                                if (blockCount == 0)
                                    packets.Add(multiBlock.ToString());
                            }
                            else if (blockCount > 0 && !indaliFile)
                            {
                                multiBlock.Append(" ").Append(trimmed);
                                blockCount--;

                                if (blockCount == 0)
                                {
                                    packets.Add(multiBlock.ToString());
                                    multiBlock.Clear();
                                }
                            }
                            else
                            {
                                packets.Add(trimmed);
                            }
                        }
                        catch
                        {

                        }
                        finally
                        {
                            toolStripStatusLabel.Text = $"🔍 Analyzing packets…";
                        }
                    }
                    if (packets.Count == 0)
                    {
                        packets.Add(rawText.Trim());
                    }
                    //Decrypting packets
                    toolStripProgressBar.Value = 0;
                    toolStripProgressBar.Maximum = packets.Count;
                    foreach (var packet in packets)
                    {
                        try
                        {
                            string DecryptedData = null;
                            string decodedXml = null;
                            string typeOfData = string.Empty;
                            bool isPacketFaulty = false;

                            TCPTestNotifier tcp = new TCPTestNotifier();
                            List<byte> receivedBytes = new List<byte>();

                            string normalized = packet.Replace(" ", "").ToUpper();

                            string[] EncPackets =
                                normalized.Split(new string[] { "00010001" }, StringSplitOptions.None);
                            for (int i = 1; i < EncPackets.Length; i++)
                            {
                                EncPackets[i] = "00010001" + EncPackets[i];

                                try
                                {
                                    int index = EncPackets[i].IndexOf("DB08");
                                    if (index < 4) throw new Exception("DB08 marker not found or invalid position");
                                    double definedPacketLength = Utilities.ConvertHexToDecimal(EncPackets[i].Substring(index - 4, 4));
                                    double packetLength = EncPackets[i].Substring(index).Length;
                                    if (definedPacketLength > packetLength / 2)
                                    {
                                        DecryptedData += $"[ERROR] {(definedPacketLength - (packetLength / 2)) / 2} BYTES MISSING {(EncPackets.Length > 2 ? $"IN BLOCK {i}" : string.Empty)}\n";
                                        isPacketFaulty = true;
                                    }
                                    else if (definedPacketLength < packetLength / 2)
                                    {
                                        DecryptedData += $"[ERROR] {((packetLength / 2) - definedPacketLength) / 2} ADDITIONAL BYTES {(EncPackets.Length > 2 ? $"IN BLOCK {i}" : string.Empty)}\n";
                                        isPacketFaulty = true;
                                    }
                                    else
                                    {
                                        try
                                        {
                                            cipherText = TSTCommon.HexToBytes(tcp.Find_DB08(EncPackets[i]));
                                            if (cipherText == null || cipherText.Length < 16)
                                                throw new Exception("Cipher text too short for AES-GCM");
                                            DecryptedData += DecryptData(true, EK, AK, ST, GXSecurity, IC, cipherText) + "\n";
                                        }
                                        catch (Exception decryptEx)
                                        {
                                            isPacketFaulty = true;
                                            DecryptedData += $"[DECRYPT ERROR] BLOCK {i}: {decryptEx.Message}\n";
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    isPacketFaulty = true;
                                    DecryptedData += $"[BLOCK ERROR] BLOCK {i}: {ex.Message}\n";
                                }
                            }
                            if (!isPacketFaulty)
                            {
                                string cleaned = new string(packet.Where(c => !char.IsWhiteSpace(c)).ToArray());
                                for (int i = 0; i < cleaned.Length; i += 2)
                                    receivedBytes.Add(Convert.ToByte(cleaned.Substring(i, 2), 16));
                                decodedXml = tcp.DecodePushDataToXML(receivedBytes.ToArray(), receivedBytes.Count);
                                // PUSH TYPE
                                if (DecryptedData.Contains("00 00 19 09 00 FF")) typeOfData = "Instant Push";
                                else if (DecryptedData.Contains("00 04 19 09 00 FF")) typeOfData = "Alert Push";
                                else if (DecryptedData.Contains("00 05 19 09 00 FF")) typeOfData = "Load Survey Push";
                                else if (DecryptedData.Contains("00 06 19 09 00 FF")) typeOfData = "Daily Energy Push";
                                else if (DecryptedData.Contains("00 82 19 09 00 FF")) typeOfData = "Self Registration Push";
                                else if (DecryptedData.Contains("00 84 19 09 00 FF")) typeOfData = "Billing Push";
                                else if (DecryptedData.Contains("00 86 19 09 00 FF")) typeOfData = "Tamper Push";
                                else if (DecryptedData.Contains("00 00 19 09 81 FF")) typeOfData = "Current Bill Push";
                                else typeOfData = "Unknown Push Type";

                                string sData = DecryptedData.Replace(" ", "");
                                int counter = 10;
                                DateTime packetSendingTime = DateTime.ParseExact(parse.Getdate(sData.Substring(12, 24), 0, false), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                counter = 36;
                                while (sData.Substring(counter, 2) != "02")
                                    counter += 4;
                                counter += 4;
                                int length = Convert.ToInt32(sData.Substring(counter + 2, 2), 16) * 2;
                                string deviceId = parse.GetProfileValueString(sData.Substring(counter, length + 4)); GetPacketCounter(typeOfData);
                                Color pushColor = GetPushColor(typeOfData);
                                AppendColoredText(txtPlainResult, $"\n{packetSendingTime:dd/MM/yyyy hh:mm:ss tt} - {typeOfData}\n", pushColor, true);
                                AppendColoredText(txtPlainResult, DecryptedData + "\n", pushColor, false);
                                AppendColoredText(txtXmlResult, $"\n{packetSendingTime:dd/MM/yyyy hh:mm:ss tt} - {typeOfData}\n", pushColor, true);
                                AppendColoredText(txtXmlResult, decodedXml + "\n", pushColor, false);
                                dtDecryptedSummary.Rows.Add(packetSendingTime.ToString("dd/MM/yyyy hh:mm:ss tt"), typeOfData, normalized.Length.ToString(), deviceId, "Success", DecryptedData);
                            }
                            else
                            {
                                string logMessage = $"\n[ERROR] [DLMS-DECRYPTION] " +
                                                    $"Ciphered data corruption detected. " +
                                                    $"Decryption failed due to {DecryptedData}\n";

                                AppendColoredText(txtPlainResult, logMessage, Color.Red, true);
                                AppendColoredText(txtXmlResult, logMessage, Color.Red, true);
                                dtDecryptedSummary.Rows.Add("-", "-", "Not matched", "", "Failed", DecryptedData);
                            }
                        }
                        catch (Exception ex)
                        {
                            dtDecryptedSummary.Rows.Add(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"), "Unknown", packet.Length, "", "Failed", ex.Message);
                            AppendColoredText(txtPlainResult, "\n[DECRYPT FAILED] Corrupted or invalid packet detected\n", Color.Red, true);
                            continue;
                        }
                        finally
                        {
                            toolStripProgressBar.Value++;
                            Application.DoEvents();
                            toolStripStatusLabel.Text = $"🔐 Decrypting packets… {toolStripProgressBar.Value}/{toolStripProgressBar.Maximum}";
                        }
                    }
                    dgPacketsSummary.DataSource = dtDecryptedSummary;
                    //Decrypted Packets Detail
                    toolStripProgressBar.Value = 0;
                    toolStripProgressBar.Maximum = dtDecryptedSummary.Rows.Count;
                    for (int i = 0; i < dtDecryptedSummary.Rows.Count; i++)
                    {
                        try
                        {
                            DataTable rawPushPacketTable = new DataTable();
                            rawPushPacketTable.Columns.Add($"Push Data", typeof(string));
                            if (dtDecryptedSummary.Rows[i]["Status"].ToString() != "Success")
                            {
                                rawPushPacketTable.Rows.Add("Corrupted Packet Data");
                                while (dtDecryptedPacketDetail.Rows.Count <= i)
                                {
                                    dtDecryptedPacketDetail.Rows.Add();
                                }
                                dtDecryptedPacketDetail.Rows[i][0] = "Corrupted Packet Data";
                                continue;
                            }
                            List<string> details = new List<string>();
                            int totalProfileParameters = 0;
                            string input = dtDecryptedSummary.Rows[i]["Decrypted Data"].ToString().Replace(" ", "");
                            int pointer = 0;
                            input_Break_Initial.Clear();
                            input_Break_Profile.Clear();
                            input_Break_Initial.Add(input.Substring(pointer, 2));
                            pointer += 2;
                            input_Break_Initial.Add(input.Substring(pointer, 8));
                            pointer += 8;
                            if (input.Substring(pointer, 2) == "0C")
                            {
                                input_Break_Initial.Add(input.Substring(pointer, 26));
                                pointer += 26;
                            }
                            if (input.Length < pointer + 4)
                                return;
                            string profileString = input.Substring(pointer);
                            if (profileString == "0200" || profileString.Length < 4 || input.Substring(pointer, 2) != "02")
                            {
                                pointer += 4;
                                if (profileString == "0200" || profileString.Length < 4 || input.Substring(pointer, 2) != "02")
                                {
                                    return;
                                }
                            }
                            if (input.Substring(pointer, 2) == "02")
                            {
                                pointer += 2;
                                totalProfileParameters = Convert.ToInt32(input.Substring(pointer, 2), 16);
                                pointer += 2;
                                for (int j = 0; j < totalProfileParameters; j++)
                                {
                                    string value = parse.GetProfileDataString(input, ref pointer);
                                    if (!string.IsNullOrEmpty(value))
                                    {
                                        input_Break_Initial.Add(value);
                                    }
                                }

                            }
                            string lastValue = input_Break_Initial.Last().Trim();
                            if ((lastValue.StartsWith("02") || lastValue.StartsWith("01")) && input_Break_Initial.Count > 0)
                            {
                                int ptr = 4;
                                string packet = dtDecryptedSummary.Rows[i]["Type"].ToString();
                                if (packet == "Load Survey Push")
                                {
                                    totalProfileParameters = Convert.ToInt32(lastValue.Substring(ptr + 2, 2), 16);
                                    while (lastValue.Length > ptr)
                                    {
                                        ptr += 4;
                                        for (int j = 0; j < totalProfileParameters; j++)
                                        {
                                            string innerValue = parse.GetProfileDataString(lastValue, ref ptr);
                                            if (!string.IsNullOrEmpty(innerValue))
                                                input_Break_Profile.Add(innerValue);
                                        }
                                        input_Break_Final = input_Break_Initial.GetRange(0, input_Break_Initial.Count - 1);
                                        input_Break_Final.AddRange(input_Break_Profile);
                                    }
                                }
                                else
                                {
                                    if (lastValue.Length > ptr)
                                    {
                                        if (lastValue.Substring(6, 2) == "81")
                                        {
                                            lastValue = lastValue.Replace("\n", "");
                                            totalProfileParameters = Convert.ToInt32(lastValue.Substring(ptr + 4, 2), 16);
                                            ptr += 6;
                                            for (int j = 0; j < totalProfileParameters; j++)
                                            {
                                                string innerValue = parse.GetProfileDataString(lastValue, ref ptr);
                                                if (!string.IsNullOrEmpty(innerValue))
                                                    input_Break_Profile.Add(innerValue);
                                            }
                                        }
                                        else if (lastValue.Substring(6, 2) == "82")
                                        {
                                            lastValue = lastValue.Replace("\n", "");
                                            totalProfileParameters = Convert.ToInt32(lastValue.Substring(ptr + 4, 4), 16);
                                            ptr += 8;
                                            for (int j = 0; j < totalProfileParameters; j++)
                                            {
                                                string innerValue = parse.GetProfileDataString(lastValue, ref ptr);
                                                if (!string.IsNullOrEmpty(innerValue))
                                                    input_Break_Profile.Add(innerValue);
                                            }
                                        }
                                        else
                                        {
                                            lastValue = lastValue.Replace("\n", "");
                                            totalProfileParameters = Convert.ToInt32(lastValue.Substring(ptr + 2, 2), 16);
                                            {
                                                ptr += 4;
                                                for (int j = 0; j < totalProfileParameters; j++)
                                                {
                                                    string innerValue = parse.GetProfileDataString(lastValue, ref ptr);
                                                    if (!string.IsNullOrEmpty(innerValue))
                                                        input_Break_Profile.Add(innerValue);
                                                }
                                            }
                                        }
                                        input_Break_Final = input_Break_Initial.GetRange(0, input_Break_Initial.Count - 1);
                                        input_Break_Final.AddRange(input_Break_Profile);
                                    }
                                }
                            }
                            input_Break_Final = (input_Break_Profile.Count > 0) ? input_Break_Final : input_Break_Initial;
                            foreach (var item in input_Break_Final)
                            {
                                rawPushPacketTable.Rows.Add(item.ToString());
                            }
                            string columnNameData = $"Push Data-{dtDecryptedSummary.Rows[i]["Timestamp"]}-{dtDecryptedSummary.Rows[i]["Type"]}";
                            string columnNameValue = $"Push Value-{dtDecryptedSummary.Rows[i]["Timestamp"]}-{dtDecryptedSummary.Rows[i]["Type"]}";
                            if (!dtDecryptedPacketDetail.Columns.Contains(columnNameData))
                                dtDecryptedPacketDetail.Columns.Add(columnNameData, typeof(string));
                            if (!dtDecryptedPacketDetail.Columns.Contains(columnNameValue))
                                dtDecryptedPacketDetail.Columns.Add(columnNameValue, typeof(string));

                            string[] data = new string[2] { "Data Notification", "Long Invoke Id and Priority" };
                            for (int k = 0; k < 2; k++)
                            {
                                if (rawPushPacketTable.Rows.Count > dtDecryptedPacketDetail.Rows.Count)
                                    dtDecryptedPacketDetail.Rows.Add();
                                dtDecryptedPacketDetail.Rows[k][columnNameData] = rawPushPacketTable.Rows[k][0].ToString();
                                dtDecryptedPacketDetail.Rows[k][columnNameValue] = data[k].ToString();
                            }
                            for (int k = 2; k < rawPushPacketTable.Rows.Count; k++)
                            {
                                if (rawPushPacketTable.Rows.Count > dtDecryptedPacketDetail.Rows.Count)
                                    dtDecryptedPacketDetail.Rows.Add();
                                dtDecryptedPacketDetail.Rows[k][columnNameData] = rawPushPacketTable.Rows[k][0].ToString();
                                dtDecryptedPacketDetail.Rows[k][columnNameValue] = parse.GetProfileValueString(rawPushPacketTable.Rows[k][0].ToString());
                            }
                            // dgPacketsDetail.DataSource = dtDecryptedPacketDetail;
                            _packetsLoaded = true;
                            if (dgPacketsSummary.Rows.Count > 0)
                            {
                                dgPacketsSummary.Rows[0].Selected = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            AppendColoredText(txtPlainResult, $"\n[ERROR] Packet {i + 1}: {ex.Message}\n", Color.Red, true);
                        }

                        finally
                        {
                            toolStripProgressBar.Value++;
                            Application.DoEvents();
                            toolStripStatusLabel.Text = $"📦 Processing decrypted packets… {toolStripProgressBar.Value}/{toolStripProgressBar.Maximum}";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Decryption failed: {ex.Message}", "Decryption Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            for (int i = 0; i < dgPacketsSummary.Columns.Count; i++)
            {
                dgPacketsSummary.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                if (i == dgPacketsSummary.Columns.Count - 1)
                {
                    dgPacketsSummary.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    foreach (DataGridViewRow row in dgPacketsSummary.Rows)
                    {
                        if (row.Cells[i].Value.ToString().Contains("ERROR"))
                        {
                            row.Cells[i].Style.ForeColor = Color.Red;
                            row.Cells[i].Style.Font = new Font(dgPacketsSummary.Font, FontStyle.Bold);
                        }
                    }
                }
                else
                {
                    dgPacketsSummary.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
                    foreach (DataGridViewRow row in dgPacketsSummary.Rows)
                    {
                        if (row.Cells["Status"].Value.ToString() != "Success")
                        {
                            row.Cells["Status"].Style.ForeColor = Color.Red;
                            row.Cells["Status"].Style.Font = new Font(dgPacketsSummary.Font, FontStyle.Bold);
                        }
                    }
                }
            }
            for (int i = 0; i < dgPacketsDetail.Columns.Count; i++)
            {
                dgPacketsDetail.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            toolStripProgressBar.Value = toolStripProgressBar.Maximum;
            toolStripProgressBar.Visible = false;
            toolStripStatusLabel.Text = "✅ Decryption Process Completed";
            toolStripStatusLabel.ForeColor = Color.Green;
            btnDecrypt.Enabled = true;
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to clear all data?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    txtCipherPacket.Clear();
                    lblFilePath.Text = "📄 File: No file loaded";
                    ResetLBLPacketCount();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in clearing data: {ex.Message}");
            }
        }
        private void btnExport_Click(object sender, EventArgs e)
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
                string baseFolder = Path.Combine(LOG_DIRECTORY, $"Push Packet Decrypted Files");
                Directory.CreateDirectory(baseFolder);
                reportFolder = Path.Combine(baseFolder, folderName);
                Directory.CreateDirectory(reportFolder);

                string excelPath = Path.Combine(reportFolder, $"Reports_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.xlsx");
                DataTableOperations.ExportDataTableToExcelWithDifferentSheet(dtDecryptedSummary, excelPath, "Summary");
                DataTableOperations.ExportDataTableToExcelWithDifferentSheet(dtDecryptedPacketDetail, excelPath, "Datailed Packet");
                string rtfPath = Path.Combine(reportFolder, $"Decrypted Data_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.rtf");
                txtPlainResult.SaveFile(rtfPath, RichTextBoxStreamType.RichText);
                rtfPath = Path.Combine(reportFolder, $"XML Data_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.rtf");
                txtXmlResult.SaveFile(rtfPath, RichTextBoxStreamType.RichText);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting reports: {ex.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                MessageBox.Show($"Reports saved at:{reportFolder}", "Report Info");
            }
        }
        private void dgvPackets_SelectionChanged(object sender, EventArgs e)
        {
            if (!_packetsLoaded)
                return;

            if (dgPacketsSummary.CurrentRow == null)
                return;

            string timestamp = $"{dgPacketsSummary.CurrentRow.Cells["Timestamp"].Value}-{dgPacketsSummary.CurrentRow.Cells["Type"].Value}";
            ShowPacketDetailsByTimestamp(timestamp);
        }
        private void BtnFilterbyType_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = sender as Button;
                if (btn == null) return;
                if (btn.Text.Contains("Total"))
                {
                    dgPacketsSummary.DataSource = null;
                    dgPacketsSummary.DataSource = dtDecryptedSummary;
                    return;
                }
                DataTable filteredDatatble = FilterPacketsByType(btn.Tag.ToString());
                if (filteredDatatble.Rows.Count > 0)
                {
                    dgPacketsSummary.DataSource = null;
                    dgPacketsSummary.DataSource = filteredDatatble;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in clearing data: {ex.Message}");
            }
        }
        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.D)
            {
                e.SuppressKeyPress = true;
                btnDecrypt.PerformClick();
            }
            if (e.Control && e.KeyCode == Keys.E)
            {
                e.SuppressKeyPress = true;
                btnExport.PerformClick();
            }
            if (e.Control && e.KeyCode == Keys.Delete)
            {
                e.SuppressKeyPress = true;
                btnClear.PerformClick();
            }
        }

        #region Helper Methods
        private void ShowPacketDetailsByTimestamp(string timestamp)
        {
            if (dtDecryptedPacketDetail == null || dtDecryptedPacketDetail.Columns.Count == 0)
                return;

            string dataCol = $"Push Data-{timestamp}";
            string valueCol = $"Push Value-{timestamp}";

            if (!dtDecryptedPacketDetail.Columns.Contains(dataCol) ||
                !dtDecryptedPacketDetail.Columns.Contains(valueCol))
            {
                dgPacketsDetail.DataSource = null;
                return;
            }

            DataTable filtered = new DataTable();
            filtered.Columns.Add(dataCol, typeof(string));
            filtered.Columns.Add(valueCol, typeof(string));

            foreach (DataRow row in dtDecryptedPacketDetail.Rows)
            {
                if (!string.IsNullOrEmpty(row[dataCol].ToString()))
                    filtered.Rows.Add(row[dataCol], row[valueCol]);
            }
            dgPacketsDetail.DataSource = filtered;

            dgPacketsDetail.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgPacketsDetail.RowHeadersVisible = false;
        }
        private DataTable FilterPacketsByType(string filterText)
        {
            if (dtDecryptedSummary == null || dtDecryptedSummary.Rows.Count == 0)
                return null;

            // Escape single quotes for DataTable filter
            string safeText = filterText.Replace("'", "''");

            DataRow[] filteredRows = dtDecryptedSummary.Select(
                $"Type LIKE '%{safeText}%'");

            if (filteredRows.Length == 0)
                return dtDecryptedSummary.Clone(); // empty table with same schema

            DataTable filteredTable = dtDecryptedSummary.Clone();
            foreach (DataRow row in filteredRows)
                filteredTable.ImportRow(row);

            return filteredTable;
        }
        private void UpdateButtonCount(Button btn, int count)
        {
            btn.Text = $"{btn.Tag}{Environment.NewLine}{count}";
        }
        public void GetPacketCounter(string typeOfData)
        {
            if (typeOfData == "Instant Push")
            {
                instantCount++;
                UpdateButtonCount(btnInstant, instantCount);
            }
            else if (typeOfData == "Alert Push")
            {
                alertCount++;
                UpdateButtonCount(btnAlert, alertCount);
            }
            else if (typeOfData == "Load Survey Push")
            {
                lsCount++;
                UpdateButtonCount(btnLS, lsCount);
            }
            else if (typeOfData == "Daily Energy Push")
            {
                deCount++;
                UpdateButtonCount(btnDE, deCount);
            }
            else if (typeOfData == "Self Registration Push")
            {
                srCount++;
                UpdateButtonCount(btnSR, srCount);
            }
            else if (typeOfData == "Billing Push")
            {
                billCount++;
                UpdateButtonCount(btnBill, billCount);
            }
            else if (typeOfData == "Current Bill Push")
            {
                cbCount++;
                UpdateButtonCount(btnCB, cbCount);
            }
            else if (typeOfData == "Tamper Push")
            {
                tamperCount++;
                UpdateButtonCount(btnTamper, tamperCount);
            }
            if (typeOfData == "Clear All")
            {
                UpdateButtonCount(btnInstant, instantCount);
                UpdateButtonCount(btnAlert, alertCount);
                UpdateButtonCount(btnLS, lsCount);
                UpdateButtonCount(btnDE, deCount);
                UpdateButtonCount(btnSR, srCount);
                UpdateButtonCount(btnBill, billCount);
                UpdateButtonCount(btnCB, cbCount);
                UpdateButtonCount(btnTamper, tamperCount);
            }
            int totalCount =
                instantCount + alertCount + lsCount + deCount +
                srCount + billCount + cbCount + tamperCount;
            UpdateButtonCount(btnTotalCount, totalCount);
        }
        private void AppendColoredText(RichTextBox rtb, string text, Color color, bool bold = false)
        {
            rtb.SelectionStart = rtb.TextLength;
            rtb.SelectionLength = 0;

            rtb.SelectionColor = color;
            rtb.SelectionFont = bold
                ? new Font(rtb.Font, FontStyle.Bold)
                : new Font(rtb.Font, FontStyle.Regular);

            rtb.AppendText(text);

            rtb.SelectionColor = rtb.ForeColor;
            rtb.SelectionFont = rtb.Font;
        }
        private Color GetPushColor(string typeOfData)
        {
            switch (typeOfData)
            {
                case "Instant Push":
                    return Color.DodgerBlue;

                case "Alert Push":
                    return Color.OrangeRed;

                case "Load Survey Push":
                    return Color.MediumPurple;

                case "Daily Energy Push":
                    return Color.SeaGreen;

                case "Self Registration Push":
                    return Color.DarkCyan;

                case "Billing Push":
                    return Color.SaddleBrown;

                case "Tamper Push":
                    return Color.DarkRed;

                case "Current Bill Push":
                    return Color.DarkGreen;

                default:
                    return Color.Gray;
            }
        }
        private void TestLogService_AppendColoredTextControlNotifier(string message, Color color, bool isBold = false)
        {
            _logBuffer2.Enqueue((message + Environment.NewLine, color, isBold));
        }
        public void ResetLBLPacketCount()
        {
            txtPlainResult.Clear();
            txtXmlResult.Clear();
            dgPacketsSummary.DataSource = null;
            dgPacketsDetail.DataSource = null;
            dtDecryptedSummary.Rows.Clear(); dtDecryptedPacketDetail.Reset();
            dtDecryptedSummary.Rows.Clear(); dtDecryptedPacketDetail.Reset();
            //tableLayoutPnl_PacketCounts.Controls.OfType<Label>().Where(l => l.Name.StartsWith("lbl")).ToList().ForEach(l => l.Text = "-");
            instantCount = 0; lsCount = 0; deCount = 0; billCount = 0; srCount = 0; cbCount = 0; alertCount = 0; tamperCount = 0;
            GetPacketCounter("Clear All");
        }
        public static void StyleControl(Control control, int radius = 12)
        {
            if (control == null) return;

            ApplyRoundedCorners(control, radius);

            switch (control)
            {
                case DataGridView dgv:
                    StyleDataGridView(dgv);
                    break;
            }

            // 🔥 RECURSIVE: Style ALL child controls
            foreach (Control child in control.Controls)
            {
                StyleControl(child, radius);
            }
        }
        private static void ApplyRoundedCorners(Control control, int radius)
        {
            if (control is TableLayoutPanel)
                return;

            control.Margin = new Padding(6);

            void apply(object s, EventArgs e)
            {
                Rectangle rect = control.ClientRectangle;
                if (rect.Width <= 0 || rect.Height <= 0) return;

                using (GraphicsPath path = GetRoundedPath(rect, radius))
                {
                    control.Region = new Region(path);
                }
            }

            control.Resize += apply;
            control.HandleCreated += apply;
        }
        private static GraphicsPath GetRoundedPath(Rectangle rect, int radius)
        {
            int d = radius * 2;
            GraphicsPath path = new GraphicsPath();

            path.StartFigure();
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();

            return path;
        }
        private static void StyleDataGridView(DataGridView dgv)
        {
            dgv.BorderStyle = BorderStyle.None;
            dgv.BackgroundColor = System.Drawing.SystemColors.Control;
            dgv.GridColor = System.Drawing.SystemColors.Control;
            dgv.EnableHeadersVisualStyles = false;

            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 94, 168);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 94, 168);
            dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;

            dgv.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 10F);
            dgv.DefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dgv.DefaultCellStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;

            dgv.RowHeadersVisible = false;
            dgv.RowTemplate.Height = 30;
        }
        #endregion

        private void chkIsDecryptedPacket_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIsDecryptedPacket.Checked)
            {
                btnDecrypt.Text = "🔓 Load DPPs";
                grpCipherInput.Text = "📦 Decrypted Push Packet Data";
                grpCipherInput.ForeColor = System.Drawing.Color.FromArgb(245, 134, 52);
                btnTotalCount.BackColor = System.Drawing.Color.FromArgb(245, 134, 52);
                btnTotalCount.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(245, 134, 52);
                btnTotalCount.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(255, 159, 82);
                btnTotalCount.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(230, 120, 40);
            }
            else
            {
                btnDecrypt.Text = "🔓 &Decrypt";
                grpCipherInput.Text = "📦 Encrypted Push Packet Data";
                grpCipherInput.ForeColor = System.Drawing.Color.FromArgb(0, 94, 168);
                btnTotalCount.BackColor = System.Drawing.Color.FromArgb(0, 94, 168);
                btnTotalCount.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(0, 80, 150);
                btnTotalCount.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(0, 70, 130);
                btnTotalCount.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(0, 110, 190);
            }
        }

        private void splitter1_SplitterMoving(object sender, SplitterEventArgs e)
        {
            if (e.Y < 240)
            {
                e.SplitY = 240;
            }
        }


        #region AUTHENTICATION & ENCRYPTION
        public string DecryptData(bool Decrypt, byte[] EK, byte[] AK, byte[] ST, Gurux.DLMS.Enums.Security GXCIPH, uint IC, byte[] cipherText)
        {
            string str;
            string reply = string.Empty;
            try
            {
                client.ClientAddress = 11;
                client.Authentication = Gurux.DLMS.Enums.Authentication.HighGMAC;
                client.Ciphering.BlockCipherKey = EK;
                client.Ciphering.AuthenticationKey = AK;
                client.Ciphering.SystemTitle = ST;
                client.Ciphering.Security = GXCIPH;
                client.UseLogicalNameReferencing = true;
                client.ServerAddress = 1;
                client.ServerAddressSize = (byte)1;
                Gurux.DLMS.GXByteBuffer data = new Gurux.DLMS.GXByteBuffer();
                data.Set(cipherText);
                AesGcmParameter p = new AesGcmParameter((byte)30, Security.AuthenticationEncryption, IC, client.Ciphering.SystemTitle, client.Ciphering.BlockCipherKey, client.Ciphering.AuthenticationKey);
                Gurux.DLMS.GXByteBuffer gxByteBuffer = new Gurux.DLMS.GXByteBuffer();
                byte[] numArray = !Decrypt ? EncryptAesGcm(p, cipherText) : DecryptAesGcm(p, data);
                gxByteBuffer.Set(numArray);
                str = gxByteBuffer.ToString();
            }
            catch
            {
                throw new Exception("Error in decryption");
            }
            return str;
        }
        private static byte[] GetNonse(uint invocationCounter, byte[] systemTitle)
        {
            byte[] nonse = new byte[12];
            systemTitle.CopyTo((Array)nonse, 0);
            ((IEnumerable<byte>)BitConverter.GetBytes(invocationCounter)).Reverse<byte>().ToArray<byte>().CopyTo((Array)nonse, 8);
            return nonse;
        }
        public static byte[] DecryptAesGcm(AesGcmParameter p, GXByteBuffer data)
        {
            Command command = data != null && data.Size >= 2 ? (Command)data.GetUInt8() : throw new ArgumentOutOfRangeException("cryptedData");
            switch (command)
            {
                case Command.GloInitiateRequest:
                case Command.GloReadRequest:
                case Command.GloWriteRequest:
                case Command.GloInitiateResponse:
                case Command.GloReadResponse:
                case Command.GloWriteResponse:
                case Command.DedInitiateRequest:
                case Command.DedReadRequest:
                case Command.DedWriteRequest:
                case Command.DedInitiateResponse:
                case Command.DedReadResponse:
                case Command.DedWriteResponse:
                case Command.GloGetRequest:
                case Command.GloSetRequest:
                //case Command.GloEventNotificationRequest:
                case Command.GloMethodRequest:
                case Command.GloGetResponse:
                case Command.GloSetResponse:
                case Command.GloMethodResponse:
                case Command.DedGetRequest:
                case Command.DedSetRequest:
                //case Command.DedEventNotificationRequest:
                case Command.DedMethodRequest:
                case Command.DedGetResponse:
                case Command.DedSetResponse:
                case Command.DedMethodResponse:
                case Command.GeneralCiphering:
                    ulong num1 = 0;
                    int num2;
                    if (command == Command.GeneralCiphering)
                    {
                        byte[] target1 = new byte[TSTCommon.GetObjectCount(data)];
                        data.Get(target1);
                        num1 = new GXByteBuffer(target1).GetUInt64();
                        byte[] target2 = new byte[TSTCommon.GetObjectCount(data)];
                        data.Get(target2);
                        p.SystemTitle = target2;
                        byte[] target3 = new byte[TSTCommon.GetObjectCount(data)];
                        data.Get(target3);
                        p.RecipientSystemTitle = target3;
                        int objectCount = TSTCommon.GetObjectCount(data);
                        if (objectCount != 0)
                        {
                            byte[] target4 = new byte[objectCount];
                            data.Get(target4);
                            p.DateTime = target4;
                        }
                        int uint8_1 = (int)data.GetUInt8();
                        if (uint8_1 != 0)
                        {
                            byte[] target5 = new byte[uint8_1];
                            data.Get(target5);
                            p.OtherInformation = target5;
                        }
                        num2 = (int)data.GetUInt8();
                        int uint8_2 = (int)data.GetUInt8();
                        num2 = (int)data.GetUInt8();
                        int uint8_3 = (int)data.GetUInt8();
                        p.KeyParameters = uint8_3;
                        if (uint8_3 == 1)
                        {
                            byte[] target6 = new byte[TSTCommon.GetObjectCount(data)];
                            data.Get(target6);
                            p.KeyCipheredData = target6;
                        }
                        else
                        {
                            if (uint8_3 != 2)
                                throw new ArgumentException("key-parameters");
                            if (TSTCommon.GetObjectCount(data) != 0)
                                throw new ArgumentException("Invalid key parameters");
                        }
                    }
                    num2 = TSTCommon.GetObjectCount(data);
                    p.CipheredContent = data.Remaining();
                    byte uint8 = data.GetUInt8();
                    Security security = (Security)((int)uint8 & 48);
                    if (((uint)uint8 & 128U) > 0U)
                        Debug.WriteLine("Compression is used.");
                    if (((uint)uint8 & 64U) > 0U)
                        Debug.WriteLine("Error: Key_Set is used.");
                    if (((uint)uint8 & 32U) > 0U)
                        Debug.WriteLine("Encryption is applied.");
                    SecuritySuite securitySuite = (SecuritySuite)((int)uint8 & 3);
                    p.Security = security;
                    //byte[] icBytes = new byte[4];
                    //data.Get(icBytes);
                    //uint uint32 = data.GetUInt32();
                    //p.InvocationCounter = (ulong)uint32;
                    int pos = data.Position;
                    byte[] icBytes = new byte[4];
                    data.Get(icBytes);
                    data.Position = pos;
                    uint uint32 = data.GetUInt32();
                    p.InvocationCounter = (ulong)uint32;

                    if (securitySuite != 0)
                        throw new NotImplementedException("Security Suite 1 is not implemented.");
                    //Debug.WriteLine("Decrypt settings: " + p.ToString());
                    //Debug.WriteLine("Encrypted: " + TSTCommon.ToHex(data.Data, false, data.Position, data.Size - data.Position));
                    byte[] numArray1 = new byte[12]; //Gmac tag
                    byte[] numArray2 = (byte[])null; //Cipher text for Auth Only
                    byte[] numArray3 = (byte[])null; //Cipher text for Encryption Only
                    if (security == Security.Authentication)
                    {
                        numArray2 = new byte[data.Size - data.Position - 12];
                        data.Get(numArray2);
                        data.Get(numArray1);
                        GXDLMSCiphering.EncryptAesGcm(p, numArray2);
                        if (!GXDLMSCipheringStream.TagsEquals(numArray1, p.CountTag))
                        {
                            if (num1 > 0UL)
                                p.InvocationCounter = num1;
                            if (p.Xml == null)
                                throw new GXDLMSException("Decrypt failed. Invalid tag.");
                            p.Xml.AppendComment("Decrypt failed. Invalid tag.");
                        }
                        return numArray2;
                    }
                    if (security == Security.Encryption)
                    {
                        numArray3 = new byte[data.Size - data.Position];
                        data.Get(numArray3);
                    }
                    else if (security == Security.AuthenticationEncryption)
                    {
                        numArray3 = new byte[data.Size - data.Position - 12];
                        data.Get(numArray3);
                        data.Get(numArray1);
                    }
                    //byte[] akFromPkt = authenticatedData.Skip(1).ToArray();
                    ////string[] akFromPkt = (authenticatedData.Skip(1).ToArray()).Select(b => b.ToString()).ToArray();
                    //if (!p.AuthenticationKey.SequenceEqual(akFromPkt))
                    //{
                    //    throw new Exception("Authentication key mismatch");
                    //}
                    byte[] authenticatedData = GetAuthenticatedData(p, numArray3);
                    byte[] nonse = GetNonse(uint32, p.SystemTitle);
                    byte[] gmacManualTag = GenerateDLMSGmac((byte)48, p.BlockCipherKey, p.AuthenticationKey, p.SystemTitle, uint32, numArray3);
                    //byte[] gmacManualTag = GenerateDLMSGmac((byte)48, p.SystemTitle, icBytes, numArray3, numArray1, p.BlockCipherKey);
                    //Debug.WriteLine("AAD: " + TSTCommon.ToHex(authenticatedData, true));
                    GXDLMSCipheringStream cipheringStream = new GXDLMSCipheringStream(security, true, p.BlockCipherKey, authenticatedData, nonse, numArray1);
                    cipheringStream.Write(numArray3);
                    //GXDLMSCipheringStream verifyStream = new GXDLMSCipheringStream(security, true, p.BlockCipherKey, authenticatedData, nonse, null);
                    //verifyStream.Write(numArray3);
                    //verifyStream.FlushFinalBlock();
                    //byte[] calculatedVerifyTag = verifyStream.GetTag();
                    if (num1 > 0UL)
                        p.InvocationCounter = num1;
                    //if (!GXDLMSCipheringStream.TagsEquals(numArray1, calculatedVerifyTag))
                    //{
                    //    throw new GXDLMSException("Invalid GMAC tag");
                    //}
                    //return verifyStream.FlushFinalBlock();
                    return cipheringStream.FlushFinalBlock();
                /*
                byte[] authenticatedData = GetAuthenticatedData(p, numArray3);
                byte[] nonse = GetNonse(uint32, p.SystemTitle);
                GXDLMSCipheringStream cipheringStream = new GXDLMSCipheringStream(security, true, p.BlockCipherKey, authenticatedData, nonse, numArray1);
                cipheringStream.Write(numArray3);
                if (num1 > 0UL)
                    p.InvocationCounter = num1;
                return cipheringStream.FlushFinalBlock();
                */
                case Command.GeneralGloCiphering:
                case Command.GeneralDedCiphering:
                    int objectCount1 = TSTCommon.GetObjectCount(data);
                    if (objectCount1 != 0)
                    {
                        p.SystemTitle = new byte[objectCount1];
                        data.Get(p.SystemTitle);
                        if (p.Xml != null && p.Xml.Comments)
                            p.Xml.AppendComment(TSTCommon.SystemTitleToString("DLMS", p.SystemTitle));
                        goto case Command.GloInitiateRequest;
                    }
                    else
                        goto case Command.GloInitiateRequest;
                default:
                    throw new ArgumentOutOfRangeException("cryptedData");
            }
        }
        internal static byte[] EncryptAesGcm(AesGcmParameter param, byte[] plainText)
        {
            Debug.WriteLine("Encrypt settings: " + param.ToString());
            param.CountTag = (byte[])null;
            GXByteBuffer gxByteBuffer = new GXByteBuffer();
            if (param.Type == CountType.Packet)
                gxByteBuffer.SetUInt8((byte)param.Security);
            byte[] array = ((IEnumerable<byte>)BitConverter.GetBytes((uint)param.InvocationCounter)).Reverse<byte>().ToArray<byte>();
            byte[] authenticatedData = GetAuthenticatedData(param, plainText);
            GXDLMSCipheringStream cipheringStream = new GXDLMSCipheringStream(param.Security, true, param.BlockCipherKey, authenticatedData, GetNonse((uint)param.InvocationCounter, param.SystemTitle), (byte[])null);
            if (param.Security != Security.Authentication)
                cipheringStream.Write(plainText);
            byte[] numArray = cipheringStream.FlushFinalBlock();
            if (param.Security == Security.Authentication)
            {
                if (param.Type == CountType.Packet)
                    gxByteBuffer.Set(array);
                if ((param.Type & CountType.Data) != 0)
                    gxByteBuffer.Set(plainText);
                if ((param.Type & CountType.Tag) != 0)
                {
                    param.CountTag = cipheringStream.GetTag();
                    gxByteBuffer.Set(param.CountTag);
                }
            }
            else if (param.Security == Security.Encryption)
            {
                if (param.Type == CountType.Packet)
                    gxByteBuffer.Set(array);
                gxByteBuffer.Set(numArray);
            }
            else
            {
                if (param.Security != Security.AuthenticationEncryption)
                    throw new ArgumentOutOfRangeException("security");
                if (param.Type == CountType.Packet)
                    gxByteBuffer.Set(array);
                if ((param.Type & CountType.Data) != 0)
                    gxByteBuffer.Set(numArray);
                if ((param.Type & CountType.Tag) != 0)
                {
                    param.CountTag = cipheringStream.GetTag();
                    gxByteBuffer.Set(param.CountTag);
                }
            }
            if (param.Type == CountType.Packet)
            {
                GXByteBuffer buff = new GXByteBuffer((ushort)(10 + gxByteBuffer.Size));
                buff.SetUInt8(param.Tag);
                if (param.Tag == (byte)219 || param.Tag == (byte)220 || param.Tag == (byte)15)
                {
                    if (!param.IgnoreSystemTitle)
                    {
                        TSTCommon.SetObjectCount(param.SystemTitle.Length, buff);
                        buff.Set(param.SystemTitle);
                    }
                    else
                        buff.SetUInt8((byte)0);
                }
                TSTCommon.SetObjectCount(gxByteBuffer.Size, buff);
                buff.Set(gxByteBuffer.Array());
                return buff.Array();
            }
            byte[] bytes = gxByteBuffer.Array();
            Debug.WriteLine("Crypted: " + TSTCommon.ToHex(bytes, true));
            return bytes;
        }
        private static byte[] GetAuthenticatedData(AesGcmParameter p, byte[] plainText)
        {
            if (p.Security == Security.Authentication)
            {
                GXByteBuffer gxByteBuffer = new GXByteBuffer();
                gxByteBuffer.SetUInt8((byte)p.Security);
                gxByteBuffer.Set(p.AuthenticationKey);
                gxByteBuffer.Set(plainText);
                return gxByteBuffer.Array();
            }
            if (p.Security == Security.Encryption)
                return p.AuthenticationKey;
            if (p.Security != Security.AuthenticationEncryption)
                return (byte[])null;
            GXByteBuffer gxByteBuffer1 = new GXByteBuffer();
            gxByteBuffer1.SetUInt8((byte)p.Security);
            gxByteBuffer1.Set(p.AuthenticationKey);
            return gxByteBuffer1.Array();
        }
        //private static byte[] GetAuthenticatedData(AesGcmParameter p, byte[] plainText = (byte[])null)
        //{
        //    GXByteBuffer bb = new GXByteBuffer();
        //    bb.SetUInt8((byte)p.Security);
        //    if (p.AuthenticationKey != null && p.AuthenticationKey.Length > 0)
        //        bb.Set(p.AuthenticationKey);
        //    //bb.SetUInt8((byte)p.Security);
        //    //bb.Set(p.AuthenticationKey);

        //    return bb.Array();
        //}

        /* public static byte[] GenerateDLMSGmac(byte securityControl, string authenticationKey, string blockCipherKey, byte[] systemTitle, uint invocationCounter)
         {
             byte[] ak = Encoding.ASCII.GetBytes(authenticationKey);
             byte[] ek = Encoding.ASCII.GetBytes(blockCipherKey);

              Build AAD
             byte[] aad = new byte[1 + ak.Length];
             aad[0] = securityControl;
             Buffer.BlockCopy(ak, 0, aad, 1, ak.Length);

              Build Nonce
             byte[] nonce = new byte[12];
             Buffer.BlockCopy(systemTitle, 0, nonce, 0, 8);
             byte[] ic = BitConverter.GetBytes(invocationCounter);
             Array.Reverse(ic);
             Buffer.BlockCopy(ic, 0, nonce, 8, 4);

             byte[] tag = new byte[16];

             using (var aes = new AesGcm(ek))
             {
                 aes.Encrypt(nonce, Array.Empty<byte>(), Array.Empty<byte>(), tag, aad);
             }

              DLMS uses first 12 bytes
             byte[] gmac = new byte[12];
             Array.Copy(tag, gmac, 12);

             return gmac;
         }*/
        public static byte[] GenerateDLMSGmac(byte securityControl, byte[] ek, byte[] ak, byte[] systemTitle, uint invocationCounter, byte[] cipherText = (byte[])null)
        {
            // Build AAD
            GXByteBuffer aad = new GXByteBuffer();
            aad.SetUInt8(securityControl);
            aad.Set(ak);
            byte[] nonce = new byte[12];
            Buffer.BlockCopy(systemTitle, 0, nonce, 0, 8);
            byte[] ic = BitConverter.GetBytes(invocationCounter);
            Array.Reverse(ic);
            Buffer.BlockCopy(ic, 0, nonce, 8, 4);
            //GXDLMSCipheringStream gcm = new GXDLMSCipheringStream(Security.Authentication, true, ek, aad.Array(), nonce, null);
            //byte[] result = gcm.FlushFinalBlock();
            //return gcm.GetTag();
            GXDLMSCipheringStream gcm = new GXDLMSCipheringStream(Security.AuthenticationEncryption, true, ek, aad.Array(), nonce, null);
            gcm.Write(cipherText);
            gcm.FlushFinalBlock();
            return gcm.GetTag();
        }
        public static byte[] GenerateDLMSGmac(byte security, byte[] systemTitle, byte[] icBytes, byte[] cipherText, byte[] tag, byte[] ek)
        {

            byte[] aad = new byte[5];
            aad[0] = security;
            Array.Copy(icBytes, 0, aad, 1, 4);

            byte[] iv = new byte[12];
            Array.Copy(systemTitle, 0, iv, 0, 8);
            Array.Copy(icBytes, 0, iv, 8, 4);

            // Combine cipher + tag
            byte[] input = new byte[cipherText.Length + tag.Length];
            Array.Copy(cipherText, 0, input, 0, cipherText.Length);
            Array.Copy(tag, 0, input, cipherText.Length, tag.Length);

            var cipher = new GcmBlockCipher(new AesEngine());
            var parameters = new AeadParameters(new KeyParameter(ek), 96, iv, aad);
            cipher.Init(false, parameters);

            byte[] output = new byte[cipher.GetOutputSize(input.Length)];
            int len = cipher.ProcessBytes(input, 0, input.Length, output, 0);

            cipher.DoFinal(output, len); // throws if GMAC invalid

            return output.Take(len).ToArray();
        }
        #endregion
    }
}
