
namespace Products.Service
{
    using View;
    using System.Threading.Tasks;
    using Xamarin.Forms;
    using System;

    public class NavigationService
    {
        public async Task Navigate(string pageName)
        {
            switch (pageName)
            {
                case "CategoryView":
                    await Application.Current.MainPage.Navigation.PushAsync(new CategoryView());
                    break;
                case "ProductView":
                    await Application.Current.MainPage.Navigation.PushAsync(new ProductView());
                    break;
                case "CreateCategoryView":
                    await Application.Current.MainPage.Navigation.PushAsync(new CreateCategoryView());
                    break;
                case "NewProductView":
                    await Application.Current.MainPage.Navigation.PushAsync(new NewProductView());
                    break;
                case "EditCategoryView":
                    await Application.Current.MainPage.Navigation.PushAsync(new EditCategoryView());
                    break;
            }
        }

        public async Task Back()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}
