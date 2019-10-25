namespace Sales.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Sales.Views.Products;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class MainViewModel
    {
        public LoginViewModel Login { get; set; }
        public ProductsViewModel Products { get; set; }
        public AddProductViewModel AddProducts { get; set; }
        public EditProductViewModel EditProducts { get; set; }
        public MenuItemViewModel MenuItem { get; set; }


        public MainViewModel()
        {
            instance = this;
            //this.Products = new ProductsViewModel();
        }

        #region Singleton
        private static MainViewModel instance;
        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModel();
            }
            return instance;
        }
        #endregion

        public ICommand cmdAddProducts
        {
            get
            {
                return new RelayCommand(CommandAddProducts);
            }
        }

        private async void CommandAddProducts()
        {
            this.AddProducts = new AddProductViewModel();
            await App.Navigator.PushAsync(new AddProductPage());
        }

    }
}
