using System;
using System.Data;
using System.Windows.Forms;

namespace MeterReader.DLMSNetSerialCommunication
{
    public static class WrapperInfo
    {
        public static bool IsCommDelayRequired { get; set; } = false;
        public static bool IsLogXML { get; set; } = true;
        public static DataTable RefDataTable { get; } = new DataTable()
        {
            Columns =
            {
                new DataColumn("Time Stamp", typeof(string)),
                new DataColumn("Ciphered Data", typeof(string)),
                new DataColumn("Without Ciphered", typeof(DateTime)),
                new DataColumn("Packet Type", typeof(bool))
            }
        };

        #region Network Info
        public static int clientAddress { get; set; } = 1;
        public static int serverAddress { get; set; } = 16;
        public static string hostName { get; set; } = "2401:4900:983a:aad4::2";
        public static int port { get; set; } = 4059;
        public static string ModuleType { get; set; } = "";
        public static string NICFotaPathCorrect { get; set; } = "";
        public static string NICFotaPathInCorrect { get; set; } = "";
        public static string ModuleFotaPathCorrect { get; set; } = "";
        public static string ModuleFotaPathInCorrect { get; set; } = "";
        #endregion

        public static string GetFiePath()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "All files (*.*)|*.*";
            openFileDialog1.Title = "Select File";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog1.FileName;
                return filePath;
            }
            return "";
        }
    }
}
