namespace ListenerUI
{
    partial class MainForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.chkListBoxConformanceBlock = new System.Windows.Forms.CheckedListBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.label22 = new System.Windows.Forms.Label();
            this.splitter_Meter_DLMS = new System.Windows.Forms.Splitter();
            this.splitter_DLMS_Source = new System.Windows.Forms.Splitter();
            this.pnl_MeterSettings = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label28 = new System.Windows.Forms.Label();
            this.txtConfBlock = new System.Windows.Forms.TextBox();
            this.checkBox_InvocationCounter = new System.Windows.Forms.CheckBox();
            this.checkBox_GMAC = new System.Windows.Forms.CheckBox();
            this.checkBox_Dedicated = new System.Windows.Forms.CheckBox();
            this.checkBox_LN = new System.Windows.Forms.CheckBox();
            this.chkConformanceBlock = new System.Windows.Forms.CheckBox();
            this.btnSaveData = new System.Windows.Forms.Button();
            this.txtDISCToNDMTimeout = new System.Windows.Forms.TextBox();
            this.label33 = new System.Windows.Forms.Label();
            this.txtResponseTimeout = new System.Windows.Forms.TextBox();
            this.label32 = new System.Windows.Forms.Label();
            this.txtInterFrameTimeout = new System.Windows.Forms.TextBox();
            this.label31 = new System.Windows.Forms.Label();
            this.txtInactivityTimeout = new System.Windows.Forms.TextBox();
            this.label30 = new System.Windows.Forms.Label();
            this.label100 = new System.Windows.Forms.Label();
            this.txtMasterKey = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.txtSysT = new System.Windows.Forms.TextBox();
            this.label102 = new System.Windows.Forms.Label();
            this.txtAK = new System.Windows.Forms.TextBox();
            this.label101 = new System.Windows.Forms.Label();
            this.txtEK = new System.Windows.Forms.TextBox();
            this.txtAuthPasswordFW = new System.Windows.Forms.TextBox();
            this.label27 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cbMeterComPort = new System.Windows.Forms.ComboBox();
            this.cbMeterBaudRate = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cbAccessLevel = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtAuthPasswordWrite = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtAuthPassword = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label15 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.pnl_MeterSettings.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.SystemColors.Info;
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Controls.Add(this.tableLayoutPanel4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(462, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(380, 898);
            this.panel1.TabIndex = 100;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.checkBox3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.chkListBoxConformanceBlock, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 61);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.853659F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 94.14634F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(380, 837);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBox3.Location = new System.Drawing.Point(3, 2);
            this.checkBox3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.checkBox3.Size = new System.Drawing.Size(374, 44);
            this.checkBox3.TabIndex = 6;
            this.checkBox3.Text = "With GMAC";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // chkListBoxConformanceBlock
            // 
            this.chkListBoxConformanceBlock.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.chkListBoxConformanceBlock.CheckOnClick = true;
            this.chkListBoxConformanceBlock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkListBoxConformanceBlock.Enabled = false;
            this.chkListBoxConformanceBlock.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkListBoxConformanceBlock.FormattingEnabled = true;
            this.chkListBoxConformanceBlock.Items.AddRange(new object[] {
            "reserved-zero",
            "general-protection",
            "general-block-transfer",
            "read",
            "write",
            "unconfirmed-write",
            "delta-value-encoding",
            "reserved-seven",
            "attribute0-supported-with-set",
            "priority-mgmt-supported",
            "attribute0-supported-with-get",
            "block-transfer-with-get-or-read",
            "block-transfer-with-set-or-write",
            "block-transfer-with-action",
            "multiple-references",
            "information-report",
            "data-notification",
            "access",
            "parameterized-access",
            "get",
            "set",
            "selective-access",
            "event-notification",
            "action"});
            this.chkListBoxConformanceBlock.Location = new System.Drawing.Point(3, 50);
            this.chkListBoxConformanceBlock.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkListBoxConformanceBlock.Name = "chkListBoxConformanceBlock";
            this.chkListBoxConformanceBlock.Size = new System.Drawing.Size(374, 785);
            this.chkListBoxConformanceBlock.TabIndex = 7;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.label22, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(380, 61);
            this.tableLayoutPanel4.TabIndex = 2;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(94)))), ((int)(((byte)(168)))));
            this.label22.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.ForeColor = System.Drawing.Color.White;
            this.label22.Location = new System.Drawing.Point(0, 0);
            this.label22.Margin = new System.Windows.Forms.Padding(0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(380, 61);
            this.label22.TabIndex = 3;
            this.label22.Text = "Meter Settings";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // splitter_Meter_DLMS
            // 
            this.splitter_Meter_DLMS.Location = new System.Drawing.Point(447, 0);
            this.splitter_Meter_DLMS.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitter_Meter_DLMS.Name = "splitter_Meter_DLMS";
            this.splitter_Meter_DLMS.Size = new System.Drawing.Size(10, 898);
            this.splitter_Meter_DLMS.TabIndex = 99;
            this.splitter_Meter_DLMS.TabStop = false;
            // 
            // splitter_DLMS_Source
            // 
            this.splitter_DLMS_Source.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(134)))), ((int)(((byte)(52)))));
            this.splitter_DLMS_Source.Location = new System.Drawing.Point(457, 0);
            this.splitter_DLMS_Source.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitter_DLMS_Source.Name = "splitter_DLMS_Source";
            this.splitter_DLMS_Source.Size = new System.Drawing.Size(5, 898);
            this.splitter_DLMS_Source.TabIndex = 97;
            this.splitter_DLMS_Source.TabStop = false;
            // 
            // pnl_MeterSettings
            // 
            this.pnl_MeterSettings.AutoScroll = true;
            this.pnl_MeterSettings.BackColor = System.Drawing.SystemColors.Info;
            this.pnl_MeterSettings.Controls.Add(this.tableLayoutPanel2);
            this.pnl_MeterSettings.Controls.Add(this.tableLayoutPanel3);
            this.pnl_MeterSettings.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnl_MeterSettings.Location = new System.Drawing.Point(0, 0);
            this.pnl_MeterSettings.Margin = new System.Windows.Forms.Padding(0);
            this.pnl_MeterSettings.Name = "pnl_MeterSettings";
            this.pnl_MeterSettings.Size = new System.Drawing.Size(447, 898);
            this.pnl_MeterSettings.TabIndex = 94;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.SystemColors.Info;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.00001F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.99999F));
            this.tableLayoutPanel2.Controls.Add(this.label28, 0, 17);
            this.tableLayoutPanel2.Controls.Add(this.txtConfBlock, 1, 17);
            this.tableLayoutPanel2.Controls.Add(this.checkBox_InvocationCounter, 1, 14);
            this.tableLayoutPanel2.Controls.Add(this.checkBox_GMAC, 0, 15);
            this.tableLayoutPanel2.Controls.Add(this.checkBox_Dedicated, 1, 15);
            this.tableLayoutPanel2.Controls.Add(this.checkBox_LN, 0, 16);
            this.tableLayoutPanel2.Controls.Add(this.chkConformanceBlock, 1, 16);
            this.tableLayoutPanel2.Controls.Add(this.btnSaveData, 0, 14);
            this.tableLayoutPanel2.Controls.Add(this.txtDISCToNDMTimeout, 1, 13);
            this.tableLayoutPanel2.Controls.Add(this.label33, 0, 13);
            this.tableLayoutPanel2.Controls.Add(this.txtResponseTimeout, 1, 12);
            this.tableLayoutPanel2.Controls.Add(this.label32, 0, 12);
            this.tableLayoutPanel2.Controls.Add(this.txtInterFrameTimeout, 1, 11);
            this.tableLayoutPanel2.Controls.Add(this.label31, 0, 11);
            this.tableLayoutPanel2.Controls.Add(this.txtInactivityTimeout, 1, 10);
            this.tableLayoutPanel2.Controls.Add(this.label30, 0, 10);
            this.tableLayoutPanel2.Controls.Add(this.label100, 0, 6);
            this.tableLayoutPanel2.Controls.Add(this.txtMasterKey, 1, 9);
            this.tableLayoutPanel2.Controls.Add(this.label18, 0, 9);
            this.tableLayoutPanel2.Controls.Add(this.txtSysT, 1, 8);
            this.tableLayoutPanel2.Controls.Add(this.label102, 0, 8);
            this.tableLayoutPanel2.Controls.Add(this.txtAK, 1, 7);
            this.tableLayoutPanel2.Controls.Add(this.label101, 0, 7);
            this.tableLayoutPanel2.Controls.Add(this.txtEK, 1, 6);
            this.tableLayoutPanel2.Controls.Add(this.txtAuthPasswordFW, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.label27, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label6, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.cbMeterComPort, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.cbMeterBaudRate, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.label8, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.cbAccessLevel, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.label7, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.txtAuthPasswordWrite, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.label10, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.txtAuthPassword, 1, 5);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 61);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 18;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.263158F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.263158F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.263158F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.263158F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.263158F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.263158F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.263158F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.263158F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.263158F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.263158F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.263158F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.263158F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.263158F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.263158F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.104129F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.181329F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.77743F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.799373F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(447, 596);
            this.tableLayoutPanel2.TabIndex = 78;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.BackColor = System.Drawing.Color.Transparent;
            this.label28.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label28.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label28.ForeColor = System.Drawing.Color.Black;
            this.label28.Location = new System.Drawing.Point(5, 548);
            this.label28.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(213, 48);
            this.label28.TabIndex = 103;
            this.label28.Text = "Block Hex:";
            this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtConfBlock
            // 
            this.txtConfBlock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtConfBlock.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtConfBlock.Location = new System.Drawing.Point(226, 550);
            this.txtConfBlock.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtConfBlock.Name = "txtConfBlock";
            this.txtConfBlock.Size = new System.Drawing.Size(218, 24);
            this.txtConfBlock.TabIndex = 104;
            // 
            // checkBox_InvocationCounter
            // 
            this.checkBox_InvocationCounter.AutoSize = true;
            this.checkBox_InvocationCounter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBox_InvocationCounter.Location = new System.Drawing.Point(226, 422);
            this.checkBox_InvocationCounter.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBox_InvocationCounter.Name = "checkBox_InvocationCounter";
            this.checkBox_InvocationCounter.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.checkBox_InvocationCounter.Size = new System.Drawing.Size(218, 31);
            this.checkBox_InvocationCounter.TabIndex = 102;
            this.checkBox_InvocationCounter.Text = "With Invocation Counter";
            this.checkBox_InvocationCounter.UseVisualStyleBackColor = true;
            // 
            // checkBox_GMAC
            // 
            this.checkBox_GMAC.AutoSize = true;
            this.checkBox_GMAC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBox_GMAC.Location = new System.Drawing.Point(3, 457);
            this.checkBox_GMAC.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBox_GMAC.Name = "checkBox_GMAC";
            this.checkBox_GMAC.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.checkBox_GMAC.Size = new System.Drawing.Size(217, 38);
            this.checkBox_GMAC.TabIndex = 100;
            this.checkBox_GMAC.Text = "With GMAC";
            this.checkBox_GMAC.UseVisualStyleBackColor = true;
            // 
            // checkBox_Dedicated
            // 
            this.checkBox_Dedicated.AutoSize = true;
            this.checkBox_Dedicated.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBox_Dedicated.Location = new System.Drawing.Point(226, 457);
            this.checkBox_Dedicated.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBox_Dedicated.Name = "checkBox_Dedicated";
            this.checkBox_Dedicated.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.checkBox_Dedicated.Size = new System.Drawing.Size(218, 38);
            this.checkBox_Dedicated.TabIndex = 101;
            this.checkBox_Dedicated.Text = "LN with Cipher Dedicated Key";
            this.checkBox_Dedicated.UseVisualStyleBackColor = true;
            // 
            // checkBox_LN
            // 
            this.checkBox_LN.AutoSize = true;
            this.checkBox_LN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBox_LN.Location = new System.Drawing.Point(3, 499);
            this.checkBox_LN.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBox_LN.Name = "checkBox_LN";
            this.checkBox_LN.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.checkBox_LN.Size = new System.Drawing.Size(217, 47);
            this.checkBox_LN.TabIndex = 98;
            this.checkBox_LN.Text = "LN with Cipher";
            this.checkBox_LN.UseVisualStyleBackColor = true;
            // 
            // chkConformanceBlock
            // 
            this.chkConformanceBlock.AutoSize = true;
            this.chkConformanceBlock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkConformanceBlock.Location = new System.Drawing.Point(226, 499);
            this.chkConformanceBlock.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkConformanceBlock.Name = "chkConformanceBlock";
            this.chkConformanceBlock.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.chkConformanceBlock.Size = new System.Drawing.Size(218, 47);
            this.chkConformanceBlock.TabIndex = 99;
            this.chkConformanceBlock.Text = "Conformance Block Negotiated";
            this.chkConformanceBlock.UseVisualStyleBackColor = true;
            // 
            // btnSaveData
            // 
            this.btnSaveData.BackColor = System.Drawing.Color.Transparent;
            this.btnSaveData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSaveData.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveData.Location = new System.Drawing.Point(3, 423);
            this.btnSaveData.Name = "btnSaveData";
            this.btnSaveData.Size = new System.Drawing.Size(217, 29);
            this.btnSaveData.TabIndex = 97;
            this.btnSaveData.Text = "💾 Save Settings";
            this.btnSaveData.UseVisualStyleBackColor = false;
            this.btnSaveData.Click += new System.EventHandler(this.btnSaveData_Click);
            // 
            // txtDISCToNDMTimeout
            // 
            this.txtDISCToNDMTimeout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDISCToNDMTimeout.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDISCToNDMTimeout.Location = new System.Drawing.Point(228, 395);
            this.txtDISCToNDMTimeout.Margin = new System.Windows.Forms.Padding(5);
            this.txtDISCToNDMTimeout.Name = "txtDISCToNDMTimeout";
            this.txtDISCToNDMTimeout.Size = new System.Drawing.Size(214, 24);
            this.txtDISCToNDMTimeout.TabIndex = 96;
            this.txtDISCToNDMTimeout.Text = "2200";
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.BackColor = System.Drawing.Color.Transparent;
            this.label33.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label33.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label33.ForeColor = System.Drawing.Color.Black;
            this.label33.Location = new System.Drawing.Point(5, 390);
            this.label33.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(213, 30);
            this.label33.TabIndex = 95;
            this.label33.Text = "DISC To NDM Timeout (ms)";
            this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtResponseTimeout
            // 
            this.txtResponseTimeout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtResponseTimeout.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtResponseTimeout.Location = new System.Drawing.Point(228, 365);
            this.txtResponseTimeout.Margin = new System.Windows.Forms.Padding(5);
            this.txtResponseTimeout.Name = "txtResponseTimeout";
            this.txtResponseTimeout.Size = new System.Drawing.Size(214, 24);
            this.txtResponseTimeout.TabIndex = 94;
            this.txtResponseTimeout.Text = "2000";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.BackColor = System.Drawing.Color.Transparent;
            this.label32.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label32.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label32.ForeColor = System.Drawing.Color.Black;
            this.label32.Location = new System.Drawing.Point(5, 360);
            this.label32.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(213, 30);
            this.label32.TabIndex = 93;
            this.label32.Text = "Response Timeout (ms)";
            this.label32.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtInterFrameTimeout
            // 
            this.txtInterFrameTimeout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtInterFrameTimeout.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInterFrameTimeout.Location = new System.Drawing.Point(228, 335);
            this.txtInterFrameTimeout.Margin = new System.Windows.Forms.Padding(5);
            this.txtInterFrameTimeout.Name = "txtInterFrameTimeout";
            this.txtInterFrameTimeout.Size = new System.Drawing.Size(214, 24);
            this.txtInterFrameTimeout.TabIndex = 92;
            this.txtInterFrameTimeout.Text = "1000";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.BackColor = System.Drawing.Color.Transparent;
            this.label31.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label31.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label31.ForeColor = System.Drawing.Color.Black;
            this.label31.Location = new System.Drawing.Point(5, 330);
            this.label31.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(213, 30);
            this.label31.TabIndex = 91;
            this.label31.Text = "Inter Frame Timeout (ms)";
            this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtInactivityTimeout
            // 
            this.txtInactivityTimeout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtInactivityTimeout.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInactivityTimeout.Location = new System.Drawing.Point(228, 305);
            this.txtInactivityTimeout.Margin = new System.Windows.Forms.Padding(5);
            this.txtInactivityTimeout.Name = "txtInactivityTimeout";
            this.txtInactivityTimeout.Size = new System.Drawing.Size(214, 24);
            this.txtInactivityTimeout.TabIndex = 90;
            this.txtInactivityTimeout.Text = "120000";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.BackColor = System.Drawing.Color.Transparent;
            this.label30.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label30.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label30.ForeColor = System.Drawing.Color.Black;
            this.label30.Location = new System.Drawing.Point(5, 300);
            this.label30.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(213, 30);
            this.label30.TabIndex = 89;
            this.label30.Text = "Inactivity Timeout (ms)";
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label100
            // 
            this.label100.AutoSize = true;
            this.label100.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label100.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label100.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label100.Location = new System.Drawing.Point(0, 180);
            this.label100.Margin = new System.Windows.Forms.Padding(0);
            this.label100.Name = "label100";
            this.label100.Size = new System.Drawing.Size(223, 30);
            this.label100.TabIndex = 88;
            this.label100.Text = "EK";
            this.label100.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtMasterKey
            // 
            this.txtMasterKey.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtMasterKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMasterKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMasterKey.Location = new System.Drawing.Point(228, 275);
            this.txtMasterKey.Margin = new System.Windows.Forms.Padding(5);
            this.txtMasterKey.Name = "txtMasterKey";
            this.txtMasterKey.Size = new System.Drawing.Size(214, 24);
            this.txtMasterKey.TabIndex = 87;
            this.txtMasterKey.Text = "GeNuSmAsteRkEy25";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.BackColor = System.Drawing.Color.Transparent;
            this.label18.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.Color.Black;
            this.label18.Location = new System.Drawing.Point(5, 270);
            this.label18.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(213, 30);
            this.label18.TabIndex = 86;
            this.label18.Text = "Master Key";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSysT
            // 
            this.txtSysT.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtSysT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSysT.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSysT.Location = new System.Drawing.Point(228, 245);
            this.txtSysT.Margin = new System.Windows.Forms.Padding(5);
            this.txtSysT.Name = "txtSysT";
            this.txtSysT.Size = new System.Drawing.Size(214, 24);
            this.txtSysT.TabIndex = 85;
            this.txtSysT.Text = "GOE00000";
            // 
            // label102
            // 
            this.label102.AutoSize = true;
            this.label102.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label102.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label102.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label102.Location = new System.Drawing.Point(0, 240);
            this.label102.Margin = new System.Windows.Forms.Padding(0);
            this.label102.Name = "label102";
            this.label102.Size = new System.Drawing.Size(223, 30);
            this.label102.TabIndex = 84;
            this.label102.Text = "Sys-T";
            this.label102.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAK
            // 
            this.txtAK.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtAK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAK.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAK.Location = new System.Drawing.Point(228, 215);
            this.txtAK.Margin = new System.Windows.Forms.Padding(5);
            this.txtAK.Name = "txtAK";
            this.txtAK.Size = new System.Drawing.Size(214, 24);
            this.txtAK.TabIndex = 83;
            this.txtAK.Text = "RsEbEkAkgjV97abc";
            // 
            // label101
            // 
            this.label101.AutoSize = true;
            this.label101.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label101.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label101.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label101.Location = new System.Drawing.Point(0, 210);
            this.label101.Margin = new System.Windows.Forms.Padding(0);
            this.label101.Name = "label101";
            this.label101.Size = new System.Drawing.Size(223, 30);
            this.label101.TabIndex = 82;
            this.label101.Text = "AK";
            this.label101.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtEK
            // 
            this.txtEK.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtEK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtEK.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEK.Location = new System.Drawing.Point(228, 185);
            this.txtEK.Margin = new System.Windows.Forms.Padding(5);
            this.txtEK.Name = "txtEK";
            this.txtEK.Size = new System.Drawing.Size(214, 24);
            this.txtEK.TabIndex = 81;
            this.txtEK.Text = "RsEbEkAkgjV97abc";
            // 
            // txtAuthPasswordFW
            // 
            this.txtAuthPasswordFW.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAuthPasswordFW.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAuthPasswordFW.Location = new System.Drawing.Point(228, 125);
            this.txtAuthPasswordFW.Margin = new System.Windows.Forms.Padding(5);
            this.txtAuthPasswordFW.Name = "txtAuthPasswordFW";
            this.txtAuthPasswordFW.Size = new System.Drawing.Size(214, 24);
            this.txtAuthPasswordFW.TabIndex = 79;
            this.txtAuthPasswordFW.Text = "RsEbHlSfgjV97abc";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.BackColor = System.Drawing.Color.Transparent;
            this.label27.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label27.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label27.ForeColor = System.Drawing.Color.Black;
            this.label27.Location = new System.Drawing.Point(5, 120);
            this.label27.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(213, 30);
            this.label27.TabIndex = 79;
            this.label27.Text = "HLS Auth Pass (FW)";
            this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(4, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(215, 30);
            this.label1.TabIndex = 1;
            this.label1.Text = "COM Port";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(5, 30);
            this.label6.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(213, 30);
            this.label6.TabIndex = 52;
            this.label6.Text = "Baud Rate";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbMeterComPort
            // 
            this.cbMeterComPort.BackColor = System.Drawing.SystemColors.Window;
            this.cbMeterComPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbMeterComPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMeterComPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbMeterComPort.FormattingEnabled = true;
            this.cbMeterComPort.Location = new System.Drawing.Point(227, 4);
            this.cbMeterComPort.Margin = new System.Windows.Forms.Padding(4);
            this.cbMeterComPort.Name = "cbMeterComPort";
            this.cbMeterComPort.Size = new System.Drawing.Size(216, 26);
            this.cbMeterComPort.TabIndex = 61;
            this.cbMeterComPort.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cbMeterComPort_MouseClick);
            // 
            // cbMeterBaudRate
            // 
            this.cbMeterBaudRate.BackColor = System.Drawing.SystemColors.Window;
            this.cbMeterBaudRate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbMeterBaudRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMeterBaudRate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbMeterBaudRate.FormattingEnabled = true;
            this.cbMeterBaudRate.Items.AddRange(new object[] {
            "9600",
            "57600"});
            this.cbMeterBaudRate.Location = new System.Drawing.Point(228, 35);
            this.cbMeterBaudRate.Margin = new System.Windows.Forms.Padding(5);
            this.cbMeterBaudRate.Name = "cbMeterBaudRate";
            this.cbMeterBaudRate.Size = new System.Drawing.Size(214, 26);
            this.cbMeterBaudRate.TabIndex = 62;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(5, 60);
            this.label8.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(213, 30);
            this.label8.TabIndex = 54;
            this.label8.Text = "Access Level ";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbAccessLevel
            // 
            this.cbAccessLevel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbAccessLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAccessLevel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbAccessLevel.Items.AddRange(new object[] {
            "Public Client",
            "Meter Reader",
            "Utility Settings",
            "Push",
            "Firmware Upgrade"});
            this.cbAccessLevel.Location = new System.Drawing.Point(228, 65);
            this.cbAccessLevel.Margin = new System.Windows.Forms.Padding(5);
            this.cbAccessLevel.Name = "cbAccessLevel";
            this.cbAccessLevel.Size = new System.Drawing.Size(214, 26);
            this.cbAccessLevel.TabIndex = 55;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(5, 90);
            this.label7.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(213, 30);
            this.label7.TabIndex = 56;
            this.label7.Text = "HLS Auth Pass (US)";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAuthPasswordWrite
            // 
            this.txtAuthPasswordWrite.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAuthPasswordWrite.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAuthPasswordWrite.Location = new System.Drawing.Point(228, 95);
            this.txtAuthPasswordWrite.Margin = new System.Windows.Forms.Padding(5);
            this.txtAuthPasswordWrite.Name = "txtAuthPasswordWrite";
            this.txtAuthPasswordWrite.Size = new System.Drawing.Size(214, 24);
            this.txtAuthPasswordWrite.TabIndex = 56;
            this.txtAuthPasswordWrite.Text = "RsEbHlSugjV97abc";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.Location = new System.Drawing.Point(5, 150);
            this.label10.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(213, 30);
            this.label10.TabIndex = 57;
            this.label10.Text = "LLS Auth Pass (MR)";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAuthPassword
            // 
            this.txtAuthPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAuthPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAuthPassword.Location = new System.Drawing.Point(228, 155);
            this.txtAuthPassword.Margin = new System.Windows.Forms.Padding(5);
            this.txtAuthPassword.Name = "txtAuthPassword";
            this.txtAuthPassword.Size = new System.Drawing.Size(214, 24);
            this.txtAuthPassword.TabIndex = 57;
            this.txtAuthPassword.Text = "1A2B3C4D";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.label15, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(447, 61);
            this.tableLayoutPanel3.TabIndex = 2;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(94)))), ((int)(((byte)(168)))));
            this.label15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.White;
            this.label15.Location = new System.Drawing.Point(0, 0);
            this.label15.Margin = new System.Windows.Forms.Padding(0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(447, 61);
            this.label15.TabIndex = 3;
            this.label15.Text = "Meter Settings";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1564, 898);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.splitter_DLMS_Source);
            this.Controls.Add(this.splitter_Meter_DLMS);
            this.Controls.Add(this.pnl_MeterSettings);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.TopMost = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.pnl_MeterSettings.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Splitter splitter_Meter_DLMS;
        private System.Windows.Forms.Splitter splitter_DLMS_Source;
        private System.Windows.Forms.Panel pnl_MeterSettings;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox txtAuthPasswordFW;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cbMeterComPort;
        private System.Windows.Forms.ComboBox cbMeterBaudRate;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cbAccessLevel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtAuthPasswordWrite;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtAuthPassword;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtDISCToNDMTimeout;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.TextBox txtResponseTimeout;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.TextBox txtInterFrameTimeout;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.TextBox txtInactivityTimeout;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label100;
        private System.Windows.Forms.TextBox txtMasterKey;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txtSysT;
        private System.Windows.Forms.Label label102;
        private System.Windows.Forms.TextBox txtAK;
        private System.Windows.Forms.Label label101;
        private System.Windows.Forms.TextBox txtEK;
        private System.Windows.Forms.Button btnSaveData;
        private System.Windows.Forms.CheckBox checkBox_GMAC;
        private System.Windows.Forms.CheckBox checkBox_Dedicated;
        private System.Windows.Forms.CheckBox checkBox_LN;
        private System.Windows.Forms.CheckBox chkConformanceBlock;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckedListBox chkListBoxConformanceBlock;
        private System.Windows.Forms.CheckBox checkBox_InvocationCounter;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.TextBox txtConfBlock;
    }
}