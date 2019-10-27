namespace Sales
{
    using Xamarin.Forms;
    using Sales.ViewModels;
    using Sales.Views.Login;
    using Sales.Helpers;
    using Sales.Views.Products;
    using Sales.Views;

    public partial class App : Application
    {
        public static NavigationPage Navigator { get; internal set; }

        public App()
        {
            InitializeComponent();
            if(Preferences.IsRemembered && !string.IsNullOrEmpty(Preferences.AccessToke))
            {
                MainViewModel.GetInstance().Products = new ProductsViewModel();
                MainPage = new MasterPage();
            }
            else
            {
                MainViewModel.GetInstance().Login = new LoginViewModel();
                Application.Current.MainPage =new NavigationPage(new LoginPage());
            }


//#if DEBUG
//            HotReloader.Current.Run(this);
//#endif 
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
