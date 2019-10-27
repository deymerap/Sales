using GalaSoft.MvvmLight.Command;
using Sales.Helpers;
using Sales.Views.Login;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sales.ViewModels
{
    public class MenuItemViewModel
    {

        #region Properties
        public string Icon { get; set; }
        public string Title { get; set; }
        public string PageName { get; set; }
        #endregion

        #region FuncAndMethods
        private void Goto()
        {
            switch (this.PageName)
            {
                case "AboutPage":
 
                    break;

                case "SetupPage":

                    break;

                case "LoginPage":
                    Preferences.AccessToke = string.Empty;
                    Preferences.TokenType = string.Empty;
                    MainViewModel.GetInstance().Login = new LoginViewModel();
                    Application.Current.MainPage = new NavigationPage(new LoginPage());
                    break;
            }

        }

        #endregion

        #region Command
        public ICommand cmdGoto
        {
            get
            {
                return new RelayCommand(Goto);
            }
        }
        #endregion
    }
}
