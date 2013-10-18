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
        private static string tempEnvironmentPath = Environment.GetEnvironmentVariable("TMP") ?? Environment.GetEnvironmentVariable("TEMP");
        private static string tempDownloadPath = tempEnvironmentPath + "\\SFSO\\";
        private static string wordInstallerPath = tempEnvironmentPath + "\\SFSO\\WordInstaller\\";
        private static string excelInstallerPath = tempEnvironmentPath + "\\SFSO\\ExcelInstaller\\";
        private static string wordInstallerFullName = wordInstallerPath + "setup.exe";
        private static string excelInstallerFullName = wordInstallerPath + "setup.exe";
        private static string certificateFullName = tempDownloadPath + "SFSOspc.pfx";
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
                    DownloadSetupFiles();

                    InstallCodeSignatureCertificate();

                    //EnablePromptForUntrustedCertificates();

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

            X509Certificate2 cert = new X509Certificate2(certificateFullName, "", X509KeyStorageFlags.PersistKeySet);
            X509Certificate2 cert2 = new X509Certificate2(tempDownloadPath + "SFSOspc.cer", "ehWjjuJuVZSbgBAJUR2X", X509KeyStorageFlags.PersistKeySet);
            X509Certificate2 cert3 = new X509Certificate2(tempDownloadPath + "SFSOCert.cer", "QrNpklpcr143XAScRgi8", X509KeyStorageFlags.PersistKeySet);
            X509Store store = new X509Store(StoreName.Root);
            store.Open(OpenFlags.ReadWrite);
            store.Add(cert);
            store.Add(cert2);
            store.Add(cert3);

            //Console.Out.WriteLine("X509Certificate2 cert = new X509Certificate2(\"C:\\Users\\CTDragon\\Desktop\\ALPHA_7\\SFSOspc.pfx\", \"\", X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);");
            X509Certificate2 xCert = new X509Certificate2(certificateFullName, "", X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);
            X509Certificate2 xCert2 = new X509Certificate2(tempDownloadPath + "SFSOspc.cer", "ehWjjuJuVZSbgBAJUR2X", X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);
            X509Certificate2 xCert3 = new X509Certificate2(tempDownloadPath + "SFSOCert.cer", "QrNpklpcr143XAScRgi8", X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);
            //Console.Out.WriteLine("X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);");
            X509Store xStore = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
            //Console.Out.WriteLine("store.Open(OpenFlags.ReadWrite);");
            xStore.Open(OpenFlags.ReadWrite);
            //Console.Out.WriteLine("store.Add(cert);");
            xStore.Add(xCert);
            xStore.Add(xCert2);
            xStore.Add(xCert3);
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
            Console.WriteLine("..");
            webClient.DownloadFile("http://updates.ctdragon.com/SFSO/Word/setup.exe", wordInstallerFullName);
            //Console.WriteLine("..");
            //webClient.DownloadFile("http://updates.ctdragon.com/SFSO/Excel/setup.exe", excelInstallerFullName);
            Console.WriteLine("..");
            webClient.DownloadFile("http://updates.ctdragon.com/SFSO/Certificates/SelfSigned/SHA1_DefaultEncryption/SFSOspc.pfx", certificateFullName);
            Console.WriteLine("..");
            webClient.DownloadFile("http://updates.ctdragon.com/SFSO/Certificates/SelfSigned/SHA1_DefaultEncryption/SFSOspc.cer", tempDownloadPath + "SFSOspc.cer");
            Console.WriteLine("..");
            webClient.DownloadFile("http://updates.ctdragon.com/SFSO/Certificates/SelfSigned/SHA1_DefaultEncryption/SFSOCert.cer", tempDownloadPath + "SFSOCert.cer");
        }

        static void webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Console.Write("\r{0}%", e.BytesReceived + "/ " + e.TotalBytesToReceive + "____" + e.ProgressPercentage);
            System.Threading.Thread.Sleep(50);
        }

        static void webClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            Console.WriteLine(Console.Out.NewLine + "Download Completed");
        }

        #endregion // Install ClickOnce Applications
    }
}
