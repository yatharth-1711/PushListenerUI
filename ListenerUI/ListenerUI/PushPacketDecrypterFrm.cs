using AutoTest.FrameWork.Converts;
using Gurux.DLMS.Enums;
using Gurux.DLMS.Secure;
using Indali.Common;
using log4net;
using MeterComm.DLMS;
using MeterReader.CommonClasses;
using MeterReader.DLMSNetSerialCommunication;
using MeterReader.TestHelperClasses;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MeterReader.HelperForms
{
    public partial class PushPacketDecrypterFrm : Form
    {
        #region Global Variables and Handlers
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
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
        #endregion
        public PushPacketDecrypterFrm()
        {
            InitializeComponent();
            this.AppendColoredTextControlNotifier += TestLogService_AppendColoredTextControlNotifier;
        }
        private void PushPacketDecrypter_Load(object sender, EventArgs e)
        {
            UIStyler.StyleControl(dgPacketsSummary);
            UIStyler.StyleControl(dgPacketsDetail);
            txtSystemTitle.Text = DLMSInfo.TxtSysT;
            txtBlockCipherKey.Text = DLMSInfo.TxtEK;
            txtAuthenticationKey.Text = DLMSInfo.TxtAK;
            pnlSummarizedPacketTable.Width = (int)(this.Size.Width * 0.5);
            dtDecryptedSummary.Columns.Add("Timestamp", typeof(string));
            dtDecryptedSummary.Columns.Add("Type", typeof(string));
            dtDecryptedSummary.Columns.Add("Packet Length", typeof(int));
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
            try
            {
                ResetLBLPacketCount();
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
                List<string> packets = new List<string>();
                string[] lines = rawText
                    .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                int blockCount = 0;
                StringBuilder multiBlock = new StringBuilder();
                //Creating Packets in datatable
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

                    }
                }
                if (packets.Count == 0)
                {
                    packets.Add(rawText.Trim());
                }
                //Decrypting Packets
                foreach (var packet in packets)
                {
                    try
                    {
                        string DecryptedData = null;
                        string decodedXml = null;
                        string typeOfData = string.Empty;

                        TCPTestNotifier tcp = new TCPTestNotifier();
                        List<byte> receivedBytes = new List<byte>();

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

                        //txtPlainResult.AppendText($"\n{packetSendingTime.ToString("dd/MM/yyyy hh:mm:ss tt")}-{typeOfData}:\n{DecryptedData}");
                        //txtXmlResult.AppendText($"\n{packetSendingTime.ToString("dd/MM/yyyy hh:mm:ss tt")}-{typeOfData}:\n{decodedXml}");
                        Color pushColor = GetPushColor(typeOfData);
                        AppendColoredText(txtPlainResult, $"\n{packetSendingTime:dd/MM/yyyy hh:mm:ss tt} - {typeOfData}\n", pushColor, true);
                        AppendColoredText(txtPlainResult, DecryptedData + "\n", pushColor, false);
                        AppendColoredText(txtXmlResult, $"\n{packetSendingTime:dd/MM/yyyy hh:mm:ss tt} - {typeOfData}\n", pushColor, true);
                        AppendColoredText(txtXmlResult, decodedXml + "\n", pushColor, false);

                        dtDecryptedSummary.Rows.Add(
                            packetSendingTime.ToString("dd/MM/yyyy hh:mm:ss tt"),
                            typeOfData,
                            normalized.Length,
                            deviceId,
                            "Success",
                            DecryptedData);
                    }
                    catch (Exception ex)
                    {
                        dtDecryptedSummary.Rows.Add(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"), "Unknown", packet.Length, "", "Failed", ex.Message);
                        AppendColoredText(txtPlainResult, "\n[DECRYPT FAILED] Corrupted or invalid packet detected\n", Color.Red, true);
                        continue;
                    }
                }
                dgPacketsSummary.DataSource = dtDecryptedSummary;
                //Decrypted Packets Detail
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
                        }
                        input_Break_Final = (input_Break_Profile.Count > 0) ? input_Break_Final : input_Break_Initial;
                        foreach (var item in input_Break_Final)
                        {
                            rawPushPacketTable.Rows.Add(item.ToString());
                        }
                        string columnNameData = $"Push Data-{dtDecryptedSummary.Rows[i]["Timestamp"]}-{dtDecryptedSummary.Rows[i]["Type"]}";
                        string columnNameValue = $"Push Value-{dtDecryptedSummary.Rows[i]["Timestamp"]}-{dtDecryptedSummary.Rows[i]["Type"]}";
                        //dtDecryptedPacketDetail.Columns.Add(columnNameData, typeof(string));
                        //dtDecryptedPacketDetail.Columns.Add(columnNameValue, typeof(string));
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

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Decryption failed: {ex.Message}", "Decryption Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                for (int i = 0; i < dgPacketsSummary.Columns.Count; i++)
                {
                    dgPacketsSummary.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                for (int i = 0; i < dgPacketsDetail.Columns.Count; i++)
                {
                    dgPacketsDetail.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
            }

        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                txtCipherPacket.Clear();
                lblFilePath.Text = "📄 File: No file loaded";
                ResetLBLPacketCount();
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
        #endregion
    }
}
