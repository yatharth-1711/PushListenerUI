using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ListenerUI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            AddListenerFormToTabPage();
        }
        private void AddListenerFormToTabPage()
        {
            if (tabPageWirelessComm != null)
            {
                ListenerForm listenerForm = new ListenerForm();
                listenerForm.TopLevel = false;
                listenerForm.FormBorderStyle = FormBorderStyle.None;
                listenerForm.Dock = DockStyle.Fill;

                tabPageWirelessComm.Controls.Clear();
                tabPageWirelessComm.Controls.Add(listenerForm);
                listenerForm.Show();
            }
            else
            {
                MessageBox.Show("tabPageLCDConfig not found in tabControl2.");
            }
        }
    }
}
