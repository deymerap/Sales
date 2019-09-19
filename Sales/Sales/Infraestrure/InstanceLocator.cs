
namespace Sales.Infraestrure
{
    using Sales.ViewModels;
    class InstanceLocator
    {
        public MainViewModel Main { get; set; }
        public InstanceLocator()
        {
            this.Main = new MainViewModel();
        }
    }
}
