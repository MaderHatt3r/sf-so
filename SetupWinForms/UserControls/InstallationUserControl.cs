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
    public partial class InstallationUserControl : UserControl
    {
        public string ID = UserControlManager.INSTALLATION_USER_CONTROL;

        public InstallationUserControl()
        {
            InitializeComponent();
        }
    }
}
