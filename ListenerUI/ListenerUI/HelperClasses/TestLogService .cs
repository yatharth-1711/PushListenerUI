using AutoTestDesktopWFA;
using log4net;
using MeterReader.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace MeterReader.TestHelperClasses
{
    public class TestLogService : ITestLogger
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        //Delegate for Execution traffic messages
        public delegate void AppendColoredTextControl(string message, Color color, bool isBold = false);
        public static event AppendColoredTextControl AppendColoredTextControlEventHandler = delegate { }; // add empty delegate!;

        #region Properties and Constructor
        public string LOG_DIRECTORY { get; set; } = @"C:\IndaliDlmsTestLogs";
        private readonly RichTextBox _logBox;
        public string testLogFileName { get; set; } = "";
        public TestLogService(RichTextBox logBox)
        {
            _logBox = logBox;
            InitializeLogDirectory();
        }

        public TestLogService()
        {
        }
        /*
        public List<TestResult> GetTestHistory()
        {
            List<TestResult> results = new List<TestResult>();
            try
            {
                string[] reportFiles = Directory.GetFiles(Path.Combine(LOG_DIRECTORY, "TestReports"), "TestReport_*.txt");
                foreach (string file in reportFiles)
                {
                    // Parse test report files and create TestResult objects
                    string[] lines = File.ReadAllLines(file);
                    TestResult result = ParseTestReport(lines);
                    results.Add(result);
                }
            }
            catch (Exception ex)
            {
                LogMessage(_logBox, $"Error reading test history: {ex.Message}", Color.Red);
            }
            return results;
        }
        //public List<TestResult> GetAllTestResults()
        //{
        //    List<TestResult> results = new List<TestResult>();
        //    try
        //    {
        //        string[] reportFiles = Directory.GetFiles(Path.Combine(LOG_DIRECTORY, "TestReports"), "TestReport_*.txt");
        //        foreach (string file in reportFiles)
        //        {
        //            // Parse test report files and create TestResult objects
        //            string[] lines = File.ReadAllLines(file);
        //            //TestResult result = ParseTestReport(lines);
        //            TestResult result = GetTestResult(lines);

        //            results.Add(result);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogMessage(_logBox, $"Error reading test history: {ex.Message}", Color.Red);
        //    }
        //    return results;
        //}
        public List<TestResult> GetAllTestResults()
        {
            List<TestResult> results = new List<TestResult>();

            try
            {
                string reportDir = Path.Combine(LOG_DIRECTORY, "TestReports");

                if (!Directory.Exists(reportDir))
                {
                    //LogMessage(_logBox, "Test report directory does not exist.", Color.Orange);
                    log.Info("Test report directory does not exist.");
                    return results;
                }

                // Get files sorted by last modified date (newest first)
                var reportFiles = new DirectoryInfo(reportDir)
                    .GetFiles("TestReport_*.txt")
                    .OrderBy(f => f.CreationTime)
                    .Select(f => f.FullName)
                    .ToList();

                foreach (string file in reportFiles)
                {
                    try
                    {
                        string[] lines = File.ReadAllLines(file);
                        TestResult result = GetTestResult(lines);
                        results.Add(result);
                    }
                    catch (Exception fileEx)
                    {
                        LogMessage(_logBox, $"Error reading file {Path.GetFileName(file)}: {fileEx.Message}", Color.Red);
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage(_logBox, $"Error reading test history: {ex.Message}", Color.Red);
            }
            return results;
        }
        */
        #endregion

        private void InitializeLogDirectory()
        {
            if (!Directory.Exists(LOG_DIRECTORY))
            {
                Directory.CreateDirectory(LOG_DIRECTORY);
            }
        }
        public void LogMessage(RichTextBox logBox, string message, Color color, bool isBold = false)
        {
            // Log to RichTextBox
            LogToRichTextBox(logBox, message, color, isBold);
            // Log to file
            //LogToFile(message);
            // Log to Rich Text File
            //LogToRtfFile();
        }
        private void LogToFile(string message)
        {
            //string logFile = Path.Combine(LOG_DIRECTORY, $"TestLog_{DateTime.Now:ddMMyyyy}.txt");
            string logFile = Path.Combine(LOG_DIRECTORY, $"{testLogFileName}.txt");
            if (!Directory.Exists(LOG_DIRECTORY))
            {
                Directory.CreateDirectory(LOG_DIRECTORY);
            }
            string logMessage = $"[{DateTime.Now:dd-MM-yyyy hh:mm:ss:fff tt}] {message}";
            File.AppendAllText(logFile, logMessage + Environment.NewLine);
        }
        public void LogToRtfFile()
        {
            try
            {
                //string filePath = Path.Combine(LOG_DIRECTORY, $"TestLog_{DateTime.Now:ddMMyyyy}.rtf");
                string filePath = Path.Combine(LOG_DIRECTORY, $"{testLogFileName}.rtf");
                if (File.Exists(filePath))
                {
                    // Use a temporary RichTextBox to load existing RTF content
                    using (System.Windows.Forms.RichTextBox tempRtb = new System.Windows.Forms.RichTextBox())
                    {

                        // Append the current RichTextBox content
                        tempRtb.SelectedRtf = _logBox.Rtf;

                        // Save the combined content back to the file
                        tempRtb.SaveFile(filePath, RichTextBoxStreamType.RichText);
                    }
                }
                else
                {
                    _logBox.SaveFile(filePath, RichTextBoxStreamType.RichText);
                }
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message.ToString()} => TRACE: {ex.StackTrace.ToString()}");
            }
        }
        private void LogToRichTextBox(RichTextBox logBox, string message, Color color, bool isBold = false)
        {
            AppendColoredTextControlEventHandler(message, color, isBold);
            //if (logBox.InvokeRequired)
            //{
            //    logBox.Invoke(new Action(() => AppendColoredText(logBox, message, color, isBold)));
            //}
            //else
            //{
            //    AppendColoredText(logBox, message, color, isBold);
            //}
        }
        private void AppendColoredText(RichTextBox richTextBox, string message, Color color, bool isBold = false)
        {
            richTextBox.SelectionStart = richTextBox.TextLength;
            richTextBox.SelectionLength = 0;
            richTextBox.SelectionColor = color;
            if (isBold)
                richTextBox.SelectionFont = new Font("Courier New", 11f, FontStyle.Bold);
            else
                richTextBox.SelectionFont = new Font("Courier New", 11f, FontStyle.Regular);
            richTextBox.AppendText(message + Environment.NewLine);
            richTextBox.SelectionColor = richTextBox.ForeColor;
            richTextBox.ScrollToCaret();
        }
        /*
        public void SaveTestReport(TestResult result)
        {
            string reportFile = Path.Combine(LOG_DIRECTORY, "TestReports",
            $"TestReport_{result.TestName}_{DateTime.Now:ddMMyyyyHHmmss}.txt");

            var report = new StringBuilder()
                .AppendLine($"Test Report - {result.TestName}")
                .AppendLine($"Status: {(result.Passed ? "PASSED" : "FAILED")}")
                .AppendLine($"Test Start Date & Time: {result.TestStartTime:dd/MM/yyyy hh:mm:ss:fff tt}")
                .AppendLine($"Test End Date & Time: {result.TestEndTime:dd/MM/yyyy hh:mm:ss:fff tt}")
                .AppendLine($"Test Execution Time: {result.ExecutionTimeMs} ms")
                .AppendLine($"Result Message: {result.ResultMessage}")
                .AppendLine($"Is Skipped: {(result.Skipped ? "Yes" : "No")}")
                .AppendLine($"{(result.MessagesList.Count > 0 ? $"Test Messages: \n{string.Join("\n", result.MessagesList)}" : "")}")
                .ToString();
            File.WriteAllText(reportFile, report);
        }
        private TestResult ParseTestReport(string[] lines)
        {
            var result = new TestResult();
            foreach (string line in lines)
            {
                if (line.StartsWith("Test Report - "))
                    result.TestName = line.Replace("Test Report - ", "");
                else if (line.StartsWith("Status: "))
                    result.Passed = line.Contains("PASSED");
                else if (line.StartsWith("Result Message: "))
                    result.ResultMessage = line.Replace("Result Message: ", "");

            }
            return result;
        }
        private TestResult GetTestResult(string[] lines)
        {
            var result = new TestResult();
            foreach (string line in lines)
            {
                if (line.StartsWith("Test Report - "))
                    result.TestName = line.Replace("Test Report - ", "");
                else if (line.StartsWith("Status: "))
                    result.Passed = line.Contains("PASSED");
                else if (line.StartsWith("Result Message: "))
                    result.ResultMessage = line.Replace("Result Message: ", "");
                else if (line.StartsWith("Test Start Date & Time:"))
                    result.TestStartTime = DateTime.ParseExact((line.Replace("Test Start Date & Time: ", "")).Trim(), "dd/MM/yyyy hh:mm:ss:fff tt", CultureInfo.InvariantCulture);
                else if (line.StartsWith("Test End Date & Time:"))
                    result.TestEndTime = DateTime.ParseExact((line.Replace("Test End Date & Time: ", "")).Trim(), "dd/MM/yyyy hh:mm:ss:fff tt", CultureInfo.InvariantCulture);
                else if (line.StartsWith("Test Execution Time: "))
                {
                    string temp = line.Replace("Test Execution Time: ", "");
                    result.ExecutionTimeMs = Convert.ToInt64(temp.Substring(0, temp.Length - 2).Trim());
                }
                else if (line.StartsWith("Is Skipped: "))
                    result.Skipped = line.Contains("No") ? false : true;
            }
            return result;
        }
        
        public void SaveAllTestReport()
        {
            var testResultList = GetAllTestResults();
            Int32 totalTest = testResultList.Count;
            Int32 testPassed = 0, testFailed = 0, testskipped = 0;
            foreach (var testResult in testResultList)
            {
                if (testResult.Passed)
                    testPassed++;
                else
                    testFailed++;
                if (testResult.Skipped)
                    testskipped++;
            }
            string resultFile = Path.Combine(LOG_DIRECTORY, $"TestingSummary_{DateTime.Now:ddMMyyyyHHmmss}.txt");
            #region Header
            File.AppendAllText(resultFile, "********************************************************************************" + Environment.NewLine);
            File.AppendAllText(resultFile, "INDALI DLMS TEST REPORT" + Environment.NewLine);
            File.AppendAllText(resultFile, $"INDALI VERSION {"1.0.0"}" + Environment.NewLine);
            File.AppendAllText(resultFile, $"{DateTime.Now:dd-MMM-yyyy hh:mm:ss tt}" + Environment.NewLine);
            File.AppendAllText(resultFile, "********************************************************************************" + Environment.NewLine);
            File.AppendAllText(resultFile, Environment.NewLine);
            #endregion

            #region Identification
            MeterIdentity.PrintIdentificationToSummary(resultFile);
            #endregion

            #region HDLC and Data Link Layer Info
            //MeterIdentity.PrintHDLCDataLinkToSummary(resultFile);
            #endregion

            File.AppendAllText(resultFile, $"***********" + Environment.NewLine);
            File.AppendAllText(resultFile, $"* Summary *" + Environment.NewLine);
            File.AppendAllText(resultFile, $"***********" + Environment.NewLine);
            File.AppendAllText(resultFile, Environment.NewLine);
            File.AppendAllText(resultFile, $"TOTAL\t\t\tPASSED\t\t\tFAILED\t\t\tSKIPPED" + Environment.NewLine);
            File.AppendAllText(resultFile, $"-----\t\t\t------\t\t\t-----\t\t\t-----" + Environment.NewLine);
            File.AppendAllText(resultFile, $"{totalTest}\t\t\t\t{testPassed}\t\t\t\t{testFailed - testskipped}\t\t\t\t{testskipped}" + Environment.NewLine);
            File.AppendAllText(resultFile, Environment.NewLine);

            Int32 testNumber = 0;
            foreach (var test in testResultList)
            {
                testNumber++;
                File.AppendAllText(resultFile, $"***************" + Environment.NewLine);
                File.AppendAllText(resultFile, $"Test {testNumber} of {totalTest}" + Environment.NewLine);
                File.AppendAllText(resultFile, $"***************" + Environment.NewLine);
                File.AppendAllText(resultFile, $"\tTest Name:\t\t\t\t\t{test.TestName}" + Environment.NewLine);
                File.AppendAllText(resultFile, $"\tTest Result:\t\t\t\t{(test.Passed ? "PASSED" : "FAILED")}" + Environment.NewLine);
                File.AppendAllText(resultFile, $"\tResult Message:\t\t\t\t{test.ResultMessage}" + Environment.NewLine);
                File.AppendAllText(resultFile, $"\tTest Start Date & Time:\t\t{test.TestStartTime:dd/MM/yyyy hh:mm:ss:fff tt}" + Environment.NewLine);
                File.AppendAllText(resultFile, $"\tTest End Date & Time:\t\t{test.TestEndTime:dd/MM/yyyy hh:mm:ss:fff tt}" + Environment.NewLine);
                File.AppendAllText(resultFile, $"\tTest Execution Time:\t\t{test.ExecutionTimeMs} ms" + Environment.NewLine);
                File.AppendAllText(resultFile, $"\tTest Skipped:\t\t\t\t{(test.Skipped ? "SKIPPED" : "No")}" + Environment.NewLine);
                File.AppendAllText(resultFile, Environment.NewLine);
            }

            #region Failed Cases
            bool IsFailed = false;
            foreach (var test in testResultList)
            {
                if (!test.Passed && !test.Skipped)
                {
                    IsFailed = true;
                    break;
                }
            }
            if (IsFailed)
            {
                File.AppendAllText(resultFile, $"***********************" + Environment.NewLine);
                File.AppendAllText(resultFile, $"** Failed Test Cases **" + Environment.NewLine);
                File.AppendAllText(resultFile, $"***********************" + Environment.NewLine);
                testNumber = 0;
                foreach (var test in testResultList)
                {
                    testNumber++;
                    if (!test.Passed && !test.Skipped)
                    {
                        File.AppendAllText(resultFile, $"{testNumber}.\t{test.TestName}" + Environment.NewLine);
                    }
                }
                File.AppendAllText(resultFile, Environment.NewLine);
            }
            #endregion

            #region Skipped Print
            bool IsSkipped = false;
            foreach (var test in testResultList)
            {
                if (test.Skipped)
                {
                    IsSkipped = true;
                    break;
                }
            }
            if (IsSkipped)
            {
                File.AppendAllText(resultFile, $"**********************" + Environment.NewLine);
                File.AppendAllText(resultFile, $"* Skipped Test Cases *" + Environment.NewLine);
                File.AppendAllText(resultFile, $"**********************" + Environment.NewLine);
                testNumber = 0;
                foreach (var test in testResultList)
                {
                    testNumber++;
                    if (test.Skipped || test.ResultMessage.Contains("SKIPPED"))
                    {
                        File.AppendAllText(resultFile, $"{testNumber}.\t{test.TestName}" + Environment.NewLine);
                    }
                }
                File.AppendAllText(resultFile, Environment.NewLine);
            }
            #endregion

            #region HDLC and Data Link Layer Info
            //MeterIdentity.PrintSystemDetailsToSummary(resultFile);
            #endregion
        }*/
        public string FormatLogLine(string label, string status, params string[] values)
        {
            string line = label.PadRight(15) + "|";

            foreach (var val in values)
            {
                if (double.TryParse(val, out double num))
                    line += $" {num,15:F3} |";
                else
                    line += $" {val,15} |";
            }
            line += $" {status}";

            return line;
        }
        public string FormatLogLineString(string label, params string[] values)
        {
            string line = label.PadRight(30) + "|";
            foreach (var val in values)
            {
                line += $" {val,25} |";
            }
            return line;
        }
        public string FormatLogLineString(string label, string[] values, params int[] columnWidths)
        {
            string line = label.PadRight(30) + "|";
            for (int i = 0; i < values.Length; i++)
            {
                int width = (i < columnWidths.Length) ? columnWidths[i] : 25;
                string val = values[i] ?? string.Empty;

                line += " " + val.PadLeft(width) + " |";
            }
            return line;
        }

        public void SaveTestReport(TestResult result)
        {
            throw new NotImplementedException();
        }

        public List<TestResult> GetTestHistory()
        {
            throw new NotImplementedException();
        }
    }
}
