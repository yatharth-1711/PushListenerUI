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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.grpProfileConfig = new System.Windows.Forms.GroupBox();
            this.tblProfileSettings = new System.Windows.Forms.TableLayoutPanel();
            this.comboBox7 = new System.Windows.Forms.ComboBox();
            this.comboBox6 = new System.Windows.Forms.ComboBox();
            this.comboBox5 = new System.Windows.Forms.ComboBox();
            this.comboBox4 = new System.Windows.Forms.ComboBox();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.grpLogs = new System.Windows.Forms.GroupBox();
            this.rtbLogs = new System.Windows.Forms.RichTextBox();
            this.grpProfileData = new System.Windows.Forms.GroupBox();
            this.tabProfiles = new System.Windows.Forms.TabControl();
            this.flowButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnStartListener = new System.Windows.Forms.Button();
            this.btnStopListener = new System.Windows.Forms.Button();
            this.btnClearLogs = new System.Windows.Forms.Button();
            this.btnSaveData = new System.Windows.Forms.Button();
            this.richTextBox8 = new System.Windows.Forms.RichTextBox();
            this.richTextBox9 = new System.Windows.Forms.RichTextBox();
            this.richTextBox10 = new System.Windows.Forms.RichTextBox();
            this.richTextBox11 = new System.Windows.Forms.RichTextBox();
            this.richTextBox12 = new System.Windows.Forms.RichTextBox();
            this.richTextBox13 = new System.Windows.Forms.RichTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.grpProfileConfig.SuspendLayout();
            this.tblProfileSettings.SuspendLayout();
            this.grpLogs.SuspendLayout();
            this.grpProfileData.SuspendLayout();
            this.flowButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.grpProfileConfig, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.grpLogs, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.grpProfileData, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.flowButtons, 0, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 262F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 42.76923F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 57.23077F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1693, 922);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Indigo;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1687, 40);
            this.label1.TabIndex = 0;
            this.label1.Text = "Push Settings and Notifications";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.tblProfileSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 242F));
            this.tblProfileSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblProfileSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblProfileSettings.Controls.Add(this.label8, 0, 6);
            this.tblProfileSettings.Controls.Add(this.label7, 0, 5);
            this.tblProfileSettings.Controls.Add(this.label6, 0, 4);
            this.tblProfileSettings.Controls.Add(this.label5, 0, 3);
            this.tblProfileSettings.Controls.Add(this.label4, 0, 2);
            this.tblProfileSettings.Controls.Add(this.label3, 0, 1);
            this.tblProfileSettings.Controls.Add(this.richTextBox8, 1, 1);
            this.tblProfileSettings.Controls.Add(this.richTextBox9, 1, 2);
            this.tblProfileSettings.Controls.Add(this.richTextBox10, 1, 3);
            this.tblProfileSettings.Controls.Add(this.richTextBox11, 1, 4);
            this.tblProfileSettings.Controls.Add(this.richTextBox12, 1, 5);
            this.tblProfileSettings.Controls.Add(this.richTextBox13, 1, 6);
            this.tblProfileSettings.Controls.Add(this.comboBox7, 2, 6);
            this.tblProfileSettings.Controls.Add(this.comboBox6, 2, 5);
            this.tblProfileSettings.Controls.Add(this.comboBox5, 2, 4);
            this.tblProfileSettings.Controls.Add(this.comboBox4, 2, 3);
            this.tblProfileSettings.Controls.Add(this.comboBox3, 2, 2);
            this.tblProfileSettings.Controls.Add(this.comboBox2, 2, 1);
            this.tblProfileSettings.Controls.Add(this.richTextBox1, 1, 0);
            this.tblProfileSettings.Controls.Add(this.label2, 0, 0);
            this.tblProfileSettings.Controls.Add(this.comboBox1, 2, 0);
            this.tblProfileSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblProfileSettings.Location = new System.Drawing.Point(3, 26);
            this.tblProfileSettings.Name = "tblProfileSettings";
            this.tblProfileSettings.RowCount = 7;
            this.tblProfileSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tblProfileSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28572F));
            this.tblProfileSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28572F));
            this.tblProfileSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28572F));
            this.tblProfileSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28572F));
            this.tblProfileSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28572F));
            this.tblProfileSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28572F));
            this.tblProfileSettings.Size = new System.Drawing.Size(1667, 223);
            this.tblProfileSettings.TabIndex = 0;
            // 
            // comboBox7
            // 
            this.comboBox7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox7.FormattingEnabled = true;
            this.comboBox7.Location = new System.Drawing.Point(957, 189);
            this.comboBox7.Name = "comboBox7";
            this.comboBox7.Size = new System.Drawing.Size(707, 31);
            this.comboBox7.TabIndex = 20;
            // 
            // comboBox6
            // 
            this.comboBox6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox6.FormattingEnabled = true;
            this.comboBox6.Location = new System.Drawing.Point(957, 158);
            this.comboBox6.Name = "comboBox6";
            this.comboBox6.Size = new System.Drawing.Size(707, 31);
            this.comboBox6.TabIndex = 17;
            // 
            // comboBox5
            // 
            this.comboBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox5.FormattingEnabled = true;
            this.comboBox5.Location = new System.Drawing.Point(957, 127);
            this.comboBox5.Name = "comboBox5";
            this.comboBox5.Size = new System.Drawing.Size(707, 31);
            this.comboBox5.TabIndex = 14;
            // 
            // comboBox4
            // 
            this.comboBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox4.FormattingEnabled = true;
            this.comboBox4.Location = new System.Drawing.Point(957, 96);
            this.comboBox4.Name = "comboBox4";
            this.comboBox4.Size = new System.Drawing.Size(707, 31);
            this.comboBox4.TabIndex = 11;
            // 
            // comboBox3
            // 
            this.comboBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Location = new System.Drawing.Point(957, 65);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(707, 31);
            this.comboBox3.TabIndex = 8;
            // 
            // comboBox2
            // 
            this.comboBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(957, 34);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(707, 31);
            this.comboBox2.TabIndex = 5;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(245, 3);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(706, 25);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(236, 31);
            this.label2.TabIndex = 0;
            this.label2.Text = "Instant Profile";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboBox1
            // 
            this.comboBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(957, 3);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(707, 31);
            this.comboBox1.TabIndex = 2;
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
            // 
            // richTextBox8
            // 
            this.richTextBox8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox8.Location = new System.Drawing.Point(245, 34);
            this.richTextBox8.Name = "richTextBox8";
            this.richTextBox8.Size = new System.Drawing.Size(706, 25);
            this.richTextBox8.TabIndex = 26;
            this.richTextBox8.Text = "";
            // 
            // richTextBox9
            // 
            this.richTextBox9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox9.Location = new System.Drawing.Point(245, 65);
            this.richTextBox9.Name = "richTextBox9";
            this.richTextBox9.Size = new System.Drawing.Size(706, 25);
            this.richTextBox9.TabIndex = 25;
            this.richTextBox9.Text = "";
            // 
            // richTextBox10
            // 
            this.richTextBox10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox10.Location = new System.Drawing.Point(245, 96);
            this.richTextBox10.Name = "richTextBox10";
            this.richTextBox10.Size = new System.Drawing.Size(706, 25);
            this.richTextBox10.TabIndex = 24;
            this.richTextBox10.Text = "";
            // 
            // richTextBox11
            // 
            this.richTextBox11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox11.Location = new System.Drawing.Point(245, 127);
            this.richTextBox11.Name = "richTextBox11";
            this.richTextBox11.Size = new System.Drawing.Size(706, 25);
            this.richTextBox11.TabIndex = 23;
            this.richTextBox11.Text = "";
            // 
            // richTextBox12
            // 
            this.richTextBox12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox12.Location = new System.Drawing.Point(245, 158);
            this.richTextBox12.Name = "richTextBox12";
            this.richTextBox12.Size = new System.Drawing.Size(706, 25);
            this.richTextBox12.TabIndex = 22;
            this.richTextBox12.Text = "";
            // 
            // richTextBox13
            // 
            this.richTextBox13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox13.Location = new System.Drawing.Point(245, 189);
            this.richTextBox13.Name = "richTextBox13";
            this.richTextBox13.Size = new System.Drawing.Size(706, 31);
            this.richTextBox13.TabIndex = 21;
            this.richTextBox13.Text = "";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(236, 31);
            this.label3.TabIndex = 27;
            this.label3.Text = "Alert Profile";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(3, 62);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(236, 31);
            this.label4.TabIndex = 28;
            this.label4.Text = "Daily Energy Profile";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(3, 93);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(236, 31);
            this.label5.TabIndex = 29;
            this.label5.Text = "Load Survey Profile";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(3, 124);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(236, 31);
            this.label6.TabIndex = 30;
            this.label6.Text = "Billing Profile";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Location = new System.Drawing.Point(3, 155);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(236, 31);
            this.label7.TabIndex = 31;
            this.label7.Text = "Self Registration Profile";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Location = new System.Drawing.Point(3, 186);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(236, 37);
            this.label8.TabIndex = 32;
            this.label8.Text = "Current Bill Profile";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ListenerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1693, 922);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ListenerForm";
            this.Text = "DLMS Push Listener UI";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.grpProfileConfig.ResumeLayout(false);
            this.tblProfileSettings.ResumeLayout(false);
            this.tblProfileSettings.PerformLayout();
            this.grpLogs.ResumeLayout(false);
            this.grpProfileData.ResumeLayout(false);
            this.flowButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
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
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox comboBox7;
        private System.Windows.Forms.ComboBox comboBox6;
        private System.Windows.Forms.ComboBox comboBox5;
        private System.Windows.Forms.ComboBox comboBox4;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RichTextBox richTextBox8;
        private System.Windows.Forms.RichTextBox richTextBox9;
        private System.Windows.Forms.RichTextBox richTextBox10;
        private System.Windows.Forms.RichTextBox richTextBox11;
        private System.Windows.Forms.RichTextBox richTextBox12;
        private System.Windows.Forms.RichTextBox richTextBox13;
    }
}
