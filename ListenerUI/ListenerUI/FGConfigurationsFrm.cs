using AutoTest.FrameWork.Converts;
using ListenerUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace MeterReader.HelperForms.FGConfigurations
{
    public partial class FGConfigurationsFrm : Form
    {
        private DataSet importedDataSet;
        public FGConfigurationsFrm()
        {
            InitializeComponent();
            InitializeGrid();
        }
        private void InitializeGrid()
        {
            dgData.AllowUserToAddRows = false;
            dgData.AllowUserToDeleteRows = false;
            dgData.ReadOnly = false;
            dgData.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
        }

        private void btnImportGFS_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "GFS Files (*.gfs)|*.gfs";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ImportGFSFile(ofd.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error importing GFS: " + ex.Message);
                }
            }
        }
        private void ImportGFSFile(string gfsFile)
        {
            importedDataSet = new DataSet();

            // IMPORTANT: Though extension is .gfs, it is XML
            importedDataSet.ReadXml(gfsFile);

            LoadTreeViewWithTables();
        }
        private void LoadTreeViewWithTables()
        {
            tvTables.Nodes.Clear();

            if (importedDataSet == null) return;

            TreeNode root = new TreeNode("Imported Tables");

            foreach (DataTable table in importedDataSet.Tables)
            {
                if (table.TableName == "TabRTCCommands" || table.TableName == "TabFGCommands")
                {
                    DataColumn dataColumn = new DataColumn("Description");
                    table.Columns.Add(dataColumn);
                    List<string> paramList = new List<string>();
                    foreach (DataRow row in table.Rows)
                    {
                        row[dataColumn] = $"{Convert.ToInt32(row[2].ToString().Substring(0, 2), 16)} - {DLMSParser.GetObis(row[2].ToString().Substring(2, 12))} - {Convert.ToInt32(row[2].ToString().Substring(row[2].ToString().Length - 2), 16)} : {DLMSParser.GetObisName(Convert.ToInt32(row[2].ToString().Substring(0, 2), 16).ToString(), DLMSParser.GetObis(row[2].ToString().Substring(2, 12)), Convert.ToInt32(row[2].ToString().Substring(row[2].ToString().Length - 2), 16).ToString())}";
                    }
                    dataColumn.SetOrdinal(0);
                }
                table.AcceptChanges();
                importedDataSet.AcceptChanges();
                TreeNode tableNode = new TreeNode(table.TableName);

                //// Child nodes as columns
                //foreach (DataColumn col in table.Columns)
                //{
                //    tableNode.Nodes.Add(col.ColumnName);
                //}

                root.Nodes.Add(tableNode);
            }

            tvTables.Nodes.Add(root);
            tvTables.ExpandAll();
        }

        private void tvTables_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (importedDataSet == null) return;

            string selectedTable = e.Node.Text;

            if (importedDataSet.Tables.Contains(selectedTable))
            {
                DisplayTable(selectedTable);
            }
        }
        private void DisplayTable(string tableName)
        {
            dgData.DataSource = null;
            DataTable dt = importedDataSet.Tables[tableName];
            dgData.DataSource = dt;
        }

        private void btnAddRow_Click(object sender, EventArgs e)
        {
            #region FG Configuration Form
            SuspendLayout();
            TamperConfigFGForm frm = new TamperConfigFGForm();
            frm.Show();
            ResumeLayout();

            #endregion

            //if (dgData.DataSource == null) return;

            //DataTable dt = (DataTable)dgData.DataSource;

            //DataRow row = dt.NewRow();

            //// initialize with default values
            //foreach (DataColumn col in dt.Columns)
            //{
            //    if (col.DataType == typeof(int))
            //        row[col.ColumnName] = 0;
            //    else if (col.DataType == typeof(bool))
            //        row[col.ColumnName] = false;
            //    else
            //        row[col.ColumnName] = string.Empty;
            //}

            //dt.Rows.Add(row);
        }
        private void btnDeleteRow_Click(object sender, EventArgs e)
        {
            if (dgData.DataSource == null) return;

            foreach (DataGridViewRow row in dgData.SelectedRows)
            {
                dgData.Rows.Remove(row);
            }
        }
        private void btnSaveXML_Click(object sender, EventArgs e)
        {
            if (importedDataSet == null) return;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "XML Files (*.xml)|*.xml";
            sfd.FileName = "Updated.xml";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    foreach (DataTable table in importedDataSet.Tables)
                    {
                        if (table.TableName == "TabRTCCommands" || table.TableName == "TabFGCommands")
                        {
                            table.Columns.RemoveAt(0);
                        }
                    }
                    importedDataSet.WriteXml(sfd.FileName);
                    MessageBox.Show("XML Saved Successfully");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Save Error: " + ex.Message);
                }
            }
        }



    }
}
