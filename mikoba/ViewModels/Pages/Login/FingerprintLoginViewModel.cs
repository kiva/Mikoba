using System;
using Autofac;
using Hyperledger.Aries.Agents;
using Hyperledger.Aries.Contracts;
using Hyperledger.Aries.Features.DidExchange;
using Hyperledger.Aries.Features.IssueCredential;
using mikoba.Services;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;

namespace mikoba.ViewModels.Pages.Login
{
    public class FingerprintLoginViewModel : KivaBaseViewModel
    {
        public FingerprintLoginViewModel(INavigationService navigationService) : base("Fingerprint Login", navigationService)
        {
            // BeginAuthentication();
        }
        
        public async void BeginAuthentication()
        {
            var fpRequest = new AuthenticationRequestConfiguration("Log in using your fingerprint", "You have enabled fingerprint scanning to log in to your wallet. You can withdraw this permission at any time.");
            var result = await CrossFingerprint.Current.AuthenticateAsync(fpRequest);
            if (result.Authenticated)
            {
                await NavigationService.NavigateToAsync<WalletPageViewModel>();
            }
            else
            {
                await NavigationService.NavigateToAsync<PINLoginViewModel>();
            }
        }
    }
}