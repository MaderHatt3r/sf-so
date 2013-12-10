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
        //public static List<Versions> VersionsToInstall = new List<Versions>();
        private static List<Versions> _versionsToInstall = new List<Versions>();
        public static List<Versions> VersionsToInstall
        {
            get { return _versionsToInstall; }
        }

        public static void AddVersionToInstall(Versions ver)
        {
            Versions? existingVer = _versionsToInstall.Find(n => n == ver);
            if (existingVer == null)
            {
                return;
            }
            else
            {
                _versionsToInstall.Add(ver);
            }
        }

        public static UserControlManager ucManager = new UserControlManager();
        public static bool ErrorsDuringInstallation = false;
        public static string ErrorMessage = "";
    }
}
