using System.Windows.Forms;

namespace MeterReader.HelperForms
{
    partial class PushPacketDecrypterFrm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.GroupBox grpSettings;
        private System.Windows.Forms.Label lblSystemTitle;
        private System.Windows.Forms.Label lblBlockCipherKey;
        private System.Windows.Forms.Label lblAuthenticationKey;
        private System.Windows.Forms.TextBox txtSystemTitle;
        private System.Windows.Forms.TextBox txtBlockCipherKey;
        private System.Windows.Forms.TextBox txtAuthenticationKey;

        private System.Windows.Forms.GroupBox grpCipherInput;

        private System.Windows.Forms.TabControl tabResults;
        private System.Windows.Forms.TabPage tabPlain;
        private System.Windows.Forms.TabPage tabXML;
        private System.Windows.Forms.RichTextBox txtPlainResult;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.grpSettings = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblSystemTitle = new System.Windows.Forms.Label();
            this.lblBlockCipherKey = new System.Windows.Forms.Label();
            this.lblAuthenticationKey = new System.Windows.Forms.Label();
            this.txtAuthenticationKey = new System.Windows.Forms.TextBox();
            this.txtSystemTitle = new System.Windows.Forms.TextBox();
            this.txtBlockCipherKey = new System.Windows.Forms.TextBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnDecrypt = new System.Windows.Forms.Button();
            this.grpCipherInput = new System.Windows.Forms.GroupBox();
            this.txtCipherPacket = new System.Windows.Forms.RichTextBox();
            this.tabResults = new System.Windows.Forms.TabControl();
            this.tabPlain = new System.Windows.Forms.TabPage();
            this.txtPlainResult = new System.Windows.Forms.RichTextBox();
            this.tabXML = new System.Windows.Forms.TabPage();
            this.txtXmlResult = new System.Windows.Forms.RichTextBox();
            this.tabMP = new System.Windows.Forms.TabPage();
            this.dgvPackets = new System.Windows.Forms.DataGridView();
            this.tlpSettings = new System.Windows.Forms.TableLayoutPanel();
            this.btnImportFile = new System.Windows.Forms.Button();
            this.lblFilePath = new System.Windows.Forms.Label();
            this.headerTitle = new System.Windows.Forms.Label();
            this.tablelayout_Header = new System.Windows.Forms.TableLayoutPanel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.tableLayout_Cipher_Buttons = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPnl_PacketCounts = new System.Windows.Forms.TableLayoutPanel();
            this.lblTamperCount = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lblTotalCount = new System.Windows.Forms.Label();
            this.lblAlertCount = new System.Windows.Forms.Label();
            this.lblCBCount = new System.Windows.Forms.Label();
            this.lblSRCount = new System.Windows.Forms.Label();
            this.lblBillCount = new System.Windows.Forms.Label();
            this.lblDECount = new System.Windows.Forms.Label();
            this.lblLSCount = new System.Windows.Forms.Label();
            this.lblInstantCount = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.dgPacketsDetail = new System.Windows.Forms.DataGridView();
            this.grpSettings.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.grpCipherInput.SuspendLayout();
            this.tabResults.SuspendLayout();
            this.tabPlain.SuspendLayout();
            this.tabXML.SuspendLayout();
            this.tabMP.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPackets)).BeginInit();
            this.tlpSettings.SuspendLayout();
            this.tablelayout_Header.SuspendLayout();
            this.tableLayout_Cipher_Buttons.SuspendLayout();
            this.tableLayoutPnl_PacketCounts.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgPacketsDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // grpSettings
            // 
            this.grpSettings.Controls.Add(this.tableLayoutPanel1);
            this.grpSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpSettings.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.grpSettings.Location = new System.Drawing.Point(0, 58);
            this.grpSettings.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpSettings.Name = "grpSettings";
            this.grpSettings.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpSettings.Size = new System.Drawing.Size(1657, 61);
            this.grpSettings.TabIndex = 1;
            this.grpSettings.TabStop = false;
            this.grpSettings.Text = " Decryption Settings ";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.596F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.71714F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.Controls.Add(this.lblSystemTitle, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblBlockCipherKey, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblAuthenticationKey, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtAuthenticationKey, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtSystemTitle, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtBlockCipherKey, 3, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 24);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1651, 35);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // lblSystemTitle
            // 
            this.lblSystemTitle.AutoSize = true;
            this.lblSystemTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSystemTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSystemTitle.Location = new System.Drawing.Point(3, 0);
            this.lblSystemTitle.Name = "lblSystemTitle";
            this.lblSystemTitle.Size = new System.Drawing.Size(269, 35);
            this.lblSystemTitle.TabIndex = 0;
            this.lblSystemTitle.Text = "System Title (Hex):";
            this.lblSystemTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblBlockCipherKey
            // 
            this.lblBlockCipherKey.AutoSize = true;
            this.lblBlockCipherKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBlockCipherKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBlockCipherKey.Location = new System.Drawing.Point(552, 0);
            this.lblBlockCipherKey.Name = "lblBlockCipherKey";
            this.lblBlockCipherKey.Size = new System.Drawing.Size(270, 35);
            this.lblBlockCipherKey.TabIndex = 1;
            this.lblBlockCipherKey.Text = "Block Cipher Key (Ek):";
            this.lblBlockCipherKey.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAuthenticationKey
            // 
            this.lblAuthenticationKey.AutoSize = true;
            this.lblAuthenticationKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAuthenticationKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAuthenticationKey.Location = new System.Drawing.Point(1103, 0);
            this.lblAuthenticationKey.Name = "lblAuthenticationKey";
            this.lblAuthenticationKey.Size = new System.Drawing.Size(269, 35);
            this.lblAuthenticationKey.TabIndex = 2;
            this.lblAuthenticationKey.Text = "Authentication Key (Ak):";
            this.lblAuthenticationKey.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAuthenticationKey
            // 
            this.txtAuthenticationKey.BackColor = System.Drawing.SystemColors.Control;
            this.txtAuthenticationKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAuthenticationKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAuthenticationKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAuthenticationKey.Location = new System.Drawing.Point(1378, 2);
            this.txtAuthenticationKey.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtAuthenticationKey.Name = "txtAuthenticationKey";
            this.txtAuthenticationKey.Size = new System.Drawing.Size(270, 27);
            this.txtAuthenticationKey.TabIndex = 5;
            // 
            // txtSystemTitle
            // 
            this.txtSystemTitle.BackColor = System.Drawing.SystemColors.Control;
            this.txtSystemTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSystemTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSystemTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSystemTitle.Location = new System.Drawing.Point(278, 2);
            this.txtSystemTitle.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtSystemTitle.Name = "txtSystemTitle";
            this.txtSystemTitle.Size = new System.Drawing.Size(268, 27);
            this.txtSystemTitle.TabIndex = 3;
            // 
            // txtBlockCipherKey
            // 
            this.txtBlockCipherKey.BackColor = System.Drawing.SystemColors.Control;
            this.txtBlockCipherKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBlockCipherKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBlockCipherKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBlockCipherKey.Location = new System.Drawing.Point(828, 2);
            this.txtBlockCipherKey.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtBlockCipherKey.Name = "txtBlockCipherKey";
            this.txtBlockCipherKey.Size = new System.Drawing.Size(269, 27);
            this.txtBlockCipherKey.TabIndex = 4;
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.IndianRed;
            this.btnClear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(1521, 2);
            this.btnClear.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(127, 37);
            this.btnClear.TabIndex = 10;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnDecrypt
            // 
            this.btnDecrypt.BackColor = System.Drawing.Color.SteelBlue;
            this.btnDecrypt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDecrypt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDecrypt.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDecrypt.ForeColor = System.Drawing.Color.White;
            this.btnDecrypt.Location = new System.Drawing.Point(1389, 2);
            this.btnDecrypt.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnDecrypt.Name = "btnDecrypt";
            this.btnDecrypt.Size = new System.Drawing.Size(126, 37);
            this.btnDecrypt.TabIndex = 9;
            this.btnDecrypt.Text = "Decrypt";
            this.btnDecrypt.UseVisualStyleBackColor = false;
            this.btnDecrypt.Click += new System.EventHandler(this.btnDecrypt_Click);
            // 
            // grpCipherInput
            // 
            this.grpCipherInput.Controls.Add(this.txtCipherPacket);
            this.grpCipherInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpCipherInput.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.grpCipherInput.Location = new System.Drawing.Point(3, 2);
            this.grpCipherInput.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpCipherInput.Name = "grpCipherInput";
            this.grpCipherInput.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpCipherInput.Size = new System.Drawing.Size(1651, 189);
            this.grpCipherInput.TabIndex = 2;
            this.grpCipherInput.TabStop = false;
            this.grpCipherInput.Text = " Ciphered Push Packet ";
            // 
            // txtCipherPacket
            // 
            this.txtCipherPacket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCipherPacket.Location = new System.Drawing.Point(3, 24);
            this.txtCipherPacket.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtCipherPacket.Name = "txtCipherPacket";
            this.txtCipherPacket.Size = new System.Drawing.Size(1645, 163);
            this.txtCipherPacket.TabIndex = 0;
            this.txtCipherPacket.Text = "";
            // 
            // tabResults
            // 
            this.tabResults.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabResults.Controls.Add(this.tabPlain);
            this.tabResults.Controls.Add(this.tabXML);
            this.tabResults.Controls.Add(this.tabMP);
            this.tabResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabResults.Location = new System.Drawing.Point(0, 0);
            this.tabResults.Margin = new System.Windows.Forms.Padding(0);
            this.tabResults.Name = "tabResults";
            this.tabResults.SelectedIndex = 0;
            this.tabResults.Size = new System.Drawing.Size(828, 411);
            this.tabResults.TabIndex = 5;
            // 
            // tabPlain
            // 
            this.tabPlain.BackColor = System.Drawing.SystemColors.Window;
            this.tabPlain.Controls.Add(this.txtPlainResult);
            this.tabPlain.Location = new System.Drawing.Point(4, 28);
            this.tabPlain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPlain.Name = "tabPlain";
            this.tabPlain.Size = new System.Drawing.Size(820, 379);
            this.tabPlain.TabIndex = 0;
            this.tabPlain.Text = " Decrypted Data";
            // 
            // txtPlainResult
            // 
            this.txtPlainResult.BackColor = System.Drawing.SystemColors.Control;
            this.txtPlainResult.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPlainResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPlainResult.Font = new System.Drawing.Font("Consolas", 10F);
            this.txtPlainResult.Location = new System.Drawing.Point(0, 0);
            this.txtPlainResult.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtPlainResult.Name = "txtPlainResult";
            this.txtPlainResult.Size = new System.Drawing.Size(820, 379);
            this.txtPlainResult.TabIndex = 0;
            this.txtPlainResult.Text = "";
            // 
            // tabXML
            // 
            this.tabXML.BackColor = System.Drawing.SystemColors.Window;
            this.tabXML.Controls.Add(this.txtXmlResult);
            this.tabXML.Location = new System.Drawing.Point(4, 28);
            this.tabXML.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabXML.Name = "tabXML";
            this.tabXML.Size = new System.Drawing.Size(820, 379);
            this.tabXML.TabIndex = 1;
            this.tabXML.Text = " XML View ";
            // 
            // txtXmlResult
            // 
            this.txtXmlResult.BackColor = System.Drawing.SystemColors.Control;
            this.txtXmlResult.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtXmlResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtXmlResult.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtXmlResult.Location = new System.Drawing.Point(0, 0);
            this.txtXmlResult.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtXmlResult.Name = "txtXmlResult";
            this.txtXmlResult.Size = new System.Drawing.Size(820, 379);
            this.txtXmlResult.TabIndex = 0;
            this.txtXmlResult.Text = "";
            // 
            // tabMP
            // 
            this.tabMP.BackColor = System.Drawing.SystemColors.Control;
            this.tabMP.Controls.Add(this.dgvPackets);
            this.tabMP.Location = new System.Drawing.Point(4, 28);
            this.tabMP.Margin = new System.Windows.Forms.Padding(4);
            this.tabMP.Name = "tabMP";
            this.tabMP.Padding = new System.Windows.Forms.Padding(4);
            this.tabMP.Size = new System.Drawing.Size(820, 379);
            this.tabMP.TabIndex = 2;
            this.tabMP.Text = "Multiple Packets";
            // 
            // dgvPackets
            // 
            this.dgvPackets.AllowUserToAddRows = false;
            this.dgvPackets.AllowUserToDeleteRows = false;
            this.dgvPackets.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPackets.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dgvPackets.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvPackets.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvPackets.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvPackets.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPackets.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvPackets.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPackets.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvPackets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPackets.EnableHeadersVisualStyles = false;
            this.dgvPackets.GridColor = System.Drawing.SystemColors.Control;
            this.dgvPackets.Location = new System.Drawing.Point(4, 4);
            this.dgvPackets.Margin = new System.Windows.Forms.Padding(0);
            this.dgvPackets.Name = "dgvPackets";
            this.dgvPackets.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPackets.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvPackets.RowHeadersVisible = false;
            this.dgvPackets.RowHeadersWidth = 51;
            this.dgvPackets.Size = new System.Drawing.Size(812, 371);
            this.dgvPackets.TabIndex = 1;
            // 
            // tlpSettings
            // 
            this.tlpSettings.ColumnCount = 5;
            this.tlpSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tlpSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36F));
            this.tlpSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8F));
            this.tlpSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8F));
            this.tlpSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8F));
            this.tlpSettings.Controls.Add(this.btnImportFile, 2, 0);
            this.tlpSettings.Controls.Add(this.btnClear, 4, 0);
            this.tlpSettings.Controls.Add(this.btnDecrypt, 3, 0);
            this.tlpSettings.Controls.Add(this.lblFilePath, 1, 0);
            this.tlpSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpSettings.Location = new System.Drawing.Point(3, 195);
            this.tlpSettings.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tlpSettings.Name = "tlpSettings";
            this.tlpSettings.RowCount = 1;
            this.tlpSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSettings.Size = new System.Drawing.Size(1651, 41);
            this.tlpSettings.TabIndex = 7;
            // 
            // btnImportFile
            // 
            this.btnImportFile.BackColor = System.Drawing.SystemColors.Control;
            this.btnImportFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnImportFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImportFile.ForeColor = System.Drawing.Color.Black;
            this.btnImportFile.Location = new System.Drawing.Point(1257, 2);
            this.btnImportFile.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnImportFile.Name = "btnImportFile";
            this.btnImportFile.Size = new System.Drawing.Size(126, 37);
            this.btnImportFile.TabIndex = 8;
            this.btnImportFile.Text = "Import File";
            this.btnImportFile.UseVisualStyleBackColor = false;
            this.btnImportFile.Click += new System.EventHandler(this.btnImportFile_Click);
            // 
            // lblFilePath
            // 
            this.lblFilePath.AutoSize = true;
            this.lblFilePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFilePath.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFilePath.ForeColor = System.Drawing.Color.Indigo;
            this.lblFilePath.Location = new System.Drawing.Point(664, 0);
            this.lblFilePath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFilePath.Name = "lblFilePath";
            this.lblFilePath.Size = new System.Drawing.Size(586, 41);
            this.lblFilePath.TabIndex = 9;
            this.lblFilePath.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // headerTitle
            // 
            this.headerTitle.AutoSize = true;
            this.headerTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(94)))), ((int)(((byte)(168)))));
            this.headerTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.headerTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.headerTitle.ForeColor = System.Drawing.Color.White;
            this.headerTitle.Location = new System.Drawing.Point(3, 0);
            this.headerTitle.Name = "headerTitle";
            this.headerTitle.Size = new System.Drawing.Size(1651, 58);
            this.headerTitle.TabIndex = 0;
            this.headerTitle.Text = "Push Packet Decrypter";
            this.headerTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tablelayout_Header
            // 
            this.tablelayout_Header.ColumnCount = 1;
            this.tablelayout_Header.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tablelayout_Header.Controls.Add(this.headerTitle, 0, 0);
            this.tablelayout_Header.Dock = System.Windows.Forms.DockStyle.Top;
            this.tablelayout_Header.Location = new System.Drawing.Point(0, 0);
            this.tablelayout_Header.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tablelayout_Header.Name = "tablelayout_Header";
            this.tablelayout_Header.RowCount = 1;
            this.tablelayout_Header.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tablelayout_Header.Size = new System.Drawing.Size(1657, 58);
            this.tablelayout_Header.TabIndex = 8;
            // 
            // splitter1
            // 
            this.splitter1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(94)))), ((int)(((byte)(168)))));
            this.splitter1.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(0, 357);
            this.splitter1.Margin = new System.Windows.Forms.Padding(4);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(1657, 6);
            this.splitter1.TabIndex = 9;
            this.splitter1.TabStop = false;
            // 
            // tableLayout_Cipher_Buttons
            // 
            this.tableLayout_Cipher_Buttons.ColumnCount = 1;
            this.tableLayout_Cipher_Buttons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayout_Cipher_Buttons.Controls.Add(this.grpCipherInput, 0, 0);
            this.tableLayout_Cipher_Buttons.Controls.Add(this.tlpSettings, 0, 1);
            this.tableLayout_Cipher_Buttons.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayout_Cipher_Buttons.Location = new System.Drawing.Point(0, 119);
            this.tableLayout_Cipher_Buttons.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayout_Cipher_Buttons.Name = "tableLayout_Cipher_Buttons";
            this.tableLayout_Cipher_Buttons.RowCount = 2;
            this.tableLayout_Cipher_Buttons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayout_Cipher_Buttons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayout_Cipher_Buttons.Size = new System.Drawing.Size(1657, 238);
            this.tableLayout_Cipher_Buttons.TabIndex = 10;
            // 
            // tableLayoutPnl_PacketCounts
            // 
            this.tableLayoutPnl_PacketCounts.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPnl_PacketCounts.ColumnCount = 9;
            this.tableLayoutPnl_PacketCounts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPnl_PacketCounts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPnl_PacketCounts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPnl_PacketCounts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPnl_PacketCounts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPnl_PacketCounts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPnl_PacketCounts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPnl_PacketCounts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPnl_PacketCounts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPnl_PacketCounts.Controls.Add(this.lblTamperCount, 7, 1);
            this.tableLayoutPnl_PacketCounts.Controls.Add(this.label9, 7, 0);
            this.tableLayoutPnl_PacketCounts.Controls.Add(this.lblTotalCount, 8, 1);
            this.tableLayoutPnl_PacketCounts.Controls.Add(this.lblAlertCount, 6, 1);
            this.tableLayoutPnl_PacketCounts.Controls.Add(this.lblCBCount, 5, 1);
            this.tableLayoutPnl_PacketCounts.Controls.Add(this.lblSRCount, 4, 1);
            this.tableLayoutPnl_PacketCounts.Controls.Add(this.lblBillCount, 3, 1);
            this.tableLayoutPnl_PacketCounts.Controls.Add(this.lblDECount, 2, 1);
            this.tableLayoutPnl_PacketCounts.Controls.Add(this.lblLSCount, 1, 1);
            this.tableLayoutPnl_PacketCounts.Controls.Add(this.lblInstantCount, 0, 1);
            this.tableLayoutPnl_PacketCounts.Controls.Add(this.label8, 8, 0);
            this.tableLayoutPnl_PacketCounts.Controls.Add(this.label7, 6, 0);
            this.tableLayoutPnl_PacketCounts.Controls.Add(this.label6, 5, 0);
            this.tableLayoutPnl_PacketCounts.Controls.Add(this.label5, 4, 0);
            this.tableLayoutPnl_PacketCounts.Controls.Add(this.label4, 3, 0);
            this.tableLayoutPnl_PacketCounts.Controls.Add(this.label3, 2, 0);
            this.tableLayoutPnl_PacketCounts.Controls.Add(this.label2, 1, 0);
            this.tableLayoutPnl_PacketCounts.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPnl_PacketCounts.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPnl_PacketCounts.Location = new System.Drawing.Point(0, 774);
            this.tableLayoutPnl_PacketCounts.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPnl_PacketCounts.Name = "tableLayoutPnl_PacketCounts";
            this.tableLayoutPnl_PacketCounts.RowCount = 2;
            this.tableLayoutPnl_PacketCounts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPnl_PacketCounts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPnl_PacketCounts.Size = new System.Drawing.Size(1657, 51);
            this.tableLayoutPnl_PacketCounts.TabIndex = 11;
            // 
            // lblTamperCount
            // 
            this.lblTamperCount.AutoSize = true;
            this.lblTamperCount.BackColor = System.Drawing.Color.Pink;
            this.lblTamperCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTamperCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTamperCount.Location = new System.Drawing.Point(1291, 25);
            this.lblTamperCount.Name = "lblTamperCount";
            this.lblTamperCount.Size = new System.Drawing.Size(178, 26);
            this.lblTamperCount.TabIndex = 18;
            this.lblTamperCount.Text = "-";
            this.lblTamperCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Pink;
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(1291, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(178, 25);
            this.label9.TabIndex = 17;
            this.label9.Text = "Tamper";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTotalCount
            // 
            this.lblTotalCount.AutoSize = true;
            this.lblTotalCount.BackColor = System.Drawing.SystemColors.Control;
            this.lblTotalCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTotalCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalCount.Location = new System.Drawing.Point(1475, 25);
            this.lblTotalCount.Name = "lblTotalCount";
            this.lblTotalCount.Size = new System.Drawing.Size(179, 26);
            this.lblTotalCount.TabIndex = 16;
            this.lblTotalCount.Text = "-";
            this.lblTotalCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAlertCount
            // 
            this.lblAlertCount.AutoSize = true;
            this.lblAlertCount.BackColor = System.Drawing.Color.SlateGray;
            this.lblAlertCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAlertCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAlertCount.Location = new System.Drawing.Point(1107, 25);
            this.lblAlertCount.Name = "lblAlertCount";
            this.lblAlertCount.Size = new System.Drawing.Size(178, 26);
            this.lblAlertCount.TabIndex = 15;
            this.lblAlertCount.Text = "-";
            this.lblAlertCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCBCount
            // 
            this.lblCBCount.AutoSize = true;
            this.lblCBCount.BackColor = System.Drawing.Color.Cyan;
            this.lblCBCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCBCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCBCount.Location = new System.Drawing.Point(923, 25);
            this.lblCBCount.Name = "lblCBCount";
            this.lblCBCount.Size = new System.Drawing.Size(178, 26);
            this.lblCBCount.TabIndex = 14;
            this.lblCBCount.Text = "-";
            this.lblCBCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSRCount
            // 
            this.lblSRCount.AutoSize = true;
            this.lblSRCount.BackColor = System.Drawing.Color.LightGreen;
            this.lblSRCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSRCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSRCount.Location = new System.Drawing.Point(739, 25);
            this.lblSRCount.Name = "lblSRCount";
            this.lblSRCount.Size = new System.Drawing.Size(178, 26);
            this.lblSRCount.TabIndex = 13;
            this.lblSRCount.Text = "-";
            this.lblSRCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblBillCount
            // 
            this.lblBillCount.AutoSize = true;
            this.lblBillCount.BackColor = System.Drawing.Color.PeachPuff;
            this.lblBillCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBillCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBillCount.Location = new System.Drawing.Point(555, 25);
            this.lblBillCount.Name = "lblBillCount";
            this.lblBillCount.Size = new System.Drawing.Size(178, 26);
            this.lblBillCount.TabIndex = 12;
            this.lblBillCount.Text = "-";
            this.lblBillCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDECount
            // 
            this.lblDECount.AutoSize = true;
            this.lblDECount.BackColor = System.Drawing.Color.MistyRose;
            this.lblDECount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDECount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDECount.Location = new System.Drawing.Point(371, 25);
            this.lblDECount.Name = "lblDECount";
            this.lblDECount.Size = new System.Drawing.Size(178, 26);
            this.lblDECount.TabIndex = 11;
            this.lblDECount.Text = "-";
            this.lblDECount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLSCount
            // 
            this.lblLSCount.AutoSize = true;
            this.lblLSCount.BackColor = System.Drawing.Color.LightCoral;
            this.lblLSCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLSCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLSCount.Location = new System.Drawing.Point(187, 25);
            this.lblLSCount.Name = "lblLSCount";
            this.lblLSCount.Size = new System.Drawing.Size(178, 26);
            this.lblLSCount.TabIndex = 10;
            this.lblLSCount.Text = "-";
            this.lblLSCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblInstantCount
            // 
            this.lblInstantCount.AutoSize = true;
            this.lblInstantCount.BackColor = System.Drawing.Color.RosyBrown;
            this.lblInstantCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblInstantCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInstantCount.Location = new System.Drawing.Point(3, 25);
            this.lblInstantCount.Name = "lblInstantCount";
            this.lblInstantCount.Size = new System.Drawing.Size(178, 26);
            this.lblInstantCount.TabIndex = 9;
            this.lblInstantCount.Text = "-";
            this.lblInstantCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.SystemColors.Control;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(1475, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(179, 25);
            this.label8.TabIndex = 8;
            this.label8.Text = "Total Packets";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.SlateGray;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(1107, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(178, 25);
            this.label7.TabIndex = 7;
            this.label7.Text = "Alert";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Cyan;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(923, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(178, 25);
            this.label6.TabIndex = 6;
            this.label6.Text = "Current Bill";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.LightGreen;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(739, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(178, 25);
            this.label5.TabIndex = 5;
            this.label5.Text = "Self Registration";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.PeachPuff;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(555, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(178, 25);
            this.label4.TabIndex = 4;
            this.label4.Text = "Billing";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.MistyRose;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(371, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(178, 25);
            this.label3.TabIndex = 3;
            this.label3.Text = "Daily Energy";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.LightCoral;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(187, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(178, 25);
            this.label2.TabIndex = 2;
            this.label2.Text = "Load Survey";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.RosyBrown;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(178, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "Instant";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.dgPacketsDetail, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.tabResults, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 363);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1657, 411);
            this.tableLayoutPanel2.TabIndex = 12;
            // 
            // dgPacketsDetail
            // 
            this.dgPacketsDetail.AllowUserToAddRows = false;
            this.dgPacketsDetail.AllowUserToDeleteRows = false;
            this.dgPacketsDetail.AllowUserToResizeRows = false;
            this.dgPacketsDetail.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgPacketsDetail.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dgPacketsDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgPacketsDetail.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(94)))), ((int)(((byte)(168)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgPacketsDetail.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgPacketsDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgPacketsDetail.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgPacketsDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgPacketsDetail.EnableHeadersVisualStyles = false;
            this.dgPacketsDetail.GridColor = System.Drawing.SystemColors.Control;
            this.dgPacketsDetail.Location = new System.Drawing.Point(831, 2);
            this.dgPacketsDetail.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgPacketsDetail.Name = "dgPacketsDetail";
            this.dgPacketsDetail.ReadOnly = true;
            this.dgPacketsDetail.RowHeadersVisible = false;
            this.dgPacketsDetail.RowHeadersWidth = 50;
            this.dgPacketsDetail.RowTemplate.Height = 24;
            this.dgPacketsDetail.Size = new System.Drawing.Size(823, 407);
            this.dgPacketsDetail.TabIndex = 6;
            // 
            // PushPacketDecrypterFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1657, 825);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.tableLayout_Cipher_Buttons);
            this.Controls.Add(this.tableLayoutPnl_PacketCounts);
            this.Controls.Add(this.grpSettings);
            this.Controls.Add(this.tablelayout_Header);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "PushPacketDecrypterFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Push Packet Decrypter";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.PushPacketDecrypterFrm_Load);
            this.grpSettings.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.grpCipherInput.ResumeLayout(false);
            this.tabResults.ResumeLayout(false);
            this.tabPlain.ResumeLayout(false);
            this.tabXML.ResumeLayout(false);
            this.tabMP.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPackets)).EndInit();
            this.tlpSettings.ResumeLayout(false);
            this.tlpSettings.PerformLayout();
            this.tablelayout_Header.ResumeLayout(false);
            this.tablelayout_Header.PerformLayout();
            this.tableLayout_Cipher_Buttons.ResumeLayout(false);
            this.tableLayoutPnl_PacketCounts.ResumeLayout(false);
            this.tableLayoutPnl_PacketCounts.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgPacketsDetail)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.TableLayoutPanel tlpSettings;
        private System.Windows.Forms.Label headerTitle;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.RichTextBox txtCipherPacket;
        private System.Windows.Forms.RichTextBox txtXmlResult;
        private System.Windows.Forms.Button btnDecrypt;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.TableLayoutPanel tablelayout_Header;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.TableLayoutPanel tableLayout_Cipher_Buttons;
        private System.Windows.Forms.Button btnImportFile;
        private System.Windows.Forms.Label lblFilePath;
        private System.Windows.Forms.DataGridView dgvPackets;
        private System.Windows.Forms.TabPage tabMP;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPnl_PacketCounts;
        private System.Windows.Forms.Label lblTotalCount;
        private System.Windows.Forms.Label lblAlertCount;
        private System.Windows.Forms.Label lblCBCount;
        private System.Windows.Forms.Label lblSRCount;
        private System.Windows.Forms.Label lblBillCount;
        private System.Windows.Forms.Label lblDECount;
        private System.Windows.Forms.Label lblLSCount;
        private System.Windows.Forms.Label lblInstantCount;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTamperCount;
        private System.Windows.Forms.Label label9;
        private TableLayoutPanel tableLayoutPanel2;
        private DataGridView dgPacketsDetail;
    }
}
