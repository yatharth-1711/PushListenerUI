using AutoTest.FrameWork.Converts;
using Gurux.DLMS.Enums;
using Gurux.DLMS.Secure;
using Indali.Common;
using MeterComm.DLMS;
using MeterReader.DLMSNetSerialCommunication;
using MeterReader.TestHelperClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MeterReader.HelperForms
{
    public partial class PushPacketDecrypterFrm : Form
    {
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
        DataTable targetTable = new DataTable();
        public PushPacketDecrypterFrm()
        {
            InitializeComponent();
        }
        private void PushPacketDecrypterFrm_Load(object sender, EventArgs e)
        {
            //UIStyler.StyleControl(txtCipherPacket);
            UIStyler.StyleControl(dgvPackets);
            //UIStyler.StyleControl(dgPacketsDetail);
            //UIStyler.StyleControl(tableLayoutPnl_PacketCounts);
            txtSystemTitle.Text = DLMSInfo.TxtSysT;
            txtBlockCipherKey.Text = DLMSInfo.TxtEK;
            txtAuthenticationKey.Text = DLMSInfo.TxtAK;

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
            ResetLBLPacketCount();
            try
            {
                string rawText = txtCipherPacket.Text;
                bool indaliFile = rawText.Contains("CIPHERED DATA");
                if (string.IsNullOrWhiteSpace(rawText))
                {
                    MessageBox.Show("No data to decrypt.", "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (string.IsNullOrEmpty(txtBlockCipherKey.Text) ||
                    string.IsNullOrEmpty(txtSystemTitle.Text) ||
                    string.IsNullOrEmpty(txtAuthenticationKey.Text))
                {
                    MessageBox.Show("Invalid Keys", "Invalid Key",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                EK = Encoding.ASCII.GetBytes(txtBlockCipherKey.Text.Trim());
                AK = Encoding.ASCII.GetBytes(txtAuthenticationKey.Text.Trim());
                ST = Encoding.ASCII.GetBytes(txtSystemTitle.Text.Trim());
                GXSecurity = Security.AuthenticationEncryption;

                txtPlainResult.Clear();
                txtXmlResult.Clear();

                // TABLE TO SHOW RESULT
                DataTable decryptedPackets = new DataTable();
                decryptedPackets.Columns.Add("Timestamp", typeof(string));
                decryptedPackets.Columns.Add("Type", typeof(string));
                decryptedPackets.Columns.Add("Packet Length", typeof(int));
                decryptedPackets.Columns.Add("Device ID", typeof(string));
                decryptedPackets.Columns.Add("Status", typeof(string));
                decryptedPackets.Columns.Add("Decrypted Data", typeof(string));

                List<string> packets = new List<string>();

                string[] lines = rawText
                    .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                int blockCount = 0;
                StringBuilder multiBlock = new StringBuilder();

                foreach (string line in lines)
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
                if (packets.Count == 0)
                {
                    packets.Add(rawText.Trim());
                }
                foreach (var packet in packets)
                {
                    string DecryptedData = null;
                    string decodedXml = null;
                    string typeOfData = string.Empty;

                    TCPTestNotifier tcp = new TCPTestNotifier();
                    List<byte> receivedBytes = new List<byte>();

                    try
                    {
                        string normalized = packet.Replace(" ", "").ToUpper();

                        string[] EncPackets =
                            normalized.Split(new string[] { "00010001" }, StringSplitOptions.None);

                        for (int i = 1; i < EncPackets.Length; i++)
                        {
                            EncPackets[i] = "00010001" + EncPackets[i];

                            cipherText = TSTCommon.HexToBytes(
                                tcp.Find_DB08(EncPackets[i]));

                            DecryptedData += tcp.DecryptData(
                                true, EK, AK, ST, GXSecurity, IC, cipherText) + "\n";
                        }

                        // XML decode
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

                        // extract metadata (same logic you already used)
                        string sData = DecryptedData.Replace(" ", "");
                        int counter = 10;

                        DateTime packetSendingTime =
                            DateTime.ParseExact(
                                parse.Getdate(sData.Substring(12, 24), 0, false),
                                "dd/MM/yyyy HH:mm:ss",
                                CultureInfo.InvariantCulture);

                        counter = 36;

                        while (sData.Substring(counter, 2) != "02")
                            counter += 4;

                        counter += 4;

                        int length = Convert.ToInt32(sData.Substring(counter + 2, 2), 16) * 2;

                        string deviceId =
                            parse.GetProfileValueString(
                                sData.Substring(counter, length + 4));

                        GetPacketCounter(typeOfData);

                        txtPlainResult.AppendText($"\n{packetSendingTime.ToString("dd/MM/yyyy hh:mm:ss tt")}-{typeOfData}:\n{DecryptedData}");
                        txtXmlResult.AppendText($"\n{packetSendingTime.ToString("dd/MM/yyyy hh:mm:ss tt")}-{typeOfData}:\n{decodedXml}");

                        decryptedPackets.Rows.Add(
                            packetSendingTime.ToString("dd/MM/yyyy hh:mm:ss tt"),
                            typeOfData,
                            normalized.Length,
                            deviceId,
                            "Success",
                            DecryptedData);
                    }
                    catch
                    {
                        decryptedPackets.Rows.Add("", "Unknown", packet.Length, "", "Failed", "");
                        MessageBox.Show("Could Not Decrypt Packet", "Decrypt Failed",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                dgvPackets.DataSource = decryptedPackets;
                for (int i = 0; i < decryptedPackets.Rows.Count; i++)
                {
                    DataTable rawPushPacketTable = new DataTable();
                    rawPushPacketTable.Columns.Add($"Push Data", typeof(string));
                    List<string> details = new List<string>();
                    int totalProfileParameters = 0;
                    string input = decryptedPackets.Rows[i]["Decrypted Data"].ToString().Replace(" ", "");
                    int pointer = 0;
                    try
                    {
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
                        string lastValue = input_Break_Initial.Last();
                        int ptr = 4;
                        if (decryptedPackets.Rows[i][1].ToString() == "Load Survey")
                        {
                            totalProfileParameters += Convert.ToInt32(lastValue.Substring(ptr + 2, 2), 16);
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

                        foreach (var item in input_Break_Final)
                        {
                            rawPushPacketTable.Rows.Add(item.ToString());
                        }
                        string columnNameData = $"Push Data-{decryptedPackets.Rows[i]["Timestamp"]}";
                        string columnNameValue = $"Push Value-{decryptedPackets.Rows[i]["Timestamp"]}";
                        targetTable.Columns.Add(columnNameData, typeof(string));
                        targetTable.Columns.Add(columnNameValue, typeof(string));
                        string[] data = new string[2] { "Data Notification", "Long Invoke Id and Priority" };
                        for (int k = 0; k < 2; k++)
                        {
                            if (rawPushPacketTable.Rows.Count > targetTable.Rows.Count)
                                targetTable.Rows.Add();
                            targetTable.Rows[k][columnNameData] = rawPushPacketTable.Rows[k][0].ToString();
                            targetTable.Rows[k][columnNameValue] = data[k].ToString();
                        }
                        for (int k = 2; k < rawPushPacketTable.Rows.Count; k++)
                        {
                            if (rawPushPacketTable.Rows.Count > targetTable.Rows.Count)
                                targetTable.Rows.Add();
                            targetTable.Rows[k][columnNameData] = rawPushPacketTable.Rows[k][0].ToString();
                            targetTable.Rows[k][columnNameValue] = parse.GetProfileValueString(rawPushPacketTable.Rows[k][0].ToString());
                        }
                        dgPacketsDetail.DataSource = targetTable;
                    }
                    catch { }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Decryption failed.\n\nDetails: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            finally
            {
                foreach (DataGridViewColumn col in dgvPackets.Columns)
                {
                    col.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
            }
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                txtCipherPacket.Clear();
                txtPlainResult.Clear();
                txtXmlResult.Clear();
                dgvPackets.DataSource = null;
                ResetLBLPacketCount();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in clearing data: {ex.Message}");
            }
        }
        public void GetPacketCounter(string typeOfData)
        {
            if (typeOfData == "Instant Push")
            {
                instantCount++;
                lblInstantCount.Text = instantCount.ToString();
            }
            else if (typeOfData == "Alert Push")
            {
                alertCount++;
                lblAlertCount.Text = alertCount.ToString();
            }

            else if (typeOfData == "Load Survey Push")
            {
                lsCount++;
                lblLSCount.Text = lsCount.ToString();
            }
            else if (typeOfData == "Daily Energy Push")
            {
                deCount++;
                lblDECount.Text = deCount.ToString();
            }
            else if (typeOfData == "Self Registration Push")
            {
                srCount++;
                lblSRCount.Text = srCount.ToString();
            }
            else if (typeOfData == "Billing Push")
            {
                billCount++;
                lblBillCount.Text = billCount.ToString();
            }
            else if (typeOfData == "Current Bill Push")
            {
                cbCount++;
                lblCBCount.Text = cbCount.ToString();
            }
            else if (typeOfData == "Tamper Push")
            {
                tamperCount++;
                lblTamperCount.Text = tamperCount.ToString();
            }
            int totalCount = instantCount + alertCount + lsCount + deCount + srCount + billCount + cbCount + tamperCount;
            lblTotalCount.Text = totalCount.ToString();
        }
        public void ResetLBLPacketCount()
        {
            tableLayoutPnl_PacketCounts.Controls.OfType<Label>()
                                                .Where(l => l.Name.StartsWith("lbl"))
                                                .ToList()
                                                .ForEach(l => l.Text = "-");
            instantCount = 0; lsCount = 0; deCount = 0; billCount = 0; srCount = 0; cbCount = 0; alertCount = 0; tamperCount = 0;
        }

    }
}
