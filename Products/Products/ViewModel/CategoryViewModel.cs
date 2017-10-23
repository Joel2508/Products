using GalaSoft.MvvmLight.Command;
using Products.Models;
using Products.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Products.ViewModel
{
    public class CategoryViewModel : INotifyPropertyChanged
    {

        
        #region Attribute
        private ObservableCollection<CategoryItemViewModel> categories { get; set; }
        #endregion

        #region Services
        private ApiService apiService;
        private DialogService dialogService;
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Properties 
        public ObservableCollection<CategoryItemViewModel> Categories
        {
            get
            {
                return categories;
            }
            set
            {
                if (categories != value)
                {
                    categories = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Categories)));
                }
            }
        }

        #endregion

        #region Constructor
        public CategoryViewModel()
        {
            apiService = new ApiService();
            dialogService = new DialogService();            
            LoadCategories();
        }

        
        #endregion

        #region Methods
        private async void LoadCategories()
        {
            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                await dialogService.ShowMessage("Error", connection.Message);
                return;
            }
            var mainViewModel = MainViewModel.GetInstance();            
            var response = await apiService.GetList<Category>("http://products.somee.com", "/api", "/Categories",
                mainViewModel.Token.TokenType,
                mainViewModel.Token.AccessToken);
            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage("Error", response.Message);
                return;
            }

            var category = (List<CategoryItemViewModel>)response.Result;

            Categories = new ObservableCollection<CategoryItemViewModel>(category.OrderBy(c=>c.Description));
        }
        #endregion

    }
}
