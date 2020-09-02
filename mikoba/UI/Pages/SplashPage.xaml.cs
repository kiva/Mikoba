using System;
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
            await Task.Delay(500);
            if (Application.Current.Properties.ContainsKey("WalletInitialized"))
            {
                Console.WriteLine("Navigating to Wallet Page");
                var page = Navigation.NavigationStack.Last();
                await Navigation.PushAsync(new WalletPage());
                Navigation.RemovePage(page);
            }
            else if (Application.Current.Properties.ContainsKey("WalletCreationDate"))
            {
                Console.WriteLine("Navigating to Wallet First Actions Sequence");
                var page = Navigation.NavigationStack.Last();
                await Navigation.PushAsync(new WalletFirstActionsPage());
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
