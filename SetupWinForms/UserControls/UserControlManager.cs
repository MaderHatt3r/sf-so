using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Setup.UserControls
{
    public class UserControlManager
    {
        #region Data

        public const string INSTALLATION_TYPE = "InstallationType";
        public const string INTRO_USER_CONTROL = "IntroUserControl";
        public const string SELECT_COMPONENTS_USER_CONTROL = "SelectComponentsUserControl";

        private List<UserControl> screens = new List<UserControl>();
        int currentScreen = -1;

        private Dictionary<string, UserControl> _userScreens = new Dictionary<string,UserControl>();

        public Dictionary<string, UserControl> UserScreens
        {
            get { return _userScreens; }
            set { _userScreens = value; }
        }

        private Dictionary<string, bool> _userScreenEnabled = new Dictionary<string, bool>();

        public Dictionary<string, bool> UserScreenEnabled
        {
            get { return _userScreenEnabled; }
            set { _userScreenEnabled = value; }
        }

        private bool _lastScreen;

        public bool LastScreen
        {
            get { return _lastScreen; }
            set { _lastScreen = value; }
        }

        #endregion // Data

        #region Construction

        public UserControlManager()
        {
            CreateScreens();
            OrderScreens();
        }

        private void CreateScreens()
        {
            UserScreens[INTRO_USER_CONTROL] = new IntroUserControl();
            UserScreens[SELECT_COMPONENTS_USER_CONTROL] = new SelectComponentsUserControl();
            UserScreens[INSTALLATION_TYPE] = new InstallationType();
            
            UserScreenEnabled[INTRO_USER_CONTROL] = true;
            UserScreenEnabled[SELECT_COMPONENTS_USER_CONTROL] = true;
            UserScreenEnabled[INSTALLATION_TYPE] = true;
        }

        private void OrderScreens()
        {
            screens.Add(UserScreens[INTRO_USER_CONTROL]);
            screens.Add(UserScreens[INSTALLATION_TYPE]);
            screens.Add(UserScreens[SELECT_COMPONENTS_USER_CONTROL]);
        }

        #endregion // Construction

        //public void RemoveScreen(string _userControl)
        //{
        //    screens.Remove(UserScreens[_userControl]);
        //}

        public UserControl GetNextScreen()
        {
            currentScreen++;

            if (currentScreen == screens.Count - 1)
            {
                LastScreen = true;
            }

            dynamic screen = screens[currentScreen];
            if (!UserScreenEnabled[screen.ID])
            {
                screen = GetNextScreen();
            }

            return screen;
        }

        
    }
}
