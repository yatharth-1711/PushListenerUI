using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MeterReader.Interfaces
{
    public interface ITestLogger
    {
        void LogMessage(RichTextBox logBox, string message, Color color, bool isBold = false);
        void SaveTestReport(TestResult result);
        List<TestResult> GetTestHistory();
    }
}
