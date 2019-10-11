namespace Sales.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using Common.Models;
    using GalaSoft.MvvmLight.Command;
    using Sales.Helpers;
    using Services;
    using Xamarin.Forms;

    public class ProductsViewModel : BaseViewModel
    {
        #region Attributes
        private ApiService apiService;
        private ObservableCollection<Product> listaProducts;
        private bool isRefreshing;
        #endregion

        #region Propperties
        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }
        public ObservableCollection<Product> ListaProducts
        {
            get { return this.listaProducts; }
            set { this.SetValue(ref this.listaProducts, value); }
        }
        #endregion

        #region Contrunctorts
        public ProductsViewModel()
        {
            instance = this;
            this.apiService = new ApiService();
            this.LoadProducts();
        }
        
        private async void LoadProducts()
        {
            var vObjConnection = await this.apiService.CheckConnection();
            if (!vObjConnection.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, vObjConnection.Message, Languages.Accept);
                return;
            }

            string vStrUrlAPI = Application.Current.Resources["UrlAPI"].ToString();
            string vStrUrlAPIPrefix = Application.Current.Resources["APIPrefix "].ToString();
            string vStrUrlProductsController = Application.Current.Resources["ProductsController"].ToString();
            this.IsRefreshing = true;
            var response = await this.apiService.GetList<Product>(vStrUrlAPI, vStrUrlAPIPrefix, vStrUrlProductsController);
            if (!response.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }
            var vLista = (List<Product>)response.Result;
            this.ListaProducts = new ObservableCollection<Product>(vLista);
            this.IsRefreshing = false;
            
        }
        #endregion

        #region Singleton
        private static ProductsViewModel instance;
        public static ProductsViewModel GetInstance()
        {
            if (instance == null)
            {
                return new ProductsViewModel();
            }
            return instance;
        }
        #endregion

        #region Command
        public ICommand RefreshCommand
        {
            get
            {
                return new  RelayCommand(LoadProducts);
            }
        }
        #endregion
    }
}
