using Autofac;
using Microsoft.Extensions.Hosting;
using Xamarin.Essentials;
using Xamarin.Forms;
using mikoba.Services;
using mikoba.UI.Pages;
using mikoba.UI.Pages.Wallet;
using mikoba.UI.ViewModels;

namespace mikoba
{
    public partial class App : Application
    {
        public new static App Current => Application.Current as App;
        public static IContainer Container { get; set; }
        private static IHost Host { get; set; }
        public App(IHost host) : this() => Host = host;

        private IMediatorTimerService _mediatorTimerService;

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
            _mediatorTimerService = Container.Resolve<IMediatorTimerService>();
        }

        protected override async void OnStart()
        {
            await Host.StartAsync();

            _navigationService.AddPageViewModelBinding<WalletPageViewModel, WalletPage>();
            // _navigationService.AddPageViewModelBinding<ConnectionsViewModel, ConnectionsPage>();
            // _navigationService.AddPageViewModelBinding<ConnectionViewModel, ConnectionPage>();
            // _navigationService.AddPageViewModelBinding<RegisterViewModel, RegisterPage>();
            // _navigationService.AddPageViewModelBinding<AcceptInviteViewModel, AcceptInvitePage>();
            // _navigationService.AddPageViewModelBinding<CredentialsViewModel, CredentialsPage>();
            // _navigationService.AddPageViewModelBinding<CredentialViewModel, CredentialPage>();
            // _navigationService.AddPageViewModelBinding<AccountViewModel, AccountPage>();
            // _navigationService.AddPageViewModelBinding<CreateInvitationViewModel, CreateInvitationPage>();

            // MainPage = NavigationService.CreateMainPage(() => new SplashPage());
            
            await _navigationService.NavigateToAsync<WalletPageViewModel>();
            
            _mediatorTimerService.Start();
        }

        protected override void OnSleep()
        {
            _mediatorTimerService.Pause();
        }

        protected override void OnResume()
        {
            _mediatorTimerService.Resume();
        }
    }
}
