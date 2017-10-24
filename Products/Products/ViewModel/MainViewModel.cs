using GalaSoft.MvvmLight.Command;
using Products.Response;
using Products.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Products.ViewModel
{
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
        
    }
}
