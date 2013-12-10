using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Setup.UserControls;

namespace Setup
{
    public enum Versions
    {
        WORD, EXCEL
    }

    public static class GlobalApplicationOptions
    {
        public static List<Versions> VersionsToInstall = new List<Versions>();
        public static UserControlManager ucManager = new UserControlManager();
        public static bool ErrorsDuringInstallation = false;
        public static string ErrorMessage = "";
    }
}
