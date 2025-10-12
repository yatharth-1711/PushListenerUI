using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MeterReader.Interfaces
{
    public interface IDlmsTest
    {
        string TestName { get; }
        string TestPurpose { get; }
        string TestStrategy { get; }
        string TestExpectedResult { get; }
        bool TestRequiredAPSource { get; }
        bool TestRequiredDataReset { get; }
        TestResult ExecuteTest(RichTextBox logBox, CancellationToken token);
    }
    public class TestResult
    {
        public string TestName { get; set; }
        public bool Passed { get; set; }
        public List<string> MessagesList { get; set; } = new List<string>();
        public string ResultMessage { get; set; }
        public long ExecutionTimeMs { get; set; }
        public DateTime TestStartTime { get; set; }
        public DateTime TestEndTime { get; set; }
        public Exception Exception { get; set; } // If you want to capture detailed exception information
        public bool Skipped { get; set; } = false;//Default Value of Skip is false.
    }
}
