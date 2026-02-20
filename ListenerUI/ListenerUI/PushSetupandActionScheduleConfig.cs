using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MeterReader.DLMSInterfaceClasses
{
    public partial class PushSetupandActionScheduleConfig : Form
    {
        private Dictionary<string, List<ObisItem>> obisMap;

        public PushSetupandActionScheduleConfig()
        {
            InitializeComponent();
            LoadData();
        }
        private void LoadData()
        {
            obisMap = new Dictionary<string, List<ObisItem>>
            {
                ["Action Schedule"] = new List<ObisItem>
                {
                    new ObisItem("0.0.15.0.0.255", "Billing"),
                    new ObisItem("0.0.15.0.2.255", "Image Activation"),
                    new ObisItem("0.0.15.0.4.255", "Instant"),
                    new ObisItem("0.4.15.0.4.255", "Load Survey"),
                    new ObisItem("0.5.15.0.4.255", "Daily Energy"),
                    new ObisItem("0.0.15.0.142.255", "2 Way Communication"),
                    new ObisItem("0.0.15.0.143.255", "Tamper")
                },

                ["Push Setup"] = new List<ObisItem>
                {
                    new ObisItem("0.0.25.9.0.255", "Instant"),
                    new ObisItem("0.4.25.9.0.255", "Alert"),
                    new ObisItem("0.5.25.9.0.255", "Load Survey"),
                    new ObisItem("0.6.25.9.0.255", "Daily Energy"),
                    new ObisItem("0.130.25.9.0.255", "Self Registration"),
                    new ObisItem("0.132.25.9.0.255", "Billing"),
                    new ObisItem("0.134.25.9.0.255", "Tamper")
                }
            };

            cmbObjectType.Items.AddRange(new object[]
            {
                "Action Schedule",
                "Push Setup"
            });

            cmbObjectType.SelectedIndex = 0;
        }
        private void cmbObjectType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbObis.Items.Clear();

            var list = obisMap[cmbObjectType.Text];
            foreach (var item in list)
                cmbObis.Items.Add(item);

            cmbObis.SelectedIndex = 0;

            txtClassId.Text = cmbObjectType.Text == "Push Setup" ? "40" : "22";
        }
        private void cmbObis_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbObis.SelectedItem is ObisItem item)
            {
                txtCurrentObis.Text = item.Obis;
                txtNewObis.Clear();
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsValidObis(txtNewObis.Text))
            {
                MarkInvalid(txtNewObis);
                txtNewObis.Focus();
                return;
            }

            MarkValid(txtNewObis);

            // Save logic
            txtCurrentObis.Text = txtNewObis.Text.Trim();
        }
        private void txtNewObis_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNewObis.Text))
            {
                MarkValid(txtNewObis);
                return;
            }

            if (!IsValidObis(txtNewObis.Text))
            {
                MarkInvalid(txtNewObis);
                txtNewObis.Focus();
            }
            else
            {
                MarkValid(txtNewObis);
            }
        }
        private void txtNewObis_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNewObis.Text))
            {
                MarkValid(txtNewObis);
                return;
            }

            if (IsValidObis(txtNewObis.Text))
                MarkValid(txtNewObis);
            else
                MarkInvalid(txtNewObis);
        }
        private bool IsValidObis(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            input = input.Trim();

            // HEX format (optional 0x, max 12 hex chars)
            if (Regex.IsMatch(input, @"^(0x)?[0-9A-Fa-f]{1,12}$"))
                return true;

            // DECIMAL OBIS format (6 parts, max 3 digits each)
            if (Regex.IsMatch(input, @"^(\d{1,3}\.){5}\d{1,3}$"))
                return true;

            return false;
        }
        private void MarkInvalid(TextBox tb)
        {
            tb.BackColor = Color.MistyRose;
            tb.ForeColor = Color.DarkRed;
        }
        private void MarkValid(TextBox tb)
        {
            tb.BackColor = SystemColors.Window;
            tb.ForeColor = SystemColors.WindowText;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }
        private void btnReset_Click(object sender, EventArgs e)
        {

        }
    }
    class ObisItem
    {
        public string Obis { get; }
        public string Description { get; }

        public ObisItem(string obis, string desc)
        {
            Obis = obis;
            Description = desc;
        }

        public override string ToString()
        {
            return $"{Obis}  - {Description}";
        }
    }
}
