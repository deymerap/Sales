using GalaSoft.MvvmLight.Command;
using Plugin.Media.Abstractions;
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

        private void ChangeImage()
        {
            throw new NotImplementedException();
        }

        private async void EditProducts()
        {
            throw new NotImplementedException();
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

            var vObjConnection = await this.apiService.CheckConnection();
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
            var response = await this.apiService.Delete(vStrUrlAPI, vStrUrlAPIPrefix, vStrUrlProductsController, this.ProductItemViewModel.ProductID, Settings.TokenType, Settings.AccessToke);
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