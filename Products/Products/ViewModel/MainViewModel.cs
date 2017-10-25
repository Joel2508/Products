namespace Products.ViewModel
{
    using GalaSoft.MvvmLight.Command;
    using Response;
    using Service;
    using System.Windows.Input;

    public class MainViewModel
    {
        #region Service
        private NavigationService navigationService;
        #endregion

        #region Properties        
        public CategoryViewModel Category { get; set; }
        public LoginViewModel Login { get; set; }
        public TokenResponse Token { get; set; }
        public ProductViewModel Product { get; set; }
        public CreateCategoryViewModel CreateCategory { get; set; }
        public NewProductViewModel NewProduct { get; set; }
        #endregion

        public MainViewModel()
        {
            instance = this;
            Login = new LoginViewModel();
            navigationService = new NavigationService(); 
        }

        #region Singleton
        private static MainViewModel instance;
        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModel();
            }

            return instance;
        }
        #endregion

        #region Command
        public ICommand CreateCategoryCommand { get {return new RelayCommand(CrearCategory); } }

        private async void CrearCategory()
        {
            CreateCategory = new CreateCategoryViewModel();
            await navigationService.Navigate("CreateCategoryView");
        }
        #endregion

        public ICommand NewProductCommand { get { return new RelayCommand(AddNewProduct); } }

        private async void AddNewProduct()
        {
            NewProduct = new NewProductViewModel();
            await navigationService.Navigate("NewProductView");
        }
    }
}
