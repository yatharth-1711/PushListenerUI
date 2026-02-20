using AutoTest.FrameWork.Converts;
using AutoTestDesktopWFA;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace ListenerUI
{
    public partial class FGComparisonForm : Form
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private Dictionary<string, FGParameter> fgDataA;
        private Dictionary<string, FGParameter> fgDataB;
        private DataSet importedDataSetA;
        private DataSet importedDataSetB;

        //NEW
        private int importedFileCount = 0;
        private string fgFilePathA = string.Empty;
        private string fgFilePathB = string.Empty;
        public static string ConnectionType { get; set; } = "";
        public static string AccuracyClass { get; set; } = "";
        public static string CategoryWCorCT { get; set; } = "";
        public static double Vref { get; set; } = 0.0;
        public static double Ib { get; set; } = 0.0;
        public static double Imax { get; set; } = 0.0;
        public FGComparisonForm()
        {
            InitializeComponent();
            InitializeState();
        }

        private void InitializeState()
        {
            lblStatus.Text = "Ready";
            lblFileA.Text = "FG File A";
            lblFileB.Text = "FG File B";
        }
        private void FGComparisonForm_Load(object sender, EventArgs e)
        {
            //UIStyler.StyleControl(dataGridViewFG_A);
            //UIStyler.StyleControl(dataGridViewFG_B);
        }

        #region Import FG
        private void BtnImportFG_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "GFS Files (*.gfs)|*.gfs",
                Multiselect = true
            };

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            if (ofd.FileNames.Length > 2)
            {
                MessageBox.Show(
                    "You can import maximum two FG files for comparison.",
                    "FG Import",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            foreach (string file in ofd.FileNames)
            {
                if (importedFileCount == 0)
                {
                    ImportGFSFile(file, treeViewFG_A, true);
                    fgFilePathA = file;
                    lblFileA.Text = Path.GetFileName(file);
                    importedFileCount++;
                }
                else if (importedFileCount == 1)
                {
                    ImportGFSFile(file, treeViewFG_B, false);
                    fgFilePathB = file;
                    lblFileB.Text = Path.GetFileName(file);
                    importedFileCount++;
                }
            }

            UpdateCompareState();
        }
        private void BtnCompare_Click(object sender, EventArgs e)
        {
            if (treeViewFG_A.SelectedNode == null || treeViewFG_B.SelectedNode == null)
            {
                MessageBox.Show("Please select a table in TreeView.", "Compare",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string tableName = treeViewFG_A.SelectedNode.Text;

            if (!importedDataSetA.Tables.Contains(tableName) ||
                !importedDataSetB.Tables.Contains(tableName))
            {
                MessageBox.Show("Selected table not available in both FG files.",
                    "Compare", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            CompareTables(
                importedDataSetA.Tables[tableName],
                importedDataSetB.Tables[tableName]);

            lblStatus.Text = $"Compared : {tableName}";
        }
        private void BtnClear_Click(object sender, EventArgs e)
        {
            ResetComparison();
        }
        private void treeViewFG_A_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SyncTreeViews(e.Node.Text, true);
        }
        private void treeViewFG_B_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SyncTreeViews(e.Node.Text, true);
        }
        private void BtnExportDifferences_Click(object sender, EventArgs e)
        {
            if (saveFileDialogExport.ShowDialog() != DialogResult.OK)
                return;

            using (StreamWriter writer = new StreamWriter(saveFileDialogExport.FileName))
            {
                writer.WriteLine("Parameter,Value A,Value B,Status");

                foreach (var key in fgDataA.Keys.Union(fgDataB.Keys))
                {
                    FGParameter a;
                    FGParameter b;

                    fgDataA.TryGetValue(key, out a);
                    fgDataB.TryGetValue(key, out b);

                    string status =
                        a == null ? "Added" :
                        b == null ? "Removed" :
                        a.Value == b.Value ? "Same" : "Modified";

                    writer.WriteLine($"{key},{a?.Value},{b?.Value},{status}");
                }
            }

            lblStatus.Text = "Comparison exported";
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        #endregion

        #region TreeView
        private void ImportGFSFile(string gfsFile, TreeView tvTables, bool isFileA)
        {
            DataSet ds = new DataSet();
            ds.ReadXml(gfsFile);

            if (isFileA)
                importedDataSetA = ds;
            else
                importedDataSetB = ds;

            if (ds.Tables.Contains("TabProjDesc") &&
                ds.Tables["TabProjDesc"].Rows.Count > 0 &&
                ds.Tables["TabProjDesc"].Columns.Contains("MtrType"))
            {
                ConvertMeterType(
                    int.Parse(ds.Tables["TabProjDesc"].Rows[0]["MtrType"].ToString(),
                    NumberStyles.HexNumber));
            }

            LoadTreeViewWithTables(tvTables, ds);
        }
        private void LoadTreeViewWithTables(TreeView tvTables, DataSet ds)
        {
            tvTables.Nodes.Clear();
            if (ds == null) return;

            foreach (DataTable table in ds.Tables)
            {
                TreeNode node = new TreeNode(table.TableName)
                {
                    Name = table.TableName
                };
                tvTables.Nodes.Add(node);
            }

            if (tvTables.Nodes.ContainsKey("TabFGCommands"))
                tvTables.SelectedNode = tvTables.Nodes["TabFGCommands"];
        }

        public static string ConvertMeterType(int intMtrTyp)
        {
            string retval = string.Empty;
            string binaryvalue = Utilities.ToBinary(intMtrTyp).PadLeft(16, '0');

            if (binaryvalue.Substring(0, 1) == "0")
                retval = retval + "1P-";
            else
                retval = retval + "3P-";

            if (binaryvalue.Substring(1, 2) == "00")
                retval = retval + "1W,";
            else if (binaryvalue.Substring(1, 2) == "01")
                retval = retval + "2W,";
            else if (binaryvalue.Substring(1, 2) == "10")
                retval = retval + "3W,";
            else if (binaryvalue.Substring(1, 2) == "11")
                retval = retval + "4W,";

            if (binaryvalue.Substring(3, 2) == "00")
                retval = retval + "Class 0.2S,";
            else if (binaryvalue.Substring(3, 2) == "01")
                retval = retval + "Class 0.5S,";
            else if (binaryvalue.Substring(3, 2) == "10")
                retval = retval + "Class 1,";
            else if (binaryvalue.Substring(3, 2) == "11")
                retval = retval + "Class 2,";

            if (binaryvalue.Substring(5, 1) == "0")
                retval = retval + "CT,";
            else
                retval = retval + "WC,";

            if (binaryvalue.Substring(6, 3) == "000")
                retval = retval + "63.5 V,";
            else if (binaryvalue.Substring(6, 3) == "001")
                retval = retval + "110 V,";
            else if (binaryvalue.Substring(6, 3) == "010")
                retval = retval + "120 V,";
            else if (binaryvalue.Substring(6, 3) == "011")
                retval = retval + "220 V,";
            else if (binaryvalue.Substring(6, 3) == "100")
                retval = retval + "230 V,";
            else if (binaryvalue.Substring(6, 3) == "101")
                retval = retval + "240 V,";
            else if (binaryvalue.Substring(6, 3) == "110")
                retval = retval + "Reserved,";
            else if (binaryvalue.Substring(6, 3) == "111")
                retval = retval + "Reserved,";

            if (binaryvalue.Substring(9, 3) == "000")
                retval = retval + "1-";
            else if (binaryvalue.Substring(9, 3) == "001")
                retval = retval + "2.5-";
            else if (binaryvalue.Substring(9, 3) == "010")
                retval = retval + "5-";
            else if (binaryvalue.Substring(9, 3) == "011")
                retval = retval + "10-";
            else if (binaryvalue.Substring(9, 3) == "100")
                retval = retval + "15-";
            else if (binaryvalue.Substring(9, 3) == "101")
                retval = retval + "20-";
            else if (binaryvalue.Substring(9, 3) == "110")
                retval = retval + "30-";
            else if (binaryvalue.Substring(9, 3) == "111")
                retval = retval + "40-";

            if (binaryvalue.Substring(12, 4) == "0000")
                retval = retval + "1.2 A";
            else if (binaryvalue.Substring(12, 4) == "0001")
                retval = retval + "2 A";
            else if (binaryvalue.Substring(12, 4) == "0010")
                retval = retval + "6 A";
            else if (binaryvalue.Substring(12, 4) == "0011")
                retval = retval + "10 A";
            else if (binaryvalue.Substring(12, 4) == "0100")
                retval = retval + "20 A";
            else if (binaryvalue.Substring(12, 4) == "0101")
                retval = retval + "30 A";
            else if (binaryvalue.Substring(12, 4) == "0110")
                retval = retval + "40 A";
            else if (binaryvalue.Substring(12, 4) == "0111")
                retval = retval + "60 A";
            else if (binaryvalue.Substring(12, 4) == "1000")
                retval = retval + "80 A";
            else if (binaryvalue.Substring(12, 4) == "1001")
                retval = retval + "100 A";
            else if (binaryvalue.Substring(12, 4) == "1010")
                retval = retval + "120 A";
            else if (binaryvalue.Substring(12, 4) == "1011")
                retval = retval + "7.5 A";
            else if (binaryvalue.Substring(12, 4) == "1100")
                retval = retval + "200 A";
            else if (binaryvalue.Substring(12, 4) == "1101")
                retval = retval + "300 A";
            //3P-4W,Class 1,WC,240 V,10-60 A
            try
            {
                ConnectionType = retval.Split(',')[0].Trim();
                AccuracyClass = retval.Split(',')[1].Trim();
                CategoryWCorCT = retval.Split(',')[2].Trim();
                Vref = Vref = Convert.ToDouble(retval.Split(',')[3].Trim().Split(' ')[0].Trim());
                Ib = Ib = Convert.ToDouble(retval.Split(',')[4].Trim().Split('-')[0].Trim());
                Imax = Imax = Convert.ToDouble(retval.Split(',')[4].Trim().Split('-')[1].Trim().Split(' ')[0].Trim());
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message.ToString()} => TRACE: {ex.StackTrace.ToString()}");
            }
            return retval;
        }
        #endregion

        #region Comparison Logic
        private void ClearRowStyles(DataGridView dgv)
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                row.DefaultCellStyle.BackColor = Color.White;
                foreach (DataGridViewCell cell in row.Cells)
                    cell.Style.BackColor = Color.White;
            }
        }
        #endregion

        #region Helper Methds
        private void SyncTreeViews(string tableName, bool fromA)
        {
            if (fromA)
            {
                // Load A
                if (importedDataSetA != null && importedDataSetA.Tables.Contains(tableName))
                {
                    DisplayTable(tableName, dataGridViewFG_A, importedDataSetA);
                }

                // Select + Load B
                if (treeViewFG_B.Nodes.ContainsKey(tableName))
                {
                    treeViewFG_B.AfterSelect -= treeViewFG_B_AfterSelect;
                    treeViewFG_B.SelectedNode = treeViewFG_B.Nodes[tableName];
                    treeViewFG_B.AfterSelect += treeViewFG_B_AfterSelect;

                    if (importedDataSetB != null && importedDataSetB.Tables.Contains(tableName))
                        DisplayTable(tableName, dataGridViewFG_B, importedDataSetB);
                }
            }
            else
            {
                // Load B
                if (importedDataSetB != null && importedDataSetB.Tables.Contains(tableName))
                    DisplayTable(tableName, dataGridViewFG_B, importedDataSetB);

                // Select + Load A
                if (treeViewFG_A.Nodes.ContainsKey(tableName))
                {
                    treeViewFG_A.AfterSelect -= treeViewFG_A_AfterSelect;
                    treeViewFG_A.SelectedNode = treeViewFG_A.Nodes[tableName];
                    treeViewFG_A.AfterSelect += treeViewFG_A_AfterSelect;

                    if (importedDataSetA != null && importedDataSetA.Tables.Contains(tableName))
                        DisplayTable(tableName, dataGridViewFG_A, importedDataSetA);
                }
            }
        }
        private void InvalidateDataGridView(DataGridView dgData)
        {
            if (dgData.Columns.Count < 1)
                return;
            foreach (DataGridViewColumn col in dgData.Columns)
            {
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                if (col.HeaderText.Contains("DataPkt"))
                {
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    col.Width = 200;
                }
            }
        }
        private void DisplayTable(string tableName, DataGridView dgData, DataSet ds)
        {
            dgData.DataSource = null;
            dgData.DataSource = ds.Tables[tableName];
            InvalidateDataGridView(dgData);
        }
        private void CompareTables(DataTable tableA, DataTable tableB)
        {
            ClearRowStyles(dataGridViewFG_A);
            ClearRowStyles(dataGridViewFG_B);

            int rowCount = Math.Max(tableA.Rows.Count, tableB.Rows.Count);
            int colCount = Math.Min(tableA.Columns.Count, tableB.Columns.Count);

            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                DataGridViewRow rowA =
                    rowIndex < dataGridViewFG_A.Rows.Count
                        ? dataGridViewFG_A.Rows[rowIndex]
                        : null;

                DataGridViewRow rowB =
                    rowIndex < dataGridViewFG_B.Rows.Count
                        ? dataGridViewFG_B.Rows[rowIndex]
                        : null;

                // Row exists only in A
                if (rowA != null && rowB == null)
                {
                    rowA.DefaultCellStyle.BackColor = Color.MistyRose;
                    continue;
                }

                // Row exists only in B
                if (rowA == null && rowB != null)
                {
                    rowB.DefaultCellStyle.BackColor = Color.MistyRose;
                    continue;
                }

                bool rowDifferent = false;

                for (int colIndex = 0; colIndex < colCount; colIndex++)
                {
                    string valA = rowA.Cells[colIndex].Value?.ToString();
                    string valB = rowB.Cells[colIndex].Value?.ToString();

                    if (!string.Equals(valA, valB, StringComparison.Ordinal))
                    {
                        rowA.Cells[colIndex].Style.BackColor = Color.LemonChiffon;
                        rowB.Cells[colIndex].Style.BackColor = Color.LemonChiffon;
                        rowDifferent = true;
                    }
                }

                if (!rowDifferent)
                {
                    rowA.DefaultCellStyle.BackColor = Color.Honeydew;
                    rowB.DefaultCellStyle.BackColor = Color.Honeydew;
                }
            }
        }


        internal class FGParameter
        {
            public string Name { get; }
            public string Value { get; }
            public string Unit { get; }
            public string Type { get; }

            public FGParameter(string name, string value, string unit, string type)
            {
                Name = name;
                Value = value;
                Unit = unit;
                Type = type;
            }
        }
        #endregion
        private void UpdateCompareState()
        {
            if (importedFileCount < 2)
            {
                btnCompare.Enabled = false;
                lblStatus.Text = "Import second FG file to enable comparison";
            }
            else
            {
                btnCompare.Enabled = true;
                lblStatus.Text = "Ready to compare";
            }
        }
        private void ResetComparison()
        {
            importedFileCount = 0;
            fgFilePathA = fgFilePathB = string.Empty;

            importedDataSetA = null;
            importedDataSetB = null;

            treeViewFG_A.Nodes.Clear();
            treeViewFG_B.Nodes.Clear();

            dataGridViewFG_A.DataSource = null;
            dataGridViewFG_B.DataSource = null;

            lblFileA.Text = "FG File A";
            lblFileB.Text = "FG File B";

            UpdateCompareState();
        }

    }
}
