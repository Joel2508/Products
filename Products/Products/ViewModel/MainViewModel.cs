using Products.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.ViewModel
{
    public class MainViewModel
    {

        #region Properties
        
        public CategoryViewModel Category { get; set; }
        public LoginViewModel Login { get; set; }
        public TokenResponse Token { get; set; }
        #endregion

        public MainViewModel()
        {
            instance = this;
            Login = new LoginViewModel();            
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
    }
}
