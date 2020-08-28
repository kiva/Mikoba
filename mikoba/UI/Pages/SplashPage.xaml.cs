using System.Linq;
using System.Threading.Tasks;
using mikoba.ViewModels;
using Xamarin.Forms;

namespace mikoba.UI.Pages
{
    public partial class SplashPage : ContentPage
    {
        public SplashPage()
        {
            InitializeComponent();
            this.BindingContext = new SplashPageViewModel(this.Navigation);
        }

        protected override async void OnAppearing()
        {
            await Task.Delay(1500);
            if (Application.Current.Properties.ContainsKey("WalletInitialized"))
            {
                var page = Navigation.NavigationStack.Last();
                await Navigation.PushAsync(new WalletPage());
                Navigation.RemovePage(page);
            }
            if (Application.Current.Properties.ContainsKey("WalletCreationDate"))
            {
                var page = Navigation.NavigationStack.Last();
                await Navigation.PushAsync(new WalletHomePage());
                Navigation.RemovePage(page);
            }
            else
            {
                this.AppLogo.IsVisible = false;
                this.gridOptions.IsVisible = true;
            }
        }
    }
}
