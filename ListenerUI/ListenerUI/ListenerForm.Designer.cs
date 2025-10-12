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
            this.lblHeader = new System.Windows.Forms.Label();
            this.grpProfileConfig = new System.Windows.Forms.GroupBox();
            this.tblProfileSettings = new System.Windows.Forms.TableLayoutPanel();
            this.grpLogs = new System.Windows.Forms.GroupBox();
            this.rtbLogs = new System.Windows.Forms.RichTextBox();
            this.grpProfileData = new System.Windows.Forms.GroupBox();
            this.tabProfiles = new System.Windows.Forms.TabControl();
            this.flowButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnStartListener = new System.Windows.Forms.Button();
            this.btnStopListener = new System.Windows.Forms.Button();
            this.btnClearLogs = new System.Windows.Forms.Button();
            this.btnSaveData = new System.Windows.Forms.Button();
            this.tblMain.SuspendLayout();
            this.grpProfileConfig.SuspendLayout();
            this.grpLogs.SuspendLayout();
            this.grpProfileData.SuspendLayout();
            this.flowButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // tblMain
            // 
            this.tblMain.ColumnCount = 1;
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblMain.Controls.Add(this.lblHeader, 0, 0);
            this.tblMain.Controls.Add(this.grpProfileConfig, 0, 1);
            this.tblMain.Controls.Add(this.grpLogs, 0, 2);
            this.tblMain.Controls.Add(this.grpProfileData, 0, 3);
            this.tblMain.Controls.Add(this.flowButtons, 0, 4);
            this.tblMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblMain.Location = new System.Drawing.Point(0, 0);
            this.tblMain.Name = "tblMain";
            this.tblMain.RowCount = 5;
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 262F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 42.76923F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 57.23077F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tblMain.Size = new System.Drawing.Size(1693, 922);
            this.tblMain.TabIndex = 0;
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
            // grpProfileConfig
            // 
            this.grpProfileConfig.Controls.Add(this.tblProfileSettings);
            this.grpProfileConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpProfileConfig.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.grpProfileConfig.Location = new System.Drawing.Point(10, 45);
            this.grpProfileConfig.Margin = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.grpProfileConfig.Name = "grpProfileConfig";
            this.grpProfileConfig.Size = new System.Drawing.Size(1673, 252);
            this.grpProfileConfig.TabIndex = 1;
            this.grpProfileConfig.TabStop = false;
            this.grpProfileConfig.Text = "Profile Configuration";
            // 
            // tblProfileSettings
            // 
            this.tblProfileSettings.ColumnCount = 3;
            this.tblProfileSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 255F));
            this.tblProfileSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblProfileSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblProfileSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblProfileSettings.Location = new System.Drawing.Point(3, 26);
            this.tblProfileSettings.Name = "tblProfileSettings";
            this.tblProfileSettings.RowCount = 7;
            this.tblProfileSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tblProfileSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tblProfileSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tblProfileSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tblProfileSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tblProfileSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tblProfileSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tblProfileSettings.Size = new System.Drawing.Size(1667, 223);
            this.tblProfileSettings.TabIndex = 0;
            // 
            // grpLogs
            // 
            this.grpLogs.Controls.Add(this.rtbLogs);
            this.grpLogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpLogs.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.grpLogs.Location = new System.Drawing.Point(10, 307);
            this.grpLogs.Margin = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.grpLogs.Name = "grpLogs";
            this.grpLogs.Size = new System.Drawing.Size(1673, 233);
            this.grpLogs.TabIndex = 2;
            this.grpLogs.TabStop = false;
            this.grpLogs.Text = "Push Packet Line Traffic";
            // 
            // rtbLogs
            // 
            this.rtbLogs.BackColor = System.Drawing.Color.White;
            this.rtbLogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbLogs.Font = new System.Drawing.Font("Consolas", 10F);
            this.rtbLogs.ForeColor = System.Drawing.Color.Transparent;
            this.rtbLogs.Location = new System.Drawing.Point(3, 26);
            this.rtbLogs.Name = "rtbLogs";
            this.rtbLogs.ReadOnly = true;
            this.rtbLogs.Size = new System.Drawing.Size(1667, 204);
            this.rtbLogs.TabIndex = 0;
            this.rtbLogs.Text = "";
            // 
            // grpProfileData
            // 
            this.grpProfileData.Controls.Add(this.tabProfiles);
            this.grpProfileData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpProfileData.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.grpProfileData.Location = new System.Drawing.Point(10, 550);
            this.grpProfileData.Margin = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.grpProfileData.Name = "grpProfileData";
            this.grpProfileData.Size = new System.Drawing.Size(1673, 316);
            this.grpProfileData.TabIndex = 3;
            this.grpProfileData.TabStop = false;
            this.grpProfileData.Text = "Profile Data";
            // 
            // tabProfiles
            // 
            this.tabProfiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabProfiles.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tabProfiles.Location = new System.Drawing.Point(3, 26);
            this.tabProfiles.Name = "tabProfiles";
            this.tabProfiles.SelectedIndex = 0;
            this.tabProfiles.Size = new System.Drawing.Size(1667, 287);
            this.tabProfiles.TabIndex = 0;
            // 
            // flowButtons
            // 
            this.flowButtons.Controls.Add(this.btnStartListener);
            this.flowButtons.Controls.Add(this.btnStopListener);
            this.flowButtons.Controls.Add(this.btnClearLogs);
            this.flowButtons.Controls.Add(this.btnSaveData);
            this.flowButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowButtons.Location = new System.Drawing.Point(10, 876);
            this.flowButtons.Margin = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.flowButtons.Name = "flowButtons";
            this.flowButtons.Size = new System.Drawing.Size(1673, 41);
            this.flowButtons.TabIndex = 4;
            // 
            // btnStartListener
            // 
            this.btnStartListener.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btnStartListener.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartListener.Location = new System.Drawing.Point(1571, 3);
            this.btnStartListener.Name = "btnStartListener";
            this.btnStartListener.Size = new System.Drawing.Size(99, 31);
            this.btnStartListener.TabIndex = 0;
            this.btnStartListener.Text = "▶ Start Listening";
            this.btnStartListener.UseVisualStyleBackColor = false;
            this.btnStartListener.Click += new System.EventHandler(this.btnStartListener_Click);
            // 
            // btnStopListener
            // 
            this.btnStopListener.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btnStopListener.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStopListener.Location = new System.Drawing.Point(1466, 3);
            this.btnStopListener.Name = "btnStopListener";
            this.btnStopListener.Size = new System.Drawing.Size(99, 31);
            this.btnStopListener.TabIndex = 1;
            this.btnStopListener.Text = "⏹ Stop Listening";
            this.btnStopListener.UseVisualStyleBackColor = false;
            this.btnStopListener.Click += new System.EventHandler(this.btnStopListener_Click);
            // 
            // btnClearLogs
            // 
            this.btnClearLogs.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btnClearLogs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearLogs.Location = new System.Drawing.Point(1361, 3);
            this.btnClearLogs.Name = "btnClearLogs";
            this.btnClearLogs.Size = new System.Drawing.Size(99, 31);
            this.btnClearLogs.TabIndex = 2;
            this.btnClearLogs.Text = "🧹 Clear Logs";
            this.btnClearLogs.UseVisualStyleBackColor = false;
            this.btnClearLogs.Click += new System.EventHandler(this.btnClearLogs_Click);
            // 
            // btnSaveData
            // 
            this.btnSaveData.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btnSaveData.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveData.Location = new System.Drawing.Point(1256, 3);
            this.btnSaveData.Name = "btnSaveData";
            this.btnSaveData.Size = new System.Drawing.Size(99, 31);
            this.btnSaveData.TabIndex = 3;
            this.btnSaveData.Text = "💾 Save Data";
            this.btnSaveData.UseVisualStyleBackColor = false;
            this.btnSaveData.Click += new System.EventHandler(this.btnSaveData_Click);
            // 
            // ListenerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1693, 922);
            this.Controls.Add(this.tblMain);
            this.Name = "ListenerForm";
            this.Text = "DLMS Push Listener UI";
            this.tblMain.ResumeLayout(false);
            this.tblMain.PerformLayout();
            this.grpProfileConfig.ResumeLayout(false);
            this.grpLogs.ResumeLayout(false);
            this.grpProfileData.ResumeLayout(false);
            this.flowButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tblMain;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.GroupBox grpProfileConfig;
        private System.Windows.Forms.TableLayoutPanel tblProfileSettings;
        private System.Windows.Forms.GroupBox grpLogs;
        private System.Windows.Forms.RichTextBox rtbLogs;
        private System.Windows.Forms.GroupBox grpProfileData;
        private System.Windows.Forms.TabControl tabProfiles;
        private System.Windows.Forms.FlowLayoutPanel flowButtons;
        private System.Windows.Forms.Button btnStartListener;
        private System.Windows.Forms.Button btnStopListener;
        private System.Windows.Forms.Button btnClearLogs;
        private System.Windows.Forms.Button btnSaveData;
    }
}
