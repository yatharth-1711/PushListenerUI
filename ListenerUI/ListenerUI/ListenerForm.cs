using System;
using System.Drawing;
using System.Windows.Forms;

namespace ListenerUI
{
    public partial class ListenerForm : Form
    {
        private readonly string[] profileNames =
        {
            "Instant", "Load Survey", "Daily Energy",
            "Self Registration", "Billing", "Current Bill", "Event Log"
        };

        public ListenerForm()
        {
            InitializeComponent();
            InitializeDynamicProfiles();
        }

        private void InitializeDynamicProfiles()
        {
            // Add row styles and controls dynamically
            for (int i = 0; i < profileNames.Length; i++)
            {
                this.tblProfileSettings.RowStyles.Add(
                    new RowStyle(SizeType.Percent, 100F / profileNames.Length));

                Label lbl = new Label
                {
                    Text = $"{profileNames[i]} IP:",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleRight,
                    Font = new Font("Segoe UI", 9F)
                };

                TextBox txtIP = new TextBox
                {
                    Name = $"txtIP_{profileNames[i].Replace(" ", "")}",
                    Dock = DockStyle.Fill
                };

                DateTimePicker dtSchedule = new DateTimePicker
                {
                    Format = DateTimePickerFormat.Time,
                    ShowUpDown = true,
                    Dock = DockStyle.Fill,
                    Name = $"dtSchedule_{profileNames[i].Replace(" ", "")}"
                };

                this.tblProfileSettings.Controls.Add(lbl, 0, i);
                this.tblProfileSettings.Controls.Add(txtIP, 1, i);
                this.tblProfileSettings.Controls.Add(dtSchedule, 2, i);
            }

            // Create tab pages and grids
            foreach (string profile in profileNames)
            {
                TabPage tab = new TabPage(profile);
                DataGridView grid = new DataGridView
                {
                    Dock = DockStyle.Fill,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    AllowUserToAddRows = false,
                    ReadOnly = true,
                    Name = $"dgProfile_{profile.Replace(" ", "")}"
                };
                tab.Controls.Add(grid);
                this.tabProfiles.TabPages.Add(tab);
            }
        }

        // Append log with timestamp + color
        public void AppendLog(string message, Color color)
        {
            if (rtbLogs.InvokeRequired)
            {
                rtbLogs.Invoke(new Action(() => AppendLog(message, color)));
                return;
            }

            string timestamp = DateTime.Now.ToString("HH:mm:ss");
            rtbLogs.SelectionColor = color;
            rtbLogs.AppendText($"[{timestamp}] {message}\n");
            rtbLogs.ScrollToCaret();
        }

        private void btnStartListener_Click(object sender, EventArgs e)
        {
            AppendLog("Listener started.", Color.LightGreen);
        }
        private void btnStopListener_Click(object sender, EventArgs e)
        {
            AppendLog("Listener stopped.", Color.Red);
        }
        private void btnClearLogs_Click(object sender, EventArgs e)
        {
            rtbLogs.Clear();
        }
        private void btnSaveData_Click(object sender, EventArgs e)
        {
            AppendLog("Data saved successfully.", Color.LightBlue);
        }
    }
}
