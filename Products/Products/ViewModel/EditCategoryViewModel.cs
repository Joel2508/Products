namespace Products.ViewModel
{
    using System;
    using Models;
    using System.ComponentModel;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Service;
    using Response;

    public class EditCategoryViewModel : INotifyPropertyChanged
    {
        
        #region Attribute
        private Category category;
        private bool isRunning;
        private bool isEnabled;
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Services
        private ApiService apiService;
        private DialogService dialogService;
        private NavigationService navigationService;
        #endregion

        #region Properties
        public bool IsRunning
        {
            get
            {
                return isRunning;
            }
            set
            {
                if (isRunning != value)
                {
                    isRunning = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsRunning)));
                }
            }
        }
        public bool IsEnabled
        {
            get
            {
                return isEnabled;
            }
            set
            {
                if (isEnabled != value)
                {
                    isEnabled = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsEnabled)));
                }
            }
        }
        public string Description { get; set; }

        #endregion

        #region Constructor
        public EditCategoryViewModel(Category category)
        {
            this.category = category;
            apiService = new ApiService();
            dialogService = new DialogService();
            navigationService = new NavigationService();

            Description = category.Description;

            IsEnabled = true;

        }
        #endregion

        #region Methods
        public override int GetHashCode()
        {
            return category.CategoryId;
        }
        #endregion

        #region Command
        public ICommand SavelCommand { get {return new RelayCommand(Savel); } }

        private async void Savel()
        {
            if (string.IsNullOrEmpty(Description))
            {
                await dialogService.ShowMessage("Error", "Not clear in this field");
                return;
            }
            IsRunning = true;
            IsEnabled = false;
            var internet = await apiService.CheckConnection();
            if (!internet.IsSuccess)
            {
                await dialogService.ShowMessage("Error", internet.Message);
                return;
            }
            category.Description = Description;

            var mainViewModel = MainViewModel.GetInstance();            
            var response = await apiService.Put("http://products.somee.com", 
                "/api", "/Categories",
                mainViewModel.Token.TokenType,
                mainViewModel.Token.AccessToken, category);

            if (!response.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await dialogService.ShowMessage("Error", response.Message);
                return;
            }

            var categoryViewModel = CategoryViewModel.GetInstanceCategory();
            categoryViewModel.UpdataCategory(category);

            await navigationService.Back();

            IsRunning = false;
            IsEnabled = true;
        }
        #endregion
    }
}
