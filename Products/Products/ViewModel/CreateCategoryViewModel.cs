namespace Products.ViewModel
{
    using GalaSoft.MvvmLight.Command;
    using Models;
    using Response;
    using Service;
    using System.ComponentModel;
    using System.Windows.Input;

    public class CreateCategoryViewModel : INotifyPropertyChanged
    {

        #region Attributes        
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

        public string Description { get; set; }
        #endregion



        public CreateCategoryViewModel()
        {
            apiService = new ApiService();
            dialogService = new DialogService();
            navigationService = new NavigationService();

            IsEnabled = true;
        }

        #region Command
        public ICommand SavelCommand { get { return new RelayCommand(Savel); } }

        private async void Savel()
        {
            if (string.IsNullOrEmpty(Description))
            {
                await dialogService.ShowMessage("Error", "You must enter an description");
                return;
            }
            IsEnabled = false;
            IsRunning = true;

            var internet = await apiService.CheckConnection();
            if (!internet.IsSuccess)
            {
                IsEnabled = true;
                IsRunning = false;
                await dialogService.ShowMessage("Error",internet.Message);
                return;
            }

            var category = new Category
            {
                Description = Description,
            };
            var mainViewModel = MainViewModel.GetInstance();
            

            var response = await apiService.Post("http://products.somee.com", "/api", "Categories",
                mainViewModel.Token.TokenType,mainViewModel.Token.AccessToken,category);

            if (!response.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await dialogService.ShowMessage("Error", response.Message);
                return;
            }

            category = (Category)response.Result;
            var categoryViewModel = CategoryViewModel.GetInstanceCategory();
            categoryViewModel.AddCategory(category);
            await navigationService.Back();

            IsRunning = false;
            IsEnabled = true;

        }
        #endregion

    }
}
