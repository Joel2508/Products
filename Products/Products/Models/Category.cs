namespace Products.Models
{
    using GalaSoft.MvvmLight.Command;
    using Service;
    using System.Collections.Generic;
    using System.Windows.Input;
    using View;
    using ViewModel;
    using Xamarin.Forms;

    public class Category
    {
        #region Attribute
        private NavigationService navigationService;
        #endregion

        #region Properties
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public List<Product> Products { get; set; }
        #endregion

        #region Contructor
        public Category()
        {
            navigationService = new NavigationService();
        }
        #endregion

        #region Command
        public ICommand SelectCategoryCommand { get { return new RelayCommand(SelectCategory); } }

        private async void SelectCategory()
        {
            await navigationService.Navigate("ProductView");
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Product = new ProductViewModel(Products);
        }
        #endregion
    }
}
