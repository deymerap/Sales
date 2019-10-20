namespace Sales.Helpers
{
    using Sales.Interfaces;
    using Sales.Resources;
    using Xamarin.Forms;
    public static class Languages
    {
        static Languages()
        {
            var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            Resource.Culture = ci;
            DependencyService.Get<ILocalize>().SetLocale(ci);
        }

        public static string Error
        {
            get { return Resource.Error; }
        }

        public static string ProductsTitle
        {
            get { return Resource.ProductsTitle; }
        }

        public static string TurnOnInternet
        {
            get { return Resource.TurnOnInternet; }
        }

        public static string NoInternet
        {
            get { return Resource.NoInternet; }
        }

        public static string Accept
        {
            get { return Resource.Accept; }
        }

        public static string AddProductsTitle
        {
            get { return Resource.AddProductsTitle; }
        }

        public static string Add
        {
            get { return Resource.Add; }
        }

        public static string Save
        {
            get { return Resource.Save; }
        }

        public static string Edit
        {
            get { return Resource.Edit; }
        }

        public static string ChangeImage
        {
            get { return Resource.ChangeImage; }
        }

        public static string AddProdDescError
        {
            get { return Resource.AddProdDescError; }
        }

        public static string AddProdPriceError
        {
            get { return Resource.AddProdPriceError; }
        }

        public static string ImageSource
        {
            get { return Resource.ImageSource; }
        }

        public static string Cancel
        {
            get { return Resource.Cancel; }
        }

        public static string FromGallery
        {
            get { return Resource.FromGallery; }
        }

        public static string NewPicture
        {
            get { return Resource.NewPicture; }
        }

        public static string Yes
        {
            get { return Resource.Yes; }
        }

        public static string No
        {
            get { return Resource.No; }
        }

        public static string EditProduct
        {
            get { return Resource.EditProduct; }
        }

        public static string Confirm
        {
            get { return Resource.Confirm; }
        }

        public static string DeleteConfirmation
        {
            get { return Resource.DeleteConfirmation; }
        }

        public static string Delete
        {
            get { return Resource.Delete; }
        }

        public static string EditProductsTitle
        {
            get { return Resource.EditProductsTitle; }
        }

        public static string AddProdIsAvailable
        {
            get { return Resource.AddProdIsAvailable; }
        }

        public static string Search
        {
            get { return Resource.Search; }
        }

        public static string Filter
        {
            get { return Resource.Filter; }
        }

        public static string Login
        {
            get { return Resource.Login; }
        }

        public static string EMail
        {
            get { return Resource.EMail; }
        }

        public static string EmailPlaceHolder
        {
            get { return Resource.EmailPlaceHolder; }
        }

        public static string PasswordUser
        {
            get { return Resource.PasswordUser; }
        }

        public static string PasswordPlaceHolder
        {
            get { return Resource.PasswordPlaceHolder; }
        }

        public static string Rememberme
        {
            get { return Resource.Rememberme; }
        }
        
        public static string Forgot
        {
            get { return Resource.Forgot; }
        }
        
        public static string Register
        {
            get { return Resource.Register; }
        }



    }
}
