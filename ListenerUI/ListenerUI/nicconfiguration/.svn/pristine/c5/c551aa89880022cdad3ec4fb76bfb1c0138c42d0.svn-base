using AutoTest.FrameWork.Converts;
using Gurux.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace MeterReader.NicConfiguration
{
    public class NicConfigIDs
    {
        #region Properties
        public static Dictionary<int, string> refIds { get; } = new Dictionary<int, string>();
        public static Dictionary<int, string> currentNicIds { get; set; } = new Dictionary<int, string>();
        public static Dictionary<string, Dictionary<int, string>> currentNICDetails { get; set; } = new Dictionary<string, Dictionary<int, string>>();
        public static Dictionary<string, Dictionary<int, string>> currentNWDetails { get; set; } = new Dictionary<string, Dictionary<int, string>>();
        #endregion

        #region Constructor
        public NicConfigIDs()
        {
            LoadRefNICConfigIds();

        }
        public static void LoadRefNICConfigIds()
        {
            // Construct the file path dynamically
            string xmlFilePath = Path.Combine(Application.StartupPath, "RefXML", "NICConfigIds.xml");
            if (!File.Exists(xmlFilePath))
            {
                MessageBox.Show("XML file not found: " + xmlFilePath);
                return;
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFilePath);

            XmlNodeList records = xmlDoc.SelectNodes("//parameter");

            foreach (XmlNode record in records)
            {
                if (record.Attributes["id"] != null)
                {
                    int id = int.Parse(record.Attributes["id"].Value);
                    string name = record.SelectSingleNode("name")?.InnerText ?? "";
                    string info = record.SelectSingleNode("info")?.InnerText ?? "";

                    refIds[id] = $"{name}|{info}";
                }
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get the all the refIds
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string> GetRefIds() { return refIds; }
        /// <summary>
        /// Get the all the currentNicIds
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string> GetCurrentIds() { return currentNicIds; }



        /// <summary>
        /// Clear all existing Ids in currentNicIds and insert the new ids from passed data.
        /// </summary>
        /// <param name="IdsArrayData"> It should be DLMS Array type data in HEX format</param>
        public void AssignAllCurrentConfigNICIds(string IdsArrayData)
        {
            IdsArrayData = IdsArrayData.Trim().Replace(" ", "");
            if (IdsArrayData.Substring(2, 2) == "00")
                return;
            currentNicIds.Clear();
            string[] splittedIds = Regex.Split(IdsArrayData, "020211");
            for (int i = 1; i < splittedIds.Length; i++)
            {
                currentNicIds.Add(int.Parse(splittedIds[i].Substring(0, 2), NumberStyles.HexNumber),
                    splittedIds[i].Substring(6));
            }
        }
        public void AssignAllCurrentNicFwInfo(string hexString)
        {
            //need to read from 0.0.0.2.128.255
            hexString = hexString.Trim().Replace(" ", "");
            if (hexString.Substring(2, 2) == "00")
                return;
            currentNICDetails.Clear();
            string[] splittedIds = Regex.Split(hexString, "020211");
            for (int i = 1; i < splittedIds.Length; i++)
            {
                string identifier = splittedIds[i].Substring(0, 2);
                int innerKey = int.Parse(splittedIds[i].Substring(0, 2), NumberStyles.HexNumber);
                string innerValue = splittedIds[i].Substring(6);
                if (identifier == "32") currentNICDetails.Add("IMEI", new Dictionary<int, string> { { innerKey, innerValue } });
                else if (identifier == "33") currentNICDetails.Add("SIM", new Dictionary<int, string> { { innerKey, innerValue } });
                else if (identifier == "34") currentNICDetails.Add("NICFW", new Dictionary<int, string> { { innerKey, innerValue } });
                else if (identifier == "35") currentNICDetails.Add("ModuleFW", new Dictionary<int, string> { { innerKey, innerValue } });
                else if (identifier == "38") currentNICDetails.Add("RSSI", new Dictionary<int, string> { { innerKey, innerValue } });
                else if (identifier == "3B") currentNICDetails.Add("NWStatus", new Dictionary<int, string> { { innerKey, innerValue } });
                else if (identifier == "40") currentNICDetails.Add("NICFOTAStatus", new Dictionary<int, string> { { innerKey, innerValue } });
            }
        }
        public void AssignAllCurrentNWInfo(string hexString)
        {
            //need to read from 0.0.96.12.131.255
            // Till now, 9 parameters are defined in Network Information.
            hexString = hexString.Trim().Replace(" ", "");
            if (hexString.Substring(2, 2) == "00")
                return;
            currentNWDetails.Clear();
            DLMSParser parse = new DLMSParser();
            List<string> networkInfoList = new List<string>(parse.GetStructureValueList(hexString));
            //string[] splittedIds = Regex.Split(hexString, "0A");
            for (int i = 0; i < networkInfoList.Count; i++)
            {
                string identifier = networkInfoList[i].Substring(0, 2);
                int lengthOfData = int.Parse(networkInfoList[i].Substring(2, 2), NumberStyles.HexNumber);
                string innerValue = networkInfoList[i].Substring(2, lengthOfData * 2);

                if (i == 0) currentNWDetails.Add("CellID", new Dictionary<int, string> { { lengthOfData, innerValue } });
                else if (i == 1) currentNWDetails.Add("RSRP", new Dictionary<int, string> { { lengthOfData, innerValue } });
                else if (i == 2) currentNWDetails.Add("RSRQ", new Dictionary<int, string> { { lengthOfData, innerValue } });
                else if (i == 3) currentNWDetails.Add("RSSIdBm", new Dictionary<int, string> { { lengthOfData, innerValue } });
                else if (i == 4) currentNWDetails.Add("SNR", new Dictionary<int, string> { { lengthOfData, innerValue } });
                else if (i == 5) currentNWDetails.Add("FreqBand", new Dictionary<int, string> { { lengthOfData, innerValue } });
                else if (i == 6) currentNWDetails.Add("LAC", new Dictionary<int, string> { { lengthOfData, innerValue } });
                else if (i == 7) currentNWDetails.Add("MCCMNC", new Dictionary<int, string> { { lengthOfData, innerValue } });
                else if (i == 8) currentNWDetails.Add("ECL", new Dictionary<int, string> { { lengthOfData, innerValue } });
            }
        }

        /// <summary>
        /// If passed id contains then update the value else add as new id in currentNicIds
        /// </summary>
        /// <param name="idsToUpdate">If should be key value pair of (id in int, value in HEX String)</param>
        public void UpdateSelectedIds(Dictionary<int, string> idsToUpdate)
        {
            foreach (var selectedId in idsToUpdate)
            {
                if (currentNicIds.ContainsKey(selectedId.Key))
                {
                    currentNicIds[selectedId.Key] = selectedId.Value;
                }
                else
                {
                    currentNicIds.Add(selectedId.Key, selectedId.Value);
                }
            }
        }

        /// <summary>
        /// If IdsArray not passed then it will make Pkt of all ids available in currentNicIds else make Pkt of passed IdsArray.
        /// </summary>
        /// <param name="notSet">This provide which Ids not available in currentNicIds for set.</param>
        /// <param name="IdsArray">Int Array of ids which you want to set. </param>
        /// <returns>Set data string in HEX format</returns>
        public string MakeIdsPkt(out List<int> notSet, int[] IdsArray = null)
        {
            notSet = new List<int>();
            StringBuilder sb = new StringBuilder();
            sb.Append("01");
            if (IdsArray == null)
            {
                sb.Append(currentNicIds.Count.ToString("X2"));
                foreach (var id in currentNicIds)
                {
                    sb.Append($"020211{id.Key}09{(id.Value.Length / 2).ToString("X2")}{id.Value}");
                }
            }
            else
            {
                int idsCount = IdsArray.Length;
                StringBuilder sb2 = new StringBuilder();
                foreach (int id in IdsArray)
                {
                    if (currentNicIds.ContainsKey(id))
                    {
                        string hexValue = currentNicIds.FirstOrDefault(x => x.Key == id).Value;
                        sb2.Append($"020211{id.ToString("X2")}09{(hexValue.Length / 2).ToString("X2")}{hexValue}");
                    }
                    else
                    {
                        notSet.Add(id);
                    }
                }
                if (notSet.Count > 0)
                {
                    idsCount = idsCount - notSet.Count;
                }
                sb.Append(idsCount.ToString("X2"));
                sb.Append(sb2.ToString());
            }
            return sb.ToString();
        }

        /// <summary>
        /// Clear All currentNicIds
        /// </summary>
        public void ClearCurrentNicIds()
        {
            currentNicIds.Clear();
        }
        /// <summary>
        /// If number of ids to set is greater than this will brake into Pkt list.
        /// </summary>
        /// <param name="IdsArrayData"></param>
        /// <returns>List of Set Data String for Ids</returns>
        public List<String> MakePktList(string IdsArrayData)
        {
            List<String> list = new List<String>();
            IdsArrayData = IdsArrayData.Trim().Replace(" ", "");
            if (IdsArrayData.Substring(2, 2) == "00")
                return list;
            List<string> IdsList = Regex.Split(IdsArrayData, "020211").Skip(1).ToArray().ToList();
            if (IdsList.Count > 9)
            {
                int numberofPkt = Convert.ToInt32(Math.Ceiling(IdsList.Count / 9.0));
                int count = 0;
                int runCount = 0;

                for (int i = 1; i <= numberofPkt; i++)
                {
                    StringBuilder sb = new StringBuilder();
                    if (IdsList.Count > 9)
                    {
                        sb.Append("0109");
                        runCount = 9;
                    }
                    else
                    {
                        sb.Append($"01{IdsList.Count.ToString("X2")}");
                        runCount = IdsList.Count;
                    }
                    for (int j = 0; j < runCount; j++)
                    {
                        sb.Append($"020211{IdsList[0]}");
                        IdsList.RemoveAt(0);
                        count++;
                    }
                    list.Add(sb.ToString());
                }
            }
            else
            {
                list.Add(IdsArrayData);
            }
            return list;
        }


        #endregion
    }
}
