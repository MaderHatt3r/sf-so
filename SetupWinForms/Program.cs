using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;

namespace Setup
{
    static class Program
    {

        private static string tempEnvironmentPath = Environment.GetEnvironmentVariable("TMP") ?? Environment.GetEnvironmentVariable("TEMP");
        private static string tempDownloadPath = tempEnvironmentPath + "\\SFSO\\";
        private static string _64bitInstaller = tempDownloadPath + "\\setup.exe";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                if (CheckSystemArchitecture())
                {
                    Application.Run(new SetupForm());
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("The installer failed to run. Please report the following error to http://CTDragon.com/discussion: \n\n" + e.Message);
            }
        }

        private static bool CheckSystemArchitecture()
        {
#if !DEBUG
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
                webClient.DownloadFile("http://updates.ctdragon.com/SFSO/Setup/x64/Setup.exe", _64bitInstaller);
                Console.WriteLine("Starting 64-bit Installation");
                Process.Start(_64bitInstaller);
                return false;
            }
#endif

            return true;
        }
    }
}
