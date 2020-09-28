using System;
using System.Linq;
using System.Threading.Tasks;
using mikoba.UI.Pages.Wallet;
using mikoba.ViewModels;
using Xamarin.Essentials;
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
            if (Preferences.Get(AppConstant.LocalWalletProvisioned, false))
            {
                if (Application.Current.Properties.ContainsKey("UseFingerprintAuth"))
                {
                    Navigation.PushAsync(new FingerprintLoginPage());
                }
                else
                {
                    Navigation.PushAsync(new PINLoginPage());
                }
            }
            else
            {
                this.AppLogo.IsVisible = false;
                this.gridOptions.IsVisible = true;
            }
        }
    }
}