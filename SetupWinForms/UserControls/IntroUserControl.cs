using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;
using System.Net;

namespace Setup.UserControls
{
    public partial class IntroUserControl : UserControl
    {
        public string ID = UserControlManager.INTRO_USER_CONTROL;

        private string tempEnvironmentPath;
        private string tempDownloadPath;
        private string _64bitInstaller;

        public IntroUserControl()
        {
            InitializeComponent();
            tempEnvironmentPath = Environment.GetEnvironmentVariable("TMP") ?? Environment.GetEnvironmentVariable("TEMP");
            tempDownloadPath = tempEnvironmentPath + "\\SFSO\\";
            _64bitInstaller = tempDownloadPath + "\\setup.exe";

#if !DEBUG
            CheckSystemArchitecture();
#endif
        }

        private void CheckSystemArchitecture()
        {
            if (Environment.Is64BitProcess)
            {
                Console.WriteLine("Starting 64-bit Installer");
            }
            else
            {
                Console.WriteLine("Starting 32-bit Installer");
            }
            Console.WriteLine("Checking Environment Settings...");

            if (Environment.Is64BitOperatingSystem && !Environment.Is64BitProcess)
            {
                System.IO.Directory.CreateDirectory(tempDownloadPath);

                WebClient webClient = new WebClient();
                webClient.DownloadFile("http://updates.ctdragon.com/SFSO/Setup/x64/setup.exe", tempDownloadPath + "\\setup.exe");
                Console.WriteLine("Starting 64-bit Installation");
                Process.Start(_64bitInstaller);
            }
        }
    }
}
