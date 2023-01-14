using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoundSourceAdjuster
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

            var audioDevices = new AudioDevicesService();
            var notifyIconController = NotifyIconController.Create(audioDevices);
            Application.Run();
        }
    }
}
