using MeterReader.DLMSInterfaceClasses;
using MeterReader.HelperForms;
using MeterReader.HelperForms.FGConfigurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ListenerUI
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new MainForm());
            //Application.Run(new ListenerForm());
            //Application.Run(new FGComparisonForm());
            ////Application.Run(new PushPacketDecrypter());
            Application.Run(new PushPacketDecrypterFrm());
            //Application.Run(new FGConfigurationsFrm());
            //Application.Run(new PushSetupandActionScheduleConfig());
        }
    }
}
