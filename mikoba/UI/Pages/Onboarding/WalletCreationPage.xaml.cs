using System.Linq;
using System.Threading.Tasks;
using mikoba.Services;
using mikoba.UI.Pages.Wallet;
using mikoba.ViewModels;
using Sentry;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace mikoba.UI.Pages.Onboarding
{
    public partial class WalletCreationPage : ContentPage
    {
        public WalletCreationPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
         
        }

        async Task CreateWalletFlow()
        {
            await Task.Delay(100);
            this.lblProgress.Text = "Checking Permissions";
            await this.progressBar.ProgressTo(0.30, 500, Easing.Linear);
            await Task.Delay(100);
            this.lblProgress.Text = "Getting Storage Access";
            await this.progressBar.ProgressTo(0.50, 500, Easing.Linear);
            await Task.Delay(100);
            this.lblProgress.Text = "Creating Wallet";
            await this.progressBar.ProgressTo(1, 500, Easing.Linear);
            await WalletService.ProvisionWallet();


            Preferences.Set(AppConstant.LocalWalletProvisioned, true);
            Preferences.Set(AppConstant.LocalWalletFirstView, false);
            await Application.Current.SavePropertiesAsync();
            this.lblProgress.Text = "Wallet Created";
            await Task.Delay(1000);
        }
    }
}
