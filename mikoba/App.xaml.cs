using System;
using System.IO;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hyperledger.Aries.Agents;
using Hyperledger.Aries.Storage;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.Extensions.DependencyInjection;
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

            AppCenter.Start("appCenterKey", typeof(Analytics), typeof(Crashes));

            ConfigureScreensAndViewModels();

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
        
        public static IHostBuilder BuildHost(Assembly platformSpecific = null)
        {
            return XamarinHost.CreateDefaultBuilder<App>()
                .ConfigureServices((_, services) =>
                {
                    var rootPath = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    var walletPath = Path.Combine(path1: rootPath, path2: ".indy_client", path3: "wallets");
                    var tailsPath = Path.Combine(path1: FileSystem.AppDataDirectory, path2: ".indy_client", path3: "tails");
                    services.AddAriesFramework(builder => builder.RegisterEdgeAgent<MikobaAgent>(
                        options: options =>
                        {
                            options.PoolName = "kiva_sandbox";
                            options.EndpointUri = AppConstant.EndpointUri;

                            options.WalletConfiguration.StorageConfiguration =
                                new WalletConfiguration.WalletStorageConfiguration
                                {
                                    Path = walletPath
                                };
                            options.WalletConfiguration.Id = "MobileWallet";
                            options.WalletCredentials.Key = "SecretWalletKey";

                            options.RevocationRegistryDirectory = tailsPath;
                        },
                        delayProvisioning: true));
                    services.AddSingleton<IPoolConfigurator, PoolConfigurator>();
                    var containerBuilder = new ContainerBuilder();
                    containerBuilder.RegisterAssemblyModules(typeof(KernelModule).Assembly);
                    if (platformSpecific != null)
                    {
                        containerBuilder.RegisterAssemblyModules(platformSpecific);
                    }
                    containerBuilder.Populate(services);
                    App.Container = containerBuilder.Build();
                });
        }

        private void ConfigureScreensAndViewModels()
        {
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
            _navigationService.AddPageViewModelBinding<WalletPinConfirmationViewModel, WalletPinConfirmationPage>();
            _navigationService.AddPageViewModelBinding<WalletCreationViewModel, WalletCreationPage>();

            //Permissions
            _navigationService.AddPageViewModelBinding<AllowCameraConfirmationViewModel, AllowCameraConfirmationPage>();
            _navigationService.AddPageViewModelBinding<AllowPushNotificationViewModel, AllowPushNotificationPage>();
            _navigationService.AddPageViewModelBinding<AllowFingerprintViewModel, AllowFingerprintPage>();

            //Login
            _navigationService.AddPageViewModelBinding<FingerprintLoginViewModel, FingerprintLoginPage>();
            _navigationService.AddPageViewModelBinding<PINLoginViewModel, PINLoginPage>();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
