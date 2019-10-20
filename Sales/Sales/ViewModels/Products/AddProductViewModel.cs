namespace Sales.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Plugin.Media;
    using Plugin.Media.Abstractions;
    using Sales.Common.Models;
    using Sales.Helpers;
    using Sales.Services;
    using System;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class AddProductViewModel : BaseViewModel
    {   
        private ApiService apiService;

        #region Attributes
        private bool isEnabledCmdSave;
        private bool isRunningActIndicator;
        private MediaFile vImageFile { get; set; }
        private ImageSource imageSource;
        #endregion

        #region Propperties
        public string Description { get; set; }
        public string Notes { get; set; }
        public string Price { get; set; }

        public bool IsEnabledCmdSave
        {
            get { return this.isEnabledCmdSave; }
            set { this.SetValue(ref this.isEnabledCmdSave, value); }
        }

        public bool IsRunningActIndicator
        {
            get { return this.isRunningActIndicator; }
            set { this.SetValue(ref this.isRunningActIndicator, value); }
        }

        public ImageSource ImageSource
        {
            get { return this.imageSource; }
            set { this.SetValue(ref this.imageSource, value); }
        }

        public object SaveProducts { get; private set; }
        #endregion

        #region Contrunctorts
        public AddProductViewModel()
        {
            this.IsEnabledCmdSave = true;
            this.ImageSource = "NoImage.png";
            apiService = new ApiService();
        }

        private async void AddProducts()
        {
            if(string.IsNullOrEmpty(this.Description))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.AddProdDescError,
                    Languages.Accept);
                return;
            }

            if (string.IsNullOrEmpty(this.Price) )
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error, 
                    Languages.AddProdPriceError, 
                    Languages.Accept);
                return;
            }

            var vPrice = decimal.Parse(this.Price);
            if (vPrice < 0)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.AddProdPriceError,
                    Languages.Accept);
                return;
            }

            this.IsRunningActIndicator = true;
            this.IsEnabledCmdSave = false;

            var vObjConnection = await this.apiService.CheckConnection();
            if (!vObjConnection.IsSuccess)
            {
                this.IsRunningActIndicator = false;
                this.IsEnabledCmdSave = true;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, vObjConnection.Message, Languages.Accept);
                return;
            }

            string vStrUrlAPI = Application.Current.Resources["UrlAPI"].ToString();
            string vStrUrlAPIPrefix = Application.Current.Resources["APIPrefix "].ToString();
            string vStrUrlProductsController = Application.Current.Resources["ProductsController"].ToString();

            byte[] vImageArray = null;
            if(vImageFile !=  null)
            {
                vImageArray = FileHelper.ReadFully(this.vImageFile.GetStream());
            }
           
            Product vObProduct = new Product {
                Description = this.Description,
                Price = vPrice,
                Notes = this.Notes,
                ImageArray = vImageArray,
            };

            var vResponse = await apiService.Post(vStrUrlAPI, vStrUrlAPIPrefix, vStrUrlProductsController, vObProduct);

            if (!vResponse.IsSuccess)
            {
                this.IsRunningActIndicator = false;
                this.IsEnabledCmdSave = true;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, vResponse.Message, Languages.Accept);
                return;
            }

            Product vNewProducts = (Product) vResponse.Result;
            //var vNewProductsItem = new Products.ProductItemViewModel
            //{
            //    ProductID = vNewProducts.ProductID,
            //    Description = vNewProducts.Description,
            //    Notes = vNewProducts.Notes,
            //    Price = vNewProducts.Price,
            //    IsAvailable = vNewProducts.IsAvailable,
            //    PublishOn = vNewProducts.PublishOn,
            //    ImageArray = vNewProducts.ImageArray,
            //    ImagePath = vNewProducts.ImagePath,
            //};
            ProductsViewModel vProductsViewModel = ProductsViewModel.GetInstance();
            //vProductsViewModel.ListProducts.Add(vNewProductsItem);
            vProductsViewModel.vObjList.Add(vNewProducts);
            vProductsViewModel.RefreshListProducts();
            this.IsRunningActIndicator = false;
            this.IsEnabledCmdSave = true;
            await Application.Current.MainPage.Navigation.PopAsync();
        }


        private async void ChangeImage()
        {
            await CrossMedia.Current.Initialize();
            var vSource = await Application.Current.MainPage.DisplayActionSheet(
                Languages.ImageSource,
                Languages.Cancel,
                null,
                Languages.FromGallery,
                Languages.NewPicture
                );

            if (vSource == Languages.Cancel)
            {
                this.vImageFile = null;
                return;
            }

            if (vSource == Languages.NewPicture)
            {
                StoreCameraMediaOptions vObjCamara = new StoreCameraMediaOptions
                {
                    Directory = "Sample",
                    Name = "test.jpg",
                    PhotoSize = PhotoSize.Small
                };

                this.vImageFile = await CrossMedia.Current.TakePhotoAsync(vObjCamara);
            }
            else
            {
                this.vImageFile = await CrossMedia.Current.PickPhotoAsync();
            }

            if (this.vImageFile != null)
            {
                ImageSource = ImageSource.FromStream(() =>
                {
                    var vStream = this.vImageFile.GetStream();
                    return vStream;
                });
            }
        }
        #endregion

        #region Singleton
        #endregion

        #region Command
        public ICommand cmdSave
        {
            get
            {
                return new RelayCommand(AddProducts);
            }
        }

        public ICommand cmdCambiarImagen
        {
            get
            {
                return new RelayCommand(ChangeImage);
            }
        }

        #endregion

    }
}
