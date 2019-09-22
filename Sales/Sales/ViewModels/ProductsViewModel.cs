
using Sales.Common.Models;

namespace Sales.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using Common.Models;
    using GalaSoft.MvvmLight.Command;
    using Services;
    using Xamarin.Forms;

    public class ProductsViewModel : BaseViewModel
    {
        private ApiService ApiService;
        private ObservableCollection<Product> listaProducts;
        private bool isRefreshing;
        

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
        


        public ProductsViewModel()
        {
            this.ApiService = new ApiService();
            this.LoadProducts();
        }

        private async void LoadProducts()
        {
            this.IsRefreshing = true;
            var response = await this.ApiService.GetList<Product>("https://xamarinsalesapi.azurewebsites.net", "/api", "/Products");
            if (!response.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
            var vLista = (List<Product>)response.Result;
            this.ListaProducts = new ObservableCollection<Product>(vLista);
            this.IsRefreshing = false;
        }

        public ICommand RefreshCommand
        {
            get
            {
                return new  RelayCommand(LoadProducts);
            }
        }
    }
}
