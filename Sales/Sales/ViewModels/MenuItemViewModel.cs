namespace Sales.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Sales.Helpers;
    using Sales.Views.Login;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class MenuItemViewModel
    {
        private void CloseSession()
        {
            Settings.AccessToke = string.Empty;
            Settings.TokenType = string.Empty;

            MainViewModel.GetInstance().Login = new LoginViewModel();
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }


        #region Command
        public ICommand cmdCloseSession
        {
            get
            {
                return new RelayCommand(CloseSession);
            }
        }
        #endregion
    }
}
