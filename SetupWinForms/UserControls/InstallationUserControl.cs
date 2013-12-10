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

        public InstallationUserControl()
        {
            InitializeComponent();
        }

        public void Install()
        {
            CustomInstaller installer = new CustomInstaller();

            HookIntoInstallerEvents(installer);

            foreach (Versions version in GlobalApplicationOptions.VersionsToInstall)
            {
                switch (version)
                {
                    case Versions.WORD:
                        installer.InstallApplication("http://updates.ctdragon.com/SFSO/Word/SFSO.vsto", version);
                        break;
                    case Versions.EXCEL:
                        installer.InstallApplication("http://updates.ctdragon.com/SFSO/Excel/SFSO-E.vsto", version);
                        break;
                    default:
                        break;
                }
            }
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
            installer.ApplicationDownloadCompleted += installer_ApplicationDownloadCompleted;
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
            //GlobalApplicationOptions.ucManager.NextScreen();
        }

        private void installer_ApplicationDownloadCompleted(object sender, System.Deployment.Application.DownloadApplicationCompletedEventArgs e)
        {
            currentTaskProgressBar.Value = 100;
            overallProgressBar.Increment(33);

            GlobalApplicationOptions.ucManager.NextScreen();
        }

        private void installer_ApplicationDownloadProgressChanged(object sender, System.Deployment.Application.DownloadProgressChangedEventArgs e)
        {
            currentTaskProgressBar.Value = e.ProgressPercentage;
        }

        private void installer_DownloadingApplicationStarted(object sender, EventArgs e)
        {
            currentTaskProgressBar.Value = 0;
            currentTaskTextBox.Text = "Downloading Appication";
        }

        private void installer_InitializeManifestCompleted(object sender, System.Deployment.Application.GetManifestCompletedEventArgs e)
        {
            overallProgressBar.Increment(33);
        }

        private void installer_InitializingManifestStarted(object sender, EventArgs e)
        {
            currentTaskProgressBar.Value = 0;
            currentTaskTextBox.Text = "Initializing Manifest";
        }

        private void installer_InstallingCertificatesCompleted(object sender, EventArgs e)
        {
            currentTaskProgressBar.Value = 100;
        }

        private void installer_InstallingCertificatesProgressChange(object sender, InstallingCertificatesProgessChangeEventArgs e)
        {
            currentTaskProgressBar.Value = e.PercentCompleted;
        }

        private void installer_InstallingCertificatesStarted(object sender, EventArgs e)
        {
            currentTaskProgressBar.Value = 0;
            currentTaskTextBox.Text = "Installing Certificates";
        }

        private void installer_CertDownloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            overallProgressBar.Increment(33);
        }

        private void installer_CertDownloadProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
        {
            currentTaskProgressBar.Value = e.ProgressPercentage;
        }

        private void installer_DownloadingCertificatesStarted(object sender, EventArgs e)
        {
            currentTaskProgressBar.Value = 0;
            currentTaskTextBox.Text = "Downloading Certificates";
        }




    }
}
