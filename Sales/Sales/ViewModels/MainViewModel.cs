namespace Sales.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class MainViewModel
    {
        public ProductsViewModel Products { get; set; }

        public MainViewModel()
        {
            this.Products = new ProductsViewModel();
        }

        public ICommand cmdAddProducts {
            get
            {
                return new RelayCommand(CommandAddProducts);
            }
        }

        private async void CommandAddProducts()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new AddProductPage());
        }
    }
}
