namespace Sales.ViewModels.Login
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Input;
    using Xamarin.Forms;
    using GalaSoft.MvvmLight.Command;
    using Plugin.Media;
    using Plugin.Media.Abstractions;
    using Sales.Services;
    using Sales.Helpers;
    using Sales.Common.Models;

    public class RegisterViewModel : BaseViewModel
    {
        #region Attributes
        private ApiService apiService;
        private bool isEnabledCmd;
        private bool isRunningActIndicator;
        private MediaFile vImageFile { get; set; }
        private ImageSource imageSource;
        #endregion

        #region Propperties
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EMail { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }

        public bool IsEnabledCmd
        {
            get { return this.isEnabledCmd; }
            set { this.SetValue(ref this.isEnabledCmd, value); }
        }

        public bool IsRunningActIndicator
        {
            get { return this.isRunningActIndicator; }
            set { this.SetValue(ref this.isRunningActIndicator, value); }
        }

        public ImageSource ImageSource
        {
            get { return this.imageSource; }
            set { this.SetValue(ref this.imageSource, value); }
        }

        public object SaveProducts { get; private set; }
        #endregion

        #region Contrunctorts
        public RegisterViewModel()
        {
            this.IsEnabledCmd = true;
            this.ImageSource = "profile.png";
            apiService = new ApiService();
        }

        private async void AddProducts()
        {
            if (string.IsNullOrEmpty(this.FirstName))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.FirstNameError,
                    Languages.Accept);
                return;
            }

            if (string.IsNullOrEmpty(this.LastName))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.LastNameError,
                    Languages.Accept);
                return;
            }

            if (string.IsNullOrEmpty(this.EMail))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.EMailError,
                    Languages.Accept);
                return;
            }

            if (!RegexHelper.IsValidEmailAddr(this.EMail))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.EMailError,
                    Languages.Accept);
                return;
            }

            if (string.IsNullOrEmpty(this.Phone))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.PhoneError,
                    Languages.Accept);
                return;
            }

            if (string.IsNullOrEmpty(this.Password))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.PasswordError,
                    Languages.Accept);
                return;
            }

            if (this.Password.Length < 6)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.PasswordError,
                    Languages.Accept);
                return;
            }

            if (string.IsNullOrEmpty(this.PasswordConfirm))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.PasswordConfirmError,
                    Languages.Accept);
                return;
            }


            if (!this.Password.Equals(this.PasswordConfirm))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.PasswordsNoMatch,
                    Languages.Accept);
                return;
            }

            this.IsRunningActIndicator = true;
            this.IsEnabledCmd = false;

            var vObjConnection = this.apiService.CheckConnection();
            if (!vObjConnection.IsSuccess)
            {
                this.IsRunningActIndicator = false;
                this.IsEnabledCmd = true;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, vObjConnection.Message, Languages.Accept);
                return;
            }

            string vStrUrlAPI = Application.Current.Resources["UrlAPI"].ToString();
            string vStrUrlAPIPrefix = Application.Current.Resources["APIPrefix "].ToString();
            string vStrUrlController = Application.Current.Resources["UsersController"].ToString();

            byte[] vImageArray = null;
            if (vImageFile != null)
            {
                vImageArray = FileHelper.ReadFully(this.vImageFile.GetStream());
            }

            var vObjUserRequest = new UserRequest
            {
                Address = this.Address,
                EMail = this.EMail,
                FirstName = this.FirstName,
                ImageArray = vImageArray,
                LastName = this.LastName,
                Password = this.Password,
            };

            var vResponse = await apiService.Post(vStrUrlAPI, vStrUrlAPIPrefix, vStrUrlController, vObjUserRequest);

            if (!vResponse.IsSuccess)
            {
                this.IsRunningActIndicator = false;
                this.IsEnabledCmd = true;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, vResponse.Message, Languages.Accept);
                return;
            }

            this.IsRunningActIndicator = false;
            this.IsEnabledCmd = true;

            await Application.Current.MainPage.DisplayAlert(
                Languages.Confirm,
                Languages.RegisterConfirmation,
                Languages.Accept);

            await Application.Current.MainPage.Navigation.PopAsync();
        }


        private async void ChangeImage()
        {
            await CrossMedia.Current.Initialize();
            var vSource = await Application.Current.MainPage.DisplayActionSheet(
                Languages.ImageSource,
                Languages.Cancel,
                null,
                Languages.FromGallery,
                Languages.NewPicture
                );

            if (vSource == Languages.Cancel)
            {
                this.vImageFile = null;
                return;
            }

            if (vSource == Languages.NewPicture)
            {
                StoreCameraMediaOptions vObjCamara = new StoreCameraMediaOptions
                {
                    Directory = "Sample",
                    Name = "test.jpg",
                    PhotoSize = PhotoSize.Small
                };

                this.vImageFile = await CrossMedia.Current.TakePhotoAsync(vObjCamara);
            }
            else
            {
                this.vImageFile = await CrossMedia.Current.PickPhotoAsync();
            }

            if (this.vImageFile != null)
            {
                ImageSource = ImageSource.FromStream(() =>
                {
                    var vStream = this.vImageFile.GetStream();
                    return vStream;
                });
            }
        }
        #endregion

        #region Singleton
        #endregion

        #region Command
        public ICommand cmdSave
        {
            get
            {
                return new RelayCommand(AddProducts);
            }
        }

        public ICommand cmdCambiarImagen
        {
            get
            {
                return new RelayCommand(ChangeImage);
            }
        }

        #endregion

    }
}
