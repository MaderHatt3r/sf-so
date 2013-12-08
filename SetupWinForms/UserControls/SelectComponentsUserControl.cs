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
    public partial class SelectComponentsUserControl : UserControl
    {
        #region Data

        public string ID = UserControlManager.SELECT_COMPONENTS_USER_CONTROL;

        #endregion // Data

        #region Construction

        public SelectComponentsUserControl()
        {
            InitializeComponent();
            SetupListBox();
        }

        private void SetupListBox()
        {
            List<dynamic> versionsItemSource = new List<dynamic>();
            versionsItemSource.Add(new { Name = "Word", Version = Versions.WORD });
            versionsItemSource.Add(new { Name = "Excel", Version = Versions.EXCEL });

            ((ListBox)this.versionCheckedListBox).DataSource = versionsItemSource;
            ((ListBox)this.versionCheckedListBox).DisplayMember = "Name";
            ((ListBox)this.versionCheckedListBox).ValueMember = "Version";
        }

        #endregion // Construction

        #region Event Handlers

        private void versionCheckedListBox_SelectedItemChanged(object sender, EventArgs e)
        {
            CheckedListBox versionsCLB = sender as CheckedListBox;
            if (versionsCLB == null)
            {
                return;
            }

            foreach (dynamic item in versionsCLB.CheckedItems)
            {
                GlobalApplicationOptions.VersionsToInstall.Add(item.Version);
            }

            foreach (dynamic item in versionsCLB.SelectedItems)
            {
                Versions ver = item.Version;
                switch (ver)
                {
                    case Versions.WORD:
                        selectionTextBox.Text = "Selection:" + Environment.NewLine + Environment.NewLine + "This option will install the SFSO Add-In for Microsoft Word";
                        break;
                    case Versions.EXCEL:
                        selectionTextBox.Text = "Selection:" + Environment.NewLine + Environment.NewLine + "This option will install the SFSO Add-In for Microsoft Excel";
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion // Event Handlers

    }
}
