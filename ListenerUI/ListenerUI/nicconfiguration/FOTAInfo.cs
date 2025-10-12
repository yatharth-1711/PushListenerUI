using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeterReader.NicConfiguration
{
    public class FOTAInfo
    {
        public string MainFileName { get; set; }
        public string TestFileName { get; set; }
        public string FTPUserName { get; set; }
        public string FTPAddress { get; set; }
        public string FTPPassword { get; set; }
        public int FTPPort { get; set; }
        public string FTPTransactionMode { get; set; }
        public string MainFWName { get; set; }
        public string TestFWName { get; set; }
    }
    public class FOTARoot
    {
        public FOTAInfo NICFOTA { get; set; }
        public FOTAInfo MODULEFOTA { get; set; }
    }
}
