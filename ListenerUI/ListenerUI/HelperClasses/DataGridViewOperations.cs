using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MeterReader.CommonClasses
{
    public static class DataGridViewOperations
    {
        /// <summary>
        /// This Provide facility to move row up
        /// </summary>
        /// <param name="dataGridView"></param>
        public static void MoveRowUp(ref DataGridView dataGridView)
        {
            var selectedRows = dataGridView.SelectedCells.Cast<DataGridViewCell>()
                                  .Select(cell => cell.OwningRow)
                                  .Distinct()
                                  .OrderBy(row => row.Index)
                                  .ToList();

            dataGridView.ClearSelection();

            foreach (var row in selectedRows)
            {
                int rowIndex = row.Index;
                if (rowIndex == 0) continue; // Skip if it's the first row

                // Swap row with the one above
                var rowAbove = dataGridView.Rows[rowIndex - 1];
                dataGridView.Rows.RemoveAt(rowIndex - 1);
                dataGridView.Rows.Insert(rowIndex, rowAbove);

                // Reselect cells in the moved row
                foreach (DataGridViewCell cell in row.Cells)
                {
                    cell.Selected = true;
                }
            }
        }

        /// <summary>
        /// This Provide facility to move row down
        /// </summary>
        /// <param name="dataGridView"></param>
        public static void MoveRowDown(ref DataGridView dataGridView)
        {
            var selectedRows = dataGridView.SelectedCells.Cast<DataGridViewCell>()
                                  .Select(cell => cell.OwningRow)
                                  .Distinct()
                                  .OrderByDescending(row => row.Index)
                                  .ToList();

            dataGridView.ClearSelection();

            foreach (var row in selectedRows)
            {
                int rowIndex = row.Index;
                if (rowIndex >= dataGridView.Rows.Count - 1) continue; // Skip if it's the last row

                // Swap row with the one below
                var rowBelow = dataGridView.Rows[rowIndex + 1];
                dataGridView.Rows.RemoveAt(rowIndex + 1);
                dataGridView.Rows.Insert(rowIndex, rowBelow);

                // Reselect cells in the moved row
                foreach (DataGridViewCell cell in row.Cells)
                {
                    cell.Selected = true;
                }
            }
        }

        /// <summary>
        /// This will delete the selected row or rows from data grid view
        /// </summary>
        /// <param name="dataGridView"></param>
        public static void DeleteSelectedRows(ref DataGridView dataGridView)
        {
            var selectedRows = dataGridView.SelectedCells.Cast<DataGridViewCell>()
                                  .Select(cell => cell.OwningRow)
                                  .Distinct()
                                  .ToList();

            foreach (var row in selectedRows)
            {
                if (!row.IsNewRow)
                {
                    dataGridView.Rows.Remove(row);
                }
            }
        }

        /// <summary>
        /// Method to update the serial numbers in the SN column
        /// </summary>
        /// <param name="dataGridView"></param>
        /// <param name="colIndex"></param> 
        public static void UpdateSerialNumbers(ref DataGridView dataGridView, Int16 colIndex)
        {
            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                dataGridView.Rows[i].Cells[colIndex].Value = (i + 1).ToString(); // Update SN starting from 1
            }
            dataGridView.Invalidate();
        }

        /// <summary>
        /// Paste Clipboard Data to DataGridView
        /// </summary>
        /// <param name="dataGridView"></param>
        public static void PasteClipboardData(ref DataGridView dataGridView)
        {
            try
            {
                // Get data from clipboard
                string clipboardText = Clipboard.GetText();
                if (string.IsNullOrEmpty(clipboardText)) return;

                // Split clipboard text into lines
                string[] rows = clipboardText.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

                // Get starting row and column
                int startRow = dataGridView.CurrentCell?.RowIndex ?? 0;
                int startCol = dataGridView.CurrentCell?.ColumnIndex ?? 0;

                foreach (string rowText in rows)
                {
                    string[] cells = rowText.Split('\t'); // Split by tab for columns
                    int currentCol = startCol;

                    if (startRow >= dataGridView.Rows.Count) // Add a new row if needed
                    {
                        dataGridView.Rows.Add();
                    }

                    foreach (string cellValue in cells)
                    {
                        if (currentCol < dataGridView.ColumnCount)
                        {
                            dataGridView.Rows[startRow].Cells[currentCol].Value = cellValue;
                            currentCol++;
                        }
                    }

                    startRow++;
                }
                DataGridViewOperations.UpdateSerialNumbers(ref dataGridView, 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error pasting data: " + ex.Message);
            }
        }

        /// <summary>
        /// The reference data grid and data table must have same number of columns
        /// </summary>
        /// <param name="dataGridView"></param>
        /// <param name="dt"></param>
        public static void PopulateDataGridViewFromDataTable(ref DataGridView dataGridView, DataTable dt)
        {
            if (dataGridView.Columns.Count != dt.Columns.Count)
                MessageBox.Show("There is mismatch in Columns Counts", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            dataGridView.Rows.Clear();
            foreach (DataRow row in dt.Rows)
            {
                dataGridView.Rows.Add(row.ItemArray);
            }
        }
        /// <summary>
        /// Take DataGridView as ref and return in the DataTable form
        /// </summary>
        /// <param name="dataGridView"></param>
        /// <returns></returns>
        public static DataTable GetDataTableFromDataGridView(ref DataGridView dataGridView)
        {
            DataTable dt = new DataTable();
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                dt.Columns.Add(column.Name);
            }

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                DataRow dr = dt.NewRow();
                for (int i = 0; i < dataGridView.ColumnCount; i++)
                {
                    dr[i] = row.Cells[i].Value ?? "";
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        /// <summary>
        /// Take DataGridView as ref and return in the DataTable form
        /// </summary>
        /// <param name="dataGridView"></param>
        /// <returns></returns>
        public static DataTable ConvertDataGridViewToDataTable(ref DataGridView dataGridView)
        {
            DataTable dataTable = new DataTable();

            // Add columns to DataTable
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                dataTable.Columns.Add(column.HeaderText, column.ValueType ?? typeof(string));
            }
            // Add rows to DataTable
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                DataRow dataRow = dataTable.NewRow();
                foreach (DataGridViewCell cell in row.Cells)
                {
                    dataRow[cell.ColumnIndex] = cell.Value;
                }
                dataTable.Rows.Add(dataRow);
            }
            return dataTable;
        }

        /// <summary>
        /// This will take Data grid view as argument and save in excel format
        /// </summary>
        /// <param name="dataGridView"></param>
        public static void ExportToExcel(DataGridView dataGridView)
        {
            dataGridView.Invalidate();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
            saveFileDialog.Title = "Export to Excel";
            saveFileDialog.ShowDialog();
            if (saveFileDialog.FileName != "")
            {
                try
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Set the license context
                    using (ExcelPackage package = new ExcelPackage())
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");

                        // Export column headers
                        for (int i = 1; i <= dataGridView.Columns.Count; i++)
                        {
                            worksheet.Cells[1, i].Value = dataGridView.Columns[i - 1].HeaderText;
                            worksheet.Cells[1, i].Style.Font.Bold = true;
                            worksheet.Cells[1, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[1, i].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                            worksheet.Cells[1, i].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            worksheet.Cells[1, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // Center align header cells
                        }

                        // Export row data with color formatting
                        for (int i = 0; i < dataGridView.Rows.Count; i++)
                        {
                            for (int j = 0; j < dataGridView.Columns.Count; j++)
                            {
                                var cell = worksheet.Cells[i + 2, j + 1];
                                cell.Value = dataGridView.Rows[i].Cells[j].Value;

                                // Apply background color if it's explicitly set
                                Color cellBackColor = dataGridView.Rows[i].Cells[j].Style.BackColor;
                                if (cellBackColor != Color.Empty)
                                {
                                    cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    cell.Style.Fill.BackgroundColor.SetColor(cellBackColor);
                                }
                                else if (dataGridView.Rows[i].DefaultCellStyle.BackColor != Color.Empty)
                                {
                                    cellBackColor = dataGridView.Rows[i].DefaultCellStyle.BackColor;
                                }
                                else
                                {
                                    cellBackColor = dataGridView.DefaultCellStyle.BackColor;
                                }
                                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                cell.Style.Fill.BackgroundColor.SetColor(cellBackColor);

                                // Set font color to black
                                cell.Style.Font.Color.SetColor(System.Drawing.Color.Black);

                                // Apply borders
                                cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            }
                        }
                        // Auto-fit columns
                        worksheet.Cells.AutoFitColumns();
                        // Autofilter for all columns
                        if (worksheet.Dimension != null)
                            worksheet.Cells[worksheet.Dimension.Address].AutoFilter = true;
                        // Save the Excel file
                        FileInfo excelFile = new FileInfo(saveFileDialog.FileName);
                        package.SaveAs(saveFileDialog.FileName);
                    }
                    MessageBox.Show("Data exported successfully!", "Export Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (IOException ex)
                {
                    MessageBox.Show($"The file '{saveFileDialog.FileName}' is already open. Please close it and try again.", "File In Use", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while exporting data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
    }
}
