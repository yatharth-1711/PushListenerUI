using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Globalization;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace MeterReader.CommonClasses
{
    public static class DataTableOperations
    {
        public static DataTable dt;
        public static void InitializeDataTable()
        {
            dt = new DataTable();

        }
        public static DataTable GetSampleDataTable()
        {
            DataTable dt = new DataTable("SampleData");
            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Date", typeof(string));


            dt.Rows.Add(1, $"John Doe - {1}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            dt.Rows.Add(2, $"Jane Smith - {2}", DateTime.Now.AddDays(1).ToString("dd/MM/yyyy HH:mm:ss"));
            dt.Rows.Add(3, $"Samuel Johnson - {3}", DateTime.Now.AddDays(2).ToString("dd/MM/yyyy HH:mm:ss"));
            dt.Rows.Add(4, $"John Doe - {4}", DateTime.Now.AddDays(3).ToString("dd/MM/yyyy HH:mm:ss"));
            dt.Rows.Add(5, $"Jane Smith - {5}", DateTime.Now.AddDays(4).ToString("dd/MM/yyyy HH:mm:ss"));
            dt.Rows.Add(6, $"Samuel Johnson - {6}", DateTime.Now.AddDays(5).ToString("dd/MM/yyyy HH:mm:ss"));
            dt.Rows.Add(7, $"John Doe - {7}", DateTime.Now.AddDays(6).ToString("dd/MM/yyyy HH:mm:ss"));
            dt.Rows.Add(8, $"Jane Smith - {8}", DateTime.Now.AddDays(7).ToString("dd/MM/yyyy HH:mm:ss"));
            dt.Rows.Add(9, $"Samuel Johnson - {9}", DateTime.Now.AddDays(8).ToString("dd/MM/yyyy HH:mm:ss"));
            return dt;
        }
        public static void ExportDataTableToExcel(DataTable dataTable, string filePath)
        {
            //ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Set the license context
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");
                if (dataTable == null || dataTable.Columns.Count == 0)
                {
                    worksheet.Cells["A1"].Value = "No data available";
                    worksheet.Cells["A1"].Style.Font.Bold = true;
                    worksheet.Cells["A1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells["A1"].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    worksheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells["A1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }
                // Load DataTable into worksheet
                else
                {
                    worksheet.Cells["A1"].LoadFromDataTable(dataTable, true);
                    // Style the header row
                    using (var headerCells = worksheet.Cells[1, 1, 1, dataTable.Columns.Count])
                    {
                        headerCells.Style.Font.Bold = true;
                        headerCells.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        headerCells.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        headerCells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        headerCells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    }
                    // Add borders to the entire data range
                    using (var allCells = worksheet.Cells[1, 1, dataTable.Rows.Count + 1, dataTable.Columns.Count])
                    {
                        allCells.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        allCells.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        allCells.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        allCells.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    }
                    // Auto-fit columns
                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                    // Autofilter for all columns
                    if (worksheet.Dimension != null)
                        worksheet.Cells[worksheet.Dimension.Address].AutoFilter = true;
                }
                // Save the Excel file
                // Ensure the directory exists
                string directoryPath = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                FileInfo excelFile = new FileInfo(filePath);
                package.SaveAs(excelFile);
            }
        }

        //By YS
        public static void ExportDataTableToExcelWithDifferentSheet(DataTable table, string filePath, string sheetName)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            FileInfo excelFile = new FileInfo(filePath);
            using (ExcelPackage package = excelFile.Exists ? new ExcelPackage(excelFile) : new ExcelPackage())
            {
                var existingSheet = package.Workbook.Worksheets[sheetName];
                if (existingSheet != null)
                {
                    package.Workbook.Worksheets.Delete(existingSheet);
                }
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(sheetName);
                worksheet.Cells["A1"].LoadFromDataTable(table, true);
                // Style the header row
                using (var headerCells = worksheet.Cells[1, 1, 1, table.Columns.Count])
                {
                    headerCells.Style.Font.Bold = true;
                    headerCells.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerCells.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    headerCells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    headerCells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }
                using (var allCells = worksheet.Cells[1, 1, table.Rows.Count + 1, table.Columns.Count])
                {
                    allCells.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    allCells.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    allCells.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    allCells.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }
                for (int row = 2; row <= table.Rows.Count + 1; row++)
                {
                    if (row % 2 == 0)
                    {
                        using (var rowCells = worksheet.Cells[row, 1, row, table.Columns.Count])
                        {
                            rowCells.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rowCells.Style.Fill.BackgroundColor.SetColor(Color.LightCyan);
                        }
                    }
                }
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                if (worksheet.Dimension != null)
                    worksheet.Cells[worksheet.Dimension.Address].AutoFilter = true;

                string directoryPath = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                package.SaveAs(excelFile);
            }
        }
        //End By YS
        public static object[][] GetColumnsAsArray(DataTable dataTable, string[] searchHeaders, bool IsIncludeHeader = false)
        {
            if (dataTable == null || searchHeaders == null || searchHeaders.Length == 0)
                return new object[0][];

            // Find matching columns by checking if the column name contains any of the substrings
            var matchingColumns = dataTable.Columns
                .Cast<DataColumn>()
                .Where(col => searchHeaders.Any(search => col.ColumnName.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0))
                .ToArray();

            if (matchingColumns.Length == 0)
                return new object[0][];

            // Declare result array
            object[][] result;
            if (IsIncludeHeader)
            {
                // Extract column headers and values
                result = matchingColumns
                    .Select(col =>
                    {
                        var columnData = dataTable.Rows.Cast<DataRow>().Select(row => row[col]).ToList();
                        columnData.Insert(0, col.ColumnName); // Add header as the first item
                        return columnData.ToArray();
                    })
                    .ToArray();
            }
            else
            {
                // Extract the column values
                result = matchingColumns
                    .Select(col => dataTable.Rows.Cast<DataRow>().Select(row => row[col]).ToArray())
                    .ToArray();
            }
            return result;
        }

        public static DataTable FilterDataTableByDate(DataTable table, DateTime startDate, DateTime endDate, string partialColumnName)
        {
            if (table == null || string.IsNullOrWhiteSpace(partialColumnName))
                return table.Clone(); // Return an empty table structure

            // Find the actual column name that contains the partialColumnName
            string dateColumn = table.Columns.Cast<DataColumn>()
                                             .FirstOrDefault(c => c.ColumnName.IndexOf(partialColumnName, StringComparison.OrdinalIgnoreCase) >= 0)
                                             ?.ColumnName;

            if (string.IsNullOrEmpty(dateColumn))
                return table.Clone(); // If no match is found, return an empty table structure

            if (startDate > endDate)
            {
                var temp = startDate;
                startDate = endDate;
                endDate = temp;
            }

            DataTable filteredTable = table.Clone(); // Clone structure

            foreach (DataRow row in table.Rows)
            {
                object value = row[dateColumn];
                DateTime date;

                // Prefer direct cast if the type is DateTime
                if (value is DateTime dt)
                {
                    date = dt;
                }
                else if (!DateTime.TryParseExact(value?.ToString().Trim(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                {
                    continue;
                }

                // Use inclusive range check
                if (date >= startDate && date <= endDate)
                {
                    filteredTable.ImportRow(row);
                }
            }
            filteredTable.AcceptChanges();

            #region OLD
            //var rows = table.AsEnumerable()
            //                .Where(row =>
            //                {
            //                    if (DateTime.TryParseExact(row[dateColumn]?.ToString(),
            //                                               "dd/MM/yyyy HH:mm:ss",
            //                                               CultureInfo.InvariantCulture,
            //                                               DateTimeStyles.None,
            //                                               out DateTime date))
            //                    {
            //                        return (date >= startDate && date <= endDate);
            //                    }
            //                    return false;
            //                });

            //foreach (var row in rows)
            //{
            //    filteredTable.ImportRow(row);
            //}
            #endregion

            return filteredTable;
        }

        /*
            public static DataTable FilterDataTableByDate(DataTable table, DateTime startDate, DateTime endDate, string searchText)
            {
                if (table == null || string.IsNullOrWhiteSpace(searchText))
                    return table.Clone();//// Return empty table structure if no search string provided


                // Clone the table structure
                DataTable filteredTable = table.Clone();

                foreach (DataRow row in table.Rows)
                {
                    if (DateTime.TryParseExact(row[searchText].ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                    {
                        if (date >= startDate && date <= endDate)
                        {
                            filteredTable.ImportRow(row);
                        }
                    }
                }

                return filteredTable;
            }
                */
        public static DataTable FilterDataTableByDay(DataTable table, DateTime specificDay, string searchText)
        {
            if (table == null || string.IsNullOrWhiteSpace(searchText))
                return table.Clone(); // Return empty table structure if no search string is provided

            // Clone the table structure
            DataTable filteredTable = table.Clone();

            // Find the column name containing the search text
            string columnName = table.Columns
                .Cast<DataColumn>()
                .FirstOrDefault(col => col.ColumnName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)?.ColumnName;

            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException($"No column found containing the text '{searchText}'.");

            foreach (DataRow row in table.Rows)
            {
                if (DateTime.TryParseExact(row[columnName].ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                {
                    if (date.Date == specificDay.Date)
                    {
                        filteredTable.ImportRow(row);
                    }
                }
            }

            return filteredTable;
        }
        public static DataTable GetSubsetDataTable(DataTable sourceTable, string option, int count)
        {
            if (sourceTable == null || sourceTable.Rows.Count == 0 || count <= 0 ||
                (!option.Equals("top", StringComparison.OrdinalIgnoreCase) &&
                 !option.Equals("last", StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException("Invalid input parameters.");
            }

            DataTable resultTable = sourceTable.Clone(); // Clone the structure of the source table
                                                         // Adjust count if it's greater than available rows
            count = Math.Min(count, sourceTable.Rows.Count);

            if (option.Equals("top", StringComparison.OrdinalIgnoreCase))
            {
                // Take the top 'count' rows
                for (int i = 0; i < Math.Min(count, sourceTable.Rows.Count); i++)
                {
                    resultTable.ImportRow(sourceTable.Rows[i]);
                }
            }
            else if (option.Equals("last", StringComparison.OrdinalIgnoreCase))
            {
                // Take the last 'count' rows
                for (int i = Math.Max(0, sourceTable.Rows.Count - count); i < sourceTable.Rows.Count; i++)
                {
                    resultTable.ImportRow(sourceTable.Rows[i]);
                }
            }
            return resultTable;
        }
        public static string GetValueFromDataTable(DataTable dataTable, string columnSearchText, int rowIndex)
        {
            rowIndex--; // Adjust for 0-based index
            if (dataTable == null || dataTable.Rows.Count == 0 || string.IsNullOrEmpty(columnSearchText))
            {
                throw new ArgumentException("Invalid input parameters.");
            }

            // Find the first column that contains the search text
            DataColumn selectedColumn = dataTable.Columns
                .Cast<DataColumn>()
                .FirstOrDefault(col => col.ColumnName.IndexOf(columnSearchText, StringComparison.OrdinalIgnoreCase) >= 0);

            if (selectedColumn == null)
            {
                //return null; // Or return any other default value like string.Empty or DBNull.Value
                return string.Empty; // Or return any other default value like string.Empty or DBNull.Value
            }

            // Validate the row index
            if (rowIndex < 0 || rowIndex >= dataTable.Rows.Count)
            {
                //return null; // Or return any other default value like string.Empty or DBNull.Value
                return string.Empty; // Or return any other default value like string.Empty or DBNull.Value
            }

            // Return the value at the specified row and column
            return dataTable.Rows[rowIndex][selectedColumn].ToString();
        }
        public static bool ColumnContainsSubstringValues(DataTable dataTable, string columnName, List<string> substrings)
        {
            if (dataTable == null || string.IsNullOrWhiteSpace(columnName) || substrings == null || substrings.Count == 0)
            {
                throw new ArgumentException("Invalid input parameters.");
            }

            // Check if the column exists in the DataTable
            if (!dataTable.Columns.Contains(columnName))
            {
                throw new ArgumentException($"Column '{columnName}' does not exist in the DataTable.");
            }

            // Check if any cell in the column contains any of the substrings
            return dataTable.AsEnumerable()
                .Select(row => row[columnName]?.ToString())
                .Any(cellValue =>
                    substrings.Any(substring =>
                        cellValue != null && cellValue.IndexOf(substring, StringComparison.OrdinalIgnoreCase) >= 0));
        }
        public static bool ColumnContainsSubstringValues(DataTable dataTable, string columnName, List<string> substrings, out List<string> foundSubstrings, out List<string> notFoundSubstrings)
        {
            if (dataTable == null || string.IsNullOrWhiteSpace(columnName) || substrings == null || substrings.Count == 0)
            {
                throw new ArgumentException("Invalid input parameters.");
            }

            // Check if the column exists in the DataTable
            if (!dataTable.Columns.Contains(columnName))
            {
                throw new ArgumentException($"Column '{columnName}' does not exist in the DataTable.");
            }

            // Initialize the lists
            foundSubstrings = new List<string>();
            notFoundSubstrings = new List<string>(substrings);

            // Check if any cell in the column contains any of the substrings
            foreach (var cellValue in dataTable.AsEnumerable()
                                               .Select(row => row[columnName]?.ToString()))
            {
                if (cellValue == null) continue;

                // Find substrings in the current cell value
                foreach (var substring in substrings)
                {
                    if (cellValue.IndexOf(substring, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        if (!foundSubstrings.Contains(substring))
                        {
                            foundSubstrings.Add(substring);
                        }
                        notFoundSubstrings.Remove(substring);
                    }
                }
            }

            // Return true if any substring was found
            return foundSubstrings.Count > 0;
        }

        public static DataRow FindRow(this DataTable table, string columnText, string rowCellValue)
        {
            if (table == null || string.IsNullOrEmpty(columnText) || string.IsNullOrEmpty(rowCellValue))
            {
                return null;
            }

            var foundColumn = table.Columns.Cast<DataColumn>()
                .FirstOrDefault(col => col.ColumnName.IndexOf(columnText, StringComparison.OrdinalIgnoreCase) >= 0); // Use IndexOf

            if (foundColumn == null)
            {
                return null;
            }

            return table.Rows.Cast<DataRow>()
                .FirstOrDefault(row => row[foundColumn] != DBNull.Value && row[foundColumn].ToString().IndexOf(rowCellValue, StringComparison.OrdinalIgnoreCase) >= 0); // Use IndexOf

            /*
                        if (table == null || string.IsNullOrEmpty(columnText) || string.IsNullOrEmpty(rowCellValue))
                        {
                            return null; // Handle null or empty input
                        }

                        // Find the column containing the specified text (case-insensitive)
                        DataColumn foundColumn = null;
                        foreach (DataColumn column in table.Columns)
                        {
                            if (column.ColumnName.IndexOf(columnText, StringComparison.OrdinalIgnoreCase) >= 0) // Use IndexOf for case-insensitive search
                            {
                                foundColumn = column;
                                break; // Stop once the column is found.
                            }
                        }


                        if (foundColumn == null)
                        {
                            return null; // Column not found
                        }

                        // Search for the row with the specified cell value in the found column (case-insensitive)
                        foreach (DataRow row in table.Rows)
                        {
                            if (row[foundColumn] != DBNull.Value && row[foundColumn].ToString().IndexOf(rowCellValue, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                return row; // Row found
                            }
                        }

                        return null; // Row not found
            */
        }
        public static List<DataRow> FindRows(DataTable table, string columnName, string matchValue)
        {
            if (table == null || string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(matchValue))
                return new List<DataRow>();

            var targetColumn = table.Columns
                .Cast<DataColumn>()
                .FirstOrDefault(col => col.ColumnName.Equals(columnName, StringComparison.OrdinalIgnoreCase));

            if (targetColumn == null)
                return new List<DataRow>();

            return table.Rows
                .Cast<DataRow>()
                .Where(row => row[targetColumn] != DBNull.Value &&
                              row[targetColumn].ToString().Equals(matchValue, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        /// <summary>
        /// Filter the PushSetupInfo class data tables in standard format of CLASS, OBIS, ATTRIBUTE, SCALER, UNIT, and DATATYPE
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static DataTable FilterPushSetUpDataTable(DataTable dataTable)
        {
            string[] finalColumnArray =
                                     {"DESCRIPTION",
                                    "CLASS",
                                    "OBIS",
                                    "ATTRIBUTE",
                                    "SCALER",
                                    "UNIT",
                                    "DATATYPE"
                                     };

            DataTable result = new DataTable();

            // These are the columns we expect in the source DataTable
            List<string> requiredColumns = new List<string>
                                                        {
                                                           "DESCRIPTION",
                                                           "CLASS",
                                                            "OBIS",
                                                            "ATTRIBUTE INDEX",
                                                            "SCALER MUTIPLIER",
                                                            "UNIT",
                                                            "DATA ANNOTATION INDIVIDUAL"
                                                        };

            // Create a mapping between source and target column names
            Dictionary<string, string> columnMap = new Dictionary<string, string>();

            for (int i = 0; i < requiredColumns.Count; i++)
            {
                string sourceColumn = requiredColumns[i];
                if (dataTable.Columns.Contains(sourceColumn))
                {
                    string targetColumn = finalColumnArray[i];
                    result.Columns.Add(targetColumn, dataTable.Columns[sourceColumn].DataType);
                    columnMap[sourceColumn] = targetColumn;
                }
            }

            // Copy data row-wise using the column map
            foreach (DataRow row in dataTable.Rows)
            {
                DataRow newRow = result.NewRow();
                foreach (var kvp in columnMap)
                {
                    newRow[kvp.Value] = row[kvp.Key];
                }
                result.Rows.Add(newRow);
            }

            return result;
        }

        /// <summary>
        /// Return Filtered Data Table with provided last number of row count.
        /// </summary>
        /// <param name="originalTable"></param>
        /// <param name="lastRowCount"></param>
        /// <returns></returns>
        public static DataTable GetLastRows(DataTable originalTable, int lastRowCount)
        {
            if (originalTable == null)
                return originalTable.Clone();

            DataTable filteredTable = originalTable.Clone(); // Clone structure without data

            int totalRows = originalTable.Rows.Count;

            // If requested rows exceed available rows, return full copy
            if (lastRowCount >= totalRows)
            {
                foreach (DataRow row in originalTable.Rows)
                {
                    filteredTable.ImportRow(row);
                }
                return filteredTable;
            }
            // Otherwise, return only the last N rows
            for (int i = totalRows - lastRowCount; i < totalRows; i++)
            {
                filteredTable.ImportRow(originalTable.Rows[i]);
            }

            return filteredTable;
        }

        /// <summary>
        /// Filter the ProfileGenericInfo class data tables in standard format of CLASS, OBIS, ATTRIBUTE, SCALER, UNIT, and DATATYPE
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static DataTable FilterProfileGenericDataTable(DataTable dataTable)
        {
            string[] finalColumnArray =
                                     {"DESCRIPTION",
                                    "CLASS",
                                    "OBIS",
                                    "ATTRIBUTE",
                                    "SCALER",
                                    "UNIT",
                                    "DATATYPE"
                                     };

            DataTable result = new DataTable();

            // These are the columns we expect in the source DataTable
            List<string> requiredColumns = new List<string>
                                                        {
                                                            "Description",
                                                            "IC",
                                                            "OBIS",
                                                            "Attribute",
                                                            "Scaler",
                                                            "Unit",
                                                            "Data Type"
                                                        };

            // Create a mapping between source and target column names
            Dictionary<string, string> columnMap = new Dictionary<string, string>();

            for (int i = 0; i < requiredColumns.Count; i++)
            {
                string sourceColumn = requiredColumns[i];
                if (dataTable.Columns.Contains(sourceColumn))
                {
                    string targetColumn = finalColumnArray[i];
                    result.Columns.Add(targetColumn, dataTable.Columns[sourceColumn].DataType);
                    columnMap[sourceColumn] = targetColumn;
                }
            }

            // Copy data row-wise using the column map
            foreach (DataRow row in dataTable.Rows)
            {
                DataRow newRow = result.NewRow();
                foreach (var kvp in columnMap)
                {
                    newRow[kvp.Value] = row[kvp.Key];
                }
                result.Rows.Add(newRow);
            }

            return result;
        }

        /// <summary>
        /// Appended dt1 with dt2 
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <returns></returns>
        public static DataTable AppendDataTables(DataTable dt1, DataTable dt2)
        {
            // Clone the schema of the first table to create a new table
            DataTable result = dt1.Clone();

            // Import rows from the first table
            foreach (DataRow row in dt1.Rows)
            {
                result.ImportRow(row);
            }

            // Import rows from the second table
            foreach (DataRow row in dt2.Rows)
            {
                result.ImportRow(row);
            }

            return result;
        }

        /// <summary>
        /// Return the new table with removed last number of rows 
        /// </summary>
        /// <param name="sourceTable"></param>
        /// <param name="numberOfRowsToRemove"></param>
        /// <returns></returns>
        public static DataTable RemoveLastRows(DataTable sourceTable, int numberOfRowsToRemove)
        {
            // Clone the schema to create a new table
            DataTable result = sourceTable.Clone();

            // Determine how many rows to keep
            int rowsToKeep = Math.Max(0, sourceTable.Rows.Count - numberOfRowsToRemove);

            // Import only the required number of rows
            for (int i = 0; i < rowsToKeep; i++)
            {
                result.ImportRow(sourceTable.Rows[i]);
            }

            return result;
        }
        /// <summary>
        /// Return TRUE if search string contain in the specific column (by name) else FALSE
        /// </summary>
        /// <param name="table"></param>
        /// <param name="columnName"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public static bool IsStringInColumn(DataTable table, string columnName, string searchString)
        {
            if (table == null || !table.Columns.Contains(columnName))
                return false;

            return table.AsEnumerable()
                        .Where(row => !row.IsNull(columnName))
                        .Any(row => row.Field<string>(columnName)
                                      .IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        public static int GetRowIndexByColumnValue(DataTable table, string columnName, string cellValue)
        {
            if (table == null || string.IsNullOrEmpty(columnName))
                return -1;

            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (table.Rows[i][columnName]?.ToString() == cellValue)
                {
                    return i;
                }
            }

            return -1;
        }
        /// <summary>
        /// Full DataTable Comparison Method
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <returns></returns>
        public static bool AreDataTablesEqual(DataTable dt1, DataTable dt2)
        {
            // Null check
            if (dt1 == null || dt2 == null)
                return false;

            // Column count
            if (dt1.Columns.Count != dt2.Columns.Count)
                return false;

            // Compare column names and types
            for (int i = 0; i < dt1.Columns.Count; i++)
            {
                var col1 = dt1.Columns[i];
                var col2 = dt2.Columns[i];

                if (col1.ColumnName != col2.ColumnName ||
                    col1.DataType != col2.DataType)
                    return false;
            }

            // Row count
            if (dt1.Rows.Count != dt2.Rows.Count)
                return false;

            // Compare row data
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                for (int j = 0; j < dt1.Columns.Count; j++)
                {
                    var val1 = dt1.Rows[i][j]?.ToString()?.Trim();
                    var val2 = dt2.Rows[i][j]?.ToString()?.Trim();

                    if (!string.Equals(val1, val2, StringComparison.OrdinalIgnoreCase))
                        return false;
                }
            }

            return true;
        }
        public static void ExportDictionaryToExcel(Dictionary<string, DataTable> tablesDict, string filePath)
        {
            if (tablesDict == null || tablesDict.Count == 0)
                throw new ArgumentException("No data provided for export.");

            //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage package = new ExcelPackage())
            {
                foreach (var kvp in tablesDict)
                {
                    string sheetName = string.IsNullOrWhiteSpace(kvp.Key) ? "Sheet" : kvp.Key;
                    if (!(kvp.Value is DataTable dataTable) || dataTable.Columns.Count == 0)
                        continue; // Skip invalid or empty tables

                    // Create worksheet
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(sheetName);

                    // Load DataTable into worksheet
                    worksheet.Cells["A1"].LoadFromDataTable(dataTable, true);

                    // Style header row
                    using (var headerCells = worksheet.Cells[1, 1, 1, dataTable.Columns.Count])
                    {
                        headerCells.Style.Font.Bold = true;
                        headerCells.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        headerCells.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        headerCells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        headerCells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    }

                    // Add borders
                    using (var allCells = worksheet.Cells[1, 1, dataTable.Rows.Count + 1, dataTable.Columns.Count])
                    {
                        allCells.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        allCells.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        allCells.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        allCells.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    }

                    // Auto-fit & autofilter
                    if (worksheet.Dimension != null)
                    {
                        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                        worksheet.Cells[worksheet.Dimension.Address].AutoFilter = true;
                    }

                    // Freeze top row
                    worksheet.View.FreezePanes(2, 1);
                }

                // Ensure directory exists
                string directoryPath = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Save file
                FileInfo excelFile = new FileInfo(filePath);
                package.SaveAs(excelFile);
            }
        }


    }
}
