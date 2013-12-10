using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Setup.UserControls;

namespace Setup
{
    public partial class SetupForm : Form
    {
        #region Data

        private UserControlManager ucManager;
        private UserControl currentControl;

        #endregion // Data

        #region Construction

        public SetupForm()
        {
            InitializeComponent();
            ucManager = GlobalApplicationOptions.ucManager;
            contentPanel.Controls.Add(ucManager.GetNextScreen());
            nextButton.Select();
            ucManager.RaiseNextScreen += ucManager_RaiseNextScreen;
        }

        void ucManager_RaiseNextScreen(object sender, EventArgs e)
        {
            NextScreen();
        }

        #endregion // Construction

        #region Event Handlers

        private void nextButton_Click(object sender, EventArgs e)
        {
            NextScreen();
        }

        private void finishButton_Click(object sender, EventArgs e)
        {
            this.Finish();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void installButton_Click(object sender, EventArgs e)
        {
            cancelButton.Visible = false;
            nextButton.Enabled = false;
            ((InstallationUserControl)currentControl).Install();
        }

        private void SetupForm_InstallationComplete(object sender, EventArgs e)
        {
            NextScreen();
        }

        #endregion // Event Handlers

        #region Private Helpers

        private void NextScreen()
        {
            try
            {
                contentPanel.Controls.Clear();
                currentControl = ucManager.GetNextScreen();
                contentPanel.Controls.Add(currentControl);
                if (ucManager.LastScreen)
                {
                    nextButton.Enabled = true;
                    nextButton.Text = "Finish";
                    nextButton.Click -= installButton_Click;
                    nextButton.Click += finishButton_Click;
                }
                if (currentControl is InstallationUserControl)
                {
                    nextButton.Text = "Install";
                    nextButton.Click -= nextButton_Click;
                    nextButton.Click += installButton_Click;
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                this.Close();
            }
        }

        private void Finish()
        {
            this.Close();
        }

        #endregion // Private Helpers


    }
}
