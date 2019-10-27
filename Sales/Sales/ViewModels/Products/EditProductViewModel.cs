using GalaSoft.MvvmLight.Command;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Sales.Common.Models;
using Sales.Helpers;
using Sales.Services;
using Sales.ViewModels.Products;
using System;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sales.ViewModels
{
    public class EditProductViewModel : BaseViewModel
    {
        #region Attributes
        private ApiService apiService;
        private ProductItemViewModel productItemViewModel;
        private bool isEnabledCmdSave;
        private bool isRunningActIndicator;
        private MediaFile vImageFile { get; set; }
        private ImageSource imageSource;

        #endregion

        #region Propperties
        public ProductItemViewModel ProductItemViewModel
        {
            get { return this.productItemViewModel; }
            set { this.SetValue(ref this.productItemViewModel, value); }
        }
        public bool IsEnabledCmdSave
        {
            get { return this.isEnabledCmdSave; }
            set { this.SetValue(ref this.isEnabledCmdSave, value); }
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
        #endregion

        #region Contrunctorts
        public EditProductViewModel(ProductItemViewModel productItemViewModel)
        {
            this.productItemViewModel = productItemViewModel;
            this.IsEnabledCmdSave = true;
            this.ImageSource = productItemViewModel.ImageFullPath;
            apiService = new ApiService();
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

        private async void EditProducts()
        {
            if (string.IsNullOrEmpty(this.productItemViewModel.Description))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.AddProdDescError,
                    Languages.Accept);
                return;
            }

            if (this.productItemViewModel.Price < 0)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.AddProdPriceError,
                    Languages.Accept);
                return;
            }

            this.IsRunningActIndicator = true;
            this.IsEnabledCmdSave = false;

            var vObjConnection = this.apiService.CheckConnection();
            if (!vObjConnection.IsSuccess)
            {
                this.IsRunningActIndicator = false;
                this.IsEnabledCmdSave = true;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, vObjConnection.Message, Languages.Accept);
                return;
            }

            string vStrUrlAPI = Application.Current.Resources["UrlAPI"].ToString();
            string vStrUrlAPIPrefix = Application.Current.Resources["APIPrefix "].ToString();
            string vStrUrlProductsController = Application.Current.Resources["ProductsController"].ToString();

            byte[] vImageArray = null;
            if (vImageFile != null)
            {
                vImageArray = FileHelper.ReadFully(this.vImageFile.GetStream());
                this.productItemViewModel.ImageArray = vImageArray;
            }

            //Product vObProduct = new Product
            //{
            //    Description = this.productItemViewModel.Description,
            //    Price = vPrice,
            //    Notes = this.productItemViewModel.Notes,
            //    ImageArray = vImageArray,
            //};

            var vResponse = await apiService.Put(vStrUrlAPI, vStrUrlAPIPrefix, vStrUrlProductsController, this.productItemViewModel, this.productItemViewModel.ProductID, Preferences.TokenType, Preferences.AccessToke);

            if (!vResponse.IsSuccess)
            {
                this.IsRunningActIndicator = false;
                this.IsEnabledCmdSave = true;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, vResponse.Message, Languages.Accept);
                return;
            }

            Product vNewProducts = (Product)vResponse.Result;
            ProductsViewModel vProductsViewModel = ProductsViewModel.GetInstance();
            var vOldProduct = vProductsViewModel.vObjList.Where(Prod => Prod.ProductID == this.productItemViewModel.ProductID).FirstOrDefault();
            if(vOldProduct != null)
            {
                vProductsViewModel.vObjList.Remove(vOldProduct);
            }
            vProductsViewModel.vObjList.Add(vNewProducts);
            vProductsViewModel.RefreshListProducts();
            this.IsRunningActIndicator = false;
            this.IsEnabledCmdSave = true;
            await App.Navigator.PopAsync();
        }

        private async void DeleteProducts()
        {

            var vAnswer = await Application.Current.MainPage.DisplayAlert(
                Languages.Confirm,
                Languages.DeleteConfirmation,
                Languages.Yes,
                Languages.No
                );

            if (vAnswer == false)
            {
                return;
            }
            this.IsRunningActIndicator = true;
            this.IsEnabledCmdSave = false;

            var vObjConnection = this.apiService.CheckConnection();
            if (!vObjConnection.IsSuccess)
            {
                this.IsRunningActIndicator = false;
                this.IsEnabledCmdSave = true;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, vObjConnection.Message, Languages.Accept);
                return;
            }

            string vStrUrlAPI = Application.Current.Resources["UrlAPI"].ToString();
            string vStrUrlAPIPrefix = Application.Current.Resources["APIPrefix "].ToString();
            string vStrUrlProductsController = Application.Current.Resources["ProductsController"].ToString();
            var response = await this.apiService.Delete(vStrUrlAPI, vStrUrlAPIPrefix, vStrUrlProductsController, this.ProductItemViewModel.ProductID, Preferences.TokenType, Preferences.AccessToke);
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error, Languages.Confirm, Languages.Accept);
                return;
            }
            ProductsViewModel vProductsViewModel = ProductsViewModel.GetInstance();
            var vDeleteProd = vProductsViewModel.vObjList.Where(Prod => Prod.ProductID == this.ProductItemViewModel.ProductID).FirstOrDefault();
            if (vDeleteProd != null)
                vProductsViewModel.vObjList.Remove(vDeleteProd);

            vProductsViewModel.RefreshListProducts();
            await App.Navigator.PopAsync();
        }

        #endregion

        #region Singleton
        #endregion

        #region Command
        public ICommand cmdSave
        {
            get
            {
                return new RelayCommand(EditProducts);
            }
        }

        public ICommand cmdDelete
        {
            get
            {
                return new RelayCommand(DeleteProducts);
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