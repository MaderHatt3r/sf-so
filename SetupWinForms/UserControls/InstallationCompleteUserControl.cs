using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Setup.UserControls
{
    public partial class InstallationCompleteUserControl : UserControl
    {
        public string ID = UserControlManager.INSTALLATION_COMPLETE_USER_CONTROL;

        public InstallationCompleteUserControl()
        {
            InitializeComponent();
            supportSiteLinkLabel.Links.Add(supportSiteLinkLabel.Text.IndexOf("CTDragon.com"), "CTDragon.com".Length, "http://ctdragon.com/");
            this.ParentChanged += InstallationCompleteUserControl_ParentChanged;
        }

        private void InstallationCompleteUserControl_ParentChanged(object sender, EventArgs e)
        {
            CheckForErrors();
        }

        private void CheckForErrors()
        {
            if (GlobalApplicationOptions.ErrorsDuringInstallation)
            {
                descriptionTextBox.ScrollBars = ScrollBars.Both;
                descriptionTextBox.BackColor = Color.White;
                descriptionTextBox.ForeColor = Color.Red;
                descriptionTextBox.Text = "There were errors during installation. Please try again.";
                descriptionTextBox.Text += GlobalApplicationOptions.ErrorMessage;
            }
        }

        private void supportSiteLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }
    }
}
