namespace Products.Models
{
    using GalaSoft.MvvmLight.Command;
    using Service;
    using System.Collections.Generic;
    using System.Windows.Input;
    using View;
    using ViewModel;
    using Xamarin.Forms;
    using System;

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
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Product = new ProductViewModel(Products);
            await navigationService.Navigate("ProductView");
        }

        public ICommand EditCommad { get { return new RelayCommand(Edit); } }

        private async void Edit()
        {
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.EditCategory = new EditCategoryViewModel(this);
            await navigationService.Navigate("EditCategoryView");
        }
        #endregion
    }
}
