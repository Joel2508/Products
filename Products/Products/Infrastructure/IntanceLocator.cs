namespace Products.Infrastructure
{
    using ViewModel;
    public class IntanceLocator
    {
        public MainViewModel Main { get; set; }

        public IntanceLocator()
        {
            Main = new MainViewModel();
        }
    }
}
