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
    public partial class InstallationType : UserControl
    {
        public string ID = UserControlManager.INSTALLATION_TYPE;

        public InstallationType()
        {
            InitializeComponent();
            this.ParentChanged += InstallationType_ParentChanged;
           
        }

        private void InstallationType_ParentChanged(object sender, EventArgs e)
        {
            EvaluateCustomInstallOption();
        }

        private void customInstallationRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb == null)
            {
                return;
            }

            EvaluateCustomInstallOption();
        }

        private void EvaluateCustomInstallOption()
        {
            GlobalApplicationOptions.VersionsToInstall.Clear();
            bool customInstallation = customInstallationRadioButton.Checked;
            UserControl uc = GlobalApplicationOptions.ucManager.UserScreens[UserControlManager.SELECT_COMPONENTS_USER_CONTROL];
            GlobalApplicationOptions.ucManager.UserScreenEnabled[UserControlManager.SELECT_COMPONENTS_USER_CONTROL] = customInstallation;

            if (!customInstallation)
            {
                foreach (Versions ver in Enum.GetValues(typeof(Versions)))
                {
                    GlobalApplicationOptions.AddVersionToInstall(ver);
                }
            }
        }
    }
}
