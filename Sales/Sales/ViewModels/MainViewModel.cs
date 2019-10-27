namespace Sales.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Sales.Helpers;
    using Sales.Views.Products;
    public class MainViewModel
    {
        public LoginViewModel Login { get; set; }
        public ProductsViewModel Products { get; set; }
        public AddProductViewModel AddProducts { get; set; }
        public EditProductViewModel EditProducts { get; set; }
        public ObservableCollection<MenuItemViewModel> Menu { get; set; }


        public MainViewModel()
        {
            instance = this;
            this.LoadMenuItem();
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

        #region Command
        public ICommand cmdAddProducts
        {
            get
            {
                return new RelayCommand(CommandAddProducts);
            }
        }
        #endregion

        #region FuncAndMethods
        private void LoadMenuItem()
        {
            this.Menu = new ObservableCollection<MenuItemViewModel>();
            this.Menu.Add(new MenuItemViewModel
            {
                Icon = "ic_info",
                PageName = "AboutPage",
                Title = Languages.About,
            });

            this.Menu.Add(new MenuItemViewModel
            {
                Icon = "ic_phonelink_setup",
                PageName = "SetupPage",
                Title = Languages.Setup,
            });

            this.Menu.Add(new MenuItemViewModel
            {
                Icon = "ic_exit_to_app",
                PageName = "LoginPage",
                Title = Languages.Exit,
            });
        }

        private async void CommandAddProducts()
        {
            this.AddProducts = new AddProductViewModel();
            await App.Navigator.PushAsync(new AddProductPage());
        }
        #endregion
    }
}
