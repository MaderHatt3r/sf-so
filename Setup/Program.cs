using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Setup
{
    class Program
    {
        private static string tempPath = Environment.GetEnvironmentVariable("TMP") ?? Environment.GetEnvironmentVariable("TEMP");
        private static string wordInstallerPath = tempPath + "\\SFSO\\WordInstaller\\";
        private static string excelInstallerPath = tempPath + "\\SFSO\\ExcelInstaller\\";
        private static string wordInstallerFullName = wordInstallerPath + "setup.exe";
        private static string excelInstallerFullName = wordInstallerPath + "setup.exe";
        private static string executionPath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

        static void Main(string[] args)
        {
            Console.WriteLine(executionPath);
            run();
            Environment.Exit(0);
        }

        #region Run

        public static void run()
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
                Console.WriteLine("Starting 64-bit Installation");
                Process.Start(executionPath + "\\Setup.exe");
            }
            else
            {
                try
                {
                    //InstallCodeSignatureCertificate();

                    EnablePromptForUntrustedCertificates();

                    InstallClickOnceApplications();
                }
                catch (Exception e)
                {
                    Console.Out.WriteLine(e);
                }

                Console.Out.Write("Press enter to continue...");
                Console.In.ReadLine();
            }
        }

        #endregion // Run

        #region Install Certificate

        private static void InstallCodeSignatureCertificate()
        {
            Console.WriteLine("Installing Certificates");

            //X509Certificate2 cert = new X509Certificate2("Word/SFSOspc.pfx", "ehWjjuJuVZSbgBAJUR2X", X509KeyStorageFlags.PersistKeySet);
            //X509Store store = new X509Store(StoreName.My);
            //store.Open(OpenFlags.ReadWrite);
            //store.Add(cert);

            Console.Out.WriteLine("X509Certificate2 cert = new X509Certificate2(\"C:\\Users\\CTDragon\\Desktop\\ALPHA_7\\SFSOspc.pfx\", \"\", X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);");
            X509Certificate2 cert = new X509Certificate2("C:\\Users\\CTDragon\\Desktop\\ALPHA_7\\SFSOspc.pfx", "", X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);
            Console.Out.WriteLine("X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);");
            X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            Console.Out.WriteLine("store.Open(OpenFlags.ReadWrite);");
            store.Open(OpenFlags.ReadWrite);
            Console.Out.WriteLine("store.Add(cert);");
            store.Add(cert);
        }

        #endregion // Install Certificate

        #region Enable Prompt for Untrusted Certificates

        private static void EnablePromptForUntrustedCertificates()
        {
            Console.WriteLine("Preparing Registry Entries");

            Microsoft.Win32.RegistryKey key;
            //key = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\.NETFramework\Security\TrustManager");
            key = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\.NETFramework\Security\TrustManager\PromptingLevel");
            if (key == null)
            {
                Console.Out.WriteLine("key was null");
            }
            else
            {
                key.OpenSubKey(@"SOFTWARE\Microsoft\.NETFramework\Security\TrustManager\PromptingLevel", true);
                key.SetValue("MyComputer", "Enabled");
                key.SetValue("LocalIntranet", "Enabled");
                key.SetValue("Internet", "Enabled");
                key.SetValue("TrustedSites", "Enabled");
                key.SetValue("UntrustedSites", "Enabled");
                key.Close();
            }
        }

        #endregion // Enable Prompt for Untrusted Certificates

        #region Install ClickOnce Applications

        private static void InstallClickOnceApplications()
        {
            DownloadSetupFiles();

            Console.WriteLine("Installing Software");
            Process.Start(wordInstallerFullName).WaitForExit();
            //Process.Start(excelInstallerFullName).WaitForExit();
        }

        private static void DownloadSetupFiles()
        {
            System.IO.Directory.CreateDirectory(wordInstallerPath);
            System.IO.Directory.CreateDirectory(excelInstallerPath);

            WebClient webClient = new WebClient();
            Console.WriteLine("Downloading Dependencies");
            webClient.DownloadProgressChanged += webClient_DownloadProgressChanged;
            webClient.DownloadFileCompleted += webClient_DownloadFileCompleted;
            Console.WriteLine();
            webClient.DownloadFile("http://updates.ctdragon.com/SFSO/Word/setup.exe", wordInstallerFullName);
            Console.WriteLine();
            //webClient.DownloadFile("http://updates.ctdragon.com/SFSO/Excel/setup.exe", excelInstallerFullName);
        }

        static void webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Console.Write("\r{0}%", e.BytesReceived + "/ " + e.TotalBytesToReceive + e.ProgressPercentage);
        }

        static void webClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            Console.WriteLine(Console.Out.NewLine + "Download Completed");
        }

        #endregion // Install ClickOnce Applications
    }
}
