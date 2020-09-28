using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using mikoba.Services;
using mikoba.UI.Pages.Wallet;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using Xamarin.Essentials;
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
                    var login = new LoginUtils(Navigation);
                    login.YouShallPass();
                }
                else
                {
                    PollFingerprintAuthentication(tokenSource);
                }
            }
        }
    }
}