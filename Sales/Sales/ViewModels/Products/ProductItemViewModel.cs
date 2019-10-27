

namespace Sales.ViewModels.Products
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Sales.Common.Models;
    using Sales.Helpers;
    using Sales.Services;
    using Sales.Views.Products;
    using Xamarin.Forms;

    public class ProductItemViewModel : Product
    {
        #region Attributes
        private ApiService apiService;
        #endregion


        #region Propperties
        #endregion


        #region Contrunctorts and  Methods
        public ProductItemViewModel()
        {
            this.apiService = new ApiService();
        }

        private async void EditProducts()
        {
            //this.EditProducts = new EditProductViewModel();
            MainViewModel.GetInstance().EditProducts = new EditProductViewModel(this);
            await App.Navigator.PushAsync(new EditProductPage());
        }

        private async void Deleteproducts()
        {
            var vAnswer =await Application.Current.MainPage.DisplayAlert(
                Languages.Confirm,
                Languages.DeleteConfirmation,
                Languages.Yes,
                Languages.No
                );

            if(vAnswer == false)
            {
                return;
            }

            var vObjConnection = this.apiService.CheckConnection();
            if (!vObjConnection.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error, vObjConnection.Message, Languages.Accept);
                return;
            }

            string vStrUrlAPI = Application.Current.Resources["UrlAPI"].ToString();
            string vStrUrlAPIPrefix = Application.Current.Resources["APIPrefix "].ToString();
            string vStrUrlProductsController = Application.Current.Resources["ProductsController"].ToString();
            var response = await this.apiService.Delete(vStrUrlAPI, vStrUrlAPIPrefix, vStrUrlProductsController, ProductID, Preferences.TokenType, Preferences.AccessToke);
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error, Languages.Confirm, Languages.Accept);
                return;
            }
            ProductsViewModel vProductsViewModel = ProductsViewModel.GetInstance();
            var vDeleteProd = vProductsViewModel.vObjList.Where(Prod => Prod.ProductID == this.ProductID).FirstOrDefault();
            if (vDeleteProd != null)
                vProductsViewModel.vObjList.Remove(vDeleteProd);

            vProductsViewModel.RefreshListProducts();
        }
        #endregion


        #region Command
        public ICommand cmdDeleteProducts
        {
            get
            {
                return new RelayCommand(Deleteproducts);
            }
        }

        public ICommand cmdEditProducts
        {
            get
            {
                return new RelayCommand(EditProducts);
            }
        }
        #endregion
    }
}
