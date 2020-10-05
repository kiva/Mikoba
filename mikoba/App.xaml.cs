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
            _navigationService.AddPageViewModelBinding<CredentialRequestPageViewModel, CredentialRequestPage>();
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

            var offeredMockup = new SSICredentialViewModel();
            /*
                Photo
                National ID
                First Name
                Last Name
                Birth Date
             */
            offeredMockup.Attributes.Add(new SSICredentialAttribute()
            {
                Name = "nationalId",
                Value = "23442"
            });
            offeredMockup.Attributes.Add(new SSICredentialAttribute()
            {
                Name = "firstName",
                Value = "Horacio"
            });
            offeredMockup.Attributes.Add(new SSICredentialAttribute()
            {
                Name = "lastName",
                Value = "Nunez"
            });
            offeredMockup.Attributes.Add(new SSICredentialAttribute()
            {
                Name = "birthDate",
                Value = "TODAY"
            });
            offeredMockup.Attributes.Add(new SSICredentialAttribute()
            {
                Name = "photo~attach",
                Value = "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"
            });
            
            
            // await _navigationService.NavigateToAsync<CredentialOfferPageViewModel>(offeredMockup);
            
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
