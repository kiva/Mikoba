using System;
using Autofac;
using Hyperledger.Aries.Agents;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.Extensions.Hosting;
using Xamarin.Essentials;
using Xamarin.Forms;
using mikoba.Services;
using mikoba.UI.Pages;
using mikoba.UI.Components;
using mikoba.UI.Pages.Connections;
using mikoba.UI.Pages.Credentials;
using mikoba.UI.Pages.Login;
using mikoba.UI.Pages.Onboarding;
using mikoba.UI.Pages.Settings;
using mikoba.UI.Pages.Wallet;
using mikoba.UI.ViewModels;
using mikoba.ViewModels;
using mikoba.ViewModels.Pages;
using mikoba.ViewModels.SSI;
using mikoba.ViewModels.Pages.Login;
using mikoba.ViewModels.Pages.Onboarding;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using Sentry;

namespace mikoba
{
    public partial class App : Application
    {
        public new static App Current => Application.Current as App;
        public static IContainer Container { get; set; }
        private static IHost Host { get; set; }
        public App(IHost host) : this() => Host = host;

        private INavigationService _navigationService;

        public App()
        {
            InitializeComponent();
            Preferences.Set(AppConstant.EnableFirstActionsView, false);
            this.StartServices();
        }

        private void StartServices()
        {
            _navigationService = Container.Resolve<INavigationService>();
        }

        protected override async void OnStart()
        {
            await Host.StartAsync();

            AppCenter.Start("android=acc3a7fc-65e0-497a-b317-3da135a86a64;" +
                            "ios=56836175-d366-4a59-893b-58f866799cbc",
                typeof(Analytics), typeof(Crashes));

            //Wallet
            _navigationService.AddPageViewModelBinding<WalletPageViewModel, WalletPage>();
            _navigationService.AddPageViewModelBinding<AcceptConnectionInviteViewModel, AcceptConnectionInvitePage>();
            _navigationService.AddPageViewModelBinding<CredentialOfferPageViewModel, CredentialOfferPage>();
            _navigationService.AddPageViewModelBinding<ProofRequestViewModel, ProofRequestPage>();
            _navigationService.AddPageViewModelBinding<EntryHubPageViewModel, EntryHubPage>();
            _navigationService.AddPageViewModelBinding<SplashPageViewModel, SplashPage>();
            _navigationService.AddPageViewModelBinding<SettingsPageViewModel, SettingsPage>();

            //Onboarding
            _navigationService.AddPageViewModelBinding<WalletOwnerInputViewModel, WalletOwnerInputPage>();
            _navigationService.AddPageViewModelBinding<WalletPinSetViewModel, WalletPinSetPage>();
            _navigationService.AddPageViewModelBinding<WalletPinConfirmViewModel, WalletPinConfirmationPage>();
            _navigationService.AddPageViewModelBinding<WalletCreationViewModel, WalletCreationPage>();

            //Permissions
            _navigationService.AddPageViewModelBinding<AllowCameraConfirmationViewModel, AllowCameraConfirmationPage>();
            _navigationService.AddPageViewModelBinding<AllowPushNotificationViewModel, AllowPushNotificationPage>();
            _navigationService.AddPageViewModelBinding<AllowFingerprintViewModel, AllowFingerprintPage>();
            //Login
            _navigationService.AddPageViewModelBinding<FingerprintLoginViewModel, FingerprintLoginPage>();
            _navigationService.AddPageViewModelBinding<PINLoginViewModel, PINLoginPage>();
            
            if (Preferences.Get(AppConstant.LocalWalletProvisioned, false))
            {
                var fpAuthStatus = await CrossFingerprint.Current.GetAvailabilityAsync();
                if (fpAuthStatus == FingerprintAvailability.Available &&
                    Preferences.Get(AppConstant.AllowFingerprint, false))
                {
                    await _navigationService.NavigateToAsync<FingerprintLoginViewModel>();
                }
                else
                {
                    await _navigationService.NavigateToAsync<PINLoginViewModel>();
                }
            }
            else
            {
                await _navigationService.NavigateToAsync<SplashPageViewModel>();
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
