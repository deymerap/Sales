namespace Sales.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
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
        private DataService dataService;
        private ObservableCollection<Products.ProductItemViewModel> listProducts;
        private bool isRefreshing;
        public String filterText;
        #endregion

        #region Properties
        public List<Product> vObjList { get; set; }

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }

        public ObservableCollection<Products.ProductItemViewModel> ListProducts
        {
            get { return this.listProducts; }
            set { this.SetValue(ref this.listProducts, value); }
        }

        public String FilterText
        {
            get { return this.filterText; }
            set { this.SetValue(ref this.filterText, value);
                RefreshListProducts();
            }
        }

        #endregion

        #region Contrunctorts
        public ProductsViewModel()
        {
            instance = this;
            this.apiService = new ApiService();
            this.dataService = new DataService();
            this.LoadProducts();
        }
        
        private async void LoadProducts()
        {
            var vObjConnection = this.apiService.CheckConnection();
            if (vObjConnection.IsSuccess)
            {
                bool vBlnIsLoadDataApi = await this.LoadDataFromApi();
                if(vBlnIsLoadDataApi)
                {
                    _ = this.SaveProductsToDB();
                }
            }
            else
            {
                 await this.LoadDataFromDB();
            }

            if(this.vObjList == null || vObjList.Count == 0)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, Languages.NoProductsMessage, Languages.Accept);
                return;
            }           
            this.RefreshListProducts();
            this.IsRefreshing = false;
        }

        private async Task LoadDataFromDB()
        {
            this.vObjList = await dataService.GetAllProducts();
        }

        private async Task SaveProductsToDB()
        {
            await this.dataService.DeleteAllProducts();
            _ = this.dataService.Insert(this.vObjList);
        }

        private async Task<bool> LoadDataFromApi()
        {
            string vStrUrlAPI = Application.Current.Resources["UrlAPI"].ToString();
            string vStrUrlAPIPrefix = Application.Current.Resources["APIPrefix "].ToString();
            string vStrUrlProductsController = Application.Current.Resources["ProductsController"].ToString();
            this.IsRefreshing = true;
            var response = await this.apiService.GetList<Product>(vStrUrlAPI,
                                                                vStrUrlAPIPrefix,
                                                                vStrUrlProductsController,
                                                                Preferences.TokenType,
                                                                Preferences.AccessToke);
            if (!response.IsSuccess)
            {
                //this.IsRefreshing = false;
                //await Application.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return false;
            }
            vObjList = (List<Product>)response.Result;
            return true;
        }

        public void RefreshListProducts()
        {
            if (string.IsNullOrEmpty(this.FilterText))
            {
                var vListProdItem = this.vObjList.Select(Prod => new Products.ProductItemViewModel
                {
                    ProductID = Prod.ProductID,
                    Description = Prod.Description,
                    Notes = Prod.Notes,
                    Price = Prod.Price,
                    IsAvailable = Prod.IsAvailable,
                    PublishOn = Prod.PublishOn,
                    ImageArray = Prod.ImageArray,
                    ImagePath = Prod.ImagePath,
                });
                this.ListProducts = new ObservableCollection<Products.ProductItemViewModel>(vListProdItem.OrderBy(Prod => Prod.Description));
            }
            else
            {
                var vListProdItem = this.vObjList.Select(Prod => new Products.ProductItemViewModel
                {
                    ProductID = Prod.ProductID,
                    Description = Prod.Description,
                    Notes = Prod.Notes,
                    Price = Prod.Price,
                    IsAvailable = Prod.IsAvailable,
                    PublishOn = Prod.PublishOn,
                    ImageArray = Prod.ImageArray,
                    ImagePath = Prod.ImagePath,
                }).Where(Prod => Prod.Description.ToLower().Contains(this.FilterText.ToLower())).ToList();
                this.ListProducts = new ObservableCollection<Products.ProductItemViewModel>(vListProdItem.OrderBy(Prod => Prod.Description));
            }
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

        public ICommand cmdSearch
        {
            get
            {
                return new RelayCommand(RefreshListProducts);
            }
        }
        #endregion
    }
}
