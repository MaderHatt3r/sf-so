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

        UserControlManager ucManager = GlobalApplicationOptions.ucManager;

        #endregion // Data

        #region Construction

        public SetupForm()
        {
            InitializeComponent();
            contentPanel.Controls.Add(ucManager.GetNextScreen());
            nextButton.Select();
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

        #endregion // Event Handlers

        #region Private Helpers

        private void NextScreen()
        {
            try
            {
                contentPanel.Controls.Clear();
                contentPanel.Controls.Add(ucManager.GetNextScreen());
                if (ucManager.LastScreen)
                {
                    nextButton.Text = "Finish";
                    nextButton.Click -= nextButton_Click;
                    nextButton.Click += finishButton_Click;
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
