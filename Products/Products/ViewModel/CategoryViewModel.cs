namespace Products.ViewModel
{
    using GalaSoft.MvvmLight.Command;
    using Models;
    using Service;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Input;
    using System;

    public class CategoryViewModel : INotifyPropertyChanged
    {

        
        #region Attribute
        private ObservableCollection<Category> categories { get; set; }
        private List<Category> categorys;
        private bool isRefreshing;
        #endregion

        #region Services
        private ApiService apiService;
        private DialogService dialogService;
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Properties 
        public ObservableCollection<Category> Categories
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

        public bool IsRefreshing
        {
            get
            {
                return isRefreshing;
            }
            set
            {
                if (isRefreshing != value)
                {
                    isRefreshing = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsRefreshing)));
                }
            }
        }
        #endregion

        #region Constructor
        public CategoryViewModel()
        {
            IsRefreshing = true;
            instanceCategory = this;
            apiService = new ApiService();
            dialogService = new DialogService();            
            LoadCategories();
        }        
        #endregion

        #region Methods

        public void AddCategory(Category category)
        {
            IsRefreshing = true;
            categorys.Add(category);
            Categories = new ObservableCollection<Category>
                (categorys.OrderBy(c => c.Description));
            IsRefreshing = false;
        }

        public void UpdataCategory(Category category)
        {
            IsRefreshing = true;
            var oldCategory = categories.Where(c => c.CategoryId == category.CategoryId).FirstOrDefault();
            oldCategory = category;
            Categories = new ObservableCollection<Category>
                (categorys.OrderBy(c => c.Description));
            IsRefreshing = false;
        }

        private async void LoadCategories()
        {
            IsRefreshing = true;
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
            categorys = (List<Category>)response.Result;
            Categories = new ObservableCollection<Category>(categorys.OrderBy(c=>c.Description));
            IsRefreshing = false;
        }
        #endregion

        #region Singleton
        private static CategoryViewModel instanceCategory;
        public static CategoryViewModel GetInstanceCategory()
        {
            if(instanceCategory==null)
            {
                return new CategoryViewModel();
            }
            return instanceCategory;
        }
        #endregion

        #region Command
        public ICommand RefreshCommand { get { return new RelayCommand(LoadCategories); } }        
        #endregion
    }
}
