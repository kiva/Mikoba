using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace mikoba.UI.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FingerprintLoginPage : ContentPage
    {
        private CancellationTokenSource _tokenSource = new CancellationTokenSource();
        
        public FingerprintLoginPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
        
            PollFingerprintAuthentication(_tokenSource);
        }
        
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _tokenSource.Dispose();
        }
        
        private async void PollFingerprintAuthentication(
            CancellationTokenSource tokenSource)
        {
            while (!tokenSource.IsCancellationRequested)
            {
                var request = new AuthenticationRequestConfiguration("Please scan your fingerprint", "We must verify your identity before granting access to your Wallet.");
                var result = await CrossFingerprint.Current.AuthenticateAsync(request, tokenSource.Token);
                
                if (result.Authenticated)
                {
                    YouShallPass();
                }
                else
                {
                    PollFingerprintAuthentication(tokenSource);
                }
            }
        }
        
        private async void YouShallPass()
        {
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
        }
    }
}