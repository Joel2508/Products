namespace Products.ViewModel
{
    using GalaSoft.MvvmLight.Command;
    using Products.Service;
    using System.ComponentModel;
    using System.Windows.Input;

    public class LoginViewModel : INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Service
        private DialogService dialogService; 
        private ApiService apiService;
        private NavigationService navigationService;
        #endregion

        #region Attribute
        string email;
        string password;
        private bool isToggled;
        private bool isRunning;
        private bool isEnabled;
        #endregion

        #region Properties
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                if (email != value)
                {
                    email = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Email)));
                }
            }
        }

        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                if (password != value)
                {
                    password = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Password)));
                }
            }
        }

        public bool IsToggled
        {
            get
            {
                return isToggled;
            }
            set
            {
                if (isToggled != value)
                {
                    isToggled = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsToggled)));
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
        #endregion

        #region Constructor
        public LoginViewModel()
        {
            IsEnabled = true;
            IsToggled = true;
            apiService = new ApiService();
            dialogService = new DialogService();
            navigationService = new NavigationService();
        }
        #endregion

        #region Command
        public ICommand LoginCommand { get { return new RelayCommand(Login); } }

        private async void Login()
        {
            if (string.IsNullOrEmpty(Email))
            {
                await dialogService.ShowMessage("Error", "You must enter an email");
                return;
            }

            if (string.IsNullOrEmpty(Password))
            {
                await dialogService.ShowMessage("Error", "You must enter an password");
                return;
            }

            IsEnabled = false;
            IsRunning = true;

            var connection = await apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true; 
                await dialogService.ShowMessage("Error", connection.Message);
                return;
            }
            var response = await apiService.GetToken("http://products.somee.com", 
                Email, Password);

            if (response == null)
            {
                IsEnabled = true;
                IsRunning = false;
                await dialogService.ShowMessage("Error", "Not service the internet");
                Email = null;
                Password = null;
                return;
            }
            if (string.IsNullOrEmpty(response.AccessToken))
            {
                IsEnabled = true;
                IsRunning = false;
                await dialogService.ShowMessage("Error", response.ErrorDescription);
                Password = null;
                return;
            }


            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Token = response;
            mainViewModel.Category = new CategoryViewModel();
            await navigationService.Navigate("CategoryView");
            Email = null;
            Password = null;

            IsEnabled = true;
            IsRunning = false;
        }
        #endregion
    }
}
