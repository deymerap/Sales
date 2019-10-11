

namespace Sales.ViewModels.Products
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Sales.Common.Models;
    class ProductItemViewModel : Product
    {
        #region Attributes
        #endregion


        #region Propperties
        #endregion


        #region Contrunctorts and  Methods
        private void EditProducts()
        {
            throw new NotImplementedException();
        }

        private void Deleteproducts()
        {
            throw new NotImplementedException();
        }
        #endregion


        #region Command
        public ICommand cmdDeleteProducts
        {
            get
            {
                return new RelayCommand(Deleteproducts);
            }
        }

        public ICommand cmdEditProducts
        {
            get
            {
                return new RelayCommand(EditProducts);
            }
        }
        #endregion
    }
}
