namespace Sales.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Sales.Common.Models;
    using Sales.Helpers;
    using Sales.Services;
    using Sales.Views;
    using Sales.Views.Products;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class LoginViewModel : BaseViewModel
    {
        private ApiService apiService;

        #region Attributes
        private bool isEnabledcmdLogin;
        private bool isRunningActIndicator;
        private bool isRemembered;

        #endregion

        #region Propperties
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsEnabledcmdLogin
        {
            get { return this.isEnabledcmdLogin; }
            set { this.SetValue(ref this.isEnabledcmdLogin, value); }
        }

        public bool IsRunningActIndicator
        {
            get { return this.isRunningActIndicator; }
            set { this.SetValue(ref this.isRunningActIndicator, value); }
        }

        public bool IsRemembered
        {
            get { return this.isRemembered; }
            set { this.SetValue(ref this.isRemembered, value); }
        }
        #endregion

        #region Contrunctorts
        public LoginViewModel()
        {
            if(Preferences.IsRemembered)
            {
                this.Email = Preferences.Email;
                this.Password = Preferences.PwdEMail;
                this.IsRemembered = Preferences.IsRemembered;
            }
            this.IsEnabledcmdLogin = true;
            this.IsRemembered = true;
            apiService = new ApiService();
        }
        #endregion

        #region FuncAndMeth
        private async void Login()
        {
            TokenResponse vObjTResponse;
            if (string.IsNullOrEmpty(this.Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.EmailPlaceHolder,
                    Languages.Accept);
                return;
            }

            if (string.IsNullOrEmpty(this.Password))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.PasswordPlaceHolder,
                    Languages.Accept);
                return;
            }
            this.IsRunningActIndicator = true;
            this.IsEnabledcmdLogin = false;

            var vObjConnection = this.apiService.CheckConnection();
            if (!vObjConnection.IsSuccess)
            {
                this.IsRunningActIndicator = false;
                this.IsEnabledcmdLogin = true;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, vObjConnection.Message, Languages.Accept);
                return;
            }

            string vStrUrlAPI = Application.Current.Resources["UrlAPI"].ToString();
            vObjTResponse = await apiService.GetToken(vStrUrlAPI, this.Email, this.Password);

            if(vObjTResponse == null || string.IsNullOrEmpty( vObjTResponse.AccessToken))
            {
                this.IsRunningActIndicator = false;
                this.IsEnabledcmdLogin = true;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, Languages.AuthError, Languages.Accept);
                return;
            }

            Preferences.TokenType = vObjTResponse.TokenType;
            Preferences.AccessToke = vObjTResponse.AccessToken;
            Preferences.IsRemembered = this.IsRemembered;
            Preferences.Email = this.Email;
            Preferences.PwdEMail = this.Password;

            MainViewModel.GetInstance().Products= new ProductsViewModel();
            Application.Current.MainPage = new MasterPage();
            this.IsRunningActIndicator = true;
            this.IsEnabledcmdLogin = false;
        }
        #endregion


        #region Singleton

        #endregion


        #region Command
        public ICommand cmdLogin
        {
            get
            {
                return new RelayCommand(Login);
            }
        }

        #endregion
    }
}
