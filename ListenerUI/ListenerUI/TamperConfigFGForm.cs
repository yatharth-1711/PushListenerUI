using AutoTest.FrameWork.Converts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ListenerUI
{
    public partial class TamperConfigFGForm : Form
    {
        public string UpdatedDataPkt { get; private set; }
        public string existingDataPkt { get; set; }
        DLMSParser parse = new DLMSParser();
        private string cmdPkt = string.Empty;
        private string meterType = string.Empty;
        bool _suppressTextEvents = false;
        public TamperConfigFGForm()
        {
            InitializeComponent();
        }
        private void GetDataFromCodeSP_Click(object sender, EventArgs e)
        {

            if (txtBoxExistingDataPkt.Text.Length < 10 || txtBoxExistingDataPkt.Text.Length != int.Parse(txtBoxExistingDataPkt.Text.Substring(2, 2), NumberStyles.HexNumber) * 2 + 4)
            {
                int num = (int)MessageBox.Show("Please Put valid Tamper Threshold Configuration String.", "GENUS DLMS CONFIG", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {
                if (meterType.Contains("1P"))
                    TamperThresholdConfiguration1P(txtBoxExistingDataPkt.Text);
                else if (meterType.Contains("3P") && cmdPkt == "01010060800BFF02")
                    TamperThresholdConfiguration3P(txtBoxExistingDataPkt.Text);
                else if (meterType.Contains("3P") && cmdPkt == "010100608004FF02")
                    TamperPersistanceConfiguration3P(txtBoxExistingDataPkt.Text);
            }
        }
        private void btnGetCode_Click(object sender, EventArgs e)
        {
            if (meterType.Contains("1P"))
                BuildTamperPkt1P();
            else if (meterType.Contains("3P") && cmdPkt == "01010060800BFF02")
            {
                if (!TamperThresholdValidation3P())
                    return;
                BuildTamperPkt3P();
            }
            else if (meterType.Contains("3P") && cmdPkt == "010100608004FF02")
            {
                BuildPersistanceTime3P();
            }
        }
        private void btn_UpdateDataPkt_Click(object sender, EventArgs e)
        {
            UpdatedDataPkt = string.Empty;
            switch (cmdPkt)
            {
                case "01010060800BFF02":
                    if (string.IsNullOrEmpty(txtNewDataPacket.Text.ToString()))
                    {
                        MessageBox.Show($"Data Packet is Empty", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    UpdatedDataPkt = txtNewDataPacket.Text;
                    break;
                case "010100608004FF02":
                    if (string.IsNullOrEmpty(txtNewDataPacket.Text.ToString()))
                    {
                        MessageBox.Show($"Data Packet is Empty", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    UpdatedDataPkt = txtNewDataPacket.Text;
                    break;
                default:
                    break;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        #region Helper Methods

        public void LoadInfo(string command, string dataPkt, string metertype)
        {
            cmdPkt = command;
            meterType = metertype;
            DisableAllPanels();
            txtBoxExistingDataPkt.Text = dataPkt;
            if (cmdPkt == "01010060800BFF02" && (txtBoxExistingDataPkt.Text.Length < 10 || txtBoxExistingDataPkt.Text.Length != int.Parse(txtBoxExistingDataPkt.Text.Substring(2, 2), NumberStyles.HexNumber) * 2 + 4))
            {
                int num = (int)MessageBox.Show("Please Put valid Tamper Threshold Configuration String.", "GENUS DLMS CONFIG", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            else if (cmdPkt == "010100608004FF02" && (txtBoxExistingDataPkt.Text.Length < 10))
            {
                int num = (int)MessageBox.Show("Please Put valid Tamper Persistance Time Configuration String.", "GENUS DLMS CONFIG", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            switch (cmdPkt.ToUpper())
            {
                case "01010060800BFF02":
                    if (metertype.Contains("1P"))
                    {
                        flowPnl1P.Visible = true;
                        flowPnl1P.Dock = DockStyle.Fill;
                        TamperThresholdConfiguration1P(txtBoxExistingDataPkt.Text.Trim());
                    }
                    else if (metertype.Contains("3P"))
                    {
                        flowPnl3P.Visible = true;
                        flowPnl3P.Dock = DockStyle.Fill;
                        TamperThresholdConfiguration3P(txtBoxExistingDataPkt.Text.Trim());
                    }
                    break;
                case "010100608004FF02":
                    if (metertype.Contains("3P"))
                    {
                        flowPnlPersistanceTime.Visible = true;
                        flowPnlPersistanceTime.Dock = DockStyle.Fill;
                        TamperPersistanceConfiguration3P(txtBoxExistingDataPkt.Text.Trim());
                    }
                    break;
                default:
                    break;
            }
        }
        private bool TamperThresholdValidation3P()
        {
            if (!isDouble(txt3P_LinkMiss_Occ_xPhaseVolt.Text))
            {
                int num = (int)MessageBox.Show("Please enter valid link miss tamper X-phase voltage (O).", "GENUS DLMS CONFIG", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                txt3P_LinkMiss_Occ_xPhaseVolt.Focus();
                return false;
            }
            if (!isDouble(txt3P_LinkMiss_Occ_xPhaseCurr.Text))
            {
                int num = (int)MessageBox.Show("Please enter valid link miss tamper X-phase current (O).", "GENUS DLMS CONFIG", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                txt3P_LinkMiss_Occ_xPhaseCurr.Focus();
                return false;
            }
            if (!isDouble(txt3P_LinkMiss_Res_xPhaseVolt.Text))
            {
                int num = (int)MessageBox.Show("Please enter valid link miss tamper X-phase voltage (R).", "GENUS DLMS CONFIG", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                txt3P_LinkMiss_Res_xPhaseVolt.Focus();
                return false;
            }
            if (!isDouble(txt3P_LinkMiss_Res_xPhaseCurr.Text))
            {
                int num = (int)MessageBox.Show("Please enter valid link miss tamper X-phase current (R).", "GENUS DLMS CONFIG", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                txt3P_LinkMiss_Res_xPhaseCurr.Focus();
                return false;
            }
            if (!isDouble(txt3P_LinkMiss_OccRes_anyPhaseVolt.Text))
            {
                int num = (int)MessageBox.Show("Please enter valid link miss tamper any phase voltage (O/R).", "GENUS DLMS CONFIG", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                txt3P_LinkMiss_Res_xPhaseCurr.Focus();
                return false;
            }
            if (!isDouble(txt3P_HighVolt_Occ_anyPhaseVoltage.Text))
            {
                int num = (int)MessageBox.Show("Please enter valid over voltage Tamper any phase voltage (O).", "GENUS DLMS CONFIG", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                txt3P_HighVolt_Occ_anyPhaseVoltage.Focus();
                return false;
            }
            if (!isDouble(txt3P_LowVolt_Occ_VoltageLess.Text))
            {
                int num = (int)MessageBox.Show("Please enter valid low voltage tamper voltage (O).", "GENUS DLMS CONFIG", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                txt3P_LowVolt_Occ_VoltageLess.Focus();
                return false;
            }
            if (!isDouble(txt3P_VoltUnbalance_Occ.Text))
            {
                int num = (int)MessageBox.Show("Please enter valid voltage unbalance tamper voltage Unbal(O).", "GENUS DLMS CONFIG", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                txt3P_VoltUnbalance_Occ.Focus();
                return false;
            }
            if (!isDouble(txt3P_CTRev_Occ_xCurrent.Text))
            {
                int num = (int)MessageBox.Show("Please enter valid CT reverse tamper X-current (O).", "GENUS DLMS CONFIG", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                txt3P_CTRev_Occ_xCurrent.Focus();
                return false;
            }
            if (!isDouble(txt3P_CTRev_Res_xCurrent.Text))
            {
                int num = (int)MessageBox.Show("Please enter valid CT reverse tamper X-current (R).", "GENUS DLMS CONFIG", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                txt3P_CTRev_Res_xCurrent.Focus();
                return false;
            }
            if (!isDouble(txt3P_CTRev_OccRes_NetPF.Text))
            {
                int num = (int)MessageBox.Show("Please enter valid CT reverse tamper Net PF limit(O/R).", "GENUS DLMS CONFIG", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                txt3P_CTRev_OccRes_xVoltage.Focus();
                return false;
            }
            if (!isDouble(txt3P_CTOpen_Occ_xCurrent.Text))
            {
                int num = (int)MessageBox.Show("Please enter valid CT Open tamper X-current (O).", "GENUS DLMS CONFIG", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                txt3P_CTOpen_Occ_xCurrent.Focus();
                return false;
            }
            if (!isDouble(txt3P_CTOpen_Res_xCurrent.Text))
            {
                int num = (int)MessageBox.Show("Please enter valid CT Open tamper X-current (R).", "GENUS DLMS CONFIG", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                txt3P_CTOpen_Res_xCurrent.Focus();
                return false;
            }
            if (!isDouble(txt3P_CTOpen_Occ_anyPhaseCurrent.Text))
            {
                int num = (int)MessageBox.Show("Please enter valid CT Open tamepr any phase current (O).", "GENUS DLMS CONFIG", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                txt3P_CTOpen_Occ_anyPhaseCurrent.Focus();
                return false;
            }
            if (!isDouble(txt3P_CTOpen_Res_anyPhaseCurrent.Text))
            {
                int num = (int)MessageBox.Show("Please enter valid CT Open tamepr any phase current (R).", "GENUS DLMS CONFIG", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                txt3P_CTOpen_Res_anyPhaseCurrent.Focus();
                return false;
            }
            if (!isDouble(txt3P_CurrUnbal_Occ.Text))
            {
                int num = (int)MessageBox.Show("Please enter valid current unbalance tamper current unbal (O).", "GENUS DLMS CONFIG", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                txt3P_CurrUnbal_Occ.Focus();
                return false;
            }
            if (!isDouble(txt3P_CurrUnbal_Res.Text))
            {
                int num = (int)MessageBox.Show("Please enter valid current unbalance tamper current unbal (R).", "GENUS DLMS CONFIG", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                txt3P_CurrUnbal_Res.Focus();
                return false;
            }
            if (!isDouble(txt3P_CTBypass_Occ_bypassCurr.Text))
            {
                int num = (int)MessageBox.Show("Please enter valid CT bypass tamper by pass current (O).", "GENUS DLMS CONFIG", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                txt3P_CTBypass_Occ_bypassCurr.Focus();
                return false;
            }
            if (!isDouble(txt3P_CTBypass_Res_bypassCurr.Text))
            {
                int num = (int)MessageBox.Show("Please enter valid CT bypass tamper by pass current (R).", "GENUS DLMS CONFIG", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                txt3P_CTBypass_Res_bypassCurr.Focus();
                return false;
            }
            if (!isDouble(txt3P_CTBypass_Occ_neutralCurr.Text))
            {
                int num = (int)MessageBox.Show("Please enter valid CT bypass tamper neutral current (O).", "GENUS DLMS CONFIG", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                txt3P_CTBypass_Occ_neutralCurr.Focus();
                return false;
            }
            if (!isDouble(txt3P_OverCurr_Occ_xPhaseCurr.Text))
            {
                int num = (int)MessageBox.Show("Please enter valid over current tamper phase cuttent (O).", "GENUS DLMS CONFIG", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                txt3P_OverCurr_Occ_xPhaseCurr.Focus();
                return false;
            }
            if (!isDouble(txt3P_ND_Occ_anyPhaseVolt.Text))
            {
                int num = (int)MessageBox.Show("Please enter valid nutral disturbance tamper any phase voltage(O).", "GENUS DLMS CONFIG", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                txt3P_ND_Occ_anyPhaseVolt.Focus();
                return false;
            }
            if (!isDouble(txt3P_veryLowPF_Occ_NetPF.Text))
            {
                int num = (int)MessageBox.Show("Please enter valid Net PF limit(O).", "GENUS DLMS CONFIG", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                txt3P_veryLowPF_Occ_NetPF.Focus();
                return false;
            }
            if (!isDouble(txt3P_veryLowPF_OccRes_xPhaseCurr.Text))
            {
                int num = (int)MessageBox.Show("Please enter valid low PF tamper X-phase current (O/R).", "GENUS DLMS CONFIG", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                txt3P_veryLowPF_OccRes_xPhaseCurr.Focus();
                return false;
            }
            if (isDouble(txt_OccRes_OtherPhaseVolLimit.Text))
                return true;
            int num1 = (int)MessageBox.Show("Please enter valid other phase voltage limit(O/R).", "GENUS DLMS CONFIG", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            txt_OccRes_OtherPhaseVolLimit.Focus();
            return false;
        }
        private bool TamperThresholdConfiguration1P(string strData)
        {
            int startIndex1 = 4;
            txt1P_OccTime_ND.Text = int.Parse(strData.Substring(startIndex1, 4), NumberStyles.HexNumber).ToString();
            int startIndex2 = startIndex1 + 4;
            txt1P_ResTime_ND.Text = int.Parse(strData.Substring(startIndex2, 4), NumberStyles.HexNumber).ToString();
            int startIndex3 = startIndex2 + 4;
            txt1P_OccThresold_ND.Text = (Convert.ToDouble(int.Parse(strData.Substring(startIndex3, 4), NumberStyles.HexNumber)) / 100.0).ToString("0.00");
            int startIndex4 = startIndex3 + 4;
            txt1P_ResThresold_ND.Text = (Convert.ToDouble(int.Parse(strData.Substring(startIndex4, 4), NumberStyles.HexNumber)) / 100.0).ToString("0.00");

            int startIndex5 = startIndex4 + 4;
            txt1P_OccTime_OverVolt.Text = int.Parse(strData.Substring(startIndex5, 4), NumberStyles.HexNumber).ToString();
            int startIndex6 = startIndex5 + 4;
            txt1P_ResTime_OverVolt.Text = int.Parse(strData.Substring(startIndex6, 4), NumberStyles.HexNumber).ToString();
            int startIndex7 = startIndex6 + 4;
            txt1P_OccThresold_OverVolt.Text = (Convert.ToDouble(int.Parse(strData.Substring(startIndex7, 4), NumberStyles.HexNumber)) / 100.0).ToString("0.00");
            int startIndex8 = startIndex7 + 4;
            txt1P_ResThresold_OverVolt.Text = (Convert.ToDouble(int.Parse(strData.Substring(startIndex8, 4), NumberStyles.HexNumber)) / 100.0).ToString("0.00");

            int startIndex9 = startIndex8 + 4;
            txt1P_OccTime_LowVolt.Text = int.Parse(strData.Substring(startIndex9, 4), NumberStyles.HexNumber).ToString();
            int startIndex10 = startIndex9 + 4;
            txt1P_ResTime_LowVolt.Text = int.Parse(strData.Substring(startIndex10, 4), NumberStyles.HexNumber).ToString();
            int startIndex11 = startIndex10 + 4;
            txt1P_OccThresold_LowVolt.Text = (Convert.ToDouble(int.Parse(strData.Substring(startIndex11, 4), NumberStyles.HexNumber)) / 100.0).ToString("0.00");
            int startIndex12 = startIndex11 + 4;
            txt1P_ResThresold_LowVolt.Text = (Convert.ToDouble(int.Parse(strData.Substring(startIndex12, 4), NumberStyles.HexNumber)) / 100.0).ToString("0.00");

            int startIndex13 = startIndex12 + 4;
            txt1P_OccTime_Reverse.Text = int.Parse(strData.Substring(startIndex13, 4), NumberStyles.HexNumber).ToString();
            int startIndex14 = startIndex13 + 4;
            txt1P_ResTime_Reverse.Text = int.Parse(strData.Substring(startIndex14, 4), NumberStyles.HexNumber).ToString();
            int startIndex15 = startIndex14 + 4;
            txt1P_OccThresold_Reverse.Text = (Convert.ToDouble(int.Parse(strData.Substring(startIndex15, 4), NumberStyles.HexNumber)) / 100.0).ToString("0.00");
            int startIndex16 = startIndex15 + 4;
            txt1P_ResThresold_Reverse.Text = (Convert.ToDouble(int.Parse(strData.Substring(startIndex16, 4), NumberStyles.HexNumber)) / 100.0).ToString("0.00");

            int startIndex17 = startIndex16 + 4;
            txt1P_OccTime_EarthLoad.Text = int.Parse(strData.Substring(startIndex17, 4), NumberStyles.HexNumber).ToString();
            int startIndex18 = startIndex17 + 4;
            txt1P_ResTime_EarthLoad.Text = int.Parse(strData.Substring(startIndex18, 4), NumberStyles.HexNumber).ToString();
            int startIndex19 = startIndex18 + 4;
            txt1P_OccThresold_EarthLoad.Text = (Convert.ToDouble(int.Parse(strData.Substring(startIndex19, 4), NumberStyles.HexNumber)) / 100.0).ToString("0.00");
            int startIndex20 = startIndex19 + 4;
            txt1P_ResThresold_EarthLoad.Text = (Convert.ToDouble(int.Parse(strData.Substring(startIndex20, 4), NumberStyles.HexNumber)) / 100.0).ToString("0.00");

            int startIndex21 = startIndex20 + 4;
            txt1P_OccTime_OverCurrent.Text = int.Parse(strData.Substring(startIndex21, 4), NumberStyles.HexNumber).ToString();
            int startIndex22 = startIndex21 + 4;
            txt1P_ResTime_OverCurrent.Text = int.Parse(strData.Substring(startIndex22, 4), NumberStyles.HexNumber).ToString();
            int startIndex23 = startIndex22 + 4;
            txt1P_OccThresold_OverCurrent.Text = (Convert.ToDouble(int.Parse(strData.Substring(startIndex23, 6), NumberStyles.HexNumber)) / 1000.0).ToString("0.000");
            int startIndex24 = startIndex23 + 6;
            txt1P_ResThresold_OverCurrent.Text = (Convert.ToDouble(int.Parse(strData.Substring(startIndex24, 6), NumberStyles.HexNumber)) / 1000.0).ToString("0.000");

            int startIndex25 = startIndex24 + 6;
            txt1P_OccTime_OverLoad.Text = int.Parse(strData.Substring(startIndex25, 4), NumberStyles.HexNumber).ToString();
            int startIndex26 = startIndex25 + 4;
            txt1P_ResTime_OverLoad.Text = int.Parse(strData.Substring(startIndex26, 4), NumberStyles.HexNumber).ToString();
            int startIndex27 = startIndex26 + 4;
            textBox48.Text = int.Parse(strData.Substring(startIndex27, 4), NumberStyles.HexNumber).ToString();
            int startIndex28 = startIndex27 + 4;
            txt1P_ResThresold_OverLoad.Text = int.Parse(strData.Substring(startIndex28, 4), NumberStyles.HexNumber).ToString();

            int startIndex29 = startIndex28 + 4;
            txt1P_OccTime_Magent.Text = int.Parse(strData.Substring(startIndex29, 4), NumberStyles.HexNumber).ToString();
            int startIndex30 = startIndex29 + 4;
            txt1P_ResTime_Magent.Text = int.Parse(strData.Substring(startIndex30, 4), NumberStyles.HexNumber).ToString();

            int startIndex31 = startIndex30 + 4;
            txt1P_OccTime_NeutralMiss.Text = int.Parse(strData.Substring(startIndex31, 4), NumberStyles.HexNumber).ToString();
            int startIndex32 = startIndex31 + 4;
            txt1P_ResTime_NeutralMiss.Text = int.Parse(strData.Substring(startIndex32, 4), NumberStyles.HexNumber).ToString();

            int startIndex33 = startIndex32 + 4;
            txt1P_ResTime_35KV.Text = int.Parse(strData.Substring(startIndex33, 4), NumberStyles.HexNumber).ToString();

            int startIndex34 = startIndex33 + 4;
            txt1P_OccTime_LowPF.Text = int.Parse(strData.Substring(startIndex34, 4), NumberStyles.HexNumber).ToString();
            int startIndex35 = startIndex34 + 4;
            txt1P_ResTime_LowPF.Text = int.Parse(strData.Substring(startIndex35, 4), NumberStyles.HexNumber).ToString();
            int startIndex36 = startIndex35 + 4;
            txt1P_OccThresold_LowPF.Text = (Convert.ToDouble(int.Parse(strData.Substring(startIndex36, 4), NumberStyles.HexNumber)) / 1000.0).ToString("0.000");
            int startIndex37 = startIndex36 + 4;
            txt1P_ResThresold_LowPF.Text = (Convert.ToDouble(int.Parse(strData.Substring(startIndex37, 4), NumberStyles.HexNumber)) / 1000.0).ToString("0.000");

            int startIndex38 = startIndex37 + 4;
            txt1P_OccTime_AbnormalFreq.Text = int.Parse(strData.Substring(startIndex38, 4), NumberStyles.HexNumber).ToString();
            int startIndex39 = startIndex38 + 4;
            txt1P_ResTime_AbnormalFreq.Text = int.Parse(strData.Substring(startIndex39, 4), NumberStyles.HexNumber).ToString();
            int startIndex40 = startIndex39 + 4;
            txt1P_OccThresold_AbnormalFreq.Text = (Convert.ToDouble(int.Parse(strData.Substring(startIndex40, 4), NumberStyles.HexNumber)) / 1000.0).ToString("0.000");
            int startIndex41 = startIndex40 + 4;
            txt1P_ResThresold_AbnormalFreq.Text = (Convert.ToDouble(int.Parse(strData.Substring(startIndex41, 4), NumberStyles.HexNumber)) / 1000.0).ToString("0.000");

            int num = startIndex41 + 4;
            return true;
        }
        private string BuildTamperPkt1P()
        {
            byte[] numArray = new byte[86];
            numArray[0] = (byte)9;
            numArray[1] = (byte)84;
            int int32_1 = Convert.ToInt32(Convert.ToDouble(txt1P_OccTime_ND.Text));
            numArray[3] = Convert.ToByte(int32_1 % 256);
            int num1 = int32_1 / 256;
            numArray[2] = Convert.ToByte(num1 % 256);
            int int32_2 = Convert.ToInt32(Convert.ToDouble(txt1P_ResTime_ND.Text));
            numArray[5] = Convert.ToByte(int32_2 % 256);
            int num2 = int32_2 / 256;
            numArray[4] = Convert.ToByte(num2 % 256);
            int int32_3 = Convert.ToInt32(Convert.ToDouble(txt1P_OccThresold_ND.Text) * 100.0);
            numArray[7] = Convert.ToByte(int32_3 % 256);
            int num3 = int32_3 / 256;
            numArray[6] = Convert.ToByte(num3 % 256);
            int int32_4 = Convert.ToInt32(Convert.ToDouble(txt1P_ResThresold_ND.Text) * 100.0);
            numArray[9] = Convert.ToByte(int32_4 % 256);
            int num4 = int32_4 / 256;
            numArray[8] = Convert.ToByte(num4 % 256);
            int int32_5 = Convert.ToInt32(Convert.ToDouble(txt1P_OccTime_OverVolt.Text));
            numArray[11] = Convert.ToByte(int32_5 % 256);
            int num5 = int32_5 / 256;
            numArray[10] = Convert.ToByte(num5 % 256);
            int int32_6 = Convert.ToInt32(Convert.ToDouble(txt1P_ResTime_OverVolt.Text));
            numArray[13] = Convert.ToByte(int32_6 % 256);
            int num6 = int32_6 / 256;
            numArray[12] = Convert.ToByte(num6 % 256);
            int int32_7 = Convert.ToInt32(Convert.ToDouble(txt1P_OccThresold_OverVolt.Text) * 100.0);
            numArray[15] = Convert.ToByte(int32_7 % 256);
            int num7 = int32_7 / 256;
            numArray[14] = Convert.ToByte(num7 % 256);
            int int32_8 = Convert.ToInt32(Convert.ToDouble(txt1P_ResThresold_OverVolt.Text) * 100.0);
            numArray[17] = Convert.ToByte(int32_8 % 256);
            int num8 = int32_8 / 256;
            numArray[16] = Convert.ToByte(num8 % 256);
            int int32_9 = Convert.ToInt32(Convert.ToDouble(txt1P_OccTime_LowVolt.Text));
            numArray[19] = Convert.ToByte(int32_9 % 256);
            int num9 = int32_9 / 256;
            numArray[18] = Convert.ToByte(num9 % 256);
            int int32_10 = Convert.ToInt32(Convert.ToDouble(txt1P_ResTime_LowVolt.Text));
            numArray[21] = Convert.ToByte(int32_10 % 256);
            int num10 = int32_10 / 256;
            numArray[20] = Convert.ToByte(num10 % 256);
            int int32_11 = Convert.ToInt32(Convert.ToDouble(txt1P_OccThresold_LowVolt.Text) * 100.0);
            numArray[23] = Convert.ToByte(int32_11 % 256);
            int num11 = int32_11 / 256;
            numArray[22] = Convert.ToByte(num11 % 256);
            int int32_12 = Convert.ToInt32(Convert.ToDouble(txt1P_ResThresold_LowVolt.Text) * 100.0);
            numArray[25] = Convert.ToByte(int32_12 % 256);
            int num12 = int32_12 / 256;
            numArray[24] = Convert.ToByte(num12 % 256);
            int int32_13 = Convert.ToInt32(Convert.ToDouble(txt1P_OccTime_Reverse.Text));
            numArray[27] = Convert.ToByte(int32_13 % 256);
            int num13 = int32_13 / 256;
            numArray[26] = Convert.ToByte(num13 % 256);
            int int32_14 = Convert.ToInt32(Convert.ToDouble(txt1P_ResTime_Reverse.Text));
            numArray[29] = Convert.ToByte(int32_14 % 256);
            int num14 = int32_14 / 256;
            numArray[28] = Convert.ToByte(num14 % 256);
            int int32_15 = Convert.ToInt32(Convert.ToDouble(txt1P_OccThresold_Reverse.Text) * 100.0);
            numArray[31] = Convert.ToByte(int32_15 % 256);
            int num15 = int32_15 / 256;
            numArray[30] = Convert.ToByte(num15 % 256);
            int int32_16 = Convert.ToInt32(Convert.ToDouble(txt1P_ResThresold_Reverse.Text) * 100.0);
            numArray[33] = Convert.ToByte(int32_16 % 256);
            int num16 = int32_16 / 256;
            numArray[32] = Convert.ToByte(num16 % 256);
            int int32_17 = Convert.ToInt32(Convert.ToDouble(txt1P_OccTime_EarthLoad.Text));
            numArray[35] = Convert.ToByte(int32_17 % 256);
            int num17 = int32_17 / 256;
            numArray[34] = Convert.ToByte(num17 % 256);
            int int32_18 = Convert.ToInt32(Convert.ToDouble(txt1P_ResTime_EarthLoad.Text));
            numArray[37] = Convert.ToByte(int32_18 % 256);
            int num18 = int32_18 / 256;
            numArray[36] = Convert.ToByte(num18 % 256);
            int int32_19 = Convert.ToInt32(Convert.ToDouble(txt1P_OccThresold_EarthLoad.Text) * 100.0);
            numArray[39] = Convert.ToByte(int32_19 % 256);
            int num19 = int32_19 / 256;
            numArray[38] = Convert.ToByte(num19 % 256);
            int int32_20 = Convert.ToInt32(Convert.ToDouble(txt1P_ResThresold_EarthLoad.Text) * 100.0);
            numArray[41] = Convert.ToByte(int32_20 % 256);
            int num20 = int32_20 / 256;
            numArray[40] = Convert.ToByte(num20 % 256);
            int int32_21 = Convert.ToInt32(Convert.ToDouble(txt1P_OccTime_OverCurrent.Text));
            numArray[43] = Convert.ToByte(int32_21 % 256);
            int num21 = int32_21 / 256;
            numArray[42] = Convert.ToByte(num21 % 256);
            int int32_22 = Convert.ToInt32(Convert.ToDouble(txt1P_ResTime_OverCurrent.Text));
            numArray[45] = Convert.ToByte(int32_22 % 256);
            int num22 = int32_22 / 256;
            numArray[44] = Convert.ToByte(num22 % 256);
            int int32_23 = Convert.ToInt32(Convert.ToDouble(txt1P_OccThresold_OverCurrent.Text) * 1000.0);
            numArray[48] = Convert.ToByte(int32_23 % 256);
            int num23 = int32_23 / 256;
            numArray[47] = Convert.ToByte(num23 % 256);
            int num24 = num23 / 256;
            numArray[46] = Convert.ToByte(num24 % 256);
            int int32_24 = Convert.ToInt32(Convert.ToDouble(txt1P_ResThresold_OverCurrent.Text) * 1000.0);
            numArray[51] = Convert.ToByte(int32_24 % 256);
            int num25 = int32_24 / 256;
            numArray[50] = Convert.ToByte(num25 % 256);
            int num26 = num25 / 256;
            numArray[49] = Convert.ToByte(num26 % 256);
            int int32_25 = Convert.ToInt32(Convert.ToDouble(txt1P_OccTime_OverLoad.Text));
            numArray[53] = Convert.ToByte(int32_25 % 256);
            int num27 = int32_25 / 256;
            numArray[52] = Convert.ToByte(num27 % 256);
            int int32_26 = Convert.ToInt32(Convert.ToDouble(txt1P_ResTime_OverLoad.Text));
            numArray[55] = Convert.ToByte(int32_26 % 256);
            int num28 = int32_26 / 256;
            numArray[54] = Convert.ToByte(num28 % 256);
            int int32_27 = Convert.ToInt32(Convert.ToDouble(textBox48.Text));
            numArray[57] = Convert.ToByte(int32_27 % 256);
            int num29 = int32_27 / 256;
            numArray[56] = Convert.ToByte(num29 % 256);
            int int32_28 = Convert.ToInt32(Convert.ToDouble(txt1P_ResThresold_OverLoad.Text));
            numArray[59] = Convert.ToByte(int32_28 % 256);
            int num30 = int32_28 / 256;
            numArray[58] = Convert.ToByte(num30 % 256);
            int int32_29 = Convert.ToInt32(Convert.ToDouble(txt1P_OccTime_Magent.Text));
            numArray[61] = Convert.ToByte(int32_29 % 256);
            int num31 = int32_29 / 256;
            numArray[60] = Convert.ToByte(num31 % 256);
            int int32_30 = Convert.ToInt32(Convert.ToDouble(txt1P_ResTime_Magent.Text));
            numArray[63] = Convert.ToByte(int32_30 % 256);
            int num32 = int32_30 / 256;
            numArray[62] = Convert.ToByte(num32 % 256);
            int int32_31 = Convert.ToInt32(Convert.ToDouble(txt1P_OccTime_NeutralMiss.Text));
            numArray[65] = Convert.ToByte(int32_31 % 256);
            int num33 = int32_31 / 256;
            numArray[64] = Convert.ToByte(num33 % 256);
            int int32_32 = Convert.ToInt32(Convert.ToDouble(txt1P_ResTime_NeutralMiss.Text));
            numArray[67] = Convert.ToByte(int32_32 % 256);
            int num34 = int32_32 / 256;
            numArray[66] = Convert.ToByte(num34 % 256);
            int int32_33 = Convert.ToInt32(Convert.ToDouble(txt1P_ResTime_35KV.Text));
            numArray[69] = Convert.ToByte(int32_33 % 256);
            int num35 = int32_33 / 256;
            numArray[68] = Convert.ToByte(num35 % 256);
            int int32_34 = Convert.ToInt32(Convert.ToDouble(txt1P_OccTime_LowPF.Text));
            numArray[71] = Convert.ToByte(int32_34 % 256);
            int num36 = int32_34 / 256;
            numArray[70] = Convert.ToByte(num36 % 256);
            int int32_35 = Convert.ToInt32(Convert.ToDouble(txt1P_ResTime_LowPF.Text));
            numArray[73] = Convert.ToByte(int32_35 % 256);
            int num37 = int32_35 / 256;
            numArray[72] = Convert.ToByte(num37 % 256);
            int int32_36 = Convert.ToInt32(Convert.ToDouble(txt1P_OccThresold_LowPF.Text) * 1000.0);
            numArray[75] = Convert.ToByte(int32_36 % 256);
            int num38 = int32_36 / 256;
            numArray[74] = Convert.ToByte(num38 % 256);
            int int32_37 = Convert.ToInt32(Convert.ToDouble(txt1P_ResThresold_LowPF.Text) * 1000.0);
            numArray[77] = Convert.ToByte(int32_37 % 256);
            int num39 = int32_37 / 256;
            numArray[76] = Convert.ToByte(num39 % 256);
            int int32_38 = Convert.ToInt32(Convert.ToDouble(txt1P_OccTime_AbnormalFreq.Text));
            numArray[79] = Convert.ToByte(int32_38 % 256);
            int num40 = int32_38 / 256;
            numArray[78] = Convert.ToByte(num40 % 256);
            int int32_39 = Convert.ToInt32(Convert.ToDouble(txt1P_ResTime_AbnormalFreq.Text));
            numArray[81] = Convert.ToByte(int32_39 % 256);
            int num41 = int32_39 / 256;
            numArray[80] = Convert.ToByte(num41 % 256);
            int int32_40 = Convert.ToInt32(Convert.ToDouble(txt1P_OccThresold_AbnormalFreq.Text) * 1000.0);
            numArray[83] = Convert.ToByte(int32_40 % 256);
            int num42 = int32_40 / 256;
            numArray[82] = Convert.ToByte(num42 % 256);
            int int32_41 = Convert.ToInt32(Convert.ToDouble(txt1P_ResThresold_AbnormalFreq.Text) * 1000.0);
            numArray[85] = Convert.ToByte(int32_41 % 256);
            int num43 = int32_41 / 256;
            numArray[84] = Convert.ToByte(num43 % 256);
            string empty = string.Empty;
            for (int index = 0; index < numArray.Length; ++index)
                empty += numArray[index].ToString("X2");
            txtNewDataPacket.Text = empty;
            return empty;
        }
        private bool TamperThresholdConfiguration3P(string strData)
        {
            int startIndex1 = 4;
            txt3P_LinkMiss_Occ_xPhaseVolt.Text = ((double)int.Parse(strData.Substring(startIndex1, 4), NumberStyles.HexNumber) / 100.0).ToString("0.00");
            int startIndex2 = startIndex1 + 4;
            txt3P_LinkMiss_Occ_xPhaseCurr.Text = ((double)int.Parse(strData.Substring(startIndex2, 4), NumberStyles.HexNumber) / 100.0).ToString("0.00");
            int startIndex3 = startIndex2 + 4;
            txt3P_LinkMiss_Res_xPhaseVolt.Text = ((double)int.Parse(strData.Substring(startIndex3, 4), NumberStyles.HexNumber) / 100.0).ToString("0.00");
            int startIndex4 = startIndex3 + 4;
            txt3P_LinkMiss_Res_xPhaseCurr.Text = ((double)int.Parse(strData.Substring(startIndex4, 4), NumberStyles.HexNumber) / 100.0).ToString("0.00");
            int startIndex5 = startIndex4 + 4;
            txt3P_HighVolt_Occ_anyPhaseVoltage.Text = ((double)int.Parse(strData.Substring(startIndex5, 4), NumberStyles.HexNumber) / 100.0).ToString("0.00");
            int startIndex6 = startIndex5 + 4;
            txt3P_HighVolt_Res_allPhaseVoltage.Text = ((double)int.Parse(strData.Substring(startIndex6, 4), NumberStyles.HexNumber) / 100.0).ToString("0.00");
            int startIndex7 = startIndex6 + 4;
            txt3P_LowVolt_Occ_VoltageLess.Text = ((double)int.Parse(strData.Substring(startIndex7, 4), NumberStyles.HexNumber) / 100.0).ToString("0.00");
            int startIndex8 = startIndex7 + 4;
            txt3P_VoltUnbalance_Occ.Text = ((double)int.Parse(strData.Substring(startIndex8, 4), NumberStyles.HexNumber) / 100.0).ToString("0.00");
            int startIndex9 = startIndex8 + 4;
            comboBox1.SelectedIndex = int.Parse(strData.Substring(startIndex9, 2), NumberStyles.HexNumber);
            int startIndex10 = startIndex9 + 2;
            txt3P_CTRev_Occ_xCurrent.Text = ((double)int.Parse(strData.Substring(startIndex10, 4), NumberStyles.HexNumber) / 100.0).ToString("0.00");
            int startIndex11 = startIndex10 + 4;
            txt3P_CTRev_Res_xCurrent.Text = ((double)int.Parse(strData.Substring(startIndex11, 4), NumberStyles.HexNumber) / 100.0).ToString("0.00");
            int startIndex12 = startIndex11 + 4;
            txt3P_CTRev_OccRes_NetPF.Text = ((double)int.Parse(strData.Substring(startIndex12, 4), NumberStyles.HexNumber) / 100.0).ToString("0.00");
            int startIndex13 = startIndex12 + 4;
            txt3P_CTOpen_Occ_xCurrent.Text = ((double)int.Parse(strData.Substring(startIndex13, 4), NumberStyles.HexNumber) / 100.0).ToString("0.00");
            int startIndex14 = startIndex13 + 4;
            txt3P_CTOpen_Res_xCurrent.Text = ((double)int.Parse(strData.Substring(startIndex14, 4), NumberStyles.HexNumber) / 100.0).ToString("0.00");
            int startIndex15 = startIndex14 + 4;
            txt3P_CTOpen_Occ_anyPhaseCurrent.Text = ((double)int.Parse(strData.Substring(startIndex15, 4), NumberStyles.HexNumber) / 100.0).ToString("0.00");
            int startIndex16 = startIndex15 + 4;
            txt3P_CTOpen_Res_anyPhaseCurrent.Text = ((double)int.Parse(strData.Substring(startIndex16, 4), NumberStyles.HexNumber) / 100.0).ToString("0.00");
            int startIndex17 = startIndex16 + 4;
            txt3P_CurrUnbal_Occ.Text = ((double)int.Parse(strData.Substring(startIndex17, 4), NumberStyles.HexNumber) / 100.0).ToString("0.00");
            int startIndex18 = startIndex17 + 4;
            txt3P_CurrUnbal_Res.Text = ((double)int.Parse(strData.Substring(startIndex18, 4), NumberStyles.HexNumber) / 100.0).ToString("0.00");
            int startIndex19 = startIndex18 + 4;
            comboBox2.SelectedIndex = int.Parse(strData.Substring(startIndex19, 2), NumberStyles.HexNumber);
            int startIndex20 = startIndex19 + 2;
            txt3P_CTBypass_Occ_bypassCurr.Text = ((double)int.Parse(strData.Substring(startIndex20, 4), NumberStyles.HexNumber) / 100.0).ToString("0.00");
            int startIndex21 = startIndex20 + 4;
            txt3P_CTBypass_Res_bypassCurr.Text = ((double)int.Parse(strData.Substring(startIndex21, 4), NumberStyles.HexNumber) / 100.0).ToString("0.00");
            int startIndex22 = startIndex21 + 4;
            txt3P_OverCurr_Occ_xPhaseCurr.Text = ((double)int.Parse(strData.Substring(startIndex22, 4), NumberStyles.HexNumber) / 100.0).ToString("0.00");
            int startIndex23 = startIndex22 + 4;
            txt3P_ND_Occ_anyPhaseVolt.Text = ((double)int.Parse(strData.Substring(startIndex23, 4), NumberStyles.HexNumber) / 100.0).ToString("0.00");
            int startIndex24 = startIndex23 + 4;
            txt3P_veryLowPF_Occ_NetPF.Text = ((double)int.Parse(strData.Substring(startIndex24, 4), NumberStyles.HexNumber) / 100.0).ToString("0.00");
            int startIndex25 = startIndex24 + 4;
            txt3P_veryLowPF_OccRes_xPhaseCurr.Text = ((double)int.Parse(strData.Substring(startIndex25, 4), NumberStyles.HexNumber) / 100.0).ToString("0.00");
            int startIndex26 = startIndex25 + 4;
            txt3P_LinkMiss_OccRes_anyPhaseVolt.Text = ((double)int.Parse(strData.Substring(startIndex26, 4), NumberStyles.HexNumber) / 100.0).ToString("0.00");
            int startIndex27 = startIndex26 + 4;
            txt3P_CTBypass_Occ_neutralCurr.Text = ((double)int.Parse(strData.Substring(startIndex27, 4), NumberStyles.HexNumber) / 100.0).ToString("0.00");
            int startIndex28 = startIndex27 + 4;
            comboBox6.SelectedIndex = int.Parse(strData.Substring(startIndex28, 2), NumberStyles.HexNumber);
            int num = startIndex28 + 2;
            return true;
        }
        private string BuildTamperPkt3P()
        {
            byte[] numArray = new byte[55];
            numArray[0] = (byte)9;
            numArray[1] = (byte)53;
            int int32_1 = Convert.ToInt32(Convert.ToDouble(txt3P_LinkMiss_Occ_xPhaseVolt.Text) * 100.0);
            numArray[3] = Convert.ToByte(int32_1 % 256);
            int num1 = int32_1 / 256;
            numArray[2] = Convert.ToByte(num1 % 256);
            int int32_2 = Convert.ToInt32(Convert.ToDouble(txt3P_LinkMiss_Occ_xPhaseCurr.Text) * 100.0);
            numArray[5] = Convert.ToByte(int32_2 % 256);
            int num2 = int32_2 / 256;
            numArray[4] = Convert.ToByte(num2 % 256);
            int int32_3 = Convert.ToInt32(Convert.ToDouble(txt3P_LinkMiss_Res_xPhaseVolt.Text) * 100.0);
            numArray[7] = Convert.ToByte(int32_3 % 256);
            int num3 = int32_3 / 256;
            numArray[6] = Convert.ToByte(num3 % 256);
            int int32_4 = Convert.ToInt32(Convert.ToDouble(txt3P_LinkMiss_Res_xPhaseCurr.Text) * 100.0);
            numArray[9] = Convert.ToByte(int32_4 % 256);
            int num4 = int32_4 / 256;
            numArray[8] = Convert.ToByte(num4 % 256);
            int int32_5 = Convert.ToInt32(Convert.ToDouble(txt3P_HighVolt_Occ_anyPhaseVoltage.Text) * 100.0);
            numArray[11] = Convert.ToByte(int32_5 % 256);
            int num5 = int32_5 / 256;
            numArray[10] = Convert.ToByte(num5 % 256);
            int int32_6 = Convert.ToInt32(Convert.ToDouble(txt3P_HighVolt_Res_allPhaseVoltage.Text) * 100.0);
            numArray[13] = Convert.ToByte(int32_6 % 256);
            int num6 = int32_6 / 256;
            numArray[12] = Convert.ToByte(num6 % 256);
            int int32_7 = Convert.ToInt32(Convert.ToDouble(txt3P_LowVolt_Occ_VoltageLess.Text) * 100.0);
            numArray[15] = Convert.ToByte(int32_7 % 256);
            int num7 = int32_7 / 256;
            numArray[14] = Convert.ToByte(num7 % 256);
            int int32_8 = Convert.ToInt32(Convert.ToDouble(txt3P_VoltUnbalance_Occ.Text) * 100.0);
            numArray[17] = Convert.ToByte(int32_8 % 256);
            int num8 = int32_8 / 256;
            numArray[16] = Convert.ToByte(num8 % 256);
            numArray[18] = Convert.ToByte(comboBox1.SelectedIndex);
            int int32_9 = Convert.ToInt32(Convert.ToDouble(txt3P_CTRev_Occ_xCurrent.Text) * 100.0);
            numArray[20] = Convert.ToByte(int32_9 % 256);
            int num9 = int32_9 / 256;
            numArray[19] = Convert.ToByte(num9 % 256);
            int int32_10 = Convert.ToInt32(Convert.ToDouble(txt3P_CTRev_Res_xCurrent.Text) * 100.0);
            numArray[22] = Convert.ToByte(int32_10 % 256);
            int num10 = int32_10 / 256;
            numArray[21] = Convert.ToByte(num10 % 256);
            int int32_11 = Convert.ToInt32(Convert.ToDouble(txt3P_CTRev_OccRes_NetPF.Text) * 100.0);
            numArray[24] = Convert.ToByte(int32_11 % 256);
            int num11 = int32_11 / 256;
            numArray[23] = Convert.ToByte(num11 % 256);
            int int32_12 = Convert.ToInt32(Convert.ToDouble(txt3P_CTOpen_Occ_xCurrent.Text) * 100.0);
            numArray[26] = Convert.ToByte(int32_12 % 256);
            int num12 = int32_12 / 256;
            numArray[25] = Convert.ToByte(num12 % 256);
            int int32_13 = Convert.ToInt32(Convert.ToDouble(txt3P_CTOpen_Res_xCurrent.Text) * 100.0);
            numArray[28] = Convert.ToByte(int32_13 % 256);
            int num13 = int32_13 / 256;
            numArray[27] = Convert.ToByte(num13 % 256);
            int int32_14 = Convert.ToInt32(Convert.ToDouble(txt3P_CTOpen_Occ_anyPhaseCurrent.Text) * 100.0);
            numArray[30] = Convert.ToByte(int32_14 % 256);
            int num14 = int32_14 / 256;
            numArray[29] = Convert.ToByte(num14 % 256);
            int int32_15 = Convert.ToInt32(Convert.ToDouble(txt3P_CTOpen_Res_anyPhaseCurrent.Text) * 100.0);
            numArray[32] = Convert.ToByte(int32_15 % 256);
            int num15 = int32_15 / 256;
            numArray[31] = Convert.ToByte(num15 % 256);
            int int32_16 = Convert.ToInt32(Convert.ToDouble(txt3P_CurrUnbal_Occ.Text) * 100.0);
            numArray[34] = Convert.ToByte(int32_16 % 256);
            int num16 = int32_16 / 256;
            numArray[33] = Convert.ToByte(num16 % 256);
            int int32_17 = Convert.ToInt32(Convert.ToDouble(txt3P_CurrUnbal_Res.Text) * 100.0);
            numArray[36] = Convert.ToByte(int32_17 % 256);
            int num17 = int32_17 / 256;
            numArray[35] = Convert.ToByte(num17 % 256);
            numArray[37] = Convert.ToByte(comboBox2.SelectedIndex);
            int int32_18 = Convert.ToInt32(Convert.ToDouble(txt3P_CTBypass_Occ_bypassCurr.Text) * 100.0);
            numArray[39] = Convert.ToByte(int32_18 % 256);
            int num18 = int32_18 / 256;
            numArray[38] = Convert.ToByte(num18 % 256);
            int int32_19 = Convert.ToInt32(Convert.ToDouble(txt3P_CTBypass_Res_bypassCurr.Text) * 100.0);
            numArray[41] = Convert.ToByte(int32_19 % 256);
            int num19 = int32_19 / 256;
            numArray[40] = Convert.ToByte(num19 % 256);
            int int32_20 = Convert.ToInt32(Convert.ToDouble(txt3P_OverCurr_Occ_xPhaseCurr.Text) * 100.0);
            numArray[43] = Convert.ToByte(int32_20 % 256);
            int num20 = int32_20 / 256;
            numArray[42] = Convert.ToByte(num20 % 256);
            int int32_21 = Convert.ToInt32(Convert.ToDouble(txt3P_ND_Occ_anyPhaseVolt.Text) * 100.0);
            numArray[45] = Convert.ToByte(int32_21 % 256);
            int num21 = int32_21 / 256;
            numArray[44] = Convert.ToByte(num21 % 256);
            int int32_22 = Convert.ToInt32(Convert.ToDouble(txt3P_veryLowPF_Occ_NetPF.Text) * 100.0);
            numArray[47] = Convert.ToByte(int32_22 % 256);
            int num22 = int32_22 / 256;
            numArray[46] = Convert.ToByte(num22 % 256);
            int int32_23 = Convert.ToInt32(Convert.ToDouble(txt3P_veryLowPF_OccRes_xPhaseCurr.Text) * 100.0);
            numArray[49] = Convert.ToByte(int32_23 % 256);
            int num23 = int32_23 / 256;
            numArray[48] = Convert.ToByte(num23 % 256);
            int int32_24 = Convert.ToInt32(Convert.ToDouble(txt3P_LinkMiss_OccRes_anyPhaseVolt.Text) * 100.0);
            numArray[51] = Convert.ToByte(int32_24 % 256);
            int num24 = int32_24 / 256;
            numArray[50] = Convert.ToByte(num24 % 256);
            int int32_25 = Convert.ToInt32(Convert.ToDouble(txt3P_CTBypass_Occ_neutralCurr.Text) * 100.0);
            numArray[53] = Convert.ToByte(int32_25 % 256);
            int num25 = int32_25 / 256;
            numArray[52] = Convert.ToByte(num25 % 256);
            numArray[54] = Convert.ToByte(comboBox6.SelectedIndex);
            string empty = string.Empty;
            for (int index = 0; index < numArray.Length; ++index)
                empty += numArray[index].ToString("X2");
            txtNewDataPacket.Text = empty;
            return empty;
        }
        private bool TamperPersistanceConfiguration3P(string strData)
        {
            int startIndex = 6;
            //020512012C12012C120384120384020312000A12003C1200C0
            txt_T1_Occ.Text = int.Parse(strData.Substring(startIndex, 4), NumberStyles.HexNumber).ToString();
            startIndex += 6;
            txt_T1_Res.Text = int.Parse(strData.Substring(startIndex, 4), NumberStyles.HexNumber).ToString();
            startIndex += 6;
            txt_T2_Occ.Text = int.Parse(strData.Substring(startIndex, 4), NumberStyles.HexNumber).ToString();
            startIndex += 6;
            txt_T2_Res.Text = int.Parse(strData.Substring(startIndex, 4), NumberStyles.HexNumber).ToString();
            startIndex += 10;
            txt_MT_Occ.Text = int.Parse(strData.Substring(startIndex, 4), NumberStyles.HexNumber).ToString();
            startIndex += 6;
            txt_MT_Res.Text = int.Parse(strData.Substring(startIndex, 4), NumberStyles.HexNumber).ToString();
            startIndex += 6;
            txtOther.Text = int.Parse(strData.Substring(startIndex, 4), NumberStyles.HexNumber).ToString();
            return true;
        }
        private string BuildPersistanceTime3P()
        {
            string str = $"020512{Convert.ToInt32(txt_T1_Occ.Text).ToString("X4") + "12"}" +
                      $"{Convert.ToInt32(txt_T1_Res.Text).ToString("X4") + "12"}" +
                      $"{Convert.ToInt32(txt_T2_Occ.Text).ToString("X4") + "12"}" +
                      $"{Convert.ToInt32(txt_T2_Res.Text).ToString("X4") + "12"}" +
                  $"0203{Convert.ToInt32(txt_MT_Occ.Text).ToString("X4") + "12"}" +
                      $"{Convert.ToInt32(txt_MT_Res.Text).ToString("X4") + "12"}" +
                      $"{Convert.ToInt32(txtOther.Text).ToString("X4")}";
            txtNewDataPacket.Text = str;
            return str;
        }
        private void DisableAllPanels()
        {
            flowPnl1P.Visible = false;
            flowPnl3P.Visible = false;
            flowPnlPersistanceTime.Visible = false;
        }
        private bool isDouble(string dblVal)
        {
            try
            {
                double num = (double)float.Parse(dblVal);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
        private void textBox11_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\b')
                return;
            e.Handled = !char.IsDigit(e.KeyChar);
        }
        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\b' || e.KeyChar == '.')
                return;
            e.Handled = !char.IsDigit(e.KeyChar);
        }
        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\b' || e.KeyChar == '.')
                return;
            e.Handled = !char.IsDigit(e.KeyChar);
        }
        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\b' || e.KeyChar == '.')
                return;
            e.Handled = !char.IsDigit(e.KeyChar);
        }
        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\b' || e.KeyChar == '.')
                return;
            e.Handled = !char.IsDigit(e.KeyChar);
        }
        private void textBox12_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\b' || e.KeyChar == '.')
                return;
            e.Handled = !char.IsDigit(e.KeyChar);
        }
        private void textBox10_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\b' || e.KeyChar == '.')
                return;
            e.Handled = !char.IsDigit(e.KeyChar);
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox4.SelectedIndex = comboBox1.SelectedIndex;
        }
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = comboBox4.SelectedIndex;
        }
        private void textBox17_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\b' || e.KeyChar == '.')
                return;
            e.Handled = !char.IsDigit(e.KeyChar);
        }
        private void textBox33_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\b' || e.KeyChar == '.')
                return;
            e.Handled = !char.IsDigit(e.KeyChar);
        }
        private void textBox16_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\b' || e.KeyChar == '.')
                return;
            e.Handled = !char.IsDigit(e.KeyChar);
        }
        private void textBox20_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\b' || e.KeyChar == '.')
                return;
            e.Handled = !char.IsDigit(e.KeyChar);
        }
        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox7.SelectedIndex = comboBox6.SelectedIndex;
        }
        private void textBox19_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\b' || e.KeyChar == '.')
                return;
            e.Handled = !char.IsDigit(e.KeyChar);
        }
        private void textBox18_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\b' || e.KeyChar == '.')
                return;
            e.Handled = !char.IsDigit(e.KeyChar);
        }
        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\b' || e.KeyChar == '.')
                return;
            e.Handled = !char.IsDigit(e.KeyChar);
        }
        private void textBox23_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\b' || e.KeyChar == '.')
                return;
            e.Handled = !char.IsDigit(e.KeyChar);
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox3.SelectedIndex = comboBox2.SelectedIndex;
        }
        private void textBox21_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\b' || e.KeyChar == '.')
                return;
            e.Handled = !char.IsDigit(e.KeyChar);
        }
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void textBox25_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\b' || e.KeyChar == '.')
                return;
            e.Handled = !char.IsDigit(e.KeyChar);
        }
        private void textBox24_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\b' || e.KeyChar == '.')
                return;
            e.Handled = !char.IsDigit(e.KeyChar);
        }
        private void textBox78_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\b' || e.KeyChar == '.')
                return;
            e.Handled = !char.IsDigit(e.KeyChar);
        }
        private void textBox27_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\b' || e.KeyChar == '.')
                return;
            e.Handled = !char.IsDigit(e.KeyChar);
        }
        private void textBox87_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\b' || e.KeyChar == '.')
                return;
            e.Handled = !char.IsDigit(e.KeyChar);
        }
        private void textBox29_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\b' || e.KeyChar == '.')
                return;
            e.Handled = !char.IsDigit(e.KeyChar);
        }
        private void textBox30_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\b' || e.KeyChar == '.')
                return;
            e.Handled = !char.IsDigit(e.KeyChar);
        }
        private void txt3P_LowVolt_Occ_VoltageLess_TextChanged(object sender, EventArgs e)
        {
            txt3P_LowVolt_Res_allPhaseVoltage.Text = txt3P_LowVolt_Occ_VoltageLess.Text;
        }
        private void txt3P_HighVolt_Occ_anyPhaseVoltage_TextChanged(object sender, EventArgs e)
        {
            txt3P_HighVolt_Res_anyPhaseVoltage.Text = txt3P_HighVolt_Occ_anyPhaseVoltage.Text;
        }
        private void txt3P_VoltUnbalance_Occ_TextChanged(object sender, EventArgs e)
        {
            txt3P_VoltUnbalance_Res.Text = txt3P_VoltUnbalance_Occ.Text;
        }
        private void txt3P_OverCurr_Occ_xPhaseCurr_TextChanged(object sender, EventArgs e)
        {
            txt3P_OverCurr_Res_allPhaseCurr.Text = txt3P_OverCurr_Occ_xPhaseCurr.Text;
        }
        private void txt3P_veryLowPF_Occ_NetPF_TextChanged(object sender, EventArgs e)
        {
            txt3P_veryLowPF_Res_NetPF.Text = txt3P_veryLowPF_Occ_NetPF.Text;
        }
        private void txt3P_CTBypass_Occ_neutralCurr_TextChanged(object sender, EventArgs e)
        {
            txt3P_CurrUnbal_Occ_NeutralCurrent.Text = txt3P_CTOpen_Occ_NeutralCurrent.Text = txt3P_CTBypass_Occ_neutralCurr.Text;
        }
        private void txt3P_ND_Occ_anyPhaseVolt_TextChanged(object sender, EventArgs e)
        {
            txt3P_ND_Res_allPhaseVolt.Text = txt3P_HighVolt_Occ_allPhaseVoltage.Text = txt3P_LowVolt_OccRes_allPhaseVoltage.Text = txt3P_ND_Occ_anyPhaseVolt.Text;
        }
        private void txt_OccRes_OtherPhaseVolLimit_TextChanged(object sender, EventArgs e)
        {
            txt3P_HighVolt_Res_allPhaseVoltage.Text = txt_OccRes_OtherPhaseVolLimit.Text;
            txt3P_VoltUnbalance_OccRes_anyPhaseVolt.Text = txt_OccRes_OtherPhaseVolLimit.Text;
            txt3P_CTRev_OccRes_xVoltage.Text = txt_OccRes_OtherPhaseVolLimit.Text;
            txt3P_CTOpen_OccRes_xVoltage.Text = txt_OccRes_OtherPhaseVolLimit.Text;
            txt3P_OverCurr_OccRes_xPhaseVolt.Text = txt_OccRes_OtherPhaseVolLimit.Text;
            txt3P_ND_Res_anyPhaseVolt.Text = txt_OccRes_OtherPhaseVolLimit.Text;
            txt3P_veryLowPF_OccRes_xPhaseVolt.Text = txt_OccRes_OtherPhaseVolLimit.Text;
        }
        private void AnyTextBox_TextChanged(object sender, EventArgs e)
        {
            if (_suppressTextEvents)
                return;
            if (meterType.Contains("1P"))
            {
                BuildTamperPkt1P();
            }
            else if (meterType.Contains("3P"))
            {
                if (!TamperThresholdValidation3P())
                    return;

                BuildTamperPkt3P();
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
