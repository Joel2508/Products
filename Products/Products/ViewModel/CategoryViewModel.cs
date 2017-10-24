namespace Products.ViewModel
{
    using Models;
    using Service;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;

    public class CategoryViewModel : INotifyPropertyChanged
    {

        
        #region Attribute
        private ObservableCollection<Category> categories { get; set; }
        private List<Category> categorys;
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

        #endregion

        #region Constructor
        public CategoryViewModel()
        {
            instanceCategory = this;
            apiService = new ApiService();
            dialogService = new DialogService();            
            LoadCategories();
        }        
        #endregion

        #region Methods

        public void AddCategory(Category category)
        {
            categorys.Add(category);
            Categories = new ObservableCollection<Category>
                (categorys.OrderBy(c => c.Description));
        }
             
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

            categorys = (List<Category>)response.Result;

            Categories = new ObservableCollection<Category>(categorys.OrderBy(c=>c.Description));
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

    }
}
