using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnlineInstaller
{
    static class Program
    {
        private static string tempEnvironmentPath = Environment.GetEnvironmentVariable("TMP") ?? Environment.GetEnvironmentVariable("TEMP");
        private static string tempDownloadPath = tempEnvironmentPath + "\\SFSO\\";
        private static string installer = tempDownloadPath + "\\setup.exe";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());

            try
            {
                Install();
            }
            catch (Exception e)
            {
                MessageBox.Show("Something went wrong during installation.\nPlease report the following error to ctdragon.com\n\n\n" + e.Message);
            }

            Application.Exit();
            Environment.Exit(0);
        }

        private static void Install()
        {
            if (Environment.Is64BitOperatingSystem)
            {
                DownloadAndRunElevated("http://updates.ctdragon.com/SFSO/Setup/x64/Setup.exe");
            }
            else
            {
                DownloadAndRunElevated("http://updates.ctdragon.com/SFSO/Setup/Setup.exe");
            }
            
        }

        private static void DownloadAndRunElevated(string url)
        {
            System.IO.Directory.CreateDirectory(tempDownloadPath);

            WebClient webClient = new WebClient();
            webClient.DownloadFile(url, installer);

            ProcessStartInfo processInfo = new ProcessStartInfo();
            processInfo.Verb = @"runas";
            //processInfo.LoadUserProfile = true;
            processInfo.FileName = installer;
            Process.Start(processInfo);
        }
    }
}
