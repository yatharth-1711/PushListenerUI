using MeterReader.NicConfiguration.Enum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using MeterReader.NicConfiguration;

namespace MeterReader.NicConfiguration
{
    public static class FOTAHelper
    {
        public static string path { get; set; } = "";
        public static Dictionary<int, string> nicFOTAData = new Dictionary<int, string>();
        public static Dictionary<int, string> moduleFOTAData = new Dictionary<int, string>();
        /// <summary>
        /// Export the Sample XML for the FOTA Information
        /// </summary>
        public static void Export()
        {
            FOTARoot data = new FOTARoot
            {
                NICFOTA = new FOTAInfo
                {
                    MainFileName = "nic_fw.bin",
                    TestFileName = "WrongNic_fw.bin",
                    FTPUserName = "user1",
                    FTPAddress = "ftp.nicserver.com",
                    FTPPassword = "pass1",
                    FTPPort = 21,
                    FTPTransactionMode = "Passive",
                    MainFWName = "nic_fw",
                    TestFWName = "nic_test_fw"
                },
                MODULEFOTA = new FOTAInfo
                {
                    MainFileName = "module_fw.bin",
                    TestFileName = "WrongModule_fw.bin",
                    FTPUserName = "user2",
                    FTPAddress = "ftp.moduleserver.com",
                    FTPPassword = "pass2",
                    FTPPort = 21,
                    FTPTransactionMode = "Active",
                    MainFWName = "module_fw",
                    TestFWName = "module_test_fw"
                }
            };

            SaveFileDialog sfd = new SaveFileDialog
            {
                FileName = $"FOTAInformation_{DateTime.Now.ToString("ddMMyyyyHHmmss")}",
                Filter = "XML Files|*.xml"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(FOTARoot));
                using (FileStream fs = new FileStream(sfd.FileName, FileMode.Create))
                {
                    serializer.Serialize(fs, data);
                    MessageBox.Show("Exported Successfully!");
                }
            }
        }
        /// <summary>
        /// Import, Validate and Assign FOTA Information from XML File
        /// </summary>
        public static void Import()
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "XML Files|*.xml"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(FOTARoot));
                    using (FileStream fs = new FileStream(ofd.FileName, FileMode.Open))
                    {
                        FOTARoot data = (FOTARoot)serializer.Deserialize(fs);
                        string message = "";
                        Clear();
                        if (data?.NICFOTA != null && data?.MODULEFOTA != null && FOTAFileValidation(data, out message))
                        {
                            path = ofd.FileName;
                            // Optional: display or verify sub-fields
                            MessageBox.Show("Import Successful. All required data found.");
                        }
                        else
                        {
                            path = "";
                            MessageBox.Show($"Missing NIC or MODULE FOTA data.\n{message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error during import: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Import, Validate and Assign FOTA Information from XML File
        /// </summary>
        public static void Import(string filePath)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    return;
                }
                if (!File.Exists(filePath))
                {
                    return;
                }
                XmlSerializer serializer = new XmlSerializer(typeof(FOTARoot));
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    FOTARoot data = (FOTARoot)serializer.Deserialize(fs);
                    string message = "";
                    Clear();
                    if (data?.NICFOTA != null && data?.MODULEFOTA != null && FOTAFileValidation(data, out message))
                    {
                        path = filePath;
                        // Optional: display or verify sub-fields
                        //MessageBox.Show("Import Successful. All required data found.");
                    }
                    else
                    {
                        path = "";
                        //MessageBox.Show($"Missing NIC or MODULE FOTA data.\n{message}");
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error during import: " + ex.Message);
            }
        }



        /// <summary>
        /// Validate the Import information of FOTA
        /// </summary>
        /// <param name="data"> It is a De serialize XML of type FOTARoot</param>
        /// <param name="message"> It output the Error Messages</param>
        /// <returns>Return true if Validation is passed.</returns>
        public static bool FOTAFileValidation(FOTARoot data, out string message)
        {
            message = "";
            bool result = true;
            StringBuilder sb = new StringBuilder();
            int nicCount = 0;
            FOTAInfo nic = data.NICFOTA;
            if (!string.IsNullOrWhiteSpace(nic.MainFileName)) nicCount++;
            if (!string.IsNullOrWhiteSpace(nic.TestFileName)) nicCount++;
            if (!string.IsNullOrWhiteSpace(nic.FTPUserName)) nicCount++;
            if (!string.IsNullOrWhiteSpace(nic.FTPAddress)) nicCount++;
            if (!string.IsNullOrWhiteSpace(nic.FTPPassword)) nicCount++;
            if (nic.FTPPort > 0) nicCount++; // assuming port 0 is invalid
            if (!string.IsNullOrWhiteSpace(nic.FTPTransactionMode)) nicCount++;
            if (!string.IsNullOrWhiteSpace(nic.MainFWName)) nicCount++;
            if (!string.IsNullOrWhiteSpace(nic.MainFWName)) nicCount++;

            int moduleCount = 0;
            FOTAInfo module = data.MODULEFOTA;
            if (!string.IsNullOrWhiteSpace(module.MainFileName)) moduleCount++;
            if (!string.IsNullOrWhiteSpace(module.TestFileName)) moduleCount++;
            if (!string.IsNullOrWhiteSpace(module.FTPUserName)) moduleCount++;
            if (!string.IsNullOrWhiteSpace(module.FTPAddress)) moduleCount++;
            if (!string.IsNullOrWhiteSpace(module.FTPPassword)) moduleCount++;
            if (nic.FTPPort > 0) moduleCount++; // assuming port 0 is invalid
            if (!string.IsNullOrWhiteSpace(module.FTPTransactionMode)) moduleCount++;
            if (!string.IsNullOrWhiteSpace(module.MainFWName)) moduleCount++;
            if (!string.IsNullOrWhiteSpace(module.MainFWName)) moduleCount++;

            if (nicCount != 9 && moduleCount != 9)
            {
                message = "File format not Correct. Kindly Check reference Exported XML File.";
                return false;
            }
            nicFOTAData.Add((int)FOTAEnumInfo.MainFileName, nic.MainFileName.Trim());
            nicFOTAData.Add((int)FOTAEnumInfo.TestFileName, nic.TestFileName.Trim());
            nicFOTAData.Add((int)FOTAEnumInfo.FTPUser, nic.FTPUserName.Trim());
            nicFOTAData.Add((int)FOTAEnumInfo.FTPAddress, nic.FTPAddress.Trim());
            nicFOTAData.Add((int)FOTAEnumInfo.FTPPassword, nic.FTPPassword.Trim());
            nicFOTAData.Add((int)FOTAEnumInfo.FTPPort, nic.FTPPort.ToString());
            nicFOTAData.Add((int)FOTAEnumInfo.FTPTransactionMode, nic.FTPTransactionMode.Trim());
            nicFOTAData.Add((int)FOTAEnumInfo.MainFWName, nic.MainFWName.Trim());
            nicFOTAData.Add((int)FOTAEnumInfo.TestFWName, nic.TestFWName.Trim());

            moduleFOTAData.Add((int)FOTAEnumInfo.MainFileName, module.MainFileName.Trim());
            moduleFOTAData.Add((int)FOTAEnumInfo.TestFileName, module.TestFileName.Trim());
            moduleFOTAData.Add((int)FOTAEnumInfo.FTPUser, module.FTPUserName.Trim());
            moduleFOTAData.Add((int)FOTAEnumInfo.FTPAddress, module.FTPAddress.Trim());
            moduleFOTAData.Add((int)FOTAEnumInfo.FTPPassword, module.FTPPassword.Trim());
            moduleFOTAData.Add((int)FOTAEnumInfo.FTPPort, module.FTPPort.ToString());
            moduleFOTAData.Add((int)FOTAEnumInfo.FTPTransactionMode, module.FTPTransactionMode.Trim());
            moduleFOTAData.Add((int)FOTAEnumInfo.MainFWName, module.MainFWName.ToString());
            moduleFOTAData.Add((int)FOTAEnumInfo.TestFWName, module.TestFWName.Trim());

            List<Dictionary<int, string>> list = new List<Dictionary<int, string>>();
            list.Add(nicFOTAData);
            list.Add(moduleFOTAData);
            string infoText = "";

            for (int i = 1; i <= list.Count; i++)
            {
                Dictionary<int, string> checkInputDetails = new Dictionary<int, string>();
                if (i == 1)
                {
                    infoText = "NIC";
                    checkInputDetails = nicFOTAData;
                }
                else
                {
                    infoText = "MODULE";
                    checkInputDetails = moduleFOTAData;
                }
                foreach (var item in checkInputDetails)
                {
                    switch (item.Key)
                    {
                        case (int)FOTAEnumInfo.MainFileName:
                            if (infoText == "NIC" && (!(item.Value.Length >= 10 && item.Value.Length <= 30)) ||
                                infoText == "MODULE" && (!(item.Value.Length >= 15 && item.Value.Length <= 70)))

                            {
                                sb.AppendLine($"{infoText} FOTA FW File Name Length should be between 10 to 30.");
                                result = false;
                            }
                            break;
                        case (int)FOTAEnumInfo.TestFileName:
                            if (infoText == "NIC" && (!(item.Value.Length >= 10 && item.Value.Length <= 30)) ||
                                infoText == "MODULE" && (!(item.Value.Length >= 15 && item.Value.Length <= 70)))
                            {
                                sb.AppendLine($"{infoText} FOTA Wrong FW File Name Length should be between 10 to 30.");
                                result = false;
                            }
                            break;
                        case (int)FOTAEnumInfo.FTPUser:
                            if (!(item.Value.Length > 0) && !(item.Value.Length <= 10))
                            {
                                sb.AppendLine($"{infoText} FOTA FTP User Name Length should be between 1 to 10.");
                                result = false;
                            }
                            break;
                        case (int)FOTAEnumInfo.FTPAddress:
                            string address = item.Value;
                            if ((address.Length >= 15 && address.Contains(".")))
                            {
                                if (address.Count(c => c == '.') != 3)
                                {
                                    sb.AppendLine($"{infoText} FOTA FTP IPv4 Address format is not valid");
                                    result = false;
                                }
                            }
                            else if ((address.Length >= 39 && address.Contains(":")))
                            {
                                if (address.Count(c => c == ':') < 5)
                                {
                                    sb.AppendLine($"{infoText} FOTA FTP IPv6 Address format is not valid");
                                    result = false;
                                }
                            }
                            break;
                        case (int)FOTAEnumInfo.FTPPassword:
                            if (!(item.Value.Length > 0) && !(item.Value.Length <= 10))
                            {
                                sb.AppendLine($"{infoText} FOTA FTP Password Length should be between 1 to 10.");
                                result = false;
                            }
                            break;
                        case (int)FOTAEnumInfo.FTPPort:
                            if (!(item.Value.Length > 0))
                            {
                                sb.AppendLine($"{infoText} FOTA FTP Port Length.");
                                result = false;
                            }
                            break;
                        case (int)FOTAEnumInfo.FTPTransactionMode:
                            if ((item.Value != "Active" && item.Value != "Passive"))
                            {
                                sb.AppendLine($"{infoText} FOTA Transaction Mode Incorrect.");
                                result = false;
                            }
                            break;
                        case (int)FOTAEnumInfo.MainFWName:
                            if (infoText == "NIC" && (!(item.Value.Length >= 10 && item.Value.Length <= 30)) ||
                                infoText == "MODULE" && (!(item.Value.Length >= 15 && item.Value.Length <= 70)))
                            {
                                sb.AppendLine($"{infoText} FOTA FW Length should be between 10 to 30.");
                                result = false;
                            }
                            break;
                        case (int)FOTAEnumInfo.TestFWName:
                            if (infoText == "NIC" && (!(item.Value.Length >= 10 && item.Value.Length <= 30)) ||
                                infoText == "MODULE" && (!(item.Value.Length >= 15 && item.Value.Length <= 70)))
                            {
                                sb.AppendLine($"{infoText} FOTA Test FW Length should be between 10 to 30.");
                                result = false;
                            }
                            break;
                    }
                }
            }
            message = sb.ToString();
            return result;
        }
        /// <summary>
        /// Clear the Dictionary nicFOTAData, moduleFOTAData and path of the FOTA information File
        /// </summary>
        public static void Clear()
        {
            nicFOTAData.Clear();
            moduleFOTAData.Clear();
            path = "";
        }

        public static bool IsNICFOTADataAvailable()
        {
            bool result = true;
            if (string.IsNullOrEmpty(path) || nicFOTAData.Count < 9)
                result = false;
            return result;
        }

        public static bool IsModuleFOTADataAvailable()
        {
            bool result = true;
            if (string.IsNullOrEmpty(path) || moduleFOTAData.Count < 9)
                result = false;
            return result;
        }
    }
}
