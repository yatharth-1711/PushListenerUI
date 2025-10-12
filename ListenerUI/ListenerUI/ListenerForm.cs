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
        }

        // Append log with timestamp + color
        public void AppendLog(string message, Color color)
        {
            //rtbLogs.ScrollToCaret();
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
            //rtbLogs.Clear();
        }
        private void btnSaveData_Click(object sender, EventArgs e)
        {
            AppendLog("Data saved successfully.", Color.LightBlue);
        }
    }
}
