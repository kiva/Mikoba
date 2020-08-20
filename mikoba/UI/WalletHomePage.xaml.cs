using mikoba.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace mikoba.UI
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WalletHomePage : ContentPage
    {
        public WalletHomePage()
        {
            InitializeComponent();
            WalletHomePageViewModel.Instance.NavigationService = this.Navigation;
            this.BindingContext = WalletHomePageViewModel.Instance;
        }
    }
}
