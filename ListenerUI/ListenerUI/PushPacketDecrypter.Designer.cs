namespace ListenerUI
{
    partial class PushPacketDecrypter
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tablelayout_Header = new System.Windows.Forms.TableLayoutPanel();
            this.headerTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlCipherPacket = new System.Windows.Forms.Panel();
            this.grpCipherInput = new System.Windows.Forms.GroupBox();
            this.txtCipherPacket = new System.Windows.Forms.RichTextBox();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnDecrypt = new System.Windows.Forms.Button();
            this.btnImportFile = new System.Windows.Forms.Button();
            this.pnlSettings = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblSystemTitle = new System.Windows.Forms.Label();
            this.lblBlockCipherKey = new System.Windows.Forms.Label();
            this.lblAuthenticationKey = new System.Windows.Forms.Label();
            this.txtAuthenticationKey = new System.Windows.Forms.TextBox();
            this.txtSystemTitle = new System.Windows.Forms.TextBox();
            this.txtBlockCipherKey = new System.Windows.Forms.TextBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.pnlPacketCounts = new System.Windows.Forms.Panel();
            this.tableLayoutPnl_PacketCounts = new System.Windows.Forms.TableLayoutPanel();
            this.btnTotalCount = new System.Windows.Forms.Button();
            this.btnTamper = new System.Windows.Forms.Button();
            this.btnAlert = new System.Windows.Forms.Button();
            this.btnCB = new System.Windows.Forms.Button();
            this.btnSR = new System.Windows.Forms.Button();
            this.btnBill = new System.Windows.Forms.Button();
            this.btnDE = new System.Windows.Forms.Button();
            this.btnLS = new System.Windows.Forms.Button();
            this.btnInstant = new System.Windows.Forms.Button();
            this.pnlSummarizedPacketTable = new System.Windows.Forms.Panel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.dgPacketsSummary = new System.Windows.Forms.DataGridView();
            this.lblFilePath = new System.Windows.Forms.Label();
            this.tabResults = new System.Windows.Forms.TabControl();
            this.tabPlain = new System.Windows.Forms.TabPage();
            this.txtPlainResult = new System.Windows.Forms.RichTextBox();
            this.tabXML = new System.Windows.Forms.TabPage();
            this.txtXmlResult = new System.Windows.Forms.RichTextBox();
            this.tabMP = new System.Windows.Forms.TabPage();
            this.dgPacketsDetail = new System.Windows.Forms.DataGridView();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.tablelayout_Header.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlCipherPacket.SuspendLayout();
            this.grpCipherInput.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.pnlSettings.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.pnlPacketCounts.SuspendLayout();
            this.tableLayoutPnl_PacketCounts.SuspendLayout();
            this.pnlSummarizedPacketTable.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgPacketsSummary)).BeginInit();
            this.tabResults.SuspendLayout();
            this.tabPlain.SuspendLayout();
            this.tabXML.SuspendLayout();
            this.tabMP.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgPacketsDetail)).BeginInit();
            this.SuspendLayout();
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
            this.tablelayout_Header.Size = new System.Drawing.Size(1348, 45);
            this.tablelayout_Header.TabIndex = 9;
            // 
            // headerTitle
            // 
            this.headerTitle.AutoSize = true;
            this.headerTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(94)))), ((int)(((byte)(168)))));
            this.headerTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.headerTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.headerTitle.ForeColor = System.Drawing.Color.White;
            this.headerTitle.Location = new System.Drawing.Point(3, 0);
            this.headerTitle.Name = "headerTitle";
            this.headerTitle.Size = new System.Drawing.Size(1342, 45);
            this.headerTitle.TabIndex = 0;
            this.headerTitle.Text = "Push Packet Decrypter";
            this.headerTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pnlCipherPacket);
            this.panel1.Controls.Add(this.pnlButtons);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 86);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1348, 182);
            this.panel1.TabIndex = 10;
            // 
            // pnlCipherPacket
            // 
            this.pnlCipherPacket.Controls.Add(this.grpCipherInput);
            this.pnlCipherPacket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCipherPacket.Location = new System.Drawing.Point(0, 0);
            this.pnlCipherPacket.Name = "pnlCipherPacket";
            this.pnlCipherPacket.Size = new System.Drawing.Size(1203, 182);
            this.pnlCipherPacket.TabIndex = 2;
            // 
            // grpCipherInput
            // 
            this.grpCipherInput.Controls.Add(this.txtCipherPacket);
            this.grpCipherInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpCipherInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpCipherInput.Location = new System.Drawing.Point(0, 0);
            this.grpCipherInput.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpCipherInput.Name = "grpCipherInput";
            this.grpCipherInput.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpCipherInput.Size = new System.Drawing.Size(1203, 182);
            this.grpCipherInput.TabIndex = 3;
            this.grpCipherInput.TabStop = false;
            this.grpCipherInput.Text = " Ciphered Push Packet ";
            // 
            // txtCipherPacket
            // 
            this.txtCipherPacket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCipherPacket.Location = new System.Drawing.Point(3, 22);
            this.txtCipherPacket.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtCipherPacket.Name = "txtCipherPacket";
            this.txtCipherPacket.Size = new System.Drawing.Size(1197, 158);
            this.txtCipherPacket.TabIndex = 0;
            this.txtCipherPacket.Text = "";
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.tableLayoutPanel2);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlButtons.Location = new System.Drawing.Point(1203, 0);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(145, 182);
            this.pnlButtons.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.btnExport, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.btnClear, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.btnDecrypt, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.btnImportFile, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(145, 175);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // btnExport
            // 
            this.btnExport.BackColor = System.Drawing.Color.SteelBlue;
            this.btnExport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnExport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.Location = new System.Drawing.Point(3, 139);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(139, 33);
            this.btnExport.TabIndex = 12;
            this.btnExport.Text = "💾 Export Data";
            this.btnExport.UseVisualStyleBackColor = false;
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.IndianRed;
            this.btnClear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(3, 102);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(139, 31);
            this.btnClear.TabIndex = 11;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnDecrypt
            // 
            this.btnDecrypt.BackColor = System.Drawing.Color.SteelBlue;
            this.btnDecrypt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDecrypt.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDecrypt.ForeColor = System.Drawing.Color.White;
            this.btnDecrypt.Location = new System.Drawing.Point(3, 65);
            this.btnDecrypt.Name = "btnDecrypt";
            this.btnDecrypt.Size = new System.Drawing.Size(139, 31);
            this.btnDecrypt.TabIndex = 10;
            this.btnDecrypt.Text = "Decrypt";
            this.btnDecrypt.UseVisualStyleBackColor = false;
            this.btnDecrypt.Click += new System.EventHandler(this.btnDecrypt_Click);
            // 
            // btnImportFile
            // 
            this.btnImportFile.BackColor = System.Drawing.Color.SteelBlue;
            this.btnImportFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnImportFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImportFile.ForeColor = System.Drawing.Color.White;
            this.btnImportFile.Location = new System.Drawing.Point(3, 28);
            this.btnImportFile.Name = "btnImportFile";
            this.btnImportFile.Size = new System.Drawing.Size(139, 31);
            this.btnImportFile.TabIndex = 9;
            this.btnImportFile.Text = "📁 Import File";
            this.btnImportFile.UseVisualStyleBackColor = false;
            this.btnImportFile.Click += new System.EventHandler(this.btnImportFile_Click);
            // 
            // pnlSettings
            // 
            this.pnlSettings.Controls.Add(this.tableLayoutPanel1);
            this.pnlSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSettings.Location = new System.Drawing.Point(0, 45);
            this.pnlSettings.Name = "pnlSettings";
            this.pnlSettings.Size = new System.Drawing.Size(1348, 41);
            this.pnlSettings.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.Window;
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
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1348, 41);
            this.tableLayoutPanel1.TabIndex = 7;
            // 
            // lblSystemTitle
            // 
            this.lblSystemTitle.AutoSize = true;
            this.lblSystemTitle.BackColor = System.Drawing.SystemColors.Window;
            this.lblSystemTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSystemTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSystemTitle.Location = new System.Drawing.Point(3, 0);
            this.lblSystemTitle.Name = "lblSystemTitle";
            this.lblSystemTitle.Size = new System.Drawing.Size(218, 41);
            this.lblSystemTitle.TabIndex = 0;
            this.lblSystemTitle.Text = "System Title (Hex):";
            this.lblSystemTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblBlockCipherKey
            // 
            this.lblBlockCipherKey.AutoSize = true;
            this.lblBlockCipherKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBlockCipherKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBlockCipherKey.Location = new System.Drawing.Point(450, 0);
            this.lblBlockCipherKey.Name = "lblBlockCipherKey";
            this.lblBlockCipherKey.Size = new System.Drawing.Size(219, 41);
            this.lblBlockCipherKey.TabIndex = 1;
            this.lblBlockCipherKey.Text = "Block Cipher Key (Ek):";
            this.lblBlockCipherKey.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAuthenticationKey
            // 
            this.lblAuthenticationKey.AutoSize = true;
            this.lblAuthenticationKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAuthenticationKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAuthenticationKey.Location = new System.Drawing.Point(899, 0);
            this.lblAuthenticationKey.Name = "lblAuthenticationKey";
            this.lblAuthenticationKey.Size = new System.Drawing.Size(218, 41);
            this.lblAuthenticationKey.TabIndex = 2;
            this.lblAuthenticationKey.Text = "Authentication Key (Ak):";
            this.lblAuthenticationKey.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAuthenticationKey
            // 
            this.txtAuthenticationKey.BackColor = System.Drawing.SystemColors.Window;
            this.txtAuthenticationKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAuthenticationKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAuthenticationKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAuthenticationKey.Location = new System.Drawing.Point(1123, 10);
            this.txtAuthenticationKey.Margin = new System.Windows.Forms.Padding(3, 10, 3, 2);
            this.txtAuthenticationKey.Name = "txtAuthenticationKey";
            this.txtAuthenticationKey.Size = new System.Drawing.Size(222, 27);
            this.txtAuthenticationKey.TabIndex = 5;
            // 
            // txtSystemTitle
            // 
            this.txtSystemTitle.BackColor = System.Drawing.SystemColors.Window;
            this.txtSystemTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSystemTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSystemTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSystemTitle.Location = new System.Drawing.Point(227, 10);
            this.txtSystemTitle.Margin = new System.Windows.Forms.Padding(3, 10, 3, 2);
            this.txtSystemTitle.Name = "txtSystemTitle";
            this.txtSystemTitle.Size = new System.Drawing.Size(217, 27);
            this.txtSystemTitle.TabIndex = 3;
            // 
            // txtBlockCipherKey
            // 
            this.txtBlockCipherKey.BackColor = System.Drawing.SystemColors.Window;
            this.txtBlockCipherKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBlockCipherKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBlockCipherKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBlockCipherKey.Location = new System.Drawing.Point(675, 10);
            this.txtBlockCipherKey.Margin = new System.Windows.Forms.Padding(3, 10, 3, 2);
            this.txtBlockCipherKey.Name = "txtBlockCipherKey";
            this.txtBlockCipherKey.Size = new System.Drawing.Size(218, 27);
            this.txtBlockCipherKey.TabIndex = 4;
            // 
            // splitter1
            // 
            this.splitter1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(94)))), ((int)(((byte)(168)))));
            this.splitter1.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(0, 268);
            this.splitter1.Margin = new System.Windows.Forms.Padding(4);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(1348, 6);
            this.splitter1.TabIndex = 11;
            this.splitter1.TabStop = false;
            // 
            // pnlPacketCounts
            // 
            this.pnlPacketCounts.Controls.Add(this.tableLayoutPnl_PacketCounts);
            this.pnlPacketCounts.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlPacketCounts.Location = new System.Drawing.Point(0, 630);
            this.pnlPacketCounts.Name = "pnlPacketCounts";
            this.pnlPacketCounts.Size = new System.Drawing.Size(1348, 91);
            this.pnlPacketCounts.TabIndex = 12;
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
            this.tableLayoutPnl_PacketCounts.Controls.Add(this.btnTotalCount, 8, 0);
            this.tableLayoutPnl_PacketCounts.Controls.Add(this.btnTamper, 7, 0);
            this.tableLayoutPnl_PacketCounts.Controls.Add(this.btnAlert, 6, 0);
            this.tableLayoutPnl_PacketCounts.Controls.Add(this.btnCB, 5, 0);
            this.tableLayoutPnl_PacketCounts.Controls.Add(this.btnSR, 4, 0);
            this.tableLayoutPnl_PacketCounts.Controls.Add(this.btnBill, 3, 0);
            this.tableLayoutPnl_PacketCounts.Controls.Add(this.btnDE, 2, 0);
            this.tableLayoutPnl_PacketCounts.Controls.Add(this.btnLS, 1, 0);
            this.tableLayoutPnl_PacketCounts.Controls.Add(this.btnInstant, 0, 0);
            this.tableLayoutPnl_PacketCounts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPnl_PacketCounts.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPnl_PacketCounts.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPnl_PacketCounts.Name = "tableLayoutPnl_PacketCounts";
            this.tableLayoutPnl_PacketCounts.RowCount = 1;
            this.tableLayoutPnl_PacketCounts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPnl_PacketCounts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 91F));
            this.tableLayoutPnl_PacketCounts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 91F));
            this.tableLayoutPnl_PacketCounts.Size = new System.Drawing.Size(1348, 91);
            this.tableLayoutPnl_PacketCounts.TabIndex = 12;
            // 
            // btnTotalCount
            // 
            this.btnTotalCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTotalCount.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnTotalCount.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightGray;
            this.btnTotalCount.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gainsboro;
            this.btnTotalCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTotalCount.Location = new System.Drawing.Point(1195, 3);
            this.btnTotalCount.Name = "btnTotalCount";
            this.btnTotalCount.Size = new System.Drawing.Size(150, 85);
            this.btnTotalCount.TabIndex = 27;
            this.btnTotalCount.Text = "Total Packets";
            this.btnTotalCount.UseVisualStyleBackColor = true;
            this.btnTotalCount.Click += new System.EventHandler(this.BtnFilterbyType_Click);
            // 
            // btnTamper
            // 
            this.btnTamper.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTamper.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnTamper.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightGray;
            this.btnTamper.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gainsboro;
            this.btnTamper.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTamper.Location = new System.Drawing.Point(1046, 3);
            this.btnTamper.Name = "btnTamper";
            this.btnTamper.Size = new System.Drawing.Size(143, 85);
            this.btnTamper.TabIndex = 26;
            this.btnTamper.Text = "Tamper";
            this.btnTamper.UseVisualStyleBackColor = true;
            this.btnTamper.Click += new System.EventHandler(this.BtnFilterbyType_Click);
            // 
            // btnAlert
            // 
            this.btnAlert.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAlert.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnAlert.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightGray;
            this.btnAlert.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gainsboro;
            this.btnAlert.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAlert.Location = new System.Drawing.Point(897, 3);
            this.btnAlert.Name = "btnAlert";
            this.btnAlert.Size = new System.Drawing.Size(143, 85);
            this.btnAlert.TabIndex = 25;
            this.btnAlert.Text = "Alert";
            this.btnAlert.UseVisualStyleBackColor = true;
            this.btnAlert.Click += new System.EventHandler(this.BtnFilterbyType_Click);
            // 
            // btnCB
            // 
            this.btnCB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCB.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnCB.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightGray;
            this.btnCB.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gainsboro;
            this.btnCB.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCB.Location = new System.Drawing.Point(748, 3);
            this.btnCB.Name = "btnCB";
            this.btnCB.Size = new System.Drawing.Size(143, 85);
            this.btnCB.TabIndex = 24;
            this.btnCB.Text = "Current Bill";
            this.btnCB.UseVisualStyleBackColor = true;
            this.btnCB.Click += new System.EventHandler(this.BtnFilterbyType_Click);
            // 
            // btnSR
            // 
            this.btnSR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSR.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnSR.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightGray;
            this.btnSR.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gainsboro;
            this.btnSR.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSR.Location = new System.Drawing.Point(599, 3);
            this.btnSR.Name = "btnSR";
            this.btnSR.Size = new System.Drawing.Size(143, 85);
            this.btnSR.TabIndex = 23;
            this.btnSR.Text = "Self Registration";
            this.btnSR.UseVisualStyleBackColor = true;
            this.btnSR.Click += new System.EventHandler(this.BtnFilterbyType_Click);
            // 
            // btnBill
            // 
            this.btnBill.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnBill.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnBill.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightGray;
            this.btnBill.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gainsboro;
            this.btnBill.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBill.Location = new System.Drawing.Point(450, 3);
            this.btnBill.Name = "btnBill";
            this.btnBill.Size = new System.Drawing.Size(143, 85);
            this.btnBill.TabIndex = 22;
            this.btnBill.Text = "Biliing";
            this.btnBill.UseVisualStyleBackColor = true;
            this.btnBill.Click += new System.EventHandler(this.BtnFilterbyType_Click);
            // 
            // btnDE
            // 
            this.btnDE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDE.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnDE.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightGray;
            this.btnDE.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gainsboro;
            this.btnDE.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDE.Location = new System.Drawing.Point(301, 3);
            this.btnDE.Name = "btnDE";
            this.btnDE.Size = new System.Drawing.Size(143, 85);
            this.btnDE.TabIndex = 21;
            this.btnDE.Text = "Daily Energy";
            this.btnDE.UseVisualStyleBackColor = true;
            this.btnDE.Click += new System.EventHandler(this.BtnFilterbyType_Click);
            // 
            // btnLS
            // 
            this.btnLS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLS.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnLS.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightGray;
            this.btnLS.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gainsboro;
            this.btnLS.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLS.Location = new System.Drawing.Point(152, 3);
            this.btnLS.Name = "btnLS";
            this.btnLS.Size = new System.Drawing.Size(143, 85);
            this.btnLS.TabIndex = 20;
            this.btnLS.Text = "Load Survey";
            this.btnLS.UseVisualStyleBackColor = true;
            this.btnLS.Click += new System.EventHandler(this.BtnFilterbyType_Click);
            // 
            // btnInstant
            // 
            this.btnInstant.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnInstant.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnInstant.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightGray;
            this.btnInstant.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gainsboro;
            this.btnInstant.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInstant.Location = new System.Drawing.Point(3, 3);
            this.btnInstant.Name = "btnInstant";
            this.btnInstant.Size = new System.Drawing.Size(143, 85);
            this.btnInstant.TabIndex = 19;
            this.btnInstant.Text = "Instant";
            this.btnInstant.UseVisualStyleBackColor = true;
            this.btnInstant.Click += new System.EventHandler(this.BtnFilterbyType_Click);
            // 
            // pnlSummarizedPacketTable
            // 
            this.pnlSummarizedPacketTable.Controls.Add(this.tableLayoutPanel3);
            this.pnlSummarizedPacketTable.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSummarizedPacketTable.Location = new System.Drawing.Point(0, 274);
            this.pnlSummarizedPacketTable.Name = "pnlSummarizedPacketTable";
            this.pnlSummarizedPacketTable.Size = new System.Drawing.Size(891, 356);
            this.pnlSummarizedPacketTable.TabIndex = 13;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.dgPacketsSummary, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.lblFilePath, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(891, 356);
            this.tableLayoutPanel3.TabIndex = 11;
            // 
            // dgPacketsSummary
            // 
            this.dgPacketsSummary.AllowUserToAddRows = false;
            this.dgPacketsSummary.AllowUserToDeleteRows = false;
            this.dgPacketsSummary.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgPacketsSummary.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dgPacketsSummary.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgPacketsSummary.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgPacketsSummary.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgPacketsSummary.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(94)))), ((int)(((byte)(168)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(94)))), ((int)(((byte)(168)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgPacketsSummary.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgPacketsSummary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgPacketsSummary.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgPacketsSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgPacketsSummary.EnableHeadersVisualStyles = false;
            this.dgPacketsSummary.GridColor = System.Drawing.SystemColors.Window;
            this.dgPacketsSummary.Location = new System.Drawing.Point(3, 33);
            this.dgPacketsSummary.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgPacketsSummary.MultiSelect = false;
            this.dgPacketsSummary.Name = "dgPacketsSummary";
            this.dgPacketsSummary.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgPacketsSummary.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgPacketsSummary.RowHeadersVisible = false;
            this.dgPacketsSummary.RowHeadersWidth = 51;
            this.dgPacketsSummary.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgPacketsSummary.Size = new System.Drawing.Size(885, 321);
            this.dgPacketsSummary.TabIndex = 2;
            this.dgPacketsSummary.SelectionChanged += new System.EventHandler(this.dgvPackets_SelectionChanged);
            // 
            // lblFilePath
            // 
            this.lblFilePath.AutoSize = true;
            this.lblFilePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFilePath.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFilePath.ForeColor = System.Drawing.Color.Indigo;
            this.lblFilePath.Location = new System.Drawing.Point(4, 0);
            this.lblFilePath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFilePath.MinimumSize = new System.Drawing.Size(100, 23);
            this.lblFilePath.Name = "lblFilePath";
            this.lblFilePath.Size = new System.Drawing.Size(883, 31);
            this.lblFilePath.TabIndex = 10;
            this.lblFilePath.Text = "File:";
            this.lblFilePath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabResults
            // 
            this.tabResults.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabResults.Controls.Add(this.tabPlain);
            this.tabResults.Controls.Add(this.tabXML);
            this.tabResults.Controls.Add(this.tabMP);
            this.tabResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabResults.Location = new System.Drawing.Point(891, 274);
            this.tabResults.Margin = new System.Windows.Forms.Padding(0);
            this.tabResults.Name = "tabResults";
            this.tabResults.SelectedIndex = 0;
            this.tabResults.Size = new System.Drawing.Size(457, 356);
            this.tabResults.TabIndex = 14;
            // 
            // tabPlain
            // 
            this.tabPlain.BackColor = System.Drawing.SystemColors.Window;
            this.tabPlain.Controls.Add(this.txtPlainResult);
            this.tabPlain.Location = new System.Drawing.Point(4, 28);
            this.tabPlain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPlain.Name = "tabPlain";
            this.tabPlain.Size = new System.Drawing.Size(449, 324);
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
            this.txtPlainResult.Size = new System.Drawing.Size(449, 324);
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
            this.tabXML.Size = new System.Drawing.Size(449, 324);
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
            this.txtXmlResult.Size = new System.Drawing.Size(449, 324);
            this.txtXmlResult.TabIndex = 0;
            this.txtXmlResult.Text = "";
            // 
            // tabMP
            // 
            this.tabMP.BackColor = System.Drawing.SystemColors.Control;
            this.tabMP.Controls.Add(this.dgPacketsDetail);
            this.tabMP.Location = new System.Drawing.Point(4, 28);
            this.tabMP.Margin = new System.Windows.Forms.Padding(0);
            this.tabMP.Name = "tabMP";
            this.tabMP.Padding = new System.Windows.Forms.Padding(4);
            this.tabMP.Size = new System.Drawing.Size(449, 324);
            this.tabMP.TabIndex = 2;
            this.tabMP.Text = "Multiple Packets";
            // 
            // dgPacketsDetail
            // 
            this.dgPacketsDetail.AllowUserToAddRows = false;
            this.dgPacketsDetail.AllowUserToDeleteRows = false;
            this.dgPacketsDetail.AllowUserToResizeColumns = false;
            this.dgPacketsDetail.AllowUserToResizeRows = false;
            this.dgPacketsDetail.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgPacketsDetail.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dgPacketsDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgPacketsDetail.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(94)))), ((int)(((byte)(168)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(94)))), ((int)(((byte)(168)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgPacketsDetail.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgPacketsDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgPacketsDetail.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgPacketsDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgPacketsDetail.EnableHeadersVisualStyles = false;
            this.dgPacketsDetail.GridColor = System.Drawing.SystemColors.Control;
            this.dgPacketsDetail.Location = new System.Drawing.Point(4, 4);
            this.dgPacketsDetail.Margin = new System.Windows.Forms.Padding(0);
            this.dgPacketsDetail.Name = "dgPacketsDetail";
            this.dgPacketsDetail.ReadOnly = true;
            this.dgPacketsDetail.RowHeadersVisible = false;
            this.dgPacketsDetail.RowHeadersWidth = 50;
            this.dgPacketsDetail.RowTemplate.Height = 24;
            this.dgPacketsDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgPacketsDetail.Size = new System.Drawing.Size(441, 316);
            this.dgPacketsDetail.TabIndex = 7;
            // 
            // splitter2
            // 
            this.splitter2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(94)))), ((int)(((byte)(168)))));
            this.splitter2.Cursor = System.Windows.Forms.Cursors.VSplit;
            this.splitter2.Location = new System.Drawing.Point(891, 274);
            this.splitter2.Margin = new System.Windows.Forms.Padding(4);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(4, 356);
            this.splitter2.TabIndex = 15;
            this.splitter2.TabStop = false;
            // 
            // PushPacketDecrypter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1348, 721);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.tabResults);
            this.Controls.Add(this.pnlSummarizedPacketTable);
            this.Controls.Add(this.pnlPacketCounts);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlSettings);
            this.Controls.Add(this.tablelayout_Header);
            this.Name = "PushPacketDecrypter";
            this.Text = "Push Packet Decrypter";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.PushPacketDecrypter_Load);
            this.tablelayout_Header.ResumeLayout(false);
            this.tablelayout_Header.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.pnlCipherPacket.ResumeLayout(false);
            this.grpCipherInput.ResumeLayout(false);
            this.pnlButtons.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.pnlSettings.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.pnlPacketCounts.ResumeLayout(false);
            this.tableLayoutPnl_PacketCounts.ResumeLayout(false);
            this.pnlSummarizedPacketTable.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgPacketsSummary)).EndInit();
            this.tabResults.ResumeLayout(false);
            this.tabPlain.ResumeLayout(false);
            this.tabXML.ResumeLayout(false);
            this.tabMP.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgPacketsDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tablelayout_Header;
        private System.Windows.Forms.Label headerTitle;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pnlSettings;
        private System.Windows.Forms.Panel pnlCipherPacket;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblSystemTitle;
        private System.Windows.Forms.Label lblBlockCipherKey;
        private System.Windows.Forms.Label lblAuthenticationKey;
        private System.Windows.Forms.TextBox txtAuthenticationKey;
        private System.Windows.Forms.TextBox txtSystemTitle;
        private System.Windows.Forms.TextBox txtBlockCipherKey;
        private System.Windows.Forms.GroupBox grpCipherInput;
        private System.Windows.Forms.RichTextBox txtCipherPacket;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btnImportFile;
        private System.Windows.Forms.Button btnDecrypt;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel pnlPacketCounts;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPnl_PacketCounts;
        private System.Windows.Forms.Panel pnlSummarizedPacketTable;
        private System.Windows.Forms.DataGridView dgPacketsSummary;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label lblFilePath;
        private System.Windows.Forms.TabControl tabResults;
        private System.Windows.Forms.TabPage tabPlain;
        private System.Windows.Forms.RichTextBox txtPlainResult;
        private System.Windows.Forms.TabPage tabXML;
        private System.Windows.Forms.RichTextBox txtXmlResult;
        private System.Windows.Forms.TabPage tabMP;
        private System.Windows.Forms.DataGridView dgPacketsDetail;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.Button btnInstant;
        private System.Windows.Forms.Button btnTotalCount;
        private System.Windows.Forms.Button btnTamper;
        private System.Windows.Forms.Button btnAlert;
        private System.Windows.Forms.Button btnCB;
        private System.Windows.Forms.Button btnSR;
        private System.Windows.Forms.Button btnBill;
        private System.Windows.Forms.Button btnDE;
        private System.Windows.Forms.Button btnLS;
    }
}