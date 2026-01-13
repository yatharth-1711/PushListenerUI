using System.Windows.Forms;

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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea5 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend5 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea6 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend6 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea7 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend7 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series7 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea8 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend8 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series8 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tblMain = new System.Windows.Forms.TableLayoutPanel();
            this.splitConMain = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.rtbPushLogs = new System.Windows.Forms.RichTextBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.PanelProfileandRawData = new System.Windows.Forms.Panel();
            this.tabControlProfiles = new System.Windows.Forms.TabControl();
            this.tbpInstant = new System.Windows.Forms.TabPage();
            this.splitcon_InstantTab = new System.Windows.Forms.SplitContainer();
            this.dgInstant = new System.Windows.Forms.DataGridView();
            this.chart_Instant = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tbpLS = new System.Windows.Forms.TabPage();
            this.splitCon_LS = new System.Windows.Forms.SplitContainer();
            this.dgLS = new System.Windows.Forms.DataGridView();
            this.chart_LS = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tbpDE = new System.Windows.Forms.TabPage();
            this.splitCon_DE = new System.Windows.Forms.SplitContainer();
            this.dgDE = new System.Windows.Forms.DataGridView();
            this.chart_DE = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tbpSR = new System.Windows.Forms.TabPage();
            this.splitCon_SR = new System.Windows.Forms.SplitContainer();
            this.dgSR = new System.Windows.Forms.DataGridView();
            this.chart_SR = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tbpBill = new System.Windows.Forms.TabPage();
            this.splitCon_Bill = new System.Windows.Forms.SplitContainer();
            this.dgBill = new System.Windows.Forms.DataGridView();
            this.chart_Bill = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tbpCB = new System.Windows.Forms.TabPage();
            this.splitCon_CB = new System.Windows.Forms.SplitContainer();
            this.dgCB = new System.Windows.Forms.DataGridView();
            this.chart_CB = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tbpAlert = new System.Windows.Forms.TabPage();
            this.splitCon_Alert = new System.Windows.Forms.SplitContainer();
            this.dgAlert = new System.Windows.Forms.DataGridView();
            this.chart_Alert = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tbpTamper = new System.Windows.Forms.TabPage();
            this.splitCon_Tamper = new System.Windows.Forms.SplitContainer();
            this.dgTamper = new System.Windows.Forms.DataGridView();
            this.chart_tamper = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.dgRawData = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.lblHeader = new System.Windows.Forms.Label();
            this.tblHeader = new System.Windows.Forms.TableLayoutPanel();
            this.flowPanelButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnPushprofileSettings = new System.Windows.Forms.Button();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.rbBtnDetailLog = new System.Windows.Forms.RadioButton();
            this.rbBtnUserDefinedLog = new System.Windows.Forms.RadioButton();
            this.btnNotifyDecryptSettings = new System.Windows.Forms.Button();
            this.btnNotificationType = new System.Windows.Forms.Button();
            this.btnStartListener = new System.Windows.Forms.Button();
            this.btnStopListener = new System.Windows.Forms.Button();
            this.btnClearLogs = new System.Windows.Forms.Button();
            this.btnSaveData = new System.Windows.Forms.Button();
            this.btnRawData = new System.Windows.Forms.Button();
            this.pnlProfileSettings = new System.Windows.Forms.Panel();
            this.grpPushObjects = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.tvPushObjects = new System.Windows.Forms.TreeView();
            this.DGPushProfile = new System.Windows.Forms.DataGridView();
            this.grpProfileConfig = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.btnSet_PS_AS = new System.Windows.Forms.Button();
            this.cbTestProfileType = new System.Windows.Forms.ComboBox();
            this.lblProfile = new System.Windows.Forms.Label();
            this.btnGet_PS_AS = new System.Windows.Forms.Button();
            this.tblProfileSettings = new System.Windows.Forms.TableLayoutPanel();
            this.txt_Random_CB = new System.Windows.Forms.TextBox();
            this.txt_Random_LS = new System.Windows.Forms.TextBox();
            this.txt_Random_DE = new System.Windows.Forms.TextBox();
            this.txt_Random_SR = new System.Windows.Forms.TextBox();
            this.txt_Random_Bill = new System.Windows.Forms.TextBox();
            this.txt_Random_Instant = new System.Windows.Forms.TextBox();
            this.lblRandomHeader = new System.Windows.Forms.Label();
            this.lblCBProfile = new System.Windows.Forms.Label();
            this.txt_CB_DestIP = new System.Windows.Forms.TextBox();
            this.lblInstant = new System.Windows.Forms.Label();
            this.txt_Instant_DestIP = new System.Windows.Forms.TextBox();
            this.cbInstant_Frequency = new System.Windows.Forms.ComboBox();
            this.lblAlert = new System.Windows.Forms.Label();
            this.txt_Alert_DestIP = new System.Windows.Forms.TextBox();
            this.lblBillingProfile = new System.Windows.Forms.Label();
            this.txt_Bill_DestIP = new System.Windows.Forms.TextBox();
            this.lblSRProfile = new System.Windows.Forms.Label();
            this.txt_SR_DestIP = new System.Windows.Forms.TextBox();
            this.cb_SR_Frequency = new System.Windows.Forms.ComboBox();
            this.lblDEProfile = new System.Windows.Forms.Label();
            this.txt_DE_DestIP = new System.Windows.Forms.TextBox();
            this.cb_DE_Frequency = new System.Windows.Forms.ComboBox();
            this.lblLSProfile = new System.Windows.Forms.Label();
            this.txt_LS_DestIP = new System.Windows.Forms.TextBox();
            this.cb_LS_Frequency = new System.Windows.Forms.ComboBox();
            this.lblProfileHeader = new System.Windows.Forms.Label();
            this.lblDestIPHeader = new System.Windows.Forms.Label();
            this.lblPushFreqHeader = new System.Windows.Forms.Label();
            this.cb_CB_Frequency = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.cb_Bill_Frequency = new System.Windows.Forms.ComboBox();
            this.txtBillFreq = new System.Windows.Forms.MaskedTextBox();
            this.cmsNotificationOption = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cbHexCiphered = new System.Windows.Forms.ToolStripMenuItem();
            this.cbHexDecrypted = new System.Windows.Forms.ToolStripMenuItem();
            this.cbXML = new System.Windows.Forms.ToolStripMenuItem();
            this.tblMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitConMain)).BeginInit();
            this.splitConMain.Panel1.SuspendLayout();
            this.splitConMain.Panel2.SuspendLayout();
            this.splitConMain.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.PanelProfileandRawData.SuspendLayout();
            this.tabControlProfiles.SuspendLayout();
            this.tbpInstant.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitcon_InstantTab)).BeginInit();
            this.splitcon_InstantTab.Panel1.SuspendLayout();
            this.splitcon_InstantTab.Panel2.SuspendLayout();
            this.splitcon_InstantTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgInstant)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_Instant)).BeginInit();
            this.tbpLS.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitCon_LS)).BeginInit();
            this.splitCon_LS.Panel1.SuspendLayout();
            this.splitCon_LS.Panel2.SuspendLayout();
            this.splitCon_LS.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgLS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_LS)).BeginInit();
            this.tbpDE.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitCon_DE)).BeginInit();
            this.splitCon_DE.Panel1.SuspendLayout();
            this.splitCon_DE.Panel2.SuspendLayout();
            this.splitCon_DE.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_DE)).BeginInit();
            this.tbpSR.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitCon_SR)).BeginInit();
            this.splitCon_SR.Panel1.SuspendLayout();
            this.splitCon_SR.Panel2.SuspendLayout();
            this.splitCon_SR.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgSR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_SR)).BeginInit();
            this.tbpBill.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitCon_Bill)).BeginInit();
            this.splitCon_Bill.Panel1.SuspendLayout();
            this.splitCon_Bill.Panel2.SuspendLayout();
            this.splitCon_Bill.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgBill)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_Bill)).BeginInit();
            this.tbpCB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitCon_CB)).BeginInit();
            this.splitCon_CB.Panel1.SuspendLayout();
            this.splitCon_CB.Panel2.SuspendLayout();
            this.splitCon_CB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgCB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_CB)).BeginInit();
            this.tbpAlert.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitCon_Alert)).BeginInit();
            this.splitCon_Alert.Panel1.SuspendLayout();
            this.splitCon_Alert.Panel2.SuspendLayout();
            this.splitCon_Alert.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgAlert)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_Alert)).BeginInit();
            this.tbpTamper.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitCon_Tamper)).BeginInit();
            this.splitCon_Tamper.Panel1.SuspendLayout();
            this.splitCon_Tamper.Panel2.SuspendLayout();
            this.splitCon_Tamper.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgTamper)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_tamper)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgRawData)).BeginInit();
            this.tblHeader.SuspendLayout();
            this.flowPanelButtons.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.pnlProfileSettings.SuspendLayout();
            this.grpPushObjects.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGPushProfile)).BeginInit();
            this.grpProfileConfig.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tblProfileSettings.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.cmsNotificationOption.SuspendLayout();
            this.SuspendLayout();
            // 
            // tblMain
            // 
            this.tblMain.ColumnCount = 1;
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblMain.Controls.Add(this.splitConMain, 0, 0);
            this.tblMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblMain.Location = new System.Drawing.Point(0, 525);
            this.tblMain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tblMain.Name = "tblMain";
            this.tblMain.RowCount = 1;
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 600F));
            this.tblMain.Size = new System.Drawing.Size(1975, 600);
            this.tblMain.TabIndex = 0;
            // 
            // splitConMain
            // 
            this.splitConMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(94)))), ((int)(((byte)(168)))));
            this.splitConMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitConMain.Location = new System.Drawing.Point(3, 2);
            this.splitConMain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitConMain.Name = "splitConMain";
            // 
            // splitConMain.Panel1
            // 
            this.splitConMain.Panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.splitConMain.Panel1.Controls.Add(this.tableLayoutPanel2);
            this.splitConMain.Panel1MinSize = 50;
            // 
            // splitConMain.Panel2
            // 
            this.splitConMain.Panel2.Controls.Add(this.tableLayoutPanel3);
            this.splitConMain.Panel2MinSize = 80;
            this.splitConMain.Size = new System.Drawing.Size(1969, 596);
            this.splitConMain.SplitterDistance = 846;
            this.splitConMain.SplitterWidth = 5;
            this.splitConMain.TabIndex = 5;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(94)))), ((int)(((byte)(168)))));
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.rtbPushLogs, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(846, 596);
            this.tableLayoutPanel2.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(94)))), ((int)(((byte)(168)))));
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(840, 30);
            this.label1.TabIndex = 5;
            this.label1.Text = "Push Notifications and Logs";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rtbPushLogs
            // 
            this.rtbPushLogs.BackColor = System.Drawing.SystemColors.Window;
            this.rtbPushLogs.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbPushLogs.DetectUrls = false;
            this.rtbPushLogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbPushLogs.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbPushLogs.Location = new System.Drawing.Point(0, 30);
            this.rtbPushLogs.Margin = new System.Windows.Forms.Padding(0);
            this.rtbPushLogs.Name = "rtbPushLogs";
            this.rtbPushLogs.ReadOnly = true;
            this.rtbPushLogs.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.rtbPushLogs.Size = new System.Drawing.Size(846, 566);
            this.rtbPushLogs.TabIndex = 4;
            this.rtbPushLogs.Text = "";
            this.rtbPushLogs.WordWrap = false;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.PanelProfileandRawData, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1118, 596);
            this.tableLayoutPanel3.TabIndex = 6;
            // 
            // PanelProfileandRawData
            // 
            this.PanelProfileandRawData.Controls.Add(this.tabControlProfiles);
            this.PanelProfileandRawData.Controls.Add(this.dgRawData);
            this.PanelProfileandRawData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelProfileandRawData.Location = new System.Drawing.Point(0, 30);
            this.PanelProfileandRawData.Margin = new System.Windows.Forms.Padding(0);
            this.PanelProfileandRawData.Name = "PanelProfileandRawData";
            this.PanelProfileandRawData.Size = new System.Drawing.Size(1118, 566);
            this.PanelProfileandRawData.TabIndex = 0;
            // 
            // tabControlProfiles
            // 
            this.tabControlProfiles.Controls.Add(this.tbpInstant);
            this.tabControlProfiles.Controls.Add(this.tbpLS);
            this.tabControlProfiles.Controls.Add(this.tbpDE);
            this.tabControlProfiles.Controls.Add(this.tbpSR);
            this.tabControlProfiles.Controls.Add(this.tbpBill);
            this.tabControlProfiles.Controls.Add(this.tbpCB);
            this.tabControlProfiles.Controls.Add(this.tbpAlert);
            this.tabControlProfiles.Controls.Add(this.tbpTamper);
            this.tabControlProfiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlProfiles.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabControlProfiles.ItemSize = new System.Drawing.Size(100, 35);
            this.tabControlProfiles.Location = new System.Drawing.Point(0, 0);
            this.tabControlProfiles.Margin = new System.Windows.Forms.Padding(0);
            this.tabControlProfiles.Name = "tabControlProfiles";
            this.tabControlProfiles.SelectedIndex = 0;
            this.tabControlProfiles.Size = new System.Drawing.Size(1118, 566);
            this.tabControlProfiles.TabIndex = 0;
            this.tabControlProfiles.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabControlProfiles_DrawItem);
            // 
            // tbpInstant
            // 
            this.tbpInstant.BackColor = System.Drawing.Color.Transparent;
            this.tbpInstant.Controls.Add(this.splitcon_InstantTab);
            this.tbpInstant.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbpInstant.Location = new System.Drawing.Point(4, 39);
            this.tbpInstant.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbpInstant.Name = "tbpInstant";
            this.tbpInstant.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tbpInstant.Size = new System.Drawing.Size(1110, 523);
            this.tbpInstant.TabIndex = 0;
            this.tbpInstant.Text = "Instant";
            this.tbpInstant.UseVisualStyleBackColor = true;
            // 
            // splitcon_InstantTab
            // 
            this.splitcon_InstantTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitcon_InstantTab.Location = new System.Drawing.Point(0, 0);
            this.splitcon_InstantTab.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitcon_InstantTab.Name = "splitcon_InstantTab";
            this.splitcon_InstantTab.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitcon_InstantTab.Panel1
            // 
            this.splitcon_InstantTab.Panel1.Controls.Add(this.dgInstant);
            // 
            // splitcon_InstantTab.Panel2
            // 
            this.splitcon_InstantTab.Panel2.Controls.Add(this.chart_Instant);
            this.splitcon_InstantTab.Size = new System.Drawing.Size(1110, 523);
            this.splitcon_InstantTab.SplitterDistance = 231;
            this.splitcon_InstantTab.TabIndex = 1;
            // 
            // dgInstant
            // 
            this.dgInstant.AllowUserToAddRows = false;
            this.dgInstant.AllowUserToDeleteRows = false;
            this.dgInstant.AllowUserToResizeRows = false;
            this.dgInstant.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgInstant.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dgInstant.BackgroundColor = System.Drawing.Color.White;
            this.dgInstant.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightSteelBlue;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgInstant.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgInstant.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgInstant.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgInstant.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgInstant.EnableHeadersVisualStyles = false;
            this.dgInstant.GridColor = System.Drawing.SystemColors.AppWorkspace;
            this.dgInstant.Location = new System.Drawing.Point(0, 0);
            this.dgInstant.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgInstant.Name = "dgInstant";
            this.dgInstant.ReadOnly = true;
            this.dgInstant.RowHeadersVisible = false;
            this.dgInstant.RowHeadersWidth = 50;
            this.dgInstant.RowTemplate.Height = 24;
            this.dgInstant.Size = new System.Drawing.Size(1110, 231);
            this.dgInstant.TabIndex = 0;
            // 
            // chart_Instant
            // 
            chartArea1.Name = "ChartArea1";
            this.chart_Instant.ChartAreas.Add(chartArea1);
            this.chart_Instant.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chart_Instant.Legends.Add(legend1);
            this.chart_Instant.Location = new System.Drawing.Point(0, 0);
            this.chart_Instant.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chart_Instant.Name = "chart_Instant";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart_Instant.Series.Add(series1);
            this.chart_Instant.Size = new System.Drawing.Size(1110, 288);
            this.chart_Instant.TabIndex = 0;
            this.chart_Instant.Text = "chart1";
            // 
            // tbpLS
            // 
            this.tbpLS.Controls.Add(this.splitCon_LS);
            this.tbpLS.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbpLS.Location = new System.Drawing.Point(4, 39);
            this.tbpLS.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbpLS.Name = "tbpLS";
            this.tbpLS.Size = new System.Drawing.Size(1110, 523);
            this.tbpLS.TabIndex = 1;
            this.tbpLS.Text = "Load Survey";
            this.tbpLS.UseVisualStyleBackColor = true;
            // 
            // splitCon_LS
            // 
            this.splitCon_LS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitCon_LS.Location = new System.Drawing.Point(0, 0);
            this.splitCon_LS.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitCon_LS.Name = "splitCon_LS";
            this.splitCon_LS.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitCon_LS.Panel1
            // 
            this.splitCon_LS.Panel1.Controls.Add(this.dgLS);
            // 
            // splitCon_LS.Panel2
            // 
            this.splitCon_LS.Panel2.Controls.Add(this.chart_LS);
            this.splitCon_LS.Size = new System.Drawing.Size(1110, 523);
            this.splitCon_LS.SplitterDistance = 230;
            this.splitCon_LS.TabIndex = 6;
            // 
            // dgLS
            // 
            this.dgLS.AllowUserToDeleteRows = false;
            this.dgLS.AllowUserToResizeRows = false;
            this.dgLS.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgLS.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dgLS.BackgroundColor = System.Drawing.Color.White;
            this.dgLS.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(134)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgLS.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgLS.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgLS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgLS.Location = new System.Drawing.Point(0, 0);
            this.dgLS.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgLS.Name = "dgLS";
            this.dgLS.RowHeadersVisible = false;
            this.dgLS.RowHeadersWidth = 51;
            this.dgLS.RowTemplate.Height = 24;
            this.dgLS.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgLS.Size = new System.Drawing.Size(1110, 230);
            this.dgLS.TabIndex = 2;
            // 
            // chart_LS
            // 
            chartArea2.Name = "ChartArea1";
            this.chart_LS.ChartAreas.Add(chartArea2);
            this.chart_LS.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.Name = "Legend1";
            this.chart_LS.Legends.Add(legend2);
            this.chart_LS.Location = new System.Drawing.Point(0, 0);
            this.chart_LS.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chart_LS.Name = "chart_LS";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chart_LS.Series.Add(series2);
            this.chart_LS.Size = new System.Drawing.Size(1110, 289);
            this.chart_LS.TabIndex = 3;
            this.chart_LS.Text = "chart1";
            // 
            // tbpDE
            // 
            this.tbpDE.Controls.Add(this.splitCon_DE);
            this.tbpDE.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbpDE.Location = new System.Drawing.Point(4, 39);
            this.tbpDE.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbpDE.Name = "tbpDE";
            this.tbpDE.Size = new System.Drawing.Size(1110, 523);
            this.tbpDE.TabIndex = 2;
            this.tbpDE.Text = "Daily Energy";
            this.tbpDE.UseVisualStyleBackColor = true;
            // 
            // splitCon_DE
            // 
            this.splitCon_DE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitCon_DE.Location = new System.Drawing.Point(0, 0);
            this.splitCon_DE.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitCon_DE.Name = "splitCon_DE";
            this.splitCon_DE.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitCon_DE.Panel1
            // 
            this.splitCon_DE.Panel1.Controls.Add(this.dgDE);
            // 
            // splitCon_DE.Panel2
            // 
            this.splitCon_DE.Panel2.Controls.Add(this.chart_DE);
            this.splitCon_DE.Size = new System.Drawing.Size(1110, 523);
            this.splitCon_DE.SplitterDistance = 231;
            this.splitCon_DE.TabIndex = 7;
            // 
            // dgDE
            // 
            this.dgDE.AllowUserToDeleteRows = false;
            this.dgDE.AllowUserToResizeRows = false;
            this.dgDE.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgDE.BackgroundColor = System.Drawing.Color.White;
            this.dgDE.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgDE.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgDE.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgDE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgDE.Location = new System.Drawing.Point(0, 0);
            this.dgDE.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgDE.Name = "dgDE";
            this.dgDE.RowHeadersVisible = false;
            this.dgDE.RowHeadersWidth = 51;
            this.dgDE.RowTemplate.Height = 24;
            this.dgDE.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgDE.Size = new System.Drawing.Size(1110, 231);
            this.dgDE.TabIndex = 2;
            // 
            // chart_DE
            // 
            chartArea3.Name = "ChartArea1";
            this.chart_DE.ChartAreas.Add(chartArea3);
            this.chart_DE.Dock = System.Windows.Forms.DockStyle.Fill;
            legend3.Name = "Legend1";
            this.chart_DE.Legends.Add(legend3);
            this.chart_DE.Location = new System.Drawing.Point(0, 0);
            this.chart_DE.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chart_DE.Name = "chart_DE";
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.chart_DE.Series.Add(series3);
            this.chart_DE.Size = new System.Drawing.Size(1110, 288);
            this.chart_DE.TabIndex = 3;
            this.chart_DE.Text = "chart1";
            // 
            // tbpSR
            // 
            this.tbpSR.Controls.Add(this.splitCon_SR);
            this.tbpSR.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbpSR.Location = new System.Drawing.Point(4, 39);
            this.tbpSR.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbpSR.Name = "tbpSR";
            this.tbpSR.Size = new System.Drawing.Size(1110, 523);
            this.tbpSR.TabIndex = 3;
            this.tbpSR.Text = "Self Registration";
            this.tbpSR.UseVisualStyleBackColor = true;
            // 
            // splitCon_SR
            // 
            this.splitCon_SR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitCon_SR.Location = new System.Drawing.Point(0, 0);
            this.splitCon_SR.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitCon_SR.Name = "splitCon_SR";
            this.splitCon_SR.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitCon_SR.Panel1
            // 
            this.splitCon_SR.Panel1.Controls.Add(this.dgSR);
            // 
            // splitCon_SR.Panel2
            // 
            this.splitCon_SR.Panel2.Controls.Add(this.chart_SR);
            this.splitCon_SR.Size = new System.Drawing.Size(1110, 523);
            this.splitCon_SR.SplitterDistance = 231;
            this.splitCon_SR.TabIndex = 7;
            // 
            // dgSR
            // 
            this.dgSR.AllowUserToDeleteRows = false;
            this.dgSR.AllowUserToResizeRows = false;
            this.dgSR.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgSR.BackgroundColor = System.Drawing.Color.White;
            this.dgSR.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgSR.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgSR.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgSR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgSR.Location = new System.Drawing.Point(0, 0);
            this.dgSR.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgSR.Name = "dgSR";
            this.dgSR.RowHeadersVisible = false;
            this.dgSR.RowHeadersWidth = 51;
            this.dgSR.RowTemplate.Height = 24;
            this.dgSR.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgSR.Size = new System.Drawing.Size(1110, 231);
            this.dgSR.TabIndex = 2;
            // 
            // chart_SR
            // 
            chartArea4.Name = "ChartArea1";
            this.chart_SR.ChartAreas.Add(chartArea4);
            this.chart_SR.Dock = System.Windows.Forms.DockStyle.Fill;
            legend4.Name = "Legend1";
            this.chart_SR.Legends.Add(legend4);
            this.chart_SR.Location = new System.Drawing.Point(0, 0);
            this.chart_SR.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chart_SR.Name = "chart_SR";
            series4.ChartArea = "ChartArea1";
            series4.Legend = "Legend1";
            series4.Name = "Series1";
            this.chart_SR.Series.Add(series4);
            this.chart_SR.Size = new System.Drawing.Size(1110, 288);
            this.chart_SR.TabIndex = 3;
            this.chart_SR.Text = "chart1";
            // 
            // tbpBill
            // 
            this.tbpBill.Controls.Add(this.splitCon_Bill);
            this.tbpBill.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbpBill.Location = new System.Drawing.Point(4, 39);
            this.tbpBill.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbpBill.Name = "tbpBill";
            this.tbpBill.Size = new System.Drawing.Size(1110, 523);
            this.tbpBill.TabIndex = 4;
            this.tbpBill.Text = "Billing";
            this.tbpBill.UseVisualStyleBackColor = true;
            // 
            // splitCon_Bill
            // 
            this.splitCon_Bill.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitCon_Bill.Location = new System.Drawing.Point(0, 0);
            this.splitCon_Bill.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitCon_Bill.Name = "splitCon_Bill";
            this.splitCon_Bill.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitCon_Bill.Panel1
            // 
            this.splitCon_Bill.Panel1.Controls.Add(this.dgBill);
            // 
            // splitCon_Bill.Panel2
            // 
            this.splitCon_Bill.Panel2.Controls.Add(this.chart_Bill);
            this.splitCon_Bill.Size = new System.Drawing.Size(1110, 523);
            this.splitCon_Bill.SplitterDistance = 231;
            this.splitCon_Bill.TabIndex = 7;
            // 
            // dgBill
            // 
            this.dgBill.AllowUserToDeleteRows = false;
            this.dgBill.AllowUserToResizeRows = false;
            this.dgBill.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgBill.BackgroundColor = System.Drawing.Color.White;
            this.dgBill.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgBill.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgBill.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgBill.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgBill.Location = new System.Drawing.Point(0, 0);
            this.dgBill.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgBill.Name = "dgBill";
            this.dgBill.RowHeadersVisible = false;
            this.dgBill.RowHeadersWidth = 51;
            this.dgBill.RowTemplate.Height = 24;
            this.dgBill.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgBill.Size = new System.Drawing.Size(1110, 231);
            this.dgBill.TabIndex = 2;
            // 
            // chart_Bill
            // 
            chartArea5.Name = "ChartArea1";
            this.chart_Bill.ChartAreas.Add(chartArea5);
            this.chart_Bill.Dock = System.Windows.Forms.DockStyle.Fill;
            legend5.Name = "Legend1";
            this.chart_Bill.Legends.Add(legend5);
            this.chart_Bill.Location = new System.Drawing.Point(0, 0);
            this.chart_Bill.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chart_Bill.Name = "chart_Bill";
            series5.ChartArea = "ChartArea1";
            series5.Legend = "Legend1";
            series5.Name = "Series1";
            this.chart_Bill.Series.Add(series5);
            this.chart_Bill.Size = new System.Drawing.Size(1110, 288);
            this.chart_Bill.TabIndex = 3;
            this.chart_Bill.Text = "chart1";
            // 
            // tbpCB
            // 
            this.tbpCB.Controls.Add(this.splitCon_CB);
            this.tbpCB.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbpCB.Location = new System.Drawing.Point(4, 39);
            this.tbpCB.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbpCB.Name = "tbpCB";
            this.tbpCB.Size = new System.Drawing.Size(1110, 523);
            this.tbpCB.TabIndex = 5;
            this.tbpCB.Text = "Current Bill";
            this.tbpCB.UseVisualStyleBackColor = true;
            // 
            // splitCon_CB
            // 
            this.splitCon_CB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitCon_CB.Location = new System.Drawing.Point(0, 0);
            this.splitCon_CB.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitCon_CB.Name = "splitCon_CB";
            this.splitCon_CB.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitCon_CB.Panel1
            // 
            this.splitCon_CB.Panel1.Controls.Add(this.dgCB);
            // 
            // splitCon_CB.Panel2
            // 
            this.splitCon_CB.Panel2.Controls.Add(this.chart_CB);
            this.splitCon_CB.Size = new System.Drawing.Size(1110, 523);
            this.splitCon_CB.SplitterDistance = 231;
            this.splitCon_CB.TabIndex = 7;
            // 
            // dgCB
            // 
            this.dgCB.AllowUserToDeleteRows = false;
            this.dgCB.AllowUserToResizeRows = false;
            this.dgCB.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgCB.BackgroundColor = System.Drawing.Color.White;
            this.dgCB.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgCB.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgCB.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgCB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgCB.Location = new System.Drawing.Point(0, 0);
            this.dgCB.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgCB.Name = "dgCB";
            this.dgCB.RowHeadersVisible = false;
            this.dgCB.RowHeadersWidth = 51;
            this.dgCB.RowTemplate.Height = 24;
            this.dgCB.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgCB.Size = new System.Drawing.Size(1110, 231);
            this.dgCB.TabIndex = 2;
            // 
            // chart_CB
            // 
            chartArea6.Name = "ChartArea1";
            this.chart_CB.ChartAreas.Add(chartArea6);
            this.chart_CB.Dock = System.Windows.Forms.DockStyle.Fill;
            legend6.Name = "Legend1";
            this.chart_CB.Legends.Add(legend6);
            this.chart_CB.Location = new System.Drawing.Point(0, 0);
            this.chart_CB.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chart_CB.Name = "chart_CB";
            series6.ChartArea = "ChartArea1";
            series6.Legend = "Legend1";
            series6.Name = "Series1";
            this.chart_CB.Series.Add(series6);
            this.chart_CB.Size = new System.Drawing.Size(1110, 288);
            this.chart_CB.TabIndex = 3;
            this.chart_CB.Text = "chart1";
            // 
            // tbpAlert
            // 
            this.tbpAlert.Controls.Add(this.splitCon_Alert);
            this.tbpAlert.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbpAlert.Location = new System.Drawing.Point(4, 39);
            this.tbpAlert.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbpAlert.Name = "tbpAlert";
            this.tbpAlert.Size = new System.Drawing.Size(1110, 523);
            this.tbpAlert.TabIndex = 6;
            this.tbpAlert.Text = "Alert";
            this.tbpAlert.UseVisualStyleBackColor = true;
            // 
            // splitCon_Alert
            // 
            this.splitCon_Alert.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitCon_Alert.Location = new System.Drawing.Point(0, 0);
            this.splitCon_Alert.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitCon_Alert.Name = "splitCon_Alert";
            this.splitCon_Alert.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitCon_Alert.Panel1
            // 
            this.splitCon_Alert.Panel1.Controls.Add(this.dgAlert);
            // 
            // splitCon_Alert.Panel2
            // 
            this.splitCon_Alert.Panel2.Controls.Add(this.chart_Alert);
            this.splitCon_Alert.Size = new System.Drawing.Size(1110, 523);
            this.splitCon_Alert.SplitterDistance = 231;
            this.splitCon_Alert.TabIndex = 7;
            // 
            // dgAlert
            // 
            this.dgAlert.AllowUserToDeleteRows = false;
            this.dgAlert.AllowUserToResizeRows = false;
            this.dgAlert.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgAlert.BackgroundColor = System.Drawing.Color.White;
            this.dgAlert.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgAlert.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.dgAlert.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgAlert.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgAlert.Location = new System.Drawing.Point(0, 0);
            this.dgAlert.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgAlert.Name = "dgAlert";
            this.dgAlert.RowHeadersVisible = false;
            this.dgAlert.RowHeadersWidth = 51;
            this.dgAlert.RowTemplate.Height = 24;
            this.dgAlert.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgAlert.Size = new System.Drawing.Size(1110, 231);
            this.dgAlert.TabIndex = 2;
            // 
            // chart_Alert
            // 
            chartArea7.Name = "ChartArea1";
            this.chart_Alert.ChartAreas.Add(chartArea7);
            this.chart_Alert.Dock = System.Windows.Forms.DockStyle.Fill;
            legend7.Name = "Legend1";
            this.chart_Alert.Legends.Add(legend7);
            this.chart_Alert.Location = new System.Drawing.Point(0, 0);
            this.chart_Alert.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chart_Alert.Name = "chart_Alert";
            series7.ChartArea = "ChartArea1";
            series7.Legend = "Legend1";
            series7.Name = "Series1";
            this.chart_Alert.Series.Add(series7);
            this.chart_Alert.Size = new System.Drawing.Size(1110, 288);
            this.chart_Alert.TabIndex = 3;
            this.chart_Alert.Text = "chart1";
            // 
            // tbpTamper
            // 
            this.tbpTamper.Controls.Add(this.splitCon_Tamper);
            this.tbpTamper.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbpTamper.Location = new System.Drawing.Point(4, 39);
            this.tbpTamper.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbpTamper.Name = "tbpTamper";
            this.tbpTamper.Size = new System.Drawing.Size(1110, 523);
            this.tbpTamper.TabIndex = 7;
            this.tbpTamper.Text = "Tamper";
            this.tbpTamper.UseVisualStyleBackColor = true;
            // 
            // splitCon_Tamper
            // 
            this.splitCon_Tamper.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitCon_Tamper.Location = new System.Drawing.Point(0, 0);
            this.splitCon_Tamper.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitCon_Tamper.Name = "splitCon_Tamper";
            this.splitCon_Tamper.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitCon_Tamper.Panel1
            // 
            this.splitCon_Tamper.Panel1.Controls.Add(this.dgTamper);
            // 
            // splitCon_Tamper.Panel2
            // 
            this.splitCon_Tamper.Panel2.Controls.Add(this.chart_tamper);
            this.splitCon_Tamper.Size = new System.Drawing.Size(1110, 523);
            this.splitCon_Tamper.SplitterDistance = 231;
            this.splitCon_Tamper.TabIndex = 7;
            // 
            // dgTamper
            // 
            this.dgTamper.AllowUserToDeleteRows = false;
            this.dgTamper.AllowUserToResizeRows = false;
            this.dgTamper.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgTamper.BackgroundColor = System.Drawing.Color.White;
            this.dgTamper.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgTamper.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dgTamper.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgTamper.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgTamper.Location = new System.Drawing.Point(0, 0);
            this.dgTamper.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgTamper.Name = "dgTamper";
            this.dgTamper.RowHeadersVisible = false;
            this.dgTamper.RowHeadersWidth = 51;
            this.dgTamper.RowTemplate.Height = 24;
            this.dgTamper.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgTamper.Size = new System.Drawing.Size(1110, 231);
            this.dgTamper.TabIndex = 2;
            // 
            // chart_tamper
            // 
            chartArea8.Name = "ChartArea1";
            this.chart_tamper.ChartAreas.Add(chartArea8);
            this.chart_tamper.Dock = System.Windows.Forms.DockStyle.Fill;
            legend8.Name = "Legend1";
            this.chart_tamper.Legends.Add(legend8);
            this.chart_tamper.Location = new System.Drawing.Point(0, 0);
            this.chart_tamper.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chart_tamper.Name = "chart_tamper";
            series8.ChartArea = "ChartArea1";
            series8.Legend = "Legend1";
            series8.Name = "Series1";
            this.chart_tamper.Series.Add(series8);
            this.chart_tamper.Size = new System.Drawing.Size(1110, 288);
            this.chart_tamper.TabIndex = 3;
            this.chart_tamper.Text = "chart1";
            // 
            // dgRawData
            // 
            this.dgRawData.AllowUserToAddRows = false;
            this.dgRawData.AllowUserToDeleteRows = false;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.LightSkyBlue;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgRawData.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle10;
            this.dgRawData.BackgroundColor = System.Drawing.Color.White;
            this.dgRawData.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(134)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgRawData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle11;
            this.dgRawData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgRawData.DefaultCellStyle = dataGridViewCellStyle12;
            this.dgRawData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgRawData.GridColor = System.Drawing.SystemColors.Window;
            this.dgRawData.Location = new System.Drawing.Point(0, 0);
            this.dgRawData.Margin = new System.Windows.Forms.Padding(0);
            this.dgRawData.Name = "dgRawData";
            this.dgRawData.ReadOnly = true;
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(134)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle13.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgRawData.RowHeadersDefaultCellStyle = dataGridViewCellStyle13;
            this.dgRawData.RowHeadersVisible = false;
            this.dgRawData.RowHeadersWidth = 50;
            this.dgRawData.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dgRawData.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgRawData.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.dgRawData.RowTemplate.Height = 24;
            this.dgRawData.Size = new System.Drawing.Size(1118, 566);
            this.dgRawData.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(94)))), ((int)(((byte)(168)))));
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(1112, 30);
            this.label2.TabIndex = 6;
            this.label2.Text = "Data and Reports";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.BackColor = System.Drawing.SystemColors.Control;
            this.lblHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(94)))), ((int)(((byte)(168)))));
            this.lblHeader.Location = new System.Drawing.Point(3, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(1969, 34);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "Push Settings and Notifications";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tblHeader
            // 
            this.tblHeader.BackColor = System.Drawing.SystemColors.Control;
            this.tblHeader.ColumnCount = 1;
            this.tblHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblHeader.Controls.Add(this.flowPanelButtons, 0, 1);
            this.tblHeader.Controls.Add(this.lblHeader, 0, 0);
            this.tblHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblHeader.Location = new System.Drawing.Point(0, 0);
            this.tblHeader.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tblHeader.Name = "tblHeader";
            this.tblHeader.RowCount = 2;
            this.tblHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tblHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblHeader.Size = new System.Drawing.Size(1975, 75);
            this.tblHeader.TabIndex = 9;
            // 
            // flowPanelButtons
            // 
            this.flowPanelButtons.AutoScroll = true;
            this.flowPanelButtons.BackColor = System.Drawing.SystemColors.Control;
            this.flowPanelButtons.Controls.Add(this.btnPushprofileSettings);
            this.flowPanelButtons.Controls.Add(this.tableLayoutPanel4);
            this.flowPanelButtons.Controls.Add(this.btnNotifyDecryptSettings);
            this.flowPanelButtons.Controls.Add(this.btnNotificationType);
            this.flowPanelButtons.Controls.Add(this.btnStartListener);
            this.flowPanelButtons.Controls.Add(this.btnStopListener);
            this.flowPanelButtons.Controls.Add(this.btnClearLogs);
            this.flowPanelButtons.Controls.Add(this.btnSaveData);
            this.flowPanelButtons.Controls.Add(this.btnRawData);
            this.flowPanelButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowPanelButtons.Location = new System.Drawing.Point(0, 36);
            this.flowPanelButtons.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.flowPanelButtons.MinimumSize = new System.Drawing.Size(0, 34);
            this.flowPanelButtons.Name = "flowPanelButtons";
            this.flowPanelButtons.Size = new System.Drawing.Size(1975, 39);
            this.flowPanelButtons.TabIndex = 5;
            // 
            // btnPushprofileSettings
            // 
            this.btnPushprofileSettings.BackColor = System.Drawing.Color.LightGray;
            this.btnPushprofileSettings.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnPushprofileSettings.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnPushprofileSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPushprofileSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPushprofileSettings.Location = new System.Drawing.Point(3, 2);
            this.btnPushprofileSettings.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnPushprofileSettings.Name = "btnPushprofileSettings";
            this.btnPushprofileSettings.Size = new System.Drawing.Size(200, 30);
            this.btnPushprofileSettings.TabIndex = 62;
            this.btnPushprofileSettings.Text = "▼ Show Push Profile Settings";
            this.btnPushprofileSettings.UseVisualStyleBackColor = false;
            this.btnPushprofileSettings.Click += new System.EventHandler(this.btnPushprofileSettings_Click);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 54.54546F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45.45454F));
            this.tableLayoutPanel4.Controls.Add(this.rbBtnDetailLog, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.rbBtnUserDefinedLog, 1, 0);
            this.tableLayoutPanel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel4.Location = new System.Drawing.Point(209, 2);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(223, 25);
            this.tableLayoutPanel4.TabIndex = 65;
            // 
            // rbBtnDetailLog
            // 
            this.rbBtnDetailLog.AutoSize = true;
            this.rbBtnDetailLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbBtnDetailLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbBtnDetailLog.Location = new System.Drawing.Point(0, 0);
            this.rbBtnDetailLog.Margin = new System.Windows.Forms.Padding(0);
            this.rbBtnDetailLog.Name = "rbBtnDetailLog";
            this.rbBtnDetailLog.Size = new System.Drawing.Size(121, 25);
            this.rbBtnDetailLog.TabIndex = 0;
            this.rbBtnDetailLog.Text = "Detailed Logging";
            this.rbBtnDetailLog.UseVisualStyleBackColor = true;
            this.rbBtnDetailLog.CheckedChanged += new System.EventHandler(this.rbBtnDetailLog_CheckedChanged);
            // 
            // rbBtnUserDefinedLog
            // 
            this.rbBtnUserDefinedLog.AutoSize = true;
            this.rbBtnUserDefinedLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbBtnUserDefinedLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbBtnUserDefinedLog.Location = new System.Drawing.Point(121, 0);
            this.rbBtnUserDefinedLog.Margin = new System.Windows.Forms.Padding(0);
            this.rbBtnUserDefinedLog.Name = "rbBtnUserDefinedLog";
            this.rbBtnUserDefinedLog.Size = new System.Drawing.Size(102, 25);
            this.rbBtnUserDefinedLog.TabIndex = 1;
            this.rbBtnUserDefinedLog.Text = "User Defined";
            this.rbBtnUserDefinedLog.UseVisualStyleBackColor = true;
            this.rbBtnUserDefinedLog.CheckedChanged += new System.EventHandler(this.rbBtnDetailLog_CheckedChanged);
            // 
            // btnNotifyDecryptSettings
            // 
            this.btnNotifyDecryptSettings.BackColor = System.Drawing.Color.LightGray;
            this.btnNotifyDecryptSettings.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnNotifyDecryptSettings.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnNotifyDecryptSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNotifyDecryptSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNotifyDecryptSettings.Location = new System.Drawing.Point(438, 2);
            this.btnNotifyDecryptSettings.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnNotifyDecryptSettings.Name = "btnNotifyDecryptSettings";
            this.btnNotifyDecryptSettings.Size = new System.Drawing.Size(181, 30);
            this.btnNotifyDecryptSettings.TabIndex = 66;
            this.btnNotifyDecryptSettings.Text = "Notification Settings ▼";
            this.btnNotifyDecryptSettings.UseVisualStyleBackColor = false;
            this.btnNotifyDecryptSettings.Click += new System.EventHandler(this.btnNotifyDecryptSettings_Click);
            // 
            // btnNotificationType
            // 
            this.btnNotificationType.BackColor = System.Drawing.Color.LightGray;
            this.btnNotificationType.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnNotificationType.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnNotificationType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNotificationType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNotificationType.Location = new System.Drawing.Point(625, 2);
            this.btnNotificationType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnNotificationType.Name = "btnNotificationType";
            this.btnNotificationType.Size = new System.Drawing.Size(140, 30);
            this.btnNotificationType.TabIndex = 63;
            this.btnNotificationType.Text = "Log Format ▼";
            this.btnNotificationType.UseVisualStyleBackColor = false;
            this.btnNotificationType.Click += new System.EventHandler(this.btnNotificationType_Click);
            // 
            // btnStartListener
            // 
            this.btnStartListener.BackColor = System.Drawing.Color.LightGray;
            this.btnStartListener.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnStartListener.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnStartListener.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartListener.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartListener.Location = new System.Drawing.Point(771, 2);
            this.btnStartListener.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnStartListener.Name = "btnStartListener";
            this.btnStartListener.Size = new System.Drawing.Size(72, 30);
            this.btnStartListener.TabIndex = 0;
            this.btnStartListener.Text = "▶ Start";
            this.btnStartListener.UseVisualStyleBackColor = false;
            this.btnStartListener.Click += new System.EventHandler(this.btnStartListener_Click);
            // 
            // btnStopListener
            // 
            this.btnStopListener.BackColor = System.Drawing.Color.LightGray;
            this.btnStopListener.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnStopListener.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnStopListener.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStopListener.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStopListener.Location = new System.Drawing.Point(849, 2);
            this.btnStopListener.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnStopListener.Name = "btnStopListener";
            this.btnStopListener.Size = new System.Drawing.Size(72, 30);
            this.btnStopListener.TabIndex = 1;
            this.btnStopListener.Text = "⏹ Stop";
            this.btnStopListener.UseVisualStyleBackColor = false;
            this.btnStopListener.Click += new System.EventHandler(this.btnStopListener_Click);
            // 
            // btnClearLogs
            // 
            this.btnClearLogs.BackColor = System.Drawing.Color.LightGray;
            this.btnClearLogs.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnClearLogs.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnClearLogs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearLogs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearLogs.Location = new System.Drawing.Point(927, 2);
            this.btnClearLogs.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClearLogs.Name = "btnClearLogs";
            this.btnClearLogs.Size = new System.Drawing.Size(72, 30);
            this.btnClearLogs.TabIndex = 2;
            this.btnClearLogs.Text = "🧹 Clear Logs";
            this.btnClearLogs.UseVisualStyleBackColor = false;
            this.btnClearLogs.Click += new System.EventHandler(this.btnClearLogs_Click);
            // 
            // btnSaveData
            // 
            this.btnSaveData.BackColor = System.Drawing.Color.LightGray;
            this.btnSaveData.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnSaveData.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnSaveData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveData.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveData.Location = new System.Drawing.Point(1005, 2);
            this.btnSaveData.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSaveData.Name = "btnSaveData";
            this.btnSaveData.Size = new System.Drawing.Size(96, 30);
            this.btnSaveData.TabIndex = 3;
            this.btnSaveData.Text = "💾 Save Data";
            this.btnSaveData.UseVisualStyleBackColor = false;
            this.btnSaveData.Click += new System.EventHandler(this.btnSaveData_Click);
            // 
            // btnRawData
            // 
            this.btnRawData.BackColor = System.Drawing.Color.LightGray;
            this.btnRawData.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnRawData.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnRawData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRawData.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRawData.Location = new System.Drawing.Point(1107, 2);
            this.btnRawData.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnRawData.Name = "btnRawData";
            this.btnRawData.Size = new System.Drawing.Size(140, 30);
            this.btnRawData.TabIndex = 5;
            this.btnRawData.Text = "Raw Data View";
            this.btnRawData.UseVisualStyleBackColor = false;
            this.btnRawData.Click += new System.EventHandler(this.btnRawData_Click);
            // 
            // pnlProfileSettings
            // 
            this.pnlProfileSettings.BackColor = System.Drawing.SystemColors.Control;
            this.pnlProfileSettings.Controls.Add(this.grpPushObjects);
            this.pnlProfileSettings.Controls.Add(this.grpProfileConfig);
            this.pnlProfileSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlProfileSettings.Location = new System.Drawing.Point(0, 75);
            this.pnlProfileSettings.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlProfileSettings.Name = "pnlProfileSettings";
            this.pnlProfileSettings.Size = new System.Drawing.Size(1975, 450);
            this.pnlProfileSettings.TabIndex = 0;
            this.pnlProfileSettings.Visible = false;
            // 
            // grpPushObjects
            // 
            this.grpPushObjects.Controls.Add(this.tableLayoutPanel7);
            this.grpPushObjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpPushObjects.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpPushObjects.Location = new System.Drawing.Point(0, 265);
            this.grpPushObjects.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpPushObjects.Name = "grpPushObjects";
            this.grpPushObjects.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpPushObjects.Size = new System.Drawing.Size(1975, 185);
            this.grpPushObjects.TabIndex = 0;
            this.grpPushObjects.TabStop = false;
            this.grpPushObjects.Text = "Push Setup Objects";
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.ColumnCount = 2;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tableLayoutPanel7.Controls.Add(this.tvPushObjects, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.DGPushProfile, 1, 0);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(3, 22);
            this.tableLayoutPanel7.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 1;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(1969, 161);
            this.tableLayoutPanel7.TabIndex = 0;
            // 
            // tvPushObjects
            // 
            this.tvPushObjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvPushObjects.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tvPushObjects.Location = new System.Drawing.Point(3, 2);
            this.tvPushObjects.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tvPushObjects.Name = "tvPushObjects";
            this.tvPushObjects.ShowLines = false;
            this.tvPushObjects.ShowPlusMinus = false;
            this.tvPushObjects.ShowRootLines = false;
            this.tvPushObjects.Size = new System.Drawing.Size(486, 157);
            this.tvPushObjects.TabIndex = 0;
            this.tvPushObjects.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvPushObjects_AfterSelect);
            // 
            // DGPushProfile
            // 
            this.DGPushProfile.AllowUserToAddRows = false;
            this.DGPushProfile.AllowUserToDeleteRows = false;
            this.DGPushProfile.BackgroundColor = System.Drawing.Color.White;
            this.DGPushProfile.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGPushProfile.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle14;
            this.DGPushProfile.ColumnHeadersHeight = 35;
            this.DGPushProfile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DGPushProfile.Location = new System.Drawing.Point(495, 2);
            this.DGPushProfile.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DGPushProfile.Name = "DGPushProfile";
            this.DGPushProfile.ReadOnly = true;
            this.DGPushProfile.RowHeadersVisible = false;
            this.DGPushProfile.RowHeadersWidth = 35;
            this.DGPushProfile.RowTemplate.Height = 24;
            this.DGPushProfile.Size = new System.Drawing.Size(1471, 157);
            this.DGPushProfile.TabIndex = 1;
            // 
            // grpProfileConfig
            // 
            this.grpProfileConfig.BackColor = System.Drawing.SystemColors.Control;
            this.grpProfileConfig.Controls.Add(this.tableLayoutPanel5);
            this.grpProfileConfig.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpProfileConfig.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpProfileConfig.ForeColor = System.Drawing.Color.Black;
            this.grpProfileConfig.Location = new System.Drawing.Point(0, 0);
            this.grpProfileConfig.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpProfileConfig.Name = "grpProfileConfig";
            this.grpProfileConfig.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpProfileConfig.Size = new System.Drawing.Size(1975, 265);
            this.grpProfileConfig.TabIndex = 10;
            this.grpProfileConfig.TabStop = false;
            this.grpProfileConfig.Text = "Push Setup And Action Schedule settings";
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel6, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.tblProfileSettings, 0, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 22);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(1969, 241);
            this.tableLayoutPanel5.TabIndex = 1;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 1;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Controls.Add(this.btnSet_PS_AS, 0, 4);
            this.tableLayoutPanel6.Controls.Add(this.cbTestProfileType, 0, 1);
            this.tableLayoutPanel6.Controls.Add(this.lblProfile, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.btnGet_PS_AS, 0, 3);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(1476, 0);
            this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 5;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(493, 241);
            this.tableLayoutPanel6.TabIndex = 0;
            // 
            // btnSet_PS_AS
            // 
            this.btnSet_PS_AS.BackColor = System.Drawing.Color.LightGray;
            this.btnSet_PS_AS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSet_PS_AS.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnSet_PS_AS.FlatAppearance.BorderSize = 2;
            this.btnSet_PS_AS.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnSet_PS_AS.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSet_PS_AS.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSet_PS_AS.ForeColor = System.Drawing.Color.Black;
            this.btnSet_PS_AS.Location = new System.Drawing.Point(3, 193);
            this.btnSet_PS_AS.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSet_PS_AS.Name = "btnSet_PS_AS";
            this.btnSet_PS_AS.Size = new System.Drawing.Size(487, 46);
            this.btnSet_PS_AS.TabIndex = 61;
            this.btnSet_PS_AS.Text = "Set Push Setup and Action Schedule";
            this.btnSet_PS_AS.UseVisualStyleBackColor = false;
            this.btnSet_PS_AS.Click += new System.EventHandler(this.btnSet_PS_AS_Click);
            // 
            // cbTestProfileType
            // 
            this.cbTestProfileType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbTestProfileType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTestProfileType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbTestProfileType.FormattingEnabled = true;
            this.cbTestProfileType.Items.AddRange(new object[] {
            "All",
            "Instant",
            "LS",
            "DE",
            "SR",
            "Bill",
            "Current Bill"});
            this.cbTestProfileType.Location = new System.Drawing.Point(3, 32);
            this.cbTestProfileType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbTestProfileType.Name = "cbTestProfileType";
            this.cbTestProfileType.Size = new System.Drawing.Size(487, 26);
            this.cbTestProfileType.TabIndex = 67;
            this.cbTestProfileType.SelectedIndexChanged += new System.EventHandler(this.cbTestProfileType_SelectedIndexChanged);
            // 
            // lblProfile
            // 
            this.lblProfile.AutoSize = true;
            this.lblProfile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblProfile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProfile.Location = new System.Drawing.Point(3, 0);
            this.lblProfile.Name = "lblProfile";
            this.lblProfile.Size = new System.Drawing.Size(487, 30);
            this.lblProfile.TabIndex = 66;
            this.lblProfile.Text = "Test Profile";
            this.lblProfile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnGet_PS_AS
            // 
            this.btnGet_PS_AS.BackColor = System.Drawing.Color.LightGray;
            this.btnGet_PS_AS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGet_PS_AS.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnGet_PS_AS.FlatAppearance.BorderSize = 2;
            this.btnGet_PS_AS.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnGet_PS_AS.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGet_PS_AS.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGet_PS_AS.ForeColor = System.Drawing.Color.Black;
            this.btnGet_PS_AS.Location = new System.Drawing.Point(3, 143);
            this.btnGet_PS_AS.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnGet_PS_AS.Name = "btnGet_PS_AS";
            this.btnGet_PS_AS.Size = new System.Drawing.Size(487, 46);
            this.btnGet_PS_AS.TabIndex = 62;
            this.btnGet_PS_AS.Text = "Get Push Setup and Action Schedule";
            this.btnGet_PS_AS.UseVisualStyleBackColor = false;
            this.btnGet_PS_AS.Click += new System.EventHandler(this.btnGet_PS_AS_Click);
            // 
            // tblProfileSettings
            // 
            this.tblProfileSettings.ColumnCount = 4;
            this.tblProfileSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tblProfileSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tblProfileSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tblProfileSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tblProfileSettings.Controls.Add(this.txt_Random_CB, 3, 7);
            this.tblProfileSettings.Controls.Add(this.txt_Random_LS, 3, 6);
            this.tblProfileSettings.Controls.Add(this.txt_Random_DE, 3, 5);
            this.tblProfileSettings.Controls.Add(this.txt_Random_SR, 3, 4);
            this.tblProfileSettings.Controls.Add(this.txt_Random_Bill, 3, 3);
            this.tblProfileSettings.Controls.Add(this.txt_Random_Instant, 3, 1);
            this.tblProfileSettings.Controls.Add(this.lblRandomHeader, 3, 0);
            this.tblProfileSettings.Controls.Add(this.lblCBProfile, 0, 7);
            this.tblProfileSettings.Controls.Add(this.txt_CB_DestIP, 1, 7);
            this.tblProfileSettings.Controls.Add(this.lblInstant, 0, 1);
            this.tblProfileSettings.Controls.Add(this.txt_Instant_DestIP, 1, 1);
            this.tblProfileSettings.Controls.Add(this.cbInstant_Frequency, 2, 1);
            this.tblProfileSettings.Controls.Add(this.lblAlert, 0, 2);
            this.tblProfileSettings.Controls.Add(this.txt_Alert_DestIP, 1, 2);
            this.tblProfileSettings.Controls.Add(this.lblBillingProfile, 0, 3);
            this.tblProfileSettings.Controls.Add(this.txt_Bill_DestIP, 1, 3);
            this.tblProfileSettings.Controls.Add(this.lblSRProfile, 0, 4);
            this.tblProfileSettings.Controls.Add(this.txt_SR_DestIP, 1, 4);
            this.tblProfileSettings.Controls.Add(this.cb_SR_Frequency, 2, 4);
            this.tblProfileSettings.Controls.Add(this.lblDEProfile, 0, 5);
            this.tblProfileSettings.Controls.Add(this.txt_DE_DestIP, 1, 5);
            this.tblProfileSettings.Controls.Add(this.cb_DE_Frequency, 2, 5);
            this.tblProfileSettings.Controls.Add(this.lblLSProfile, 0, 6);
            this.tblProfileSettings.Controls.Add(this.txt_LS_DestIP, 1, 6);
            this.tblProfileSettings.Controls.Add(this.cb_LS_Frequency, 2, 6);
            this.tblProfileSettings.Controls.Add(this.lblProfileHeader, 0, 0);
            this.tblProfileSettings.Controls.Add(this.lblDestIPHeader, 1, 0);
            this.tblProfileSettings.Controls.Add(this.lblPushFreqHeader, 2, 0);
            this.tblProfileSettings.Controls.Add(this.cb_CB_Frequency, 2, 7);
            this.tblProfileSettings.Controls.Add(this.tableLayoutPanel1, 2, 3);
            this.tblProfileSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblProfileSettings.Location = new System.Drawing.Point(3, 2);
            this.tblProfileSettings.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tblProfileSettings.Name = "tblProfileSettings";
            this.tblProfileSettings.RowCount = 8;
            this.tblProfileSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tblProfileSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tblProfileSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tblProfileSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tblProfileSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tblProfileSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tblProfileSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tblProfileSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tblProfileSettings.Size = new System.Drawing.Size(1470, 237);
            this.tblProfileSettings.TabIndex = 0;
            // 
            // txt_Random_CB
            // 
            this.txt_Random_CB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_Random_CB.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_Random_CB.Location = new System.Drawing.Point(1104, 212);
            this.txt_Random_CB.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt_Random_CB.Name = "txt_Random_CB";
            this.txt_Random_CB.Size = new System.Drawing.Size(363, 24);
            this.txt_Random_CB.TabIndex = 60;
            // 
            // txt_Random_LS
            // 
            this.txt_Random_LS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_Random_LS.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_Random_LS.Location = new System.Drawing.Point(1104, 182);
            this.txt_Random_LS.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt_Random_LS.Name = "txt_Random_LS";
            this.txt_Random_LS.Size = new System.Drawing.Size(363, 24);
            this.txt_Random_LS.TabIndex = 57;
            // 
            // txt_Random_DE
            // 
            this.txt_Random_DE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_Random_DE.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_Random_DE.Location = new System.Drawing.Point(1104, 152);
            this.txt_Random_DE.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt_Random_DE.Name = "txt_Random_DE";
            this.txt_Random_DE.Size = new System.Drawing.Size(363, 24);
            this.txt_Random_DE.TabIndex = 54;
            // 
            // txt_Random_SR
            // 
            this.txt_Random_SR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_Random_SR.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_Random_SR.Location = new System.Drawing.Point(1104, 122);
            this.txt_Random_SR.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt_Random_SR.Name = "txt_Random_SR";
            this.txt_Random_SR.Size = new System.Drawing.Size(363, 24);
            this.txt_Random_SR.TabIndex = 51;
            // 
            // txt_Random_Bill
            // 
            this.txt_Random_Bill.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_Random_Bill.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_Random_Bill.Location = new System.Drawing.Point(1104, 92);
            this.txt_Random_Bill.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt_Random_Bill.Name = "txt_Random_Bill";
            this.txt_Random_Bill.Size = new System.Drawing.Size(363, 24);
            this.txt_Random_Bill.TabIndex = 48;
            // 
            // txt_Random_Instant
            // 
            this.txt_Random_Instant.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_Random_Instant.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_Random_Instant.Location = new System.Drawing.Point(1104, 32);
            this.txt_Random_Instant.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt_Random_Instant.Name = "txt_Random_Instant";
            this.txt_Random_Instant.Size = new System.Drawing.Size(363, 24);
            this.txt_Random_Instant.TabIndex = 42;
            // 
            // lblRandomHeader
            // 
            this.lblRandomHeader.AutoSize = true;
            this.lblRandomHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRandomHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRandomHeader.Location = new System.Drawing.Point(1104, 0);
            this.lblRandomHeader.Name = "lblRandomHeader";
            this.lblRandomHeader.Size = new System.Drawing.Size(363, 30);
            this.lblRandomHeader.TabIndex = 48;
            this.lblRandomHeader.Text = "Randomisation (In Min)";
            this.lblRandomHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCBProfile
            // 
            this.lblCBProfile.AutoSize = true;
            this.lblCBProfile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCBProfile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCBProfile.Location = new System.Drawing.Point(3, 210);
            this.lblCBProfile.Name = "lblCBProfile";
            this.lblCBProfile.Size = new System.Drawing.Size(361, 30);
            this.lblCBProfile.TabIndex = 45;
            this.lblCBProfile.Text = "Current Bill Profile";
            this.lblCBProfile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txt_CB_DestIP
            // 
            this.txt_CB_DestIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_CB_DestIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_CB_DestIP.Location = new System.Drawing.Point(370, 212);
            this.txt_CB_DestIP.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt_CB_DestIP.Name = "txt_CB_DestIP";
            this.txt_CB_DestIP.Size = new System.Drawing.Size(361, 24);
            this.txt_CB_DestIP.TabIndex = 58;
            // 
            // lblInstant
            // 
            this.lblInstant.AutoSize = true;
            this.lblInstant.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblInstant.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInstant.Location = new System.Drawing.Point(3, 30);
            this.lblInstant.Name = "lblInstant";
            this.lblInstant.Size = new System.Drawing.Size(361, 30);
            this.lblInstant.TabIndex = 39;
            this.lblInstant.Text = "Instant Profile";
            this.lblInstant.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txt_Instant_DestIP
            // 
            this.txt_Instant_DestIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_Instant_DestIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_Instant_DestIP.Location = new System.Drawing.Point(370, 32);
            this.txt_Instant_DestIP.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt_Instant_DestIP.Name = "txt_Instant_DestIP";
            this.txt_Instant_DestIP.Size = new System.Drawing.Size(361, 24);
            this.txt_Instant_DestIP.TabIndex = 40;
            // 
            // cbInstant_Frequency
            // 
            this.cbInstant_Frequency.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbInstant_Frequency.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbInstant_Frequency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbInstant_Frequency.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbInstant_Frequency.FormattingEnabled = true;
            this.cbInstant_Frequency.Items.AddRange(new object[] {
            "15 Min",
            "30 Min",
            "1 Hour",
            "4 Hour",
            "6 Hour",
            "8 Hour",
            "12 Hour",
            "24 Hour",
            "Disabled"});
            this.cbInstant_Frequency.Location = new System.Drawing.Point(737, 32);
            this.cbInstant_Frequency.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbInstant_Frequency.Name = "cbInstant_Frequency";
            this.cbInstant_Frequency.Size = new System.Drawing.Size(361, 25);
            this.cbInstant_Frequency.TabIndex = 41;
            this.cbInstant_Frequency.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cbInstant_Frequency_DrawItem);
            // 
            // lblAlert
            // 
            this.lblAlert.AutoSize = true;
            this.lblAlert.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAlert.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAlert.Location = new System.Drawing.Point(3, 60);
            this.lblAlert.Name = "lblAlert";
            this.lblAlert.Size = new System.Drawing.Size(361, 30);
            this.lblAlert.TabIndex = 36;
            this.lblAlert.Text = "Alert Profile";
            this.lblAlert.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txt_Alert_DestIP
            // 
            this.txt_Alert_DestIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_Alert_DestIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_Alert_DestIP.Location = new System.Drawing.Point(370, 62);
            this.txt_Alert_DestIP.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt_Alert_DestIP.Name = "txt_Alert_DestIP";
            this.txt_Alert_DestIP.Size = new System.Drawing.Size(361, 24);
            this.txt_Alert_DestIP.TabIndex = 43;
            // 
            // lblBillingProfile
            // 
            this.lblBillingProfile.AutoSize = true;
            this.lblBillingProfile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBillingProfile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBillingProfile.Location = new System.Drawing.Point(3, 90);
            this.lblBillingProfile.Name = "lblBillingProfile";
            this.lblBillingProfile.Size = new System.Drawing.Size(361, 30);
            this.lblBillingProfile.TabIndex = 33;
            this.lblBillingProfile.Text = "Billing Profile";
            this.lblBillingProfile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txt_Bill_DestIP
            // 
            this.txt_Bill_DestIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_Bill_DestIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_Bill_DestIP.Location = new System.Drawing.Point(370, 92);
            this.txt_Bill_DestIP.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt_Bill_DestIP.Name = "txt_Bill_DestIP";
            this.txt_Bill_DestIP.Size = new System.Drawing.Size(361, 24);
            this.txt_Bill_DestIP.TabIndex = 46;
            // 
            // lblSRProfile
            // 
            this.lblSRProfile.AutoSize = true;
            this.lblSRProfile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSRProfile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSRProfile.Location = new System.Drawing.Point(3, 120);
            this.lblSRProfile.Name = "lblSRProfile";
            this.lblSRProfile.Size = new System.Drawing.Size(361, 30);
            this.lblSRProfile.TabIndex = 30;
            this.lblSRProfile.Text = "Self Registration Profile";
            this.lblSRProfile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txt_SR_DestIP
            // 
            this.txt_SR_DestIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_SR_DestIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_SR_DestIP.Location = new System.Drawing.Point(370, 122);
            this.txt_SR_DestIP.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt_SR_DestIP.Name = "txt_SR_DestIP";
            this.txt_SR_DestIP.Size = new System.Drawing.Size(361, 24);
            this.txt_SR_DestIP.TabIndex = 49;
            // 
            // cb_SR_Frequency
            // 
            this.cb_SR_Frequency.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb_SR_Frequency.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cb_SR_Frequency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_SR_Frequency.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_SR_Frequency.FormattingEnabled = true;
            this.cb_SR_Frequency.Items.AddRange(new object[] {
            "15 Min",
            "30 Min",
            "1 Hour",
            "4 Hour",
            "6 Hour",
            "8 Hour",
            "12 Hour",
            "24 Hour",
            "Disabled"});
            this.cb_SR_Frequency.Location = new System.Drawing.Point(737, 122);
            this.cb_SR_Frequency.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cb_SR_Frequency.Name = "cb_SR_Frequency";
            this.cb_SR_Frequency.Size = new System.Drawing.Size(361, 25);
            this.cb_SR_Frequency.TabIndex = 50;
            this.cb_SR_Frequency.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cbInstant_Frequency_DrawItem);
            // 
            // lblDEProfile
            // 
            this.lblDEProfile.AutoSize = true;
            this.lblDEProfile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDEProfile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDEProfile.Location = new System.Drawing.Point(3, 150);
            this.lblDEProfile.Name = "lblDEProfile";
            this.lblDEProfile.Size = new System.Drawing.Size(361, 30);
            this.lblDEProfile.TabIndex = 27;
            this.lblDEProfile.Text = "Daily Energy Profile";
            this.lblDEProfile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txt_DE_DestIP
            // 
            this.txt_DE_DestIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_DE_DestIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_DE_DestIP.Location = new System.Drawing.Point(370, 152);
            this.txt_DE_DestIP.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt_DE_DestIP.Name = "txt_DE_DestIP";
            this.txt_DE_DestIP.Size = new System.Drawing.Size(361, 24);
            this.txt_DE_DestIP.TabIndex = 52;
            // 
            // cb_DE_Frequency
            // 
            this.cb_DE_Frequency.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb_DE_Frequency.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cb_DE_Frequency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_DE_Frequency.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_DE_Frequency.FormattingEnabled = true;
            this.cb_DE_Frequency.Items.AddRange(new object[] {
            "24 Hour",
            "Disabled"});
            this.cb_DE_Frequency.Location = new System.Drawing.Point(737, 152);
            this.cb_DE_Frequency.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cb_DE_Frequency.Name = "cb_DE_Frequency";
            this.cb_DE_Frequency.Size = new System.Drawing.Size(361, 25);
            this.cb_DE_Frequency.TabIndex = 53;
            this.cb_DE_Frequency.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cbInstant_Frequency_DrawItem);
            // 
            // lblLSProfile
            // 
            this.lblLSProfile.AutoSize = true;
            this.lblLSProfile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLSProfile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLSProfile.Location = new System.Drawing.Point(3, 180);
            this.lblLSProfile.Name = "lblLSProfile";
            this.lblLSProfile.Size = new System.Drawing.Size(361, 30);
            this.lblLSProfile.TabIndex = 24;
            this.lblLSProfile.Text = "Load Survey Profile";
            this.lblLSProfile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txt_LS_DestIP
            // 
            this.txt_LS_DestIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_LS_DestIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_LS_DestIP.Location = new System.Drawing.Point(370, 182);
            this.txt_LS_DestIP.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt_LS_DestIP.Name = "txt_LS_DestIP";
            this.txt_LS_DestIP.Size = new System.Drawing.Size(361, 24);
            this.txt_LS_DestIP.TabIndex = 55;
            // 
            // cb_LS_Frequency
            // 
            this.cb_LS_Frequency.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb_LS_Frequency.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cb_LS_Frequency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_LS_Frequency.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_LS_Frequency.FormattingEnabled = true;
            this.cb_LS_Frequency.Items.AddRange(new object[] {
            "15 Min",
            "30 Min",
            "1 Hour",
            "4 Hour",
            "6 Hour",
            "8 Hour",
            "12 Hour",
            "24 Hour",
            "Disabled"});
            this.cb_LS_Frequency.Location = new System.Drawing.Point(737, 182);
            this.cb_LS_Frequency.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cb_LS_Frequency.Name = "cb_LS_Frequency";
            this.cb_LS_Frequency.Size = new System.Drawing.Size(361, 25);
            this.cb_LS_Frequency.TabIndex = 56;
            this.cb_LS_Frequency.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cbInstant_Frequency_DrawItem);
            // 
            // lblProfileHeader
            // 
            this.lblProfileHeader.AutoSize = true;
            this.lblProfileHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblProfileHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProfileHeader.Location = new System.Drawing.Point(3, 0);
            this.lblProfileHeader.Name = "lblProfileHeader";
            this.lblProfileHeader.Size = new System.Drawing.Size(361, 30);
            this.lblProfileHeader.TabIndex = 0;
            this.lblProfileHeader.Text = "Profiles";
            this.lblProfileHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDestIPHeader
            // 
            this.lblDestIPHeader.AutoSize = true;
            this.lblDestIPHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDestIPHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDestIPHeader.Location = new System.Drawing.Point(370, 0);
            this.lblDestIPHeader.Name = "lblDestIPHeader";
            this.lblDestIPHeader.Size = new System.Drawing.Size(361, 30);
            this.lblDestIPHeader.TabIndex = 1;
            this.lblDestIPHeader.Text = "Destination IP Address";
            this.lblDestIPHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPushFreqHeader
            // 
            this.lblPushFreqHeader.AutoSize = true;
            this.lblPushFreqHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPushFreqHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPushFreqHeader.Location = new System.Drawing.Point(737, 0);
            this.lblPushFreqHeader.Name = "lblPushFreqHeader";
            this.lblPushFreqHeader.Size = new System.Drawing.Size(361, 30);
            this.lblPushFreqHeader.TabIndex = 2;
            this.lblPushFreqHeader.Text = "Push Frequency Schedule";
            this.lblPushFreqHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cb_CB_Frequency
            // 
            this.cb_CB_Frequency.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb_CB_Frequency.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cb_CB_Frequency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_CB_Frequency.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_CB_Frequency.FormattingEnabled = true;
            this.cb_CB_Frequency.Items.AddRange(new object[] {
            "24 Hour",
            "Disabled"});
            this.cb_CB_Frequency.Location = new System.Drawing.Point(737, 212);
            this.cb_CB_Frequency.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cb_CB_Frequency.Name = "cb_CB_Frequency";
            this.cb_CB_Frequency.Size = new System.Drawing.Size(361, 25);
            this.cb_CB_Frequency.TabIndex = 59;
            this.cb_CB_Frequency.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cbInstant_Frequency_DrawItem);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.Controls.Add(this.cb_Bill_Frequency, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtBillFreq, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(734, 90);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(367, 30);
            this.tableLayoutPanel1.TabIndex = 69;
            // 
            // cb_Bill_Frequency
            // 
            this.cb_Bill_Frequency.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb_Bill_Frequency.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cb_Bill_Frequency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_Bill_Frequency.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_Bill_Frequency.FormattingEnabled = true;
            this.cb_Bill_Frequency.Items.AddRange(new object[] {
            "Custom",
            "Disabled"});
            this.cb_Bill_Frequency.Location = new System.Drawing.Point(3, 2);
            this.cb_Bill_Frequency.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cb_Bill_Frequency.Name = "cb_Bill_Frequency";
            this.cb_Bill_Frequency.Size = new System.Drawing.Size(140, 25);
            this.cb_Bill_Frequency.TabIndex = 47;
            this.cb_Bill_Frequency.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cbInstant_Frequency_DrawItem);
            this.cb_Bill_Frequency.SelectedIndexChanged += new System.EventHandler(this.cb_Bill_Frequency_SelectedIndexChanged);
            // 
            // txtBillFreq
            // 
            this.txtBillFreq.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBillFreq.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBillFreq.Location = new System.Drawing.Point(149, 2);
            this.txtBillFreq.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtBillFreq.Mask = "00/\\*/\\* 00:00:00";
            this.txtBillFreq.Name = "txtBillFreq";
            this.txtBillFreq.Size = new System.Drawing.Size(215, 25);
            this.txtBillFreq.TabIndex = 70;
            this.txtBillFreq.Leave += new System.EventHandler(this.txtBillFreq_Leave);
            // 
            // cmsNotificationOption
            // 
            this.cmsNotificationOption.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsNotificationOption.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cbHexCiphered,
            this.cbHexDecrypted,
            this.cbXML});
            this.cmsNotificationOption.Name = "cmsNotificationOption";
            this.cmsNotificationOption.Size = new System.Drawing.Size(188, 76);
            // 
            // cbHexCiphered
            // 
            this.cbHexCiphered.CheckOnClick = true;
            this.cbHexCiphered.Name = "cbHexCiphered";
            this.cbHexCiphered.Size = new System.Drawing.Size(187, 24);
            this.cbHexCiphered.Text = "Hex (Ciphered)";
            this.cbHexCiphered.Click += new System.EventHandler(this.cbHexCiphered_Click);
            // 
            // cbHexDecrypted
            // 
            this.cbHexDecrypted.CheckOnClick = true;
            this.cbHexDecrypted.Name = "cbHexDecrypted";
            this.cbHexDecrypted.Size = new System.Drawing.Size(187, 24);
            this.cbHexDecrypted.Text = "Hex (Decrypted)";
            this.cbHexDecrypted.Click += new System.EventHandler(this.cbHexDecrypted_Click);
            // 
            // cbXML
            // 
            this.cbXML.CheckOnClick = true;
            this.cbXML.Name = "cbXML";
            this.cbXML.Size = new System.Drawing.Size(187, 24);
            this.cbXML.Text = "Xml";
            this.cbXML.Click += new System.EventHandler(this.cbXML_Click);
            // 
            // ListenerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoScrollMargin = new System.Drawing.Size(0, 600);
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1996, 850);
            this.Controls.Add(this.tblMain);
            this.Controls.Add(this.pnlProfileSettings);
            this.Controls.Add(this.tblHeader);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "ListenerForm";
            this.Text = "DLMS Push Listener UI";
            this.Load += new System.EventHandler(this.ListenerForm_Load);
            this.tblMain.ResumeLayout(false);
            this.splitConMain.Panel1.ResumeLayout(false);
            this.splitConMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitConMain)).EndInit();
            this.splitConMain.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.PanelProfileandRawData.ResumeLayout(false);
            this.tabControlProfiles.ResumeLayout(false);
            this.tbpInstant.ResumeLayout(false);
            this.splitcon_InstantTab.Panel1.ResumeLayout(false);
            this.splitcon_InstantTab.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitcon_InstantTab)).EndInit();
            this.splitcon_InstantTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgInstant)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_Instant)).EndInit();
            this.tbpLS.ResumeLayout(false);
            this.splitCon_LS.Panel1.ResumeLayout(false);
            this.splitCon_LS.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitCon_LS)).EndInit();
            this.splitCon_LS.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgLS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_LS)).EndInit();
            this.tbpDE.ResumeLayout(false);
            this.splitCon_DE.Panel1.ResumeLayout(false);
            this.splitCon_DE.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitCon_DE)).EndInit();
            this.splitCon_DE.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgDE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_DE)).EndInit();
            this.tbpSR.ResumeLayout(false);
            this.splitCon_SR.Panel1.ResumeLayout(false);
            this.splitCon_SR.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitCon_SR)).EndInit();
            this.splitCon_SR.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgSR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_SR)).EndInit();
            this.tbpBill.ResumeLayout(false);
            this.splitCon_Bill.Panel1.ResumeLayout(false);
            this.splitCon_Bill.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitCon_Bill)).EndInit();
            this.splitCon_Bill.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgBill)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_Bill)).EndInit();
            this.tbpCB.ResumeLayout(false);
            this.splitCon_CB.Panel1.ResumeLayout(false);
            this.splitCon_CB.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitCon_CB)).EndInit();
            this.splitCon_CB.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgCB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_CB)).EndInit();
            this.tbpAlert.ResumeLayout(false);
            this.splitCon_Alert.Panel1.ResumeLayout(false);
            this.splitCon_Alert.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitCon_Alert)).EndInit();
            this.splitCon_Alert.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgAlert)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_Alert)).EndInit();
            this.tbpTamper.ResumeLayout(false);
            this.splitCon_Tamper.Panel1.ResumeLayout(false);
            this.splitCon_Tamper.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitCon_Tamper)).EndInit();
            this.splitCon_Tamper.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgTamper)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_tamper)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgRawData)).EndInit();
            this.tblHeader.ResumeLayout(false);
            this.tblHeader.PerformLayout();
            this.flowPanelButtons.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.pnlProfileSettings.ResumeLayout(false);
            this.grpPushObjects.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DGPushProfile)).EndInit();
            this.grpProfileConfig.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.tblProfileSettings.ResumeLayout(false);
            this.tblProfileSettings.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.cmsNotificationOption.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tblMain;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.SplitContainer splitConMain;
        private System.Windows.Forms.TabControl tabControlProfiles;
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
        private System.Windows.Forms.Label lblProfileHeader;
        private System.Windows.Forms.Label lblDestIPHeader;
        private System.Windows.Forms.Label lblPushFreqHeader;
        private System.Windows.Forms.Label lblRandomHeader;
        private System.Windows.Forms.TextBox txt_Random_CB;
        private System.Windows.Forms.TextBox txt_Random_LS;
        private System.Windows.Forms.TextBox txt_Random_DE;
        private System.Windows.Forms.TextBox txt_Random_SR;
        private System.Windows.Forms.TextBox txt_Random_Bill;
        private System.Windows.Forms.TextBox txt_Random_Instant;
        private System.Windows.Forms.RichTextBox rtbPushLogs;
        private System.Windows.Forms.Panel PanelProfileandRawData;
        private System.Windows.Forms.Button btnSet_PS_AS;
        private System.Windows.Forms.Button btnGet_PS_AS;
        private System.Windows.Forms.ComboBox cbTestProfileType;
        private System.Windows.Forms.Label lblProfile;
        private System.Windows.Forms.DataGridView dgInstant;
        private System.Windows.Forms.DataGridView dgLS;
        private System.Windows.Forms.DataGridView dgDE;
        private System.Windows.Forms.DataGridView dgSR;
        private System.Windows.Forms.DataGridView dgBill;
        private System.Windows.Forms.DataGridView dgCB;
        private System.Windows.Forms.DataGridView dgAlert;
        private System.Windows.Forms.DataGridView dgTamper;
        private System.Windows.Forms.SplitContainer splitcon_InstantTab;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_Instant;
        private System.Windows.Forms.SplitContainer splitCon_LS;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_LS;
        private System.Windows.Forms.SplitContainer splitCon_DE;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_DE;
        private System.Windows.Forms.SplitContainer splitCon_SR;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_SR;
        private System.Windows.Forms.SplitContainer splitCon_Bill;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_Bill;
        private System.Windows.Forms.SplitContainer splitCon_CB;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_CB;
        private System.Windows.Forms.SplitContainer splitCon_Alert;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_Alert;
        private System.Windows.Forms.SplitContainer splitCon_Tamper;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_tamper;
        private System.Windows.Forms.FlowLayoutPanel flowPanelButtons;
        private System.Windows.Forms.Button btnPushprofileSettings;
        private System.Windows.Forms.Button btnStartListener;
        private System.Windows.Forms.Button btnStopListener;
        private System.Windows.Forms.Button btnClearLogs;
        private System.Windows.Forms.Button btnSaveData;
        private System.Windows.Forms.Button btnRawData;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.MaskedTextBox txtBillFreq;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnNotificationType;
        private ContextMenuStrip cmsNotificationOption;
        private ToolStripMenuItem cbHexCiphered;
        private ToolStripMenuItem cbHexDecrypted;
        private ToolStripMenuItem cbXML;
        private RadioButton rbBtnUserDefinedLog;
        private RadioButton rbBtnDetailLog;
        private TableLayoutPanel tableLayoutPanel4;
        private DataGridView dgRawData;
        private TableLayoutPanel tableLayoutPanel5;
        private TableLayoutPanel tableLayoutPanel6;
        private GroupBox grpPushObjects;
        private TableLayoutPanel tableLayoutPanel7;
        private TreeView tvPushObjects;
        private DataGridView DGPushProfile;
        private Button btnNotifyDecryptSettings;
    }
}
