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
            this.pnlMain = new System.Windows.Forms.Panel();
            this.splitConMain = new System.Windows.Forms.SplitContainer();
            this.pnlLogs = new System.Windows.Forms.Panel();
            this.rtbPushLogs = new System.Windows.Forms.RichTextBox();
            this.lblLogsHeader = new System.Windows.Forms.Label();
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
            this.lblDataHeader = new System.Windows.Forms.Label();
            this.pnlProfileSettings = new System.Windows.Forms.Panel();
            this.grpPushObjects = new System.Windows.Forms.GroupBox();
            this.splitPushObjects = new System.Windows.Forms.SplitContainer();
            this.tvPushObjects = new System.Windows.Forms.TreeView();
            this.DGPushProfile = new System.Windows.Forms.DataGridView();
            this.grpProfileConfig = new System.Windows.Forms.GroupBox();
            this.tblProfileSettings = new System.Windows.Forms.TableLayoutPanel();
            this.txt_Tamper_DestIP = new System.Windows.Forms.TextBox();
            this.txt_Random_Tamper = new System.Windows.Forms.TextBox();
            this.cb_Tamper_Frequency = new System.Windows.Forms.ComboBox();
            this.lblTamperProfile = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblRandomHeader = new System.Windows.Forms.Label();
            this.chkRandomisation = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.chkFreq = new System.Windows.Forms.CheckBox();
            this.lblPushFreqHeader = new System.Windows.Forms.Label();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.lblDestIPHeader = new System.Windows.Forms.Label();
            this.chkDestination = new System.Windows.Forms.CheckBox();
            this.txt_Random_CB = new System.Windows.Forms.TextBox();
            this.txt_Random_LS = new System.Windows.Forms.TextBox();
            this.txt_Random_DE = new System.Windows.Forms.TextBox();
            this.txt_Random_SR = new System.Windows.Forms.TextBox();
            this.txt_Random_Bill = new System.Windows.Forms.TextBox();
            this.txt_Random_Instant = new System.Windows.Forms.TextBox();
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
            this.cb_CB_Frequency = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.cb_Bill_Frequency = new System.Windows.Forms.ComboBox();
            this.txtBillFreq = new System.Windows.Forms.MaskedTextBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.lblSelectProfile = new System.Windows.Forms.Label();
            this.cbTestProfileType = new System.Windows.Forms.ComboBox();
            this.btnGet_PS_AS = new System.Windows.Forms.Button();
            this.btnSet_PS_AS = new System.Windows.Forms.Button();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.flowPanelButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnPushprofileSettings = new System.Windows.Forms.Button();
            this.pnlLoggingMode = new System.Windows.Forms.Panel();
            this.rbBtnUserDefinedLog = new System.Windows.Forms.RadioButton();
            this.rbBtnDetailLog = new System.Windows.Forms.RadioButton();
            this.btnNotifyDecryptSettings = new System.Windows.Forms.Button();
            this.btnNotificationType = new System.Windows.Forms.Button();
            this.btnStartListener = new System.Windows.Forms.Button();
            this.btnStopListener = new System.Windows.Forms.Button();
            this.btnClearLogs = new System.Windows.Forms.Button();
            this.btnSaveData = new System.Windows.Forms.Button();
            this.btnRawData = new System.Windows.Forms.Button();
            this.btnSpecificOBIS = new System.Windows.Forms.Button();
            this.btnCommSettings = new System.Windows.Forms.Button();
            this.lblHeader = new System.Windows.Forms.Label();
            this.splitLoadSurvey = new System.Windows.Forms.SplitContainer();
            this.dgvLoadSurvey = new System.Windows.Forms.DataGridView();
            this.chartLoadSurvey = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.splitDailyEnergy = new System.Windows.Forms.SplitContainer();
            this.dgvDailyEnergy = new System.Windows.Forms.DataGridView();
            this.chartDailyEnergy = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.splitSelfReg = new System.Windows.Forms.SplitContainer();
            this.dgvSelfReg = new System.Windows.Forms.DataGridView();
            this.chartSelfReg = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.splitBilling = new System.Windows.Forms.SplitContainer();
            this.dgvBilling = new System.Windows.Forms.DataGridView();
            this.chartBilling = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.splitCurrentBill = new System.Windows.Forms.SplitContainer();
            this.dgvCurrentBill = new System.Windows.Forms.DataGridView();
            this.chartCurrentBill = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.splitAlert = new System.Windows.Forms.SplitContainer();
            this.dgvAlert = new System.Windows.Forms.DataGridView();
            this.chartAlert = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.splitTamper = new System.Windows.Forms.SplitContainer();
            this.dgvTamper = new System.Windows.Forms.DataGridView();
            this.chartTamper = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.cmsNotificationOption = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cbHexCiphered = new System.Windows.Forms.ToolStripMenuItem();
            this.cbHexDecrypted = new System.Windows.Forms.ToolStripMenuItem();
            this.cbXML = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitConMain)).BeginInit();
            this.splitConMain.Panel1.SuspendLayout();
            this.splitConMain.Panel2.SuspendLayout();
            this.splitConMain.SuspendLayout();
            this.pnlLogs.SuspendLayout();
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
            this.pnlProfileSettings.SuspendLayout();
            this.grpPushObjects.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitPushObjects)).BeginInit();
            this.splitPushObjects.Panel1.SuspendLayout();
            this.splitPushObjects.Panel2.SuspendLayout();
            this.splitPushObjects.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGPushProfile)).BeginInit();
            this.grpProfileConfig.SuspendLayout();
            this.tblProfileSettings.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.flowPanelButtons.SuspendLayout();
            this.pnlLoggingMode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitLoadSurvey)).BeginInit();
            this.splitLoadSurvey.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLoadSurvey)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartLoadSurvey)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitDailyEnergy)).BeginInit();
            this.splitDailyEnergy.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDailyEnergy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartDailyEnergy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitSelfReg)).BeginInit();
            this.splitSelfReg.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelfReg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartSelfReg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitBilling)).BeginInit();
            this.splitBilling.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBilling)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartBilling)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitCurrentBill)).BeginInit();
            this.splitCurrentBill.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCurrentBill)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartCurrentBill)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitAlert)).BeginInit();
            this.splitAlert.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAlert)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartAlert)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitTamper)).BeginInit();
            this.splitTamper.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTamper)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartTamper)).BeginInit();
            this.cmsNotificationOption.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.pnlMain.Controls.Add(this.splitConMain);
            this.pnlMain.Controls.Add(this.pnlProfileSettings);
            this.pnlMain.Controls.Add(this.pnlHeader);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(1600, 900);
            this.pnlMain.TabIndex = 0;
            // 
            // splitConMain
            // 
            this.splitConMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(94)))), ((int)(((byte)(168)))));
            this.splitConMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitConMain.Location = new System.Drawing.Point(0, 626);
            this.splitConMain.Name = "splitConMain";
            // 
            // splitConMain.Panel1
            // 
            this.splitConMain.Panel1.Controls.Add(this.pnlLogs);
            // 
            // splitConMain.Panel2
            // 
            this.splitConMain.Panel2.Controls.Add(this.PanelProfileandRawData);
            this.splitConMain.Size = new System.Drawing.Size(1600, 274);
            this.splitConMain.SplitterDistance = 696;
            this.splitConMain.SplitterWidth = 5;
            this.splitConMain.TabIndex = 2;
            // 
            // pnlLogs
            // 
            this.pnlLogs.BackColor = System.Drawing.Color.White;
            this.pnlLogs.Controls.Add(this.rtbPushLogs);
            this.pnlLogs.Controls.Add(this.lblLogsHeader);
            this.pnlLogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLogs.Location = new System.Drawing.Point(0, 0);
            this.pnlLogs.Name = "pnlLogs";
            this.pnlLogs.Size = new System.Drawing.Size(696, 274);
            this.pnlLogs.TabIndex = 0;
            // 
            // rtbPushLogs
            // 
            this.rtbPushLogs.BackColor = System.Drawing.SystemColors.Window;
            this.rtbPushLogs.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbPushLogs.DetectUrls = false;
            this.rtbPushLogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbPushLogs.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbPushLogs.Location = new System.Drawing.Point(0, 35);
            this.rtbPushLogs.Margin = new System.Windows.Forms.Padding(0);
            this.rtbPushLogs.Name = "rtbPushLogs";
            this.rtbPushLogs.ReadOnly = true;
            this.rtbPushLogs.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.rtbPushLogs.Size = new System.Drawing.Size(696, 239);
            this.rtbPushLogs.TabIndex = 5;
            this.rtbPushLogs.Text = "";
            this.rtbPushLogs.WordWrap = false;
            // 
            // lblLogsHeader
            // 
            this.lblLogsHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(94)))), ((int)(((byte)(168)))));
            this.lblLogsHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLogsHeader.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblLogsHeader.ForeColor = System.Drawing.Color.White;
            this.lblLogsHeader.Location = new System.Drawing.Point(0, 0);
            this.lblLogsHeader.Name = "lblLogsHeader";
            this.lblLogsHeader.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.lblLogsHeader.Size = new System.Drawing.Size(696, 35);
            this.lblLogsHeader.TabIndex = 0;
            this.lblLogsHeader.Text = "Push Notifications and Logs";
            this.lblLogsHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PanelProfileandRawData
            // 
            this.PanelProfileandRawData.BackColor = System.Drawing.Color.White;
            this.PanelProfileandRawData.Controls.Add(this.tabControlProfiles);
            this.PanelProfileandRawData.Controls.Add(this.dgRawData);
            this.PanelProfileandRawData.Controls.Add(this.lblDataHeader);
            this.PanelProfileandRawData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelProfileandRawData.Location = new System.Drawing.Point(0, 0);
            this.PanelProfileandRawData.Name = "PanelProfileandRawData";
            this.PanelProfileandRawData.Size = new System.Drawing.Size(899, 274);
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
            this.tabControlProfiles.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.tabControlProfiles.ItemSize = new System.Drawing.Size(90, 35);
            this.tabControlProfiles.Location = new System.Drawing.Point(0, 35);
            this.tabControlProfiles.Name = "tabControlProfiles";
            this.tabControlProfiles.SelectedIndex = 0;
            this.tabControlProfiles.Size = new System.Drawing.Size(899, 239);
            this.tabControlProfiles.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControlProfiles.TabIndex = 1;
            // 
            // tbpInstant
            // 
            this.tbpInstant.Controls.Add(this.splitcon_InstantTab);
            this.tbpInstant.Location = new System.Drawing.Point(4, 39);
            this.tbpInstant.Name = "tbpInstant";
            this.tbpInstant.Padding = new System.Windows.Forms.Padding(3);
            this.tbpInstant.Size = new System.Drawing.Size(891, 196);
            this.tbpInstant.TabIndex = 0;
            this.tbpInstant.Text = "Instant";
            this.tbpInstant.UseVisualStyleBackColor = true;
            // 
            // splitcon_InstantTab
            // 
            this.splitcon_InstantTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitcon_InstantTab.Location = new System.Drawing.Point(3, 3);
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
            this.splitcon_InstantTab.Size = new System.Drawing.Size(885, 190);
            this.splitcon_InstantTab.SplitterDistance = 120;
            this.splitcon_InstantTab.TabIndex = 2;
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
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgInstant.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgInstant.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
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
            this.dgInstant.Size = new System.Drawing.Size(885, 120);
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
            this.chart_Instant.Size = new System.Drawing.Size(885, 66);
            this.chart_Instant.TabIndex = 0;
            this.chart_Instant.Text = "chart1";
            // 
            // tbpLS
            // 
            this.tbpLS.Controls.Add(this.splitCon_LS);
            this.tbpLS.Location = new System.Drawing.Point(4, 39);
            this.tbpLS.Name = "tbpLS";
            this.tbpLS.Padding = new System.Windows.Forms.Padding(3);
            this.tbpLS.Size = new System.Drawing.Size(891, 196);
            this.tbpLS.TabIndex = 1;
            this.tbpLS.Text = "Load Survey";
            this.tbpLS.UseVisualStyleBackColor = true;
            // 
            // splitCon_LS
            // 
            this.splitCon_LS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitCon_LS.Location = new System.Drawing.Point(3, 3);
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
            this.splitCon_LS.Size = new System.Drawing.Size(885, 190);
            this.splitCon_LS.SplitterDistance = 120;
            this.splitCon_LS.TabIndex = 7;
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
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
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
            this.dgLS.Size = new System.Drawing.Size(885, 120);
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
            this.chart_LS.Size = new System.Drawing.Size(885, 66);
            this.chart_LS.TabIndex = 3;
            this.chart_LS.Text = "chart1";
            // 
            // tbpDE
            // 
            this.tbpDE.Controls.Add(this.splitCon_DE);
            this.tbpDE.Location = new System.Drawing.Point(4, 39);
            this.tbpDE.Name = "tbpDE";
            this.tbpDE.Size = new System.Drawing.Size(891, 196);
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
            this.splitCon_DE.Size = new System.Drawing.Size(891, 196);
            this.splitCon_DE.SplitterDistance = 121;
            this.splitCon_DE.TabIndex = 8;
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
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
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
            this.dgDE.Size = new System.Drawing.Size(891, 121);
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
            this.chart_DE.Size = new System.Drawing.Size(891, 71);
            this.chart_DE.TabIndex = 3;
            this.chart_DE.Text = "chart1";
            // 
            // tbpSR
            // 
            this.tbpSR.Controls.Add(this.splitCon_SR);
            this.tbpSR.Location = new System.Drawing.Point(4, 39);
            this.tbpSR.Name = "tbpSR";
            this.tbpSR.Size = new System.Drawing.Size(891, 196);
            this.tbpSR.TabIndex = 3;
            this.tbpSR.Text = "Self Reg";
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
            this.splitCon_SR.Size = new System.Drawing.Size(891, 196);
            this.splitCon_SR.SplitterDistance = 121;
            this.splitCon_SR.TabIndex = 8;
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
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
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
            this.dgSR.Size = new System.Drawing.Size(891, 121);
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
            this.chart_SR.Size = new System.Drawing.Size(891, 71);
            this.chart_SR.TabIndex = 3;
            this.chart_SR.Text = "chart1";
            // 
            // tbpBill
            // 
            this.tbpBill.Controls.Add(this.splitCon_Bill);
            this.tbpBill.Location = new System.Drawing.Point(4, 39);
            this.tbpBill.Name = "tbpBill";
            this.tbpBill.Size = new System.Drawing.Size(891, 196);
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
            this.splitCon_Bill.Size = new System.Drawing.Size(891, 196);
            this.splitCon_Bill.SplitterDistance = 121;
            this.splitCon_Bill.TabIndex = 8;
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
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
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
            this.dgBill.Size = new System.Drawing.Size(891, 121);
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
            this.chart_Bill.Size = new System.Drawing.Size(891, 71);
            this.chart_Bill.TabIndex = 3;
            this.chart_Bill.Text = "chart1";
            // 
            // tbpCB
            // 
            this.tbpCB.Controls.Add(this.splitCon_CB);
            this.tbpCB.Location = new System.Drawing.Point(4, 39);
            this.tbpCB.Name = "tbpCB";
            this.tbpCB.Size = new System.Drawing.Size(891, 196);
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
            this.splitCon_CB.Size = new System.Drawing.Size(891, 196);
            this.splitCon_CB.SplitterDistance = 121;
            this.splitCon_CB.TabIndex = 8;
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
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
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
            this.dgCB.Size = new System.Drawing.Size(891, 121);
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
            this.chart_CB.Size = new System.Drawing.Size(891, 71);
            this.chart_CB.TabIndex = 3;
            this.chart_CB.Text = "chart1";
            // 
            // tbpAlert
            // 
            this.tbpAlert.Controls.Add(this.splitCon_Alert);
            this.tbpAlert.Location = new System.Drawing.Point(4, 39);
            this.tbpAlert.Name = "tbpAlert";
            this.tbpAlert.Size = new System.Drawing.Size(891, 196);
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
            this.splitCon_Alert.Size = new System.Drawing.Size(891, 196);
            this.splitCon_Alert.SplitterDistance = 121;
            this.splitCon_Alert.TabIndex = 8;
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
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
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
            this.dgAlert.Size = new System.Drawing.Size(891, 121);
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
            this.chart_Alert.Size = new System.Drawing.Size(891, 71);
            this.chart_Alert.TabIndex = 3;
            this.chart_Alert.Text = "chart1";
            // 
            // tbpTamper
            // 
            this.tbpTamper.Controls.Add(this.splitCon_Tamper);
            this.tbpTamper.Location = new System.Drawing.Point(4, 39);
            this.tbpTamper.Name = "tbpTamper";
            this.tbpTamper.Size = new System.Drawing.Size(891, 196);
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
            this.splitCon_Tamper.Size = new System.Drawing.Size(891, 196);
            this.splitCon_Tamper.SplitterDistance = 121;
            this.splitCon_Tamper.TabIndex = 8;
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
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
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
            this.dgTamper.Size = new System.Drawing.Size(891, 121);
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
            this.chart_tamper.Size = new System.Drawing.Size(891, 71);
            this.chart_tamper.TabIndex = 3;
            this.chart_tamper.Text = "chart1";
            // 
            // dgRawData
            // 
            this.dgRawData.AllowUserToAddRows = false;
            this.dgRawData.AllowUserToDeleteRows = false;
            this.dgRawData.AllowUserToResizeRows = false;
            this.dgRawData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgRawData.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dgRawData.BackgroundColor = System.Drawing.Color.White;
            this.dgRawData.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.LightSteelBlue;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle10.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgRawData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.dgRawData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgRawData.DefaultCellStyle = dataGridViewCellStyle11;
            this.dgRawData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgRawData.EnableHeadersVisualStyles = false;
            this.dgRawData.GridColor = System.Drawing.SystemColors.AppWorkspace;
            this.dgRawData.Location = new System.Drawing.Point(0, 35);
            this.dgRawData.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgRawData.Name = "dgRawData";
            this.dgRawData.ReadOnly = true;
            this.dgRawData.RowHeadersVisible = false;
            this.dgRawData.RowHeadersWidth = 50;
            this.dgRawData.RowTemplate.Height = 24;
            this.dgRawData.Size = new System.Drawing.Size(899, 239);
            this.dgRawData.TabIndex = 2;
            // 
            // lblDataHeader
            // 
            this.lblDataHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(94)))), ((int)(((byte)(168)))));
            this.lblDataHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDataHeader.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblDataHeader.ForeColor = System.Drawing.Color.White;
            this.lblDataHeader.Location = new System.Drawing.Point(0, 0);
            this.lblDataHeader.Name = "lblDataHeader";
            this.lblDataHeader.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.lblDataHeader.Size = new System.Drawing.Size(899, 35);
            this.lblDataHeader.TabIndex = 0;
            this.lblDataHeader.Text = "Data and Reports";
            this.lblDataHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlProfileSettings
            // 
            this.pnlProfileSettings.BackColor = System.Drawing.Color.White;
            this.pnlProfileSettings.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlProfileSettings.Controls.Add(this.grpPushObjects);
            this.pnlProfileSettings.Controls.Add(this.grpProfileConfig);
            this.pnlProfileSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlProfileSettings.Location = new System.Drawing.Point(0, 104);
            this.pnlProfileSettings.Name = "pnlProfileSettings";
            this.pnlProfileSettings.Padding = new System.Windows.Forms.Padding(10);
            this.pnlProfileSettings.Size = new System.Drawing.Size(1600, 522);
            this.pnlProfileSettings.TabIndex = 1;
            this.pnlProfileSettings.Visible = false;
            // 
            // grpPushObjects
            // 
            this.grpPushObjects.Controls.Add(this.splitPushObjects);
            this.grpPushObjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpPushObjects.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.grpPushObjects.Location = new System.Drawing.Point(10, 311);
            this.grpPushObjects.Name = "grpPushObjects";
            this.grpPushObjects.Padding = new System.Windows.Forms.Padding(10);
            this.grpPushObjects.Size = new System.Drawing.Size(1578, 199);
            this.grpPushObjects.TabIndex = 1;
            this.grpPushObjects.TabStop = false;
            this.grpPushObjects.Text = "Push Setup Objects";
            // 
            // splitPushObjects
            // 
            this.splitPushObjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitPushObjects.Location = new System.Drawing.Point(10, 33);
            this.splitPushObjects.Name = "splitPushObjects";
            // 
            // splitPushObjects.Panel1
            // 
            this.splitPushObjects.Panel1.Controls.Add(this.tvPushObjects);
            // 
            // splitPushObjects.Panel2
            // 
            this.splitPushObjects.Panel2.Controls.Add(this.DGPushProfile);
            this.splitPushObjects.Size = new System.Drawing.Size(1558, 156);
            this.splitPushObjects.SplitterDistance = 400;
            this.splitPushObjects.TabIndex = 0;
            // 
            // tvPushObjects
            // 
            this.tvPushObjects.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.tvPushObjects.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tvPushObjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvPushObjects.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tvPushObjects.Location = new System.Drawing.Point(0, 0);
            this.tvPushObjects.Name = "tvPushObjects";
            this.tvPushObjects.ShowLines = false;
            this.tvPushObjects.ShowPlusMinus = false;
            this.tvPushObjects.ShowRootLines = false;
            this.tvPushObjects.Size = new System.Drawing.Size(400, 156);
            this.tvPushObjects.TabIndex = 0;
            this.tvPushObjects.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvPushObjects_AfterSelect);
            // 
            // DGPushProfile
            // 
            this.DGPushProfile.AllowUserToAddRows = false;
            this.DGPushProfile.AllowUserToDeleteRows = false;
            this.DGPushProfile.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.DGPushProfile.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGPushProfile.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle12;
            this.DGPushProfile.ColumnHeadersHeight = 35;
            this.DGPushProfile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DGPushProfile.Location = new System.Drawing.Point(0, 0);
            this.DGPushProfile.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DGPushProfile.Name = "DGPushProfile";
            this.DGPushProfile.ReadOnly = true;
            this.DGPushProfile.RowHeadersVisible = false;
            this.DGPushProfile.RowHeadersWidth = 35;
            this.DGPushProfile.RowTemplate.Height = 24;
            this.DGPushProfile.Size = new System.Drawing.Size(1154, 156);
            this.DGPushProfile.TabIndex = 2;
            // 
            // grpProfileConfig
            // 
            this.grpProfileConfig.Controls.Add(this.tblProfileSettings);
            this.grpProfileConfig.Controls.Add(this.flowLayoutPanel1);
            this.grpProfileConfig.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpProfileConfig.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.grpProfileConfig.Location = new System.Drawing.Point(10, 10);
            this.grpProfileConfig.Name = "grpProfileConfig";
            this.grpProfileConfig.Padding = new System.Windows.Forms.Padding(10);
            this.grpProfileConfig.Size = new System.Drawing.Size(1578, 301);
            this.grpProfileConfig.TabIndex = 0;
            this.grpProfileConfig.TabStop = false;
            this.grpProfileConfig.Text = "Push Setup And Action Schedule Settings";
            // 
            // tblProfileSettings
            // 
            this.tblProfileSettings.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.tblProfileSettings.ColumnCount = 4;
            this.tblProfileSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblProfileSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblProfileSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblProfileSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblProfileSettings.Controls.Add(this.txt_Tamper_DestIP, 1, 8);
            this.tblProfileSettings.Controls.Add(this.txt_Random_Tamper, 3, 8);
            this.tblProfileSettings.Controls.Add(this.cb_Tamper_Frequency, 2, 8);
            this.tblProfileSettings.Controls.Add(this.lblTamperProfile, 0, 8);
            this.tblProfileSettings.Controls.Add(this.tableLayoutPanel1, 3, 0);
            this.tblProfileSettings.Controls.Add(this.tableLayoutPanel5, 2, 0);
            this.tblProfileSettings.Controls.Add(this.tableLayoutPanel6, 1, 0);
            this.tblProfileSettings.Controls.Add(this.txt_Random_CB, 3, 7);
            this.tblProfileSettings.Controls.Add(this.txt_Random_LS, 3, 5);
            this.tblProfileSettings.Controls.Add(this.txt_Random_DE, 3, 4);
            this.tblProfileSettings.Controls.Add(this.txt_Random_SR, 3, 6);
            this.tblProfileSettings.Controls.Add(this.txt_Random_Bill, 3, 3);
            this.tblProfileSettings.Controls.Add(this.txt_Random_Instant, 3, 2);
            this.tblProfileSettings.Controls.Add(this.lblCBProfile, 0, 7);
            this.tblProfileSettings.Controls.Add(this.txt_CB_DestIP, 1, 7);
            this.tblProfileSettings.Controls.Add(this.lblInstant, 0, 2);
            this.tblProfileSettings.Controls.Add(this.txt_Instant_DestIP, 1, 2);
            this.tblProfileSettings.Controls.Add(this.cbInstant_Frequency, 2, 2);
            this.tblProfileSettings.Controls.Add(this.lblAlert, 0, 1);
            this.tblProfileSettings.Controls.Add(this.txt_Alert_DestIP, 1, 1);
            this.tblProfileSettings.Controls.Add(this.lblBillingProfile, 0, 3);
            this.tblProfileSettings.Controls.Add(this.txt_Bill_DestIP, 1, 3);
            this.tblProfileSettings.Controls.Add(this.lblSRProfile, 0, 6);
            this.tblProfileSettings.Controls.Add(this.txt_SR_DestIP, 1, 6);
            this.tblProfileSettings.Controls.Add(this.cb_SR_Frequency, 2, 6);
            this.tblProfileSettings.Controls.Add(this.lblDEProfile, 0, 4);
            this.tblProfileSettings.Controls.Add(this.txt_DE_DestIP, 1, 4);
            this.tblProfileSettings.Controls.Add(this.cb_DE_Frequency, 2, 4);
            this.tblProfileSettings.Controls.Add(this.lblLSProfile, 0, 5);
            this.tblProfileSettings.Controls.Add(this.txt_LS_DestIP, 1, 5);
            this.tblProfileSettings.Controls.Add(this.cb_LS_Frequency, 2, 5);
            this.tblProfileSettings.Controls.Add(this.lblProfileHeader, 0, 0);
            this.tblProfileSettings.Controls.Add(this.cb_CB_Frequency, 2, 7);
            this.tblProfileSettings.Controls.Add(this.tableLayoutPanel8, 2, 3);
            this.tblProfileSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblProfileSettings.Location = new System.Drawing.Point(10, 77);
            this.tblProfileSettings.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.tblProfileSettings.Name = "tblProfileSettings";
            this.tblProfileSettings.RowCount = 9;
            this.tblProfileSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tblProfileSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tblProfileSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tblProfileSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tblProfileSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tblProfileSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tblProfileSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tblProfileSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tblProfileSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tblProfileSettings.Size = new System.Drawing.Size(1558, 214);
            this.tblProfileSettings.TabIndex = 1;
            // 
            // txt_Tamper_DestIP
            // 
            this.txt_Tamper_DestIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_Tamper_DestIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_Tamper_DestIP.Location = new System.Drawing.Point(394, 186);
            this.txt_Tamper_DestIP.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.txt_Tamper_DestIP.Name = "txt_Tamper_DestIP";
            this.txt_Tamper_DestIP.Size = new System.Drawing.Size(381, 24);
            this.txt_Tamper_DestIP.TabIndex = 81;
            // 
            // txt_Random_Tamper
            // 
            this.txt_Random_Tamper.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_Random_Tamper.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_Random_Tamper.Location = new System.Drawing.Point(1172, 186);
            this.txt_Random_Tamper.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.txt_Random_Tamper.Name = "txt_Random_Tamper";
            this.txt_Random_Tamper.Size = new System.Drawing.Size(381, 24);
            this.txt_Random_Tamper.TabIndex = 79;
            // 
            // cb_Tamper_Frequency
            // 
            this.cb_Tamper_Frequency.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb_Tamper_Frequency.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cb_Tamper_Frequency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_Tamper_Frequency.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_Tamper_Frequency.FormattingEnabled = true;
            this.cb_Tamper_Frequency.Items.AddRange(new object[] {
            "15 Min",
            "30 Min",
            "1 Hour",
            "4 Hour",
            "6 Hour",
            "8 Hour",
            "12 Hour",
            "24 Hour",
            "Disabled"});
            this.cb_Tamper_Frequency.Location = new System.Drawing.Point(783, 186);
            this.cb_Tamper_Frequency.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.cb_Tamper_Frequency.Name = "cb_Tamper_Frequency";
            this.cb_Tamper_Frequency.Size = new System.Drawing.Size(381, 25);
            this.cb_Tamper_Frequency.TabIndex = 80;
            this.cb_Tamper_Frequency.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cbInstant_Frequency_DrawItem);
            // 
            // lblTamperProfile
            // 
            this.lblTamperProfile.AutoSize = true;
            this.lblTamperProfile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTamperProfile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTamperProfile.Location = new System.Drawing.Point(5, 186);
            this.lblTamperProfile.Name = "lblTamperProfile";
            this.lblTamperProfile.Size = new System.Drawing.Size(381, 26);
            this.lblTamperProfile.TabIndex = 78;
            this.lblTamperProfile.Text = "Tamper Profile";
            this.lblTamperProfile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 73.08868F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 26.91132F));
            this.tableLayoutPanel1.Controls.Add(this.lblRandomHeader, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.chkRandomisation, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(1170, 3);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(385, 19);
            this.tableLayoutPanel1.TabIndex = 77;
            // 
            // lblRandomHeader
            // 
            this.lblRandomHeader.AutoSize = true;
            this.lblRandomHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRandomHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRandomHeader.Location = new System.Drawing.Point(3, 0);
            this.lblRandomHeader.Name = "lblRandomHeader";
            this.lblRandomHeader.Size = new System.Drawing.Size(275, 19);
            this.lblRandomHeader.TabIndex = 72;
            this.lblRandomHeader.Text = "Randomisation (In Min)";
            this.lblRandomHeader.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkRandomisation
            // 
            this.chkRandomisation.AutoSize = true;
            this.chkRandomisation.Checked = true;
            this.chkRandomisation.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRandomisation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkRandomisation.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkRandomisation.Location = new System.Drawing.Point(284, 0);
            this.chkRandomisation.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.chkRandomisation.Name = "chkRandomisation";
            this.chkRandomisation.Padding = new System.Windows.Forms.Padding(10, 3, 0, 0);
            this.chkRandomisation.Size = new System.Drawing.Size(98, 19);
            this.chkRandomisation.TabIndex = 71;
            this.chkRandomisation.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 79.01234F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.98765F));
            this.tableLayoutPanel5.Controls.Add(this.chkFreq, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.lblPushFreqHeader, 0, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(781, 3);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(1);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(385, 19);
            this.tableLayoutPanel5.TabIndex = 76;
            // 
            // chkFreq
            // 
            this.chkFreq.AutoSize = true;
            this.chkFreq.Checked = true;
            this.chkFreq.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFreq.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkFreq.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkFreq.Location = new System.Drawing.Point(307, 0);
            this.chkFreq.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.chkFreq.Name = "chkFreq";
            this.chkFreq.Padding = new System.Windows.Forms.Padding(10, 3, 0, 0);
            this.chkFreq.Size = new System.Drawing.Size(75, 19);
            this.chkFreq.TabIndex = 71;
            this.chkFreq.UseVisualStyleBackColor = true;
            // 
            // lblPushFreqHeader
            // 
            this.lblPushFreqHeader.AutoSize = true;
            this.lblPushFreqHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPushFreqHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPushFreqHeader.Location = new System.Drawing.Point(3, 0);
            this.lblPushFreqHeader.Name = "lblPushFreqHeader";
            this.lblPushFreqHeader.Size = new System.Drawing.Size(298, 19);
            this.lblPushFreqHeader.TabIndex = 2;
            this.lblPushFreqHeader.Text = "Push Frequency Schedule";
            this.lblPushFreqHeader.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 76.23457F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.76543F));
            this.tableLayoutPanel6.Controls.Add(this.lblDestIPHeader, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.chkDestination, 1, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(392, 3);
            this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(1);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(385, 19);
            this.tableLayoutPanel6.TabIndex = 75;
            // 
            // lblDestIPHeader
            // 
            this.lblDestIPHeader.AutoSize = true;
            this.lblDestIPHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDestIPHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDestIPHeader.Location = new System.Drawing.Point(3, 0);
            this.lblDestIPHeader.Name = "lblDestIPHeader";
            this.lblDestIPHeader.Size = new System.Drawing.Size(287, 19);
            this.lblDestIPHeader.TabIndex = 2;
            this.lblDestIPHeader.Text = "Destination IP Address";
            this.lblDestIPHeader.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkDestination
            // 
            this.chkDestination.AutoSize = true;
            this.chkDestination.Checked = true;
            this.chkDestination.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDestination.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkDestination.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkDestination.Location = new System.Drawing.Point(296, 0);
            this.chkDestination.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.chkDestination.Name = "chkDestination";
            this.chkDestination.Padding = new System.Windows.Forms.Padding(10, 3, 0, 0);
            this.chkDestination.Size = new System.Drawing.Size(86, 19);
            this.chkDestination.TabIndex = 70;
            this.chkDestination.UseVisualStyleBackColor = true;
            // 
            // txt_Random_CB
            // 
            this.txt_Random_CB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_Random_CB.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_Random_CB.Location = new System.Drawing.Point(1172, 163);
            this.txt_Random_CB.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.txt_Random_CB.Name = "txt_Random_CB";
            this.txt_Random_CB.Size = new System.Drawing.Size(381, 24);
            this.txt_Random_CB.TabIndex = 60;
            // 
            // txt_Random_LS
            // 
            this.txt_Random_LS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_Random_LS.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_Random_LS.Location = new System.Drawing.Point(1172, 117);
            this.txt_Random_LS.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.txt_Random_LS.Name = "txt_Random_LS";
            this.txt_Random_LS.Size = new System.Drawing.Size(381, 24);
            this.txt_Random_LS.TabIndex = 57;
            // 
            // txt_Random_DE
            // 
            this.txt_Random_DE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_Random_DE.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_Random_DE.Location = new System.Drawing.Point(1172, 94);
            this.txt_Random_DE.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.txt_Random_DE.Name = "txt_Random_DE";
            this.txt_Random_DE.Size = new System.Drawing.Size(381, 24);
            this.txt_Random_DE.TabIndex = 54;
            // 
            // txt_Random_SR
            // 
            this.txt_Random_SR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_Random_SR.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_Random_SR.Location = new System.Drawing.Point(1172, 140);
            this.txt_Random_SR.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.txt_Random_SR.Name = "txt_Random_SR";
            this.txt_Random_SR.Size = new System.Drawing.Size(381, 24);
            this.txt_Random_SR.TabIndex = 51;
            // 
            // txt_Random_Bill
            // 
            this.txt_Random_Bill.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_Random_Bill.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_Random_Bill.Location = new System.Drawing.Point(1172, 71);
            this.txt_Random_Bill.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.txt_Random_Bill.Name = "txt_Random_Bill";
            this.txt_Random_Bill.Size = new System.Drawing.Size(381, 24);
            this.txt_Random_Bill.TabIndex = 48;
            // 
            // txt_Random_Instant
            // 
            this.txt_Random_Instant.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_Random_Instant.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_Random_Instant.Location = new System.Drawing.Point(1172, 48);
            this.txt_Random_Instant.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.txt_Random_Instant.Name = "txt_Random_Instant";
            this.txt_Random_Instant.Size = new System.Drawing.Size(381, 24);
            this.txt_Random_Instant.TabIndex = 42;
            // 
            // lblCBProfile
            // 
            this.lblCBProfile.AutoSize = true;
            this.lblCBProfile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCBProfile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCBProfile.Location = new System.Drawing.Point(5, 163);
            this.lblCBProfile.Name = "lblCBProfile";
            this.lblCBProfile.Size = new System.Drawing.Size(381, 21);
            this.lblCBProfile.TabIndex = 45;
            this.lblCBProfile.Text = "Current Bill Profile";
            this.lblCBProfile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txt_CB_DestIP
            // 
            this.txt_CB_DestIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_CB_DestIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_CB_DestIP.Location = new System.Drawing.Point(394, 163);
            this.txt_CB_DestIP.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.txt_CB_DestIP.Name = "txt_CB_DestIP";
            this.txt_CB_DestIP.Size = new System.Drawing.Size(381, 24);
            this.txt_CB_DestIP.TabIndex = 58;
            // 
            // lblInstant
            // 
            this.lblInstant.AutoSize = true;
            this.lblInstant.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblInstant.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInstant.Location = new System.Drawing.Point(5, 48);
            this.lblInstant.Name = "lblInstant";
            this.lblInstant.Size = new System.Drawing.Size(381, 21);
            this.lblInstant.TabIndex = 39;
            this.lblInstant.Text = "Instant Profile";
            this.lblInstant.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txt_Instant_DestIP
            // 
            this.txt_Instant_DestIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_Instant_DestIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_Instant_DestIP.Location = new System.Drawing.Point(394, 48);
            this.txt_Instant_DestIP.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.txt_Instant_DestIP.Name = "txt_Instant_DestIP";
            this.txt_Instant_DestIP.Size = new System.Drawing.Size(381, 24);
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
            this.cbInstant_Frequency.Location = new System.Drawing.Point(783, 48);
            this.cbInstant_Frequency.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.cbInstant_Frequency.Name = "cbInstant_Frequency";
            this.cbInstant_Frequency.Size = new System.Drawing.Size(381, 25);
            this.cbInstant_Frequency.TabIndex = 41;
            this.cbInstant_Frequency.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cbInstant_Frequency_DrawItem);
            // 
            // lblAlert
            // 
            this.lblAlert.AutoSize = true;
            this.lblAlert.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAlert.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAlert.Location = new System.Drawing.Point(5, 25);
            this.lblAlert.Name = "lblAlert";
            this.lblAlert.Size = new System.Drawing.Size(381, 21);
            this.lblAlert.TabIndex = 36;
            this.lblAlert.Text = "Alert Profile";
            this.lblAlert.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txt_Alert_DestIP
            // 
            this.txt_Alert_DestIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_Alert_DestIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_Alert_DestIP.Location = new System.Drawing.Point(394, 25);
            this.txt_Alert_DestIP.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.txt_Alert_DestIP.Name = "txt_Alert_DestIP";
            this.txt_Alert_DestIP.Size = new System.Drawing.Size(381, 24);
            this.txt_Alert_DestIP.TabIndex = 43;
            // 
            // lblBillingProfile
            // 
            this.lblBillingProfile.AutoSize = true;
            this.lblBillingProfile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBillingProfile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBillingProfile.Location = new System.Drawing.Point(5, 71);
            this.lblBillingProfile.Name = "lblBillingProfile";
            this.lblBillingProfile.Size = new System.Drawing.Size(381, 21);
            this.lblBillingProfile.TabIndex = 33;
            this.lblBillingProfile.Text = "Billing Profile";
            this.lblBillingProfile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txt_Bill_DestIP
            // 
            this.txt_Bill_DestIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_Bill_DestIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_Bill_DestIP.Location = new System.Drawing.Point(394, 71);
            this.txt_Bill_DestIP.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.txt_Bill_DestIP.Name = "txt_Bill_DestIP";
            this.txt_Bill_DestIP.Size = new System.Drawing.Size(381, 24);
            this.txt_Bill_DestIP.TabIndex = 46;
            // 
            // lblSRProfile
            // 
            this.lblSRProfile.AutoSize = true;
            this.lblSRProfile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSRProfile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSRProfile.Location = new System.Drawing.Point(5, 140);
            this.lblSRProfile.Name = "lblSRProfile";
            this.lblSRProfile.Size = new System.Drawing.Size(381, 21);
            this.lblSRProfile.TabIndex = 30;
            this.lblSRProfile.Text = "Self Registration Profile";
            this.lblSRProfile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txt_SR_DestIP
            // 
            this.txt_SR_DestIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_SR_DestIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_SR_DestIP.Location = new System.Drawing.Point(394, 140);
            this.txt_SR_DestIP.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.txt_SR_DestIP.Name = "txt_SR_DestIP";
            this.txt_SR_DestIP.Size = new System.Drawing.Size(381, 24);
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
            this.cb_SR_Frequency.Location = new System.Drawing.Point(783, 140);
            this.cb_SR_Frequency.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.cb_SR_Frequency.Name = "cb_SR_Frequency";
            this.cb_SR_Frequency.Size = new System.Drawing.Size(381, 25);
            this.cb_SR_Frequency.TabIndex = 50;
            this.cb_SR_Frequency.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cbInstant_Frequency_DrawItem);
            // 
            // lblDEProfile
            // 
            this.lblDEProfile.AutoSize = true;
            this.lblDEProfile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDEProfile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDEProfile.Location = new System.Drawing.Point(5, 94);
            this.lblDEProfile.Name = "lblDEProfile";
            this.lblDEProfile.Size = new System.Drawing.Size(381, 21);
            this.lblDEProfile.TabIndex = 27;
            this.lblDEProfile.Text = "Daily Energy Profile";
            this.lblDEProfile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txt_DE_DestIP
            // 
            this.txt_DE_DestIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_DE_DestIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_DE_DestIP.Location = new System.Drawing.Point(394, 94);
            this.txt_DE_DestIP.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.txt_DE_DestIP.Name = "txt_DE_DestIP";
            this.txt_DE_DestIP.Size = new System.Drawing.Size(381, 24);
            this.txt_DE_DestIP.TabIndex = 52;
            // 
            // cb_DE_Frequency
            // 
            this.cb_DE_Frequency.BackColor = System.Drawing.SystemColors.Window;
            this.cb_DE_Frequency.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb_DE_Frequency.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cb_DE_Frequency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_DE_Frequency.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_DE_Frequency.FormattingEnabled = true;
            this.cb_DE_Frequency.Items.AddRange(new object[] {
            "24 Hour",
            "Disabled"});
            this.cb_DE_Frequency.Location = new System.Drawing.Point(783, 94);
            this.cb_DE_Frequency.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.cb_DE_Frequency.Name = "cb_DE_Frequency";
            this.cb_DE_Frequency.Size = new System.Drawing.Size(381, 25);
            this.cb_DE_Frequency.TabIndex = 53;
            this.cb_DE_Frequency.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cbInstant_Frequency_DrawItem);
            // 
            // lblLSProfile
            // 
            this.lblLSProfile.AutoSize = true;
            this.lblLSProfile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLSProfile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLSProfile.Location = new System.Drawing.Point(5, 117);
            this.lblLSProfile.Name = "lblLSProfile";
            this.lblLSProfile.Size = new System.Drawing.Size(381, 21);
            this.lblLSProfile.TabIndex = 24;
            this.lblLSProfile.Text = "Load Survey Profile";
            this.lblLSProfile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txt_LS_DestIP
            // 
            this.txt_LS_DestIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_LS_DestIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_LS_DestIP.Location = new System.Drawing.Point(394, 117);
            this.txt_LS_DestIP.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.txt_LS_DestIP.Name = "txt_LS_DestIP";
            this.txt_LS_DestIP.Size = new System.Drawing.Size(381, 24);
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
            this.cb_LS_Frequency.Location = new System.Drawing.Point(783, 117);
            this.cb_LS_Frequency.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.cb_LS_Frequency.Name = "cb_LS_Frequency";
            this.cb_LS_Frequency.Size = new System.Drawing.Size(381, 25);
            this.cb_LS_Frequency.TabIndex = 56;
            this.cb_LS_Frequency.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cbInstant_Frequency_DrawItem);
            // 
            // lblProfileHeader
            // 
            this.lblProfileHeader.AutoSize = true;
            this.lblProfileHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblProfileHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProfileHeader.Location = new System.Drawing.Point(5, 2);
            this.lblProfileHeader.Name = "lblProfileHeader";
            this.lblProfileHeader.Size = new System.Drawing.Size(381, 21);
            this.lblProfileHeader.TabIndex = 0;
            this.lblProfileHeader.Text = "Profiles";
            this.lblProfileHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.cb_CB_Frequency.Location = new System.Drawing.Point(783, 163);
            this.cb_CB_Frequency.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.cb_CB_Frequency.Name = "cb_CB_Frequency";
            this.cb_CB_Frequency.Size = new System.Drawing.Size(381, 25);
            this.cb_CB_Frequency.TabIndex = 59;
            this.cb_CB_Frequency.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cbInstant_Frequency_DrawItem);
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.ColumnCount = 2;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel8.Controls.Add(this.cb_Bill_Frequency, 0, 0);
            this.tableLayoutPanel8.Controls.Add(this.txtBillFreq, 1, 0);
            this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel8.Location = new System.Drawing.Point(780, 71);
            this.tableLayoutPanel8.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 1;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(387, 21);
            this.tableLayoutPanel8.TabIndex = 69;
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
            this.cb_Bill_Frequency.Size = new System.Drawing.Size(148, 25);
            this.cb_Bill_Frequency.TabIndex = 47;
            this.cb_Bill_Frequency.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cbInstant_Frequency_DrawItem);
            this.cb_Bill_Frequency.SelectedIndexChanged += new System.EventHandler(this.cb_Bill_Frequency_SelectedIndexChanged);
            // 
            // txtBillFreq
            // 
            this.txtBillFreq.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBillFreq.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBillFreq.Location = new System.Drawing.Point(157, 2);
            this.txtBillFreq.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtBillFreq.Mask = "00/\\*/\\* 00:00:00";
            this.txtBillFreq.Name = "txtBillFreq";
            this.txtBillFreq.Size = new System.Drawing.Size(227, 25);
            this.txtBillFreq.TabIndex = 70;
            this.txtBillFreq.Leave += new System.EventHandler(this.txtBillFreq_Leave);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.lblSelectProfile);
            this.flowLayoutPanel1.Controls.Add(this.cbTestProfileType);
            this.flowLayoutPanel1.Controls.Add(this.btnGet_PS_AS);
            this.flowLayoutPanel1.Controls.Add(this.btnSet_PS_AS);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(10, 33);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(0, 5, 0, 10);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1558, 44);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // lblSelectProfile
            // 
            this.lblSelectProfile.AutoSize = true;
            this.lblSelectProfile.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblSelectProfile.Location = new System.Drawing.Point(3, 15);
            this.lblSelectProfile.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.lblSelectProfile.Name = "lblSelectProfile";
            this.lblSelectProfile.Size = new System.Drawing.Size(104, 20);
            this.lblSelectProfile.TabIndex = 0;
            this.lblSelectProfile.Text = "Select Profile:";
            // 
            // cbTestProfileType
            // 
            this.cbTestProfileType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTestProfileType.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.cbTestProfileType.FormattingEnabled = true;
            this.cbTestProfileType.Items.AddRange(new object[] {
            "All",
            "Alert",
            "Instant",
            "LS",
            "DE",
            "SR",
            "Bill",
            "Current Bill",
            "Tamper"});
            this.cbTestProfileType.Location = new System.Drawing.Point(113, 10);
            this.cbTestProfileType.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.cbTestProfileType.Name = "cbTestProfileType";
            this.cbTestProfileType.Size = new System.Drawing.Size(250, 28);
            this.cbTestProfileType.TabIndex = 1;
            this.cbTestProfileType.SelectedIndexChanged += new System.EventHandler(this.cbTestProfileType_SelectedIndexChanged);
            // 
            // btnGet_PS_AS
            // 
            this.btnGet_PS_AS.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(94)))), ((int)(((byte)(168)))));
            this.btnGet_PS_AS.FlatAppearance.BorderColor = System.Drawing.Color.Purple;
            this.btnGet_PS_AS.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SteelBlue;
            this.btnGet_PS_AS.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGet_PS_AS.ForeColor = System.Drawing.Color.White;
            this.btnGet_PS_AS.Location = new System.Drawing.Point(369, 7);
            this.btnGet_PS_AS.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnGet_PS_AS.Name = "btnGet_PS_AS";
            this.btnGet_PS_AS.Size = new System.Drawing.Size(151, 31);
            this.btnGet_PS_AS.TabIndex = 74;
            this.btnGet_PS_AS.Text = "GET";
            this.btnGet_PS_AS.UseVisualStyleBackColor = false;
            this.btnGet_PS_AS.Click += new System.EventHandler(this.btnGet_PS_AS_Click);
            // 
            // btnSet_PS_AS
            // 
            this.btnSet_PS_AS.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(134)))), ((int)(((byte)(52)))));
            this.btnSet_PS_AS.FlatAppearance.BorderColor = System.Drawing.Color.Purple;
            this.btnSet_PS_AS.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SteelBlue;
            this.btnSet_PS_AS.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSet_PS_AS.ForeColor = System.Drawing.SystemColors.WindowText;
            this.btnSet_PS_AS.Location = new System.Drawing.Point(526, 7);
            this.btnSet_PS_AS.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSet_PS_AS.Name = "btnSet_PS_AS";
            this.btnSet_PS_AS.Size = new System.Drawing.Size(151, 31);
            this.btnSet_PS_AS.TabIndex = 75;
            this.btnSet_PS_AS.Text = "SET";
            this.btnSet_PS_AS.UseVisualStyleBackColor = false;
            this.btnSet_PS_AS.Click += new System.EventHandler(this.btnSet_PS_AS_Click);
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.White;
            this.pnlHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlHeader.Controls.Add(this.flowPanelButtons);
            this.pnlHeader.Controls.Add(this.lblHeader);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Padding = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.pnlHeader.Size = new System.Drawing.Size(1600, 104);
            this.pnlHeader.TabIndex = 0;
            // 
            // flowPanelButtons
            // 
            this.flowPanelButtons.AutoScroll = true;
            this.flowPanelButtons.Controls.Add(this.btnPushprofileSettings);
            this.flowPanelButtons.Controls.Add(this.pnlLoggingMode);
            this.flowPanelButtons.Controls.Add(this.btnNotifyDecryptSettings);
            this.flowPanelButtons.Controls.Add(this.btnNotificationType);
            this.flowPanelButtons.Controls.Add(this.btnStartListener);
            this.flowPanelButtons.Controls.Add(this.btnStopListener);
            this.flowPanelButtons.Controls.Add(this.btnClearLogs);
            this.flowPanelButtons.Controls.Add(this.btnSaveData);
            this.flowPanelButtons.Controls.Add(this.btnRawData);
            this.flowPanelButtons.Controls.Add(this.btnSpecificOBIS);
            this.flowPanelButtons.Controls.Add(this.btnCommSettings);
            this.flowPanelButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowPanelButtons.Location = new System.Drawing.Point(0, 45);
            this.flowPanelButtons.Name = "flowPanelButtons";
            this.flowPanelButtons.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.flowPanelButtons.Size = new System.Drawing.Size(1598, 47);
            this.flowPanelButtons.TabIndex = 1;
            // 
            // btnPushprofileSettings
            // 
            this.btnPushprofileSettings.BackColor = System.Drawing.Color.LightGray;
            this.btnPushprofileSettings.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(213)))), ((int)(((byte)(219)))));
            this.btnPushprofileSettings.FlatAppearance.BorderSize = 2;
            this.btnPushprofileSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPushprofileSettings.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnPushprofileSettings.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnPushprofileSettings.Location = new System.Drawing.Point(3, 8);
            this.btnPushprofileSettings.Name = "btnPushprofileSettings";
            this.btnPushprofileSettings.Size = new System.Drawing.Size(160, 35);
            this.btnPushprofileSettings.TabIndex = 0;
            this.btnPushprofileSettings.Text = "▼ Show Profile Settings";
            this.btnPushprofileSettings.UseVisualStyleBackColor = false;
            this.btnPushprofileSettings.Click += new System.EventHandler(this.btnPushprofileSettings_Click);
            // 
            // pnlLoggingMode
            // 
            this.pnlLoggingMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.pnlLoggingMode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlLoggingMode.Controls.Add(this.rbBtnUserDefinedLog);
            this.pnlLoggingMode.Controls.Add(this.rbBtnDetailLog);
            this.pnlLoggingMode.Location = new System.Drawing.Point(169, 8);
            this.pnlLoggingMode.Name = "pnlLoggingMode";
            this.pnlLoggingMode.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.pnlLoggingMode.Size = new System.Drawing.Size(236, 35);
            this.pnlLoggingMode.TabIndex = 1;
            // 
            // rbBtnUserDefinedLog
            // 
            this.rbBtnUserDefinedLog.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbBtnUserDefinedLog.BackColor = System.Drawing.Color.Transparent;
            this.rbBtnUserDefinedLog.Dock = System.Windows.Forms.DockStyle.Right;
            this.rbBtnUserDefinedLog.FlatAppearance.BorderSize = 0;
            this.rbBtnUserDefinedLog.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(194)))), ((int)(((byte)(168)))));
            this.rbBtnUserDefinedLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbBtnUserDefinedLog.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.rbBtnUserDefinedLog.Location = new System.Drawing.Point(117, 3);
            this.rbBtnUserDefinedLog.Name = "rbBtnUserDefinedLog";
            this.rbBtnUserDefinedLog.Size = new System.Drawing.Size(112, 27);
            this.rbBtnUserDefinedLog.TabIndex = 1;
            this.rbBtnUserDefinedLog.Text = "User Defined";
            this.rbBtnUserDefinedLog.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbBtnUserDefinedLog.UseVisualStyleBackColor = false;
            this.rbBtnUserDefinedLog.CheckedChanged += new System.EventHandler(this.rbBtnDetailLog_CheckedChanged);
            // 
            // rbBtnDetailLog
            // 
            this.rbBtnDetailLog.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbBtnDetailLog.BackColor = System.Drawing.Color.Transparent;
            this.rbBtnDetailLog.Checked = true;
            this.rbBtnDetailLog.Dock = System.Windows.Forms.DockStyle.Left;
            this.rbBtnDetailLog.FlatAppearance.BorderSize = 0;
            this.rbBtnDetailLog.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(194)))), ((int)(((byte)(168)))));
            this.rbBtnDetailLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbBtnDetailLog.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.rbBtnDetailLog.Location = new System.Drawing.Point(5, 3);
            this.rbBtnDetailLog.Name = "rbBtnDetailLog";
            this.rbBtnDetailLog.Size = new System.Drawing.Size(106, 27);
            this.rbBtnDetailLog.TabIndex = 0;
            this.rbBtnDetailLog.TabStop = true;
            this.rbBtnDetailLog.Text = "Detailed Log";
            this.rbBtnDetailLog.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbBtnDetailLog.UseVisualStyleBackColor = false;
            this.rbBtnDetailLog.CheckedChanged += new System.EventHandler(this.rbBtnDetailLog_CheckedChanged);
            // 
            // btnNotifyDecryptSettings
            // 
            this.btnNotifyDecryptSettings.BackColor = System.Drawing.Color.LightGray;
            this.btnNotifyDecryptSettings.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(213)))), ((int)(((byte)(219)))));
            this.btnNotifyDecryptSettings.FlatAppearance.BorderSize = 2;
            this.btnNotifyDecryptSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNotifyDecryptSettings.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnNotifyDecryptSettings.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnNotifyDecryptSettings.Location = new System.Drawing.Point(411, 8);
            this.btnNotifyDecryptSettings.Name = "btnNotifyDecryptSettings";
            this.btnNotifyDecryptSettings.Size = new System.Drawing.Size(151, 35);
            this.btnNotifyDecryptSettings.TabIndex = 2;
            this.btnNotifyDecryptSettings.Text = "Notification Settings ▼";
            this.btnNotifyDecryptSettings.UseVisualStyleBackColor = false;
            this.btnNotifyDecryptSettings.Click += new System.EventHandler(this.btnNotifyDecryptSettings_Click);
            // 
            // btnNotificationType
            // 
            this.btnNotificationType.BackColor = System.Drawing.Color.LightGray;
            this.btnNotificationType.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(213)))), ((int)(((byte)(219)))));
            this.btnNotificationType.FlatAppearance.BorderSize = 2;
            this.btnNotificationType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNotificationType.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnNotificationType.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnNotificationType.Location = new System.Drawing.Point(568, 8);
            this.btnNotificationType.Name = "btnNotificationType";
            this.btnNotificationType.Size = new System.Drawing.Size(112, 35);
            this.btnNotificationType.TabIndex = 3;
            this.btnNotificationType.Text = "Log Format ▼";
            this.btnNotificationType.UseVisualStyleBackColor = false;
            this.btnNotificationType.Click += new System.EventHandler(this.btnNotificationType_Click);
            // 
            // btnStartListener
            // 
            this.btnStartListener.BackColor = System.Drawing.Color.LightGray;
            this.btnStartListener.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.btnStartListener.FlatAppearance.BorderSize = 2;
            this.btnStartListener.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartListener.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnStartListener.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnStartListener.Location = new System.Drawing.Point(686, 8);
            this.btnStartListener.Name = "btnStartListener";
            this.btnStartListener.Size = new System.Drawing.Size(75, 35);
            this.btnStartListener.TabIndex = 4;
            this.btnStartListener.Text = "▶ Start";
            this.btnStartListener.UseVisualStyleBackColor = false;
            this.btnStartListener.Click += new System.EventHandler(this.btnStartListener_Click);
            // 
            // btnStopListener
            // 
            this.btnStopListener.BackColor = System.Drawing.Color.LightGray;
            this.btnStopListener.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.btnStopListener.FlatAppearance.BorderSize = 2;
            this.btnStopListener.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStopListener.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnStopListener.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnStopListener.Location = new System.Drawing.Point(767, 8);
            this.btnStopListener.Name = "btnStopListener";
            this.btnStopListener.Size = new System.Drawing.Size(80, 35);
            this.btnStopListener.TabIndex = 5;
            this.btnStopListener.Text = "⏹ Stop";
            this.btnStopListener.UseVisualStyleBackColor = false;
            this.btnStopListener.Click += new System.EventHandler(this.btnStopListener_Click);
            // 
            // btnClearLogs
            // 
            this.btnClearLogs.BackColor = System.Drawing.Color.LightGray;
            this.btnClearLogs.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(213)))), ((int)(((byte)(219)))));
            this.btnClearLogs.FlatAppearance.BorderSize = 2;
            this.btnClearLogs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearLogs.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnClearLogs.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnClearLogs.Location = new System.Drawing.Point(853, 8);
            this.btnClearLogs.Name = "btnClearLogs";
            this.btnClearLogs.Size = new System.Drawing.Size(83, 35);
            this.btnClearLogs.TabIndex = 6;
            this.btnClearLogs.Text = "Clear Logs";
            this.btnClearLogs.UseVisualStyleBackColor = false;
            this.btnClearLogs.Click += new System.EventHandler(this.btnClearLogs_Click);
            // 
            // btnSaveData
            // 
            this.btnSaveData.BackColor = System.Drawing.Color.LightGray;
            this.btnSaveData.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(213)))), ((int)(((byte)(219)))));
            this.btnSaveData.FlatAppearance.BorderSize = 2;
            this.btnSaveData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveData.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSaveData.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSaveData.Location = new System.Drawing.Point(942, 8);
            this.btnSaveData.Name = "btnSaveData";
            this.btnSaveData.Size = new System.Drawing.Size(90, 35);
            this.btnSaveData.TabIndex = 7;
            this.btnSaveData.Text = "Save Data";
            this.btnSaveData.UseVisualStyleBackColor = false;
            this.btnSaveData.Click += new System.EventHandler(this.btnSaveData_Click);
            // 
            // btnRawData
            // 
            this.btnRawData.BackColor = System.Drawing.Color.LightGray;
            this.btnRawData.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(213)))), ((int)(((byte)(219)))));
            this.btnRawData.FlatAppearance.BorderSize = 2;
            this.btnRawData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRawData.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnRawData.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnRawData.Location = new System.Drawing.Point(1038, 8);
            this.btnRawData.Name = "btnRawData";
            this.btnRawData.Size = new System.Drawing.Size(114, 35);
            this.btnRawData.TabIndex = 8;
            this.btnRawData.Text = "Raw Data View";
            this.btnRawData.UseVisualStyleBackColor = false;
            this.btnRawData.Click += new System.EventHandler(this.btnRawData_Click);
            // 
            // btnSpecificOBIS
            // 
            this.btnSpecificOBIS.BackColor = System.Drawing.Color.LightGray;
            this.btnSpecificOBIS.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(213)))), ((int)(((byte)(219)))));
            this.btnSpecificOBIS.FlatAppearance.BorderSize = 2;
            this.btnSpecificOBIS.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSpecificOBIS.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSpecificOBIS.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSpecificOBIS.Location = new System.Drawing.Point(1158, 8);
            this.btnSpecificOBIS.Name = "btnSpecificOBIS";
            this.btnSpecificOBIS.Size = new System.Drawing.Size(137, 35);
            this.btnSpecificOBIS.TabIndex = 9;
            this.btnSpecificOBIS.Text = "Update Profile OBIS";
            this.btnSpecificOBIS.UseVisualStyleBackColor = false;
            this.btnSpecificOBIS.Click += new System.EventHandler(this.btnSpecificOBIS_Click);
            // 
            // btnCommSettings
            // 
            this.btnCommSettings.BackColor = System.Drawing.Color.LightGray;
            this.btnCommSettings.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(213)))), ((int)(((byte)(219)))));
            this.btnCommSettings.FlatAppearance.BorderSize = 2;
            this.btnCommSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCommSettings.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnCommSettings.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnCommSettings.Location = new System.Drawing.Point(1301, 8);
            this.btnCommSettings.Name = "btnCommSettings";
            this.btnCommSettings.Size = new System.Drawing.Size(121, 35);
            this.btnCommSettings.TabIndex = 10;
            this.btnCommSettings.Text = "COMM Settings";
            this.btnCommSettings.UseVisualStyleBackColor = false;
            this.btnCommSettings.Click += new System.EventHandler(this.btnCommSettings_Click);
            // 
            // lblHeader
            // 
            this.lblHeader.BackColor = System.Drawing.Color.Transparent;
            this.lblHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(94)))), ((int)(((byte)(168)))));
            this.lblHeader.Location = new System.Drawing.Point(0, 10);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(1598, 35);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "Push Settings and Notifications";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // splitLoadSurvey
            // 
            this.splitLoadSurvey.Location = new System.Drawing.Point(0, 0);
            this.splitLoadSurvey.Name = "splitLoadSurvey";
            this.splitLoadSurvey.Size = new System.Drawing.Size(150, 100);
            this.splitLoadSurvey.TabIndex = 0;
            // 
            // dgvLoadSurvey
            // 
            this.dgvLoadSurvey.ColumnHeadersHeight = 29;
            this.dgvLoadSurvey.Location = new System.Drawing.Point(0, 0);
            this.dgvLoadSurvey.Name = "dgvLoadSurvey";
            this.dgvLoadSurvey.RowHeadersWidth = 51;
            this.dgvLoadSurvey.Size = new System.Drawing.Size(240, 150);
            this.dgvLoadSurvey.TabIndex = 0;
            // 
            // chartLoadSurvey
            // 
            this.chartLoadSurvey.Location = new System.Drawing.Point(0, 0);
            this.chartLoadSurvey.Name = "chartLoadSurvey";
            this.chartLoadSurvey.Size = new System.Drawing.Size(300, 300);
            this.chartLoadSurvey.TabIndex = 0;
            // 
            // splitDailyEnergy
            // 
            this.splitDailyEnergy.Location = new System.Drawing.Point(0, 0);
            this.splitDailyEnergy.Name = "splitDailyEnergy";
            this.splitDailyEnergy.Size = new System.Drawing.Size(150, 100);
            this.splitDailyEnergy.TabIndex = 0;
            // 
            // dgvDailyEnergy
            // 
            this.dgvDailyEnergy.ColumnHeadersHeight = 29;
            this.dgvDailyEnergy.Location = new System.Drawing.Point(0, 0);
            this.dgvDailyEnergy.Name = "dgvDailyEnergy";
            this.dgvDailyEnergy.RowHeadersWidth = 51;
            this.dgvDailyEnergy.Size = new System.Drawing.Size(240, 150);
            this.dgvDailyEnergy.TabIndex = 0;
            // 
            // chartDailyEnergy
            // 
            this.chartDailyEnergy.Location = new System.Drawing.Point(0, 0);
            this.chartDailyEnergy.Name = "chartDailyEnergy";
            this.chartDailyEnergy.Size = new System.Drawing.Size(300, 300);
            this.chartDailyEnergy.TabIndex = 0;
            // 
            // splitSelfReg
            // 
            this.splitSelfReg.Location = new System.Drawing.Point(0, 0);
            this.splitSelfReg.Name = "splitSelfReg";
            this.splitSelfReg.Size = new System.Drawing.Size(150, 100);
            this.splitSelfReg.TabIndex = 0;
            // 
            // dgvSelfReg
            // 
            this.dgvSelfReg.ColumnHeadersHeight = 29;
            this.dgvSelfReg.Location = new System.Drawing.Point(0, 0);
            this.dgvSelfReg.Name = "dgvSelfReg";
            this.dgvSelfReg.RowHeadersWidth = 51;
            this.dgvSelfReg.Size = new System.Drawing.Size(240, 150);
            this.dgvSelfReg.TabIndex = 0;
            // 
            // chartSelfReg
            // 
            this.chartSelfReg.Location = new System.Drawing.Point(0, 0);
            this.chartSelfReg.Name = "chartSelfReg";
            this.chartSelfReg.Size = new System.Drawing.Size(300, 300);
            this.chartSelfReg.TabIndex = 0;
            // 
            // splitBilling
            // 
            this.splitBilling.Location = new System.Drawing.Point(0, 0);
            this.splitBilling.Name = "splitBilling";
            this.splitBilling.Size = new System.Drawing.Size(150, 100);
            this.splitBilling.TabIndex = 0;
            // 
            // dgvBilling
            // 
            this.dgvBilling.ColumnHeadersHeight = 29;
            this.dgvBilling.Location = new System.Drawing.Point(0, 0);
            this.dgvBilling.Name = "dgvBilling";
            this.dgvBilling.RowHeadersWidth = 51;
            this.dgvBilling.Size = new System.Drawing.Size(240, 150);
            this.dgvBilling.TabIndex = 0;
            // 
            // chartBilling
            // 
            this.chartBilling.Location = new System.Drawing.Point(0, 0);
            this.chartBilling.Name = "chartBilling";
            this.chartBilling.Size = new System.Drawing.Size(300, 300);
            this.chartBilling.TabIndex = 0;
            // 
            // splitCurrentBill
            // 
            this.splitCurrentBill.Location = new System.Drawing.Point(0, 0);
            this.splitCurrentBill.Name = "splitCurrentBill";
            this.splitCurrentBill.Size = new System.Drawing.Size(150, 100);
            this.splitCurrentBill.TabIndex = 0;
            // 
            // dgvCurrentBill
            // 
            this.dgvCurrentBill.ColumnHeadersHeight = 29;
            this.dgvCurrentBill.Location = new System.Drawing.Point(0, 0);
            this.dgvCurrentBill.Name = "dgvCurrentBill";
            this.dgvCurrentBill.RowHeadersWidth = 51;
            this.dgvCurrentBill.Size = new System.Drawing.Size(240, 150);
            this.dgvCurrentBill.TabIndex = 0;
            // 
            // chartCurrentBill
            // 
            this.chartCurrentBill.Location = new System.Drawing.Point(0, 0);
            this.chartCurrentBill.Name = "chartCurrentBill";
            this.chartCurrentBill.Size = new System.Drawing.Size(300, 300);
            this.chartCurrentBill.TabIndex = 0;
            // 
            // splitAlert
            // 
            this.splitAlert.Location = new System.Drawing.Point(0, 0);
            this.splitAlert.Name = "splitAlert";
            this.splitAlert.Size = new System.Drawing.Size(150, 100);
            this.splitAlert.TabIndex = 0;
            // 
            // dgvAlert
            // 
            this.dgvAlert.ColumnHeadersHeight = 29;
            this.dgvAlert.Location = new System.Drawing.Point(0, 0);
            this.dgvAlert.Name = "dgvAlert";
            this.dgvAlert.RowHeadersWidth = 51;
            this.dgvAlert.Size = new System.Drawing.Size(240, 150);
            this.dgvAlert.TabIndex = 0;
            // 
            // chartAlert
            // 
            this.chartAlert.Location = new System.Drawing.Point(0, 0);
            this.chartAlert.Name = "chartAlert";
            this.chartAlert.Size = new System.Drawing.Size(300, 300);
            this.chartAlert.TabIndex = 0;
            // 
            // splitTamper
            // 
            this.splitTamper.Location = new System.Drawing.Point(0, 0);
            this.splitTamper.Name = "splitTamper";
            this.splitTamper.Size = new System.Drawing.Size(150, 100);
            this.splitTamper.TabIndex = 0;
            // 
            // dgvTamper
            // 
            this.dgvTamper.ColumnHeadersHeight = 29;
            this.dgvTamper.Location = new System.Drawing.Point(0, 0);
            this.dgvTamper.Name = "dgvTamper";
            this.dgvTamper.RowHeadersWidth = 51;
            this.dgvTamper.Size = new System.Drawing.Size(240, 150);
            this.dgvTamper.TabIndex = 0;
            // 
            // chartTamper
            // 
            this.chartTamper.Location = new System.Drawing.Point(0, 0);
            this.chartTamper.Name = "chartTamper";
            this.chartTamper.Size = new System.Drawing.Size(300, 300);
            this.chartTamper.TabIndex = 0;
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
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(248)))), ((int)(((byte)(250)))));
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar,
            this.toolStripStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 874);
            this.statusStrip1.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1600, 26);
            this.statusStrip1.TabIndex = 17;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar
            // 
            this.toolStripProgressBar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripProgressBar.Name = "toolStripProgressBar";
            this.toolStripProgressBar.Size = new System.Drawing.Size(233, 18);
            this.toolStripProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.toolStripProgressBar.Visible = false;
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(94)))), ((int)(((byte)(168)))));
            this.toolStripStatusLabel.Margin = new System.Windows.Forms.Padding(1, 4, 1, 4);
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(56, 18);
            this.toolStripStatusLabel.Text = "Status";
            // 
            // ListenerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoScrollMargin = new System.Drawing.Size(0, 600);
            this.ClientSize = new System.Drawing.Size(1600, 900);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.pnlMain);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "ListenerForm";
            this.Text = "DLMS Push Listener Dashboard";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.ListenerForm_Load);
            this.pnlMain.ResumeLayout(false);
            this.splitConMain.Panel1.ResumeLayout(false);
            this.splitConMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitConMain)).EndInit();
            this.splitConMain.ResumeLayout(false);
            this.pnlLogs.ResumeLayout(false);
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
            this.pnlProfileSettings.ResumeLayout(false);
            this.grpPushObjects.ResumeLayout(false);
            this.splitPushObjects.Panel1.ResumeLayout(false);
            this.splitPushObjects.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitPushObjects)).EndInit();
            this.splitPushObjects.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DGPushProfile)).EndInit();
            this.grpProfileConfig.ResumeLayout(false);
            this.tblProfileSettings.ResumeLayout(false);
            this.tblProfileSettings.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tableLayoutPanel8.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.pnlHeader.ResumeLayout(false);
            this.flowPanelButtons.ResumeLayout(false);
            this.pnlLoggingMode.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitLoadSurvey)).EndInit();
            this.splitLoadSurvey.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLoadSurvey)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartLoadSurvey)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitDailyEnergy)).EndInit();
            this.splitDailyEnergy.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDailyEnergy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartDailyEnergy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitSelfReg)).EndInit();
            this.splitSelfReg.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelfReg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartSelfReg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitBilling)).EndInit();
            this.splitBilling.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBilling)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartBilling)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitCurrentBill)).EndInit();
            this.splitCurrentBill.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCurrentBill)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartCurrentBill)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitAlert)).EndInit();
            this.splitAlert.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAlert)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartAlert)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitTamper)).EndInit();
            this.splitTamper.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTamper)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartTamper)).EndInit();
            this.cmsNotificationOption.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.FlowLayoutPanel flowPanelButtons;
        private System.Windows.Forms.Button btnPushprofileSettings;
        private System.Windows.Forms.Panel pnlLoggingMode;
        private System.Windows.Forms.RadioButton rbBtnDetailLog;
        private System.Windows.Forms.RadioButton rbBtnUserDefinedLog;
        private System.Windows.Forms.Button btnNotifyDecryptSettings;
        private System.Windows.Forms.Button btnNotificationType;
        private System.Windows.Forms.Button btnStartListener;
        private System.Windows.Forms.Button btnStopListener;
        private System.Windows.Forms.Button btnClearLogs;
        private System.Windows.Forms.Button btnSaveData;
        private System.Windows.Forms.Button btnRawData;
        private System.Windows.Forms.Button btnSpecificOBIS;
        private System.Windows.Forms.Button btnCommSettings;
        private System.Windows.Forms.Panel pnlProfileSettings;
        private System.Windows.Forms.GroupBox grpProfileConfig;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label lblSelectProfile;
        private System.Windows.Forms.ComboBox cbTestProfileType;
        private System.Windows.Forms.GroupBox grpPushObjects;
        private System.Windows.Forms.SplitContainer splitPushObjects;
        private System.Windows.Forms.TreeView tvPushObjects;
        private System.Windows.Forms.SplitContainer splitConMain;
        private System.Windows.Forms.Panel pnlLogs;
        private System.Windows.Forms.Label lblLogsHeader;
        private System.Windows.Forms.Panel PanelProfileandRawData;
        private System.Windows.Forms.TabControl tabControlProfiles;
        private System.Windows.Forms.Label lblDataHeader;
        private System.Windows.Forms.TabPage tbpInstant;
        private System.Windows.Forms.TabPage tbpLS;
        private System.Windows.Forms.SplitContainer splitLoadSurvey;
        private System.Windows.Forms.DataGridView dgvLoadSurvey;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartLoadSurvey;
        private System.Windows.Forms.TabPage tbpDE;
        private System.Windows.Forms.SplitContainer splitDailyEnergy;
        private System.Windows.Forms.DataGridView dgvDailyEnergy;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartDailyEnergy;
        private System.Windows.Forms.TabPage tbpSR;
        private System.Windows.Forms.SplitContainer splitSelfReg;
        private System.Windows.Forms.DataGridView dgvSelfReg;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartSelfReg;
        private System.Windows.Forms.TabPage tbpBill;
        private System.Windows.Forms.SplitContainer splitBilling;
        private System.Windows.Forms.DataGridView dgvBilling;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartBilling;
        private System.Windows.Forms.TabPage tbpCB;
        private System.Windows.Forms.SplitContainer splitCurrentBill;
        private System.Windows.Forms.DataGridView dgvCurrentBill;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartCurrentBill;
        private System.Windows.Forms.TabPage tbpAlert;
        private System.Windows.Forms.SplitContainer splitAlert;
        private System.Windows.Forms.DataGridView dgvAlert;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartAlert;
        private System.Windows.Forms.TabPage tbpTamper;
        private System.Windows.Forms.SplitContainer splitTamper;
        private System.Windows.Forms.DataGridView dgvTamper;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartTamper;
        private TableLayoutPanel tblProfileSettings;
        private TextBox txt_Tamper_DestIP;
        private TextBox txt_Random_Tamper;
        private ComboBox cb_Tamper_Frequency;
        private Label lblTamperProfile;
        private TableLayoutPanel tableLayoutPanel1;
        private Label lblRandomHeader;
        private CheckBox chkRandomisation;
        private TableLayoutPanel tableLayoutPanel5;
        private CheckBox chkFreq;
        private Label lblPushFreqHeader;
        private TableLayoutPanel tableLayoutPanel6;
        private Label lblDestIPHeader;
        private CheckBox chkDestination;
        private TextBox txt_Random_CB;
        private TextBox txt_Random_LS;
        private TextBox txt_Random_DE;
        private TextBox txt_Random_SR;
        private TextBox txt_Random_Bill;
        private TextBox txt_Random_Instant;
        private Label lblCBProfile;
        private TextBox txt_CB_DestIP;
        private Label lblInstant;
        private TextBox txt_Instant_DestIP;
        private ComboBox cbInstant_Frequency;
        private Label lblAlert;
        private TextBox txt_Alert_DestIP;
        private Label lblBillingProfile;
        private TextBox txt_Bill_DestIP;
        private Label lblSRProfile;
        private TextBox txt_SR_DestIP;
        private ComboBox cb_SR_Frequency;
        private Label lblDEProfile;
        private TextBox txt_DE_DestIP;
        private ComboBox cb_DE_Frequency;
        private Label lblLSProfile;
        private TextBox txt_LS_DestIP;
        private ComboBox cb_LS_Frequency;
        private Label lblProfileHeader;
        private ComboBox cb_CB_Frequency;
        private TableLayoutPanel tableLayoutPanel8;
        private ComboBox cb_Bill_Frequency;
        private MaskedTextBox txtBillFreq;
        private SplitContainer splitCon_LS;
        private DataGridView dgLS;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_LS;
        private SplitContainer splitCon_DE;
        private DataGridView dgDE;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_DE;
        private SplitContainer splitCon_SR;
        private DataGridView dgSR;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_SR;
        private SplitContainer splitCon_Bill;
        private DataGridView dgBill;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_Bill;
        private SplitContainer splitCon_CB;
        private DataGridView dgCB;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_CB;
        private SplitContainer splitCon_Alert;
        private DataGridView dgAlert;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_Alert;
        private SplitContainer splitCon_Tamper;
        private DataGridView dgTamper;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_tamper;
        private SplitContainer splitcon_InstantTab;
        private DataGridView dgInstant;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_Instant;
        private DataGridView dgRawData;
        private ContextMenuStrip cmsNotificationOption;
        private ToolStripMenuItem cbHexCiphered;
        private ToolStripMenuItem cbHexDecrypted;
        private ToolStripMenuItem cbXML;
        private Button btnGet_PS_AS;
        private Button btnSet_PS_AS;
        private RichTextBox rtbPushLogs;
        private DataGridView DGPushProfile;
        private StatusStrip statusStrip1;
        private ToolStripProgressBar toolStripProgressBar;
        private ToolStripStatusLabel toolStripStatusLabel;
    }
}
