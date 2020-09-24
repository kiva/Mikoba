using System.Linq;
using System.Threading.Tasks;
using mikoba.Services;
using mikoba.ViewModels;
using Sentry;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace mikoba.UI.Pages
{
    public partial class WalletCreationPage : ContentPage
    {
        public WalletCreationPage()
        {
            InitializeComponent();
            this.BindingContext = new WalletCreationViewModel(this.Navigation);
        }

        protected override async void OnAppearing()
        {
            await CreateWalletFlow();
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
            await WalletService.CreateWallet();
            SentrySdk.CaptureEvent(new SentryEvent() {Message = "Wallet Creation"});
            //TODO: Add Error Handling
            Preferences.Set(AppConstant.LocalWalletProvisioned, true);
            Preferences.Set(AppConstant.LocalWalletFirstView, true);
            await Application.Current.SavePropertiesAsync();
            this.lblProgress.Text = "Wallet Created";
            await Task.Delay(1000);
            
            var page = Navigation.NavigationStack.Last();
            await Navigation.PushAsync(new WalletFirstActionsPage());
            Navigation.RemovePage(page);
        }
    }
}
