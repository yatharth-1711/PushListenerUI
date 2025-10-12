using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeterComm
{
    class DLMSClass
    {
        private const UInt16 PPPINITFCS16 = 0xffff;
        private const UInt16 PPPGOODFCS16 = 0xf0b8;
        private const UInt16 P = 0x8408;
        private UInt16[] fcstab = new UInt16[256];

        public DLMSClass()
        {
            Fcs_Tab();
        }

        private void Fcs_Tab()
        {
            UInt16 b, v;
            Int16 i;
            for (b = 0; ;)
            {
                v = b;
                for (i = 8; Convert.ToBoolean(i--);)
                    v = Convert.ToUInt16(Convert.ToBoolean(v & Convert.ToUInt16(1)) ? (v >> Convert.ToUInt16(1)) ^ P : v >> Convert.ToUInt16(1));
                fcstab[b] = Convert.ToUInt16(v & Convert.ToUInt16(0xffff));
                if (++b == 256)
                    break;
            }

        }

        private UInt16 fcs_cal(UInt16 fcs, ref byte[] cp, int length)
        {
            int i = 1;
            while (Convert.ToBoolean(length--))
                fcs = Convert.ToUInt16((fcs >> Convert.ToByte(8)) ^ fcstab[(fcs ^ Convert.ToByte(cp[i++])) & Convert.ToByte(0xff)]);
            return fcs;
        }

        public bool fcs(ref byte[] cp, int len, byte flag)
        {
            UInt16 final_fcs = 0;
            if (Convert.ToBoolean(flag))
            {
                final_fcs = fcs_cal(PPPINITFCS16, ref cp, len);
                final_fcs ^= Convert.ToUInt16(0xFFFF);
                cp[len + 1] = Convert.ToByte(final_fcs & Convert.ToUInt16(0x00FF));
                //x = cp[len];
                cp[len + 2] = Convert.ToByte(((final_fcs >> Convert.ToUInt16(8)) & Convert.ToUInt16(0x00FF)));
                //y = cp[len + 1];
                return true;
            }
            else
            {
                final_fcs = fcs_cal(PPPINITFCS16, ref cp, len);
                if (final_fcs == PPPGOODFCS16)
                    return true;
                else
                    return false;
            }
        }

        public bool CountParameter(string sFilePath, string sType)
        {
            string sTmp, sLine;
            bool bflag = false;
            long no_of_lines = 0, no_of_spaces = 0, no_of_tabs = 0, no_of_char = 0, no_of_words = 0;
            byte[] CRC = new byte[128];
            byte UC_CRC = 0;
            //int intcnt=0;

            try
            {
                TextReader oReadTxt = new StreamReader(sFilePath);
                sLine = oReadTxt.ReadToEnd();
                for (int i = 0; i < sLine.Length; i++)
                {
                    //CRCcnt = 1;
                    sTmp = sLine.Substring(i, 1);
                    //CRC[CRCcnt++] = byte.Parse(sTmp, NumberStyles.HexNumber);
                    //if (CRCcnt == 127)
                    //{
                    //    CRC[0] = UC_CRC;
                    //    if (intcnt != 0)
                    //        UC_CRC = calculate_crc(CRC, 127);
                    //    intcnt++;
                    //    CRCcnt = 0;
                    //}

                    if (sTmp == "\r" || sTmp == "\n")
                    {
                        if (bflag)
                            bflag = false;
                        no_of_lines++;
                    }
                    else if (sTmp == "\t")
                    {
                        if (bflag)
                            bflag = false;
                        no_of_tabs++;
                    }
                    else if (sTmp == " ")
                    {
                        if (bflag)
                            bflag = false;
                        no_of_spaces++;
                    }
                    else
                    {
                        if (!bflag)
                        {
                            no_of_words++;
                            bflag = true;
                        }
                        no_of_char++;
                    }
                }

                //CRC[0] = UC_CRC;
                //UC_CRC = calculate_crc(CRC, CRCcnt);

                no_of_lines = (no_of_lines / 2) + 1;
                oReadTxt.Close();
                oReadTxt = null;
                string shead = "<" + UC_CRC.ToString("00") + "#" + no_of_char.ToString("00000000") + "#" + no_of_spaces.ToString("00000000") + "#" + no_of_lines.ToString("00000000") + "#" + no_of_tabs.ToString("00000000") + "#" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "#" + sType + "><";
                TextWriter oWriteTxt = new StreamWriter(sFilePath);
                oWriteTxt.Write(shead); //Write Header Information
                oWriteTxt.Write(sLine); //Write Data
                oWriteTxt.Close();
                oWriteTxt = null;
                return true;
            }
            catch
            {
                return false;
                //  new ClsHandleException("ClsDataDownload.CountParameter", ex);
                //StreamWriter oSw = new StreamWriter(Directory.GetCurrentDirectory() + "\\ExLog.txt", true);
                //oSw.WriteLine("ClsDataDownload.CountParameter->" + ex.Message);
                //oSw.Close();
                //oSw.Dispose();
            }

        }

        private byte calculate_crc(byte[] sInptStr, byte nTotalBytes)
        {
            byte crc = sInptStr[0];

            for (byte i = 1; i <= nTotalBytes; i++)
                crc = Convert.ToByte(crc ^ sInptStr[i]);

            return crc;
        }
        public string UnitToName(string sUnit)
        {
            string name = String.Empty;
            switch (sUnit)
            {
                case "1B":
                    name = "W";
                    break;
                case "1C":
                    name = "VA";
                    break;
                case "1D":
                    name = "var";
                    break;
                case "1E":
                    name = "Wh";
                    break;
                case "1F":
                    name = "VAh";
                    break;
                case "20":
                    name = "varh";
                    break;
                case "21":
                    name = "Amp";
                    break;
                case "23":
                    name = "Volt";
                    break;
                case "06":
                    name = "Min";
                    break;
                case "0A":
                    name = "Currency";
                    break;
                default:
                    name = "N/A";
                    break;
            }
            return name;
        }

        public string ObistoName(string ObisCode, int MtrTyp, int MtrCatagory, bool isObjectList)
        {
            string name = string.Empty;
            if (isObjectList == true)
                name = int.Parse(ObisCode.Substring(0, 4), NumberStyles.HexNumber).ToString() + "-" + int.Parse(ObisCode.Substring(8, 2), NumberStyles.HexNumber).ToString() + "." + int.Parse(ObisCode.Substring(10, 2), NumberStyles.HexNumber).ToString() + "." + int.Parse(ObisCode.Substring(12, 2), NumberStyles.HexNumber).ToString() + "." + int.Parse(ObisCode.Substring(14, 2), NumberStyles.HexNumber).ToString() + "." + int.Parse(ObisCode.Substring(16, 2), NumberStyles.HexNumber).ToString() + "." + int.Parse(ObisCode.Substring(18, 2), NumberStyles.HexNumber).ToString() + " ";
            else
                name = int.Parse(ObisCode.Substring(0, 4), NumberStyles.HexNumber).ToString() + "-" + int.Parse(ObisCode.Substring(8, 2), NumberStyles.HexNumber).ToString() + "." + int.Parse(ObisCode.Substring(10, 2), NumberStyles.HexNumber).ToString() + "." + int.Parse(ObisCode.Substring(12, 2), NumberStyles.HexNumber).ToString() + "." + int.Parse(ObisCode.Substring(14, 2), NumberStyles.HexNumber).ToString() + "." + int.Parse(ObisCode.Substring(16, 2), NumberStyles.HexNumber).ToString() + "." + int.Parse(ObisCode.Substring(18, 2), NumberStyles.HexNumber).ToString() + "-" + int.Parse(ObisCode.Substring(22, 2), NumberStyles.HexNumber).ToString() + " ";
            switch (ObisCode)
            {
                case "000309060000600700FF0F02":
                    name += "Aux Power ON duration";//	0.0.96.7.0.255
                    break;
                case "000309060100810800FF0F02":
                case "000309060100810800FF":
                case "000309060100811D00FF0F02":
                case "000309060100811D00FF":
                    name += "(Fund Wh-Import)";
                    break;
                case "000309060100820800FF0F02":
                case "000309060100820800FF":
                case "000309060100821D00FF0F02":
                case "000309060100821D00FF":
                    name += "(Fund Wh-Export)";
                    break;
                case "000809060000010000FF0F02":
                case "000809060000010000FF":
                case "000809060100000105FF0F02":
                    name += "(RTC)";
                    break;
                case "000309060100201B00FF0F02":
                case "000309060100201B00FF":
                    if (MtrTyp == 1)
                        name += "(V-RN)";
                    else
                        name += "(V-RY)";
                    break;
                case "000309060100341B00FF0F02":
                case "000309060100341B00FF":
                    if (MtrTyp == 1)
                        name += "(V-YN)";
                    else
                        name += "(V-BY)";
                    break;
                case "000309060100481B00FF0F02":
                case "000309060100481B00FF":
                    name += "(V-BN)";
                    break;
                case "000309060100200700FF0F02":
                case "000309060100200700FF":
                    if (MtrTyp == 1)
                        name += "(V-RN)";
                    else
                        name += "(V-RY)";
                    break;
                case "000309060100340700FF0F02":
                case "000309060100340700FF":
                    if (MtrTyp == 1)
                        name += "(V-YN)";
                    else
                        name += "(V-BY)";
                    break;
                case "000309060100480700FF0F02":
                case "000309060100480700FF":
                    name += "(V-BN)";
                    break;
                case "0003090601000D1B00FF0F02":
                case "0003090601000D1B00FF":
                    name += "(System-PF)";
                    break;
                case "000309060100101D00FF0F02":
                case "000309060100101D00FF":
                    name += "(Net-Wh)";
                    break;
                case "000309060100021D00FF0F02":
                case "000309060100021D00FF":
                    name += "(Wh-Export)";
                    break;
                case "0003090601000A1D00FF0F02":
                case "0003090601000A1D00FF":
                    name += "(VAh-Export)";
                    break;
                case "000309060100011D00FF0F02":
                case "000309060100011D00FF":
                    if (MtrCatagory == 1)
                        name += "(Wh-Import)";
                    else
                        name += "(Wh)";
                    break;
                case "000309060100051D00FF0F02":
                case "000309060100051D00FF":
                    if (MtrCatagory == 1)
                        name += "(varh-Q1)";
                    else
                        name += "(varh-Lag)";
                    break;
                case "000309060100061D00FF0F02":
                case "000309060100061D00FF":
                    name += "(varh-Q2)";
                    break;
                case "000309060100071D00FF0F02":
                case "000309060100071D00FF":
                    name += "(varh-Q3)";
                    break;
                case "000309060100081D00FF0F02":
                case "000309060100081D00FF":
                    if (MtrCatagory == 1)
                        name += "(varh-Q4)";
                    else
                        name += "(varh-Lead)";
                    break;
                case "000309060100091D00FF0F02":
                case "000309060100091D00FF":
                    name += "(VAh)";
                    break;
                case "000309060100010800FF0F02":
                case "000309060100010800FF":
                case "000309060101010800FF":
                case "000309060101010800FF0F02":
                    if (MtrCatagory == 1)
                        name += "(Wh-Import)";
                    else
                        name += "(Cum Wh)";
                    break;
                case "0003090601010F0800FF":
                case "0003090601010F0800FF0F02":
                case "0003090601000F0800FF":
                case "0003090601000F0800FF0F02":
                    name += "(Cum Wh)";
                    break;
                case "000309060100020800FF0F02":
                case "000309060100020800FF":
                case "000309060101020800FF":
                case "000309060101020800FF0F02":
                    name += "(Wh-Export)";
                    break;
                case "000309060100090800FF0F02":
                case "000309060100090800FF":
                case "000309060101090800FF":
                case "000309060101090800FF0F02":
                    //if (MtrCatagory == 1)
                    name += "(VAh-Import)";
                    //else
                    //    name += "(Cum VAh)";
                    break;
                case "0003090601000A0800FF0F02":
                case "0003090601000A0800FF":
                case "0003090601010A0800FF":
                case "0003090601010A0800FF0F02":
                    name += "(VAh-Export)";
                    break;
                case "000309060100A00800FF0F02":
                case "000309060100A00800FF":
                case "000309060101A00800FF":
                case "000309060101A00800FF0F02":
                    name += "(VAh-Total)";
                    break;
                case "000309060100050800FF0F02":
                case "000309060100050800FF":
                case "000309060101050800FF":
                case "000309060101050800FF0F02":
                    if (MtrCatagory == 1)
                        name += "(varh-Q1)";
                    else
                        name += "(varh-Lag)";
                    break;
                case "000309060100060800FF0F02":
                case "000309060100060800FF":
                case "000309060101060800FF":
                case "000309060101060800FF0F02":
                    name += "(varh-Q2)";
                    break;
                case "000309060100070800FF0F02":
                case "000309060100070800FF":
                case "000309060101070800FF":
                case "000309060101070800FF0F02":
                    name += "(varh-Q3)";
                    break;
                case "000309060100080800FF0F02":
                case "000309060100080800FF":
                case "000309060101080800FF":
                case "000309060101080800FF0F02":
                    if (MtrCatagory == 1)
                        name += "(varh-Q4)";
                    else
                        name += "(varh-Lead)";
                    break;
                case "000309060100830800FF0F02":
                case "000309060100830800FF":
                case "000309060100831D00FF0F02":
                case "000309060100831D00FF":
                    name += "(varh High Q1)";
                    break;
                case "000309060100840800FF0F02":
                case "000309060100840800FF":
                case "000309060100841D00FF0F02":
                case "000309060100841D00FF":
                    name += "(varh High Q2)";
                    break;
                case "000309060100850800FF0F02":
                case "000309060100850800FF":
                case "000309060100851D00FF0F02":
                case "000309060100851D00FF":
                    name += "(varh High Q3)";
                    break;
                case "000309060100860800FF0F02":
                case "000309060100860800FF":
                case "000309060100861D00FF0F02":
                case "000309060100861D00FF":
                    name += "(varh High Q4)";
                    break;
                case "000309060100870800FF0F02":
                case "000309060100870800FF":
                case "000309060100871D00FF0F02":
                case "000309060100871D00FF":
                    name += "(varh Low Q1)";
                    break;
                case "000309060100880800FF0F02":
                case "000309060100880800FF":
                case "000309060100881D00FF0F02":
                case "000309060100881D00FF":
                    name += "(varh Low Q2)";
                    break;
                case "000309060100890800FF0F02":
                case "000309060100890800FF":
                case "000309060100891D00FF0F02":
                case "000309060100891D00FF":
                    name += "(varh Low Q3)";
                    break;
                case "0003090601008A0800FF0F02":
                case "0003090601008A0800FF":
                case "0003090601008A1D00FF0F02":
                case "0003090601008A1D00FF":
                    name += "(varh Low Q4)";
                    break;
                case "0003090601001F0700FF0F02":
                case "0003090601001F0700FF":
                    name += "(I-R)";
                    break;
                case "000309060100330700FF0F02":
                case "000309060100330700FF":
                    name += "(I-Y)";
                    break;
                case "000309060100470700FF0F02":
                case "000309060100470700FF":
                    name += "(I-B)";
                    break;
                case "000309060100210700FF0F02":
                case "000309060100210700FF":
                    name += "(PF-R)";
                    break;
                case "000309060100350700FF0F02":
                case "000309060100350700FF":
                    name += "(PF-Y)";
                    break;
                case "000309060100490700FF0F02":
                case "000309060100490700FF":
                    name += "(PF-B)";
                    break;
                case "000109060000600B00FF0F02":
                case "000109060000600B00FF":
                    name += "(Voltage Events)";
                    break;
                case "000109060000600B01FF0F02":
                case "000109060000600B01FF":
                    name += "(Current Events)";
                    break;
                case "000109060000600B02FF0F02":
                case "000109060000600B02FF":
                    name += "(Power Events)";
                    break;
                case "000109060000600B03FF0F02":
                case "000109060000600B03FF":
                    name += "(Transaction Events)";
                    break;
                case "000109060000600B04FF0F02":
                case "000109060000600B04FF":
                    name += "(Other Events)";
                    break;
                case "000109060000600B05FF0F02":
                case "000109060000600B05FF":
                    name += "(Non Rollover Events)";
                    break;
                case "000109060000600B84FF0F02":
                case "000109060000600B84FF":
                    name += "(Street Light Events)";
                    break;
                case "000109060000600B06FF0F02":
                case "000109060000600B06FF":
                    name += "(Control Events)";
                    break;
                case "000109060000600B07FF0F02":
                case "000109060000600B07FF":
                    name += "(Self Diagnostic Events)";
                    break;
                case "0003090601000B1B00FF0F02":
                case "0003090601000B0700FF0F02":
                case "0003090601000B0700FF":
                case "0003090601000B0500FF0F02":
                case "0003090601000B0500FF":
                    name += "(Phase Current)";
                    break;
                case "0003090601000C1B00FF0F02":
                case "0003090601000C0700FF0F02":
                case "0003090601000C0700FF":
                case "0003090601000C0500FF0F02":
                case "0003090601000C0500FF":
                    name += "(Phase Voltage)";
                    break;
                case "0003090601005B0700FF0F02":
                case "0003090601005B0700FF":
                    name += "(Neutral Current)";
                    break;
                case "0003090601001F1B00FF0F02":
                case "0003090601001F1B00FF":
                    name += "(I-R)";
                    break;
                case "000309060100331B00FF0F02":
                case "000309060100331B00FF":
                    name += "(I-Y)";
                    break;
                case "000309060100471B00FF0F02":
                case "000309060100471B00FF":
                    name += "(I-B)";
                    break;
                case "0001090601008D0000FF0F02":
                case "0001090601008D0000FF":
                    name += "(RTC A/R status)";
                    break;

                case "0003090601005E5B01FF0F02":
                case "0003090601005E5B01FF":
                    name += "(Rect Energy [V > 103%])";
                    break;
                case "0003090601005E5B02FF0F02":
                case "0003090601005E5B02FF":
                    name += "(Rect Energy [V < 97%])";
                    break;
                case "0003090601000E1B00FF0F02":
                case "0003090601000E1B00FF":
                    name += "(Frequency)";
                    break;
                case "000109060000600100FF0F02":
                case "000109060000600100FF":
                    name += "(Meter Serial Number)";
                    break;
                case "000109060000600101FF0F02":
                case "000109060000600101FF":
                    name += "(Manufacturer Name)";
                    break;
                case "000109060100000200FF0F02":
                case "000109060100000200FF":
                    name += "(Firmware Version)";
                    break;
                case "000109060100000402FF0F02":
                case "000109060100000402FF":
                    name += "(Internal CT Ratio)";
                    break;
                case "000109060100000403FF0F02":
                case "000109060100000403FF":
                    name += "(Internal PT Ratio)";
                    break;
                case "0001090600005E5B09FF0F02":
                case "0001090600005E5B09FF":
                    name += "(Meter Type)";
                    break;
                case "000109060000600104FF0F02":
                case "000109060000600104FF":
                    name += "(Year of Manufacture)";
                    break;
                case "000109060100000800FF0F02":
                case "000109060100000800FF":
                    name += "(Demand Integration Period)";
                    break;
                case "000109060100000804FF0F02":
                case "000109060100000804FF":
                    name += "(Profile Capture Period)";
                    break;
                case "0016090600000F0000FF":
                    name += "(Single Action Schedule)";
                    break;
                case "0003090601008E0800FF":
                    name += "(High Resolution kWh)";
                    break;
                case "0003090601008F0800FF":
                    name += "(High Resolution kVAh)";
                    break;
                case "000309060100900800FF":
                    name += "(High Resolution kvarh Lag)";
                    break;
                case "000309060100910800FF":
                    name += "(High Resolution kvarh Lead)";
                    break;
                case "0003090601008E0700FF":
                    name += "(High Resolution kW)";
                    break;
                case "0003090601008F0700FF":
                    name += "(High Resolution kVA)";
                    break;
                case "000309060100900700FF":
                    name += "(High Resolution kvar Lag)";
                    break;
                case "000309060100910700FF":
                    name += "(High Resolution kvar Lead)";
                    break;
                case "0014090600000D0000FF":
                    name += "(Activity Calandar)";
                    break;
                case "000709060100630100FF":
                    name += "(Block Load Profile)";
                    break;
                case "000709060100630200FF":
                    name += "(Daily Load Profile)";
                    break;
                case "0007090601005E5B03FF":
                    name += "(Scaler Unit Instant Profile)";
                    break;
                case "0007090601005E5B04FF":
                    name += "(Scaler Unit Block Load)";
                    break;
                case "0007090601005E5B05FF":
                    name += "(Scaler Unit Daily Load)";
                    break;
                case "0007090601005E5B06FF":
                    name += "(Scaler Unit Bill Profile)";
                    break;
                case "0007090601005E5B07FF":
                    name += "(Scaler Unit Tamper Profile)";
                    break;
                case "000709060000636200FF":
                    name += "(Voltage Events Profile)";
                    break;
                case "000709060000636201FF":
                    name += "(Current Events Profile)";
                    break;
                case "000709060000636202FF":
                    name += "(Power Events Profile)";
                    break;
                case "000709060000636203FF":
                    name += "(Transaction Events Profile)";
                    break;
                case "000709060000636204FF":
                    name += "(Other Events Profile)";
                    break;
                case "000709060000636205FF":
                    name += "(Non Rollover Events Profile)";
                    break;
                case "000709060000636206FF":
                    name += "(Control Events Profile)";
                    break;
                case "000709060000636207FF":
                    name += "(Self Diagnostic Profile)";
                    break;
                case "0007090601005E5B00FF":
                    name += "(Instant Profile)";
                    break;
                case "0001090600002A0000FF0F02":
                case "0001090600002A0000FF":
                    name += "(Logical Device Name)";
                    break;
                case "0003090601000D0700FF0F02":
                case "0003090601000D0700FF":
                    name += "(Net Power Factor)";
                    break;
                case "0003090601000E0700FF0F02":
                case "0003090601000E0700FF":
                    name += "(Frequency)";
                    break;
                case "000309060100090700FF0F02":
                case "000309060100090700FF":
                    name += "(Apparent Power VA)";
                    break;
                case "000309060100A00700FF0F02":
                case "000309060100A00700FF":
                    name += "(Apparent Power Total VA)";
                    break;
                case "000309060100010700FF0F02":
                case "000309060100010700FF":
                    name += "(Signed Active Power)";
                    break;
                case "000309060100030700FF0F02":
                case "000309060100030700FF":
                    name += "(Signed Reactive Power)";
                    break;
                case "000109060000600700FF":
                case "000109060000600700FF0F02":
                    name += "(No of Power Failures)";
                    break;
                case "0003090600005E5B0DFF":
                case "0003090600005E5B0DFF0F02":
                    name += "(Total Power On Duration)";
                    break;
                case "0003090600005E5B08FF":
                    name += "(Cum Power Fail Duration)";
                    break;
                case "0001090600005E5B00FF":
                    name += "(Cum Tamper Count)";
                    break;
                case "000109060000000100FF":
                    name += "(Cum Billing Count)";
                    break;
                case "000109060000600200FF":
                    name += "(Cum Programming Count)";
                    break;
                case "000109060000000102FF":
                case "000309060000000102FF":
                    name += "(Billing Date)";
                    break;
                case "001109060000290000FF":
                    name += "(SAP Assignment)";
                    break;
                case "0009090600000A0064FF":
                    name += "(Tarrification Script)";
                    break;
                case "000F09060000280000FF":
                    name += "(Object List)";
                    break;
                case "000F09060000280001FF":
                    name += "(Object List - Public Client)";
                    break;
                case "000F09060000280002FF":
                    name += "(Object List - Meter Reader)";
                    break;
                case "000F09060000280003FF":
                    name += "(Object List - Utility Setting)";
                    break;
                case "000709060100620100FF":
                    name += "(Billing Profile)";
                    break;
                case "000409060100010600FF":
                    name += "(MD W)";
                    break;
                case "000409060100A00600FF":
                    name += "(MD VA Total)";
                    break;
                case "000409060100020600FF":
                    name += "(MD W-Export)";
                    break;
                case "000409060100090600FF":
                    name += "(MD VA)";
                    break;
                case "0004090601000A0600FF":
                    name += "(MD VA-Export)";
                    break;
                case "000409060100810600FF":
                    name += "(Fun MD W-Import)";
                    break;
                case "000409060100820600FF":
                    name += "(Fun MD W-Export)";
                    break;
                case "0000160000FF":
                case "001709060000160000FF":
                    name += "(IEC HDLC Setup)";
                    break;
                case "000109060000000101FF":
                    name += "(Avaiable Billing Periods)";
                    break;
                case "000309060000600800FF":
                case "000309060000600800FF0F02":
                    name += "(Power On Duration)";
                    break;
                case "003F09060000600680FF":
                    name += "(Battary Status)";
                    break;
                case "000109060000603300FF0F02":
                    name += "(Daily Power Fail)";
                    break;
                case "000109060000603301FF0F02":
                    name += "(Daily Long Power Fail)";
                    break;
                case "003F09060000603200FF0F02":
                case "003F09060000603200FF":
                    name += "(Tamper Status)";
                    break;
                case "003F09060000603201FF0F02":
                case "003F09060000603201FF":
                    name += "(Alert Status)";
                    break;
                case "003F09060000603202FF0F02":
                case "003F09060000603202FF":
                    name += "(Transaction Status)";
                    break;
                case "003F09060000603203FF0F02":
                case "003F09060000603203FF":
                    name += "(Tamper Alert/Transaction)";
                    break;
                case "003F09060000603204FF0F02":
                case "003F09060000603204FF":
                    name += "(Zone Wise Load Limit)";
                    break;
                case "00460906000060030AFF":
                    name += "(Connect/Disconnect Class)";
                    break;
                case "004709060000110000FF":
                    name += "(Limiter Class Over Voltage)";
                    break;
                case "004709060000110001FF":
                    name += "(Limiter Class Under Voltage)";
                    break;
                case "004709060000110002FF":
                    name += "(Limiter Class Over Load)";
                    break;
                case "004709060000110003FF":
                    name += "(Limiter Class Emergency Over Load)";
                    break;
                case "004709060000110004FF":
                    name += "(Limiter Class Over Current)";
                    break;
                case "0012090600002C0000FF":
                    name += "(Image Transfer)";
                    break;
                case "002909060000190000FF":
                    name += "(TCP/UDP Setup)";
                    break;
                case "002A09060000190100FF":
                    name += "(IPV4 Setup)";
                    break;
                case "002C09060000190300FF":
                    name += "(PPP Setup)";
                    break;
                case "002D09060000190400FF":
                    name += "(GPRS Modem Setup)";
                    break;
                case "001C09060000020200FF":
                    name += "(Auto Answer)";
                    break;
                case "001D09060000020100FF":
                    name += "(Auto Connect)";
                    break;
                case "0009090600000A006BFF":
                    name += "(Image Script)";
                    break;
                case "0009090600000A006CFF":
                    name += "(Push Script)";
                    break;
                case "000109060000600715FF":
                    name += "(No of Power Fail (1 Phase))";
                    break;
                case "000109060000600709FF":
                    name += "(No of Power Fail - Long (1 Phase))";
                    break;
                case "000109060000603300FF":
                    name += "(No of Daily Power Fail)";
                    break;
                case "000109060000603301FF":
                    name += "(No of Daily Power Fail - Long)";
                    break;
                case "0001090600005E5B01FF":
                    name += "(Bill Tamper Count)";
                    break;
                case "002809060000190900FF":
                    name += "(Push  Set Up Daily Audit)";
                    break;
                case "002809060000190901FF":
                    name += "(Push  Set Up Billing)";
                    break;
                case "002809060000190902FF":
                    name += "(Push  Set Up Event)";
                    break;
                case "002809060000190903FF":
                    name += "(Push  Set Up Comm)";
                    break;
                case "002809060000190904FF":
                    name += "(Push  Set Up Power Outage)";
                    break;
                case "002809060000190905FF":
                    name += "(Push  Set Up Instant)";
                    break;
                case "003C09060000000203FF":
                    name += "(Message Handler)";
                    break;
                case "0003090601000B1B00FF":
                    name += "(Avg Current)";
                    break;
                case "0003090601000C1B00FF":
                    name += "(Avg Voltage)";
                    break;
                case "000109060000600C05FF":
                    name += "(GSM Field Strength)";
                    break;
                case "00460906000060030AFF0F02":
                    name += "(Relay Status)";
                    break;
                case "000109060000600C05FF0F02":
                    name += "(RSSI)";
                    break;
                case "000409060100010600FF0F02":
                    name += "(Demand)";
                    break;
                case "000109060100603200FF0F02":
                case "000109060100603200FF":
                    name += "(R Phase Miss Count)";
                    break;
                case "000109060100603201FF0F02":
                case "000109060100603201FF":
                    name += "(Y Phase Miss Count)";
                    break;
                case "000109060100603202FF0F02":
                case "000109060100603202FF":
                    name += "(B Phase Miss Count)";
                    break;
                case "000109060100603300FF0F02":
                case "000109060100603300FF":
                    name += "(High Voltage Count)";
                    break;
                case "000109060100603400FF0F02":
                case "000109060100603400FF":
                    name += "(Low Voltage Count)";
                    break;
                case "000109060100603F00FF0F02":
                case "000109060100603F00FF":
                    name += "(Voltage Unbalance Count)";
                    break;
                case "000109060100603500FF0F02":
                case "000109060100603500FF":
                    name += "(R Phase CT Rev Count)";
                    break;
                case "000109060100603501FF0F02":
                case "000109060100603501FF":
                    name += "(Y Phase CT Rev Count)";
                    break;
                case "000109060100603502FF0F02":
                case "000109060100603502FF":
                    name += "(B Phase CT Rev Count)";
                    break;
                case "000109060100604000FF0F02":
                case "000109060100604000FF":
                    name += "(R Phase CT Open Count)";
                    break;
                case "000109060100604001FF0F02":
                case "000109060100604001FF":
                    name += "(Y Phase CT Open Count)";
                    break;
                case "000109060100604002FF0F02":
                case "000109060100604002FF":
                    name += "(B Phase CT Open Count)";
                    break;
                case "000109060100604100FF0F02":
                case "000109060100604100FF":
                    name += "(Current Ubbalance Count)";
                    break;

                case "000109060100603600FF0F02":
                case "000109060100603600FF":
                    name += "(CT Bypass Count)";
                    break;
                case "000109060100603700FF0F02":
                case "000109060100603700FF":
                    name += "(Over Current Count)";
                    break;
                case "000109060100603900FF0F02":
                case "000109060100603900FF":
                    name += "(Magnet Count)";
                    break;
                case "000109060100603A00FF0F02":
                case "000109060100603A00FF":
                    name += "(Neutral Disturbance Count)";
                    break;
                case "000109060100603B00FF0F02":
                case "000109060100603B00FF":
                    name += "(Very Low PF Count)";
                    break;

                case "000309060100170700FF0F02":
                case "000309060100170700FF":
                    name += "(R Phase Rect Power)";
                    break;
                case "0003090601002B0700FF0F02":
                case "0003090601002B0700FF":
                    name += "(Y Phase Rect Power)";
                    break;
                case "0003090601003F0700FF0F02":
                case "0003090601003F0700FF":
                    name += "(B Phase Rect Power)";
                    break;
                case "000309060100150700FF0F02":
                case "000309060100150700FF":
                    name += "(R Phase Act Power)";
                    break;
                case "000309060100290700FF0F02":
                case "000309060100290700FF":
                    name += "(Y Phase Act Power)";
                    break;
                case "0003090601003D0700FF0F02":
                case "0003090601003D0700FF":
                    name += "(B Phase Act Power)";
                    break;
                case "000309060100030800FF":
                case "000309060100030800FF0F02":
                case "000309060101030800FF":
                case "000309060101030800FF0F02":
                    name += "(varh - Import)"; ;
                    break;
                case "000309060100040800FF":
                case "000309060100040800FF0F02":
                case "000309060101040800FF":
                case "000309060101040800FF0F02":
                    name += "(varh - Export)"; ;
                    break;
                case "0004090601000F0600FF":
                case "0004090601000F0600FF0F02":
                    name += "(MD W - ABS)"; ;
                    break;
                case "000409060100100600FF":
                case "000409060100100600FF0F02":
                    name += "(MD W - NET)"; ;
                    break;
                case "000409060100030600FF":
                case "000409060100030600FF0F02":
                    name += "(MD W - Import)"; ;
                    break;
                case "000409060100040600FF":
                case "000409060100040600FF0F02":
                    name += "(MD W - Export)"; ;
                    break;
                case "000309060101230800FF":
                case "000309060101230800FF0F02":
                case "000309060100230800FF":
                case "000309060100230800FF0F02":
                    name += "(R Phase - Wh)"; ;
                    break;
                case "000309060101150800FF":
                case "000309060101150800FF0F02":
                case "000309060100150800FF":
                case "000309060100150800FF0F02":
                    name += "(R Phase - Wh-Import)"; ;
                    break;
                case "000309060101160800FF":
                case "000309060101160800FF0F02":
                case "000309060100160800FF":
                case "000309060100160800FF0F02":
                    name += "(R Phase - Wh-Export)"; ;
                    break;
                case "000309060101370800FF":
                case "000309060101370800FF0F02":
                case "000309060100370800FF":
                case "000309060100370800FF0F02":
                    name += "(Y Phase - Wh)"; ;
                    break;
                case "000309060101290800FF":
                case "000309060101290800FF0F02":
                case "000309060100290800FF":
                case "000309060100290800FF0F02":
                    name += "(Y Phase - Wh-Import)"; ;
                    break;
                case "0003090601012A0800FF":
                case "0003090601012A0800FF0F02":
                case "0003090601002A0800FF":
                case "0003090601002A0800FF0F02":
                    name += "(Y Phase - Wh-Export)"; ;
                    break;
                case "0003090601014B0800FF":
                case "0003090601014B0800FF0F02":
                case "0003090601004B0800FF":
                case "0003090601004B0800FF0F02":
                    name += "(B Phase - Wh)"; ;
                    break;
                case "0003090601013D0800FF":
                case "0003090601013D0800FF0F02":
                case "0003090601003D0800FF":
                case "0003090601003D0800FF0F02":
                    name += "(B Phase - Wh-Import)"; ;
                    break;
                case "0003090601013E0800FF":
                case "0003090601013E0800FF0F02":
                case "0003090601003E0800FF":
                case "0003090601003E0800FF0F02":
                    name += "(B Phase - Wh-Export)"; ;
                    break;
                //case "000309060101230800FF":
                //case "000309060101230800FF0F02":
                //case "000309060100230800FF":
                //case "000309060100230800FF0F02":
                //    name += "(R Phase - VAh)"; ;
                //    break;
                case "0003090601011D0800FF":
                case "0003090601011D0800FF0F02":
                case "0003090601001D0800FF":
                case "0003090601001D0800FF0F02":
                    name += "(R Phase - VAh-Import)"; ;
                    break;
                case "0003090601011E0800FF":
                case "0003090601011E0800FF0F02":
                case "0003090601001E0800FF":
                case "0003090601001E0800FF0F02":
                    name += "(R Phase - VAh-Export)"; ;
                    break;
                //case "000309060101370800FF":
                //case "000309060101370800FF0F02":
                //case "000309060100370800FF":
                //case "000309060100370800FF0F02":
                //    name += "(Y Phase - VAh)"; ;
                //    break;
                case "000309060101310800FF":
                case "000309060101310800FF0F02":
                case "000309060100310800FF":
                case "000309060100310800FF0F02":
                    name += "(Y Phase - VAh-Import)"; ;
                    break;
                case "000309060101320800FF":
                case "000309060101320800FF0F02":
                case "000309060100320800FF":
                case "000309060100320800FF0F02":
                    name += "(Y Phase - VAh-Export)"; ;
                    break;
                //case "0003090601014B0800FF":
                //case "0003090601014B0800FF0F02":
                //case "0003090601004B0800FF":
                //case "0003090601004B0800FF0F02":
                //    name += "(B Phase - VAh)"; ;
                //    break;
                case "000309060101450800FF":
                case "000309060101450800FF0F02":
                case "000309060100450800FF":
                case "000309060100450800FF0F02":
                    name += "(B Phase - VAh-Import)"; ;
                    break;
                case "000309060101460800FF":
                case "000309060101460800FF0F02":
                case "000309060100460800FF":
                case "000309060100460800FF0F02":
                    name += "(B Phase - VAh-Export)"; ;
                    break;
                case "000309060101190800FF":
                case "000309060101190800FF0F02":
                case "000309060100190800FF":
                case "000309060100190800FF0F02":
                    name += "(R Phase - var-Q1)"; ;
                    break;
                case "0003090601011A0800FF":
                case "0003090601011A0800FF0F02":
                case "0003090601001A0800FF":
                case "0003090601001A0800FF0F02":
                    name += "(R Phase - var-Q2)"; ;
                    break;
                case "0003090601011B0800FF":
                case "0003090601011B0800FF0F02":
                case "0003090601001B0800FF":
                case "0003090601001B0800FF0F02":
                    name += "(R Phase - var-Q3)"; ;
                    break;
                case "0003090601011C0800FF":
                case "0003090601011C0800FF0F02":
                case "0003090601001C0800FF":
                case "0003090601001C0800FF0F02":
                    name += "(R Phase - var-Q4)"; ;
                    break;
                case "0003090601012D0800FF":
                case "0003090601012D0800FF0F02":
                case "0003090601002D0800FF":
                case "0003090601002D0800FF0F02":
                    name += "(Y Phase - var-Q1)"; ;
                    break;
                case "0003090601012E0800FF":
                case "0003090601012E0800FF0F02":
                case "0003090601002E0800FF":
                case "0003090601002E0800FF0F02":
                    name += "(Y Phase - var-Q2)"; ;
                    break;
                case "0003090601012F0800FF":
                case "0003090601012F0800FF0F02":
                case "0003090601002F0800FF":
                case "0003090601002F0800FF0F02":
                    name += "(Y Phase - var-Q3)"; ;
                    break;
                case "000309060101300800FF":
                case "000309060101300800FF0F02":
                case "000309060100300800FF":
                case "000309060100300800FF0F02":
                    name += "(Y Phase - var-Q4)"; ;
                    break;
                case "000309060101410800FF":
                case "000309060101410800FF0F02":
                case "000309060100410800FF":
                case "000309060100410800FF0F02":
                    name += "(B Phase - var-Q1)"; ;
                    break;
                case "000309060101420800FF":
                case "000309060101420800FF0F02":
                case "000309060100420800FF":
                case "000309060100420800FF0F02":
                    name += "(B Phase - var-Q2)"; ;
                    break;
                case "000309060101430800FF":
                case "000309060101430800FF0F02":
                case "000309060100430800FF":
                case "000309060100430800FF0F02":
                    name += "(B Phase - var-Q3)"; ;
                    break;
                case "000309060101440800FF":
                case "000309060101440800FF0F02":
                case "000309060100440800FF":
                case "000309060100440800FF0F02":
                    name += "(B Phase - var-Q4)"; ;
                    break;
                case "000309060101170800FF":
                case "000309060101170800FF0F02":
                case "000309060100170800FF":
                case "000309060100170800FF0F02":
                    name += "(R Phase - var-Import)"; ;
                    break;
                case "000309060101180800FF":
                case "000309060101180800FF0F02":
                case "000309060100180800FF":
                case "000309060100180800FF0F02":
                    name += "(R Phase - var-Export)"; ;
                    break;
                case "0003090601012B0800FF":
                case "0003090601012B0800FF0F02":
                case "0003090601002B0800FF":
                case "0003090601002B0800FF0F02":
                    name += "(Y Phase - var-Import)"; ;
                    break;
                case "0003090601012C0800FF":
                case "0003090601012C0800FF0F02":
                case "0003090601002C0800FF":
                case "0003090601002C0800FF0F02":
                    name += "(Y Phase - var-Export)"; ;
                    break;
                case "0003090601013F0800FF":
                case "0003090601013F0800FF0F02":
                case "0003090601003F0800FF":
                case "0003090601003F0800FF0F02":
                    name += "(B Phase - var-Import)"; ;
                    break;
                case "000309060101400800FF":
                case "000309060101400800FF0F02":
                case "000309060100400800FF":
                case "000309060100400800FF0F02":
                    name += "(B Phase - var-Export)"; ;
                    break;
                case "0003090601005E5B0EFF0F02":
                    name += "(Current)"; ;
                    break;
                case "000409060100011100FF0F02":
                case "000409060100011100FF":
                    name += "(Universal MD kW)"; ;
                    break;
                case "000309060100960800FF0F02":
                case "000309060100960800FF":
                    name += "(Cum Megnet Defraud Energy)"; ;
                    break;
                case "000309060100970800FF0F02":
                case "000309060100970800FF":
                    name += "(Cum Freq Defraud Energy)"; ;
                    break;
                case "000109060100603D00FF0F02":
                case "000109060100603D00FF":
                    name += "(Freq Tamper Count)"; ;
                    break;
                case "000109060100603E00FF0F02":
                case "000109060100603E00FF":
                    name += "(TC Tamper Count)"; ;
                    break;
                case "000109060100603800FF0F02":
                case "000109060100603800FF":
                    name += "(Over Load Tamper Count)"; ;
                    break;
                case "000109060100608012FF":
                    name += "(Utility Code)";
                    break;
                case "000309060100920800FF0F02":
                case "000309060100920800FF":
                case "000309060100921D00FF0F02":
                    name += "(varh High-Import)";
                    break;
                case "000309060100930800FF0F02":
                case "000309060100930800FF":
                case "000309060100931D00FF0F02":
                    name += "(varh High-Export)";
                    break;
                case "000309060100940800FF0F02":
                case "000309060100940800FF":
                case "000309060100941D00FF0F02":
                    name += "(varh Low-Import)";
                    break;
                case "000309060100950800FF0F02":
                case "000309060100950800FF":
                case "000309060100951D00FF0F02":
                    name += "(varh Low-Export)";
                    break;
                case "000309060100980800FF":
                    name += "(varh Lead High Resolution Export)";
                    break;
                case "000309060100990800FF":
                    name += "(vah High Resolution Export)";
                    break;
                case "000109060100608017FF":
                    name += "(Meter Type)";
                    break;
                case "003F09060000600A01FF":
                    name += "(Tamper Flag)";
                    break;
                case "000109060100A30000FF0F02":
                    name += "(* in Volt)";
                    break;
                case "0016090600000F0080FF":
                    name += "(Over Voltage Threshold App. Date)";
                    break;
                case "0016090600000F0081FF":
                    name += "(Under Current Threshold App. Date)";
                    break;
                case "0016090600000F0082FF":
                    name += "(Over Current Threshold App. Date)";
                    break;
                case "0016090600000F0083FF":
                    name += "(Over Load Threshold App. Date)";
                    break;
                case "002809060000190907FF":
                    name += "(Alert Receive Mobile Number)";
                    break;
                case "004709060000110082FF":
                    name += "(Passive Over Current Threshold)";
                    break;
                case "004709060000110083FF":
                    name += "(Passive Over Load Threshold)";
                    break;
                case "000109060100608010FF":
                    name += "(Alert Flag)";
                    break;
                case "000109060100608014FF":
                    name += "(PFC Configuration)";
                    break;
                case "00010906010060801EFF":
                    name += "(Passive Relay Configuration)";
                    break;
                case "000109060100608019FF":
                    name += "(Relay Configuration)";
                    break;
                case "000109060100608023FF":
                    name += "(Local Button Enable for Reconnection of Relay)";
                    break;
                case "000109060100608024FF":
                    name += "(Relay Always ON Enable)";
                    break;
                case "000109060100608025FF":
                    name += "(Meter Functionality Mode)";
                    break;
                case "00010906010060802BFF":
                    name += "(Disconnection Date)";
                    break;
                case "00010906010060801DFF":
                    name += "(Rate Per Unit)";
                    break;
                case "000709060000636284FF":
                    name += "(Street Light Profile)";
                    break;
                case "000309060100AA0800FF":
                    name += "(Daytime Wh)";
                    break;
                case "000709060000636283FF":
                    name += "(Event Counters for All Compartments)";
                    break;
                case "004609060000600380FF":
                case "004609060000600380FF0F02":
                    name += "(Phase Wise Relay Status)";
                    break;
                case "0016090600000F0002FF":
                    name += "(Single Action Shedule Firmware)";
                    break;
                case "00010906010060801FFF":
                    name += "(GPRS Meter Configuration Type)";
                    break;
                case "000109060100608026FF":
                    name += "(Street Light Timing Manual or Astronomical)";
                    break;
                case "000109060100608027FF":
                    name += "(Street Light Manual ON-OFF Timings )";
                    break;
                case "000109060100608028FF":
                    name += "(Configuration for Astronomical Timings)";
                    break;
                case "000109060100AB0000FF":
                    name += "(Astro On-Off Timings)";
                    break;
                case "000109060100608029FF":
                    name += "(Maintenance Mode Activation)";
                    break;
                case "00010906010060802CFF":
                    name += "(Configuration of Non-Happy Hours Timings)";
                    break;
                case "00010906010060802DFF":
                    name += "(Balance Amount)";
                    break;
                case "00010906010060802EFF":
                    name += "(Low Balance Alarm)";
                    break;
                case "000109060100608021FF":
                    name += "(Ping Time GPRS)";
                    break;
                case "000109060100608033FF":
                    name += "(Two Way Time Configuration)";
                    break;
                case "000109060000028002FF":
                    name += "(IMEI number)";
                    break;
                case "000109060000028000FF":
                    name += "(SIM ID)";
                    break;
                case "006F09060000130000FF":
                    name += "(Balance (INR))";
                    break;
                case "000709060100638000FF":
                    name += "(Prepayment Instant Profile)";
                    break;
                case "000709060100638100FF":
                    name += "(Prepayment Instant Profile Scalar)";
                    break;
                case "000309060100AC0800FF":
                    name += "(DG Cum. Energy Wh)";
                    break;
                case "000309060100AD0800FF":
                    name += "(DG Cum. Energy VAh)";
                    break;
                case "000409060100AC0600FF":
                    name += "(DG MD W)";
                    break;
                case "000409060100AD0600FF":
                    name += "(DG MD VA)";
                    break;
                case "00030906000060060AFF":
                case "00030906000060060AFF0F02":
                    name += "(DG Power On Duration)";
                    break;
                case "000109060000600704FF":
                case "000109060000600704FF0F02":
                    name += "(DG Power Off Count)";
                    break;
                case "007109060000131401FF0F05":
                    name += "(Fixed Charge Deduction Rate)";
                    break;
                case "007109060000131404FF0F05":
                    name += "(DG Fixed Charge Deduction Rate)";
                    break;
                case "000309060000138300FF0F02":
                    name += "(Last Day Consumption)";
                    break;
                case "000309060000138301FF0F02":
                    name += "(DG Last Day Consumption)";
                    break;
                case "0003090601009B0000FF0F02":
                    name += "(POH for All Phase)";
                    break;
                case "0003090601009C0000FF0F02":
                    name += "(POH for Partial Phase)";
                    break;
                case "0003090601009D0000FF0F02":
                    name += "(POH for No Load)";
                    break;
                case "0003090601000D0000FF0F02":
                    name += "(Net PF)";
                    break;
                case "004609060000600381FF0F02":
                    name += "(PFC RelayStatus)";
                    break;
                case "000309060100AC0800FF0F02":
                case "000309060100AC1D00FF0F02":
                    name += "(DG Cum. Wh)";
                    break;
                case "000309060100AD0800FF0F02":
                case "000309060100AD1D00FF0F02":
                    name += "(DG Cum. VAh)";
                    break;
                case "000109060000600F00FF0F02":
                case "000109060000600F01FF0F02":
                case "000109060000600F02FF0F02":
                case "000109060000600F03FF0F02":
                case "000109060000600F04FF0F02":
                case "000109060000600F05FF0F02":
                case "000109060000600F06FF0F02":
                case "000709060100630100FF0FFF":
                case "000109060100000103FF0F02":
                    name += "(Sequence Number)";
                    break;
                //START BY AAC
                case "0001090600005E5B0CFF":
                    name += "(Current Rating)";
                    break;
                case "0007090600005E5B0AFF":
                    name += "(Nameplate Profile)";
                    break;
                case "000309060100010200FF":
                    name += "(Active power+ (QI+QIV) Cum.)";
                    break;
                case "000309060100090200FF":
                    name += "(Apparent power+ (QI+QIV) Cum. max.)";
                    break;
                //case "000309060100010200FF":
                //    name += "(Active power+ (QI+QIV) Cum.)";
                //    break;
                //case "000309060100010200FF":
                //    name += "(Active power+ (QI+QIV) Cum.)";
                //    break;
                //END AAC
                default:
                    name += "OBIS Code not Present";
                    break;


            }
            return name;
        }

        //private void ChkCRC(string sFilePath)
        //{
        //    string sTmp = "", sTmpStr = ""; ;
        //    string[] sFileLins = File.ReadAllLines(sFilePath);
        //    for (int i = 1; i < sFileLins.Length; i++)
        //    {
        //        sTmp = sTmp + sFileLins[i] + "\n";
        //        if (sTmp.Length >= 126)
        //        {
        //            if (sTmp.Length > 126)
        //            {
        //                sTmpStr = string.Empty;
        //                sTmpStr = sTmp.Substring(127);
        //            }
        //            sTmp = sTmp.Substring(1, 126);
        //            GetCRC(sTmp, ref nFlCRC, sTmp.Length);
        //            sTmp = sTmpStr;
        //        }
        //    }
        //    if (sTmp.Length >= 1)
        //        GetCRC(sTmp, ref nFlCRC, sTmp.Length);
        //}
        //private void Writehead(double no_of_char, double no_of_spaces, double no_of_lines, double no_of_tabs, double no_of_words, string sFilePath)
        //{

        //    try
        //    {

        //        string shead, sLine;

        //        this.ChkCRC(sFilePath);

        //        shead = "<" + nFlCRC + "#" + no_of_char + "#" + no_of_spaces + "#" + no_of_lines + "#" + no_of_tabs + "#" + DateTime.Now.Date + " " + "Format( Time, 'h:m:s')" + "><";

        //        if (shead.Length < 60)
        //        {

        //            for (int i = shead.Length; i < 61; i++)

        //                shead = shead + " ";

        //        }

        //        TextReader oReadTxt = new StreamReader(sFilePath);

        //        sLine = oReadTxt.ReadToEnd();

        //        oReadTxt.Close();

        //        oReadTxt = null;

        //        TextWriter oWrtTxt = new StreamWriter(sFilePath);

        //        oWrtTxt.Write(sLine.Replace("############################################################", shead));

        //        oWrtTxt.Close();

        //        oWrtTxt = null;

        //    }

        //    catch (Exception ex)
        //    {

        //        new ClsHandleException("ClsDataDownload.WriteHead", ex);

        //        //StreamWriter oSw = new StreamWriter(Directory.GetCurrentDirectory() + "\\ExLog.txt", true);

        //        //oSw.WriteLine("ClsDataDownload.WriteHead->" + ex.Message);

        //        //oSw.Close();

        //        //oSw.Dispose();

        //    }

        //}

    }
}
