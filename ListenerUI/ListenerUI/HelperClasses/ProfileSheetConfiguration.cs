using log4net;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;
using System.Windows.Forms;

namespace MeterReader.TestHelperClasses
{
    public static class ProfileSheetConfiguration
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region Properties
        /// <summary>
        /// This will hold All Profile Tables 
        /// </summary>
        public static Dictionary<string, DataTable> profilesDT { get; set; } = new Dictionary<string, DataTable>();
        /// <summary>
        /// This will hold the Instantaneous Profile 
        /// </summary>
        public static bool IsCheckProfile { get; set; } = false;
        /// <summary>
        /// This will hold the Nameplate Profile
        /// </summary>
        public static DataTable nameplateDT { get; set; } = new DataTable();
        /// <summary>
        /// This will hold the Instantaneous Profile 
        /// </summary>
        public static DataTable instantDT { get; set; } = new DataTable();
        /// <summary>
        /// This will hold the Instantaneous Profile 
        /// </summary>
        public static DataTable billingDT { get; set; } = new DataTable();
        /// <summary>
        /// This will hold the Voltage Related Events Profile 
        /// </summary>
        public static DataTable voltageDT { get; set; } = new DataTable();
        /// <summary>
        /// This will hold the Current Related Events Profile 
        /// </summary>
        public static DataTable currentDT { get; set; } = new DataTable();
        /// <summary>
        /// This will hold the Power Related Events Profile 
        /// </summary>
        public static DataTable powerDT { get; set; } = new DataTable();
        /// <summary>
        /// This will hold the Other Tamper Events Profile 
        /// </summary>
        public static DataTable otherDT { get; set; } = new DataTable();
        /// <summary>
        /// This will hold the Transaction Events Profile 
        /// </summary>
        public static DataTable transactionDT { get; set; } = new DataTable();
        /// <summary>
        /// This will hold the NonRollOver Events Profile 
        /// </summary>
        public static DataTable nonrolloverDT { get; set; } = new DataTable();
        /// <summary>
        /// This will hold the Control Events Profile 
        /// </summary>
        public static DataTable controlDT { get; set; } = new DataTable();
        /// <summary>
        /// This will hold the Block Load Profile 
        /// </summary>
        public static DataTable blockloadDT { get; set; } = new DataTable();
        /// <summary>
        /// This will hold the Daily Load Profile 
        /// </summary>
        public static DataTable dailyloadDT { get; set; } = new DataTable();
        /// <summary>
        /// This will hold the Push Instant Profile 
        /// </summary>
        public static DataTable pushInstantDT { get; set; } = new DataTable();
        /// <summary>
        /// This will hold the Push Alert Profile 
        /// </summary>
        public static DataTable pushAlertDT { get; set; } = new DataTable();
        /// <summary>
        /// This will hold the Push LS Profile 
        /// </summary>
        public static DataTable pushLSDT { get; set; } = new DataTable();
        /// <summary>
        /// This will hold the Push DE Profile 
        /// </summary>
        public static DataTable pushDEDT { get; set; } = new DataTable();
        /// <summary>
        /// This will hold the Push Self Registration Profile 
        /// </summary>
        public static DataTable pushSelfRegDT { get; set; } = new DataTable();
        /// <summary>
        /// This will hold the Push Bill Profile 
        /// </summary>
        public static DataTable pushBillDT { get; set; } = new DataTable();
        /// <summary>
        /// This will hold the Push Tamper Profile 
        /// </summary>
        public static DataTable pushTamperDT { get; set; } = new DataTable();
        /// <summary>
        /// This holds the standards profiles for all category of meters. some of them not applicable to specific category meter.
        /// </summary>
        public static List<string> profileList { get; } = new List<string> {"Nameplate", "Instantaneous", "Billing", "BlockLoad", "DailyLoad",
                                                                            "VoltageEvents", "CurrentEvents", "PowerEvents","TransactionEvents","OtherEvents","NonrolloverEvents","ControlEvents",
        "PushInstant","PushAlert","PushLS","PushDE","PushSelfRegistration","PushBill","PushTamper"};
        public static string[] columnArray { get; } = { "CLASS", "OBIS", "ATTRIBUTE", "SCALER", "UNIT", "DATATYPE" };

        public static DataTable[] profileDTArray { get; set; } = { nameplateDT, instantDT, billingDT, blockloadDT, dailyloadDT, voltageDT, currentDT, powerDT, transactionDT, otherDT, nonrolloverDT, controlDT, pushInstantDT, pushAlertDT, pushLSDT, pushDEDT, pushSelfRegDT, pushBillDT, pushTamperDT };

        public static List<string> commonDataTypesList = new List<string>
        {
            "00:null-data",
            "03:boolean",
            "04:bit-string",
            "05:double-long",
            "06:double-long-unsigned",
            "09:octet-string",
            "0A:visible-string",
            "0D:bcd",
            "0F:integer",
            "10:long",
            "11:unsigned",
            "12:long-unsigned",
            "14:long64",
            "15:long64-unsigned",
            "16:enum",
            "17:float32",
            "18:float64",
            "19:date-time",
            "1A:date",
            "1B:time",
            "1C:delta-integer",
            "1D:delta-long",
            "1E:delta-double-long",
            "1F:delta-unsigned",
            "20:delta-long-unsigned",
            "21:delta-double-long-unsigned",
            "01:array",
            "02:structure",
            "13:compact-array"
        };
        public static List<string> standardScalerList = new List<string>
        {
            "FA:-6",
            "FB:-5",
            "FC:-4",
            "FD:-3",
            "FE:-2",
            "FF:-1",
            "00:0",
            "01:1",
            "02:2",
            "03:3",
            "04:4",
            "05:5",
            "06:6"
        };
        public static List<string> standardUnitList = new List<string>
        {
            "01:a",
            "02:mo",
            "03:wk",
            "04:d",
            "05:h",
            "06:min.",
            "07:s",
            "08:°",
            "09:°C",
            "1B:W",
            "1C:VA",
            "1D:var",
            "1E:Wh",
            "1F:VAh",
            "20:varh",
            "21:A",
            "23:V",
            "2C:Hz",
            "48:dB",
            "FF:unitless",
            "FE:other unit",
            "38:%"
        };

        #endregion

        /// <summary>
        /// This will initialize all profile DataTable
        /// </summary>
        public static void IniAllProfileTableColumns()
        {
            try
            {
                for (int i = 0; i < profileDTArray.Length; i++)
                {
                    if (profileDTArray[i].Columns.Count > 0)
                        profileDTArray[i].Columns.Clear();
                    foreach (var colName in columnArray)
                    {
                        profileDTArray[i].Columns.Add(colName, typeof(string));
                    }
                    profileDTArray[i].AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message.ToString());
            }

        }
        /// <summary>
        /// This create a sample excel sheet for the verification
        /// </summary>
        public static void ExportSampleSheet()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
            saveFileDialog.Title = "Export to Excel";
            saveFileDialog.ShowDialog();
            if (saveFileDialog.FileName != "")
            {
                try
                {
                    ExportToExcel(saveFileDialog.FileName);
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
        /// <summary>
        /// This take file path where the excel sheet will be saved.
        /// </summary>
        /// <param name="filePath"></param>
        public static void ExportToExcel(string filePath)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Set the license context
                ExcelPackage package = new ExcelPackage();
                profileList.Add("Guide");
                ExcelWorksheet[] excelWorksheets = new ExcelWorksheet[profileList.Count];
                foreach (string selectedProfile in profileList)
                {
                    // Check if the worksheet "Project Details" already exists
                    var existingWorksheetProject = package.Workbook.Worksheets[selectedProfile];
                    if (existingWorksheetProject != null)
                    {
                        package.Workbook.Worksheets.Delete(existingWorksheetProject);
                    }
                }
                for (int profileIndex = 0; profileIndex < profileList.Count; profileIndex++)
                {
                    excelWorksheets[profileIndex] = package.Workbook.Worksheets.Add(profileList[profileIndex]);
                    if (profileIndex == profileList.Count - 1)
                    {
                        List<string> guidePoint = new List<string>();
                        guidePoint.Add("Class column contain class id in decimal format.");
                        guidePoint.Add("Obis column contain obis in decimal format.");
                        guidePoint.Add("Attribute column contain attribute in decimal format.");
                        guidePoint.Add("Scaler Column will contain the Scaler in the format: <scaler byte>:<scaler> ");
                        guidePoint.Add("Unit Column will contain the format: <scaler byte>:<unit name> ");
                        guidePoint.Add("Data type column will contain the format: <data type byte>:<description>");
                        var cell = excelWorksheets[profileIndex].Cells["A1"];
                        cell.Value = "Guide";
                        cell.Style.Font.Size = 12;
                        cell.Style.Font.Bold = true;
                        cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Orange);
                        cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // Center align header cells
                        cell = excelWorksheets[profileIndex].Cells["B1"];
                        cell.Style.Font.Size = 12;
                        cell.Style.Font.Bold = true;
                        cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Orange);
                        cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // Center align header cells
                        for (int i = 2; i <= guidePoint.Count + 1; i++)
                        {
                            cell = excelWorksheets[profileIndex].Cells[$"A{i}"];
                            cell.Value = i - 1;
                            cell.Style.Font.Size = 12;
                            cell.Style.Font.Bold = false;
                            cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                            cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // Left align cells
                            cell = excelWorksheets[profileIndex].Cells[$"B{i}"];
                            cell.Value = guidePoint[i - 2];
                            cell.Style.Font.Size = 12;
                            cell.Style.Font.Bold = false;
                            cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                            cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; // Left align cells
                        }

                        cell = excelWorksheets[profileIndex].Cells["D1"];
                        cell.Value = "Common Data Types";
                        cell.Style.Font.Size = 12;
                        cell.Style.Font.Bold = true;
                        cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Orange);
                        cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // Center align header cells
                        cell = excelWorksheets[profileIndex].Cells["D2"];
                        cell.Value = "<data type byte>:<description>";
                        cell.Style.Font.Size = 12;
                        cell.Style.Font.Bold = true;
                        cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Orange);
                        cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // Center align header cells
                        for (int i = 3; i <= commonDataTypesList.Count + 2; i++)
                        {
                            cell = excelWorksheets[profileIndex].Cells[$"D{i}"];
                            cell.Value = commonDataTypesList[i - 3];
                            cell.Style.Font.Size = 12;
                            cell.Style.Font.Bold = false;
                            cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                            cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; // Left align cells
                        }


                        cell = excelWorksheets[profileIndex].Cells["E1"];
                        cell.Value = "Scaler";
                        cell.Style.Font.Size = 12;
                        cell.Style.Font.Bold = true;
                        cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Orange);
                        cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // Center align header cells
                        cell = excelWorksheets[profileIndex].Cells["E2"];
                        cell.Value = "<scaler byte>:<scaler>";
                        cell.Style.Font.Size = 12;
                        cell.Style.Font.Bold = true;
                        cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Orange);
                        cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // Center align header cells
                        for (int i = 3; i <= standardScalerList.Count + 2; i++)
                        {
                            cell = excelWorksheets[profileIndex].Cells[$"E{i}"];
                            cell.Value = commonDataTypesList[i - 3];
                            cell.Style.Font.Size = 12;
                            cell.Style.Font.Bold = false;
                            cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                            cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; // Left align cells
                        }

                        cell = excelWorksheets[profileIndex].Cells["F1"];
                        cell.Value = "Unit";
                        cell.Style.Font.Size = 12;
                        cell.Style.Font.Bold = true;
                        cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Orange);
                        cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // Center align header cells
                        cell = excelWorksheets[profileIndex].Cells["F2"];
                        cell.Value = "<unit byte>:<unit name>";
                        cell.Style.Font.Size = 12;
                        cell.Style.Font.Bold = true;
                        cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Orange);
                        cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // Center align header cells
                        for (int i = 3; i <= standardScalerList.Count + 2; i++)
                        {
                            cell = excelWorksheets[profileIndex].Cells[$"F{i}"];
                            cell.Value = standardScalerList[i - 3];
                            cell.Style.Font.Size = 12;
                            cell.Style.Font.Bold = false;
                            cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                            cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; // Left align cells
                        }

                        excelWorksheets[profileIndex].Cells.AutoFitColumns();
                        package.Workbook.Worksheets.MoveBefore(excelWorksheets[profileIndex].Index, 0);
                        profileList.RemoveAt(profileList.Count - 1);
                        break;
                    }

                    // Export column headers 
                    for (int j = 1; j <= columnArray.Length; j++)
                    {
                        var cell = excelWorksheets[profileIndex].Cells[1, j];
                        cell.Value = columnArray[j - 1];
                        cell.Style.Font.Size = 12;
                        cell.Style.Font.Bold = true;
                        cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Orange);
                        cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // Center align header cells
                    }
                    // Set header row background color and center alignment
                    var Row1 = excelWorksheets[profileIndex].Cells["A1:F1"];
                    Row1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    Row1.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Orange);
                    Row1.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    Row1.Style.Font.Size = 12;
                    Row1.Style.Font.Bold = true;
                    // Set borders for header row
                    Row1.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    Row1.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    Row1.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    Row1.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    excelWorksheets[profileIndex].Cells.AutoFitColumns();
                }
                // Save the Excel file
                FileInfo excelFile = new FileInfo(filePath);
                package.SaveAs(excelFile);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message.ToString());
            }
        }
        /// <summary>
        /// It take file path and import the excel into system and validate it first then assign data table value.
        /// </summary>
        /// <param name="filePath"></param>
        public static void ImportProfileSheet(string filePath)
        {
            IsCheckProfile = false;
            int startRow = 0;
            int rowCount = 0;
            bool IsValidated = false;
            IniAllProfileTableColumns();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Set the license context
            ExcelPackage package = new ExcelPackage(new FileInfo(filePath));
            try
            {
                nameplateDT.Rows.Clear();
                instantDT.Rows.Clear();
                billingDT.Rows.Clear();
                blockloadDT.Rows.Clear();
                dailyloadDT.Rows.Clear();
                voltageDT.Rows.Clear();
                currentDT.Rows.Clear();
                powerDT.Rows.Clear();
                transactionDT.Rows.Clear();
                otherDT.Rows.Clear();
                nonrolloverDT.Rows.Clear();
                controlDT.Rows.Clear();

                #region PUsh Profiles
                pushInstantDT.Rows.Clear();
                pushAlertDT.Rows.Clear();
                pushLSDT.Rows.Clear();
                pushDEDT.Rows.Clear();
                pushSelfRegDT.Rows.Clear();
                pushBillDT.Rows.Clear();
                pushTamperDT.Rows.Clear();
                #endregion

                foreach (ExcelWorksheet worksheet in package.Workbook.Worksheets)
                {
                    if (worksheet.Name.Trim() == "Guide")
                        continue;
                    if (!profileList.Contains(worksheet.Name.Trim()))
                    {
                        MessageBox.Show($"Kindly follow standard naming for {worksheet.Name}. Refer to sample sheet.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    IsValidated = ValidateWorksheet(worksheet, out rowCount);
                    if (IsValidated && worksheet.Dimension.Rows > 1)
                    {
                        startRow = 2;
                        for (int row = startRow; row <= rowCount; row++)
                        {
                            string classValue = worksheet.Cells[row, 1].Text;
                            string obis = worksheet.Cells[row, 2].Text;
                            string attribute = worksheet.Cells[row, 3].Text;
                            string scaler = worksheet.Cells[row, 4].Text;
                            string unit = worksheet.Cells[row, 5].Text;
                            string dataType = worksheet.Cells[row, 6].Text;
                            switch (worksheet.Name)
                            {
                                case "Nameplate":
                                    nameplateDT.Rows.Add(classValue, obis, attribute, scaler, unit, dataType);
                                    break;
                                case "Instantaneous":
                                    instantDT.Rows.Add(classValue, obis, attribute, scaler, unit, dataType);
                                    break;
                                case "Billing":
                                    billingDT.Rows.Add(classValue, obis, attribute, scaler, unit, dataType);
                                    break;
                                case "BlockLoad":
                                    blockloadDT.Rows.Add(classValue, obis, attribute, scaler, unit, dataType);
                                    break;
                                case "DailyLoad":
                                    dailyloadDT.Rows.Add(classValue, obis, attribute, scaler, unit, dataType);
                                    break;
                                case "VoltageEvents":
                                    voltageDT.Rows.Add(classValue, obis, attribute, scaler, unit, dataType);
                                    break;
                                case "CurrentEvents":
                                    currentDT.Rows.Add(classValue, obis, attribute, scaler, unit, dataType);
                                    break;
                                case "PowerEvents":
                                    powerDT.Rows.Add(classValue, obis, attribute, scaler, unit, dataType);
                                    break;
                                case "TransactionEvents":
                                    transactionDT.Rows.Add(classValue, obis, attribute, scaler, unit, dataType);
                                    break;
                                case "OtherEvents":
                                    otherDT.Rows.Add(classValue, obis, attribute, scaler, unit, dataType);
                                    break;
                                case "NonrolloverEvents":
                                    nonrolloverDT.Rows.Add(classValue, obis, attribute, scaler, unit, dataType);
                                    break;
                                case "ControlEvents":
                                    controlDT.Rows.Add(classValue, obis, attribute, scaler, unit, dataType);
                                    break;
                                #region Push Profiles
                                case "PushInstant":
                                    pushInstantDT.Rows.Add(classValue, obis, attribute, scaler, unit, dataType);
                                    break;
                                case "PushAlert":
                                    pushAlertDT.Rows.Add(classValue, obis, attribute, scaler, unit, dataType);
                                    break;
                                case "PushLS":
                                    pushLSDT.Rows.Add(classValue, obis, attribute, scaler, unit, dataType);
                                    break;
                                case "PushDE":
                                    pushDEDT.Rows.Add(classValue, obis, attribute, scaler, unit, dataType);
                                    break;
                                case "PushSelfRegistration":
                                    pushSelfRegDT.Rows.Add(classValue, obis, attribute, scaler, unit, dataType);
                                    break;
                                case "PushBill":
                                    pushBillDT.Rows.Add(classValue, obis, attribute, scaler, unit, dataType);
                                    break;
                                case "PushTamper":
                                    pushTamperDT.Rows.Add(classValue, obis, attribute, scaler, unit, dataType);
                                    break;
                                    #endregion
                            }
                        }
                    }
                }
                if (IsValidated)
                {
                    IsCheckProfile = true;
                    MessageBox.Show("Data imported successfully!", "Import Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message.ToString());
            }
        }
        /// <summary>
        /// Validate the work sheet for formating.
        /// </summary>
        /// <param name="worksheet"></param>
        /// <returns></returns>
        public static bool ValidateWorksheet(ExcelWorksheet worksheet, out int rowCount)
        {
            bool result = true;
            rowCount = 0;
            int colCount = 0;
            string _class = "";
            string _obis = "";
            string _attr = "";
            string _scaler = "";
            string _unit = "";
            string _datatype = "";
            try
            {
                var headerRow = worksheet.Cells["A1:F1"].Select(cell => cell.Text).ToArray();
                // Check if headers match
                bool headersMatch = columnArray.SequenceEqual(headerRow);
                if (headersMatch)
                {
                    rowCount = worksheet.Dimension.Rows;
                    //colCount = worksheet.Dimension.Columns;
                    colCount = 6;
                    for (int i = 2; i <= rowCount; i++)
                    {
                        // Read all 6 cells A to F in the current row
                        bool isEmptyRow = true;
                        for (int j = 1; j <= 6; j++)
                        {
                            var cellValue = worksheet.Cells[i, j].Value;
                            if (cellValue != null && !string.IsNullOrWhiteSpace(cellValue.ToString()))
                            {
                                isEmptyRow = false;
                                break;
                            }
                        }

                        if (isEmptyRow)
                        {
                            rowCount = i - 1; // Set actual row count till last non-empty row
                            break;
                        }
                    }
                }
                else
                {
                    MessageBox.Show($"For {worksheet.Name} profile the column headers in the selected Excel file do not match the expected headers defined in sample sheet.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                for (int i = 2; i <= rowCount; i++)
                {
                    _class = worksheet.Cells[i, 1].Value?.ToString().Trim() ?? "";
                    _obis = worksheet.Cells[i, 2].Value?.ToString().Trim() ?? "";
                    _attr = worksheet.Cells[i, 3].Value?.ToString().Trim() ?? "";
                    _scaler = worksheet.Cells[i, 4].Value?.ToString().Trim() ?? "";
                    _unit = worksheet.Cells[i, 5].Value?.ToString().Trim() ?? "";
                    _datatype = worksheet.Cells[i, 6].Value?.ToString().Trim() ?? "";

                    /*
                    _class = (worksheet.Cells[i, 1].Value != null ? worksheet.Cells[i, 1].Value.ToString().Trim() : "");
                    _obis = (worksheet.Cells[i, 2].Value != null ? worksheet.Cells[i, 2].Value.ToString().Trim() : "");
                    _attr = (worksheet.Cells[i, 3].Value != null ? worksheet.Cells[i, 3].Value.ToString().Trim() : "");
                    _scaler = (worksheet.Cells[i, 4].Value != null ? worksheet.Cells[i, 4].Value.ToString().Trim() : "");
                    _unit = (worksheet.Cells[i, 5].Value != null ? worksheet.Cells[i, 5].Value.ToString().Trim() : "");
                    _datatype = worksheet.Cells[i, 6].Value.ToString().Trim();
                    */

                    if (string.IsNullOrEmpty(_class) || string.IsNullOrEmpty(_obis) || string.IsNullOrEmpty(_attr) || string.IsNullOrEmpty(_datatype))
                        result = false;
                    if (_obis.Split('.').Length != 6)
                        result = false;
                    if (!string.IsNullOrEmpty(_scaler) && _scaler.Length < 3 && _scaler.Split(':').Length != 2 && _scaler.Split(':')[0].Length != 2)
                        result = false;
                    if (!string.IsNullOrEmpty(_unit) && _unit.Length < 3 && _unit.Split(':').Length != 2 && _unit.Split(':')[0].Length != 2)
                        result = false;
                    if (!string.IsNullOrEmpty(_datatype) && _datatype.Length < 3 && _datatype.Split(':').Length != 2 && _datatype.Split(':')[0].Length != 2)
                        result = false;
                    if (!result)
                    {
                        MessageBox.Show($"In {worksheet.Name} profile check row no. {i}. This row value is not in correct format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"Error while validating worksheet '{worksheet.Name}': {ex.Message.ToString()}");
            }
            return result;
        }
    }
}
