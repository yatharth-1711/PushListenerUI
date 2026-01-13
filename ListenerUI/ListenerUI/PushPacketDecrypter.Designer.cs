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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tablelayout_Header = new System.Windows.Forms.TableLayoutPanel();
            this.headerTitle = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
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
            this.pnlFileHeader = new System.Windows.Forms.Panel();
            this.lblFilePath = new System.Windows.Forms.Label();
            this.tabResults = new System.Windows.Forms.TabControl();
            this.tabPlain = new System.Windows.Forms.TabPage();
            this.txtPlainResult = new System.Windows.Forms.RichTextBox();
            this.tabXML = new System.Windows.Forms.TabPage();
            this.txtXmlResult = new System.Windows.Forms.RichTextBox();
            this.tabMP = new System.Windows.Forms.TabPage();
            this.dgPacketsDetail = new System.Windows.Forms.DataGridView();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
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
            this.pnlFileHeader.SuspendLayout();
            this.tabResults.SuspendLayout();
            this.tabPlain.SuspendLayout();
            this.tabXML.SuspendLayout();
            this.tabMP.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgPacketsDetail)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tablelayout_Header
            // 
            this.tablelayout_Header.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(94)))), ((int)(((byte)(168)))));
            this.tablelayout_Header.ColumnCount = 1;
            this.tablelayout_Header.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tablelayout_Header.Controls.Add(this.headerTitle, 0, 0);
            this.tablelayout_Header.Controls.Add(this.lblSubtitle, 0, 1);
            this.tablelayout_Header.Dock = System.Windows.Forms.DockStyle.Top;
            this.tablelayout_Header.Location = new System.Drawing.Point(0, 0);
            this.tablelayout_Header.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tablelayout_Header.Name = "tablelayout_Header";
            this.tablelayout_Header.RowCount = 2;
            this.tablelayout_Header.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tablelayout_Header.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tablelayout_Header.Size = new System.Drawing.Size(1348, 70);
            this.tablelayout_Header.TabIndex = 9;
            // 
            // headerTitle
            // 
            this.headerTitle.AutoSize = true;
            this.headerTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(94)))), ((int)(((byte)(168)))));
            this.headerTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.headerTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.headerTitle.ForeColor = System.Drawing.Color.White;
            this.headerTitle.Location = new System.Drawing.Point(3, 0);
            this.headerTitle.Name = "headerTitle";
            this.headerTitle.Size = new System.Drawing.Size(1342, 45);
            this.headerTitle.TabIndex = 0;
            this.headerTitle.Text = "🔐 Push Packet Decrypter";
            this.headerTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(94)))), ((int)(((byte)(168)))));
            this.lblSubtitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSubtitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(230)))), ((int)(((byte)(255)))));
            this.lblSubtitle.Location = new System.Drawing.Point(3, 45);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(1342, 25);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "Based on AES-GCM - Packet Decryption & Analysis Tool";
            this.lblSubtitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pnlCipherPacket);
            this.panel1.Controls.Add(this.pnlButtons);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 114);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(8, 8, 8, 4);
            this.panel1.Size = new System.Drawing.Size(1348, 193);
            this.panel1.TabIndex = 10;
            // 
            // pnlCipherPacket
            // 
            this.pnlCipherPacket.Controls.Add(this.grpCipherInput);
            this.pnlCipherPacket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCipherPacket.Location = new System.Drawing.Point(8, 8);
            this.pnlCipherPacket.Name = "pnlCipherPacket";
            this.pnlCipherPacket.Size = new System.Drawing.Size(1153, 181);
            this.pnlCipherPacket.TabIndex = 2;
            // 
            // grpCipherInput
            // 
            this.grpCipherInput.Controls.Add(this.txtCipherPacket);
            this.grpCipherInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpCipherInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpCipherInput.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(94)))), ((int)(((byte)(168)))));
            this.grpCipherInput.Location = new System.Drawing.Point(0, 0);
            this.grpCipherInput.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpCipherInput.Name = "grpCipherInput";
            this.grpCipherInput.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.grpCipherInput.Size = new System.Drawing.Size(1153, 181);
            this.grpCipherInput.TabIndex = 3;
            this.grpCipherInput.TabStop = false;
            this.grpCipherInput.Text = "📦 Encrypted Push Packet Data";
            // 
            // txtCipherPacket
            // 
            this.txtCipherPacket.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(255)))));
            this.txtCipherPacket.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCipherPacket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCipherPacket.Font = new System.Drawing.Font("Consolas", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCipherPacket.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.txtCipherPacket.Location = new System.Drawing.Point(10, 27);
            this.txtCipherPacket.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtCipherPacket.Name = "txtCipherPacket";
            this.txtCipherPacket.Size = new System.Drawing.Size(1133, 146);
            this.txtCipherPacket.TabIndex = 0;
            this.txtCipherPacket.Text = "";
            this.toolTip1.SetToolTip(this.txtCipherPacket, "Paste your encrypted packet data here (hex format)");
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.tableLayoutPanel2);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlButtons.Location = new System.Drawing.Point(1161, 8);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.pnlButtons.Size = new System.Drawing.Size(179, 181);
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
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(8, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(171, 181);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // btnExport
            // 
            this.btnExport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this.btnExport.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnExport.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(110)))), ((int)(((byte)(160)))));
            this.btnExport.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(100)))), ((int)(((byte)(140)))));
            this.btnExport.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(145)))), ((int)(((byte)(195)))));
            this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExport.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.Location = new System.Drawing.Point(3, 144);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(165, 34);
            this.btnExport.TabIndex = 12;
            this.btnExport.Text = "💾 Export Data";
            this.toolTip1.SetToolTip(this.btnExport, "Export decrypted packets to file");
            this.btnExport.UseVisualStyleBackColor = false;
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnClear.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClear.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(40)))), ((int)(((byte)(55)))));
            this.btnClear.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(33)))), ((int)(((byte)(49)))));
            this.btnClear.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(68)))), ((int)(((byte)(84)))));
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(3, 107);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(165, 31);
            this.btnClear.TabIndex = 11;
            this.btnClear.Text = "🗑️ Clear All";
            this.toolTip1.SetToolTip(this.btnClear, "Clear all data and reset");
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnDecrypt
            // 
            this.btnDecrypt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnDecrypt.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDecrypt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDecrypt.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(147)))), ((int)(((byte)(59)))));
            this.btnDecrypt.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(137)))), ((int)(((byte)(49)))));
            this.btnDecrypt.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(182)))), ((int)(((byte)(84)))));
            this.btnDecrypt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDecrypt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDecrypt.ForeColor = System.Drawing.Color.White;
            this.btnDecrypt.Location = new System.Drawing.Point(3, 70);
            this.btnDecrypt.Name = "btnDecrypt";
            this.btnDecrypt.Size = new System.Drawing.Size(165, 31);
            this.btnDecrypt.TabIndex = 10;
            this.btnDecrypt.Text = "🔓 Decrypt";
            this.toolTip1.SetToolTip(this.btnDecrypt, "Decrypt the packet data using provided keys");
            this.btnDecrypt.UseVisualStyleBackColor = false;
            this.btnDecrypt.Click += new System.EventHandler(this.btnDecrypt_Click);
            // 
            // btnImportFile
            // 
            this.btnImportFile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.btnImportFile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnImportFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnImportFile.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.btnImportFile.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(90)))), ((int)(((byte)(200)))));
            this.btnImportFile.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(138)))), ((int)(((byte)(255)))));
            this.btnImportFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImportFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImportFile.ForeColor = System.Drawing.Color.White;
            this.btnImportFile.Location = new System.Drawing.Point(3, 33);
            this.btnImportFile.Name = "btnImportFile";
            this.btnImportFile.Size = new System.Drawing.Size(165, 31);
            this.btnImportFile.TabIndex = 9;
            this.btnImportFile.Text = "📁 Import File";
            this.toolTip1.SetToolTip(this.btnImportFile, "Import encrypted packet file");
            this.btnImportFile.UseVisualStyleBackColor = false;
            this.btnImportFile.Click += new System.EventHandler(this.btnImportFile_Click);
            // 
            // pnlSettings
            // 
            this.pnlSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(248)))), ((int)(((byte)(250)))));
            this.pnlSettings.Controls.Add(this.tableLayoutPanel1);
            this.pnlSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSettings.Location = new System.Drawing.Point(0, 70);
            this.pnlSettings.Name = "pnlSettings";
            this.pnlSettings.Padding = new System.Windows.Forms.Padding(8);
            this.pnlSettings.Size = new System.Drawing.Size(1348, 44);
            this.pnlSettings.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.White;
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
            this.tableLayoutPanel1.Location = new System.Drawing.Point(8, 8);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1332, 28);
            this.tableLayoutPanel1.TabIndex = 7;
            // 
            // lblSystemTitle
            // 
            this.lblSystemTitle.AutoSize = true;
            this.lblSystemTitle.BackColor = System.Drawing.Color.White;
            this.lblSystemTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSystemTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSystemTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(94)))), ((int)(((byte)(168)))));
            this.lblSystemTitle.Location = new System.Drawing.Point(3, 0);
            this.lblSystemTitle.Name = "lblSystemTitle";
            this.lblSystemTitle.Size = new System.Drawing.Size(216, 28);
            this.lblSystemTitle.TabIndex = 0;
            this.lblSystemTitle.Text = "System Title (Hex):";
            this.lblSystemTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblBlockCipherKey
            // 
            this.lblBlockCipherKey.AutoSize = true;
            this.lblBlockCipherKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBlockCipherKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBlockCipherKey.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(94)))), ((int)(((byte)(168)))));
            this.lblBlockCipherKey.Location = new System.Drawing.Point(446, 0);
            this.lblBlockCipherKey.Name = "lblBlockCipherKey";
            this.lblBlockCipherKey.Size = new System.Drawing.Size(216, 28);
            this.lblBlockCipherKey.TabIndex = 1;
            this.lblBlockCipherKey.Text = "Block Cipher Key (Ek):";
            this.lblBlockCipherKey.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAuthenticationKey
            // 
            this.lblAuthenticationKey.AutoSize = true;
            this.lblAuthenticationKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAuthenticationKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAuthenticationKey.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(94)))), ((int)(((byte)(168)))));
            this.lblAuthenticationKey.Location = new System.Drawing.Point(890, 0);
            this.lblAuthenticationKey.Name = "lblAuthenticationKey";
            this.lblAuthenticationKey.Size = new System.Drawing.Size(216, 28);
            this.lblAuthenticationKey.TabIndex = 2;
            this.lblAuthenticationKey.Text = "Authentication Key (Ak):";
            this.lblAuthenticationKey.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAuthenticationKey
            // 
            this.txtAuthenticationKey.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(255)))));
            this.txtAuthenticationKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAuthenticationKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAuthenticationKey.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAuthenticationKey.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.txtAuthenticationKey.Location = new System.Drawing.Point(1112, 2);
            this.txtAuthenticationKey.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtAuthenticationKey.Name = "txtAuthenticationKey";
            this.txtAuthenticationKey.Size = new System.Drawing.Size(217, 25);
            this.txtAuthenticationKey.TabIndex = 5;
            this.toolTip1.SetToolTip(this.txtAuthenticationKey, "Enter the Authentication Key (Ak) in hexadecimal format");
            // 
            // txtSystemTitle
            // 
            this.txtSystemTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(255)))));
            this.txtSystemTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSystemTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSystemTitle.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSystemTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.txtSystemTitle.Location = new System.Drawing.Point(225, 2);
            this.txtSystemTitle.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtSystemTitle.Name = "txtSystemTitle";
            this.txtSystemTitle.Size = new System.Drawing.Size(215, 25);
            this.txtSystemTitle.TabIndex = 3;
            this.toolTip1.SetToolTip(this.txtSystemTitle, "Enter the System Title in hexadecimal format");
            // 
            // txtBlockCipherKey
            // 
            this.txtBlockCipherKey.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(255)))));
            this.txtBlockCipherKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBlockCipherKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBlockCipherKey.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBlockCipherKey.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.txtBlockCipherKey.Location = new System.Drawing.Point(668, 2);
            this.txtBlockCipherKey.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtBlockCipherKey.Name = "txtBlockCipherKey";
            this.txtBlockCipherKey.Size = new System.Drawing.Size(216, 25);
            this.txtBlockCipherKey.TabIndex = 4;
            this.toolTip1.SetToolTip(this.txtBlockCipherKey, "Enter the Block Cipher Key (Ek) in hexadecimal format");
            // 
            // splitter1
            // 
            this.splitter1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(94)))), ((int)(((byte)(168)))));
            this.splitter1.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(0, 307);
            this.splitter1.Margin = new System.Windows.Forms.Padding(4);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(1348, 5);
            this.splitter1.TabIndex = 11;
            this.splitter1.TabStop = false;
            // 
            // pnlPacketCounts
            // 
            this.pnlPacketCounts.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(248)))), ((int)(((byte)(250)))));
            this.pnlPacketCounts.Controls.Add(this.tableLayoutPnl_PacketCounts);
            this.pnlPacketCounts.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlPacketCounts.Location = new System.Drawing.Point(0, 620);
            this.pnlPacketCounts.Name = "pnlPacketCounts";
            this.pnlPacketCounts.Padding = new System.Windows.Forms.Padding(8);
            this.pnlPacketCounts.Size = new System.Drawing.Size(1348, 77);
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
            this.tableLayoutPnl_PacketCounts.Location = new System.Drawing.Point(8, 8);
            this.tableLayoutPnl_PacketCounts.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPnl_PacketCounts.Name = "tableLayoutPnl_PacketCounts";
            this.tableLayoutPnl_PacketCounts.RowCount = 1;
            this.tableLayoutPnl_PacketCounts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPnl_PacketCounts.Size = new System.Drawing.Size(1332, 61);
            this.tableLayoutPnl_PacketCounts.TabIndex = 12;
            // 
            // btnTotalCount
            // 
            this.btnTotalCount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(94)))), ((int)(((byte)(168)))));
            this.btnTotalCount.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTotalCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTotalCount.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(80)))), ((int)(((byte)(150)))));
            this.btnTotalCount.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(130)))));
            this.btnTotalCount.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(110)))), ((int)(((byte)(190)))));
            this.btnTotalCount.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTotalCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTotalCount.ForeColor = System.Drawing.Color.White;
            this.btnTotalCount.Location = new System.Drawing.Point(1187, 3);
            this.btnTotalCount.Name = "btnTotalCount";
            this.btnTotalCount.Size = new System.Drawing.Size(142, 55);
            this.btnTotalCount.TabIndex = 27;
            this.btnTotalCount.Text = "📊 Total\r\n0 Packets";
            this.toolTip1.SetToolTip(this.btnTotalCount, "Show all packets");
            this.btnTotalCount.UseVisualStyleBackColor = false;
            this.btnTotalCount.Click += new System.EventHandler(this.BtnFilterbyType_Click);
            // 
            // btnTamper
            // 
            this.btnTamper.BackColor = System.Drawing.Color.White;
            this.btnTamper.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTamper.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTamper.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.btnTamper.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.btnTamper.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(248)))), ((int)(((byte)(255)))));
            this.btnTamper.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTamper.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTamper.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnTamper.Location = new System.Drawing.Point(1039, 3);
            this.btnTamper.Name = "btnTamper";
            this.btnTamper.Size = new System.Drawing.Size(142, 55);
            this.btnTamper.TabIndex = 26;
            this.btnTamper.Text = "🔴 Tamper\r\n0";
            this.toolTip1.SetToolTip(this.btnTamper, "Filter by Tamper packets");
            this.btnTamper.UseVisualStyleBackColor = false;
            this.btnTamper.Click += new System.EventHandler(this.BtnFilterbyType_Click);
            // 
            // btnAlert
            // 
            this.btnAlert.BackColor = System.Drawing.Color.White;
            this.btnAlert.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAlert.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAlert.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.btnAlert.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.btnAlert.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(248)))), ((int)(((byte)(255)))));
            this.btnAlert.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAlert.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAlert.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnAlert.Location = new System.Drawing.Point(891, 3);
            this.btnAlert.Name = "btnAlert";
            this.btnAlert.Size = new System.Drawing.Size(142, 55);
            this.btnAlert.TabIndex = 25;
            this.btnAlert.Text = "⚠️ Alert\r\n0";
            this.toolTip1.SetToolTip(this.btnAlert, "Filter by Alert packets");
            this.btnAlert.UseVisualStyleBackColor = false;
            this.btnAlert.Click += new System.EventHandler(this.BtnFilterbyType_Click);
            // 
            // btnCB
            // 
            this.btnCB.BackColor = System.Drawing.Color.White;
            this.btnCB.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCB.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.btnCB.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.btnCB.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(248)))), ((int)(((byte)(255)))));
            this.btnCB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCB.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnCB.Location = new System.Drawing.Point(743, 3);
            this.btnCB.Name = "btnCB";
            this.btnCB.Size = new System.Drawing.Size(142, 55);
            this.btnCB.TabIndex = 24;
            this.btnCB.Text = "💵 Current Bill\r\n0";
            this.toolTip1.SetToolTip(this.btnCB, "Filter by Current Bill packets");
            this.btnCB.UseVisualStyleBackColor = false;
            this.btnCB.Click += new System.EventHandler(this.BtnFilterbyType_Click);
            // 
            // btnSR
            // 
            this.btnSR.BackColor = System.Drawing.Color.White;
            this.btnSR.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSR.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.btnSR.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.btnSR.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(248)))), ((int)(((byte)(255)))));
            this.btnSR.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSR.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSR.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnSR.Location = new System.Drawing.Point(595, 3);
            this.btnSR.Name = "btnSR";
            this.btnSR.Size = new System.Drawing.Size(142, 55);
            this.btnSR.TabIndex = 23;
            this.btnSR.Text = "📝 Self Reg\r\n0";
            this.toolTip1.SetToolTip(this.btnSR, "Filter by Self Registration packets");
            this.btnSR.UseVisualStyleBackColor = false;
            this.btnSR.Click += new System.EventHandler(this.BtnFilterbyType_Click);
            // 
            // btnBill
            // 
            this.btnBill.BackColor = System.Drawing.Color.White;
            this.btnBill.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBill.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnBill.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.btnBill.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.btnBill.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(248)))), ((int)(((byte)(255)))));
            this.btnBill.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBill.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBill.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnBill.Location = new System.Drawing.Point(447, 3);
            this.btnBill.Name = "btnBill";
            this.btnBill.Size = new System.Drawing.Size(142, 55);
            this.btnBill.TabIndex = 22;
            this.btnBill.Text = "💳 Billing\r\n0";
            this.toolTip1.SetToolTip(this.btnBill, "Filter by Billing packets");
            this.btnBill.UseVisualStyleBackColor = false;
            this.btnBill.Click += new System.EventHandler(this.BtnFilterbyType_Click);
            // 
            // btnDE
            // 
            this.btnDE.BackColor = System.Drawing.Color.White;
            this.btnDE.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDE.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.btnDE.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.btnDE.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(248)))), ((int)(((byte)(255)))));
            this.btnDE.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDE.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDE.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnDE.Location = new System.Drawing.Point(299, 3);
            this.btnDE.Name = "btnDE";
            this.btnDE.Size = new System.Drawing.Size(142, 55);
            this.btnDE.TabIndex = 21;
            this.btnDE.Text = "📅 Daily Energy\r\n0";
            this.toolTip1.SetToolTip(this.btnDE, "Filter by Daily Energy packets");
            this.btnDE.UseVisualStyleBackColor = false;
            this.btnDE.Click += new System.EventHandler(this.BtnFilterbyType_Click);
            // 
            // btnLS
            // 
            this.btnLS.BackColor = System.Drawing.Color.White;
            this.btnLS.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLS.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.btnLS.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.btnLS.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(248)))), ((int)(((byte)(255)))));
            this.btnLS.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLS.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLS.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnLS.Location = new System.Drawing.Point(151, 3);
            this.btnLS.Name = "btnLS";
            this.btnLS.Size = new System.Drawing.Size(142, 55);
            this.btnLS.TabIndex = 20;
            this.btnLS.Text = "📈 Load Survey\r\n0";
            this.toolTip1.SetToolTip(this.btnLS, "Filter by Load Survey packets");
            this.btnLS.UseVisualStyleBackColor = false;
            this.btnLS.Click += new System.EventHandler(this.BtnFilterbyType_Click);
            // 
            // btnInstant
            // 
            this.btnInstant.BackColor = System.Drawing.Color.White;
            this.btnInstant.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnInstant.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnInstant.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.btnInstant.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.btnInstant.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(248)))), ((int)(((byte)(255)))));
            this.btnInstant.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInstant.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInstant.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnInstant.Location = new System.Drawing.Point(3, 3);
            this.btnInstant.Name = "btnInstant";
            this.btnInstant.Size = new System.Drawing.Size(142, 55);
            this.btnInstant.TabIndex = 19;
            this.btnInstant.Text = "⚡ Instant\r\n0";
            this.toolTip1.SetToolTip(this.btnInstant, "Filter by Instant packets");
            this.btnInstant.UseVisualStyleBackColor = false;
            this.btnInstant.Click += new System.EventHandler(this.BtnFilterbyType_Click);
            // 
            // pnlSummarizedPacketTable
            // 
            this.pnlSummarizedPacketTable.Controls.Add(this.tableLayoutPanel3);
            this.pnlSummarizedPacketTable.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSummarizedPacketTable.Location = new System.Drawing.Point(0, 312);
            this.pnlSummarizedPacketTable.Name = "pnlSummarizedPacketTable";
            this.pnlSummarizedPacketTable.Padding = new System.Windows.Forms.Padding(8, 0, 4, 8);
            this.pnlSummarizedPacketTable.Size = new System.Drawing.Size(891, 308);
            this.pnlSummarizedPacketTable.TabIndex = 13;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.dgPacketsSummary, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.pnlFileHeader, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(8, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(879, 300);
            this.tableLayoutPanel3.TabIndex = 11;
            // 
            // dgPacketsSummary
            // 
            this.dgPacketsSummary.AllowUserToAddRows = false;
            this.dgPacketsSummary.AllowUserToDeleteRows = false;
            this.dgPacketsSummary.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgPacketsSummary.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dgPacketsSummary.BackgroundColor = System.Drawing.Color.White;
            this.dgPacketsSummary.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgPacketsSummary.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(94)))), ((int)(((byte)(168)))));
            dataGridViewCellStyle16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle16.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle16.Padding = new System.Windows.Forms.Padding(5);
            dataGridViewCellStyle16.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(94)))), ((int)(((byte)(168)))));
            dataGridViewCellStyle16.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle16.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgPacketsSummary.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle16;
            this.dgPacketsSummary.ColumnHeadersHeight = 40;
            this.dgPacketsSummary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle17.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle17.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            dataGridViewCellStyle17.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
            dataGridViewCellStyle17.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle17.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(94)))), ((int)(((byte)(168)))));
            dataGridViewCellStyle17.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgPacketsSummary.DefaultCellStyle = dataGridViewCellStyle17;
            this.dgPacketsSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgPacketsSummary.EnableHeadersVisualStyles = false;
            this.dgPacketsSummary.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.dgPacketsSummary.Location = new System.Drawing.Point(3, 37);
            this.dgPacketsSummary.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgPacketsSummary.MultiSelect = false;
            this.dgPacketsSummary.Name = "dgPacketsSummary";
            this.dgPacketsSummary.ReadOnly = true;
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle18.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle18.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle18.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle18.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle18.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle18.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgPacketsSummary.RowHeadersDefaultCellStyle = dataGridViewCellStyle18;
            this.dgPacketsSummary.RowHeadersVisible = false;
            this.dgPacketsSummary.RowHeadersWidth = 51;
            this.dgPacketsSummary.RowTemplate.Height = 32;
            this.dgPacketsSummary.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgPacketsSummary.Size = new System.Drawing.Size(873, 261);
            this.dgPacketsSummary.TabIndex = 2;
            this.dgPacketsSummary.SelectionChanged += new System.EventHandler(this.dgvPackets_SelectionChanged);
            // 
            // pnlFileHeader
            // 
            this.pnlFileHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(248)))), ((int)(((byte)(250)))));
            this.pnlFileHeader.Controls.Add(this.lblFilePath);
            this.pnlFileHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFileHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlFileHeader.Margin = new System.Windows.Forms.Padding(0);
            this.pnlFileHeader.Name = "pnlFileHeader";
            this.pnlFileHeader.Padding = new System.Windows.Forms.Padding(8, 5, 8, 5);
            this.pnlFileHeader.Size = new System.Drawing.Size(879, 35);
            this.pnlFileHeader.TabIndex = 10;
            // 
            // lblFilePath
            // 
            this.lblFilePath.AutoEllipsis = true;
            this.lblFilePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFilePath.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFilePath.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.lblFilePath.Location = new System.Drawing.Point(8, 5);
            this.lblFilePath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFilePath.MinimumSize = new System.Drawing.Size(100, 23);
            this.lblFilePath.Name = "lblFilePath";
            this.lblFilePath.Size = new System.Drawing.Size(1079, 31);
            this.lblFilePath.TabIndex = 10;
            this.lblFilePath.Text = "📄 File: No file loaded";
            this.lblFilePath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabResults
            // 
            this.tabResults.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabResults.Controls.Add(this.tabMP);
            this.tabResults.Controls.Add(this.tabPlain);
            this.tabResults.Controls.Add(this.tabXML);
            this.tabResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabResults.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabResults.Location = new System.Drawing.Point(891, 312);
            this.tabResults.Margin = new System.Windows.Forms.Padding(0);
            this.tabResults.Name = "tabResults";
            this.tabResults.Padding = new System.Drawing.Point(12, 6);
            this.tabResults.SelectedIndex = 0;
            this.tabResults.Size = new System.Drawing.Size(457, 308);
            this.tabResults.TabIndex = 14;
            // 
            // tabPlain
            // 
            this.tabPlain.BackColor = System.Drawing.Color.White;
            this.tabPlain.Controls.Add(this.txtPlainResult);
            this.tabPlain.Location = new System.Drawing.Point(4, 36);
            this.tabPlain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPlain.Name = "tabPlain";
            this.tabPlain.Padding = new System.Windows.Forms.Padding(8);
            this.tabPlain.Size = new System.Drawing.Size(449, 264);
            this.tabPlain.TabIndex = 0;
            this.tabPlain.Text = "📄 Decrypted Packets";
            // 
            // txtPlainResult
            // 
            this.txtPlainResult.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(255)))));
            this.txtPlainResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPlainResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPlainResult.Font = new System.Drawing.Font("Consolas", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPlainResult.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.txtPlainResult.Location = new System.Drawing.Point(8, 8);
            this.txtPlainResult.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtPlainResult.Name = "txtPlainResult";
            this.txtPlainResult.ReadOnly = true;
            this.txtPlainResult.Size = new System.Drawing.Size(433, 248);
            this.txtPlainResult.TabIndex = 0;
            this.txtPlainResult.Text = "";
            // 
            // tabXML
            // 
            this.tabXML.BackColor = System.Drawing.Color.White;
            this.tabXML.Controls.Add(this.txtXmlResult);
            this.tabXML.Location = new System.Drawing.Point(4, 36);
            this.tabXML.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabXML.Name = "tabXML";
            this.tabXML.Padding = new System.Windows.Forms.Padding(8);
            this.tabXML.Size = new System.Drawing.Size(449, 264);
            this.tabXML.TabIndex = 1;
            this.tabXML.Text = "📋 XML View";
            // 
            // txtXmlResult
            // 
            this.txtXmlResult.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(255)))));
            this.txtXmlResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtXmlResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtXmlResult.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtXmlResult.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.txtXmlResult.Location = new System.Drawing.Point(8, 8);
            this.txtXmlResult.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtXmlResult.Name = "txtXmlResult";
            this.txtXmlResult.ReadOnly = true;
            this.txtXmlResult.Size = new System.Drawing.Size(433, 248);
            this.txtXmlResult.TabIndex = 0;
            this.txtXmlResult.Text = "";
            // 
            // tabMP
            // 
            this.tabMP.BackColor = System.Drawing.Color.White;
            this.tabMP.Controls.Add(this.dgPacketsDetail);
            this.tabMP.Location = new System.Drawing.Point(4, 36);
            this.tabMP.Margin = new System.Windows.Forms.Padding(0);
            this.tabMP.Name = "tabMP";
            this.tabMP.Padding = new System.Windows.Forms.Padding(8);
            this.tabMP.Size = new System.Drawing.Size(449, 268);
            this.tabMP.TabIndex = 2;
            this.tabMP.Text = "📦 Packet Detail";
            // 
            // dgPacketsDetail
            // 
            this.dgPacketsDetail.AllowUserToAddRows = false;
            this.dgPacketsDetail.AllowUserToDeleteRows = false;
            this.dgPacketsDetail.AllowUserToResizeColumns = false;
            this.dgPacketsDetail.AllowUserToResizeRows = false;
            this.dgPacketsDetail.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgPacketsDetail.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dgPacketsDetail.BackgroundColor = System.Drawing.Color.White;
            this.dgPacketsDetail.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgPacketsDetail.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(94)))), ((int)(((byte)(168)))));
            dataGridViewCellStyle19.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle19.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle19.Padding = new System.Windows.Forms.Padding(5);
            dataGridViewCellStyle19.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(94)))), ((int)(((byte)(168)))));
            dataGridViewCellStyle19.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle19.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgPacketsDetail.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle19;
            this.dgPacketsDetail.ColumnHeadersHeight = 38;
            this.dgPacketsDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle20.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle20.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle20.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            dataGridViewCellStyle20.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
            dataGridViewCellStyle20.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle20.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(94)))), ((int)(((byte)(168)))));
            dataGridViewCellStyle20.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgPacketsDetail.DefaultCellStyle = dataGridViewCellStyle20;
            this.dgPacketsDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgPacketsDetail.EnableHeadersVisualStyles = false;
            this.dgPacketsDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.dgPacketsDetail.Location = new System.Drawing.Point(8, 8);
            this.dgPacketsDetail.Margin = new System.Windows.Forms.Padding(0);
            this.dgPacketsDetail.Name = "dgPacketsDetail";
            this.dgPacketsDetail.ReadOnly = true;
            this.dgPacketsDetail.RowHeadersVisible = false;
            this.dgPacketsDetail.RowHeadersWidth = 50;
            this.dgPacketsDetail.RowTemplate.Height = 28;
            this.dgPacketsDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgPacketsDetail.Size = new System.Drawing.Size(433, 252);
            this.dgPacketsDetail.TabIndex = 7;
            // 
            // splitter2
            // 
            this.splitter2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(94)))), ((int)(((byte)(168)))));
            this.splitter2.Cursor = System.Windows.Forms.Cursors.VSplit;
            this.splitter2.Location = new System.Drawing.Point(891, 312);
            this.splitter2.Margin = new System.Windows.Forms.Padding(4);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(4, 308);
            this.splitter2.TabIndex = 15;
            this.splitter2.TabStop = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(248)))), ((int)(((byte)(250)))));
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel,
            this.toolStripProgressBar,
            this.toolStripStatusTime});
            this.statusStrip1.Location = new System.Drawing.Point(0, 697);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1348, 24);
            this.statusStrip1.TabIndex = 16;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.toolStripStatusLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(50, 18);
            this.toolStripStatusLabel.Text = "Ready";
            // 
            // toolStripProgressBar
            // 
            this.toolStripProgressBar.Name = "toolStripProgressBar";
            this.toolStripProgressBar.Size = new System.Drawing.Size(233, 18);
            this.toolStripProgressBar.Visible = false;
            // 
            // toolStripStatusTime
            // 
            this.toolStripStatusTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.toolStripStatusTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.toolStripStatusTime.Name = "toolStripStatusTime";
            this.toolStripStatusTime.Size = new System.Drawing.Size(1281, 18);
            this.toolStripStatusTime.Spring = true;
            this.toolStripStatusTime.Text = "00:00:00";
            this.toolStripStatusTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "All Files|*.*|Text Files|*.txt|Log Files|*.log|Hex Files|*.hex";
            this.openFileDialog.Title = "Select Encrypted Packet File";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "txt";
            this.saveFileDialog.Filter = "Text Files|*.txt|XML Files|*.xml|All Files|*.*";
            this.saveFileDialog.Title = "Export Decrypted Data";
            // 
            // PushPacketDecrypter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1348, 721);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.tabResults);
            this.Controls.Add(this.pnlSummarizedPacketTable);
            this.Controls.Add(this.pnlPacketCounts);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlSettings);
            this.Controls.Add(this.tablelayout_Header);
            this.Controls.Add(this.statusStrip1);
            this.MinimumSize = new System.Drawing.Size(1200, 700);
            this.Name = "PushPacketDecrypter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Push Packet Decrypter - Professional Edition v1.0";
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
            ((System.ComponentModel.ISupportInitialize)(this.dgPacketsSummary)).EndInit();
            this.pnlFileHeader.ResumeLayout(false);
            this.tabResults.ResumeLayout(false);
            this.tabPlain.ResumeLayout(false);
            this.tabXML.ResumeLayout(false);
            this.tabMP.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgPacketsDetail)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tablelayout_Header;
        private System.Windows.Forms.Label headerTitle;
        private System.Windows.Forms.Label lblSubtitle;
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
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusTime;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.Panel pnlFileHeader;
    }
}
