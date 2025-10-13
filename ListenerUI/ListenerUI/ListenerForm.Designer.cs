namespace ListenerUI
{
    partial class ListenerForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.tblMain = new System.Windows.Forms.TableLayoutPanel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.rtbPushLogs = new System.Windows.Forms.RichTextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tbpInstant = new System.Windows.Forms.TabPage();
            this.tbpLS = new System.Windows.Forms.TabPage();
            this.tbpDE = new System.Windows.Forms.TabPage();
            this.tbpSR = new System.Windows.Forms.TabPage();
            this.tbpBill = new System.Windows.Forms.TabPage();
            this.tbpCB = new System.Windows.Forms.TabPage();
            this.tbpAlert = new System.Windows.Forms.TabPage();
            this.tbpTamper = new System.Windows.Forms.TabPage();
            this.btnStartListener = new System.Windows.Forms.Button();
            this.btnStopListener = new System.Windows.Forms.Button();
            this.btnClearLogs = new System.Windows.Forms.Button();
            this.btnSaveData = new System.Windows.Forms.Button();
            this.lblHeader = new System.Windows.Forms.Label();
            this.tblHeader = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnPushprofileSettings = new System.Windows.Forms.Button();
            this.pnlProfileSettings = new System.Windows.Forms.Panel();
            this.grpProfileConfig = new System.Windows.Forms.GroupBox();
            this.tblProfileSettings = new System.Windows.Forms.TableLayoutPanel();
            this.lblCBProfile = new System.Windows.Forms.Label();
            this.txt_CB_DestIP = new System.Windows.Forms.TextBox();
            this.cb_CB_Frequency = new System.Windows.Forms.ComboBox();
            this.lblInstant = new System.Windows.Forms.Label();
            this.txt_Instant_DestIP = new System.Windows.Forms.TextBox();
            this.cbInstant_Frequency = new System.Windows.Forms.ComboBox();
            this.lblAlert = new System.Windows.Forms.Label();
            this.txt_Alert_DestIP = new System.Windows.Forms.TextBox();
            this.cb_Alert_Frequency = new System.Windows.Forms.ComboBox();
            this.lblBillingProfile = new System.Windows.Forms.Label();
            this.txt_Bill_DestIP = new System.Windows.Forms.TextBox();
            this.cb_Bill_Frequency = new System.Windows.Forms.ComboBox();
            this.lblSRProfile = new System.Windows.Forms.Label();
            this.txt_SR_DestIP = new System.Windows.Forms.TextBox();
            this.cb_SR_Frequency = new System.Windows.Forms.ComboBox();
            this.lblDEProfile = new System.Windows.Forms.Label();
            this.txt_DE_DestIP = new System.Windows.Forms.TextBox();
            this.cb_DE_Frequency = new System.Windows.Forms.ComboBox();
            this.lblLSProfile = new System.Windows.Forms.Label();
            this.txt_LS_DestIP = new System.Windows.Forms.TextBox();
            this.cb_LS_Frequency = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tblMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tblHeader.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.pnlProfileSettings.SuspendLayout();
            this.grpProfileConfig.SuspendLayout();
            this.tblProfileSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // tblMain
            // 
            this.tblMain.ColumnCount = 1;
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblMain.Controls.Add(this.splitContainer1, 0, 0);
            this.tblMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblMain.Location = new System.Drawing.Point(0, 356);
            this.tblMain.Name = "tblMain";
            this.tblMain.RowCount = 2;
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblMain.Size = new System.Drawing.Size(1693, 566);
            this.tblMain.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.rtbPushLogs);
            this.splitContainer1.Panel1MinSize = 50;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Panel2MinSize = 50;
            this.splitContainer1.Size = new System.Drawing.Size(1687, 515);
            this.splitContainer1.SplitterDistance = 625;
            this.splitContainer1.TabIndex = 5;
            // 
            // rtbPushLogs
            // 
            this.rtbPushLogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbPushLogs.Location = new System.Drawing.Point(0, 0);
            this.rtbPushLogs.Name = "rtbPushLogs";
            this.rtbPushLogs.Size = new System.Drawing.Size(625, 515);
            this.rtbPushLogs.TabIndex = 0;
            this.rtbPushLogs.Text = "";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tbpInstant);
            this.tabControl1.Controls.Add(this.tbpLS);
            this.tabControl1.Controls.Add(this.tbpDE);
            this.tabControl1.Controls.Add(this.tbpSR);
            this.tabControl1.Controls.Add(this.tbpBill);
            this.tabControl1.Controls.Add(this.tbpCB);
            this.tabControl1.Controls.Add(this.tbpAlert);
            this.tabControl1.Controls.Add(this.tbpTamper);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1058, 515);
            this.tabControl1.TabIndex = 0;
            // 
            // tbpInstant
            // 
            this.tbpInstant.Location = new System.Drawing.Point(4, 25);
            this.tbpInstant.Margin = new System.Windows.Forms.Padding(0);
            this.tbpInstant.Name = "tbpInstant";
            this.tbpInstant.Padding = new System.Windows.Forms.Padding(3);
            this.tbpInstant.Size = new System.Drawing.Size(1050, 486);
            this.tbpInstant.TabIndex = 0;
            this.tbpInstant.Text = "Instant";
            this.tbpInstant.UseVisualStyleBackColor = true;
            // 
            // tbpLS
            // 
            this.tbpLS.Location = new System.Drawing.Point(4, 25);
            this.tbpLS.Name = "tbpLS";
            this.tbpLS.Padding = new System.Windows.Forms.Padding(3);
            this.tbpLS.Size = new System.Drawing.Size(1050, 493);
            this.tbpLS.TabIndex = 1;
            this.tbpLS.Text = "Load Survey";
            this.tbpLS.UseVisualStyleBackColor = true;
            // 
            // tbpDE
            // 
            this.tbpDE.Location = new System.Drawing.Point(4, 25);
            this.tbpDE.Name = "tbpDE";
            this.tbpDE.Size = new System.Drawing.Size(1050, 493);
            this.tbpDE.TabIndex = 2;
            this.tbpDE.Text = "Daily Energy";
            this.tbpDE.UseVisualStyleBackColor = true;
            // 
            // tbpSR
            // 
            this.tbpSR.Location = new System.Drawing.Point(4, 25);
            this.tbpSR.Name = "tbpSR";
            this.tbpSR.Size = new System.Drawing.Size(1050, 493);
            this.tbpSR.TabIndex = 3;
            this.tbpSR.Text = "Self Registration";
            this.tbpSR.UseVisualStyleBackColor = true;
            // 
            // tbpBill
            // 
            this.tbpBill.Location = new System.Drawing.Point(4, 25);
            this.tbpBill.Name = "tbpBill";
            this.tbpBill.Size = new System.Drawing.Size(1050, 493);
            this.tbpBill.TabIndex = 4;
            this.tbpBill.Text = "Billing";
            this.tbpBill.UseVisualStyleBackColor = true;
            // 
            // tbpCB
            // 
            this.tbpCB.Location = new System.Drawing.Point(4, 25);
            this.tbpCB.Name = "tbpCB";
            this.tbpCB.Size = new System.Drawing.Size(1050, 493);
            this.tbpCB.TabIndex = 5;
            this.tbpCB.Text = "Current Bill";
            this.tbpCB.UseVisualStyleBackColor = true;
            // 
            // tbpAlert
            // 
            this.tbpAlert.Location = new System.Drawing.Point(4, 25);
            this.tbpAlert.Name = "tbpAlert";
            this.tbpAlert.Size = new System.Drawing.Size(1050, 493);
            this.tbpAlert.TabIndex = 6;
            this.tbpAlert.Text = "Alert";
            this.tbpAlert.UseVisualStyleBackColor = true;
            // 
            // tbpTamper
            // 
            this.tbpTamper.Location = new System.Drawing.Point(4, 25);
            this.tbpTamper.Name = "tbpTamper";
            this.tbpTamper.Size = new System.Drawing.Size(1050, 493);
            this.tbpTamper.TabIndex = 7;
            this.tbpTamper.Text = "Tamper";
            this.tbpTamper.UseVisualStyleBackColor = true;
            // 
            // btnStartListener
            // 
            this.btnStartListener.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btnStartListener.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartListener.Location = new System.Drawing.Point(261, 3);
            this.btnStartListener.Name = "btnStartListener";
            this.btnStartListener.Size = new System.Drawing.Size(177, 30);
            this.btnStartListener.TabIndex = 0;
            this.btnStartListener.Text = "▶ Start Listening";
            this.btnStartListener.UseVisualStyleBackColor = false;
            this.btnStartListener.Click += new System.EventHandler(this.btnStartListener_Click);
            // 
            // btnStopListener
            // 
            this.btnStopListener.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btnStopListener.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStopListener.Location = new System.Drawing.Point(444, 3);
            this.btnStopListener.Name = "btnStopListener";
            this.btnStopListener.Size = new System.Drawing.Size(182, 30);
            this.btnStopListener.TabIndex = 1;
            this.btnStopListener.Text = "⏹ Stop Listening";
            this.btnStopListener.UseVisualStyleBackColor = false;
            this.btnStopListener.Click += new System.EventHandler(this.btnStopListener_Click);
            // 
            // btnClearLogs
            // 
            this.btnClearLogs.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btnClearLogs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearLogs.Location = new System.Drawing.Point(632, 3);
            this.btnClearLogs.Name = "btnClearLogs";
            this.btnClearLogs.Size = new System.Drawing.Size(151, 30);
            this.btnClearLogs.TabIndex = 2;
            this.btnClearLogs.Text = "🧹 Clear Logs";
            this.btnClearLogs.UseVisualStyleBackColor = false;
            this.btnClearLogs.Click += new System.EventHandler(this.btnClearLogs_Click);
            // 
            // btnSaveData
            // 
            this.btnSaveData.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btnSaveData.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveData.Location = new System.Drawing.Point(789, 3);
            this.btnSaveData.Name = "btnSaveData";
            this.btnSaveData.Size = new System.Drawing.Size(146, 30);
            this.btnSaveData.TabIndex = 3;
            this.btnSaveData.Text = "💾 Save Data";
            this.btnSaveData.UseVisualStyleBackColor = false;
            this.btnSaveData.Click += new System.EventHandler(this.btnSaveData_Click);
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.BackColor = System.Drawing.Color.Indigo;
            this.lblHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold);
            this.lblHeader.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblHeader.Location = new System.Drawing.Point(3, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(1687, 40);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "Push Settings and Notifications";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tblHeader
            // 
            this.tblHeader.ColumnCount = 1;
            this.tblHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblHeader.Controls.Add(this.flowLayoutPanel1, 0, 1);
            this.tblHeader.Controls.Add(this.lblHeader, 0, 0);
            this.tblHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblHeader.Location = new System.Drawing.Point(0, 0);
            this.tblHeader.Name = "tblHeader";
            this.tblHeader.RowCount = 2;
            this.tblHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tblHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tblHeader.Size = new System.Drawing.Size(1693, 76);
            this.tblHeader.TabIndex = 9;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel1.Controls.Add(this.btnPushprofileSettings);
            this.flowLayoutPanel1.Controls.Add(this.btnStartListener);
            this.flowLayoutPanel1.Controls.Add(this.btnStopListener);
            this.flowLayoutPanel1.Controls.Add(this.btnClearLogs);
            this.flowLayoutPanel1.Controls.Add(this.btnSaveData);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 40);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1693, 40);
            this.flowLayoutPanel1.TabIndex = 5;
            // 
            // btnPushprofileSettings
            // 
            this.btnPushprofileSettings.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btnPushprofileSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPushprofileSettings.Location = new System.Drawing.Point(3, 3);
            this.btnPushprofileSettings.Name = "btnPushprofileSettings";
            this.btnPushprofileSettings.Size = new System.Drawing.Size(252, 30);
            this.btnPushprofileSettings.TabIndex = 4;
            this.btnPushprofileSettings.Text = "▼ Push Profile Settings";
            this.btnPushprofileSettings.UseVisualStyleBackColor = false;
            this.btnPushprofileSettings.Click += new System.EventHandler(this.btnPushprofileSettings_Click);
            // 
            // pnlProfileSettings
            // 
            this.pnlProfileSettings.Controls.Add(this.grpProfileConfig);
            this.pnlProfileSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlProfileSettings.Location = new System.Drawing.Point(0, 76);
            this.pnlProfileSettings.MaximumSize = new System.Drawing.Size(2000, 280);
            this.pnlProfileSettings.MinimumSize = new System.Drawing.Size(1700, 280);
            this.pnlProfileSettings.Name = "pnlProfileSettings";
            this.pnlProfileSettings.Size = new System.Drawing.Size(1700, 280);
            this.pnlProfileSettings.TabIndex = 0;
            this.pnlProfileSettings.Visible = false;
            // 
            // grpProfileConfig
            // 
            this.grpProfileConfig.Controls.Add(this.tblProfileSettings);
            this.grpProfileConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpProfileConfig.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpProfileConfig.Location = new System.Drawing.Point(0, 0);
            this.grpProfileConfig.Margin = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.grpProfileConfig.Name = "grpProfileConfig";
            this.grpProfileConfig.Size = new System.Drawing.Size(1700, 280);
            this.grpProfileConfig.TabIndex = 10;
            this.grpProfileConfig.TabStop = false;
            this.grpProfileConfig.Text = "Push Configuration Settings";
            // 
            // tblProfileSettings
            // 
            this.tblProfileSettings.ColumnCount = 4;
            this.tblProfileSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 255F));
            this.tblProfileSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblProfileSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblProfileSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 500F));
            this.tblProfileSettings.Controls.Add(this.lblCBProfile, 0, 7);
            this.tblProfileSettings.Controls.Add(this.txt_CB_DestIP, 1, 7);
            this.tblProfileSettings.Controls.Add(this.cb_CB_Frequency, 2, 7);
            this.tblProfileSettings.Controls.Add(this.lblInstant, 0, 1);
            this.tblProfileSettings.Controls.Add(this.txt_Instant_DestIP, 1, 1);
            this.tblProfileSettings.Controls.Add(this.cbInstant_Frequency, 2, 1);
            this.tblProfileSettings.Controls.Add(this.lblAlert, 0, 2);
            this.tblProfileSettings.Controls.Add(this.txt_Alert_DestIP, 1, 2);
            this.tblProfileSettings.Controls.Add(this.cb_Alert_Frequency, 2, 2);
            this.tblProfileSettings.Controls.Add(this.lblBillingProfile, 0, 3);
            this.tblProfileSettings.Controls.Add(this.txt_Bill_DestIP, 1, 3);
            this.tblProfileSettings.Controls.Add(this.cb_Bill_Frequency, 2, 3);
            this.tblProfileSettings.Controls.Add(this.lblSRProfile, 0, 4);
            this.tblProfileSettings.Controls.Add(this.txt_SR_DestIP, 1, 4);
            this.tblProfileSettings.Controls.Add(this.cb_SR_Frequency, 2, 4);
            this.tblProfileSettings.Controls.Add(this.lblDEProfile, 0, 5);
            this.tblProfileSettings.Controls.Add(this.txt_DE_DestIP, 1, 5);
            this.tblProfileSettings.Controls.Add(this.cb_DE_Frequency, 2, 5);
            this.tblProfileSettings.Controls.Add(this.lblLSProfile, 0, 6);
            this.tblProfileSettings.Controls.Add(this.txt_LS_DestIP, 1, 6);
            this.tblProfileSettings.Controls.Add(this.cb_LS_Frequency, 2, 6);
            this.tblProfileSettings.Controls.Add(this.label1, 0, 0);
            this.tblProfileSettings.Controls.Add(this.label2, 1, 0);
            this.tblProfileSettings.Controls.Add(this.label3, 2, 0);
            this.tblProfileSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblProfileSettings.Location = new System.Drawing.Point(3, 26);
            this.tblProfileSettings.Name = "tblProfileSettings";
            this.tblProfileSettings.RowCount = 8;
            this.tblProfileSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tblProfileSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tblProfileSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tblProfileSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tblProfileSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tblProfileSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tblProfileSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tblProfileSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tblProfileSettings.Size = new System.Drawing.Size(1694, 251);
            this.tblProfileSettings.TabIndex = 0;
            // 
            // lblCBProfile
            // 
            this.lblCBProfile.AutoSize = true;
            this.lblCBProfile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCBProfile.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCBProfile.Location = new System.Drawing.Point(3, 217);
            this.lblCBProfile.Name = "lblCBProfile";
            this.lblCBProfile.Size = new System.Drawing.Size(249, 34);
            this.lblCBProfile.TabIndex = 45;
            this.lblCBProfile.Text = "Current Bill Profile";
            this.lblCBProfile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txt_CB_DestIP
            // 
            this.txt_CB_DestIP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_CB_DestIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_CB_DestIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_CB_DestIP.Location = new System.Drawing.Point(258, 220);
            this.txt_CB_DestIP.Name = "txt_CB_DestIP";
            this.txt_CB_DestIP.Size = new System.Drawing.Size(463, 27);
            this.txt_CB_DestIP.TabIndex = 46;
            this.txt_CB_DestIP.Text = "2403:8600:2090:14::27[4059]";
            // 
            // cb_CB_Frequency
            // 
            this.cb_CB_Frequency.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb_CB_Frequency.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_CB_Frequency.FormattingEnabled = true;
            this.cb_CB_Frequency.Items.AddRange(new object[] {
            "*/*/* 00:00:00 (Midnight)"});
            this.cb_CB_Frequency.Location = new System.Drawing.Point(727, 220);
            this.cb_CB_Frequency.Name = "cb_CB_Frequency";
            this.cb_CB_Frequency.Size = new System.Drawing.Size(463, 28);
            this.cb_CB_Frequency.TabIndex = 47;
            // 
            // lblInstant
            // 
            this.lblInstant.AutoSize = true;
            this.lblInstant.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblInstant.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInstant.Location = new System.Drawing.Point(3, 31);
            this.lblInstant.Name = "lblInstant";
            this.lblInstant.Size = new System.Drawing.Size(249, 31);
            this.lblInstant.TabIndex = 39;
            this.lblInstant.Text = "Instant Profile";
            this.lblInstant.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txt_Instant_DestIP
            // 
            this.txt_Instant_DestIP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_Instant_DestIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_Instant_DestIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_Instant_DestIP.Location = new System.Drawing.Point(258, 34);
            this.txt_Instant_DestIP.Name = "txt_Instant_DestIP";
            this.txt_Instant_DestIP.Size = new System.Drawing.Size(463, 27);
            this.txt_Instant_DestIP.TabIndex = 40;
            this.txt_Instant_DestIP.Text = "2403:8600:2090:14::27[4059]";
            // 
            // cbInstant_Frequency
            // 
            this.cbInstant_Frequency.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbInstant_Frequency.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbInstant_Frequency.FormattingEnabled = true;
            this.cbInstant_Frequency.Items.AddRange(new object[] {
            "15 Min",
            "30 Min",
            "1 Hour",
            "4 Hour",
            "6 Hour",
            "8 Hour",
            "12 Hour",
            "24 Hour"});
            this.cbInstant_Frequency.Location = new System.Drawing.Point(727, 34);
            this.cbInstant_Frequency.Name = "cbInstant_Frequency";
            this.cbInstant_Frequency.Size = new System.Drawing.Size(463, 28);
            this.cbInstant_Frequency.TabIndex = 41;
            // 
            // lblAlert
            // 
            this.lblAlert.AutoSize = true;
            this.lblAlert.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAlert.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAlert.Location = new System.Drawing.Point(3, 62);
            this.lblAlert.Name = "lblAlert";
            this.lblAlert.Size = new System.Drawing.Size(249, 31);
            this.lblAlert.TabIndex = 36;
            this.lblAlert.Text = "Alert Profile";
            this.lblAlert.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txt_Alert_DestIP
            // 
            this.txt_Alert_DestIP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_Alert_DestIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_Alert_DestIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_Alert_DestIP.Location = new System.Drawing.Point(258, 65);
            this.txt_Alert_DestIP.Name = "txt_Alert_DestIP";
            this.txt_Alert_DestIP.Size = new System.Drawing.Size(463, 27);
            this.txt_Alert_DestIP.TabIndex = 37;
            this.txt_Alert_DestIP.Text = "2403:8600:2090:14::27[4059]";
            // 
            // cb_Alert_Frequency
            // 
            this.cb_Alert_Frequency.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb_Alert_Frequency.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_Alert_Frequency.FormattingEnabled = true;
            this.cb_Alert_Frequency.Location = new System.Drawing.Point(727, 65);
            this.cb_Alert_Frequency.Name = "cb_Alert_Frequency";
            this.cb_Alert_Frequency.Size = new System.Drawing.Size(463, 28);
            this.cb_Alert_Frequency.TabIndex = 38;
            // 
            // lblBillingProfile
            // 
            this.lblBillingProfile.AutoSize = true;
            this.lblBillingProfile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBillingProfile.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBillingProfile.Location = new System.Drawing.Point(3, 93);
            this.lblBillingProfile.Name = "lblBillingProfile";
            this.lblBillingProfile.Size = new System.Drawing.Size(249, 31);
            this.lblBillingProfile.TabIndex = 33;
            this.lblBillingProfile.Text = "Billing Profile";
            this.lblBillingProfile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txt_Bill_DestIP
            // 
            this.txt_Bill_DestIP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_Bill_DestIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_Bill_DestIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_Bill_DestIP.Location = new System.Drawing.Point(258, 96);
            this.txt_Bill_DestIP.Name = "txt_Bill_DestIP";
            this.txt_Bill_DestIP.Size = new System.Drawing.Size(463, 27);
            this.txt_Bill_DestIP.TabIndex = 34;
            this.txt_Bill_DestIP.Text = "2403:8600:2090:14::27[4059]";
            // 
            // cb_Bill_Frequency
            // 
            this.cb_Bill_Frequency.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb_Bill_Frequency.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_Bill_Frequency.FormattingEnabled = true;
            this.cb_Bill_Frequency.Location = new System.Drawing.Point(727, 96);
            this.cb_Bill_Frequency.Name = "cb_Bill_Frequency";
            this.cb_Bill_Frequency.Size = new System.Drawing.Size(463, 28);
            this.cb_Bill_Frequency.TabIndex = 35;
            // 
            // lblSRProfile
            // 
            this.lblSRProfile.AutoSize = true;
            this.lblSRProfile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSRProfile.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSRProfile.Location = new System.Drawing.Point(3, 124);
            this.lblSRProfile.Name = "lblSRProfile";
            this.lblSRProfile.Size = new System.Drawing.Size(249, 31);
            this.lblSRProfile.TabIndex = 30;
            this.lblSRProfile.Text = "Self Registration Profile";
            this.lblSRProfile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txt_SR_DestIP
            // 
            this.txt_SR_DestIP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_SR_DestIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_SR_DestIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_SR_DestIP.Location = new System.Drawing.Point(258, 127);
            this.txt_SR_DestIP.Name = "txt_SR_DestIP";
            this.txt_SR_DestIP.Size = new System.Drawing.Size(463, 27);
            this.txt_SR_DestIP.TabIndex = 31;
            this.txt_SR_DestIP.Text = "2403:8600:2090:14::27[4059]";
            // 
            // cb_SR_Frequency
            // 
            this.cb_SR_Frequency.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb_SR_Frequency.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_SR_Frequency.FormattingEnabled = true;
            this.cb_SR_Frequency.Items.AddRange(new object[] {
            "15 Min",
            "30 Min",
            "1 Hour",
            "4 Hour",
            "6 Hour",
            "8 Hour",
            "12 Hour",
            "24 Hour"});
            this.cb_SR_Frequency.Location = new System.Drawing.Point(727, 127);
            this.cb_SR_Frequency.Name = "cb_SR_Frequency";
            this.cb_SR_Frequency.Size = new System.Drawing.Size(463, 28);
            this.cb_SR_Frequency.TabIndex = 32;
            // 
            // lblDEProfile
            // 
            this.lblDEProfile.AutoSize = true;
            this.lblDEProfile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDEProfile.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDEProfile.Location = new System.Drawing.Point(3, 155);
            this.lblDEProfile.Name = "lblDEProfile";
            this.lblDEProfile.Size = new System.Drawing.Size(249, 31);
            this.lblDEProfile.TabIndex = 27;
            this.lblDEProfile.Text = "Daily Energy Profile";
            this.lblDEProfile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txt_DE_DestIP
            // 
            this.txt_DE_DestIP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_DE_DestIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_DE_DestIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_DE_DestIP.Location = new System.Drawing.Point(258, 158);
            this.txt_DE_DestIP.Name = "txt_DE_DestIP";
            this.txt_DE_DestIP.Size = new System.Drawing.Size(463, 27);
            this.txt_DE_DestIP.TabIndex = 28;
            this.txt_DE_DestIP.Text = "2403:8600:2090:14::27[4059]";
            // 
            // cb_DE_Frequency
            // 
            this.cb_DE_Frequency.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb_DE_Frequency.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_DE_Frequency.FormattingEnabled = true;
            this.cb_DE_Frequency.Items.AddRange(new object[] {
            "*/*/* 00:00:00 (Midnight)"});
            this.cb_DE_Frequency.Location = new System.Drawing.Point(727, 158);
            this.cb_DE_Frequency.Name = "cb_DE_Frequency";
            this.cb_DE_Frequency.Size = new System.Drawing.Size(463, 28);
            this.cb_DE_Frequency.TabIndex = 29;
            // 
            // lblLSProfile
            // 
            this.lblLSProfile.AutoSize = true;
            this.lblLSProfile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLSProfile.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLSProfile.Location = new System.Drawing.Point(3, 186);
            this.lblLSProfile.Name = "lblLSProfile";
            this.lblLSProfile.Size = new System.Drawing.Size(249, 31);
            this.lblLSProfile.TabIndex = 24;
            this.lblLSProfile.Text = "Load Survey Profile";
            this.lblLSProfile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txt_LS_DestIP
            // 
            this.txt_LS_DestIP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_LS_DestIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_LS_DestIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_LS_DestIP.Location = new System.Drawing.Point(258, 189);
            this.txt_LS_DestIP.Name = "txt_LS_DestIP";
            this.txt_LS_DestIP.Size = new System.Drawing.Size(463, 27);
            this.txt_LS_DestIP.TabIndex = 25;
            this.txt_LS_DestIP.Text = "2403:8600:2090:14::27[4059]";
            // 
            // cb_LS_Frequency
            // 
            this.cb_LS_Frequency.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb_LS_Frequency.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_LS_Frequency.FormattingEnabled = true;
            this.cb_LS_Frequency.Items.AddRange(new object[] {
            "15 Min",
            "30 Min",
            "1 Hour",
            "4 Hour",
            "6 Hour",
            "8 Hour",
            "12 Hour",
            "24 Hour"});
            this.cb_LS_Frequency.Location = new System.Drawing.Point(727, 189);
            this.cb_LS_Frequency.Name = "cb_LS_Frequency";
            this.cb_LS_Frequency.Size = new System.Drawing.Size(463, 28);
            this.cb_LS_Frequency.TabIndex = 26;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(249, 31);
            this.label1.TabIndex = 0;
            this.label1.Text = "Profiles";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(258, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(463, 31);
            this.label2.TabIndex = 1;
            this.label2.Text = "Destination IP Address";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(727, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(463, 31);
            this.label3.TabIndex = 2;
            this.label3.Text = "Push Frequency Schedule";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ListenerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1693, 922);
            this.Controls.Add(this.tblMain);
            this.Controls.Add(this.pnlProfileSettings);
            this.Controls.Add(this.tblHeader);
            this.Name = "ListenerForm";
            this.Text = "DLMS Push Listener UI";
            this.tblMain.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tblHeader.ResumeLayout(false);
            this.tblHeader.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.pnlProfileSettings.ResumeLayout(false);
            this.grpProfileConfig.ResumeLayout(false);
            this.tblProfileSettings.ResumeLayout(false);
            this.tblProfileSettings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tblMain;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Button btnStartListener;
        private System.Windows.Forms.Button btnStopListener;
        private System.Windows.Forms.Button btnClearLogs;
        private System.Windows.Forms.Button btnSaveData;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.RichTextBox rtbPushLogs;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tbpInstant;
        private System.Windows.Forms.TabPage tbpLS;
        private System.Windows.Forms.TabPage tbpDE;
        private System.Windows.Forms.TabPage tbpSR;
        private System.Windows.Forms.TabPage tbpBill;
        private System.Windows.Forms.TabPage tbpCB;
        private System.Windows.Forms.TabPage tbpAlert;
        private System.Windows.Forms.TabPage tbpTamper;
        private System.Windows.Forms.TableLayoutPanel tblHeader;
        private System.Windows.Forms.Panel pnlProfileSettings;
        private System.Windows.Forms.GroupBox grpProfileConfig;
        private System.Windows.Forms.TableLayoutPanel tblProfileSettings;
        private System.Windows.Forms.Label lblCBProfile;
        private System.Windows.Forms.TextBox txt_CB_DestIP;
        private System.Windows.Forms.ComboBox cb_CB_Frequency;
        private System.Windows.Forms.Label lblInstant;
        private System.Windows.Forms.TextBox txt_Instant_DestIP;
        private System.Windows.Forms.ComboBox cbInstant_Frequency;
        private System.Windows.Forms.Label lblAlert;
        private System.Windows.Forms.TextBox txt_Alert_DestIP;
        private System.Windows.Forms.ComboBox cb_Alert_Frequency;
        private System.Windows.Forms.Label lblBillingProfile;
        private System.Windows.Forms.TextBox txt_Bill_DestIP;
        private System.Windows.Forms.ComboBox cb_Bill_Frequency;
        private System.Windows.Forms.Label lblSRProfile;
        private System.Windows.Forms.TextBox txt_SR_DestIP;
        private System.Windows.Forms.ComboBox cb_SR_Frequency;
        private System.Windows.Forms.Label lblDEProfile;
        private System.Windows.Forms.TextBox txt_DE_DestIP;
        private System.Windows.Forms.ComboBox cb_DE_Frequency;
        private System.Windows.Forms.Label lblLSProfile;
        private System.Windows.Forms.TextBox txt_LS_DestIP;
        private System.Windows.Forms.ComboBox cb_LS_Frequency;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnPushprofileSettings;
    }
}
