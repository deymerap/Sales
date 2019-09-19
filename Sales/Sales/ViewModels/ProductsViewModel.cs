
using Sales.Common.Models;

namespace Sales.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Common.Models;
    using Services;
    using Xamarin.Forms;

    public class ProductsViewModel : BaseViewModel
    {
        private ApiService ApiService;
        private ObservableCollection<Product> listaProducts;
        public ObservableCollection<Product> ListaProducts
        {
            get { return this.listaProducts; }
            set { this.SetValue(ref this.listaProducts, value); }
        }
        public List<Product> MyProducts { get; set; }


        public ProductsViewModel()
        {
            this.ApiService = new ApiService();
            this.LoadProducts();
        }

        private async void LoadProducts()
        {
            var response = await this.ApiService.GetList<Product>("https://xamarinsalesapi.azurewebsites.net", "/api", "/Products");
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                
            }
            this.ListaProducts = (List<Product>)response.Result;
        }
    }
}
