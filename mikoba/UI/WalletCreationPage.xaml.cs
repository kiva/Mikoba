using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using System.Threading.Tasks;
using Hyperledger.Indy.DidApi;
using Hyperledger.Indy.WalletApi;
using Xamarin.Forms;
using System.IO;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq;
using mikoba.Services;
using mikoba.UI;
using mikoba.ViewModels;
using Sentry;

namespace mikoba
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
            await Application.Current.SavePropertiesAsync();
            this.lblProgress.Text = "Wallet Created";
            await Task.Delay(1000);
            
            var page = Navigation.NavigationStack.Last();
            await Navigation.PushAsync(new WalletHomePage());
            Navigation.RemovePage(page);
        }
    }
}
