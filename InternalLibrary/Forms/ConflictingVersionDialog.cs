using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InternalLibrary.Data;

namespace InternalLibrary.Forms
{
    public partial class ConflictingVersionDialog : Form
    {
        private ConflictResolutionOptions _userSelection;

        public ConflictResolutionOptions UserSelection
        {
            get { return _userSelection; }
            set { _userSelection = value; }
        }

        
        public ConflictingVersionDialog()
        {
            InitializeComponent();
            UserSelection = ConflictResolutionOptions.CANCEL;
        }

        //public void Set(ref ConflictResolutionOptions selectedOption)
        //{
        //    this.Show();
        //    this.selectedOption = selectedOption;
        //}

        private void pullLatestVersionButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._userSelection = ConflictResolutionOptions.PULL;
            this.Close();
        }

        private void overwriteDriveVersionButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._userSelection = ConflictResolutionOptions.FORCE_PUSH;
            this.Close();
        }

        private void mergeChangesButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._userSelection = ConflictResolutionOptions.MERGE;
            this.Close();
        }

        private void createNewCopyButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._userSelection = ConflictResolutionOptions.CREATE_NEW;
            this.Close();
        }
    }
}
