using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Deployment.Application;
using System.Windows.Forms;
using System.Security.Cryptography.X509Certificates;

namespace Setup.Model
{
    public class InstallingCertificatesProgessChangeEventArgs : EventArgs
    {
        public int PercentCompleted { get; set; }
    }

    public class InstallationCompleteEventArgs : EventArgs
    {
        public Versions version { get; set; }
    }

    public class CustomInstaller
    {

        #region Data

        private Versions version;

        private static string tempEnvironmentPath = Environment.GetEnvironmentVariable("TMP") ?? Environment.GetEnvironmentVariable("TEMP");
        private static string tempDownloadPath = tempEnvironmentPath + "\\SFSO\\";
        private static string wordInstallerPath = tempEnvironmentPath + "\\SFSO\\WordInstaller\\";
        private static string excelInstallerPath = tempEnvironmentPath + "\\SFSO\\ExcelInstaller\\";
        private static string wordInstallerFullName = wordInstallerPath + "setup.exe";
        private static string excelInstallerFullName = excelInstallerPath + "setup.exe";
        private static string certificateFullName = tempDownloadPath + "SFSOspc.pfx";
        private static string certificateFullName_Excel = tempDownloadPath + "SFSOEspc.pfx";
        //private static string executionPath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

        InPlaceHostingManager iphm = null;

        public event EventHandler DownloadingCertificatesStarted;
        public event EventHandler<System.Net.DownloadProgressChangedEventArgs> CertDownloadProgressChanged;
        public event EventHandler<System.ComponentModel.AsyncCompletedEventArgs> CertDownloadCompleted;
        public event EventHandler InstallingCertificatesStarted;
        public event EventHandler<InstallingCertificatesProgessChangeEventArgs> InstallingCertificatesProgressChange;
        public event EventHandler InstallingCertificatesCompleted;
        public event EventHandler InitializingManifestStarted;
        public event EventHandler<GetManifestCompletedEventArgs> InitializeManifestCompleted;
        public event EventHandler DownloadingApplicationStarted;
        public event EventHandler<DownloadProgressChangedEventArgs> ApplicationDownloadProgressChanged;
        public event EventHandler<InstallationCompleteEventArgs> InstallationComplete;
        public event EventHandler ErrorDuringInstallation;

        #endregion // Data

        public void InstallApplication(Versions version)
        {
            this.version = version;
            string deployManifestUriStr = "";

            switch (version)
            {
                case Versions.WORD:
                    deployManifestUriStr = "http://updates.ctdragon.com/SFSO/Word/SFSO.vsto";
                    break;
                case Versions.EXCEL:
                    deployManifestUriStr = "http://updates.ctdragon.com/SFSO/Excel/SFSO-E.vsto";
                    break;
                default:
                    break;
            }

            try
            {
                DownloadSetupFiles();

                switch (version)
                {
                    case Versions.WORD:
                        InstallCodeSignatureCertificate();
                        break;
                    case Versions.EXCEL:
                        InstallCodeSignatureCertificate_Excel();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {

                GlobalApplicationOptions.ErrorMessage += Environment.NewLine + "Error installing certificates: " + e.Message;
                ErrorDuringInstallation(this, new EventArgs());
                return;
            }

            try
            {
                InitializingManifestStarted(this, new EventArgs());
                Uri deploymentUri = new Uri(deployManifestUriStr);
                iphm = new InPlaceHostingManager(deploymentUri, false);
            }
            catch (UriFormatException uriEx)
            {
                //MessageBox.Show("Cannot install the application: " +
                //    "The deployment manifest URL supplied is not a valid URL. " +
                //    "Error: " + uriEx.Message);

                GlobalApplicationOptions.ErrorMessage += Environment.NewLine + "Cannot install the application: " +
                    "The deployment manifest URL supplied is not a valid URL. " +
                    "Error: " + uriEx.Message;
                ErrorDuringInstallation(this, new EventArgs());
                return;
            }
            catch (PlatformNotSupportedException platformEx)
            {
                //MessageBox.Show("Cannot install the application: " +
                //    "This program requires Windows XP or higher. " +
                //    "Error: " + platformEx.Message);

                GlobalApplicationOptions.ErrorMessage += Environment.NewLine + "Cannot install the application: " +
                    "This program requires Windows XP or higher. " +
                    "Error: " + platformEx.Message;
                ErrorDuringInstallation(this, new EventArgs());
                return;
            }
            catch (ArgumentException argumentEx)
            {
                //MessageBox.Show("Cannot install the application: " +
                //    "The deployment manifest URL supplied is not a valid URL. " +
                //    "Error: " + argumentEx.Message);

                GlobalApplicationOptions.ErrorMessage += Environment.NewLine + "Cannot install the application: " +
                    "The deployment manifest URL supplied is not a valid URL. " +
                    "Error: " + argumentEx.Message;
                ErrorDuringInstallation(this, new EventArgs());
                return;
            }

            iphm.GetManifestCompleted += new EventHandler<GetManifestCompletedEventArgs>(iphm_GetManifestCompleted);
            iphm.GetManifestAsync();
        }

        void iphm_GetManifestCompleted(object sender, GetManifestCompletedEventArgs e)
        {
            InitializeManifestCompleted(this, e);

            // Check for an error. 
            if (e.Error != null)
            {
                // Cancel download and install.
                //MessageBox.Show("Could not download manifest. Error: " + e.Error.Message);
                GlobalApplicationOptions.ErrorMessage = "Could not download manifest. Error: " + e.Error.Message;
                ErrorDuringInstallation(this, new EventArgs());
                return;
            }

            // bool isFullTrust = CheckForFullTrust(e.ApplicationManifest); 

            // Verify this application can be installed. 
            //try
            //{
            //    // the true parameter allows InPlaceHostingManager 
            //    // to grant the permissions requested in the applicaiton manifest.
            //    iphm.AssertApplicationRequirements(true);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("An error occurred while verifying the application. " +
            //        "Error: " + ex.Message);
            //    return;
            //}

            // Use the information from GetManifestCompleted() to confirm  
            // that the user wants to proceed. 
            string appInfo = "Application Name: " + e.ProductName;
            appInfo += "\nVersion: " + e.Version;
            appInfo += "\nSupport/Help Requests: " + (e.SupportUri != null ?
                e.SupportUri.ToString() : "N/A");
            appInfo += "\n\nConfirmed that this application can run with its requested permissions.";
            // if (isFullTrust) 
            // appInfo += "\n\nThis application requires full trust in order to run.";
            appInfo += "\n\nProceed with installation?";

            //DialogResult dr = MessageBox.Show(appInfo, "Confirm Application Install",
            //    MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            //if (dr != System.Windows.Forms.DialogResult.OK)
            //{
            //    return;
            //}

            // Download the deployment manifest. 
            iphm.DownloadProgressChanged += new EventHandler<DownloadProgressChangedEventArgs>(iphm_DownloadProgressChanged);
            iphm.DownloadApplicationCompleted += new EventHandler<DownloadApplicationCompletedEventArgs>(iphm_DownloadApplicationCompleted);

            try
            {
                // Usually this shouldn't throw an exception unless AssertApplicationRequirements() failed,  
                // or you did not call that method before calling this one.
                DownloadingApplicationStarted(this, new EventArgs());
                iphm.DownloadApplicationAsync();
            }
            catch (Exception downloadEx)
            {
                //MessageBox.Show("Cannot initiate download of application. Error: " +
                //    downloadEx.Message);

                GlobalApplicationOptions.ErrorMessage += Environment.NewLine + "Cannot initiate download of application. Error: " + downloadEx.Message;
                ErrorDuringInstallation(this, new EventArgs());
                return;
            }
        }

        /*
        private bool CheckForFullTrust(XmlReader appManifest)
        {
            if (appManifest == null)
            {
                throw (new ArgumentNullException("appManifest cannot be null."));
            }

            XAttribute xaUnrestricted =
                XDocument.Load(appManifest)
                    .Element("{urn:schemas-microsoft-com:asm.v1}assembly")
                    .Element("{urn:schemas-microsoft-com:asm.v2}trustInfo")
                    .Element("{urn:schemas-microsoft-com:asm.v2}security")
                    .Element("{urn:schemas-microsoft-com:asm.v2}applicationRequestMinimum")
                    .Element("{urn:schemas-microsoft-com:asm.v2}PermissionSet")
                    .Attribute("Unrestricted"); // Attributes never have a namespace

            if (xaUnrestricted != null)
                if (xaUnrestricted.Value == "true")
                    return true;

            return false;
        }
        */

        void iphm_DownloadApplicationCompleted(object sender, DownloadApplicationCompletedEventArgs e)
        {
            InstallationCompleteEventArgs icea = new InstallationCompleteEventArgs();
            icea.version = this.version;
            InstallationComplete(this, icea);
            // Check for an error. 
            if (e.Error != null)
            {
                // Cancel download and install.
                //MessageBox.Show("Could not download and install application. Error: " + e.Error.Message);
                GlobalApplicationOptions.ErrorMessage = "Could not download and install application. Error: " + e.Error.Message;
                ErrorDuringInstallation(this, new EventArgs());
                return;
            }

            // Inform the user that their application is ready for use. 
            //MessageBox.Show("Application installed!");
            //MessageBox.Show(e.LogFilePath);
        }

        void iphm_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            ApplicationDownloadProgressChanged(this, e);
            // you can show percentage of task completed using e.ProgressPercentage
        }

        #region Old Installer Methods

        #region Download Setup Files

        private void DownloadSetupFiles()
        {
            DownloadingCertificatesStarted(this, new EventArgs());

            System.IO.Directory.CreateDirectory(wordInstallerPath);
            System.IO.Directory.CreateDirectory(excelInstallerPath);

            System.Net.WebClient webClient = new System.Net.WebClient();
            //Console.WriteLine("Downloading Dependencies");
            webClient.DownloadProgressChanged += webClient_DownloadProgressChanged;
            webClient.DownloadFileCompleted += webClient_DownloadFileCompleted;
            //Console.WriteLine("..");
            //webClient.DownloadFile("http://updates.ctdragon.com/SFSO/Word/setup.exe", wordInstallerFullName);
            ////Console.WriteLine("..");
            //webClient.DownloadFile("http://updates.ctdragon.com/SFSO/Excel/setup.exe", excelInstallerFullName);
            //Console.WriteLine("..");
            webClient.DownloadFile("http://updates.ctdragon.com/SFSO/Certificates/SelfSigned/SHA1_DefaultEncryption/SFSOspc.pfx", certificateFullName);
            //Console.WriteLine("..");
            webClient.DownloadFile("http://updates.ctdragon.com/SFSO/Certificates/SelfSigned/SHA1_DefaultEncryption/SFSOspc.cer", tempDownloadPath + "SFSOspc.cer");
            //Console.WriteLine("..");
            webClient.DownloadFile("http://updates.ctdragon.com/SFSO/Certificates/SelfSigned/SHA1_DefaultEncryption/SFSOCert.cer", tempDownloadPath + "SFSOCert.cer");
            //Console.WriteLine("..");
            webClient.DownloadFile("http://updates.ctdragon.com/SFSO/Certificates/SelfSigned/SHA1_DefaultEncryption/SFSOEspc.pfx", certificateFullName_Excel);
            //Console.WriteLine("..");
            webClient.DownloadFile("http://updates.ctdragon.com/SFSO/Certificates/SelfSigned/SHA1_DefaultEncryption/SFSOEspc.cer", tempDownloadPath + "SFSOEspc.cer");
            //Console.WriteLine("..");
            webClient.DownloadFile("http://updates.ctdragon.com/SFSO/Certificates/SelfSigned/SHA1_DefaultEncryption/SFSOECert.cer", tempDownloadPath + "SFSOECert.cer");

        }

        private void webClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            CertDownloadCompleted(this, e);
        }

        private void webClient_DownloadProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
        {
            CertDownloadProgressChanged(this, e);
        }

        #endregion // Download Setup Files

        #region Install Certificates

        private void InstallCodeSignatureCertificate()
        {
            InstallingCertificatesStarted(this, new EventArgs());
            //Console.WriteLine("Installing Certificates");

            X509Certificate2 cert = new X509Certificate2(certificateFullName, "", X509KeyStorageFlags.PersistKeySet);
            X509Certificate2 cert2 = new X509Certificate2(tempDownloadPath + "SFSOspc.cer", "ehWjjuJuVZSbgBAJUR2X", X509KeyStorageFlags.PersistKeySet);
            X509Certificate2 cert3 = new X509Certificate2(tempDownloadPath + "SFSOCert.cer", "QrNpklpcr143XAScRgi8", X509KeyStorageFlags.PersistKeySet);
            X509Store store = new X509Store(StoreName.Root);
            store.Open(OpenFlags.ReadWrite);
            store.Add(cert);
            store.Add(cert2);
            store.Add(cert3);

            InstallingCertificatesProgessChangeEventArgs icpcea = new InstallingCertificatesProgessChangeEventArgs();
            icpcea.PercentCompleted = 50;
            InstallingCertificatesProgressChange(this, icpcea);

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

            icpcea.PercentCompleted = 100;
            InstallingCertificatesProgressChange(this, icpcea);

            InstallingCertificatesCompleted(this, new EventArgs());
        }

        private void InstallCodeSignatureCertificate_Excel()
        {
            InstallingCertificatesStarted(this, new EventArgs());
            //Console.WriteLine("Installing Certificates");

            X509Certificate2 cert = new X509Certificate2(certificateFullName_Excel, "", X509KeyStorageFlags.PersistKeySet);
            X509Certificate2 cert2 = new X509Certificate2(tempDownloadPath + "SFSOEspc.cer", "Fe5Tb1Y0xpgShvYMwbiw", X509KeyStorageFlags.PersistKeySet);
            X509Certificate2 cert3 = new X509Certificate2(tempDownloadPath + "SFSOECert.cer", "Pk9NF8xBQUToIBc0PfRb", X509KeyStorageFlags.PersistKeySet);
            X509Store store = new X509Store(StoreName.Root);
            store.Open(OpenFlags.ReadWrite);
            store.Add(cert);
            store.Add(cert2);
            store.Add(cert3);

            InstallingCertificatesProgessChangeEventArgs icpcea = new InstallingCertificatesProgessChangeEventArgs();
            icpcea.PercentCompleted = 50;
            InstallingCertificatesProgressChange(this, icpcea);

            //Console.Out.WriteLine("X509Certificate2 cert = new X509Certificate2(\"C:\\Users\\CTDragon\\Desktop\\ALPHA_7\\SFSOspc.pfx\", \"\", X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);");
            X509Certificate2 xCert = new X509Certificate2(certificateFullName_Excel, "", X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);
            X509Certificate2 xCert2 = new X509Certificate2(tempDownloadPath + "SFSOEspc.cer", "Fe5Tb1Y0xpgShvYMwbiw", X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);
            X509Certificate2 xCert3 = new X509Certificate2(tempDownloadPath + "SFSOECert.cer", "Pk9NF8xBQUToIBc0PfRb", X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);
            //Console.Out.WriteLine("X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);");
            X509Store xStore = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
            //Console.Out.WriteLine("store.Open(OpenFlags.ReadWrite);");
            xStore.Open(OpenFlags.ReadWrite);
            //Console.Out.WriteLine("store.Add(cert);");
            xStore.Add(xCert);
            xStore.Add(xCert2);
            xStore.Add(xCert3);

            icpcea.PercentCompleted = 100;
            InstallingCertificatesProgressChange(this, icpcea);

            InstallingCertificatesCompleted(this, new EventArgs());
        }

        #endregion // Install Certificates

        #endregion // Olde Installer Methods


    }
}
