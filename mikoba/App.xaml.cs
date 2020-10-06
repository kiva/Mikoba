using Autofac;
using Hyperledger.Aries.Agents;
using Microsoft.Extensions.Hosting;
using Xamarin.Essentials;
using Xamarin.Forms;
using mikoba.Services;
using mikoba.UI.Pages;
using mikoba.UI.Pages.Connections;
using mikoba.UI.Pages.Credentials;
using mikoba.UI.Pages.Onboarding;
using mikoba.UI.Pages.Wallet;
using mikoba.ViewModels;
using mikoba.ViewModels.Pages;
using mikoba.ViewModels.SSI;

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
            
            //Wallet
            _navigationService.AddPageViewModelBinding<WalletPageViewModel, WalletPage>();
            _navigationService.AddPageViewModelBinding<AcceptConnectionInviteViewModel, AcceptConnectionInvitePage>();
            _navigationService.AddPageViewModelBinding<CredentialOfferPageViewModel, CredentialOfferPage>();
            _navigationService.AddPageViewModelBinding<ProofRequestViewModel, ProofRequestPage>();
            _navigationService.AddPageViewModelBinding<EntryHubPageViewModel, EntryHubPage>();
            _navigationService.AddPageViewModelBinding<SplashPageViewModel, SplashPage>();
           
            //Onboarding
            _navigationService.AddPageViewModelBinding<WalletOwnerInputViewModel, WalletOwnerInputPage>();
            _navigationService.AddPageViewModelBinding<WalletPinSetViewModel, WalletPinSetPage>();
            _navigationService.AddPageViewModelBinding<WalletPinConfirmViewModel, WalletPinConfirmationPage>();
            _navigationService.AddPageViewModelBinding<WalletCreationViewModel, WalletCreationPage>();
            
            //Permissions
            _navigationService.AddPageViewModelBinding<AllowCameraConfirmationViewModel, AllowCameraConfirmationPage>();
            _navigationService.AddPageViewModelBinding<AllowPushNotificationViewModel, AllowPushNotificationPage>();
            

            if (Preferences.Get(AppConstant.LocalWalletProvisioned, false))
            {
                await _navigationService.NavigateToAsync<WalletPageViewModel>();
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
