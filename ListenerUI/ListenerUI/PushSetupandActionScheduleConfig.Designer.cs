namespace MeterReader.DLMSInterfaceClasses
{
    partial class PushSetupandActionScheduleConfig
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.ComboBox cmbObjectType;
        private System.Windows.Forms.ComboBox cmbObis;
        private System.Windows.Forms.TextBox txtClassId;
        private System.Windows.Forms.TextBox txtCurrentObis;
        private System.Windows.Forms.TextBox txtNewObis;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnReset;

        private System.Windows.Forms.Label lblObjectType;
        private System.Windows.Forms.Label lblObis;
        private System.Windows.Forms.Label lblClassId;
        private System.Windows.Forms.Label lblCurrentObis;
        private System.Windows.Forms.Label lblNewObis;
        private System.Windows.Forms.Label lblTitle;

        private System.Windows.Forms.GroupBox grpObjectSelection;
        private System.Windows.Forms.GroupBox grpObisConfiguration;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Panel pnlMain;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.cmbObjectType = new System.Windows.Forms.ComboBox();
            this.cmbObis = new System.Windows.Forms.ComboBox();
            this.txtClassId = new System.Windows.Forms.TextBox();
            this.txtCurrentObis = new System.Windows.Forms.TextBox();
            this.txtNewObis = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.lblObjectType = new System.Windows.Forms.Label();
            this.lblObis = new System.Windows.Forms.Label();
            this.lblClassId = new System.Windows.Forms.Label();
            this.lblCurrentObis = new System.Windows.Forms.Label();
            this.lblNewObis = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.grpObjectSelection = new System.Windows.Forms.GroupBox();
            this.grpObisConfiguration = new System.Windows.Forms.GroupBox();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.grpObjectSelection.SuspendLayout();
            this.grpObisConfiguration.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbObjectType
            // 
            this.cmbObjectType.BackColor = System.Drawing.SystemColors.Window;
            this.cmbObjectType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbObjectType.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbObjectType.FormattingEnabled = true;
            this.cmbObjectType.Location = new System.Drawing.Point(239, 37);
            this.cmbObjectType.Name = "cmbObjectType";
            this.cmbObjectType.Size = new System.Drawing.Size(391, 25);
            this.cmbObjectType.TabIndex = 1;
            this.cmbObjectType.SelectedIndexChanged += new System.EventHandler(this.cmbObjectType_SelectedIndexChanged);
            // 
            // cmbObis
            // 
            this.cmbObis.BackColor = System.Drawing.SystemColors.Window;
            this.cmbObis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbObis.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbObis.FormattingEnabled = true;
            this.cmbObis.Location = new System.Drawing.Point(239, 87);
            this.cmbObis.Name = "cmbObis";
            this.cmbObis.Size = new System.Drawing.Size(391, 25);
            this.cmbObis.TabIndex = 3;
            this.cmbObis.SelectedIndexChanged += new System.EventHandler(this.cmbObis_SelectedIndexChanged);
            // 
            // txtClassId
            // 
            this.txtClassId.BackColor = System.Drawing.SystemColors.Window;
            this.txtClassId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtClassId.Font = new System.Drawing.Font("Consolas", 10F);
            this.txtClassId.Location = new System.Drawing.Point(239, 37);
            this.txtClassId.Name = "txtClassId";
            this.txtClassId.ReadOnly = true;
            this.txtClassId.Size = new System.Drawing.Size(391, 23);
            this.txtClassId.TabIndex = 1;
            // 
            // txtCurrentObis
            // 
            this.txtCurrentObis.BackColor = System.Drawing.SystemColors.Window;
            this.txtCurrentObis.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCurrentObis.Font = new System.Drawing.Font("Consolas", 10F);
            this.txtCurrentObis.Location = new System.Drawing.Point(239, 82);
            this.txtCurrentObis.Name = "txtCurrentObis";
            this.txtCurrentObis.ReadOnly = true;
            this.txtCurrentObis.Size = new System.Drawing.Size(391, 23);
            this.txtCurrentObis.TabIndex = 3;
            // 
            // txtNewObis
            // 
            this.txtNewObis.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNewObis.Font = new System.Drawing.Font("Consolas", 10F);
            this.txtNewObis.Location = new System.Drawing.Point(239, 127);
            this.txtNewObis.Name = "txtNewObis";
            this.txtNewObis.Size = new System.Drawing.Size(391, 23);
            this.txtNewObis.TabIndex = 5;
            this.txtNewObis.TextChanged += new System.EventHandler(this.txtNewObis_TextChanged);
            this.txtNewObis.Leave += new System.EventHandler(this.txtNewObis_Leave);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(94)))), ((int)(((byte)(168)))));
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(545, 7);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(130, 35);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "💾 Save Mapping";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.White;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.btnCancel.Location = new System.Drawing.Point(419, 7);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(120, 35);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "✖ Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnReset
            // 
            this.btnReset.BackColor = System.Drawing.Color.White;
            this.btnReset.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReset.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.btnReset.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.btnReset.Location = new System.Drawing.Point(24, 7);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(120, 35);
            this.btnReset.TabIndex = 2;
            this.btnReset.Text = "🔄 Reset";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // lblObjectType
            // 
            this.lblObjectType.AutoSize = true;
            this.lblObjectType.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblObjectType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.lblObjectType.Location = new System.Drawing.Point(20, 40);
            this.lblObjectType.Name = "lblObjectType";
            this.lblObjectType.Size = new System.Drawing.Size(80, 17);
            this.lblObjectType.TabIndex = 0;
            this.lblObjectType.Text = "Object Type:";
            // 
            // lblObis
            // 
            this.lblObis.AutoSize = true;
            this.lblObis.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblObis.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.lblObis.Location = new System.Drawing.Point(20, 90);
            this.lblObis.Name = "lblObis";
            this.lblObis.Size = new System.Drawing.Size(179, 17);
            this.lblObis.TabIndex = 2;
            this.lblObis.Text = "Standard OBIS && Description:";
            // 
            // lblClassId
            // 
            this.lblClassId.AutoSize = true;
            this.lblClassId.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblClassId.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.lblClassId.Location = new System.Drawing.Point(20, 40);
            this.lblClassId.Name = "lblClassId";
            this.lblClassId.Size = new System.Drawing.Size(99, 17);
            this.lblClassId.TabIndex = 0;
            this.lblClassId.Text = "Class ID (Fixed):";
            // 
            // lblCurrentObis
            // 
            this.lblCurrentObis.AutoSize = true;
            this.lblCurrentObis.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblCurrentObis.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.lblCurrentObis.Location = new System.Drawing.Point(20, 85);
            this.lblCurrentObis.Name = "lblCurrentObis";
            this.lblCurrentObis.Size = new System.Drawing.Size(177, 17);
            this.lblCurrentObis.TabIndex = 2;
            this.lblCurrentObis.Text = "Current OBIS (Logical Name):";
            // 
            // lblNewObis
            // 
            this.lblNewObis.AutoSize = true;
            this.lblNewObis.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblNewObis.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(204)))));
            this.lblNewObis.Location = new System.Drawing.Point(20, 130);
            this.lblNewObis.Name = "lblNewObis";
            this.lblNewObis.Size = new System.Drawing.Size(139, 17);
            this.lblNewObis.TabIndex = 4;
            this.lblNewObis.Text = "New OBIS (Override):";
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(702, 34);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "DLMS OBIS Configuration";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grpObjectSelection
            // 
            this.grpObjectSelection.BackColor = System.Drawing.Color.White;
            this.grpObjectSelection.Controls.Add(this.lblObjectType);
            this.grpObjectSelection.Controls.Add(this.cmbObjectType);
            this.grpObjectSelection.Controls.Add(this.lblObis);
            this.grpObjectSelection.Controls.Add(this.cmbObis);
            this.grpObjectSelection.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.grpObjectSelection.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.grpObjectSelection.Location = new System.Drawing.Point(25, 17);
            this.grpObjectSelection.Name = "grpObjectSelection";
            this.grpObjectSelection.Padding = new System.Windows.Forms.Padding(15);
            this.grpObjectSelection.Size = new System.Drawing.Size(650, 136);
            this.grpObjectSelection.TabIndex = 0;
            this.grpObjectSelection.TabStop = false;
            this.grpObjectSelection.Text = "Object Selection";
            // 
            // grpObisConfiguration
            // 
            this.grpObisConfiguration.BackColor = System.Drawing.Color.White;
            this.grpObisConfiguration.Controls.Add(this.lblClassId);
            this.grpObisConfiguration.Controls.Add(this.txtClassId);
            this.grpObisConfiguration.Controls.Add(this.lblCurrentObis);
            this.grpObisConfiguration.Controls.Add(this.txtCurrentObis);
            this.grpObisConfiguration.Controls.Add(this.lblNewObis);
            this.grpObisConfiguration.Controls.Add(this.txtNewObis);
            this.grpObisConfiguration.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.grpObisConfiguration.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.grpObisConfiguration.Location = new System.Drawing.Point(25, 168);
            this.grpObisConfiguration.Name = "grpObisConfiguration";
            this.grpObisConfiguration.Padding = new System.Windows.Forms.Padding(15);
            this.grpObisConfiguration.Size = new System.Drawing.Size(650, 175);
            this.grpObisConfiguration.TabIndex = 1;
            this.grpObisConfiguration.TabStop = false;
            this.grpObisConfiguration.Text = "OBIS Configuration";
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(94)))), ((int)(((byte)(168)))));
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(702, 34);
            this.pnlHeader.TabIndex = 0;
            // 
            // pnlFooter
            // 
            this.pnlFooter.BackColor = System.Drawing.Color.White;
            this.pnlFooter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlFooter.Controls.Add(this.btnReset);
            this.pnlFooter.Controls.Add(this.btnCancel);
            this.pnlFooter.Controls.Add(this.btnSave);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 410);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(702, 51);
            this.pnlFooter.TabIndex = 2;
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.pnlMain.Controls.Add(this.grpObjectSelection);
            this.pnlMain.Controls.Add(this.grpObisConfiguration);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 34);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(25, 20, 25, 20);
            this.pnlMain.Size = new System.Drawing.Size(702, 376);
            this.pnlMain.TabIndex = 1;
            // 
            // PushSetupandActionScheduleConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(702, 461);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(718, 500);
            this.MinimumSize = new System.Drawing.Size(718, 500);
            this.Name = "PushSetupandActionScheduleConfig";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DLMS Push & Action Schedule Configuration";
            this.grpObjectSelection.ResumeLayout(false);
            this.grpObjectSelection.PerformLayout();
            this.grpObisConfiguration.ResumeLayout(false);
            this.grpObisConfiguration.PerformLayout();
            this.pnlHeader.ResumeLayout(false);
            this.pnlFooter.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }
    }
}
