namespace ListenerUI
{
    partial class FGComparisonForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnCompare = new System.Windows.Forms.Button();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnExportDifferences = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.splitContainerTreeView = new System.Windows.Forms.SplitContainer();
            this.pnlTreeViewA = new System.Windows.Forms.Panel();
            this.treeViewFG_A = new System.Windows.Forms.TreeView();
            this.lblFileA = new System.Windows.Forms.Label();
            this.pnlTreeViewB = new System.Windows.Forms.Panel();
            this.treeViewFG_B = new System.Windows.Forms.TreeView();
            this.lblFileB = new System.Windows.Forms.Label();
            this.splitContainerDataGrid = new System.Windows.Forms.SplitContainer();
            this.pnlDataGridA = new System.Windows.Forms.Panel();
            this.dataGridViewFG_A = new System.Windows.Forms.DataGridView();
            this.lblDataGridA = new System.Windows.Forms.Label();
            this.pnlDataGridB = new System.Windows.Forms.Panel();
            this.dataGridViewFG_B = new System.Windows.Forms.DataGridView();
            this.lblDataGridB = new System.Windows.Forms.Label();
            this.openFileDialogFG = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialogExport = new System.Windows.Forms.SaveFileDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.flowPanelButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.BtnImportFG = new System.Windows.Forms.Button();
            this.BtnClear = new System.Windows.Forms.Button();
            this.pnlFooter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTreeView)).BeginInit();
            this.splitContainerTreeView.Panel1.SuspendLayout();
            this.splitContainerTreeView.Panel2.SuspendLayout();
            this.splitContainerTreeView.SuspendLayout();
            this.pnlTreeViewA.SuspendLayout();
            this.pnlTreeViewB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerDataGrid)).BeginInit();
            this.splitContainerDataGrid.Panel1.SuspendLayout();
            this.splitContainerDataGrid.Panel2.SuspendLayout();
            this.splitContainerDataGrid.SuspendLayout();
            this.pnlDataGridA.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFG_A)).BeginInit();
            this.pnlDataGridB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFG_B)).BeginInit();
            this.panel1.SuspendLayout();
            this.flowPanelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCompare
            // 
            this.btnCompare.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCompare.BackColor = System.Drawing.Color.Transparent;
            this.btnCompare.FlatAppearance.BorderSize = 0;
            this.btnCompare.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCompare.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCompare.ForeColor = System.Drawing.Color.White;
            this.btnCompare.Location = new System.Drawing.Point(162, 8);
            this.btnCompare.Name = "btnCompare";
            this.btnCompare.Size = new System.Drawing.Size(144, 32);
            this.btnCompare.TabIndex = 4;
            this.btnCompare.Text = "⇄ Compare";
            this.btnCompare.UseVisualStyleBackColor = false;
            this.btnCompare.Click += new System.EventHandler(this.BtnCompare_Click);
            // 
            // pnlFooter
            // 
            this.pnlFooter.BackColor = System.Drawing.Color.White;
            this.pnlFooter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlFooter.Controls.Add(this.btnClose);
            this.pnlFooter.Controls.Add(this.btnExportDifferences);
            this.pnlFooter.Controls.Add(this.lblStatus);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 679);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(1554, 44);
            this.pnlFooter.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnClose.Location = new System.Drawing.Point(1446, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(85, 28);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnExportDifferences
            // 
            this.btnExportDifferences.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportDifferences.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.btnExportDifferences.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnExportDifferences.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportDifferences.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnExportDifferences.Location = new System.Drawing.Point(1284, 5);
            this.btnExportDifferences.Name = "btnExportDifferences";
            this.btnExportDifferences.Size = new System.Drawing.Size(145, 28);
            this.btnExportDifferences.TabIndex = 1;
            this.btnExportDifferences.Text = "Export Differences";
            this.btnExportDifferences.UseVisualStyleBackColor = false;
            this.btnExportDifferences.Click += new System.EventHandler(this.BtnExportDifferences_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.lblStatus.Location = new System.Drawing.Point(12, 9);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(50, 20);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "Ready";
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 47);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.splitContainerTreeView);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.splitContainerDataGrid);
            this.splitContainerMain.Size = new System.Drawing.Size(1554, 632);
            this.splitContainerMain.SplitterDistance = 175;
            this.splitContainerMain.TabIndex = 2;
            // 
            // splitContainerTreeView
            // 
            this.splitContainerTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerTreeView.Location = new System.Drawing.Point(0, 0);
            this.splitContainerTreeView.Name = "splitContainerTreeView";
            // 
            // splitContainerTreeView.Panel1
            // 
            this.splitContainerTreeView.Panel1.Controls.Add(this.pnlTreeViewA);
            // 
            // splitContainerTreeView.Panel2
            // 
            this.splitContainerTreeView.Panel2.Controls.Add(this.pnlTreeViewB);
            this.splitContainerTreeView.Size = new System.Drawing.Size(1554, 175);
            this.splitContainerTreeView.SplitterDistance = 774;
            this.splitContainerTreeView.TabIndex = 0;
            // 
            // pnlTreeViewA
            // 
            this.pnlTreeViewA.BackColor = System.Drawing.Color.White;
            this.pnlTreeViewA.Controls.Add(this.treeViewFG_A);
            this.pnlTreeViewA.Controls.Add(this.lblFileA);
            this.pnlTreeViewA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTreeViewA.Location = new System.Drawing.Point(0, 0);
            this.pnlTreeViewA.Name = "pnlTreeViewA";
            this.pnlTreeViewA.Size = new System.Drawing.Size(774, 175);
            this.pnlTreeViewA.TabIndex = 0;
            // 
            // treeViewFG_A
            // 
            this.treeViewFG_A.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeViewFG_A.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewFG_A.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.treeViewFG_A.Location = new System.Drawing.Point(0, 30);
            this.treeViewFG_A.Name = "treeViewFG_A";
            this.treeViewFG_A.Size = new System.Drawing.Size(774, 145);
            this.treeViewFG_A.TabIndex = 1;
            this.treeViewFG_A.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewFG_A_AfterSelect);
            // 
            // lblFileA
            // 
            this.lblFileA.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.lblFileA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblFileA.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblFileA.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblFileA.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lblFileA.Location = new System.Drawing.Point(0, 0);
            this.lblFileA.Name = "lblFileA";
            this.lblFileA.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.lblFileA.Size = new System.Drawing.Size(774, 30);
            this.lblFileA.TabIndex = 0;
            this.lblFileA.Text = "FG File A";
            this.lblFileA.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlTreeViewB
            // 
            this.pnlTreeViewB.BackColor = System.Drawing.Color.White;
            this.pnlTreeViewB.Controls.Add(this.treeViewFG_B);
            this.pnlTreeViewB.Controls.Add(this.lblFileB);
            this.pnlTreeViewB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTreeViewB.Location = new System.Drawing.Point(0, 0);
            this.pnlTreeViewB.Name = "pnlTreeViewB";
            this.pnlTreeViewB.Size = new System.Drawing.Size(776, 175);
            this.pnlTreeViewB.TabIndex = 0;
            // 
            // treeViewFG_B
            // 
            this.treeViewFG_B.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeViewFG_B.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewFG_B.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.treeViewFG_B.Location = new System.Drawing.Point(0, 30);
            this.treeViewFG_B.Name = "treeViewFG_B";
            this.treeViewFG_B.Size = new System.Drawing.Size(776, 145);
            this.treeViewFG_B.TabIndex = 1;
            this.treeViewFG_B.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewFG_B_AfterSelect);
            // 
            // lblFileB
            // 
            this.lblFileB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.lblFileB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblFileB.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblFileB.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblFileB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lblFileB.Location = new System.Drawing.Point(0, 0);
            this.lblFileB.Name = "lblFileB";
            this.lblFileB.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.lblFileB.Size = new System.Drawing.Size(776, 30);
            this.lblFileB.TabIndex = 0;
            this.lblFileB.Text = "FG File B";
            this.lblFileB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // splitContainerDataGrid
            // 
            this.splitContainerDataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerDataGrid.Location = new System.Drawing.Point(0, 0);
            this.splitContainerDataGrid.Name = "splitContainerDataGrid";
            // 
            // splitContainerDataGrid.Panel1
            // 
            this.splitContainerDataGrid.Panel1.Controls.Add(this.pnlDataGridA);
            // 
            // splitContainerDataGrid.Panel2
            // 
            this.splitContainerDataGrid.Panel2.Controls.Add(this.pnlDataGridB);
            this.splitContainerDataGrid.Size = new System.Drawing.Size(1554, 453);
            this.splitContainerDataGrid.SplitterDistance = 774;
            this.splitContainerDataGrid.TabIndex = 0;
            // 
            // pnlDataGridA
            // 
            this.pnlDataGridA.BackColor = System.Drawing.Color.White;
            this.pnlDataGridA.Controls.Add(this.dataGridViewFG_A);
            this.pnlDataGridA.Controls.Add(this.lblDataGridA);
            this.pnlDataGridA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDataGridA.Location = new System.Drawing.Point(0, 0);
            this.pnlDataGridA.Name = "pnlDataGridA";
            this.pnlDataGridA.Size = new System.Drawing.Size(774, 453);
            this.pnlDataGridA.TabIndex = 0;
            // 
            // dataGridViewFG_A
            // 
            this.dataGridViewFG_A.AllowUserToAddRows = false;
            this.dataGridViewFG_A.AllowUserToDeleteRows = false;
            this.dataGridViewFG_A.AllowUserToResizeRows = false;
            this.dataGridViewFG_A.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewFG_A.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewFG_A.ColumnHeadersHeight = 28;
            this.dataGridViewFG_A.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridViewFG_A.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewFG_A.Location = new System.Drawing.Point(0, 30);
            this.dataGridViewFG_A.MultiSelect = false;
            this.dataGridViewFG_A.Name = "dataGridViewFG_A";
            this.dataGridViewFG_A.ReadOnly = true;
            this.dataGridViewFG_A.RowHeadersVisible = false;
            this.dataGridViewFG_A.RowHeadersWidth = 51;
            this.dataGridViewFG_A.RowTemplate.Height = 26;
            this.dataGridViewFG_A.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewFG_A.Size = new System.Drawing.Size(774, 423);
            this.dataGridViewFG_A.TabIndex = 1;
            // 
            // lblDataGridA
            // 
            this.lblDataGridA.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.lblDataGridA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDataGridA.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDataGridA.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDataGridA.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lblDataGridA.Location = new System.Drawing.Point(0, 0);
            this.lblDataGridA.Name = "lblDataGridA";
            this.lblDataGridA.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.lblDataGridA.Size = new System.Drawing.Size(774, 30);
            this.lblDataGridA.TabIndex = 0;
            this.lblDataGridA.Text = "Parameters - File A";
            this.lblDataGridA.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlDataGridB
            // 
            this.pnlDataGridB.BackColor = System.Drawing.Color.White;
            this.pnlDataGridB.Controls.Add(this.dataGridViewFG_B);
            this.pnlDataGridB.Controls.Add(this.lblDataGridB);
            this.pnlDataGridB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDataGridB.Location = new System.Drawing.Point(0, 0);
            this.pnlDataGridB.Name = "pnlDataGridB";
            this.pnlDataGridB.Size = new System.Drawing.Size(776, 453);
            this.pnlDataGridB.TabIndex = 0;
            // 
            // dataGridViewFG_B
            // 
            this.dataGridViewFG_B.AllowUserToAddRows = false;
            this.dataGridViewFG_B.AllowUserToDeleteRows = false;
            this.dataGridViewFG_B.AllowUserToResizeRows = false;
            this.dataGridViewFG_B.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewFG_B.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewFG_B.ColumnHeadersHeight = 28;
            this.dataGridViewFG_B.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridViewFG_B.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewFG_B.Location = new System.Drawing.Point(0, 30);
            this.dataGridViewFG_B.MultiSelect = false;
            this.dataGridViewFG_B.Name = "dataGridViewFG_B";
            this.dataGridViewFG_B.ReadOnly = true;
            this.dataGridViewFG_B.RowHeadersVisible = false;
            this.dataGridViewFG_B.RowHeadersWidth = 51;
            this.dataGridViewFG_B.RowTemplate.Height = 26;
            this.dataGridViewFG_B.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewFG_B.Size = new System.Drawing.Size(776, 423);
            this.dataGridViewFG_B.TabIndex = 1;
            // 
            // lblDataGridB
            // 
            this.lblDataGridB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.lblDataGridB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDataGridB.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDataGridB.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDataGridB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lblDataGridB.Location = new System.Drawing.Point(0, 0);
            this.lblDataGridB.Name = "lblDataGridB";
            this.lblDataGridB.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.lblDataGridB.Size = new System.Drawing.Size(776, 30);
            this.lblDataGridB.TabIndex = 0;
            this.lblDataGridB.Text = "Parameters - File B";
            this.lblDataGridB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // openFileDialogFG
            // 
            this.openFileDialogFG.Filter = "FG Files (*.gfs;*.fg;*.xml)|*.gfs;*.fg;*.xml|All Files (*.*)|*.*";
            this.openFileDialogFG.Title = "Select FG File";
            // 
            // saveFileDialogExport
            // 
            this.saveFileDialogExport.DefaultExt = "txt";
            this.saveFileDialogExport.Filter = "Text Files (*.txt)|*.txt|CSV Files (*.csv)|*.csv|All Files (*.*)|*.*";
            this.saveFileDialogExport.Title = "Export Comparison Report";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.splitContainerMain);
            this.panel1.Controls.Add(this.pnlFooter);
            this.panel1.Controls.Add(this.flowPanelButtons);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1554, 723);
            this.panel1.TabIndex = 3;
            // 
            // flowPanelButtons
            // 
            this.flowPanelButtons.AutoScroll = true;
            this.flowPanelButtons.BackColor = System.Drawing.Color.White;
            this.flowPanelButtons.Controls.Add(this.BtnImportFG);
            this.flowPanelButtons.Controls.Add(this.btnCompare);
            this.flowPanelButtons.Controls.Add(this.BtnClear);
            this.flowPanelButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowPanelButtons.Location = new System.Drawing.Point(0, 0);
            this.flowPanelButtons.Name = "flowPanelButtons";
            this.flowPanelButtons.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.flowPanelButtons.Size = new System.Drawing.Size(1554, 47);
            this.flowPanelButtons.TabIndex = 3;
            // 
            // BtnImportFG
            // 
            this.BtnImportFG.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnImportFG.BackColor = System.Drawing.Color.Transparent;
            this.BtnImportFG.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.BtnImportFG.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.BtnImportFG.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnImportFG.Location = new System.Drawing.Point(3, 8);
            this.BtnImportFG.Name = "BtnImportFG";
            this.BtnImportFG.Size = new System.Drawing.Size(153, 32);
            this.BtnImportFG.TabIndex = 5;
            this.BtnImportFG.Text = "📂 Import FG";
            this.BtnImportFG.UseVisualStyleBackColor = false;
            this.BtnImportFG.Click += new System.EventHandler(this.BtnImportFG_Click);
            // 
            // BtnClear
            // 
            this.BtnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnClear.BackColor = System.Drawing.Color.Transparent;
            this.BtnClear.FlatAppearance.BorderSize = 0;
            this.BtnClear.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.BtnClear.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnClear.ForeColor = System.Drawing.Color.White;
            this.BtnClear.Location = new System.Drawing.Point(312, 8);
            this.BtnClear.Name = "BtnClear";
            this.BtnClear.Size = new System.Drawing.Size(150, 32);
            this.BtnClear.TabIndex = 6;
            this.BtnClear.Text = "Clear";
            this.BtnClear.UseVisualStyleBackColor = false;
            this.BtnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // FGComparisonForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1554, 723);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(1000, 600);
            this.Name = "FGComparisonForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FG Comparison Tool";
            this.Load += new System.EventHandler(this.FGComparisonForm_Load);
            this.pnlFooter.ResumeLayout(false);
            this.pnlFooter.PerformLayout();
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.splitContainerTreeView.Panel1.ResumeLayout(false);
            this.splitContainerTreeView.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTreeView)).EndInit();
            this.splitContainerTreeView.ResumeLayout(false);
            this.pnlTreeViewA.ResumeLayout(false);
            this.pnlTreeViewB.ResumeLayout(false);
            this.splitContainerDataGrid.Panel1.ResumeLayout(false);
            this.splitContainerDataGrid.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerDataGrid)).EndInit();
            this.splitContainerDataGrid.ResumeLayout(false);
            this.pnlDataGridA.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFG_A)).EndInit();
            this.pnlDataGridB.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFG_B)).EndInit();
            this.panel1.ResumeLayout(false);
            this.flowPanelButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnCompare;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnExportDifferences;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.SplitContainer splitContainerTreeView;
        private System.Windows.Forms.Panel pnlTreeViewA;
        private System.Windows.Forms.Label lblFileA;
        private System.Windows.Forms.TreeView treeViewFG_A;
        private System.Windows.Forms.Panel pnlTreeViewB;
        private System.Windows.Forms.Label lblFileB;
        private System.Windows.Forms.TreeView treeViewFG_B;
        private System.Windows.Forms.SplitContainer splitContainerDataGrid;
        private System.Windows.Forms.Panel pnlDataGridA;
        private System.Windows.Forms.Label lblDataGridA;
        private System.Windows.Forms.Panel pnlDataGridB;
        private System.Windows.Forms.Label lblDataGridB;
        private System.Windows.Forms.OpenFileDialog openFileDialogFG;
        private System.Windows.Forms.SaveFileDialog saveFileDialogExport;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dataGridViewFG_A;
        private System.Windows.Forms.DataGridView dataGridViewFG_B;
        private System.Windows.Forms.FlowLayoutPanel flowPanelButtons;
        private System.Windows.Forms.Button BtnImportFG;
        private System.Windows.Forms.Button BtnClear;
    }
}
