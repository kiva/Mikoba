using System;
using System.Threading.Tasks;
using Autofac;
using Hyperledger.Aries.Agents;
using Hyperledger.Aries.Agents.Edge;
using Hyperledger.Indy.AnonCredsApi;
using Microsoft.AppCenter.Analytics;
using mikoba.Services;
using mikoba.ViewModels.Pages;
using ReactiveUI;
using Sentry;
using Sentry.Protocol;
using Xamarin.Essentials;

namespace mikoba.ViewModels.Pages.Onboarding
{
    public sealed class WalletCreationViewModel : MikobaBaseViewModel
    {

        public WalletCreationViewModel(
            INavigationService navigationService,
            IPoolConfigurator poolConfigurator,
            IEdgeProvisioningService edgeProvisioningService
        )
        : base("Wallet Creation", navigationService)
        {
            _edgeProvisioningService = edgeProvisioningService;
            _poolConfigurator = poolConfigurator;
        }

        private IEdgeProvisioningService _edgeProvisioningService;
        private readonly IPoolConfigurator _poolConfigurator;
        
        #region UI Properties
        
        private string _progressInfo;
        public string ProgressInfo
        {
            get => _progressInfo;
            set => this.RaiseAndSetIfChanged(ref _progressInfo, value);
        }
        
        private double _progress;
        public double Progress
        {
            get => _progress;
            set => this.RaiseAndSetIfChanged(ref _progress, value);
        }
        
        #endregion
        
        #region Lifecyle
        
        public override async Task InitializeAsync(object navigationData)
        {
            try
            {
                await _poolConfigurator.ConfigurePoolsAsync();
                await _edgeProvisioningService.ProvisionAsync();
                Preferences.Set(AppConstant.LocalWalletProvisioned, true);

                Tracking.TrackEvent("Initialized Wallet");

                await Task.Delay(100);
                ProgressInfo = "Checking Permissions";
                Progress = 0.30;
                await Task.Delay(100);
                ProgressInfo = "Getting Storage Access";
                Progress = 0.50;
                await Task.Delay(100);
                ProgressInfo = "Creating Wallet";
                Progress = 1;
                ProgressInfo = "Wallet Created";
                await Task.Delay(2000);
                await NavigationService.NavigateToAsync<WalletPageViewModel>();
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }
        
        #endregion

    }
}
