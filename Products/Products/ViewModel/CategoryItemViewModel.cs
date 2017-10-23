namespace Products.ViewModel
{
    using GalaSoft.MvvmLight.Command;
    using Models;
    using System;
    using System.Windows.Input;

    public class CategoryItemViewModel : Category
    {


        #region Command
        public ICommand SelectCategoryCommand { get { return new RelayCommand(SelectCategory); } }

        private void SelectCategory()
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
