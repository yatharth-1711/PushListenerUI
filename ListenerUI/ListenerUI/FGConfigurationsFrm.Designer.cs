namespace MeterReader.HelperForms.FGConfigurations
{
    partial class FGConfigurationsFrm
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
            this.btnImportGFS = new System.Windows.Forms.Button();
            this.tvTables = new System.Windows.Forms.TreeView();
            this.dgData = new System.Windows.Forms.DataGridView();
            this.btnAddRow = new System.Windows.Forms.Button();
            this.btnDeleteRow = new System.Windows.Forms.Button();
            this.btnSaveXML = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.dgData)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnImportGFS
            // 
            this.btnImportGFS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnImportGFS.Location = new System.Drawing.Point(3, 3);
            this.btnImportGFS.Name = "btnImportGFS";
            this.btnImportGFS.Size = new System.Drawing.Size(331, 39);
            this.btnImportGFS.TabIndex = 0;
            this.btnImportGFS.Text = "Import GFS";
            this.btnImportGFS.UseVisualStyleBackColor = true;
            this.btnImportGFS.Click += new System.EventHandler(this.btnImportGFS_Click);
            // 
            // tvTables
            // 
            this.tvTables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvTables.Location = new System.Drawing.Point(3, 3);
            this.tvTables.Name = "tvTables";
            this.tvTables.Size = new System.Drawing.Size(443, 670);
            this.tvTables.TabIndex = 1;
            this.tvTables.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvTables_AfterSelect);
            // 
            // dgData
            // 
            this.dgData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgData.Location = new System.Drawing.Point(452, 3);
            this.dgData.Name = "dgData";
            this.dgData.RowHeadersWidth = 51;
            this.dgData.RowTemplate.Height = 24;
            this.dgData.Size = new System.Drawing.Size(893, 670);
            this.dgData.TabIndex = 2;
            // 
            // btnAddRow
            // 
            this.btnAddRow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAddRow.Location = new System.Drawing.Point(340, 3);
            this.btnAddRow.Name = "btnAddRow";
            this.btnAddRow.Size = new System.Drawing.Size(331, 39);
            this.btnAddRow.TabIndex = 3;
            this.btnAddRow.Text = "Add Row";
            this.btnAddRow.UseVisualStyleBackColor = true;
            this.btnAddRow.Click += new System.EventHandler(this.btnAddRow_Click);
            // 
            // btnDeleteRow
            // 
            this.btnDeleteRow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDeleteRow.Location = new System.Drawing.Point(677, 3);
            this.btnDeleteRow.Name = "btnDeleteRow";
            this.btnDeleteRow.Size = new System.Drawing.Size(331, 39);
            this.btnDeleteRow.TabIndex = 4;
            this.btnDeleteRow.Text = "Delete Row";
            this.btnDeleteRow.UseVisualStyleBackColor = true;
            this.btnDeleteRow.Click += new System.EventHandler(this.btnDeleteRow_Click);
            // 
            // btnSaveXML
            // 
            this.btnSaveXML.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSaveXML.Location = new System.Drawing.Point(1014, 3);
            this.btnSaveXML.Name = "btnSaveXML";
            this.btnSaveXML.Size = new System.Drawing.Size(331, 39);
            this.btnSaveXML.TabIndex = 5;
            this.btnSaveXML.Text = "Save XML";
            this.btnSaveXML.UseVisualStyleBackColor = true;
            this.btnSaveXML.Click += new System.EventHandler(this.btnSaveXML_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.btnImportGFS, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnSaveXML, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnAddRow, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnDeleteRow, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1348, 45);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 66.66666F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Controls.Add(this.tvTables, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.dgData, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 45);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1348, 676);
            this.tableLayoutPanel2.TabIndex = 7;
            // 
            // FGConfigurationsFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1348, 721);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FGConfigurationsFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FGConfigurationsFrm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.dgData)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnImportGFS;
        private System.Windows.Forms.TreeView tvTables;
        private System.Windows.Forms.DataGridView dgData;
        private System.Windows.Forms.Button btnAddRow;
        private System.Windows.Forms.Button btnDeleteRow;
        private System.Windows.Forms.Button btnSaveXML;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    }
}