using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Setup.Model;

namespace Setup.UserControls
{
    public partial class InstallationUserControl : UserControl
    {
        public string ID = UserControlManager.INSTALLATION_USER_CONTROL;

        private object lockObj = new object();
        // Used to maintain a reference to the object so they are not collected by garbage
        private List<CustomInstaller> customInstallers = new List<CustomInstaller>();

        public InstallationUserControl()
        {
            InitializeComponent();
            currentTaskTextBox.Visible = false;
            currentTaskProgressBar.Visible = false;
            overallTextBox.Visible = false;
            overallProgressBar.Visible = false;
        }

        public void Install()
        {
            titleTextBox.Text = "Installing";
            descriptionTextBox.Text = "Please wait while your installation is in progress.";
            currentTaskTextBox.Visible = true;
            currentTaskProgressBar.Visible = true;
            overallTextBox.Visible = true;
            overallProgressBar.Visible = true;

            this.Refresh();

            CustomInstaller installer = new CustomInstaller();
            customInstallers.Add(installer);

            HookIntoInstallerEvents(installer);

            //if (GlobalApplicationOptions.VersionsToInstall.Count <= 0)
            //{
            //    GlobalApplicationOptions.ucManager.NextScreen();
            //}
            //else
            //{
            //    lock (lockObj)
            //    {
            //        installer.InstallApplication(GlobalApplicationOptions.VersionsToInstall[0]);
            //        try
            //        {
            //            GlobalApplicationOptions.VersionsToInstall.RemoveAt(0);
            //        }
            //        catch (ArgumentOutOfRangeException)
            //        {

            //        }
            //    }
            //}

            //foreach (Versions version in GlobalApplicationOptions.VersionsToInstall)
            //{
            //    switch (version)
            //    {
            //        case Versions.WORD:
            //            installing = true;
            //            installer.InstallApplication("http://updates.ctdragon.com/SFSO/Word/SFSO.vsto", version);
            //            break;
            //        case Versions.EXCEL:
            //            installing = true;
            //            while (installing)
            //            {
            //                System.Threading.Thread.Sleep(1000);
            //                continue;
            //            }
            //            installer.InstallApplication("http://updates.ctdragon.com/SFSO/Excel/SFSO-E.vsto", version);
            //            break;
            //        default:
            //            break;
            //    }
            //}

            foreach (Versions version in GlobalApplicationOptions.VersionsToInstall)
            {
                installer.InstallApplication(version);
                overallProgressBar.Value = (int)(100 / (GlobalApplicationOptions.VersionsToInstall.Count + .001));
            }

            GlobalApplicationOptions.ucManager.NextScreen();
        }

        private void HookIntoInstallerEvents(CustomInstaller installer)
        {
            installer.DownloadingCertificatesStarted += installer_DownloadingCertificatesStarted;
            installer.CertDownloadProgressChanged += installer_CertDownloadProgressChanged;
            installer.CertDownloadCompleted += installer_CertDownloadCompleted;
            installer.InstallingCertificatesStarted += installer_InstallingCertificatesStarted;
            installer.InstallingCertificatesProgressChange += installer_InstallingCertificatesProgressChange;
            installer.InstallingCertificatesCompleted += installer_InstallingCertificatesCompleted;
            installer.InitializingManifestStarted += installer_InitializingManifestStarted;
            installer.InitializeManifestCompleted += installer_InitializeManifestCompleted;
            installer.DownloadingApplicationStarted += installer_DownloadingApplicationStarted;
            installer.ApplicationDownloadProgressChanged += installer_ApplicationDownloadProgressChanged;
            installer.InstallationComplete += installer_InstallationComplete;
            installer.ErrorDuringInstallation += installer_ErrorDuringInstallation;

        //    public event EventHandler DownloadingCertificatesStarted;
        //public event EventHandler<System.Net.DownloadProgressChangedEventArgs> CertDownloadProgressChanged;
        //public event EventHandler<System.ComponentModel.AsyncCompletedEventArgs> CertDownloadCompleted;
        //public event EventHandler InstallingCertificatesStarted;
        //public event EventHandler<System.Net.DownloadProgressChangedEventArgs> InstallingCertificatesProgressChange;
        //public event EventHandler InstallingCertificatesCompleted;
        //public event EventHandler InitializingManifestStarted;
        //public event EventHandler<GetManifestCompletedEventArgs> InitializeManifestCompleted;
        //public event EventHandler DownloadingApplicationStarted;
        //public event EventHandler<DownloadProgressChangedEventArgs> ApplicationDownloadProgressChanged;
        //public event EventHandler<DownloadApplicationCompletedEventArgs> ApplicationDownloadCompleted;
            //public event EventHandler ErrorDuringInstallation;
        }

        private void installer_ErrorDuringInstallation(object sender, EventArgs e)
        {
            GlobalApplicationOptions.ErrorsDuringInstallation = true;
            //lock (lockObj)
            //{
            //    //GlobalApplicationOptions.VersionsToInstall.Clear();
            //}
            //Install();
        }

        private void installer_InstallationComplete(object sender, InstallationCompleteEventArgs e)
        {
            currentTaskProgressBar.Value = 100;
            overallProgressBar.Increment((int)(33 / (GlobalApplicationOptions.VersionsToInstall.Count + .001)));
            //System.Threading.Thread.Sleep(300);

            //Install();
        }

        private void installer_ApplicationDownloadProgressChanged(object sender, System.Deployment.Application.DownloadProgressChangedEventArgs e)
        {
            currentTaskProgressBar.Value = e.ProgressPercentage;
            //System.Threading.Thread.Sleep(200);
        }

        private void installer_DownloadingApplicationStarted(object sender, EventArgs e)
        {
            currentTaskProgressBar.Value = 0;
            currentTaskTextBox.Text = "Downloading Appication";
        }

        private void installer_InitializeManifestCompleted(object sender, System.Deployment.Application.GetManifestCompletedEventArgs e)
        {
            currentTaskProgressBar.Value = 100;
            overallProgressBar.Increment((int)(33 / (GlobalApplicationOptions.VersionsToInstall.Count + .001)));
            //System.Threading.Thread.Sleep(200);
        }

        private void installer_InitializingManifestStarted(object sender, EventArgs e)
        {
            currentTaskProgressBar.Value = 0;
            currentTaskTextBox.Text = "Initializing Manifest";
            //System.Threading.Thread.Sleep(200);
        }

        private void installer_InstallingCertificatesCompleted(object sender, EventArgs e)
        {
            currentTaskProgressBar.Value = 100;
            //System.Threading.Thread.Sleep(200);
        }

        private void installer_InstallingCertificatesProgressChange(object sender, InstallingCertificatesProgessChangeEventArgs e)
        {
            currentTaskProgressBar.Value = e.PercentCompleted;
            //System.Threading.Thread.Sleep(200);
        }

        private void installer_InstallingCertificatesStarted(object sender, EventArgs e)
        {
            currentTaskProgressBar.Value = 0;
            currentTaskTextBox.Text = "Installing Certificates";
        }

        private void installer_CertDownloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            currentTaskProgressBar.Value = 100;
            overallProgressBar.Value += (int)(33 / (GlobalApplicationOptions.VersionsToInstall.Count + .001));
            System.Threading.Thread.Sleep(80);
        }

        private void installer_CertDownloadProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
        {
            currentTaskProgressBar.Value = e.ProgressPercentage;
            //System.Threading.Thread.Sleep(200);
        }

        private void installer_DownloadingCertificatesStarted(object sender, EventArgs e)
        {
            currentTaskProgressBar.Value = 10;
            currentTaskTextBox.Text = "Downloading Certificates";
            //System.Threading.Thread.Sleep(200);
        }




    }
}
